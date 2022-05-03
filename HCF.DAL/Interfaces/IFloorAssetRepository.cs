using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IFloorAssetRepository
    {
        TFloorAssets BindFloorAsset(DataRow row, List<InspectionGroup> inspectiongroup = null);
        List<TFloorAssets> BindFloorAssets(DataTable dt);
        List<TFloorAssets> GetTFloorAssetslist();
        int AddFloorAssets(TFloorAssets newFloorAsset);
        bool AddFloorAssetsToStop(TFloorAssets newFloorAsset);
        DataSet AddImportedFloorAssets(TFloorAssets newFloorAsset);
        bool CheckExistingAssets(string Deviceno);
        TFloorAssets GetAssetEpInsHistory(int floorAssetsId, int ePdetailId, int userId);
        TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp);
        List<Buildings> GetAssetsBuilding(int assetId);
        List<TFloorAssets> GetAssetsByPrefix(string prefix);
        TFloorAssets GetAssetsOpenItems(int floorAssetsId);
        List<TFloorAssets> GetAssetsReports(string assetids);
        List<TFloorAssets> GetAssetsWithoutFloor();
        TFloorAssets GetFeFloorAssetInspActivity(int floorAssetId, int userId);
        List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate);
        List<TFloorAssets> GetFloorAsset();
        List<TFloorAssets> GetFloorAsset(Guid floorPlanId);
        TFloorAssets GetFloorAsset(int floorAssetId);
        List<TFloorAssets> GetFloorAssetByAsset(Request objRequest, string ids);
        TFloorAssets GetFloorAssetByTmsAssetId(int tmsAssetId);
        TFloorAssets GetFloorAssetDescription(int floorAssetId);
        TFloorAssets GetFloorAssetInspectionActivity(int floorAssetsId, int epDetailId, int userId);
        List<TFloorAssets> GetFloorAssets();
        List<TFloorAssets> GetFloorAssets(Guid floorPlanId);
        List<TFloorAssets> GetFloorAssets(int floorId);
        List<TFloorAssets> GetFloorAssetsByAssetType(int? assetTypeId, int userId, int? floorAssetId);
        List<TFloorAssets> GetFloorAssetsbyBarcode(string barcode);
        List<TFloorAssets> GetFloorAssetsbyBarcodeSearch(string barcode, int? IsAssetDevice = 0);
        List<TFloorAssets> GetFloorAssetsEps(int? assetTypeId, int userId, int? floorAssetId);
        List<TFloorAssets> GetFloorAssetsForInspections(string assetId, string ascIds);
        List<Inspection> GetInspection(int epDetailId, int floorAssetsId, bool isStepsOnReport, DataTable dt);
        List<TFloorAssets> GetTFloorAssetByAssets(string ids);
        List<TFloorAssets> GetTFloorAssets();
        List<TFloorAssets> GetTFloorAssets(int assetId, int routId);
        //List<TFloorAssets> GetTFloorAssetslist();
        List<Assets> InspectByFloor(string assetId, string ascIds, int buildingId, int floorId, int routeId);
        int InspectionCount(int floorAssetId, int epDetailId);
        List<TFloorAssets> InventoryAssetsReport(string ids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest, string ids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId);
        int UpdateAssetType(int ascid, int FloorAssetsId);
        bool UpdateFloorAssets(TFloorAssets floorAsset);
        int UpdateInspectionDate(int floorAssetId, DateTime lastPmDate, string referenceNo);
        bool UpdateTfloorassetstatus(int floorId, int assetId, string statusCode);
        List<TFloorAssets> View_getFloorAssets(int? assetId, int? buildingId, int? floorId, int? groupId);
    }
}