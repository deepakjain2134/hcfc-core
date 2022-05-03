using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Hcf.Api.Tms.AGH;
using Hcf.Api.Tms.Interface;
using Hcf.Api.Tms.MaintConnection;

namespace HCF.Web.Areas.Api.Controllers
{
    [Route("api/tms")]
    [ApiController]
    public class TMSConnectorController : ApiController
    {
        private readonly IAssetsService _assetService;
        private readonly IAssetTypeService _assetTypeService;
        private readonly IUserService _userService;
        private readonly IFloorService _floorService;
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly ISkillsService _skillsService;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly ICommonService _commonService;
        private readonly IEmailService _emailService;
        private readonly IApiCommon _apiCommon;
        private readonly IWorkOrderService _workOrderService;
        private readonly ISubStatusService _subStatusService;
        private readonly IBuildingsService _buildingsService;
        private readonly ITypeService _typeService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly IHCFSession _session;

        public TMSConnectorController(
            IWorkOrderService workOrderService,
            IAssetTypeService assetTypeService,
            ISubStatusService subStatusService,
            ICommonService commonService,
            IEmailService emailService,
            ITypeService typeService,
            IBuildingsService buildingsService,
            IApiCommon apiCommon,
            IStatusService statusService, IAssetsService assetService, IUserService userService, IFloorService floorService,
            IDepartmentService departmentService, ICategoryService categoryService, ISkillsService skillsService, IPriorityService priorityService,
            IFloorAssetService floorAssetService, IHCFSession session)
        {
            _buildingsService = buildingsService;
            _subStatusService = subStatusService;
            _workOrderService = workOrderService;
            _typeService = typeService;
            _assetTypeService = assetTypeService;
            _apiCommon = apiCommon;
            _commonService = commonService;
            _emailService = emailService;
            _statusService = statusService;
            _assetService = assetService;
            _userService = userService;
            _floorService = floorService;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _skillsService = skillsService;
            _priorityService = priorityService;
            _floorAssetService = floorAssetService;
            _session = session;
        }

