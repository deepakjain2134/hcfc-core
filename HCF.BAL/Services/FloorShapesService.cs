using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;



namespace HCF.BAL
{
    public class FloorShapesService : IFloorShapesService
    {
        private readonly IFloorShapesRepository _floorShapesRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;

        public FloorShapesService(IFloorShapesRepository floorShapesRepository, IFloorAssetRepository floorAssetRepository)
        {
            _floorAssetRepository = floorAssetRepository;
               _floorShapesRepository = floorShapesRepository;
        }
        public  List<FloorShapes> GetFloorShapes()
        {
            return _floorShapesRepository.GetFloorShapes().ToList();
        }

        public int SaveFloorShapes(FloorShapes newFloorShapes)
        {
            var mlist = GetMaxMinValue(newFloorShapes.Data);
            newFloorShapes.XMin = mlist[0];
            newFloorShapes.XMax = mlist[1];
            newFloorShapes.YMin = mlist[2];
            newFloorShapes.YMax = mlist[3];
            var floorShapesId = _floorShapesRepository.Save(newFloorShapes);
            if (floorShapesId <= 0)
                return floorShapesId;

            newFloorShapes.Id = floorShapesId;
            //_floorAssetRepository.SaveLocation(newFloorShapes);
            return floorShapesId;
        }

        public bool SaveFloorShapes(int id)
        {
            return _floorShapesRepository.DeleteFloorShapes(id);
        }

        private List<string> GetMaxMinValue(string json)
        {
            //string json = _json;//"{\"Id\":0,\"lineList\":[{\"x\":1593.951,\"y\":1474.6357},{\"x\":2762.516,\"y\":1171.2087},{\"x\":2680.4841,\"y\":2491.6484}],\"modalPolygon\":{\"isFill\":false,\"modalTxt\":{\"overlayId\":\"\",\"text\":\"test\",\"textColor\":\"0\",\"textSize\":0,\"transparency\":0,\"typefaceStyle\":0,\"typefaceType\":0},\"thickness\":0,\"transparencylevel\":0,\"type\":0}}";

            return new List<string>();
        }
    }
}
