using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.Utility.Extensions;

        
namespace HCF.Web.Controllers
{
    public class FireExtinguisherController : BaseController
    {
        private readonly IManufactureService _manufactureService;
        private readonly IInsActivityService _inspectionActivityService;
        private readonly IAssetsService _assetsService;
        private readonly ILocationService _locationService;
        private readonly IFloorService _floorService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IAssetTypeService _assetTypeService;
        private readonly IFireExtinguisherService _fireExtinguisherService;
        private readonly IBuildingsService _buildingsService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ITransactionService _transactionService;       
        private readonly IHttpPostFactory _httpService;
        public FireExtinguisherController(IBuildingsService buildingsService, IManufactureService manufactureService, IInsActivityService inspectionActivityService,
            IAssetsService assetsService, ILocationService locationService, IFloorService floorService, IMainGoalService mainGoalService,
            IAssetTypeService assetTypeService, IFireExtinguisherService fireExtinguisherService, IFloorAssetService floorAssetService, IHttpPostFactory httpService,
            ICommonModelFactory commonModelFactory,
        ITransactionService transactionService
            ) 
        {
            _commonModelFactory = commonModelFactory;           
            _buildingsService = buildingsService;
            _assetTypeService = assetTypeService;
            _mainGoalService = mainGoalService;
            _floorService = floorService;
            _locationService = locationService;
            _assetsService = assetsService;
            _manufactureService = manufactureService;
            _inspectionActivityService = inspectionActivityService;
            _fireExtinguisherService = fireExtinguisherService;
            _floorAssetService = floorAssetService;
            _transactionService = transactionService;
            _httpService = httpService;

        }
        public ActionResult Index(int floorId, int? inspType, int assetId, string assetids, int? routeId, int? epdetailId = null)
        {
            //int userId = UserSession.CurrentUser.UserId;
            UISession.AddCurrentPage("FireExtinguisher_Index", 0, "Route Based Inspection");
            ViewBag.inspType = inspType;
            ViewBag.AssetId = assetId;
            ViewBag.routeId = routeId;
            ViewBag.EPDetailId = epdetailId;
            ViewBag.FloorAssetId = TempData["FloorAssetId"];
            ViewBag.LocationId = TempData["LocationId"];
            ViewBag.Mode = TempData["Mode"];
            ViewBag.Floor = _floorService.GetFloor(floorId);
            ViewBag.Asset = _assetsService.Get(assetId);
            if(assetids ==null || assetids == "")
            {
                var assetEPs = _assetsService.GetAssetEp(assetId);
                ViewBag.AssetEPs = assetEPs;
            }
            else
            {
                var assetEPs = _assetsService.GetBulkAssetEp(assetids);
                ViewBag.AssetEPs = assetEPs;
            }
                      
            
           
            return View();
        }

        [AjaxOnly]
        public ActionResult GetExtinguisherAssets(int floorId, int inspType, int? routeId, int assetId, int? floorAssetId = null, int? locationId = null)
        {
            ViewBag.FloorAssetId = floorAssetId ?? 0;
            ViewBag.LocationId = locationId ?? 0;
            var stops = _fireExtinguisherService.GetExtinguisherAssets(floorId, inspType, routeId ?? 0, assetId);
            return PartialView("_GetExtinguisherAssets", stops);
        }

        public ActionResult GetExtinguisherAssetsWithOutFloor(int assetId)
        {
            var floorAssets = _fireExtinguisherService.GetExtinguisherAssetsWithOutFloor(assetId);
            return PartialView("_GetExtinguisherAssetsWithOutfloor", floorAssets);
        }

        public ActionResult SerachIndex(int floorId, int? inspType, int assetId, int? routeId, int? floorAssetId = null, int? locationId = null)
        {
            TempData["FloorAssetId"] = floorAssetId ?? 0;
            TempData["LocationId"] = locationId ?? 0;
            TempData["Mode"] = 1;
            return RedirectToAction("Index", "FireExtinguisher", new { floorId, inspType, assetId, routeId });
        }