        #region Work Order

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage TmsGetWorkOrder(string workOrderNumber)
        {
            //TMS.TMSHolyName tmsWorkOrderObj = new TMS.TMSHolyName();
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<TFloorAssets> floorAsset = new List<TFloorAssets>();
            //var wo = new HCF.BDO.WorkOrder();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    wo = tmsWorkOrderObj.GetWorkOrder(workOrderNumber);
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    wo = tmsWorkOrderObj.GetBurkeWorkOrder(Convert.ToInt32(workOrderNumber)).FirstOrDefault();
            //if (wo != null)
            //{
            //    if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //        floorAsset = tmsWorkOrderObj.GetAssetsByWorkOrder(wo.WorkOrderId.Value);
            //    else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //        floorAsset = tmsWorkOrderObj.GetBurkeAssetsByWorkOrder(wo.WorkOrderId.Value);
            //}
            //_objHttpResponseMessage.Result = new Result
            //{
            //    TFloorAssets = floorAsset,
            //    WorkOrder = wo,

            //};
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsWorkOrders()
        {
            //TMS.TMSHolyName tmsWorkOrderObj = new TMS.TMSHolyName();
            List<WorkOrder> workOrders = new List<WorkOrder>();
            var _objHttpResponseMessage = new HttpResponseMessage();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    workOrders = tmsWorkOrderObj.GetAllWorkOrders();
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    workOrders = tmsWorkOrderObj.BurkeGetAllWorkOrders();
            //foreach (WorkOrder wo in workOrders)
            //{
            //    if (wo.DateCompleted == DateTime.MinValue)
            //        wo.DateCompleted = null;
            //    wo.CreatedBy = 4;
            //    wo.DeficiencyCode = "OT";
            //    int issueId = _workOrderService.AddWorkOrder(wo);
            //    if (issueId > 0)
            //    {
            //        wo.IssueId = issueId;
            //        //WorkOrderRepository.UpdateWorkOrder(wo);
            //        //List<TFloorAssets> floorAssets = obj.GetAssetsByWorkOrder(wo.WorkOrderId.Value);
            //        //List<UserProfile> users = obj.GetResourceByWorkOrder(wo.WorkOrderId.Value);
            //        //string tinsStepsId = string.Join(",", floorAssets.Select(x => x.TmsReference).ToArray());
            //        //string resourceIds = string.Join(",", users.Select(x => x.ResourceId).ToArray());
            //        //if (!string.IsNullOrEmpty(tinsStepsId) || !string.IsNullOrEmpty(resourceIds))
            //        //    WorkOrderRepository.SaveWorkOrderResources(wo.IssueId, "", "", "", "", tinsStepsId, resourceIds);

            //    }
            //}
            //_objHttpResponseMessage.Result = new Result
            //{
            //    WorkOrders = workOrders

            //};
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// This method will return an array of work orders for the supplied Work Order Number. 
        /// Since the same Work Order Number can exist across multiple segments, 
        /// the returned array may contain more than one work order of the Work Order Number supplied. 
        /// Only the segments the given user within the Authentication Header has access to will be searched.
        /// </summary>
        /// <param name=woNumber></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage WorkOrderLoad(string clientName, string woNumber)
        {  
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<WorkOrder> workOrders = new List<WorkOrder>();
            //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    workOrders = objTms.GetWorkOrder(Convert.ToInt32(woNumber));
            //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    workOrders = objTms.GetBurkeWorkOrder(Convert.ToInt32(woNumber));
            //if (workOrders.Count > 0)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result
            //    {
            //        WorkOrders = workOrders
            //    };
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}

            return _objHttpResponseMessage;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("workordersave/{*floorAssetId}")]
        public HttpResponseMessage WorkOrderSave(WorkOrder workOrder, string floorAssetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var clientNo = 0;
            var offlineTempWorkOrderNo = workOrder.TempWorkOrderNumber;
            if (!string.IsNullOrEmpty(_session.ClientNo) && Convert.ToInt32(_session.ClientNo) > 0)
                clientNo = Convert.ToInt32(_session.ClientNo);
            _objHttpResponseMessage.Success = true;
            var org = _apiCommon.GetOrganization(clientNo);
            workOrder.DateCompleted = workOrder.DateCompletedDateTimeSpan > 0 ? Conversion.ConvertToDateTime(workOrder.DateCompletedDateTimeSpan) : workOrder.DateCompleted;
            workOrder.TargetDate = workOrder.TargetDateTimeSpan > 0 ? Conversion.ConvertToDateTime(workOrder.TargetDateTimeSpan) : workOrder.TargetDate;
            if (org != null)
            {
                if (org.IsTmsWo)
                {
                    _objHttpResponseMessage = CreateTMSWorkOrder(workOrder, org.DbName.ToLower());
                    if (_objHttpResponseMessage.Result != null)
                    {
                        if (_objHttpResponseMessage.Result.WorkOrder != null)
                        {
                            workOrder.WorkOrderNumber = _objHttpResponseMessage.Result.WorkOrder.WorkOrderNumber;
                            workOrder.WorkOrderId = _objHttpResponseMessage.Result.WorkOrder.WorkOrderId;
                            workOrder.SourceWoId = _objHttpResponseMessage.Result.WorkOrder.SourceWoId;
                        }
                    }
                }
                workOrder = CeateLocalWorkOrder(ref workOrder, floorAssetId);
                workOrder.TempWorkOrderNumber = offlineTempWorkOrderNo;
                _objHttpResponseMessage.Result = new Result
                {
                    WorkOrder = workOrder
                };
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("savetmsworkorder")]
        public HttpResponseMessage SaveTmsWorkOrder(WorkOrder workOrder)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var clientNo = 0;
            var offlineTempWorkOrderNo = workOrder.TempWorkOrderNumber;
            if (!string.IsNullOrEmpty(_session.ClientNo) && Convert.ToInt32(_session.ClientNo) > 0)
                clientNo = Convert.ToInt32(_session.ClientNo);
            _objHttpResponseMessage.Success = true;
            var org = _apiCommon.GetOrganization(clientNo);
            if (org != null)
            {
                var isExistingWorkOrder = _workOrderService.GetWorkOrderByWorkOrderNumber(workOrder.WorkOrderNumber);
                if (isExistingWorkOrder != null && isExistingWorkOrder.IssueId > 0)
                    workOrder.IssueId = isExistingWorkOrder.IssueId;

                workOrder = CeateLocalWorkOrder(ref workOrder, "");

                _objHttpResponseMessage.Result = new Result
                {
                    WorkOrder = workOrder
                };
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private HttpResponseMessage CreateTMSWorkOrder(WorkOrder workOrder, string clientName)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var obj = new TMS.TMSHolyName();          
            if (workOrder.IssueId > 0)
            {
                try
                {
                    //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
                    //    obj.UpdateWorkOrder(workOrder);
                    //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
                    //    obj.UpdateBurkeWorkOrder(workOrder);
                    if (clientName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
                    {
                        WorkOrderFactory wofa = new MaintConnection();
                        wofa.UpdateTmsWorkOrder(workOrder);
                    }
                    else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower())
                    {
                        WorkOrderFactory wofa = new AGHWorkOrders();
                        workOrder = wofa.UpdateWorkOrder(workOrder);
                    }
                    else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_NBIMC.ToString().ToLower())
                    {
                        workOrder = TMS.NBIH.WorkOrders.UpdateTmsWorkOrder(workOrder);
                    }
                    _objHttpResponseMessage.Success = true;
                    _objHttpResponseMessage.Result = new Result { WorkOrder = workOrder };
                }
                catch (Exception ex)
                {
                    //ErrorLog.LogError(ex);
                }
            }
            else
            {
                try
                {
                    //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
                    //    workOrder = obj.SaveTMSWorkOrder(workOrder);
                    //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
                    //    workOrder = obj.SaveBurkeTMSWorkOrder(workOrder);
                    if (clientName.ToLower() == BDO.Enums.ClientName.HCF_Atlantic.ToString().ToLower())
                    {
                        WorkOrderFactory wofa = new AGHWorkOrders();
                        if (workOrder.IssueId == 0 && (workOrder.WorkOrderId == null || workOrder.WorkOrderId == 0))
                        {
                            workOrder = wofa.SaveWorkOrder(workOrder);
                        }
                    }
                    else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
                    {
                        WorkOrderFactory woObject = new MaintConnection();
                        workOrder = woObject.SaveTmsWorkOrder(workOrder);
                    }
                    else if (clientName.ToLower() == BDO.Enums.ClientName.HCF_NBIMC.ToString().ToLower())
                    {
                        workOrder = TMS.NBIH.WorkOrders.SaveTmsWorkOrder(workOrder);
                    }

                    if (workOrder.WorkOrderId > 0)
                    {
                        _objHttpResponseMessage.Success = true;
                        _objHttpResponseMessage.Message = ConstMessage.Success;
                    }
                    else { _objHttpResponseMessage.Message = ConstMessage.TunnelDown; _objHttpResponseMessage.Success = true; }
                    _objHttpResponseMessage.Result = new Result { WorkOrder = workOrder };
                }
                catch (Exception ex)
                {
                    //ErrorLog.LogError(ex);
                }
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="floorAssetId"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private WorkOrder CeateLocalWorkOrder(ref WorkOrder workOrder, string floorAssetId)
        {
            var issueId = 0;
            if (workOrder.IssueId > 0)
            {
                issueId = _workOrderService.UpdateWorkOrder(workOrder);
                UpdateWOResource(workOrder, issueId);
                if (issueId > 0)
                {
                    workOrder = _workOrderService.GetWorkOrder(issueId);
                    if (workOrder.StatusCode == BDO.Enums.StatusCode.CANCL.ToString() || workOrder.StatusCode == BDO.Enums.StatusCode.CMPLT.ToString())
                    {
                        if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.WorkOrder.ToString(), BDO.Enums.NotificationEvent.OnClosed.ToString()))
                        {
                            _emailService.SendWorkOrderClosedMail(workOrder, Convert.ToInt32(BDO.Enums.NotificationCategory.WorkOrder));
                        }
                    }
                }
            }
            else
            {
                try
                {
                    issueId = _workOrderService.AddWorkOrder(workOrder);
                    UpdateWOResource(workOrder, issueId);
                    if (issueId > 0)
                    {
                        workOrder = _workOrderService.GetWorkOrder(issueId);
                        if (workOrder.StatusCode == BDO.Enums.StatusCode.ACTIV.ToString())
                        {
                            if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.WorkOrder.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString()))
                            {
                                _emailService.SendWorkOrderCreatedMail(workOrder, Convert.ToInt32(BDO.Enums.NotificationCategory.WorkOrder));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ErrorLog.LogError(ex);
                }
            }
            return workOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="issueId"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void UpdateWOResource(WorkOrder workOrder, int issueId)
        {
            if (issueId > 0)
            {
                string userIds = "";
                string floorAssetsIds = "";
                string tinsStepsId = "";
                string activityId = "";
                string tmsRefrenceIds = "";
                if (workOrder.Resources != null)
                {
                    userIds = string.Join(",", workOrder.Resources.Select(x => x.UserId).ToArray());
                }
                if (workOrder.WorkOrderFloorAssets != null)
                {
                    //workOrder.WorkOrderFloorAssets = workOrder.WorkOrderFloorAssets.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                    activityId = string.Join(",", workOrder.WorkOrderFloorAssets.Select(x => x.ActivityId).ToArray());
                }

                if (workOrder.WorkOrderFloorAssets != null && workOrder.WorkOrderFloorAssets.Any(x => x.TmsAssetId.HasValue))
                {
                    tmsRefrenceIds = string.Join(",", workOrder.WorkOrderFloorAssets.Where(x => x.TmsAssetId.HasValue).Select(x => x.TmsAssetId.Value).ToArray());
                }

                if (workOrder.InspectionSteps != null)
                {
                    tinsStepsId = string.Join(",", workOrder.InspectionSteps.Select(x => x.TInsStepsId).ToArray());
                }
                if (!string.IsNullOrEmpty(userIds) || !string.IsNullOrEmpty(activityId) || !string.IsNullOrEmpty(tinsStepsId) || !string.IsNullOrEmpty(tmsRefrenceIds))
                {
                    _workOrderService.SaveWorkOrderResources(issueId, userIds, floorAssetsIds, tinsStepsId, activityId, tmsRefrenceIds, "");
                }
                if (workOrder.TRoundWorkOrders != null && workOrder.TRoundWorkOrders.Count > 0)
                {
                    foreach (TRoundWorkOrders item in workOrder.TRoundWorkOrders)
                    {
                        item.IssueId = issueId;
                        _workOrderService.SaveTRounWorkOrder(item);
                    }
                }
            }
        }

        /// <summary>
        /// Get Work Order by Status
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage WorkOrderByStatus(string statusCode)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<WorkOrder> workOrders = objTms.GetWorkOrders(statusCode);
            //if (workOrders.Count > 0)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result
            //    {
            //        WorkOrders = workOrders
            //    };
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            //}

            return _objHttpResponseMessage;
        }

        /// <summary>
        /// Get Work Order by Status
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage WorkOrderFloorAssets(string workOrderId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<TFloorAssets> floorAssets = objTms.WorkOrderAssets(Convert.ToInt32(workOrderId));
            //if (floorAssets.Count > 0)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result
            //    {
            //        TFloorAssets = floorAssets
            //    };
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}

            return _objHttpResponseMessage;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmsAssetId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetWorkOrders(string tmsAssetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<WorkOrder> workOrders = objTms.GetAssetWorkOrders(Convert.ToInt32(tmsAssetId));
            //if (workOrders.Count > 0)
            //{
            //    _objHttpResponseMessage.Success = true;
            //    _objHttpResponseMessage.Result = new Result
            //    {
            //        WorkOrders = workOrders
            //    };
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetDowntime(string tmsAssetId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<TFloorAssets> floorAssets = new List<TFloorAssets>();
            //floorAssets = _floorAssetService.GetTFloorAssets().Where(x => !string.IsNullOrWhiteSpace(x.TmsReference) && !string.IsNullOrWhiteSpace(x.TmsReference)).ToList();
            //foreach (var downtime in floorAssets.Select(item => objTms.GetAssetDowntime(Convert.ToInt32(item.TmsReference))))
            //{
            //    if (downtime.Count > 0)
            //    {
            //        _objHttpResponseMessage.Success = true;
            //        _objHttpResponseMessage.Result = new Result
            //        {
            //            DownTimes = downtime
            //        };
            //    }
            //    else
            //    {
            //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //    }
            //}
            return _objHttpResponseMessage;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWorkOrderAssignement(string workOrderId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName objTms = new TMS.TMSHolyName();
            //List<UserProfile> users = objTms.GetResourceByWorkOrder(Convert.ToInt32(workOrderId));
            //if (users.Count > 0)
            //{

            //}
            return _objHttpResponseMessage;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayCount"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWorkOrdersByCauseCode(string dayCount)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            string clientName = _session.ClientDbName.ToLower();
            UpdateWorkOrders(clientName);
            int days = Convert.ToInt32(dayCount);

            var workOrders = AddTmsWorkOrders(days, clientName);
            _objHttpResponseMessage.Result = new Result { WorkOrders = workOrders };
            return _objHttpResponseMessage;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayCount"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetWorkOrdersByCauseCodeCount(string dayCount)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            string clientName = _session.ClientDbName.ToLower();
            int days = Convert.ToInt32(dayCount);
            var workOrders = GetTmsWorkOrders(days, clientName).Count;
            _objHttpResponseMessage.PageCount = workOrders;
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateWorkOrders(string clientName)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            try
            {
                var dbWorkOrders = _workOrderService.GetWorkOrders();
                List<string> workOrder = dbWorkOrders
                    .Where(x => x.StatusCode != "CMPLT" && x.StatusCode != "CANCL").Select(x => x.WorkOrderNumber).ToList();

                List<WorkOrder> workOrders = new List<WorkOrder>();
                if (workOrder.Count > 0)
                {
                    //TMS.TMSHolyName obj = new TMS.TMSHolyName();
                    //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
                    //    workOrders = obj.UpdateWorkOrders(workOrder);
                    //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
                    //    workOrders = obj.UpdateBurkeWorkOrders(workOrder);
                    //foreach (WorkOrder wo in workOrders)
                    //{
                    //    wo.IssueId = dbWorkOrders.Where(x => x.WorkOrderNumber == wo.WorkOrderNumber).FirstOrDefault().IssueId;
                    //    if (wo.IssueId > 0)
                    //    {
                    //        _workOrderService.UpdateWorkOrder(wo);

                    //        List<TFloorAssets> floorAssets = obj.GetAssetsByWorkOrder(wo.WorkOrderId.Value);
                    //        List<UserProfile> users = obj.GetResourceByWorkOrder(wo.WorkOrderId.Value);
                    //        string tinsStepsId = string.Join(",", floorAssets.Select(x => x.TmsReference).ToArray());
                    //        string resourceIds = string.Join(",", users.Select(x => x.ResourceId).ToArray());

                    //        if (!string.IsNullOrEmpty(tinsStepsId) || !string.IsNullOrEmpty(resourceIds))
                    //            _workOrderService.SaveWorkOrderResources(wo.IssueId, "", "", "", "", tinsStepsId, resourceIds);
                    //    }

                    //}
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            _objHttpResponseMessage = new HttpResponseMessage();
            return _objHttpResponseMessage;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private List<WorkOrder> AddTmsWorkOrders(int day, string clientName)
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            List<WorkOrder> workOrders = new List<WorkOrder>();
            //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    workOrders = obj.GetWorkOrdersByCauseCode(day).OrderBy(x => x.DateCreated).ToList();
            //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    workOrders = obj.GetBurkeWorkOrdersByCauseCode(day).OrderBy(x => x.DateCreated).ToList();
            if (clientName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
            {
                WorkOrderFactory woObject = new MaintConnection();
                string lastupdateDate = DateTime.Now.AddDays(-day).ToString("yyyy-MM-dd");
                string filterexp = $"&$filter=LastModifiedDate%20ge%20\"{lastupdateDate}\"";
                workOrders = woObject.GetWorkOrderByQuery(filterexp);
            }
            foreach (WorkOrder wo in workOrders)
            {
                if (wo.DateCompleted == DateTime.MinValue)
                    wo.DateCompleted = null;
                wo.CreatedBy = 4;
                wo.DeficiencyCode = "OT";
                int issueId = _workOrderService.AddWorkOrder(wo);
                if (issueId > 0)
                {
                    wo.IssueId = issueId;
                    _workOrderService.UpdateWorkOrder(wo);
                    List<TFloorAssets> floorAssets = GetAssetsByWorkOrder(wo.WorkOrderId.Value);
                    List<UserProfile> users = GetResourceByWorkOrder(wo.WorkOrderId.Value);
                    string tinsStepsId = string.Join(",", floorAssets.Select(x => x.TmsReference).ToArray());
                    string resourceIds = string.Join(",", users.Select(x => x.ResourceId).ToArray());
                    if (!string.IsNullOrEmpty(tinsStepsId) || !string.IsNullOrEmpty(resourceIds))
                        _workOrderService.SaveWorkOrderResources(wo.IssueId, "", "", "", "", tinsStepsId, resourceIds);
                }
            }
            return workOrders;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private static List<WorkOrder> GetTmsWorkOrders(int day, string clientName)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();           
            //if (clientName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //{
            //    workOrders = obj.GetWorkOrdersByCauseCode(day).OrderBy(x => x.DateCreated).ToList();
            //}
            //else if (clientName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    workOrders = obj.GetBurkeWorkOrdersByCauseCode(day).OrderBy(x => x.DateCreated).ToList();

            return workOrders;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private List<UserProfile> GetResourceByWorkOrder(int WorkOrderId)
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            List<UserProfile> users = new List<UserProfile>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    users = obj.GetResourceByWorkOrder(WorkOrderId);
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    users = obj.GetBurkeResourceByWorkOrder(WorkOrderId);
            if (_session.ClientDbName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
            {
                ResourceFactory UserObject = new Resources();
                users = UserObject.GetResourceByWorkOrder(WorkOrderId);
            }
            return users;
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private List<TFloorAssets> GetAssetsByWorkOrder(int WorkOrderId)
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            List<TFloorAssets> floorAssets = new List<TFloorAssets>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    floorAssets = obj.GetAssetsByWorkOrder(WorkOrderId);
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    floorAssets = obj.GetBurkeAssetsByWorkOrder(WorkOrderId);
            if (_session.ClientDbName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
            {
                AssetsFactory objwotask = new WorkOrderTask();
                floorAssets = objwotask.GetAssetsByWorkOrder(WorkOrderId);
            }
            return floorAssets;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("workorderfiles")]
        public HttpResponseMessage WorkOrderfiles(WorkOrderfiles workOrderfiles)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            if (!string.IsNullOrEmpty(workOrderfiles.FileContent))
            {
                workOrderfiles.IssueId = workOrderfiles.IssueId;
                Files newFile = _apiCommon.SaveFile(workOrderfiles.FileContent, workOrderfiles.FileName, "DeficiencyCloseFile", Convert.ToString(workOrderfiles.IssueId));
                workOrderfiles.FileName = newFile.FileName;
                workOrderfiles.FilePath = newFile.FilePath;
                workOrderfiles.WorkOrderFileId = _workOrderService.SaveWorkOrderFiles(workOrderfiles.IssueId, workOrderfiles);
            }


            if (workOrderfiles.WorkOrderFileId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    WorkOrderfiles = workOrderfiles
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;
        }

        #endregion

        #region resource

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetTmsResource/{*days}")]
        public HttpResponseMessage GetTmsResource(string days)
        {
            int day = 0;
            if (!string.IsNullOrEmpty(days))
                day = Convert.ToInt32(days);
            int clientNo = 0;
            List<UserProfile> users = new List<UserProfile>();
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(_session.ClientNo) && Convert.ToInt32(_session.ClientNo) > 0)
                clientNo = Convert.ToInt32(_session.ClientNo);
            Organization org = _apiCommon.GetOrganization(clientNo);
            if (org != null)
            {
                if (org.IsTmsWo)
                {
                    //if (org.DbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
                    //    users = TMS.Resource.GetResources(day);
                    //else if (org.DbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
                    //    users = TMS.Resource.GetBurkeResources(day);
                    if (org.DbName.ToLower() == BDO.Enums.ClientName.HCF_ACH.ToString().ToLower())
                    {
                        ResourceFactory UserObject = new Resources();
                        users = UserObject.GetResource();
                    }
                    else
                        users = _userService.GetUsers().Where(x => x.ResourceNumber != null).ToList();
                    //foreach (UserProfile userProfile in users)
                    //{
                    //    Users.AddResource(userProfile);
                    //}
                    users = _userService.GetUsers().Where(x => x.ResourceNumber != null).ToList();
                }
                else
                    users = _userService.GetUsers();
            }
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                Users = users//users.Where(x => x.IsActive).ToList()
            };
            return _objHttpResponseMessage;
        }

        #endregion

        #region Department
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsDepartment()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var depts = TMS.Department.GetDepartments();
            //_objHttpResponseMessage.Result = new Result
            //{
            //    Departments = depts
            //};
            return _objHttpResponseMessage;
        }

        #endregion

        #region Assets

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsAssets(string days)
        {
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            int day = 0;

            if (!string.IsNullOrEmpty(days))
                day = Convert.ToInt32(days);

            List<AssetType> assetsTypes = new List<AssetType>();
            var _objHttpResponseMessage = new HttpResponseMessage();

            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    assetsTypes = TMS.TMSAssets.GetTmsAssets(day);
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    assetsTypes = TMS.TMSAssets.GetBurkeTmsAssets(day);
            //else if (HcfSession.ClientDBName.ToLower() == Enums.ClientName.HCF_ACH.ToString().ToLower())
            //    assetsTypes = ;
            int userId = 4;
            foreach (AssetType assettype in assetsTypes)
            {
                assettype.CreatedBy = userId;
                assettype.IsActive = true;
                int assetTypeId = _assetTypeService.Save(assettype);
                if (assetTypeId > 0)
                {
                    foreach (Assets asset in assettype.Assets)
                    {
                        asset.AssetTypeId = assetTypeId;
                        asset.CreatedBy = userId;
                        int assetId = _assetService.AddAssets(asset);
                        if (assetId > 0)
                        {
                            foreach (TFloorAssets tfloorAssets in asset.TFloorAssets)
                            {
                                tfloorAssets.AssetId = assetId;
                                tfloorAssets.CreatedBy = userId;
                                tfloorAssets.FloorAssetsId = _floorAssetService.AddFloorAssets(tfloorAssets).FloorAssetsId;

                                //if (tfloorAssets.FloorAssetsId > 0)
                                //{
                                //    try
                                //    {
                                //        var workOrder = obj.GetAssetWorkOrders(Convert.ToInt32(tfloorAssets.TmsReference)).OrderByDescending(x => x.DateCreated).FirstOrDefault();
                                //        if (workOrder != null)
                                //        {
                                //            TEPInspectionRepository.SaveAssetInspectionDate(tfloorAssets.FloorAssetsId, workOrder.DateCreated.Value);
                                //        }
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        ErrorLog.LogError(ex);

                                //    }
                                //}

                            }
                        }
                    }
                }
            }

            if (assetsTypes.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result { AssetType = assetsTypes };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// get assets last workorder from tms and save Pm date 
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetsWorkOrders()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //TMS.TMSHolyName obj = new TMS.TMSHolyName();
            //List<WorkOrder> workOrders = new List<WorkOrder>();         

            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //{
            //    List<TFloorAssets> floorAssets = new List<TFloorAssets>();
            //    floorAssets = _floorAssetService.GetTFloorAssets().Where(x => !string.IsNullOrWhiteSpace(x.TmsReference) && !string.IsNullOrWhiteSpace(x.TmsReference)).ToList();
            //    foreach (var item in floorAssets)
            //    {
            //        if (!string.IsNullOrEmpty(item.TmsReference))
            //        {
            //            var currentWo = TMS.TMSAssets.GetBurkeAssetsWO(Convert.ToInt32(item.TmsReference)).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            //            if (currentWo != null)
            //            {
            //                _floorAssetService.UpdateInspectionDate(item.FloorAssetsId, currentWo.DateCreated.Value, currentWo.WorkOrderNumber);
            //            }
            //        }
            //    }
            //}
            //if (workOrders.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { WorkOrders = workOrders };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;

            return _objHttpResponseMessage;
        }



        #endregion

        #region Accounts

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsAccounts()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Department> departments = new List<Department>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    departments = TMS.Account.GetTmsAccount();
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    departments = TMS.Account.GetBrukeTmsAccount();
            //foreach (Department item in departments)
            //{
            //    _departmentService.Save(item);
            //}
            //if (departments.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Departments = departments };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Buildings/Floors


        //public HttpResponseMessage GetTmsAssetsBuildings()
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var lists = TMS.TMSAssets.GetTmsBuildings();
        //    if (lists.Count > 0)
        //    {
        //        objHttpResponseMessage.Result = new Result { Buildings = lists };
        //        objHttpResponseMessage.Success = true;
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return objHttpResponseMessage;

        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsBuildings()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Buildings> lists = new List<Buildings>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    lists = TMS.Buildings.GetTmsBuildings();
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    lists = TMS.Buildings.GetBurkeTmsBuildings();
            //foreach (Buildings building in lists)
            //{
            //    building.CreatedBy = 4;
            //    int buildingId = _buildingsService.Save(building);
            //    if (buildingId > 0)
            //    {
            //        foreach (Floor floor in building.Floor)
            //        {
            //            floor.BuildingId = buildingId;
            //            floor.CreatedBy = 4;
            //            _floorService.Save(floor);
            //        }
            //    }
            //}

            //if (lists.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Buildings = lists };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Sites

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsSites()
        {

            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Site> sites = new List<Site>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    sites = TMS.Site.GetTmsSites();
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    sites = TMS.Site.GetBurkeTmsSites();
            //foreach (Site item in sites)
            //{
            //    //SiteRepository.Save(item);
            //}
            //if (sites.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Sites = sites };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Priority
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsPriority(string modulecode = "WO")
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Priority> priorities = new List<Priority>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    priorities = TMS.Priority.GetTmsPriorities(modulecode.ToUpper());
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    priorities = TMS.Priority.GetBrukeTmsPriorities(modulecode.ToUpper());
            //foreach (Priority item in priorities)
            //{
            //    _priorityService.Save(item);
            //}
            //if (priorities.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Priorities = priorities };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Category/SubCategory

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsCategory(string modulecode = "WO")
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //var categories = TMS.Category.GetTmsCategory(modulecode.ToUpper());
            //foreach (Category item in categories)
            //{
            //    int categoryId = _categoryService.Save(item);
            //    if (categoryId > 0)
            //    {
            //        //foreach (SubCategory subCategory in item.SubCategorys)
            //        //{
            //        //    subCategory.CategoryId = categoryId;
            //        //    HCF.BAL.SubCategoryRepository.Save(subCategory);
            //        //}
            //    }
            //}
            //if (categories.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Categories = categories };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Status/SubStatus

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsStatus(string modulecode = "WO")
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<WoStatus> statuses = new List<WoStatus>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    statuses = TMS.Status.GetTmsStatus(modulecode.ToUpper());
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    statuses = TMS.Status.GetBrukeTmsStatus(modulecode.ToUpper());
            //foreach (WoStatus item in statuses)
            //{
            //    int statusId = _statusService.Save(item);
            //    if (statusId > 0)
            //    {
            //        foreach (SubStatus subStatus in item.SubStatus)
            //        {
            //            subStatus.WoStatusId = statusId;
            //            _subStatusService.Save(subStatus);
            //        }
            //    }
            //}
            //if (statuses.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { TMSStatus = statuses };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Type

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsType(string modulecode = "WO")
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Types> types = new List<Types>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    types = TMS.Type.GetTmsTypes(modulecode.ToUpper());
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    types = TMS.Type.GetBrukeTmsTypes(modulecode.ToUpper());
            //foreach (var item in types)
            //{
            //    _typeService.Save(item);
            //}
            //if (types.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Types = types };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }

        #endregion

        #region Skill

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetTmsSkill()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //List<Skills> skills = new List<Skills>();
            //if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Holy.ToString().ToLower())
            //    skills = TMS.Skill.GetTmsSkill();
            //else if (_session.ClientDbName.ToLower() == Enums.ClientName.HCF_Burke.ToString().ToLower())
            //    skills = TMS.Skill.GetBrukeTmsSkill();
            //foreach (var item in skills)
            //{
            //    _skillsService.Save(item);
            //}
            //if (skills.Count > 0)
            //{
            //    _objHttpResponseMessage.Result = new Result { Skills = skills };
            //    _objHttpResponseMessage.Success = true;
            //}
            //else
            //{
            //    _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            //}
            return _objHttpResponseMessage;
        }


        #endregion

        #region Requester

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRequester()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //_objHttpResponseMessage = new HttpResponseMessage { Result = new Result { Users = TMS.Requester.Query() } };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage LoadRequester(string name)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            //_objHttpResponseMessage = new HttpResponseMessage
            //{
            //    Result = new Result
            //    {
            //        User = TMS.Requester.LoadRequester(name)
            //    }
            //};
            return _objHttpResponseMessage;
        }

        #endregion
    }
}