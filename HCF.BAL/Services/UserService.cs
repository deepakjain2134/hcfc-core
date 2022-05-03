using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using HCF.Utility;
using HCF.BAL.Interfaces.Services;
using System.Threading.Tasks;
using HCF.Utility.AppConfig;
using HCF.DAL.Contexts;
using HCF.DAL.Repository;

namespace HCF.BAL.Services
{
    public class UserService : IUserService
    {       
        private readonly IEmailService _emailService;
        private readonly IUsersRepository _usersRepository;
        private readonly IAppSetting _appSetting;
        private readonly IFileUpload _fileUpload;

        public UserService(IFileUpload fileUpload, IEmailService emailService, IUsersRepository usersRepository, IAppSetting appSetting)
        {
            _fileUpload = fileUpload;
            _appSetting = appSetting;
            _emailService = emailService;
            _usersRepository = usersRepository;
        }        

        public IList<UserProfile> GetUserList()
        {
            var result = _usersRepository.GetUsersList().Where(x => x.IsActive).ToList();
            return result;
        }

        public UserProfile GetUserList(int userId)
        {
            var result = _usersRepository.GetUsersList().FirstOrDefault(x =>  x.UserId== userId);
            return result;
        }

        public List<UserProfile> Get_lockingUsers()
        {
            return _usersRepository.Get_lockingUsers().ToList();
        }

        public bool IsInLastPasswords(int userId, string password)
        {
            var user = GetUserList().FirstOrDefault(x => x.UserId == userId);
            var dt = GetSaltFromEmailAddress(user.UserName);
            if (dt != null)
            {
                user.Salt = Guid.Parse(dt.Rows[0]["Salt"].ToString());               
                password = Conversion.Encrypt(password, Convert.ToString(user.Salt));
                return _usersRepository.IsInLastPasswords(userId, password);
            }
            return true;            
        }

       

        public List<UserProfile> GetMasterUsers(bool includeCRxUser = true)
        {
            if (!includeCRxUser)
                return _usersRepository.GetMasterUsers().Where(x => !x.IsCRxUser).ToList();
            else
                return _usersRepository.GetMasterUsers().ToList();
        }

      

        public IList<UserProfile> GetVendorUsers()
        {
            return _usersRepository.GetUsersList().Where(x => x.VendorId != null).ToList();
        }

        public IList<UserProfile> GetVendorUsers(int vendorId)
        {
            return _usersRepository.GetUsersList().Where(x => x.VendorId != null && x.VendorId == vendorId).ToList();
        }

        public bool UpdateUserDrawing(int userId, long[] drawingIds)
        {
            return _usersRepository.UpdateUserDrawing(userId, drawingIds);
        }

        public List<UserProfile> Get()
        {
            return _usersRepository.Get().ToList();
        }
        public UserProfile GetUser(int userId)
        {
            return _usersRepository.GetUser(userId);
        }

        public List<UserProfile> GetUsers(bool includeCRxUser = false)
        {
            if (!includeCRxUser)
                return _usersRepository.GetUsers().Where(x => !x.IsCRxUser).ToList();
            else
                return _usersRepository.GetUsers().ToList();
        }

        public IList<UserProfile> GetUsers(int[] userIds)
        {
            return GetUsers().Where(p => userIds.Contains(p.UserId)).ToList();
        }

        public int CreateUser(UserProfile userProfile, string siteAssigneeType)
        {
            userProfile.Salt = Guid.NewGuid();
            if (!string.IsNullOrEmpty(userProfile.Password))
                userProfile.Password = Conversion.Encrypt(userProfile.Password, Convert.ToString(userProfile.Salt));

            return _usersRepository.CreateUser(userProfile, siteAssigneeType);
        }

        public int AddCertificates(Certificates certificate)
        {
            return _usersRepository.AddCertificates(certificate);
        }


        public int CreateUserLoginForGU(UserLogin userLogin)
        {
            var salt = Guid.NewGuid();
            userLogin.RefreshToken = salt;
            return _usersRepository.CreateUserLoginForGU(userLogin);
        }


        public bool DeleteCertificates(int userId)
        {
            return _usersRepository.DeleteCertificates(userId);
        }

        public List<Certificates> GetCertificates()
        {
            return _usersRepository.GetCertificates();
        }

        public UserProfile AuthenticateUser(UserProfile user, string deviceToken, int deviceType, string version, string ipAddress, UserLogin loginInfo)
        {
            string userSalt = GetUserSalt(user.UserName);
            if (!string.IsNullOrEmpty(userSalt))
            {
                user.Salt = Guid.Parse(userSalt);
                user.Password = Conversion.Encrypt(user.Password, Convert.ToString(user.Salt));
                return _usersRepository.AuthenticateUser(user.UserName, user.Password, deviceToken, deviceType, version, ipAddress, loginInfo);
            }
            else     
                return user;
        }

