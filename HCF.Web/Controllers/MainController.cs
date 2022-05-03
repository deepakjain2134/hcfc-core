using System;
using HCF.BDO;
using HCF.BAL;
using HCF.Web.Base;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using HCF.Module.Core.Services;
using HCF.Module.Core.Extensions;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        #region ctor
        private readonly IOrganizationService _organizationService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManagerService _userManagerService;
        private readonly IHCFSession _session;
        private readonly IWorkContext _workContext;
        private readonly UserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;

        public MainController(IOrganizationService organizationService, IHttpContextAccessor httpContextAccessor, IUserService userService,
             IUserManagerService userManagerService, IHCFSession session, IWorkContext workContext, UserManager<UserProfile> userManager, SignInManager<UserProfile> signInManager)
        {
            _organizationService = organizationService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userManagerService = userManagerService;
            _session = session;
            _workContext = workContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        public ActionResult Clients(string returnUrl, Guid guid)
        {
            ViewBag.ReturnUrl = returnUrl;
            UISession.ClearSession();
            List<Organization> orgs = new();
            var contextUser = _httpContextAccessor.HttpContext.User;
            if (contextUser.Identity.IsAuthenticated)
            {
                var userId = Convert.ToInt32(contextUser.GetUserId());
                orgs = _userService.GetUserOrgsByUserId(userId);
            }
            if (orgs.Count > 1)
                return View(orgs);
            else if (orgs.Count == 1)
            {
                var first = orgs.FirstOrDefault();
                if (first != null)
                    return RedirectToAction("SetClient", new { orgkey = first.Orgkey });
            }
            return RedirectToAction("LogOff", "Auth");
        }

        public async Task<IActionResult> SetClient(Guid orgKey, string returnUrl, string refreshToken)
        {
            var org = await _organizationService.GetOrganizationAsync(orgKey);
            if (org != null)
            {

                _session.Set(SessionKey.currentorg, org);
                _session.Set(SessionKey.OrgName, org.DbName);
                _session.Set(SessionKey.ClientDBName, org.DbName);
                _session.Set(SessionKey.ClientNo, org.ClientNo);

                if (refreshToken == null || refreshToken == "")
                    refreshToken = await _workContext.GetLoginToken();
                else
                    await _workContext.SetLoginToken(refreshToken);

                var users = await _userService.GetRefreshtoken("0", refreshToken);
                _session.Set(SessionKey.User, users);
                await _workContext.SetCurrentOrg(org);

                if (users.UserId == 0)
                    return RedirectToAction("LogOff", "Auth");

                if (users.UserLogin.FirstOrDefault().IsNewDevice)
                    return RedirectToAction("NewDeviceVerification", "Auth");

                // await CreateUserIdentity(org, refreshToken, users); 
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToRoute("client");
        }   

        #region Private method
        private async Task CreateUserIdentity(Organization org, string refreshToken, UserProfile users)
        {
            //await _userManagerService.SignOutAsync(HttpContext);
            await _userManagerService.SignInAsync(HttpContext, users, org, refreshToken);
        }

        public async Task<IActionResult> ChangeUser(int userId)
        {
            var newUser = _userService.GetUser(userId);
            if (newUser != null && newUser.UserId > 0)
            {
                var org = await _organizationService.GetOrganizationAsync(newUser.Orgkey);
                if (org != null)
                {
                    try
                    {
                        var refreshToken = await _workContext.GetLoginToken();
                        //var users = await _userService.GetRefreshtoken(Convert.ToString(userId), refreshToken);
                        //await _userManagerService.SignOutAsync(HttpContext);
                        //await _userManagerService.SignInAsync(HttpContext, users, org, refreshToken);

                        await _signInManager.SignOutAsync();
                        var newuser = await _userManager.FindByEmailAsync(newUser.Email);
                        await _signInManager.SignInAsync(newuser, true);
                        _session.Set(SessionKey.User, newUser);
                        //_session.Set<Organization>(SessionKey.currentorg, org);
                        //await _workContext.SetCurrentOrg(org);
                        var result = new { Result = true, Messages = "msg", pageUrl = "dashboard" };
                        return Json(result);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Result = false, Messages = ex.Message, pageUrl = "LogOff" });
                    }
                }
            }
            return Json(new { Result = false, Messages = "msg", pageUrl = "LogOff" });
        }
        #endregion
    }
}