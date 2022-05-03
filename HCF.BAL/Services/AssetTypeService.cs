using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class AssetTypeService : IAssetTypeService
    {
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IEpRepository _epRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;

        public AssetTypeService(IAssetTypeRepository assetTypeRepository, IInspectionActivityRepository inspectionActivityRepository, IEpRepository epRepository,IFloorAssetRepository floorAssetRepository)
        {
            _inspectionActivityRepository = inspectionActivityRepository;
            _floorAssetRepository = floorAssetRepository;
            _assetTypeRepository = assetTypeRepository;
            _epRepository = epRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newAssetsType"></param>
        /// <returns></returns>
        public  int Save(AssetType newAssetsType)
        {
            return _assetTypeRepository.Save(newAssetsType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<AssetType> GetAssetsType()
        {
            return _assetTypeRepository.GetAssestsType(4);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<AssetType> GetAssetTypeMaster(int userId)
        {
            return _assetTypeRepository.AssetTypeMaster(userId);
        }


        public  List<AssetType> GetAssetMaster()
        {
            return _assetTypeRepository.GetAssetMaster();
        }  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public  List<AssetType> GetAssetsByCategory(int userId, int type)
        {
            var assetTypes = _assetTypeRepository.GetAssestsType(userId).Where(x => x.IsActive).ToList();
            //var floors = DAL.FloorRepository.GetAssetLocations();
            var floorAssetsMain = _floorAssetRepository.GetFloorAsset();
            var activity = _inspectionActivityRepository.GetAssetInspectionActivity().Where(x=>x.IsCurrent).ToList();

            if (activity.Count > 0)
            {
                foreach (var floorAssets in floorAssetsMain)
                {
                    floorAssets.TInspectionActivity =
                        activity.Where(x => x.FloorAssetId == floorAssets.FloorAssetsId).ToList();
                }

            }

            foreach (var assetType in assetTypes)
            {
                foreach (var assets in assetType.Assets.Where(x => x.IsActive).ToList())
                {
                    var assetLists = floorAssetsMain.Where(x => x.AssetId == assets.AssetId).ToList();
                    //foreach (var floorAsset in assetLists)
                    //{

                    //    //if (floorAsset.FloorId.HasValue)
                    //    //    floorAsset.Floor = floors.SingleOrDefault(x => x.FloorId == floorAsset.FloorId);

                    //    //floorAsset.EPDetails = SetAssetEPs(activity, floorAsset);
                    //}
                    assets.TFloorAssets = assetLists;
                }
            }
            return assetTypes.Where(x => x.Assets.Count > 0).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="floorAsset"></param>
        /// <returns></returns>
        private  List<EPDetails> SetAssetEPs(List<TInspectionActivity> activity, TFloorAssets floorAsset)
        {
            var epDetails = _epRepository.GetEpByAssets(floorAsset.AssetId.Value);
            //epDetails = EPdetails;
            foreach (var ep in epDetails)
            {
                ep.TInspectionActivity = activity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId 
                                                             && x.EPDetailId == ep.EPDetailId && x.IsCurrent == true).ToList();
            }
            return epDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<AssetType> GetInternalAssetsByType(int userId)
        {
            return _assetTypeRepository.GetInternalAssetsByType(userId);
        }


        #region Offline Assets

        public  HttpResponseMessage GetOfflineAssets()
        {
            return _assetTypeRepository.GetOfflineAssets();
        }

        #endregion
    }
}
