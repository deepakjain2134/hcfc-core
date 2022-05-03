using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IInsStepsService
    {
        List<InspectionSteps> GetInspectionSteps();
        InspectionSteps GetInspectionSteps(int tInsStepsId);
        List<InspectionSteps> GetTinspectionStepsByActivityId(Guid activityId);
        TInspectionActivity GetTransactionSteps(Guid activityId);
        bool Update(InspectionSteps inspectionSteps, int? createdBy);
        bool Update(InspectionSteps inspectionSteps, int? createdBy, Guid activityId);

        int Save(TInsStepsTask tInsStepsTask);

        List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId);
    }
}