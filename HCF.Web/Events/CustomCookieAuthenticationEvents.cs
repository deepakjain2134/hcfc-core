using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Utility.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace HCF.Web.Events
{
    //public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    //{
    //    private readonly IUserService _userService;

    //    public CustomCookieAuthenticationEvents(IUserService userService)
    //    {
    //        // Get the database from registered DI services.
    //        _userService = userService;
    //    }
    //    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    //    {
    //        var userPrincipal = context.Principal;

    //        // Look for the LastChanged claim.
    //        var lastChanged = (from c in userPrincipal.Claims
    //                           where c.Type == "LastChanged"
    //                           select c.Value).FirstOrDefault();

    //        var userId = (from c in userPrincipal.Claims
    //                      where c.Type == ClaimTypes.NameIdentifier
    //                      select c.Value).FirstOrDefault();         

    //        if (!string.IsNullOrEmpty(userId) && !_userService.ValidateLastChanged(userId))
    //        {
    //            context.RejectPrincipal();               
    //            await context.HttpContext.SignOutAsync(
    //                AuthenticationDefaults.AuthenticationScheme);
    //        }
    //    }

    //    private static bool IsAjaxRequest(HttpRequest request)
    //    {
    //        return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
    //            string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    //    }
    //}
}
