using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCF.BDO;

namespace Hcf.Api.Tms.Interface
{
    public abstract class WorkOrderFactory
    {
        public abstract WorkOrder SaveWorkOrder(WorkOrder workOrder);

        public abstract WorkOrder UpdateWorkOrder(WorkOrder workOrder);

        public abstract List<WorkOrder> GetWorkOrders();

        public abstract List<WorkOrder> GetWorkorderByWoIds(List<string> workorderIds);

        public abstract WorkOrder GetWorkOrder(string workorderId);

        public abstract bool IsServiceUp();

        public abstract List<WorkOrder> GetWorkOrderByQuery(string fieldName, string condition, string value);

        public abstract List<WorkOrder> GetWorkOrderByQuery(string strquery);

        public abstract WorkOrder SaveTmsWorkOrder(WorkOrder workOrder);

        public abstract WorkOrder UpdateTmsWorkOrder(WorkOrder workOrder);

    }
}