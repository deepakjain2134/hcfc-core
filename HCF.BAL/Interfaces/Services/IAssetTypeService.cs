using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IAssetTypeService
    {
        List<AssetType> GetAssetMaster();
        List<AssetType> GetAssetsByCategory(int userId, int type);
        List<AssetType> GetAssetsType();
        List<AssetType> GetAssetTypeMaster(int userId);
        List<AssetType> GetInternalAssetsByType(int userId);
        HttpResponseMessage GetOfflineAssets();
        int Save(AssetType newAssetsType);
    }
}