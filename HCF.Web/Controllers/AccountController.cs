//using AutoMapper;
//using HCF.BAL;
//using HCF.BAL.Interfaces.Services;
//using HCF.BDO;
//using HCF.Module.Core.Areas.Core.Controllers;
//using HCF.Module.Core.Services;
//using HCF.Utility;
//using HCF.Utility.AppConfig;
//using HCF.Utility.Enum;
//using HCF.Utility.Extensions;
//using HCF.Web.Areas.Api.Filters;
//using HCF.Web.Base;
//using HCF.Web.Models;
//using HCF.Web.ViewModels.Account;
//using HCF.Web.ViewModels.ExtensionMethods;
//using HCF.Web.ViewModels.Users;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Threading.Tasks;

//namespace HCF.Web.Controllers
//{
//    public class AccountController : BasePublicController
//    {
//        #region ctor

//        private readonly IUserLoginService _userLoginService;
//        private readonly ILogger<AccountController> _logger;
//        private readonly IVendorService _vendorService;
//        private readonly IUserService _userService;
//        private readonly IOrganizationService _organizationService;
//        private readonly IHCFSession _session;
//        private const string recentUser = "_C9057879X";
//        private readonly string CRxAppId = "";
//        private readonly IAppSetting _appSetting;
//        private readonly ICommonModelFactory _commonSerives;
//        private readonly ICacheService _cacheService;
//        private readonly IHttpPostFactory _httpService;
//        private readonly ICookieUtilFactory _cookieUtilFactory;
//        private readonly IMapper _mapper;
//        private readonly IUserManagerService _userManagerService;

//        public AccountController(
//            IMapper mapper,
//            ILogger<AccountController> logger,
//            IUserManagerService userManagerService,
//            ICookieUtilFactory cookieUtilFactory,
//            IAppSetting appSetting,
//            ICommonModelFactory commonSerives,
//            IVendorService vendorService,
//            IUserService userService,
//            IOrganizationService organizationService,
//            IHCFSession session,
//            ICacheService cacheService,
//            IUserLoginService userLoginService,
//            IHttpPostFactory httpService)
//        {
//            _userLoginService = userLoginService;
//            _cookieUtilFactory = cookieUtilFactory;
//            _appSetting = appSetting;
//            _session = session;
//            _organizationService = organizationService;
//            _vendorService = vendorService;
//            _userService = userService;
//            _commonSerives = commonSerives;
//            _cacheService = cacheService;
//            _httpService = httpService;
//            _mapper = mapper;
//            _logger = logger;
//            _userManagerService = userManagerService;
//        }
//        #endregion

//        #region User login / log off

//        [AllowAnonymous]
//        public ActionResult Login(string returnUrl)
//        {
//            return RedirectToAction(nameof(AuthController.Login), "Auth");
//            //_logger.LogInformation("Login url called");
//            //_logger.LogError("Login url called error");

//            //var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
//            //ViewBag.TaggedUserUrl = ((!string.IsNullOrEmpty(returnUrl)) && returnUrl.Contains("/deficiencies/"));
//            //if (UserSession.CurrentUser.UserId > 0 && !string.IsNullOrEmpty(returnUrl))
//            //{
//            //    if (Url.IsLocalUrl(returnUrl))
//            //        return Redirect(returnUrl);
//            //    else
//            //        return RedirectToRoute("login");
//            //}
//            //else if (UserSession.CurrentUser.UserId > 0 && string.IsNullOrEmpty(returnUrl))
//            //    return Redirect("/Home/Index");

//            //var loginModel = new LoginModel
//            //{
//            //    RememberMe = true,
//            //    UserOrganization = new List<Organization>(),
//            //    dType = 3,
//            //    dToken = "3"
//            //};
//            //ViewBag.RecentUsers = GetRecentLoginUsers();
//            //ViewBag.ReturnUrl = returnUrl;
//            //ViewBag.IPAddress = ipAddress;
//            //return View(loginModel);
//        }

