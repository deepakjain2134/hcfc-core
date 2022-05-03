using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFloorService
    {
        bool DeleteFloorPlan(Guid FloorPlanId);
        Tuple<string, string, string, string, string, string, string> GetAssetsIds(string assetName, string buildingName, string floorName, string stopName, string assetsCategory, string assetsSubCategory);
        List<Floor> GetBuildingFloors(int buildingId);
        List<Buildings> GetBuildingFloors(string buildingIds);
        List<DrawingViewer> GetDrawingViewer();
        List<DrawingViewer> GetDrawingViewer(Guid floorPlanId, int type);
        Floor GetFloor(int floorId);
        IEnumerable<Floor> GetFloorByAssets(List<int> values);
        List<FloorAssetStatus> GetFloorByAssetsWithStatus(string assetId, string ascIds, int insptype);
        Floor GetFloorDetails(int floorId);
        FloorPlan GetFloorPlans(Guid floorPlanId);
        List<Floor> GetFloors();
        Floor GetFloorWithPlans(int floorId);
        int Save(Floor floor);
        int SaveDrawingViewer(DrawingViewer item);
        bool UpdateFloor(Floor updateFloor);
        int UpdateFloorPlan(FloorPlan item);
        bool UpdateFloorPlan(int floorId, Guid? floorPlanId);
        int UpdatePermitsDrawingViewer(string PermitNo, string TempPermitNo, int? ModifiedBy = 0);
        int UpdateThumbImageFloorPlan(FloorPlan item);
    }
}