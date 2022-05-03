using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HCF.BAL
{
    public  class AssetsService : IAssetsService
    {
        private readonly IAssetsSubCategoryRepository _assetsSubCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IEpRepository _epRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        private readonly ITransaction _transactionRepository;

        public AssetsService(ITransaction transactionRepository,IInspectionActivityRepository inspectionActivityRepository,IEpRepository epRepository,IFloorAssetRepository floorAssetRepository,IInspectionRepository inspectionRepository,IAssetsRepository assetsRepository,IAssetsSubCategoryRepository assetsSubCategoryRepository, ICategoryRepository categoryRepository, IAssetTypeRepository assetTypeRepository)
        {
            _transactionRepository = transactionRepository;
            _inspectionActivityRepository = inspectionActivityRepository;
            _epRepository = epRepository;
            _floorAssetRepository = floorAssetRepository;
            _inspectionRepository = inspectionRepository;
            _assetsRepository = assetsRepository;
               _assetsSubCategoryRepository = assetsSubCategoryRepository;
            _categoryRepository = categoryRepository;
            _assetTypeRepository = assetTypeRepository;

        }

        #region Assets

        public  List<Assets> Get()
        {
            return _assetsRepository.Get();
        }

        public  Assets Get(int assetId)
        {
            return _assetsRepository.Get().FirstOrDefault(x => x.AssetId == assetId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssets"></param>
        /// <returns></returns>
        public  int AddAssets(Assets newAssets)
        {
            return _assetsRepository.UpdateAsset(newAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssets"></param>
        /// <returns></returns>
        public  int UpdateAsset(Assets newAssets)
        {
            return _assetsRepository.UpdateAsset(newAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<Assets> GetAssets(int userId)
        {
            return _assetsRepository.GetAssets(userId);
        }

        public  List<Assets> GetAllAsset()
        {
            return _assetsRepository.GetAllAsset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<Assets> GetAssetMaster(int userId)
        {
            return _assetsRepository.GetAssetMaster(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public  List<AssetType> GetAssetsByType(int userId, int typeId)
        {
            List<Assets> assets = _assetsRepository.GetAssetMaster(userId);
            List<AssetType> types = _assetTypeRepository.AssetTypeMaster(userId);
            foreach (var item in types)
            {
                item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
            }
            return types;
           
        }


        public List<AssetType> GetAssetTypes(int userId)
        {
            var assets = _assetsRepository.Get().Where(x => x.IsActive).ToList();
            var types = _assetTypeRepository.AssetTypeMaster(userId).Where(x => x.IsActive).ToList();
            foreach (var item in types)
                item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
            return types;
        }


        public  int AttachAssetToEp(int assetId, int epId, int userId)
        {
            return _assetsRepository.AttachAssetToEp(assetId, epId, userId);
        }

        public  List<Assets> GetAssetFrequency()
        {
            return _assetsRepository.GetAssetFrequency();
        }

        #endregion

        #region Category

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Category> GetCategory()
        {
            return _categoryRepository.GetCategory();
        }

        #endregion

        #region Floor Assets


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssets(int floorId)
        {
            return _floorAssetRepository.GetFloorAssets(floorId);
        }

        public  IEnumerable<TFloorAssets> GetFloorAssets(Guid floorPlanId)
        {
            return _floorAssetRepository.GetFloorAssets(floorPlanId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  IEnumerable<TFloorAssets> GetTFloorAssets()
        {
            return _floorAssetRepository.GetTFloorAssets();
        }

        public  List<TFloorAssets> GetTFloorAssetList()
        {
            return _floorAssetRepository.GetTFloorAssetslist();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssets()
        {
            return _floorAssetRepository.GetFloorAssets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> View_getFloorAssets(int? assetId, int? buildingId, int? floorId, int? groupId)
        {
            return _floorAssetRepository.View_getFloorAssets(assetId, buildingId, floorId, groupId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  TFloorAssets GetFloorAssetDescription(int floorAssetId)
        {
            return _floorAssetRepository.GetFloorAssetDescription(floorAssetId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds)
        {
            return _floorAssetRepository.GetFloorAssetsForInspections(assetId, ascIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="ascIds"></param>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  List<Assets> InspectByFloor(string assetId, string ascIds, int buildingId, int floorId, int routeId)
        {
            return _floorAssetRepository.InspectByFloor(assetId, ascIds, buildingId, floorId, routeId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetAssetsWithoutFloor()
        {
            return _floorAssetRepository.GetAssetsWithoutFloor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public  bool EditFloorAssets(TFloorAssets newItem)
        {
            return _floorAssetRepository.UpdateFloorAssets(newItem);
        }

        public  bool CheckExistingAssets(string Deviceno)
        {
            return _floorAssetRepository.CheckExistingAssets(Deviceno);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TFloorAssets> GetFloorAssetsByBarcode(string barcode)
        {
            return _floorAssetRepository.GetFloorAssetsbyBarcode(barcode);
        }
        public List<TFloorAssets> GetFloorAssetsByBarcodeSearch(string barcode,int? IsAssertdevicechange= 0)
        {
            return _floorAssetRepository.GetFloorAssetsbyBarcodeSearch(barcode, IsAssertdevicechange.Value);
        }
        public  int UpdateAssetType(int ascid, int FloorAssetsId)
        {
            return _floorAssetRepository.UpdateAssetType(ascid, FloorAssetsId);
        }
        #endregion

        #region TfloorAsset Inspection History

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetsId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="selectedEp"></param>
        /// <returns></returns>
        public  TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp = 0)
        {
            return _inspectionRepository.GetAssetInsHistory(floorAssetsId, epDetailId, selectedEp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetsId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TFloorAssets GetAssetEpInsHistory(int floorAssetsId, int epDetailId, int userId)
        {
            var floorAssets = _floorAssetRepository.GetAssetEpInsHistory(floorAssetsId, epDetailId, userId);
            if (floorAssets != null)
            {
                floorAssets.EPDetails = new List<EPDetails>();
                var ep = _epRepository.GetEpDescription(epDetailId);
                ep.TInspectionActivity = _inspectionActivityRepository.GetInspectionActivityByEpAssets(ep.EPDetailId, floorAssetsId, userId).OrderByDescending(x => x.CreatedDate).ToList();
                foreach (var inspection in ep.TInspectionActivity)
                {
                    inspection.InspectionDocs = _transactionRepository.GetInspectionDocs(inspection.ActivityId);
                }
                floorAssets.EPDetails.Add(ep);
            }
            return floorAssets;

        }

        public  TFloorAssets GetFloorAssetInspectionActivity(int floorAssetsId, int epDetailId, int userId)
        {
            var floorAssets= _floorAssetRepository.GetFloorAssetInspectionActivity(floorAssetsId, epDetailId, userId);

            if (floorAssets != null)
            {
                floorAssets.TInspectionActivity =
                    _inspectionActivityRepository.GetFloorAssetInspectionActivity(floorAssetsId, epDetailId, userId);
            }
            return floorAssets;
        }

        #endregion

        #region Asset Mapping

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public  List<EPDetails> GetAssetEp(int assetId)
        {
            return _epRepository.GetEpByAssets(assetId);
        }
        

        public  EpAssets UpdateEpAssets(EpAssets epAssets)
        {
            return _epRepository.UpdateEpAssets(epAssets);
        }

        public  List<EPDetails> GetFloorAssetEp(int floorAssetId)
        {
            return _inspectionRepository.GetFloorAssetEp(floorAssetId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssetsMapping"></param>
        /// <returns></returns>
        public  int AddAssetsMapping(AssetsMapping newAssetsMapping)
        {
            return _assetsRepository.AddAssetsMapping(newAssetsMapping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public  List<Assets> GetAssetsMapping(string mode, string id)
        {
            return _assetsRepository.GetAssetsMapping(mode, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public  List<Assets> GetAssetsUserMapping(string mode, string id)
        {
            return _assetsRepository.GetAssetsUserMapping(mode, id);
        }

        #endregion

        #region Assets Inspections       
              

        public  List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate)
        {
            return _inspectionActivityRepository.GetFilterAssetsReports(assetids, buildingId, floorId, fromdate, todate);
        }

        public  List<TFloorAssets> InventoryAssetsReport(string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            return _floorAssetRepository.InventoryAssetsReport(ids, buildingId, floorId, fromdate, todate, status);
        }

        public  List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest, string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            return _inspectionActivityRepository.InventoryInspectionAssetsReport(objRequest, ids, buildingId, floorId, fromdate, todate, status);
        }

        #endregion

        public  Assets GetAssetEPs(int assetId)
        {
            return _assetsRepository.GetAssetEPs(assetId);
        }

        public  object GetAssetSubCategory(int assetId)
        {
            return _assetsSubCategoryRepository.GetAssetsSubCategory(assetId);
        }

        public  List<FireExtinguisherTypes> GetAssetSubCategorySize(int ascId)
        {
            return _assetsSubCategoryRepository.GetAssetSubCategorySize(ascId);
        }

        public Assets ScheduleAssets(int assetId, int epDetailId)
        {
            return _assetsSubCategoryRepository.ScheduleAssets(assetId, epDetailId);
        }


        public  List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId = null, int inprogress = 0, int pastDue = 0, int dueWitndays = 0,
            string assetIds = null, string buildingIds = null, string floorIds = null)
        {
            return _inspectionActivityRepository.GetAssetsDashBoard(userId, floorAssetId, inprogress, pastDue, dueWitndays, assetIds, buildingIds, floorIds);
        }

        public  DataTable GetAssetsDetailsByTmsAssetId(string tmsAssetsIds)
        {
            return _assetsRepository.GetAssetsDetailsByTmsAssetId(tmsAssetsIds);
        }
        public  string GetAssetsCurrentStatus(int Epdetailid, string FAssestId)
        {
            return _assetsRepository.GetAssetsCurrentStatus(Epdetailid, FAssestId);
        }
        #region Get Tracking Asset Creation
        public  List<object> GetTrackingAssetCreation(string assetIds, string campusid, string buildingId)
        {
            return _assetsRepository.GetTrackingAssetCreation(assetIds, campusid, buildingId);
        }

        public  List<TFloorAssets> GetAllTypesfloorassets()
        {
            return _assetsRepository.GetAllTypesfloorassets();
        }
        #endregion

        public List<EPDetails> GetAssetsEPOnRoute( int? routeId, string assetids)
        {
            return _epRepository.GetAssetsEPOnRoute( routeId, assetids);
        }
        public List<EPDetails> GetBulkAssetEp(string assetId)
        {
            return _epRepository.GetEpByBulkAssets(assetId);
        }

    }
}
