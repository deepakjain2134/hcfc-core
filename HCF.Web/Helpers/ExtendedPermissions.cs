//using System.Web.Mvc;

//namespace HCF.Web.Helpers
//{
//    public static class ExtendedPermissions
//    {
//        public static bool HasRole(this ControllerBase controller, string role)
//        {
//            bool Found = false;
//            try
//            {
//                //Check if the requesting user has the specified role...
//                Found = Base.UserSession.CurrentUser.HasRole(role);
//            }
//            catch { }
//            return Found;
//        }

//        public static bool HasRoles(this ControllerBase controller, string roles)
//        {
//            bool bFound = false;
//            try
//            {
//                //Check if the requesting user has any of the specified roles...
//                //Make sure you separate the roles using ';' (ie "Sales Manager;Sales Operator")
//                if (Base.UserSession.CurrentUser.IsPowerUser || Base.UserSession.CurrentUser.IsSystemUser)
//                {
//                    bFound = true;
//                }
//                else
//                {
//                    bFound = Base.UserSession.CurrentUser.HasRoles(roles);
//                }
               
//            }
//            catch { }
//            return bFound;
//        }

//        public static bool HasPermission(this ControllerBase controller, string permission)
//        {
//            bool Found = false;
//            try
//            {
//                //Check if the requesting user has the specified application permission...
//                Found = Base.UserSession.CurrentUser.HasPermission(permission);
//            }
//            catch { }
//            return Found;
//        }

//        public static bool IsSysAdmin(this ControllerBase controller)
//        {
//            bool IsSysAdmin = false;
//            try
//            {
//                //Check if the requesting user has the System Administrator privilege...
//                IsSysAdmin = Base.UserSession.CurrentUser.IsSystemUser;
//            }
//            catch { }
//            return IsSysAdmin;
//        }

    

//    }
//}