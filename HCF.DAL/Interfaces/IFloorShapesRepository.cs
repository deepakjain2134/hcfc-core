using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFloorShapesRepository
    {
        bool DeleteFloorShapes(int id);
        IEnumerable<FloorShapes> GetFloorShapes();
        List<FloorShapes> GetFloorShapes(int floorId);
        int Save(FloorShapes newFloorShapes);
        int SaveTFloorAssetsShaps(TFloorAssetsShaps newShapes);
    }
}