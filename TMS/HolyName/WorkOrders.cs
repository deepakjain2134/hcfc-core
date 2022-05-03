using System;
using System.Collections.Generic;
using System.Linq;
using TMS.HolyNameTmsService;
using HCF.BDO;

namespace TMS
{
    public class TMSHolyName
    {
        HolyNameTmsClient client = new HolyNameTmsClient();
        BurkeWorkOrderClient burkWOClient = new BurkeWorkOrderClient();

        public HCF.BDO.WorkOrder GetWorkOrder(string workOrderNumber)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria[] criteria = new WorkOrderCriteria[1];
            WorkOrderCriteria c1 = new WorkOrderCriteria
            {
                fieldField = FIELDS6.WORKORDER_NUMBER,
                fieldValueField = workOrderNumber,
                operatorField = OPERATORS6.EQUAL,
                logicalOperatorField = LOGICAL_OPERATORS6.AND
            };
            criteria[0] = c1;
            HolyNameTmsService.WorkOrder2[] workOrders = client.Query(criteria);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders.FirstOrDefault();
        }

        #region HolyName

        #region HolyName WO Methods

        /// <summary>
        /// This method will save the supplied Work Order to the database, only after passing TMS business validation rules. 
        /// </summary>
        /// <param name="workorder"></param>
        /// <param name="_tfloorAssets"></param>
        public HCF.BDO.WorkOrder SaveTMSWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            HolyNameTmsClient obj = new HolyNameTmsClient();
            WorkOrder2 wo = ConvertWorkOrder(workorder);
            try
            {
                wo = obj.Save(wo);
                if (wo.workOrderIDField > 0)
                {
                    workorder.WorkOrderId = wo.workOrderIDField;
                    workorder.WorkOrderNumber = wo.workOrderNumberField.ToString();

                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            Resource1 resource = new Resource1
                            {
                                resourceNumberField = user.ResourceNumber,
                                resourceIDField = user.ResourceId.Value,
                                firstNameField = user.FirstName,
                                lastNameField = user.LastName
                            };
                            obj.SaveAssignment(wo.workOrderIDField, resource);
                        }
                    }
                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        Asset2 asset = ConvertAssets(floorAssets);
                        obj.SaveAsset(wo.workOrderIDField, asset);
                    }
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);
                workorder.WorkOrderId = -2;
                workorder.WorkOrderNumber = "-2";
            }
            return workorder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workorder"></param>
        /// <returns></returns>
        public HCF.BDO.WorkOrder UpdateWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            HolyNameTmsClient obj = new HolyNameTmsClient();
            WorkOrder2 wo = ConvertWorkOrder(workorder);
            try
            {
                wo = obj.Save(wo);
                if (wo.workOrderIDField > 0)
                {
                    workorder.WorkOrderId = wo.workOrderIDField;
                    workorder.WorkOrderNumber = wo.workOrderNumberField.ToString();

                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            Resource1 resource = new Resource1
                            {
                                resourceNumberField = user.ResourceNumber,
                                resourceIDField = user.ResourceId.Value,
                                firstNameField = user.FirstName,
                                lastNameField = user.LastName
                            };
                            obj.SaveAssignment(wo.workOrderIDField, resource);
                        }
                    }

                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        if (!string.IsNullOrEmpty(floorAssets.TmsReference))
                        {
                            try
                            {
                                Asset2 asset = ConvertAssets(floorAssets);
                                obj.SaveAsset(wo.workOrderIDField, asset);
                            }
                            catch (Exception ex)
                            {
                                HCF.Utility.ErrorLog.LogError(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);

            }
            return workorder;
        }

        /// <summary>
        /// This method will return an array of work orders for the supplied Work Order Number.
        /// </summary>
        /// <param name="workOrderNumber"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetWorkOrder(int workOrderNumber)
        {
            WorkOrder2[] workOrders = client.GetWorkOrder(Convert.ToString(workOrderNumber));
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// This method will return List of work orders for the supplied Work Order Status. 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetWorkOrders(string statusCode)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria[] criteria = new WorkOrderCriteria[1];
            WorkOrderCriteria c1 = new WorkOrderCriteria
            {
                fieldField = FIELDS6.STATUS_CODE,
                fieldValueField = statusCode,
                operatorField = OPERATORS6.EQUAL,
                logicalOperatorField = LOGICAL_OPERATORS6.AND
            };
            criteria[0] = c1;
            WorkOrder2[] workOrders = client.Query(criteria);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrdrId"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetUpdatedWorkOrder(string workOrdrId)
        {
            int totalRec = 20;
            string[] workOrdersArray = workOrdrId.Split(',');
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            int result = workOrdersArray.Length % totalRec;
            int Index = result > 0 ? (workOrdersArray.Length / totalRec) + 1 : workOrdersArray.Length / totalRec;
            int lastIndex = workOrdersArray.Length;
            for (int j = 0; j < Index; j++)
            {
                int fIndex = j * totalRec;
                int lIndex = j * totalRec + totalRec;
                int loop_index = lastIndex > lIndex ? lIndex : lastIndex;
                int resultArray = loop_index % totalRec;
                int dArray = resultArray > 0 ? resultArray : totalRec;
                WorkOrderCriteria[] criteria = new WorkOrderCriteria[dArray];
                int Counter = 0;
                for (int i = fIndex; i < loop_index; i++)
                {
                    WorkOrderCriteria c1 = new WorkOrderCriteria
                    {
                        fieldField = FIELDS6.WORKORDER_NUMBER,
                        fieldValueField = workOrdersArray[i],
                        operatorField = OPERATORS6.EQUAL,
                        logicalOperatorField = LOGICAL_OPERATORS6.OR
                    };
                    criteria[Counter] = c1;
                    Counter++;
                }
                WorkOrder2[] workOrders = client.Query(criteria);

                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// This method will return an array of Asset objects for a given Work Order ID. In order to determine your Work Order ID, 
        /// you can load a work order prior to returning its assets.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public List<TFloorAssets> WorkOrderAssets(int workOrderId)
        {
            List<TFloorAssets> floorAssets = new List<TFloorAssets>();
            Asset2[] assets = client.WorkOrderAssets(workOrderId);
            for (int i = 0; i < assets.Length; i++)
            {
                TFloorAssets floorAsset = ConvertlocalAssets(assets[i]);
                floorAssets.Add(floorAsset);
            }
            return floorAssets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetAllWorkOrders()
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria[] criteria = new WorkOrderCriteria[1];
            WorkOrderCriteria c1 = new WorkOrderCriteria
            {
                fieldField = FIELDS6.WORKORDER_NUMBER,
                fieldValueField = 1,
                operatorField = OPERATORS6.GREATER,
                logicalOperatorField = LOGICAL_OPERATORS6.AND
            };
            criteria[0] = c1;
            WorkOrder2[] workOrders = client.Query(criteria);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// This method will return List of work orders for the supplied Work Order Status. 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetWorkOrdersByCauseCode(int days)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            try
            {
                WorkOrderCriteria[] criteria = new WorkOrderCriteria[2];
                WorkOrderCriteria c1 = new WorkOrderCriteria
                {
                    fieldField = FIELDS6.TYPE_CODE,
                    fieldValueField = "PM",
                    operatorField = OPERATORS6.EQUAL,
                    logicalOperatorField = LOGICAL_OPERATORS6.OR
                };

                //WorkOrderCriteria c3 = new WorkOrderCriteria();
                //c3.fieldField = FIELDS.TYPE_CODE;
                //c3.fieldValueField = "PE";
                //c3.operatorField = OPERATORS.EQUAL;
                //c3.logicalOperatorField = LOGICAL_OPERATORS.OR;

                WorkOrderCriteria c6 = new WorkOrderCriteria();
                c6.fieldField = FIELDS6.DATE_CREATED;
                c6.fieldValueField = DateTime.UtcNow.AddDays(-days);
                c6.operatorField = OPERATORS6.GREATER_OR_EQUAL;
                c6.logicalOperatorField = LOGICAL_OPERATORS6.AND;

                WorkOrderCriteria c7 = new WorkOrderCriteria();
                c6.fieldField = FIELDS6.DATE_UPDATED;
                c6.fieldValueField = DateTime.UtcNow.AddDays(-days);
                c6.operatorField = OPERATORS6.GREATER_OR_EQUAL;
                c6.logicalOperatorField = LOGICAL_OPERATORS6.AND;


                //WorkOrderCriteria c8 = new WorkOrderCriteria();
                //c8.fieldField = FIELDS1.TYPE_CODE;
                //c8.fieldValueField = "PE";
                //c8.operatorField = OPERATORS1.EQUAL;
                //c8.logicalOperatorField = LOGICAL_OPERATORS1.AND;



                criteria[0] = c1;
                criteria[1] = c6;
                //criteria[2] = c6;
                //criteria[2] = c8;
                //criteria[2] = c7;

                WorkOrder2[] workOrders = client.Query(criteria);
                workOrders = workOrders.Where(x => x.dateCreatedField > DateTime.UtcNow.AddDays(-days) || x.dateUpdatedField > DateTime.UtcNow.AddDays(-days)).OrderBy(x => x.dateCreatedField).ThenBy(x => x.dateUpdatedField).ToArray();

                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {
                //HCF.DAL.ErrorRepository.Instance.SaveError(ex, "TMSHolyName_GetWorkOrdersByCauseCode");
                HCF.Utility.ErrorLog.LogError(ex);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderNumber"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> UpdateWorkOrders(List<string> workOrderNumber)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria[] criteria = new WorkOrderCriteria[workOrderNumber.Count];
            for (int i = 0; i < workOrderNumber.Count; i++)
            {
                WorkOrderCriteria c1 = new WorkOrderCriteria
                {
                    fieldField = FIELDS6.WORKORDER_NUMBER,
                    fieldValueField = workOrderNumber[i],
                    operatorField = OPERATORS6.EQUAL,
                    logicalOperatorField = LOGICAL_OPERATORS6.OR
                };
                criteria[i] = c1;
            }
            try
            {
                WorkOrder2[] workOrders = client.Query(criteria);
                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);
            }
            return bdoWorkOrders;
        }


        #endregion

        #region HolyName Object Convert

        private WorkOrder2 ConvertWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            WorkOrder2 wo = new WorkOrder2
            {
                accountCodeField = workorder.AccountCode,
                descriptionField = workorder.Description,
                priorityCodeField = workorder.PriorityCode,
                segmentIDField = workorder.SegmentID,
                skillCodeField = workorder.SkillCode,
                statusCodeField = workorder.StatusCode,
                subStatusCodeField = workorder.SubStatusCode,
                typeCodeField = workorder.TypeCode
            };
            if (workorder.WorkOrderId.HasValue && workorder.WorkOrderId != -2)
            {
                wo.workOrderIDField = workorder.WorkOrderId.Value;
            }
            else { wo.workOrderIDField = 0; }
            if (!string.IsNullOrEmpty(workorder.WorkOrderNumber) && workorder.WorkOrderNumber != "-2")
            {
                wo.workOrderNumberField = Convert.ToInt32(workorder.WorkOrderNumber);
            }
            else
            {
                wo.workOrderNumberField = 0;
            }
            wo.requesterEmailField = workorder.RequesterEmail;
            wo.requesterPhoneField = workorder.RequesterPhone;
            wo.requesterCommentsField = workorder.RequesterComments;
            wo.requesterNameField = workorder.RequesterName;
            wo.problemCodeField = workorder.ProblemCode;
            wo.causeCodeField = workorder.CauseCode;
            wo.completionCommentsField = workorder.CompletionComments;

            return wo;
        }

        private static HCF.BDO.WorkOrder ConvertlocalWorkOrder(WorkOrder1 workOrders)
        {
            HCF.BDO.WorkOrder bdoWorkOrder = new HCF.BDO.WorkOrder();
            bdoWorkOrder.AccountCode = workOrders.accountCodeField;
            bdoWorkOrder.AccountDescription = workOrders.accountDescriptionField;
            bdoWorkOrder.AccountDescription = workOrders.accountDescriptionField;
            bdoWorkOrder.DateCreated = workOrders.dateCreatedField;
            bdoWorkOrder.WorkOrderId = workOrders.workOrderIDField;
            bdoWorkOrder.WorkOrderNumber = workOrders.workOrderNumberField.ToString();
            bdoWorkOrder.RequesterEmail = workOrders.requesterEmailField;
            bdoWorkOrder.RequesterPhone = workOrders.requesterPhoneField;
            bdoWorkOrder.Description = workOrders.descriptionField;
            bdoWorkOrder.SkillCode = workOrders.skillCodeField;
            bdoWorkOrder.StatusCode = workOrders.statusCodeField;
            bdoWorkOrder.StatusDescription = workOrders.statusDescriptionField;
            bdoWorkOrder.SubStatusCode = workOrders.subStatusCodeField;
            bdoWorkOrder.SubStatusDescription = workOrders.subStatusDescriptionField;
            bdoWorkOrder.PriorityCode = workOrders.priorityCodeField;
            bdoWorkOrder.PriorityDescription = workOrders.priorityDescriptionField;
            bdoWorkOrder.TypeCode = workOrders.typeCodeField;
            bdoWorkOrder.TypeDescription = workOrders.typeDescriptionField;
            bdoWorkOrder.RequesterComments = workOrders.requesterCommentsField;
            bdoWorkOrder.CompletionComments = workOrders.completionCommentsField;
            bdoWorkOrder.ItemCode = workOrders.itemCodeField;
            bdoWorkOrder.ItemDescription = workOrders.itemDescriptionField;
            bdoWorkOrder.RequesterName = workOrders.requesterNameField;
            bdoWorkOrder.AssetNumber = workOrders.assetNumberField;
            bdoWorkOrder.DateUpdated = workOrders.dateUpdatedField;
            bdoWorkOrder.ProblemCode = workOrders.problemCodeField;
            bdoWorkOrder.ProblemDescription = workOrders.problemDescriptionField;
            bdoWorkOrder.CauseCode = workOrders.causeCodeField;
            bdoWorkOrder.CauseDescription = workOrders.causeDescriptionField;
            bdoWorkOrder.SegmentID = workOrders.segmentIDField;
            bdoWorkOrder.CompletionComments = workOrders.completionCommentsField;
            bdoWorkOrder.DateCompleted = workOrders.dateCompletedField;
            bdoWorkOrder.DateNeeded = workOrders.dateNeededField;
            //bdoWorkOrder.WeekScheduled = workOrders.weekScheduledField;
            bdoWorkOrder.CreatedDate = workOrders.dateCreatedField;
            bdoWorkOrder.UpdateDate = workOrders.dateUpdatedField;
            bdoWorkOrder.DateAvailable = workOrders.dateAvailableField;
            return bdoWorkOrder;
        }

        private Asset2 ConvertAssets(TFloorAssets tfloorAssets)
        {
            Asset2 asset = new Asset2
            {
                assetIDField = Convert.ToInt32(tfloorAssets.TmsReference),
                assetNumberField = tfloorAssets.AssetNo
            };
            return asset;
        }

        private TFloorAssets ConvertlocalAssets(Asset2 asset)
        {
            TFloorAssets floorAssets = new TFloorAssets
            {
                TmsReference = asset.assetIDField.ToString(),
                DeviceNo = asset.assetNumberField
            };
            return floorAssets;
        }

        private static HCF.BDO.WorkOrder ConvertWorkOrder(WorkOrder2 workOrders)
        {
            HCF.BDO.WorkOrder bdoWorkOrder = new HCF.BDO.WorkOrder
            {
                AccountCode = workOrders.accountCodeField,
                AccountDescription = workOrders.accountDescriptionField
            };
            bdoWorkOrder.AccountDescription = workOrders.accountDescriptionField;
            bdoWorkOrder.DateCreated = workOrders.dateCreatedField;
            bdoWorkOrder.WorkOrderId = workOrders.workOrderIDField;
            bdoWorkOrder.WorkOrderNumber = workOrders.workOrderNumberField.ToString();
            bdoWorkOrder.RequesterEmail = workOrders.requesterEmailField;
            bdoWorkOrder.RequesterPhone = workOrders.requesterPhoneField;
            bdoWorkOrder.Description = workOrders.descriptionField;
            bdoWorkOrder.SkillCode = workOrders.skillCodeField;
            bdoWorkOrder.StatusCode = workOrders.statusCodeField;
            bdoWorkOrder.StatusDescription = workOrders.statusDescriptionField;
            bdoWorkOrder.SubStatusCode = workOrders.subStatusCodeField;
            bdoWorkOrder.SubStatusDescription = workOrders.subStatusDescriptionField;
            bdoWorkOrder.PriorityCode = workOrders.priorityCodeField;
            bdoWorkOrder.PriorityDescription = workOrders.priorityDescriptionField;
            bdoWorkOrder.TypeCode = workOrders.typeCodeField;
            bdoWorkOrder.TypeDescription = workOrders.typeDescriptionField;
            bdoWorkOrder.RequesterComments = workOrders.requesterCommentsField;
            bdoWorkOrder.CompletionComments = workOrders.completionCommentsField;
            bdoWorkOrder.ItemCode = workOrders.itemCodeField;
            bdoWorkOrder.ItemDescription = workOrders.itemDescriptionField;
            bdoWorkOrder.RequesterName = workOrders.requesterNameField;
            bdoWorkOrder.AssetNumber = workOrders.assetNumberField;
            bdoWorkOrder.DateUpdated = workOrders.dateUpdatedField;
            // bdoWorkOrder.CreateDate = workOrders.dateCreatedField.Value.ToShortDateString();
            return bdoWorkOrder;
        }

        #endregion

        #region  HolyName ASSET WS

        public List<TFloorAssets> GetAssetsByWorkOrder(int workOrderId)
        {
            List<TFloorAssets> floorAssets = new List<TFloorAssets>();
           Asset2[] assets = client.WorkOrderAssets(workOrderId);
            foreach (Asset2 data in assets)
            {
                TFloorAssets dataNew = new TFloorAssets();               
                dataNew.TmsReference = Convert.ToString(data.assetIDField);
                floorAssets.Add(dataNew);
            }
            return floorAssets;
        }

        public List<HCF.BDO.WorkOrder> GetAssetWorkOrders(int tmsAssetId)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrder1[] workOrders = client.GetAssetWorkOrders(tmsAssetId);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertlocalWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }


        public List<HCF.BDO.DownTime> GetAssetDowntime(int tmsAssetId)
        {
            List<HCF.BDO.DownTime> bdoDownTimes = new List<HCF.BDO.DownTime>();
            DownTime1[] downtime = client.GetAssetsDownTime(tmsAssetId.ToString());
            for (int i = 0; i < downtime.Length; i++)
            {
                HCF.BDO.DownTime bdoDownTime = ConvertlocalDowntime(downtime[i]);
                bdoDownTimes.Add(bdoDownTime);
            }
            return bdoDownTimes;
        }

        private HCF.BDO.DownTime ConvertlocalDowntime(DownTime1 downTime1)
        {
            HCF.BDO.DownTime bdoDownTime = new HCF.BDO.DownTime();
            bdoDownTime.AssetId = downTime1.assetIDField;
            bdoDownTime.AssetNumber = downTime1.assetNumberField;
            bdoDownTime.Description = downTime1.descriptionField;
            bdoDownTime.DownTimeId = downTime1.downTimeIDField;
            bdoDownTime.EndDate = downTime1.endDateField;
            bdoDownTime.StartDate = downTime1.startDateField;
            bdoDownTime.TotalHours = downTime1.totalHoursField;
            bdoDownTime.WorkOrderId = downTime1.wOIdField;
            bdoDownTime.WorkOrderNumber = downTime1.wONumberField;
            return bdoDownTime;
        }


        #endregion

        #region HolyName WorkOrder AssigenMent

        public List<UserProfile> GetResourceByWorkOrder(int workOrderId)
        {
            List<UserProfile> users = new List<UserProfile>();
            Resource1[] resources = client.GetAssignments(workOrderId);
            foreach (Resource1 data in resources)
            {
                UserProfile user = new UserProfile
                {
                    ResourceId = data.resourceIDField,
                    ResourceNumber = data.resourceNumberField
                };
                users.Add(user);
            }
            return users;
        }

        #endregion

        #endregion

        #region Burke 

        #region BurKe WO Methods

        /// <summary>
        /// This method will save the supplied Work Order to the database, only after passing TMS business validation rules. 
        /// </summary>
        /// <param name="workorder"></param>
        /// <param name="_tfloorAssets"></param>
        public HCF.BDO.WorkOrder SaveBurkeTMSWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            BurkeWorkOrderClient obj = new BurkeWorkOrderClient();
            WorkOrder3 wo = ConvertBurkeWorkOrder(workorder);
            try
            {
                wo = obj.SaveBurkeWoOrder(wo);
                if (wo.workOrderIDField > 0)
                {
                    workorder.WorkOrderId = wo.workOrderIDField;
                    workorder.WorkOrderNumber = wo.workOrderNumberField.ToString();

                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            Resource3 resource = new Resource3
                            {
                                resourceNumberField = user.ResourceNumber,
                                resourceIDField = user.ResourceId.Value,
                                firstNameField = user.FirstName,
                                lastNameField = user.LastName
                            };
                            obj.SaveBurkeAssignment(wo.workOrderIDField, resource);
                        }
                    }
                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        Asset3 asset = ConvertBurkeAssets(floorAssets);
                        obj.BurkeSaveAsset(wo.workOrderIDField, asset);
                    }
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);
                workorder.WorkOrderId = -2;
                workorder.WorkOrderNumber = "-2";
            }
            return workorder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workorder"></param>
        /// <returns></returns>
        public HCF.BDO.WorkOrder UpdateBurkeWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            BurkeWorkOrderClient obj = new BurkeWorkOrderClient();
            WorkOrder3 wo = ConvertBurkeWorkOrder(workorder);
            try
            {
                wo = obj.SaveBurkeWoOrder(wo);
                if (wo.workOrderIDField > 0)
                {
                    workorder.WorkOrderId = wo.workOrderIDField;
                    workorder.WorkOrderNumber = wo.workOrderNumberField.ToString();
                    foreach (UserProfile user in workorder.Resources)
                    {
                        if (user.ResourceId.HasValue)
                        {
                            Resource3 resource = new Resource3
                            {
                                resourceNumberField = user.ResourceNumber,
                                resourceIDField = user.ResourceId.Value,
                                firstNameField = user.FirstName,
                                lastNameField = user.LastName
                            };
                            obj.SaveBurkeAssignment(wo.workOrderIDField, resource);
                        }
                    }

                    foreach (TFloorAssets floorAssets in workorder.FloorAssets)
                    {
                        if (!string.IsNullOrEmpty(floorAssets.TmsReference))
                        {
                            try
                            {
                                Asset3 asset = ConvertBurkeAssets(floorAssets);
                                obj.BurkeSaveAsset(wo.workOrderIDField, asset);
                            }
                            catch (Exception ex)
                            {
                                HCF.Utility.ErrorLog.LogError(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);

            }
            return workorder;
        }

        public List<HCF.BDO.WorkOrder> GetBurkeWorkOrder(int workOrderNumber)
        {
            WorkOrder3[] workOrders = burkWOClient.GetBurkeWorkOrder(Convert.ToString(workOrderNumber));
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertBurkeWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// This method will return List of work orders for the supplied Work Order Status. 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> GetBurkeWorkOrders(string statusCode)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria1[] criteria = new WorkOrderCriteria1[1];
            WorkOrderCriteria1 c1 = new WorkOrderCriteria1
            {
                fieldField = FIELDS8.STATUS_CODE,
                fieldValueField = statusCode,
                operatorField = OPERATORS8.EQUAL,
                logicalOperatorField = LOGICAL_OPERATORS8.AND
            };
            criteria[0] = c1;
            WorkOrder3[] workOrders = burkWOClient.QueryBurkeWO(criteria);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertBurkeWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        public List<HCF.BDO.WorkOrder> BurkeGetAllWorkOrders()
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria1[] criteria = new WorkOrderCriteria1[1];
            WorkOrderCriteria1 c1 = new WorkOrderCriteria1
            {
                fieldField = FIELDS8.WORKORDER_NUMBER,
                fieldValueField = 1,
                operatorField = OPERATORS8.GREATER,
                logicalOperatorField = LOGICAL_OPERATORS8.AND
            };
            criteria[0] = c1;
            WorkOrder3[] workOrders = burkWOClient.QueryBurkeWO(criteria);
            for (int i = 0; i < workOrders.Length; i++)
            {
                HCF.BDO.WorkOrder bdoWorkOrder = ConvertBurkeWorkOrder(workOrders[i]);
                bdoWorkOrders.Add(bdoWorkOrder);
            }
            return bdoWorkOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderNumber"></param>
        /// <returns></returns>
        public List<HCF.BDO.WorkOrder> UpdateBurkeWorkOrders(List<string> workOrderNumber)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            WorkOrderCriteria1[] criteria = new WorkOrderCriteria1[workOrderNumber.Count];
            for (int i = 0; i < workOrderNumber.Count; i++)
            {
                WorkOrderCriteria1 c1 = new WorkOrderCriteria1
                {
                    fieldField = FIELDS8.WORKORDER_NUMBER,
                    fieldValueField = workOrderNumber[i],
                    operatorField = OPERATORS8.EQUAL,
                    logicalOperatorField = LOGICAL_OPERATORS8.OR
                };
                criteria[i] = c1;
            }
            try
            {
                WorkOrder3[] workOrders = burkWOClient.QueryBurkeWO(criteria);
                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertBurkeWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogError(ex);
            }
            return bdoWorkOrders;
        }

        public List<HCF.BDO.WorkOrder> GetBurkeWorkOrdersByCauseCode(int days)
        {
            List<HCF.BDO.WorkOrder> bdoWorkOrders = new List<HCF.BDO.WorkOrder>();
            try
            {
                WorkOrderCriteria1[] criteria = new WorkOrderCriteria1[2];
                WorkOrderCriteria1 c1 = new WorkOrderCriteria1();
                c1.fieldField = FIELDS8.TYPE_CODE;
                c1.fieldValueField = "PM";
                c1.operatorField = OPERATORS8.EQUAL;
                c1.logicalOperatorField = LOGICAL_OPERATORS8.OR;

                //WorkOrderCriteria1 c3 = new WorkOrderCriteria1();
                //c3.fieldField = FIELDS3.TYPE_CODE;
                //c3.fieldValueField = "PE";
                //c3.operatorField = OPERATORS3.EQUAL;
                //c3.logicalOperatorField = LOGICAL_OPERATORS3.OR;

                WorkOrderCriteria1 c6 = new WorkOrderCriteria1();
                c6.fieldField = FIELDS8.DATE_CREATED;
                c6.fieldValueField = DateTime.UtcNow.AddDays(-days);
                c6.operatorField = OPERATORS8.GREATER_OR_EQUAL;
                c6.logicalOperatorField = LOGICAL_OPERATORS8.AND;

                WorkOrderCriteria1 c7 = new WorkOrderCriteria1();
                c6.fieldField = FIELDS8.DATE_UPDATED;
                c6.fieldValueField = DateTime.UtcNow.AddDays(-days);
                c6.operatorField = OPERATORS8.GREATER_OR_EQUAL;
                c6.logicalOperatorField = LOGICAL_OPERATORS8.AND;


                //WorkOrderCriteria c8 = new WorkOrderCriteria();
                //c8.fieldField = FIELDS1.TYPE_CODE;
                //c8.fieldValueField = "PE";
                //c8.operatorField = OPERATORS1.EQUAL;
                //c8.logicalOperatorField = LOGICAL_OPERATORS1.AND;



                criteria[0] = c1;
                criteria[1] = c6;
                //criteria[2] = c6;
                //criteria[2] = c8;
                //criteria[2] = c7;

                WorkOrder3[] workOrders = burkWOClient.QueryBurkeWO(criteria);
                workOrders = workOrders.Where(x => x.dateCreatedField > DateTime.UtcNow.AddDays(-days) || x.dateUpdatedField > DateTime.UtcNow.AddDays(-days)).OrderBy(x => x.dateCreatedField).ThenBy(x => x.dateUpdatedField).ToArray();

                for (int i = 0; i < workOrders.Length; i++)
                {
                    HCF.BDO.WorkOrder bdoWorkOrder = ConvertBurkeWorkOrder(workOrders[i]);
                    bdoWorkOrders.Add(bdoWorkOrder);
                }
            }
            catch (Exception ex)
            {
                //HCF.DAL.ErrorRepository.Instance.SaveError(ex, "TMSHolyName_GetWorkOrdersByCauseCode");
                HCF.Utility.ErrorLog.LogError(ex);
            }
            return bdoWorkOrders;
        }

        #endregion

        #region Burke Object Convert

        private WorkOrder3 ConvertBurkeWorkOrder(HCF.BDO.WorkOrder workorder)
        {
            WorkOrder3 wo = new WorkOrder3
            {
                accountCodeField = workorder.AccountCode,
                descriptionField = workorder.Description,
                priorityCodeField = workorder.PriorityCode,
                segmentIDField = workorder.SegmentID,
                skillCodeField = workorder.SkillCode,
                statusCodeField = workorder.StatusCode,
                subStatusCodeField = workorder.SubStatusCode,
                typeCodeField = workorder.TypeCode
            };
            wo.workOrderIDField = workorder.WorkOrderId.HasValue ? workorder.WorkOrderId.Value : 0;
            if (!string.IsNullOrEmpty(workorder.WorkOrderNumber))
            {
                wo.workOrderNumberField = Convert.ToInt32(workorder.WorkOrderNumber);
            }
            else
            {
                wo.workOrderNumberField = 0;
            }
            wo.requesterEmailField = workorder.RequesterEmail;
            wo.requesterPhoneField = workorder.RequesterPhone;
            wo.requesterCommentsField = workorder.RequesterComments;
            wo.requesterNameField = workorder.RequesterName;
            wo.problemCodeField = workorder.ProblemCode;
            wo.causeCodeField = workorder.CauseCode;
            wo.completionCommentsField = workorder.CompletionComments;

            return wo;
        }

        private static HCF.BDO.WorkOrder ConvertBurkeWorkOrder(WorkOrder3 workOrders)
        {
            HCF.BDO.WorkOrder bdoWorkOrder = new HCF.BDO.WorkOrder
            {
                AccountCode = workOrders.accountCodeField,
                AccountDescription = workOrders.accountDescriptionField
            };
            bdoWorkOrder.AccountDescription = workOrders.accountDescriptionField;
            bdoWorkOrder.DateCreated = workOrders.dateCreatedField;
            bdoWorkOrder.WorkOrderId = workOrders.workOrderIDField;
            bdoWorkOrder.WorkOrderNumber = workOrders.workOrderNumberField.ToString();
            bdoWorkOrder.RequesterEmail = workOrders.requesterEmailField;
            bdoWorkOrder.RequesterPhone = workOrders.requesterPhoneField;
            bdoWorkOrder.Description = workOrders.descriptionField;
            bdoWorkOrder.SkillCode = workOrders.skillCodeField;
            bdoWorkOrder.StatusCode = workOrders.statusCodeField;
            bdoWorkOrder.StatusDescription = workOrders.statusDescriptionField;
            bdoWorkOrder.SubStatusCode = workOrders.subStatusCodeField;
            bdoWorkOrder.SubStatusDescription = workOrders.subStatusDescriptionField;
            bdoWorkOrder.PriorityCode = workOrders.priorityCodeField;
            bdoWorkOrder.PriorityDescription = workOrders.priorityDescriptionField;
            bdoWorkOrder.TypeCode = workOrders.typeCodeField;
            bdoWorkOrder.TypeDescription = workOrders.typeDescriptionField;
            bdoWorkOrder.RequesterComments = workOrders.requesterCommentsField;
            bdoWorkOrder.CompletionComments = workOrders.completionCommentsField;
            bdoWorkOrder.ItemCode = workOrders.itemCodeField;
            bdoWorkOrder.ItemDescription = workOrders.itemDescriptionField;
            bdoWorkOrder.RequesterName = workOrders.requesterNameField;
            bdoWorkOrder.AssetNumber = workOrders.assetNumberField;
            bdoWorkOrder.DateUpdated = workOrders.dateUpdatedField;
            return bdoWorkOrder;
        }

        private Asset3 ConvertBurkeAssets(TFloorAssets tfloorAssets)
        {
            Asset3 asset = new Asset3();
            asset.assetIDField = Convert.ToInt32(tfloorAssets.TmsReference);
            asset.assetNumberField = tfloorAssets.AssetNo;
            return asset;
        }

        private TFloorAssets ConvertBurkeAssets(Asset3 asset)
        {
            TFloorAssets floorAssets = new TFloorAssets
            {
                TmsReference = asset.assetIDField.ToString(),
                DeviceNo = asset.assetNumberField
            };
            return floorAssets;
        }

        #endregion

        #region Burke ASSET WS

        public List<TFloorAssets> GetBurkeAssetsByWorkOrder(int workOrderId)
        {
            List<TFloorAssets> floorAssets = new List<TFloorAssets>();
            Asset3[] assets = burkWOClient.WorkOrderAssetsBurke(workOrderId);
            foreach (Asset3 data in assets)
            {
                TFloorAssets dataNew = new TFloorAssets();
                dataNew.TmsReference = Convert.ToString(data.assetIDField);
                floorAssets.Add(dataNew);
            }
            return floorAssets;
        }

        #endregion

        #region HolyName WorkOrder AssigenMent

        public List<UserProfile> GetBurkeResourceByWorkOrder(int workOrderId)
        {
            List<UserProfile> users = new List<UserProfile>();
            Resource3[] resources = burkWOClient.GetBurkeAssignments(workOrderId);
            foreach (Resource3 data in resources)
            {
                UserProfile user = new UserProfile();
                user.ResourceId = data.resourceIDField;
                user.ResourceNumber = data.resourceNumberField;
                users.Add(user);
            }
            return users;
        }

        #endregion

        #endregion

    }
}
