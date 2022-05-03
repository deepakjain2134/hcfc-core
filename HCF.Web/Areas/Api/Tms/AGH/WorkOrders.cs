using HCF.BDO;
using HCF.BDO.AGH;
using Hcf.Api.Tms.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Request = HCF.BDO.AGH.Request;

namespace Hcf.Api.Tms.AGH
{
    public class AGHWorkOrders : WorkOrderFactory
    {
        private static string AghServiceDeskBaseUrl = HCF.Utility.ServiceDeskConstantFields.HCF_Altantic.AGHServiceDeskBaseURL;

        private static string AghServiceDeskAuthToken = HCF.Utility.ServiceDeskConstantFields.HCF_Altantic.AGHServiceDeskAuthToken;

        public override WorkOrder GetWorkOrder(string workorderId)
        {
            var wo = new HCF.BDO.WorkOrder();
            var serviceUrl = $"/requests/{workorderId}";
            var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Authorization", AghServiceDeskAuthToken)
            };
            var responseJson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
            if (!string.IsNullOrEmpty(responseJson))
            {
                var response = JsonConvert.DeserializeObject<ServiceDeskRequestResponse>(responseJson);
                var requests = new List<Request> { response.request };
                var woList = ConvertServiceDeskRequestToWo(requests);
                return woList.FirstOrDefault();
            }
            return wo;
        }

        public override List<WorkOrder> GetWorkorderByWoIds(List<string> workorderIds)
        {
            List<WorkOrder> woList = new List<WorkOrder>();
            foreach (var woid in workorderIds)
            {
                try
                {
                    if (!string.IsNullOrEmpty(woid))
                    {
                        WorkOrder wo = GetWorkOrder(woid);
                        if (!string.IsNullOrEmpty(wo.SourceWoId))
                            woList.Add(wo);
                    }
                }
                catch (Exception ex)
                {
                    //HCF.Utility.ErrorLog.LogError(ex);
                }
            }
            return woList;
        }

        public override List<WorkOrder> GetWorkOrderByQuery(string fieldName, string queryCondition, string value)
        {
            //var array[]= new // new string[](value);
            var requestCondition = new RequestConditions.RequestCondition()
            {
                list_info = new RequestConditions.ListInfo()
                {
                    start_index = 1,
                    row_count = 500,
                    sort_field = "created_time.value",
                    sort_order = "desc",
                    search_criteria = new RequestConditions.SearchCriteria2()
                    {
                        field = fieldName,
                        condition = queryCondition,
                        value = value
                    }
                }
            };

            var condition = JsonConvert.SerializeObject(requestCondition);
            var serviceUrl = $"/requests?input_data={condition}";
            var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
            var headers = new List<Tuple<string, string>>();
            headers.Add(new Tuple<string, string>("Authorization", AghServiceDeskAuthToken));
            var responsejson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
            //var json_serializer = new JavaScriptSerializer();
            var Response = JsonConvert.DeserializeObject<ServiceDeskRequestsResponse>(responsejson);
            var hasMoreRows = Response.list_info.has_more_rows;
            var startIndex = 500;
            while (hasMoreRows)
            {
                requestCondition.list_info.start_index = startIndex;
                condition = JsonConvert.SerializeObject(requestCondition);
                serviceUrl = $"/requests?input_data={condition}";
                url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
                responsejson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
                var moreResp = JsonConvert.DeserializeObject<ServiceDeskRequestsResponse>(responsejson);
                Response.requests.AddRange(moreResp.requests);
                hasMoreRows = moreResp.list_info.has_more_rows;
                startIndex += 100;
            }
            var woList = new List<WorkOrder>();
            //foreach (var req in Response.requests)
            //{
            //    WorkOrder newWo = GetWorkOrder(req.id);
            //    woList.Add(newWo);
            //}
            woList = ConvertServiceDeskRequestToWo(Response.requests.Where(x => x.template != null && x.template.id == "35057000000544244"));
            return woList;
        }

        public override WorkOrder SaveTmsWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        public override WorkOrder UpdateTmsWorkOrder(WorkOrder workOrder)
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkOrderByQuery(string strquery)
        {
            throw new NotImplementedException();
        }