        public string GetUserSalt(string userName)
        {
            string salt = string.Empty;
            DataTable dt = GetSaltFromEmailAddress(userName);
            if (dt != null && dt.Rows.Count > 0)
            {
                salt = dt.Rows[0]["Salt"].ToString();
            }
            return salt;
        }

        public async Task<UserProfile> GetRefreshtoken(string userId, string refreshToken)
        {
            return await _usersRepository.GetRefreshtoken(userId, refreshToken);
        }

        public bool LogOut(UserLogin loginUser)
        {
            return _usersRepository.LogOut(loginUser);
        }

        public bool IsResourceAllowed(int userId, object epId, object floorAssetId)
        {
            return _usersRepository.IsResourceAllowed(userId, epId, floorAssetId);
        }

        private DataTable GetSaltFromEmailAddress(string userEmail)
        {
            return _usersRepository.GetSaltFromEmailAddress(userEmail);
        }

        public IEnumerable<UserProfile> GetAllClientUsers()
        {
            return _usersRepository.GetAllClientUsers().ToList();
        }

        public UserProfile GetUserProfile(int userId)
        {
            return _usersRepository.GetUserProfile(userId);
        }

        public string PasswordResetQueue(string emailAddress, char recoveryMethod, string recoveryAddress, string token, string passwordRequestType)
        {
            return _usersRepository.PasswordResetQueue(emailAddress, recoveryMethod, recoveryAddress, token, passwordRequestType);
        }

        //public int UpdateUserLoginDetails(UserLogin userLoginData)
        //{
        //    return _usersRepository.UpdateUserLoginDetails(userLoginData);
        //}

        public bool IsValidRecoveryCode(string recoveryCode)
        {
            return _usersRepository.IsValidRecoveryCode(recoveryCode);
        }

        public bool IsValidInvitationIdCode(string recoveryCode)
        {
            return _usersRepository.IsValidInvitationIdCode(recoveryCode);
        }

        public VendorOrganizations GetVendorOrgInvitation(Guid token)
        {
            return _usersRepository.GetVendorOrgInvitation(token);
        }


        public string GetEmailAddressFromRecoveryCode(string recoveryCode)
        {
            return _usersRepository.GetEmailAddressFromRecoveryCode(recoveryCode);
        }

        public bool ChangeRecoverPassword(string emailAddress, string password, string salt, string recoveryCode)
        {
            return _usersRepository.ChangeRecoverPassword(emailAddress, password, salt, recoveryCode);
        }

        public bool FlagRecoveryCode(string recoveryCode)
        {
            return _usersRepository.FlagRecoveryCode(recoveryCode);
        }

        public bool UpdateUser(UserProfile newUser, string siteAssigneeType)
        {
            if (!string.IsNullOrEmpty(newUser.FileContent))
                newUser.FilePath = _fileUpload.SaveFileUsingContent(newUser.FileContent, newUser.FileName,
                    "UserImage", newUser.UserGuid.ToString(),
                    false).FilePath;

            return _usersRepository.UpdateUser(newUser, siteAssigneeType);
        }

        public bool UpdatePassword(int userId, string password, string newPassword)
        {
            var user = GetUser(userId);
            var dt = GetSaltFromEmailAddress(user.UserName);
            if (dt != null)
            {
                user.Salt = Guid.Parse(dt.Rows[0]["Salt"].ToString());
                user.UserId = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
                user.Password = dt.Rows[0]["Password"].ToString();
                user.UserGuid = Guid.Parse(dt.Rows[0]["UserProfileId"].ToString());
                password = Conversion.Encrypt(password, Convert.ToString(user.Salt));
                newPassword = Conversion.Encrypt(newPassword, Convert.ToString(user.Salt));
                return user.Password == password && _usersRepository.UpdatePassword(userId, newPassword, userId, false);
            }
            return false;
        }

        public bool UpdatePassword(string userName, string newPassword, int modifiedBy, bool isPwdChange)
        {
            var dt = GetSaltFromEmailAddress(userName);
            if (dt != null)
            {
                var salt = dt.Rows[0]["Salt"].ToString();
                var userId = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
                newPassword = Conversion.Encrypt(newPassword, salt);
                return _usersRepository.UpdatePassword(userId, newPassword, modifiedBy, isPwdChange);
            }
            return
              false;
        }

        public int SendRecoveryMail(string emailAddress, string emailBody, string webUrl)
        {
            int status;
            var sender = _appSetting.SenderMailFrom;
            var token = Guid.NewGuid().ToString();
            var type = "recover";
            token = PasswordResetQueue(emailAddress, 'E', emailAddress, token, Convert.ToString(BDO.Enums.passwordStatus.recover));
            // var url = $"{webUrl}user/password/{type}/{token}";
            var url = string.Format("<a href='{0}' title='Reset Your Account on HCF'>{0}</a>", webUrl);
            emailBody = emailBody.Replace("{ConfirmationLink}", url);
            var msg = _emailService.SendPasswordResetMail(emailAddress, "Reset Your Account on CRx", emailBody, sender);
            status = msg ? 1 : -1;
            return status;
        }