//        /// </summary>
//        /// <param name="model"></param>
//        /// <param name="returnUrl"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
//        {
//            if (UserSession.CurrentUser.UserId > 0)
//                return Redirect("/Home/Index");

//            var loginUser = _mapper.Map<UserProfile>(model);
//            var result = _userService.GetUserProfileforlockout(loginUser.UserName);
//            if (Convert.ToInt32(result.StatusCode) == 204)
//                return Json(new { result = -1, msg = ConstMessage.User_account_lockout });
//            if (ModelState.IsValid)
//            {
//                var loginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
//                var user = _userService.AuthenticateUser(loginUser, model.dToken, 3, _appSetting.BuildVersion, loginInfo.ip, loginInfo);
//                if (user.UserId > 0)
//                {
//                    if (!user.IsLoginEnable)
//                        return Json(new { result = -1, msg = user.IsLoginEnableText });

//                    if (user.UserLogin.FirstOrDefault().IsNewDevice)
//                    {
//                        var token = user.UserLogin.FirstOrDefault().RefreshToken;
//                        _userService.UpdateNewDevice(user.UserId, Convert.ToString(token.Value));
//                        SendVerificationCodeEmail(user.UserId, user);
//                    }

//                    await SaveUserSessionData(user);
//                    SetCookie("loginUserId", Convert.ToString(user.UserId));

//                    if (model.RememberMe)
//                        SaveUserCache(user.ToRecentLoginModel(), model.Password);
//                    if (user.IsChangePasswordRequired)
//                    {
//                        return Json(new
//                        {
//                            result = 4,
//                            statusCode = user.UserStatusCode,
//                            orgkey = user.Orgkey,
//                            userId = user.UserId
//                        });
//                    }
//                    return Json(new
//                    {
//                        result = 0,
//                        returnUrl,
//                        statusCode = user.UserStatusCode,
//                        orgkey = user.Orgkey,
//                        refreshToken = user.UserLogin.FirstOrDefault().RefreshToken
//                    });
//                }
//                else if (user.UserId == -1)
//                {
//                    return Json(new
//                    {
//                        result = 0,
//                        returnUrl,
//                        statusCode = user.UserStatusCode,
//                        orgkey = user.Orgkey
//                    });
//                }
//            }
//            var lockoutstatuscode = UserlockoutCheck(model.ToUserProfile());

//            if (lockoutstatuscode >= 3 && lockoutstatuscode < 5)
//                return Json(new { result = -1, msg = "You have made " + lockoutstatuscode + " unsuccessful attempts. A maximum of 5 login attempts are permitted. The password is case-sensitive. To generate a new password, use the 'Forgot Password' option." });
//            else if (lockoutstatuscode >= 5)
//                return Json(new { result = -1, msg = ConstMessage.User_account_lockout });

//            return Json(new { result = -1, msg = ConstMessage.Invalid_UserName_and_Password });
//        }

//        private async Task SaveUserSessionData(UserProfile user)
//        {
//            var defaultOrg = user.Organizations.FirstOrDefault(x => x.Orgkey == user.Orgkey);
//            var refreshToken = user.UserLogin.FirstOrDefault().RefreshToken;
//            // await _userManagerService.SignInAsync(HttpContext, user, defaultOrg, Convert.ToString(refreshToken), true);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public async Task<IActionResult> LogOff(string currentpage)
//        {
//            return RedirectToAction(nameof(AuthController.LogOff), "Auth");
//            //return RedirectToAction(nameof(LogOff), nameof(AuthController));
//            //await ClearLoginInData(currentpage);
//            //await _userManagerService.SignOutAsync(HttpContext);
//            //return RedirectToAction("login");
//        }

//        private async Task<bool> ClearLoginInData(string currentpage)
//        {
//            bool status = true;
//            if (!string.IsNullOrEmpty(UserSession.LogOnToken))
//                status = await _userLoginService.LogoutUserAsync(Guid.Parse(UserSession.LogOnToken), currentpage);

//            _session.clear();
//            return status;
//        }

