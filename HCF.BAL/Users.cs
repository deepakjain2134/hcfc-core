//using HCF.BDO;
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class Users
//    {        
//        public static List<UserProfile> Get()
//        {
//            return DAL.Users.Get().ToList();
//        }
//        public static UserProfile GetUser(int userId)
//        {
//            return DAL.Users.GetUser(userId);
//        }

//        public static int CreateUser(UserProfile userProfile, string siteAssigneeType)
//        {
//            userProfile.Salt = Guid.NewGuid();          
//            if (!string.IsNullOrEmpty(userProfile.Password))           
//                userProfile.Password = Conversion.Encrypt(userProfile.Password, Convert.ToString(userProfile.Salt)); 
         
//            return DAL.Users.CreateUser(userProfile,siteAssigneeType);
//        }

//        public static int AddCertificates(Certificates certificate)
//        {
//            return DAL.Users.AddCertificates(certificate);
//        }


//        //public static int CreateUserLoginForGU(UserLogin userLogin)
//        //{
//        //    var salt = Guid.NewGuid();
//        //    userLogin.RefereshToken = salt;
//        //    return DAL.Users.CreateUserLoginForGU(userLogin);
//        //}


//        //public static bool DeleteCertificates(int userId)
//        //{
//        //    return DAL.Users.DeleteCertificates(userId);
//        //}

//        //public static List<Certificates> GetCertificates()
//        //{
//        //    return DAL.Users.GetCertificates();
//        //}

//        public static UserProfile AuthenticateUser(UserProfile user, string deviceToken, int deviceType, string version,string ipAddress)
//        {
//            DataTable dt = GetSaltFromEmailAddress(user.UserName);
//            if (dt != null && dt.Rows.Count > 0)
//            {
//                user.Salt = Guid.Parse(dt.Rows[0]["Salt"].ToString());
//                user.Password = Conversion.Encrypt(user.Password, Convert.ToString(user.Salt));
//                return DAL.Users.AuthenticateUser(user.UserName, user.Password, deviceToken, deviceType, version, ipAddress);
//            }
//            else
//                return user;
//        }

//        public static UserProfile GetRefreshtoken(string userId, string refreshToken)
//        {
//            return DAL.Users.GetRefreshtoken(userId, refreshToken);
//        }

//        public static bool LogOut(UserLogin loginUser)
//        {
//            return DAL.Users.Instance.LogOut(loginUser);
//        }

//        public static bool IsResourceAllowed(int userId, object epId, object floorAssetId)
//        {
//            return DAL.Users.Instance.IsResourceAllowed(userId, epId, floorAssetId);
//        }

//        private static DataTable GetSaltFromEmailAddress(string userEmail)
//        {
//            return DAL.Users.GetSaltFromEmailAddress(userEmail);
//        }

//        public static List<UserProfile> GetUsers()
//        {
//            return DAL.Users.GetUsers().ToList();
//        }
     
//        public static IEnumerable<UserProfile> GetAllClientUsers()
//        {
//            return DAL.Users.GetAllClientUsers().ToList();
//        }

//        public static UserProfile GetUserProfile(int userId)
//        {
//            return DAL.Users.GetUserProfile(userId);
//        }

//        private static string PasswordResetQueue(string emailAddress, char recoveryMethod, string recoveryAddress, string token)
//        {
//            return DAL.Users.PasswordResetQueue(emailAddress, recoveryMethod, recoveryAddress, token);
//        }

//        //public static int UpdateUserLoginDetails(UserLogin userLoginData)
//        //{
//        //    return DAL.Users.UpdateUserLoginDetails(userLoginData);
//        //}

//        //public static bool IsValidRecoveryCode(string recoveryCode)
//        //{
//        //    return DAL.Users.IsValidRecoveryCode(recoveryCode);
//        //}

//        //public static bool IsValidInvitationIdCode(string recoveryCode)
//        //{
//        //    return DAL.Users.IsValidInvitationIdCode(recoveryCode);
//        //}

//        //public static VendorOrganizations GetVendorOrgInvitation(Guid token)
//        //{
//        //    return DAL.Users.GetVendorOrgInvitation(token);
//        //}


//        //public static string GetEmailAddressFromRecoveryCode(string recoveryCode)
//        //{
//        //    return DAL.Users.GetEmailAddressFromRecoveryCode(recoveryCode);
//        //}

//        //public static bool ChangeRecoverPassword(string emailAddress, string password, string salt, string recoveryCode)
//        //{
//        //    return DAL.Users.ChangeRecoverPassword(emailAddress, password, salt, recoveryCode);
//        //}

//        //public static bool FlagRecoveryCode(string recoveryCode)
//        //{
//        //    return DAL.Users.FlagRecoveryCode(recoveryCode);
//        //}

//        public static bool UpdateUser(UserProfile newUser,string siteAssigneeType)
//        {
//            return DAL.Users.UpdateUser(newUser, siteAssigneeType);
//        }

