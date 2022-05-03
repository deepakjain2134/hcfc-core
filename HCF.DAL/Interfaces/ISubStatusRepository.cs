using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ISubStatusRepository
    {
        int Save(SubStatus substatus);
        List<SubStatus> GetSubStatus();
    }
}