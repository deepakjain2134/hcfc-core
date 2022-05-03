using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IShiftRepository
    {
        List<Shift> GetShift();
        int Save(Shift shift);
    }
}