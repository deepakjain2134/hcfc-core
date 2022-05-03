using HCF.BDO;
using HCF.Web.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.ExtensionMethods
{

    public static partial class UserAddViewModelExtensions
    {
        /// <summary>
        /// Converts a add view model to a user profile
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static UserProfile ToUserProfile(this UserAddViewModel viewModel)
        {
            var userToSave = new UserProfile
            {
                UserId = viewModel.UserId,
                UserName = viewModel.Email,
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                CellNo = viewModel.CellNumber,
                PhoneNumber = viewModel.PhoneNumber,
                Password = viewModel.Password,
                IsActive = viewModel.IsActive,
                IsPowerUser = viewModel.IsPowerUser,
                IsPwdChange = viewModel.IsPwdChange,
                VendorId = viewModel.VendorId > 0 ? viewModel.VendorId : null,
                FileName=viewModel.FileName,
                FileContent=viewModel.FileContent,
                OrientationDate = viewModel.OrientationDate
                
            };

            if (viewModel.DrpUserGroupsIds != null && viewModel.DrpUserGroupsIds.Length > 0)
                userToSave.UserGroupIds = string.Join(",", viewModel.DrpUserGroupsIds);
            if (viewModel.DrpUserAdditionRoleIds != null && viewModel.DrpUserAdditionRoleIds.Length > 0)
                userToSave.UserAdditionalRoleIds = string.Join(",", viewModel.DrpUserAdditionRoleIds);
            if (viewModel.DrpDrawingsIds != null && viewModel.DrpDrawingsIds.Length > 0)
                userToSave.DrawingIds = viewModel.DrpDrawingsIds;
            return userToSave;
        }
    }
}