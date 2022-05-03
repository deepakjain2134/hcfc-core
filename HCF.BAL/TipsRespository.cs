//using HCF.BDO;
//using System.Collections.Generic;

//namespace HCF.BAL
//{
//    public static class TipsRespository
//    { 
//        public static List<Tips> GetTipsByClientNo(int clientNo,string routeUrl)
//        {
//            return DAL.Repository.TipsRepository.GetTipsByClientNo(clientNo, routeUrl);
//        }
//        public static List<Tips> GetAllTips(int clientNo)
//        {
//            return DAL.Repository.TipsRepository.GetAllTipsByClientNo(clientNo);
//        }
//        public static bool InsertOrUpdateTip(Tips tip)
//        {
//          return DAL.Repository.TipsRepository.InsertOrUpdateTips(tip);
//        }

//        public static bool ApproveTip(int tipId,int ApproveStatus)
//        {
//            return DAL.Repository.TipsRepository.ApproveTip(tipId, ApproveStatus);
//        }
//    }
//}