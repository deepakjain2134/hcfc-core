using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.Web.Models;    
using HCF.Web.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Shyjus.BrowserDetection;
namespace HCF.Web.Controllers
{
    [Authorize]
    public class AssetsController : BaseController
    {
        private readonly ISiteService _siteService;
        private readonly IBuildingsService _buildingsService;
        private readonly IAssetTypeService _assetTypeService;
        private readonly IAssetsService _assetService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly ILocationService _locationService;
        private readonly IEpService _epService;
        private readonly IFloorService _floorService;
        private readonly IStandardService _standardService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IInsStepsService _insStepsService;
        private readonly ITransactionService _transactionService;      
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IAppSetting _appsetting;
        private readonly IBrowserDetector _browserDetector;

        public AssetsController(IAppSetting appsetting,
            ICommonModelFactory commonModelFactory,              
            ISiteService siteService, IBuildingsService buildingsService,
            IAssetTypeService assetTypeService, IAssetsService assetService,
            ILocationService locationService, IMainGoalService mainGoalService, IFloorAssetService floorAssetService,
            IEpService epService, IFloorService floorService,
            IStandardService standardService,
            IInsStepsService insStepsService, ITransactionService transactionService, IHttpPostFactory httpService, IBrowserDetector browserDetector)            
        {
            _appsetting = appsetting;
            _commonModelFactory = commonModelFactory;         
            _transactionService = transactionService;
            _siteService = siteService;
            _buildingsService = buildingsService;
            _assetTypeService = assetTypeService;
            _assetService = assetService;
            _locationService = locationService; _floorAssetService = floorAssetService; _epService = epService;
            _floorService = floorService;          
            _standardService = standardService;
            _mainGoalService = mainGoalService;
            _insStepsService = insStepsService;
            _httpService = httpService;
            _browserDetector = browserDetector;
        }


        #region AssetType

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AssetsType()
        {
            UISession.AddCurrentPage("assets_AssetsType", 0, "Assets Type");
            var lists = _assetTypeService.GetAssetMaster();
            return View(lists);
        }


        /// <summary>
        /// gets data for edit/add operation for assets type
        /// </summary>
        /// <param name="assetTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAssetsType(int? assetTypeId)
        {
            string assetBreadCrumb = assetTypeId == 0 ? "Create Asset Type" : "Update Asset Type";
            UISession.AddCurrentPage("assets_AssetsType", 0, assetBreadCrumb);
            var newAssetType = new AssetType();
            if (assetTypeId.HasValue)
                newAssetType = _assetTypeService.GetAssetMaster().FirstOrDefault(m => m.TypeId == assetTypeId);
            else
                newAssetType.IsActive = true;

            return View(newAssetType);
        }

