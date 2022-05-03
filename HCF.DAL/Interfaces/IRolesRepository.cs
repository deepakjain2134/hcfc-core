using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IRolesRepository
    {
        List<RolesInGroups> GetPermissionMatrix();
        List<Roles> GetPermissionMatrixbyRoles();
        List<Roles> GetRoles();
        List<RolesInGroups> GetUserGroupPermission(int userGroupId);
        bool UpdatePermissionMatrix(RolesInGroups roleInGroup);
    }
}