using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IInsDetailRepository
    {
        List<TInspectionDetail> GetTInspectionDetails(Guid activityId);
        // List<TInspectionDetail> TInspectionDetail(Guid? activityId);
    }
}