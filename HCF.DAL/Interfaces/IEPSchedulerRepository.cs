using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IEPSchedulerRepository
    {
        List<EPScheduler> GetEpSchedulers();
        int Save(EPScheduler scheduler);
    }
}