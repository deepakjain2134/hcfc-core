using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFloorAssetService
    {
        TFloorAssets AddFloorAssets(TFloorAssets newItem);
        bool AddFloorAssetsToStop(TFloorAssets newfloorAssets);
        TFloorAssets AddImportedFloorAssets(TFloorAssets newItem);
        List<Buildings> GetAssetsBuilding(int assetId);
        List<TFloorAssets> GetAssetsByPrefix(string prefix);
        TFloorAssets GetAssetsOpenItems(int floorAssetId);
        TFloorAssets GetFeFloorAssetInspActivity(int floorAssetId, int userId);
        TFloorAssets GetFloorAsset(int floorAssetId);
        TFloorAssets GetFloorAssetByTmsAssetId(int tmsAssetid);
        TFloorAssets GetFloorAssetDescription(int floorAssetId);
        List<TFloorAssets> GetFloorAssets();
        List<TFloorAssets> GetFloorAssetsByAssetType(int typeId, int userId);
        List<TFloorAssets> GetFloorAssetsEps(int typeId, int floorAssetid, int userId);
        List<TFloorAssets> GetTFloorAssets();
        List<TFloorAssets> GetTFloorAssets(int assetId, int routId);
        bool Movefloorassets(TFloorAssets newItem, string mode);
        void SaveLocation(FloorShapes newItem);
        int UpdateInspectionDate(int floorAssetId, DateTime lastPmDate, string referenceNo);
        bool UpdateTfloorassetstatus(int floorId, int assetId, string statusCode);
    }
}