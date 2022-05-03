//using HCF.BAL;
//using HCF.BDO;
//using HCF.Module.Core.Extensions;
//using HCF.Module.Core.Models;
//using HCF.Utility;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace HCF.Module.Core.Areas.Core.Controllers
//{
//    [Area("Core")]
//    [Authorize]
//    public class CampusController : Controller
//    {
//        private readonly IUserService _userService;
//        private readonly IWorkContext _workContext;
//        private readonly IOrganizationService _organizationService;
//        private readonly UserManager<UserProfile> _userManager;

//        public CampusController(UserManager<UserProfile> userManager, IUserService userService, IWorkContext workContext, IOrganizationService organizationService)
//        {
//            _userManager = userManager;
//            _userService = userService;
//            _workContext = workContext;
//            _organizationService = organizationService;
//        }
//        [Route("clients")]
//        public async Task<ActionResult> Clients(string returnUrl)
//        {
//            var currentUser = await _workContext.GetUser();
//            ViewBag.ReturnUrl = returnUrl;
//            List<Organization> orgs = new();
//            var contextUser = User;
//            if (contextUser.Identity.IsAuthenticated)
//            {
//                var userId = Convert.ToInt32(currentUser.Id);
//                orgs = _userService.GetUserOrgsByUserId(userId);
//            }
//            if (orgs.Count > 1)
//                return View(orgs);
//            else if (orgs.Count == 1)
//            {
//                var first = orgs.FirstOrDefault();
//                if (first != null)
//                    return RedirectToAction("SetClient", new { orgkey = first.Orgkey });
//            }
//            return RedirectToAction("LogOff", "Account");
//        }

//        [Route("setclient/{orgKey}")]
//        public async Task<IActionResult> SetClient(Guid orgKey, string returnUrl)
//        {
//            var currentUser = await _workContext.GetUser();
//            var org = await _organizationService.GetOrganizationAsync(orgKey);
//            if (org != null)
//            {
//                await _workContext.SetCurrentOrg(org);

//                var claims2 = new List<Claim>() {
//                        new Claim(SessionKey.OrgName,org.DbName),
//                        new Claim(SessionKey.UserOrgName,org.Name),
//                        new Claim(SessionKey.ClientNo,Convert.ToString(org.ClientNo)),
//                };
//                var userIdentity = await _userManager.FindByIdAsync(currentUser.Id.ToString());
//                await _userManager.AddClaimsAsync(userIdentity, claims2);

//                //var lists = new List<Claim>();
//                //lists.Add(new Claim(Convert.ToString(SessionKey.OrgName), Convert.ToString(org.DbName)));

//                var users = _userService.GetUser(Convert.ToInt32(currentUser.Id));
//                var claims = new List<Claim>() {
//                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(users.UserId)),
//                        new Claim(ClaimTypes.Name, users.FullName),
//                        new Claim(ClaimTypes.Email, users.Email),
//                        new Claim(ClaimTypes.Role, string.Join(",", users.UserGroups.Select(x=>x.Name).ToList())),
//                        new Claim(ClaimTypes.NameIdentifier,Convert.ToString(users.UserId)),
//                        new Claim("FirstName",users.FirstName),
//                        new Claim("LastName",users.LastName),
//                        new Claim("VendorId",(users.VendorId.HasValue ? Convert.ToString(users.VendorId.Value):"0")),
//                        new Claim(SessionKey.OrgName,org.DbName),
//                        new Claim(SessionKey.UserOrgName,org.Name),
//                        new Claim(SessionKey.ClientNo,Convert.ToString(org.ClientNo)),
//                       // new Claim(SessionKey.LogOnToken,Convert.ToString(refreshToken)),
//                        new Claim("IsPowerUser",Convert.ToString(users.IsPowerUser)),
//                        new Claim("IsCRxUser",Convert.ToString(users.IsCRxUser)),
//                        new Claim("IsSystemUser",Convert.ToString(users.IsSystemUser)),
//                        new Claim("RoleIds", string.Join(",", users.UserGroups.Select(x=>x.GroupId).ToList())),
//                        new Claim("Roles",string.Join(",", users.Roles.Select(x=>x.RoleName).ToList())),
//                        new Claim("PhoneNumber",Convert.ToString(users.PhoneNumber)),
//                        new Claim("UserProfileId",Convert.ToString(users.UserProfileId)),
//                        new Claim("Orgkey",Convert.ToString(org.Orgkey)),
//                        new Claim("UserAdditionalRoleIds",string.Join(",", users.AdditionalRoles.Select(x=>x.RoleId).ToList()))};
//                await _userManager.AddClaimsAsync(userIdentity, claims);

//                //var currentUser = await _workContext.GetUser();
//                //IdentityUser user = await _userManager.FindByIdAsync(currentUser.Id.ToString());
//                //var result = await _userManager.ReplaceClaimAsync(user,Convert.ToString(SessionKey.OrgName), Convert.ToString(org.DbName));
//                //var refreshToken = currentUser.RefreshTokenHash;
//                //var users = await _userService.GetRefreshtoken(Convert.ToString(currentUser.Id), refreshToken);
//                //if (users.UserId == 0)
//                //    return RedirectToAction("LogOff", "Account");

//                ////await CreateUserIdentity(org, refreshToken, users);
//                return RedirectToRoute("dashboard");
//            }
//            else
//                return RedirectToRoute("account/login");
//        }
//    }
//}
