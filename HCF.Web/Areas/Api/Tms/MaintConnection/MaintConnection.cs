using Hcf.Api.Tms.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Script.Serialization;
using HCF.BDO;
using HCF.BDO.MaintenanceConnection;
using Newtonsoft.Json;
using HCF.Utility;

namespace Hcf.Api.Tms.MaintConnection
{
    public class MaintConnection : WorkOrderFactory
    {
        private static string _postWorkOrder = "https://api.maintenanceconnection.com/v8/workorders";
        private static readonly string _getWorkOrder = "https://api.maintenanceconnection.com/v8/workorders";

        private const string _mcCnKey = "AKRONCH";
        private const string _mcApiKey = "fc5ff399-cb8d-4bdd-b4a2-19361678b072";


        public override WorkOrder SaveWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        public override WorkOrder UpdateWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkOrders()
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkorderByWoIds(List<string> workOrderPks)
        {
            var bdoWorkOrders = new List<WorkOrder>();
            if (workOrderPks.Count > 0)
            {
                var sb=new StringBuilder();
                foreach (var s in workOrderPks)
                    sb.Append(" PK eq " + s + " or");

                var qString= sb.ToString().Remove(sb.Length - 3);

                var responseJson = CommonUtility.MaintConnectionGet(_getWorkOrder + "?$filter=" + qString, -1,_mcCnKey, _mcApiKey);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderRes>>(responseJson);
                bdoWorkOrders = ConvertToLocalWorkOrder(response.Results);
            }

            return bdoWorkOrders;
        }

        public override WorkOrder GetWorkOrder(string workorderId)
        {
            var woList = new List<WorkOrder>();
            var tmsTmsWorkOrderRes = new List<TMS_WorkOrderRes>();
            var dataString = string.Empty;
            try
            {
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionGet(_getWorkOrder + "/" + workorderId, 0, _mcCnKey, _mcApiKey);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderRes>>(responseJson);
                woList = ConvertToLocalWorkOrder(response.Results);
            }
            catch (Exception)
            {
            }
            return woList.FirstOrDefault();
        }

        public override bool IsServiceUp()
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkOrderByQuery(string fieldName, string condition, string value)
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkOrderByQuery(string strquery)
        {
            var woList = new List<WorkOrder>();
            var tmsTmsWorkOrderRes = new List<TMS_WorkOrderRes>();
            try
            {
                //var responseJson = HCF.Utility.CommonUtility.MaintConnectionGet(_getWorkOrder, 0, _mcCnKey, _mcApiKey, strquery);
                ////var jsonSerializer = new JavaScriptSerializer();
                //var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderRes>>(responseJson);
                //woList = ConvertToLocalWorkOrder(response.Results);
                var startIndex = 0;
                int totalRecords = 0;
                bool isMoreData = true;
                //int remaininingItems = 500;
                while (isMoreData)
                {
                    var responseJson = CommonUtility.MaintConnectionGet(_getWorkOrder, startIndex, _mcCnKey, _mcApiKey, strquery);
                    //var jsonSerializer = new JavaScriptSerializer();
                    var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderRes>>(responseJson);
                    totalRecords = totalRecords + 500;
                    startIndex = startIndex + 500;
                    //lastIndex = startIndex + 500;
                    if (totalRecords >= response.Total)
                        isMoreData = false;

                    woList.AddRange(ConvertToLocalWorkOrder(response.Results));
                }

            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
            }
            return woList.Where(x => x.TypeCode == HCF.BDO.Enums.WODeficiencyCode.PM.ToString()).ToList();
        }

