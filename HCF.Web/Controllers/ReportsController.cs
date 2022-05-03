using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BDO;
using HCF.BAL;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Text;
using HCF.Web.Base;
using HCF.Web.Filters;
using Newtonsoft.Json;
using HCF.BAL.Interfaces.Services;
using HCF.BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using HCF.Utility;
using HCF.Utility.Extensions;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly ISiteService _siteService;
        private readonly ITransactionService _transactionService;
        private readonly IEpService _epService;      
        private readonly IAssetsService _assetService;
        private readonly IReportsService _reportService;     
        private readonly IIlsmService _ilsmService;
        private readonly IEPSchedulerService _ePSchedulerService;
        private readonly IStandardService _standardService;
        private readonly IAssetsService _assetsService;
        private readonly IUserService _userService;
        private readonly IRoundInspectionsService _roundInspectionsService;       
        private readonly IHttpPostFactory _httpService;
        private readonly IHCFSession _session;
        private readonly ICommonModelFactory _commonModelFactory;

        public ReportsController(ICommonModelFactory commonModelFactory,ISiteService siteService, ITransactionService transactionService, IEpService epService,
            IAssetsService assetService, IReportsService reportService,
            IIlsmService ilsmService,
            IEPSchedulerService ePSchedulerService, IStandardService standardService, IAssetsService assetsService, IUserService userService,
            IRoundInspectionsService roundInspectionsService, IHttpPostFactory httpService, IHCFSession session
            )
        {
            _commonModelFactory = commonModelFactory;
            _session = session;           
            _roundInspectionsService = roundInspectionsService;
            _standardService = standardService;
            _ePSchedulerService = ePSchedulerService;
            // _inspectionActivityService = inspectionActivityService;
            _siteService = siteService;
            _transactionService = transactionService;
            _epService = epService;
            _assetService = assetService;
            _reportService = reportService;
           // _inspectionService = inspectionService;
            _ilsmService = ilsmService;
            _assetsService = assetsService;
            _userService = userService;
            _httpService = httpService;
        }


        #region Compliance

        [CrxAuthorize(Roles = "reports_compliance")]
        public ActionResult ComplianceIndex(string assetids)
        {
            var assetIds = _session.Get<string>("AssetIDs");

            UISession.AddCurrentPage("Reports_ComplianceIndex", 0, "Compliance Report");
            ViewBag.SelectedAssetIDs = (!string.IsNullOrEmpty(assetIds)) ? assetIds : null;
            if (assetids != null)
            {
                ViewBag.SelectedAssetIDs = assetids;               
            }
            return View();
        }

        public ActionResult ComplianceReport(string assetIds, string buildingId, string floorId, string fromdate, string todate, string status)
        {
            var inspectionActivity = _transactionService.GetComplianceReports(assetIds, buildingId, floorId, fromdate, todate, status);
            return PartialView("_complianceIndex", inspectionActivity);
        }



        #endregion

        #region Assets

        [CrxAuthorize(Roles = "repors_assets")]
        public ActionResult AssetsIndex()
        {
            UISession.AddCurrentPage("Reports_AssetsIndex", 0, "Assets");
            return View();
        }

        public ActionResult AssetsReports(string assetIds, string buildingId, string floorId, string fromdate, string todate)
        {
            var model = _assetService.GetFilterAssetsReports(assetIds, buildingId, floorId, fromdate, todate);
            return PartialView("_assetReports", model);
        }

        #endregion

        #region reports_risk_management_reports

        [CrxAuthorize(Roles = "goal_risk_management")]
        public ActionResult RiskManagementIndex()
        {
            UISession.AddCurrentPage("Reports_RiskManagementIndex", 0, "ILSM Reports");
            List<EPDetails> epDetails = _epService.GetDeficientEPs(UserSession.CurrentUser.UserId);
            return View(epDetails);
        }

        #endregion

        #region reports_documents

        [CrxAuthorize(Roles = "reports_documents")]
        public ActionResult DocumentsIndex()
        {
            UISession.AddCurrentPage("Reports_DocumentsIndex", 0, "Documents");
            return View();
        }


        public ActionResult DocumentsReport(string fromdate, string todate, int documenttype)
        {

            //Session["combinevaluearr"]
            var combinevaluearr = fromdate + "," + todate + "," + documenttype;
            _session.Set("combinevaluearr", combinevaluearr);

            var model = _transactionService.GetDocumentReport(UserSession.CurrentUser.UserId);
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate) && documenttype != 0)
            {
                model = model.Where(x => x.DtEffectiveDate.Value.Date >= Convert.ToDateTime(fromdate).Date
                && x.DtEffectiveDate.Value.Date <= Convert.ToDateTime(todate).Date && x.UploadDocTypeId == documenttype).ToList();
            }
            else
            {
                model = model.Where(x => x.DtEffectiveDate.Value.Date >= Convert.ToDateTime(fromdate).Date
               && x.DtEffectiveDate.Value.Date <= Convert.ToDateTime(todate).Date).ToList();
            }
            return PartialView("~/Views/Reports/_documentsIndex.cshtml", model);
        }
        public ActionResult EpDocs(int? epDetailId)
        {
            if (!epDetailId.HasValue)
                return RedirectToRoute("dashboard");

            UISession.AddCurrentPage("Reports_EpDocs", 0, "EP Document History");
            var epDetails = _epService.GetEpDescription(epDetailId.Value);

            if (epDetails != null)
            {
                epDetails.EpDocs = _transactionService.GetEpsDocument(epDetails.EPDetailId);
            }
            return View(epDetails);
        }
        #endregion

        #region reports_epassignments

        [CrxAuthorize(Roles = "reports_epassignmentsindex")]
        public ActionResult EpAssignmentsIndex()
        {
            UISession.AddCurrentPage("Reports_EpAssignmentsIndex", 0, "EP Assignments");           
            var filterUsers = _userService.GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && !x.IsCRxUser).OrderBy(x => x.FullName).ToList();

            return View(filterUsers);
        }

        public ActionResult EpAssignmentsView()
        {
            var epdetails = _epService.GetEpAssignment().ToList();
            return PartialView("_epassignmentReport", epdetails);
        }

        public ActionResult EmailEpAssignmentReport()
        {
            return PartialView("_ModalEmailEpAssignmentReport");
        }

        #endregion

        private void SetAssetList()
        {
            var assetList = _assetService.GetAssets(UserSession.CurrentUser.UserId);
            assetList.Insert(0, new Assets { AssetId = 0, Name = "All" });

            var listYear = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = @"All",
                    Value = "All"
                }
            };
            int startyear = DateTime.Now.AddYears(-7).Year;
            for (int i = startyear; i <= startyear + 7; i++)
            {
                listYear.Add(new SelectListItem
                {
                    Text = "" + i,
                    Value = "" + i
                });
            }

            ViewBag.YearFilter = listYear;
            ViewBag.AssetTypeList = new SelectList(assetList, "AssetId", "Name");
        }

        [CrxAuthorize(Roles = "reports_sign")]
        public ActionResult Reports(string epId)
        {
            var signedAssetIDs = _session.Get<string>("SignedAssetIDs");
            UISession.AddCurrentPage("Reports_Reports", 0, "Signed Reports");
            ViewBag.SignedAssetIDs = signedAssetIDs;
            if (!string.IsNullOrEmpty(epId))
            {
                ViewBag.SignedAssetIDs = epId;
            }
            var lists = _assetsService.Get().Where(x => x.IsActive).ToList();
            return View(lists);
        }  

        public ActionResult ReportsPartial(int? epId, string fromdate, string todate, string assetids, int? signedby)
        {
            UISession.AddCurrentPage("Reports_Reports", 0, "Signed Reports");
            var tinspectionReports = new List<TInspectionReport>();
            if (epId != null)
            {
                tinspectionReports = _reportService.GetInspectionReport(assetids, epId.Value, Convert.ToDateTime(fromdate), Convert.ToDateTime(todate), signedby);
            }
            else { tinspectionReports = _reportService.GetInspectionReport(assetids, null, Convert.ToDateTime(fromdate), Convert.ToDateTime(todate), signedby); }
            return PartialView("_Reports", tinspectionReports);


        }

        #region Signed Reports

        public ActionResult GenerateReports()
        {
            UISession.AddCurrentPage("Reports_GenerateReports", 0, "Review & Finalize Reports");
            List<Inspection> inspectionlist = _transactionService.GetComplianceReport().Where(x => x.TInspectionActivity.Any(y => y.ActivityType == 2)).ToList();
            foreach (var item in inspectionlist)
            {
                item.TInspectionActivity = item.TInspectionActivity.Where(m => !m.IsSubmit).ToList();
            }
            return View(inspectionlist);
        }

        [HttpPost]
       
        [ValidateAntiForgeryToken]
        public JsonResult CreateReports(TInspectionReport tInspectionReport, string htmlcontent)
        {
            tInspectionReport.CreatedBy = UserSession.CurrentUser.UserId;
            tInspectionReport.FilesContent = Convert.ToBase64String(createPDF(htmlcontent));
            string postData = _commonModelFactory.JsonSerialize<TInspectionReport>(tInspectionReport);
            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, APIUrls.Reports_CreateReports, ref StatusCode);
            return Json(new { Result = tInspectionReport.TInsReportDetails });
        }

        public static byte[] createPDF(string htmlstr)
        {
            var html = htmlstr.Replace("controlname=\"addCloseTag\">", "controlname=\"addCloseTag\" />");

            // step 1: creation of a document-object  
            using (Document document = new Document(PageSize.A4, 30, 30, 30, 30))
            {
                using (MemoryStream msOutput = new MemoryStream())
                {
                    // step 2:
                    // we create a writer that listens to the document
                    // and directs a XML-stream to a file
                    using (PdfWriter writer = PdfWriter.GetInstance(document, msOutput))
                    {
                        // step 3: we create a worker parse the document
                        //HTMLWorker worker = new HTMLWorker(document);
                        //writer.setInitialLeading(12);
                        document.Open();
                        //var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                        //tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                        //tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                        //CssFilesImpl cssFiles = new CssFilesImpl();
                        //cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                        //var cssResolver = new StyleAttrCSSResolver(cssFiles);
                        //cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                        //var charset = Encoding.UTF8;
                        //var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                        //hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                        //var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(document, writer));
                        //var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                        //var workers = new XMLWorker(pipeline, true);
                        //var xmlParser = new XMLParser(true, workers, charset);
                        //xmlParser.Parse(new StringReader(html));
                        writer.CloseStream = false;
                        document.Close();
                    }
                    // Return the bytes
                    return msOutput.ToArray();
                }
            }
        }

        public ActionResult GetInspectionActivity(int inspectionId)
        {
            var inspection = _transactionService.GetComplianceReport().FirstOrDefault(x => x.InspectionId == inspectionId);
            return PartialView("_GetInspectionActivity", inspection);
        }

        public ActionResult PreviewReports(List<int> _tinsActivityId)
        {
            var report = new TInspectionReport();
            var inspectionlist = _transactionService.GetComplianceReport().Where(x => x.TInspectionActivity.Any(y => y.ActivityType == 2)).ToList();
            foreach (var item in inspectionlist)
            {
                item.TInspectionActivity = item.TInspectionActivity.Where(m => _tinsActivityId.Contains(m.TInsActivityId)).ToList();
            }
            ViewBag.Inspectionlist = inspectionlist;
            TempData.Keep();
            return PartialView("~/Views/Shared/PopUp/_ReportPreview.cshtml", report);
        }

        #endregion

        #region Employee

        // UserDataSet ds = new UserDataSet();
        //public ActionResult ReportEmployee()
        //{
        //    var reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Local,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(900),
        //        Height = Unit.Percentage(900)
        //    };






        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\Users.rdlc";
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("EpDataSet", users));
        //    ViewBag.ReportViewer = reportViewer;
        //    return View();
        //}


        //public ActionResult Report()
        //{
        //    UISession.AddCurrentPage("Reports_Report", 0,  "Report");
        //    ReportViewer rptViewer = new ReportViewer
        //    {

        //        // ProcessingMode will be Either Remote or Local  
        //        ProcessingMode = ProcessingMode.Remote,
        //        SizeToReportContent = true,
        //        ZoomMode = ZoomMode.PageWidth,
        //        Width = Unit.Percentage(99),
        //        Height = Unit.Pixel(1000),
        //        AsyncRendering = true
        //    };
        //    rptViewer.ServerReport.ReportServerUrl = new Uri("http://localhost/ReportServer/");
        //    List<EPDetails> users = BAL.EpRepository.GetEpDetails();
        //    rptViewer.ServerReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\Users.rdlc";
        //    rptViewer.LocalReport.DataSources.Add(new ReportDataSource("EpDataSet", users));
        //    ViewBag.ReportViewer = rptViewer;
        //    return View();
        //}


        #endregion

        #region InspectionDetail Report

        // [CrxAuthorize(Roles = "reports_scheduler")]
        public ActionResult InspectionDetailReports()
        {
            UISession.AddCurrentPage("Reports_InspectionDetailReports", 0, "Compliance History");
            var users = _userService.GetUsers().Where(x => (x.IsActive && !string.IsNullOrEmpty(x.Email) && x.EpsCount > 0) || x.UserId == HCF.Web.Base.UserSession.CurrentUser.UserId).OrderBy(x => x.FullName).ToList();

            return View(users);
        }


        public ActionResult InspectionSummary(int year, int userId, string sortOrder = "", string OrderbySort = "", int? categoryId = null)
        {
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "StandardEP" : sortOrder;
            OrderbySort = string.IsNullOrEmpty(OrderbySort) ? "ASC" : OrderbySort;
            var currentUserId = UserSession.CurrentUser.UserId;
            var InspectionReport = _reportService.GetInspectionDetailReports(sortOrder, OrderbySort, year, userId, currentUserId, categoryId);
            // List<EPScheduler> ePScheduler = EPSchedulerRepository.GetEPSchedulers();
            // ViewBag.EPScheduler = ePScheduler;
            return PartialView("_InspectionSummary", InspectionReport);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveInspectionDate(int date, int year, int month, int epDetailld)
        {
            EPScheduler ePScheduler = new EPScheduler
            {
                Month = month,
                Year = year,
                Day = date,
                EpDetailld = epDetailld,
                CreatedBy = UserSession.CurrentUser.UserId
            };
            var result = _ePSchedulerService.Save(ePScheduler);
            return Json(new { Result = result });
        }
        #endregion

        #region Document Review Report

        public ActionResult DocumentReviewReports()
        {
            var standards = _standardService.DocumentStandardReports();
            return View(standards);
        }

        #endregion

        #region Assets Generate Report
        [CrxAuthorize(Roles = "repository_generatereport")]
        public ActionResult AssetsInspectionReport()
        {
            UISession.AddCurrentPage("Reports_AssetsInspectionReport", 0, "Review & Finalize Reports");
            return View();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public ActionResult GetFloorAssetsForReports(string buildingId, string floorId, string assetId, string ascIds)
        {
            var floorAssets = _reportService.GetFloorAssetsForReports(assetId, ascIds);
            if (!string.IsNullOrEmpty(buildingId) && floorId != string.Empty && floorId != "null")
            {
                floorAssets = floorAssets.Where(x => x.Floor.FloorId == Convert.ToInt32(floorId) && x.Floor.Building.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            else if (!string.IsNullOrEmpty(buildingId) && floorAssets.Any())
            {
                floorAssets = floorAssets.Where(x => x.FloorId.HasValue && x.Floor.Building.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            return PartialView("_GetFloorAssetsForReports", floorAssets);
        }

        #endregion

        #region Inventory assets report 

        [CrxAuthorize(Roles = "reports_inventoryreport")]
        public ActionResult InventoryAssetsReport(string assetIds = null, string buildingId = null, string floorId = null, string ids = null)
        {
            var selectedInventoryAssetIDs = _session.Get<string>("SelectedInventoryAssetIDs");
            UISession.AddCurrentPage("Reports_InventoryAssetsReport", 0, "Inventory Assets Report");          
            ViewBag.AssetsId = assetIds;
            ViewBag.BuildingId = !string.IsNullOrEmpty(buildingId) ? Convert.ToInt32(buildingId) : 0;
            ViewBag.FloorId = !string.IsNullOrEmpty(floorId) ? Convert.ToInt32(floorId) : 0;
            ViewBag.SelectedInventoryAssetIDs = selectedInventoryAssetIDs;
            if (!string.IsNullOrEmpty(ids))
            {
                ViewBag.SelectedInventoryAssetIDs = ids;
            }

            var model = new List<TFloorAssets>();
            return View(model);
        }

        public ActionResult InventoryReport(string assetIds, string buildingId, string floorId, string fromdate, string todate, string status)
        {
            var model = _assetService.InventoryAssetsReport(assetIds, buildingId, floorId, fromdate, todate, status);
            return PartialView("_InventoryAssetReports", model);
        }

        #endregion 

        #region Ilsm Report

        [CrxAuthorize(Roles = "reports_risk_management_reports")]
        public ActionResult IlsmReports()
        {
            UISession.AddCurrentPage("Reports_IlsmReports", 0, "ILSM Reports");
            return View();
        }
        public ActionResult IlsmReportsPartial(string fromdate, string todate)
        {
            UISession.AddCurrentPage("Reports_IlsmReports", 0, "ILSM Reports");
            var lsttilsm = _ilsmService.GetAllIlsm(Convert.ToDateTime(fromdate), Convert.ToDateTime(todate)).Where(x => x.FilePath != string.Empty).ToList();
            return PartialView("_IlsmReports", lsttilsm);
        }
        #endregion


        #region  Assets Inventory Inspection Report

        public ActionResult AssetsInventoryInspectionReport(string assetIds = null, string buildingId = null, string floorId = null)
        {
            var inventoryAssetIDs = _session.Get<string>("InventoryAssetIDs");
            UISession.AddCurrentPage("Reports_AssetsInventoryInspectionReport", 0, "Assets Inventory Inspection Report");          
            ViewBag.AssetsId = assetIds;
            ViewBag.BuildingId = !string.IsNullOrEmpty(buildingId) ? Convert.ToInt32(buildingId) : 0;
            ViewBag.FloorId = !string.IsNullOrEmpty(floorId) ? Convert.ToInt32(floorId) : 0;
            ViewBag.InventoryAssetIDs = inventoryAssetIDs;
            if (assetIds != null)
            {
                ViewBag.InventoryAssetIDs = assetIds;
            }
            var model = new List<TFloorAssets>();
            return View(model);
        }

        public ActionResult GetAssetsInventoryInspectionReport(int? page, string assetIds, string buildingId, string floorId, string fromdate, string todate, string status, bool isScroll = false)
        {
            DateTime from = Convert.ToDateTime(fromdate);
            DateTime To = Convert.ToDateTime(todate);
            fromdate = from.ToShortDateString();
            todate = To.AddDays(1).ToShortDateString();
            var objrequest = new Request();
            var fixedPageSize = 50;
            int fixedPageIndex;
            var pageIndex = 0;
            int pageSize;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : pageIndex;
            if (isScroll)
            {
                fixedPageIndex = (pageIndex - 1) * fixedPageSize;
                pageSize = fixedPageSize;
            }
            else
            {
                pageSize = pageIndex * fixedPageSize;
                fixedPageIndex = 0;
            }

            objrequest.PageIndex = fixedPageIndex;
            objrequest.PageSize = pageSize > 0 ? pageSize : fixedPageSize;
            var model = _assetService.InventoryInspectionAssetsReport(objrequest, assetIds, buildingId, floorId, fromdate, todate, status);
            //TempData["AssetsReportdata"] = model;
            TempData.Put("AssetsReportdata", model);
            //ViewBag.AssetsReportdata = model;

            ViewBag.isDataExists = model.Count;
            return PartialView("_AssetsInventoryInspectionReport", model);
        }



        #endregion

        #region Round Reports

        public ActionResult RoundReports()
        {
            UISession.AddCurrentPage("Reports_RoundReports", 0, "Round Reports");
            var roundUserGroup = _roundInspectionsService.GetRoundUserGroup().Where(x => x.IsActive==1).ToList();
            return View(roundUserGroup);
        }
        public ActionResult GetRoundReportData(int locationGroupId, string from, string todate)
        {
            var RoundReportData = _reportService.GetRoundReport(locationGroupId, from, todate);
            return PartialView("_RoundReport", RoundReportData);
        }

        public ActionResult ManagementReport()
        {
            UISession.AddCurrentPage("Reports_ManagementReports", 0, "Management Reports");
            return View();
        }

        public ActionResult GetAssetDeficiencyData(string from, string todate)
        {
            var AssetDeficiencyData = _reportService.GetAssetDeficiencyData(from, todate);
            return PartialView("_AssetDeficiencyData", AssetDeficiencyData);
        }

        #endregion

        #region Compliance Assets Tracking Reports

        public ActionResult ComplianceAssetsTrackingReports(int? assetId)
        {
            UISession.AddCurrentPage("Reports_ComplianceAssetsTrackingReports", 0, "Asset Compliance Matrix");
            var Campusite = _siteService.GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            var GetCampus = new SelectList(Campusite, "Code", "Name", "select");
            ViewBag.GetCampus = JsonConvert.SerializeObject(GetCampus);
            ViewBag.AssetId = assetId.HasValue ? assetId.Value : 0;
            return View();
        }

        public ActionResult GetComplianceAssetsTrackingReports(string assetIds, string campusid, string buildingId)
        {
            var data = new List<object>();
            var result = _reportService.GetComplianceAssetsTrackingReports(assetIds, campusid, buildingId).ToList();
            ViewBag.Site = result[0];
            ViewBag.Buildings = result[1];
            ViewBag.assettype = result[2];
            ViewBag.assets = result[3];
            ViewBag.results = result[4];
            //ViewBag.FloorAssetdata = JsonConvert.SerializeObject(result[4]);
            //ViewBag.datalist = JsonConvert.SerializeObject(data);
            //ViewBag.TrackAssetIds = assetIds;
            //ViewBag.TrackCampusIds = campusid;
            //ViewBag.TrackBuildingIds = buildingId;
            return PartialView("_ComplianceAssetsTrackingReports");
        }

        #endregion


        #region EP Reports

        public ActionResult ArchivedEPsReport()
        {
            UISession.AddCurrentPage("Reports_ArchivedEPsReport", 0, "Non Active EP Reports");
            List<EPDetails> eps = _transactionService.GetArchivedEPsInspection(UserSession.CurrentUser.UserId);
            return View(eps);
        }

        #endregion

    }
}



