using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFloorTypeService
    {
        FloorTypes GetFloorTypeFloors(int floorTypeId, int? CheckFileExmpty);
        List<FloorTypes> GetFloorTypes();
        IEnumerable<FloorTypes> GetFloorTypes(int userId);
    }
}