        public ActionResult ExtinguisherInspetion(string mode, int floorAssetId, int inspType, int locationid, int isloaddata, int assetId, int epId)
        {
            var floorAsset = new TFloorAssets();
            ViewBag.AssetId = assetId;
            ViewBag.inspType = inspType;
            ViewBag.locationId = locationid;
            ViewBag.isloaddata = isloaddata;
            ViewBag.Mode = mode;
            ViewBag.RBIEPid = epId;
            var currentLocation = _locationService.GetLocationsMaster(locationid).FirstOrDefault(); //new LocationMaster();
            if (currentLocation != null)
            {
                ViewBag.LocationName = currentLocation.StopName;
                ViewBag.StopInfo = $"{currentLocation.StopName} ({currentLocation.StopCode})";
            }

            if (floorAssetId > 0)
                floorAsset = _floorAssetService.GetFloorAsset(floorAssetId);

            if (mode == "addassets")
                return PartialView("_ExtinguisherInspetionPopUp", floorAsset);
            else
            {
                if (floorAsset.AssetId != null && epId == 0)
                    floorAsset.Assets.MainGoals = _mainGoalService.GetAssetMainGoals(floorAsset.AssetId.Value)
                        .Where(x => x.FrequencyId == null || x.FrequencyId == inspType).ToList();
                else
                {
                    if (floorAsset != null && floorAsset.Assets != null)
                        floorAsset.Assets.MainGoals = _mainGoalService.GetAssetMainGoals(floorAsset.AssetId.Value, epId).ToList();
                }

                floorAsset.InspectionFiles = CreateInspectionFiles();
                ViewBag.InspResult = _fireExtinguisherService.GetInspResult();
                ViewBag.InspStatus = _fireExtinguisherService.GetInspStatus();
                return PartialView("_RouteInspPoints", floorAsset);
            }
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

        public JsonResult SaveExtinguisherInsp(int orgFloorAssetsId, int floorAssetId, string status, int result, string serialNo, string comment, int inspType, int locationId, bool isMarkLocationEmpty, string assetId)
        {
            TFloorAssets tfloorAssets;
            if (!string.IsNullOrEmpty(serialNo))
            {
                var saveActivity = new TInspectionActivity
                {
                    Status = result,
                    SubStatus = status,
                    FloorAssetId = floorAssetId,
                    FrequencyId = inspType,
                    ActivityId = Guid.NewGuid(),
                    ActivityType = 2,
                    Comment = comment,
                    CreatedBy = UserSession.CurrentUser.UserId
                };
                tfloorAssets = _fireExtinguisherService.GetExtinguisherAsset(serialNo, null, assetId);
                if (tfloorAssets == null)
                {
                    return Json(new { msg = "no record found for this tag #", status = -1, isClosedPopUp = 0 });
                }
                else if (orgFloorAssetsId != floorAssetId)
                {
                    var res = _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(orgFloorAssetsId);
                    if (res)
                    {
                        var objtfloorAssets = new TFloorAssets
                        {
                            FloorAssetsId = floorAssetId,
                            StopId = locationId,
                            CreatedBy = UserSession.CurrentUser.UserId
                        };
                        _fireExtinguisherService.AddFloorAssetsToLocation(objtfloorAssets);
                    }
                    saveActivity.TInsActivityId = SaveInspection(saveActivity, isMarkLocationEmpty);
                }
                else
                    saveActivity.TInsActivityId = SaveInspection(saveActivity, isMarkLocationEmpty);

                // save inspection and  
                if (saveActivity.TInsActivityId > 0 && isMarkLocationEmpty)
                    return Json(new { msg = "Inspection saved successfully and asset move to inventory.", isClosedPopUp = 1 });
                else if (saveActivity.TInsActivityId > 0)
                    return Json(new { msg = "Inspection saved successfully.", isClosedPopUp = 1 });
                else
                    return Json(new { msg = "Error occurred during save inspection.", isClosedPopUp = 0 });
            }
            return Json(new { msg = "please enter tag #", isClosedPopUp = 0 });
        }

        public ActionResult GetTfloorAssetsByTagNo(string tagNo, string stopcode, string assetId)
        {
            var success = true;
            var message = string.Empty;
            var tfloorAssets = _fireExtinguisherService.GetExtinguisherAsset(tagNo, stopcode, assetId);
            if (tfloorAssets != null)
            {
                return Json(new { Result = tfloorAssets, success, msg = message });
            }
            else
            {
                message = ConstMessage.Tag_you_entered_does_not_exist;
                success = false;
            }
            return Json(new { Result = tfloorAssets, success, msg = message });
        }

        public ActionResult GetAssetsByTagNo(string tagNo, string stopcode, string assetId, string mode)
        {
            bool success = true;
            string message = string.Empty;
            TFloorAssets tfloorAssets;
            if (mode.ToLower() == "asset")
            {
                tfloorAssets = _fireExtinguisherService.GetExtinguisherAsset(tagNo, stopcode, assetId);
                if (tfloorAssets != null)
                {
                    var assets = _assetsService.GetAssetFrequency().FirstOrDefault(x => x.AssetId == tfloorAssets.AssetId);
                    if (assets != null && tfloorAssets.Assets != null)
                    {
                        tfloorAssets.Assets = assets;
                    }
                    return PartialView("_assetsView", tfloorAssets);
                }
                else
                {
                    message = ConstMessage.Tag_you_entered_does_not_exist;
                    success = false;
                }
            }
            else if (mode.ToLower() == "stop")
            {
                var stop = _locationService.GetStopByCode(stopcode);
                if (stop != null)
                {
                    stop.TfloorAssets = _locationService.GetStopAssets(stop.StopId).Where(x => x.AssetId == Convert.ToInt32(assetId)).ToList();
                    //if (stop != null && stop.TfloorAssets != null)
                    //{
                    //   var assetFrequency= _assetsService.GetAssetFrequency().Where(x=>x.AssetId== stop.TfloorAssets.ass);
                    //    foreach (var floorAssets in stop.TfloorAssets)
                    //    {

                    //    }
                    //}

                    return PartialView("_stopView", stop);
                }
                else
                {
                    //var stopMaster = new StopMaster();
                    message = ConstMessage.StopCode_you_entered_does_not_have_any_assets;
                    success = false;
                }
            }
            return Json(new { success, msg = message });
        }

        #region Inspection & History

        public int SaveInspection(TInspectionActivity activity, bool isMarkLocationEmpty)
        {
            var tinsActivityId = _transactionService.AddTInspectionActivity(activity);
            //bool isMarkLocationEmpty = Convert.ToBoolean(IsMarkLocationEmpty);
            if (tinsActivityId > 0 && isMarkLocationEmpty)
            {
                RemoveFloorAssetsFromCurrentLocation(activity.FloorAssetId.Value);
            }
            return tinsActivityId;
        }


        public JsonResult RemoveFloorAssetsFromCurrentLocation(int orgFloorAssetsId)
        {
            var res = _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(orgFloorAssetsId);
            return Json(new { msg = "Asset removed from stop successfully and moved to inventory.", isClosedPopUp = 1 });
        }


        public JsonResult AddAssetTlocation(int floorAssetId, int locationId, int orgFloorAssetsId, int assetId)
        {
            if (orgFloorAssetsId > 0)
            {
                _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(Convert.ToInt32(orgFloorAssetsId));
            }

            var objtfloorAssets = new TFloorAssets
            {
                FloorAssetsId = Convert.ToInt32(floorAssetId),
                StopId = Convert.ToInt32(locationId),
                CreatedBy = UserSession.CurrentUser.UserId,
                AssetId = assetId
            };
            _fireExtinguisherService.AddFloorAssetsToLocation(objtfloorAssets);
            return Json(new { msg = "Assets replace successfully.", isClosedPopUp = 1 });
        }

        public ActionResult ActivityHistory(int floorAssetId)
        {
            UISession.AddCurrentPage("FireExtinguisher_ActivityHistory", 0, "Activity History");
            var userId = UserSession.CurrentUser.UserId;
            var floorAsset = _floorAssetService.GetFeFloorAssetInspActivity(floorAssetId, userId);
            if (floorAsset.AssetId != null)
            {
                var inspectionType = _fireExtinguisherService.GetAssetInspFrequency(floorAsset.AssetId.Value);
                ViewBag.InspectionType = inspectionType;
            }

            //  TempData["floorAssetsActivity"] = floorAsset;
            return View(floorAsset);
        }

        public ActionResult ActivityHistoryDetails(Guid? activityId)
        {
            if (!activityId.HasValue)
                return RedirectToRoute("routeInspection");
            var activity = _inspectionActivityService.GetRouteInspectionActivity(activityId.Value);
            return View(activity);
        }


        public ActionResult FilterActivityHistory(int floorAssetId, int insptype)
        {
            var userId = UserSession.CurrentUser.UserId;
            var floorAsset = TempData["floorAssetsActivity"] == null
                ? _floorAssetService.GetFeFloorAssetInspActivity(floorAssetId, userId)
                : (TFloorAssets)TempData["floorAssetsActivity"];
            if (insptype > 0)
            { floorAsset.TInspectionActivity = floorAsset.TInspectionActivity.Where(x => x.FrequencyId == insptype).ToList(); }
            return PartialView("_activityFE", floorAsset);
        }

        #endregion

        #region Manage Assets

        public ActionResult AddNewAssets(int? floorAssetsId, int? locationId = 0, int? assetId = 0, string serialNo = "")
        {
            var userId = UserSession.CurrentUser.UserId;
            var currentStop = new StopMaster();
            var assetsTypes = _assetTypeService.GetInternalAssetsByType(userId);
            ViewBag.AssetsTypes = new SelectList(assetsTypes, "TypeId", "Name", "");
            ViewBag.Manufactures = _manufactureService.GetAll().Where(x => x.IsActive).ToList();
            var floorAssets = new TFloorAssets { SerialNo = serialNo };
            if (locationId > 0)
            {
                currentStop = _locationService.GetLocationsMaster(locationId).FirstOrDefault();
            }

            ViewBag.CurrentStop = currentStop;
            ViewBag.AssetId = assetId;

            return PartialView("_AddNewAssets", floorAssets);
        }

        [HttpPost]
        public JsonResult SaveAssets(TFloorAssets tfloorAssets)
        {
            //var tfloorAssets = _tfloorAssets;
            tfloorAssets.CreatedBy = UserSession.CurrentUser.UserId;
            tfloorAssets.Source = (int)BDO.Enums.Source.Internal;
            tfloorAssets.DateCreated = DateTime.Now;
            var postData = _commonModelFactory.JsonSerialize<TFloorAssets>(tfloorAssets);
            string uri;
            uri = tfloorAssets.FloorAssetsId > 0 ? APIUrls.Assets_tfloorAssetEdit_Edit : APIUrls.Assets_tfloorAssetEdit_Add;

            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            return Json(statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success) ? new { Result = result } : new { Result = "" });
        }

