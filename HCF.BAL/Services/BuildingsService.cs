using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class BuildingsService :IBuildingsService
    {
        private readonly IBuildingsRepository _buildingsRepository;
        public BuildingsService(IBuildingsRepository buildingsRepository)
        {
            _buildingsRepository = buildingsRepository;
        }
               
        public  int Save(Buildings building)
        {
            return _buildingsRepository.Save(building);
        }

        public  List<Buildings> GetBuildings()
        {
            return _buildingsRepository.GetBuildings();
        }
        public  List<Site> GetAllBuildingsbySite(string sitecode)
        {            
            return _buildingsRepository.GetAllBuildingsbySite(sitecode);
        }

        public  List<BuildingDetails> GetBuildingDetails()
        {
            return _buildingsRepository.GetBuildingDetails();
        }

        public  List<BuildingType> GetBuildingByType()
        {
            return _buildingsRepository.GetBuildingByType();
        }             

        public  int UpdateBuilding(Buildings newBuilding)
        {
            return _buildingsRepository.UpdateBuliding(newBuilding);
        }

        public  List<Buildings> GetBuildingFloors(string siteCode=null, string stateId=null, string cityId=null)
        {
            return _buildingsRepository.GetBuildingFloors(siteCode, stateId, cityId);
        }

        public List<Buildings> GetBuildingFloors()
        {
            return _buildingsRepository.GetBuildingFloors();
        }

        public  List<Buildings> GetFilterBuildings(string assetId, string ascIds)
        {
            return _buildingsRepository.GetFilterBuildings(assetId, ascIds);
        }

        public  int Insert_BBI(BuildingDetails buildingDetails)
        {
            return _buildingsRepository.Insert_BBI(buildingDetails);
        }

        public  bool DeleteBbi(int id)
        {
            return _buildingsRepository.DeleteBBI(id);
        }

        public List<Buildings> GetCampus(int userid ,  int epdetailid)
        {
            return _buildingsRepository.GetCampus(userid,  epdetailid);
        }
    }
}
