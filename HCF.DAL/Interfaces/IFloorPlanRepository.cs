using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFloorPlanRepository
    {
        bool DeleteFloorPlan(Guid floorPlanId);
        List<FloorPlan> FloorPlans();
        List<DrawingViewer> GetDrawingViewer();
        List<FloorPlan> GetFloorPlans(int floorId);
        int SaveDrawingViewer(DrawingViewer newDrawingViewer);
        bool UpdateActiveFloorPlan(Guid floorPlanId);
        int UpdatePermitsDrawingViewer(string PermitNo, string TempPermitNo, int? ModifiedBy = 0);
    }
}