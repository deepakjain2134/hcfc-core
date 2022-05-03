using HCF.BDO;
using Microsoft.AspNetCore.Identity;
using System;

namespace HCF.Module.Core.Models
{
    public class UserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }

        public override long RoleId { get; set; }

        public UserProfile User { get; set; }        

        public Role Role { get; set; }

        public Guid OrgKey { get; set; }
    }
}
