using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IStatusService
    {
        List<WoStatus> GetStatus();
        int Save(WoStatus status);
    }
}