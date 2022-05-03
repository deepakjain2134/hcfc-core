//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class AssetTypeRepository
//    {
       

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newAssetsType"></param>
//        /// <returns></returns>
//        public static int Save(AssetType newAssetsType)
//        {
//            return DAL.AssetTypeRepository.Save(newAssetsType);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<AssetType> GetAssetsType()
//        {
//            return DAL.AssetTypeRepository.GetAssestsType(4);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<AssetType> GetAssetTypeMaster(int userId)
//        {
//            return DAL.AssetTypeRepository.AssetTypeMaster(userId);
//        }


//        public static List<AssetType> GetAssetMaster()
//        {
//            return DAL.AssetTypeRepository.GetAssetMaster();
//        }  

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public static List<AssetType> GetAssetsByCategory(int userId, int type)
//        {
//            var assetTypes = DAL.AssetTypeRepository.GetAssestsType(userId).Where(x => x.IsActive).ToList();
//            //var floors = DAL.FloorRepository.GetAssetLocations();
//            var floorAssetsMain = DAL.FloorAssetRepository.GetFloorAsset();
//            var activity = DAL.InspectionActivityRepository.GetAssetInspectionActivity().Where(x=>x.IsCurrent).ToList();

//            if (activity.Count > 0)
//            {
//                foreach (var floorAssets in floorAssetsMain)
//                {
//                    floorAssets.TInspectionActivity =
//                        activity.Where(x => x.FloorAssetId == floorAssets.FloorAssetsId).ToList();
//                }

//            }

//            foreach (var assetType in assetTypes)
//            {
//                foreach (var assets in assetType.Assets.Where(x => x.IsActive).ToList())
//                {
//                    var assetLists = floorAssetsMain.Where(x => x.AssetId == assets.AssetId).ToList();
//                    //foreach (var floorAsset in assetLists)
//                    //{

//                    //    //if (floorAsset.FloorId.HasValue)
//                    //    //    floorAsset.Floor = floors.SingleOrDefault(x => x.FloorId == floorAsset.FloorId);

//                    //    //floorAsset.EPDetails = SetAssetEPs(activity, floorAsset);
//                    //}
//                    assets.TFloorAssets = assetLists;
//                }
//            }
//            return assetTypes.Where(x => x.Assets.Count > 0).ToList();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="activity"></param>
//        /// <param name="floorAsset"></param>
//        /// <returns></returns>
//        private static List<EPDetails> SetAssetEPs(List<TInspectionActivity> activity, TFloorAssets floorAsset)
//        {
//            var epDetails = EpRepository.GetEpByAssets(floorAsset.AssetId.Value);
//            //epDetails = EPdetails;
//            foreach (var ep in epDetails)
//            {
//                ep.TInspectionActivity = activity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId 
//                                                             && x.EPDetailId == ep.EPDetailId && x.IsCurrent == true).ToList();
//            }
//            return epDetails;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public static List<AssetType> GetInternalAssetsByType(int userId)
//        {
//            return DAL.AssetTypeRepository.GetInternalAssetsByType(userId);
//        }


//        #region Offline Assets

//        public static HttpResponseMessage GetOfflineAssets()
//        {
//            return DAL.AssetTypeRepository.GetOfflineAssets();
//        }

//        #endregion
//    }
//}
