using System;
using System.Collections.Generic;
using System.Linq;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
   
    [Route("api/reports")]   
    [ApiController]
    public class ReportsApiController : ApiController
    {


        #region ctor
        private readonly IConstructionService _constructionService;
        private readonly IReportsService _reportsService;
        private readonly IFloorAssetService _floorAssetService;
        private readonly ILoggingService _loggingService;
        private readonly IAssetsService _assetService;
        private readonly IEmailService _emailService;
        private readonly IApiCommon _apiCommon;
        public ReportsApiController(IConstructionService constructionService, IReportsService reportsService, IFloorAssetService floorAssetService,
            IApiCommon apiCommon, ILoggingService loggingService, IAssetsService assetService, IEmailService emailService)
        {
            _apiCommon = apiCommon;
            _loggingService = loggingService; _assetService = assetService;
            _emailService = emailService;
            _constructionService = constructionService;
            _reportsService = reportsService;
            _floorAssetService = floorAssetService;
        }

        #endregion


        #region Inspection Reports

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newReport"></param>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("saveinsreport")]
        public HttpResponseMessage Save(TInspectionReport newReport)
        {
           var _objHttpResponseMessage = new HttpResponseMessage();
            TFloorAssets tfloorAssets = _floorAssetService.GetFloorAssetDescription(newReport.TInsReportDetails.FirstOrDefault().FloorAssetId.Value);
            string fileName = DateTime.UtcNow.Ticks.ToString();
            if (tfloorAssets != null)
            {
                fileName =
                    $"{tfloorAssets.Assets.Name.Substring(0, 4)}_{DateTime.Now.Date.Day}{DateTime.Now.ToString("MMM")}{DateTime.Now.Year}_{DateTime.Now.Minute}{DateTime.Now.Second}_{newReport.SignImageName.Split('.')[0]}";
            }
            newReport.ReportName = fileName.ToUpper();
            newReport.FileName = newReport.ReportName + ".pdf";
            int reportId = _reportsService.Save(newReport);
            if (reportId > 0)
            {
                newReport.ReportId = reportId;
                //foreach (Inspection inspection in newReport.Inspections)
                //{
                //    foreach (TInspectionActivity activity in inspection.TInspectionActivity)
                //    {
                //        if (activity.ActivityType == 2)
                //            _reportsService.Save(reportId, inspection.InspectionId, inspection.EPDetailId, activity.FloorAssetId.Value, activity.ActivityId);
                //    }
                //}
                foreach (TInsReportDetails details in newReport.TInsReportDetails)
                {
                    details.ReportId = reportId;
                    _reportsService.Save(details);
                }
                SaveReportFile(newReport);

                //objHttpResponseMessage.Result = new Result
                //{
                //    TInspectionReport = newReport
                //};
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Success;

            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetInspectionReport(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TInspectionReport> reports = _reportsService.GetInspectionReport();
            if (reports.Count > 0)
            {
                _objHttpResponseMessage.Success = true; _objHttpResponseMessage.Result = new Result
                {
                    TInspectionReports = reports
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Methods

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private void SaveReportFile(TInspectionReport report)
        {
            if (!string.IsNullOrEmpty(report.FilesContent))
            {
                Files newFile = _apiCommon.SaveFile(report.FilesContent, report.FileName, "InspectionReport", Convert.ToString(report.ReportId));
                report.FileName = newFile.FileName;
                report.FilePath = newFile.FilePath;
            }
            if (!string.IsNullOrEmpty(report.SignContent))
            {
                Files signaturePath = _apiCommon.SaveFile(report.SignContent, report.SignImageName, "InspectionReport", Convert.ToString(report.ReportId));
                report.SignPath = signaturePath.FilePath;
            }
            _reportsService.Save(report);
        }

        #endregion

        #region GetAssetsInventoryReport

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAssetsInventoryReport(string userId, string assetIds, string buildingId, string floorId, string fromdatetimespan, string todatetimespan)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            string fromdate = Conversion.ConvertToDateTime(Convert.ToDouble(fromdatetimespan)).ToShortDateString();
            string todate = Conversion.ConvertToDateTime(Convert.ToDouble(todatetimespan)).ToShortDateString();
            List<TFloorAssets> floorAssets = _assetService.InventoryAssetsReport(assetIds, buildingId, floorId, fromdate, todate);
            _objHttpResponseMessage.Result = new Result { TFloorAssets = floorAssets };
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;

        }

        #endregion

        #region CRx Auto Report


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getAutoReportdata")]
        public HttpResponseMessage GetAutoReportdata()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            _objHttpResponseMessage = _reportsService.GetAutoReportdata();
            if (_objHttpResponseMessage.Result != null)
            {
                _emailService.SendAutoReportEmails(_objHttpResponseMessage, Convert.ToInt32(BDO.Enums.NotificationCategory.AutoReport));
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region All Permit Report
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getAllPermitReport/{fromdate}/{todate}/{permittype}")]
        public HttpResponseMessage GetAllPermitReport(string fromdate, string todate, string permittype)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();

            AllPermitReport permitcount = _constructionService.GetAllPermitCount(fromdate, todate, permittype);
            _objHttpResponseMessage.Result = new Result { AllPermitReport = permitcount };
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }
        #endregion
    }
}