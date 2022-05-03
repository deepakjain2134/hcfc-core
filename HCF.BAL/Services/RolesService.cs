using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
namespace HCF.BAL
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<RolesInGroups> GetPermissionMatrix()
        {
            return _rolesRepository.GetPermissionMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Roles> GetPermissionMatrixByRoles()
        {
            return _rolesRepository.GetPermissionMatrixbyRoles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleInGroup"></param>
        /// <returns></returns>
        public  bool UpdatePermissionMatrix(RolesInGroups roleInGroup)
        {
            return _rolesRepository.UpdatePermissionMatrix(roleInGroup);
        }

        /// <summary>
        /// Created by & Date : Avinash Varshney & 03-11-2016
        /// to get all the Permisssion according to user group
        /// </summary>
        /// <returns></returns>
        public List<RolesInGroups> GetUserGroupPermission(int userGroupId)
        {
            return _rolesRepository.GetUserGroupPermission(userGroupId);
        }


        public List<Roles> GetRoles()
        {
            return _rolesRepository.GetRoles();
        }
    }
}
