using Microsoft.EntityFrameworkCore;

namespace HCF.Infrastructure.Data
{
    public interface ICustomModelBuilder
{
        void Build(ModelBuilder modelBuilder);
    }
}
