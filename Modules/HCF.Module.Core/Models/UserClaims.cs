using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Models
{
    public class UserClaims : IdentityUserClaim<int>
    {
        public Guid RefershToken { get; set; }
    }
}
