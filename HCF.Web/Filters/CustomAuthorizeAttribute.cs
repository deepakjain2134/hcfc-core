using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace HCF.Web.Filters
{
    public sealed class CrxAuthorizeAttribute : Attribute
    {
        public string Roles { get; set; }



        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    //if (OnlyCRxUser && UserSession.CurrentUser.IsSystemUser == false)
        //    //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Message", action = "AccessDenied" }));


        //    //if (UserSession.CurrentUser.UserId > 0)
        //    //{
        //    //    Roles = Roles.Trim();
        //    //    if (filterContext.HttpContext.Request.IsAuthenticated && !string.IsNullOrEmpty(Roles))
        //    //    {
        //    //        var currentPage = UISession.CurrentPage();
        //    //        var loginGuid = new Guid(UserSession.LogOnToken);
        //    //        var status = UnityContextFactory<IUserService>.CreateContext().CheckloginToken(loginGuid, UserSession.CurrentUser.UserId, Roles.ToLower(), currentPage);
        //    //        switch (status)
        //    //        {
        //    //            case "200":
        //    //                {
        //    //                    if (string.IsNullOrEmpty(Utility.HcfSession.ClientDBName) && UserSession.CurrentUser.Orgkey == Guid.Empty)
        //    //                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Main", action = "Clients" }));
        //    //                    break;
        //    //                }
        //    //            case "401":
        //    //                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Message", action = "AccessDenied" }));
        //    //                break;
        //    //            default:
        //    //                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogOff" }));
        //    //                break;
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    filterContext.Result = new RedirectToRouteResult(new
        //    //                                RouteValueDictionary(new { controller = "Account", action = "LogOff" }));
        //    //}
        //}

    }
}