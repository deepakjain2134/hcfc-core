using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Infrastructure.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Action("Index","Home");
            }
            return localUrl;
        }
    }
}
