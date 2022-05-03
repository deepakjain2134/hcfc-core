//using HCF.BDO;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class AssetsRepository //: IAssetsService
//    {

//        #region Assets

//        public static List<Assets> Get()
//        {
//            return DAL.AssetsRepository.Get();
//        }

//        public static Assets Get(int assetId)
//        {
//            return DAL.AssetsRepository.Get().FirstOrDefault(x => x.AssetId == assetId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newAssets"></param>
//        /// <returns></returns>
//        public static int AddAssets(Assets newAssets)
//        {
//            return DAL.AssetsRepository.UpdateAsset(newAssets);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newAssets"></param>
//        /// <returns></returns>
//        public static int UpdateAsset(Assets newAssets)
//        {
//            return DAL.AssetsRepository.UpdateAsset(newAssets);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public static List<Assets> GetAssets(int userId)
//        {
//            return DAL.AssetsRepository.GetAssets(userId);
//        }

//        public static List<Assets> GetAllAsset()
//        {
//            return DAL.AssetsRepository.GetAllAsset();
//        }

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="userId"></param>
//        ///// <returns></returns>
//        //public static List<Assets> GetAssetMaster(int userId)
//        //{
//        //    return DAL.AssetsRepository.GetAssetMaster(userId);
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="typeId"></param>
//        /// <returns></returns>
//        public static List<AssetType> GetAssetsByType(int userId, int typeId)
//        {
//            List<Assets> assets = DAL.AssetsRepository.GetAssetMaster(userId);
//            List<AssetType> types = DAL.AssetTypeRepository.AssetTypeMaster(userId);
//            foreach (var item in types)
//            {
//                item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
//            }
//            return types;
//            //return AssetTypeRepository.GetAssetsByCategory()
//            //return DAL.AssetTypeRepository.GetAssetsByType(userId, typeId);
//        }


//        public static List<AssetType> GetAssetTypes(int userId)
//        {
//            List<Assets> assets = DAL.AssetsRepository.Get().Where(x => x.IsActive).ToList();
//            List<AssetType> types = DAL.AssetTypeRepository.AssetTypeMaster(userId).Where(x => x.IsActive).ToList();
//            foreach (var item in types)
//                item.Assets = assets.Where(x => x.AssetTypeId == item.TypeId).ToList();
//            return types;
//        }


//        public static int AttachAssetToEp(int assetId, int epId, int userId)
//        {
//            return DAL.AssetsRepository.AttachAssetToEp(assetId, epId, userId);
//        }

//        public static List<Assets> GetAssetFrequency()
//        {
//            return DAL.AssetsRepository.GetAssetFrequency();
//        }

//        #endregion

//        #region Category

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<Category> GetCategory()
//        {
//            return DAL.CategoryRepository.GetCategory();
//        }

//        #endregion

//        #region Floor Assets


//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="floorId"></param>
//        ///// <returns></returns>
//        //public static List<TFloorAssets> GetFloorAssets(int floorId)
//        //{
//        //    return DAL.FloorAssetRepository.GetFloorAssets(floorId);
//        //}

//        //public static IEnumerable<TFloorAssets> GetFloorAssets(Guid floorPlanId)
//        //{
//        //    return DAL.FloorAssetRepository.GetFloorAssets(floorPlanId);
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static IEnumerable<TFloorAssets> GetTFloorAssets()
//        {
//            return DAL.FloorAssetRepository.GetTFloorAssets();
//        }

//        //public static List<TFloorAssets> GetTFloorAssetList()
//        //{
//        //    return DAL.FloorAssetRepository.GetTFloorAssetslist();
//        //}


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<TFloorAssets> GetFloorAssets()
//        {
//            return DAL.FloorAssetRepository.GetFloorAssets();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        //public static List<TFloorAssets> View_getFloorAssets(int? assetId, int? buildingId, int? floorId, int? groupId)
//        //{
//        //    return DAL.FloorAssetRepository.View_getFloorAssets(assetId, buildingId, floorId, groupId);
//        //}


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        //public static TFloorAssets GetFloorAssetDescription(int floorAssetId)
//        //{
//        //    return DAL.FloorAssetRepository.GetFloorAssetDescription(floorAssetId);
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="assetId"></param>
//        /// <param name="ascIds"></param>
//        /// <returns></returns>
//        public static List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetsForInspections(assetId, ascIds);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="assetId"></param>
//        /// <param name="ascIds"></param>
//        /// <param name="buildingId"></param>
//        /// <param name="floorId"></param>
//        /// <returns></returns>
//        public static List<Assets> InspectByFloor(string assetId, string ascIds, int buildingId, int floorId, int routeId)
//        {
//            return DAL.FloorAssetRepository.InspectByFloor(assetId, ascIds, buildingId, floorId, routeId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<TFloorAssets> GetAssetsWithoutFloor()
//        {
//            return DAL.FloorAssetRepository.GetAssetsWithoutFloor();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        /// <returns></returns>
//        public static bool EditFloorAssets(TFloorAssets newItem)
//        {
//            return DAL.FloorAssetRepository.UpdateFloorAssets(newItem);
//        }

//        //public static bool CheckExistingAssets(string Deviceno)
//        //{
//        //    return DAL.FloorAssetRepository.CheckExistingAssets(Deviceno);
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<TFloorAssets> GetFloorAssetsByBarcode(string barcode)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetsbyBarcode(barcode);
//        }

