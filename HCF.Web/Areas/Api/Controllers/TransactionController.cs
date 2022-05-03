using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
    
    [Route("api/trans")]
    [ApiController]
    public class TransactionApiController : ApiController
    {
        #region ctor
        private readonly IIlsmService _ilsmService;
        private readonly ILoggingService _loggingService;
        private readonly ITransactionService _transactionService;
        private readonly IStandardService _standardService;
        private readonly IEmailService _emailService;
        private readonly IScheduleService _scheduleService;

        private readonly ICommonService _commonService;
        private readonly IInspectionService _inspectionService;
        private readonly IApiCommon _apiCommon;
        private readonly IInsStepsService _insStepsService;
        private readonly IInsActivityService _insActivityService;
        private readonly IRoundInspectionsService _roundInspectionsService;
        private readonly IWorkOrderService _workOrderService;
        private readonly ITEPInspectionService _tEPInspectionService;
        private readonly IEpService _epService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly IPdfService _pdfService;


        public TransactionApiController(
            IPdfService pdfService,
            IScheduleService scheduleService,
            IWorkOrderService workOrderService,
            IRoundInspectionsService roundInspectionsService,
            IInsActivityService insActivityService,
            IInsStepsService insStepsService,
            IApiCommon apiCommon,
            IIlsmService ilsmService,
            ILoggingService loggingService,
            ITransactionService transactionService,
            IStandardService standardService, // no reference
            IEmailService emailService,
            ICommonService commonService,
            IInspectionService inspectionService,
            ITEPInspectionService tEPInspectionService,// no reference
            IEpService epService, // no reference
            IFloorAssetService floorAssetService, // no reference
            IGoalMasterService goalMasterService // no reference

            )
        {
            _pdfService = pdfService;
            _scheduleService = scheduleService;
            _epService = epService;
            _tEPInspectionService = tEPInspectionService;
            _workOrderService = workOrderService;
            _roundInspectionsService = roundInspectionsService;
            _insActivityService = insActivityService;
            _insStepsService = insStepsService;
            _apiCommon = apiCommon;
            _commonService = commonService;
            _inspectionService = inspectionService;
            _emailService = emailService;
            _ilsmService = ilsmService;
            _loggingService = loggingService;
            _transactionService = transactionService;
            _standardService = standardService;
            _floorAssetService = floorAssetService;
            _goalMasterService = goalMasterService;
        }

        #endregion

        #region   TInspection

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetInspections()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var inspections = _transactionService.GetAllInspections();
            if (inspections.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Inspections = inspections };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Make Pass on Floor 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("SaveWoInspection")]
        public ActionResult SaveWoInspection(List<MarkPassAssets> assetsList)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<MarkPassAssets> newAssetsList = new List<MarkPassAssets>();
            if (assetsList != null && assetsList.Count > 0)
            {
                var frequencyId = (int)assetsList.FirstOrDefault().Frequency;
                foreach (var item in assetsList)
                {
                    newAssetsList = new List<MarkPassAssets>();
                    try
                    {
                        var tFloorAssets = _floorAssetService.GetFloorAssetByTmsAssetId(item.TmsAsssetId);
                        if (tFloorAssets != null && tFloorAssets.FloorAssetsId > 0)
                        {
                            var isWorkOrderExits = tFloorAssets.WorkOrders.FirstOrDefault(x => x.WorkOrderId == assetsList.FirstOrDefault().WorkOrderId);
                            if (isWorkOrderExits != null)
                            {
                                foreach (var eps in tFloorAssets.Assets.EPdetails.Where(x => x.FrequencyId == frequencyId))
                                {
                                    if (isWorkOrderExits.TInspectionActivity == null)
                                    {
                                        item.FloorAssetId = tFloorAssets.FloorAssetsId;
                                        item.EPDetailId = eps.EPDetailId;
                                        item.AssetsId = tFloorAssets.AssetId.Value;
                                        newAssetsList.Add(item);
                                        _objHttpResponseMessage = SaveMarkPassAssets(assetsList, Convert.ToString(1));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _loggingService.Error(ex);
                    }
                }
            }

            return Ok(_objHttpResponseMessage);

        }

        /// <summary>
        /// </summary>
        /// <param name="assetsList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("MarkPassAssets/{userId}")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult MarkPassAssets(List<MarkPassAssets> assetsList, string userId)
        {
            HttpResponseMessage _objHttpResponseMessage = SaveMarkPassAssets(assetsList, userId);
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        private HttpResponseMessage SaveMarkPassAssets(List<MarkPassAssets> assetsList, string userId)
        {
            var mode = "ASSET";
            var modeType = Convert.ToInt32(BDO.Enums.Mode.ASSET);
            var tFiles = new List<TFiles>();

            WorkOrder wo = new WorkOrder();
            if (assetsList.FirstOrDefault()?.WorkOrderId > 0)
                wo.WorkOrderId = assetsList.FirstOrDefault()?.WorkOrderId;

            if (!string.IsNullOrEmpty(assetsList.FirstOrDefault()?.AttachFiles))
                tFiles = _apiCommon.GetTFiles(assetsList.FirstOrDefault()?.AttachFiles).ToList();

            string comment = assetsList.FirstOrDefault()?.Comment;
            var newInspectionId = 0; var createdBy = Convert.ToInt32(userId);
            List<EPSteps> epDocs = new List<EPSteps>();
            if (assetsList != null && assetsList.Count > 0)
            {
                var assets = assetsList.FirstOrDefault();
                epDocs = _transactionService.GetGoalStepsByActivity(assets.FloorAssetId, assets.AssetsId, assets.EPDetailId, assets.InspectionId, modeType, 0);

                var inspection = new Inspection
                {
                    InspectionId = assets.InspectionId,
                    EPDetailId = assets.EPDetailId,
                    DocStatus = _epService.DocStatus(assets.EPDetailId),
                    InspectionGroupId = assets.InspectionGroupId,
                    IsPreviousCycle = assets.InspectionId>0?1:0,//assets.IsPreviousCycle,
                    TCycleId = assets.TCycleId
                };
                newInspectionId = _transactionService.AddInspection(inspection);
            }

            var tinsDocId = 0;
            Guid _Activityid = Guid.Empty;
            if (!epDocs.Find(x => x.EPDetailId == assetsList.FirstOrDefault().EPDetailId).IsDocRequired)
            {
                foreach (var item in tFiles)
                {
                    var epAssetDocs = new InspectionEPDocs
                    {
                        ActivityType = 2,
                        ActivityId = _apiCommon.GetActivityId(),
                        Path = item.FilePath,
                        DocumentName = item.FileName,
                        CreatedBy = createdBy,
                        DocTypeId = Convert.ToInt32(BDO.Enums.UploadDocTypes.AssetsReport),
                        FileId = item.TFileId,
                        InspectionId = newInspectionId,
                        UploadDocTypeId = Convert.ToInt32(BDO.Enums.UploadDocTypes.AssetsReport)
                    };
                    _Activityid = epAssetDocs.ActivityId;
                    tinsDocId = _transactionService.AddInspectionDocs(epAssetDocs);
                }
            }


            if (newInspectionId > 0)
            {
                List<TInspectionActivity> activities = new List<TInspectionActivity>();
                foreach (var assets in assetsList)
                {

                    var inspectionActivity = new TInspectionActivity
                    {
                        ActivityType = CommonUtility.SetActivityType(mode),
                        ActivityId = _Activityid == Guid.Empty ? _apiCommon.GetActivityId() : _Activityid,// GetActivityId(),
                        CreatedBy = createdBy,
                        InspectionId = newInspectionId,
                        Status = 1,
                        ActivityInspectionDate = assets.InspectionDateTimeSpan == 0 ? assets.InspectionDate : Conversion.ConvertToDateTime(assets.InspectionDateTimeSpan),
                        IsDeficiency = false,
                        FloorAssetId = assets.FloorAssetId,
                        SubStatus = BDO.Enums.InspectionSubStatus.NA.ToString(),
                        Comment = comment,
                        EPDetailId = assets.EPDetailId,
                        DtEffectiveDate = DateTime.UtcNow,
                        TinsDocId = tinsDocId,
                    };
                    inspectionActivity.WorkOrders = new List<WorkOrder>
                    {
                            wo
                    };

                    var activityId = _transactionService.AddTInspectionActivity(inspectionActivity);
                    _Activityid = Guid.Empty;
                    inspectionActivity.TInsActivityId = activityId;
                    if (activityId > 0 && epDocs != null && epDocs.Count > 0)
                    {
                        foreach (var goal in epDocs[0].MainGoal)
                        {
                            var detail = new TInspectionDetail
                            {
                                MainGoalId = goal.MainGoalId,
                                ActivityId = inspectionActivity.ActivityId
                            };
                            var lists = goal.Steps.Select(x => x.StepsId).ToList();
                            var stepsId = string.Join(",", lists);
                            _transactionService.AddInspectionDetails(detail, stepsId);
                        }
                    }

                    activities.Add(inspectionActivity);

                }
            }



            // add assets reports if all assets done and get latest status of EP.
            // add assets reports if all assets done 
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = true,
                Message = ConstMessage.Inspection_Added_Successfully,
                Result = new Result
                {
                    EPDocument = _goalMasterService.GetEpTransInfo(assetsList.FirstOrDefault().EPDetailId, assetsList.FirstOrDefault().InspectionId, assetsList.FirstOrDefault().InspectionGroupId)

                }
            };
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="assetsList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UploadFloorAssetsInsReport/{userId}")]
        public ActionResult UploadFloorAssetsInspectionReport(List<MarkPassAssets> assetsList, string userId)
        {
            var mode = "ASSET";

            string filePath = string.Empty;
            string fileName = string.Empty;

            if (!string.IsNullOrEmpty(assetsList.FirstOrDefault()?.FileName))
            {

                fileName = assetsList.FirstOrDefault()?.FileName;
                if (!string.IsNullOrEmpty(assetsList.FirstOrDefault()?.FilePath))
                    filePath = assetsList.FirstOrDefault()?.FilePath;
                else if (!string.IsNullOrEmpty(assetsList.FirstOrDefault()?.FileContent))
                    filePath = _apiCommon.SaveFile(assetsList.FirstOrDefault()?.FileContent, assetsList.FirstOrDefault()?.FileName, "EPDocs", "CommonFiles").FilePath;

            }
            var comment = assetsList.FirstOrDefault()?.Comment;

            foreach (var assets in assetsList)
            {
                var createdBy = Convert.ToInt32(userId);
                var inspection = new Inspection
                {
                    InspectionId = assets.InspectionId,
                    EPDetailId = assets.EPDetailId,
                    DocStatus = _epService.DocStatus(assets.EPDetailId),
                    InspectionGroupId = assets.InspectionGroupId
                };
                var newInspectionId = _transactionService.AddInspection(inspection);
                if (newInspectionId > 0)
                {
                    var inspectionActivity = new TInspectionActivity
                    {
                        ActivityType = CommonUtility.SetActivityType(mode),
                        ActivityId = _apiCommon.GetActivityId(),
                        CreatedBy = createdBy,
                        InspectionId = newInspectionId,
                        Status = 1,
                        ActivityInspectionDate = assets.InspectionDateTimeSpan == 0 ? assets.InspectionDate : Conversion.ConvertToDateTime(assets.InspectionDateTimeSpan),
                        IsDeficiency = false,
                        FloorAssetId = assets.FloorAssetId,
                        SubStatus = BDO.Enums.InspectionSubStatus.NA.ToString(),
                        Comment = comment,
                        InsType = 0,
                        DtEffectiveDate = DateTime.UtcNow
                    };
                    var activityId = _transactionService.AddTInspectionActivity(inspectionActivity);

                    if (activityId > 0 && !string.IsNullOrEmpty(filePath))
                    {
                        var epAssetDocs = new InspectionEPDocs
                        {
                            ActivityType = (int)BDO.Enums.Mode.ASSET,
                            ActivityId = inspectionActivity.ActivityId,
                            Path = filePath,
                            DocumentName = fileName,
                            FileId = assets.FileId,
                            CreatedBy = createdBy,
                            DocTypeId = (int)BDO.Enums.UploadDocTypes.AssetsReport,
                            UploadDocTypeId = (int)BDO.Enums.UploadDocTypes.AssetsReport
                        };
                        _transactionService.AddInspectionDocs(epAssetDocs);
                    }
                }
            }

            var _objHttpResponseMessage = new HttpResponseMessage { Success = true, Message = ConstMessage.Inspection_Added_Successfully, Result = new Result { EPDocument = _goalMasterService.GetEpTransInfo(assetsList.FirstOrDefault().EPDetailId, assetsList.FirstOrDefault().InspectionId, assetsList.FirstOrDefault().InspectionGroupId) } };
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Activity

        /// <summary>
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetActivityHistory(string activityId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var assets = _transactionService.GetActivityHistory(new Guid(activityId));
            if (assets != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Inspection = assets };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetInspectionsforWorkOrder(string activityId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var inspections = _transactionService.GetInspectionsforWorkOrder(activityId);
            if (inspections != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Inspections = inspections };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("Updatetrsetp/{userId}")]
        public ActionResult UpdateTrSteps(InspectionSteps steps, string userId)
        {
            int createdBy = Convert.ToInt32(userId);
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (steps.TInsStepsId > 0)
            {
                if (steps.DeficiencyStatus == BDO.Enums.StatusCode.CMPLT.ToString())
                {
                    _objHttpResponseMessage.Success = _insStepsService.Update(steps, createdBy);
                }
                else
                {
                    _objHttpResponseMessage.Message = ConstMessage.Workorder_Open_Against_Steps;
                }
            }
            else { _objHttpResponseMessage.Success = _insStepsService.Update(steps, createdBy); }

            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Inspection

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private int RaScore(TInspectionActivity activity)
        {
            int raTotal;
            raTotal = activity.TInspectionDetail
                .SelectMany(x => x.InspectionSteps.Where(y => y.Status == 0 && y.DRTime == 0 && y.IsRA)).Sum(y => y.RAScore.Value);

            return raTotal;
        }

        /// <summary>
        /// New Method
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="mode"></param>
        /// <param name="isComplete"></param>
        /// <param name="multipleEpDetailId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddInspection/")]
        public ActionResult AddInspection(Inspection newItem, string mode, bool isComplete, string multipleEpDetailId = null)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var httpResponseResult = new HttpResponseMessage();
            mode = mode.ToUpper();

            var totalEpDetailId = string.Join(",", newItem.EPDetailId, multipleEpDetailId);

            var activityType = CommonUtility.SetActivityType(mode);
            if (activityType != 2)
            {
                var epStatus = SetActivityStatus(newItem.TInspectionActivity.FirstOrDefault(x => x.ActivityType == 1), isComplete);
                newItem.Status = (epStatus == -1) ? (int?)null : epStatus;
            }


            var inspections = CheckReInspection(totalEpDetailId, newItem.InspectionGroupId, newItem.IsAllDocumentUploaded, newItem.Status, mode, newItem);
            foreach (var TinspectionCampus in newItem.Campus)
                SaveCampus(TinspectionCampus, inspections);
            foreach (var inspectionActivity in newItem.TInspectionActivity)
                httpResponseResult = SaveInspection(newItem, inspectionActivity, isComplete, false, inspections);

            var newInspectionId = newItem.InspectionId;
            if (inspections.Count > 0)
                newInspectionId = inspections.FirstOrDefault().InspectionId;

            // add assets reports if all assets done and get latest status of EP.
            var EPDocument = _goalMasterService.GetEpTransInfo(newItem.EPDetailId, newInspectionId, newItem.InspectionGroupId); // getting latest status of this ep to generate assets auto reports 
            if (EPDocument.FirstOrDefault() != null && EPDocument.FirstOrDefault().IsAllAssetsdone != null)               
                    if (EPDocument.FirstOrDefault().IsAllAssetsdone.Value == 1 && EPDocument.FirstOrDefault().IsAssetEP && mode == BDO.Enums.Mode.ASSET.ToString())
                        SaveAssetsReportsinTinspectionEPDocs(newItem.TInspectionActivity.FirstOrDefault().CreatedBy, EPDocument.FirstOrDefault().EPDetailId, EPDocument.FirstOrDefault().InspectionId, EPDocument.FirstOrDefault().DoctypeId);
            // add assets reports if all assets done 

            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                Inspection = newItem,
                EPDocument = _goalMasterService.GetEpTransInfo(newItem.EPDetailId, newInspectionId, newItem.InspectionGroupId), // getting latest status of this ep after reports 
                TIlsm = httpResponseResult.Result.TIlsm
            };
            _objHttpResponseMessage.Message = mode == BDO.Enums.Mode.DOC.ToString() ? ConstMessage.Document_Added_Successfully : ConstMessage.Inspection_Added_Successfully;

            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void SaveAssetsReportsinTinspectionEPDocs(int userId, int ePDetailId, int inspectionId, int? Doctypeid)
        {
            InspectionEPDocs epAssetDocs = new InspectionEPDocs();
            EPDetails epdetails = new EPDetails();
            var base64EncodePdf = _pdfService.PrintAssetsReportsInbytes(userId, ePDetailId, ref epdetails);
            if (!string.IsNullOrEmpty(base64EncodePdf))
            {
                //AssetsReport_AssetCode_StandardEP_DateTime.Now.ToShortDateString()_hdgc.pdf
                Files newFile = _apiCommon.SaveFile(base64EncodePdf, $"AssetsReport_{inspectionId }_{DateTime.Now.ToShortDateString()}_{getRandomNumber()}" + ".pdf", "InspectionReport", "AssetsReports");
                epAssetDocs.DocumentName = newFile.FileName;
                epAssetDocs.Path = newFile.FilePath;
                epAssetDocs.ActivityType = 3;
                epAssetDocs.ActivityId = _apiCommon.GetActivityId();
                epAssetDocs.DocTypeId = Doctypeid > 0 ? Doctypeid : Convert.ToInt32(BDO.Enums.UploadDocTypes.AssetsReport);
                epAssetDocs.UploadDocTypeId = Convert.ToInt32(BDO.Enums.UploadDocTypes.AssetsReport);
                epAssetDocs.InspectionId = Convert.ToInt32(inspectionId);
            }
            var tinsDocId = _transactionService.AddInspectionDocs(epAssetDocs);
            var inspectionActivity = new TInspectionActivity
            {
                InspectionId = inspectionId,
                ActivityType = 3, // Convert.ToInt32(Enums.Mode.DOC),
                ActivityId = epAssetDocs.ActivityId,
                Status = 1,
                ActivityInspectionDate = System.DateTime.Now,
                IsDeficiency = false,
                SubStatus = BDO.Enums.InspectionSubStatus.NA.ToString(),
                EPDetailId = ePDetailId,
                DtEffectiveDate = DateTime.UtcNow,
                TinsDocId = tinsDocId,
                DocTypeId = Doctypeid > 0 ? Doctypeid : null,

            };
            _transactionService.AddTInspectionActivity(inspectionActivity);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void SaveCampus(Buildings TinspectionCampus, IEnumerable<Inspection> InspectionObj)
        {
            foreach (var item in InspectionObj)
            {
                if (item.InspectionId > 0)
                {
                    int inspectionId = Convert.ToInt32(item.InspectionId);
                    int Epdetailid = item.EPDetailId;
                    _transactionService.AddTInspectionCampus(TinspectionCampus, inspectionId, Epdetailid);
                }
            }
        }
       
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public int getRandomNumber()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private ActionResult Inspect(Inspection newItem, TInspectionActivity objTInspectionActivity, bool isComplete, bool isDeficiency, IEnumerable<Inspection> inspections)
        {
            var _objHttpResponseMessage = SaveInspection(newItem, objTInspectionActivity, isComplete, isDeficiency, inspections);
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private HttpResponseMessage SaveInspection(Inspection newItem, TInspectionActivity objTInspectionActivity, bool isComplete, bool isDeficiency, IEnumerable<Inspection> inspections)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var activityType = objTInspectionActivity.ActivityType;
            var currentTilsm = new TIlsm();
            var inspectiontime = objTInspectionActivity.InspectionDateTimeSpan > 0 ? Conversion.ConvertToDateTime(objTInspectionActivity.InspectionDateTimeSpan) : DateTime.UtcNow;

            foreach (var item in inspections)
            {
                var activityId = _apiCommon.GetActivityId();
                if (item.InspectionId > 0)
                {
                    int inspectionId = Convert.ToInt32(item.InspectionId);
                    int? frequencyId = item.FrequencyId;
                    objTInspectionActivity.EPDetailId = item.EPDetailId;
                    objTInspectionActivity = AddTinspectionActivity(newItem, activityId, isComplete, isDeficiency, activityType, inspectiontime, inspectionId, objTInspectionActivity, frequencyId);
                }
            }
           
            if (objTInspectionActivity.TInsActivityId > 0)
            {               
                if (objTInspectionActivity.ActivityType == 3)
                {

                    if (isComplete)
                        foreach (var inspectionDetail in objTInspectionActivity.TInspectionDetail)

                            foreach (var inspectionSteps in inspectionDetail.InspectionSteps)
                                if (inspectionSteps.Status == -1)
                                    inspectionSteps.Status = 0;
                }
                if (objTInspectionActivity.ActivityType == 3 || objTInspectionActivity.ActivityType == 2)
                {
                    if (objTInspectionActivity.InspectionDocs != null)
                        AddInspectionDocs(objTInspectionActivity);
                }

                foreach (var inspectionDetail in objTInspectionActivity.TInspectionDetail)
                {
                    List<int> lists = inspectionDetail.InspectionSteps.Where(x => x.Status == 1 && string.IsNullOrEmpty(x.FileName) && string.IsNullOrEmpty(x.Comments) && string.IsNullOrEmpty(x.InputValue)).Select(x => x.StepsId).ToList();
                    string stepsId = string.Join(",", lists);
                    inspectionDetail.ActivityId = objTInspectionActivity.ActivityId;
                    inspectionDetail.InspectionDetailId = _transactionService.AddInspectionDetails(inspectionDetail, stepsId);
                    if (inspectionDetail.InspectionDetailId > 0)
                        AddInspectionSteps(objTInspectionActivity.TInsActivityId, inspectionDetail);
                }

                int rAFailCount = 0;
                if (objTInspectionActivity.TInspectionDetail != null)
                    rAFailCount = objTInspectionActivity.TInspectionDetail.SelectMany(x => x.InspectionSteps).Count(x => x.Status == 0 && x.IsRA);

                AddinspectionFiles(newItem, objTInspectionActivity.ActivityId);
                //string subStatus = Enums.InspectionSubStatus.NA.ToString();
                if (isDeficiency)
                {
                    string subStatus = BDO.Enums.InspectionSubStatus.DE.ToString();
                    _transactionService.UpdateInspectionSubstatus(newItem.InspectionId, objTInspectionActivity.ActivityId, subStatus);
                }
                if (objTInspectionActivity.Status == 0 && rAFailCount > 0)
                {

                    _ilsmService.UpdateIlsmStatus(objTInspectionActivity.ActivityId);
                    currentTilsm = _ilsmService.GetCurrentTilsm(objTInspectionActivity.ActivityId);
                }
                if (objTInspectionActivity.SubStatus != BDO.Enums.InspectionSubStatus.NA.ToString())
                {
                    NotificationEmails objNotificationEmails = new NotificationEmails()
                    {
                        NotificationCatId = Convert.ToInt32(BDO.Enums.NotificationCategory.Inspection),
                        NotificationEventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnDeficiencies)
                    };
                    _emailService.SendCommonEmail(objNotificationEmails, newItem);

                    //if (CommonRepository.IsSendMail(Enums.NotificationCategory.Inspection.ToString(), Enums.NotificationEvent.OnDeficiencies.ToString()))
                    //{
                    //    Email.SendInspectionFailMail(newItem, Convert.ToInt32(Enums.NotificationCategory.Inspection));
                    //}
                }
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    TIlsm = currentTilsm
                };

                if (_objHttpResponseMessage.Result == null)
                    _objHttpResponseMessage.Result = new Result();
            }

            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private TInspectionActivity AddTinspectionActivity(Inspection newItem,
            Guid activityId, bool isComplete, bool isDeficiency, int activityType,
            DateTime inspectionTime, int inspectionId, TInspectionActivity objTInspectionActivity, int? frequencyId)
        {


            //inspectionTime = DateTime.UtcNow;
            //TInspectionActivity objTInspectionActivity = newItem.TInspectionActivity.Where(x => x.ActivityType == activityType).FirstOrDefault();
            var tinspectionDetailstatus = SetActivityStatus(objTInspectionActivity, isComplete);
            if (isComplete)
                isDeficiency = SetDeficiencyStatus(newItem);
            //   int maxDrTime = CalculateMaxDRTime(newItem);
            // objTInspectionActivity = ;
            if (objTInspectionActivity.ActivityId == Guid.Empty)
                objTInspectionActivity.ActivityId = activityId;


            objTInspectionActivity.ScheduleDueDate = CalculateScheduleDueDate(newItem.EPDetailId, objTInspectionActivity.FloorAssetId);

            objTInspectionActivity.RaScore = RaScore(objTInspectionActivity);
            objTInspectionActivity.IsReInspection = newItem.SubStatus == BDO.Enums.InspectionSubStatus.RE.ToString();
            objTInspectionActivity.InspectionId = inspectionId;
            objTInspectionActivity.InsType = 1;
            objTInspectionActivity.IsDeficiency = isDeficiency;
            if (isDeficiency)
                objTInspectionActivity.DeficiencyDate = DateTime.UtcNow;
            objTInspectionActivity.Status = tinspectionDetailstatus;
            objTInspectionActivity.ActivityType = activityType;
            objTInspectionActivity.CreatedDate = inspectionTime;
            objTInspectionActivity.FrequencyId = frequencyId;
            objTInspectionActivity.SubStatus = isComplete ? CalculateActivitySubStatus(newItem) : BDO.Enums.InspectionSubStatus.IN.ToString();
            objTInspectionActivity.WorkOrders = objTInspectionActivity.WorkOrders != null ? objTInspectionActivity.WorkOrders : new List<WorkOrder>();
            if (objTInspectionActivity.InspectionDocs != null && objTInspectionActivity.InspectionDocs.Count > 0)
            {

                objTInspectionActivity.DocTypeId = objTInspectionActivity.InspectionDocs[0].DocTypeId != null ? Convert.ToInt32(objTInspectionActivity.InspectionDocs[0].DocTypeId) : 0;
                var effectiveDate = objTInspectionActivity.InspectionDocs[0].EffectiveDate;
                if (effectiveDate != null)

                    objTInspectionActivity.DtEffectiveDateUTC = Conversion.ConvertToDateTime(effectiveDate.Value);
                DateTime DtEffectiveDate = TimeZoneInfo.ConvertTimeToUtc(objTInspectionActivity.DtEffectiveDateUTC);
                objTInspectionActivity.DtEffectiveDate = DtEffectiveDate;
                objTInspectionActivity.ActivityInspectionDate = DtEffectiveDate;
            }
            objTInspectionActivity.TInsActivityId = _transactionService.AddTInspectionActivity(objTInspectionActivity);
            return objTInspectionActivity;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private DateTime CalculateScheduleDueDate(int ePDetailId, int? floorAssetId)
        {
            var datetime = DateTime.Now;
            var schedule = _scheduleService.GetSchedules(ePDetailId, floorAssetId);
            if (schedule != null && schedule.ScheduleFrequency != null)
                return schedule.ScheduleFrequency.FirstOrDefault().NextScheduleDate;
            return datetime;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private List<Inspection> CheckReInspection(string totalEPdetailId, int inspectionGroupId, int? isAllDocumentUploaded, int? status, string mode, Inspection currentInspection)
        {
            var objinspection = new List<Inspection>();
            var array = totalEPdetailId.Split(',');
            var docTypeId = 0;
            var _inspectionid = currentInspection.PreviousCycleInspectionId ?? 0;

            var currentActivity = currentInspection.TInspectionActivity.FirstOrDefault(x => x.ActivityType == (int)BDO.Enums.Mode.DOC);
            if (currentActivity?.InspectionDocs.FirstOrDefault() != null)
            {
                var typeId = currentActivity.InspectionDocs.FirstOrDefault().DocTypeId;
                if (typeId != null)
                    docTypeId = typeId.Value;
            }
            if (_inspectionid > 0)
            {
                currentInspection.InspectionId = _inspectionid;
                currentInspection.IsCurrentCycle = 0;
            }
               
            if (currentInspection.InspectionId > 0 && currentInspection.IsCurrentCycle != 1)
            {
                currentInspection.IsPreviousCycle = 1;
                
            }
                
           
            foreach (var item in array)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var newitem = new Inspection
                    {
                        Status = status,
                        IsAllDocumentUploaded = isAllDocumentUploaded,
                        InspectionId =currentInspection.IsCurrentCycle==1?0: currentInspection.InspectionId,
                        EPDetailId=currentInspection.EPDetailId
                    };
                    //var inspection = _transactionService.GetEpsInspections(Convert.ToInt32(item)).FirstOrDefault(x => x.IsCurrent && x.EPDetailId == Convert.ToInt32(item));
                    if (currentInspection.InspectionId > 0 && currentInspection.IsCurrentCycle !=1)
                    {

                        //if (mode == BDO.Enums.Mode.DOC.ToString() && docTypeId == (int)BDO.Enums.UploadDocTypes.MiscDocuments)
                        //{
                        //    newitem.EPDetailId = currentInspection.EPDetailId;
                        //    newitem.InspectionGroupId = currentInspection.InspectionGroupId;
                        //    newitem.SubStatus = BDO.Enums.InspectionSubStatus.RE.ToString();
                        //    newitem.TCycleId = currentInspection.TCycleId;
                        //    newitem.IsPreviousCycle = currentInspection.IsPreviousCycle;
                        //    newitem.InspectionId = currentInspection.PreviousCycleInspectionId.Value;
                        //}
                        //else
                        //{
                        //    newitem.EPDetailId = currentInspection.EPDetailId;                            
                        //    newitem.InspectionGroupId = currentInspection.InspectionGroupId; 
                        //    if (newitem.Status == 0)
                        //    {
                        //        newitem.SubStatus = "DE";
                        //    }
                        //    else
                        //    {
                        //        newitem.SubStatus = "NA";
                        //    }
                        //    newitem.TCycleId = currentInspection.TCycleId;
                        //    newitem.IsPreviousCycle = currentInspection.IsPreviousCycle;
                        //    newitem.InspectionId = _transactionService.AddInspection(newitem);

                        //}

                    }
                    //else if (inspection != null)
                    //{
                    //    //if (inspection.SubStatus == BDO.Enums.InspectionSubStatus.RE.ToString()
                    //    //                          || (mode == BDO.Enums.Mode.DOC.ToString() && docTypeId == (int)BDO.Enums.UploadDocTypes.MiscDocuments))
                    //    //{
                    //    //    newitem.EPDetailId = inspection.EPDetailId;                          
                    //    //    newitem.InspectionGroupId = inspection.InspectionGroupId;                           
                    //    //    newitem.SubStatus = BDO.Enums.InspectionSubStatus.RE.ToString();
                    //    //    newitem.TCycleId = currentInspection.TCycleId;
                    //    //    newitem.IsPreviousCycle = currentInspection.IsPreviousCycle;
                    //    //    newitem.InspectionId = inspection.InspectionId;
                    //    //}
                    //    //else
                    //    //{
                    //    //    newitem.EPDetailId = inspection.EPDetailId;
                    //    //    newitem.InspectionGroupId = inspection.InspectionGroupId;                          
                    //    //    newitem.SubStatus = inspection.SubStatus;
                    //    //    newitem.TCycleId = currentInspection.TCycleId;
                    //    //    newitem.IsPreviousCycle = currentInspection.IsPreviousCycle;
                    //    //    newitem.InspectionId = _transactionService.AddInspection(newitem);

                    //    //}
                    //}
                    else
                    {
                        newitem.EPDetailId = Convert.ToInt32(item);
                        newitem.InspectionGroupId = inspectionGroupId;
                        newitem.TCycleId = currentInspection.TCycleId;
                        newitem.IsPreviousCycle = currentInspection.IsPreviousCycle;                        
                        newitem.InspectionId = _transactionService.AddInspection(newitem);
                    }
                    objinspection.Add(newitem);
                }
            }
            return objinspection;
        }

        //private static int CheckReInspection(Inspection newItem)
        //{
        //    var inspection = _transactionService.GetEpsInspections(newItem.EPDetailId).FirstOrDefault(x => x.InspectionGroupId == newItem.InspectionGroupId && x.IsCurrent); //InspectionRepository.GetCurrentInspection(newItem.EPDetailId);
        //    if (inspection != null)
        //    {
        //        if (inspection.SubStatus == Enums.InspectionSubStatus.RE.ToString())
        //        {
        //            newItem.InspectionId = inspection.InspectionId;
        //            newItem.SubStatus = Enums.InspectionSubStatus.RE.ToString();
        //        }
        //        else
        //        {
        //            newItem.DocStatus = GetDocStatus(newItem, inspection);
        //            newItem.InspectionId = _transactionService.AddInspection(newItem);
        //        }
        //    }
        //    else
        //    {
        //        newItem.DocStatus = GetDocStatus(newItem, inspection);
        //        newItem.InspectionId = _transactionService.AddInspection(newItem);
        //    }
        //    return newItem.InspectionId;
        //}


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private int GetDocStatus(Inspection newItem, Inspection inspection)
        {
            int docstatus = _epService.CheckDocRequired(newItem.EPDetailId);
            if (docstatus != -3)
            {
                if (inspection != null)
                {
                    newItem.DocStatus = inspection.DocStatus != -3 ? inspection.DocStatus : docstatus;
                }
                else
                {
                    newItem.DocStatus = docstatus;
                }
            }
            else
            {
                newItem.DocStatus = docstatus;
            }
            if (newItem.DocStatus != -3)
            {
                if (newItem.IsAllDocumentUploaded.HasValue)
                    newItem.DocStatus = newItem.IsAllDocumentUploaded.Value;
            }
            return newItem.DocStatus.Value;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="newItem"></param>
        ///// <param name="mode"></param>
        ///// <param name="isComplete"></param>
        ///// <returns></returns>
        //public void AddInspectionManual(Inspection newItem, string mode, bool isComplete)
        //{
        //    //objHttpResponseMessage = new HttpResponseMessage();
        //    mode = mode.ToUpper();
        //    var isDeficiency = false;
        //    //int? inspectiondocsId = null;
        //    var activityType = Common.SetActivityType(mode);
        //    if (mode != Enums.Mode.EP.ToString())
        //        newItem.Status = null;
        //    else
        //        newItem.Status = SetActivityStatus(newItem, isComplete);

        //    newItem.DocStatus = EpRepository.DocStatus(newItem.EPDetailId);
        //    if (newItem.DocStatus != -3)
        //    {
        //        if (newItem.IsAllDocumentUploaded.HasValue)
        //            newItem.DocStatus = newItem.IsAllDocumentUploaded.Value;
        //    }
        //    newItem.InspectionId = CheckReInspection(newItem);
        //    if (newItem.InspectionId > 0)
        //    {

        //        var activityId = GetActivityId();
        //        var tinspectionDetailstatus = SetActivityStatus(newItem, isComplete);

        //        if (isComplete)
        //            isDeficiency = SetDeficiencyStatus(newItem);

        //        int maxDrTime = 0;// CalculateMaxDRTime(newItem);
        //        DateTime inspectionTime = DateTime.UtcNow;
        //        TInspectionActivity objTInspectionActivity;
        //        objTInspectionActivity = newItem.TInspectionActivity[0];
        //        objTInspectionActivity.ActivityId = activityId;
        //        objTInspectionActivity.IsReInspection = newItem.SubStatus == Enums.InspectionSubStatus.RE.ToString() ? true : false;
        //        objTInspectionActivity.InspectionId = newItem.InspectionId;
        //        objTInspectionActivity.IsDeficiency = isDeficiency;
        //        if (isDeficiency)
        //        {
        //            objTInspectionActivity.DeficiencyDate = DateTime.UtcNow;
        //        }
        //        objTInspectionActivity.Status = tinspectionDetailstatus;
        //        objTInspectionActivity.EstimatedTime = maxDrTime;
        //        objTInspectionActivity.ActivityType = activityType;
        //        objTInspectionActivity.CreatedDate = inspectionTime;
        //        objTInspectionActivity.FrequencyId = newItem.FrequencyId.HasValue ? newItem.FrequencyId : null;
        //        if (newItem.TInspectionActivity[0].InspectionDocs != null)
        //        {
        //            if (newItem.TInspectionActivity[0].InspectionDocs.Count > 0)
        //            {
        //                objTInspectionActivity.DocTypeId = newItem.TInspectionActivity[0].InspectionDocs[0].DocTypeId != null ? Convert.ToInt32(newItem.TInspectionActivity[0].InspectionDocs[0].DocTypeId) : 0;
        //                objTInspectionActivity.DtEffectiveDate = Conversion.ConvertToDateTime(newItem.TInspectionActivity[0].InspectionDocs[0].EffectiveDate.Value);
        //            }
        //        }
        //        if (objTInspectionActivity.ActivityType == 3)
        //        {
        //            if (isComplete)
        //                foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
        //                    foreach (var inspectionsteps in inspectionDetail.InspectionSteps)
        //                        if (inspectionsteps.Status == -1)
        //                            inspectionsteps.Status = 0;
        //        }
        //        objTInspectionActivity.SubStatus = "NA";// Enums.InspectionSubStatus.NA.ToString();
        //        objTInspectionActivity.DtEffectiveDate = DateTime.UtcNow;
        //        _transactionService.AddTInspectionActivity(objTInspectionActivity);

        //        newItem.TInspectionActivity[0] = objTInspectionActivity;

        //        if (objTInspectionActivity.ActivityType == 3 || objTInspectionActivity.ActivityType == 2)
        //        {
        //            if (objTInspectionActivity.InspectionDocs != null)
        //                AddInspectionDocs(objTInspectionActivity);
        //        }
        //        //else if (activityType == 2)
        //        //{
        //        //    if (objTInspectionActivity.InspectionDocs != null)
        //        //        inspectiondocsId = AddInspectionDocs(objTInspectionActivity);
        //        //}
        //        foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
        //        {
        //            inspectionDetail.ActivityId = activityId;
        //            inspectionDetail.InspectionDetailId = _transactionService.AddInspectionDetails(inspectionDetail);
        //            if (inspectionDetail.InspectionDetailId > 0)
        //                AddInspectionSteps(newItem.InspectionId, inspectionDetail, inspectionTime);
        //        }
        //        // AddinspectionFiles(newItem, activityId);

        //        //string subStatus = Enums.InspectionSubStatus.NA.ToString();
        //        //if (isDeficiency && activityType == 2)
        //        //{
        //        //    //SendIlsmFailMail(newItem.EPDetailId, activityId);
        //        //    subStatus = Enums.InspectionSubStatus.DE.ToString();
        //        //    if (IsRaStepFail(newItem.TInspectionActivity[0]))
        //        //    {
        //        //        subStatus = Enums.InspectionSubStatus.RA.ToString();
        //        //        if (CheckIlsmFail(newItem))
        //        //            subStatus = Enums.InspectionSubStatus.IL.ToString();
        //        //    }
        //        //}

        //        ////if (subStatus != HCF.Utility.Enums.InspectionSubStatus.NA.ToString())
        //        //_transactionService.UpdateInspectionSubstatus(newItem.InspectionId, activityId, subStatus);



        //        //if (MaxDRTime > 0)
        //        //    SaveAssetTimeToResolve(newItem, activityId, MaxDRTime);

        //        //if (newItem.InspectionId > 0)
        //        //{
        //        //    objHttpResponseMessage.Success = true;
        //        //    objHttpResponseMessage.Result = new Result { Inspection = newItem };
        //        //    objHttpResponseMessage.Message = mode == Enums.Mode.DOC.ToString() ? ConstMessage.Document_Added_Successfully : ConstMessage.Inspection_Added_Successfully;
        //        //}
        //    }
        //    //else
        //    //{
        //    //    objHttpResponseMessage.Message = ConstMessage.Error;
        //    //}
        //    ////}
        //    //return objHttpResponseMessage;
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private string CalculateActivitySubStatus(Inspection newItem)
        {
            var subStatus = BDO.Enums.InspectionSubStatus.NA.ToString();
            //var totalCount = 0;
            var failCount = 0;
            var raCount = 0;
            foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
            {
                //totalCount = totalCount + inspectionDetail.InspectionSteps.Count;
                failCount += inspectionDetail.InspectionSteps.Count(m => m.Status == 0);
                raCount += inspectionDetail.InspectionSteps.Count(m => m.Status == 0 && m.IsRA && m.DRTime == 0);
                if (raCount > 0)
                {
                    subStatus = BDO.Enums.InspectionSubStatus.RA.ToString(); break;
                }
                else if (failCount > 0)
                {
                    subStatus = BDO.Enums.InspectionSubStatus.DE.ToString();
                }
            }
            return subStatus;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="inspection"></param>
        ///// <returns></returns>
        //private bool UpdateDeficienceSteps(Inspection inspection)
        //{
        //    bool status = true;
        //    foreach (TInspectionActivity activity in inspection.TInspectionActivity)
        //    {
        //        foreach (TInspectionDetail details in activity.TInspectionDetail)
        //        {
        //            foreach (InspectionSteps steps in details.InspectionSteps)
        //            {
        //                try
        //                {
        //                    if (!string.IsNullOrEmpty(steps.FileContent))
        //                        steps.FilePath = SaveFile(steps.FileContent, steps.FileName, "StepsImagePath", Convert.ToString(inspection.InspectionId)).FilePath;

        //                    TInsStepsTask stepTask = new TInsStepsTask();
        //                    stepTask.ActivityId = activity.ActivityId;
        //                    stepTask.Comment = steps.Comments;
        //                    stepTask.CreatedBy = inspection.CreatedBy;
        //                    stepTask.StepsId = steps.StepsId;
        //                    stepTask.TInsStepsId = steps.TInsStepsId;
        //                    stepTask.FileName = steps.FileName;
        //                    stepTask.FilePath = steps.FilePath;
        //                    stepTask.Status = steps.Status;
        //                    stepTask.TinsStepTaskId = HCF.BAL.InsStepsTaskRepository.Save(stepTask);
        //                }
        //                catch (Exception ex)
        //                {
        //                    HCF.Utility.ErrorLog.ErrorMsg(ex.Message);
        //                    status = false;
        //                }
        //            }
        //        }
        //    }
        //    return status;
        //}



        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private void CalculateMaxDRTime(Inspection newItem)
        {
            int drTime = 0;
            foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
            {
                foreach (var steps in inspectionDetail.InspectionSteps.Where(x => x.DRTime > 0 && x.Status == 0))
                {
                    if (steps.DRTime != null && steps.DRTime.Value > drTime)
                    {
                        drTime = steps.DRTime.Value;
                    }
                }
            }
        }

        //private static bool IsRaStepFail(TInspectionActivity objTInspectionActivity)
        //{
        //    bool status = false;
        //    foreach (var details in objTInspectionActivity.TInspectionDetail)
        //    {
        //        if (details.InspectionSteps.Count(x => x.Status == 0 && x.IsRA) > 0)
        //            status = true;
        //        break;

        //    }
        //    return status;
        //}

        /// <summary>
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void AddinspectionFiles(Inspection newItem, Guid activityId)
        {
            if (newItem.TInspectionFiles != null)
                if (newItem.TInspectionFiles.Count > 0)
                    foreach (var item in newItem.TInspectionFiles)
                    {
                        item.ActivityId = activityId;
                        if (!string.IsNullOrEmpty(item.FileContent))
                        {
                            item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "StepsImagePath", newItem.InspectionId.ToString()).FilePath;
                            //if (!string.IsNullOrEmpty(item.FilePath))
                            //{
                            //    _transactionService.AddInspectionFiles(item);
                            //}
                            _transactionService.AddInspectionFiles(item);
                        }
                    }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("SaveInspectionFiles/")]
        public ActionResult SaveInspectionFiles(TInspectionFiles item, string inspectionId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            //int TInsFileId = 0;
            if (!string.IsNullOrEmpty(item.FileContent))
            {
                item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "StepsImagePath", inspectionId).FilePath;
                item.TInsFileId = _transactionService.AddInspectionFiles(item);
            }
            if (item.TInsFileId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    TInspectionFiles = item
                };
            }
            else { _objHttpResponseMessage.Success = false; }
            return Ok(_objHttpResponseMessage);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsActivityId"></param>
        /// <param name="inspectionDetail"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void AddInspectionSteps(int tInsActivityId, TInspectionDetail inspectionDetail)
        {
            DateTime inspectiontime = DateTime.Now;
            foreach (var inspectionsteps in inspectionDetail.InspectionSteps.Where(x => x.Status != 1 || !string.IsNullOrEmpty(x.FileName) || !string.IsNullOrEmpty(x.Comments) || !string.IsNullOrEmpty(x.InputValue)))
            {
                inspectionsteps.InspectionDetailId = inspectionDetail.InspectionDetailId;
                if (!string.IsNullOrEmpty(inspectionsteps.FileContent))
                    inspectionsteps.FilePath = _apiCommon.SaveFile(inspectionsteps.FileContent, inspectionsteps.FileName, "StepsImagePath", tInsActivityId.ToString()).FilePath;
                if (inspectionsteps.Status == 1)
                    inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.NA.ToString();
                if (inspectionsteps.Status == 0)
                {
                    if (inspectionsteps.IsRA && inspectionsteps.DRTime == 0)
                    {
                        inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.RA.ToString();
                        inspectionsteps.RaDate = inspectiontime;
                    }
                    else
                    {
                        inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.DE.ToString();
                    }
                    inspectionsteps.DeficiencyDate = inspectiontime;
                }
                else { inspectionsteps.DRTime = 0; }
                inspectionsteps.TInsStepsId = _transactionService.AddInspectionSteps(inspectionsteps);
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void AddILSMInspectionSteps(int inspectionId, TInspectionDetail inspectionDetail, DateTime inspectiontime)
        {
            foreach (var inspectionsteps in inspectionDetail.InspectionSteps)
            {
                inspectionsteps.InspectionDetailId = inspectionDetail.InspectionDetailId;
                if (!string.IsNullOrEmpty(inspectionsteps.FileContent))
                    inspectionsteps.FilePath = _apiCommon.SaveFile(inspectionsteps.FileContent, inspectionsteps.FileName, "StepsImagePath", inspectionId.ToString()).FilePath;
                if (inspectionsteps.Status == 1)
                    inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.NA.ToString();
                if (inspectionsteps.Status == 0)
                {
                    if (inspectionsteps.IsRA && inspectionsteps.DRTime == 0)
                    {
                        inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.RA.ToString();
                        inspectionsteps.RaDate = inspectiontime;
                    }
                    else
                    {
                        inspectionsteps.DeficiencyCode = BDO.Enums.InspectionSubStatus.DE.ToString();
                    }
                    inspectionsteps.DeficiencyDate = inspectiontime;
                }
                else { inspectionsteps.DRTime = 0; }
                inspectionsteps.TInsStepsId = _transactionService.AddInspectionSteps(inspectionsteps);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionActivity"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void AddInspectionDocs(TInspectionActivity inspectionActivity)
        {
            // var document = new Documents();
            // var attachments = new List<FilesRepository>();
            foreach (var inspectiondocs in inspectionActivity.InspectionDocs)
            {
                inspectiondocs.ActivityId = inspectionActivity.ActivityId;
                //if (inspectiondocs.AttachmentId.HasValue && inspectiondocs.AttachmentId.Value > 0)
                //    inspectiondocs.DocReferenceId = $"I_{inspectiondocs.AttachmentId}";
                //inspectiondocs.InspectionDetailId = inspectionDetailId;
                if (!string.IsNullOrEmpty(inspectiondocs.Path))
                    inspectiondocs.Path = inspectiondocs.Path;// //UploadFileOnServer(document, attachments, inspectiondocs, inspectionActivity.InspectionId.ToString());
                else if (!string.IsNullOrEmpty(inspectiondocs.FilesContent))
                    inspectiondocs.Path = _apiCommon.SaveFile(inspectiondocs.FilesContent, inspectiondocs.DocumentName, "EPDocs",
                        inspectionActivity.InspectionId.ToString()).FilePath;
                _transactionService.AddInspectionDocs(inspectiondocs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="isComplete"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private int SetActivityStatus(TInspectionActivity activity, bool isComplete)
        {
            if (activity != null)
            {
                var status = isComplete == false
                    ? Convert.ToInt32(BDO.Enums.Status.In_Progress)
                    : CalculateGoalStatus(activity);
                return status;
            }
            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private bool SetDeficiencyStatus(Inspection newItem)
        {
            var isDeficiency = true;
            var result = CalculateDeficienyStatus(newItem);
            if (result > 0)
                isDeficiency = false;
            return isDeficiency;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private int CalculateGoalStatus(TInspectionActivity activity)
        {
            var inspectionDetailStatus = 0;
            var totalCount = 0;
            var passCount = 0;
            //var defeciencyCount = 0;
            //var resolvetimeCount = 0;
            foreach (var inspectionDetail in activity.TInspectionDetail)
            {
                totalCount += inspectionDetail.InspectionSteps.Count;
                passCount += inspectionDetail.InspectionSteps.Count(m => m.Status == 1 || m.Status == -3); // -3 are used for not applicable and treat as pass steps.
                                                                                                           //defeciencyCount = defeciencyCount + inspectionDetail.InspectionSteps.Count(m => m.Status == 0 && m.DRTime == 0);
                                                                                                           //resolvetimeCount = resolvetimeCount + inspectionDetail.InspectionSteps.Count(m => m.Status == 0 && m.DRTime > 0);
            }
            if (totalCount == passCount)
                inspectionDetailStatus = Convert.ToInt32(BDO.Enums.Status.Pass);
            //else if (totalCount != passCount && resolvetimeCount > 0)
            //    inspectionDetailStatus = Convert.ToInt32(Enums.Status.In_Progress);
            //else if (totalCount != passCount && defeciencyCount > 0)
            //    inspectionDetailStatus = Convert.ToInt32(Enums.Status.Fail);
            return inspectionDetailStatus;
        }

        /// <summary>
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private int CalculateDeficienyStatus(Inspection newItem)
        {
            var inspectionDetailStatus = 0;
            var totalCount = 0;
            var passCount = 0;
            foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
            {
                totalCount += inspectionDetail.InspectionSteps.Count;
                passCount += inspectionDetail.InspectionSteps.Count(m => m.Status == 1);
            }
            if (totalCount == passCount)
                inspectionDetailStatus = Convert.ToInt32(BDO.Enums.Status.Pass);

            return inspectionDetailStatus;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetAssetsEpDetails(string floorAssetId, string assetsId, string epDetailId, string inspectionId, string frequencyId) // get Asset Steps
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            var mode = Convert.ToInt32(BDO.Enums.Mode.ASSET);
            var epdocs = _transactionService.GetGoalStepsByActivity(
                Convert.ToInt32(floorAssetId),
                Convert.ToInt32(assetsId),
                Convert.ToInt32(epDetailId),
                Convert.ToInt32(inspectionId),
                mode, Convert.ToInt32(frequencyId));
            if (epdocs != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { EPDocument = epdocs };
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetEpHistory(string epDetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var standardHistory = _standardService.GetEpHistory(Convert.ToInt32(epDetailId));
            if (standardHistory != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Standard = standardHistory };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetEpInspectionHistory(string userId, string epDetailId, string inspectionId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result(),
                Success = true
            };
            _objHttpResponseMessage.Result.EPDetail = _transactionService.GetInspectionHistory(Convert.ToInt32(userId),
                Convert.ToInt32(epDetailId), Convert.ToInt32(inspectionId));


            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Ticket Work Order

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorWorkOrder"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("saveFloorInspections")]
        public ActionResult SaveRoundInspections(WorkOrder floorWorkOrder)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            floorWorkOrder.DateCreated = DateTime.UtcNow;

            if (floorWorkOrder.IssueId > 0)
                floorWorkOrder.IssueId = _roundInspectionsService.UpdateRoundInspection(floorWorkOrder);
            else
            {
                floorWorkOrder.WorkOrderId = 0;
                floorWorkOrder.WorkOrderNumber = "0";
                floorWorkOrder.IssueId = _roundInspectionsService.AddRoundInspection(floorWorkOrder);
            }


            if (floorWorkOrder.IssueId > 0)
            {
                if (floorWorkOrder.WorkOrderFiles != null)
                {
                    foreach (WorkOrderfiles file in floorWorkOrder.WorkOrderFiles)
                    {
                        if (!string.IsNullOrEmpty(file.FileContent))
                        {
                            file.IssueId = floorWorkOrder.IssueId;
                            Files newFile = _apiCommon.SaveFile(file.FileContent, file.FileName, "FloorInspectionFile", Convert.ToString(floorWorkOrder.IssueId));
                            file.FileName = newFile.FileName;
                            file.FilePath = newFile.FilePath;
                            _workOrderService.SaveWorkOrderFiles(floorWorkOrder.IssueId, file);
                        }
                    }
                }
                //floorWorkOrder.IssueId = IssueId;
                foreach (UserProfile user in floorWorkOrder.Resources)
                    _workOrderService.SaveWorkOrderResource(floorWorkOrder.IssueId, user);
                SendRoundInspectionsMail(floorWorkOrder);
                _objHttpResponseMessage.Success = true;

            }
            _objHttpResponseMessage.Result = new Result
            {
                WorkOrder = floorWorkOrder
            };
            return Ok(_objHttpResponseMessage);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private void SendRoundInspectionsMail(WorkOrder floorWorkOrder)
        {
            _emailService.SendRoundInsMail(floorWorkOrder, Convert.ToInt32(BDO.Enums.NotificationCategory.RoundsInspection));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getRoundInspection/")]
        public ActionResult GetFloorRoundInspection(string floorId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var lists = _workOrderService.GetFloorWorkOrder(Convert.ToInt32(floorId));
            if (lists.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    WorkOrders = lists
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return Ok(_objHttpResponseMessage);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetFloorsByWo(string roundId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var workOrders = _workOrderService.GetFloorWorkOrder().Where(x => x.FloorId.HasValue).ToList();

            var floor = workOrders.GroupBy(i => i.FloorId)
              .Select(g => new Floor
              {
                  FloorId = g.Key.Value

              }).ToList();
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                Floors = floor
            };
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetIssues()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var workOrder = _workOrderService.GetWorkOrders();
            if (workOrder.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result();
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result.WorkOrders = workOrder;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetIssuesPaging(Request objRequest)
        //{
        //    int? profilerId = null;
        //    if (objRequest.UserId.HasValue)
        //        profilerId = Convert.ToInt32(objRequest.UserId);

        //   var _objHttpResponseMessage = new HttpResponseMessage();
        //    _objHttpResponseMessage = GetUpdatedWorkOrders_1(objRequest, profilerId);
        //    _objHttpResponseMessage.Result.WorkOrders = _objHttpResponseMessage.Result.WorkOrders.Where(x => objRequest.WhereCondition.Split(',').Contains(x.StatusCode)).ToList();
        //    if (_objHttpResponseMessage.Result.WorkOrders.Count > 0)
        //        _objHttpResponseMessage.Success = true;
        //    else
        //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    return Ok(_objHttpResponseMessage);
        //}

        //private static HttpResponseMessage GetUpdatedWorkOrders_1(Request objRequest, int? userId)
        //{
        //    // var workOrder = new List<WorkOrder>();
        //    var org = new Organization();
        //    //  List<UserProfile> users = new List<UserProfile>();
        //    // var httpresponse = new HttpResponseMessage();

        //    //var clientNo = 0;
        //    if (HcfSession.ClientNo > 0)
        //        org = ApiCommon.GetOrganization(HcfSession.ClientNo);
        //    else
        //        return new HttpResponseMessage { Success = false, Message = "No client # found." };


        //    var httpResponse = WorkOrderRepository.GetWorkOrdersPaging(objRequest, userId);

        //    if (httpResponse.Result != null && httpResponse.Result.WorkOrders.Count > 0 && org != null && org.IsTmsWo)
        //    {
        //        var workOrders = GetlastWorkOrder(objRequest, userId, httpResponse.Result.WorkOrders, org);
        //        foreach (var tmsOrder in httpResponse.Result.WorkOrders)
        //        {
        //            var isExistingWorkOrders = workOrders.FirstOrDefault(z => z.WorkOrderNumber == tmsOrder.WorkOrderNumber);
        //            if (isExistingWorkOrders != null)
        //            {
        //                tmsOrder.StatusCode = isExistingWorkOrders.StatusCode;
        //            }
        //        }
        //    }           
        //    return httpResponse;
        //}




        #endregion

        #region ILSM           


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult UpdateIlsmStatus()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _ilsmService.UpdateIlsmStatus();
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetIlsm()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TIlsm> tilsm = _ilsmService.GetIlsm();
            if (tilsm.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    TIlsms = tilsm
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult DeleteTInspectionFiles(string tinsFileId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var TinsFileId = Convert.ToInt32(tinsFileId);
            var status = _transactionService.DeleteTInspectionFiles(TinsFileId);
            _objHttpResponseMessage.Success = status;
            _objHttpResponseMessage.Message = ConstMessage.Document_Deleted_Successfully;
            return Ok(_objHttpResponseMessage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsmId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetIlsmInspection(string tilsmId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            TIlsm tilsm = _ilsmService.GetIlsmInspection(Convert.ToInt32(tilsmId));
            if (tilsm != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    TIlsm = tilsm
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="mode"></param>
        /// <param name="isComplete"></param>
        /// <param name="tilsmId"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddIlsmInspection/")]
        //[Route("AddIlsmInspection/{Mode}/{IsComplete}/{tilsmId}")]
        public ActionResult AddIlsmInspection(Inspection newItem, string mode, bool isComplete, string tilsmId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            mode = mode.ToUpper();
            var isDeficiency = false;

            var activityType = CommonUtility.SetActivityType(mode);
            if (mode != BDO.Enums.Mode.EP.ToString())
                newItem.Status = null;
            else
                newItem.Status = SetActivityStatus(newItem.TInspectionActivity.FirstOrDefault(), isComplete);

            TInspectionActivity objTInspectionActivity;
            if (newItem.InspectionId > 0)
            {

                var activityId = _apiCommon.GetActivityId();
                var tinspectionDetailstatus = SetActivityStatus(newItem.TInspectionActivity.FirstOrDefault(), isComplete);

                if (isComplete)
                    isDeficiency = SetDeficiencyStatus(newItem);

                CalculateMaxDRTime(newItem);
                var inspectiontime = DateTime.UtcNow;
                objTInspectionActivity = newItem.TInspectionActivity[0];
                if (objTInspectionActivity.ActivityId == Guid.Empty)
                    objTInspectionActivity.ActivityId = activityId;
                objTInspectionActivity.IsReInspection = newItem.SubStatus == BDO.Enums.InspectionSubStatus.RE.ToString() ? true : false;
                objTInspectionActivity.InspectionId = newItem.InspectionId;
                objTInspectionActivity.IsDeficiency = isDeficiency;
                if (isDeficiency)
                {
                    objTInspectionActivity.DeficiencyDate = DateTime.UtcNow;
                }
                objTInspectionActivity.Status = tinspectionDetailstatus;
                //objTInspectionActivity.EstimatedTime = maxDrTime;
                objTInspectionActivity.ActivityType = activityType;
                objTInspectionActivity.CreatedDate = inspectiontime;
                objTInspectionActivity.FrequencyId = newItem.FrequencyId.HasValue ? newItem.FrequencyId : null;
                if (newItem.TInspectionActivity[0].InspectionDocs != null)
                {
                    if (newItem.TInspectionActivity[0].InspectionDocs.Count > 0)
                    {
                        objTInspectionActivity.DocTypeId = newItem.TInspectionActivity[0].InspectionDocs[0].DocTypeId != null ? Convert.ToInt32(newItem.TInspectionActivity[0].InspectionDocs[0].DocTypeId) : 0;
                        objTInspectionActivity.DtEffectiveDate = Conversion.ConvertToDateTime(newItem.TInspectionActivity[0].InspectionDocs[0].EffectiveDate.Value);
                    }
                }
                objTInspectionActivity.SubStatus = CalculateActivitySubStatus(newItem);// Enums.InspectionSubStatus.NA.ToString();
                objTInspectionActivity.DtEffectiveDate = DateTime.UtcNow;
                int id = _transactionService.AddTInspectionActivity(objTInspectionActivity);
                if (id > 0)
                {
                    newItem.TInspectionActivity[0] = objTInspectionActivity;
                    foreach (var inspectionDetail in newItem.TInspectionActivity[0].TInspectionDetail)
                    {
                        inspectionDetail.ActivityId = objTInspectionActivity.ActivityId;
                        inspectionDetail.InspectionDetailId = _transactionService.AddInspectionDetails(inspectionDetail);
                        if (inspectionDetail.InspectionDetailId > 0)
                            AddILSMInspectionSteps(newItem.InspectionId, inspectionDetail, inspectiontime);
                    }
                    AddinspectionFiles(newItem, objTInspectionActivity.ActivityId);

                    // string subStatus = Enums.InspectionSubStatus.NA.ToString();

                    //if (newItem.TInspectionActivity[0].Status == 1)
                    //    IlsmRepository.UpdateIlsmIncident(newItem.InspectionId);

                    string msg = "";
                    if (!string.IsNullOrEmpty(tilsmId))
                        msg = _ilsmService.CompleteIlsmIncident(Convert.ToInt32(tilsmId));

                    if (newItem.InspectionId > 0)
                    {
                        _objHttpResponseMessage.Success = true;
                        _objHttpResponseMessage.Result = new Result { Inspection = newItem };
                        _objHttpResponseMessage.Message = msg; //Enums.Mode.DOC.ToString() ? ConstMessage.Document_Added_Successfully : ConstMessage.Inspection_Added_Successfully;
                    }
                }
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return Ok(_objHttpResponseMessage);
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult SaveTIlsmComment(TComment tComment)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (tComment.AddedDateTimeSpan > 0)
                tComment.AddedDate = Conversion.ConvertToDateTime(tComment.AddedDateTimeSpan);
            tComment = _insActivityService.SaveTComment(tComment);
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { TComment = tComment };
            _objHttpResponseMessage.Message = ConstMessage.Success;
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("UpdateInspection/")]
        //[Route("UpdateInspection/{createdBy}/{activityId}/{tilsmId}")]
        public ActionResult UpdateRaInspection(List<InspectionSteps> lstInspectionSteps, string createdBy, string activityId, string tilsmId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var item in lstInspectionSteps)
            {
                if (!string.IsNullOrEmpty(item.FileContent))
                    item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "StepsImagePath", tilsmId).FilePath;
                _insStepsService.Update(item, Convert.ToInt32(createdBy), Guid.Parse(activityId));
            }
            if (!string.IsNullOrEmpty(tilsmId))
            {
                _objHttpResponseMessage.Message = _ilsmService.CompleteIlsmIncident(Convert.ToInt32(tilsmId));
            }
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTilsm"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("SaveTilsmReport")]
        public ActionResult SaveTilsmReport(TIlsm newTilsm)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(newTilsm.FilesContent))
            {
                Files newFile = _apiCommon.SaveFile(newTilsm.FilesContent, newTilsm.FileName, "TilsmReport", Convert.ToString(newTilsm.TIlsmId));
                newTilsm.FileName = newFile.FileName;
                newTilsm.FilePath = newFile.FilePath;
                bool status = _ilsmService.SaveTilsmReport(newTilsm);
                if (status)
                {
                    _objHttpResponseMessage.Success = true;
                }
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Reports

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetComplianceReport(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var list = _transactionService.GetComplianceReport();
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result();
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result.Inspections = list;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Manual Inspection

        //public void AddAllInspection(string frequencyId, string startIndex, string limit)
        //{
        //    int index = Convert.ToInt32(startIndex);
        //    int count = Convert.ToInt32(limit);
        //    List<EPDetails> epdetails = HCF.BAL.EpRepository.GetEPByFrequency(Convert.ToInt32(frequencyId));

        //    if (count > 0)
        //        epdetails = epdetails.Skip(index).Take(count).ToList();

        //    LogFile.WriteLog(string.Format("Total Records {0}", epdetails.Count()));
        //    List<MainGoal> Maingoals = HCF.BAL.MainGoalRepository.GetMainGoal();
        //    List<MainGoal> ActivityMainGoal = new List<MainGoal>();
        //    foreach (var item in epdetails)
        //    {
        //        LogFile.WriteLog(string.Format("Inspection Start for {0}", item.EPDetailId));
        //        //foreach (var freq in item.EPFrequency)
        //        //{
        //        //List<MainGoal> maingoals = Maingoal.Where(x => x.EPDetailId == EPdetailId && x.ActivityType == ActivityType).ToList();
        //        var IsInspection = true;
        //        //if (item.IsDocRequired)
        //        //{
        //        //    ActivityMainGoal = Maingoals.Where(x => x.EPDetailId == item.EPDetailId && x.ActivityType == 3).ToList();
        //        //    if (ActivityMainGoal.Count > 0)
        //        //    {
        //        //        GenerateInspectionJson(item.EPDetailId, Convert.ToInt32(frequencyId), ActivityMainGoal, 3);
        //        //    }
        //        //    else
        //        //    {
        //        //        LogFile.WriteLog(string.Format("Doc - No Steps or mail goal for epDetailId {0}",item.EPDetailId));
        //        //        IsInspection = false;
        //        //    }
        //        //}
        //        //if (item.IsAssetsRequired)
        //        //{
        //        //    ActivityMainGoal = Maingoals.Where(x => x.EPDetailId == item.EPDetailId && x.ActivityType == 2).ToList();
        //        //    if (ActivityMainGoal.Count > 0)
        //        //    {
        //        //        List<MarkPassAssets> lstMarkPassAssets = new List<HCF.BDO.MarkPassAssets>();
        //        //        //List<TFloorAssets> tfloorAssets = HCF.BAL.FloorAssetRepository.get
        //        //        MarkPassAssets(lstMarkPassAssets, "4");
        //        //    }
        //        //    else
        //        //    {
        //        //        LogFile.WriteLog(string.Format("Assets - No Steps or mail goal for epDetailId {0}", item.EPDetailId));
        //        //        IsInspection = false;
        //        //    }
        //        //}
        //        if (IsInspection)
        //        {
        //            ActivityMainGoal = Maingoals.Where(x => x.EPDetailId == item.EPDetailId && x.ActivityType == 1).ToList();
        //            if (ActivityMainGoal.Count > 0)
        //            {
        //                GenerateInspectionJson(item.EPDetailId, Convert.ToInt32(frequencyId), ActivityMainGoal, 1);
        //            }
        //            else
        //            {
        //                LogFile.WriteLog(string.Format("EP - No Steps or mail goal for epDetailId {0}", item.EPDetailId));
        //            }
        //        }
        //        LogFile.WriteLog(string.Format("Inspection end for {0}", item.EPDetailId));
        //        //}
        //    }
        //}

        //private void GenerateInspectionJson(int EPdetailId, int frequencyId, List<MainGoal> maingoals, int ActivityType)
        //{
        //    Inspection tInspection = new Inspection
        //    {
        //        InspectionId = 0,
        //        CreatedBy = 4,
        //        //Status = 1,
        //        //DocStatus = 
        //        IsAllDocumentUploaded = 1,
        //        EPDetailId = EPdetailId,
        //        FrequencyId = Convert.ToInt32(frequencyId)
        //    };
        //    tInspection.TInspectionActivity = new List<TInspectionActivity>();
        //    TInspectionActivity tinspectionActivity = new TInspectionActivity();
        //    tInspection.TInspectionActivity.Add(tinspectionActivity);
        //    tInspection.TInspectionActivity[0].FloorAssetId = null;
        //    tInspection.TInspectionActivity[0].InspectionId = tInspection.InspectionId;
        //    tInspection.TInspectionActivity[0].CreatedBy = 4;
        //    tInspection.TInspectionActivity[0].TInspectionDetail = new List<TInspectionDetail>();
        //    for (int i = 0; i < maingoals.Count; i++)
        //    {
        //        TInspectionDetail tInspectionDetail =
        //            new TInspectionDetail
        //            {
        //                MainGoalId = maingoals[i].MainGoalId,
        //                InspectionDetailId = Convert.ToInt32(maingoals[i].InspectionDetailId)
        //            };
        //        tInspection.TInspectionActivity[0].TInspectionDetail.Add(tInspectionDetail);
        //        tInspection.TInspectionActivity[0].TInspectionDetail[i].InspectionSteps = new List<InspectionSteps>();
        //        for (int j = 0; j < maingoals[i].Steps.Count; j++)
        //        {
        //            InspectionSteps tInspectionSteps =
        //                new InspectionSteps
        //                {
        //                    StepsId = maingoals[i].Steps[j].StepsId,
        //                    Status = 1,
        //                    Comments = maingoals[i].Steps[j].Comments,
        //                    FileContent = maingoals[i].Steps[j].FileContent,
        //                    FileName = maingoals[i].Steps[j].FileName,
        //                    FilePath = maingoals[i].Steps[j].FilePath,
        //                    IsRA = maingoals[i].Steps[j].IsRA,
        //                    RAScore = maingoals[i].Steps[j].RAScore,
        //                    //Steps = HCF.BAL.StepsRepository.GetStep(maingoals[i].Steps[j].StepsId),
        //                    IsMarkDefeciencies = maingoals[i].Steps[j].IsMarkDefeciencies,
        //                    DRTime = maingoals[i].Steps[j].IsMarkDefeciencies == true ? 0 : maingoals[i].Steps[j].DRTime,
        //                    InputValue = maingoals[i].Steps[j].InputValue
        //                };
        //            tInspection.TInspectionActivity[0].TInspectionDetail[i].InspectionSteps.Add(tInspectionSteps);
        //        }
        //    }
        //    if (ActivityType == 3)
        //    {
        //        tInspection.TInspectionActivity[0].InspectionDocs = new List<InspectionEPDocs>();
        //        InspectionEPDocs inspectiondocs = new InspectionEPDocs
        //        {
        //            DocTypeId = 8
        //        };
        //        inspectiondocs.Path = "~/Files/TInspectionEPDocs/338/636456547599546170__SampleDOCFile_100kb.doc";
        //        //inspectiondocs.FilesContent = fileContent;
        //        inspectiondocs.DocumentName = "SampleDOCFile_100kb.doc";
        //        inspectiondocs.EffectiveDate = Conversion.ConvertToTimeSpan(Convert.ToDateTime(DateTime.UtcNow));
        //        tInspection.TInspectionActivity[0].InspectionDocs.Add(inspectiondocs);
        //    }
        //    if (ActivityType == 1)
        //    {
        //        AddInspectionManual(tInspection, "EP", true);
        //    }
        //    else if (ActivityType == 3)
        //    {
        //        AddInspectionManual(tInspection, "DOC", true);
        //    }
        //}

        #endregion

        #region Add Fixed Inspection Date

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addTEPInspectionDate")]
        public ActionResult AddTEpInspectionDate(List<EPDetails> epList)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var item in epList)
            {
                foreach (var inspectiondate in item.InspectionDates)
                {
                    if (inspectiondate.InspectionDate != null && inspectiondate.InspectionDate != default(DateTime))
                    {
                        _tEPInspectionService.SaveEpInspectionDate(item.EPDetailId, inspectiondate.InspectionDate.Value);
                    }
                }

            }
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addFixedInspectionDate")]
        public ActionResult AddFixedInspectionDate(List<AssetsFixInsDate> lstAssetsFixInsDate)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var item in lstAssetsFixInsDate)
            {
                if (item.InspectionDate != null && item.InspectionDate != default(DateTime))
                {
                    _tEPInspectionService.SaveFixedInspectionDate(item);
                }
            }
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public ActionResult AddInspectionDueDate()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _tEPInspectionService.AddInspectionDueDate();
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);

        }

        #endregion

        #region Service Email

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult SendDueDateEmail()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _transactionService.SendDueDateEmail();
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult SendConfigurationEmail()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _commonService.SendConfigurationEmail();
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);

        }

        #endregion

        #region

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult GetInspectionCal(string startTimeSpan, string endTimeSpan)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            DateTime startDate = Conversion.ConvertToDateTime(Convert.ToDouble(startTimeSpan));
            DateTime endDate = Conversion.ConvertToDateTime(Convert.ToDouble(endTimeSpan));
            var list = _transactionService.GetInspectionsForCalendar(4, startDate, endDate);
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Result = new Result();
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result.Inspections = list;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return Ok(_objHttpResponseMessage);
        }


        #endregion

        #region RouteInspection 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("saverouteinspection")]
        public ActionResult SaveRouteInspection(Inspection inspection)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var inspectionActicity in inspection.TInspectionActivity)
            {
                DateTime inspectiontime = inspectionActicity.InspectionDateTimeSpan > 0 ? Conversion.ConvertToDateTime(inspectionActicity.InspectionDateTimeSpan) : DateTime.UtcNow;
                inspectionActicity.ActivityInspectionDate = inspectiontime;
                inspectionActicity.ActivityId = Guid.NewGuid();
                inspectionActicity.InsType = 1;
                inspectionActicity.Status = inspectionActicity.Status > 0 ? inspectionActicity.Status : SetActivityStatus(inspectionActicity, true);
                inspectionActicity.SubStatus = string.IsNullOrEmpty(inspectionActicity.SubStatus) ? CalculateActivitySubStatus(inspection) : inspectionActicity.SubStatus;
                var tinsActivityId = _transactionService.AddTInspectionActivity(inspectionActicity);
                inspectionActicity.TInsActivityId = tinsActivityId;
                if (tinsActivityId > 0)
                {
                    foreach (var inspectionDetail in inspectionActicity.TInspectionDetail)
                    {
                        List<int> lists = inspectionDetail.InspectionSteps.Where(x => x.Status == 1 && string.IsNullOrEmpty(x.FileName) && string.IsNullOrEmpty(x.Comments) && string.IsNullOrEmpty(x.InputValue)).Select(x => x.StepsId).ToList();
                        string stepsId = string.Join(",", lists);
                        inspectionDetail.ActivityId = inspectionActicity.ActivityId;
                        inspectionDetail.InspectionDetailId = _transactionService.AddInspectionDetails(inspectionDetail, stepsId);
                        if (inspectionDetail.InspectionDetailId > 0)
                            AddInspectionSteps(tinsActivityId, inspectionDetail);
                    }
                    if (inspectionActicity.Status == 0)
                    {
                        var objNotificationEmails = new NotificationEmails()
                        {
                            NotificationCatId = Convert.ToInt32(BDO.Enums.NotificationCategory.Inspection),
                            NotificationEventId = Convert.ToInt32(BDO.Enums.NotificationEvent.OnDeficiencies)
                        };
                        _emailService.SendCommonEmail(objNotificationEmails, inspection);
                        //if (CommonRepository.IsSendMail(Enums.NotificationCategory.RouteBasedInspection.ToString(), Enums.NotificationEvent.OnFailed.ToString()))
                        //{
                        //    Email.SendInspectionFailMail(inspection, Convert.ToInt32(Enums.NotificationCategory.RouteBasedInspection));
                        //}
                    }
                }
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    Inspection = inspection
                };
            }
            return Ok(_objHttpResponseMessage);

        }
        #endregion       
    }
}