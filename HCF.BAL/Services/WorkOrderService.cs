using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        public WorkOrderService(IWorkOrderRepository workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
        }
        #region Work Order

        public int AddWorkOrder(HCF.BDO.WorkOrder newWorkOrder)
        {
            return _workOrderRepository.Save(newWorkOrder);
        }

        public int UpdateWorkOrder(HCF.BDO.WorkOrder newWorkOrder)
        {
            return _workOrderRepository.UpdatetWorkOrder(newWorkOrder);
        }

        public List<WorkOrder> GetWorkOrders()
        {
            return _workOrderRepository.GetWorkOrders();
        }


        public HttpResponseMessage GetWorkOrdersPaging(Request objRequest, int? userId)
        {
            return _workOrderRepository.GetWorkOrdersPaging(objRequest, userId);
        }

        public List<WorkOrder> GetFloorWorkOrder(int floorId)
        {
            return _workOrderRepository.GetFloorWorkOrder(floorId);
        }

        public List<WorkOrder> GetFloorWorkOrder()
        {
            return _workOrderRepository.GetFloorWorkOrder();
        }

        public WorkOrder GetWorkOrder(int issueId)
        {
            return _workOrderRepository.GetWorkOrder(issueId);

        }

        public List<TFloorAssets> GetIssuesFloorAssets(int issueId)
        {
            return _workOrderRepository.GetIssuesFloorAssets(issueId);
        }

        public List<WorkOrder> LinkWorkorder(string assetid)
        {
            return _workOrderRepository.LinkWorkorder(assetid);
        }

        #endregion

        public List<SubStatus> GetSubStatus()
        {
            return _workOrderRepository.GetSubStatus();
        }

        public bool SaveWorkOrderResource(int issueId, UserProfile user)
        {
            return _workOrderRepository.SaveWorkOrderResource(issueId, user);
        }

        //public  bool SaveWorkOrderAssets(int issueId, TFloorAssets floorAsset)
        //{
        //    return _workOrderRepository.SaveWorkOrderAssets(issueId, floorAsset);
        //}

        public int SaveWorkOrderFiles(int issueId, WorkOrderfiles file)
        {
            return _workOrderRepository.SaveWorkOrderFiles(issueId, file);
        }

        public int SaveWorkOrderFailStep(int issueId, Guid activityId, InspectionSteps inspectionSteps)
        {
            return _workOrderRepository.SaveWorkOrderFailStep(issueId, activityId, inspectionSteps);
        }




        public List<WorkOrder> GetRoundWorkOrders()
        {
            return _workOrderRepository.GetRoundWorkOrders();
        }

        public List<WorkOrder> GetRoundWorkOrders(int? buildingId, int? floorId)
        {
            return _workOrderRepository.GetRoundWorkOrders(buildingId, floorId);
        }

        public bool SaveWorkOrderResources(int issueId, string userIds, string floorAssetsIds, string tinsStepsId, string activityIds, string tmsReferenceIds, string resourceIds)
        {
            return _workOrderRepository.SaveWorkOrderResources(issueId, userIds, floorAssetsIds, tinsStepsId, activityIds, tmsReferenceIds, resourceIds);
        }

        public List<WorkOrder> QueueWorkOrders()
        {
            return _workOrderRepository.QueueWorkOrders();
        }

        public bool UpdateWorkOrderId(int issueId, int woId, int woNumber, string sourceWoId = null)
        {
            return _workOrderRepository.UpdateWorkOrderId(issueId, woId, woNumber, sourceWoId);
        }

        public int SaveTRounWorkOrder(TRoundWorkOrders objTRoundWorkOrders)
        {
            return _workOrderRepository.SaveTRounWorkOrder(objTRoundWorkOrders);
        }

        public WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber)
        {
            return _workOrderRepository.GetWorkOrderByWorkOrderNumber(workOrderNumber);
        }

        public List<FloorAssetsWorkOrderCount> GetFloorAssetsWorkOrderCount()
        {
            return _workOrderRepository.GetFloorAssetsWorkOrderCount();
        }

        public int UpdateTempWorkOrder(WorkOrder newWorkOrder)
        {
            return _workOrderRepository.UpdateTempWorkOrder(newWorkOrder);
        }
    }
}
