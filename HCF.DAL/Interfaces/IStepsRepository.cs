using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IStepsRepository
    {
        List<Steps> GetAllSteps();
        Steps GetStep(int stepId);
        List<Steps> GetSteps();
        List<Steps> GetSteps(bool isRaSteps);
        List<Steps> GetSteps(int mainGoalId);
        List<TComment> GetStepsComments(Guid activityId, int tIlsmId);
        Steps Save(Steps step);
        List<Steps> SetTransSteps();
        bool Update(Steps step);
    }
}