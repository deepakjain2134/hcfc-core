//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class StandardRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="epdetailId"></param>
//        /// <returns></returns>
//        public static Standard GetEpHistory(int epdetailId)
//        {
//            return DAL.StandardRepository.GetEpHistory(epdetailId);
//        }

//        public static List<Standard> Get()
//        {
//            return DAL.StandardRepository.Get();
//        }

//        public static Standard Get(int stdescId)
//        {
//            return DAL.StandardRepository.Get().FirstOrDefault(x=>x.StDescID== stdescId);
//        }

//        public static int AddStandard(Standard newdata)
//        {
//            return DAL.StandardRepository.Save(newdata);
//        }

//        public static List<Standard> GetStandards()
//        {
//            return DAL.StandardRepository.GetStandards();
//        }


//        public static List<Standard> DocumentStandardReports()
//        {
//            return DAL.StandardRepository.DocumentStandardReports();
//        }

//        public static Standard GetStandard(int stdescId)
//        {
//            return DAL.StandardRepository.GetStandard(stdescId);
//        }

//        public static bool UpdateStandard(Standard newdata)
//        {
//            return DAL.StandardRepository.UpdateStandard(newdata);
//        }

//        public static List<Standard> GetEPlists()
//        {
//            return DAL.StandardRepository.GetEPlists();
//        }

//        public static List<EPDetails> GetUserEPs(int userId)
//        {
//            return DAL.StandardRepository.GetUserEPs(userId);
//        }
//    }
//}
