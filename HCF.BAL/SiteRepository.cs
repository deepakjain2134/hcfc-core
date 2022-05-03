//using HCF.BDO;
//using HCF.DAL;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public class SiteService : ISiteService
//    {
//        public static IRepository<Site> _siteRepository;
//        public static IBuildingsService _buildingsService;

//        public SiteService(IRepository<Site> siteRepository,IBuildingsService buildingsService)
//        {
//            _siteRepository = siteRepository;
//            _buildingsService = buildingsService;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="site"></param>
//        /// <returns></returns>
//        public int Save(Site site)
//        {
//            return DAL.SiteRepository.Save(site);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public List<Site> GetSites()
//        {
//            return DAL.SiteRepository.GetSites();
//        }
//        public List<StateMaster> GetStates()
//        {
//            return DAL.SiteRepository.GetStates();
//        }
//        public int AddStateName(StateMaster state)
//        {
//            return DAL.SiteRepository.AddStateName(state);
//        }

//        public List<CityMaster> GetCities(int stateId)
//        {
//            return DAL.SiteRepository.GetCities(stateId);
//        }
//        public int AddCity(CityMaster city)
//        {
//            return DAL.SiteRepository.AddCity(city);
//        }

//        public List<SiteType> GetSiteType()
//        {
//            return DAL.SiteRepository.GetSiteType();
//        }

//        public List<Site> GetSitesBuildings()
//        {
//            var sites = SiteRepository.GetSites();
//            var buildings = _buildingsService.GetBuildings();
//            foreach (var item in sites)
//            {
//                item.Buildings = buildings.Where(x => x.SiteCode == item.Code).ToList();
//            }
//            return sites;
//        }
//        public List<Site> GetBBISitesBuildings()
//        {
//            var sites = SiteRepository.GetSites();
//            var buildings = _buildingsService.GetBuildings();
//            foreach (var item in sites)
//            {
//                item.Buildings = buildings.Where(x => x.SiteCode == item.Code).ToList();
//                var buildingDetails = _buildingsService.GetBuildingDetails();
//                if (buildingDetails != null)
//                {
//                    foreach (var building in item.Buildings)
//                    {
//                        building.BuildingDetails = buildingDetails.Where(x => x.BuildingId == building.BuildingId).FirstOrDefault();
//                    }
//                }

//            }
//            return sites;
//        }

//        public List<FireSystemType> GetFireSystemTypeDetails()
//        {
//            var FireSystemType = SiteRepository.GetFireSystemTypeDetails();

//            return FireSystemType;
//        }
//        public int Insert_FireSystemType(FireSystemType FireSystemType)
//        {
//            int retval = SiteRepository.Insert_FireSystemType(FireSystemType);

//            return retval;
//        }
//    }
//}
