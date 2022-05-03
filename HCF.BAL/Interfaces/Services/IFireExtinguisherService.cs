using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFireExtinguisherService
    {
        bool AddFloorAssetsToLocation(TFloorAssets newfloorAssets);
        List<FrequencyMaster> GetAssetInspFrequency(int assetId);
        TFloorAssets GetExtinguisherAsset(string tagNo, string stopcode, string assetId);
        List<StopMaster> GetExtinguisherAssets(int? floorId, int? inspType, int? routeId, int? assetId);
        List<TFloorAssets> GetExtinguisherAssetsWithOutFloor(int assetId, int? inspType = null);
        List<StopMaster> GetExtinguisherInverntoryAssets(int assetId, int inspType);
        List<InspResult> GetInspResult();
        List<InspStatus> GetInspStatus();
        List<TFloorAssets> Get_ExtinguisherAssetsReport(int year, int? routNo, int? assetId, int? epdetailId);
        List<TFloorAssets> Get_ExtinguisherAssetsReports(int assetId, int? buildingId, int? floorId, int? inspType);
        bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId);
        int SaveInspection(TInspectionActivity objtinspectionActivity);
    }
}