

using HCF.Infrastructure.Data;
using HCF.Infrastructure.Models;

namespace HCF.Module.Core.Data
{
    public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
       where T : class, IEntityWithTypedId<long>
    {
        public Repository(HcfDatabaseContext context) : base(context)
        {
        }
    }
}
