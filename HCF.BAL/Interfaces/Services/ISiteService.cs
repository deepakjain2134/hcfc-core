using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ISiteService
    {
        int AddCity(CityMaster city);
        int AddStateName(StateMaster state);
        List<Site> GetBBISitesBuildings();
        List<CityMaster> GetCities(int stateId);
        List<FireSystemType> GetFireSystemTypeDetails();
        List<Site> GetSites();
        List<Site> GetSitesBuildings();
        List<SiteType> GetSiteType();
        List<StateMaster> GetStates();
        int Insert_FireSystemType(FireSystemType FireSystemType);
        int Save(Site site);
        List<Site> GetSitesByUserId(int userid);
    }
}