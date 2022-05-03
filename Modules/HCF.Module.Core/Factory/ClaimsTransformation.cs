using HCF.BAL;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Models;
using HCF.Utility;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Factory
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly IWorkContext _workContext;

        public ClaimsTransformation(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var currentOrg = await _workContext.GetCurrnetOrg();

            var hasDdNameClaim = principal.Claims.Any(x => x.Type == SessionKey.ClientDBName);
            var hasClientClaim = principal.Claims.Any(x => x.Type == SessionKey.ClientNo);

            if (currentOrg != null && !string.IsNullOrEmpty(currentOrg.DbName) && currentOrg.ClientNo>0)
            {
                if (!hasDdNameClaim)
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(SessionKey.ClientDBName, currentOrg.DbName));

                if (!hasClientClaim)
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(SessionKey.ClientNo, Convert.ToString(currentOrg.ClientNo)));
            }

            return principal;
        }
    }
}
