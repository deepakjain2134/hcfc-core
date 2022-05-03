//using HCF.BDO;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class StatusRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="status"></param>
//        /// <returns></returns>
//        public static int Save(WoStatus status)
//        {
//            return DAL.StatusRepository.Save(status);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<WoStatus> GetStatus()
//        {
//            return DAL.StatusRepository.GetStatus();
//        }
//    }
//    #region Shift

//    public static class ShiftRepository
//    {
        
//        public static int Save(Shift shift)
//        {
//            return DAL.ShiftRepository.Save(shift);
//        }

       
//        public static List<Shift> GetShift()
//        {
//            return DAL.ShiftRepository.GetShift();
//        }
//    }

//    #endregion
//}