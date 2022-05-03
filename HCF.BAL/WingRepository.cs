//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class WingRepository
//    {
//        public static int AddWing(Wing newWings)
//        {
//            return DAL.WingRepository.Save(newWings);
//        }

//        public static List<Wing> GetWings()
//        {
//            return DAL.WingRepository.GetWings();
//        }

//        public static List<Wing> GetBuildingWings(int buildingId)
//        {
//            return DAL.WingRepository.GetWings().Where(x => x.BuildingId == buildingId).ToList();
//        }

//        public static Wing GetWing(int wingId)
//        {
//            return DAL.WingRepository.GetWings().FirstOrDefault(x => x.WingId == wingId);
//        }

//        public static List<Wing> GetFloorWing(int floorId)
//        {
//            return DAL.WingRepository.GetWings(floorId);
//        }

//        public static int  UpdateWing(Wing updateWing)
//        {
//            return DAL.WingRepository.Save(updateWing);
//        }       
//    }
//}
