using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Interfaces
{
   public interface IUserSiteRepository
    {
        IEnumerable<UserSiteMapping> GetUserSiteMappings();
        IEnumerable<UserSiteMapping> GetVendorSiteMappings();
        IEnumerable<UserSiteMapping> GetVendorSiteMappings(int vendorId);
        IEnumerable<UserSiteMapping> GetUserSiteMappings(int userId);
    }
}
