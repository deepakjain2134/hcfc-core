using HCF.BDO;
using System.Collections.Generic;

namespace HCF.Web.ViewModels
{
    public class UserSiteViewModel
    {
        public IList<UserProfile> Users { get; set; }

        public IList<Site> Sites { get; set; }

        public IList<UserSiteMapping> UserSites { get;set;}

        public IList<UserSiteMapping> VendorSites { get; set; }

        public int? VendorId { get; set; }

        public int? UserId { get; set; }

        public IList<UserSiteMapping> SiteMapping { get; set; }       

    }
}