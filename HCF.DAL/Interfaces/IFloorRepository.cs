using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IFloorRepository
    {
        List<Floor> GetAssetLocations();
        Floor ConvertToFloor(DataTable dataTable);
        List<FloorPlan> FloorPlans(int floorId);
        List<FloorShapes> FloorShapes(int floorId);
        //List<Floor> GetAssetLocations();
        Tuple<string, string, string, string, string, string, string> GetAssetsIds(string assetName, string buildingName, string floorName, string stopName, string assetsCategory, string assetsSubCategory);
        Floor GetFloor(int floorId);
        IEnumerable<Floor> GetFloorByAssets(IEnumerable<int> values);
        List<BDO.FloorAssetStatus> GetFloorByAssetsWithStatus(string assetId, string ascIds, int insptype);
        Floor GetFloorDescription(int floorId);
        Floor GetFloorDetails(int floorId);
        List<FloorPlan> GetFloorPlans();
        FloorPlan GetFloorPlans(Guid? floorPlanId);
        Floor GetFloorPlans(int floorId);
        List<Floor> GetFloors();
        List<Floor> GetFloorsByFloorType(int floorTypeId, int? CheckFileEmpty);
        DrawingpathwayFiles GetUploadedDrawing(string fileId);
        int Save(Floor newFloor);
        bool UpdateFloor(Floor updateFloor);
        int UpdateFloorPlan(FloorPlan item);
        bool UpdateFloorPlan(int floorId, Guid? floorPlanId);
        int UpdateThumbImageFloorPlan(FloorPlan item);
        List<Wing> Wing(int floorId);

        public List<Floor> GetFloorsList();
    }
}