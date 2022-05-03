using HCF.BDO;
using System;
using System.Linq;
using System.Collections.Generic;
using HCF.BAL;
using HCF.Utility;
using HCF.Utility.Configuration;
using Microsoft.AspNetCore.Http;
using HCF.Module.Core.Extensions;
using System.ServiceModel.Channels;

namespace HCF.Web.Base
{
    public static class UserSession
    {
        public static int GetUserId()
        {
            return CurrentUser.UserId;
        }

        public static UserProfile CurrentUser
        {
            get
            {

                var currentUser = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<UserProfile>(SessionKey.User);
                var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                if (currentUser == null)
                    currentUser = new UserProfile();


                //var userOrg = ServiceLocator.ServiceProvider.GetService<IWorkContext>().GetCurrnetOrg().Result;
                //var currentUserOrgSession = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<string>(SessionKey.ClientDBName);
                //if (currentUserOrgSession == null && userOrg.ClientNo > 0)
                //{
                //    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.ClientDBName, userOrg.DbName);
                //    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.ClientNo, userOrg.ClientNo);
                //}

                if (currentUser.UserId == 0 && contextUser.GetUserId() != "0")
                {
                    var userId = contextUser.GetUserId();
                    currentUser = ServiceLocator.ServiceProvider.GetService<IUserService>().GetUser(Convert.ToInt32(userId));
                    if (currentUser == null)
                        return new UserProfile();
                    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.User, currentUser);
                }
                return currentUser;

                //var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                //if (contextUser.Identity.IsAuthenticated)
                //{
                //    currentUser.UserId = Convert.ToInt32(contextUser.GetUserId());
                //    currentUser.Email = contextUser.GetUserEmail();
                //    currentUser.FirstName = contextUser.GetFirstName();
                //    currentUser.LastName = contextUser.GetLastName();
                //    currentUser.VendorId = Convert.ToInt32(contextUser.GetVendorId());
                //    currentUser.Vendor = new Vendors();
                //    currentUser.UserGroups = new List<UserGroup>();
                //    var userGroups = contextUser.GetUserGroups();
                //    if (userGroups != null)
                //    {
                //        foreach (var item in userGroups.Split(','))
                //        {
                //            if (!string.IsNullOrEmpty(item))
                //            {
                //                var userGroup = new UserGroup
                //                {
                //                    Name = item
                //                };
                //                currentUser.UserGroups.Add(userGroup);
                //            }
                //        }
                //    }
                //    currentUser.IsPowerUser = Convert.ToBoolean(contextUser.GetPowerUser());
                //    currentUser.IsCRxUser = Convert.ToBoolean(contextUser.GetCRxUser());
                //    currentUser.IsSystemUser = Convert.ToBoolean(contextUser.GetSystemUser());
                //    var userGroupsids = contextUser.GetUserGroupsids();
                //    if (!string.IsNullOrEmpty(userGroupsids))
                //    {
                //        foreach (var item in userGroupsids.Split(','))
                //        {
                //            if (!string.IsNullOrEmpty(item))
                //            {
                //                var userGroup = new UserGroup();
                //                userGroup.GroupId = Convert.ToInt32(item);
                //                currentUser.UserGroups.Add(userGroup);
                //            }
                //        }
                //    }

                //    var userRoles = contextUser.GetUserRoles();
                //    if (!string.IsNullOrEmpty(userRoles))
                //    {

                //        foreach (var item in userRoles.Split(','))
                //        {
                //            if (!string.IsNullOrEmpty(item))
                //            {
                //                var UserRoles = new Roles();
                //                UserRoles.RoleName = item;
                //                currentUser.Roles.Add(UserRoles);
                //            }
                //        }
                //    }
                //    currentUser.ClientNo = Convert.ToInt32(contextUser.GetClientNo());
                //    currentUser.PhoneNumber =contextUser.GetPhoneNumber();
                //    if (!string.IsNullOrEmpty(contextUser.GetUserProfileId()))
                //    {
                //        currentUser.UserGuid = Guid.Parse(contextUser.GetUserProfileId());
                //    }
                //    var userAdditionalRoleIds = contextUser.GetUserAdditionalRoleIds();
                //    if (!string.IsNullOrEmpty(userAdditionalRoleIds))
                //    {
                //        currentUser.UserAdditionalRoleIds = userAdditionalRoleIds;
                //    }
                //}
                //return currentUser;
            }
        }

        public static bool IsSwitchUser
        {
            get
            {
                var isSwitchUser = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<bool>(SessionKey.IsSwitchUser);
                return isSwitchUser;
            }
        }

