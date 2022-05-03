//using HCF.BDO;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class FireExtinguisherRepository
//    {
//        #region Fire Extinguisher Assets

//        public static List<StopMaster> GetExtinguisherAssets(int? floorId, int? inspType, int? routeId, int? assetId)
//        {
//            if(routeId.HasValue && routeId == -2)
//            {
//                return GetExtinguisherInverntoryAssets(assetId.Value, inspType.Value);
//            }
//            return DAL.FireExtinguisherRepository.GetExtinguisherAssets(floorId, inspType, routeId, assetId);
//        }


//        public static List<TFloorAssets> GetExtinguisherAssetsWithOutFloor(int assetId, int? inspType = null)
//        {
//            return DAL.FireExtinguisherRepository.GetExtinguisherAssetsWithOutFloor(assetId, inspType);
//        }

//        public static List<StopMaster> GetExtinguisherInverntoryAssets(int assetId, int inspType)
//        {
//            List<StopMaster> stops = new List<StopMaster>();
//            List<TFloorAssets> floorAssets = GetExtinguisherAssetsWithOutFloor(assetId, inspType);
//            foreach (TFloorAssets item in floorAssets)
//            {
//                StopMaster objStop = new StopMaster();
//                objStop.TfloorAssets.Add(item);
//                stops.Add(objStop);
//            }
//            return stops;
//        }

//        public static TFloorAssets GetExtinguisherAsset(string tagNo , string stopcode , string assetId)
//        {            
//            return DAL.FireExtinguisherRepository.GetExtinguisherAssets(tagNo, stopcode, assetId);
//        }

//        #endregion

//        #region floorAssets

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newfloorAssets"></param>
//        /// <returns></returns>
//        public static bool AddFloorAssetsToLocation(TFloorAssets newfloorAssets)
//        {
//            return DAL.FloorAssetRepository.AddFloorAssetsToStop(newfloorAssets);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetId"></param>
//        /// <returns></returns>
//        public static bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId)
//        {
//            return DAL.FireExtinguisherRepository.RemoveFloorAssetsFromCurrentLocation(floorAssetId);
//        }

//        #endregion

//        #region Inspection

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="objtinspectionActivity"></param>
//        /// <returns></returns>
//        public static int SaveInspection(TInspectionActivity objtinspectionActivity)
//        {
//            return DAL.FireExtinguisherRepository.SaveInspection(objtinspectionActivity);
//        }

//        #endregion

//        #region Asset Insp Frequency

//        public static List<FrequencyMaster> GetAssetInspFrequency(int assetId)
//        {
//            return DAL.FireExtinguisherRepository.GetAssetInspFrequency(assetId);
//        }

//        #endregion

//        #region InspResult

//        public static List<InspResult> GetInspResult()
//        {
//            return DAL.FireExtinguisherRepository.GetInspResult();
//        }

//        #endregion

//        #region InspStatus

//        public static List<InspStatus> GetInspStatus()
//        {
//            return DAL.FireExtinguisherRepository.GetInspStatus();
//        }

//        #endregion

//        #region Reports

//        public static List<TFloorAssets> Get_ExtinguisherAssetsReports(int assetId, int? buildingId, int? floorId, int? inspType)
//        {
//            return DAL.FireExtinguisherRepository.Get_ExtinguisherAssetsReports(assetId, buildingId, floorId, inspType);
//        }


//        public static List<TFloorAssets> Get_ExtinguisherAssetsReport(int year, int? routNo, int? assetId, int? epdetailId)
//        {
//            return DAL.FireExtinguisherRepository.Get_ExtinguisherAssetsReport(year, routNo, assetId, epdetailId);
//        }
//        #endregion
//    }
//}
