using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IInsStepsTaskService
    {
        List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId);
        int Save(TInsStepsTask tInsStepsTask);
    }
}