        public override WorkOrder SaveTmsWorkOrder(WorkOrder workOrder)
        {
            var wo = new WorkOrder();
            var tmsWorkOrderRes = new List<TMS_WorkOrderRes>();
            var postDataString = string.Empty;
            try
            {
                tmsWorkOrderRes.Add(ConvertToTmsWorkOrder(workOrder));
                postDataString = JsonConvert.SerializeObject(tmsWorkOrderRes);
                var responseJson = HCF.Utility.CommonUtility.MaintConnectionPost(_postWorkOrder, _mcCnKey, _mcApiKey, postDataString);
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<RootObject<TMS_WorkOrderRes>>(responseJson);
                wo = ConvertToLocalWorkOrder(response.Results).FirstOrDefault();
                if (wo.WorkOrderId > 0)
                {
                    foreach (UserProfile user in workOrder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            ResourceFactory UserObject = new Resources();
                            UserObject.SaveWoAssignment(user, wo.WorkOrderId.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
                //HCF.Utility.ErrorLog.LogMsg(postDataString);
                wo.WorkOrderId = -2;
                wo.WorkOrderNumber = "-2";
                //wo.Add(workOrder);
            }
            return wo;
        }

        public override WorkOrder UpdateTmsWorkOrder(WorkOrder workOrder)
        {
            var woList = new List<WorkOrder>();
            var tmsTmsWorkOrderRes = new TMS_WorkOrderRes();
            var postDataString = string.Empty;
            try
            {
                ConvertToTmsWorkOrder(workOrder);
                postDataString = JsonConvert.SerializeObject(tmsTmsWorkOrderRes);
                _postWorkOrder = $"{_postWorkOrder}/{workOrder.SourceWoId}";
                var responseJson = CommonUtility.MaintConnectionPost(_postWorkOrder, _mcCnKey, _mcApiKey, postDataString, HCF.BDO.Enums.MethodType.PUT.ToString());
                //var jsonSerializer = new JavaScriptSerializer();
                var response = JsonConvert.DeserializeObject<List<TMS_WorkOrderRes>>(responseJson);
                woList = ConvertToLocalWorkOrder(response);
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
                //HCF.Utility.ErrorLog.LogMsg(postDataString);
                woList.Add(workOrder);
            }
            return woList.FirstOrDefault();
        }

        #region Private Methods

        private static TMS_WorkOrderRes ConvertToTmsWorkOrder(WorkOrder workOrder)
        {
            var tmsWorkOrderObj = new TMS_WorkOrderRes
            {
                PK = workOrder.WorkOrderId.HasValue && workOrder.WorkOrderId.Value > 0 ? workOrder.WorkOrderId.Value : 0,
                ID = workOrder.WorkOrderNumber,
                Reason = workOrder.Description,
                RequesterPhone = workOrder.RequesterPhone,
                RequesterEmail = workOrder.RequesterEmail,
                RequesterName = workOrder.RequesterName,
                PriorityDetails = new PriorityDetails
                {
                    Value = workOrder.PriorityCode
                },
                TypeDetails = new TypeDetails
                {
                    Value = workOrder.TypeCode
                },
                StatusDetails = new StatusDetails
                {
                    Value = workOrder.StatusCode
                },
                RepairCenterRef = new RepairCenterRef
                {
                    ID = workOrder.SiteCode
                },
                AssetRef = new AssetRef
                {
                    ID = workOrder.BuildingCode
                },
                CompleteDate = workOrder.DateCompleted,
                RequestedDate = workOrder.DateCreated,
                TargetDate = workOrder.TargetDate               
            };

            return tmsWorkOrderObj;
        }

        private static List<WorkOrder> ConvertToLocalWorkOrder(IEnumerable<TMS_WorkOrderRes> tmsWorkOrders)
        {
            var wos = new List<WorkOrder>();
            foreach (var workOrder in tmsWorkOrders)
            {
                try
                {
                    var wo = new WorkOrder
                    {
                        Description = workOrder.Reason,
                        WorkOrderNumber = workOrder.ID,
                        WorkOrderId = workOrder.PK,
                        SourceWoId = Convert.ToString(workOrder.PK),
                        RequesterPhone = workOrder.RequesterPhone,
                        RequesterEmail = workOrder.RequesterEmail,
                        RequesterName = workOrder.RequesterName,
                        StatusCode = (workOrder.StatusDetails != null) ? workOrder.StatusDetails.Value : string.Empty,
                        PriorityCode = (workOrder.PriorityDetails != null) ? workOrder.PriorityDetails.Value : string.Empty,
                        TypeCode = workOrder.TypeDetails.Value,
                        TargetDate = workOrder.TargetDate,
                        DateCreated = workOrder.RequestedDate,
                        SiteCode = (workOrder.RepairCenterRef != null) ? workOrder.RepairCenterRef.ID : string.Empty,
                        BuildingCode = (workOrder.AssetRef != null) ? workOrder.AssetRef.ID : string.Empty,
                       
                    };
                    if (workOrder.AssetRef != null)
                    {
                        wo.WorkOrderFloorAssets = new List<WorkOrderFloorAssets>();
                        WorkOrderFloorAssets workOrderFloorAssets = new WorkOrderFloorAssets();
                        workOrderFloorAssets.TmsAssetId = workOrder.AssetRef?.PK;
                        wo.WorkOrderFloorAssets.Add(workOrderFloorAssets);
                    }
                    wos.Add(wo);
                }
                catch (Exception e)
                {
                    //ErrorLog.LogError(e);
                }

            }
            return wos;
        }

        #endregion
    }
}