        public ActionResult Asset(string mode)
        {
            ViewBag.PageMode = mode;
            //int userId = UserSession.CurrentUser.UserId;
            var types = _assetTypeService.GetInternalAssetsByType(UserSession.CurrentUser.UserId);
            return PartialView("_InternalAssets", types);
        }

        public JsonResult FilterFloorAssets(string assetId, string ascIds, int insptype)
        {
            var floorAssetStatus = _floorService.GetFloorByAssetsWithStatus(assetId, ascIds, insptype);
            return Json(new { Result = floorAssetStatus });
        }



        #endregion

        #region Reports

        public ActionResult Reports()
        {
            UISession.AddCurrentPage("FireExtinguisher_Reports", 0, "Fire Extinguisher Reports");
            var userId = UserSession.CurrentUser.UserId;
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = new SelectList(buildings, "BuildingId", "BuildingName", "");
            var assetsTypes = _assetTypeService.GetInternalAssetsByType(userId);
            ViewBag.AssetsTypes = new SelectList(assetsTypes, "TypeId", "Name", "");
            return View();
        }

        public ActionResult GetReports(int assetId, int? buildingId = null, int? floorId = null, int? inspType = 1)
        {
            List<TFloorAssets> tfloorAsset = _fireExtinguisherService.Get_ExtinguisherAssetsReports(assetId, buildingId, floorId, inspType);
            return PartialView("_getReports", tfloorAsset);
        }