//        //public static int UpdateAssetType(int ascid, int FloorAssetsId)
//        //{
//        //    return DAL.FloorAssetRepository.UpdateAssetType(ascid, FloorAssetsId);
//        //}
//        #endregion

//        #region TfloorAsset Inspection History

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetsId"></param>
//        /// <param name="epDetailId"></param>
//        /// <param name="selectedEp"></param>
//        /// <returns></returns>
//        public static TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp = 0)
//        {
//            return DAL.FloorAssetRepository.GetAssetInsHistory(floorAssetsId, epDetailId, selectedEp);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetsId"></param>
//        /// <param name="epDetailId"></param>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public static TFloorAssets GetAssetEpInsHistory(int floorAssetsId, int epDetailId, int userId)
//        {
//            return DAL.FloorAssetRepository.GetAssetEpInsHistory(floorAssetsId, epDetailId, userId);
//        }

//        //public static TFloorAssets GetFloorAssetInspectionActivity(int floorAssetsId, int epDetailId, int userId)
//        //{
//        //    return DAL.FloorAssetRepository.GetFloorAssetInspectionActivity(floorAssetsId, epDetailId, userId);
//        //}

//        #endregion

//        #region Asset Mapping

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="assetId"></param>
//        /// <returns></returns>
//        public static List<EPDetails> GetAssetEp(int assetId)
//        {
//            return DAL.EpRepository.GetEpByAssets(assetId);
//        }

//        //public static EpAssets UpdateEpAssets(EpAssets epAssets)
//        //{
//        //    return DAL.EpRepository.UpdateEpAssets(epAssets);
//        //}

//        //public static List<EPDetails> GetFloorAssetEp(int floorAssetId)
//        //{
//        //    return DAL.EpRepository.GetFloorAssetEp(floorAssetId);

//        //}


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newAssetsMapping"></param>
//        /// <returns></returns>
//        public static int AddAssetsMapping(AssetsMapping newAssetsMapping)
//        {
//            return DAL.AssetsRepository.AddAssetsMapping(newAssetsMapping);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="mode"></param>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static List<Assets> GetAssetsMapping(string mode, string id)
//        {
//            return DAL.AssetsRepository.GetAssetsMapping(mode, id);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="mode"></param>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static List<Assets> GetAssetsUserMapping(string mode, string id)
//        {
//            return DAL.AssetsRepository.GetAssetsUserMapping(mode, id);
//        }

//        #endregion

//        #region Assets Inspections       

//        //public static List<TFloorAssets> GetAssetsReports()
//        //{
//        //    return DAL.FloorAssetRepository.GetAssetsReports();
//        //}

//        public static List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate)
//        {
//            return DAL.FloorAssetRepository.GetFilterAssetsReports(assetids, buildingId, floorId, fromdate, todate);
//        }

//        public static List<TFloorAssets> InventoryAssetsReport(string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
//        {
//            return DAL.FloorAssetRepository.InventoryAssetsReport(ids, buildingId, floorId, fromdate, todate, status);
//        }

//        public static List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest, string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
//        {
//            return DAL.FloorAssetRepository.InventoryInspectionAssetsReport(objRequest, ids, buildingId, floorId, fromdate, todate, status);
//        }

//        #endregion

//        public static Assets GetAssetEPs(int assetId)
//        {
//            return DAL.AssetsRepository.GetAssetEPs(assetId);
//        }

//        //public static object GetAssetSubCategory(int assetId)
//        //{
//        //    return DAL.AssetsSubCategoryRepository.GetAssetsSubCategory(assetId);
//        //}

//        //public static List<FireExtinguisherTypes> GetAssetSubCategorySize(int ascId)
//        //{
//        //    return DAL.AssetsSubCategoryRepository.GetAssetSubCategorySize(ascId);
//        //}

//        //public static Assets ScheduleAssets(int assetId, int epDetailId)
//        //{
//        //    return DAL.AssetsSubCategoryRepository.ScheduleAssets(assetId, epDetailId);
//        //}


//        //public static List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId = null, int inprogress = 0, int pastDue = 0, int dueWitndays = 0,
//        //    string assetIds = null, string buildingIds = null, string floorIds = null)
//        //{
//        //    return DAL.AssetsRepository.GetAssetsDashBoard(userId, floorAssetId, inprogress, pastDue, dueWitndays, assetIds, buildingIds, floorIds);
//        //}

//        public static DataTable GetAssetsDetailsByTmsAssetId(string tmsAssetsIds)
//        {
//            return DAL.AssetsRepository.GetAssetsDetailsByTmsAssetId(tmsAssetsIds);
//        }
//        //public static string GetAssetsCurrentStatus(int Epdetailid, string FAssestId)
//        //{
//        //    return DAL.AssetsRepository.GetAssetsCurrentStatus(Epdetailid, FAssestId);
//        //}
//        #region Get Tracking Asset Creation
//        public static List<object> GetTrackingAssetCreation(string assetIds, string campusid, string buildingId)
//        {
//            return DAL.AssetsRepository.GetTrackingAssetCreation(assetIds, campusid, buildingId);
//        }

//        //public static List<TFloorAssets> GetAllTypesfloorassets()
//        //{
//        //    return DAL.AssetsRepository.GetAllTypesfloorassets();
//        //}
//        #endregion
//    }
//}
