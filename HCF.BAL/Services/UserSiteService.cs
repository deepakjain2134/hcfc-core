using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL.Services
{
    public class UserSiteService : IUserSiteService
    {

        private readonly IUserSiteRepository _userSiteMapping;

        public UserSiteService(IUserSiteRepository userSiteMapping)
        {
            _userSiteMapping = userSiteMapping;
        }

        //public IEnumerable<UserProfile> GetSitesUsers(int siteId)
        //{
        //    var results = _userSiteMapping.GetAll().Where(x => x.SiteId == siteId && x.IsActive).Select(x => x.UserProfile);
        //    return results;
        //}        

        //public IEnumerable<Site> GetUserSites(int userId)
        //{
        //    var results = _userSiteMapping.GetAll().Where(x => x.UserId == userId && x.IsActive).Select(x => x.Site);
        //    return results;
        //}

        //public IEnumerable<Site> GetVendorSites(int vendorId)
        //{
        //    var results = _userSiteMapping.GetAll().Where(x => x.VendorId == vendorId && x.IsActive).Select(x => x.Site);
        //    return results;
        //}

        public IEnumerable<UserSiteMapping> GetVendorSiteMappings()
        {
            return _userSiteMapping.GetVendorSiteMappings();
        }

        public IEnumerable<UserSiteMapping> GetVendorSiteMappings(int vendorId)
        {
            return _userSiteMapping.GetVendorSiteMappings(vendorId);
        }

        public IEnumerable<UserSiteMapping> GetUserSiteMappings()
        {
            return _userSiteMapping.GetUserSiteMappings();
        }

        public IEnumerable<UserSiteMapping> GetUserSiteMappings(int userId)
        {
            return _userSiteMapping.GetUserSiteMappings(userId);
        }

       
    }
}
