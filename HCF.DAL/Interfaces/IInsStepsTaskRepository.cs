using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IInsStepsTaskRepository
    {
        List<TInsStepsTask> GetTInsStepsTask();
        List<TInsStepsTask> GetTInsStepsTask(Guid activityId);
        List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId);
        int Save(TInsStepsTask saveObject);
    }
}