//        public async Task<JsonResult> UserLogOff(string logOnToken)
//        {
//            var result = await _userLoginService.LogoutUserAsync(Guid.Parse(logOnToken), "");
//            return Json(result);
//        }

//        ///// <summary>
//        ///// /
//        ///// </summary>
//        ///// <returns></returns>
//        //public ActionResult ForgotPassword()
//        //{
//        //    return RedirectToAction("Login");
//        //}

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="userName"></param>
//        ///// <returns></returns>
//        //[HttpPost]
//        //[IsAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult ForgotPassword(string userName)
//        //{
//        //    string msg = string.Empty;
//        //    var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
//        //    var uri = $"{APIUrls.Account_ForgotPassword}/{userName}/";

//        //    var result = _httpService.CallGetMethodAnonymous(uri, ref statusCode);
//        //    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
//        //    {
//        //        var httpResponse = _commonSerives.JsonDeserialize<HttpResponseMessage>(result);
//        //        if (httpResponse.Success)
//        //            msg = "An email with instructions on how to reset your password has been sent to " + userName + " .Check your spam or junk folder if you don’t see the email in your inbox. ";
//        //        else
//        //            msg = httpResponse.Message;
//        //    }
//        //    else
//        //        msg = ConstMessage.Internal_Server_Error;
//        //    return Json(new { Result = msg });
//        //}

//        public ActionResult RedirectToLogin(string returnUrl)
//        {
//            return PartialView("_RedirectToLogin", returnUrl);
//        }

//        //[AllowAnonymous]
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="token"></param>
//        /// <returns></returns>
//        public ActionResult Recover(string type, string token)
//        {
//            var accountRecoverStatus = new AccountRecoverStatus();
//            if (string.IsNullOrWhiteSpace(token))
//                return RedirectToRoute("login");
//            else
//            {
//                if (!_userService.IsValidRecoveryCode(token))
//                {
//                    accountRecoverStatus.ErrorMsg = "The link you are trying to access is either expired or does not exist. Please check if you are entering the correct URL or re-initiate the password recovery to reset your CRx account password.";
//                    accountRecoverStatus.StatusCode = 404;
//                    accountRecoverStatus.IsSuccess = true;
//                }
//                else
//                {
//                    accountRecoverStatus.PasswordMode = type;
//                    accountRecoverStatus.IsSuccess = false;
//                }
//            }
//            return View(accountRecoverStatus);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="token"></param>
//        /// <param name="newPassword"></param>
//        /// <param name="confirmPassword"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Recover(string token, string newPassword, string confirmPassword)
//        {
//            var accountRecoverStatus = new AccountRecoverStatus();
//            var status = true;
//            var recoveryCode = token;
//            var emailAddress = _userService.GetEmailAddressFromRecoveryCode(recoveryCode);
//            var usersList = _userService.GetUserList();
//            var user = usersList.FirstOrDefault(x => x.UserName == emailAddress);
//            if (!newPassword.Equals(confirmPassword))
//            {
//                accountRecoverStatus.ErrorMsg = "Password and Confirm password Must be same.";
//                accountRecoverStatus.IsSuccess = false;
//                accountRecoverStatus.StatusCode = 406;
//            }
//            if (!_userService.IsValidRecoveryCode(token))
//            {
//                status = false;
//                accountRecoverStatus.ErrorMsg = "The link you are trying to access is either expired or does not exist. Please check if you are entering the correct URL or re-initiate the password recovery to reset your CRx account password.";
//                accountRecoverStatus.StatusCode = 404;
//            }
//            if (status)
//            {

//                bool isInLastPassword = _userService.IsInLastPasswords(user.UserId, newPassword);
//                if (isInLastPassword)
//                {
//                    accountRecoverStatus.ErrorMsg = "Password must be not same as last 3 passwords.";
//                    accountRecoverStatus.IsSuccess = false;
//                    accountRecoverStatus.StatusCode = 406;
//                    return View(accountRecoverStatus);
//                }
//                var newPasswordSalt = _userService.GetUserSalt(emailAddress);
//                var newHashedPassword = Conversion.Encrypt(newPassword, Convert.ToString(newPasswordSalt));
//                _userService.ChangeRecoverPassword(emailAddress, newHashedPassword, newPasswordSalt, recoveryCode);

