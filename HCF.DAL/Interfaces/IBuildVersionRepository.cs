using HCF.BDO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IBuildVersionRepository
    {
        List<BuildVersion> GetBuildVersions();

        Task<IEnumerable<BuildVersion>> GetAsync();
    }
}