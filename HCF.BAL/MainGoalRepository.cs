//using System;
//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;
//namespace HCF.BAL
//{
//    public class MainGoalRepository
//    {
//        #region MainGoal

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="mainGoal"></param>
//        /// <returns></returns>
//        public static int AddMaingoal(MainGoal mainGoal)
//        {
//            return DAL.MainGoalRepository.Save(mainGoal);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="maingoal"></param>
//        /// <returns></returns>
//        public static int UpdateMainGoal(MainGoal maingoal)
//        {
//            return DAL.MainGoalRepository.Update(maingoal);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="epDetailId"></param>
//        /// <returns></returns>
//        public static List<MainGoal> GetMainGoals(int epDetailId)
//        {
//            return DAL.MainGoalRepository.GetMainGoal(epDetailId);
//        }

//        //public static MainGoal GetMainGoal(int mainGoalId)
//        //{
//        //    return DAL.MainGoalRepository.GetMainGoal(mainGoalId).FirstOrDefault();
//        //}


//        public static List<MainGoal> GetAllMainGoal(int? epDetailId)
//        {
//            return DAL.MainGoalRepository.GetAllMainGoal(epDetailId);
//        }

//        public static List<MainGoal> GetAllMainGoal(int? epDetailId, int? activityType)
//        {
//            return DAL.MainGoalRepository.GetAllMainGoal(epDetailId, activityType);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<MainGoal> GetMainGoal()
//        {
//            return DAL.MainGoalRepository.GetMainGoal();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="activityId"></param>
//        /// <returns></returns>
//        public static List<MainGoal> GetActivityMainGoals(Guid activityId)
//        {
//            return DAL.MainGoalRepository.GetActivityMainGoals(activityId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<MainGoal> GetILSMMainGoal()
//        {
//            return DAL.MainGoalRepository.GetIlsmMainGoal();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="assetId"></param>
//        /// <returns></returns>
//        public static List<MainGoal> GetAssetMainGoals(int assetId)
//        {
//            return DAL.MainGoalRepository.GetAssetMainGoals(assetId, null);
//        }


//        public static List<MainGoal> GetAssetMainGoals(int assetId, int epDetailId)
//        {
//            return DAL.MainGoalRepository.GetAssetMainGoals(assetId, epDetailId);
//        }

//        public static MainGoal GetMainGoalStep(int mainGoalId)
//        {
//            var mainGoals = GetMainGoal().FirstOrDefault(x => x.MainGoalId == mainGoalId);
//            return mainGoals;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="epDetailId"></param>
//        /// <param name="activityType"></param>
//        /// <param name="floorAssetId"></param>
//        /// <param name="frequencyId"></param>
//        /// <returns></returns>
//        public static List<MainGoal> GetGoalAndSteps(int epDetailId, int activityType, int? floorAssetId, int frequencyId , int? ClientNo)
//        {
//            return DAL.MainGoalRepository.GetMainGoalByActivity(activityType, epDetailId, floorAssetId, frequencyId ,ClientNo);
//        }

//        public static List<MainGoal> GetMainGoalByActivity(int epDetailId, int activityType, int? floorAssetId, int frequencyId, int? ClientNo)
//        {
//            return DAL.MainGoalRepository.GetMainGoalByActivity(activityType, epDetailId, floorAssetId, frequencyId, ClientNo);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="activityType"></param>
//        /// <param name="ePdetailId"></param>
//        /// <param name="floorAssetId"></param>
//        /// <param name="inspectionId"></param>
//        /// <param name="status"></param>
//        /// <param name="frequencyId"></param>
//        /// <returns></returns>
//        public static List<MainGoal> GetGoalTransactionalRecords(int activityType, int ePdetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId)
//        {
//            return DAL.MainGoalRepository.GetGoalTransactionalRecords(activityType, ePdetailId, floorAssetId, inspectionId, status, frequencyId);
//        }

//        public static List<MainGoal> GetGoalTransactionalRecords(Guid activityId)
//        {
//            return DAL.MainGoalRepository.GetGoalTransactionalRecords(activityId);
//        }

//        public static MainGoal GetMainGoalById(int maingoalId)
//        {
//            return DAL.MainGoalRepository.GetMainGoalById(maingoalId);
//        }

//        public static List<MainGoal> GetMainGoalsbyFloorAssetId(int clientno, int? floorAssetId, int? assetId)
//        {
//            return DAL.MainGoalRepository.GetMainGoalsbyFloorAssetId(clientno, floorAssetId, assetId);
//        }

//        #endregion

//    }
//}
