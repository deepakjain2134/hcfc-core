using HCF.BDO;
using System.Collections.Generic;

namespace HCF.BAL
{
    public interface IFloorShapesService
    {
        int SaveFloorShapes(FloorShapes newFloorShapes);
        bool SaveFloorShapes(int id);
        List<FloorShapes> GetFloorShapes();
    }
}