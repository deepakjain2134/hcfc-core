using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL.Interfaces.Services
{
    public interface IWorkOrderService
    {
        int AddWorkOrder(WorkOrder newWorkOrder);
        List<FloorAssetsWorkOrderCount> GetFloorAssetsWorkOrderCount();
        List<WorkOrder> GetFloorWorkOrder();
        List<WorkOrder> GetFloorWorkOrder(int floorId);
        List<TFloorAssets> GetIssuesFloorAssets(int issueId);
        List<WorkOrder> GetRoundWorkOrders();
        List<WorkOrder> GetRoundWorkOrders(int? buildingId, int? floorId);
        List<SubStatus> GetSubStatus();
        WorkOrder GetWorkOrder(int issueId);
        WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber);
        List<WorkOrder> GetWorkOrders();
        HttpResponseMessage GetWorkOrdersPaging(Request objRequest, int? userId);
        List<WorkOrder> LinkWorkorder(string assetid);
        List<WorkOrder> QueueWorkOrders();
        int SaveTRounWorkOrder(TRoundWorkOrders objTRoundWorkOrders);
        int SaveWorkOrderFailStep(int issueId, Guid activityId, InspectionSteps inspectionSteps);
        int SaveWorkOrderFiles(int issueId, WorkOrderfiles file);
        bool SaveWorkOrderResource(int issueId, UserProfile user);
        bool SaveWorkOrderResources(int issueId, string userIds, string floorAssetsIds, string tinsStepsId, string activityIds, string tmsReferenceIds, string resourceIds);
        int UpdateWorkOrder(WorkOrder newWorkOrder);
        bool UpdateWorkOrderId(int issueId, int woId, int woNumber, string sourceWoId = null);
        int UpdateTempWorkOrder(WorkOrder newWorkOrder);
    }
}