using HCF.BDO;
using HCF.Module.Core.Areas.Core.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Extensions
{
    //public static class MappingExtensions
    //{
    //    public static RecentLoginModel ToRecentLoginModel(this UserProfile entity)
    //    {
    //        if (entity == null)
    //            return null;

    //        var model = new RecentLoginModel
    //        {
    //            UserName = entity.UserName,
    //            Email = entity.Email,
    //            FirstName = entity.FirstName,
    //            LastName = entity.LastName,
    //            FilePath = entity.FilePath,
    //            Name = entity.Name,
    //            FullName = entity.FullName,
    //            LoginDate = DateTime.UtcNow,
    //            UserId = entity.UserId

    //        };
    //        return model;
    //    }

    //    public static UserProfile ToUserProfile(this LoginViewModel entity)
    //    {
    //        if (entity == null)
    //            return null;

    //        var model = new UserProfile
    //        {
    //            UserName = entity.UserName.Trim(),
    //            Password = entity.Password
    //        };
    //        return model;
    //    }

    //    public static UserProfile ToUserProfile(this UserAddViewModel viewModel)
    //    {
    //        var userToSave = new UserProfile
    //        {
    //            UserId = viewModel.UserId,
    //            UserName = viewModel.Email,
    //            Email = viewModel.Email,
    //            FirstName = viewModel.FirstName,
    //            LastName = viewModel.LastName,
    //            CellNo = viewModel.CellNumber,
    //            PhoneNumber = viewModel.PhoneNumber,
    //            Password = viewModel.Password,
    //            IsActive = viewModel.IsActive,
    //            IsPowerUser = viewModel.IsPowerUser,
    //            IsPwdChange = viewModel.IsPwdChange,
    //            VendorId = viewModel.VendorId > 0 ? viewModel.VendorId : null,
    //            FileName = viewModel.FileName,
    //            FileContent = viewModel.FileContent

    //        };

    //        if (viewModel.DrpUserGroupsIds != null && viewModel.DrpUserGroupsIds.Length > 0)
    //            userToSave.UserGroupIds = string.Join(",", viewModel.DrpUserGroupsIds);
    //        if (viewModel.DrpUserAdditionRoleIds != null && viewModel.DrpUserAdditionRoleIds.Length > 0)
    //            userToSave.UserAdditionalRoleIds = string.Join(",", viewModel.DrpUserAdditionRoleIds);
    //        if (viewModel.DrpDrawingsIds != null && viewModel.DrpDrawingsIds.Length > 0)
    //            userToSave.DrawingIds = viewModel.DrpDrawingsIds;
    //        return userToSave;
    //    }
    //}
}
