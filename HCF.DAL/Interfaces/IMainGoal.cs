using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Interface
{
    interface IMainGoal
    {
        int Save(MainGoal maingoal);

        List<MainGoal> GetActivityMainGoals(Guid activityId);

        int UpdateMainGoal(MainGoal maingoal);

        List<MainGoal> GetMainGoal(int? epDetailId = null, int? activityType = null);

        List<MainGoal> GetAssetMainGoals(int assetId);

        List<MainGoal> GetMainGoalByActivity(int activityType, int epDetailId, int? floorAssetId, int frequencyId);

        List<MainGoal> GetGoalTransactionalRecords(int activityType, int ePdetailId, int? floorAssetId, int? inspectionId, int status, int? frequencyId);

        List<MainGoal> GetILSMMainGoal();
    }
}