        [CrxAuthorize(Roles = "reports_fireextinguisherreport")]
        public ActionResult FeRouteReports(int? assetId, int routeId = 0)
        {
            UISession.AddCurrentPage("FireExtinguisher_FERouteReports", 0, "Route Based Inspection Reports");
            var userId = UserSession.CurrentUser.UserId;
            var routeMaster = _locationService.GetRouteMaster(null).Where(x => x.IsActive).ToList(); //.Where(x => x.IsDefault == false).ToList();
            var assetsTypes = _assetTypeService.GetInternalAssetsByType(userId);
            ViewBag.AssetsTypes = new SelectList(assetsTypes, "TypeId", "Name", "");
            ViewBag.Routes = new SelectList(routeMaster, "RouteId", "RouteNo", "");
            ViewBag.routeId = routeId;
            ViewBag.assetId = assetId.HasValue ? assetId.Value : 0;
            return View();
        }

        public ActionResult FireExReports(int year, int? routNo, int? assetId, int? epdetailId, string reportType, string monthNo)
        {
            var tfloorAsset = new List<TFloorAssets>();
            tfloorAsset = _fireExtinguisherService.Get_ExtinguisherAssetsReport(year, routNo, assetId, epdetailId);
            var route = _locationService.GetRouteMaster(routNo).FirstOrDefault();
            ViewBag.RouteName = route != null ? route.RouteNo : "All Routes";
            ViewBag.reportType = reportType;
            ViewBag.Year = year;
            ViewBag.MonthNo = monthNo;
            //ViewBag.RBIReportsdata = tfloorAsset;
            //TempData["RBIReportsdata"] = tfloorAsset;
            TempData.Put("RBIReportsdata", tfloorAsset);
            if (!string.IsNullOrEmpty(reportType) && reportType.ToLower() == "sm" || reportType.ToLower() == "sa")
                return PartialView("_FireExReportsSingle", tfloorAsset);
            else
                return PartialView("_FireExReports", tfloorAsset);
        }

