using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        public bool AddFloorAssetsLocation(TFloorAssetsLocations newLocation)
        {
            return _locationRepository.AddFloorAssetsLocation(newLocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        public bool UpdateFloorAssetsLocation(TFloorAssetsLocations newLocation)
        {
            return _locationRepository.UpdateFloorAssetsLocation(newLocation);
        }


        #region Location Master

        public int SaveLocation(StopMaster newStopMaster)
        {
            return _locationRepository.SaveLocation(newStopMaster);
        }

        public StopMaster GetStopByCode(string codeName)
        {
            return _locationRepository.GetStopByCode(codeName);
        }

        public List<TFloorAssets> GetStopAssets(int stopId)
        {
            return _locationRepository.GetStopAssets(stopId);
        }
        public List<StopMaster> GetLocationsMaster(int? locationMasterId)
        {
            return _locationRepository.GetLocationsMaster(locationMasterId);
        }

        public int UpdateLocation(StopMaster stopMaster)
        {
            return _locationRepository.UpdateLocation(stopMaster);
        }

        public List<StopMaster> GetStopNotExistInFe(int assetId, int floorId)
        {
            return _locationRepository.GetStopNotExistInFe(assetId, floorId);
        }

        #endregion



        #region Route Master

        public int SaveRoute(RouteMaster newRouteMaster, string locationsId)
        {
            int routeId = _locationRepository.SaveRoute(newRouteMaster);
            if (!string.IsNullOrEmpty(locationsId) || string.IsNullOrEmpty(locationsId))
            {
                StopsRouteMapping item = new StopsRouteMapping { RouteId = routeId, IsActive = newRouteMaster.IsActive };
                SaveLocationRouteMapping(item, locationsId);
            }
            return routeId;
        }

        public List<RouteMaster> GetRouteMaster(int? routeId)
        {
            return _locationRepository.GetRouteMaster(routeId);
        }

        public List<RouteMaster> GetRouteByAsset(int assetId)
        {
            return _locationRepository.GetRouteByAsset(assetId);
        }


        #endregion

        #region Location Route Mapping

        public int SaveLocationRouteMapping(StopsRouteMapping newStopsRouteMapping, string locationsId)
        {
            return _locationRepository.SaveLocationRouteMapping(newStopsRouteMapping, locationsId);
        }

        public List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId)
        {
            return _locationRepository.GetLocationrouteMapping(floorId, routeId);
        }

        public List<StopsRouteMapping> GetLocationRouteMapping(int? routeId)
        {
            return _locationRepository.GetLocationrouteMapping(null, routeId);
        }

        public List<RouteMaster> GetRouteByFloor(int floorId)
        {
            return _locationRepository.GetRouteByFloor(floorId);
        }



        #endregion

        #region FileDetails
        public List<InspectionEPDocs> GetFileDetailsById(int FileId)
        {
            return _locationRepository.GetFileDetailsById(FileId);
        }
        #endregion


        #region  Location Groups

        public int SaveLocationGroup(LocationGroups objLocationGroups)
        {
            return _locationRepository.SaveLocationGroup(objLocationGroups);
        }


        public List<LocationGroups> GetLocationGroup()
        {
            return _locationRepository.GetLocationGroup();
        }

        public int SaveGroupLocationsDetails(LocationGroupDetails objlocationGroupDetails)
        {
            return _locationRepository.SaveGroupLocationsDetails(objlocationGroupDetails);
        }

        public List<Buildings> GetGroupLocationsDetails(int? locationGroupId)
        {
            return _locationRepository.GetGroupLocationsDetails(locationGroupId);
        }

        public List<Buildings> GetGroupBuildings()
        {
            return _locationRepository.GetGroupBuildings();
        }

        #endregion
    }
}
