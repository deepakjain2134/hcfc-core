using HCF.BDO;
using System.Collections.Generic;

namespace HCF.BAL.Interfaces.Services
{
    public partial interface IUserSiteService
    {

        //IEnumerable<Site> GetUserSites(int userId);
        //IEnumerable<UserProfile> GetSitesUsers(int siteId);
        //IEnumerable<Site> GetVendorSites(int vendorId);
        IEnumerable<UserSiteMapping> GetUserSiteMappings();
        IEnumerable<UserSiteMapping> GetVendorSiteMappings();
        IEnumerable<UserSiteMapping> GetVendorSiteMappings(int vendorId);
        IEnumerable<UserSiteMapping> GetUserSiteMappings(int userId);
    }
}
