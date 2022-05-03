using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IWingRepository
    {
        Wing GetWing(int wingId);
        List<Wing> GetWings();
        List<Wing> GetWings(int floorId);
        int Save(Wing wing);
    }
}