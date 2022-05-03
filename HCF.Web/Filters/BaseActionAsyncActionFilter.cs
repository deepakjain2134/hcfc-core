using HCF.BAL;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using HCF.Utility.Configuration;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using HCF.Module.Core.Extensions;

namespace HCF.Web.Filters
{
    public class BaseActionAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecuting(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context,    ActionExecutionDelegate next)
        {
            var result = "200";
            //if (UserSession.CurrentUser.UserId == 0)
            //    result = "404";

            //var dbName = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<string>(SessionKey.ClientDBName);
            //if (string.IsNullOrEmpty(dbName))
            //    result = "404";

             result =await CheckUserLogin(context); 

            if (result == "401")           
                context.Result =new RedirectToRouteResult(new RouteValueDictionary(new { action = "Message", controller = "AccessDenied" })); // set wrong controller name mismatch       
            else if (result == "500")           
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "LogOff", controller = "Auth" }));
            else if (result == "404" || result == "202"|| string.IsNullOrEmpty(result)) // 404 user logout 202 user Marked InActive 
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "LogOff", controller = "Auth" }));
            else if (result == "502")
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "NewDeviceVerification", controller = "Auth" }));
            else           
                await next();            
        }

        #region Methods

        private  Task<string> CheckUserLogin(ActionExecutingContext filterContext)
        {
            var status = "200";
            if (!IsAjaxRequest(filterContext.HttpContext.Request))
            {
                var loginToken =  ServiceLocator.ServiceProvider.GetService<IWorkContext>().GetLoginToken().Result;
                var currentPage = UISession.CurrentPage();
                var currentUser = UserSession.CurrentUser;
                if (!string.IsNullOrEmpty(loginToken))
                {
                    var requiredPermission = Convert.ToString(filterContext.RouteData.DataTokens[ConstMessage.PermissionDataTokenName]);
                    if (string.IsNullOrEmpty(requiredPermission) && filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        requiredPermission = $"{controllerActionDescriptor.ControllerName}_{controllerActionDescriptor.ActionName}";
                    }
                    var loginGuid = Guid.Parse(loginToken);
                    status = ServiceLocator.ServiceProvider.GetService<IUserService>().CheckloginToken(loginGuid, currentUser.UserId, requiredPermission.ToLower(), currentPage);

                }
            }
            return Task.FromResult(status);
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

        #endregion

    }
}