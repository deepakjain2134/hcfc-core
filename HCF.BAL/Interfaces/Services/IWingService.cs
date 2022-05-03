using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IWingService
    {
        int AddWing(Wing newWings);
        List<Wing> GetBuildingWings(int buildingId);
        List<Wing> GetFloorWing(int floorId);
        Wing GetWing(int wingId);
        List<Wing> GetWings();
        int UpdateWing(Wing updateWing);
    }
}