        public List<UserRecordsCount> GetUserRecords(int userId)
        {
            return _usersRepository.GetUserRecords(userId);
        }

        public int InsertorUpdateUserRole(UserRoles userRole)
        {
            return _usersRepository.InsertorUpdateUserRole(userRole);
        }

        public List<UserProfile> GetAllUsersFromMasterDb()
        {
            return _usersRepository.GetAllUsersFromMasterDb();
        }


        #region Security



        public string SaveUserViewHistory(Guid loginGuid, int userId, string currentPage)
        {
            return _usersRepository.SaveUserViewHistory(loginGuid, userId, currentPage);
        }


        //public bool CheckloginToken(Guid loginToken)
        //{
        //    return _usersRepository.CheckloginToken(loginToken);
        //}

        public string CheckloginToken(Guid loginToken, int userId, string requiredPermission, string currentPage)
        {
            return _usersRepository.CheckRequiredPermission(loginToken, userId, requiredPermission, currentPage);
        }

        public UserProfile GetUserEmailPassword(string userName)
        {
            return _usersRepository.GetUserEmailPassword(userName);
        }

        public List<Organization> GetUserOrgs(string userName)
        {
            return _usersRepository.GetUserOrgs(userName);
        }

        //public object GetUserLogins(Guid userProfileId) => _usersRepository.GetUserLogins(userProfileId);

        public int AddUserToClient(int userid, int clientNo, bool isActive, string userGroupIds)
        {
            return _usersRepository.AddUserToClient(userid, clientNo, isActive, userGroupIds);

        }

        public int CheckExistingEmail(string email, int userId)
        {
            return _usersRepository.CheckExistingEmail(email, userId);
        }
        public bool UserVendorCreated(int VendorId)
        {
            return _usersRepository.UserVendorCreated(VendorId);
        }
        #endregion

        #region User Group


        public int UpdateUserGroup(UserGroup usergroup)
        {
            return _usersRepository.UpdateUserGroup(usergroup);
        }

        public List<UserGroup> GetUserGroups()
        {
            return _usersRepository.UserGroups(false);
        }

        public List<UserGroup> GetUserGroupeAccess(int userId)
        {
            return _usersRepository.GetUserGroupeAccess(userId);
        }

        public UserGroup GetUserGroup(int id)
        {
            return _usersRepository.UserGroups(true).FirstOrDefault(x => x.GroupId == id);
        }

        public List<UserAdditionalRole> GetUserAdditionalRole()
        {
            return _usersRepository.UserAdditionalRole(false);
        }





        #endregion

        #region

        public List<UserRoles> GetUserRoles(string roleId)
        {
            return _usersRepository.GetUserRoles(roleId);
        }
        public int UpdateUserRole(UserRoles userRoles)
        {
            return _usersRepository.UpdateUserRole(userRoles);
        }

        public int UpdateUserRole(string roleId)
        {
            return _usersRepository.DeleteUserRole(roleId);
        }

        public bool SaveUserSites(int userId, string siteAssignees, int createdBy)
        {
            return _usersRepository.SaveUserSites(userId, siteAssignees, createdBy);
        }
       
        #endregion

        #region Reset User password

        public int ResetUserPassword(int userid)
        {

            return _usersRepository.ResetUserPassword(userid);
        }
        #endregion


        public UserProfile Userlockout(UserProfile user, int Islock)
        {


            return _usersRepository.Userlockout(user.UserName, Islock);


        }

         
        
        public UserProfile GetUserProfileforlockout(string UserName)
        {
            return _usersRepository.GetUserProfileforlockout(UserName);
        }


        public bool IsValidNewPassword(string password, string UserName)
        {
            return _usersRepository.IsValidNewPassword(password, UserName);
        }       

        public bool IsNewDevice(int UserId, string dToken, string ipAddress)
        {
            return _usersRepository.IsNewDevice(UserId, dToken, ipAddress);
        }
        public bool SendVerificationCode(string userName, string newdevicecode, string fullName)
        {

            return _emailService.SendVerificationCode(userName, newdevicecode, fullName);

        }
        public int UpdateNewDevice(int UserId, string token)
        {

            return _usersRepository.UpdateNewDevice(UserId, token);

        }

        public bool ValidateLastChanged(string userId)
        {
            return true;
        }


        public string UnlocklUserLogin(string userids)
        {
            return _usersRepository.UnlocklUserLogin(userids);
        }

        public List<Organization> GetUserOrgsByUserId(int Userid)
        {
            return _usersRepository.GetUserOrgsByUserId(Userid);
        }


        public UserLogin GetUserOrgsByRefreshToken(string refreshToken)
        {
            return _usersRepository.GetUserOrgsByRefreshToken(refreshToken);
        }

        public int DeleteUser(int userId)
        {
            return _usersRepository.DeleteUser(userId);
        }
    }
}
