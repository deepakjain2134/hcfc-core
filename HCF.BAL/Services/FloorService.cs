using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HCF.BAL
{
    public  class FloorService : IFloorService
    {
        private readonly IBuildingsRepository _buildingsRepository;
        //private readonly IFloorService _floorService;
        private readonly IFloorPlanRepository _floorPlanRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IFloorRepository _floorRepository;

        public FloorService(IFloorRepository floorRepository,IBuildingsRepository buildingsRepository, 
            //IFloorService floorService,
            IFloorPlanRepository floorPlanRepository,
            IFloorAssetRepository floorAssetRepository)
        {
            _floorRepository = floorRepository;
               _floorAssetRepository = floorAssetRepository;
            _floorPlanRepository = floorPlanRepository;
            //_floorService = floorService;
            _buildingsRepository = buildingsRepository;

        }
        public  int Save(Floor floor)
        {
            return _floorRepository.Save(floor);
        }

        public  bool UpdateFloor(Floor updateFloor)
        {
            return _floorRepository.UpdateFloor(updateFloor);
        }

        public  List<Floor> GetFloors()
        {
            return _floorRepository.GetFloors();
        }

        //public  List<Floor> GetFilterFloors( string floorId, int? buildingId)
        //{
        //    return _floorService.GetFilterFloors(null, buildingId);
        //}

        public  FloorPlan GetFloorPlans(Guid floorPlanId)
        {
            return _floorRepository.GetFloorPlans(floorPlanId);
            //return _floorService.GetFloorPlans().FirstOrDefault(x => x.FloorPlanId == floorPlanId);

        }

        public  List<Floor> GetBuildingFloors(int buildingId)
        {
            return _floorRepository.GetAssetLocations().Where(x => x.BuildingId == buildingId).ToList();
        }

        public  List<Buildings> GetBuildingFloors(string buildingIds)
        {
            var buildings = _buildingsRepository.GetBuildingFloors();
            if (!string.IsNullOrEmpty(buildingIds))
            {
                int[] strFile = buildingIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                buildings = buildings.Where(x => strFile.Contains(x.BuildingId)).ToList();
            }
            return buildings;
        }

        public  Floor GetFloor(int floorId)
        {
            return _floorRepository.GetFloors().FirstOrDefault(x => x.FloorId == floorId);
        }

        public Floor GetFloorWithPlans(int floorId)
        {
            return _floorRepository.GetFloorPlans(floorId);
        }

        public  IEnumerable<Floor> GetFloorByAssets(List<int> values)
        {
            return _floorRepository.GetFloorByAssets(values);
        }

        public  List<BDO.FloorAssetStatus> GetFloorByAssetsWithStatus(string assetId, string ascIds, int insptype)
        {
            return _floorRepository.GetFloorByAssetsWithStatus(assetId, ascIds, insptype);
        }

        public Floor GetFloorDetails(int floorId)
        {
            var floor = _floorRepository.GetFloorDetails(floorId);
            if (floor != null)
                floor.TFloorAssets = _floorAssetRepository.GetFloorAssets(floorId);
            return floor;
        }

        public  bool UpdateFloorPlan(int floorId, Guid? floorPlanId)
        {
            return _floorRepository.UpdateFloorPlan(floorId, floorPlanId);
        }

        public  int UpdateFloorPlan(FloorPlan item)
        {
            return _floorRepository.UpdateFloorPlan(item);
        }

        public  bool DeleteFloorPlan(Guid FloorPlanId)
        {
            return _floorPlanRepository.DeleteFloorPlan(FloorPlanId);
        }
       

        public  int UpdateThumbImageFloorPlan(FloorPlan item)
        {
            return _floorRepository.UpdateThumbImageFloorPlan(item);
        }

        /// <summary>
        /// returns AssetId (Item1), BuildingId (Item2), FloorId (Item3), StopCode (Item4) from Asset Name, Building Name, FloorName, Stop Name
        /// </summary>
        public  Tuple<string, string, string, string, string, string, string> GetAssetsIds(string assetName, string buildingName, string floorName, string stopName, string assetsCategory, string assetsSubCategory)
        {
            return _floorRepository.GetAssetsIds(assetName, buildingName, floorName, stopName, assetsCategory, assetsSubCategory);
        }


        #region Drawing Viewer

        public  int SaveDrawingViewer(DrawingViewer item)
        {
            return _floorPlanRepository.SaveDrawingViewer(item);
        }
        public  int UpdatePermitsDrawingViewer(string PermitNo, string TempPermitNo, int? ModifiedBy = 0)
        {
            return _floorPlanRepository.UpdatePermitsDrawingViewer(PermitNo,TempPermitNo,ModifiedBy.Value);
        }

        public  List<DrawingViewer> GetDrawingViewer()
        {
            return _floorPlanRepository.GetDrawingViewer();
        }

        public  List<DrawingViewer> GetDrawingViewer(Guid floorPlanId, int type)
        {
            return _floorPlanRepository.GetDrawingViewer().Where(x => x.FloorPlanId == floorPlanId && x.ViewerMode == type).ToList();
        }

        #endregion

    }
}
