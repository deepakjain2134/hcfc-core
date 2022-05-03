using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IPriorityService
    {
        List<Priority> GetPriority();
        int Save(Priority newPriority);
    }
}