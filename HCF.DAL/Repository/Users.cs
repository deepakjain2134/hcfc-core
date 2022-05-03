using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.DAL.Contexts;
using HCF.DAL.Repository;
using HCF.Utility;
using HCF.Utility.AppConfig;

namespace HCF.DAL
{
    public class Users : IUsersRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IAppSetting _appSetting;

        public Users(ISqlHelper sqlHelper, IAppSetting appSetting)
        {
            _sqlHelper = sqlHelper;
            _appSetting = appSetting;
        }

        #region Users

        public IEnumerable<UserProfile> Get()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_UsersProfileDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var userLogOns = _sqlHelper.ConvertDataTable<UserLogin>(ds.Tables[2]).ToList();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var user =
                            new UserProfile
                            {
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UserGuid = Guid.Parse(row["UserProfileId"].ToString()),
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                                Email = row["Email"].ToString(),
                                UserName = row["UserName"].ToString(),
                                FileName = row["FileName"].ToString(),
                                SkillCode = row["SkillCode"].ToString(),
                                ResourceNumber = row["ResourceNumber"].ToString(),
                                FilePath = row["FilePath"].ToString(),
                                IsPowerUser = Convert.ToBoolean(row["IsPowerUser"].ToString()),
                                Skills = new Skills(),

                            };
                        user.UserGroups = new List<UserGroup>();
                        user.UserGroups = GetUsergroups(ds.Tables[1], user.UserId);
                        user.ResourceId = Conversion.TryCastInt32(row["ResourceId"].ToString());
                        user.IsLoginEnable = Conversion.TryCastBoolean(row["IsLoginEnable"].ToString());
                        user.Email = row["Email"].ToString();
                        user.PhoneNumber = row["PhoneNumber"].ToString();
                        user.FirstName = row["FirstName"].ToString();
                        user.LastName = row["LastName"].ToString();
                        user.PhoneNumber = row["PhoneNumber"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        user.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        user.IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString());
                        user.IsCRxUser = Convert.ToBoolean(row["IsCRxUser"].ToString());
                        user.UserLogin = userLogOns.Where(x => x.UserId == user.UserId).ToList();
                        user.VendorId = Conversion.TryCastInt32(row["VendorId"].ToString());
                        user.UserAdditionalRoleIds = row["UserAdditionalRoleIds"].ToString();
                        users.Add(user);
                    }
                }
            }
            return users;
        }


        public bool UpdateUserDrawing(int userId, long[] drawingIds)
        {
            bool status = false;
            const string sql = StoredProcedures.Update_UserDrawing;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@drawingIds", string.Join(",", drawingIds));
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        private List<UserGroup> GetUsergroups(DataTable dt, int userId)
        {
            List<UserGroup> UserGroups = new List<UserGroup>();
            if (userId > 0)
            {
                var expression = "UserId = '" + userId + "'";
                var sortOrder = "Name ASC";
                var drfoundRows = dt.Select(expression, sortOrder);
                for (int i = 0; i < drfoundRows.Length; i++)
                {
                    var usergroup = new UserGroup
                    {
                        Name = Convert.ToString(drfoundRows[i]["Name"]),
                        GroupId = Convert.ToInt32(drfoundRows[i]["GroupId"])
                    };
                    UserGroups.Add(usergroup);
                }
            }
            return UserGroups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserProfile> GetUsers()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_Users;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var userLogOns = _sqlHelper.ConvertDataTable<UserLogin>(ds.Tables[4]).ToList();
                    var userCertificates = _sqlHelper.ConvertDataTable<Certificates>(ds.Tables[1]).ToList();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var user =
                            new UserProfile
                            {
                                EpsCount = Convert.ToInt32(row["EpsCount"].ToString()),
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UserGuid = Guid.Parse(row["UserProfileId"].ToString()),
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                                Email = row["Email"].ToString(),
                                UserName = row["UserName"].ToString(),
                                FileName = row["FileName"].ToString(),
                                SkillCode = row["SkillCode"].ToString(),
                                //UserGroupIds = row["UserGroupIds"].ToString(),
                                ResourceNumber = row["ResourceNumber"].ToString(),
                                //UserGroupNames = row["UserGroupNames"].ToString(),
                                FilePath = row["FilePath"].ToString(),
                                IsPowerUser = Convert.ToBoolean(row["IsPowerUser"].ToString()),
                                //UserGroup = row["UserGroup"].ToString(),
                                Skills = new Skills(),
                                DocsCount = Convert.ToInt32(row["DocsCount"].ToString()),
                                PolicyCount = Convert.ToInt32(row["PolicyCount"].ToString()),

                            };

                        user.ResourceId = Conversion.TryCastInt32(row["ResourceId"].ToString());
                        user.IsLoginEnable = Conversion.TryCastBoolean(row["IsLoginEnable"].ToString());

                        user.UserGroups = new List<UserGroup>();
                        user.UserGroups = GetUsergroups(ds.Tables[7], user.UserId);

                        if (!string.IsNullOrEmpty(row["DrawingIds"].ToString()))
                        {
                            user.DrawingIds = row["DrawingIds"].ToString().Split(',').Select(long.Parse).ToArray();

                        }

                        if (!string.IsNullOrEmpty(row["OrientationDate"].ToString()))
                        {
                            user.OrientationDate = Convert.ToDateTime(row["OrientationDate"]);
                        }

                        user.Email = row["Email"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        user.FirstName = row["FirstName"].ToString();
                        user.LastName = row["LastName"].ToString();
                        user.PhoneNumber = row["PhoneNumber"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        user.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        user.IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString());
                        user.IsCRxUser = Convert.ToBoolean(row["IsCRxUser"].ToString());
                        user.VendorId = Conversion.TryCastInt32(row["VendorId"].ToString());
                        if (user.VendorId > 0)
                        {
                            user.Vendor = new Vendors();
                            user.Vendor.VendorId = Conversion.TryCastInt32(row["VendorId"].ToString());
                            user.Vendor.Name = row["VendorName"].ToString();


                        }

                        user.UserAdditionalRoleIds = row["UserAdditionalRoleIds"].ToString();
                        user.UserLogin = userLogOns.Where(x => x.UserId == user.UserId).ToList();
                        user.UserCertificates = userCertificates.Where(x => x.UserId == user.UserId).ToList();
                        users.Add(user);
                    }
                }
            }
            return users;
        }


        public IEnumerable<UserProfile> GetMasterUsers()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_MasterUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var user =
                            new UserProfile
                            {
                                EpsCount = Convert.ToInt32(row["EpsCount"].ToString()),
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UserGuid = Guid.Parse(row["UserProfileId"].ToString()),
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                                Email = row["Email"].ToString(),
                                UserName = row["UserName"].ToString(),
                                FileName = row["FileName"].ToString(),
                                SkillCode = row["SkillCode"].ToString(),
                                ResourceNumber = row["ResourceNumber"].ToString(),
                                FilePath = row["FilePath"].ToString(),
                                IsPowerUser = Convert.ToBoolean(row["IsPowerUser"].ToString()),
                                Skills = new Skills(),
                                DocsCount = Convert.ToInt32(row["DocsCount"].ToString()),
                                PolicyCount = Convert.ToInt32(row["PolicyCount"].ToString()),
                            };
                        user.ResourceId = Conversion.TryCastInt32(row["ResourceId"].ToString());
                        user.IsLoginEnable = Conversion.TryCastBoolean(row["IsLoginEnable"].ToString());
                        user.Email = row["Email"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        user.FirstName = row["FirstName"].ToString();
                        user.LastName = row["LastName"].ToString();
                        user.PhoneNumber = row["PhoneNumber"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        user.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        user.IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString());
                        user.IsCRxUser = Convert.ToBoolean(row["IsCRxUser"].ToString());
                        user.VendorId = Conversion.TryCastInt32(row["VendorId"].ToString());
                        if (user.VendorId > 0)
                        {
                            user.Vendor = new Vendors();
                            user.Vendor.VendorId = Conversion.TryCastInt32(row["VendorId"].ToString());
                            user.Vendor.Name = row["VendorName"].ToString();
                        }
                        user.UserAdditionalRoleIds = row["UserAdditionalRoleIds"].ToString();
                        users.Add(user);
                    }
                }
            }
            return users;
        }



        /// <summary>
        /// use this method for the bind drop down or list 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserProfile> GetUsersList()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_UsersList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    users = ConvertToUser(ds.Tables[0]);
                    var userEps = new List<UserProfile>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        UserProfile up = new UserProfile();
                        up.UserId = Convert.ToInt32(item["UserId"].ToString());
                        up.EpsCount = Convert.ToInt32(item["EPsCount"].ToString());
                        userEps.Add(up);
                    }

                    foreach (var item in users)
                    {
                        var epsCount = userEps.FirstOrDefault(x => x.UserId == item.UserId)?.EpsCount;
                        item.EpsCount = (epsCount.HasValue) ? epsCount.Value : 0;
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// use this method if you want to set created by / alter by or user only
        /// First Name/Last name / Email/Username
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfile GetUsersList(int userId)
        {
            var users = new UserProfile();
            const string sql = StoredProcedures.Get_UsersList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    users = ConvertToUser(ds.Tables[0]).FirstOrDefault();
            }
            return users;
        }

        public IEnumerable<UserProfile> GetAllClientUsers()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_AllClientUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var user =
                            new UserProfile
                            {
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UserGuid = Guid.Parse(row["UserProfileId"].ToString()),
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                                Email = row["Email"].ToString(),
                                UserName = row["UserName"].ToString(),
                                FileName = row["FileName"].ToString(),
                                IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                                IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString()),
                                Orgkey = Guid.Parse(row["Orgkey"].ToString()),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString()
                            };
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public bool IsResourceAllowed(int userId, object epId, object floorAssetId)
        {
            bool status;
            const string sql = StoredProcedures.IsResourceAllowed;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@epId", epId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                status = Convert.ToBoolean(_sqlHelper.GetScalarValue(command));
            }
            return status;
        }

        public long UpdateUserLoginDetails(UserLogin userLoginData)
        {
            const string sql = StoredProcedures.Update_UserLoginIP;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserLoginId", userLoginData.UserLoginId);
                command.Parameters.AddWithValue("@ip", userLoginData.ip);
                command.Parameters.AddWithValue("@city", userLoginData.city);
                command.Parameters.AddWithValue("@country_name", userLoginData.country_name);
                command.Parameters.AddWithValue("@organisation", userLoginData.organisation);
                command.Parameters.AddWithValue("@BrowserName", userLoginData.BrowserName);
                command.Parameters.AddWithValue("@OsName", userLoginData.OsName);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return userLoginData.UserLoginId;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfile GetUserProfile(int userId)
        {
            var userProfile = new UserProfile();
            const string sql = StoredProcedures.Get_Users;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                userProfile.EPDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[5]).ToList();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userProfile.UserId = Convert.ToInt32(row["UserId"].ToString());
                    userProfile.UserName = row["UserName"].ToString();
                    userProfile.FirstName = row["FirstName"].ToString();
                    userProfile.Email = row["Email"].ToString();
                    userProfile.LastName = row["LastName"].ToString();
                    userProfile.FilePath = row["FilePath"].ToString();
                    userProfile.ResourceNumber = row["ResourceNumber"].ToString();
                    userProfile.UserAdditionalRoleIds = Conversion.TryCastString(row["UserAdditionalRoleIds"].ToString());
                    userProfile.ResourceId = Conversion.TryCastInt32(row["ResourceId"].ToString());
                    userProfile.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                }
            }
            return userProfile;
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfile GetUser(int userId, bool isNeedUserlogin = true)
        {
            var user = new UserProfile();
            const string sql = StoredProcedures.Get_Users;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var roles = ConvertToRole(ds.Tables[2]);
                    var userGroup = _sqlHelper.ConvertDataTable<UserGroup>(ds.Tables[3]);
                    var userlogin = _sqlHelper.ConvertDataTable<UserLogin>(ds.Tables[4]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        user.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        user.UserId = Convert.ToInt32(row["UserId"].ToString());
                        user.UserName = row["UserName"].ToString();
                        user.UserGuid = Guid.Parse(row["UserProfileId"].ToString());
                        user.FileName = row["FileName"].ToString();
                        user.FilePath = row["FilePath"].ToString();
                        user.Email = row["Email"].ToString();
                        user.FirstName = row["FirstName"].ToString();
                        user.LastName = row["LastName"].ToString();
                        user.IsPwdChange = Convert.ToBoolean(row["IsPwdChange"].ToString());
                        user.IsPowerUser = Convert.ToBoolean(row["IsPowerUser"].ToString());
                        user.IsCRxUser = Convert.ToBoolean(row["IsCRxUser"].ToString());
                        user.ResourceNumber = row["ResourceNumber"].ToString();
                        user.PhoneNumber = row["PhoneNumber"].ToString();
                        user.CellNo = row["CellNo"].ToString();
                        if (!string.IsNullOrEmpty(row["LastUpdatedPasswordDate"].ToString()))
                        {
                            user.LastUpdatedPasswordDate = Convert.ToDateTime(row["LastUpdatedPasswordDate"].ToString());
                        }
                        user.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        user.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        user.IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString());
                        if (isNeedUserlogin)
                            user.UserLogin = userlogin.Where(x => x.UserId == userId).ToList();

                        if (!string.IsNullOrEmpty(row["DrawingIds"].ToString()))
                            user.DrawingIds = row["DrawingIds"].ToString().Split(',').Select(long.Parse).ToArray();

                        if (!string.IsNullOrEmpty(row["PermitTypeIds"].ToString()))
                            user.PermitTypeIds = row["PermitTypeIds"].ToString().Split(',').Select(long.Parse).ToArray();

                        user.IsVerified = Convert.ToBoolean(row["IsVerified"].ToString());
                        user.ResourceId = Conversion.TryCastInt32(row["ResourceId"].ToString());
                        if (!string.IsNullOrEmpty(row["VendorId"].ToString()))
                        {
                            user.VendorId = Convert.ToInt32(row["VendorId"].ToString());
                            user.Vendor = new Vendors()
                            {
                                VendorId = user.VendorId.Value,
                                Name = row["VendorName"].ToString(),
                                IsActive = Convert.ToBoolean(row["VendorIsActive"])
                            };
                        }

                        if (!string.IsNullOrEmpty(row["OrientationDate"].ToString()))
                            user.OrientationDate = Convert.ToDateTime(row["OrientationDate"].ToString());

                        user.UserStatusCode = Conversion.TryCastInt32(row["UserStatusCode"].ToString());
                        user.Orgkey = Guid.Parse(row["Orgkey"].ToString());
                        user.Roles = roles;
                        user.UserGroups = userGroup;
                        user.UserAdditionalRoleIds = row["UserAdditionalRoleIds"].ToString();
                    }
                }
            }
            return user;
        }

        private List<Roles> ConvertToRole(DataTable dataTable)
        {
            var roles = new List<Roles>();
            foreach (DataRow item in dataTable.Rows)
            {
                var role = new Roles
                {
                    DisplayText = item["DisplayText"].ToString(),
                    IsActive = Convert.ToBoolean(item["IsActive"].ToString()),
                    RoleName = item["RoleName"].ToString()
                };

                if (!string.IsNullOrEmpty(item["Status"].ToString()))
                    role.Status = Convert.ToBoolean(item["Status"].ToString());
                roles.Add(role);
            }
            return roles;
        }

        public int CheckExistingEmail(string email, int userId)
        {
            const string sql = StoredProcedures.Check_ExistingEmail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@emailDoamin", email.Split('@')[1]);
                command.Parameters.Add("@canCreate", SqlDbType.Int);
                command.Parameters["@canCreate"].Direction = ParameterDirection.Output;
                var canCreate = _sqlHelper.ExecuteNonQuery(command, "@canCreate");
                //if (canCreate == 2)
                //{
                //    return canCreate;
                //}

                return canCreate;
                // return (canCreate > 0);
            }
            //return false;
        }

        public bool UserVendorCreated(int VendorId)
        {
            bool status;
            const string sql = StoredProcedures.Check_UserVendorExist;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vendorId", VendorId);
                status = Convert.ToBoolean(_sqlHelper.GetScalarValue(command));
            }
            return status;
        }

        public int AddUserToClient(int userid, int clientNo, bool isActive, string userGroupIds)
        {
            const string sql = StoredProcedures.Insert_ClientUser;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@clientNo", clientNo);
                command.Parameters.AddWithValue("@isActive", isActive);
                command.Parameters.AddWithValue("@userGroupIds", userGroupIds);
                _sqlHelper.CommonExecuteNonQuery(command);
            }
            return 0;
        }

        public List<UserLogin> GetUserLogins(Guid userProfileId)
        {
            var userLogins = new List<UserLogin>();
            const string sql = StoredProcedures.Get_UserLogins;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userProfileId", userProfileId);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    userLogins = _sqlHelper.ConvertDataTable<UserLogin>(ds.Tables[0]);
            }
            return userLogins;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        /// 
        public int CreateUser(UserProfile userProfile, string siteAssigneeType)
        {
            int newId;
            const string sql = StoredProcedures.Insert_User;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                command.Parameters.AddWithValue("@LastName", userProfile.LastName);
                command.Parameters.AddWithValue("@UserName", userProfile.UserName);
                command.Parameters.AddWithValue("@Salt", userProfile.Salt);
                command.Parameters.AddWithValue("@Password", userProfile.Password);
                command.Parameters.AddWithValue("@IsInternalUser", userProfile.IsInternalUser);
                command.Parameters.AddWithValue("@PhoneNumber", userProfile.PhoneNumber);
                command.Parameters.AddWithValue("@CellNo", userProfile.CellNo);
                command.Parameters.AddWithValue("@VendorId", userProfile.VendorId);
                command.Parameters.AddWithValue("@IsActive", userProfile.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", userProfile.CreatedBy);
                command.Parameters.AddWithValue("@ResourceId", userProfile.ResourceId);
                command.Parameters.AddWithValue("@ResourceNumber", userProfile.ResourceNumber);
                command.Parameters.AddWithValue("@IsPwdChange", userProfile.IsPwdChange);
                command.Parameters.AddWithValue("@UserGroupIds", userProfile.UserGroupIds);
                command.Parameters.AddWithValue("@IsPowerUser", userProfile.IsPowerUser);
                command.Parameters.AddWithValue("@siteAssigneeType", siteAssigneeType);
                command.Parameters.AddWithValue("@UserAdditionalRoleIds", userProfile.UserAdditionalRoleIds);
                command.Parameters.AddWithValue("@userId", userProfile.UserId);
                command.Parameters.AddWithValue("@userProfileId", userProfile.UserGuid);
                command.Parameters.AddWithValue("@OrientationDate", userProfile.OrientationDate);
                var drawingIds = string.Empty;
                if (userProfile.DrawingIds != null && userProfile.DrawingIds.Length > 0)
                    drawingIds = string.Join(",", userProfile.DrawingIds);
                command.Parameters.AddWithValue("@DrawingIds", drawingIds);

                var permitTypes = string.Empty;
                if (userProfile.PermitTypeIds != null && userProfile.PermitTypeIds.Length > 0)
                    permitTypes = string.Join(",", userProfile.PermitTypeIds);
                command.Parameters.AddWithValue("@permitTypeIds", permitTypes);

                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userCertificate"></param>
        /// <returns></returns>
        ///
        public int CreateUserLoginForGU(UserLogin userLogin)
        {
            int newId;
            const string sql = StoredProcedures.insert_userloginforguest;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@refreshToken", userLogin.RefreshToken);
                command.Parameters.AddWithValue("@userid", userLogin.UserId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public List<Certificates> GetCertificates()
        {
            var Certificates = new List<Certificates>();
            const string sql = StoredProcedures.Get_Certificates;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    Certificates = _sqlHelper.ConvertDataTable<Certificates>(ds.Tables[0]);
                foreach (var certi in Certificates)
                {
                    var name = certi.UserName.Split(' ');
                    if (name.Count() != 1)
                        certi.UserName = (name[0] ?? string.Empty) + " " + (!string.IsNullOrEmpty(name[1]) ? name[1].Substring(0, 1) : string.Empty);
                    var uploaded = certi.UploadedBy.Split(' ');
                    if (uploaded.Count() != 1)
                        certi.UploadedBy = (uploaded[0] ?? string.Empty) + " " + (!string.IsNullOrEmpty(uploaded[1]) ? uploaded[1].Substring(0, 1) : string.Empty);
                }
            }
            return Certificates;
        }

        public int AddCertificates(Certificates userCertificate)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Certificates;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CertificateId", userCertificate.CertificateId);
                command.Parameters.AddWithValue("@UserId", userCertificate.UserId);
                command.Parameters.AddWithValue("@VendorId", userCertificate.VendorId);
                command.Parameters.AddWithValue("@CertificateName", userCertificate.CertificateName);
                command.Parameters.AddWithValue("@Name", userCertificate.FileName);
                command.Parameters.AddWithValue("@Path", userCertificate.Path);
                command.Parameters.AddWithValue("@ValidUpTo", userCertificate.ValidUpTo);
                command.Parameters.AddWithValue("@CreatedBy", userCertificate.CreatedBy);
                command.Parameters.AddWithValue("@CreatedDate", userCertificate.CreatedDate);
                command.Parameters.AddWithValue("@IsActive", userCertificate.IsActive);
                command.Parameters.AddWithValue("@IssueDate", userCertificate.IssueDate);
                command.Parameters.AddWithValue("@CertificateType", userCertificate.CertificateType);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteCertificates(int userId)
        {
            const string sql = StoredProcedures.Delete_Certificates;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        public bool UpdateUser(UserProfile updateUser, string siteAssigneeType)
        {
            const string sql = StoredProcedures.Update_User;
            var drawingIds = string.Empty;
            var permitTypeIds = string.Empty;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", updateUser.UserId);
                command.Parameters.AddWithValue("@FirstName", updateUser.FirstName);
                command.Parameters.AddWithValue("@LastName", updateUser.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", updateUser.PhoneNumber);
                command.Parameters.AddWithValue("@FileName", updateUser.FileName);
                command.Parameters.AddWithValue("@FilePath", updateUser.FilePath);
                command.Parameters.AddWithValue("@CellNo", updateUser.CellNo);
                command.Parameters.AddWithValue("@IsActive", updateUser.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", updateUser.CreatedBy);
                command.Parameters.AddWithValue("@VendorId", updateUser.VendorId);
                command.Parameters.AddWithValue("@Email", !string.IsNullOrEmpty(updateUser.Email) ? updateUser.Email : null);
                command.Parameters.AddWithValue("@ResourceNumber", updateUser.ResourceNumber);
                command.Parameters.AddWithValue("@IsInternalUser", updateUser.IsInternalUser);
                command.Parameters.AddWithValue("@IsPwdChange", updateUser.IsPwdChange);
                command.Parameters.AddWithValue("@UserGroupIds", updateUser.UserGroupIds);
                command.Parameters.AddWithValue("@IsPowerUser", updateUser.IsPowerUser);
                command.Parameters.AddWithValue("@UserAdditionalRoleIds", updateUser.UserAdditionalRoleIds);
                command.Parameters.AddWithValue("@siteAssigneeType", siteAssigneeType);
                command.Parameters.AddWithValue("@OrientationDate", updateUser.OrientationDate);
                if (updateUser.DrawingIds != null && updateUser.DrawingIds.Length > 0)
                    drawingIds = string.Join(",", updateUser.DrawingIds);
                command.Parameters.AddWithValue("@DrawingIds", drawingIds);

                if (updateUser.PermitTypeIds != null && updateUser.PermitTypeIds.Length > 0)
                    permitTypeIds = string.Join(",", updateUser.PermitTypeIds);
                command.Parameters.AddWithValue("@permitTypeIds", permitTypeIds);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="isPwdChange"></param>
        /// <returns></returns>
        public bool UpdatePassword(int userId, string newPassword, int modifiedBy, bool isPwdChange)
        {
            const string sql = StoredProcedures.Update_Password;
            using var command = new SqlCommand(sql);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Password", newPassword);
            command.Parameters.AddWithValue("@ModifiedBy", modifiedBy);
            command.Parameters.AddWithValue("@IsPwdChange", isPwdChange);
            return _sqlHelper.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="deviceToken"></param>
        /// <param name="deviceType"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public UserProfile AuthenticateUser(string userName, string password, string deviceToken, int deviceType, string version, string ipAddress, UserLogin loginInfo)
        {
            const string sql = StoredProcedures.AuthenticateUser;
            var user = new UserProfile();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@deviceToken", deviceToken);
                command.Parameters.AddWithValue("@DeviceTypeId", deviceType);
                command.Parameters.AddWithValue("@version", version);
                command.Parameters.AddWithValue("@ipAddress", ipAddress);
                command.Parameters.AddWithValue("@browserName", loginInfo.BrowserName);
                command.Parameters.AddWithValue("@osName", loginInfo.OsName);
                command.Parameters.AddWithValue("@deviceType", loginInfo.DeviceType);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    try
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                            return user;
                        if (ds.Tables[0].Rows[0]["IsFailedLogin"].ToString() == "0")
                        {
                            var userLogins = new List<UserLogin>();
                            var userLogin = new UserLogin
                            {
                                RefreshToken = (Guid)ds.Tables[0].Rows[0]["RefreshToken"],
                                UserLoginId = (long)ds.Tables[0].Rows[0]["UserLoginId"],
                                IsNewDevice = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsNewDevice"])
                            };
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Orgkey"].ToString()))
                                user.Orgkey = (Guid)ds.Tables[0].Rows[0]["Orgkey"];

                            user.UserGuid = (Guid)ds.Tables[0].Rows[0]["UserProfileId"];
                            user.UserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);

                            user.UserName = userName;
                            user.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                            user.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                            user.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                            user.IsSystemUser = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSystemUser"].ToString());
                            user.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]);
                            user.IsCRxUser = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCRxUser"]);
                            user.IsPwdChange = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPwdChange"].ToString());
                            user.IsLoginEnable = Conversion.TryCastBoolean(ds.Tables[0].Rows[0]["IsLoginEnable"].ToString());
                            user.IsLoginEnableText = Conversion.TryCastString(ds.Tables[0].Rows[0]["IsLoginEnableText"].ToString());
                            user.IsChangePasswordRequired = Conversion.TryCastBoolean(ds.Tables[0].Rows[0]["IsChangePasswordRequired"].ToString());
                            userLogins.Add(userLogin);
                            user.UserLogin = userLogins;
                            user.Organizations = new List<Organization>();
                            user.Organizations = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[1]);
                            user.TotalOrgCount = user.Organizations.Count();
                        }
                        else
                        {
                            user.IsLoginEnableText = "InValid Username or password";
                            user.IsLoginEnable = false;
                            user.UserId = 0;
                            user.UserStatusCode = 500;
                        }
                    }
                    catch (Exception)
                    {
                        user.IsLoginEnableText = "Error occurred while connecting server....";
                        user.IsLoginEnable = false;
                        user.UserId = -1;
                        user.UserStatusCode = 500;
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<UserProfile> GetRefreshtoken(string userEmail, string refreshToken)
        {
            const string sql = StoredProcedures.Get_Refreshtoken;
            var user = new UserProfile();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userEmail);
                command.Parameters.AddWithValue("@refreshToken", refreshToken);
                var ds = await _sqlHelper.GetCommonDataSetAsync(command);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count <= 0)
                        return user;

                    var userLogins = new List<UserLogin>();
                    var userId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                    var userLogin = new UserLogin
                    {
                        RefreshToken = Conversion.TryCastGuid(ds.Tables[0].Rows[0]["RefreshToken"]),
                        IsNewDevice = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsNewDevice"])
                    };

                    var lastlogin = new UserLogin();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["RefreshToken"].ToString()))
                    {
                        lastlogin.RefreshToken = Conversion.TryCastGuid(ds.Tables[0].Rows[0]["RefreshToken"]);
                        string loginUrl = ds.Tables[0].Rows[0]["LastLoginURL"].ToString();
                        if (loginUrl.Contains(_appSetting.WebUrlPath))
                        {
                            loginUrl = loginUrl.Replace(_appSetting.WebUrlPath, "");
                            lastlogin.LastLoginURL = _appSetting.WebUrlPath + loginUrl;
                        }
                    }
                    user = GetUser(userId);
                    userLogins.Add(userLogin);
                    user.UserLogin = userLogins;
                    user.Organizations = user.Orgkey == Guid.Empty ? _sqlHelper.ConvertDataTable<Organization>(ds.Tables[1]) : _sqlHelper.ConvertDataTable<Organization>(ds.Tables[1]).Where(x => x.Orgkey == user.Orgkey).ToList();

                }
            }
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public bool LogOut(UserLogin loginUser)
        {
            var sql = StoredProcedures.Update_LogOutUser;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", loginUser.UserId);
                command.Parameters.AddWithValue("@RefreshToken", loginUser.RefreshToken);
                if (loginUser.LastLoginURL.Contains(_appSetting.WebUrlPath))
                    loginUser.LastLoginURL = loginUser.LastLoginURL.Replace(_appSetting.WebUrlPath, "");
                command.Parameters.AddWithValue("@LastLoginURL", loginUser.LastLoginURL);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public DataTable GetSaltFromEmailAddress(string userEmail)
        {
            const string sql = StoredProcedures.Get_SaltFromEmailAddress;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userName", userEmail);
                var dt = _sqlHelper.GetDataTable(command);
                return dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="recoveryMethod"></param>
        /// <param name="recoveryAddress"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string PasswordResetQueue(string emailAddress, char recoveryMethod, string recoveryAddress, string token, string passwordRequestType)
        {
            const string sql = StoredProcedures.Insert_PasswordResetQueue;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                command.Parameters.AddWithValue("@RecoveryMethod", recoveryMethod);
                command.Parameters.AddWithValue("@RecoveryAddress", recoveryAddress);
                command.Parameters.AddWithValue("@RecoveryToken", token);
                command.Parameters.AddWithValue("@passwordRequestType", passwordRequestType);
                return Conversion.TryCastString(_sqlHelper.GetScalarValue(command));
            }
        }

        public bool IsValidRecoveryCode(string recoveryCode)
        {
            bool status;
            const string sql = StoredProcedures.IsValidRecoveryCode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecoveryToken", recoveryCode);
                status = (Conversion.TryCastString(_sqlHelper.GetScalarValue(command)) == "1");
            }
            return status;
        }

        public bool IsValidInvitationIdCode(string recoveryCode)
        {
            bool status;
            const string sql = StoredProcedures.IsValidInvitationIdCode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecoveryToken", recoveryCode);
                status = (Conversion.TryCastString(_sqlHelper.GetScalarValue(command)) == "1");
            }
            return status;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveryCode"></param>
        /// <returns></returns>
        public string GetEmailAddressFromRecoveryCode(string recoveryCode)
        {
            const string sql = StoredProcedures.GetEmailAddressFromRecoveryCode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecoveryToken", recoveryCode);
                return Conversion.TryCastString(_sqlHelper.GetScalarValue(command));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="recoveryCode"></param>
        /// <returns></returns>
        public bool ChangeRecoverPassword(string emailAddress, string password, string salt, string recoveryCode)
        {
            const string sql = StoredProcedures.ChangerecoverPassword;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userName", emailAddress);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@RecoveryCode", recoveryCode);
                command.Parameters.AddWithValue("@Salt", salt);
                return _sqlHelper.CommonExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveryCode"></param>
        /// <returns></returns>
        public bool FlagRecoveryCode(string recoveryCode)
        {
            const string sql = StoredProcedures.FlagRecoveryCode;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecoveryToken", recoveryCode);
                return _sqlHelper.CommonExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserGroup> UserGroups(bool isAll)
        {
            List<UserGroup> lists;
            const string sql = StoredProcedures.Get_UserGroups;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@isAll", isAll);
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<UserGroup>(dt);
            }
            return lists;
        }


        public List<UserGroup> GetUserGroupeAccess(int userId)
        {
            List<UserGroup> lists;
            const string sql = StoredProcedures.Get_UserGroupeAccess;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<UserGroup>(dt);
            }
            return lists;
        }


        public List<UserAdditionalRole> UserAdditionalRole(bool isAll)
        {
            List<UserAdditionalRole> lists;
            const string sql = StoredProcedures.Get_UserAdditionalRoles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@isAll", isAll);
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<UserAdditionalRole>(dt);
            }
            return lists;
        }

        public List<UserRecordsCount> GetUserRecords(int userId)
        {
            List<UserRecordsCount> lists = new List<UserRecordsCount>();
            const string sql = StoredProcedures.Get_UserRecordsCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<UserRecordsCount>(ds.Tables[0]);
                    var userProfiles = ConvertToUser(ds.Tables[1]);
                    var tag = new List<TaggedMaster>();
                    foreach (var item in lists)
                    {
                        item.StandardEps = new List<StandardEps>();
                        item.StandardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                        item.TotalInProgressEps = item.StandardEps.Count;
                        foreach (var user in item.StandardEps)
                        {
                            user.UserProfile = userProfiles.FirstOrDefault(x => x.UserId == user.LastUpdatedBy);
                        }

                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            var taggedMaster = new TaggedMaster
                            {
                                CreatedDate = DateTime.Parse(row["CreatedDate"].ToString()),
                                TaggedId = Guid.Parse(row["TaggedId"].ToString())
                            };
                            if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                            {
                                taggedMaster.ActivityId = Guid.Parse(row["ActivityId"].ToString());
                            }

                            if (!string.IsNullOrEmpty(row["AssetName"].ToString()))
                                taggedMaster.AssetName = row["AssetName"].ToString();
                            if (!string.IsNullOrEmpty(row["AssetNo"].ToString()))
                                taggedMaster.AssetNo = row["AssetNo"].ToString();
                            if (!string.IsNullOrEmpty(row["CreatedName"].ToString()))
                                taggedMaster.CreatedName = row["CreatedName"].ToString();
                            if (!string.IsNullOrEmpty(row["TaggedType"].ToString()))
                                taggedMaster.TaggedType = Convert.ToInt32(row["TaggedType"].ToString());
                            if (!string.IsNullOrEmpty(row["tag"].ToString()))
                                taggedMaster.MainTaggedType = Convert.ToInt32(row["tag"].ToString());

                            if (!string.IsNullOrEmpty(row["PermitNo"].ToString()))
                            {
                                taggedMaster.PermitNo = row["PermitNo"].ToString();

                            }
                            if (!string.IsNullOrEmpty(row["IssueId"].ToString()))
                            {
                                taggedMaster.IssueId =Convert.ToInt32(row["IssueId"].ToString());

                            }

                            tag.Add(taggedMaster);

                        }
                        item.TaggedMasters = tag.Where(x => x.TaggedType != 0 || x.TaggedType != 1).ToList();

                        item.TaggedItem = item.TaggedMasters.Count;


                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public bool CheckloginToken(Guid loginToken)
        {
            bool status;
            const string sql = StoredProcedures.Get_LoginToken;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@loginToken", loginToken);
                status = Convert.ToBoolean(_sqlHelper.GetCommonScalarValue(command));
            }
            return status;
        }

        public List<UserProfile> ConvertToUser(DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
                dataTable = dataTable.AsEnumerable().Distinct(DataRowComparer.Default).CopyToDataTable();

            var users = new List<UserProfile>();
            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["IsActive"].ToString()) && !string.IsNullOrEmpty(item["IsCRxUser"].ToString()))
                {
                    var user = new UserProfile
                    {
                        UserId = !string.IsNullOrEmpty(item["UserId"].ToString()) ? Convert.ToInt32(item["UserId"].ToString()) : 0,
                        FirstName = item["FirstName"].ToString(),
                        LastName = item["LastName"].ToString(),
                        Email = item["Email"].ToString(),
                        UserName = item["UserName"].ToString(),
                        IsActive = Convert.ToBoolean(item["IsActive"].ToString()),
                        IsCRxUser = Convert.ToBoolean(item["IsCRxUser"].ToString())
                    };
                    users.Add(user);
                }
            }
            return users.GroupBy(x => x.UserId).Select(x => x.FirstOrDefault()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <param name="userId"></param>
        /// <param name="requiredPermission"></param>
        /// <returns></returns>
        public string CheckRequiredPermission(Guid loginToken, int userId, string requiredPermission, string currentPage)
        {
            string status = "";
            const string sql = StoredProcedures.Get_RequiredPermission;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@loginToken", loginToken);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@requiredPermission", requiredPermission);
                command.Parameters.AddWithValue("@currentPage", currentPage);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    string result1 = ds.Tables[0].Rows[0][0].ToString();
                    string result2 = ds.Tables[1].Rows[0][0].ToString();
                    string deviceStatusCode = ds.Tables[2].Rows[0][0].ToString();
                    if (deviceStatusCode == "502") // 502 new Device Found
                        status = deviceStatusCode;
                    else if (result1 == result2)
                        status = "200";
                    else if (result2 == "401")
                        status = result2;
                    else if (result1 == "202" || result1 == "404" || result1 == "502")
                        status = result1;

                    else
                        status = "0";
                }
            }
            return status;
        }

        public string SaveUserViewHistory(Guid loginToken, int userId, string currentPage)
        {
            const string sql = StoredProcedures.Save_UserViewHistory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@currentPage", currentPage);
                _sqlHelper.GetDataSet(command);
            }
            return currentPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserProfile GetUserEmailPassword(string userName)
        {
            var userProfile = new UserProfile();
            DataTable dt = GetSaltFromEmailAddress(userName);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    userProfile.Salt = Guid.Parse(row["Salt"].ToString());
                    userProfile.Password = row["Password"].ToString();
                }
            }
            return userProfile;
        }

        public List<Organization> GetUserOrgs(string userName)
        {
            var lists = new List<Organization>();
            const string sql = StoredProcedures.Get_UserOrgs;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@userName", userName);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
            }
            return lists;
        }


        public int InsertorUpdateUserRole(UserRoles userRole)
        {
            int newId;
            const string sql = StoredProcedures.Insert_UserRole;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userRole.UserId);
                command.Parameters.AddWithValue("@RoleId", userRole.RoleId);
                command.Parameters.AddWithValue("@Status", userRole.Status);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        #region Update User Group        

        public int UpdateUserGroup(UserGroup userGroup)
        {
            const string sql = StoredProcedures.Update_UserGroup;
            int newId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", userGroup.GroupId);
                command.Parameters.AddWithValue("@Name", userGroup.Name);
                command.Parameters.AddWithValue("@Description", userGroup.Description);
                command.Parameters.AddWithValue("@IsReadOnly", userGroup.IsReadOnly);
                command.Parameters.AddWithValue("@IsActive", userGroup.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", userGroup.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        #region Insert / Update / delete (inactive) User Role

        public List<UserRoles> GetUserRoles(string roleId)
        {
            const string sql = StoredProcedures.Get_UserRoles;
            List<UserRoles> userRole;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", roleId);
                var ds = _sqlHelper.GetDataSet(command);
                userRole = _sqlHelper.ConvertDataTable<UserRoles>(ds.Tables[0]);
            }
            return userRole;
        }

        public int UpdateUserRole(UserRoles userRole)
        {
            int newId;
            const string sql = StoredProcedures.Update_UserRole;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", userRole.RoleId);
                command.Parameters.AddWithValue("@RoleName", userRole.RoleName);
                command.Parameters.AddWithValue("@Sequence", userRole.Sequence);
                command.Parameters.AddWithValue("@DisplayText", userRole.DisplayText);
                command.Parameters.AddWithValue("@ParentId", userRole.ParentId);
                command.Parameters.AddWithValue("@IsChild", userRole.IsChild);
                command.Parameters.AddWithValue("@IsActive", userRole.IsActive);
                command.Parameters.AddWithValue("@IsUserRole", userRole.IsUserRole);
                command.Parameters.AddWithValue("@CreatedBy", userRole.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int DeleteUserRole(string roleId)
        {
            int newId;
            const string sql = StoredProcedures.Delete_UserRole;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", roleId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        public VendorOrganizations GetVendorOrgInvitation(Guid invitationId)
        {
            VendorOrganizations lists = new VendorOrganizations();
            const string sql = StoredProcedures.Get_VendorOrgInvitation;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@invitationId", invitationId);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<VendorOrganizations>(ds.Tables[0]).FirstOrDefault();

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        lists.Organization = new Organization
                        {
                            Name = item["OrganizationName"].ToString(),
                            DbName = item["DbName"].ToString(),
                            Orgkey = Guid.Parse(item["Orgkey"].ToString())
                        };
                        lists.Vendors = new Vendors
                        {
                            Name = item["VendorsName"].ToString(),
                            Email = item["Email"].ToString(),
                            VendorId = Convert.ToInt32(item["VendorId"].ToString())
                        };
                    }
                }
            }
            return lists;
        }

        public bool SaveUserSites(int userId, string siteAssignees, int createdBy)
        {
            bool status;
            const string sql = StoredProcedures.Insert_UserSiteMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@siteAssigneeType", siteAssignees);
                command.Parameters.AddWithValue("@CreatedBy", createdBy);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public int ResetUserPassword(int userid)
        {
            int newId;
            const string sql = StoredProcedures.ResetUserPassword;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public UserProfile Userlockout(string userName, int Islock)
        {
            const string sql = StoredProcedures.Userlockout;
            var user = new UserProfile();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userName", userName);
                command.Parameters.AddWithValue("@Islock", Islock);
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null && dt.Rows.Count > 0)
                {
                    user.UserStatusCode = Convert.ToInt32(dt.Rows[0]["UserStatusCode"].ToString());
                }
            }
            return user;
        }

        public UserProfile GetUserProfileforlockout(string UserName)
        {
            var userProfile = new UserProfile();
            const string sql = StoredProcedures.Get_Usersforlockout;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", UserName);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        userProfile.UserId = Convert.ToInt32(row["UserId"].ToString());
                        userProfile.UserName = row["UserName"].ToString();
                        userProfile.UserGuid = Guid.Parse(row["UserProfileId"].ToString());
                        userProfile.Email = row["Email"].ToString();
                        userProfile.FirstName = row["FirstName"].ToString();
                        userProfile.LastName = row["LastName"].ToString();
                        userProfile.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                        userProfile.StatusCode = row["UserStatusCode"].ToString();
                        userProfile.CellNo = row["CellNo"].ToString();
                        userProfile.PhoneNumber = row["PhoneNumber"].ToString();
                        if (!string.IsNullOrEmpty(row["Orgkey"].ToString()))
                            userProfile.Orgkey = Guid.Parse(row["Orgkey"].ToString());
                    }
                }

            }
            return userProfile;
        }

        public bool IsValidNewPassword(string password, string UserName)
        {
            bool status;
            const string sql = StoredProcedures.IsValidNewPassword;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@username", UserName);
                status = (Conversion.TryCastString(_sqlHelper.GetScalarValue(command)) == "1");
            }
            return status;
        }

        public bool IsNewDevice(int UserId, string dToken, string ipAddress)
        {
            bool status;
            const string sql = StoredProcedures.IsNewDevice;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@deviceToken", dToken);
                command.Parameters.AddWithValue("@ipAddress", ipAddress);
                status = (Conversion.TryCastString(_sqlHelper.GetCommonScalarValue(command)) == "1");
            }
            return status;
        }

        public int UpdateNewDevice(int UserId, string token)
        {
            int newId;
            const string sql = StoredProcedures.UpdateNewDevice;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@token", token);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public List<UserProfile> Get_lockingUsers()
        {
            var Lockinguser = new List<UserProfile>();
            const string sql = StoredProcedures.Get_lockingUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Lockinguser = ConvertToUser(ds.Tables[0]);
                }
            }
            return Lockinguser;
        }

        public bool IsInLastPasswords(int userId, string password)
        {
            int newId;
            const string sql = StoredProcedures.Get_LastPasswords;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@currentPassword", password);
                command.Parameters.Add("@status", SqlDbType.Int);
                command.Parameters["@status"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@status");
            }
            return (newId > 0);
        }

        public string UnlocklUserLogin(string userids)
        {
            const string sql = StoredProcedures.sp_UnlockUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userids", userids);
                _sqlHelper.GetCommonDataSet(command);
            }
            return "OK";
        }

        public List<Organization> GetUserOrgsByUserId(int UserId)
        {
            var lists = new List<Organization>();
            const string sql = StoredProcedures.Get_UserOrgsByUserId;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
            }
            return lists;
        }

        public List<UserProfile> GetAllUsersFromMasterDb()
        {
            var users = new List<UserProfile>();
            const string sql = StoredProcedures.Get_UsersList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var userProfile = new UserProfile();
                        userProfile.UserId = Convert.ToInt32(row["UserId"].ToString());
                        userProfile.UserName = row["UserName"].ToString();
                        userProfile.Email = row["Email"].ToString();
                        userProfile.FirstName = row["FirstName"].ToString();
                        userProfile.LastName = row["LastName"].ToString();
                        userProfile.Salt = Guid.Parse(row["Salt"].ToString());
                        userProfile.Password = row["Password"].ToString();
                        userProfile.PasswordHash = row["PasswordHash"].ToString();
                        users.Add(userProfile);
                    }
                }
            }
            return users;
        }


        public UserLogin GetUserOrgsByRefreshToken(string refreshToken)
        {
            var userlogins = new UserLogin();
            const string sql = StoredProcedures.Get_UserOrgsByRefreshToken;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@refreshToken", refreshToken);
                var dt = _sqlHelper.GetCommonDataTable(command);
                userlogins = _sqlHelper.ConvertDataTable<UserLogin>(dt).FirstOrDefault();
            }
            return userlogins;
        }

        public int DeleteUser(int userId)
        {
          int newId;
          const string sql = StoredProcedures.Delete_User;
          using (var command = new SqlCommand(sql))
          {
             command.CommandType = CommandType.StoredProcedure;
             command.Parameters.AddWithValue("@userId", userId);
             command.Parameters.Add("@status", SqlDbType.Int);
             command.Parameters["@status"].Direction = ParameterDirection.Output;
             newId = _sqlHelper.ExecuteNonQuery(command, "@status");
            }
            return newId;
        }
    }
}

// 1408 Line on 1 july 2021