using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IStepsService
    {
        List<Steps> GetAllSteps();
        List<Steps> GetRASetps();
        Steps GetStep(int stepId);
        List<Steps> GetSteps();
        List<Steps> GetSteps(int mainGoalId);
        List<TComment> GetStepsComments(Guid activityId, int tIlsmId);
        Steps Save(Steps step);
        bool Update(Steps step);
    }
}