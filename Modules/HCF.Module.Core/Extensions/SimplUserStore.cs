using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HCF.Module.Core.Data;
using HCF.Module.Core.Models;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.BDO.Users;

namespace SimplCommerce.Module.Core.Extensions
{
    public class SimplUserStore : UserStore<UserProfile, Role, HcfDatabaseContext, long, IdentityUserClaim<long>, UserRole,
        IdentityUserLogin<long>,
        IdentityUserToken<long>, IdentityRoleClaim<long>>
    {
        public SimplUserStore(HcfDatabaseContext context, IdentityErrorDescriber describer) : base(context, describer)
        {

        }       

        public Task AddToUsedPasswordAsync(UserProfile appuser, string userpassword)
        {
            appuser.UserUsedPassword.Add(new UsedPassword() { UserID = appuser.Id, HashPassword = userpassword });
            return UpdateAsync(appuser);
        }
    }
}
