//using HCF.BDO;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public class LocationRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newLocation"></param>
//        /// <returns></returns>
//        public static bool AddFloorAssetsLocation(TFloorAssetsLocations newLocation)
//        {
//            return DAL.LocationRepository.AddFloorAssetsLocation(newLocation);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newLocation"></param>
//        /// <returns></returns>
//        public static bool UpdateFloorAssetsLocation(TFloorAssetsLocations newLocation)
//        {
//            return DAL.LocationRepository.UpdateFloorAssetsLocation(newLocation);
//        }


//        #region Location Master

//        public static int SaveLocation(StopMaster newStopMaster)
//        {
//            return DAL.LocationRepository.SaveLocation(newStopMaster);
//        }

//        public static StopMaster GetStopByCode(string codeName)
//        {
//            return DAL.LocationRepository.GetStopByCode(codeName);
//        }

//        public static List<TFloorAssets> GetStopAssets(int stopId)
//        {
//            return DAL.LocationRepository.GetStopAssets(stopId);
//        }
//        public static List<StopMaster> GetLocationsMaster(int? locationMasterId)
//        {
//            return DAL.LocationRepository.GetLocationsMaster(locationMasterId);
//        }

//        public static int UpdateLocation(StopMaster stopMaster)
//        {
//            return DAL.LocationRepository.UpdateLocation(stopMaster);
//        }

//        public static List<StopMaster> GetStopNotExistInFe(int assetId,int floorId)
//        {
//            return DAL.LocationRepository.GetStopNotExistInFe(assetId, floorId);
//        }

//        #endregion



//        #region Route Master

//        public static int SaveRoute(RouteMaster newRouteMaster , string locationsId)
//        {
//            int routeId = DAL.LocationRepository.SaveRoute(newRouteMaster);
//            if (!string.IsNullOrEmpty(locationsId) || string.IsNullOrEmpty(locationsId))
//            {
//                StopsRouteMapping item = new StopsRouteMapping {RouteId = routeId, IsActive = newRouteMaster.IsActive};
//                SaveLocationRouteMapping(item, locationsId);
//            }
//            return routeId;
//        }

//        public static List<RouteMaster> GetRouteMaster(int? routeId)
//        {
//            return DAL.LocationRepository.GetRouteMaster(routeId);
//        }

//        public static List<RouteMaster> GetRouteByAsset(int assetId)
//        {
//            return DAL.LocationRepository.GetRouteByAsset(assetId);
//        }
      
        
//        #endregion

//        #region Location Route Mapping

//        public static int SaveLocationRouteMapping(StopsRouteMapping newStopsRouteMapping, string locationsId)
//        {
//            return DAL.LocationRepository.SaveLocationRouteMapping(newStopsRouteMapping, locationsId);
//        }

//        public static List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId)
//        {
//            return DAL.LocationRepository.GetLocationrouteMapping(floorId ,routeId);
//        }

//        public static List<StopsRouteMapping> GetLocationRouteMapping(int? routeId)
//        {
//            return DAL.LocationRepository.GetLocationrouteMapping(null,routeId);
//        }

//        public static List<RouteMaster> GetRouteByFloor(int floorId)
//        {
//            return DAL.LocationRepository.GetRouteByFloor(floorId);
//        }



//        #endregion

//        #region FileDetails
//        public static List<InspectionEPDocs> GetFileDetailsById(int FileId)
//        {
//            return DAL.LocationRepository.GetFileDetailsById(FileId);
//        }
//        #endregion
//    }
//}
