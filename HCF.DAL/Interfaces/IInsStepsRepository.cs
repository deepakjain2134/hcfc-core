using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IInsStepsRepository
    {
        int AddInspectionSteps(InspectionSteps inspectionSteps);
        List<InspectionSteps> GetInspectionSteps();
        List<InspectionSteps> GetInspectionSteps(Guid activityId);
        List<InspectionSteps> GetInspectionSteps(int inspectionId, int epDetailId);
        List<InspectionSteps> GetTinspectionStepsByActivityId(Guid activityId);
        List<InspectionSteps> GetTinspectionStepsByFrequency(int? frequencyId, int? floorAssetId, int? stepsId, int? activityType);
        TInspectionActivity GetTransactionSteps(Guid activityId);
        bool Update(InspectionSteps inspectionSteps, int createdBy);
        bool Update(InspectionSteps inspectionSteps, int createdBy, Guid activityId);
    }
}