using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IStandardRepository
    {
        List<Standard> DocumentStandardReports();
        List<Standard> Get();
        Standard GetEpHistory(int epDetailId);
        List<Standard> GetEPlists();
        Standard GetStandard(int stdescId);
        List<Standard> GetStandards();
        List<EPDetails> GetUserEPs(int userId);
        int Save(Standard standard);
        bool UpdateStandard(Standard standard);
    }
}