using HCF.BDO;
using HCF.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IUsersRepository
    {
        List<UserProfile> ConvertToUser(DataTable dataTable);
        UserProfile GetUsersList(int userId);
        IEnumerable<UserProfile> GetUsersList();
        int AddCertificates(Certificates userCertificate);
        int AddUserToClient(int userid, int clientNo, bool isActive, string userGroupIds);
        UserProfile AuthenticateUser(string userName, string password, string deviceToken, int deviceType, string version, string ipAddress, UserLogin loginInfo);
        bool ChangeRecoverPassword(string emailAddress, string password, string salt, string recoveryCode);
        int CheckExistingEmail(string email, int userId);
        bool CheckloginToken(Guid loginToken);
        string CheckRequiredPermission(Guid loginToken, int userId, string requiredPermission, string currentPage);
        int CreateUser(UserProfile userProfile, string siteAssigneeType);
        int CreateUserLoginForGU(UserLogin userLogin);
        bool DeleteCertificates(int userId);
        int DeleteUserRole(string roleId);
        bool FlagRecoveryCode(string recoveryCode);
        IEnumerable<UserProfile> Get();
        IEnumerable<UserProfile> GetAllClientUsers();
        List<Certificates> GetCertificates();
        string GetEmailAddressFromRecoveryCode(string recoveryCode);
        Task<UserProfile> GetRefreshtoken(string userEmail, string refreshToken);
        DataTable GetSaltFromEmailAddress(string userEmail);
        UserProfile GetUser(int userId, bool isNeedUserlogin = true);
        UserProfile GetUserEmailPassword(string userName);
        List<UserGroup> GetUserGroupeAccess(int userId);
        List<UserLogin> GetUserLogins(Guid userProfileId);
        List<Organization> GetUserOrgs(string userName);
        UserProfile GetUserProfile(int userId);
        List<UserRecordsCount> GetUserRecords(int userId);
        List<UserRoles> GetUserRoles(string roleId);
        IEnumerable<UserProfile> GetUsers();
        IEnumerable<UserProfile> GetMasterUsers();
        VendorOrganizations GetVendorOrgInvitation(Guid invitationId);
        List<UserProfile> Get_lockingUsers();
        int InsertorUpdateUserRole(UserRoles userRole);
        bool IsResourceAllowed(int userId, object epId, object floorAssetId);
        bool IsValidInvitationIdCode(string recoveryCode);
        bool IsValidRecoveryCode(string recoveryCode);
        bool LogOut(UserLogin loginUser);
        string PasswordResetQueue(string emailAddress, char recoveryMethod, string recoveryAddress, string token, string passwordRequestType);
        bool SaveUserSites(int userId, string siteAssignees, int createdBy);
        string SaveUserViewHistory(Guid loginToken, int userId, string currentPage);
        bool UpdatePassword(int userId, string newPassword, int modifiedBy, bool isPwdChange);
        bool UpdateUser(UserProfile updateUser, string siteAssigneeType);
        bool UpdateUserDrawing(int userId, long[] drawingIds);
        int UpdateUserGroup(UserGroup userGroup);
        long UpdateUserLoginDetails(UserLogin userLoginData);
        int UpdateUserRole(UserRoles userRole);
        List<UserAdditionalRole> UserAdditionalRole(bool isAll);
        List<UserGroup> UserGroups(bool isAll);
        bool UserVendorCreated(int VendorId);
        int ResetUserPassword(int userid);
        UserProfile Userlockout(string userName, int Islock);
        UserProfile GetUserProfileforlockout(string UserName);
        bool IsValidNewPassword(string password, string UserName);
        bool IsNewDevice(int UserId, string dToken, string ipAddress);
        int UpdateNewDevice(int UserId, string token);
        bool IsInLastPasswords(int userId, string password);
        string UnlocklUserLogin(string userids);
        List<Organization> GetUserOrgsByUserId(int UserId);
        List<UserProfile> GetAllUsersFromMasterDb();
        UserLogin GetUserOrgsByRefreshToken(string refreshToken);
        int DeleteUser(int userId);
    }
}