//                accountRecoverStatus.ErrorMsg = "Password Changed Sucessfully.";
//                accountRecoverStatus.IsSuccess = true;
//                accountRecoverStatus.StatusCode = 200;

//            }
//            return View(accountRecoverStatus);
//        }

//        #endregion

//        #region Recent Login Box

//        private void SaveUserCache(RecentLoginModel tempUser, string password)
//        {


//            tempUser.Password = password;
//            var users = new List<RecentLoginModel>();
//            var userData = GetCookieValue(recentUser);
//            if (!string.IsNullOrEmpty(userData))
//                users = _commonSerives.JsonDeserialize<List<RecentLoginModel>>(userData);
//            var isExistingUser = users.FirstOrDefault(x => x.UserName == tempUser.UserName);
//            if (isExistingUser != null)
//                tempUser.PassCode = isExistingUser.PassCode;

//            users.Add(tempUser);
//            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(users.GroupBy(x => x.Email).Select(g => g.OrderByDescending(c => c.LoginDate).FirstOrDefault()));
//            SetCookie(recentUser, userserializeData);

//        }

//        private List<RecentLoginModel> GetRecentLoginUsers()
//        {
//            var users = new List<RecentLoginModel>();
//            var userData = GetCookieValue(recentUser);
//            if (!string.IsNullOrEmpty(userData))
//                users = _commonSerives.JsonDeserialize<List<RecentLoginModel>>(userData);
//            return users.GroupBy(x => x.Email).Select(x => x.First()).ToList();
//        }

//        private void SetCookie(string name, string value)
//        {
//            _cookieUtilFactory.CreateCookie(name, value, null);
//        }

//        private string GetCookieValue(string keyName)
//        {
//            return _cookieUtilFactory.GetCookieValue(keyName);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult RemoveUser(string userName)
//        {
//            var recentUsers = GetRecentLoginUsers();
//            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
//            recentUsers.Remove(user);
//            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
//            SetCookie(recentUser, userserializeData);
//            return Json(new { ucont = recentUsers.Count });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult SavePassCode(string pin, string status)
//        {
//            var recentUsers = GetRecentLoginUsers();
//            var userName = Base.UserSession.CurrentUser.UserName;
//            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
//            if (user != null)
//            {
//                user.PassCode = pin;
//                user.IsPassCodeLogin = Convert.ToBoolean(status);
//            }

//            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
//            SetCookie(recentUser, userserializeData);
//            return Json(new { ucont = recentUsers.Count });
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult LoginUsingPassCode(string status)
//        {
//            var recentUsers = GetRecentLoginUsers();
//            var userName = Base.UserSession.CurrentUser.UserName;
//            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
//            if (user != null) user.IsPassCodeLogin = Convert.ToBoolean(status);
//            var userserializeData = _commonSerives.JsonSerialize<List<UserProfile>>(recentUsers);
//            SetCookie(recentUser, userserializeData);
//            return Json(new { ucont = recentUsers.Count });
//        }




//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ValidatePassCode(string userName, string pin, string dToken)
//        {
//            var recentUsers = GetRecentLoginUsers();
//            var user = recentUsers.FirstOrDefault(x => x.UserName == userName && x.PassCode == pin);
//            if (user == null)
//            {
//                return Json(new { result = -1, msg = "Invalid passcode" });
//            }
//            else
//            {
//                var userloginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
//                var loginModel = new UserProfile { Password = user.Password, UserName = user.UserName };
//                var version = _commonSerives.GetVersion();
//                var newUser = _userService.AuthenticateUser(loginModel, dToken, 3, version, userloginInfo.ip, userloginInfo);
//                if (newUser == null)
//                    return Json(new { result = -2, msg = "please login using your user name and password." });

