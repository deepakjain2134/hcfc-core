using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ILocationRepository
    {
        bool AddFloorAssetsLocation(TFloorAssetsLocations newLocation);
        List<InspectionEPDocs> GetFileDetailsById(int fileId);
        List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId);
        List<TFloorAssetsLocations> GetLocations();
        List<TFloorAssetsLocations> GetLocations(bool status);
        List<StopMaster> GetLocationsMaster(int? locationMasterId, string stopCode = null);
        List<RouteMaster> GetRouteByAsset(int assetId);
        List<RouteMaster> GetRouteByFloor(int? floorId);
        List<RouteMaster> GetRouteMaster(int? routeId);
        List<TFloorAssets> GetStopAssets(int stopId);
        StopMaster GetStopByCode(string stopCode);
        List<StopMaster> GetStopNotExistInFe(int assetId, int floorId);
        int SaveLocation(StopMaster newStopMaster);
        int SaveLocationRouteMapping(StopsRouteMapping newStopsRouteMapping, string locationsId);
        int SaveRoute(RouteMaster newRouteMaster);
        bool UpdateFloorAssetsLocation(TFloorAssetsLocations newLocation);
        int UpdateLocation(StopMaster newStopMaster);

        #region Location Groups

        int SaveLocationGroup(LocationGroups objLocationGroups);
        List<LocationGroups> GetLocationGroup();
        int SaveGroupLocationsDetails(LocationGroupDetails objlocationGroupDetails);
        List<Buildings> GetGroupLocationsDetails(int? locationGroupId);
        List<Buildings> GetGroupBuildings();

        #endregion
    }
}