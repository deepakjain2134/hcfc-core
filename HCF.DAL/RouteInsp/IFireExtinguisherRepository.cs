using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFireExtinguisherRepository
    {
        List<FrequencyMaster> GetAssetInspFrequency(int assetId);
        List<StopMaster> GetExtinguisherAssets(int? floorId, int? inspType, int? routeId, int? assetId);
        TFloorAssets GetExtinguisherAssets(string tagNo, string stopcode, string assetId);
        List<TFloorAssets> GetExtinguisherAssetsWithOutFloor(int assetId, int? inspType);
        List<InspResult> GetInspResult();
        List<InspStatus> GetInspStatus();
        List<TFloorAssets> Get_ExtinguisherAssetsReport(int year, int? routNo, int? assetId, int? epdetailId);
        List<TFloorAssets> Get_ExtinguisherAssetsReports(int assetId, int? buildingId, int? floorId, int? inspType);
        bool RemoveFloorAssetsFromCurrentLocation(int floorAssetId);
        int SaveInspection(TInspectionActivity objtinspectionActivity);
    }
}