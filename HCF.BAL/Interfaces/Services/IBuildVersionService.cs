using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IBuildVersionService
    {
        List<BuildVersion> GetBuildVersions();
        BuildVersion GetCurrentVersion();
    }
}