        public ActionResult GetAssetsEPs(int assetId)
        {
            var assetEps = _assetsService.GetAssetEp(assetId);
            return Json(assetEps);
        }

        #endregion

        #region Stops

        public ActionResult GetStopsbyRouteId(int routeId)
        {
            List<StopMaster> stops = _fireExtinguisherService.GetExtinguisherAssets(null, null, routeId, null);
            return PartialView("_Getstopsbyroutes", stops);
        }

        #endregion

        public ActionResult LoadRoutes(int assetId)
        {
            var routes = _locationService.GetRouteByAsset(assetId);
            return PartialView("_routes", routes);
        }


        public JsonResult GetRouteByAssetId(int assetId)
        {
            var routes = _locationService.GetRouteByAsset(assetId);
            return Json(routes);
        }

        #region RouteBasedInspction

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveRouteInspection(TFloorAssets viewModel, int epDetailId, int inspType, int status = -1, string inspStatusCode = "")
        {
            var httpresponseResult = new HttpResponseMessage();
            //var id = FloorAssetRepository.GetFloorAssetDescription(viewModel.FloorAssetsId);
            var tInspection = CovertToRoundInspection(viewModel, inspType, status, inspStatusCode, epDetailId);
            if (epDetailId == 0)
                httpresponseResult = SaveInspection(tInspection);
            else
            {
                var multipleEpDetailId = Convert.ToString(epDetailId);
                var mode = BDO.Enums.Mode.ASSET.ToString();
                bool isComplete = true;//status == 1 ? true : false;
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string postData = _commonModelFactory.JsonSerialize<Inspection>(tInspection);
                string uri = $"{APIUrls.Goal_Save}?Mode={mode}&IsComplete={isComplete}&multipleEPDetailId={multipleEpDetailId}";
                string result = _httpService.CallPostMethod(postData, uri, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    httpresponseResult = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
            }
            if (httpresponseResult.Success)
            {
                var _isInspReady = 0;
                if (httpresponseResult.Result.EPDocument != null && httpresponseResult.Result.EPDocument.Count() > 0)
                {
                    _isInspReady = httpresponseResult.Result.EPDocument.FirstOrDefault().IsInspReady.Value;
                }
                else
                {
                    _isInspReady = 0;
                }
                var result = new
                {
                    Result = true,
                    IsInspReady = _isInspReady,
                    httpresponseResult.Result.Inspection.TInspectionActivity.FirstOrDefault().TInsActivityId,
                    httpresponseResult.Result.Inspection.TInspectionActivity[0].ActivityId,
                    httpresponseResult.Result.Inspection.TInspectionActivity[0].Status,
                    httpresponseResult.Result.Inspection.InspectionId,
                    httpresponseResult.Result.Inspection.EPDetailId,
                    httpresponseResult.Result.Inspection
                };

                return Json(result);
                //return Redirect(Request.UrlReferrer.ToString());
            }
            return Json("");
            //return Redirect(Request.UrlReferrer.ToString());
        }

        private HttpResponseMessage SaveInspection(Inspection objInspection)
        {
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var httpresponse = new HttpResponseMessage();
            var postData = _commonModelFactory.JsonSerialize<Inspection>(objInspection);
            var uri = $"{APIUrls.Save_RouteInspection}";
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);

            return httpresponse;

        }

