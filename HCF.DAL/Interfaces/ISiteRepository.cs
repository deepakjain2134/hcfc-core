using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ISiteRepository
    {
        int AddCity(CityMaster city);
        int AddStateName(StateMaster state);
        List<CityMaster> GetCities(int stateId);
        List<FireSystemType> GetFireSystemTypeDetails();
        List<Site> GetSites();
        List<SiteType> GetSiteType();
        List<StateMaster> GetStates();
        int Insert_FireSystemType(FireSystemType FireSystemType);
        int Save(Site site);
        List<Site> GetSitesByUserId(int UserId);
      

    }
}