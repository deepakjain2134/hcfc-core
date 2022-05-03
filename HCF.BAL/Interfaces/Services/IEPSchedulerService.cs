using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IEPSchedulerService
    {
        List<EPScheduler> GetEPSchedulers();
        int Save(EPScheduler scheduler);
    }
}