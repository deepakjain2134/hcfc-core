//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;
//namespace HCF.BAL
//{
//    public static class FloorTypeRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorTypeId"></param>
//        /// <returns></returns>
//        public static FloorTypes GetFloorTypeFloors(int floorTypeId,int? CheckFileExmpty)
//        {
//            FloorTypes floorType = DAL.FloorTypeRepository.FloorTypes(floorTypeId);
//            if (floorType != null)
//                floorType.Floors = DAL.FloorTypeRepository.Floors(floorTypeId, CheckFileExmpty).Where(x => x.IsActive).ToList();           
//            return floorType;
//        }

//        public static List<FloorTypes> GetFloorTypes()
//        {
//            return DAL.FloorTypeRepository.FloorTypes(true);
//        }

//        //public static IEnumerable<FloorTypes> GetFloorTypes(int userId)
//        //{
//        //    return DAL.FloorTypeRepository.GetUserDrawings(userId);
//        //}
//    }
//}
