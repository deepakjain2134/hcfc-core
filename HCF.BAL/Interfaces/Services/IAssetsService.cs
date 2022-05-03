using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.BAL.Interfaces.Services
{
    public interface IAssetsService
    {
        int AddAssets(Assets newAssets);
        int AddAssetsMapping(AssetsMapping newAssetsMapping);
        int AttachAssetToEp(int assetId, int epId, int userId);
        bool CheckExistingAssets(string Deviceno);
        bool EditFloorAssets(TFloorAssets newItem);
        List<Assets> Get();
        Assets Get(int assetId);
        List<Assets> GetAllAsset();
        List<TFloorAssets> GetAllTypesfloorassets();
        List<EPDetails> GetAssetEp(int assetId);
        TFloorAssets GetAssetEpInsHistory(int floorAssetsId, int epDetailId, int userId);
        Assets GetAssetEPs(int assetId);
        List<Assets> GetAssetFrequency();
        TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp = 0);
        List<Assets> GetAssetMaster(int userId);
        List<Assets> GetAssets(int userId);
        List<AssetType> GetAssetsByType(int userId, int typeId);
        string GetAssetsCurrentStatus(int Epdetailid, string FAssestId);
        List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId = null, int inprogress = 0, int pastDue = 0, int dueWitndays = 0, string assetIds = null, string buildingIds = null, string floorIds = null);
        DataTable GetAssetsDetailsByTmsAssetId(string tmsAssetsIds);
        List<Assets> GetAssetsMapping(string mode, string id);
        object GetAssetSubCategory(int assetId);
        List<FireExtinguisherTypes> GetAssetSubCategorySize(int ascId);
        List<Assets> GetAssetsUserMapping(string mode, string id);
        List<TFloorAssets> GetAssetsWithoutFloor();
        List<AssetType> GetAssetTypes(int userId);
        List<Category> GetCategory();
        List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate);
        TFloorAssets GetFloorAssetDescription(int floorAssetId);
        List<EPDetails> GetFloorAssetEp(int floorAssetId);
        TFloorAssets GetFloorAssetInspectionActivity(int floorAssetsId, int epDetailId, int userId);
        List<TFloorAssets> GetFloorAssets();
        IEnumerable<TFloorAssets> GetFloorAssets(Guid floorPlanId);
        List<TFloorAssets> GetFloorAssets(int floorId);
        List<TFloorAssets> GetFloorAssetsByBarcode(string barcode);
        List<TFloorAssets> GetFloorAssetsByBarcodeSearch(string barcode, int? IsAssertdevicechange = 0);
        List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds);
        List<TFloorAssets> GetTFloorAssetList();
        IEnumerable<TFloorAssets> GetTFloorAssets();
        List<object> GetTrackingAssetCreation(string assetIds, string campusid, string buildingId);
        List<Assets> InspectByFloor(string assetId, string ascIds, int buildingId, int floorId, int routeId);
        List<TFloorAssets> InventoryAssetsReport(string ids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest, string ids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        Assets ScheduleAssets(int assetId, int epDetailId);
        int UpdateAsset(Assets newAssets);
        int UpdateAssetType(int ascid, int FloorAssetsId);
        EpAssets UpdateEpAssets(EpAssets epAssets);
        List<TFloorAssets> View_getFloorAssets(int? assetId, int? buildingId, int? floorId, int? groupId);
        List<EPDetails> GetAssetsEPOnRoute(int? routeId,string assetids);
        List<EPDetails> GetBulkAssetEp(string assetId);
    }
}
