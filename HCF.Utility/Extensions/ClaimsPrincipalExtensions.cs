using System;
using System.Security.Claims;

namespace HCF.Utility
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("sub");
            //return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetFirstName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("FirstName");
        }

        public static string GetLastName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("LastName");
        }

        public static string GetVendorId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("VendorId");
        }

        public static bool IsCurrentUser(this ClaimsPrincipal principal, string id)
        {
            var currentUserId = GetUserId(principal);
            return string.Equals(currentUserId, id, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetCurrentDb(this ClaimsPrincipal principal)
        {
            var orgDbName = principal.FindFirstValue(SessionKey.OrgName);
            return orgDbName;
        }

        public static string GetUserGroups(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Role);
        }




        public static string GetLogOnToken(this ClaimsPrincipal principal)
        {
            var logOnToken = principal.FindFirstValue(SessionKey.LogOnToken);
            return logOnToken;
        }
        public static string GetPowerUser(this ClaimsPrincipal principal)
        {
            var IsPowerUser = principal.FindFirstValue("IsPowerUser");
            return IsPowerUser;
        }
        public static string GetCRxUser(this ClaimsPrincipal principal)
        {
            var IsCRxUser = principal.FindFirstValue("IsCRxUser");
            return IsCRxUser;
        }
        public static string GetSystemUser(this ClaimsPrincipal principal)
        {
            var IsSystemUser = principal.FindFirstValue("IsSystemUser");
            return IsSystemUser;
        }
        public static string GetUserGroupsids(this ClaimsPrincipal principal)
        {
            var UserAdditionalRoleIds= principal.FindFirstValue("RoleIds");
            return UserAdditionalRoleIds;
        }
        public static string GetUserRoles(this ClaimsPrincipal principal)
        {
            var UserAdditionalRoleIds = principal.FindFirstValue("Roles");
            return UserAdditionalRoleIds;
        }
        public static string GetClientNo(this ClaimsPrincipal principal)
        {
            var Clientno = principal.FindFirstValue("Clientno");
            return Clientno;
        }
        public static string GetPhoneNumber(this ClaimsPrincipal principal)
        {
            var PhoneNumber = principal.FindFirstValue("PhoneNumber");
            return PhoneNumber;
        }
        public static string GetUserProfileId(this ClaimsPrincipal principal)
        {
            var UserProfileId = principal.FindFirstValue("UserProfileId");
            return UserProfileId;
        }
        public static string GetUserAdditionalRoleIds(this ClaimsPrincipal principal)
        {
            var UserAdditionalRoleIds = principal.FindFirstValue("UserAdditionalRoleIds");
            return UserAdditionalRoleIds;
        }
        public static string GetOrgKey(this ClaimsPrincipal principal)
        {
            var orgKey = principal.FindFirstValue("Orgkey");
            return orgKey;
        }
    }
}
