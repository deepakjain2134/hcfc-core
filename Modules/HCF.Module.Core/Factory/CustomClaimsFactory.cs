using HCF.BDO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
namespace HCF.Module.Core.Factory
{
    public class CustomClaimsFactory : UserClaimsPrincipalFactory<UserProfile>
    {
        public CustomClaimsFactory(UserManager<UserProfile> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserProfile user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FirstName", (!string.IsNullOrEmpty(user.FirstName)) ? user.FirstName : ""));
            identity.AddClaim(new Claim("LastName", (!string.IsNullOrEmpty(user.LastName)) ? user.LastName : ""));
            identity.AddClaim(new Claim("FullName", (!string.IsNullOrEmpty(user.FullName)) ? user.FullName : ""));
            identity.AddClaim(new Claim("Name", user.Name));
            identity.AddClaim(new Claim("Orgkey", Convert.ToString(user.Orgkey)));
            return identity;
        }
    }
}
