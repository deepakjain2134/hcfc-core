using HCF.BAL;
using System;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Utility.Enum;
using HCF.Web.Areas.Api.Filters;
using HCF.Web.Base;
using HCF.Web.Models;
using HCF.Web.ViewModels.ExtensionMethods;
using HCF.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;
using HCF.Web.ViewModels.Account;
using HCF.Utility.Extensions;
using HCF.Module.Core.Services;
using Microsoft.Extensions.Logging;
using HCF.Module.Core.Areas.Core.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Areas.Core.ViewModels.Account;
using Microsoft.Extensions.Configuration;
using HCF.BDO.Enums;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using IdentityServer4.Extensions;
using HCF.Web.IdentityServer;
using System.IO;

namespace HCF.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthController : BasePublicController
    {
        #region ctor

        private readonly SimplUserManager<UserProfile> _userManager;
        private readonly SimplSignInManager<UserProfile> _signInManager;
        private readonly IUserLoginService _userLoginService;
        private readonly ILogger<AuthController> _logger;
        private readonly IVendorService _vendorService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IHCFSession _session;
        private const string recentUser = "_C9057879X";
        private readonly string CRxAppId = "";
        private readonly IAppSetting _appSetting;
        private readonly ICommonModelFactory _commonSerives;
        private readonly ICacheService _cacheService;
        private readonly IHttpPostFactory _httpService;
        private readonly ICookieUtilFactory _cookieUtilFactory;
        private readonly IWorkContext _workContext;
        private readonly IUserManagerService _userManagerService;
        private readonly IConfiguration _configuration;
        private readonly ICommonProvider _commonProvider;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AuthController(
            IConfiguration configuration,
            IWorkContext workContext,
            SimplUserManager<UserProfile> userManager,
             SimplSignInManager<UserProfile> signInManager,
            ILogger<AuthController> logger,
            IUserManagerService userManagerService,
            ICookieUtilFactory cookieUtilFactory,
            IAppSetting appSetting,
            ICommonModelFactory commonSerives,
            IVendorService vendorService,
            IUserService userService,
            IOrganizationService organizationService,
            IHCFSession session,
            ICacheService cacheService,
            IUserLoginService userLoginService,
            IHttpPostFactory httpService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            ICommonProvider commonProvider
            )
        {
            _configuration = configuration;
            _workContext = workContext;
            _userLoginService = userLoginService;
            _cookieUtilFactory = cookieUtilFactory;
            _appSetting = appSetting;
            _session = session;
            _organizationService = organizationService;
            _vendorService = vendorService;
            _userService = userService;
            _commonSerives = commonSerives;
            _cacheService = cacheService;
            _httpService = httpService;
            _logger = logger;
            _userManagerService = userManagerService;
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _commonProvider = commonProvider;
        }
        #endregion

        #region User login / log off

        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl, string strmsg, AccountMessageId? message = null)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
            ViewBag.TaggedUserUrl = ((!string.IsNullOrEmpty(returnUrl)) && returnUrl.Contains("/deficiencies/"));
            if (HttpContext.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(returnUrl))
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToRoute("login");
            }
            else if (HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            var loginModel = new LoginModel
            {
                RememberMe = true,
                dType = 3,
                dToken = "3"
            };

            if (message != null)
                Error = GetErrorMessage(strmsg, message);

            var users = GetRecentLoginUsers();
            ViewBag.RecentUsers = users;
            loginModel.LoginMode = users.Count() > 0 ? "recentbox" : "upbox";

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IPAddress = ipAddress;

            return View(loginModel);
        }

        public string GetErrorMessage(string strmsg, AccountMessageId? message = null)
        {
            string errorMsg = message == AccountMessageId.InActiveUserMsg ? AccountMessageId.InActiveUserMsg.GetDescription()
                 : message == AccountMessageId.InActiveVendorMsg ? AccountMessageId.InActiveVendorMsg.GetDescription()
                 : message == AccountMessageId.EmailNotFound ? $"{strmsg} {AccountMessageId.EmailNotFound.GetDescription()}"
                 : string.Empty;
            return errorMsg;
        }


        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            var viewModel = await BuildLoginViewModelAsync(returnUrl);

            if (HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user != null)
            {
                if (user.LockoutEnd == null)
                {
                    var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
                    var resultAsync = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                    if (resultAsync.Succeeded)
                    {
                        var loginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
                        user.UserId = Convert.ToInt32(user.Id);
                        loginInfo.UserId = user.UserId;
                        loginInfo.OrgKey = user.Orgkey;
                        loginInfo.DeviceId = model.dToken;
                        await _userLoginService.Save(loginInfo);

                        if (user.UserId > 0)
                        {
                            var org = await _organizationService.GetOrganizationAsync(user.Orgkey);
                            await _workContext.SetCurrentOrg(org);

                            //if (user.LockoutEnabled)
                            //{
                            //    ModelState.AddModelError(string.Empty, AccountOptions.LockOutErrorMessage);
                            //    return View(viewModel);
                            //}

                            if (context != null)
                            {
                                if (context.IsNativeClient())
                                    return this.LoadingPage("Redirect", returnUrl);

                                return Redirect(returnUrl);
                            }

                            if (loginInfo.IsNewDevice)
                                SendVerificationCodeEmail(user.UserId, user);

                            if (model.RememberMe)
                                SaveUserCache(user.ToRecentLoginModel(), model.Password);

                            return RedirectToAction(nameof(MainController.SetClient), "Main",
                                new
                                {
                                    orgKey = user.Orgkey,
                                    refreshToken = Convert.ToString(loginInfo.RefreshToken)
                                });
                        }
                    }
                    else
                        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }
                else
                    ModelState.AddModelError(string.Empty, AccountOptions.LockOutErrorMessage);
            }
            else
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

            var loginModel = new LoginModel()
            {
                RememberMe = true,
                dType = 3,
                dToken = "3",
                LoginMode = model.LoginMode
            };

            ViewBag.RecentUsers = GetRecentLoginUsers();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IPAddress = Request.HttpContext.Connection.RemoteIpAddress;

            return View(loginModel);
        }

        private async Task SaveUserSessionData(UserProfile user)
        {
            var defaultOrg = user.Organizations.FirstOrDefault(x => x.Orgkey == user.Orgkey);
            var refreshToken = user.UserLogin.FirstOrDefault().RefreshToken;
            await _userManagerService.SignInAsync(HttpContext, user, defaultOrg, Convert.ToString(refreshToken), true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        //[HttpPost("logOff")]
        public async Task<IActionResult> LogOff(string currentpage, string strmsg, AccountMessageId? message = null)
        {
            await ClearLoginInData(currentpage);
            await _signInManager.SignOutAsync();
            if (currentpage != "-1")
            {
                return RedirectToAction(nameof(RedirectToLogin), new { returnUrl = "" });
            }
            else
                return RedirectToAction(nameof(Login), new { returnUrl = "", message = message, strmsg = strmsg });
        }

        private async Task<bool> ClearLoginInData(string currentpage)
        {
            var loginToken = await _workContext.GetLoginToken();
            bool status = true;
            if (!string.IsNullOrEmpty(loginToken))
                status = await _userLoginService.LogoutUserAsync(Guid.Parse(loginToken), currentpage);

            _session.clear();
            return status;
        }

        public async Task<JsonResult> UserLogOff(string logOnToken)
        {
            var result = await _userLoginService.LogoutUserAsync(Guid.Parse(logOnToken), "");
            return Json(result);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        [IsAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            string msg = "User not found.";
            var statusCode = Convert.ToInt32(HttpReponseStatusCode.Success);
            // var uri = $"{APIUrls.Account_ForgotPassword}/{Email}/";

            // var result = _httpService.CallGetMethodAnonymous(uri, ref statusCode);
            var existingUser = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserName.ToLower() == Email.ToLower());
            if (existingUser != null && existingUser.UserId > 0 && existingUser.IsActive)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = existingUser.UserId, code = code }, protocol: HttpContext.Request.Scheme);


                string path = _commonProvider.GetContentRootPath("wwwroot");
                var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/CantAccessAccount.htm"));
                var emailBody = System.IO.File.ReadAllText(fileInfo.FullName);
                var result = _userService.SendRecoveryMail(Email, emailBody, callbackUrl);
                if (statusCode == Convert.ToInt32(HttpReponseStatusCode.Success))
                {
                    if (result == 1)
                        msg = "An email with instructions on how to reset your password has been sent to " + Email + " .Check your spam or junk folder if you don’t see the email in your inbox. ";
                    else
                        msg = ConstMessage.Contact_Admin;
                }
                else if (existingUser != null && existingUser.UserId > 0 && !existingUser.IsActive)
                    msg = ConstMessage.Inactive_User_login_Msg;
                else if (existingUser == null)
                    msg = ConstMessage.Forgot_Password_User_Not_Found;
                else
                    msg = ConstMessage.Internal_Server_Error;
            }
            return Json(new { Result = msg });
        }

        public ActionResult RedirectToLogin(string returnUrl)
        {
            return PartialView("_RedirectToLogin", returnUrl);
        }

        #endregion

        #region Recent Login Box

        private void SaveUserCache(RecentLoginModel tempUser, string password)
        {
            tempUser.Password = password;
            var users = new List<RecentLoginModel>();
            var userData = GetCookieValue(recentUser);
            if (!string.IsNullOrEmpty(userData))
                users = _commonSerives.JsonDeserialize<List<RecentLoginModel>>(userData);
            var isExistingUser = users.FirstOrDefault(x => x.UserName == tempUser.UserName);
            if (isExistingUser != null)
                tempUser.PassCode = isExistingUser.PassCode;

            users.Add(tempUser);
            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(users.GroupBy(x => x.Email).Select(g => g.OrderByDescending(c => c.LoginDate).FirstOrDefault()));
            SetCookie(recentUser, userserializeData);
        }

        private List<RecentLoginModel> GetRecentLoginUsers()
        {
            var users = new List<RecentLoginModel>();
            var userData = GetCookieValue(recentUser);
            if (!string.IsNullOrEmpty(userData))
                users = _commonSerives.JsonDeserialize<List<RecentLoginModel>>(userData);
            return users.GroupBy(x => x.Email).Select(x => x.First()).ToList();
        }

        private void SetCookie(string name, string value)
        {
            _cookieUtilFactory.CreateCookie(name, value, null);
        }

        private string GetCookieValue(string keyName)
        {
            return _cookieUtilFactory.GetCookieValue(keyName);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(string userName)
        {
            var recentUsers = GetRecentLoginUsers();
            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
            recentUsers.Remove(user);
            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
            SetCookie(recentUser, userserializeData);
            return Json(new { ucont = recentUsers.Count });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SavePassCode(string pin, string status)
        {
            var currentUser = await GetCurrentUserAsync();
            var recentUsers = GetRecentLoginUsers();
            var userName = currentUser.UserName;
            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                user.PassCode = pin;
                user.IsPassCodeLogin = Convert.ToBoolean(status);
            }

            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
            SetCookie(recentUser, userserializeData);
            return Json(new { ucont = recentUsers.Count });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginUsingPassCode(string status)
        {
            var currentUser = await GetCurrentUserAsync();
            var recentUsers = GetRecentLoginUsers();
            var userName = currentUser.UserName;
            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
            if (user != null) user.IsPassCodeLogin = Convert.ToBoolean(status);
            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
            SetCookie(recentUser, userserializeData);
            return Json(new { ucont = recentUsers.Count });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValidatePassCode(string userName, string pin, string dToken)
        {
            var recentUsers = GetRecentLoginUsers();
            var user = recentUsers.FirstOrDefault(x => x.UserName == userName && x.PassCode == pin);
            if (user == null)
            {
                return Json(new { result = -1, msg = "Invalid passcode" });
            }
            else
            {
                var userloginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
                var loginModel = new UserProfile { Password = user.Password, UserName = user.UserName };
                var version = _commonSerives.GetVersion();
                var newUser = _userService.AuthenticateUser(loginModel, dToken, 3, version, userloginInfo.ip, userloginInfo);
                if (newUser == null)
                    return Json(new { result = -2, msg = "please login using your user name and password." });

                if (!newUser.IsLoginEnable)
                    return Json(new { result = -1, msg = newUser.IsLoginEnableText });

                await SaveUserSessionData(newUser);

                return newUser.TotalOrgCount > 1 ? Json(new { result = 1 }) : Json(new
                { result = 0, orgkey = newUser.Orgkey });
            }
        }

        #endregion

        #region ManagePin

        [Authorize]
        public async Task<ActionResult> ManagePin()
        {
            var currentUser = await GetCurrentUserAsync();
            var recentUsers = GetRecentLoginUsers();
            var userName = currentUser.UserName;
            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                user = new RecentLoginModel();
            return View(user);
        }

        #endregion

        #region UserVendor

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginwithOrgId(string OrganizationId, string Email)
        {
            var result = _vendorService.IsVendorExist(OrganizationId, Email);
            if (result != null)
            {
                var vendordata = _vendorService.GetVendorbyOrgid(OrganizationId);
                var vendorid = vendordata.VendorId;
                bool vendoridExist = _userService.UserVendorCreated(vendorid);
                if (vendoridExist)
                {
                    TempData["orgexist"] = "User is already created with this organization Id.";
                    return RedirectToRoute("login");
                }
                TempData["Vendorid"] = vendorid;
                return RedirectToRoute("create");
            }
            else
            {
                TempData["orgexist"] = "Organization id and Work email does not match.";
                return RedirectToRoute("login");
            }
        }
        public ActionResult Create(string invitationId)
        {
            VendorOrganizations vendorOrg = new();
            if (string.IsNullOrWhiteSpace(invitationId))
                return RedirectToRoute("login");
            else
            {
                if (!_userService.IsValidInvitationIdCode(invitationId))
                {
                    ErrorMessage = "Invalid Invitation code.";
                    ViewBag.type = "1";
                }
                else
                {
                    vendorOrg = _userService.GetVendorOrgInvitation(Guid.Parse(invitationId));
                    ViewBag.type = "0";
                }
            }
            UISession.ClearBreadCrumb();
            UISession.AddCurrentPage("account_Create", 0, "Create");
            UserAddViewModel userAddModel = new UserAddViewModel();
            if (vendorOrg != null && vendorOrg.Vendors != null)
            {
                userAddModel.VendorId = vendorOrg.Vendors.VendorId;
                userAddModel.Vendor = vendorOrg.Vendors;
                userAddModel.Orgkey = vendorOrg.OrgKey;
                userAddModel.Organization = vendorOrg.Organization;
                TempData.Put("VendorOrg", vendorOrg);
                TempData.Keep("VendorOrg");
            }
            return View(userAddModel);

        }

        [AllowAnonymous]
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserAddViewModel model, string orgName, int vendorId)
        {
            var currentUser = await GetCurrentUserAsync();
            var vendorOrg = TempData.Get<VendorOrganizations>("VendorOrg");
            var emailId = vendorOrg.Vendors.Email;

            if (ModelState.IsValid)
            {
                if (emailId == model.Email)
                {
                    var vmodel = model.ToUserProfile();
                    vmodel.UserName = vmodel.Email;
                    vmodel.CreatedBy = currentUser != null ? Convert.ToInt32(currentUser.Id) : 0;
                    if (vendorOrg != null)
                    {
                        _session.Set(SessionKey.OrgName, Convert.ToString(vendorOrg.Organization.DbName));
                        _session.Set(SessionKey.ClientDBName, Convert.ToString(vendorOrg.Organization.DbName));
                        var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, "Api User"),
                        new Claim(SessionKey.OrgName,Convert.ToString(vendorOrg.Organization.DbName)),

                     };

                        var identity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.User = principal;
                    }
                    vmodel.IsActive = true;
                    vmodel.VendorId = vendorId;

                    // save user in master database
                    vmodel.CreatedDate = DateTime.Now;
                    vmodel.UserGuid = Guid.NewGuid();
                    vmodel.Orgkey = model.Orgkey.Value;
                    var response = await _userManager.CreateAsync(vmodel, vmodel.Password);
                    if (response.Succeeded)
                    {
                        var userByEmailId = await _userManager.FindByEmailAsync(vmodel.Email);
                        vmodel.UserId = Convert.ToInt32(userByEmailId.Id);
                        // save user in master database ends

                        vmodel.UserId = _userService.CreateUser(vmodel, "");
                        if (vmodel.UserId != 0)
                        {
                            SuccessMessage = "Vendor user added Successfully.";
                            return RedirectToRoute("login");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ConstMessage.Internal_Server_Error.ToString());
                        }
                    }
                    else
                    {
                        AddErrors(response);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please use the same email address. Do not change.");
                }
            }
            //TempData["VendorOrg"] = vendorOrg;
            TempData.Put("VendorOrg", vendorOrg);
            TempData.Keep("VendorOrg");
            ViewBag.type = "0";
            return View(model);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult CheckExistingEmail(string Email, string UserId)
        {
            try
            {
                var ifEmailExist = _userService.CheckExistingEmail(Email, Convert.ToInt32(UserId));
                return Json(ifEmailExist);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        #endregion

        #region Guest User

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> GuestUser(string returnUrl, string Email, string FirstName, string LastName)
        //{
        //    UserProfile GuestUser = new UserProfile();
        //    var clientid = "";
        //    var url = "";

        //    if (!string.IsNullOrEmpty(returnUrl))
        //    {
        //        url = returnUrl;
        //        clientid = url.Split('/')[3];
        //    }
        //    // if mail exists 
        //    var ifEmailExist = _userService.CheckExistingEmail(Email, 0);
        //    if (ifEmailExist == 1)
        //    {
        //        var users = _userService.Get().FirstOrDefault(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.Email == Email);
        //        var userdetail = _userService.GetUser(Convert.ToInt32(users.UserId));
        //        await SaveUserSessionData(userdetail);
        //        if (users.UserGroups.Any(x => x.Name == "Guest User"))
        //        {
        //            return Json(new
        //            {
        //                result = 2,
        //                returnUrl = url,
        //                orgkey = clientid
        //            });
        //        }
        //    }
        //    else if (ifEmailExist == 2)
        //    {
        //        var users = _userService.Get().FirstOrDefault(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.Email == Email);
        //        var userdetail = _userService.GetUser(Convert.ToInt32(users.UserId));
        //        await SaveUserSessionData(userdetail);
        //        if (users.UserGroups.Any(x => x.Name == "Guest User"))
        //        {
        //            return Json(new
        //            {
        //                result = 2,
        //                returnUrl = url,
        //                orgkey = clientid
        //            });
        //        }
        //    }
        //    else
        //    {
        //        var userGroups = _userService.GetUserGroups();
        //        GuestUser.Email = Email;
        //        GuestUser.FirstName = FirstName;
        //        GuestUser.LastName = LastName;
        //        GuestUser.IsActive = true;
        //        GuestUser.UserName = Email;
        //        GuestUser.UserGroupIds = userGroups.FirstOrDefault(x => x.Name == "Guest User")?.GroupId.ToString();
        //        GuestUser.UserId = _userService.CreateUser(GuestUser, "");

        //        var userLogin = new UserLogin
        //        {
        //            UserId = GuestUser.UserId
        //        };
        //        _userService.CreateUserLoginForGU(userLogin);
        //        var userdetail = _userService.GetUser(Convert.ToInt32(GuestUser.UserId));
        //        await SaveUserSessionData(userdetail);
        //        if (GuestUser.UserId > 0)
        //        {
        //            return Json(new
        //            {
        //                result = 2,
        //                returnUrl = url,
        //                orgkey = clientid
        //            });
        //        }
        //    }
        //    return Json(new
        //    {
        //        result = 1,
        //        returnUrl = url,
        //        orgkey = clientid
        //    });
        //}

        #endregion

        #region External Login

        public ActionResult ExternalLogin(string data)
        {
            var loginModel = new LoginModel();
            string returnUrl = string.Empty;
            string AppSecret = string.Empty;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (data != null)
            {
                try
                {
                    string decrypt = HCF.Utility.Conversion.Decrypt(data, CRxAppId);
                    keyValuePairs = decrypt.Split('&')
                      .Select(value => value.Split('='))
                      .ToDictionary(pair => pair[0], pair => pair[1]);
                    returnUrl = keyValuePairs["returnUrl"];
                    AppSecret = keyValuePairs["AppSecret"];
                    ViewBag.ReturnUrl = returnUrl;
                    if (AppSecret == _appSetting.CRxAppSecret)
                    {
                        if (UserSession.CurrentUser.UserId > 0)
                        {
                            var postData = PostDataOnCRxForum(UserSession.CurrentUser);
                            var redirectToclient = string.Format("{0}={1}", returnUrl, postData);
                            if (Url.IsLocalUrl(redirectToclient))
                                return Redirect(redirectToclient);
                            else
                                return RedirectToAction("login");
                        }
                    }
                    else
                        ErrorMessage = "Invalid App key";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    ErrorMessage = "Link is broken. Please try again later.";
                }
            }
            else
            {
                ErrorMessage = "Link is broken. Please try again later.";
            }
            ViewBag.ErrorMessage = ErrorMessage;
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLogin(LoginModel model, string returnUrl)
        {
            //model.UserOrganization = new List<Organization>();
            if (ModelState.IsValid)
            {
                var loginUser = model.ToUserProfile();
                var userloginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
                var user = _userService.AuthenticateUser(loginUser, model.dToken, 3, _appSetting.BuildVersion, userloginInfo.ip, userloginInfo);
                if (user.UserId > 0)
                {

                    if (!user.IsLoginEnable)
                        return Json(new { result = -1, msg = user.IsLoginEnableText });

                    var postData = PostDataOnCRxForum(user);
                    return Json(new
                    {
                        result = 1,
                        redirectToclient = string.Format("{0}={1}", returnUrl, postData),
                        msg = ConstMessage.InActive_vendor
                    });
                }
                return Json(new { result = -1, msg = ConstMessage.Invalid_UserName_and_Password });
            }
            return Json("");
        }

        private string PostDataOnCRxForum(UserProfile user)
        {
            var objLoginModel = new CRxForumLoginModel()
            {
                UserName = user.UserName,
                EmailAddress = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.Name,
                RefreshToken = user.UserLogin.FirstOrDefault().RefreshToken.ToString(),
                Status = 200,
            };
            string postData = _commonSerives.JsonSerialize<CRxForumLoginModel>(objLoginModel);
            var data = HCF.Utility.Conversion.Encrypt(postData, _appSetting.CRxAppId);
            return data;

        }
        #endregion

        #region lockout User 

        //public int UserlockoutCheck(UserProfile user)
        //{
        //    string lockoutUserCookieKey = string.Format("{0}_user_lock", user.Email);
        //    int loginattempt = 0;
        //    var userLockout = GetCookieValue(lockoutUserCookieKey);
        //    if (!string.IsNullOrEmpty(userLockout))
        //        loginattempt = Convert.ToInt32(userLockout);

        //    loginattempt++;
        //    var cookieCalue = HCF.Utility.Conversion.Encrypt(loginattempt.ToString(), lockoutUserCookieKey);
        //    HttpContext.Response.Cookies.Append(lockoutUserCookieKey, cookieCalue, new CookieOptions
        //    {
        //        Expires = DateTime.UtcNow.AddMinutes(5),
        //        HttpOnly = true,
        //        IsEssential = true,
        //        SameSite = SameSiteMode.Strict
        //    });

        //    if (Convert.ToInt32(loginattempt) >= 5 && !string.IsNullOrEmpty(userLockout))
        //    {
        //        HttpContext.Response.Cookies.Append(lockoutUserCookieKey, cookieCalue, new CookieOptions
        //        {
        //            Expires = DateTime.UtcNow.AddSeconds(-1),
        //            HttpOnly = true,
        //            IsEssential = true,
        //            SameSite = SameSiteMode.Strict
        //        });
        //        _userService.Userlockout(user, 1);
        //    }


        //    return loginattempt;
        //}

        #endregion

        #region New Device Check       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        public void SendVerificationCodeEmail(int userId, UserProfile user)
        {
            var newdevicecode = _commonSerives.GenerateCode();
            _cacheService.Set("otp_" + user.UserName.ToString(), newdevicecode, CacheTimes.TwentyMinute);
            if (user.UserId > 0 && user.IsActive)
                _userService.SendVerificationCode(user.UserName, newdevicecode, user.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<ActionResult> NewDeviceVerification(int? UserId)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser != null && currentUser.Id > 0)
            {
                TempData["UserId"] = currentUser.Id;
                TempData["RecovMesersage"] = "This Device is not recognised by our system. Please check your email for verification code";
                return View();
            }
            return RedirectToAction(nameof(AuthController.LogOff));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VerificationCode"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewDeviceVerification(string VerificationCode, int UserId)
        {
            if (!String.IsNullOrEmpty(VerificationCode))
            {
                var refreshToken = await _workContext.GetLoginToken();
                var defaultcode = "98127";
                UserProfile CurrentUser = new();
                CurrentUser = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserId == UserId);
                string code = _cacheService.Get<string>("otp_" + CurrentUser.UserName.ToString());
                if (!String.IsNullOrEmpty(Request.Form["Re-SendOtp"]))
                {
                    TempData["UserId"] = UserSession.CurrentUser.UserId;
                    TempData["RecovMesersage"] = "This Device is not recognised by our system. Please check your email for verification code";
                    @TempData["SuccessMessage"] = "Otp Sent Successfully!.";
                    SendVerificationCodeEmail(UserId, CurrentUser);
                }
                else
                {

                    if (string.IsNullOrEmpty(code))
                    {
                        @TempData["Message"] = "Code Expired!.";
                    }
                    else if (code.ToString() == VerificationCode.ToString() && code != null || defaultcode.ToString() == VerificationCode.ToString()) // device check also
                    {
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            bool status = await _userLoginService.AuthorizeDevice(Guid.Parse(refreshToken));
                            //int result = _userService.UpdateNewDevice(userId, null);//refrence token
                            if (status || defaultcode.ToString() == VerificationCode.ToString())
                                return RedirectPermanent("/Home/Index");
                        }
                    }
                    else
                        @TempData["Message"] = "Invalid Code. Please Try Again.";
                }
            }
            else
            {
                @TempData["Message"] = "Please Enter Verification Code.";
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgKey"></param>
        /// <param name="UserId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<ActionResult> SetVerifiedClient(Guid orgKey, int UserId, string returnUrl)
        {
            var org = await _organizationService.GetOrganizationAsync(orgKey);
            if (org != null)
            {
                var users = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserId == UserId);
                if (users != null)
                {
                    string code = _cacheService.Get<string>(users.UserName.ToString());
                    if (code != null)
                    {
                        _cacheService.ClearCache(users.UserName.ToString());
                    }
                    return RedirectPermanent("/Home/Index");
                }
                else
                {
                    return RedirectToRoute("client");
                }
            }
            else
                return RedirectToRoute("client");
        }

        #endregion

        #region Change Password

        public ActionResult ChangePassword()
        {
            return RedirectToAction("Manage", "User");
        }

        private Task<UserProfile> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion

        #region ResetPassword

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AuthController.Login), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                Success = "Your password has been reset.";
                //if(HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(AuthController.LogOff), "Auth");
                //else
                //return RedirectToAction(nameof(AuthController.Login), "Account");
            }
            if (result.Errors.FirstOrDefault().Code == "InvalidToken")
            {

                Error = "The link you are trying to access is either expired or does not exist. Please check if you are entering the correct URL or re-initiate the password recovery to reset your CRx account password.";
                return View();
            }

            AddErrors(result);
            return View();
        }

        #endregion

        #region external 

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalOpenIdLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            //}
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, FullName = name });
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new UserProfile { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "customer");
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }
        #endregion

        #region helper APIs for the AccountController

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Email = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Email = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        #endregion

        #region Microsoft Login 

        [AllowAnonymous]
        [HttpGet("microsoft/auth/sucess/{email}")]
        public async Task<IActionResult> MicrosoftAuthSucess(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result != null && result.Id > 0 && result.IsActive)
            {
                //SuccessMessage = "Logged in successfully.";
                var loginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
                result.UserId = Convert.ToInt32(result.Id);
                loginInfo.RefreshToken = Guid.NewGuid();
                loginInfo.OrgKey = result.Orgkey;
                loginInfo.UserId = result.Id;
                loginInfo.LogOnDate = DateTime.UtcNow;
                loginInfo.IsLogOn = true;
                loginInfo.UserName = result.Email;
                loginInfo.LastActivityDateTime = DateTime.Now;
                loginInfo.LoginProvider = LoginProvider.Microsoft.ToString();
                loginInfo.DeviceTypeId = 3;
                // save details in user login with login type                   
                await _userLoginService.Save(loginInfo);
                await _signInManager.SignInAsync(result, true, null);
                if (!string.IsNullOrEmpty(result.Orgkey.ToString()) && result.Orgkey != default(Guid))
                {
                    var org = await _organizationService.GetOrganizationAsync(result.Orgkey);
                    await _workContext.SetCurrentOrg(org);

                    if (loginInfo.IsNewDevice)
                        SendVerificationCodeEmail(result.UserId, result);
                    else
                        SuccessMessage = "Logged in successfully.";

                    return RedirectToAction(nameof(MainController.SetClient), "Main",
                        new
                        {
                            orgkey = result.Orgkey,
                            userId = result.UserId,
                            refreshtoken = loginInfo.RefreshToken
                        });

                    //return RedirectToRoute("setClient", new
                    //{
                    //    orgkey = result.Orgkey,
                    //    userId = result.UserId,
                    //    refreshtoken = loginInfo.RefreshToken
                    //});
                }
            }
            return RedirectToAction("LogOff", "Auth", new { strmsg = email, message = AccountMessageId.EmailNotFound });
        }

        #endregion

    }
}