        public override List<WorkOrder> GetWorkOrders()
        {

            var requestCondition = new RequestConditions.RequestCondition()
            {
                list_info = new RequestConditions.ListInfo()
                {
                    start_index = 1,
                    row_count = 100,
                    sort_field = "created_time.value",
                    sort_order = "desc",
                    search_criteria = new RequestConditions.SearchCriteria()
                    {
                        field = "mode",
                        condition = "is",
                        value = new RequestConditions.Value()
                        {
                            name = "CRx",
                            id = "35057000011954003"
                        }
                    }
                }
            };


            var condition = JsonConvert.SerializeObject(requestCondition);
            var serviceUrl = $"/requests?input_data={condition}";
            var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Authorization", AghServiceDeskAuthToken)
            };
            var responsejson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
            //var json_serializer = new JavaScriptSerializer();
            var Response = JsonConvert.DeserializeObject<ServiceDeskRequestsResponse>(responsejson);
            var hasMoreRows = Response.list_info.has_more_rows;
            var startIndex = Response.list_info.start_index;
            while (hasMoreRows)
            {
                requestCondition.list_info.start_index = Response.list_info.start_index + Response.list_info.row_count;
                condition = JsonConvert.SerializeObject(requestCondition);
                serviceUrl = string.Format("/requests?input_data={0}", condition);
                url = string.Format("{0}{1}", AghServiceDeskBaseUrl, serviceUrl);
                responsejson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
                var MoreResp = JsonConvert.DeserializeObject<HCF.BDO.AGH.ServiceDeskRequestsResponse>(responsejson);
                Response.requests.AddRange(MoreResp.requests);
                hasMoreRows = MoreResp.list_info.has_more_rows;
                startIndex = MoreResp.list_info.start_index;
            }
            List<WorkOrder> woList = ConvertServiceDeskRequestToWo(Response.requests);
            return woList;
        }

        public override bool IsServiceUp()
        {
            try
            {
                var requestCondition = new HCF.BDO.AGH.RequestConditions.RequestCondition()
                {
                    list_info = new HCF.BDO.AGH.RequestConditions.ListInfo()
                    {
                        start_index = 1,
                        row_count = 1
                    }
                };


                var condition = "{'list_info':{'start_index':1,'row_count':1}}"; //new JavaScriptSerializer().Serialize(requestCondition);
                var serviceUrl = $"/requests?input_data={condition}";
                var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
                var headers = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("Authorization", AghServiceDeskAuthToken)
                };
                var responsejson = HCF.Utility.CommonUtility.CallGetMethod1(url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
                //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                JsonConvert.DeserializeObject<ServiceDeskRequestsResponse>(responsejson);
            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
                return false;
            }
            return true;
        }

        public override WorkOrder SaveWorkOrder(WorkOrder workOrder)
        {
            var woList = new List<WorkOrder>();
            var postDataString = string.Empty;
            try
            {
                var request = GetRequestObjByWO(workOrder);
                var postData = new { request = request };
                postDataString = JsonConvert.SerializeObject(postData);
                var serviceUrl = string.Format("/requests");
                var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
                var headers = new List<Tuple<string, string>>();
                headers.Add(new Tuple<string, string>("Authorization", AghServiceDeskAuthToken));
                var responsejson = HCF.Utility.CommonUtility.CallPostMethod(postDataString, url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
                //var json_serializer = new JavaScriptSerializer();
                var Response = JsonConvert.DeserializeObject<ServiceDeskRequestResponse>(responsejson);
                var requests = new List<HCF.BDO.AGH.Request> { Response.request };
                woList = ConvertServiceDeskRequestToWo(requests);

            }
            catch (Exception ex)
            {
                //HCF.Utility.ErrorLog.LogError(ex);
                //HCF.Utility.ErrorLog.LogMsg(postDataString);
                workOrder.WorkOrderId = -2;
                workOrder.WorkOrderNumber = "-2";
                woList.Add(workOrder);
            }
            return woList.FirstOrDefault();
        }

        public override WorkOrder UpdateWorkOrder(WorkOrder workOrder)
        {
            var request = GetRequestObjByWO(workOrder);
            var postData = new { request = request };
            var postDataString = JsonConvert.SerializeObject(postData);

            var serviceUrl = $"/requests/{workOrder.SourceWoId}";
            var url = $"{AghServiceDeskBaseUrl}{serviceUrl}";
            var headers = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Authorization", AghServiceDeskAuthToken)
            };
            var responseJson = HCF.Utility.CommonUtility.CallPutMethod(postDataString, url, "application/x-www-form-urlencoded", "application/vnd.manageengine.sdp.v3+json", headers);
            //var jsonSerializer = new JavaScriptSerializer();
            var response = JsonConvert.DeserializeObject<ServiceDeskRequestResponse>(responseJson);
            var requests = new List<Request> { response.request };
            var woList = ConvertServiceDeskRequestToWo(requests);
            return woList.FirstOrDefault();
        }

        private static List<WorkOrder> ConvertServiceDeskRequestToWo(IEnumerable<Request> requests)
        {
            var listWo = new List<WorkOrder>();
            foreach (var item in requests)
            {
                if (item != null)
                {
                    var wo = new WorkOrder
                    {
                        WorkOrderNumber = item.display_id,
                        WorkOrderId = item.display_id == null ? default(int?) : Convert.ToInt32(item.display_id),
                        SourceWoId = item.id,
                        Description = !string.IsNullOrEmpty(item.description) ? item.description : item.subject,
                        StatusCode = item.status.name,
                        SubStatusCode = item.status.name,
                        StatusDescription = item.status.name,
                        SubStatusDescription = item.status.name,
                        RequesterPhone = item.requester.phone,
                        SiteName = item.site != null ? item.site.name : "",
                        RequesterName = item.requester.name,
                        RequesterEmail = item.requester.email_id,
                        DateCreated = item.created_time != null ? DateTime.Parse(item.created_time.display_value) : null,
                        TypeCode = (item.template.name == "Plant Operations") ? "PO" : item.template.name
                    };
                    if (item.technician != null)
                    {
                        var usersAssigned = new List<UserProfile>();
                        var user = new UserProfile { Email = item.technician.email_id, Name = item.technician.name };
                        usersAssigned.Add(user);
                        wo.Resources = usersAssigned;
                    }
                    listWo.Add(wo);
                }
            }
            return listWo;
        }

        private static Request GetRequestObjByWO(WorkOrder workOrder, bool updateRequest = false)
        {

            var request = new HCF.BDO.AGH.Request();
            if (!updateRequest)
            {
                var description = !string.IsNullOrEmpty(workOrder.RequesterComments) ? $"{workOrder.Description}, '{"Requester Comment"} : {workOrder.RequesterComments}'"
                    : workOrder.Description;
                request.subject = HCF.Utility.CommonUtility.TruncateLongString(description, 100);
                request.description = description;

                var template = new Template { name = workOrder.TemplateName };
                request.template = template;

                var mode = new Mode { name = workOrder.Mode };
                request.mode = mode;

                var group = new Group();
                group.name = workOrder.Group;
                request.group = group;

                var requester = new Requester { name = workOrder.RequesterName };

                request.requester = requester;

                var status = new HCF.BDO.AGH.Status { name = workOrder.StatusCode };
                request.status = status;

                if (workOrder.Resources != null && workOrder.Resources.Any())
                {
                    var technician = new Technician
                    {
                        email_id = workOrder.Resources.FirstOrDefault()?.Email,
                        name = workOrder.Resources.FirstOrDefault()?.Name
                    };
                    request.technician = technician;
                }

                var site = new HCF.BDO.AGH.Site { id = workOrder.SiteCode, name = workOrder.SiteName };
                request.site = site;

                var category = new HCF.BDO.AGH.Category { name = workOrder.CategoryName };
                request.category = category;
            }
            else
            {
                var status = new HCF.BDO.AGH.Status { name = workOrder.StatusDescription };
                request.status = status;
            }

            return request;
        }
    }
}