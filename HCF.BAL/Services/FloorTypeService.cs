using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace HCF.BAL
{
    public class FloorTypeService :IFloorTypeService
    {
        private readonly IFloorTypeRepository _floorTypeRepository;
        private readonly IFloorRepository _floorRepository;
        public FloorTypeService(IFloorTypeRepository floorTypeRepository, IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
            _floorTypeRepository = floorTypeRepository;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorTypeId"></param>
        /// <returns></returns>
        public FloorTypes GetFloorTypeFloors(int floorTypeId,int? CheckFileExmpty)
        {
            var floorType = _floorTypeRepository.FloorTypes(floorTypeId);
            if (floorType != null)
                floorType.Floors = _floorRepository.GetFloorsByFloorType(floorTypeId, CheckFileExmpty).Where(x => x.IsActive).ToList();           
            return floorType;
        }


        public List<FloorTypes> GetFloorTypes()
        {
            return _floorTypeRepository.FloorTypes(true);
        }

        public  IEnumerable<FloorTypes> GetFloorTypes(int userId)
        {
            return _floorTypeRepository.GetUserDrawings(userId);
        }


    }
}