        /// <summary>
        /// saves data for edit/add operation for assets type
        /// </summary>
        /// <param name="newAssetType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAssetsType(AssetType newAssetType)
        {
            if (ModelState.IsValid)
            {
                newAssetType.CreatedBy = UserSession.CurrentUser.UserId;
                int typeId = _assetTypeService.Save(newAssetType);
                if (typeId > 0)
                {
                    SuccessMessage = ConstMessage.Success;
                    return RedirectToAction("assetstype");
                }
                else
                {
                    ErrorMessage = ConstMessage.AssetTypeCode_AlreadyExists;
                }
            }
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetTypeId"></param>
        /// <returns></returns>
        public ActionResult DeleteAssetsType(int assetTypeId)
        {
            AssetType newAssetType = _assetTypeService.GetAssetsType().FirstOrDefault(m => m.TypeId == assetTypeId);
            if (newAssetType != null)
            {
                newAssetType.IsActive = false;
                int typeId = _assetTypeService.Save(newAssetType);
                if (typeId > 0)
                {
                    SuccessMessage = ConstMessage.Delete_Successfully;
                    return RedirectToAction("assetstype");
                }
            }
            return RedirectToAction("assetstype");
        }

        #endregion

        #region Assets

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult Assets(int? id)
        {
            var lists = _assetService.GetAssetMaster(UserSession.CurrentUser.UserId);
            if (id.HasValue)
                lists = lists.Where(x => x.AssetTypeId == id).ToList();
            return View(lists);
        }





        /// <summary>
        ///  gets data for edit/add operation for asset
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAssets(int assetId)
        {
            if (assetId == 0)
            {
                ViewBag.Category = _assetService.GetCategory();
            }

            var newAssets = assetId == 0 ? new Assets() : _assetService.GetAssetMaster(4).FirstOrDefault(m => m.AssetId == assetId);
            ViewBag.AssetsTypes = _assetTypeService.GetAssetsType();
            return View(newAssets);
        }

        /// <summary>
        ///  gets data for edit/add operation for asset
        /// </summary>
        /// <param name="newAssets"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAssets(Assets newAssets)
        {
            if (ModelState.IsValid)
            {
                newAssets.CreatedBy = UserSession.CurrentUser.UserId;
                string postData = _commonModelFactory.JsonSerialize<Assets>(newAssets);
                int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                string result = _httpService.CallPostMethod(postData, APIUrls.Assets_EditAssets, ref statusCode);
                if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                {
                    var httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                    var assets = httpresponse.Result.Asset;
                    if (assets != null && assets.AssetId > 0)
                    {
                        SuccessMessage = ConstMessage.Saved_Successfully;
                        return RedirectToAction("Assets");
                    }
                }
                ViewBag.AssetsTypes = _assetTypeService.GetAssetsType();
            }
            return View(newAssets);
        }


        //[OutputCache(VaryByParam = "assetId", Duration = 360)]
        public ActionResult AssetEpListSideMenu(int assetId)
        {
            var epDetails = _epService.GetEpByAssets(assetId);
            return PartialView(epDetails);
        }

        #endregion

        #region Floor Assets

        /// <summary>
        ///   
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        // [CrxAuthorize(Roles = "Assets_FloorAssets")]
        public ActionResult FloorAssets(int? floorAssetId)
        {
            UISession.AddCurrentPage("assets_FloorAssets", 0, "Manage Assets");
            ViewBag.Assets = _assetService.GetAssets(UserSession.CurrentUser.UserId);

            var inspectionGroups = _transactionService.GetInspectionGroup().Where(x => x.IsActive).ToList();
            ViewBag.InspectionGroups = new SelectList(inspectionGroups, "InspectionGroupId", "GroupName", string.Empty);
            if (floorAssetId.HasValue)
                ViewBag.floorAssetId = floorAssetId.Value;
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="assetId"></param>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [AjaxOnly]
        public ActionResult FilterTFloorAssets(int? floorAssetId, int? assetId, int? buildingId, int? floorId, int? groupId)
        {
            var lists = _assetService.View_getFloorAssets(assetId, buildingId, floorId, groupId);
            if (floorAssetId.HasValue && floorAssetId.Value > 0)
            {
                lists = lists.Where(x => x.FloorAssetsId == floorAssetId.Value).ToList();
            }
            return PartialView("_GetFilterTFloorAssets", lists);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateFloorassets(int? floorAssetId)
        {
            var floorAssets = new TFloorAssets();
            if (floorAssetId.HasValue)
                floorAssets = _floorAssetService.GetFloorAsset(floorAssetId.Value);
            return View(floorAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFloorAssets(TFloorAssets modal)
        {
            bool status = false;
            modal.CreatedBy = UserSession.CurrentUser.UserId;
            if (modal.FloorAssetsId > 0)
                status = _assetService.EditFloorAssets(modal);
            else
                modal = _floorAssetService.AddFloorAssets(modal);

            if (modal.FloorAssetsId > 0 || status)
            {
                var text = ConstMessage.Success;
                return ReturnJsonResult(text);
            }
            return RedirectToRoute("floorAssets");
        }


        public ActionResult CheckExistingAssets(string Deviceno)
        {
            try
            {
                var ifAssetsExist = _assetService.CheckExistingAssets(Deviceno.Trim());
                return ReturnJsonResult(ifAssetsExist);
            }
            catch (Exception)
            {
                return ReturnJsonResult(false);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public ActionResult AddAssetsView(int assetId)
        {
            var floorAssets = new TFloorAssets
            {
                EPDetails = _epService.GetEpByAssets(assetId),
                Assets = _assetService.GetAssets(4).FirstOrDefault(x => x.AssetId == assetId)
            };
            if (floorAssets.Assets != null)
                floorAssets.AssetId = floorAssets.Assets.AssetId;

            return PartialView("_addFloorAssets", floorAssets);
        }
        public ActionResult TfloorAssetEdit(int? assetId, int? tfloorAssetId, string mode)
        {
            var tfloorAssets = new TFloorAssets();
            if (mode != "Inspection")
            {
                int userId = UserSession.CurrentUser.UserId;
                if (assetId > 0)
                {
                    tfloorAssets.EPDetails = _epService.GetEpByAssets(assetId.Value);
                    tfloorAssets.Assets = _assetService.GetAssets(userId).FirstOrDefault(x => x.AssetId == assetId);
                    if (tfloorAssets.Assets != null)
                        tfloorAssets.AssetId = tfloorAssets.Assets.AssetId;
                }
                else
                {
                    if (tfloorAssetId != null)
                        tfloorAssets = _assetService.GetFloorAssetDescription(tfloorAssetId.Value);
                    if (tfloorAssets.AssetId != null)
                        tfloorAssets.EPDetails = _epService.GetEpByAssets(tfloorAssets.AssetId.Value);
                }
                if ((tfloorAssets.EPDetails == null || tfloorAssets.EPDetails.Count == 0) && tfloorAssets.FloorAssetsId > 0)
                {
                    tfloorAssets.EPDetails = _assetService.GetFloorAssetEp(Convert.ToInt32(tfloorAssets.FloorAssetsId));
                }
                return PartialView("~/Views/Shared/PopUp/_tfloorAssetsEditdelete.cshtml", tfloorAssets);
            }
            else
            {
                if (tfloorAssetId != null)
                    tfloorAssets = _assetService.GetFloorAssetDescription(tfloorAssetId.Value);
                if (tfloorAssets.AssetId != null)
                    tfloorAssets.EPDetails = _epService.GetEpByAssets(tfloorAssets.AssetId.Value);
                if ((tfloorAssets.EPDetails == null || tfloorAssets.EPDetails.Count == 0) && tfloorAssets.FloorAssetsId > 0)
                {
                    tfloorAssets.EPDetails = _assetService.GetFloorAssetEp(Convert.ToInt32(tfloorAssets.FloorAssetsId));
                }
                return PartialView("~/Views/Shared/_floorAssetsView.cshtml", tfloorAssets);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TfloorAssetEdit(TFloorAssets _tfloorAssets)
        {
            var tfloorAssets = _tfloorAssets;
            var userId = UserSession.CurrentUser.UserId;
            _tfloorAssets.CreatedBy = userId;
            _tfloorAssets.Source = (int)BDO.Enums.Source.Internal;
            _tfloorAssets.DateCreated = DateTime.Now;
            if (_tfloorAssets.FloorAssetsId > 0)
                _tfloorAssets.OnFloorPlan = true;

            var postData = _commonModelFactory.JsonSerialize<TFloorAssets>(tfloorAssets);
            var uri = (_tfloorAssets.FloorAssetsId > 0) ? APIUrls.Assets_tfloorAssetEdit_Edit : APIUrls.Assets_tfloorAssetEdit_Add;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            return ReturnJsonResult(statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success) ? new { Result = result } :
                new { Result = string.Empty }
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveFloorassets(TFloorAssets modal)
        {
            modal.CreatedBy = UserSession.CurrentUser.UserId;
            bool result = _floorAssetService.Movefloorassets(modal, "move");
            return ReturnJsonResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public ActionResult FloorAssetsWithOutFloor(string floorId, bool? isShow)
        {
            ViewBag.FloorId = floorId;
            ViewBag.isShow = isShow ?? false;
            var floorAssets = string.IsNullOrEmpty(floorId) ? _assetService.GetAssetsWithoutFloor().Where(x => x.StatusCode == "ACTIV").ToList() : _assetService.GetTFloorAssetList()
                .Where(x => !x.OnFloorPlan && x.StatusCode == "ACTIV").ToList();
            if (ViewBag.AssetValues != null)
            {
                floorAssets = floorAssets.Where(x => ViewBag.AssetValues is List<int> values && values.Contains(Convert.ToInt32(x.AssetId))).ToList();
            }

            return ViewBag.isShow ? PartialView("_FloorAssetsWithOutFloor", floorAssets.OrderBy(x => x.AssetNo)) : PartialView("_setupAssetsWithOutFloor", floorAssets.OrderBy(x => x.AssetNo));
        }


        [HttpGet]
        public JsonResult GetFloorAsset(int floorAssetId)
        {
            var floorAssets = _assetService.GetFloorAssetDescription(floorAssetId);
            return ReturnJsonResult(new { result = floorAssets });
        }



        #endregion

        #region Common

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ActionResult FillSubTypes(int typeId)
        {
            var subTypes = _assetTypeService.GetAssetsType().Where(x => x.TypeId == typeId).ToList();
            return ReturnJsonResult(subTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetTypeId"></param>
        /// <returns></returns>  
        public ActionResult GetAssets(int assetTypeId)
        {
            var subTypes = _assetService.Get().Where(x => x.AssetTypeId == assetTypeId && x.IsActive).ToList();
            return ReturnJsonResult(subTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public ActionResult GetAssetSubCategory(int assetId)
        {
            var subTypes = _assetService.GetAssetSubCategory(assetId);
            return ReturnJsonResult(subTypes);
        }

        public ActionResult GetAssetSubCategorySize(int ascId)
        {

            var subTypesSize = _assetService.GetAssetSubCategorySize(ascId);
            return ReturnJsonResult(subTypesSize);
        }

        #endregion

        #region Set Goal

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetGoal(int? id)
        {
            var newMapping = new EpAssets();
            ViewBag.Standards = _standardService.Get().Where(m => m.IsActive = true).ToList();
            newMapping.AssetId = id;

            var assets = new Assets
            {
                AssetId = Convert.ToInt32(id),
                EPdetails = _assetService.GetAssetEp(Convert.ToInt32(id))
            };
            ViewBag.MappingEPAssets = assets;

            if (assets.EPdetails.Count > 0)
            {
                newMapping.EPDetail = assets.EPdetails.FirstOrDefault();
                newMapping.EPDetailId = assets.EPdetails.FirstOrDefault().EPDetailId;
                newMapping.AssetId = id;
            }

            return View(newMapping);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetGoal(EpAssets model)
        {
            model.IsActive = true;
            model.CreatedBy = UserSession.CurrentUser.UserId;
            model = _assetService.UpdateEpAssets(model);
            return ReturnJsonResult(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FillEpDetails(int id)
        {
            var assets = new Assets
            {
                AssetId = id,
                EPdetails = _assetService.GetAssetEp(id)
            };
            return PartialView("_setGoalEpAssets", assets);
        }

        public ActionResult DeleteAssetEp(int epId, int assetId)
        {
            var newMapping = new EpAssets
            {
                AssetId = assetId,
                EPDetailId = epId,
                IsActive = false
                ,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            _assetService.UpdateEpAssets(newMapping);
            return RedirectToAction("SetGoal", new { id = assetId });

        }
        #endregion

        #region User Assets

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AssetsUser()
        {
            var asssets = new List<Assets>();
            return View(asssets);
        }

        #endregion

        #region Setup / Inspections
        [CrxAuthorize(Roles = "inspection_assetinspection")]
        public ViewResult InsAssets(int? assetId)
        {
            UISession.AddCurrentPage("assets_InsAssets", 0, "Buildings & Floors");
            ViewBag.PageTitle = "Inspection";
            var buildings = _buildingsService.GetBuildingFloors();
            var types = _assetService.GetAssetsByType(UserSession.CurrentUser.UserId, 0).Where(x => x.IsActive).ToList();
            var assetTypes = types.Select(parent => new AssetType
            {
                TypeId = parent.TypeId,
                Name = parent.Name,
                IsActive = parent.IsActive,
                Assets = parent.Assets.Where(x => x.Count > 0 && x.StandardEps.Any()).ToList(),
                IsInternal = parent.IsInternal
            }).ToList();
            ViewBag.SetUpAssetsType = assetTypes;
            ViewBag.AssetId = assetId;
            ViewBag.BuildingFloors = buildings;
            ViewBag.SiteData = _siteService.GetSites();
            ViewBag.DistinctStates = JsonConvert.SerializeObject(buildings.Select(o => o.StateId).Distinct());
            ViewBag.DistinctCities = JsonConvert.SerializeObject(buildings.Select(o => o.CityId).Distinct());
            return View("Setup", buildings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        //[OutputCache(Duration=120*60)]
        //[CrxAuthorize(Roles = "assets_setup")]
        public IActionResult Setup(int? assetId)
        {
            UISession.AddCurrentPage("assets_Setup", 0, "Setup Assets");
            ViewBag.PageTitle = "Setup Assets";
            var buildings = _buildingsService.GetBuildingFloors();
            ViewBag.SiteData = _siteService.GetSites();
            ViewBag.SetUpAssetsType = _assetService.GetAssetTypes(UserSession.CurrentUser.UserId);
            ViewBag.AssetId = assetId;
            ViewBag.BuildingFloors = buildings;
            ViewBag.DistinctStates = JsonConvert.SerializeObject(buildings.Select(o => o.StateId).Distinct());
            ViewBag.DistinctCities = JsonConvert.SerializeObject(buildings.Select(o => o.CityId).Distinct());
            return View();
        }
        public ActionResult GetBuildingFloors(string SiteCode, int? StateId, int? CityId, int? SiteType)
        {
            List<Buildings> buildings;
            if (SiteType == 2 && CityId != 0 && StateId != 0)
                buildings = _buildingsService.GetBuildingFloors(SiteCode, StateId.ToString(), CityId.ToString());
            else
                buildings = _buildingsService.GetBuildingFloors(SiteCode);

            return PartialView("_buildingfloors", buildings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GetTfloorAssets(int floorId, int? type, int? epdetailid)
        {
            var tfloorsAssets = new List<TFloorAssets>();

            if (type.HasValue && type.Value == 1)
            {
                ViewBag.PageTitle = "Inspection";
                ViewBag.DisplayName = "Asset Inspection";
            }
            else
            {
                ViewBag.PageTitle = "Setup Assets";
                ViewBag.DisplayName = "Setup Assets On Floor";
            }
            UISession.AddCurrentPage("assets_GetTfloorAssets", 0, ViewBag.DisplayName);

            var floors = _floorService.GetFloor(floorId);
            ViewBag.Floors = floors;
            if (floors != null)
                tfloorsAssets = _assetService.GetFloorAssets(floors.FloorPlanId).Where(x => x.OnFloorPlan && !x.IsDeleted).ToList();

            ViewBag.epdetailid = epdetailid;

            if (ViewBag.AssetValues is List<int> values)
            {

                ViewBag.AssetsId = string.Join(",", values.Select(n => n.ToString()).ToArray());
                ViewBag.IsSetupBack = true;
                return View(tfloorsAssets.Where(x => values.Contains(Convert.ToInt32(x.AssetId))).ToList());
            }
            else
                return View(tfloorsAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public JsonResult FilterFloorAssets(List<int> values)
        {

            if (values == null && ViewBag.IsSetupBack != null && ViewBag.AssetsId != null && ViewBag.AssetValues != null)
            {
                values = ViewBag.AssetValues as List<int>;
                ViewBag.AssetsId = string.Join(",", values.Select(n => n.ToString()).ToArray());
            }


            ViewBag.AssetValues = values;
            if (values != null && values.Count > 0)
            {
                IEnumerable<Floor> floors = _floorService.GetFloorByAssets(values);
                if (ViewBag.IsSetupBack != null && ViewBag.AssetsId != null)
                {
                    ViewBag.IsSetupBack = null;
                    return ReturnJsonResult(new { Result = floors.Select(x => x.FloorId).ToList(), AssetsId = values.ToList() });
                }
                return ReturnJsonResult(new { Result = floors.Select(x => x.FloorId).ToList() });
            }

            return ReturnJsonResult(new { Result = 0 });

        }

        public ActionResult GetFilterBuildings(string assetId, string ascIds)
        {
            var buildings = _buildingsService.GetFilterBuildings(assetId, ascIds).Where(x => x.BuildingName != null && x.BuildingId > 0).ToList();
            return ReturnJsonResult(buildings);
        }

        #endregion

        #region App

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="epId"></param>
        /// <returns></returns>
        public ActionResult AssetEps(int floorAssetId, int epId, int ismultipleEP = 0)
        {
            if (ismultipleEP == 1)
                epId = 0;
            ViewBag.ismulipleEp = null;
            UISession.AddCurrentPage("assets_AssetEps", 0, "Inspection Summary");
            var floorAssets = _assetService.GetAssetInsHistory(floorAssetId, epId, epId);
            return View(floorAssets);
        }

        public ActionResult AssetEpInspDetails(int floorAssetId, int epId)
        {
            var floorAssets = _assetService.GetAssetInsHistory(floorAssetId, epId, epId);
            return PartialView("_AssetsEpsInspDetails", floorAssets);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorassetId"></param>
        /// <param name="epId"></param>
        /// <returns></returns>
        public ActionResult History(int floorassetId, int? epId)
        {
            UISession.AddCurrentPage("assets_History", 0, "Asset History");
            var userId = UserSession.CurrentUser.UserId;
            var floorAssets = _assetService.GetFloorAssetInspectionActivity(floorassetId, (epId ?? 0), userId);
            return View(floorAssets);
        }

        public ActionResult AssetsHistory(int floorassetId, int? epId)
        {
            UISession.AddCurrentPage("assets_AssetsHistory", 0, "Asset History");
            var userId = UserSession.CurrentUser.UserId;
            var floorAssets = _assetService.GetFloorAssetInspectionActivity(floorassetId, (epId ?? 0), userId);
            return View(floorAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult ActivityHistory(Guid? activityId, int? status)
        {
            if (!activityId.HasValue)
                return RedirectToRoute("error404");

            var inspection = _transactionService.GetActivityHistory(activityId.Value);
            UISession.AddCurrentPage("assets_ActivityHistory", 0, "Activity History");
            if (inspection != null)
            {
                ViewBag.epId = inspection.EPDetailId;
                ViewBag.inspectionId = inspection.InspectionId;
                int? floorAssetId = inspection.TInspectionActivity.Where(x => x.ActivityId == activityId.Value && x.ActivityType == 2).Select(x => x.FloorAssetId).FirstOrDefault();
                ViewBag.floorAssetId = floorAssetId;
                ViewBag.Status = status;
                return View(inspection);
            }
            return RedirectToRoute("error404");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult Activity(TInspectionActivity activity, int? status)
        {
            if (activity != null)
            {
                ViewBag.Status = status;
                return PartialView("~/Views/Shared/_activity.cshtml", activity);
            }
            return RedirectToRoute("error404");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsStepId"></param>
        /// <returns></returns>
        public ActionResult StepActivity(int? tInsStepId)
        {
            var tinsStepsTask = new List<TInsStepsTask>();
            if (tInsStepId.HasValue)
                tinsStepsTask = _insStepsService.GetTInsStepsTask(tInsStepId.Value);
            return PartialView("~/Views/Shared/_stepActivity.cshtml", tinsStepsTask);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>       
        public ActionResult DocActivityHistory(Guid activityId)
        {
            var inspection = _transactionService.GetActivityHistory(activityId);
            if (inspection != null)
            {
                ViewBag.epId = inspection.EPDetailId;
                ViewBag.inspectionId = inspection.InspectionId;
                return View(inspection);
            }
            return RedirectToRoute("error404");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CrxAuthorize(Roles = "inspection_assetview")]
        public ActionResult AssetView()
        {
            UISession.AddCurrentPage("assets_AssetView", 0, "Inspect by Asset ID");
            var assetTypes = _assetTypeService.GetAssetTypeMaster(UserSession.CurrentUser.UserId);
            return View(assetTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetTypeId"></param>
        /// <returns></returns>
        public ActionResult AssetsByType(int assetTypeId, int? assetid, int? epdetailid)
        {
            if (assetid > 0 && assetid != null)
            {
                var florAssets = _floorAssetService.GetFloorAssetsByAssetType(assetTypeId, UserSession.CurrentUser.UserId).Where(x => x.AssetId == assetid).ToList();
                ViewBag.epdetailid = epdetailid;
                return PartialView("_AssetView", florAssets);
            }
            else
            {
                var florAssets = _floorAssetService.GetFloorAssetsByAssetType(assetTypeId, UserSession.CurrentUser.UserId);
                return PartialView("_AssetView", florAssets);
            }



        }


        public ActionResult FloorAssetEPs(int floorAssetId, int assetTypeId)
        {
            var florAssets = _floorAssetService.GetFloorAssetsEps(assetTypeId, floorAssetId, UserSession.CurrentUser.UserId).ToList();
            return PartialView("_FloorAssetsView", florAssets);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetsid"></param>
        /// <returns></returns>
        [CrxAuthorize(Roles = "inspection_linear")]
        public ActionResult Inspection(int? assetsid)
        {
            UISession.AddCurrentPage("assets_Inspection", 0, "Upload Inspection");
            var buildings = _buildingsService.GetBuildings().Where(x => x.IsActive).ToList();
            ViewBag.Buildings = new SelectList(buildings, "BuildingId", "BuildingName", string.Empty);
            if ((assetsid != null || assetsid > 0))
            {
                ViewBag.Assetsid = assetsid;
            }
            else
            {
                ViewBag.Assetsid = 0;
            }
            return View();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Asset()
        {
            var userId = UserSession.CurrentUser.UserId;
            var types = _assetService.GetAssetsByType(userId, 0);
            var filterType = types.Where(y => y.Assets.Any(x => x.Count > 0 && x.StandardEps.Any())).ToList();
            return PartialView("_Assets", filterType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <returns></returns>
        public ActionResult GetFloorAssetsForIns(string buildingId, string floorId, string routeId, string assetId, string ascIds)
        {
            var assets = _assetService.InspectByFloor(assetId, ascIds, Convert.ToInt32(buildingId), Convert.ToInt32(floorId), Convert.ToInt32(routeId));
            return PartialView("_GetFloorAssetsForIns", assets);
        }

        public ActionResult GetFloorAssetStepsForIns(string buildingId, string floorId, string routeId, string assetId, string ascIds, int EPDetailId, int floorassetid)
        {
            var assets = _assetService.InspectByFloor(assetId, ascIds, Convert.ToInt32(buildingId), Convert.ToInt32(floorId), Convert.ToInt32(routeId));
            var floorasset = (assets.FirstOrDefault().TFloorAssets.Where(x => x.EpDetailId == EPDetailId && x.FloorAssetsId == floorassetid).OrderByDescending(x => x.Source).ThenBy(x => x.DeviceNo).ThenBy(x => x.EpDetailId)).FirstOrDefault();

            return PartialView("_FloorAssetStep", floorasset);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public JsonResult GetRoutesbyAssetId(string assetId)
        {
            var routes = _locationService.GetRouteByAsset(Convert.ToInt32(assetId)).Where(x => x.AssetCounts > 0 && x.RouteNo.Trim().ToLower() != "Inventory Assets".ToLower());
            return ReturnJsonResult(new { result = routes });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epId"></param>
        /// <returns></returns>
        public ActionResult DocumentAssets(int? epId)
        {
            if (!epId.HasValue)
                return RedirectToRoute("dashboard");

            var ePSteps = new EPSteps
            {
                EPDetailId = epId.Value,
                InspectionId = 0,
                EPDetails = _epService.GetEpDescription(epId.Value)
            };

            UISession.AddCurrentPage("assets_DocumentAssets", 0, "Assets Inspection Report");
            return View(ePSteps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epId"></param>
        /// <returns></returns>
        public ActionResult EpHistory(int epId)
        {
            UISession.AddCurrentPage("assets_EpHistory", 0, "EP History");
            var epDetail = _transactionService.GetEpInspectionHistory(epId);
            return View(epDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epId"></param>
        /// <returns></returns>
        public ActionResult EpActivityHistory(int inspectionId, int epId)
        {
            UISession.AddCurrentPage("assets_EpActivityHistory", 0, "EP Activity History");
            var epDetails = _transactionService.GetEpInspectionActivity(epId, inspectionId);

            if (epDetails != null && epDetails.Inspection != null)
            {
                epDetails.InspectionEPDocs = _transactionService.GetInspectionDocs(epDetails.Inspection.InspectionId).ToList();
            }

            return View(epDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAssetList"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult GetFloorAssetsForIns(List<MarkPassAssets> objAssetList)
        {
            int userId = UserSession.CurrentUser.UserId;
            var postData = _commonModelFactory.JsonSerialize<List<MarkPassAssets>>(objAssetList);
            var uri = $"{APIUrls.Assets_MarkPassAssets}/{userId}";
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var results = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
                var objEpSteps = httpResponse.Result.EPDocument.FirstOrDefault();
                if (httpResponse.Success && objEpSteps != null)
                {
                    var result = new { Result = true, objEpSteps.IsInspReady, objEpSteps.EPDetailId };
                    return ReturnJsonResult(result);
                }
            }
            return ReturnJsonResult(string.Empty);
        }

        #endregion       

        #region Assets for Schedule

        public ActionResult ScheduleAssets(int assetId, int epDetailId)
        {
            var assets = _assetService.ScheduleAssets(assetId, epDetailId);
            return PartialView("_scheduleAssets", assets);
        }


        #endregion

        #region BarCode 

        [HttpPost]
        public JsonResult GetAssetsByPrefix(string Prefix)
        {
            var floorAssets = _floorAssetService.GetAssetsByPrefix(Prefix);
            return Json(floorAssets);
        }


        public ActionResult AssetsBarCodes(int type, int assetId, int routeId)
        {
            var lists = new List<BarCodeViewModel>();
            if (type == 1)
            {
                var floorAssets = _floorAssetService.GetTFloorAssets(assetId, routeId);
                lists.AddRange(floorAssets.Select(item => new BarCodeViewModel { SerialNumber = item.SerialNo, Id = item.FloorAssetsId }));
            }
            else
            {
                var stopMasters = _locationService.GetLocationsMaster(null);
                lists.AddRange(stopMasters.Select(item => new BarCodeViewModel { SerialNumber = item.StopCode, Id = item.StopId }));
            }
            return View(lists.GroupBy(x => x.SerialNumber).Select(f => f.First()).ToList());
        }

        public ActionResult AssetsBarCodePdf(int type, int assetId, int routeId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var lists = new List<BarCodeViewModel>();
                if (type == 1)
                {
                    var floorAssets = _floorAssetService.GetTFloorAssets(assetId, routeId);
                    lists.AddRange(floorAssets.Select(item => new BarCodeViewModel { SerialNumber = item.SerialNo, Id = item.FloorAssetsId }));
                }
                else
                {
                    var stopMasters = _locationService.GetLocationsMaster(null);
                    lists.AddRange(stopMasters.Select(item => new BarCodeViewModel { SerialNumber = item.StopCode, Id = item.StopId }));
                }
                if (lists.Count > 0)
                {
                    var doc = new Document(PageSize.LETTER);
                    var writer = PdfWriter.GetInstance(doc, ms);
                    var BarCodePage = _appsetting.BarCodePage.Split('|');
                    var BarCodeImage = _appsetting.BarCodeImage.Split('|');

                    var topMargin = float.Parse(BarCodePage[0]);
                    var bottomMargin = float.Parse(BarCodePage[1]);
                    var leftMargin = float.Parse(BarCodePage[2]);
                    var rightMargin = float.Parse(BarCodePage[3]);

                    const int pageCols = 3;

                    doc.SetMargins(leftMargin, rightMargin, topMargin, bottomMargin);

                    doc.Open();

                    // Create the Label table

                    var table = new PdfPTable(pageCols) { WidthPercentage = 100f };
                    table.DefaultCell.Border = 0;


                    foreach (var list in lists.Where(x => !(string.IsNullOrEmpty(x.SerialNumber))))
                    {
                        #region Label Construction

                        Image PatImage1 = BarcodeImg(writer, list.SerialNumber.ToString(), BarCodeImage);
                        var cell = new PdfPCell(PatImage1)
                        {
                            Border = 0,
                            FixedHeight = float.Parse(BarCodePage[4]),
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);

                        #endregion
                    }
                    table.CompleteRow();
                    doc.Add(table);
                    // Close PDF document and send
                    writer.CloseStream = false;
                    doc.Close();
                    byte[] data = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.Headers.Add("Content-Disposition", "attachment;filename=barcodes.pdf");
                    Response.Body.WriteAsync(data, 0, data.Length);


                }
            }
            return ReturnJsonResult(new { Result = "nothing to print" });
        }

        private static Image BarcodeImg(PdfWriter writer, string code, IReadOnlyList<string> barCodeImage)
        {

            var cb = writer.DirectContent;
            var bc = new Barcode128
            {
                TextAlignment = Element.ALIGN_CENTER,
                Code = code.Replace("–", "-"),
                StartStopText = false,
                CodeType = Barcode.CODE128,
                Extended = true
            };
            var img = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
            img.ScaleAbsoluteHeight(Convert.ToInt32(barCodeImage[0]));
            img.ScaleAbsoluteWidth(Convert.ToInt32(barCodeImage[1]));
            return img;
        }

        #endregion

        #region Inspection by barcode
        [CrxAuthorize(Roles = "inspection_barcode")]
        public ActionResult Inspectionbybarcode()
        {
            UISession.AddCurrentPage("Assets_Inspectionbybarcode", 0, "Inspection by barcode");
            ViewBag.OsName = _browserDetector.Browser.OS; 
            return View();
        }

        [HttpGet]
        public ActionResult GetFloorAssetsbyBarcode(string deviceno)
        {
            var flooorAssets = _assetService.GetFloorAssetsByBarcode(deviceno);
            if (flooorAssets != null)
            {
                var assetFrequency = _assetService.GetAssetFrequency();
                foreach (var item in flooorAssets)
                {
                    item.Assets = assetFrequency.FirstOrDefault(x => x.AssetId == item.AssetId);
                }
            }
            return PartialView("~/Views/Shared/PopUp/_getFloorAssetsbyBarcode.cshtml", flooorAssets);
        }


        #endregion

        #region Assets Dashboard

        [CrxAuthorize(Roles = "AssetsDashBoard")]
        public ActionResult AssetsDashBoard()
        {
            UISession.AddCurrentPage("assets_AssetsDashBoard", 0, "Asset Dashboard");
            var assetsDashboardViewModel = new AssetsDashboardViewModel
            {
                assets = _assetService.GetAssetsDashBoard(UserSession.CurrentUser.UserId),
                buildings = _buildingsService.GetBuildingFloors()
            };


            return View(assetsDashboardViewModel);
        }


        public ActionResult PartialAssetsDashBoard(int Inprogress = 0, int pastDue = 0, int dueWitndays = 0, string assetIds = null, string buildingIds = null)
        {
            var assets = _assetService.GetAssetsDashBoard(UserSession.CurrentUser.UserId, null, Inprogress, pastDue, dueWitndays, assetIds, buildingIds, null);
            return PartialView("_assetsDashBoard", assets);
        }



        public ActionResult DashBoardInspPopup(int floorAssetId)
        {
            var currentAssets = _assetService.GetAssetsDashBoard(UserSession.CurrentUser.UserId, floorAssetId).FirstOrDefault(x => x.TFloorAssets.Count > 0);
            return PartialView("_dashBoardInspPopup", currentAssets);
        }
        #endregion

        #region Assets SeachPopUp

        public ActionResult SearchAssetsPopUp(string clickCtr,int? ddlDeviceType = 0,int? IsAssertdevicechange=0,int? Path=0)
        {
            ViewBag.ddlDeviceType = ddlDeviceType;
            ViewBag.IsAssertdevicechange = IsAssertdevicechange;
            ViewBag.Path = Path;
            return PartialView("_SearchAssetsPopUp", clickCtr);
        }

        public ActionResult SearchAssetsPopUpResult(string searchData, string clickCtr,int? ddlDeviceType=0,int? IsAssertdevicechange=0, int? Path = 0)
        {
            var flooorAsset = _assetService.GetFloorAssetsByBarcodeSearch(searchData, IsAssertdevicechange);
            if(IsAssertdevicechange.HasValue && IsAssertdevicechange.Value>0)
            {
                if(ddlDeviceType.HasValue && ddlDeviceType > 0)
                flooorAsset = flooorAsset.Where(x => x.AssetChangeDeviceType == ddlDeviceType.Value).ToList();
                else
                 flooorAsset = flooorAsset.Where(x => x.AssetChangeDeviceType>0).ToList();

                if (Path.HasValue && Path > 0)
                    flooorAsset = flooorAsset.Where(x => x.Path == Path).ToList();
                else
                    flooorAsset = flooorAsset.Where(x => x.Path > 0).ToList();
            }
            ViewBag.AssetsSearchclickCtr = clickCtr;
            return PartialView("_SearchAssetsPopUpResult", flooorAsset);
        }


        #endregion

        [HttpPost]

        public ActionResult UpdateAssetType(int ascid, int FloorAssetsId)
        {
            var result = _assetService.UpdateAssetType(ascid, FloorAssetsId);
            return ReturnJsonResult(result);
        }

        #region Tracking Asset Creation
        public ActionResult TrackingAssetCreation()
        {
            UISession.AddCurrentPage("assets_TrackingAssetCreation", 0, "Tracking Assets");
            var Campusite = _siteService.GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            var GetCampus = new SelectList(Campusite, "Code", "Name", "select");
            ViewBag.GetCampus = JsonConvert.SerializeObject(GetCampus);

            return View();
        }


        public ActionResult TrackAssetReport(string assetIds, string campusid, string buildingId)
        {
            var data = new List<object>();
            var result = _assetService.GetTrackingAssetCreation(assetIds, campusid, buildingId).ToList();
            data.Add(result[0]);
            data.Add(result[1]);
            data.Add(result[2]);
            ViewBag.headerassetsdata = result[3];
            ViewBag.assetdata = JsonConvert.SerializeObject(result[3]);
            ViewBag.FloorAssetdata = JsonConvert.SerializeObject(result[4]);
            ViewBag.datalist = JsonConvert.SerializeObject(data);
            ViewBag.TrackAssetIds = assetIds;
            ViewBag.TrackCampusIds = campusid;
            ViewBag.TrackBuildingIds = buildingId;
            return PartialView("_trackAssetReport");
        }

        public ActionResult Trackingassetmodel(int buildingId, int assetId, int edit = 0, int floorassetId = 0)
        {
            if (edit == 1)
            {

                var floorAssets = _floorAssetService.GetFloorAsset(floorassetId);

                var buildingdata = _buildingsService.GetBuildings().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
                ViewBag.buildingbbyId = new SelectList(buildingdata, "BuildingId", "BuildingName", 0);
                var floordata = _floorService.GetFloors().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
                ViewBag.floorbybuildingId = new SelectList(floordata, "FloorId", "FloorName", "Select");
                var assetsdata = _assetService.GetAllAsset().Where(x => x.AssetId == assetId).ToList();
                ViewBag.allAssets = new SelectList(assetsdata, "AssetId", "Name", "Select");
                int assettypeid = assetsdata.Select(x => x.AssetTypeId).FirstOrDefault();
                var assettypesdata = _assetTypeService.GetAssetMaster().Where(x => x.IsActive && x.TypeId == assettypeid).ToList();
                ViewBag.assettypesbyassetid = new SelectList(assettypesdata, "TypeId", "Name", "Select");
                return PartialView("_TrackingAssets", floorAssets);
            }
            else
            {
                var floorAssetslist = new TFloorAssets();
                var buildingdata = _buildingsService.GetBuildings().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
                ViewBag.buildingbbyId = new SelectList(buildingdata, "BuildingId", "BuildingName", 0);
                var floordata = _floorService.GetFloors().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
                ViewBag.floorbybuildingId = new SelectList(floordata, "FloorId", "FloorName", "Select");
                var assetsdata = _assetService.GetAllAsset().Where(x => x.AssetId == assetId).ToList();
                ViewBag.allAssets = new SelectList(assetsdata, "AssetId", "Name", "Select");
                int assettypeid = assetsdata.Select(x => x.AssetTypeId).FirstOrDefault();
                var assettypesdata = _assetTypeService.GetAssetMaster().Where(x => x.IsActive && x.TypeId == assettypeid).ToList();
                ViewBag.assettypesbyassetid = new SelectList(assettypesdata, "TypeId", "Name", "Select");

                var tfloorAssets = _floorAssetService.GetFloorAssets().Where(x => x.AssetId == assetId && x.IsTrackingAsset).ToList();

                var assetsName = GenerateTrackingAssetsName(assetsdata.FirstOrDefault().Name);
                var trackingassetsName = $"{assetsName}-{buildingdata.FirstOrDefault().BuildingCode}-{string.Format("{0:000}", tfloorAssets.Count + 1)}";
                floorAssetslist.Name = trackingassetsName;
                floorAssetslist.DeviceNo = trackingassetsName;
                return PartialView("_TrackingAssets", floorAssetslist);
            }

        }

        public string GenerateTrackingAssetsName(string str)
        {
            str = str.Replace("(", "").Replace(")", "");
            string[] assetsName = !string.IsNullOrEmpty(str) ? str.Split(new char[] { ' ', '-', '–', '/' }, StringSplitOptions.RemoveEmptyEntries) : null;           
            var asstsName = string.Empty;
            switch (assetsName.Length)
            {
                case 1:
                    asstsName = asstsName + assetsName[0].Substring(0, (assetsName[0].Length >= 4 ? 4 : assetsName[0].Length));
                    break;
                case 2:
                    asstsName = asstsName + assetsName[0].Substring(0, (assetsName[0].Length >= 2 ? 2 : assetsName[0].Length)) +
                        assetsName[1].Substring(0, (assetsName[1].Length >= 2 ? 2 : assetsName[1].Length));
                    break;
                case 3:
                    asstsName = asstsName + assetsName[0].Substring(0, (assetsName[0].Length >= 1 ? 1 : assetsName[0].Length)) +
                        assetsName[1].Substring(0, (assetsName[1].Length >= 2 ? 2 : assetsName[1].Length)) +
                        assetsName[2].Substring(0, (assetsName[2].Length >= 1 ? 1 : assetsName[2].Length));
                    break;
                default:
                    foreach (string strassets in assetsName)
                    {
                        asstsName = asstsName + strassets.Substring(0, (strassets.Length >= 1 ? 1 : strassets.Length));
                    }
                    break;
            }
            return asstsName.ToUpper();
        }



        public ActionResult Trackingeditassetmodel(int buildingId, int assetId)
        {
            dynamic expando = new ExpandoObject();
            var model = expando as IDictionary<string, object>;
            var Tfloorassetslst = new List<TFloorAssets>();
            var Tfloorassetsdetaillst = new List<TFloorAssets>();

            var buildingdata = _buildingsService.GetBuildings().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
            var floordata = _floorService.GetFloors().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
            var assetsdata = _assetService.GetAllAsset().Where(x => x.AssetId == assetId).ToList();
            var FloorAssets = _assetService.GetAllTypesfloorassets().ToList();
            var floorfilterdata = new List<Floor>();

            foreach (var floor in floordata)
            {
                if (assetsdata != null)
                {
                    foreach (var asset in assetsdata)
                    {
                        var data = FloorAssets.Where(x => x.FloorId == floor.FloorId && x.AssetId == asset.AssetId);
                        foreach (var fdata in data)
                        {
                            Tfloorassetslst.Add(fdata);
                            var Tfloorassetscls = _floorAssetService.GetFloorAsset(fdata.FloorAssetsId);
                            Tfloorassetsdetaillst.Add(Tfloorassetscls);
                        }
                        var flooraasetdatacount = FloorAssets.Count(x => x.FloorId == floor.FloorId && x.AssetId == asset.AssetId);

                        if (flooraasetdatacount > 0)
                        {
                            for (int index = 0; index < flooraasetdatacount; index++)
                            {
                                floorfilterdata.Add(floor);
                            }
                        }


                    }
                }
            }

            model.Add("Buildings", buildingdata);
            model.Add("Floors", floorfilterdata);
            model.Add("Assets", assetsdata);
            model.Add("Tfloorassetslst", Tfloorassetslst);
            model.Add("Tfloorassetsdetaillst", Tfloorassetsdetaillst);
            return PartialView("_TrackingEditAssets", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrackingFloorAssets(TFloorAssets modal)
        {
            bool status = false;
            modal.IsTrackingAsset = true;
            modal.CreatedBy = UserSession.CurrentUser.UserId;
            if (modal.FloorAssetsId > 0)
                status = _assetService.EditFloorAssets(modal);
            else
                modal = _floorAssetService.AddFloorAssets(modal);


            if (modal.FloorAssetsId > 0 && status)
            {
                SuccessMessage = ConstMessage.Success_Updated;
                return RedirectToRoute("TrackingAssets");
            }
            else if (modal.FloorAssetsId > 0 && !status)
            {
                SuccessMessage = ConstMessage.Success;
                return RedirectToRoute("TrackingAssets");
            }
            return RedirectToRoute("TrackingAssets");
        }
        public JsonResult UpdateTfloorassetstatus(int floorId, int assetId, string statusCode)
        {
            bool status = _floorAssetService.UpdateTfloorassetstatus(floorId, assetId, statusCode);
            return ReturnJsonResult(status);
        }

        #endregion

        #region GetAssetsCurrentStatus
        [HttpPost]
        public JsonResult GetAssetsCurrentStatus(int Epdetailid, List<int> FassetId)
        {
            string FAssetId = string.Join(",", FassetId);
            var results = _assetService.GetAssetsCurrentStatus(Epdetailid, FAssetId);
            return ReturnJsonResult(new { data = Convert.ToInt32(results) });
        }

        #endregion

        public ActionResult ConfugureFloorAssetSteps(int? floorAssetId)
        {
            UISession.AddCurrentPage("assets_ConfugureFloorAssetSteps", 0, "Configure ATS");
            ViewBag.Assets = _assetService.GetAssets(UserSession.CurrentUser.UserId);
            if (floorAssetId.HasValue)
                ViewBag.floorAssetId = floorAssetId.Value;
            return View();
        }

        [AjaxOnly]
        public ActionResult ConfugureTFloorAssets(int? assetId)
        {
            var lists = _assetService.View_getFloorAssets(assetId, null, null, null).Where(x => x.StatusCode == "ACTIV").ToList();
            return PartialView("_ConfugureTFloorAssets", lists);
        }

        [AjaxOnly]
        public ActionResult GetTFloorAssetSteps(int? floorAssetId, int? assetId)
        {
            ViewBag.FloorAssetId = floorAssetId ?? floorAssetId;
            ViewBag.AssetId = assetId ?? assetId;
            int clientno = UserSession.CurrentOrg.ClientNo;
            var lists = _mainGoalService.GetMainGoalsbyFloorAssetId(clientno, floorAssetId, assetId).Where(x => x.IsActive).ToList();
            return PartialView("_GetTFloorAssetSteps", lists);
        }
        #region Files details

        public JsonResult GetFileDetailsById(string epId , int? inspectionid)
        {           
            List<Inspection> allInspection = _transactionService.GetEpsInspections(Convert.ToInt32(epId)).OrderByDescending(x => x.CreatedDate).ToList();
            var currentInspection = new Inspection();
            if (inspectionid>0)
            currentInspection = allInspection.Where(x => x.InspectionId==inspectionid && x.EPDetailId == Convert.ToInt32(epId)).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            else
            currentInspection = allInspection.Where(x => x.IsCurrent && x.EPDetailId == Convert.ToInt32(epId)).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var inspectionepdocs = _transactionService.GetInspectionDocs(currentInspection.InspectionId).ToList();
            return ReturnJsonResult(new { result = inspectionepdocs });
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