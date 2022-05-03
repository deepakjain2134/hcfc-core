using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hcf.Api.Application;
using Hcf.Api.Tms.AGH;
using Hcf.Api.Tms.Interface;
using Hcf.Api.Tms.MaintConnection;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Areas.Api.Models.Request;
using HCF.Web.Areas.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/wo")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WorkOrder")]
    public class WorkOrderApiController : ApiController
    {
        //private readonly ILoggingService _loggingService;
        private readonly IAssetsService _assetService;
        private readonly IApiCommon _apiCommon;
        private readonly IWorkOrderService _workOrderService;
        private readonly ITEPInspectionService _tEPInspectionService;
        private readonly IOrganizationService _organizationService;
        private readonly IHCFSession _session;
        private readonly IMapper _mapper;
        public WorkOrderApiController(IWorkOrderService workOrderService
            , IApiCommon apiCommon, IHCFSession session,
            //ILoggingService loggingService, 
            IAssetsService assetService,
            ITEPInspectionService tEPInspectionService, IOrganizationService organizationService, IMapper mapper
            )
        {
            _session = session;
            _tEPInspectionService = tEPInspectionService;
            _workOrderService = workOrderService;
            _apiCommon = apiCommon;
            //_loggingService = loggingService;
            _assetService = assetService;
            _organizationService = organizationService;
            _mapper = mapper;
        }

        #region Methods 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWorkOrdertFloorAssets(string IssueId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            // var tfloorAssets = new List<TFloorAssets>();
            if (!string.IsNullOrEmpty(IssueId))
            {
                _objHttpResponseMessage.Result = new Result
                {
                    TFloorAssets = _workOrderService.GetIssuesFloorAssets(Convert.ToInt32(IssueId))
                };
            }
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("getIssuesPaging")]
        public ActionResult GetIssuesPaging(Request objRequest)
        {
            int? profilerId = null;
            if (objRequest.UserId.HasValue)
                profilerId = Convert.ToInt32(objRequest.UserId);

            var _objHttpResponseMessage = new HttpResponseMessage();
            _objHttpResponseMessage = GetUpdatedWorkOrders_1(objRequest, profilerId);
            if (_objHttpResponseMessage.Result != null)
            {
                _objHttpResponseMessage.Result.WorkOrders = !string.IsNullOrEmpty(objRequest.WhereCondition) ? _objHttpResponseMessage.Result.WorkOrders.Where(x => objRequest.WhereCondition.Split(',').Contains(x.StatusCode)).ToList() : _objHttpResponseMessage.Result.WorkOrders.ToList();
                if (_objHttpResponseMessage.Result.WorkOrders.Count > 0)
                    _objHttpResponseMessage.Success = true;
                else
                    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private HttpResponseMessage GetUpdatedWorkOrders_1(Request objRequest, int? userId)
        {
            // var workOrder = new List<WorkOrder>();
            var org = new Organization();
            //  List<UserProfile> users = new List<UserProfile>();
            // var httpresponse = new HttpResponseMessage();

            //var clientNo = 0;
            if (Convert.ToInt32(_session.ClientNo) > 0)
                org = _apiCommon.GetOrganization(Convert.ToInt32(_session.ClientNo));
            else
                return new HttpResponseMessage { Success = false, Message = "No client # found." };


            var httpResponse = _workOrderService.GetWorkOrdersPaging(objRequest, userId);

            if (httpResponse.Result != null && httpResponse.Result.WorkOrders.Count > 0 && org != null && org.IsTmsWo)
            {
                var workOrders = GetlastWorkOrder(objRequest, userId, httpResponse.Result.WorkOrders, org);
                foreach (var tmsOrder in httpResponse.Result.WorkOrders)
                {
                    var isExistingWorkOrders = workOrders.FirstOrDefault(z => z.WorkOrderNumber == tmsOrder.WorkOrderNumber);
                    if (isExistingWorkOrders != null)
                    {
                        tmsOrder.StatusCode = isExistingWorkOrders.StatusCode;
                    }
                }
            }
            //if (httpresponse.Result.WorkOrders != null && httpresponse.Result.WorkOrders.Count > 0 && org != null && org.IsTmsWo)
            //{
            //    workOrder = httpresponse.Result.WorkOrders.ToList();
            //    httpresponse = GetlastWorkOrder(objRequest, userId, workOrder, org, httpresponse);

            //    if (HcfSession.ClientDBName.ToLower() == Enums.ClientName.HCF_Atlantic.ToString().ToLower())
            //    {
            //        AGHWorkOrders aghWo = new AGHWorkOrders();
            //        List<WorkOrder> woList = aghWo.GetWorkOrderByQuery("display_id", "like", objRequest.SearchcBy);
            //        httpresponse.PageCount = 0;
            //        httpresponse.Success = true;
            //        httpresponse.Result.WorkOrders = woList;
            //    }
            //}
            return httpResponse;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private List<WorkOrder> GetlastWorkOrder(Request objRequest, int? userId, List<WorkOrder> workOrder, Organization org)
        {
            var tmsWorkOrders = new List<WorkOrder>();
            WorkOrderFactory wof = new AGHWorkOrders();
            var serviceStatus = org.DbName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower() ? wof.IsServiceUp() : true;
            if (serviceStatus)
            {
                if (workOrder.Count > 0)
                {
                    var lastUpdateDate = workOrder.Where(x => x.DateCreated != null).OrderByDescending(x => x.DateCreated).FirstOrDefault().DateCreated.Value;
                    var currentDateTime = DateTime.Now;
                    var days = Convert.ToInt32((currentDateTime - lastUpdateDate).TotalDays);
                    var workOrderLists = new List<string>();


                    if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_Holy.ToString().ToLower() || org.DbName.ToLower() == BDO.Enums.ClientName.HCF_Burke.ToString().ToLower())
                        workOrderLists = workOrder.Where(x => x.StatusCode != "CMPLT" && x.StatusCode != "CANCL" && x.StatusCode.ToLower() != "Closed".ToLower()).Select(x => x.WorkOrderNumber).ToList();
                    else if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower())
                        workOrderLists = workOrder.Where(x => x.CRxCode != "CMPLT" && x.CRxCode != "CANCL" && x.CRxCode != "CLOSE" && x.StatusCode.ToLower() != "Closed".ToLower()).Select(x => x.SourceWoId).ToList();
                    else if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
                        workOrderLists = workOrder.Where(x => x.CRxCode != "CMPLT" && x.CRxCode != "CANCL" && x.CRxCode != "CLOSE" && x.StatusCode.ToLower() != "Closed".ToLower()).Select(x => x.WorkOrderId.ToString()).ToList();
                    else if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_NBIMC.ToString().ToLower())
                        workOrderLists = workOrder.Where(x => x.StatusCode != "CMPLT" && x.StatusCode != "CANCL" && x.StatusCode.ToLower() != "Closed".ToLower()).Select(x => x.WorkOrderNumber).ToList();

                    if (workOrderLists.Count > 0)
                    {
                        tmsWorkOrders = UpdateWorkOrders(workOrderLists, org.DbName.ToLower(), workOrder); //org.DbName.ToLower() == Enums.ClientName.HCF_Atlantic.ToString().ToLower() ? UpdateWorkOrders(workOrderLists, org.DbName.ToLower(), workOrder) : UpdateWorkOrders(workOrderLists, org.DbName.ToLower());
                        if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower())
                        {
                            foreach (var wo in tmsWorkOrders)
                            {
                                wo.IssueId = workOrder.FirstOrDefault(x => x.WorkOrderNumber == wo.WorkOrderNumber).IssueId;
                                if (wo.IssueId > 0)
                                    _workOrderService.UpdateWorkOrder(wo);
                            }
                        }
                    }
                    //update pending workOrder
                    //if (objRequest.PageIndex == 1)
                    //{
                    //    AddTmsWorkOrders(days, org.DbName.ToLower());
                    //}
                }
                else
                {
                    //if (objRequest.PageIndex == 1)
                    //{
                    //    AddTmsWorkOrders(30, org.DbName.ToLower());
                    //}
                }
                //httpresponse.Result=new Result
                //{
                //    WorkOrders = tmsWorkOrders
                //};
                //  httpresponse = WorkOrderRepository.GetWorkOrdersPaging(objRequest, userId);
            }

            return tmsWorkOrders;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private static List<WorkOrder> UpdateWorkOrders(List<string> workOrderLists, string clientName, List<WorkOrder> workOrders = null)
        {
            var workorder = new List<WorkOrder>();
            var wo = new AGHWorkOrders();
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    workorder = obj.UpdateWorkOrders(workOrderLists);
            //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    workorder = obj.UpdateBurkeWorkOrders(workOrderLists);
            //else 
            if (clientName.ToLower() == BDO.Enums.ClientName.HCF_NBIMC.ToString().ToLower())
                TMS.NBIH.WorkOrders.UpdateWorkOrders(workOrderLists);
            else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
            {
                WorkOrderFactory woObject = new MaintConnection();
                workorder = woObject.GetWorkorderByWoIds(workOrderLists);
            }
            else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower() && wo.IsServiceUp())
            {
                var allWrokOrder = new List<WorkOrder>();
                //allWrokOrder = wo.GetWorkOrders();
                allWrokOrder = wo.GetWorkorderByWoIds(workOrderLists);
                foreach (var worder in allWrokOrder)
                {
                    foreach (var item in workOrderLists)
                    {
                        if (item == worder.SourceWoId)
                        {
                            workorder.Add(worder);
                            break;
                        }
                    }
                }
                var workOrdersList = new List<WorkOrder>();
                foreach (var item in workorder)
                {
                    foreach (var workOrder in workOrders)
                    {
                        if (workOrder.WorkOrderNumber == item.WorkOrderNumber)
                        {
                            workOrder.StatusCode = item.StatusCode;
                            workOrder.SubStatusCode = item.SubStatusCode;
                            workOrdersList.Add(workOrder);
                        }
                    }
                }
                workorder = workOrdersList;
            }
            return workorder;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetsWorkOrder(string tmsAssetId)
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    // WorkOrders = obj.GetAssetWorkOrders(Convert.ToInt32(tmsAssetId)) 
                }
            };
            return _objHttpResponseMessage;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWorkOrderByNo(string workOrderNumber)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            // TMS.TMSHolyName obj = new TMS.TMSHolyName();
            //var workOrders = obj.GetWorkOrder(Convert.ToInt32(workOrderNumber));
            //foreach (WorkOrder wo in workOrders)
            //{
            //    wo.FloorAssets = obj.GetAssetsByWorkOrder(wo.WorkOrderId.Value);
            //}
            _objHttpResponseMessage.Result = new Result
            {
                // WorkOrders = workOrders
            };
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateAssetInspectionDate()
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<TFloorAssets> floorAssets = _assetService.GetFloorAssets().Where(x => x.StatusCode == "ACTIV").ToList();
            //foreach (var item in floorAssets)
            //{
            //    if (!string.IsNullOrEmpty(item.TmsReference))
            //    {
            //        try
            //        {
            //            var workOrder = obj.GetAssetWorkOrders(Convert.ToInt32(item.TmsReference)).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            //            if (workOrder != null)
            //            {
            //                _tEPInspectionService.SaveAssetInspectionDate(item.FloorAssetsId, workOrder.DateCreated.Value);
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            //HCF.Utility.ErrorLog.LogError(ex);
            //        }
            //    }
            //}
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// Get WorkOrder by IssueId (issueId is CRx internal number for a work request originated within CRx)
        /// </summary>       
        /// <param name="issueId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{issueId}")]
        public ActionResult GetWorkOrder(string issueId)
        {
            WorkOrder workorder = _workOrderService.GetWorkOrder(Convert.ToInt32(issueId));
            WorkOrderViewModel workOrderViewModel = _mapper.Map<WorkOrderViewModel>(workorder);
            if (workOrderViewModel.IssueId > 0)
            {
                var data = new Response<WorkOrderViewModel>(workOrderViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));

        }


        /// <summary>
        /// Returns a list of workorders based on last update date time
        /// </summary>
        /// <param name="lastupdatedtime"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("workorders/{lastupdatedtime}")]
        public ActionResult GetworkOrders(DateTime? lastupdatedtime)
        {
            var workorders = _workOrderService.GetWorkOrders().Where(x => x.UpdateDate >= lastupdatedtime);
            List<WorkOrderViewModel> workOrderViewModel = _mapper.Map<List<WorkOrderViewModel>>(workorders);
            if (workOrderViewModel.Count > 0)
            {
                var data = new Response<List<WorkOrderViewModel>>(workOrderViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Creates workorders with the details specified in the request body
        /// </summary>       
        /// <param name="RequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveWorkOrder")]
        public ActionResult SaveWorkOrderRequest(SaveWorkOrderRequestModel RequestModel)
        {

            WorkOrder workOrder = _mapper.Map<WorkOrder>(RequestModel);
            workOrder.WorkOrderId = 0;
            workOrder.IssueId = _workOrderService.AddWorkOrder(workOrder);
            if (workOrder.IssueId > 0)
            {
                var data = new Response<Int32>(workOrder.IssueId, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }

        /// <summary>
        /// Creates multiple workorders with the details specified in the request body
        /// </summary>
        /// <param name="RequestModels"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveWorkOrderRequests")]
        public ActionResult SaveWorkOrderRequests(List<SaveWorkOrderRequestModel> RequestModels)
        {
            var addedWO = 0;
            foreach (var item in RequestModels)
            {
                WorkOrder workOrder = _mapper.Map<WorkOrder>(item);
                workOrder.IssueId = _workOrderService.AddWorkOrder(workOrder);
                addedWO = addedWO++;
            }
            if (addedWO > 0)
            {
                var data = new Response<Int32>(addedWO, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }


        /// <summary>
        /// Update  workorders by IssueId
        /// </summary>       
        /// <param name="RequestModel"></param>
        /// <param name="issueId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{issueId}")]
        public ActionResult UpdateWorkOrder(UpdateWorkOrderRequestModel RequestModel, int issueId)
        {
            WorkOrder workOrder = _mapper.Map<WorkOrder>(RequestModel);
            workOrder.IssueId = issueId;
            workOrder.IssueId = _workOrderService.UpdateWorkOrder(workOrder);
            if (workOrder.IssueId > 0)
            {

                var data = new Response<Int32>(workOrder.IssueId, ConstMessage.Module_Training);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }



        /// <summary>
        /// Update Multiple Temporary WorkOrder Number
        /// </summary>
        /// <param name="RequestModels"></param>        
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTempWorkOrderRequests")]
        public ActionResult UpdateTempWorkOrderRequests(List<UpdateTempWorkOrderRequestModel> RequestModels)
        {
            int addedWO = 0;
            foreach (var item in RequestModels)
            {
                WorkOrder workOrder = _mapper.Map<WorkOrder>(item);
                workOrder.IssueId = _workOrderService.UpdateTempWorkOrder(workOrder);
                addedWO = addedWO + 1;
            }
            if (addedWO > 0)
            {
                var data = new Response<Int32>(addedWO, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));

        }


        #endregion

        #region Off line work order tms update

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateQueueWorkOrders()
        {
            // TMS.TMSHolyName obj = new TMS.TMSHolyName();
            List<WorkOrder> workorders = _workOrderService.QueueWorkOrders();
            WorkOrderFactory wofa = new AGHWorkOrders();
            //if (TMS.HolyName.Common.IsConnectionUp())
            //{
            //    foreach (var workorder in workorders)
            //    {
            //        WorkOrder updatedWorkOrder = new WorkOrder();
            //        if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //            updatedWorkOrder = obj.SaveTMSWorkOrder(workorder);
            //        else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //            updatedWorkOrder = obj.SaveBurkeTMSWorkOrder(workorder);
            //        if (updatedWorkOrder.WorkOrderId > 0)
            //        {
            //            _workOrderService.UpdateWorkOrderId(workorder.IssueId, workorder.WorkOrderId.Value, Convert.ToInt32(workorder.WorkOrderNumber));
            //        }
            //    }
            //}
            if (wofa.IsServiceUp())
            {
                foreach (var workorder in workorders)
                {
                    WorkOrder updatedWorkOrder = new WorkOrder();
                    if (_session.ClientDbName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower())
                    {
                        workorder.TemplateName = ServiceDeskConstantFields.HCF_Altantic.TemplateName;
                        workorder.Mode = ServiceDeskConstantFields.HCF_Altantic.Mode;
                        workorder.Group = ServiceDeskConstantFields.HCF_Altantic.Group;
                        workorder.CategoryId = ServiceDeskConstantFields.HCF_Altantic.CategoryId;
                        workorder.CategoryName = ServiceDeskConstantFields.HCF_Altantic.CategoryName;

                        updatedWorkOrder = wofa.SaveWorkOrder(workorder);
                    }
                    if (updatedWorkOrder.WorkOrderId > 0)
                    {
                        _workOrderService.UpdateWorkOrderId(workorder.IssueId, updatedWorkOrder.WorkOrderId.Value, Convert.ToInt32(updatedWorkOrder.WorkOrderNumber), updatedWorkOrder.SourceWoId);
                    }
                }
            }
            var _objHttpResponseMessage = new HttpResponseMessage();
            return _objHttpResponseMessage;
        }

        #endregion


        #region New Methods for sync TMS database
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("syncworkorders")]
        public HttpResponseMessage SyncWorkOrders(Request objRequest)
        {
            int? profilerId = null;
            if (objRequest.UserId.HasValue)
                profilerId = Convert.ToInt32(objRequest.UserId);

            var _objHttpResponseMessage = new HttpResponseMessage();
            _objHttpResponseMessage = SyncWorkOrders(objRequest, profilerId);
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private HttpResponseMessage SyncWorkOrders(Request objRequest, int? userId)
        {
            var httpResponse = new HttpResponseMessage();
            var workOrders = new List<WorkOrder>();
            if (!string.IsNullOrEmpty(_session.ClientNo))
            {
                var org = _organizationService.GetOrganization();
                if (org != null && org.IsTmsWo)
                {
                    var clientName = org.DbName;
                    var lastWoCreatedDate = org.LastWoTmsSync;
                    var currentDateTime = DateTime.Now;
                    var days = Convert.ToInt32((currentDateTime - lastWoCreatedDate.Value).TotalDays);

                    if (lastWoCreatedDate == null)
                        lastWoCreatedDate = currentDateTime;

                    //if (string.Equals(clientName, Enums.ClientName.HCF_Holy.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    //    workOrders = AddHolyNameWorkOrders(days);
                    //else if (string.Equals(clientName, Enums.ClientName.HCF_Burke.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    //    workOrders = AddBurkWorkOrders(days);
                    //else 
                    if (string.Equals(clientName, BDO.Enums.ClientName.HCF_ACH.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        workOrders = AddAkronWorkOrders(days);
                    else if (string.Equals(clientName, BDO.Enums.ClientName.HCF_Atlantic.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        workOrders = AddAghWorkOrders(objRequest, lastWoCreatedDate.Value.AddDays(-1));
                    else if (string.Equals(clientName, BDO.Enums.ClientName.HCF_NBIMC.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        workOrders = AddNBIWorkOrders(days);
                }

                // save tms work orders to local 
                foreach (var wo in workOrders)
                {
                    if (wo.DateCompleted == DateTime.MinValue)
                        wo.DateCompleted = null;
                    wo.CreatedBy = 4;
                    wo.DeficiencyCode = "OT";
                    wo.IssueId = _workOrderService.AddWorkOrder(wo);
                    if (wo.IssueId > 0)
                    {
                        // wo.IssueId = issueId;
                        _workOrderService.UpdateWorkOrder(wo);
                        //var floorAssets = GetAssetsByWorkOrder(wo.WorkOrderId.Value);
                        //var users = GetResourceByWorkOrder(wo.WorkOrderId.Value);
                        //var tmsAssetsIds = string.Join(",", floorAssets.Select(x => x.TmsReference).ToArray());
                        //var resourceIds = string.Join(",", users.Select(x => x.ResourceId).ToArray());
                        //if (!string.IsNullOrEmpty(tmsAssetsIds) || !string.IsNullOrEmpty(resourceIds))
                        //   WorkOrderRepository.SaveWorkOrderResources(wo.IssueId, "", "", "", "", tmsAssetsIds, resourceIds);
                        //SaveAssetInspectionafterTMSsync(org, tmsAssetsIds, wo);
                    }
                }
            }
            httpResponse.Result = new Result
            {
                WorkOrders = workOrders.Where(x => x.IssueId > 0).ToList()
            };
            httpResponse.TotalResult = workOrders.Count(x => x.IssueId > 0);
            return httpResponse;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void SaveAssetInspectionafterTMSsync(Organization org, string tmsAssetsIds, WorkOrder wo)
        {
            List<MarkPassAssets> assetsList = new List<MarkPassAssets>();
            var assetDetails = _assetService.GetAssetsDetailsByTmsAssetId(tmsAssetsIds);
            for (int i = 0; i < assetDetails.Rows.Count; i++)
            {
                MarkPassAssets objassets = new MarkPassAssets();
                objassets.FloorAssetId = Convert.ToInt32(assetDetails.Rows[i]["FloorAssetsId"].ToString());
                objassets.AssetsId = Convert.ToInt32(assetDetails.Rows[i]["AssetId"].ToString());
                objassets.InspectionId = Convert.ToInt32(assetDetails.Rows[i]["InspectionId"].ToString());
                objassets.EPDetailId = Convert.ToInt32(assetDetails.Rows[i]["EPDetailId"].ToString());
                objassets.InspectionDate = wo.DateCompleted;
                assetsList.Add(objassets);
            }
            string userId = string.Empty;
            //HcfService objservice = new HcfService();
            //var clientName = org.DbName;
            //if (string.Equals(clientName, Enums.ClientName.HCF_Holy.ToString(), StringComparison.CurrentCultureIgnoreCase))
            //    objservice.MarkPassAssets(assetsList, userId);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private static List<WorkOrder> AddAghWorkOrders(Request objRequest, DateTime currentDateTime)
        {
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var logDate = (long)(currentDateTime.ToUniversalTime() - baseDate).TotalMilliseconds;
            var aghWo = new AGHWorkOrders();
            var woList = aghWo.GetWorkOrderByQuery("created_time.value", "greater than", logDate.ToString());
            return woList;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private static List<WorkOrder> AddAkronWorkOrders(int days)
        {
            WorkOrderFactory woObject = new MaintConnection();
            var lastupdateDate = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
            var filterexp = $"&$filter=LastModifiedDate%20ge%20\"{lastupdateDate}\"";
            var workOrders = woObject.GetWorkOrderByQuery(filterexp);
            return workOrders;
        }

        //private static List<WorkOrder> AddBurkWorkOrders(int days)
        //{
        //    var obj = new TMS.TMSHolyName();
        //    var workOrders = obj.GetBurkeWorkOrdersByCauseCode(days).OrderBy(x => x.DateCreated).ToList();
        //    return workOrders;
        //}

        //private static List<WorkOrder> AddHolyNameWorkOrders(int day)
        //{
        //    var obj = new TMS.TMSHolyName();
        //    var workOrders = obj.GetWorkOrdersByCauseCode(day).OrderBy(x => x.DateCreated).ToList();
        //    return workOrders;
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private static List<WorkOrder> AddNBIWorkOrders(int days)
        {
            var workOrders = TMS.NBIH.WorkOrders.GetWorkOrdersByCauseCode(days).OrderBy(x => x.DateCreated).ToList();
            return workOrders;
        }

        #endregion


        #region Workorder Assets 


        [HttpPost]
        public ActionResult AddWorkOrderResources()
        {
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        [HttpDelete]
        public ActionResult DeleteWorkOrderResources()
        {
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        #endregion


        #region Workorder Resources 

        [HttpPost]
        [Route("WorkOrderAssets")]
        public ActionResult AddWorkOrderAssets()
        {
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        [HttpDelete]
        [Route("DeleteWorkOrderAssets")]
        public ActionResult DeleteWorkOrderAssets()
        {
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        #endregion
    }
}