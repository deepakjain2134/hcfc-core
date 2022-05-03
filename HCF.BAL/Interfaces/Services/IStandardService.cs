using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IStandardService
    {
        int AddStandard(Standard newdata);
        List<Standard> DocumentStandardReports();
        List<Standard> Get();
        Standard Get(int stdescId);
        Standard GetEpHistory(int epdetailId);
        List<Standard> GetEPlists();
        Standard GetStandard(int stdescId);
        List<Standard> GetStandards();
        List<EPDetails> GetUserEPs(int userId);
        bool UpdateStandard(Standard newdata);
    }
}