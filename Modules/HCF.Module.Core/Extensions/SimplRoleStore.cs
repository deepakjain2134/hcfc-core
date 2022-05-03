using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HCF.Module.Core.Data;
using HCF.Module.Core.Models;

namespace HCF.Module.Core.Extensions
{
    public class SimplRoleStore: RoleStore<Role, HcfDatabaseContext, long, UserRole, IdentityRoleClaim<long>>
    {
        public SimplRoleStore(HcfDatabaseContext context) : base(context)
        {
        }
    }
}
