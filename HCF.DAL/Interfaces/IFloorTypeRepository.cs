using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFloorTypeRepository
    {
        //List<Floor> Floors(int floorTypeId, int? CheckFileEmpty);
        List<FloorTypes> FloorTypes(bool status);
        FloorTypes FloorTypes(int floorTypeId);
        IEnumerable<FloorTypes> GetUserDrawings(int userId);
    }
}