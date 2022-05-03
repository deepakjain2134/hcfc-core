using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class BuildVersionService : IBuildVersionService
    {
        public readonly IBuildVersionRepository _buildVersionRepository;

        public BuildVersionService(IBuildVersionRepository buildVersionRepository)
        {
            _buildVersionRepository = buildVersionRepository;
        }
        public List<BuildVersion> GetBuildVersions()
        {
            return _buildVersionRepository.GetBuildVersions();
        }

        public BuildVersion GetCurrentVersion()
        {
            return _buildVersionRepository.GetBuildVersions().FirstOrDefault(x => x.IsCurrent);
        }
    }
}
