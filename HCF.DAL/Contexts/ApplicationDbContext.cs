using HCF.BDO;
using Microsoft.EntityFrameworkCore;

namespace HCF.DAL.Contexts
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<DeviceTypes> DeviceTypes { get; set; }

        public DbSet<Activity> Activity { get; set; }
    }
}