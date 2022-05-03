using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Module.Core.Extensions;
using HCF.Module.Core.Services;
using HCF.Utility;
using HCF.Web.Base;
using HCF.Web.Filters;
using HCF.Web.ViewModels;
using HCF.Web.ViewModels.Account;
using HCF.Web.ViewModels.ExtensionMethods;
using HCF.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roles = HCF.BDO.Roles;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IUserSiteService _userSiteService;
        private readonly IVendorService _vendorService;
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly IFloorTypeService _floorTypeService;
        private readonly IEmailService _emailService;
        private readonly IHttpPostFactory _httpService;
        private readonly IHCFSession _session;
        private readonly IRolesService _roleService;
        private readonly IUserLoginService _loginService;
        private readonly IUserManagerService _userManagerService;
        private readonly SimplUserManager<UserProfile> _userManager;
        private readonly SignInManager<UserProfile> _signInManager;
        private readonly IWorkContext _workContext;
        private readonly IUserLoginCodesService _userCodeService;
        public UserController(
                    IUserLoginCodesService userCodeService,
                    SimplUserManager<UserProfile> userManager,
                    SignInManager<UserProfile> signInManager,
                    IUserManagerService userManagerService,
                    IWorkContext workContext,
                    IUserLoginService loginService,
                    ICommonModelFactory commonModelFactory,
                    IHCFSession session,
                    IUserSiteService userSiteService,
                    IVendorService vendorService,
                    ISiteService siteService,
                    IUserService userService,
                    IOrganizationService organizationService,
                    IFloorTypeService floorTypeService,
                    IRolesService roleService,
                    IEmailService emailService,
                    IHttpPostFactory httpService
                    )
        {
            _userCodeService = userCodeService;
            _userManager = userManager;
            _signInManager = signInManager;
            _userManagerService = userManagerService;
            _workContext = workContext;
            _loginService = loginService;
            _roleService = roleService;
            _commonModelFactory = commonModelFactory;
            _session = session;
            _userSiteService = userSiteService;
            _vendorService = vendorService;
            _siteService = siteService;
            _userService = userService;
            _organizationService = organizationService;
            _floorTypeService = floorTypeService;
            _emailService = emailService;
            _httpService = httpService;
        }

        #region User

        [HttpGet]
        public ActionResult Users()
        {
            var currentUser = UserSession.CurrentUser;
            var users = _userService.GetUsers().Where(x => !x.IsCRxUser);
            UISession.AddCurrentPage("user_users", 0, "Users", true);
            var logingroupids = string.Join(",", currentUser.UserGroups.Select(x => x.GroupId));
            if (currentUser.IsPowerUser || currentUser.IsSystemUser)
                users = users.ToList();
            else if (!currentUser.IsPowerUser && !currentUser.IsSystemUser)
                users = users.Where(p => p.UserGroups.Any(l => logingroupids.Contains(l.GroupId.ToString()))).ToList();
            else if (currentUser.IsVendorUser)
                users = users.Where(x => x.VendorId == currentUser.VendorId).ToList();
            else if (currentUser.UserGroups.Any(x => x.GroupId == 7))
                users = users.Where(x => x.UserGroups.Any(p => p.GroupId == 6 || p.GroupId == 7)).ToList();
            return View(users);
        }

        public ActionResult Manage(string mode)
        {
            var model = new LocalPasswordModel();
            if (mode == "lastlogin")
                model.PasswordChangeMessage = "Need to update password because you have login before 90 days";
            if (mode == "expired")
                model.PasswordChangeMessage = "The password has expired. Update your password ";
            else if (mode == "asktochange")
                model.PasswordChangeMessage = "Need to update password";
            else
                model.PasswordChangeMessage = "";

            UISession.ClearBreadCrumb();
            UISession.AddCurrentPage("user_manage", 0, "Change Password");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Manage(LocalPasswordModel model)
        {
            HttpResponseMessage httpresponse = new();
            if (ModelState.IsValid)
            {
                if (model.OldPassword != model.NewPassword)
                {
                    int userId = UserSession.CurrentUser.UserId;
                    var obj = new UserUpdatePassword
                    {
                        UserId = userId,
                        Password = model.OldPassword,
                        NewPassword = model.NewPassword
                    };
                    string uri = $"{APIUrls.User_Manage}";
                    var postData = _commonModelFactory.JsonSerialize<UserUpdatePassword>(obj);
                    int statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
                    string results = _httpService.CallPostMethod(postData, uri, ref statusCode);
                    if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    {
                        httpresponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(results);
                    }
                    else
                        httpresponse.Message = ConstMessage.Current_Password_And_New_Password_Mathch;
                }
                else
                    httpresponse.Message = ConstMessage.SameOldNewPassword;
            }
            else
                httpresponse.Message = "New Password and Confirm New Password Must be Same ";

            var result = new { Result = httpresponse.Success, Messages = httpresponse.Message };
            return Json(result);
        }

        public ActionResult MyProfile()
        {
            UISession.ClearBreadCrumb();
            UISession.AddCurrentPage("user_MyProfile", 0, "My Profile", true);
            var newUserProfile = _userService.GetUser(UserSession.CurrentUser.UserId);
            return View(newUserProfile);
        }


        public ActionResult Register(int? userId, int? vendorId)
        {
            UISession.AddCurrentPage("user_register", 0, (userId > 0) ? "Update User" : "Add User");
            var userProfile = new UserProfile();

            var model = new UserAddViewModel
            {
                VendorId = vendorId
            };

            model.Organization = _organizationService.GetOrganization();
            if (userId.HasValue)
            {
                ViewBag.PageTitle = "Update User";
                ViewBag.SubmitValue = "Update";
                userProfile = _userService.GetUser(userId.Value);

                model.LoginCount = userProfile.UserLogin.Count();
                model.FirstName = userProfile.FirstName;
                model.LastName = userProfile.LastName;
                model.ResourceNumber = userProfile.ResourceNumber;
                model.PhoneNumber = userProfile.PhoneNumber;
                model.CellNumber = userProfile.CellNo;
                model.Email = userProfile.Email;
                model.IsPowerUser = userProfile.IsPowerUser;
                model.IsActive = userProfile.IsActive;
                model.VendorId = userProfile.VendorId;
                model.UserId = userProfile.UserId;
                model.PermitTypeIds = userProfile.PermitTypeIds;
                model.OrientationDate = userProfile.OrientationDate;
                if (userProfile.UserGroups.Count > 0)
                    model.DrpUserGroupsIds = userProfile.UserGroups?.Select(x => (long)x.GroupId).ToArray();
                if (!string.IsNullOrEmpty(userProfile.UserAdditionalRoleIds))
                    model.DrpUserAdditionRoleIds = userProfile.UserAdditionalRoleIds.Split(',').Select(long.Parse).ToArray();
                if (userProfile.DrawingIds != null && userProfile.DrawingIds.Length > 0)
                    model.DrpDrawingsIds = userProfile.DrawingIds;

            }
            else
            {
                userProfile.VendorId = vendorId;
                ViewBag.PageTitle = "Add User";
                ViewBag.SubmitValue = "Add";
                model.IsActive = true;

                if (vendorId.HasValue)
                {
                    var vendors = _vendorService.GetVendors().FirstOrDefault(x => x.IsActive && x.VendorId == vendorId);
                    if (vendors == null)
                        return RedirectToAction("PageNotFound", "Message");
                }
            }
            BindRegisterData(model, vendorId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserAddViewModel model, bool? chkPassword, string submitType, string siteAssigneeType,
            string drawingsIds, string permitIds)
        {
            UserProfile vmodel = new();
            string siteAssignees = string.Empty;
            if (ModelState.IsValid)
            {
                vmodel = model.ToUserProfile();

                if (!string.IsNullOrEmpty(siteAssigneeType))
                {
                    var siteMappings = _commonModelFactory.JsonDeserialize<List<UserSiteMapping>>(siteAssigneeType);
                    siteAssignees = string.Join(",", siteMappings.Where(x => x.AssignedType != 0).Select(x => string.Format("{0}%{1}", x.SiteId, x.AssignedType)));
                }
                if (!string.IsNullOrEmpty(drawingsIds))
                {
                    vmodel.DrawingIds = drawingsIds.Split(',').Select(long.Parse).ToArray();
                }
                if (!string.IsNullOrEmpty(permitIds))
                {
                    vmodel.PermitTypeIds = permitIds.Split(',').Select(long.Parse).ToArray();
                }
                if (vmodel.VendorId.HasValue)
                    vmodel.IsPowerUser = false;

                vmodel.CreatedBy = UserSession.CurrentUser.UserId;
                vmodel.Password = !string.IsNullOrEmpty(vmodel.Password) ? vmodel.Password : "";
                vmodel.UserName = vmodel.Email;
                var token = Guid.NewGuid().ToString();
                if (submitType == "Add")
                {
                    // save user in master database
                    vmodel.CreatedDate = DateTime.Now;
                    vmodel.Orgkey = UserSession.CurrentOrg.Orgkey;
                    vmodel.UserGuid = Guid.NewGuid();
                    await _userManager.CreateAsync(vmodel, "GYDRabfg@1234");
                    //await _userManager.AddToRolesAsync(vmodel, vmodel.UserGroupIds.Split(',').ToList());

                    var userByEmailId = await _userManager.FindByEmailAsync(vmodel.Email);
                    vmodel.UserId = Convert.ToInt32(userByEmailId.Id);
                    // save user in master database ends

                    vmodel.UserId = _userService.CreateUser(vmodel, siteAssignees);
                    if (vmodel.UserId > 0)
                    {
                        vmodel.Password = model.Password;
                        SuccessMessage = ConstMessage.Success;
                        if (!string.IsNullOrEmpty(vmodel.Email))
                        {
                            //var code = await _userManager.GeneratePasswordResetTokenAsync(userByEmailId);
                            //var createPasswordUrl = Url.Action("ResetPassword", "Auth", new { userId = userByEmailId.Id, code = code, mode = PasswordUpdateType.create }, protocol: HttpContext.Request.Scheme);

                            var createPasswordUrl = await GeneratePasswordResetTokenAsync(vmodel.UserName);
                            token = _userService.PasswordResetQueue(vmodel.Email, 'E', vmodel.Email, token, Convert.ToString(BDO.Enums.passwordStatus.create));
                            _emailService.SendMailOnUserRegistration(vmodel, token, createPasswordUrl);
                        }
                        return RedirectToAction(@"Users", @"User");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(userByEmailId);
                        SuccessMessage = ConstMessage.Internal_Server_Error;
                    }
                }
                else if (submitType == "Update")
                {
                    bool status = _userService.UpdateUser(vmodel, siteAssignees);
                    if (!string.IsNullOrEmpty(vmodel.Password) && status)
                    {
                        _userService.UpdatePassword(vmodel.UserName, vmodel.Password, vmodel.CreatedBy, vmodel.IsPwdChange);
                    }
                    if (status)
                    {
                        if (model.UserId == UserSession.CurrentUser.UserId)
                        {
                            var updatedValue = _userService.GetUser(UserSession.CurrentUser.UserId);
                            _session.Set(SessionKey.User, updatedValue);
                        }
                        SuccessMessage = ConstMessage.Update_Profile_Success;
                        if (vmodel.VendorId != null && vmodel.VendorId > 0)
                        {
                            return RedirectToAction(@"Vendors", @"Vendor");
                        }
                        else
                            return RedirectToAction(@"Users", @"User");
                    }
                    else
                        SuccessMessage = ConstMessage.Internal_Server_Error;
                }

            }
            BindRegisterData(model, model.VendorId);
            return View(model);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            string passwordLink = string.Empty;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                passwordLink = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code, mode = PasswordUpdateType.create },
                    protocol: HttpContext.Request.Scheme);
            }
            return passwordLink;
        }

        private void BindRegisterData(UserAddViewModel model, int? vendorId)
        {
            var currentUser = UserSession.CurrentUser;
            List<Vendors> vendors = _vendorService.GetVendors().Where(x => x.IsActive).ToList();
            if (currentUser.IsVendorUser)
            {
                vendors = vendors.Where(x => x.VendorId == currentUser.VendorId && x.IsActive).ToList();
                ViewBag.Vendors = vendors;
            }
            else
                ViewBag.Vendors = vendors.Where(x => x.IsActive).ToList();

            var userGroups = _userService.GetUserGroupeAccess(currentUser.UserId);
            if (vendorId.HasValue && vendorId > 0)
                userGroups = userGroups.Where(x => x.GroupId == (int)BDO.Enums.UserGroup.Inspector || x.GroupId == (int)BDO.Enums.UserGroup.Vendor).ToList();
            else
                userGroups = userGroups.Where(x => x.GroupId != (int)BDO.Enums.UserGroup.Vendor).ToList();

            var listItems = new List<SelectListItem>();
            var lists = from BDO.Enums.PermitType e in Enum.GetValues(typeof(BDO.Enums.PermitType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };

            foreach (var item in lists.OrderBy(x => x.Text))
            {

                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                listItems.Add(selItem);
            }
            model.DrpPermitTypes = listItems;
            model.DrpVendors = vendors.Select(x => new SelectListItem { Text = x.Name, Value = x.VendorId.ToString() }).ToList();
            model.DrpDrawings = _floorTypeService.GetFloorTypes().Select(x => new SelectListItem { Text = x.FloorType, Value = x.FloorTypeId.ToString() }).ToList();

            model.DrpUserGroups = userGroups.Where(x => x.IsActive).Select(x => new SelectListItem { Text = x.Name, Value = x.GroupId.ToString() }).ToList();
            model.DrpUserAdditionRole = _userService.GetUserAdditionalRole().Where(x => x.IsActive).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            if (model.UserSites == null)
                model.UserSites = new List<UserSiteMapping>();

        }

        public ActionResult CheckExistingEmail(string Email, int UserId)
        {
            try
            {
                var ifEmailExist = _userService.CheckExistingEmail(Email, UserId);
                return Json(ifEmailExist);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            var status = _userService.DeleteUser(id);
            var msg = status == 1 ? ConstMessage.User_Deleted_Successfully : ConstMessage.User_NotDeleted;
            return Json(new { result = status, msg = msg });
        }

        #endregion

        #region permissions

        [CrxAuthorize(Roles = "user_permissions")]
        public ActionResult Permissions()
        {
            UISession.AddCurrentPage("user_Permissions", 0, "Roles Master");
            var rolesInGroups = _roleService.GetPermissionMatrix();
            return View(rolesInGroups);
        }


        public static IEnumerable<Roles> FindAllParents(List<Roles> allData, Roles child)
        {
            var parent = allData.FirstOrDefault(x => x.RoleId == child.ParentId);

            if (parent == null)
                return Enumerable.Empty<Roles>();

            return new[] { parent }.Concat(FindAllParents(allData, parent));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdatePermissions(int groupId, int roleId, bool newValue)
        {
            var roles = new RolesInGroups { Status = newValue, GroupId = groupId, RoleId = roleId };
            bool status = _roleService.UpdatePermissionMatrix(roles);
            return Json(status);
        }

        [HttpGet]
        public ActionResult UserGroups()
        {
            UISession.AddCurrentPage("user_UserGroups", 0, "UserGroups");
            var userGroup = _userService.GetUserGroups();
            return View(userGroup);
        }

        #endregion

        #region Certificates

        public ActionResult UserCertificates(string userId, string vendorid, string view)
        {
            UISession.AddCurrentPage("User_UserCertificates", 0, "Licenses and Certificates");
            var certificates = new List<Certificates>();
            if (!string.IsNullOrEmpty(userId))
            {
                ViewBag.UserID = userId;
                certificates = _userService.GetCertificates().Where(x => x.UserId == Convert.ToInt32(userId)).ToList();
            }
            if (!string.IsNullOrEmpty(vendorid))
            {
                ViewBag.VendorID = vendorid;
                certificates = _userService.GetCertificates().Where(x => x.VendorId == Convert.ToInt32(vendorid)).ToList();
            }
            if (string.IsNullOrEmpty(view) || view == "BinderView")
            {
                certificates = _userService.GetCertificates().ToList();
            }
            ViewBag.View = view;
            return View("Certificates", certificates);
        }

        public ActionResult AddOrEditCertificates(string Id, string userId, string vendorid, string view)
        {
            Certificates certificate = new Certificates();
            if (!string.IsNullOrEmpty(Id))
            {
                UISession.AddCurrentPage("user_addoreditcertificates", 0, "Edit Certificates");
                certificate = _userService.GetCertificates().FirstOrDefault(x => x.CertificateId == Convert.ToInt32(Id));
            }
            else
                UISession.AddCurrentPage("user_addoreditcertificates", 0, "Add Certificates");
            ViewBag.view = view;
            if (!string.IsNullOrEmpty(userId))
                ViewBag.UserIDCertificate = userId;
            if (!string.IsNullOrEmpty(vendorid))
                ViewBag.CertificateVendorId = vendorid;
            return View("~/Views/User/AddOrEditCertificates.cshtml", certificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult AddOrEditCertificates(Certificates certificate)
        {
            var certi = _userService.GetCertificates().FirstOrDefault(x => x.CertificateId == Convert.ToInt32(certificate.CertificateId));
            certificate.CreatedBy = UserSession.CurrentUser.UserId;

            DateTime dateTime = DateTime.UtcNow.Date;
            if (certificate.ValidUpTo < dateTime && certificate.IsActive && certificate.ValidUpTo != dateTime)
                certificate.IsActive = false;

            if (certi != null)
            {
                if (!string.IsNullOrEmpty(certificate.FileContent))
                    certificate.CreatedDate = dateTime;
                else
                    certificate.CreatedDate = certi.CreatedDate;

            }
            List<Certificates> lstCertificates = new List<Certificates>();
            if (certificate != null)
            {

                lstCertificates.Add(certificate);
            }
            string postData = JsonConvert.SerializeObject(lstCertificates);

            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            string result = _httpService.CallPostMethod(postData, APIUrls.Certificate_Add, ref StatusCode);
            var newresult = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);

            if (newresult.Message == "Certificate name already exist")
            {
                ErrorMessage = certificate.CertificateName + "  Certificate name already exist. ";

            }
            else
            {
                if (StatusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
                    SuccessMessage = ConstMessage.Saved_Successfully;
                else
                    ErrorMessage = ConstMessage.Internal_Server_Error;

            }

            if (certificate.View == "BinderView")
                return RedirectToAction("UserCertificates", new { view = certificate.View });
            if (certificate.View == "UserView" && certificate.UserId != null)
                return RedirectToAction("UserCertificates", new { userId = certificate.UserId, view = certificate.View });
            if (certificate.View == "UserView" && certificate.VendorId != null)
                return RedirectToAction("UserCertificates", new { vendorid = certificate.VendorId, view = certificate.View });
            return RedirectToAction("UserCertificates");
        }



        #endregion

        #region User Activity 

        public async Task<ActionResult> LoginHistory(int? userid)
        {
            UISession.AddCurrentPage("User_LoginHistory", 0, "Login History", true);
            var userLogins = await _loginService.GetUserLogins(userid.HasValue && userid > 0 ? userid.Value : UserSession.CurrentUser.UserId);
            return View(userLogins);
        }

        #endregion

        #region User Org Management 

        public ActionResult UserHospitals()
        {
            var orgList = _organizationService.GetOrganizations();
            return View(orgList);
        }

        public ActionResult UserHospitalView(int userId)
        {
            var orgList = _organizationService.GetOrganizations();
            var userOrgList = _organizationService.UserOrganizations(userId);
            ViewBag.UserOrgLists = userOrgList;
            return PartialView("_userHospitals", orgList);
        }

        public static bool IsChecked(Organization org, List<Organization> list)
        {
            if (list == null)
                return false;
            else
                return list.Any(x => x.ClientNo == org.ClientNo);
        }


        public ActionResult SaveUserHospital(int userid, int clientNo, bool isActive)
        {
            string userGroupIds = string.Join(",", UserSession.CurrentUser.UserGroups.Select(x => x.GroupId));
            _userService.AddUserToClient(userid, clientNo, isActive, userGroupIds);
            return Json("");

        }


        #endregion

        #region Switch User


        public ActionResult SwitchUser()
        {
            if (UserSession.CurrentUser.IsSystemUser || UserSession.IsSwitchUser)
            {
                UISession.AddCurrentPage("user_users", 0, "Switch Users", true);
                List<UserProfile> users = _userService.GetUsers().Where(x => x.IsActive).ToList();
                return View(users);
            }
            else
                return RedirectToRoute("unauthorized");
        }

        public async Task<IActionResult> ChangeUser(int userId)
        {
            return RedirectToAction(nameof(MainController.ChangeUser), "Main", new { userId });
        }

        #endregion

        #region UserGroup

        [HttpGet]
        public ActionResult UserGroup(string success)
        {
            UISession.AddCurrentPage("CRxSetting_Usergroup", 0, "User Group");
            var userGroups = _userService.GetUserGroups();
            if (!string.IsNullOrEmpty(success))
            {
                bool scs = int.TryParse(success, out int newId);
                if (scs)
                { if (newId > 0) { SuccessMessage = ConstMessage.Success_Updated; } else { ErrorMessage = (newId == -1) ? "This name already exists. Please use another name" : Utility.ConstMessage.Error; } }
                else
                { SuccessMessage = success; }
            }
            return View(userGroups);
        }

        [HttpGet]
        public ActionResult EditUserGroup(string id)
        {
            var userGroup = new UserGroup();
            if (string.IsNullOrEmpty(id))
                userGroup.CreatedBy = UserSession.CurrentUser.UserId;
            else
                userGroup = _userService.GetUserGroup(Convert.ToInt32(id));

            return View(userGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserGroup(UserGroup userGroup)
        {
            _userService.UpdateUserGroup(userGroup);
            return RedirectToAction("UserGroup");
        }


        #endregion

        #region user campus Assignment

        public ActionResult UserSites()
        {
            UISession.AddCurrentPage("user_userSites", 0, "Campus Assignment");
            var model = new UserSiteViewModel
            {
                Sites = _siteService.GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList(),
                Users = _userService.GetUsers(),
                UserSites = _userSiteService.GetUserSiteMappings().ToList(),
                VendorSites = _userSiteService.GetVendorSiteMappings().ToList()
            };
            return View(model);
        }

        // [ChildActionOnly]
        public ActionResult UserSiteView(int? userId, int? vendorId)
        {
            var userSiteViewModel = new UserSiteViewModel
            {
                Sites = _siteService.GetSites().Where(x => x.IsActive).ToList(),
                UserId = userId,
                VendorId = vendorId
            };
            if (userSiteViewModel.UserId.HasValue && userSiteViewModel.UserId.Value > 0)
                userSiteViewModel.SiteMapping = _userSiteService.GetUserSiteMappings(userSiteViewModel.UserId.Value).ToList();

            if (userSiteViewModel.VendorId.HasValue && userSiteViewModel.VendorId.Value > 0)
                userSiteViewModel.SiteMapping = _userSiteService.GetVendorSiteMappings(userSiteViewModel.VendorId.Value).ToList();

            return PartialView("_userSites", userSiteViewModel);

        }

        public ActionResult UserDrawingView(int? userId, int? vendorId, long[] selectedValues)
        {
            var data = _floorTypeService.GetFloorTypes().ToList();
            var model = new UserDrawingViewModel
            {
                Drawings = data,
                UserId = userId,
                VendorId = vendorId,
                SelectedItems = selectedValues
            };
            return PartialView("_userDrawings", model);
        }

        //[ChildActionOnly]
        public ActionResult UserPermitView(int? userId, int? vendorId, long[] selectedValues)
        {
            var listItems = new List<SelectListItem>();
            var lists = from BDO.Enums.PermitType e in Enum.GetValues(typeof(BDO.Enums.PermitType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };

            foreach (var item in lists.OrderBy(x => x.Text))
            {

                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                listItems.Add(selItem);
            }

            var model = new UserPermitViewModel
            {
                PermitTypes = listItems,
                UserId = userId,
                VendorId = vendorId,
                SelectedPermitTypes = selectedValues
            };
            return PartialView("_UserPermits", model);
        }

        [HttpPost]

        public ActionResult SaveUserSites(List<UserSiteMapping> userSiteData, int userId)
        {
            string siteAssignees = string.Empty;
            if (userSiteData.Count > 0)
            {
                siteAssignees = string.Join(",", userSiteData.Where(x => x.AssignedType != 0).Select(x => string.Format("{0}%{1}", x.SiteId, x.AssignedType)));

                if (userId > 0)
                    _userService.SaveUserSites(userId, siteAssignees, UserSession.CurrentUser.UserId);
            }
            return Json(userSiteData);
        }

        #endregion

        #region user Drawing Assignment

        public ActionResult UserDrawings()
        {
            UISession.AddCurrentPage("user_userDrawings", 0, "Drawings Assignment");
            var model = new UserDrawingViewModel
            {
                Drawings = _floorTypeService.GetFloorTypes(),
                Users = _userService.GetUsers(),
                Vendors = _vendorService.GetVendors().Where(x => x.IsActive).ToList()

            };
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]

        public ActionResult SaveUserDrawings(long[] drawingIds, int userId)
        {
            if (drawingIds != null)
                _userService.UpdateUserDrawing(userId, drawingIds);
            return Json(drawingIds);
        }

        #endregion

        #region Reset Password
        public async Task<ActionResult> ResetPassword(UserAddViewModel model)
        {
            var status = "Not Done";
            if (ModelState.IsValid)
            {
                UserProfile vmodel = new();
                vmodel = model.ToUserProfile();
                var token = Guid.NewGuid().ToString();
                status = "Done";
                if (vmodel.UserId > 0)
                {
                    _userService.ResetUserPassword(vmodel.UserId);
                }
                if (!string.IsNullOrEmpty(vmodel.Email))
                {

                    var user = await _userManager.FindByEmailAsync(vmodel.Email);
                    if (user != null)
                    {
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = vmodel.Id, code = code }, protocol: HttpContext.Request.Scheme);
                        token = _userService.PasswordResetQueue(vmodel.Email, 'E', vmodel.Email, token, Convert.ToString(BDO.Enums.passwordStatus.reset));
                        _emailService.SendMailOnResetPasswordRequest(vmodel, token, callbackUrl);
                    }
                    else
                        status = $"email {vmodel.Email} not found.";
                }
                else
                    status = "Fail";
            }
            else
                status = "Model not valid";
            return Json(status);
        }
        #endregion

        #region Unlock User
        public ActionResult UnlockUserPopUp()
        {
            var users = _userService.Get_lockingUsers().Where(x => !x.IsCRxUser).ToList();
            return PartialView("_LockedUserPopup", users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UnlocklUserLogin(int[] userids)
        {
            if (userids.Length > 0)
            {
                var Status = _userService.UnlocklUserLogin(string.Join(',', userids));
                var result = new { Result = Status };
                return Json(result);
            }
            return Json("No");
        }
        #endregion

        #region change password

        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    SuccessMessage = "Your password has been changed.";
                    return RedirectToAction(nameof(Users), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Users), new { Message = ManageMessageId.Error });
        }

        #endregion

        #region common

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<UserProfile> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion

        #region user codes

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLoginCodes()
        {
            var orgKeys = UserSession.CurrentOrg.Orgkey;
            List<UserLoginCodes> codes = _userCodeService.GetUserLoginCodes(orgKeys);
            return View(codes);
        }

        #endregion

    }
}