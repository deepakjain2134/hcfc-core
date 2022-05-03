using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class FireExtinguisherService : IFireExtinguisherService
    {
        #region ctor

        private readonly IFireExtinguisherRepository _fireExtinguisherRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;

        public FireExtinguisherService(IFireExtinguisherRepository fireExtinguisherRepository, IFloorAssetRepository floorAssetRepository)
        {
            _floorAssetRepository = floorAssetRepository;
            _fireExtinguisherRepository = fireExtinguisherRepository;

        }

        #endregion

        #region Fire Extinguisher Assets

        public List<StopMaster> GetExtinguisherAssets(int? floorId, int? inspType, int? routeId, int? assetId)
        {
            if(routeId.HasValue && routeId == -2)
            {
                return GetExtinguisherInverntoryAssets(assetId.Value, inspType.Value);
            }
            return _fireExtinguisherRepository.GetExtinguisherAssets(floorId, inspType, routeId, assetId);
        }


        public List<TFloorAssets> GetExtinguisherAssetsWithOutFloor(int assetId, int? inspType = null)
        {
            return _fireExtinguisherRepository.GetExtinguisherAssetsWithOutFloor(assetId, inspType);
        }

        public  List<StopMaster> GetExtinguisherInverntoryAssets(int assetId, int inspType)
        {
            List<StopMaster> stops = new List<StopMaster>();
            List<TFloorAssets> floorAssets = GetExtinguisherAssetsWithOutFloor(assetId, inspType);
            foreach (TFloorAssets item in floorAssets)
            {
                StopMaster objStop = new StopMaster();
                objStop.TfloorAssets.Add(item);
                stops.Add(objStop);
            }
            return stops;
        }

        public  TFloorAssets GetExtinguisherAsset(string tagNo , string stopcode , string assetId)
        {            
            return _fireExtinguisherRepository.GetExtinguisherAssets(tagNo, stopcode, assetId);
        }

        #endregion

        #region floorAssets

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newfloorAssets"></param>
        /// <returns></returns>
        public  bool AddFloorAssetsToLocation(TFloorAssets newfloorAssets)
        {
            return _floorAssetRepository.AddFloorAssetsToStop(newfloorAssets);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        public  bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId)
        {
            return _fireExtinguisherRepository.RemoveFloorAssetsFromCurrentLocation(floorAssetId);
        }

        #endregion

        #region Inspection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objtinspectionActivity"></param>
        /// <returns></returns>
        public  int SaveInspection(TInspectionActivity objtinspectionActivity)
        {
            return _fireExtinguisherRepository.SaveInspection(objtinspectionActivity);
        }

        #endregion

        #region Asset Insp Frequency

        public  List<FrequencyMaster> GetAssetInspFrequency(int assetId)
        {
            return _fireExtinguisherRepository.GetAssetInspFrequency(assetId);
        }

        #endregion

        #region InspResult

        public  List<InspResult> GetInspResult()
        {
            return _fireExtinguisherRepository.GetInspResult();
        }

        #endregion

        #region InspStatus

        public  List<InspStatus> GetInspStatus()
        {
            return _fireExtinguisherRepository.GetInspStatus();
        }

        #endregion

        #region Reports

        public  List<TFloorAssets> Get_ExtinguisherAssetsReports(int assetId, int? buildingId, int? floorId, int? inspType)
        {
            return _fireExtinguisherRepository.Get_ExtinguisherAssetsReports(assetId, buildingId, floorId, inspType);
        }


        public  List<TFloorAssets> Get_ExtinguisherAssetsReport(int year, int? routNo, int? assetId, int? epdetailId)
        {
            return _fireExtinguisherRepository.Get_ExtinguisherAssetsReport(year, routNo, assetId, epdetailId);
        }
        #endregion
    }
}