        public static List<Organization> UserOrganizations
        {
            get
            {
                List<Organization> orgs = new();
                var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                if (contextUser!=null && contextUser.Identity!=null && contextUser.Identity.IsAuthenticated)
                {
                    var userId = contextUser.GetUserId();
                    orgs = ServiceLocator.ServiceProvider.GetService<IUserService>().GetUserOrgsByUserId(Convert.ToInt32(userId));
                }
                return orgs;
            }
        }

        public static string GetUserImage()
        {
            var profileImage = "/dist/Images/User/default.png";
            if (!string.IsNullOrEmpty(CurrentUser.FilePath))
                profileImage = ServiceLocator.ServiceProvider.GetService<ICommonProvider>().FilePath(CurrentUser.FilePath);
            return profileImage;
        }

        public static string UserOrg
        {
            get
            {
                var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                if (contextUser.Identity.IsAuthenticated)
                    return contextUser.GetCurrentDb();
                else
                    return string.Empty;
            }
        }

        public static string InboxEmail
        {
            get
            {
                if (CurrentOrg.PopEmails != null)
                {
                    var popEmails = CurrentOrg.PopEmails.FirstOrDefault(x => x.IsActive);
                    return popEmails != null ? popEmails.EmailId : "";
                }
                else return "";
            }

        }

        public static string LogOnToken
        {
            get
            {
                var loginToken = ServiceLocator.ServiceProvider.GetService<IWorkContext>().GetLoginToken().Result;
                if (loginToken != null)
                    return loginToken;
                else
                    return string.Empty;
            }
        }

        public static bool IsInRole(string requiredPermission)
        {
            var currentUser = CurrentUser;
            if (currentUser.IsSystemUser || currentUser.IsCRxUser || currentUser.IsPowerUser)
                return true;
            else
            {
                var userRoles = currentUser.Roles;
                var status = userRoles.Any(x => string.Equals(x.RoleName.Trim(), requiredPermission, StringComparison.CurrentCultureIgnoreCase));
                return status;
            }
        }

        public static bool HasRoleForMenu(string requiredPermission)
        {
            var userRoles = CurrentUser.Roles;
            var status = userRoles.Any(x => string.Equals(x.RoleName.Trim(), requiredPermission, StringComparison.CurrentCultureIgnoreCase));
            return status;
        }

        public static bool IsInfectionistRole(int? hasInfectionistRole = 0)
        {
            if (hasInfectionistRole == 1)
            {
                if (CurrentUser != null && (!string.IsNullOrEmpty(CurrentUser.UserAdditionalRoleIds) && CurrentUser.UserAdditionalRoleIds.Contains("2")))
                    return true;
                else
                    return false;
            }
            else
            {
                if (CurrentUser != null && (CurrentUser.IsSystemUser ||
                    CurrentUser.IsCRxUser ||
                    CurrentUser.IsPowerUser ||
                   (!string.IsNullOrEmpty(CurrentUser.UserAdditionalRoleIds) && CurrentUser.UserAdditionalRoleIds.Contains("2"))))
                    return true;
                else
                    return false;
            }

        }

        public static bool IsPowerUser()
        {
            return CurrentUser.UserId > 0 && CurrentUser.IsPowerUser;
        }

        public static bool IsEPAllowed(int epId)
        {
            var status = ServiceLocator.ServiceProvider.GetService<IUserService>().IsResourceAllowed(CurrentUser.UserId, epId, null);
            return status;
        }

        public static Organization CurrentOrg
        {
            get
            {
                var userOrg = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<Organization>(SessionKey.currentorg);
                if (userOrg == null)
                {
                    var contextUser = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
                    var dbName = contextUser.GetCurrentDb();
                    userOrg = ServiceLocator.ServiceProvider.GetService<IOrganizationService>().GetMasterOrganization(dbName);
                    if (userOrg == null)
                        return new Organization();

                    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.currentorg, userOrg);
                }
                return userOrg;
            }
        }

        public static List<Tips> PageHelp
        {
            get
            {
                var pageHelps = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<List<Tips>>(SessionKey.HelpTexts);
                if (pageHelps == null)
                {
                    pageHelps = ServiceLocator.ServiceProvider.GetService<ITipsService>().GetAllTips(CurrentOrg.ClientNo);
                    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.HelpTexts, pageHelps);
                }
                return pageHelps;
            }
        }

        public static bool HasRoles(string roles)
        {
            bool bFound = false;
            if (CurrentUser.IsPowerUser || CurrentUser.IsSystemUser)
                bFound = true;
            else
                bFound = CurrentUser.HasRoles(roles);

            return bFound;
        }

        public static bool HasPermission(string permission)
        {
            return CurrentUser.HasPermission(permission);
        }

    }
}