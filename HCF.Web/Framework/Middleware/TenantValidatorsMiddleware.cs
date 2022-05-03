using HCF.Module.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HCF.Web.Framework.Middleware
{
    public class TenantValidatorsMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;

        }
        public async Task Invoke(HttpContext context, IWorkContext workContext)
        {
            if (!context.Request.Headers.Keys.Contains("tenant"))
            {
                context.Request.Headers.Add("tenant", "alpha");
            }
            await _next.Invoke(context);
        }

    }
}
