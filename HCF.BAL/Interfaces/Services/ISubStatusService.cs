using HCF.BDO;
using System.Collections.Generic;

namespace HCF.BAL
{
    public interface ISubStatusService
    {
        int Save(SubStatus substatus);
        List<SubStatus> GetSubStatus();
    }
}