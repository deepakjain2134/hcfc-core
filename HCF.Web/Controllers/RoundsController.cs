using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
using HCF.Web.Base;
using System.Globalization;
using HCF.Web.Filters;
using HCF.BAL.Interfaces.Services;
using HCF.BAL.Interfaces;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HCF.Utility;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility.Extensions;
using HCF.BDO.Enums;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class RoundsController : BaseController
    {

        #region ctor

        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IBuildingsService _buildingsService;
        private readonly IRoundsService _roundService;
        private readonly IFloorService _floorService;
        private readonly IOrganizationService _organizationService;
        private readonly IWingService _wingService;
        private readonly IExercisesService _exercisesService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IShiftService _shiftService;
        private readonly IUserService _userService;
        private readonly IHttpPostFactory _httpService;
        private readonly IHCFSession _session;
        private readonly ILocationService _locationService;

        public RoundsController(
            IBuildingsService buildingsService,
            ILocationService locationService,
            IRoundsService roundService,
            IFloorService floorService,
            IOrganizationService organizationService,
            IWingService wingService,
            IExercisesService exercisesService,
            IHttpPostFactory httpService,
            IWorkOrderService workOrderService,
            IHCFSession session,
            IShiftService shiftService,
            IUserService userService,
            ICommonModelFactory commonModelFactory
            )
        {
            _session = session;
            _commonModelFactory = commonModelFactory;
            _exercisesService = exercisesService;
            _wingService = wingService;
            _buildingsService = buildingsService;
            _roundService = roundService;
            _floorService = floorService;
            _organizationService = organizationService;
            _workOrderService = workOrderService;
            _shiftService = shiftService;
            _userService = userService;
            _httpService = httpService;
            _locationService = locationService;
        }


        #endregion

        #region Rounds

        public ActionResult Rounds()
        {
            UISession.AddCurrentPage("Rounds_Rounds", 0, "Readiness Rounds");
            var buildings = _buildingsService.GetBuildingFloors();
            ViewBag.Title = "Readiness Rounds";
            return View(buildings);
        }


        public ActionResult RoundsCategories(int? rId)
        {
            var rounds = _roundService.GetRoundsCategories(UserSession.CurrentUser.UserId);
            ViewBag.RoundId = rId;
            return PartialView("_RoundsCategories", rounds);
        }

        public ActionResult FloorRounds(int? floorId, int? rId)
        {
            if (!floorId.HasValue)
                return RedirectToRoute("Rounds");
            UISession.AddCurrentPage("rounds_FloorRounds", 0, "Readiness Rounds");
            var rounds = new TRounds
            {
                FloorId = floorId,
                RoundCatId = rId
            };
            return View(rounds);
        }

        public JsonResult GetActiveRoundDeficiency()
        {
            var lists = _workOrderService.GetRoundWorkOrders().Where(x => x.CRxCode != "CMPLT" && x.CRxCode != "CANCL").Select(x => x.FloorId.Value).ToList();
            return Json(new { Result = lists });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCurrentRoundStatus()
        {

            var lists = _roundService.GetCurrentRoundStatus(14).Where(x => x.IsCurrent).ToList();
            return Json(new { Result = lists });

        }

        public ActionResult RoundsItems(int floorId, int rId, bool showShare)
        {
            var rounds = _roundService.GetRoundsCategories(UserSession.CurrentUser.UserId, floorId, 0, rId).FirstOrDefault();
            if (rounds != null)
            {
                rounds.DigitalSignature = new DigitalSignature();
                rounds.FloorId = floorId;
                rounds.Floors = _floorService.GetFloor(floorId);
                ViewBag.Wings = _wingService.GetFloorWing(floorId);
                return PartialView("~/Views/Rounds/_FloorRounds.cshtml", rounds);
            }
            else
                return View("Rounds");
        }


        public ActionResult RoundHistory(int? floorId, int? roundId, int? buildingId)
        {
            if (!floorId.HasValue || !roundId.HasValue)
                return RedirectToRoute("Rounds");
            UISession.AddCurrentPage("rounds_RoundHistory", 0, "Rounds History");
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = new SelectList(buildings, "BuildingId", "BuildingName", buildingId ?? 0);
            ViewBag.FloorId = floorId;
            return View();
        }


        public ActionResult RoundHistoryDetails(int floorId, int roundId, int troundId)
        {
            UISession.AddCurrentPage("rounds_RoundHistoryDetails", 0, "History Details");
            var tRounds = _roundService.GetRoundsHistory(roundId, floorId).Where(x => x.TRoundId == troundId).ToList();
            return View(tRounds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FloorRounds(TRounds tRounds, string submitButton)
        {
            switch (submitButton)
            {
                case "Save & Done":
                    Save(tRounds, 1);
                    break;
                case "Save Incomplete":
                    Save(tRounds, 2);
                    break;
                default:
                    return (View());
            }
            return RedirectToAction("Rounds", "Rounds");
        }






        private void Save(TRounds tRounds, int status)
        {
            bool isComplete = status == 1;
            tRounds.CreatedBy = UserSession.CurrentUser.UserId;
            string postData = _commonModelFactory.JsonSerialize<TRounds>(tRounds);
            string uri = $"{Utility.APIUrls.Rounds_Save}?IsComplete={isComplete}";
            int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, uri, ref statusCode);
        }

        public ActionResult FilterRounds(int floorId, int buildingId, string category)
        {
            var tRounds = _roundService.GetRoundsHistory(category, floorId, buildingId);
            return PartialView("_RoundHistory", tRounds);
        }

        #endregion

        #region Fire Drills

        //[OutputCache(Duration = 60 * 60)]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TodayDril"></param>
        /// <returns></returns>
        public ActionResult FireDrill(bool TodayDril = false)
        {
            UISession.AddCurrentPage("rounds_FireDrill", 0, "Fire Drill");
            var buildingType = _buildingsService.GetBuildingByType();
            var locationGroup = _locationService.GetLocationGroup().Where(x => x.IsActive && x.BuildingCount > 0).ToList();
            //ViewBag.BuildingType = buildingType;
            ViewBag.locationGroup = locationGroup;
            if (TodayDril)
                ViewBag.buildingIds = "-1";
            else
                ViewBag.buildingIds = null;
            ViewBag.TDril = TodayDril;
            return View(buildingType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingIds"></param>
        /// <param name="locationGroupIds"></param>
        /// <param name="mode"></param>
        /// <param name="year"></param>
        /// <param name="timeformat"></param>
        /// <param name="todaydrill"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult LoadFireDrill(string buildingIds, string locationGroupIds, string mode, int year, int timeformat, bool todaydrill = false)
        {
            Enum.TryParse(mode, out FireDrillMode enumMode);
            var org = _organizationService.GetOrganization();
            var isEditable = false;
            var quarter = _exercisesService.GetQuarterSettings(buildingIds, locationGroupIds, enumMode.ToString(), year, todaydrill);
            ViewBag.PlanFireDrillsForMe = org.PlanFireDrillsForMe;
            ViewBag.buildingIds = buildingIds;
            ViewBag.TDril = todaydrill;
            ViewBag.IsEditable = isEditable;
            ViewBag.buildingIds = buildingIds;
            ViewBag.timeFormat = timeformat;

            ViewBag.locationGroupIds = locationGroupIds;
            ViewBag.mode = mode;
            //ViewBag.TDril = todaydrill;
            return PartialView("_fireDrill", quarter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="quarter"></param>
        /// <param name="pageName"></param>
        /// <param name="texerciseId"></param>
        /// <param name="mode"></param>
        /// <param name="submitButton"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult FireDrill(QuarterMaster quarter, string pageName, string texerciseId, string mode, string submitButton = "1")
        {
            var httpresponse = new HttpResponseMessage();
            var objExercises = GenerateExerciseJson(quarter, texerciseId, mode);
            quarter.Exercises = objExercises;
            var postData = _commonModelFactory.JsonSerialize<QuarterMaster>(quarter);
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var results = _httpService.CallPostMethod(postData, Utility.APIUrls.Rounds_FireDrills, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
            }
            var buildingType = _buildingsService.GetBuildingByType();
            var locationGroup = _locationService.GetLocationGroup().Where(x => x.IsActive && x.BuildingCount > 0).ToList();
            ViewBag.locationGroup = locationGroup;

            if (!string.IsNullOrEmpty(submitButton) && Convert.ToInt32(submitButton) >= 0)
            {
                var result = new
                {
                    Isload = Convert.ToInt32(submitButton) > 0,
                    IsGenerateReport = Convert.ToInt32(submitButton) > 0,
                    texerciseId = httpresponse.Result.QuarterMasterSetting.Exercises.FirstOrDefault().TExerciseId,
                    //quarterId = httpresponse.Result.QuarterMasterSetting.QuarterId,
                    date = httpresponse.Result.QuarterMasterSetting.Exercises.FirstOrDefault().Date.ToFormatDate(),
                    httpresponse.Result.QuarterMasterSetting
                };
                return Json(result);
            }
            else
            {
                if (ViewBag.buildingIds != null)
                {
                    ViewBag.buildingIds = ViewBag.buildingIds;
                }
                return View(buildingType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tExerciseId"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult RemoveExercise(int tExerciseId)
        {
            var status = _exercisesService.DeleteExercises(tExerciseId);

            var result = new { Result = status };
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult LoadTExercise(int exerciseId)
        {
            var texerise = new List<TExerciseFiles>();
            if (exerciseId > 0)
            {
                var exerise = _exercisesService.GetExerciseFiles(exerciseId).FirstOrDefault();
                if (exerise != null)
                    texerise = exerise.TExerciseFiles;
            }
            ViewBag.IsFireDrillReportCreated = texerise.Where(x => x.DocumentType == Convert.ToInt32(BDO.Enums.FileType.FireDrillReport)).Count() > 0 ? 1 : 0;
            return PartialView("_loadTExercise", texerise);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firedrilllocationId"></param>
        /// <param name="locationGroupIds"></param>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="shiftId"></param>
        /// <param name="shiftNo"></param>
        /// <param name="buildingNo"></param>
        /// <param name="texerciseCount"></param>
        /// <param name="name"></param>
        /// <param name="buildingIds"></param>
        /// <returns></returns>
        public ActionResult AddNewDrill(int firedrilllocationId, string locationGroupIds, string mode, int type, int year, int shiftId, int shiftNo, int buildingNo, int texerciseCount, string name, string buildingIds)
        {
            //var quarter = new QuarterMaster();
            //var b = new Buildings { BuildingId = buildingId };
            Enum.TryParse(mode, out FireDrillMode enumMode);
            List<TExercises> exercises = new List<TExercises>();
            List<TExerciseFiles> tExercisesfiles = new List<TExerciseFiles>();
            var quarter = _exercisesService.GetQuarterSettings(buildingIds, locationGroupIds, enumMode.ToString(), year, false);
            var newDrillTypes = new FireDrillTypes { IsAdded = true, FireDrillType = name, Id = type, IsActive = true };
            quarter.FireDrillTypes.Add(newDrillTypes);
            foreach (var item in quarter.Buildings.Where(x => x.BuildingId == firedrilllocationId || x.LocationGroupId == firedrilllocationId))
            {
                foreach (var shift in item.Shifts.Where(x => x.ShiftId == shiftId))
                {
                    _exercisesService.GetExercises(shift, shift.ShiftId, item.BuildingId, quarter.FireDrillTypes, exercises, tExercisesfiles, enumMode.ToString());
                }
            }
            //quarter.Buildings.Add(b);
            ViewData["drillType"] = newDrillTypes;
            ViewData["i"] = buildingNo;
            ViewData["kk"] = shiftNo;
            ViewData["m"] = type;
            ViewData["texerciseCount"] = (texerciseCount * 4); // 4 for quarter
            return PartialView("_yearlyFiredrill", quarter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingIds"></param>
        /// <param name="locationGroupIds"></param>
        /// <param name="mode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult PlanFireDrill(string buildingIds, string locationGroupIds, string mode, int year)
        {
            int[] buildArray;
            var exerise = new List<TExercises>();
            Enum.TryParse(mode, out FireDrillMode enumMode);
            if (FireDrillMode.ByBuildings.ToString() == enumMode.ToString())
            {
                buildArray = buildingIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                exerise = _exercisesService.GetExercises(null).ToList().Where(x => x.BuildingId != null && buildArray.Contains(x.BuildingId.Value) && x.Year == year && x.IsDeleted == false && x.DrillType == 0).ToList();
            }
            else
            {
                buildArray = locationGroupIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                exerise = _exercisesService.GetExercises(null).ToList().Where(x => x.LocationGroupId != null && buildArray.Contains(x.LocationGroupId.Value) && x.Year == year && x.IsDeleted == false && x.DrillType == 0).ToList();
            }
            if (exerise.Count == 0)
            {
                return Json(new { success = "Error" });
            }
            else
            {
                GenerateFireDrill(exerise, year);
            }
            return Json(new { success = ErrorMessage });
        }

        private void GenerateFireDrill(List<TExercises> exerises, int year)
        {
            var exercises = exerises;
            var lastexercise = exerises.OrderByDescending(x => x.QuarterNo).GroupBy(test => new { test.ShiftId, test.BuildingId, test.LocationGroupId }).Select(grp => grp.First()).ToList();
            foreach (var exerise in lastexercise)
            {
                //var _exer = exerises.Where(x => x.TExerciseId == exerise.TExerciseId).FirstOrDefault();
                //exerise.IsAdditional = _exer.IsAdditional;
                _exercisesService.PlanDrill(exerise, year, exerise.QuarterNo);
            }
        }

        public ActionResult DeleteRowDrill(int buildingId, int year, int shiftId, string name)
        {
            bool isdeleted = true;
            var exercises = _exercisesService.GetExercises(null);
            exercises = exercises.Where(x => x.BuildingId == buildingId && x.Year == year && x.ShiftId == shiftId && x.Name == name).ToList();
            foreach (var _exer in exercises)
            {
                isdeleted = _exercisesService.DeleteExercises(_exer.TExerciseId);
            }
            return Json(isdeleted);
        }



        private List<TExercises> GenerateExerciseJson(QuarterMaster viewModel, string texerciseId, string mode)
        {
            return (from item in viewModel.Buildings
                    from itemShift in item.Shifts
                    from itemExercise in itemShift.Exercises
                    select new TExercises
                    {
                        TExerciseId = !string.IsNullOrEmpty(texerciseId) ? Convert.ToInt32(texerciseId) : itemExercise.TExerciseId,
                        Announced = itemExercise.Announced,
                        DateTimeSpan = (Utility.Conversion.ConvertToTimeSpan(itemExercise.Date) != 0 && Utility.Conversion.ConvertToTimeSpan(itemExercise.Date) > 0) ? Utility.Conversion.ConvertToTimeSpan(itemExercise.Date) : itemExercise.DateTimeSpan,
                        BuildingId = itemExercise.BuildingId,
                        LocationGroupId = itemExercise.LocationGroupId,
                        ShiftId = itemExercise.ShiftId,
                        StartTime = itemExercise.StartTime,
                        Status = itemExercise.Status,
                        Comment = itemExercise.Comment,
                        Name = itemExercise.Name,
                        //QuarterId = Convert.ToInt32(viewModel.QuarterId),
                        QuarterNo = itemExercise.QuarterNo,
                        CreatedBy = UserSession.CurrentUser.UserId,
                        TExerciseFiles = itemExercise.TExerciseFiles,
                        DigitalSignature = itemExercise.DigitalSignature,
                        TExerciseQuestionnaires = itemExercise.TExerciseQuestionnaires,
                        TExerciseActions = itemExercise.TExerciseActions,
                        NearBy = itemExercise.NearBy,
                        DrillType = itemExercise.DrillType,
                        Observers = itemExercise.Observers,
                        ConductedBy = ((!string.IsNullOrEmpty(itemExercise.ConductedBy)) ? itemExercise.ConductedBy : UserSession.CurrentUser.Name),
                        EducationComment = itemExercise.EducationComment,
                        CritiquesComment = itemExercise.CritiquesComment,
                        CritiqueDigitalSignature = itemExercise.CritiqueDigitalSignature,
                        EducationDigitalSignature = itemExercise.EducationDigitalSignature,
                        FireDrillMode = Convert.ToInt32(mode),
                        IsAdditional = itemExercise.IsAdditional,
                        IsAudible = itemExercise.IsAudible,
                        //Year = itemExercise.Year,
                        Year = viewModel.Year,
                        FiredrillDocStatus = (!string.IsNullOrEmpty(itemExercise.FiredrillDocStatus.ToString()) && itemExercise.FiredrillDocStatus > 0) ? itemExercise.FiredrillDocStatus : Convert.ToInt32(BDO.Enums.FiredrillDocStatus.DocumentationPending),
                        //CritiqueDate = itemExercise.CritiqueDate,
                        //EducationDate = itemExercise.EducationDate,
                        CritiqueDateTimeSpan = (itemExercise.CritiqueDate != null) ? Utility.Conversion.ConvertToTimeSpan(itemExercise.CritiqueDate) : Utility.Conversion.ConvertToTimeSpan(itemExercise.Date),
                        EducationDateTimeSpan = (itemExercise.EducationDate != null) ? Utility.Conversion.ConvertToTimeSpan(itemExercise.EducationDate) : Utility.Conversion.ConvertToTimeSpan(itemExercise.Date),
                    }).Where(x => x.DateTimeSpan > 0).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TExerciseId"></param>
        /// <returns></returns>
        public ActionResult GenerateFiredrillDoc(int TExerciseId)
        {
            var httpresponse = new HttpResponseMessage();
            var exercise = _exercisesService.GetExercises(TExerciseId);
            var postData = _commonModelFactory.JsonSerialize<TExercises>(exercise.FirstOrDefault());
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var uri = $"{Utility.APIUrls.Rounds_GenerateFireDrillDoc}";
            var results = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
            }
            return Json(true);
        }

        public ActionResult DeleteTExerciseActions(int texerciseActionId)
        {
            var status = _exercisesService.DeleteTExerciseActionsTExerciseActionId(texerciseActionId);
            return Json(new { Result = status });
        }

        public ActionResult firedrillCalender(string start)
        {
            DateTime myDate = DateTime.ParseExact(start, "d/M/yyyy", CultureInfo.InvariantCulture);
            var exercises = _exercisesService.GetExercises(null);
            exercises = exercises.Where(x => x.Date == myDate).ToList();
            return PartialView("_FiredrillCalender", exercises);
        }


        public ActionResult FireDrillReport()
        {
            UISession.AddCurrentPage("rounds_FireDrillReport", 0, "Fire Drill Report");
            var buildingType = _buildingsService.GetBuildingByType();
            ViewBag.BuildingType = buildingType;
            return View();
        }

        public ActionResult GetFireDrillReportData(string buildingIds, int quarterno)
        {
            List<TExercises> listExercises = new List<TExercises>();
            listExercises = _exercisesService.GetExerciseFiles(buildingIds, quarterno);
            return PartialView("_firedrillreport", listExercises);
        }

        #region Fire Drill Settings

        public ActionResult FireDrillSettings()
        {
            UISession.AddCurrentPage("fire_drill", 0, "Fire Drill Settings");
            var org = _organizationService.GetOrganization();
            return View(org);
        }

        [HttpPost]
        public ActionResult FireDrillSettings(bool status)
        {
            var org = new Organization();
            org.PlanFireDrillsForMe = status;
            var result = _organizationService.FireDrillSettings(org);
            return Json(result);
        }

        #endregion Fire Drill Settings

        //[AjaxOnly]
        //public ActionResult LoadFireDrill(int buildingTypeId, int quaterId, int year, bool todaydrill = false)
        //{
        //    var org = OrganizationRepository.GetOrganization();
        //    var isEditable = false;
        //    var currentQuarter = Convert.ToInt32(DateTime.Now.Year + "" + Common.GetQuarter(DateTime.Now));
        //    var selectedQuarter = Convert.ToInt32(year + "" + quaterId);
        //    if (selectedQuarter >= currentQuarter)
        //        isEditable = true;
        //    var quarter = ExercisesRepository.GetQuarterSettings(buildingTypeId, quaterId, year, todaydrill).FirstOrDefault();

        //    ViewBag.IsEditable = isEditable;
        //    ViewBag.TDril = todaydrill;
        //    return PartialView("_fireDrill", quarter);
        //}  


        //public ActionResult BindNewDrill(int buildingId, int type, int buildingTypeId, int quarterId, int year, int buildingNo, int drillNo, string name)
        //{
        //    var quarter = new QuarterMaster(); //ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterId, year).FirstOrDefault();
        //    var b = new Buildings { BuildingId = buildingId };
        //    var getquarter = ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterId, year, false);
        //    quarterId = Convert.ToInt32(getquarter.Select(x => x.QuarterId).FirstOrDefault());
        //    var newDrillTypes = new FireDrillTypes { IsAdded = true, FireDrillType = name, Id = type, IsActive = true };
        //    quarter.FireDrillTypes.Add(newDrillTypes);
        //    b.FireDrillTypes = new List<FireDrillTypes> { newDrillTypes };
        //    var shifts = ShiftRepository.GetShift().Where(x => x.IsActive);
        //    foreach (var dhiShift in shifts)
        //    {
        //        // var dhiShift = new Shift();
        //        var exercises = new TExercises { DrillType = type, BuildingId = buildingId, QuarterId = quarterId, Name = name, IsAdditional = true, ShiftId = dhiShift.ShiftId };
        //        dhiShift.Exercises = new List<TExercises> { exercises };
        //        b.Shifts.Add(dhiShift);
        //    }

        //    quarter.Buildings.Add(b);
        //    // ViewData["ii"] = type;
        //    ViewData["kk"] = drillNo;
        //    ViewBag.buildingNo = buildingNo;

        //    ViewData["drillType"] = newDrillTypes;
        //    return PartialView("_buildingFireDrill", quarter);
        //}

        //public ActionResult GetQuarterMasterCount(int buildingTypeId, int quaterId, int year, int? buildingId, string sitecode = null)
        //{
        //    var quater = ExercisesRepository.GetQuarterMaster(buildingTypeId, quaterId, year, buildingId, sitecode).FirstOrDefault();
        //    return PartialView("_firedrillCountDetails", quater);
        //}

        //public ActionResult PlanFireDrill(int buildingTypeId, int quaterId, int year)
        //{
        //    var getquarter = ExercisesRepository.GetQuarterSettings(buildingTypeId, quaterId, year, false);
        //    var currentQuarter = getquarter.Select(x => x.QuarterId).FirstOrDefault();
        //    var exerise = ExercisesRepository.GetExercises(null).Where(x => x.QuarterId == Convert.ToInt32(currentQuarter) && x.IsDeleted == false && x.DrillType == 0).ToList();
        //    var currentquarterId = quaterId;
        //    if (exerise.Count == 0)
        //    {
        //        getquarter = ExercisesRepository.GetQuarterSettings(buildingTypeId, quaterId - 1, year, false);
        //        currentQuarter = getquarter.Select(x => x.QuarterId).FirstOrDefault();
        //        quaterId = Convert.ToInt32(getquarter.Select(x => x.QuarterNo).FirstOrDefault());
        //        exerise = ExercisesRepository.GetExercises(null).Where(x => x.QuarterId == Convert.ToInt32(currentQuarter) && x.IsDeleted == false && x.DrillType == 0).ToList();
        //        currentquarterId = quaterId;
        //        if (exerise.Count == 0)
        //        {
        //            return Json(new { success = "Error" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    if (buildingTypeId == 2) { year = year + 1; } else { quaterId = quaterId + 1; }
        //    if (quaterId > 4)
        //    {
        //        quaterId = 1;
        //        GenerateFireDrill(buildingTypeId, exerise, quaterId, year, currentquarterId);
        //    }
        //    else
        //    {
        //        GenerateFireDrill(buildingTypeId, exerise, quaterId, year, currentquarterId);
        //    }
        //    return Json(new { success = ErrorMessage }, JsonRequestBehavior.AllowGet);
        //}     

        //public ActionResult DeleteRowDrill(int buildingId, int buildingTypeId, int quarterId, int year, int buildingNo, string name)
        //{
        //    bool isdeleted = false;
        //    var getquarter = ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterId, year, false);
        //    var currentQuarter = getquarter.Select(x => x.QuarterId).FirstOrDefault();
        //    var exercises = ExercisesRepository.GetExercises(null);
        //    exercises = exercises.Where(x => x.BuildingId == buildingId && x.QuarterId == currentQuarter && x.Name == name).ToList();
        //    foreach (var _exer in exercises)
        //    {
        //        isdeleted = ExercisesRepository.DeleteExercises(_exer.TExerciseId);
        //    }
        //    return Json(isdeleted, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult FireDrill(QuarterMaster quarter, string pageName, string texerciseId, string mode, string submitButton = "1")
        //{
        //    var httpresponse = new HttpResponseMessage();
        //    var objExercises = GenerateExerciseJson(quarter, texerciseId, mode);
        //    quarter.Exercises = objExercises;
        //    var postData = _commonModelFactory.JsonSerialize<QuarterMaster>(quarter);
        //    var statusCode = Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success);
        //    var results = _httpService.CallPostMethod(postData, Utility.APIUrls.Rounds_FireDrill, ref statusCode);
        //    if (statusCode == Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success))
        //    {
        //        httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
        //    }
        //    var buildingType = BuildingsRepository.GetBuildingByType();

        //    if (!string.IsNullOrEmpty(submitButton) && Convert.ToInt32(submitButton) > 0)
        //    {
        //        var result = new
        //        {
        //            Isload = Convert.ToInt32(submitButton) > 0,
        //            IsGenerateReport = Convert.ToInt32(submitButton) > 0,
        //            texerciseId = httpresponse.Result.QuarterMasterSetting.Exercises.FirstOrDefault().TExerciseId,
        //            quarterId = httpresponse.Result.QuarterMasterSetting.QuarterId,
        //            date = httpresponse.Result.QuarterMasterSetting.Exercises.FirstOrDefault().Date.ToFormatDate(),
        //            httpresponse.Result.QuarterMasterSetting
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return View(buildingType);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoadFireDrillForm()
        //{
        //    var statusCode = Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success);
        //    var results = _httpService.CallGetMethod(Utility.APIUrls.Rounds_LoadFireDrillForm, ref statusCode);
        //    if (statusCode == Convert.ToInt32(Utility.Enums.HttpReponseStatusCode.Success))
        //    {
        //        var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
        //        string fileUrl = Common.FilePath(httpresponse.Result.result);
        //        var result = new { Result = true, Fileurl = fileUrl };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        //private QuarterMaster GenerateQuarterJson(QuarterMaster viewModel)
        //{
        //    QuarterMaster objQuarterMaster = new QuarterMaster
        //    {
        //        TotalPlanned = viewModel.TotalPlanned,
        //        TotalUnAnnounced = viewModel.TotalUnAnnounced,
        //        TotalDone = viewModel.TotalDone,
        //        TotalUnAnnouncedDone = viewModel.TotalUnAnnouncedDone,
        //        QuarterId = viewModel.QuarterId,

        //    };
        //    return objQuarterMaster;
        //}

        #endregion

        #region RoundInspection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public ActionResult FloorRoundInspection(int? floorId)
        {
            if (!floorId.HasValue)
                return RedirectToRoute("survey");

            UISession.AddCurrentPage("rounds_FloorRoundInspection", 0, "Floor Inspection");
            var floors = _floorService.GetFloor(floorId.Value);
            var lists = _workOrderService.GetFloorWorkOrder(Convert.ToInt32(floorId.Value)).Where(x => x.StatusCode == BDO.Enums.StatusCode.ACTIV.ToString() || x.StatusCode == BDO.Enums.StatusCode.PENDG.ToString() || x.CRxCode == BDO.Enums.StatusCode.ACTIV.ToString() || x.CRxCode == BDO.Enums.StatusCode.PENDG.ToString());
            ViewBag.Floors = floors;
            return View(lists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FloorRoundInspection(WorkOrder objWorkOrder, string userIds, string isIlsmRequired, bool IsCloseRoundDeficiency = false)
        {
            var ilsmRequired = Convert.ToBoolean(isIlsmRequired);
            var resources = new List<UserProfile>();
            if (userIds != null)
            {
                var ids = userIds.Split(',');
                for (var i = 0; i < ids.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(ids[i]))
                    {
                        var user = _userService.GetUserProfile(Convert.ToInt32(ids[i]));
                        resources.Add(user);
                    }
                }
            }
            if (objWorkOrder.IssueId == 0)
                objWorkOrder.StatusCode = "ACTIV";

            objWorkOrder.Resources = resources;
            objWorkOrder.DeficiencyCode = WODeficiencyCode.FI.ToString();
            objWorkOrder.CreatedBy = UserSession.CurrentUser.UserId;
            if (objWorkOrder.IsDeficiency)
            {
                objWorkOrder.DeficiencyCode = WODeficiencyCode.FD.ToString();
                objWorkOrder.IsIlsm = ilsmRequired;
                TempData.Put("WO", objWorkOrder);
                //ViewBag.WO = objWorkOrder;
                return RedirectToAction(nameof(WorkOrderController.CreateWO), "WorkOrder");
            }
            var postData = _commonModelFactory.JsonSerialize<WorkOrder>(objWorkOrder);
            var statusCode = Convert.ToInt32(HttpReponseStatusCode.Success);
            var uri = APIUrls.Rounds_FloorRoundInspection;
            var results = _httpService.CallPostMethod(postData, uri, ref statusCode);

            if (statusCode == Convert.ToInt32(HttpReponseStatusCode.Success) && ilsmRequired)
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
                var newWo = httpResponse.Result.WorkOrder;
                if (newWo != null)
                    return RedirectToAction(nameof(ILSMController.AddILSM), "ILSM", new { issueId = newWo.IssueId });
            }
            if (IsCloseRoundDeficiency)
            {
                return RedirectToAction(nameof(HomeController.Deficiencies), "Home");
            }
            return RedirectToAction(nameof(RoundsController.FloorRoundInspection), "Rounds", new { floorId = objWorkOrder.FloorId });
        }

        #region Round Defeciency
        public ActionResult CloseRoundDeficiency(string IssueId)
        {
            UISession.AddCurrentPage("rounds_CloseRoundDeficiency", 0, "Close Round Deficiency");
            var objWorkOrder = _workOrderService.GetWorkOrder(Convert.ToInt32(IssueId));
            objWorkOrder.DeficiencyCode = WODeficiencyCode.FI.ToString();
            return View(objWorkOrder);
        }

        #endregion

        #endregion

        #region Fire Drill Common Questionnaires

        [CrxAuthorize(Roles = "Rounds_FireDrillQuestionnaries")]
        public ActionResult FireDrillQuestionnaries(int? firedrillCatId)
        {
            UISession.AddCurrentPage("rounds_FireDrillQuestionnaries", 0, "Fire Drill Questionnaires");
            var lists = _exercisesService.GetFiredrillQuestionnaries();
            ViewBag.FireDrillCategory = lists;
            ViewBag.fireCatId = firedrillCatId;
            if (firedrillCatId != null || firedrillCatId > 0)
            {
                _session.Set("CmnfiredrillCatId", firedrillCatId);
                // Session["CmnfiredrillCatId"] = firedrillCatId;
            }
            _session.Set("firedrillcatid", firedrillCatId);
            // Session["firedrillcatid"] = firedrillCatId;
            return View("~/Views/Rounds/FireDrillQuestionnaries.cshtml", lists);
        }


        public ActionResult ManageQuestionnaires(int? fireDrillquesid)
        {
            UISession.AddCurrentPage("rounds_ManageQuestionnaires", 0, "Manage Questionnaires");

            var cmnfiredrillCatId = _session.Get<int>("CmnfiredrillCatId");

            //if (Session["CmnfiredrillCatId"].ToString() != "")
            if (cmnfiredrillCatId != null)
                ViewBag.CmnfireDrillCatId = cmnfiredrillCatId; //(int)Session["CmnfiredrillCatId"];


            var newData = new FireDrillQuestionnaires();
            var lists = _exercisesService.GetFiredrillQuestionnaries();
            ViewBag.FireDrillCategory = lists.Where(x => x.IsActive && x.IsCommonCat).ToList();
            if (fireDrillquesid > 0)
            {
                newData = _exercisesService.GetFiredrillQuestionnarie(fireDrillquesid.Value);
            }
            else { ViewBag.FireDrillCategory = lists.Where(x => x.IsActive).ToList(); }
            //Session["CmnfiredrillCatId"] = "";
            _session.Set("CmnfiredrillCatId", string.Empty);
            return View(newData);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageQuestionnaires(FireDrillQuestionnaires newData)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            newData.CreatedBy = UserSession.CurrentUser.UserId;
            newData.IsCommQues = true;

            if (newData.FireDrillQuesId > 0)
            {
                var Exists = _exercisesService.GetFiredrillQuestionnarie(newData.FireDrillQuesId);
                if (!string.IsNullOrEmpty(Exists.Applicable))
                    clientIds = Exists.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (newData.IsActive)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                newData.Applicable = string.Join(",", clientIds.Distinct());
            }
            else
            {
                newData.Applicable = _session.ClientNo; //Utility.HcfSession.ClientNo.ToString();
            }
            var postData = _commonModelFactory.JsonSerialize<FireDrillQuestionnaires>(newData);
            var uri = Utility.APIUrls.Rounds_ManageQuestionnaires;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpResponse.Success)
                {
                    SuccessMessage = httpResponse.Message;
                    return RedirectToAction("FireDrillQuestionnaries");
                }
                else
                {
                    ErrorMessage = httpResponse.Message;
                    return View("ManageQuestionnaires", newData);
                }
            }
            else
            {
                ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
                return View("ManageQuestionnaires", newData);
            }
        }


        public ActionResult FireDrillActive(int id, bool IsActive)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            if (id > 0)
            {
                var newData = _exercisesService.GetFiredrillQuestionnarie(id);
                newData.IsCommQues = true;
                newData.IsActive = IsActive;
                if (!string.IsNullOrEmpty(newData.Applicable))
                    clientIds = newData.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (IsActive)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                newData.Applicable = string.Join(",", clientIds.Distinct());
                var postData = _commonModelFactory.JsonSerialize<FireDrillQuestionnaires>(newData);
                var uri = Utility.APIUrls.Rounds_ManageQuestionnaires;
                var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    if (httpResponse.Success)
                    {
                        SuccessMessage = httpResponse.Message;
                    }
                }
            }
            // return Json(new { result = SuccessMessage }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("FireDrillQuestionnaries");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetTExerciseQuestionnaires(TExercises tExercise, int quarterNo, int year, int buildingTypeId, int isEditable)
        {
            //tExercise.Date = Conversion.ConvertToDateTime(tExercise.DateTimeSpan);
            var shift = _shiftService.GetShift().FirstOrDefault(x => x.ShiftId == tExercise.ShiftId);
            if (shift != null)
            {
                ViewBag.StartTime = shift.StartTime;
                ViewBag.Endtime = shift.EndTime;
            }

            ViewBag.IsEditable = isEditable;
            var objQuarter = new QuarterMaster
            {
                //QuarterId = tExercise.QuarterId,
                QuarterNo = quarterNo,
                Year = year,
                BuildingTypeId = buildingTypeId,
                Buildings = new List<Buildings>()
            };
            var objBuilding = new Buildings
            {
                BuildingId = tExercise.BuildingId.HasValue ? tExercise.BuildingId.Value : 0,
                LocationGroupId = tExercise.LocationGroupId.HasValue ? tExercise.LocationGroupId.Value : 0,
            };
            //objBuilding = _buildingsService.GetBuildings().FirstOrDefault(x => x.BuildingId == objBuilding.BuildingId);
            if (objBuilding != null)
            {
                objBuilding.Shifts = new List<Shift>();
                var objShift = new Shift
                {
                    ShiftId = tExercise.ShiftId,
                    Exercises = new List<TExercises>()
                };
                if (tExercise.TExerciseId > 0)
                {
                    objShift.Exercises = _exercisesService.GetExercises(tExercise.TExerciseId).ToList();
                    tExercise.DateTimeSpan = objShift.Exercises.FirstOrDefault(x => x.TExerciseId == tExercise.TExerciseId).DateTimeSpan;
                    foreach (var obj in objShift.Exercises)
                    {
                        var users = _userService.GetUserList(obj.CreatedBy);//.FirstOrDefault(x => x.UserId == obj.CreatedBy);
                        obj.ConductedBy = (!string.IsNullOrEmpty(obj.ConductedBy) ? obj.ConductedBy : (users != null ? users.Name : string.Empty));
                        obj.StartTime = tExercise.StartTime;
                        obj.Date = Convert.ToDateTime(tExercise.Date);
                        obj.Announced = Convert.ToBoolean(tExercise.Announced);
                        obj.IsAudible = Convert.ToBoolean(tExercise.IsAudible);
                        obj.Status = tExercise.Status;
                        DateTime firstDayOfNextQuarter = _exercisesService.getFirstdayofNextQuarter(obj.Date.Value, obj.QuarterNo);
                        var drillcompleted = obj.TExerciseFiles.Where(x => x.DocumentType == 110).ToList();
                        obj.FiredrillDocStatus = drillcompleted.Count > 0 ? Convert.ToInt32(FiredrillDocStatus.DocumentationCompleted) : firstDayOfNextQuarter > obj.Date.Value ? Convert.ToInt32(FiredrillDocStatus.DocumentationPastdue) : obj.TExerciseQuestionnaires.Any(x => x.TExerciseQuestId > 0) ? Convert.ToInt32(FiredrillDocStatus.DocumentationInprogress) : Convert.ToInt32(FiredrillDocStatus.DocumentationPending);
                        if (obj.DigitalSignature.Count == 0)
                        {
                            TExerciseDigitalSignature(obj);
                        }
                        obj.TExerciseActions = obj.TExerciseActions.Count > 0 ? obj.TExerciseActions : CreateExerciseActions();
                        if (!obj.TExerciseFiles.Any())
                            AddTexerciseFiles(obj);
                    }
                }
                else
                {
                    var obj = new TExercises
                    {

                        TExerciseId = tExercise.TExerciseId,
                        BuildingId = tExercise.BuildingId,
                        LocationGroupId = tExercise.LocationGroupId,
                        StartTime = tExercise.StartTime,
                        ShiftId = tExercise.ShiftId,
                        Date = Convert.ToDateTime(tExercise.Date),
                        Announced = true,
                        Status = tExercise.Status,
                        DrillType = tExercise.DrillType,
                        Comment = tExercise.Comment,
                        NearBy = tExercise.NearBy,
                        ConductedBy = UserSession.CurrentUser.Name,
                        Name = tExercise.Name,
                        QuarterNo = quarterNo,
                        CritiqueDate = Convert.ToDateTime(tExercise.Date),
                        EducationDate = Convert.ToDateTime(tExercise.Date),
                        IsAdditional = tExercise.IsAdditional,
                        IsAudible = true,
                        FiredrillDocStatus = Convert.ToInt32(FiredrillDocStatus.DocumentationPending),
                        TExerciseQuestionnaires = new List<TExerciseQuestionnaires>(),
                        TExerciseFiles = new List<TExerciseFiles>()
                    };
                    obj.TExerciseQuestionnaires = _exercisesService.GetTExerciseQuestionnaires(tExercise.TExerciseId).OrderBy(x => x.FireDrillQuestionnaires.FireDrillCatId).ToList();
                    obj.TExerciseActions = obj.TExerciseActions.Count > 0 ? obj.TExerciseActions : CreateExerciseActions();
                    AddTexerciseFiles(obj);
                    AddDigitalSignature(obj);
                    AddCritiqueDigitalSignature(obj);
                    AddEducationDigitalSignature(obj);
                    objShift.Exercises.Add(obj);
                }
                objBuilding.Shifts.Add(objShift);
                objQuarter.Buildings.Add(objBuilding);
            }
            return PartialView("_tExerciseQuestionnaires", objQuarter);
        }

        public static void TExerciseDigitalSignature(TExercises obj)
        {
            DigitalSignature newDigitalSignature = new DigitalSignature();
            obj.DigitalSignature = new List<DigitalSignature>();
            obj.DigitalSignature.Add(newDigitalSignature);
            obj.CritiqueDigitalSignature = new List<DigitalSignature>();
            obj.CritiqueDigitalSignature.Add(newDigitalSignature);
            obj.EducationDigitalSignature = new List<DigitalSignature>();
            obj.EducationDigitalSignature.Add(newDigitalSignature);
        }

        private static void AddDigitalSignature(TExercises obj)
        {
            DigitalSignature newDigitalSignature = new DigitalSignature();
            obj.DigitalSignature = new List<DigitalSignature>();
            obj.DigitalSignature.Add(newDigitalSignature);

        }
        private void AddCritiqueDigitalSignature(TExercises obj)
        {
            DigitalSignature newDigitalSignature = new DigitalSignature();
            obj.CritiqueDigitalSignature = new List<DigitalSignature>();
            obj.CritiqueDigitalSignature.Add(newDigitalSignature);
        }

        private void AddEducationDigitalSignature(TExercises obj)
        {
            DigitalSignature newDigitalSignature = new DigitalSignature();
            obj.EducationDigitalSignature = new List<DigitalSignature>();
            obj.EducationDigitalSignature.Add(newDigitalSignature);
        }

        private static void AddTexerciseFiles(TExercises obj)
        {
            obj.TExerciseFiles = new List<TExerciseFiles>();
            var newTExerciseFiles = new TExerciseFiles();
            obj.TExerciseFiles.Add(newTExerciseFiles);
        }

        private static List<TExerciseActions> CreateExerciseActions()
        {
            List<TExerciseActions> list = new List<TExerciseActions>();
            TExerciseActions obj = new TExerciseActions
            {
                TExerciseActionId = 0,
                Issue = "",
                Action = "",
                IsDeleted = false
            };
            list.Add(obj);
            return list;
        }

        public ActionResult FiredrillCommonCategories()
        {
            UISession.AddCurrentPage("Firedrills_FiredrillCommonCategories", 0, "FireDrill Category");
            var firedrilcateg = _exercisesService.GetCommonFireDrillCategory().ToList();
            return View(firedrilcateg);
        }

        public ActionResult AddFireDrilCommonCategory(int? Id)
        {
            FireDrillCategory firedrillcat = new FireDrillCategory();
            if (Id.HasValue)
            {
                firedrillcat = _exercisesService.GetCommonFireDrillCategory().Where(x => x.FiredrillCatId == Id).FirstOrDefault();
            }
            return PartialView("~/Views/Rounds/_AddFireDrillCommonCategory.cshtml", firedrillcat);
        }

        [HttpPost]
        public ActionResult AddFireDrilCommonCategory(FireDrillCategory firedrillCat)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            firedrillCat.CreatedBy = UserSession.CurrentUser.UserId;
            firedrillCat.IsCommonCat = true;
            if (firedrillCat.FiredrillCatId > 0)
            {
                var firedrillCateg = _exercisesService.GetCommonFireDrillCategory().Where(x => x.FiredrillCatId == firedrillCat.FiredrillCatId).FirstOrDefault();
                if (!string.IsNullOrEmpty(firedrillCateg.Applicable))
                    clientIds = firedrillCateg.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (firedrillCat.IsActive)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                firedrillCat.Applicable = string.Join(",", clientIds.Distinct());
            }
            else
            {
                firedrillCat.Applicable = _session.ClientNo.ToString();
            }
            _exercisesService.AddFireDrilCommonCategory(firedrillCat);
            return RedirectToAction("FiredrillCommonCategories");
        }

        #endregion

        #region

        [CrxAuthorize(Roles = "rounds_survey")]
        public ActionResult Survey()
        {
            UISession.AddCurrentPage("Rounds_Survey", 0, "Adhoc Survey");
            var buildings = _buildingsService.GetBuildingFloors();
            ViewBag.Title = "Adhoc Survey";
            return View("Rounds", buildings);
        }


        public ActionResult SurveyHistory(int? floorId, int? buildingId)
        {
            UISession.AddCurrentPage("Rounds_Survey", 0, "Adhoc Survey History");
            ViewBag.buildingId = buildingId.HasValue && buildingId > 0 ? buildingId.Value : 0;
            ViewBag.floorId = floorId.HasValue && floorId > 0 ? floorId.Value : 0;
            return View();
        }

        [HttpGet]
        public ActionResult FloorSurveyHistory(int? buildingId, int? floorId)
        {
            var lists = _workOrderService.GetRoundWorkOrders(buildingId, floorId);//.Where(x => x.FloorId == floorId).ToList();
            return View("_floorSurveyHistory", lists);
        }



        #endregion

        #region PM Logs
        public ActionResult DailyLogs(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null && fromDate == null)
            {
                toDate = DateTime.Today;
                fromDate = DateTime.Today.AddMonths(-1);
            }
            ViewBag.FromDate = $"{fromDate:MMM d, yyyy}";
            ViewBag.ToDate = $"{toDate:MMM d, yyyy}"; ;
            UISession.AddCurrentPage("rounds_dailylogs", 0, "PM Logs");
            var pmLogs = _roundService.PMDailyLogs(fromDate, toDate);
            return View(pmLogs);
        }

        public ActionResult PmDailyLogs(int? pmlogId)
        {
            UISession.AddCurrentPage("rounds_pmdailylogs", 0, "Add PM Logs");
            var pmLogs = _roundService.PMDailyLog(pmlogId);
            if (pmLogs.PMLogId == 0)
            {
                pmLogs.Date = DateTime.Now.ToClientTime();
                pmLogs.POTime = _commonModelFactory.ConvertToAMPM(DateTime.UtcNow.ToClientTime());
                pmLogs.Name = UserSession.CurrentUser.FullName;
            }

            return View(pmLogs);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PmDailyLogs(TPMLogs logs)
        {
            if (logs.PMLogId > 0)
                logs.ParentLogId = logs.PMLogId;

            if (!string.IsNullOrEmpty(logs.POTime))
            {
                var dateTime = DateTime.ParseExact(logs.POTime, "hh:mm tt", CultureInfo.InvariantCulture);
                logs.Time = dateTime.TimeOfDay;
            }
            logs.CreatedBy = UserSession.CurrentUser.UserId;
            var pmlogId = _roundService.SavePMDailyLog(logs);
            logs.PMLogId = pmlogId;
            if (pmlogId > 0)
            {
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                return RedirectToAction("DailyLogs");
            }

            return View(logs);
        }

        #endregion

        #region Fire drill ques 
        [CrxAuthorize(Roles = "Rounds_FireDrillQuestionnaries")]
        public ActionResult FireDrillQues(int? firedrillCatId, int? isCommon)
        {
            UISession.AddCurrentPage("rounds_FireDrillQuestionnaries", 0, "Fire Drill Questionnaries");
            var firedrillQues = _exercisesService.GetFiredrillQues();
            ViewBag.fireCatId = firedrillCatId;
            ViewBag.isCommon = isCommon;
            _session.Set<int?>("firedrillcatid", firedrillCatId);
            //Session["firedrillcatid"] = firedrillCatId;
            return View("~/Views/Rounds/FireDrillQues.cshtml", firedrillQues);
        }

        public ActionResult AddFiredrillQuestionnaires(int? id)
        {
            var firedrillcatid = _session.Get<int?>("firedrillcatid");
            if (id == null && firedrillcatid.HasValue)
            {
                ViewBag.fireDrillCatId = firedrillcatid;// (int)Session["firedrillcatid"];
            }
            UISession.AddCurrentPage("rounds_ManageQuestionnaires", 0, "Manage Questionnaires");
            var newData = new FireDrillQuestionnaires();
            var lists = _exercisesService.GetFiredrillQues().Where(x => !x.IsCommonCat).ToList();
            ViewBag.FireDrillCategory = lists;
            if (id.HasValue)
            {
                newData = _exercisesService.GetFiredrillQues(HCF.Utility.Conversion.TryCastInt32(id));
            }
            else { ViewBag.FireDrillCategory = lists.Where(x => x.IsActive).ToList(); }
            return View(newData);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFiredrillQuestionnaires(FireDrillQuestionnaires newData)
        {
            newData.CreatedBy = UserSession.CurrentUser.UserId;
            var postData = _commonModelFactory.JsonSerialize<FireDrillQuestionnaires>(newData);
            var uri = Utility.APIUrls.Rounds_ManageQuestionnaires;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpResponse.Success)
                {
                    SuccessMessage = httpResponse.Message;

                }
                else
                {
                    ErrorMessage = httpResponse.Message;
                }
            }
            else
            {
                ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
            }
            return RedirectToAction("FireDrillQues");
        }

        #endregion

        #region FireDrill Categ
        public ActionResult FiredrillCategories()
        {
            UISession.AddCurrentPage("Firedrills_FiredrillCategories", 0, "FireDrill Category");
            var firedrilcateg = _exercisesService.GetFireDrillCategory().ToList();
            return View(firedrilcateg);
        }

        public ActionResult AddFireDrilCategory(int? Id)
        {
            FireDrillCategory firedrillcat = new FireDrillCategory();
            ViewBag.actionstatus = "true";
            if (Id.HasValue)
            {
                firedrillcat = _exercisesService.GetFireDrillCategory().Where(x => x.FiredrillCatId == Id).FirstOrDefault();
            }
            return PartialView("~/Views/Rounds/_AddFireDrillCategory.cshtml", firedrillcat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFireDrilCategory(FireDrillCategory firedrillCat)
        {
            firedrillCat.CreatedBy = UserSession.CurrentUser.UserId;
            firedrillCat.IsCommonCat = true;
            _exercisesService.AddFireDrilCategory(firedrillCat);
            return RedirectToAction("FiredrillCategories");
        }
        public ActionResult FireDrillCategoriesActiveStatus(int CatId, bool status)
        {
            var clientNo = Convert.ToInt32(_session.ClientNo);
            List<int> clientIds = new List<int>();
            if (CatId > 0)
            {
                var firedrillCateg = _exercisesService.GetCommonFireDrillCategory().Where(x => x.FiredrillCatId == CatId).FirstOrDefault();
                firedrillCateg.IsCommonCat = true;
                firedrillCateg.IsActive = status;
                if (!string.IsNullOrEmpty(firedrillCateg.Applicable))
                    clientIds = firedrillCateg.Applicable.Split(',').Select(x => int.Parse(x)).ToList();
                if (status)
                    clientIds.Add(clientNo);
                else
                    clientIds.Remove(clientNo);

                firedrillCateg.Applicable = string.Join(",", clientIds.Distinct());
                _exercisesService.AddFireDrilCommonCategory(firedrillCateg);
            }
            return RedirectToAction("FiredrillCategories");
        }
        #endregion

    }
}
