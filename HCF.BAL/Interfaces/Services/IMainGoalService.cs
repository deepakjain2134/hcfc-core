using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IMainGoalService
    {
        int AddMaingoal(MainGoal mainGoal);
        List<MainGoal> GetActivityMainGoals(Guid activityId);
        List<MainGoal> GetAllMainGoal(int? epDetailId);
        List<MainGoal> GetAllMainGoal(int? epDetailId, int? activityType);
        List<MainGoal> GetAssetMainGoals(int assetId);
        List<MainGoal> GetAssetMainGoals(int assetId, int epDetailId);
        List<MainGoal> GetGoalAndSteps(int epDetailId, int activityType, int? floorAssetId, int frequencyId, int? ClientNo);
        List<MainGoal> GetGoalTransactionalRecords(Guid activityId);
        List<MainGoal> GetGoalTransactionalRecords(int activityType, int ePdetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId);
        List<MainGoal> GetILSMMainGoal();
        List<MainGoal> GetMainGoal();
        MainGoal GetMainGoalById(int maingoalId);
        List<MainGoal> GetMainGoals(int epDetailId);
        List<MainGoal> GetMainGoalsbyFloorAssetId(int clientno, int? floorAssetId, int? assetId);
        MainGoal GetMainGoalStep(int mainGoalId);
        int UpdateMainGoal(MainGoal maingoal);
    }
}