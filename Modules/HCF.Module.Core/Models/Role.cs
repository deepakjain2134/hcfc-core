using HCF.BDO.Enums;
using HCF.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HCF.Module.Core.Models
{
    public class Role : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public IList<UserRole> Users { get; set; } = new List<UserRole>();
    }
}
