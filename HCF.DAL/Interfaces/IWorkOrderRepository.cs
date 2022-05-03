using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IWorkOrderRepository
    {
        List<FloorAssetsWorkOrderCount> GetFloorAssetsWorkOrderCount();
        List<WorkOrder> GetFloorWorkOrder();
        List<WorkOrder> GetFloorWorkOrder(int floorId);
        List<TFloorAssets> GetIssuesFloorAssets(int issueId);
        List<WorkOrder> GetRoundWorkOrders(int? buildingId = null, int? floorId = null);
        List<SubStatus> GetSubStatus();
        List<WorkOrder> GetWorkOrder(Guid activityId, int stepsId);
        WorkOrder GetWorkOrder(int issueId);
        WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber);
        int GetWorkOrderCount(string dbName, string date);
        List<WorkOrder> GetWorkOrders();
        HttpResponseMessage GetWorkOrdersPaging(Request objRequest, int? userId);
        List<WorkOrder> LinkWorkorder(string assetno);
        List<WorkOrder> QueueWorkOrders();
        int Save(WorkOrder newWorkOrder);
        int SaveTRounWorkOrder(TRoundWorkOrders objTRoundWorkOrders);
        int SaveWorkOrderFailStep(int issueId, Guid activityId, InspectionSteps inspectionSteps);
        int SaveWorkOrderFiles(int issueId, WorkOrderfiles files);
        bool SaveWorkOrderResource(int issueId, UserProfile user);
        bool SaveWorkOrderResources(int issueId, string userIds, string floorAssetsIds, string tinsStepsId, string activityIds, string tmsAssetsIds, string resourceIds);
        int UpdatetWorkOrder(WorkOrder newWorkOrder);
        bool UpdateWorkOrderId(int issueId, int woId, int woNumber, string sourceWoId = null);
        int UpdateTempWorkOrder(WorkOrder newWorkOrder);
    }
}