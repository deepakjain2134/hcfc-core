using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IStatusRepository
    {
        List<WoStatus> GetStatus();
        int Save(WoStatus status);
    }
}