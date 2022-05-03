using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.BDO.NBIH;
using NbiWorkOrderWS;

namespace TMS.NBIH
{
    public class WorkOrders
    {
        public static Task<NbiWorkOrderWS.WorkOrder[]> GetWorkOrder(string workOrderNumber)
        {
            var authHeader = SetNbiHeader();
            var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);
            var lists = client.LoadAsync(authHeader, "");
            return lists;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }

        #region OneTime Methods

        public static List<TMSNBIWorkOrderResults> GetTmsWorkOrder(int day)
        {
            DateTime createdDate = DateTime.Now;
            WorkOrderCriteria[] woCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                woCriteria = WorkOrderCriteria(createdDate);
            }
            else
            {
                woCriteria = new WorkOrderCriteria[2];
                var c1 = new WorkOrderCriteria
                {
                    Field = FIELDS.DATE_CREATED,
                    FieldValue = createdDate.AddDays(-400),
                    Operator = OPERATORS.GREATER_OR_EQUAL
                };
                var c2 = new WorkOrderCriteria
                {
                    Field = FIELDS.TYPE_CODE,
                    FieldValue = "PM",
                    Operator = OPERATORS.EQUAL
                };

                woCriteria[0] = c1;
                woCriteria[1] = c2;
            }
            AuthenticationHeader authHeader = SetNbiHeader();
            var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);

            var woList = client.QueryAsync(authHeader, woCriteria).Result;
            List<HCF.BDO.NBIH.TMSNBIWorkOrderResults> TMS_Results = ConverttoResult(woList);
            return TMS_Results;
        }

        private static List<TMSNBIWorkOrderResults> ConverttoResult(NbiWorkOrderWS.WorkOrder[] woList)
        {
            List<TMSNBIWorkOrderResults> lists = new List<TMSNBIWorkOrderResults>();
            foreach (var item in woList)
            {
                HCF.BDO.NBIH.TMSNBIWorkOrderResults obj = new TMSNBIWorkOrderResults();
                obj.AccountCode = item.AccountCode;
                obj.AccountDescription = item.AccountDescription;
                obj.ActionCode = item.ActionCode;
                obj.ActionDescription = item.ActionDescription;
                obj.AssetGroupNumber = item.AssetGroupNumber;
                obj.AssetNumber = item.AssetNumber;
                obj.BuildingCode = item.BuildingCode;
                obj.BuildingName = item.BuildingName;
                obj.CauseCode = item.CauseCode;
                obj.CauseDescription = item.CauseDescription;
                obj.CompletionComments = item.CompletionComments;
                obj.DateAvailable = Convert.ToDateTime(item.DateAvailable.ToString());
                obj.DateCompleted = Convert.ToDateTime(item.DateCompleted.ToString());
                obj.DateCreated = Convert.ToDateTime(item.DateCreated.ToString());
                obj.DateNeeded = Convert.ToDateTime(item.DateNeeded.ToString());
                obj.DateUpdated = Convert.ToDateTime(item.DateUpdated.ToString());
                obj.ItemCode = item.ItemCode;
                obj.ItemDescription = item.ItemDescription;
                obj.LocationCode = item.LocationCode;
                obj.LocationDescription = item.LocationDescription;
                obj.MeterReading = Convert.ToDouble(item.MeterReading.ToString());
                obj.PriorityCode = item.PriorityCode;
                obj.PriorityDescription = item.PriorityDescription;
                obj.ProblemCode = item.ProblemCode;
                obj.ProblemDescription = item.ProblemDescription;
                obj.ReferenceNumber = item.ReferenceNumber;
                obj.RequesterComments = item.RequesterComments;
                obj.RequesterEmail = item.RequesterEmail;
                obj.RequesterName = item.RequesterName;
                obj.RequesterPager = item.RequesterPager;
                obj.RequesterPhone = item.RequesterPhone;
                obj.SegmentID = item.SegmentID;
                obj.ShopCode = item.ShopCode;
                obj.ShopDescription = item.ShopDescription;
                obj.SiteCode = item.SiteCode;
                obj.SiteName = item.SiteName;
                obj.SkillCode = item.SkillCode;
                obj.SkillDescription = item.SkillDescription;
                obj.StatusCode = item.StatusCode;
                obj.StatusDescription = item.StatusDescription;
                obj.SubStatusCode = item.SubStatusCode;
                obj.SubStatusDescription = item.SubStatusDescription;
                obj.TotalCost = item.TotalCost;
                obj.TotalHours = item.TotalHours;
                obj.TotalMaterialCharges = item.TotalMaterialCharges;
                obj.TotalTimeCharges = item.TotalTimeCharges;
                obj.TypeCode = item.TypeCode;
                obj.TypeDescription = item.TypeDescription;
                obj.UDF1 = item.UDF1;
                obj.UDF2 = item.UDF2;
                obj.UDF3 = item.UDF3;
                obj.UDF4 = item.UDF4;
                obj.UDF5 = item.UDF5;
                obj.UDF6 = item.UDF6;
                obj.UDF7 = item.UDF7;
                obj.UDF8 = item.UDF8;
                obj.UDF9 = item.UDF9;
                obj.UDF10 = item.UDF10;
                obj.WeekScheduled = Convert.ToDateTime(item.WeekScheduled.ToString());
                obj.WorkOrderID = item.WorkOrderID;
                obj.WorkOrderNumber = item.WorkOrderNumber;
                lists.Add(obj);
            }
            return lists;
        }

        #endregion

        #region WorkOrder Related Methods

        public static List<HCF.BDO.WorkOrder> GetWorkOrdersByCauseCode(int days)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            try
            {
                WorkOrderCriteria[] criteria = new WorkOrderCriteria[2];
                WorkOrderCriteria c1 = new WorkOrderCriteria
                {
                    Field = FIELDS.TYPE_CODE,
                    FieldValue = "PM",
                    Operator = OPERATORS.EQUAL,
                    LogicalOperator = LOGICAL_OPERATORS.OR
                };

                //WorkOrderCriteria c3 = new WorkOrderCriteria();
                //c3.fieldField = FIELDS.TYPE_CODE;
                //c3.fieldValueField = "PE";
                //c3.operatorField = OPERATORS.EQUAL;
                //c3.logicalOperatorField = LOGICAL_OPERATORS.OR;

                //WorkOrderCriteria c6 = new WorkOrderCriteria();
                //c6.Field = FIELDS.DATE_CREATED;
                //c6.FieldValue = DateTime.UtcNow.AddDays(-days);
                //c6.Operator = OPERATORS.GREATER_OR_EQUAL;
                //c6.LogicalOperator = LOGICAL_OPERATORS.AND;

                //WorkOrderCriteria c7 = new WorkOrderCriteria();
                //c6.Field = FIELDS.DATE_UPDATED;
                //c6.FieldValue = DateTime.UtcNow.AddDays(-days);
                //c6.Operator = OPERATORS.GREATER_OR_EQUAL;
                //c6.LogicalOperator = LOGICAL_OPERATORS.AND;


                WorkOrderCriteria c8 = new WorkOrderCriteria();
                c8.Field = FIELDS.STATUS_CODE;
                c8.FieldValue = "ACTIV";
                c8.Operator = OPERATORS.EQUAL;
                c8.LogicalOperator = LOGICAL_OPERATORS.AND;


                //WorkOrderCriteria c8 = new WorkOrderCriteria();
                //c8.fieldField = FIELDS1.TYPE_CODE;
                //c8.fieldValueField = "PE";
                //c8.operatorField = OPERATORS1.EQUAL;
                //c8.logicalOperatorField = LOGICAL_OPERATORS1.AND;



                criteria[0] = c1;
                criteria[1] = c8;
                //criteria[2] = c6;
                //criteria[2] = c8;

                //criteria[2] = c7;
                AuthenticationHeader authHeader = SetNbiHeader();
                var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);
                NbiWorkOrderWS.WorkOrder[] workOrders = client.QueryAsync(authHeader, criteria).Result;
                //workOrders = workOrders.Where(x => x.DateCreated > DateTime.UtcNow.AddDays(-days) || x.DateUpdated > DateTime.UtcNow.AddDays(-days)).OrderBy(x => x.DateCreated).ThenBy(x => x.DateUpdated).ToArray();

                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {                
            }
            return bdoWorkOrders;
        }

        private static WorkOrderCriteria[] WorkOrderCriteria(DateTime createdDate)
        {
            WorkOrderCriteria[] woCriteria;
            woCriteria = new WorkOrderCriteria[2];
            var c1 = new WorkOrderCriteria
            {
                Field = FIELDS.DATE_CREATED,
                FieldValue = createdDate,
                Operator = OPERATORS.GREATER_OR_EQUAL
            };
            var c2 = new WorkOrderCriteria
            {
                Field = FIELDS.TYPE_CODE,
                FieldValue = "PM",
                Operator = OPERATORS.EQUAL
            };
            woCriteria[0] = c1;
            woCriteria[1] = c2;
            return woCriteria;
        }

        public static HCF.BDO.WorkOrder SaveTmsWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);
            var authHeader = SetNbiHeader();
            NbiWorkOrderWS.WorkOrder wo = ConvertWorkOrder(workorder);
            try
            {
                wo = client.SaveAsync(authHeader, wo).Result;
                if (wo.WorkOrderID > 0)
                {
                    workorder.WorkOrderId = wo.WorkOrderID;
                    workorder.WorkOrderNumber = wo.WorkOrderNumber.ToString();

                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            NbiWorkOrderWS.Resource resource = new NbiWorkOrderWS.Resource
                            {
                                ResourceNumber = user.ResourceNumber,
                                ResourceID = user.ResourceId.Value,
                                FirstName = user.FirstName,
                                LastName = user.LastName
                            };
                            client.SaveAssignmentAsync(authHeader, wo.WorkOrderID, resource);
                        }
                    }
                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        NbiWorkOrderWS.Asset asset = ConvertAssets(floorAssets);
                        client.SaveAssetAsync(authHeader, wo.WorkOrderID, asset);
                    }
                }
            }
            catch (Exception ex)
            {
                workorder.WorkOrderId = -2;
                workorder.WorkOrderNumber = "-2";
            }
            return workorder;
        }

        public static HCF.BDO.WorkOrder UpdateTmsWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);
            var authHeader = SetNbiHeader();
            NbiWorkOrderWS.WorkOrder wo = ConvertWorkOrder(workorder);
            try
            {
                wo = client.SaveAsync(authHeader, wo).Result;
                if (wo.WorkOrderID > 0)
                {
                    workorder.WorkOrderId = wo.WorkOrderID;
                    workorder.WorkOrderNumber = wo.WorkOrderNumber.ToString();

                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            NbiWorkOrderWS.Resource resource = new NbiWorkOrderWS.Resource
                            {
                                ResourceNumber = user.ResourceNumber,
                                ResourceID = user.ResourceId.Value,
                                FirstName = user.FirstName,
                                LastName = user.LastName
                            };
                            client.SaveAssignmentAsync(authHeader, wo.WorkOrderID, resource);
                        }
                    }

                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        if (!string.IsNullOrEmpty(floorAssets.TmsReference))
                        {
                            try
                            {
                                NbiWorkOrderWS.Asset asset = ConvertAssets(floorAssets);
                                client.SaveAssetAsync(authHeader, wo.WorkOrderID, asset);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return workorder;
        }

        public static List<HCF.BDO.WorkOrder> UpdateWorkOrders(List<string> workOrderNumber)
        {
            var client = new WorkOrderWSClient(WorkOrderWSClient.EndpointConfiguration.WorkOrderWS);
            var authHeader = SetNbiHeader();
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            NbiWorkOrderWS.WorkOrderCriteria[] criteria = new NbiWorkOrderWS.WorkOrderCriteria[workOrderNumber.Count];
            for (int i = 0; i < workOrderNumber.Count; i++)
            {
                WorkOrderCriteria c1 = new WorkOrderCriteria
                {
                    Field = FIELDS.WORKORDER_NUMBER,
                    FieldValue = workOrderNumber[i],
                    Operator = OPERATORS.EQUAL,
                    LogicalOperator = LOGICAL_OPERATORS.OR
                };
                criteria[i] = c1;
            }
            try
            {
                NbiWorkOrderWS.WorkOrder[] workOrders = client.QueryAsync(authHeader, criteria).Result;
                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {

            }
            return bdoWorkOrders;
        }

        #endregion

        #region Common Methods

        private static NbiWorkOrderWS.WorkOrder ConvertWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            NbiWorkOrderWS.WorkOrder wo = new NbiWorkOrderWS.WorkOrder
            {
                AccountCode = workorder.AccountCode,
                Description = workorder.Description,
                PriorityCode = workorder.PriorityCode,
                SegmentID = workorder.SegmentID,
                SkillCode = workorder.SkillCode,
                StatusCode = workorder.StatusCode,
                SubStatusCode = workorder.SubStatusCode,
                TypeCode = workorder.TypeCode
            };
            if (workorder.WorkOrderId.HasValue && workorder.WorkOrderId != -2)
            {
                wo.WorkOrderID = workorder.WorkOrderId.Value;
            }
            else { wo.WorkOrderID = 0; }
            if (!string.IsNullOrEmpty(workorder.WorkOrderNumber) && workorder.WorkOrderNumber != "-2")
            {
                wo.WorkOrderNumber = Convert.ToInt32(workorder.WorkOrderNumber);
            }
            else
            {
                wo.WorkOrderNumber = 0;
            }
            wo.RequesterEmail = workorder.RequesterEmail;
            wo.RequesterPhone = workorder.RequesterPhone;
            wo.RequesterComments = workorder.RequesterComments;
            wo.RequesterName = workorder.RequesterName;
            wo.ProblemCode = workorder.ProblemCode;
            wo.CauseCode = workorder.CauseCode;
            wo.CompletionComments = workorder.CompletionComments;
            return wo;
        }

        private static NbiWorkOrderWS.Asset ConvertAssets(TFloorAssets tfloorAssets)
        {
            NbiWorkOrderWS.Asset asset = new NbiWorkOrderWS.Asset
            {
                AssetID = Convert.ToInt32(tfloorAssets.TmsReference),
                AssetNumber = tfloorAssets.AssetNo
            };
            return asset;
        }

        private static HCF.BDO.WorkOrder ConvertWorkOrder(NbiWorkOrderWS.WorkOrder workOrders)
        {
            HCF.BDO.WorkOrder bdoWorkOrder = new HCF.BDO.WorkOrder
            {
                AccountCode = workOrders.AccountCode,
                AccountDescription = workOrders.AccountDescription
            };
            bdoWorkOrder.AccountDescription = workOrders.AccountDescription;
            bdoWorkOrder.DateCreated = workOrders.DateCreated;
            bdoWorkOrder.WorkOrderId = workOrders.WorkOrderID;
            bdoWorkOrder.WorkOrderNumber = workOrders.WorkOrderNumber.ToString();
            bdoWorkOrder.RequesterEmail = workOrders.RequesterEmail;
            bdoWorkOrder.RequesterPhone = workOrders.RequesterPhone;
            bdoWorkOrder.Description = workOrders.Description;
            bdoWorkOrder.SkillCode = workOrders.SkillCode;
            bdoWorkOrder.StatusCode = workOrders.StatusCode;
            bdoWorkOrder.StatusDescription = workOrders.StatusDescription;
            bdoWorkOrder.SubStatusCode = workOrders.SubStatusCode;
            bdoWorkOrder.SubStatusDescription = workOrders.SubStatusDescription;
            bdoWorkOrder.PriorityCode = workOrders.PriorityCode;
            bdoWorkOrder.PriorityDescription = workOrders.PriorityDescription;
            bdoWorkOrder.TypeCode = workOrders.TypeCode;
            bdoWorkOrder.TypeDescription = workOrders.TypeDescription;
            bdoWorkOrder.RequesterComments = workOrders.RequesterComments;
            bdoWorkOrder.CompletionComments = workOrders.CompletionComments;
            bdoWorkOrder.ItemCode = workOrders.ItemCode;
            bdoWorkOrder.ItemDescription = workOrders.ItemDescription;
            bdoWorkOrder.RequesterName = workOrders.RequesterName;
            bdoWorkOrder.AssetNumber = workOrders.AssetNumber;
            bdoWorkOrder.DateUpdated = workOrders.DateUpdated;
            // bdoWorkOrder.CreateDate = workOrders.dateCreatedField.Value.ToShortDateString();
            return bdoWorkOrder;
        }

        #endregion
    }
}
