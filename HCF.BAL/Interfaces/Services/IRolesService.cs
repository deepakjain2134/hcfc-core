using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IRolesService
    {
        List<RolesInGroups> GetPermissionMatrix();
        List<Roles> GetPermissionMatrixByRoles();
        List<Roles> GetRoles();
        List<RolesInGroups> GetUserGroupPermission(int userGroupId);
        bool UpdatePermissionMatrix(RolesInGroups roleInGroup);
    }
}