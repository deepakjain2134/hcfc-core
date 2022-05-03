using HCF.Module.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Localization
{
    public class EfRequestCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var workContext = httpContext.RequestServices.GetRequiredService<IWorkContext>();
            var user = await workContext.GetCurrentUser();
            var culture = "en-us";// user.Culture;

            if (culture == null)
            {
                return await Task.FromResult((ProviderCultureResult)null);
            }

            var providerResultCulture = new ProviderCultureResult(culture);

            return await Task.FromResult(providerResultCulture);
        }
    }
}
