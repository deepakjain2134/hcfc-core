using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IAssetTypeRepository
    {
        List<AssetType> AssetTypeMaster(int userId);
        List<AssetType> GetAssestsType(int userId);
        List<AssetType> GetAssetMaster();
        List<AssetType> GetAssetsByType(int userId, int assetType);
        List<AssetType> GetInternalAssetsByType(int userId);
        HttpResponseMessage GetOfflineAssets();
        int Save(AssetType newAssetsType);
    }
}