using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using HCF.Utility;

namespace HCF.DAL
{
    public class RolesRepository : IRolesRepository
    {
        #region ctor
        private readonly ISqlHelper _sqlHelper;
        public RolesRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<RolesInGroups> GetPermissionMatrix()
        {
            var lists = new List<RolesInGroups>();
            const string sql = StoredProcedures.Get_Permissions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                foreach (DataRow row in dt.Rows)
                {
                    // int? parentId = (string.IsNullOrEmpty(row["ParentId"].ToString())) ? 0 : Convert.ToInt32(row["ParentId"].ToString());
                    var list = new RolesInGroups
                    {
                        Status = Convert.ToBoolean(row["Status"].ToString()),
                        RoleId = Convert.ToInt32(row["RoleId"].ToString()),
                        GroupId = string.IsNullOrWhiteSpace(row["GroupId"].ToString())
                            ? 0
                            : Convert.ToInt32(row["GroupId"].ToString()),
                        Roles = new Roles
                        {
                            RoleName = row["RoleName"].ToString(),
                            RoleId = Convert.ToInt32(row["RoleId"].ToString()),
                            DisplayText = row["DisplayText"].ToString(),
                            ParentId = (string.IsNullOrEmpty(row["ParentId"].ToString())) ? 0 : Convert.ToInt32(row["ParentId"].ToString()),
                            IsChild = Convert.ToBoolean(row["IsChild"].ToString())
                        },
                        UserGroup = new UserGroup
                        {
                            GroupId = Convert.ToInt32(row["GroupId"].ToString()),
                            Name = row["Name"].ToString(),
                            IsReadOnly = Convert.ToBoolean(row["IsReadOnly"].ToString())
                        }
                    };
                    lists.Add(list);
                }
            }

            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Roles> GetPermissionMatrixbyRoles()
        {
            var lists = GetPermissionMatrix();
            var rolesLists = GetRoles();
            foreach (var item in rolesLists)
                item.RolesInGroups = lists.Where(x => x.RoleId == item.RoleId).ToList();

            return rolesLists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Roles> GetRoles()
        {
            List<Roles> roles;
            const string sql = StoredProcedures.Get_Roles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                roles = _sqlHelper.ConvertDataTable<Roles>(dt);
            }
            return roles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleInGroup"></param>
        /// <returns></returns>
        public bool UpdatePermissionMatrix(RolesInGroups roleInGroup)
        {
            const string sql = StoredProcedures.Update_RoleInGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoleId", roleInGroup.RoleId);
                command.Parameters.AddWithValue("@GroupId", roleInGroup.GroupId);
                command.Parameters.AddWithValue("@Status", roleInGroup.Status);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Created by & Date : Avinash Varshney & 03-11-2016
        /// to get all the Permission according to user group
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        public List<RolesInGroups> GetUserGroupPermission(int userGroupId)
        {
            var lists = new List<RolesInGroups>();
            const string sql = StoredProcedures.Get_UserGroupPermissions;
            try
            {
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserGroupId", userGroupId);
                    var dt = _sqlHelper.GetDataTable(command);
                    foreach (DataRow row in dt.Rows)
                    {
                        var list = new RolesInGroups
                        {
                            Status = Convert.ToBoolean(row["Status"].ToString()),
                            RoleId = Convert.ToInt32(row["RoleId"].ToString()),
                            GroupId = string.IsNullOrWhiteSpace(row["GroupId"].ToString())
                                ? 0
                                : Convert.ToInt32(row["GroupId"].ToString()),
                            Roles = new Roles { RoleName = row["RoleName"].ToString() },
                            UserGroup = new UserGroup
                            {
                                Name = row["Name"].ToString()
                            }
                        };
                        lists.Add(list);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return lists;
        }
    }
}
