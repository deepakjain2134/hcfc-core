using HCF.BDO;
using HCF.BDO.Users;
using HCF.Infrastructure.Data;
using HCF.Module.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCF.Module.Core.Data
{
    public class CoreCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserProfile>()
               .ToTable("UserProfile");

            // modelBuilder.Entity<AppSetting>().ToTable("Core_AppSetting");

            //            modelBuilder.Entity<User>()
            //                .ToTable("Core_User");

            modelBuilder.Entity<Role>()
                .ToTable("Core_Role");

            modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("Core_UserClaim");
            });

            modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("Core_RoleClaim");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.HasOne(ur => ur.Role).WithMany(x => x.Users).HasForeignKey(r => r.RoleId);
                //b.HasOne(ur => ur.User).WithMany(x => x.Roles).HasForeignKey(u => u.UserId);
                b.ToTable("Core_UserRole");
            });

            modelBuilder.Entity<UsedPassword>()
               .ToTable("UsedPassword");

            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.ToTable("Core_UserToken");
            });

            //            modelBuilder.Entity<Entity>(e =>
            //            {
            //                e.HasKey(x => x.Id);
            //                e.Property(x => x.EntityId);
            //            });

            modelBuilder.Entity<UserLogin>()
             .ToTable("UserLogin");

            modelBuilder.Entity<DeviceTypes>()
          .ToTable("DeviceTypes");

            modelBuilder.Entity<CityMaster>(b =>
            {
                b.ToTable("CityMaster");
            });


            CoreSeedData.SeedData(modelBuilder);
        }
    }
}
