using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IAssetsSubCategoryRepository
    {        
        List<AssetsSubCategory> GetAssetsSubCategory();
        List<AssetsSubCategory> GetAssetsSubCategory(int assetId);
        List<FireExtinguisherTypes> GetAssetSubCategorySize(int? ascId);
        Assets ScheduleAssets(int assetId, int epDetailId);
    }
}