using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ILocationService
    {
        bool AddFloorAssetsLocation(TFloorAssetsLocations newLocation);
        List<InspectionEPDocs> GetFileDetailsById(int FileId);
        List<StopsRouteMapping> GetLocationRouteMapping(int? routeId);
        List<StopsRouteMapping> GetLocationrouteMapping(int? floorId, int? routeId);
        List<StopMaster> GetLocationsMaster(int? locationMasterId);
        List<RouteMaster> GetRouteByAsset(int assetId);
        List<RouteMaster> GetRouteByFloor(int floorId);
        List<RouteMaster> GetRouteMaster(int? routeId);
        List<TFloorAssets> GetStopAssets(int stopId);
        StopMaster GetStopByCode(string codeName);
        List<StopMaster> GetStopNotExistInFe(int assetId, int floorId);
        int SaveLocation(StopMaster newStopMaster);
        int SaveLocationRouteMapping(StopsRouteMapping newStopsRouteMapping, string locationsId);
        int SaveRoute(RouteMaster newRouteMaster, string locationsId);
        bool UpdateFloorAssetsLocation(TFloorAssetsLocations newLocation);
        int UpdateLocation(StopMaster stopMaster);

        #region  Location Groups
        int SaveLocationGroup(LocationGroups objLocationGroups);
        List<LocationGroups> GetLocationGroup();
        int SaveGroupLocationsDetails(LocationGroupDetails objlocationGroupDetails);
        List<Buildings> GetGroupLocationsDetails(int? locationGroupId);
        List<Buildings> GetGroupBuildings();
        #endregion
    }
}