//                if (!newUser.IsLoginEnable)
//                    return Json(new { result = -1, msg = newUser.IsLoginEnableText });

//                await SaveUserSessionData(newUser);

//                return newUser.TotalOrgCount > 1 ? Json(new { result = 1 }) : Json(new
//                { result = 0, orgkey = newUser.Orgkey });
//            }
//        }

//        #endregion

//        #region ManagePin

//        [Authorize]
//        public ActionResult ManagePin()
//        {
//            var recentUsers = GetRecentLoginUsers();
//            var userName = Base.UserSession.CurrentUser.UserName;
//            var user = recentUsers.FirstOrDefault(x => x.UserName == userName);
//            if (user == null)
//                user = new RecentLoginModel();
//            return View(user);
//        }

//        #endregion

//        #region UserVendor

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult LoginwithOrgId(string OrganizationId, string Email)
//        {
//            var result = _vendorService.IsVendorExist(OrganizationId, Email);
//            if (result != null)
//            {
//                var vendordata = _vendorService.GetVendorbyOrgid(OrganizationId);
//                var vendorid = vendordata.VendorId;
//                bool vendoridExist = _userService.UserVendorCreated(vendorid);
//                if (vendoridExist)
//                {
//                    TempData["orgexist"] = "User is already created with this organization Id.";
//                    return RedirectToRoute("login");
//                }
//                TempData["Vendorid"] = vendorid;
//                return RedirectToRoute("create");
//            }
//            else
//            {
//                TempData["orgexist"] = "Organization id and Work email does not match.";
//                return RedirectToRoute("login");
//            }
//        }
//        public ActionResult Create(string invitationId)
//        {
//            VendorOrganizations vendorOrg = new();
//            if (string.IsNullOrWhiteSpace(invitationId))
//                return RedirectToRoute("login");
//            else
//            {
//                if (!_userService.IsValidInvitationIdCode(invitationId))
//                {
//                    ErrorMessage = "Invalid Invitation code.";
//                    ViewBag.type = "1";
//                }
//                else
//                {
//                    vendorOrg = _userService.GetVendorOrgInvitation(Guid.Parse(invitationId));
//                    ViewBag.type = "0";
//                }
//            }
//            UISession.ClearBreadCrumb();
//            UISession.AddCurrentPage("account_Create", 0, "Create");
//            UserAddViewModel userAddModel = new UserAddViewModel();
//            if (vendorOrg != null && vendorOrg.Vendors != null)
//            {
//                userAddModel.VendorId = vendorOrg.Vendors.VendorId;
//                userAddModel.Vendor = vendorOrg.Vendors;
//                userAddModel.Orgkey = vendorOrg.OrgKey;
//                userAddModel.Organization = vendorOrg.Organization;
//                TempData.Put("VendorOrg", vendorOrg);
//                TempData.Keep("VendorOrg");
//            }
//            return View(userAddModel);

//        }

//        [AllowAnonymous]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(UserAddViewModel model, string orgName, int vendorId)
//        {

//            var vendorOrg = TempData.Get<VendorOrganizations>("VendorOrg");
//            var emailId = vendorOrg.Vendors.Email;

//            if (ModelState.IsValid)
//            {
//                if (emailId == model.Email)
//                {
//                    var vmodel = model.ToUserProfile();
//                    vmodel.UserName = vmodel.Email;
//                    vmodel.CreatedBy = UserSession.CurrentUser.UserId;
//                    if (vendorOrg != null)
//                    {
//                        var claims = new List<Claim>() {
//                        new Claim(ClaimTypes.NameIdentifier, "Api User"),
//                        new Claim(SessionKey.OrgName,Convert.ToString(vendorOrg.Organization.DbName)),

//                     };

//                        var identity = new ClaimsIdentity(claims, AuthenticationDefaults.AuthenticationScheme);
//                        var principal = new ClaimsPrincipal(identity);
//                        HttpContext.User = principal;
//                    }
//                    vmodel.IsActive = true;
//                    vmodel.VendorId = vendorId;
//                    vmodel.UserId = _userService.CreateUser(vmodel, "");

