using System;
using HCF.BDO;
using System.Collections.Generic;
using System.Linq;
using HCF.DAL;

namespace HCF.BAL
{
    public class MainGoalService : IMainGoalService
    {
        private readonly IMainGoalRepository _mainGoalRepository;
        public MainGoalService(IMainGoalRepository mainGoalRepository)
        {
            _mainGoalRepository = mainGoalRepository;
        }
        #region MainGoal

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainGoal"></param>
        /// <returns></returns>
        public  int AddMaingoal(MainGoal mainGoal)
        {
            return _mainGoalRepository.Save(mainGoal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maingoal"></param>
        /// <returns></returns>
        public  int UpdateMainGoal(MainGoal maingoal)
        {
            return _mainGoalRepository.Update(maingoal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetMainGoals(int epDetailId)
        {
            return _mainGoalRepository.GetMainGoal(epDetailId);
        }

        //public  MainGoal GetMainGoal(int mainGoalId)
        //{
        //    return _mainGoalRepository.GetMainGoal(mainGoalId).FirstOrDefault();
        //}


        public  List<MainGoal> GetAllMainGoal(int? epDetailId)
        {
            return _mainGoalRepository.GetAllMainGoal(epDetailId);
        }

        public  List<MainGoal> GetAllMainGoal(int? epDetailId, int? activityType)
        {
            return _mainGoalRepository.GetAllMainGoal(epDetailId, activityType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<MainGoal> GetMainGoal()
        {
            return _mainGoalRepository.GetMainGoal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetActivityMainGoals(Guid activityId)
        {
            return _mainGoalRepository.GetActivityMainGoals(activityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<MainGoal> GetILSMMainGoal()
        {
            return _mainGoalRepository.GetIlsmMainGoal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetAssetMainGoals(int assetId)
        {
            return _mainGoalRepository.GetAssetMainGoals(assetId, null);
        }


        public  List<MainGoal> GetAssetMainGoals(int assetId, int epDetailId)
        {
            return _mainGoalRepository.GetAssetMainGoals(assetId, epDetailId);
        }

        public  MainGoal GetMainGoalStep(int mainGoalId)
        {
            var mainGoals = GetMainGoal().FirstOrDefault(x => x.MainGoalId == mainGoalId);
            return mainGoals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="activityType"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetGoalAndSteps(int epDetailId, int activityType, int? floorAssetId, int frequencyId , int? ClientNo)
        {
            return _mainGoalRepository.GetMainGoalByActivity(activityType, epDetailId, floorAssetId, frequencyId ,ClientNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityType"></param>
        /// <param name="ePdetailId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="inspectionId"></param>
        /// <param name="status"></param>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        public  List<MainGoal> GetGoalTransactionalRecords(int activityType, int ePdetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId)
        {
            return _mainGoalRepository.GetGoalTransactionalRecords(activityType, ePdetailId, floorAssetId, inspectionId, status, frequencyId);
        }

        public  List<MainGoal> GetGoalTransactionalRecords(Guid activityId)
        {
            return _mainGoalRepository.GetGoalTransactionalRecords(activityId);
        }

        public  MainGoal GetMainGoalById(int maingoalId)
        {
            return _mainGoalRepository.GetMainGoalById(maingoalId);
        }

        public  List<MainGoal> GetMainGoalsbyFloorAssetId(int clientno, int? floorAssetId, int? assetId)
        {
            return _mainGoalRepository.GetMainGoalsbyFloorAssetId(clientno, floorAssetId, assetId);
        }

        #endregion

    }
}
