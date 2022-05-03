using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HCF.BAL;
using HCF.Utility;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.BAL.Interfaces.Services;
using HCF.Web.ViewModels.Goal;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using HCF.Utility.Extensions;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class GoalController : BaseController
    {

        private readonly IInsActivityService _inspectionActivityService;
        private readonly IStandardService _standardService;
        private readonly IEpService _epService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly IFrequencyService _frequencyService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly IAssetsService _assetService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IIlsmService _ilsmService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IOrganizationService _orgService;
        private readonly ICommonService _commonService;
        private readonly IDocumentsService _documentService;
        private readonly ITransactionService _transactionService;
        private readonly IStepsService _stepService;
        private readonly IUserService _userService;
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IPdfService _pdfService;
        private readonly IBuildingsService _buildingsService;
        private readonly IReportsService _reportService;
        public GoalController(
            ICommonModelFactory commonModelFactory,
            ITransactionService transactionService,
            IAssetsService assetService,
            IInsActivityService inspectionActivityService,
            ICommonService commonService,
            IDocumentsService documentService,
            IDocumentTypeService documentTypeService,
            IMainGoalService mainGoalService,
            IOrganizationService orgService,
            IIlsmService ilsmService,
            IFrequencyService frequencyService,
            IEpService epService,
            IGoalMasterService goalMasterService,
            IStandardService standardService,
            IStepsService stepService,
            IFloorAssetService floorAssetService,
            IHttpPostFactory httpService,
            IUserService userService,
            IPdfService pdfService,
            IBuildingsService buildingsService,
            IReportsService reportService
            )
        {
            _buildingsService = buildingsService;
            _commonModelFactory = commonModelFactory;
            _transactionService = transactionService;
            _assetService = assetService;
            _documentService = documentService;
            _commonService = commonService;
            _documentTypeService = documentTypeService;
            _ilsmService = ilsmService;
            _goalMasterService = goalMasterService;
            _epService = epService;
            _frequencyService = frequencyService;
            _mainGoalService = mainGoalService;
            _orgService = orgService;
            _inspectionActivityService = inspectionActivityService;
            _standardService = standardService;
            _stepService = stepService;
            _floorAssetService = floorAssetService;
            _userService = userService;
            _httpService = httpService;
            _pdfService = pdfService;
            _reportService = reportService;
        }

        #region Standards

        [CrxAuthorize(Roles = "Goal_Index")]
        public ActionResult Index(string categoryId)
        {
            UISession.AddCurrentPage("Goal_Index", 0, "Setup Master EP");
            ViewBag.Category = _assetService.GetCategory();
            var items = _standardService.Get();
            if (!string.IsNullOrEmpty(categoryId))
                items = items.Where(x => x.CategoryId == Convert.ToInt32(categoryId)).ToList();
            return View(items.OrderBy(x => x.TJCStandard).ToList());
        }

        [HttpGet]
        public ActionResult CreateGoal()
        {
            UISession.AddCurrentPage("Goal_creategoal", 0, "Add Standard");
            ViewBag.Frequencs = _frequencyService.GetFrequency();
            ViewBag.Category = _assetService.GetCategory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGoal(BDO.Standard model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = UserSession.CurrentUser.UserId;
                var stdescId = _standardService.AddStandard(model);
                if (stdescId > 0) { SuccessMessage = ConstMessage.Success; return RedirectToAction("Index"); }
                else
                    ErrorMessage = ConstMessage.Error;
            }
            ViewBag.Frequencs = _frequencyService.GetFrequency();
            ViewBag.Category = _assetService.GetCategory();
            return View(model);
        }


        [HttpGet]
        public ActionResult EditStandard(int gid)
        {
            UISession.AddCurrentPage("Goal_editstandard", 0, "Update Standard");
            var stdescId = gid;
            ViewBag.Category = _assetService.GetCategory();
            var objStandard = _standardService.GetStandards().FirstOrDefault(x => x.StDescID == stdescId);
            return View(objStandard);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStandard(BDO.Standard data)
        {
            if (ModelState.IsValid)
            {
                var status = _standardService.UpdateStandard(data);
                if (status)
                {
                    SuccessMessage = ConstMessage.Success_Updated;
                    return RedirectToRoute("goals");
                }
                else { ErrorMessage = ConstMessage.Error; }
            }
            return View();
        }


        //[CustomAuthorize(Roles = "Remove_Goals")]
        public ActionResult Deletestandard(int gid)
        {
            var stdescId = gid;
            var standard = _standardService.GetStandard(stdescId);
            standard.IsActive = false;
            var status = _standardService.UpdateStandard(standard);
            if (status)
                return RedirectToRoute("goals");
            return RedirectToAction("Index");
        }

        #endregion

        #region EP details

        public ActionResult NonTrackedEPs()
        {
            UISession.AddCurrentPage("goal_NonTrackedEPs", 0, "Non-Tracked EP Report");
            var eps = _epService.GetStandardEPs().Where(x => !x.IsApplicable).ToList();
            return View(eps);
        }

        [HttpGet]
        public ActionResult AddEP(int? stdescId, int? epdetailId)
        {
            UISession.AddCurrentPage("goal_addep", 0, "Add EPs");
            var objEPdetails = new EPDetails();
            ViewBag.RecFrequencyId = "";
            ViewBag.DocTypeId = "";
            ViewBag.TjcFrequencyId = "";
            if (epdetailId.HasValue && epdetailId > 0)
            {
                objEPdetails = _epService.GetEpDetails().FirstOrDefault(x => x.EPDetailId == epdetailId.Value);
                if (objEPdetails != null)
                {
                    if (objEPdetails.EPFrequency != null && objEPdetails.EPFrequency.Count > 0)
                    {
                        ViewBag.RecFrequencyId = objEPdetails.EPFrequency.FirstOrDefault()?.RecFrequencyId;
                        ViewBag.TjcFrequencyId = objEPdetails.EPFrequency.FirstOrDefault()?.TjcFrequencyId;
                    }
                    if (objEPdetails.DocumentType != null && objEPdetails.DocumentType.Count > 0)
                        ViewBag.DocTypeId = objEPdetails.DocumentType.FirstOrDefault()?.DocTypeId;
                }
            }
            else
            {
                objEPdetails.EPDescriptions = _epService.GetHospitalTypeEpDescriptions();
                var frq = new EPFrequency();
                frq.TjcFrequencyId = 0;
                frq.RecFrequencyId = 0;
                objEPdetails.EPFrequency.Add(frq);

            }

            var epPriorityLists = from BDO.Enums.EPPriority d in Enum.GetValues(typeof(BDO.Enums.EPPriority))
                                  select new { ID = (int)d, Name = d.ToString() };
            var epPriority = new SelectList(epPriorityLists, "ID", "Name", objEPdetails?.Priority ?? 0);

            ViewBag.EPPriority = epPriority;
            ViewBag.Frequencs = _frequencyService.GetFrequency().Where(x => x.IsActive).ToList();
            ViewBag.Goals = _standardService.Get().Where(x => x.IsActive).ToList();
            ViewBag.Scores = _orgService.GetScores();
            ViewBag.DocumentTypes = _documentTypeService.GetDocumentTypes();
            ViewBag.StdescId = stdescId ?? 0;
            return View(objEPdetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEP(EPDetails model, string docTypeId, string assetId)
        {
            model.CreatedBy = UserSession.CurrentUser.UserId;
            if (!string.IsNullOrEmpty(docTypeId))
                model.IsDocRequired = true;

            if (!string.IsNullOrEmpty(assetId))
                model.IsAssetsRequired = true;

            var epId = _epService.Save(model);
            model.EPDetailId = epId;
            if (epId > 0)
            {
                if (!string.IsNullOrEmpty(docTypeId))
                    SaveEPDocumentType(epId, docTypeId);

                if (!string.IsNullOrEmpty(assetId))
                    SaveEPAssets(epId, assetId);

                if (model.EPDescriptions.Count > 0)
                {
                    SaveEPDescriptions(model);
                }

                return RedirectToAction("addepdetails", "Goal", new { stDescId = model.StDescID });
            }

            ViewBag.Frequencs = _frequencyService.GetFrequency();
            ViewBag.Goals = _standardService.GetStandards();
            ViewBag.Scores = _orgService.GetScores();
            ViewBag.DocumentTypes = _documentTypeService.GetDocumentTypes();
            var epPriorityLists = from BDO.Enums.EPPriority d in Enum.GetValues(typeof(BDO.Enums.EPPriority))
                                  select new { ID = (int)d, Name = d.ToString() };
            var epPriority = new SelectList(epPriorityLists, "ID", "Name", model.Priority);
            ViewBag.EPPriority = epPriority;
            return View();
        }

        public JsonResult SaveEPDescription(EPDetails model)
        {
            if (model.EPDescriptions.Count > 0)
            {
                SaveEPDescriptions(model);
            }
            return Json("");
        }

        private void SaveEPDescriptions(EPDetails model)
        {
            foreach (var item in model.EPDescriptions.Where(item => !string.IsNullOrEmpty(item.Description)))
            {
                item.EPDetailId = model.EPDetailId;
                item.Description = model.Description;
                _epService.SaveEpDescription(item);
            }
        }


        private void SaveEPDocumentType(int epId, string docTypeId)
        {
            var docTypes = docTypeId.Split(',');
            foreach (var doctype in docTypes)
            {
                var objEpdocuments = new EpDocuments
                {
                    CreatedBy = UserSession.CurrentUser.UserId,
                    EPDetailId = epId,
                    DocTypeId = Convert.ToInt32(doctype),
                    IsActive = true
                };
                _epService.Save(objEpdocuments);
            }
        }


        private void SaveEPAssets(int epId, string assetIds)
        {
            var assets = assetIds.Split(',');
            foreach (var assetId in assets)
                _assetService.AttachAssetToEp(Convert.ToInt32(assetId), epId, UserSession.CurrentUser.UserId);
        }

        public ActionResult EpByStandard(int stdescId)
        {
            var epNumbers = _epService.GetEpDetails(stdescId).ToList();
            return Json(epNumbers);
        }

        public ActionResult FillEPnumber(int stdescId)
        {
            var epNumbers = _epService.GetEpNumber(stdescId).ToList();
            return Json(epNumbers);
        }

        public ActionResult FillEPnumberforBinders(int stdescId, int epBinderId = 0)
        {
            var epNumbers = _epService.GetEpDetailsforBinders(stdescId, epBinderId).ToList();
            return Json(epNumbers);
        }

        public ActionResult NonTrackedEPsPopup(int epdetailId)
        {
            EPDetails epdetails = new EPDetails();
            epdetails = _epService.GetEpShortDetails(epdetailId);
            return PartialView("~/Views/Shared/PopUp/_NonTrackedEPs.cshtml", epdetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateEPApplicablestatus(int epdetailId, bool newValue, string reason)
        {
            var epDetail = new EPDetails { IsApplicable = newValue, EPDetailId = epdetailId, ReasonforNonTracked = reason };
            var status = _epService.UpdateEPApplicablestatus(epDetail);
            return Json(status);
        }



        public ActionResult EpbyId(int? id)
        {
            ViewBag.Frequencs = _frequencyService.GetFrequency().OrderBy(x => x.Days).ToList();
            ViewBag.Scores = _orgService.GetScores();
            var standard = new BDO.Standard();
            if (id.HasValue)
            {
                ViewBag.StdescId = id.Value;
                standard = GetStandardEps(id);
            }
            return PartialView("bulkEP", standard);
        }

        private Standard GetStandardEps(int? id)
        {
            var standard = new Standard();
            if (id.HasValue)
            {
                standard = _standardService.Get(id.Value);
                List<EPDetails> epNumber = _epService.GetAllEps(id.Value, null);
                standard.EPDetails = epNumber;
                return standard;
            }
            return standard;
        }

        public ActionResult AddEPDetails(int? stDescId)
        {
            UISession.AddCurrentPage("goal_addepdetails", 0, "Standard EPs");
            var standard = GetStandardEps(Convert.ToInt32(stDescId));
            return View(standard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEPDetails(BDO.Standard standard, string GoalId, string EPDetailId)
        {
            if (ModelState.IsValid)
            {
                foreach (var ep in standard.EPDetails)
                    _epService.Save(ep);
                SuccessMessage = ConstMessage.Success_Updated;
            }
            return RedirectToRoute("goals");
        }

        #endregion

        #region MainGoal

        // Created by Avinash Varshney on date 09-11-2016

        public ActionResult GetMainGoal(int? epDetailId, int? mainGoalId, int? assetId, int? floorAssetId)
        {
            var goal = new MainGoal();
            if (mainGoalId.HasValue && epDetailId.HasValue)
                goal = _mainGoalService.GetMainGoalById(mainGoalId.Value);
            else if (mainGoalId.HasValue && mainGoalId.Value > 0)
                goal = _mainGoalService.GetMainGoalById(mainGoalId.Value);
            if (goal == null)
            {
                goal = new MainGoal { ActivityType = 1 };
                if (epDetailId.HasValue)
                    goal.EPDetailId = epDetailId;
                if (assetId.HasValue)
                {
                    goal.AssetId = assetId;
                    goal.ActivityType = 2;
                }
                if (floorAssetId.HasValue && floorAssetId.Value > 0)
                {
                    goal.FloorAssetId = floorAssetId.Value;
                    goal.ClientNo = HCF.Web.Base.UserSession.CurrentOrg.ClientNo;
                }
            }
            if (goal.AssetId.HasValue && goal.AssetId.Value > 0)
            {
                goal.epDetails = _assetService.GetAssetEp(goal.AssetId.Value);
            }
            return PartialView("_mngGoals", goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGoal(MainGoal mainGoal)
        {
            int mainGoalId = 0;
            mainGoal.CreatedBy = UserSession.CurrentUser.UserId;
            mainGoalId = mainGoal.MainGoalId > 0 ? _mainGoalService.UpdateMainGoal(mainGoal) : _mainGoalService.AddMaingoal(mainGoal);
            return Json(new { Result = mainGoalId });
        }

        public ActionResult AddMainGoal(int? epdetailid, int? assetId, int? floorAssetId = null)
        {
            ViewBag.EPDetailId = epdetailid;
            ViewBag.AssetId = assetId;
            return View();
        }

        public ActionResult AddMainGoalPartial(int? epdetailid, int? assetId, int? floorAssetId = null)
        {
            List<MainGoal> model = GetMainGoalView(epdetailid, assetId, floorAssetId);
            return PartialView("_AddMainGoal", model);
        }

        private List<MainGoal> GetMainGoalView(int? epdetailid, int? assetId, int? floorAssetId)
        {
            var model = new List<MainGoal>();
            if (epdetailid.HasValue && epdetailid.Value > 0)
            {
                ViewBag.EpdetailId = epdetailid;
                var eps = _epService.GetEpDescription(epdetailid);
                ViewBag.CurrentEP = eps;
                model = _mainGoalService.GetAllMainGoal(epdetailid);
            }
            else if (assetId.HasValue && floorAssetId.HasValue)
            {
                ViewBag.FloorAssetId = floorAssetId ?? floorAssetId;
                var assets = _assetService.Get(assetId.Value);
                if (assets != null)
                {
                    assets.EPdetails = _assetService.GetAssetEp(assetId.Value);
                    assets.TFloorAsset = _floorAssetService.GetFloorAsset(floorAssetId.Value);
                }
                ViewBag.CurrentAssets = assets;
                model = _mainGoalService.GetMainGoalsbyFloorAssetId(HCF.Web.Base.UserSession.CurrentOrg.ClientNo, floorAssetId.Value, assetId.Value);
            }
            else if (assetId.HasValue)
            {
                ViewBag.AssetId = assetId;
                var assets = _assetService.Get(assetId.Value);
                if (assets != null)
                    assets.EPdetails = _assetService.GetAssetEp(assetId.Value);
                ViewBag.CurrentAssets = assets;
                model = _mainGoalService.GetAssetMainGoals(assetId.Value);
            }
            ViewBag.AllAssets = _assetService.Get().Where(x => x.IsActive).ToList();
            return model;
        }

        public ActionResult loadMainGoallist(int? epdetailid, int? assetId, int? floorAssetId = null)
        {
            List<MainGoal> model = GetMainGoalView(epdetailid, assetId, floorAssetId);
            return PartialView("_CheckLists", model);
        }

        #endregion

        #region Steps

        [HttpGet]
        public ActionResult StepsToComplete(int maingoalId)
        {
            var steps = _mainGoalService.GetMainGoalById(maingoalId);
            return PartialView("_stepsToComplete", steps);
        }


        public ActionResult MainGoalStep(int? maingoalId, int? stepsId)
        {
            var mainGoal = new MainGoal();
            var step = new Steps();
            if (maingoalId.HasValue)
                mainGoal = _mainGoalService.GetMainGoalById(maingoalId.Value);

            if (stepsId.HasValue && mainGoal != null)
            {
                step = mainGoal.Steps.FirstOrDefault(x => x.StepsId == stepsId.Value);
                if (step == null)
                    step = new Steps();
            }

            if (maingoalId != null) step.MainGoalId = maingoalId.Value;
            step.MainGoal = mainGoal;
            return PartialView("_mngsteps", step);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSteps(Steps steps)
        {
            steps.CreatedBy = UserSession.CurrentUser.UserId;
            var stepsId = _stepService.Save(steps).StepsId;
            return Json(new { Result = stepsId });
        }


        [HttpGet]
        public ActionResult DeleteSteps(int stepId)
        {
            Steps steps = new Steps
            {
                StepsId = stepId,
                IsActive = false,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            var result = _stepService.Update(steps);
            return Json(new { Result = result });
        }
        #endregion

        #region epstate

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult EPstate(int? epId, int? inspectionId, int? floorAssetId, int? frequencyId, int? Isback, int? inspectionGroupId, int ismulipleEp = 0)
        {
            if (epId.HasValue)
            {
                var epSteps = new EPSteps
                {
                    IsAllowed = true
                };
                if (!UserSession.IsEPAllowed(epId.Value))
                    epSteps.IsAllowed = false;

                string screenName = "Review";
                if (floorAssetId.HasValue)
                {
                    screenName = "Asset Inspection";
                    UISession.AddToBreadCrumbList("goal_AssetInspection", 0, screenName);
                    epSteps.FloorAssetId = floorAssetId;
                }
                else
                    UISession.AddToBreadCrumbList("goal_EPstate", 0, screenName);
                UISession.AddToList("goal_EPstate", 0, screenName);
                TempData["ismulipleEp"] = ismulipleEp;

                if (Isback == 1)
                {
                    if (floorAssetId.HasValue)
                        return RedirectToAction("AssetEps", "Assets", new { floorAssetId, epId = epId.Value });
                    else
                        epSteps.ActivityType = 1;

                    epSteps.EPDetailId = epId.Value;
                    epSteps.InspectionId = inspectionId ?? 0;
                    return View(epSteps);
                }
                epSteps.ActivityType = (floorAssetId.HasValue) ? 2 : 1;
                epSteps.EPDetailId = epId.Value;
                epSteps.InspectionId = inspectionId ?? 0;
                return View(epSteps);
            }
            else
                return RedirectToRoute("error404");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPstate(EPSteps viewModel, int? fileId, string fileName, string btnSubmit, string pageName, string activityType, int frequencyId, string multipleEPDetailId)
        {
            if (string.IsNullOrEmpty(activityType))
                activityType = "3";

            var tInspection = GenerateInspectionJson(viewModel, fileId, frequencyId, fileName);


            HttpResponseMessage httpresponseResult = new HttpResponseMessage();

            switch (btnSubmit)
            {
                case "Save & Done":
                    httpresponseResult = Save(tInspection, 1, multipleEPDetailId);
                    break;
                case "Resume Later":
                    httpresponseResult = Save(tInspection, 2, multipleEPDetailId);
                    break;
                case "Cancel":
                    if (activityType == "2")
                        return RedirectToAction("AssetEps", "Assets", new { floorAssetId = viewModel.FloorAssetId, epId = viewModel.EPDetailId });
                    else
                        return RedirectToAction("ActivityDashboard", "Home", new { isback = 1 });

            }

            if (httpresponseResult.Success)
            {
                var objEPSteps = httpresponseResult.Result.EPDocument.FirstOrDefault();

                if (objEPSteps.IsInspReady == 1 && activityType != Convert.ToInt32(BDO.Enums.Mode.EP).ToString() && UserSession.CurrentOrg.DefaultEPInspection)
                    MarkEPCompliant(objEPSteps.InspectionId, objEPSteps.EPDetailId);

                if (httpresponseResult.Result.Inspection.TInspectionActivity.FirstOrDefault().TInsActivityId > 0)
                {
                    var result = new
                    {
                        Result = true,
                        objEPSteps.IsInspReady,
                        PageName = pageName,
                        TinsActivityId = httpresponseResult.Result.Inspection.TInspectionActivity.FirstOrDefault().TInsActivityId,
                        httpresponseResult.Result.Inspection.TInspectionActivity[0].ActivityId,
                        tilsmId = httpresponseResult.Result.TIlsm.TIlsmId,
                        isExistingILSM = (httpresponseResult.Result != null && httpresponseResult.Result.TIlsm != null && httpresponseResult.Result.TIlsm.TIlsmfloorAssets.Count > 1),
                        IncidentId = (httpresponseResult.Result != null && httpresponseResult.Result.TIlsm != null) ? httpresponseResult.Result.TIlsm.IncidentId : "",
                        objEPSteps.Inspection,
                        objEPSteps.IsRelation
                    };
                    return Json(result);
                }
                else
                {
                    var result = new
                    {
                        Result = true,
                        objEPSteps.IsInspReady,
                        PageName = pageName,
                        TinsActivityId = httpresponseResult.Result.Inspection.TInspectionActivity.FirstOrDefault().TInsActivityId
                    };
                    return Json(result);
                }
            }
            else
            {

                var result = new
                {
                    Result = false
                };
                return Json(result);
            }
        }

        //[ChildActionOnly]
        public ActionResult AssetsSteps(int epId, int? inspectionId, int? floorAssetId, bool? showEp, int? frequencyId, string pagemode, int? inspectionGroupId, bool isPopUp = false)
        {
            List<EPSteps> epsteps;


            int _frequencyId = frequencyId ?? 0;
            TempData["frequencyId"] = _frequencyId;
            var userid = UserSession.CurrentUser.UserId;
            if (floorAssetId.HasValue)
            {
                int mode = Convert.ToInt32(BDO.Enums.Mode.ASSET);
                epsteps = _transactionService.GetGoalStepsByActivity(floorAssetId, 0, epId, inspectionId ?? 0, mode, _frequencyId, inspectionGroupId);


                foreach (var item in epsteps)
                {
                    if (item.TInspectionActivity == null)
                        item.TInspectionActivity = new TInspectionActivity();
                    if (item.CurrentStatus != "NA")
                        item.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(epsteps.FirstOrDefault().InspectionId).Where(x => (x.DocTypeId != 108)).OrderByDescending(x => x.CreatedDate).FirstOrDefault(x => x.IsCurrent);
                    item.IsInspReady = 1;
                    item.ActivityType = mode;
                    if (_frequencyId > 0)
                        item.FrequencyId = _frequencyId;

                    if (item.InspectionFiles == null)
                        item.InspectionFiles = CreateInspectionFiles();
                    if (item.Comment != null)
                        item.Comment = epsteps.FirstOrDefault().Comment;

                    item.Campus = _buildingsService.GetCampus(userid, epId).ToList();

                }
            }
            else
            {
                int mode = Convert.ToInt32(BDO.Enums.Mode.EP);
                epsteps = _transactionService.GetGoalStepsByActivity(null, null, epId, inspectionId ?? 0, mode, _frequencyId, inspectionGroupId);

                foreach (var item in epsteps)
                {
                    if (item.TInspectionActivity == null)
                        item.TInspectionActivity = new TInspectionActivity();
                    if (item.EPDetails.Inspection != null && item.EPDetails.Inspection.SubStatus != "NA")
                        item.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(epsteps.FirstOrDefault().InspectionId).Where(x => (x.DocTypeId != 108)).OrderByDescending(x => x.CreatedDate).FirstOrDefault(x => x.IsCurrent);


                    item.ActivityType = mode;
                    if (_frequencyId > 0)
                        item.FrequencyId = _frequencyId;
                    else
                        TempData["frequencyId"] = item.FrequencyId;
                    if (item.InspectionFiles == null)
                        item.InspectionFiles = CreateInspectionFiles();
                    if (item.Comment != null)
                        item.TInspectionActivity.Comment = item.Comment;
                    item.Campus = _buildingsService.GetCampus(userid, epId).ToList();

                }
            }
            if (showEp != null)
                TempData["showEp"] = true;
            else
                TempData["showEp"] = false;

            ViewBag.isPopUp = isPopUp;

            if (epsteps.Count > 0 && epsteps.FirstOrDefault().FloorAssetId.HasValue)
            {
                if (epsteps.SingleOrDefault().MainGoal.Any(x => x.Steps.Any(y => y.StepType == 2)))
                {
                    epsteps.FirstOrDefault().InspectionActivities = _transactionService.GetInspections(epsteps.FirstOrDefault().FloorAssetId.Value, epsteps.SingleOrDefault().EPDetailId).OrderByDescending(x => x.ActivityInspectionDate).Take(4).ToList();
                }
            }

            epsteps.FirstOrDefault().epdocument = _transactionService.GetInspectionDocs(epsteps.SingleOrDefault().InspectionId).ToList();
            epsteps.FirstOrDefault().epdetials = _epService.GetEpShortDescription(epId);

            return PartialView("_AssetsSteps", epsteps);
        }

        public static List<TInspectionFiles> CreateInspectionFiles()
        {
            var inspectionfiles = new List<TInspectionFiles>();
            var inspection = new TInspectionFiles
            {
                FileType = "C",
            };
            var inspection1 = new TInspectionFiles
            {
                FileType = "D",
            };
            inspectionfiles.Add(inspection);
            inspectionfiles.Add(inspection1);
            return inspectionfiles;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InspectionReportUpdate(List<TFloorAssets> floorAssets, int fileId, int epId, string fileName, string fileContent, string InspectionDate)
        {
            int userId = UserSession.CurrentUser.UserId;

            var assetsList = new List<MarkPassAssets>();
            var file = _commonService.GetFile(fileId);
            if (file != null)
            {
                foreach (var item in floorAssets)
                {
                    var insAsset = new MarkPassAssets
                    {
                        EPDetailId = epId,
                        FileId = file.TFileId,
                        FloorAssetId = item.FloorAssetsId,
                        AssetsId = item.Assets.AssetId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                        InspectionGroupId = item.InspectionGroupId,
                        InspectionDate = Convert.ToDateTime(InspectionDate)
                    };
                    assetsList.Add(insAsset);
                }


                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string postData =
                    _commonModelFactory.JsonSerialize<List<MarkPassAssets>>(assetsList);
                string uri =
                    $"{APIUrls.Goal_InspectionReportUpdate}/{userId}";
                string apiResult = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(apiResult);
                    var objEPSteps = httpresponse.Result.EPDocument.FirstOrDefault();
                    var resutnResult = new { Result = true, objEPSteps.IsInspReady };
                    return Json(resutnResult);
                }
            }

            return Json("");
        }


        public JsonResult MarkEPCompliant(int inspectionid, int epdetaild)
        {
            EPSteps epDocs = new();
            HttpResponseMessage httpresponseResult = new();
            epDocs = _transactionService.GetGoalStepsByActivity(null, null, epdetaild, inspectionid, 1, 0).FirstOrDefault();
            foreach (var item in epDocs.MainGoal)
            {
                foreach (var step in item.Steps)
                {
                    step.Status = 1;
                }
            }
            var tInspection = GenerateInspectionJson(epDocs, null, epDocs.FrequencyId.Value, string.Empty);
            httpresponseResult = Save(tInspection, 1);
            if (httpresponseResult.Success)
            {
                var result = new
                {
                    Result = true
                };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    Result = false
                };
                return Json(result);
            }
        }




        //[ChildActionOnly]
        public ActionResult AssetsInspection(int? epId, int? inspectionId, int? floorAssetId, int? mode, int ismultipleEP = 0)
        {
            var userId = UserSession.CurrentUser.UserId;
            var epDetail = new EPDetails();
            if (epId > 0)
            {
                var floorAssets = new TFloorAssets();
                epDetail = _transactionService.GetInspectionHistory(Convert.ToInt32(userId), epId == null ? 0 : Convert.ToInt32(epId), inspectionId == null ? 0 : Convert.ToInt32(inspectionId));

                if (floorAssetId > 0)
                    floorAssets = _floorAssetService.GetFloorAssetDescription(floorAssetId.Value);

                ViewBag.FloorAssetId = floorAssetId;
                if (floorAssets.Floor != null)
                {
                    ViewBag.FloorId = floorAssets.FloorId;
                    if (floorAssets.Floor.Building != null)
                        ViewBag.BuildingID = floorAssets.Floor.BuildingId;
                }
                var pageMode = "assetEps";

                if (mode.HasValue)
                    pageMode = (mode.Value == 1) ? "history" : "assetshistory";

                ViewBag.PageMode = pageMode;
                ViewBag.ismultipleEP = TempData["ismulipleEp"] ?? ismultipleEP;
            }
            if (inspectionId.HasValue)
            {
                ViewBag.IshowDoc = 0;
            }
            else
            {
                ViewBag.IshowDoc = 1;
            }
            if (epDetail != null && epDetail.EPDetailId > 0)
            {
                epDetail.EPsDocument = _transactionService.GetEpsDocument(epDetail.EPDetailId);
                if (epDetail.Inspection != null && epDetail.Inspection.InspectionId > 0)
                {
                    epDetail.InspectionEPDocs = _transactionService.GetInspectionDocs(epDetail.Inspection.InspectionId).ToList();
                }
            }
            return PartialView("_assetsInspection", epDetail);
        }
        //[ChildActionOnly]
        public ActionResult InspectionActivityHistory(int? epId, int? inspectionId)
        {
            List<TInspectionActivity> activity = new List<TInspectionActivity>();
            if (epId.HasValue && inspectionId.HasValue)
            {
                ViewBag.EPDetailId = epId.Value;
                activity = _inspectionActivityService.GetInspectionActivityDetails(inspectionId.Value);
            }
            var model = new InspectionActivityHistoryViewModel
            {
                activity = activity,
                EPDetails = _epService.GetEpDescription(epId.Value)
            };
            return PartialView("_InspectionActivityHistory", model);
        }


        #endregion

        #region EP Docs Inspection

        public ActionResult EPDocs(int epId, string epIds, int? inspectionid)
        {
            if (epIds != null)
            {
                TempData["epids"] = epIds;
            }
            int _inspectionId = inspectionid ?? 0;
            UISession.AddCurrentPage("goal_EpInspections", 0, "Document Inspection");

           

            var ePSteps = new EPSteps
            {

                EPDetailId = epId,
                InspectionId = 0
            };
            if (epIds == null|| string.IsNullOrEmpty(epIds))
            {
                ePSteps = _transactionService.GetGoalStepsByActivity(null, null, epId, _inspectionId, (int)BDO.Enums.Mode.DOC, 0).FirstOrDefault();
                if (_inspectionId == 0)
                    ePSteps.IsCurrentCycle = ePSteps?.Inspection?.IsCurrent==true ? Convert.ToInt32(ePSteps?.Inspection?.IsCurrent) : 0;
            }
            else
            {
                ePSteps.EPDetails = _epService.GetEpDescription(epId);
                ePSteps.DocName = ePSteps.EPDetails.DocumentType.FirstOrDefault()?.Name;

                var currentInspection = _transactionService.GetEpsInspections(epId).Where(x => x.IsCurrent).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (currentInspection != null)
                {
                    ePSteps.InspectionId = currentInspection.InspectionId;
                    ePSteps.Inspection = currentInspection;
                    ePSteps.Status = currentInspection.Status;
                    ePSteps.Inspection.InspectionDocs = new List<InspectionEPDocs>();
                    var inspectionepdocs = _transactionService.GetInspectionDocs(ePSteps.InspectionId).ToList();
                    ePSteps.Inspection.InspectionDocs = inspectionepdocs.GroupBy(test => test.FileId).Select(grp => grp.First()).ToList();
                }
                ePSteps.EpTransStatus = _epService.EpTransStatus(epId);
                ePSteps.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(ePSteps.InspectionId).FirstOrDefault(x => x.ActivityType == 1 && x.IsCurrent);

                ePSteps.ActivityType = Convert.ToInt32(BDO.Enums.Mode.DOC);
                if (ePSteps.InspectionFiles == null)
                    ePSteps.InspectionFiles = CreateInspectionFiles();
            }
            return View(ePSteps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EPDocs(EPSteps viewModel,
    int? fileId,
    string fileContent,
    string txtfileName,
    string fileext,
    string epId,
    string effectiveDate,
    string submitButton,
    string frequencyId,
    string docTypeId,
    string uploaddocTypeId,
    string filePaths,
    int? IsDocDashboard,
    string Comment,
    int? IsCurrentCycle,
    int? IsPreviousCycle,
    int? PreviousCycleInspectionId)
        {
            var inspectionId = 0;
            var inspectionGroupId = 1;
            var tinsActivityId = 0;
            var sucess = false;
            for (var i = 0; i < viewModel.EPStep.Count; i++)
            {
                if (!string.IsNullOrEmpty(uploaddocTypeId))
                    viewModel.EPStep[i].UploadDocTypeId = Convert.ToInt32(uploaddocTypeId);
               // viewModel.EPStep[i].IsCurrentCycle = Convert.ToInt32(IsCurrentCycle);
                viewModel.EPStep[i].IsPreviousCycle = Convert.ToInt32(IsPreviousCycle);
                viewModel.EPStep[i].PreviousCycleInspectionId = Convert.ToInt32(PreviousCycleInspectionId);
                viewModel.EPStep[i].TCycleId = viewModel.TCycleId;
                var httpResponseMessage = new HttpResponseMessage();
                var fileName = txtfileName.Trim() + "." + fileext.Trim();
                if (string.Equals(submitButton, "No", StringComparison.CurrentCultureIgnoreCase))
                    viewModel.EPStep[i].MainGoal = viewModel.EPStep[i].MainGoal.Where(x => x.ActivityType != 1).ToList();
                var tInspection = GenerateInspectionJson(viewModel.EPStep[i], fileId, 7, fileName, filePaths, fileContent, effectiveDate);

                switch (submitButton)
                {
                    case "Yes":
                        tInspection.IsAllDocumentUploaded = Convert.ToInt32(BDO.Enums.Status.Pass);
                        httpResponseMessage = Save(tInspection, 1);
                        break;
                    case "No":
                        tInspection.IsAllDocumentUploaded = Convert.ToInt32(BDO.Enums.Status.In_Progress);
                        httpResponseMessage = Save(tInspection, 2);
                        break;
                }
                if (httpResponseMessage.Result.Inspection != null)
                {
                    inspectionId = httpResponseMessage.Result.Inspection.InspectionId;
                    inspectionGroupId = httpResponseMessage.Result.Inspection.InspectionGroupId;
                    if (httpResponseMessage.Result.Inspection.TInspectionActivity.Count > 0)
                    {
                        tinsActivityId = httpResponseMessage.Result.Inspection.TInspectionActivity.FirstOrDefault().TInsActivityId;
                    }
                }

            }

            TempData["epdetailId"] = viewModel.EPDetailId;
            if (tinsActivityId > 0)
                sucess = true;

            if (IsDocDashboard == 1)
            {
                epId = "0";
                submitButton = "MultiDocUpload";
            }
            else
            {
                for (var i = 0; i < viewModel.EPStep.Count; i++)
                {
                    epId = (viewModel.EPStep[i].EPDetailId).ToString();
                }

            }
            var result = new
            {
                Result = sucess,
                pageMode = submitButton,
                inspectionId,
                inspectionGroupId,
                epId,
                tinsActivityId
            };

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveEpDoc(string activityId)
        {
            if (!string.IsNullOrEmpty(activityId))
            {
                var docActivityId = Guid.Parse(activityId);
                _transactionService.DeleteEpDocs(docActivityId);
            }

            return Json(activityId);
        }

        public PartialViewResult EpDocHistory(int epId)
        {


            var epDocs = _transactionService.GetEpInspectionDoc(epId);
            TempData.Keep();
            return PartialView("_epDocHistory", epDocs);
        }

        public ActionResult EPDocsPartial()
        {
            int epId = (int)TempData["epdetailId"];
            var epdocs = _transactionService.GetAllEpDocs(UserSession.CurrentUser.UserId, epId);
            TempData.Keep();
            return PartialView("_epdocs", epdocs);
        }

        public ActionResult GetEpDocSteps(int epDetailId, string epIds, int? inspectionid)
        {
            int[] _epIds = new int[1];
            int _Inspectionid = inspectionid ?? 0;
            if (epIds != "" && epIds != null)
            {

                _epIds = epIds.Split(',').Select(int.Parse).ToArray();
                TempData["IsDocDashboard"] = 1;
            }
            else
            {
                _epIds[0] = epDetailId;
                TempData["IsDocDashboard"] = 0;
            }

            var epdocs = new List<EPSteps>();
            var ep = new EPSteps();
            foreach (int epid in _epIds)
            {
                epDetailId = epid;
                int _Mode = Convert.ToInt32(BDO.Enums.Mode.DOC);
                var _epdocs = _transactionService.GetGoalStepsByActivity(null, null, epDetailId, _Inspectionid, _Mode, 0);
                foreach (EPSteps item in _epdocs)
                {
                    item.ActivityType = Convert.ToInt32(BDO.Enums.Mode.DOC);
                    item.IsCurrentCycle = Convert.ToInt32(item.Inspection.IsCurrent);
                }



                ep.EPStep.AddRange(_epdocs);
            }
            epdocs.Add(ep);
            return PartialView("_epDocSteps", epdocs);
        }

        public ActionResult EpDocFileUploadPreview(string docFiles)
        {
            var files = _commonService.GetTFiles(docFiles);
            return PartialView("_epDocFileUploadPreview", files);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMiscEPDocs(int? fileId, string txtfileName, string epId, string docTypeId, string filePaths)
        {
            var sucess = false;
            int inspectionId = 0;
            var viewModel = new EPSteps
            {
                InspectionId = 0,
                EPDetailId = Convert.ToInt32(epId),
                DoctypeId = Convert.ToInt32(docTypeId),
                ActivityType = Convert.ToInt32(BDO.Enums.Mode.DOC),
                DocInspectionGroupId = 1,
                UploadDocTypeId = Convert.ToInt32(BDO.Enums.UploadDocTypes.MiscDocuments)
            };
            var effectiveDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            viewModel.MainGoal = new List<MainGoal>();
            viewModel.InspectionFiles = new List<TInspectionFiles>();
            string fileName = txtfileName;
            var tInspection = GenerateInspectionJson(viewModel, fileId, 7, fileName, filePaths, effectiveDate);
            tInspection.IsAllDocumentUploaded = Convert.ToInt32(BDO.Enums.Status.In_Progress);
            var httpResponseMessage = Save(tInspection, 2);
            if (httpResponseMessage.Result.Inspection != null)
            {
                inspectionId = httpResponseMessage.Result.Inspection.InspectionId;
                sucess = true;
            }
            var result = new
            {
                Result = sucess,
                inspectionId
            };
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddMiscEPDocuments(string files, string epId, string docTypeId, int? IsPreviousCycle,
            int? PreviousCycleInspectionId)
        {
            bool sucess = false;
            int inspectionId = PreviousCycleInspectionId ?? 0;
            var tFiles = new List<TFiles>();
            if (!string.IsNullOrEmpty(files))
            {
                tFiles = _commonService.GetTFiles(files);
            }

            var tInspection = new Inspection
            {

                InspectionId = inspectionId,
                EPDetailId = Convert.ToInt32(epId),
                //FrequencyId = Convert.ToInt32(frequencyId),
                Status = Convert.ToInt32(BDO.Enums.Status.In_Progress),
                InspectionGroupId = 1,
                IsPreviousCycle = IsPreviousCycle > 0 ? IsPreviousCycle : 0,
                PreviousCycleInspectionId = IsPreviousCycle > 0 ? PreviousCycleInspectionId : 0
            };
            var activities = new List<TInspectionActivity>();
            var effectiveDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            foreach (var item in tFiles)
            {
                var objTInspectionActivity = new TInspectionActivity()
                {
                    ActivityType = Convert.ToInt32(BDO.Enums.Mode.DOC),
                    InspectionId = tInspection.InspectionId,
                    CreatedBy = UserSession.CurrentUser.UserId,
                    TInspectionDetail = new List<TInspectionDetail>()
                };
                if (!string.IsNullOrEmpty(item.FileName))
                {
                    objTInspectionActivity.InspectionDocs = new List<InspectionEPDocs>();
                    var inspectiondocs = new InspectionEPDocs
                    {
                        FileId = item.TFileId,
                        UploadDocTypeId = (int)item.FileType,
                        DocTypeId = Convert.ToInt32(docTypeId),
                        // UploadDocTypeId = Convert.ToInt32(Enums.UploadDocTypes.MiscDocuments)
                    };
                    if (!string.IsNullOrEmpty(item.FilePath))
                    {
                        inspectiondocs.Path = item.FilePath;
                    }

                    inspectiondocs.DocumentName = item.FileName;
                    inspectiondocs.EffectiveDate = effectiveDate != null ? Conversion.ConvertToTimeSpan(Convert.ToDateTime(effectiveDate).ToUniversalTime()) : 0;
                    objTInspectionActivity.InspectionDocs.Add(inspectiondocs);
                }
                activities.Add(objTInspectionActivity);
            }
            tInspection.TInspectionActivity = activities;
            tInspection.IsAllDocumentUploaded = Convert.ToInt32(BDO.Enums.Status.In_Progress);
            var httpResponseMessage = Save(tInspection, 2);
            if (httpResponseMessage.Result.Inspection != null)
            {
                inspectionId = httpResponseMessage.Result.Inspection.InspectionId;
                sucess = true;
            }
            var result = new
            {
                Result = sucess,
                inspectionId
            };
            return Json(result);
        }

        #endregion

        #region Risk_management

        //[CRxAuthorize(Roles = "dash_risk_management")]
        public ActionResult Risk_management()
        {
            UISession.AddCurrentPage("goal_Risk_management", 0, "Risk Management Dashboard");
            var objRisk = _goalMasterService.GetRiskCount(UserSession.CurrentUser.UserId);
            return View(objRisk);

        }
        //
        public ActionResult EpDeficiency(int? score)
        {
            UISession.AddCurrentPage("goal_EpDeficiency", 0, "Deficient EPs");
            var epdetails = _epService.GetUserDeficientEPs(UserSession.CurrentUser.UserId);
            ViewBag.Score = score;
            return View(epdetails.Where(x => x.Inspection != null && x.Inspection.TInspectionActivity.Any()).ToList());
        }

        public ActionResult EpDeficiencyDetails(int epDetailId, int inspectionId)
        {
            int userid = UserSession.CurrentUser.UserId;
            var epdetails = _epService.GetDeficientEPs(userid).Where(x => x.EPDetailId == epDetailId).ToList();
            return PartialView("EpDeficiencyDetails", epdetails);
        }

        #endregion

        #region EP frequency

        //[CrxAuthorize(Roles = "Goal_EpFrequency")]
        public ActionResult EpFrequency()
        {
            UISession.AddCurrentPage("goal_EpFrequency", 0, "EPFrequency");
            var epDetails = _epService.OngoingEpDetails().Where(m => m.IsActive = true).ToList();
            TempData["freqecny"] = _frequencyService.GetFrequency();
            return View(epDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateEpFrequency(int epDetailId, int frequencyId)
        {
            var newData = new EPFrequency
            {
                FrequencyId = frequencyId,
                EpDetailId = epDetailId,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            int res = _goalMasterService.UpdateEpFrequency(newData);
            if (res > 0)
            {
                var result = new { value = res, Result = true, Messgae = ConstMessage.Success_Updated };
                return Json(result);
            }
            return Json(new { value = res, Result = false, Messgae = ConstMessage.Frequency_Notupadted_Inprogress });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult UpdateEpInspStartDate(int epdetailId, string inspectiondate, int frequencyId)
        {
            if (!string.IsNullOrEmpty(inspectiondate.ToString()))
            {
                string joinDate = string.Empty;
                DateTime InspectionDate = DateTime.ParseExact(inspectiondate, "M/d/yyyy", CultureInfo.InvariantCulture);
                if (epdetailId > 0)
                {
                    var EpConfig = new EpConfig()
                    {
                        EPDetailId = epdetailId,
                        InspectionDate = InspectionDate,
                        ClientNo = UserSession.CurrentOrg.ClientNo,
                        IsActiveEp = true,
                        IsApplicable = true,
                        InspectionGroupId = -1,
                        CreatedBy = UserSession.CurrentUser.UserId
                    };
                    var data = ScheduleDateTime.GetDates(frequencyId, InspectionDate, 0, 0, 0, string.Empty);
                    if (data != null && data.Any())
                        joinDate = string.Join(",", data);


                    _transactionService.InsertUpdateEpConfig(EpConfig, 1, joinDate);
                }
            }
            var result = new { Result = true, EPDetailId = epdetailId };
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public JsonResult UpdateInspStartDate(int[] epdetailIds, string inspectiondate, int[] frequncyIds)
        {
            if (!string.IsNullOrEmpty(inspectiondate.ToString()) && epdetailIds.Length > 0 && frequncyIds.Length > 0)
            {
                DateTime InspectionDate = DateTime.ParseExact(inspectiondate.ToString(), "M/d/yyyy", CultureInfo.InvariantCulture);
                string joinDate = string.Empty;
                for (var i = 0; i < epdetailIds.Length; i++)
                {
                    var data = ScheduleDateTime.GetDates(frequncyIds[i], InspectionDate, 0, 0, 0, string.Empty);
                    if (data != null && data.Any())
                        joinDate = string.Join(",", data);
                    var EpConfig = new EpConfig()
                    {
                        EPDetailId = epdetailIds[i],
                        InspectionDate = InspectionDate,
                        ClientNo = UserSession.CurrentOrg.ClientNo,
                        IsActiveEp = true,
                        IsApplicable = true,
                        InspectionGroupId = -1,
                        CreatedBy = UserSession.CurrentUser.UserId
                    };
                    _transactionService.InsertUpdateEpConfig(EpConfig, 2, joinDate);
                }
            }
            var result = new { Result = true, EPDetailId = epdetailIds };
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateBulkEpFrequency(int[] epdetailIds, int frequncyId)
        {
            if (!string.IsNullOrEmpty(frequncyId.ToString()) && epdetailIds.Length > 0 && frequncyId > 0)
            {


                for (var i = 0; i < epdetailIds.Length; i++)
                {
                    var newData = new EPFrequency
                    {
                        FrequencyId = frequncyId,
                        EpDetailId = epdetailIds[i],
                        CreatedBy = UserSession.CurrentUser.UserId
                    };
                    _goalMasterService.UpdateEpFrequency(newData);
                }
            }
            var result = new { Result = true, EPDetailId = epdetailIds };
            return Json(result);
        }

        public ActionResult EpFrequencyScheduleDatePopUp()
        {
            var epDetails = _epService.OngoingEpDetails().Where(m => m.IsActive = true).ToList();
            return PartialView("EpFrequencyScheduleDatePopUp", epDetails);
        }
        public ActionResult EpFrequencyPopUp()
        {
            var epDetails = _epService.OngoingEpDetails().Where(m => m.IsActive = true).ToList();
            TempData["freqecny"] = _frequencyService.GetFrequency();
            return PartialView("EpFrequencyPopUp", epDetails);
        }

        #endregion 

        #region Document Repository

        public JsonResult DeleteDocuments(int documentRepoId)
        {
            var status = _documentService.DeleteDocuments(documentRepoId);
            return Json(status);
        }

        #endregion

        #region User Define Function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="fileId"></param>
        /// <param name="frequencyId"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        private Inspection GenerateInspectionJson(EPSteps viewModel,
            int? fileId,
            int frequencyId, string fileName, string filePaths = null,
            string fileContent = null,
            string effectiveDate = null)
        {

            var tInspection = new Inspection
            {
                InspectionId = Convert.ToInt32(viewModel.InspectionId),
                EPDetailId = Convert.ToInt32(viewModel.EPDetailId),
                FrequencyId = Convert.ToInt32(frequencyId),
                Status = Convert.ToInt32(BDO.Enums.Status.In_Progress),
                InspectionGroupId = viewModel.ActivityType == Convert.ToInt32(BDO.Enums.Mode.DOC) ? viewModel.DocInspectionGroupId : viewModel.InspectionGroupId,
                IsCurrentCycle = viewModel.IsCurrentCycle,
                IsPreviousCycle = viewModel.IsPreviousCycle,
                PreviousCycleInspectionId = viewModel.PreviousCycleInspectionId,
                TCycleId = viewModel.TCycleId
            };


            var activities = new List<TInspectionActivity>();
            var activityType = viewModel.MainGoal.GroupBy(x => x.ActivityType, (key, group) => group.First());
            if (activityType.Any())
            {
                foreach (var item in activityType)
                {
                    var inspectionActivity = new TInspectionActivity
                    {
                        ActivityType = item.ActivityType
                    };
                    if (item.ActivityType == 2)
                        inspectionActivity.FloorAssetId = viewModel.FloorAssetId;
                    if (item.ActivityType == 1)
                        inspectionActivity.Comment = viewModel.Comment;

                    inspectionActivity.InspectionId = tInspection.InspectionId;
                    inspectionActivity.CreatedBy = UserSession.CurrentUser.UserId;

                    if (viewModel.TInspectionActivity != null)
                        inspectionActivity.Comment = viewModel.TInspectionActivity.Comment;

                    if (item.ActivityType == 3)
                    {
                        inspectionActivity.DtEffectiveDate = effectiveDate != null ? (Convert.ToDateTime(effectiveDate).ToUniversalTime()) : DateTime.UtcNow;
                        inspectionActivity.ActivityInspectionDate = effectiveDate != null ? (Convert.ToDateTime(effectiveDate).ToUniversalTime()) : null;
                    }
                    //else
                    //{
                    //    inspectionActivity.ActivityInspectionDate = DateTime.UtcNow;
                    //}


                    inspectionActivity.TInspectionDetail = new List<TInspectionDetail>();
                    inspectionActivity.TInspectionDetail = SetJsonMainGoal(viewModel.MainGoal, tInspection, item.ActivityType);
                    activities.Add(inspectionActivity);
                }
            }
            else
            {
                var inspectionActivity = new TInspectionActivity
                {
                    ActivityType = viewModel.ActivityType,
                    InspectionId = tInspection.InspectionId,
                    CreatedBy = UserSession.CurrentUser.UserId,
                    TInspectionDetail = new List<TInspectionDetail>()
                };

                if (viewModel.TInspectionActivity != null)
                    inspectionActivity.Comment = viewModel.TInspectionActivity.Comment;

                inspectionActivity.TInspectionDetail = SetJsonMainGoal(viewModel.MainGoal, tInspection, viewModel.ActivityType);
                activities.Add(inspectionActivity);

            }
            if (viewModel.InspectionFiles != null && viewModel.InspectionFiles.Any())
            {
                tInspection.TInspectionFiles = new List<TInspectionFiles>();
                for (var k = 0; k < viewModel.InspectionFiles.Count; k++)
                {
                    if (viewModel.InspectionFiles[k].FileName != null || viewModel.InspectionFiles[k].Comment != null)
                    {
                        var insp =
                            new TInspectionFiles
                            {
                                Comment = viewModel.InspectionFiles[k].Comment,
                                FileContent = viewModel.InspectionFiles[k].FileContent,
                                FileName = viewModel.InspectionFiles[k].FileName,
                                FilePath = viewModel.InspectionFiles[k].FilePath,
                                FileType = viewModel.InspectionFiles[k].FileType
                            };
                        tInspection.TInspectionFiles.Add(insp);
                    }
                }
            }
            tInspection.TInspectionActivity = activities;

            var inspectiondocs = new InspectionEPDocs
            {
                FileId = fileId,
                DocTypeId = viewModel.DoctypeId,
                UploadDocTypeId = viewModel.UploadDocTypeId

            };
            if (!string.IsNullOrEmpty(fileName))
            {
                tInspection.TInspectionActivity[0].InspectionDocs = new List<InspectionEPDocs>();
                if (!string.IsNullOrEmpty(filePaths))
                {
                    inspectiondocs.Path = filePaths;
                    inspectiondocs.FileId = fileId;
                }
                else if (fileId.HasValue && fileId > 0)
                {
                    var tFiles = _commonService.GetFile(fileId.Value);
                    inspectiondocs.FilesContent = string.Empty;
                    if (tFiles != null)
                    {
                        inspectiondocs.Path = tFiles.FilePath;
                        inspectiondocs.FileId = tFiles.TFileId;
                    }
                }
                inspectiondocs.DocumentName = fileName;
                inspectiondocs.EffectiveDate = effectiveDate != null ? Conversion.ConvertToTimeSpan(Convert.ToDateTime(effectiveDate).ToUniversalTime()) : 0;
                tInspection.TInspectionActivity[0].InspectionDocs.Add(inspectiondocs);
            }
            if (viewModel.Campus.Any())
            {
                tInspection.Campus = new List<Buildings>();
                for (var k = 0; k < viewModel.Campus.Count; k++)
                {
                    if (viewModel.Campus[k].SiteId > 0)
                    {
                        var insp =
                            new Buildings
                            {
                                SiteId = viewModel.Campus[k].SiteId

                            };
                        tInspection.Campus.Add(insp);
                    }
                }
            }
            return tInspection;
        }

        public static List<TInspectionDetail> SetJsonMainGoal(List<MainGoal> allmainGoals, Inspection tInspection, int activityType)
        {
            var details = new List<TInspectionDetail>();
            var mainGoals = allmainGoals.Where(x => x.ActivityType == activityType).ToList();
            for (var i = 0; i < mainGoals.Count; i++)
            {
                var tInspectionDetail =
                    new TInspectionDetail
                    {
                        MainGoalId = mainGoals[i].MainGoalId,
                        InspectionDetailId = Convert.ToInt32(mainGoals[i].InspectionDetailId),
                        InspectionSteps = new List<InspectionSteps>()
                    };

                for (int j = 0; j < mainGoals[i].Steps.Count; j++)
                {
                    var tInspectionSteps =
                        new InspectionSteps
                        {
                            StepsId = mainGoals[i].Steps[j].StepsId,
                            Status = mainGoals[i].Steps[j].StepType == 1 ? mainGoals[i].Steps[j].Status : 1,
                            Comments = mainGoals[i].Steps[j].Comments,
                            FileContent = mainGoals[i].Steps[j].FileContent,
                            FileName = mainGoals[i].Steps[j].FileName,
                            FilePath = mainGoals[i].Steps[j].FilePath,
                            IsRA = mainGoals[i].Steps[j].IsRA,
                            RAScore = mainGoals[i].Steps[j].RAScore,
                            IsMarkDeficiencies = mainGoals[i].Steps[j].IsMarkDefeciencies,
                            DRTime = mainGoals[i].Steps[j].IsMarkDefeciencies ? 0 : mainGoals[i].Steps[j].DRTime,
                            InputValue = mainGoals[i].Steps[j].InputValue
                        };
                    tInspectionDetail.InspectionSteps.Add(tInspectionSteps);
                }
                details.Add(tInspectionDetail);
            }
            return details;
        }


        //[HttpPost]
        public ActionResult AddInspectionFiles(int inspectionId, IFormCollection data)
        {
            TInspectionFiles objTinsfiles = new TInspectionFiles();
            if (data.Files.Count > 0)
            {
                var files = HttpContext.Request.Form.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    objTinsfiles.StepsId = Convert.ToInt32(Request.Form["StepsId"]);
                    objTinsfiles.FileType = Request.Form["FileType"];
                    objTinsfiles.ActivityId = Guid.Parse(Request.Form["ActivityId"]);

                    string thePictureDataAsString = string.Empty;
                    using (var ms = new MemoryStream())
                    {
                        files[0].CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        thePictureDataAsString = Convert.ToBase64String(fileBytes);
                    }

                    objTinsfiles.FileContent = thePictureDataAsString;
                    objTinsfiles.FileName = files[0].FileName;
                    var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    var postData = _commonModelFactory.JsonSerialize<TInspectionFiles>(objTinsfiles);
                    var uri = $"{APIUrls.Goal_AddInspectionFiles}?inspectionId={inspectionId}";
                    var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    objTinsfiles = httpResponse.Result.TInspectionFiles;
                }
                return PartialView("_ILSMFiles", objTinsfiles);
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInspection"></param>
        /// <param name="status"></param>
        private HttpResponseMessage Save(Inspection objInspection, int status, string multipleEPDetailId = null)
        { 
            var httpResponse = new HttpResponseMessage();
            var mode = "";
            if (objInspection.TInspectionActivity[0].FloorAssetId != null)
                mode = BDO.Enums.Mode.ASSET.ToString();
            else if (objInspection.TInspectionActivity[0].FloorAssetId == null && objInspection.TInspectionActivity[0].InspectionDocs.Count > 0)
                mode = BDO.Enums.Mode.DOC.ToString();
            else
                mode = BDO.Enums.Mode.EP.ToString();

            var isComplete = (status == 1);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);

            var postData = JsonConvert.SerializeObject(objInspection);
            var uri = $"{APIUrls.Goal_Save}?Mode={mode}&IsComplete={isComplete}&multipleEPDetailId={multipleEPDetailId}";
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
            return httpResponse;
        }

        #endregion

        #region ILSM

        public ActionResult IlsmMatrix()
        {
            UISession.AddCurrentPage("goal_IlsmMatrix", 0, "ILSM Matrix");
            IlsmMatrixViewModel ilsmMatrix = new IlsmMatrixViewModel
            {
                MainGoals = _mainGoalService.GetILSMMainGoal().Where(x => x.ActivityType == 0).ToList(),
                IlsmEPs = _epService.GetIlsmEPs(),
                AffectedEPs = _goalMasterService.GetAffectedEPs()
            };
            return View(ilsmMatrix);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateIlsmMatrix(int epdetailId, int affectedEpDetailId, bool isActive, int stepsId)
        {
            var affectedEps = new AffectedEPs { AffectedEPDetailId = affectedEpDetailId, IsActive = isActive, CreatedBy = UserSession.CurrentUser.UserId, StepsId = stepsId };
            var status = _ilsmService.UpdateIlsmMatrix(affectedEps);
            return Json(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTIlsmComment(TComment tComment)
        {
            tComment = _inspectionActivityService.SaveTComment(tComment);
            return PartialView("_ILSMComment", tComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult StartNewInspection(int epdetailId, int inspectiongroupId)
        {
            var inspection = new Inspection
            {
                EPDetailId = epdetailId,
                InspectionGroupId = inspectiongroupId
            };
            var inspctionId = _transactionService.AddInspection(inspection);
            return Json(inspctionId);
        }


        public ActionResult IlsmScore()
        {
            UISession.AddCurrentPage("goal_IlsmScore", 0, "ILSM Score");
            var userId = UserSession.CurrentUser.UserId;
            var assets = _assetService.GetAssets(userId);
            return View(assets);
        }

        public ActionResult GetAssetsSteps(int assetId)
        {
            var maingoals = _mainGoalService.GetILSMMainGoal().ToList();
            ViewBag.MainGoals = maingoals.Where(x => x.ActivityType == 0).ToList();
            var assetSteps = _mainGoalService.GetAssetMainGoals(assetId);
            return PartialView("_GetAssetsSteps", assetSteps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIlsmScore(List<MainGoal> goals)
        {
            foreach (var goal in goals)
            {
                foreach (var step in goal.Steps)
                {
                    step.CreatedBy = UserSession.CurrentUser.UserId;
                    if (step.IlsmStepsMapping.Count > 0)
                    {
                        var ilsmAssetsMapping = step.IlsmStepsMapping.FirstOrDefault();
                        if (ilsmAssetsMapping.ILsmStepsId > 0)
                            step.IsIlsmLink = true;

                    }

                    if (step.RAScore > 0)
                        step.IsRA = true;
                    else
                    {
                        step.IsRA = false;
                        step.IsIlsmLink = false;
                    }

                    _ilsmService.UpdateIlsmScore(step);
                    foreach (var item in step.IlsmStepsMapping.Where(x => x.ILsmStepsId > 0))
                    {
                        _ilsmService.AddStepWithIlLsmStep(step.StepsId, item.ILsmStepsId);
                    }

                }
            }

            SuccessMessage = ConstMessage.Success_Updated;
            return RedirectToAction("IlsmScore");
        }

        [CrxAuthorize(Roles = "inspection_ilsm")]
        public ActionResult IlsmView(int? assetId)
        {
            UISession.AddCurrentPage("goal_IlsmView", 0, "ILSM");
            var tilsm = new List<TIlsm>();
            if (assetId>0)
             tilsm = _ilsmService.GetIlsmAsync().Where(x=>x.TIlsmfloorAssets.Where(x=>x.FloorAssetId == assetId).Any()).ToList();
            else
                tilsm = _ilsmService.GetIlsmAsync();
            return View(tilsm);
        }

        public ActionResult GetIlsmInspection(int tilsmId, string view)
        {
            UISession.AddCurrentPage("goal_GetIlsmInspection", 0, "ILSM Documentation");
            var tilsminspection = _ilsmService.GetIlsmInspection(tilsmId);
            if (tilsminspection.TIlsmId > 0)
            {
                ViewBag.IsRoundView = view;
                if (tilsminspection.TInspectionFiles == null)
                {
                    tilsminspection.TInspectionFiles = new List<TInspectionFiles>();
                    tilsminspection.TInspectionFiles = CreateInspectionFiles();
                }
                TempData.Put("TIlsm", tilsminspection);
                return View(tilsminspection);
            }
            else
            {
                return RedirectToAction("IlsmView");
            }
        }

        public ActionResult GetIlsmDetails(int tilsmId)
        {
            var tilsminspection = _ilsmService.GetIlsmDetails(tilsmId);
            return PartialView("_ilsmInspection", tilsminspection);
        }

        public JsonResult DeleteTInspectionFiles(int tinsFileId)
        {
            var result = _transactionService.DeleteTInspectionFiles(tinsFileId);
            return Json(result);
        }
        public ActionResult FailStepsUpdate(int epId, int inspectionId, int? floorAssetId, int? TilsmId)
        {
            var epsteps = new List<EPSteps>();
            var tilsminspection = TempData.Get<TIlsm>("TIlsm");
            ViewBag.TilsmId = null;
            var activity =tilsminspection.SourceInspection.TInspectionActivity.FirstOrDefault(x => x.FloorAssetId == floorAssetId.Value);           
            if (activity != null)
            {
                ViewBag.TilsmId = TilsmId;
                var _frequencyId = 0;
                TempData.Put("frequencyId", _frequencyId.ToString());

                if (floorAssetId.HasValue)
                {
                    var mode = Convert.ToInt32(BDO.Enums.Mode.ASSET);
                    epsteps = _transactionService.GetFloorAssetsActivity(epId, inspectionId, activity.ActivityId, activity.FloorAssetId.Value);
                    foreach (var item in epsteps)
                    {
                        item.IsInspReady = 1;
                        item.ActivityType = mode;
                        if (item.InspectionFiles == null)
                        {
                            item.InspectionFiles = CreateInspectionFiles();
                        }
                    }
                }

            }
            return View("_FailStepsUpdate", epsteps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInspection(EPSteps viewModel, int? fileId, string fileName, string btnSubmit, string pageName, string activityType, int frequencyId, string ActivityId, int? TilsmId)
        {
            var activityId = Guid.Parse(ActivityId);
            var createdBy = UserSession.CurrentUser.UserId;
            var objInspectionSteps = new List<InspectionSteps>();
            for (int i = 0; i < viewModel.MainGoal.Count; i++)
            {
                for (int j = 0; j < viewModel.MainGoal[i].Steps.Count; j++)
                {
                    var tInspectionSteps =
                        new InspectionSteps
                        {
                            StepsId = viewModel.MainGoal[i].Steps[j].StepsId,
                            Status = viewModel.MainGoal[i].Steps[j].StepType == 1 ? viewModel.MainGoal[i].Steps[j].Status : 1,
                            Comments = viewModel.MainGoal[i].Steps[j].Comments,
                            FileContent = viewModel.MainGoal[i].Steps[j].FileContent,
                            FileName = viewModel.MainGoal[i].Steps[j].FileName,
                            FilePath = viewModel.MainGoal[i].Steps[j].FilePath,
                            IsRA = viewModel.MainGoal[i].Steps[j].IsRA,
                            RAScore = viewModel.MainGoal[i].Steps[j].RAScore,
                            //Steps = StepsRepository.GetStep(viewModel.MainGoal[i].Steps[j].StepsId),
                            IsMarkDeficiencies = viewModel.MainGoal[i].Steps[j].IsMarkDefeciencies,
                            DRTime = viewModel.MainGoal[i].Steps[j].IsMarkDefeciencies ? 0 : viewModel.MainGoal[i].Steps[j].DRTime,
                            InputValue = viewModel.MainGoal[i].Steps[j].InputValue
                        };
                    objInspectionSteps.Add(tInspectionSteps);
                }
            }
            var postData = _commonModelFactory.JsonSerialize<List<InspectionSteps>>(objInspectionSteps);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var uri = $"{APIUrls.Goal_UpdateInspection}?createdBy={createdBy}&activityId={activityId}&tilsmId={TilsmId}";
            var results = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
                var result = new { Result = true, Messgae = httpresponse.Message };
                return Json(result);
            }
            return Json("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteTIlsm(int tilsmId)
        {
            var result = _ilsmService.CompleteIlsmIncident(tilsmId);
            var resultSave = new { Result = true, msg = result };
            return Json(resultSave);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IlsmInspection(TIlsm viewModel, string activityType, string btnSubmit, string tilsmId)
        {
            var tInspection = GenerateIlsmInspectionJson(viewModel);
            var msg = SaveIlsm(tInspection, 1, tilsmId);
            var resultSave = new { Result = true, msg = msg.Message };
            return Json(resultSave);
        }

        private HttpResponseMessage SaveIlsm(Inspection objInspection, int status, string tilsmId)
        {
            string mode = BDO.Enums.Mode.EP.ToString();
            bool isComplete = (status == 1);
            string postData = _commonModelFactory.JsonSerialize<Inspection>(objInspection);
            string uri = $"{APIUrls.Goal_SaveIlsm}?Mode={mode}&IsComplete={isComplete}&tilsmId={tilsmId}";
            int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var httpresponse = new HttpResponseMessage();
            string result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
            return httpresponse;
        }

        private Inspection GenerateIlsmInspectionJson(TIlsm viewModel)
        {
            var comment = "";
            var tInspection = new Inspection(viewModel.InspectionId);
            if (viewModel.TInspectionFiles != null && viewModel.TInspectionFiles.Count > 0)
            {
                comment = viewModel.TInspectionFiles[0].Comment;
                viewModel.TInspectionFiles[0].Comment = "";
                tInspection.TInspectionFiles = new List<TInspectionFiles>();
                tInspection.TInspectionFiles = viewModel.TInspectionFiles;

            }

            tInspection.TInspectionActivity = new List<TInspectionActivity>();
            var tinspectionActivity = new TInspectionActivity
            {
                Comment = comment
            };
            if (viewModel.TinspectionActivity[0].ActivityId != Guid.Empty)
                tinspectionActivity.ActivityId = viewModel.TinspectionActivity[0].ActivityId;
            tInspection.TInspectionActivity.Add(tinspectionActivity);
            tInspection.TInspectionActivity[0].InspectionId = tInspection.InspectionId;
            tInspection.TInspectionActivity[0].CreatedBy = UserSession.CurrentUser.UserId;

            tInspection.TInspectionActivity[0].TInspectionDetail = new List<TInspectionDetail>();


            for (int i = 0; i < viewModel.MainGoal.Count; i++)
            {
                var tInspectionDetail =
                    new TInspectionDetail
                    {
                        MainGoalId = viewModel.MainGoal[i].MainGoalId,
                        InspectionDetailId = Convert.ToInt32(viewModel.MainGoal[i].InspectionDetailId)
                    };
                tInspection.TInspectionActivity[0].TInspectionDetail.Add(tInspectionDetail);
                tInspection.TInspectionActivity[0].TInspectionDetail[i].InspectionSteps = new List<InspectionSteps>();
                for (int j = 0; j < viewModel.MainGoal[i].Steps.Count; j++)
                {
                    var tInspectionSteps =
                        new InspectionSteps
                        {
                            StepsId = viewModel.MainGoal[i].Steps[j].StepsId,
                            Status = viewModel.MainGoal[i].Steps[j].StepType == 1 ? viewModel.MainGoal[i].Steps[j].Status : 1,
                            Comments = viewModel.MainGoal[i].Steps[j].Comments,
                            FileContent = viewModel.MainGoal[i].Steps[j].FileContent,
                            FileName = viewModel.MainGoal[i].Steps[j].FileName,
                            FilePath = viewModel.MainGoal[i].Steps[j].FilePath,
                            IsRA = viewModel.MainGoal[i].Steps[j].IsRA,
                            RAScore = viewModel.MainGoal[i].Steps[j].RAScore,
                            Steps = _stepService.GetStep(viewModel.MainGoal[i].Steps[j].StepsId),
                            IsMarkDeficiencies = viewModel.MainGoal[i].Steps[j].IsMarkDefeciencies,
                            DRTime = viewModel.MainGoal[i].Steps[j].IsMarkDefeciencies ? 0 : viewModel.MainGoal[i].Steps[j].DRTime,
                            InputValue = viewModel.MainGoal[i].Steps[j].InputValue,
                            AddedBy = viewModel.MainGoal[i].Steps[j].AddedBy,
                            AddedDate = viewModel.MainGoal[i].Steps[j].AddedDate
                        };
                    tInspection.TInspectionActivity[0].TInspectionDetail[i].InspectionSteps.Add(tInspectionSteps);
                }
            }
            return tInspection;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTilsmReport(string tilsmId)
        {
            var objtilsm = _ilsmService.GetIlsmInspection(Convert.ToInt32(tilsmId));
            objtilsm.TIlsmId = Convert.ToInt32(tilsmId);
            objtilsm.FileName = "IlsmReports_" + tilsmId + ".pdf";
            objtilsm.FilesContent = _pdfService.CreateTilsmReportsbytes(Convert.ToInt32(tilsmId));
            var postData = _commonModelFactory.JsonSerialize<TIlsm>(objtilsm);
            var uri = APIUrls.Goal_GenerateIlsmReport;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, uri, ref statusCode);
            return Json(new { Result = "" });
        }

        #endregion

        #region EPAssignee

        public ViewResult EpAssignee(int? page, int? userId, int? standard)
        {
            UISession.AddCurrentPage("goal_EpAssignee", 0, "EP Assignee");

            ViewBag.Users = _userService.GetUsers();
            var epDetails = _epService.GetEpDetails();
            ViewBag.Standard = _standardService.GetStandards();
            if (standard.HasValue)
                epDetails = epDetails.Where(x => x.StDescID == standard.Value).ToList();

            return View(epDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateEpAssignee(int ePDetailId, int userId)
        {
            var epAssignees = new EPAssignee
            {
                UserId = userId,
                EPDetailId = ePDetailId,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            int status = _goalMasterService.AddEpAssignees(epAssignees);
            return Json(status);
        }

        public ActionResult UsersEps()
        {
            UISession.AddCurrentPage("goal_UsersEps", 0, "Users EPs");

            var userLists = _userService.GetUsers();
            var standards = _standardService.GetEPlists();
            TempData["Standards"] = standards;
            return View(userLists);
        }

        public ActionResult UsersEpsLists(int userId)
        {
            var UserEPs = _standardService.GetUserEPs(userId);
            TempData.Keep();
            return PartialView("_usersEps", UserEPs);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveUserEps(int userId, int epdetailId, string epdetailIds, bool status)
        {
            string type = "E";
            int res = _goalMasterService.SaveUserEPs(userId, epdetailId, epdetailIds, UserSession.CurrentUser.UserId, type, status);
            return Json(res);
        }


        #endregion

        #region EP Inspection Date

        public ActionResult EPInspectionDate()
        {
            ViewBag.Standards = _standardService.GetStandards().OrderBy(x => x.TJCStandard).ToList();
            var standard = new Standard();
            var epdetails = _epService.GetEpDetails().OrderBy(x => x.EPNumber).ToList();
            standard.EPDetails = epdetails;
            return View(standard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPInspectionDate(List<EPDetails> epdetails)
        {
            var postData = _commonModelFactory.JsonSerialize<List<EPDetails>>(epdetails);
            var uri = APIUrls.Goal_EPInspectionDate;
            var StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, uri, ref StatusCode);
            var standard = new BDO.Standard();
            ViewBag.Standards = _standardService.GetStandards();
            var eplist = _epService.GetEpDetails();
            standard.EPDetails = eplist;
            return View(standard);
        }


        public ActionResult EPDetails(int? id)
        {
            var standard = new Standard();
            if (id.HasValue)
            {
                standard = _standardService.GetStandard(id.Value);
                standard.EPDetails = _epService.GetEpDetails(id.Value).OrderBy(x => x.EPNumber).ToList();
            }
            else
                standard.EPDetails = _epService.GetEpDetails();
            ViewBag.Standards = _standardService.GetStandards().OrderBy(x => x.TJCStandard).ToList();
            return PartialView("_epdetails", standard);
        }


        [HttpGet]
        public ActionResult AddFixedInsDate(int? stDescId)
        {
            ViewBag.StDescID = stDescId;
            ViewBag.Frequencs = _frequencyService.GetFrequency().OrderBy(x => x.Days).ToList();
            ViewBag.Scores = _orgService.GetScores();
            ViewBag.Standards = _standardService.GetStandards();
            var standard = GetStandardEps(Convert.ToInt32(stDescId));
            standard.Inspectiongroup = _transactionService.GetInspectionGroup().Where(x => x.IsActive).ToList();
            ViewBag.InspectionGroups = new SelectList(standard.Inspectiongroup, "InspectionGroupId", "GroupName", "");


            return PartialView(standard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFixedInsDate(List<AssetsFixInsDate> objAssetsFixInsDate)
        {
            var postData = _commonModelFactory.JsonSerialize<List<AssetsFixInsDate>>(objAssetsFixInsDate);
            var uri = APIUrls.Goal_AddfixedInsDate;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                var res = new { Result = true, httpresponse.Message };
                return Json(res);
            }
            return Json("");
        }





        #endregion

        #region EP Document Inspection Dashboard

        [CrxAuthorize(Roles = "inspection_documentdashboard")]
        public ActionResult DocumentDashBoard()
        {
            UISession.AddCurrentPage("goal_DocumentDashBoard", 0, "Document Dashboard");
            var users = _userService.GetUsers().Where(x => (x.IsActive && !string.IsNullOrEmpty(x.Email) && x.EpsCount > 0) || x.UserId == HCF.Web.Base.UserSession.CurrentUser.UserId).OrderBy(x => x.FullName).ToList();

            return View(users);
        }

        public ActionResult DocumentDashBoardView()
        {
            var docs = _transactionService.GetPolicyBinders(UserSession.CurrentUser.UserId, UserSession.CurrentUser.UserId, null);
            return PartialView("_documentDashBoard", docs);
        }

        public ActionResult PartialDocDashBoard(int selectedUser, int inprogress = 0, int dueWitndays = 0, int pastDueordef = 0)
        {
            ViewBag.SelectedUser = selectedUser;
            ViewBag.Inprogress = inprogress;
            ViewBag.DueWitndays = dueWitndays;
            ViewBag.PastDueordef = pastDueordef;
            var docs = _transactionService.GetPolicyBinders(UserSession.CurrentUser.UserId, selectedUser, null);
            if (inprogress > 0)
            {
                docs = docs.Where(x => x.DocStatus == 2).ToList();
            }
            if (pastDueordef > 0)
            {
                docs = docs.Where(x => x.DocStatus != 1 && x.DocStatus != 2).ToList();
            }
            if (dueWitndays > 0)
            {
                docs = docs.Where(x => x.DueWithInDays <= 0 || x.DueWithInDays <= dueWitndays).ToList();
            }
            return PartialView("_documentDashBoard", docs);

        }

        #endregion

        #region

        public ActionResult EpInspections(int epId, string pagename = "")
        {
            UISession.AddCurrentPage("goal_EpInspections", 0, " EP Review");
            var ePSteps = new EPSteps
            {
                EPDetailId = epId,
                InspectionId = 0
            };
            ePSteps.EPDetails = _transactionService.GetCurrentEp(epId);
            if (ePSteps != null && ePSteps.EPDetails != null && ePSteps.EPDetails.EPDetailId > 0)
            {
                ePSteps.DocName = ePSteps?.EPDetails?.DocumentType?.FirstOrDefault()?.Name;
                List<Inspection> CampusInspection = _transactionService.GetEpsCampusInspections(epId).OrderByDescending(x => x.CreatedDate).ToList();
                List<Inspection> allInspection = _transactionService.GetEpsInspections(epId).OrderByDescending(x => x.CreatedDate).ToList();
                var currentInspection = allInspection.Where(x => x.IsCurrent && x.EPDetailId == epId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                var lastInspection = allInspection.OrderByDescending(x => x.CreatedDate).Take(2).FirstOrDefault(x => !x.IsCurrent);
                if (currentInspection != null)
                {
                    ePSteps.InspectionId = currentInspection.InspectionId;
                    ePSteps.Inspection = currentInspection;
                    ePSteps.Status = currentInspection.Status;
                    ePSteps.Inspection.InspectionDocs = new List<InspectionEPDocs>();
                    var inspectionepdocs = _transactionService.GetInspectionDocs(ePSteps.InspectionId).ToList();
                    ePSteps.Inspection.InspectionDocs = inspectionepdocs;
                    ePSteps.CampusInspection = CampusInspection;
                }

                ePSteps.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(ePSteps.InspectionId).Where(x => (x.DocTypeId != 108)).OrderByDescending(x => x.CreatedDate).FirstOrDefault(x => x.IsCurrent);
                if (ePSteps.TInspectionActivity == null)
                    ePSteps.TInspectionActivity = new TInspectionActivity();
                ePSteps.ActivityType = Convert.ToInt32(BDO.Enums.Mode.EP);
                if (ePSteps.InspectionFiles == null)
                    ePSteps.InspectionFiles = CreateInspectionFiles();

                if (lastInspection != null && currentInspection != null && currentInspection.Status == Convert.ToInt32(BDO.Enums.Status.In_Progress))
                {
                    ePSteps.LastInspection = lastInspection;
                    ePSteps.LastInspection.TInspectionActivity = new List<TInspectionActivity>();
                    ePSteps.LastInspection.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(lastInspection.InspectionId).Where(x => (x.ActivityType == 1)).OrderByDescending(x => x.CreatedDate).ToList();
                }

                ViewBag.epType = 0;
                var activitydetails = _inspectionActivityService.GetAllInspectionActivity(ePSteps.InspectionId).Where(x => (x.DocTypeId != 108) && x.IsCurrent).OrderByDescending(x => x.CreatedDate).ToList();
                if (ePSteps.EPDetails.IsAssetsRequired)
                {
                    ViewBag.epType = activitydetails.Where(x => x.ActivityType == 2).OrderBy(x => x.ActivityInspectionDate).FirstOrDefault(x => x.IsCurrent)?.ActivityType;
                }
                if (ePSteps.EPDetails.IsDocRequired && ePSteps.EPDetails.IsAssetsRequired == false)
                {
                    ViewBag.epType = activitydetails.Where(x => x.ActivityType == 3).OrderBy(x => x.ActivityInspectionDate).FirstOrDefault(x => x.IsCurrent)?.ActivityType;
                }
                if (ePSteps.EPDetails.IsDocRequired == false && ePSteps.EPDetails.IsAssetsRequired == false)
                {
                    ViewBag.epType = activitydetails.Where(x => x.ActivityType == 1 && x.Status == 1).OrderBy(x => x.ActivityInspectionDate).FirstOrDefault(x => x.IsCurrent)?.ActivityType;
                }
                if (ViewBag.epType == null)
                {
                    ViewBag.epType = 0;
                }
            }
            if (pagename == "epComplainceSteps")
            {
                ViewBag.triggerRequiredDoc = 1;
            }
            else
            {
                ViewBag.triggerRequiredDoc = 0;
            }
            var currentUser = UserSession.CurrentUser;

            var currentUserId = currentUser.UserId;
            var year = DateTime.Now.Year;
            var userId = (currentUser.IsPowerUser) ? 0 : currentUser.UserId;
            ePSteps.LastInspectionSummary = _epService.GetTinspectionCycle("StandardEP", "ASC", year, userId, currentUserId, null).Where(x => x.EPDetailId == epId).ToList();
            ePSteps.Campus = _buildingsService.GetCampus(currentUserId, epId).ToList();
            return View(ePSteps);
        }

        public ActionResult EpInspectionsHistory(int epId)
        {
            UISession.AddCurrentPage("goal_EpInspectionsHistory", 0, "EP Inspection History");
            var epDetail = _transactionService.GetEpInspectionHistory(epId);
            epDetail.EpTransStatus = _epService.EpTransStatus(epId);
            foreach (var item in epDetail.Inspections)
            {
                item.TInspectionActivity = new List<TInspectionActivity>();
                item.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(item.InspectionId).Where(x => x.ActivityType == 1 || x.ActivityType == 2).ToList();
                item.InspectionDocs = new List<InspectionEPDocs>();
                var inspectionepdocs = _transactionService.GetInspectionDocs(item.InspectionId).ToList();
                item.InspectionDocs = inspectionepdocs;
            }
            return View(epDetail);
        }

        #endregion

        #region CMS EPMapping

        public ActionResult CMSEpMapping()
        {
            UISession.AddCurrentPage("Goal_CMSEpMapping", 0, "CMS EpMapping");

            var model = new CmsEpMappingViewModel
            {
                CmsEpMapping = _epService.GetCmsEpMapping(),
                EPDetails = _epService.GetEpDetails(),
                CopDetails = _epService.GetCopDetails().ToList()
            };
            return View(model);
        }

        public ActionResult CMSstandardep(string standardeps, string cmsval)
        {
            List<EPDetails> EPDetails = new List<EPDetails>();
            List<int> numbers = standardeps.Split(',').Select(Int32.Parse).ToList();
            foreach (var item in numbers)
            {
                var epDetails = _epService.GetEpDetails().FirstOrDefault(x => x.EPDetailId == item);
                if (epDetails != null)
                    EPDetails.Add(epDetails);
            }
            return PartialView("_CMSstandardep", EPDetails);
        }

        public ActionResult CMSdetails(string cmsvalue, string standardeps)
        {
            List<CopDetails> CopDetails = new List<CopDetails>();
            List<int> numbers = cmsvalue.Split(',').Select(Int32.Parse).ToList();
            foreach (var item in numbers)
            {
                var list = _epService.GetCopDetails().FirstOrDefault(x => x.CopDetailsId == item);
                if (list != null)
                    CopDetails.Add(list);
            }
            return PartialView("_CMSdetails", CopDetails);
        }

        public ActionResult SaveCMSdata(string standardeps, string cmsval)
        {
            int id = 0;
            CmsEpMapping cmsEpMapping = new CmsEpMapping();
            List<int> eps = standardeps.Split(',').Select(Int32.Parse).ToList();
            List<int> cmsvalues = cmsval.Split(',').Select(Int32.Parse).ToList();
            cmsEpMapping.CreatedBy = UserSession.CurrentUser.UserId;
            foreach (var item in eps)
            {
                foreach (var item1 in cmsvalues)
                {
                    cmsEpMapping.EPDetailId = item;
                    cmsEpMapping.CopDetailsId = item1;
                    var isExists = _epService.GetCmsEpMapping().Any(x => x.CopDetailsId == cmsEpMapping.CopDetailsId && x.EPDetailId == cmsEpMapping.EPDetailId && !x.IsDeleted);
                    if (!isExists)
                        id = _goalMasterService.SaveCMSdata(cmsEpMapping);
                }
            }
            return Json(id);
        }

        public ActionResult DeleteCMSEpMapping(int id, string view)
        {
            if (view == "standardEps")
                _goalMasterService.Delete_CmsEpMapping(id, null);
            else
                _goalMasterService.Delete_CmsEpMapping(null, id);
            return RedirectToAction("CMSEpMapping");
        }
        #endregion

        #region EPReviewNotification
        public PartialViewResult EPReviewNotification(int frequencyId, int epid, string standardep, string PageName, DateTime DueDate, int RecentActivityType, DateTime effectiveDate, string epStatus)
        {
            DateTime NextDueDate;
            if (PageName == "DocUplaod" || PageName == "InspectByFloor")
            {
                NextDueDate = ScheduleDateTime.RecurUntilEpNotification(frequencyId, effectiveDate.ToUniversalTime(), 1);
            }
            else if (PageName == "scheduledDate")
            {
                NextDueDate = ScheduleDateTime.GetScheduleFixedDate(frequencyId, effectiveDate.ToUniversalTime(), 0);
            }
            else
            {
                NextDueDate = ScheduleDateTime.RecurUntilEpNotification(frequencyId, DateTime.Now.Date, 1);
            }
            var eP = new EPSteps
            {
                FrequencyId = frequencyId,
                EPDetailId = epid,
                StandardEP = standardep
            };
            if (epStatus == "IN")
            {
                if (PageName == "scheduledDate")
                {
                    eP.Inspection.RecentActivityType = -1;
                    eP.Inspection.DueDate = NextDueDate;
                }
                else
                {
                    eP.Inspection.RecentActivityType = RecentActivityType;
                    eP.Inspection.DueDate = DueDate;
                }
            }
            else
            {
                eP.Inspection.DueDate = NextDueDate;
                eP.Inspection.RecentActivityType = RecentActivityType;
            }
            eP.Inspection.StartDate = effectiveDate;
            eP.Type = PageName;

            return PartialView("_EpReviewNotification", eP);
        }
        #endregion

        #region EpRelation
        public JsonResult EpRelationStatus(int Epdetailid)
        {

            var results = _goalMasterService.EpRelationStatus(Epdetailid);
            return Json(new { result = (results) });
        }

        public JsonResult MarkRelationalEPCompliant(int epdetaild)
        {
            EPSteps epDocs = _transactionService.GetGoalStepsByActivity(null, null, epdetaild, 0, 1, 0).FirstOrDefault();
            foreach (var item in epDocs.MainGoal)
            {
                foreach (var step in item.Steps)
                {
                    step.Status = 1;
                }
            }

            var tInspection = GenerateInspectionJson(epDocs, null, epDocs.FrequencyId.Value, string.Empty);
            HttpResponseMessage httpresponseResult = Save(tInspection, 1);
            var objEPSteps = httpresponseResult.Result.EPDocument.FirstOrDefault();
            if (httpresponseResult.Success)
            {
                List<InspectionEPDocs> eprelationDocs = _transactionService.GetRelationEpInspectionDoc(epdetaild);
                foreach (var doc in eprelationDocs)
                {
                    doc.ActivityId = Guid.NewGuid();
                    var tinsDocId = _transactionService.AddInspectionDocs(doc);
                    var inspectionActivity = new TInspectionActivity
                    {
                        InspectionId = objEPSteps.InspectionId,
                        ActivityType = 3, // Convert.ToInt32(Enums.Mode.DOC),
                        ActivityId = doc.ActivityId,
                        Status = 1,
                        ActivityInspectionDate = System.DateTime.Now,
                        IsDeficiency = false,
                        SubStatus = BDO.Enums.InspectionSubStatus.NA.ToString(),
                        EPDetailId = epdetaild,
                        DtEffectiveDate = DateTime.UtcNow,
                        TinsDocId = tinsDocId,
                        DocTypeId = doc.DocTypeId > 0 ? doc.DocTypeId : null
                    };
                    _transactionService.AddTInspectionActivity(inspectionActivity);
                }
            }
            var result = new { Result = httpresponseResult.Success };
            return Json(result);
        }
        #endregion

        //public ActionResult LastInspectionSummary(int Year, int userId, int epId, string sortOrder = "", string OrderbySort = "", int? categoryId = null)
        //{
        //    sortOrder = string.IsNullOrEmpty(sortOrder) ? "StandardEP" : sortOrder;
        //    OrderbySort = string.IsNullOrEmpty(OrderbySort) ? "ASC" : OrderbySort;
        //    userId = (UserSession.CurrentUser.IsPowerUser) ? 0 : UserSession.CurrentUser.UserId;
        //    var currentUserId = UserSession.CurrentUser.UserId;

        //    List<InspectionReport> InspectionReport = _epService.GetTinspectionCycle("StandardEP", "ASC", Year, userId, currentUserId, null).Where(x => x.EPDetailId == epId).ToList();

        //    return PartialView("_LastInspectionSummary", InspectionReport);
        //}

        public ActionResult LastInspectionSummary(int epId, int? buildingid, int? frequencyid, int year, int month)
        {
            return ViewComponent("ComplianceHistory", new { epdetailid = epId, frequencyid = frequencyid, year = year, month = month });
        }

        public ActionResult EpSearchbyEpNumber(string epSearchText)
        {
            var currentUser = UserSession.CurrentUser;
            var userId = currentUser.IsPowerUser ? 0 : currentUser.UserId;
            var currentUserId = currentUser.UserId;
            var result = _goalMasterService.EpSearchbyEpNumber(epSearchText, currentUserId, userId);
            return Json(result);
        }

        #region Upgrade EP

        public ActionResult UpgradeEp(string stdescId, string epdetailId)
        {
            var CreatedBy = UserSession.CurrentUser.UserId;
            var epId = _epService.UpgradeEp(stdescId, epdetailId, CreatedBy);

            return Json(epId);
        }
        #endregion
    }
}