//                    if (vmodel.UserId != 0)
//                    {

//                        SuccessMessage = "Vendor user added Successfully.";
//                        return RedirectToRoute("login");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError(string.Empty, ConstMessage.Internal_Server_Error.ToString());
//                    }
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Please use the same email address. Do not change.");
//                }
//            }
//            TempData["VendorOrg"] = vendorOrg;
//            return View(model);

//        }

//        public JsonResult CheckExistingEmail(string Email, string UserId)
//        {
//            try
//            {
//                var ifEmailExist = _userService.CheckExistingEmail(Email, Convert.ToInt32(UserId));
//                return Json(ifEmailExist);
//            }
//            catch (Exception)
//            {
//                return Json(false);
//            }
//        }

//        #endregion

//        #region Guest User
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> GuestUser(string returnUrl, string Email, string FirstName, string LastName)
//        {
//            UserProfile GuestUser = new UserProfile();
//            var clientid = "";
//            var url = "";

//            if (!string.IsNullOrEmpty(returnUrl))
//            {
//                url = returnUrl;
//                clientid = url.Split('/')[3];
//            }
//            // if mail exists 
//            var ifEmailExist = _userService.CheckExistingEmail(Email, 0);
//            if (ifEmailExist == 1)
//            {
//                var users = _userService.Get().FirstOrDefault(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.Email == Email);
//                var userdetail = _userService.GetUser(Convert.ToInt32(users.UserId));
//                await SaveUserSessionData(userdetail);
//                if (users.UserGroups.Any(x => x.Name == "Guest User"))
//                {
//                    return Json(new
//                    {
//                        result = 2,
//                        returnUrl = url,
//                        orgkey = clientid
//                    });
//                }
//            }
//            else if (ifEmailExist == 2)
//            {
//                var users = _userService.Get().FirstOrDefault(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.Email == Email);
//                var userdetail = _userService.GetUser(Convert.ToInt32(users.UserId));
//                await SaveUserSessionData(userdetail);
//                if (users.UserGroups.Any(x => x.Name == "Guest User"))
//                {
//                    return Json(new
//                    {
//                        result = 2,
//                        returnUrl = url,
//                        orgkey = clientid
//                    });
//                }
//            }
//            else
//            {
//                var userGroups = _userService.GetUserGroups();
//                GuestUser.Email = Email;
//                GuestUser.FirstName = FirstName;
//                GuestUser.LastName = LastName;
//                GuestUser.IsActive = true;
//                GuestUser.UserName = Email;
//                GuestUser.UserGroupIds = userGroups.FirstOrDefault(x => x.Name == "Guest User")?.GroupId.ToString();
//                GuestUser.UserId = _userService.CreateUser(GuestUser, "");

//                var userLogin = new UserLogin
//                {
//                    UserId = GuestUser.UserId
//                };
//                _userService.CreateUserLoginForGU(userLogin);
//                var userdetail = _userService.GetUser(Convert.ToInt32(GuestUser.UserId));
//                await SaveUserSessionData(userdetail);
//                if (GuestUser.UserId > 0)
//                {
//                    return Json(new
//                    {
//                        result = 2,
//                        returnUrl = url,
//                        orgkey = clientid
//                    });
//                }
//            }
//            return Json(new
//            {
//                result = 1,
//                returnUrl = url,
//                orgkey = clientid
//            });
//        }

//        #endregion

//        #region External Login

