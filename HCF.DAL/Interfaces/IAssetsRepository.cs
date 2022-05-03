using HCF.BDO;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IAssetsRepository
    {
        List<Assets> ConvertToAssets(DataTable dt);
        int AddAssetsMapping(AssetsMapping newAssetsUserMapping);
        int AttachAssetToEp(int assetId, int epId, int userId);
        List<Assets> Get();
        List<Assets> GetAllAsset();
        List<TFloorAssets> GetAllTypesfloorassets();
        //DataTable GetAssestsDataCSVMatch();
        Assets GetAsset(int assetId);
        Assets GetAssetEPs(int assetId);
        List<Assets> GetAssetEPs(string assetIds);
        List<Assets> GetAssetFrequency();
        List<Assets> GetAssetMaster(int userId);
        List<Assets> GetAssets(int userId);
        //List<Assets> GetAssetsByFloor(int floorId);
        string GetAssetsCurrentStatus(int Epdetailid, string FAssestId);
        //List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId, int Inprogress = 0, int pastDue = 0, int dueWitndays = 0, string assetIds = null, string buildingIds = null, string floorIds = null);
        DataTable GetAssetsDetailsByTmsAssetId(string tmsAssetsIds);
        List<Assets> GetAssetsMapping(string mode, string id);
        List<Assets> GetAssetsUserMapping(string mode, string id);
        List<Assets> GetEpAssets(int epDetailId);
        List<object> GetTrackingAssetCreation(string assetIds, string campusid, string buildingId);
        //DataSet SaveCSVFilterNewAssets(string xmlRecord, int userId);
        int SaveFloorAssetsInspection(FloorAssetsInspection newData);
        int UpdateAsset(Assets newAssets);
        //bool UpdateExistAssetsFromCSV(string xmlRecord, int userId);
    }
}