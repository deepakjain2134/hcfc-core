using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IBuildingsService
    {
        bool DeleteBbi(int id);
        List<Site> GetAllBuildingsbySite(string sitecode);
        List<BuildingType> GetBuildingByType();
        List<BuildingDetails> GetBuildingDetails();
        List<Buildings> GetBuildingFloors(string siteCode = null, string stateId = null, string cityId = null);
        List<Buildings> GetBuildings();
        List<Buildings> GetFilterBuildings(string assetId, string ascIds);
        int Insert_BBI(BuildingDetails buildingDetails);
        int Save(Buildings building);
        int UpdateBuilding(Buildings newBuilding);
        List<Buildings> GetBuildingFloors();
        List<Buildings> GetCampus(int userid, int epdetailid);
    }
}