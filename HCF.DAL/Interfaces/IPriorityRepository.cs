using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IPriorityRepository
    {
        List<Priority> GetPriority();
        int Save(Priority newPriority);
    }
}