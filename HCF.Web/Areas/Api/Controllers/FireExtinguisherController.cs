using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{    
    [Route("api/fe")] 
    [ApiController]
    public class FireExtinguisherApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly ILocationService _locationService;
        private readonly IInsActivityService _insActivityService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IFireExtinguisherService _fireExtinguisherService;
        private readonly ITransactionService _transactionService;
        private readonly IFloorAssetService _floorAssetService;

        private readonly IFloorService _floorService;
        public FireExtinguisherApiController(IFireExtinguisherService fireExtinguisherService,IMainGoalService mainGoalService ,IInsActivityService insActivityService,
            ILoggingService loggingService, IFloorService floorService, ILocationService locationService, ITransactionService transactionService, IFloorAssetService floorAssetService)
        {
            _fireExtinguisherService = fireExtinguisherService;
            _mainGoalService = mainGoalService;
            _insActivityService = insActivityService;
            _loggingService = loggingService;
            _floorService = floorService;
            _locationService = locationService;
            _transactionService = transactionService;
            _floorAssetService = floorAssetService;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFireExtinguisherAssets(string floorId, string inspType, string routeId, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var stops = _fireExtinguisherService.GetExtinguisherAssets(Convert.ToInt32(floorId), Convert.ToInt32(inspType), !(string.IsNullOrEmpty(routeId)) ? Convert.ToInt32(routeId) : 0, Convert.ToInt32(assetId));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { Stops = stops };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetExtinguisherAssetsWithOutFloor(string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floorAssets = _fireExtinguisherService.GetExtinguisherAssetsWithOutFloor(Convert.ToInt32(assetId));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { TFloorAssets = floorAssets };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage RemoveFloorAssetsFromCurrentLocation(string orgFloorAssetsId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var res = _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(Convert.ToInt32(orgFloorAssetsId));
            _objHttpResponseMessage.Success = res;
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage ActivityHistory(string floorAssetId, string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floorAsset = _floorAssetService.GetFeFloorAssetInspActivity(Convert.ToInt32(floorAssetId), Convert.ToInt32(userId));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { TFloorAsset = floorAsset };
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetsByTagNo(string mode, string tagNo, string stopNo, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            switch (mode.ToLower())
            {
                case "asset":
                    {
                        var tfloorAssets = _fireExtinguisherService.GetExtinguisherAsset(tagNo, stopNo, assetId);
                        if (tfloorAssets != null)
                        {
                            _objHttpResponseMessage.Success = true;
                            _objHttpResponseMessage.Result = new Result { TFloorAsset = tfloorAssets };
                        }
                        else
                        {
                            _objHttpResponseMessage.Message = !string.IsNullOrEmpty(tagNo) ? ConstMessage.Tag_you_entered_does_not_exist : !string.IsNullOrEmpty(stopNo) ? ConstMessage.StopCode_you_entered_does_not_have_any_assets : string.Empty;
                            _objHttpResponseMessage.Success = false;
                        }

                        break;
                    }
                case "stop":
                    {
                        var stop = _locationService.GetStopByCode(stopNo) ?? new StopMaster();
                        if (stop.StopId > 0)
                        {
                            stop.TfloorAssets = _locationService.GetStopAssets(stop.StopId);
                            _objHttpResponseMessage.Result = new Result { Stop = stop };
                            _objHttpResponseMessage.Success = true;
                        }

                        break;
                    }
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage SaveInspection(TInspectionActivity objtInspectionActivity, string tagNo, string orgFloorAssetsId, string locationId, bool isMarkLocationEmpty, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var tfloorAssets = new TFloorAssets();
            if (!string.IsNullOrEmpty(tagNo))
            {
                objtInspectionActivity.ActivityId = Guid.NewGuid();
                tfloorAssets = _fireExtinguisherService.GetExtinguisherAsset(tagNo, null, assetId);
                if (tfloorAssets == null)
                {
                    _objHttpResponseMessage.Success = false;
                    _objHttpResponseMessage.Message = ConstMessage.Tag_you_entered_does_not_exist;
                    return _objHttpResponseMessage;
                }
                else if (Convert.ToInt32(orgFloorAssetsId) != objtInspectionActivity.FloorAssetId)
                {
                    var res = _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(Convert.ToInt32(orgFloorAssetsId));
                    if (res)
                    {
                        if (objtInspectionActivity.FloorAssetId != null)
                        {
                            var objtfloorAssets = new TFloorAssets
                            {
                                FloorAssetsId = objtInspectionActivity.FloorAssetId.Value,
                                StopId = Convert.ToInt32(locationId),
                                CreatedBy = objtInspectionActivity.CreatedBy
                            };
                            _fireExtinguisherService.AddFloorAssetsToLocation(objtfloorAssets);
                        }

                        objtInspectionActivity.TInsActivityId = SaveInspection(objtInspectionActivity, isMarkLocationEmpty);
                    }
                }
                else
                {
                    objtInspectionActivity.TInsActivityId = SaveInspection(objtInspectionActivity, isMarkLocationEmpty);
                }
            }
            if (objtInspectionActivity.TInsActivityId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    tInspectionActivity = objtInspectionActivity
                };
                _objHttpResponseMessage.Message = ConstMessage.Inspection_Added_Successfully;
            }
            else
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public int SaveInspection(TInspectionActivity activity, bool isMarkLocationEmpty)
        {
            var tinsActivityId = _transactionService.AddTInspectionActivity(activity);
            if (tinsActivityId > 0 && isMarkLocationEmpty)
            {
                if (activity.FloorAssetId != null)
                    _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(activity.FloorAssetId.Value);
            }
            return tinsActivityId;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage AddAssetsToLocation(string floorAssetId, string locationId, string userId, string orgFloorAssetsId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (Convert.ToInt32(orgFloorAssetsId) > 0)
            {
                _fireExtinguisherService.RemoveFloorAssetsFromCurrentLocation(Convert.ToInt32(orgFloorAssetsId));
            }

            var objtfloorAssets = new TFloorAssets
            {
                FloorAssetsId = Convert.ToInt32(floorAssetId),
                StopId = Convert.ToInt32(locationId),
                CreatedBy = Convert.ToInt32(userId)
            };
            var res = _fireExtinguisherService.AddFloorAssetsToLocation(objtfloorAssets);
            if (res)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Add_Success;
            }
            else
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <param name="inspType"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFloorAssetsStatus(string assetId, string ascIds, string inspType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floorAssetStatus = _floorService.GetFloorByAssetsWithStatus(assetId, ascIds, Convert.ToInt32(inspType));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { FloorAssetStatus = floorAssetStatus };
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFeReports(string assetId, string buildingId, string floorId, string inspType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int? buildingid = null;
            int? floorid = null;
            if (!string.IsNullOrEmpty(buildingId))
                buildingid = Convert.ToInt32(buildingId);
            if (!string.IsNullOrEmpty(floorId))
                floorid = Convert.ToInt32(floorId);
            var floorAssets = _fireExtinguisherService.Get_ExtinguisherAssetsReports(Convert.ToInt32(assetId), buildingid, floorid, Convert.ToInt32(inspType));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { TFloorAssets = floorAssets };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFeReport(string year, string routeId, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int? routNo = null;
            if (!string.IsNullOrEmpty(routeId))
                routNo = Convert.ToInt32(routeId);
            var floorAssets = _fireExtinguisherService.Get_ExtinguisherAssetsReport(Convert.ToInt32(year), routNo, Convert.ToInt32(assetId), 0);
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { TFloorAssets = floorAssets };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetStopsbyRouteId(string routeId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var stops = _fireExtinguisherService.GetExtinguisherAssets(null, null, Convert.ToInt32(routeId), null);
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { Stops = stops };
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// This method is used for get the routes on the basis of assets.
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRouteByAsset(string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var routes = _locationService.GetRouteByAsset(Convert.ToInt32(assetId));
            if (routes != null)
            {
                _objHttpResponseMessage.Result.Routes = routes;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// This method is used for get the Maingoal on the basis of assets.
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRouteAssetsMainGoal(string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var mainGoals = _mainGoalService.GetAssetMainGoals(Convert.ToInt32(assetId));
            if (mainGoals != null)
            {
                _objHttpResponseMessage.Result.MainGoals = mainGoals;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage FillStops(string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var stops = _locationService.GetLocationsMaster(null);
            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0) { stops = stops.Where(x => x.FloorId == Convert.ToInt32(floorId)).ToList(); }
            if (stops != null)
            {
                _objHttpResponseMessage.Result.Stops = stops;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetFrequency(string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var inspectionType = new List<FrequencyMaster>();
            inspectionType = _fireExtinguisherService.GetAssetInspFrequency(Convert.ToInt32(assetId));
            if (inspectionType != null)
            {
                _objHttpResponseMessage.Result.FrequencyMaster = inspectionType;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetActivityHistoryDetails(string activityId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var activity = _insActivityService.GetRouteInspectionActivity(Guid.Parse(activityId));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { tInspectionActivity = activity };
            return _objHttpResponseMessage;
        }
    }
}