//        public ActionResult ExternalLogin(string data)
//        {
//            var loginModel = new LoginModel();
//            string returnUrl = string.Empty;
//            string AppSecret = string.Empty;
//            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
//            if (data != null)
//            {
//                try
//                {
//                    string decrypt = Conversion.Decrypt(data, CRxAppId);
//                    keyValuePairs = decrypt.Split('&')
//                      .Select(value => value.Split('='))
//                      .ToDictionary(pair => pair[0], pair => pair[1]);
//                    returnUrl = keyValuePairs["returnUrl"];
//                    AppSecret = keyValuePairs["AppSecret"];
//                    ViewBag.ReturnUrl = returnUrl;
//                    if (AppSecret == _appSetting.CRxAppSecret)
//                    {
//                        if (UserSession.CurrentUser.UserId > 0)
//                        {
//                            var postData = PostDataOnCRxForum(UserSession.CurrentUser);
//                            var redirectToclient = string.Format("{0}={1}", returnUrl, postData);
//                            if (Url.IsLocalUrl(redirectToclient))
//                                return Redirect(redirectToclient);
//                            else
//                                return RedirectToAction("login");
//                        }
//                    }
//                    else
//                        ErrorMessage = "Invalid App key";
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex.Message);
//                    ErrorMessage = "Link is broken. Please try again later.";
//                }
//            }
//            else
//            {
//                ErrorMessage = "Link is broken. Please try again later.";
//            }
//            ViewBag.ErrorMessage = ErrorMessage;
//            return View(loginModel);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ExternalLogin(LoginModel model, string returnUrl)
//        {
//            model.UserOrganization = new List<Organization>();
//            if (ModelState.IsValid)
//            {
//                var loginUser = model.ToUserProfile();
//                var userloginInfo = await _userManagerService.GetBrowserInfo(HttpContext);
//                var user = _userService.AuthenticateUser(loginUser, model.dToken, 3, _appSetting.BuildVersion, userloginInfo.ip, userloginInfo);
//                if (user.UserId > 0)
//                {

//                    if (!user.IsLoginEnable)
//                        return Json(new { result = -1, msg = user.IsLoginEnableText });

//                    var postData = PostDataOnCRxForum(user);
//                    return Json(new
//                    {
//                        result = 1,
//                        redirectToclient = string.Format("{0}={1}", returnUrl, postData),
//                        msg = ConstMessage.InActive_vendor
//                    });
//                }
//                return Json(new { result = -1, msg = ConstMessage.Invalid_UserName_and_Password });
//            }
//            return Json("");
//        }

//        private string PostDataOnCRxForum(UserProfile user)
//        {
//            var objLoginModel = new CRxForumLoginModel()
//            {
//                UserName = user.UserName,
//                EmailAddress = user.Email,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                FullName = user.Name,
//                RefreshToken = user.UserLogin.FirstOrDefault().RefreshToken.ToString(),
//                Status = 200,
//            };
//            string postData = _commonSerives.JsonSerialize<CRxForumLoginModel>(objLoginModel);
//            var data = Conversion.Encrypt(postData, _appSetting.CRxAppId);
//            return data;

//        }
//        #endregion 

//        #region lockout User 
//        public int UserlockoutCheck(UserProfile user)
//        {
//            string lockoutUserCookieKey = string.Format("{0}_user_lock", user.Email);
//            int loginattempt = 0;
//            var userLockout = GetCookieValue(lockoutUserCookieKey);
//            if (!string.IsNullOrEmpty(userLockout))
//                loginattempt = Convert.ToInt32(userLockout);

//            loginattempt++;
//            var cookieCalue = Conversion.Encrypt(loginattempt.ToString(), lockoutUserCookieKey);
//            HttpContext.Response.Cookies.Append(lockoutUserCookieKey, cookieCalue, new CookieOptions
//            {
//                Expires = DateTime.UtcNow.AddMinutes(5),
//                HttpOnly = true,
//                IsEssential = true,
//                SameSite = SameSiteMode.Strict
//            });

//            if (Convert.ToInt32(loginattempt) >= 5 && !string.IsNullOrEmpty(userLockout))
//            {
//                HttpContext.Response.Cookies.Append(lockoutUserCookieKey, cookieCalue, new CookieOptions
//                {
//                    Expires = DateTime.UtcNow.AddSeconds(-1),
//                    HttpOnly = true,
//                    IsEssential = true,
//                    SameSite = SameSiteMode.Strict
//                });
//                _userService.Userlockout(user, 1);
//            }


//            return loginattempt;
//        }

