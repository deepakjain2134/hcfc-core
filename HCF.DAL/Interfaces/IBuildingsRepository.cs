using HCF.BDO;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IBuildingsRepository
    {
        bool DeleteBBI(int Id);
        List<Site> GetAllBuildingsbySite(string sitecode);       
        List<BuildingType> GetBuildingByType();
        List<BuildingDetails> GetBuildingDetails();
        List<Buildings> GetBuildingFloors();
        List<Buildings> GetBuildingFloors(string siteCode = null, string stateId = null, string cityId = null);
        List<Buildings> GetBuildings();
        List<Buildings> GetFilterBuildings(string assetId, string ascIds);
        int Insert_BBI(BuildingDetails BuildingDetails);
        int Save(Buildings newBuildings);
        int UpdateBuliding(Buildings newBuilding);
        List<Buildings> ConvertToBuildings(DataTable dt);
        List<Buildings> GetCampus(int userid, int epdetailid);
    }
}