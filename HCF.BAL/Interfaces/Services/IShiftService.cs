using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IShiftService
    {
        List<Shift> GetShift();
        int Save(Shift shift);
    }
}