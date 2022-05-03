using HCF.Utility;
using HCF.Utility.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using HCF.BAL;

namespace HCF.Web.Areas.Api.Filters
{
    public class ActionWebApiFilter : IAsyncActionFilter
    {
        public readonly IHCFSession _session;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IUserService _userService;
        public ActionWebApiFilter(IHCFSession session, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _session = session;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isAnonymous = false;
            var isValidRequest = true;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(IsAnonymousAttribute), true);
                if (actionAttributes.Length > 0)
                {
                    isAnonymous = true;
                    await next();

                }
            }

            try
            {
                if (!isAnonymous)
                {

                    foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
                    {
                        requestHeaders.Add(header.Key, header.Value);
                    }

                    if (requestHeaders.ContainsKey(APIkeys.LogOnToken) && (!requestHeaders.ContainsKey(APIkeys.ClientNo)))
                    {
                        var userlogins = _userService.GetUserOrgsByRefreshToken(Convert.ToString(requestHeaders[APIkeys.LogOnToken]));
                        requestHeaders.Add(APIkeys.ClientNo, userlogins.ClientNo.ToString());
                        requestHeaders.Add(APIkeys.DBName, userlogins.DbName);
                    }

                    if (!requestHeaders.ContainsKey(APIkeys.ClientNo))
                    {
                        isValidRequest = false;
                        context.Result = new BadRequestObjectResult($"missing {APIkeys.ClientNo} required request parameters!");
                    }
                    else if (!requestHeaders.ContainsKey(APIkeys.LogOnToken))
                    {
                        isValidRequest = false;
                        context.Result = new BadRequestObjectResult($"missing {APIkeys.LogOnToken} required request parameters!");
                    }
                    else if (!requestHeaders.ContainsKey(APIkeys.DBName))
                    {
                        isValidRequest = false;
                        context.Result = new BadRequestObjectResult($"missing {APIkeys.DBName} required request parameters!");
                    }
                    if (isValidRequest)
                    {

                        var claims = new List<Claim>()
                        {
                        new Claim(ClaimTypes.NameIdentifier, "Api User"),
                        new Claim(SessionKey.OrgName,Convert.ToString(requestHeaders[APIkeys.DBName])),
                        new Claim(SessionKey.ClientNo,Convert.ToString(requestHeaders[APIkeys.ClientNo])),
                        new Claim(SessionKey.LogOnToken,Convert.ToString(requestHeaders[APIkeys.LogOnToken]))
                         };

                        var identity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await _httpContextAccessor.HttpContext.SignInAsync(AuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        {
                            IsPersistent = true
                        });
                        context.HttpContext.User = principal;
                        _session.Set(SessionKey.ClientDBName, requestHeaders[APIkeys.DBName]);
                        _session.Set(SessionKey.OrgName, requestHeaders[APIkeys.DBName]);
                        _session.Set(SessionKey.ClientNo, requestHeaders[APIkeys.ClientNo]);
                        await next();
                    }
                    else
                    {
                        //throw new UnauthorizedException($"you are not authorized or api token is expired");
                        context.Result = new BadRequestObjectResult($"you are not authorized or login token is expired!");
                    }
                }
                else
                {
                    //var userName = context.HttpContext.User.Identity.Name;
                    _session.Set(SessionKey.ClientDBName, context.HttpContext.User.FindFirstValue(APIkeys.DBName));
                    _session.Set(SessionKey.OrgName, context.HttpContext.User.FindFirstValue(APIkeys.DBName));
                    _session.Set(SessionKey.ClientNo, context.HttpContext.User.FindFirstValue(APIkeys.ClientNo));
                    await next();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message + " at exception time";
                context.Result = new BadRequestObjectResult($"{exp}");
            }

            // execute any code before the action executes
            // var result = await next();
            // execute any code after the action executes
        }
    }
}
