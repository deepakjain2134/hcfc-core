using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public class SiteService : ISiteService
    {        
        public static IBuildingsService _buildingsService;
        private readonly ISiteRepository _siteRepository;

        public SiteService(IBuildingsService buildingsService, ISiteRepository siteRepository)
        {
            
            _buildingsService = buildingsService;
            _siteRepository = siteRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public int Save(Site site)
        {
            return _siteRepository.Save(site);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Site> GetSites()
        {
            return _siteRepository.GetSites();
        }
        public List<StateMaster> GetStates()
        {
            return _siteRepository.GetStates();
        }
        public int AddStateName(StateMaster state)
        {
            return _siteRepository.AddStateName(state);
        }

        public List<CityMaster> GetCities(int stateId)
        {
            return _siteRepository.GetCities(stateId);
        }
        public int AddCity(CityMaster city)
        {
            return _siteRepository.AddCity(city);
        }

        public List<SiteType> GetSiteType()
        {
            return _siteRepository.GetSiteType();
        }

        public List<Site> GetSitesBuildings()
        {
            var sites = _siteRepository.GetSites();
            var buildings = _buildingsService.GetBuildings();
            foreach (var item in sites)
            {
                item.Buildings = buildings.Where(x => x.SiteCode == item.Code).ToList();
            }
            return sites;
        }
        public List<Site> GetBBISitesBuildings()
        {
            var sites = _siteRepository.GetSites();
            var buildings = _buildingsService.GetBuildings();
            foreach (var item in sites)
            {
                item.Buildings = buildings.Where(x => x.SiteCode == item.Code).ToList();
                var buildingDetails = _buildingsService.GetBuildingDetails();
                if (buildingDetails != null)
                {
                    foreach (var building in item.Buildings)
                    {
                        building.BuildingDetails = buildingDetails.Where(x => x.BuildingId == building.BuildingId).FirstOrDefault();
                    }
                }

            }
            return sites;
        }

        public List<FireSystemType> GetFireSystemTypeDetails()
        {
            var FireSystemType = _siteRepository.GetFireSystemTypeDetails();

            return FireSystemType;
        }

        public int Insert_FireSystemType(FireSystemType FireSystemType)
        {
            int retval = _siteRepository.Insert_FireSystemType(FireSystemType);

            return retval;
        }
        public List<Site> GetSitesByUserId(int userid)
        {
            return _siteRepository.GetSitesByUserId(userid);
        }
    }
}
