using HCF.BDO;
using HCF.Web.Models;
using System;


namespace HCF.Web
{
    public static class MappingExtensions
    {   
        public static RecentLoginModel ToRecentLoginModel(this UserProfile entity)
        {
            if (entity == null)
                return null;

            var model = new RecentLoginModel
            {
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName=entity.LastName,
                FilePath=entity.FilePath,
                Name = entity.Name,
                FullName = entity.FullName,
                LoginDate=DateTime.UtcNow,
                UserId=entity.UserId
                
            };
            return model;
        }

        public static UserProfile ToUserProfile(this LoginModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserProfile
            {
                UserName = entity.UserName.Trim(),
                Password = entity.Password
            };
            return model;
        }
    }
}