        public static Inspection CovertToRoundInspection(TFloorAssets viewModel, int inspType, int status, string inspStatusCode, int epDetailId)
        {
            var tInspection = new Inspection();
            if (epDetailId > 0)
                tInspection.EPDetailId = epDetailId;
            var activities = new List<TInspectionActivity>();
            var activityType = viewModel.Assets.MainGoals.GroupBy(x => x.ActivityType, (key, group) => group.First());
            if (activityType.Any())
            {
                foreach (var item in activityType)
                {
                    var inspectionActivity = new TInspectionActivity
                    {
                        ActivityType = item.ActivityType,
                        Status = status > 0 ? status : -1,
                        SubStatus = (status == 1) ? "NA" : "DE",
                        FloorAssetId = viewModel.FloorAssetsId,
                        InspectionId = tInspection.InspectionId,
                        CreatedBy = UserSession.CurrentUser.UserId,
                        FrequencyId = inspType,
                        InspStatusCode = (!string.IsNullOrEmpty(inspStatusCode)) ? inspStatusCode : string.Empty,
                        TInspectionDetail = new List<TInspectionDetail>()
                    };

                    // inspectionActivity.InspStatus = new InspStatus();


                    inspectionActivity.TInspectionDetail = SetJsonMainGoal(viewModel.Assets.MainGoals, tInspection, item.ActivityType);
                    if (viewModel.InspectionFiles != null)
                    {
                        inspectionActivity.TInspectionFiles = viewModel.InspectionFiles;
                        inspectionActivity.Comment = viewModel.InspectionFiles.FirstOrDefault().Comment;
                    }
                    activities.Add(inspectionActivity);

                }
            }
            tInspection.TInspectionActivity = activities;
            return tInspection;
        }


        private static List<TInspectionDetail> SetJsonMainGoal(List<MainGoal> allmainGoals, Inspection tInspection, int activityType)
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
                            DRTime = mainGoals[i].Steps[j].IsMarkDefeciencies == true ? 0 : mainGoals[i].Steps[j].DRTime,
                            InputValue = mainGoals[i].Steps[j].InputValue
                        };
                    tInspectionDetail.InspectionSteps.Add(tInspectionSteps);
                }
                details.Add(tInspectionDetail);
            }
            return details;
        }

        #endregion
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult AllAssetsEpOnRoute(List<EPDetails> Models)
        {
           return PartialView("_allAssetEpOnRoute", Models);
        }

        
    }

}