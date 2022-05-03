//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Hosting;

//namespace HCF.BAL
//{
//    public static class WorkOrderRepository
//    {
//        #region Work Order

//        public static int AddWorkOrder(HCF.BDO.WorkOrder newWorkOrder)
//        {
//            return DAL.WorkOrderRepository.Save(newWorkOrder);
//        }

//        public static int UpdateWorkOrder(HCF.BDO.WorkOrder newWorkOrder)
//        {
//            return DAL.WorkOrderRepository.UpdatetWorkOrder(newWorkOrder);
//        }

//        public static List<WorkOrder> GetWorkOrders()
//        {
//            return DAL.WorkOrderRepository.GetWorkOrders();
//        }


//        public static HttpResponseMessage GetWorkOrdersPaging(Request objRequest, int? userId)
//        {
//            return DAL.WorkOrderRepository.GetWorkOrdersPaging(objRequest, userId);
//        }

//        public static List<WorkOrder> GetFloorWorkOrder(int floorId)
//        {
//            return DAL.WorkOrderRepository.GetFloorWorkOrder(floorId);
//        }

//        public static List<WorkOrder> GetFloorWorkOrder()
//        {
//            return DAL.WorkOrderRepository.GetFloorWorkOrder();
//        }

//        public static WorkOrder GetWorkOrder(int issueId)
//        {
//            return DAL.WorkOrderRepository.GetWorkOrder(issueId);

//        }

//        public static List<TFloorAssets> GetIssuesFloorAssets(int issueId)
//        {
//            return DAL.WorkOrderRepository.GetIssuesFloorAssets(issueId);
//        }

//        public static List<WorkOrder> LinkWorkorder(string assetid)
//        {
//            return DAL.WorkOrderRepository.LinkWorkorder(assetid);
//        }

//        #endregion

//        public static List<SubStatus> GetSubStatus()
//        {
//            return DAL.WorkOrderRepository.GetSubStatus();
//        }

//        public static bool SaveWorkOrderResource(int issueId, UserProfile user)
//        {
//            return DAL.WorkOrderRepository.SaveWorkOrderResource(issueId, user);
//        }

//        //public static bool SaveWorkOrderAssets(int issueId, TFloorAssets floorAsset)
//        //{
//        //    return DAL.WorkOrderRepository.SaveWorkOrderAssets(issueId, floorAsset);
//        //}

//        public static int SaveWorkOrderFiles(int issueId, WorkOrderfiles file)
//        {
//            return DAL.WorkOrderRepository.SaveWorkOrderFiles(issueId, file);
//        }

//        public static int SaveWorkOrderFailStep(int issueId, Guid activityId, InspectionSteps inspectionSteps)
//        {
//            return DAL.WorkOrderRepository.SaveWorkOrderFailStep(issueId, activityId, inspectionSteps);
//        }




//        public static List<WorkOrder> GetRoundWorkOrders()
//        {
//            return DAL.WorkOrderRepository.GetRoundWorkOrders();
//        }

//        public static List<WorkOrder> GetRoundWorkOrders(int? buildingId , int? floorId)
//        {
//            return DAL.WorkOrderRepository.GetRoundWorkOrders(buildingId, floorId);
//        }

//        public static bool SaveWorkOrderResources(int issueId, string userIds, string floorAssetsIds, string tinsStepsId, string activityIds, string tmsReferenceIds, string resourceIds)
//        {
//            return DAL.WorkOrderRepository.SaveWorkOrderResources(issueId, userIds, floorAssetsIds, tinsStepsId, activityIds, tmsReferenceIds, resourceIds);
//        }

//        public static List<WorkOrder> QueueWorkOrders()
//        {
//            return DAL.WorkOrderRepository.QueueWorkOrders();
//        }

//        public static bool UpdateWorkOrderId(int issueId, int woId, int woNumber,string sourceWoId=null)
//        {
//            return DAL.WorkOrderRepository.UpdateWorkOrderId(issueId, woId, woNumber, sourceWoId);
//        }

//        public static int SaveTRounWorkOrder(TRoundWorkOrders objTRoundWorkOrders)
//        {
//            return DAL.WorkOrderRepository.SaveTRounWorkOrder(objTRoundWorkOrders);
//        }

//        public static WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber)
//        {
//            return DAL.WorkOrderRepository.GetWorkOrderByWorkOrderNumber(workOrderNumber);
//        }

//        public static List<FloorAssetsWorkOrderCount> GetFloorAssetsWorkOrderCount()
//        {
//            return DAL.WorkOrderRepository.GetFloorAssetsWorkOrderCount();
//        }
//    }
//}
