using AutoMapper;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Infrastructure.Helpers;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.Web.Areas.Api.Models.Response;
using HCF.Web.Areas.Api.Models.Request;


namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/assets")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Assets")]
    public class AssetsApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssetsService _assetsService;
        private readonly ITransactionService _transactionservice;
        private readonly IAssetTypeService _assetTypeService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly IReportsService _reportService;
        private readonly IOrganizationService _organizationService;
        private readonly IFloorService _floorService;
        private readonly IApiCommon _apiCommon;
        private readonly IMapper _mapper;


        public AssetsApiController(IApiCommon apiCommon, ILoggingService loggingService, IAssetsService assetsService, ITransactionService transactionservice,
            IAssetTypeService assetTypeService, IFloorAssetService floorAssetService, IReportsService reportService, IOrganizationService organizationService,
            IFloorService floorService, IMapper mapper)
        {
            _apiCommon = apiCommon;
            _loggingService = loggingService;
            _assetsService = assetsService;
            _transactionservice = transactionservice;
            _assetTypeService = assetTypeService;
            _floorAssetService = floorAssetService;
            _reportService = reportService;
            _organizationService = organizationService;
            _floorService = floorService;
            _mapper = mapper;
        }


        #region Doc Status

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// 
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult MarkdocStatus(string inspectionId, string status)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var sts = _transactionservice.MarkInsdocStatus(Convert.ToInt32(inspectionId), Convert.ToInt32(status));
            _objHttpResponseMessage.Success = sts;
            _objHttpResponseMessage.Message = ConstMessage.Success;
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Assets

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsByType(string userId, string type)
        {
            if (string.IsNullOrEmpty(type))
                type = "0";

            var _objHttpResponseMessage = new HttpResponseMessage();
            var types = _assetsService.GetAssetsByType(Convert.ToInt32(userId), Convert.ToInt32(type));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { AssetType = types };
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetInternalAssetsByType(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var types = _assetTypeService.GetInternalAssetsByType(Convert.ToInt32(userId));
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { AssetType = types };
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetEPs(string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(assetId))
            {
                var results = _assetsService.GetAssetEPs(Convert.ToInt32(assetId));
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Asset = results };
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetDeficientAssets(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(userId))
            {
                var inspectionActivities = new List<TInspectionActivity>(); //InsActivityRepository.GetDeficiencyAssets(Convert.ToInt32(userId),null,null);
                var deficiencyAssets = inspectionActivities.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { tInspectionActivites = deficiencyAssets };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.Invalid_Parameter;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsByCategory(string userId, string type)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var types = _assetTypeService.GetAssetsByCategory(Convert.ToInt32(userId), Convert.ToInt32(type));
            if (types.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { AssetType = types };
                return Ok(_objHttpResponseMessage);
            }
            _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("editassets")]
        public ActionResult EditAssets(Assets newAssets)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(newAssets.FileContent) && !string.IsNullOrEmpty(newAssets.FileName))
            {
                //string fileName = $"{newAssets.AssetCode.ToLower().Replace(" ", "")}.{CommonUtility.GetFileExtension(newAssets.FileName).ToLower()}";
                string fileName = $"{newAssets.AssetCode.ToLower().Replace(" ", "")}.{newAssets.FileName.FileExtension()}";
                newAssets.IconPath = _apiCommon.SaveFile(newAssets.FileContent, fileName, "masterAssetFilePath", "", true).FilePath;
            }

            //if (!string.IsNullOrEmpty(newAssets.IconPath))
            //{
            newAssets.AssetId = newAssets.AssetId > 0 ? _assetsService.UpdateAsset(newAssets) : _assetsService.AddAssets(newAssets);
            _objHttpResponseMessage.Result = new Result { Asset = newAssets };
            //}

            if (newAssets.AssetId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Saved_Successfully;
                _objHttpResponseMessage.Result = new Result { Asset = newAssets };
                return Ok(_objHttpResponseMessage);
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// Get All SubTypes
        /// </summary>
        [HttpGet]
        [Route("subtype")]
        public ActionResult GetAssets()
        {
            var assets = _assetsService.GetAllAsset();
            List<AssetViewModel> assetViewModel = _mapper.Map<List<AssetViewModel>>(assets);
            if (assetViewModel.Count > 0)
            {
                var data = new Response<List<AssetViewModel>>(assetViewModel);
                return Ok(data);
            }

            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Get SubType by Id
        /// </summary>       
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("subtype/{id}")]
        public ActionResult GetAssetByAssetId(string id)
        {

            Assets asset = _assetsService.Get(Convert.ToInt32(id));
            AssetViewModel assetViewModel = _mapper.Map<AssetViewModel>(asset);
            if (assetViewModel != null && assetViewModel.Id > 0)
            {
                var data = new Response<AssetViewModel>(assetViewModel);
                return Ok(data);
            }

            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }


        // /// <summary>
        // /// Save Assets 
        // /// </summary>       
        // /// <param name="RequestModel"></param>
        // /// <returns></returns>
        // [HttpPost]
        //[Route("")]
        // public ActionResult SaveAssetRequest(AssetRequestModel RequestModel)
        // {

        //     Assets asset = _mapper.Map<Assets>(RequestModel);
        //     asset.AssetId = _assetsService.AddAssets(asset);

        //     if (asset.AssetId > 0)
        //     {
        //         var data = new Response<Int32>(asset.AssetId, ConstMessage.Saved_Successfully);
        //         return Ok(data);
        //     }
        //     return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        // }

        // /// <summary>
        // /// Update  Assets by AssetCode
        // /// </summary>       
        // /// <param name="RequestModel"></param>
        // /// <param name="assetcode"></param>
        // /// <returns></returns>
        // [HttpPut]
        // [Route("{assetcode}")]
        // public ActionResult UpdateAsset(AssetRequestModel RequestModel, string assetcode)
        // {
        //     Assets asset = _mapper.Map<Assets>(RequestModel);

        //     asset.AssetCode = assetcode;
        //     asset.AssetId = _assetsService.UpdateAsset(asset);
        //     if (asset.AssetId > 0)
        //     {

        //         var data = new Response<Int32>(asset.AssetId, ConstMessage.Floor_Assets_Update_Sucessfully);
        //         return Ok(data);
        //     }
        //     return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        // }

        #endregion




        #region Floor Assets

        /// <summary> 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addfloorassets")]
        public ActionResult AddFloorAssets(TFloorAssets newItem)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            newItem = _floorAssetService.AddFloorAssets(newItem);
            if (newItem.FloorAssetsId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result();
                _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Add_Success;
                _objHttpResponseMessage.Result.TFloorAsset = _floorAssetService.GetFloorAsset(newItem.FloorAssetsId);
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Add_AlreadyExists;
            }

            return Ok(_objHttpResponseMessage);
        }

        /// <summary> 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addImportedFloorAssets")]
        public ActionResult AddImportedFloorAssets(TFloorAssets newItem)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            try
            {
                newItem = _floorAssetService.AddImportedFloorAssets(newItem);
                if (newItem.FloorAssetsId > 0)
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result();
                    _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Add_Success;
                    _objHttpResponseMessage.Result.TFloorAsset = _floorAssetService.GetFloorAsset(newItem.FloorAssetsId);
                    _objHttpResponseMessage.Result.TFloorAssetItem = newItem;
                }
                else
                {
                    _objHttpResponseMessage.Success = false;
                    _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Add_AlreadyExists;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex);
                //ErrorLog.LogError(ex);
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        /// 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("editfloorassets")]
        public ActionResult Editfloorassets(TFloorAssets newItem)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = _assetsService.EditFloorAssets(newItem)
            };
            if (!_objHttpResponseMessage.Success)
                return Ok(_objHttpResponseMessage);
            _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Update_Sucessfully;
            _objHttpResponseMessage.Result = new Result
            {
                TFloorAsset = _floorAssetService.GetFloorAsset(newItem.FloorAssetsId)
            };
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("movefloorassets/{mode}")]
        public ActionResult MoveFloorAssets(TFloorAssets newItem, string mode)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = _floorAssetService.Movefloorassets(newItem, mode)
            };
            if (_objHttpResponseMessage.Success)
            {
                _objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Move_Sucessfully;
                _objHttpResponseMessage.Result = new Result
                {
                    TFloorAsset = _floorAssetService.GetFloorAsset(newItem.FloorAssetsId)
                };
            }
            return Ok(_objHttpResponseMessage);
        }





        ///// <summary>
        ///// </summary>
        ///// <param name="floorAssetId"></param>
        ///// <returns></returns>
        //public HttpResponseMessage Deletefloorassets(string floorAssetId)
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var status = AssetsRepository.Deletefloorassets(floorAssetId);
        //    if (status == 1)
        //    {
        //        objHttpResponseMessage.Success = true;
        //        objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Remove_Sucessfully;
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Message = ConstMessage.Floor_Assets_Is_In_Used;
        //    }
        //    return objHttpResponseMessage;
        //}

        /// <summary>
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetFloorAssets(string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var floor = _floorService.GetFloorDetails(Convert.ToInt32(floorId));
            if (floor != null)
            {
                var floorAssets = floor.TFloorAssets.Where(x => x.OnFloorPlan == true && x.IsDeleted == false).ToList();
                floor.TFloorAssets = floorAssets;
                if (floor != null)
                {
                    if (floor.FloorId > 0)
                    {
                        _objHttpResponseMessage.Result = new Result();
                        _objHttpResponseMessage.Success = true;
                        _objHttpResponseMessage.Result.Floor = floor;
                    }
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        ///// <summary>
        ///// </summary>
        ///// <returns></returns>
        //public HttpResponseMessage GetAssetsWithoutFloor(string floorId)
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    if (string.IsNullOrEmpty(floorId))
        //    {
        //        var floorAssets = AssetsRepository.GetAssetsWithoutFloor();
        //        if (floorAssets.Count > 0)
        //        {
        //            objHttpResponseMessage.Result = new Result();
        //            objHttpResponseMessage.Success = true;
        //            objHttpResponseMessage.Result.TFloorAssets = floorAssets;
        //        }
        //        else
        //        {
        //            objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //        }
        //    }
        //    else
        //    {
        //        List<TFloorAssets> allFloorAssets = HCF.BAL.AssetsRepository.GetTFloorAssets().Where(x => x.FloorId != Convert.ToInt32(floorId) && x.Xcoordinate == "0").ToList();
        //        var floor = FloorRepository.GetFloorDetails(Convert.ToInt32(floorId));
        //        List<TFloorAssets> floorAssets = floor.TFloorAssets.Where(x => x.Xcoordinate == "0" && x.IsDeleted == false).ToList();
        //        floorAssets.AddRange(allFloorAssets);
        //        //floor.TFloorAssets = floorAssets;
        //        if (floor != null)
        //        {
        //            if (floor.FloorId > 0)
        //            {
        //                objHttpResponseMessage.Result = new Result();
        //                objHttpResponseMessage.Success = true;
        //                objHttpResponseMessage.Result.TFloorAssets = floorAssets;
        //            }
        //        }
        //        else
        //        {
        //            objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //        }

        //    }
        //    return objHttpResponseMessage;
        //}

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsWithoutFloor(List<Assets> assets, string floorId)
        {
            var floorAssets = new List<TFloorAssets>();
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (assets.Count > 0)
            {
                if (string.IsNullOrEmpty(floorId))
                    floorAssets = _assetsService.GetAssetsWithoutFloor();
                else
                {
                    floorAssets = _assetsService.GetTFloorAssets()
                       .Where(x => x.OnFloorPlan == false).ToList();
                }
                floorAssets = floorAssets.Where(x => assets.Any(y => y.AssetId == x.AssetId)).ToList();
            }

            if (floorAssets.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result();
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result.TFloorAssets = floorAssets;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetFloorAssetsForIns(string userid, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _assetsService.GetFloorAssetsForInspections(assetId, "");
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { TFloorAssets = assets };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetFloorAssetsForReport(string userid, string assetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _reportService.GetFloorAssetsForReports(assetId, "");
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { TFloorAssets = assets };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult InspectByFloor(string assetId, string ascIds, string buildingId, string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _assetsService.InspectByFloor(assetId, ascIds, Convert.ToInt32(buildingId), Convert.ToInt32(floorId), 0);
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { Assets = assets };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetFloorAssetsByBarcode(string barcode)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _assetsService.GetFloorAssetsByBarcode(barcode);
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { TFloorAssets = assets };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// Save Floor Assets 
        /// </summary>       
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public ActionResult SaveFloorAssetRequest(SaveFloorAssetRequestModel RequestModel)
        {

            TFloorAssets floorasset = _mapper.Map<TFloorAssets>(RequestModel);
            var newItem = _floorAssetService.AddFloorAssets(floorasset);

            if (newItem.FloorAssetsId > 0)
            {
                FloorAssetViewModel floorassetview = _mapper.Map<FloorAssetViewModel>(newItem);
                var data = new Response<FloorAssetViewModel>(floorassetview, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Update  Assets 
        /// </summary>       
        /// <param name="RequestModel"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Assets/{cmmsReference}")]
        public ActionResult UpdateFloorAssetRequest(UpdateFloorAssetRequestModel RequestModel, string cmmsReference)
        {
            var floorAssets = _floorAssetService.GetFloorAssets().FirstOrDefault(x => x.TmsReference == cmmsReference);
            TFloorAssets floorasset = _mapper.Map<TFloorAssets>(RequestModel);
            floorasset.DeviceNo = floorAssets.DeviceNo;
            floorasset.TmsReference = cmmsReference;
            var newItem = _floorAssetService.AddFloorAssets(floorasset);

            if (newItem.FloorAssetsId > 0)
            {
                FloorAssetViewModel floorassetview = _mapper.Map<FloorAssetViewModel>(newItem);
                var data = new Response<FloorAssetViewModel>(floorassetview, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Get  Assets       
        /// </summary>       
        [HttpGet]
        [Route("Assets")]
        public ActionResult GetFloorAsset(int? subtypeid, int? buildingid, int? floorId)
        {
            SubType SubType = new SubType();
            TypesViewModel type = new TypesViewModel();
            List<FloorAssetViewModel> floorassetViewModel = new List<FloorAssetViewModel>();
            var floor = _assetsService.View_getFloorAssets(subtypeid, buildingid, floorId, null);
            if (floor != null && floor.Count > 0)
            {


                foreach (var Floor in floor)
                {
                    FloorAssetViewModel floorasset = _mapper.Map<FloorAssetViewModel>(Floor);
                    SubType = _mapper.Map<SubType>(Floor.Assets);
                    type = _mapper.Map<TypesViewModel>(Floor.Assets.AssetType);
                    floorasset.SubType = SubType;
                    floorasset.Type = type;
                    floorassetViewModel.Add(floorasset);
                }


                var data = new Response<List<FloorAssetViewModel>>(floorassetViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }
        /// <summary>
        /// Get  Assets By Id      
        /// </summary>
        ///<param name="id"></param>
        [HttpGet]
        [Route("Assets/{id}")]
        public ActionResult GetFloorAsset(int id)
        {
            TFloorAssets floor = new TFloorAssets();
            floor = _assetsService.View_getFloorAssets(null, null, null, null).FirstOrDefault(x => x.FloorAssetsId == id);
            if (floor != null && floor.FloorAssetsId > 0)
            {
                FloorAssetViewModel floorassetViewModel = _mapper.Map<FloorAssetViewModel>(floor);
                var data = new Response<FloorAssetViewModel>(floorassetViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }



        #endregion

        #region Assign Goal

        /// <summary>
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="ePdetailId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetInsHistory(string floorAssetId, string ePdetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets =
                _assetsService.GetAssetInsHistory(Convert.ToInt32(floorAssetId), Convert.ToInt32(ePdetailId));
            if (assets != null)
            {
                if (assets.FloorAssetsId > 0)
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { TFloorAsset = assets };
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="ePdetailId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetEpInsHistory(string floorAssetId, string ePdetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets =
                _assetsService.GetAssetEpInsHistory(Convert.ToInt32(floorAssetId), Convert.ToInt32(ePdetailId), 4);
            if (assets != null)
            {
                if (assets.FloorAssetsId > 0)
                {
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { TFloorAsset = assets };
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Assets Mapping

        /// <summary>
        /// </summary>
        /// <param name="newAssetsMapping"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult SaveAssetsMapping(AssetsMapping newAssetsMapping)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = _assetsService.AddAssetsMapping(newAssetsMapping);
            if (result > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Success;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsMapping(string mode, string id)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _assetsService.GetAssetsMapping(mode.ToUpper(), id);
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Assets = assets };
                return Ok(_objHttpResponseMessage);
            }
            _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsUserMapping(string mode, string id)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _assetsService.GetAssetsUserMapping(mode.ToUpper(), id);
            if (assets.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Assets = assets };
                return Ok(_objHttpResponseMessage);
            }
            _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Floor Shapes

        /// <summary>
        /// </summary>
        /// <param name="newFloorShapes"></param>
        /// <returns></returns>
        //public ActionResult SaveFloorShape(FloorShapes newFloorShapes)
        //{
        //  var  _objHttpResponseMessage = new HttpResponseMessage();
        //    var result = _organizationService.SaveFloorShapes(newFloorShapes); use floorServices
        //    if (result > 0)
        //    {
        //        newFloorShapes.Id = result;
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Message = ConstMessage.Success;
        //        _objHttpResponseMessage.Result = new Result { FloorShape = newFloorShapes };
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Success = false;
        //        _objHttpResponseMessage.Message = ConstMessage.Error;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult DeleteFloorShape(string id)
        //{
        //   var _objHttpResponseMessage = new HttpResponseMessage();
        //    var status = _organizationService.SaveFloorShapes(Convert.ToInt32(id)); use floorServices do not use _organizationService
        //    if (status)
        //    {
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Message = ConstMessage.Delete_Successfully;
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Success = false;
        //        _objHttpResponseMessage.Message = ConstMessage.Error;
        //    }
        //    return Ok(_objHttpResponseMessage);
        //}

        #endregion

        #region Offline Assets 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetOfflineAssets()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _objHttpResponseMessage = _assetTypeService.GetOfflineAssets();
            //objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            return Ok(_objHttpResponseMessage);
        }
        #endregion

        #region AssetsType
        /// <summary>
        /// Get All Assets Type
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        [Route("type")]
        public ActionResult GetAssetsType()
        {
            var types = _assetTypeService.GetAssetMaster();
            List<AssetTypeViewModel> assetTypeViewModel = _mapper.Map<List<AssetTypeViewModel>>(types);
            if (assetTypeViewModel.Count > 0)
            {
                var data = new Response<List<AssetTypeViewModel>>(assetTypeViewModel);
                return Ok(data);
            }

            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Get Asset type by TypeId
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("type/{typeid}")]
        public ActionResult GetAssetsTypeByType(string typeid)
        {
            AssetType type = _assetTypeService.GetAssetMaster().FirstOrDefault(x => x.TypeId == Convert.ToInt32(typeid));
            AssetTypeViewModel assetTypeViewModel = _mapper.Map<AssetTypeViewModel>(type);
            if (assetTypeViewModel != null && assetTypeViewModel.Id > 0)
            {
                var data = new Response<AssetTypeViewModel>(assetTypeViewModel);
                return Ok(data);
            }

            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        ///// <summary>
        ///// Create Asset Type
        ///// </summary>
        ///// <param name="RequestModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("assettype")]
        //public ActionResult SaveAssetTypeRequest(AssetTypeRequestModel RequestModel)
        //{
        //    AssetType type = _mapper.Map<AssetType>(RequestModel);

        //    type.TypeId = _assetTypeService.Save(type);
        //    if (type.TypeId > 0)
        //    {
        //        var data = new Response<Int32>(type.TypeId, ConstMessage.Saved_Successfully);
        //        return Ok(data);
        //    }
        //    return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        //}


        ///// <summary>
        ///// Update Asset Type By TypeId
        ///// </summary>
        ///// <param name="RequestModel"></param>
        ///// /// <param name="typeid"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("assettype/{typeid}")]
        //public ActionResult SaveAssetTypeRequest(AssetTypeRequestModel RequestModel, int typeid)
        //{
        //    AssetType type = _mapper.Map<AssetType>(RequestModel);
        //    type.TypeId = typeid;
        //    type.TypeId = _assetTypeService.Save(type);
        //    if (type.TypeId > 0)
        //    {

        //        var data = new Response<Int32>(type.TypeId, ConstMessage.Module_Training);
        //        return Ok(data);
        //    }
        //    return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        //}
        #endregion
    }
}