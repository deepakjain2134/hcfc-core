using HCF.BDO;
using System;
using System.Linq;
using HCF.BAL;
using System.Collections.Generic;
using HCF.Web.Base;
using HCF.Web.Helpers;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        private readonly IRolesService _roleService;
        private readonly IOrganizationService _orgService;       
        private readonly IHttpPostFactory _httpService;
        private readonly ICommonModelFactory _commonModelFactory;

        public MenuController(ICommonModelFactory commonModelFactory,IMenuService menuService, IUserService userService, IRolesService roleService, IOrganizationService orgService, IHttpPostFactory httpService
           
            ) 
        {
            _commonModelFactory = commonModelFactory;           
            _menuService = menuService; 
            _userService = userService;
            _roleService = roleService; 
            _orgService = orgService; 
            _httpService = httpService;
        }
        // GET: Menu
        public ActionResult Index()
        {
            UISession.AddCurrentPage("CrxSetting_Menus", 0, "Menus");
            var currentMenus = _menuService.GetMenuMaster();
            return View(currentMenus);
        }

        public ActionResult ManageMenu(int menuId, string mode)
        {
            var currentMenus = new Menus();
            var User = _userService.GetUser(UserSession.CurrentUser.UserId);
            var isAdmin = User.UserGroups.Any(x => x.Name.Equals(BDO.Enums.UserGroup.Admin.ToString(), StringComparison.OrdinalIgnoreCase));
            ViewBag.isAdmin = isAdmin.ToString();
            var currentMainMenus = _menuService.GetMenuMaster().Where(m => m.ParentId == 0);
            switch (mode)
            {
                // update
                case "0":
                    ViewBag.PageTitle = "Update Menu";
                    currentMenus = _menuService.GetMenuMaster().FirstOrDefault(x => x.Id == menuId);
                    if (currentMenus.ParentId != 0)
                    {
                        ViewBag.UpdateFor = "Child Menu";
                        ViewBag.MainMenuID = menuId;
                        currentMenus.MainMenu = new SelectList(currentMainMenus, "Id", "Name");
                    }
                    break;
                case "2":
                    ViewBag.PageTitle = "Add Menu";
                    currentMenus.ParentId = 0;
                    currentMenus.Id = 0;
                    break;
                default:
                    ViewBag.PageTitle = "Add New Child Menu";
                    ViewBag.UpdateFor = "Child Menu";
                    ViewBag.MainMenuID = menuId;
                    currentMenus.MainMenu = new SelectList(currentMainMenus, "Id", "Name");
                    break;
            }
            return PartialView(currentMenus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageMenu(Menus currentMenus)
        {
            currentMenus.CreatedBy = UserSession.CurrentUser.UserId;
            var postData = _commonModelFactory.JsonSerialize<Menus>(currentMenus);
            var uri = Utility.APIUrls.Menu_ManageMenu;
            var statusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            var result = _httpService.CallPostMethod(postData, uri, ref statusCode);
            if (statusCode == Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success))
            {
                var httpResponse = _commonModelFactory.JsonDeserialize<HttpResponseMessage>(result);
                if (httpResponse.Success)
                    SuccessMessage = httpResponse.Message;
                else
                    ErrorMessage = httpResponse.Message;
            }
            else
                ErrorMessage = Utility.ConstMessage.Internal_Server_Error;
            return RedirectToAction("Index");
        }

        public JsonResult UpdateOrders(string newOrder)
        {
            _menuService.UpdateMenuOrders(newOrder);
            return Json(newOrder);
        }


        #region Usermenu configurations

        public ActionResult UsermenuConfigurations()
        {
            OrgServices orgServices = new OrgServices();
            UISession.AddCurrentPage("Usermenu_UsermenuConfiguratons", 0, "Config Module");

            var menulist = _menuService.GetMenus().Where(x => x.Type == 0).ToList();
            var organizationlist = _orgService.GetOrganizations().Where(x => x.IsActive && !x.ParentOrgKey.HasValue).OrderBy(x => x.Name).ToList();
            ViewBag.MenuSelectList = new SelectList(menulist, "Id", "Name", "select");
            ViewBag.OrganizationSelectList = new SelectList(organizationlist, "ClientNo", "Name", "select");
            return View(orgServices);
        }


        public ActionResult GetData(string mode, int id)
        {
            if (mode.ToLower() == "menu")
            {
                var menu = _menuService.GetHCFMenus(id);
                //ViewBag.MenuList = menu;
                return PartialView("_OrganizationData", menu);
            }
            else if (mode.ToLower() == "organization")
            {
                var organization = _orgService.GetMenuOrganization(id);
                // ViewBag.Organizationlist = organization;
                return PartialView("_MenuData", organization);
            }
            else if (mode.ToLower() == "module")
            {
                var menu = _menuService.GetAllModules(id);
                ViewBag.IsShowOnOff = true;
                return PartialView("_ModuleData", menu);
            }
            return new EmptyResult();
        }

        public ActionResult GetModules()
        {
            UISession.AddCurrentPage("Menu_GetModules", 0, "My Plan",true);
            return View();
        }

        public ActionResult GetModulesData()
        {
            int clientno = HCF.Web.Base.UserSession.CurrentOrg.ClientNo;
            var modules = _menuService.GetAllModules(clientno);
            return PartialView("_ModuleData", modules);
        }



        public JsonResult UpdateMenuOrganization(int? menuId, int? moduleId, int organizationId, bool IsActive, int serviceMode)
        {
            var newUsermenu = new OrgServices
            {
                MenuID = menuId,
                OrganizationKey = organizationId,
                Status = IsActive,
                ModuleId = moduleId,
                Createdby = UserSession.CurrentUser.UserId,
                ServiceMode = serviceMode
            };
            _orgService.InsertMenuOrganization(newUsermenu);
            return Json("");
        }

        #endregion

        #region UserRole

        public ActionResult UserRole()
        {
            UISession.AddCurrentPage("Menu_UserRole", 0, "User Roles");
            var userlist = _userService.GetUsers().Where(x => x.IsActive == true && !string.IsNullOrEmpty(x.Email));
            return View(userlist);
        }

        public ActionResult GetUserData(int Userid)
        {
            var user = _userService.GetUser(Userid);
            var roles = user.Roles.GroupBy(x => x.RoleId).Select(g => g.OrderByDescending(x => x.RoleId).First()).ToList();
            user.Roles = roles;
            var allRoles = _roleService.GetRoles();
            List<Roles> allUnAssigned = (from lst1 in allRoles
                                         where !user.Roles.Where(x => x.IsAdditionalRole == 0).Any(
                                                           x => x.RoleId == lst1.RoleId)
                                         select lst1).ToList();
            ViewBag.OtherRoles = allUnAssigned;
            return PartialView("_GetUserData", user);
        }

        // [CrxAuthorize(Roles = "Menu_UpdateUserrole")]
        public JsonResult UpdateUserrole(int userID, int roleID, bool status)
        {
            var userrole = new UserRoles
            {
                UserId = userID,
                RoleId = roleID,
                Status = status
            };
            _userService.InsertorUpdateUserRole(userrole);
            return Json("");
        }
        #endregion

        #region Add or Edit User Role

        public ActionResult Roles()
        {
            UISession.AddCurrentPage("Menu_Roles", 0, "Roles");
            var userRole = _userService.GetUserRoles("0");

            var root = userRole.GenerateTree(c => c.RoleId, c => c.ParentId);
            ViewBag.RoleMaster = root;

            return View(userRole);
        }
        public ActionResult AddorEditRoles(string roleId, string parentRoleId)
        {
            var userRole = new UserRoles();
            userRole.CreatedBy = UserSession.CurrentUser.UserId;
            userRole.IsActive = true;
            if (!String.IsNullOrEmpty(roleId))
            {
                if (int.TryParse(roleId, out int id))
                {
                    userRole = _userService.GetUserRoles(roleId).FirstOrDefault(x => x.RoleId == id);
                }
                else
                {
                    ErrorMessage = "This Roles ID doesn't exists.";
                    return View(userRole);
                }
            }

            else if (!String.IsNullOrEmpty(parentRoleId))
            {
                if (int.TryParse(parentRoleId, out int id))
                {
                    userRole = _userService.GetUserRoles(roleId).FirstOrDefault(x => x.RoleId == id);
                    userRole.RoleId = 0;
                    userRole.ParentId = id;
                }
                else
                {
                    ErrorMessage = "This Roles ID doesn't exists.";
                    return View(userRole);
                }
            }
            return View(userRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddorEditRoles(UserRoles userRole)
        {
            var newId = _userService.UpdateUserRole(userRole);
            if (newId == 0)
                ErrorMessage = "Display Text or Roles Name already exists";
            else
                SuccessMessage = "Role Updated successfully";

            return RedirectToAction("Roles", userRole.RoleId);
        }

        public ActionResult DeleteRoles(string roleId)
        {
            var newId = _userService.UpdateUserRole(roleId);
            if (newId == 0)
                ErrorMessage = "Role Not Deleted.";
            else
                SuccessMessage = "Role Updated successfully.";

            return RedirectToAction("UserRoles", roleId);
        }

        public ActionResult ActionNameDropdown(string controllerName)
        {
            return PartialView("_ActionNameDropdown", controllerName);
        }
        #endregion
    }
}