using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public partial interface IUserService
    {
        string GetUserSalt(string userName);
        IList<UserProfile> GetUserList();
        UserProfile GetUserList(int userId);
        IList<UserProfile> GetVendorUsers();
        IList<UserProfile> GetVendorUsers(int vendorId);
        IList<UserProfile> GetUsers(int[] userIds);
        List<UserProfile> GetUsers(bool includeCRxUser = true);
        bool UpdateUserDrawing(int userId, long[] drawingIds);
        int AddCertificates(Certificates certificate);
        int AddUserToClient(int userid, int clientNo, bool isActive, string userGroupIds);
        UserProfile AuthenticateUser(UserProfile user, string deviceToken, int deviceType, string version, string ipAddress, UserLogin loginInfo);
        bool ChangeRecoverPassword(string emailAddress, string password, string salt, string recoveryCode);
        int CheckExistingEmail(string email, int userId);
        string CheckloginToken(Guid loginToken, int userId, string requiredPermission, string currentPage);
        int CreateUser(UserProfile userProfile, string siteAssigneeType);
        int CreateUserLoginForGU(UserLogin userLogin);
        bool DeleteCertificates(int userId);
        bool FlagRecoveryCode(string recoveryCode);
        List<UserProfile> Get();
        IEnumerable<UserProfile> GetAllClientUsers();
        List<Certificates> GetCertificates();
        string GetEmailAddressFromRecoveryCode(string recoveryCode);
        Task<UserProfile> GetRefreshtoken(string userId, string refreshToken);
        UserProfile GetUser(int userId);
        List<UserAdditionalRole> GetUserAdditionalRole();
        UserProfile GetUserEmailPassword(string userName);
        List<Organization> GetUserOrgs(string userName);
        UserProfile GetUserProfile(int userId);
        List<UserRecordsCount> GetUserRecords(int userId);
        List<UserRoles> GetUserRoles(string roleId);
        List<UserProfile> GetMasterUsers(bool includeCRxUser = true);
        List<UserProfile> Get_lockingUsers();
        VendorOrganizations GetVendorOrgInvitation(Guid token);
        int InsertorUpdateUserRole(UserRoles userRole);
        bool IsResourceAllowed(int userId, object epId, object floorAssetId);
        bool IsValidInvitationIdCode(string recoveryCode);
        bool IsValidRecoveryCode(string recoveryCode);
        bool SaveUserSites(int userId, string siteAssignees, int createdBy);
        string SaveUserViewHistory(Guid loginGuid, int userId, string currentPage);
        int SendRecoveryMail(string emailAddress, string emailBody, string webUrl);
        bool UpdatePassword(int userId, string password, string newPassword);
        bool UpdatePassword(string userName, string newPassword, int modifiedBy, bool isPwdChange);
        bool UpdateUser(UserProfile newUser, string siteAssigneeType);
        bool LogOut(UserLogin loginUser);
        int UpdateUserRole(string roleId);
        int UpdateUserRole(UserRoles userRoles);
        bool UserVendorCreated(int VendorId);
        UserGroup GetUserGroup(int id);
        List<UserGroup> GetUserGroups();
        int UpdateUserGroup(UserGroup usergroup);
        List<UserGroup> GetUserGroupeAccess(int userId);
        string PasswordResetQueue(string emailAddress, char recoveryMethod, string recoveryAddress, string token, string passwordRequestType);
        int ResetUserPassword(int userid);
        UserProfile Userlockout(UserProfile user, int Islock);
        UserProfile GetUserProfileforlockout(string UserName);
        bool IsValidNewPassword(string password, string UserName);
        bool IsNewDevice(int UserId, string dToken, string ipAddress);
        bool SendVerificationCode(string userName, string newdevicecode, string fullName);
        int UpdateNewDevice(int UserId, string token);
        bool ValidateLastChanged(string userId);
        bool IsInLastPasswords(int userId, string password);
        string UnlocklUserLogin(string userids);
        List<Organization> GetUserOrgsByUserId(int Userid);
        List<UserProfile> GetAllUsersFromMasterDb();
        UserLogin GetUserOrgsByRefreshToken(string refreshToken);
        int DeleteUser(int userId);
    }
}