//        #endregion

//        #region New Device Check
//        public bool IsNewDevice(int UserId, string dToken, string ipAddress)
//        {
//            var result = _userService.IsNewDevice(UserId, dToken, ipAddress);
//            if (result)
//                _userService.UpdateNewDevice(UserId, null);
//            return result;
//        }

//        public void SendVerificationCodeEmail(int userId, UserProfile user)
//        {
//            var newdevicecode = GenerateCode();
//            _cacheService.Set("otp_" + user.UserName.ToString(), newdevicecode, CacheTimes.TwentyMinute);
//            if (user.UserId > 0 && user.IsActive)
//                _userService.SendVerificationCode(user.UserName, newdevicecode, user.Name);
//        }

//        private static string GenerateCode()
//        {
//            string newdevicecode = "";
//            var random = RandomNumberGenerator.Create();
//            var bytes = new byte[sizeof(int)]; // 4 bytes
//            random.GetNonZeroBytes(bytes);
//            var val = Convert.ToString(BitConverter.ToInt32(bytes)).Replace("-", "");
//            Console.WriteLine(val);
//            if (val.Length > 5)
//                newdevicecode = val.Substring(0, Math.Min(val.Length, 5));
//            else
//                newdevicecode = GenerateCode();
//            return newdevicecode;
//        }

//        public ActionResult NewDeviceVerification(int UserId)
//        {
//            TempData["UserId"] = UserSession.CurrentUser.UserId;
//            TempData["RecovMesersage"] = "This Device is not recognised by our system. Please check your email for verification code";


//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult NewDeviceVerification(string VerificationCode, int UserId)
//        {
//            var defaultcode = "98127";
//            UserProfile CurrentUser = new();
//            CurrentUser = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserId == UserId);
//            string code = _cacheService.Get<string>("otp_" + CurrentUser.UserName.ToString());
//            if (!String.IsNullOrEmpty(Request.Form["Re-SendOtp"]))
//            {
//                TempData["UserId"] = UserSession.CurrentUser.UserId;
//                TempData["RecovMesersage"] = "This Device is not recognised by our system. Please check your email for verification code";
//                @TempData["SuccessMessage"] = "Otp Send Successfully!.";
//                SendVerificationCodeEmail(UserId, CurrentUser);
//            }
//            else
//            {

//                if (string.IsNullOrEmpty(code))
//                {
//                    @TempData["Message"] = "Code Expired!.";
//                }
//                else if (code.ToString() == VerificationCode.ToString() && code != null || defaultcode.ToString() == VerificationCode.ToString()) // device check also
//                {
//                    int userId = 0;
//                    if (CurrentUser.UserId > 0)
//                        userId = CurrentUser.UserId;
//                    int result = _userService.UpdateNewDevice(userId, null);//refrence token
//                    if (result == 2 || defaultcode.ToString() == VerificationCode.ToString())
//                        return RedirectPermanent("/Home/Index");
//                }
//                else
//                    @TempData["Message"] = "Invalid Code. Please Try Again.";
//            }


//            return View();
//        }

//        public async Task<ActionResult> SetVerifiedClient(Guid orgKey, int UserId, string returnUrl)
//        {
//            var org = await _organizationService.GetOrganizationAsync(orgKey);
//            if (org != null)
//            {
//                var users = _userService.GetAllClientUsers().FirstOrDefault(x => x.UserId == UserId);
//                if (users != null)
//                {
//                    string code = _cacheService.Get<string>(users.UserName.ToString());
//                    if (code != null)
//                    {
//                        _cacheService.ClearCache(users.UserName.ToString());
//                    }
//                    return RedirectPermanent("/Home/Index");
//                }
//                else
//                {
//                    return RedirectToRoute("client");
//                }
//            }
//            else
//                return RedirectToRoute("client");
//        }


//        #endregion

//        #region Change Password

//        public ActionResult ChangePassword()
//        {
//            return RedirectToAction("Manage", "User");
//        }
//        #endregion       
//    }
//}