//        public static bool UpdatePassword(int userId, string password, string newPassword)
//        {
//            var user = GetUser(userId);
//            var dt = GetSaltFromEmailAddress(user.UserName);
//            if (dt != null)
//            {
//                user.Salt = Guid.Parse(dt.Rows[0]["Salt"].ToString());
//                user.UserId = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
//                user.Password = dt.Rows[0]["Password"].ToString();
//                user.UserProfileId = Guid.Parse(dt.Rows[0]["UserProfileId"].ToString());
//                password = Conversion.Encrypt(password, Convert.ToString(user.Salt));
//                newPassword = Conversion.Encrypt(newPassword, Convert.ToString(user.Salt));
//                return user.Password == password && DAL.Users.UpdatePassword(userId, newPassword, userId, false);
//            }
//            return false;
//        }

//        public static bool UpdatePassword(string userName, string newPassword, int modifiedBy, bool isPwdChange)
//        {
//            var dt = GetSaltFromEmailAddress(userName);
//            if (dt != null)
//            {
//                var salt = dt.Rows[0]["Salt"].ToString();
//                var userId = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
//                newPassword = Conversion.Encrypt(newPassword, salt);
//                return DAL.Users.UpdatePassword(userId, newPassword, modifiedBy, isPwdChange);
//            }
//            return
//              false;
//        }

//        public static List<DashboardDetails> GetDashBoardData(int userId)
//        {
//            return DAL.Users.GetDashBoardData(userId);
//        }

//        //public static int SendRecoveryMail(string emailAddress, string emailBody, string webUrl)
//        //{
//        //    int status;
//        //    var sender = AppSettings.SenderMailFrom;
//        //    var token = Guid.NewGuid().ToString();
//        //    token = PasswordResetQueue(emailAddress, 'E', emailAddress, token);
//        //    var url = $"{webUrl}{token}";
//        //    url = string.Format("<a href='{0}' title='Reset Your Account on HCF'>{0}</a>", url);
//        //    emailBody = emailBody.Replace("{ConfirmationLink}", url);
//        //    var msg = Email.SendPasswordResetMail(emailAddress, "Reset Your Account on CRx", emailBody, sender);
//        //    status = msg ? 1 : -1;
//        //    return status;

//        //}

//        public static List<UserRecordsCount> GetUserRecords(int userId)
//        {
//            return DAL.Users.GetUserRecords(userId);
//        }       

//        //public static int InsertorUpdateUserRole(UserRoles userRole)
//        //{
//        //    return DAL.Users.InsertorUpdateUserRole(userRole);
//        //}

//        #region UserGroup

//        public static List<UserGroup> GetUserGroups()
//        {
//            return DAL.Users.UserGroups(false);
//        }
//        //public static List<UserAdditionalRole> GetUserAdditionalRole()
//        //{
//        //    return DAL.Users.UserAdditionalRole(false);
//        //}
//        //public static List<UserGroup> GetAllUserGroups()
//        //{
//        //    return DAL.Users.UserGroups(true);
//        //}

//        #endregion

//        #region Security

//        public static string CheckloginToken(Guid loginToken, int userId, string requiredPermission,string currentPage)
//        {
//            return DAL.Users.CheckRequiredPermission(loginToken, userId, requiredPermission, currentPage);
//        }

//        //public static string SaveUserViewHistory(Guid loginGuid, int userId, string currentPage)
//        //{
//        //    return DAL.Users.SaveUserViewHistory(loginGuid, userId, currentPage);
//        //}


//        public static bool CheckloginToken(Guid loginToken)
//        {
//            return DAL.Users.CheckloginToken(loginToken);
//        }

//        public static UserProfile GetUserEmailPassword(string userName)
//        {
//            return DAL.Users.GetUserEmailPassword(userName);
//        }

//        //public static List<Organization> GetUserOrgs(string userName)
//        //{
//        //    return DAL.Users.GetUserOrgs(userName);
//        //}

//        //public static object GetUserLogins(Guid userProfileId) => DAL.Users.GetUserLogins(userProfileId);

//        //public static int AddUserToClient(int userid, int clientNo, bool isActive, string userGroupIds)
//        //{
//        //    return DAL.Users.AddUserToClient(userid, clientNo, isActive, userGroupIds);

//        //}

//        //public static bool CheckExistingEmail(string email, int userId)
//        //{
//        //    return DAL.Users.CheckExistingEmail(email, userId);
//        //}
//        //public static bool UserVendorCreated(int VendorId)
//        //{
//        //    return DAL.Users.UserVendorCreated(VendorId);
//        //}
//        #endregion

//        #region User Group
//        //public static List<UserGroup> GetUserGroup(string id)
//        //{
//        //    return DAL.Users.GetUserGroup(id);
//        //}

//        //public static int UpdateUserGroup(UserGroup usergroup)
//        //{

//        //    return DAL.Users.UpdateUserGroup(usergroup);
//        //}

       

//        #endregion

//        #region

//        public static List<UserRoles> GetUserRoles(string roleId)
//        {
//            return DAL.Users.GetUserRoles(roleId);
//        }
//        //public static int UpdateUserRole(UserRoles userRoles)
//        //{
//        //    return DAL.Users.UpdateUserRole(userRoles);
//        //}

//        //public static int UpdateUserRole(string roleId)
//        //{
//        //    return DAL.Users.DeleteUserRole(roleId);
//        //}

//        //public static bool SaveUserSites(int userId, string siteAssignees,int createdBy)
//        //{
//        //    return DAL.Users.SaveUserSites(userId, siteAssignees, createdBy);
//        //}
//        #endregion
//    }
//}