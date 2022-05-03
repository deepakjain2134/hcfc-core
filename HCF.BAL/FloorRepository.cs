//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//namespace HCF.BAL
//{
//    public static class FloorRepository
//    {
//        public static int Save(Floor floor)
//        {
//            return DAL.FloorRepository.Save(floor);
//        }

//        public static bool UpdateFloor(Floor updateFloor)
//        {
//            return DAL.FloorRepository.UpdateFloor(updateFloor);
//        }

//        public static List<Floor> GetFloors()
//        {
//            return DAL.FloorRepository.GetFloors();
//        }

//        //public static List<Floor> GetFilterFloors( string floorId, int? buildingId)
//        //{
//        //    return DAL.FloorRepository.GetFilterFloors(null, buildingId);
//        //}

//        public static FloorPlan GetFloorPlans(Guid floorPlanId)
//        {
//            return DAL.FloorRepository.GetFloorPlans(floorPlanId);
//            //return DAL.FloorRepository.GetFloorPlans().FirstOrDefault(x => x.FloorPlanId == floorPlanId);

//        }

//        public static List<Floor> GetBuildingFloors(int buildingId)
//        {
//            return DAL.FloorRepository.GetAssetLocations().Where(x => x.BuildingId == buildingId).ToList();
//        }

//        public static List<Buildings> GetBuildingFloors(string buildingIds)
//        {
//            var buildings = DAL.BuildingsRepository.GetBuildingFloors();
//            if (!string.IsNullOrEmpty(buildingIds))
//            {
//                int[] strFile = buildingIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
//                buildings = buildings.Where(x => strFile.Contains(x.BuildingId)).ToList();
//            }
//            return buildings;
//        }

//        public static Floor GetFloor(int floorId)
//        {
//            return DAL.FloorRepository.GetFloors().FirstOrDefault(x => x.FloorId == floorId);
//        }

//        public static Floor GetFloorWithPlans(int floorId)
//        {
//            return DAL.FloorRepository.GetFloorPlans(floorId);
//        }

//        public static IEnumerable<Floor> GetFloorByAssets(List<int> values)
//        {
//            return DAL.FloorRepository.GetFloorByAssets(values);
//        }

//        public static List<FloorAssetStatus> GetFloorByAssetsWithStatus(string assetId, string ascIds, int insptype)
//        {
//            return DAL.FloorRepository.GetFloorByAssetsWithStatus(assetId, ascIds, insptype);
//        }

//        public static Floor GetFloorDetails(int floorId)
//        {
//            return DAL.FloorRepository.GetFloorDetails(floorId);
//        }

//        public static bool UpdateFloorPlan(int floorId, Guid? floorPlanId)
//        {
//            return DAL.FloorRepository.UpdateFloorPlan(floorId, floorPlanId);
//        }

//        public static int UpdateFloorPlan(FloorPlan item)
//        {
//            return DAL.FloorRepository.UpdateFloorPlan(item);
//        }

//        public static bool DeleteFloorPlan(Guid FloorPlanId)
//        {
//            return DAL.FloorPlanRepository.DeleteFloorPlan(FloorPlanId);
//        }
       

//        public static int UpdateThumbImageFloorPlan(FloorPlan item)
//        {
//            return DAL.FloorRepository.UpdateThumbImageFloorPlan(item);
//        }

//        /// <summary>
//        /// returns AssetId (Item1), BuildingId (Item2), FloorId (Item3), StopCode (Item4) from Asset Name, Building Name, FloorName, Stop Name
//        /// </summary>
//        public static Tuple<string, string, string, string, string, string, string> GetAssetsIds(string assetName, string buildingName, string floorName, string stopName, string assetsCategory, string assetsSubCategory)
//        {
//            return DAL.FloorRepository.GetAssetsIds(assetName, buildingName, floorName, stopName, assetsCategory, assetsSubCategory);
//        }


//        #region Drawing Viewer

//        public static int SaveDrawingViewer(DrawingViewer item)
//        {
//            return DAL.FloorPlanRepository.SaveDrawingViewer(item);
//        }
//        public static int UpdatePermitsDrawingViewer(string PermitNo, string TempPermitNo, int? ModifiedBy = 0)
//        {
//            return DAL.FloorPlanRepository.UpdatePermitsDrawingViewer(PermitNo,TempPermitNo,ModifiedBy.Value);
//        }

//        public static List<DrawingViewer> GetDrawingViewer()
//        {
//            return DAL.FloorPlanRepository.GetDrawingViewer();
//        }

//        public static List<DrawingViewer> GetDrawingViewer(Guid floorPlanId, int type)
//        {
//            return DAL.FloorPlanRepository.GetDrawingViewer().Where(x => x.FloorPlanId == floorPlanId && x.ViewerMode == type).ToList();
//        }

//        #endregion

//    }
//}
