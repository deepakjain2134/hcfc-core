using HCF.BDO;
using System;
using System.Linq;
using HCF.BAL;
using HCF.Utility;
using System.Collections;
using HCF.Web.Base;
using System.Globalization;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility.Extensions;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class ILSMController : BaseController
    {       
        private readonly IMainGoalService _mainGoalService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IIlsmService _ilsmService;
        private readonly ICommonService _commonService;
        private readonly IGoalMasterService _goalService;
        private readonly IEmailService _emailService;
        private readonly IEpService _epService;
        private readonly IPermitService _permitService;
        private readonly IHttpPostFactory _httpService;
        private readonly IPdfService _pdfService;
        private readonly ICommonModelFactory _commonModelFactory;

        public ILSMController(
            ICommonModelFactory commonModelFactory,        
            IGoalMasterService goalService,
            IMainGoalService mainGoalService,
            IWorkOrderService workOrderService,
            IIlsmService ilsmService,
            ICommonService commonService,
            IEmailService emailService, 
            IEpService epService, 
            IPermitService permitService, 
            IHttpPostFactory httpService, 
            IPdfService pdfService) 
        {
            _commonModelFactory = commonModelFactory;
            _mainGoalService = mainGoalService;
            _workOrderService = workOrderService;
            _ilsmService = ilsmService;
            _commonService = commonService;
            _goalService = goalService;
            _emailService = emailService;
            _epService = epService;
            _permitService = permitService;
            _httpService = httpService;
            _pdfService = pdfService;
        }   

        private static string _returntoIcra = string.Empty;

        #region Create an ILSM manually

        public ActionResult AddILSM(int? epdetailId, int? issueId, string redirectRoute = null, int? permitId = null, int? rId = null)
        {

            UISession.AddCurrentPage("ilsm_AddILSM", 0, "Add ILSM");

            var mainGoal = _mainGoalService.GetILSMMainGoal().Where(x => x.ActivityType == 0 && x.IsActive).ToList();
            var objTilsm = new TIlsm
            {
                TicraId = 0,
                EpDetailId = (epdetailId.HasValue && epdetailId.Value > 0) ? epdetailId.Value : epdetailId,
                MainGoal = mainGoal
            };
            if (rId != null && rId > 0)
                objTilsm.TRoundId = Convert.ToInt32(rId);

            if (permitId != null)
            {
                var fsbdetails = _permitService.GetFSBPermit(permitId);
                var fsbPbuilding = string.Join(",", fsbdetails.TFSBPBuildingDetails.Select(x => x.BuildingId));
                ViewBag.FSBBuilding = fsbPbuilding;
            }
            _returntoIcra = redirectRoute;
            if (TempData["IcraItem"] != null)
            {
                ArrayList arraylist=TempData.Get<ArrayList>("IcraItem");
               
                    objTilsm.Notes = Convert.ToString(arraylist[0]);
                    objTilsm.TicraId = Convert.ToInt32(arraylist[1]);
                
                _returntoIcra = "inspectionicra";
            }

            if (issueId.HasValue)
            {
                var wo = _workOrderService.GetWorkOrder(issueId.Value);
                if (wo != null)
                {
                    objTilsm.Notes = wo.Description;
                    objTilsm.FloorId = wo.FloorId;
                    objTilsm.IssueId = wo.IssueId;
                    objTilsm.Floor = wo.Floor;
                }
            }

            return View(objTilsm);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddILSM(TIlsm objtilsm)
        {
            var lists = objtilsm.MainGoal.SelectMany(x => x.Steps.Where(y => y.Status == 1))
                .Select(x => x.StepsId)
                .ToList();
            var listeps = _goalService.GetAffectedEPs().Where(w => lists.Contains(w.StepsId)).GroupBy(x => x.AffectedEPDetailId).Select(x => x.First());
            var epdetails = string.Join(",", listeps.Select(x => x.AffectedEPDetailId));

            objtilsm.ConstIlsmStepId = string.Join(",", lists);
            objtilsm.llsmDate = objtilsm.llsmDate.ToUniversalTime();

            if (!string.IsNullOrEmpty(objtilsm.llsmStartTime))
            {
                var dateTime = DateTime.ParseExact(objtilsm.llsmStartTime, "hh:mm tt", CultureInfo.InvariantCulture);
                objtilsm.llsmTime = dateTime.TimeOfDay;
            }

            objtilsm.CreatedBy = UserSession.CurrentUser.UserId;
            var dt = _ilsmService.InsertConstructionTilsm(objtilsm, epdetails);
            if (dt != null && dt.Rows.Count > 0)
            {
                var tilsmId = Convert.ToInt32(dt.Rows[0]["TILsmId"].ToString());
                var incidentId = dt.Rows[0]["IncidentId"].ToString();
                if (tilsmId > 0)
                {
                    NotificationMapping mailNotification = _commonService.MailNotification(BDO.Enums.NotificationCategory.ILSM.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString());
                    if (mailNotification != null)
                    {
                        var newobjtilsm = _ilsmService.GetIlsmInspection(tilsmId);
                        _emailService.SendManualIlsmMail(newobjtilsm, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM), mailNotification);
                    }
                    SuccessMessage = ConstMessage.ICRAToILSM_SaveMessage.Replace("[IncidentId]", incidentId);
                    if (objtilsm.TRoundId != null)
                        return RedirectToAction("RoundInspection", "Round", new { rid = objtilsm.TRoundId });
                    if (!string.IsNullOrEmpty(_returntoIcra))
                        return RedirectToRoute(_returntoIcra);
                    else
                        return RedirectToAction("AddIlsmShow", new { tilsmId });
                }
            }
            else
                ErrorMessage = ConstMessage.Error;
            return RedirectToRoute("ilsmView");
        }

        [HttpPost]
        public JsonResult UpdateILSM(TIlsm objtilsm)
        {
            var status = _ilsmService.UpdateIlsm(objtilsm);
            if (objtilsm.Status == BDO.Enums.ILSMStatus.Closed)
            {
                SaveTilsmReport(objtilsm.TIlsmId.ToString());
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.ILSM.ToString(), BDO.Enums.NotificationEvent.OnClosed.ToString()))
                    _emailService.SendIlsmCloseMail(objtilsm.TIlsmId, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM));
            }
            return ReturnJsonResult(status);
        }


        public string SaveTilsmReport(string tilsmId)
        {
            var objtilsm = _ilsmService.GetIlsmInspection(Convert.ToInt32(tilsmId));
            objtilsm.TIlsmId = Convert.ToInt32(tilsmId);
            objtilsm.FileName = "IlsmReports_" + tilsmId + ".pdf";
            objtilsm.FilesContent = _pdfService.CreateTilsmReportsbytes(Convert.ToInt32(tilsmId));
            var postData = _commonModelFactory.JsonSerialize<TIlsm>(objtilsm);
            var uri = APIUrls.Goal_GenerateIlsmReport;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            return result;
        }


        public ActionResult AddIlsmShow(int tilsmId)
        {
            UISession.AddCurrentPage("ilsm_AddILSMShow", 0, "Additional LS Measures");
            var tilsm = _ilsmService.GetIlsmInspection(tilsmId);
            tilsm.AllTilsmEPs = _epService.GetIlsmEPs();
            return View(tilsm);
        }

        public JsonResult AddAdditionalTilsmEP(int epdetailId, int tilsmId, bool isActive)
        {
            var status = _ilsmService.SaveAdditionalTilsmEP(epdetailId, tilsmId, isActive);
            return ReturnJsonResult(status);
        }

        public JsonResult ReopenILSM(TIlsm tilsm, string view)
        {
            if (tilsm.IsDeleted)
                tilsm.DeletedBy = UserSession.CurrentUser.UserId;
            var status = _ilsmService.UpdateILSMStatus(tilsm);
            if (tilsm.Status == BDO.Enums.ILSMStatus.Closed)
            {
              
                SaveTilsmReport(tilsm.TIlsmId.ToString());
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.ILSM.ToString(), BDO.Enums.NotificationEvent.OnClosed.ToString()))
                {
                    _emailService.SendIlsmCloseMail(tilsm.TIlsmId, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM));
                }
                if (view == "roundIlsm" && tilsm.TRoundId != null)
                    return ReturnJsonResult(tilsm.TRoundId);
            }
            else if (!tilsm.IsDeleted)
            {
                _emailService.SendIlsmReOpenMail(tilsm.TIlsmId, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM));
            }
            return ReturnJsonResult(status);
        }

        public JsonResult ILSMlinkToWO(int tilsmId, int issueId)
        {
            var status = _ilsmService.ILSMlinkToWO(tilsmId, issueId);
            return ReturnJsonResult(status);
        }

        public ActionResult CloseILSM(int tilsmId, string view)
        {
            var ilsm = _ilsmService.GetIlsm(tilsmId);
            ViewBag.IsRoundView = view;
            return PartialView("_closeILSMPopUp", ilsm);
        }

        #endregion

        #region Link ICRA to ILSM

        public JsonResult ILSMlinkToICRA(int tilsmId, int icraId)
        {
            var status = _ilsmService.IcraLinkToIslm(icraId, tilsmId, UserSession.CurrentUser.UserId);
            return ReturnJsonResult(status);
        }

        #endregion

        #region ILSM Binder
        public ActionResult IlsmBinder()
        {           
            var tilsm = _ilsmService.GetIlsmAsync();
            return View(tilsm.Where(x => x.Status == BDO.Enums.ILSMStatus.Closed).ToList());
        }


        #endregion

        #region Private Methods

        private JsonResult ReturnJsonResult(Object obj)
        {
            return Json(obj);
        }

        #endregion

    }
}