using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IMainGoalRepository
    {
        List<MainGoal> GetActivityMainGoals(Guid activityId);
        List<MainGoal> GetAllMainGoal(int? epDetailId = null, int? activityType = null);
        List<MainGoal> GetAssetMainGoals(int assetId, int? epdetailId);
        List<MainGoal> GetGoalTransactionalRecords(Guid activityId);
        List<MainGoal> GetGoalTransactionalRecords(int activityType, int epDetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId);
        List<MainGoal> GetIlsmMainGoal();
        List<MainGoal> GetMainGoal(int? epDetailId = null, int? activityType = null);
        List<MainGoal> GetMainGoalByActivity(int activityType, int epDetailId, int? floorAssetId, int frequencyId, int? ClientNo);
        MainGoal GetMainGoalById(int maingoalId);
        List<MainGoal> GetMainGoalsbyFloorAssetId(int clientno, int? floorAssetId, int? assetId);
        int Save(MainGoal maingoal);
        int Update(MainGoal maingoal);
    }
}