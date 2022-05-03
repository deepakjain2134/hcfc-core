using HCF.BAL;
using HCF.BAL.Interfaces;
using HCF.BAL.Interfaces.Services;
using HCF.BAL.Ioc;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using HCF.Web.Helpers;
using HCF.Web.Localize;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using HCF.Utility.Extensions;
using HCF.BDO.Enums;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class PdfController : BaseController
    {
        private readonly IAssetsService _assetService;
        private readonly IEpService _epService;
        private readonly IFireWatchService _fireWatchService;
        private readonly ICommonService _commonService;
        private readonly IIlsmService _ilsmService;
        private readonly IExercisesService _exercisesService;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;
        private readonly IHotWorkPermitService _hotWorkPermitService;
        private readonly IPCRAService _pcraService;
        private readonly IConstructionService _constructionService;
        private readonly IPermitService _permitService;
        private readonly IInsActivityService _insActivityService;
        private readonly IRoundsService _roundsService;
        private readonly IHttpPostFactory _httpService;
        private readonly IPdfService _pdfService;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ISiteService _siteService;
        private readonly IHCFSession _session;
        private readonly IEmailProcessor _emailProcessor;
        private readonly ICommonProvider _commonProvider;
        private readonly IFireExtinguisherService _fireExtinguisherService;

        public PdfController(IWebHostEnvironment hostingEnvironment, ICommonModelFactory commonModelFactory,
            IInsActivityService insActivityService,
            IPCRAService pcraService, IUserService userService,
            IExercisesService exercisesService, IIlsmService ilsmService, IAssetsService assetService, IEpService epService, IFireWatchService fireWatchService,
            ICommonService commonService, ITransactionService transactionService, IHotWorkPermitService hotWorkPermitService, IConstructionService
            constructionService, ICommonProvider commonProvider,
            IHttpPostFactory httpService,
            IPermitService permitService, IEmailProcessor emailProcessor, IRoundsService roundsService, IPdfService pdfService, ISiteService siteService, IHCFSession session, IEmailService emailService,
            IFireExtinguisherService fireExtinguisherService)

        {
            _httpService = httpService;
            _commonModelFactory = commonModelFactory;
            _insActivityService = insActivityService;
            _pcraService = pcraService;
            _userService = userService;
            _exercisesService = exercisesService;
            _ilsmService = ilsmService;
            _commonService = commonService;
            _assetService = assetService;
            _epService = epService;
            _fireWatchService = fireWatchService;
            _transactionService = transactionService;
            _hotWorkPermitService = hotWorkPermitService;
            _constructionService = constructionService;
            _permitService = permitService;
            _roundsService = roundsService;
            _pdfService = pdfService;
            _siteService = siteService;
            _session = session;
            _emailProcessor = emailProcessor;
            _commonProvider = commonProvider;
            _fireExtinguisherService = fireExtinguisherService;
        }


        #region Color       

        public static readonly System.Drawing.Color GrayColor = System.Drawing.Color.FromArgb(231, 231, 231);
        public static readonly BaseColor Gray = new BaseColor(GrayColor);

        public static readonly System.Drawing.Color CyanColor = System.Drawing.Color.FromArgb(0, 162, 232);
        public static readonly BaseColor CYAN = new BaseColor(CyanColor);

        #endregion

        #region Font

        private static readonly Font CellFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        static readonly Font CellBoldFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
        static readonly Font smallfont = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);
        static readonly Font smallfontbold = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
        static readonly Font CellFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        static readonly Font UnderlineTitleBoldFont = new Font(Font.FontFamily.HELVETICA, 11.5f, Font.BOLD | Font.UNDERLINE);
        static readonly Font UnderlineCellBoldFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD | Font.UNDERLINE);
        static readonly Font UnderlineCellFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.UNDERLINE);
        static readonly Font CellFontSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        static readonly Font CellFontDatetimeblue = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, CYAN);
        static readonly Font CellFontBold = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD);
        static readonly Font TitleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.NORMAL);
        static readonly Font TitleFontS = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
        static readonly Font UnderlineTitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.UNDERLINE);
        static readonly Font ParagraphFont = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
        static readonly Font ParagraphFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        static readonly Font Strikethru = new Font(Font.FontFamily.HELVETICA, 11, Font.STRIKETHRU);
        static readonly Font CellFontBoldSmall = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
        static readonly Font CellFontSmall1 = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.NORMAL);
        static readonly Font CellFontSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.NORMAL);
        static readonly Font CellFontBoldSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.BOLD, BaseColor.BLACK);
        static readonly Font CellFontBoldBlue = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
        static readonly Font CellFontBoldBlack = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
        static readonly Font CellFontBoldBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.BLACK);
        static readonly Font CellFontNormalBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, BaseColor.BLACK);
        static readonly Font CellStatusFont = new Font(Font.FontFamily.HELVETICA, 14f, Font.BOLD, BaseColor.RED);
        static readonly Font CellRedtext = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.RED);
        static readonly Font CellFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD, BaseColor.WHITE);
        static readonly Font TitleFontHWP = new Font(Font.FontFamily.HELVETICA, 76f, Font.NORMAL, BaseColor.WHITE);
        static readonly Font TitleFont2HWP = new Font(Font.FontFamily.HELVETICA, 35f, Font.BOLD, BaseColor.BLACK);
        #endregion

        #region  Common Methods

        private void SetHeader(out PdfPTable table)
        {
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                //0=Left, 1=Centre, 2=Right
                HorizontalAlignment = 0,
                SpacingBefore = 10f,
                //SpacingAfter = 10f
            };

            //Cell no 1
            PdfPCell cell = new PdfPCell
            {
                Border = 0
            };
            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Image image = Image.GetInstance(_commonProvider.GetS3StaticImage(UserSession.CurrentOrg.LogoPath));
            image.ScaleAbsolute(100, 80);
            cell.AddElement(image);
            table.AddCell(cell);

            Chunk chunk = new Chunk("" + UserSession.CurrentOrg.Name, FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK));
            Chunk chunk1 = new Chunk("" + UserSession.CurrentOrg.Address + "", FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.AddElement(chunk);
            cell.AddElement(chunk1);

            table.AddCell(cell);


        }
        private void SetHeaderBlue(out PdfPTable table, string Heading)
        {
            table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                //0=Left, 1=Centre, 2=Right
                HorizontalAlignment = 0,
                SpacingBefore = 10f,
                //SpacingAfter = 10f
            };

            float[] widths = new float[] { 15f, 5f, 80f };
            table.SetTotalWidth(widths);

            //Cell no 1
            PdfPCell cell = new PdfPCell
            {
                Border = 0
            };
            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Image image = Image.GetInstance(_commonProvider.GetS3StaticImage(UserSession.CurrentOrg.LogoPath));
            image.ScaleAbsolute(80, 80);
            image.SetAbsolutePosition(10, 0);
            cell.AddElement(image);
            table.AddCell(cell);

            PdfPCell cell2 = new PdfPCell
            {
                Border = PdfPCell.RIGHT_BORDER,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 3f
            };
            table.AddCell(cell2);

            cell = new PdfPCell
            {
                Border = 0
            };
            cell.Padding = 0;
            Chunk chunk1 = new Chunk("   " + Heading + "", CellFontBoldBlue);
            var para = new Paragraph(chunk1)
            {
                Alignment = Element.ALIGN_LEFT
            };
            cell.AddElement(chunk1);
            table.AddCell(cell);

            cell2 = new PdfPCell();
            cell2.UseVariableBorders = true;
            cell2.Colspan = 4;
            cell2.Border = PdfPCell.TOP_BORDER;
            cell2.BorderColor = BaseColor.BLACK;
            cell2.BorderWidth = 3f;
            cell2.Padding = 2;
            table.AddCell(cell2);


        }

        private void SetStatusHeaderBlue(out PdfPTable table, string Heading, string Status, string ApproveBy, string Date)
        {
            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                //0=Left, 1=Centre, 2=Right
                HorizontalAlignment = 0,
                SpacingBefore = 10f,
                //SpacingAfter = 10f
            };

            float[] widths = new float[] { 15f, 5f, 65f, 25f };
            table.SetTotalWidth(widths);

            //Cell no 1
            PdfPCell cell = new PdfPCell
            {
                Border = 0
            };
            Image image = Image.GetInstance(_commonProvider.GetS3StaticImage(UserSession.CurrentOrg.LogoPath));
            image.ScaleAbsolute(80, 80);
            image.SetAbsolutePosition(10, 0);
            cell.AddElement(image);
            table.AddCell(cell);

            PdfPCell cell2 = new PdfPCell
            {
                Border = PdfPCell.RIGHT_BORDER,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 3f
            };
            table.AddCell(cell2);

            cell = new PdfPCell
            {
                Border = 0
            };
            cell.Padding = 0;
            Chunk chunk1 = new Chunk("   " + Heading + "", CellFontBoldBlue);
            var para = new Paragraph(chunk1);
            para.Alignment = Element.ALIGN_LEFT;
            cell.AddElement(chunk1);
            table.AddCell(cell);
            //cell = new PdfPCell
            //{
            //    Border = 0
            //};
            //cell.Padding = 0;
            //cell.Colspan = 3;
            //Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //  table.AddCell(line1);

            PdfPTable table2 = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 5f

            };
            //table2.DefaultCell.Border = 1;
            widths = new float[] { 40, 5, 45 };
            table2.SetTotalWidth(widths);
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.Colspan = 3;
            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Status) ? "       " + Status : " ", CellStatusFont));
            table2.AddCell(cell);
            cell2 = new PdfPCell();
            cell2.UseVariableBorders = true;
            cell2.BorderColor = BaseColor.BLACK;
            cell2.BorderWidth = 1f;
            cell2.Border = PdfPCell.BOTTOM_BORDER;
            cell2.Colspan = 3;
            table2.AddCell(cell2);
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.AddElement(new Phrase(!string.IsNullOrEmpty(ApproveBy) ? ApproveBy : " ", CellFontNormalBlueSmall));
            table2.AddCell(cell);
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.AddElement(new Phrase("|", CellFontNormalBlueSmall));
            table2.AddCell(cell);
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Date) ? Date : " ", CellFontNormalBlueSmall));
            table2.AddCell(cell);
            cell2 = new PdfPCell();
            cell2.Border = PdfPCell.BOX;
            cell2.BorderWidth = 2f;

            cell2.AddElement(table2);

            table.AddCell(cell2);

            cell2 = new PdfPCell
            {
                UseVariableBorders = true,
                Colspan = 4,
                Border = PdfPCell.TOP_BORDER,
                BorderColor = BaseColor.BLACK,
                BorderWidth = 3f
            };
            table.AddCell(cell2);


        }

        private void CreatePdfFile(Document pdfDoc, string fileName, MemoryStream ms)
        {
            byte[] data = ms.ToArray();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);
            Response.Headers.Add("Transfer-Encoding", "identity");
            Response.Headers.Add("Content-Length", data.Length.ToString());
            Response.Body.WriteAsync(data, 0, data.Length).Wait();
            //Response.Flush();
            //Response.SuppressContent = true;
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            //Response.End();


        }

        public static Rectangle SetPaperType()
        {
            return PageSize.LETTER;
        }

        public static Rectangle SetLandScapePaperType()
        {
            return PageSize.A4.Rotate();
        }

        #endregion

        #region ICRA

        [HttpPost]
        [ActionName("ICRAObserverReport")]
        [ValidateAntiForgeryToken]
        public ActionResult ObserverReport(string icraId, string reportId)
        {

            TIcraLog icraLog = new TIcraLog();
            if (!string.IsNullOrEmpty(icraId))
            {
                icraLog = _constructionService.GetICRAObservationReport(Convert.ToInt32(icraId), Convert.ToInt32(reportId));
                icraLog.ObservtionReport = icraLog.ObservtionReports.FirstOrDefault(x => x.TICRAReportId == Convert.ToInt32(reportId));
            }
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();

                //Table
                PdfPTable table;
                Chunk chunk;
                SetHeader(out table);
                //Add table to document
                pdfDoc.Add(table);

                Paragraph para = new Paragraph("ICRA OBSERVER REPORT", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };

                table.AddCell(new Phrase("Project Name: " + icraLog.ProjectNo, CellFont));
                table.AddCell(new Phrase("Contractor: " + icraLog.Contractor, CellFont));
                table.AddCell(new Phrase("Project Location: " + icraLog.Location, CellFont));
                table.AddCell(new Phrase("Date: " + icraLog.ObservtionReport.ReportDate.ToFormatDate(), CellFont));

                pdfDoc.Add(table);
                //Table
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };

                pdfDoc.Add(table);


                table.AddCell("");
                table.AddCell(new Phrase("Yes", CellFont));
                table.AddCell(new Phrase("No", CellFont));
                table.AddCell(new Phrase("N/A", CellFont));

                foreach (var item in icraLog.ObservtionReport.CheckPoints)
                {
                    table.AddCell(new Phrase(item.CheckPoints.CheckPoints, CellFont));
                    table.AddCell(new Phrase((item.Status == 0) ? "Yes" : "", CellFont));
                    table.AddCell(new Phrase((item.Status == 1) ? "No" : "", CellFont));
                    table.AddCell(new Phrase((item.Status == 2) ? "N/A" : "", CellFont));
                }


                //table.AddCell("June 28");

                pdfDoc.Add(table);

                para = new Paragraph("Comment,\n" + icraLog.ObservtionReport.Comment, ParagraphFont);
                pdfDoc.Add(para);


                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                pdfDoc.Add(line);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    //0=Left, 1=Centre, 2=Right
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };


                chunk = new Chunk("Contractor: " + icraLog.ObservtionReport.ContractorSign + "\nDate: " + icraLog.ObservtionReport.ContractorSignDate.CastLocaleDate() + "\nTime: " + icraLog.ObservtionReport.ContractorSignTime + "", CellFont);

                var cell = new PdfPCell
                {
                    Border = 0
                };
                cell.AddElement(chunk);
                table.AddCell(cell);


                chunk = new Chunk("Observer: " + icraLog.ObservtionReport.ObserverSign + "\nDate: " + icraLog.ObservtionReport.ObserverSignDate.CastLocaleDate() + "\nTime: " + icraLog.ObservtionReport.ObserverSignTime + "", CellFont);

                cell = new PdfPCell
                {
                    Border = 0
                };
                cell.AddElement(chunk);
                table.AddCell(cell);

                pdfDoc.Add(table);
                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "ObserverReport_" + icraLog.TicraId + "_" + icraLog.ObservtionReport.TICRAReportId + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        //[HttpPost]
        //[ActionName("ICRAProjectObserverReport")]
        //[ValidateAntiForgeryToken]
        //public ActionResult ProjectObserverReport(string projectIds, string reportId)
        //{
        //    TICRAReports ObservtionReport = new TICRAReports();
        //    if (!string.IsNullOrEmpty(projectIds))
        //    {
        //        //icraLog = _constructionService.GetICRAObservationReport(Convert.ToInt32(icraId), Convert.ToInt32(reportId));
        //        ObservtionReport = _constructionService.GetICRAProjectObservationReport(projectIds, reportId);

        //        projectIds = string.Join(",", projectIds.Split(',').Select(x => Convert.ToInt32(x)).OrderBy(x => x).ToList());

        //        ObservtionReport = ObservtionReport.RelatedReports.Where(x => x.ProjectIds == projectIds).FirstOrDefault();
        //    }
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
        //        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
        //        pdfWriter.PageEvent = new PDFFooter();
        //        pdfDoc.Open();

        //        //Table
        //        PdfPTable table;
        //        PdfPCell cell;
        //        Chunk chunk;
        //        SetHeader(out table);
        //        //Add table to document
        //        pdfDoc.Add(table);

        //        Paragraph para = new Paragraph("ICRA OBSERVER REPORT", TitleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER
        //        };
        //        pdfDoc.Add(para);

        //        //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        //        //pdfDoc.Add(line);
        //        table = new PdfPTable(2)
        //        {
        //            WidthPercentage = 100,
        //            HorizontalAlignment = 0,
        //            SpacingBefore = 20f,
        //            SpacingAfter = 30f
        //        };

        //        table.AddCell(new Phrase("Project Name: " + ObservtionReport.ProjectNames, CellFont));
        //        table.AddCell(new Phrase("Contractor: " + ObservtionReport.ProjectContractors, CellFont));//
        //        table.AddCell(new Phrase("Project Location: " + ObservtionReport.ProjectLocations, CellFont));
        //        table.AddCell(new Phrase("Date: " + ObservtionReport.ReportDate.ToFormatDate(), CellFont));

        //        pdfDoc.Add(table);
        //        //Table
        //        table = new PdfPTable(4)
        //        {
        //            WidthPercentage = 100,
        //            HorizontalAlignment = 0,
        //            SpacingBefore = 20f,
        //            SpacingAfter = 10f
        //        };

        //        pdfDoc.Add(table);


        //        table.AddCell("");
        //        table.AddCell(new Phrase("Yes", CellFont));
        //        table.AddCell(new Phrase("No", CellFont));
        //        table.AddCell(new Phrase("N/A", CellFont));

        //        foreach (var item in ObservtionReport.CheckPoints)
        //        {
        //            table.AddCell(new Phrase(item.CheckPointText, CellFont));
        //            table.AddCell(new Phrase((item.Status == 0) ? "Yes" : "", CellFont));
        //            table.AddCell(new Phrase((item.Status == 1) ? "No" : "", CellFont));
        //            table.AddCell(new Phrase((item.Status == 2) ? "N/A" : "", CellFont));
        //        }


        //        //table.AddCell("June 28");

        //        pdfDoc.Add(table);

        //        para = new Paragraph("Comment,\n" + ObservtionReport.Comment, ParagraphFont);
        //        pdfDoc.Add(para);


        //        Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        //        pdfDoc.Add(line);

        //        table = new PdfPTable(2)
        //        {
        //            WidthPercentage = 100,
        //            //0=Left, 1=Centre, 2=Right
        //            HorizontalAlignment = 0,
        //            SpacingBefore = 20f,
        //            SpacingAfter = 10f
        //        };

        //        cell = new PdfPCell { Border = 0 };
        //        chunk = new Chunk("Contractor: " + ObservtionReport.DSContractorSignId.SignByUserName + "\nDate: " + ObservtionReport.ContractorSignDate.CastLocaleDate(), CellFont);
        //        cell.AddElement(chunk);
        //        table.AddCell(cell);

        //        cell = new PdfPCell { Border = 0 };
        //        chunk = new Chunk("Observer: " + ObservtionReport.DSObserverSignId.SignByUserName + "\nDate: " + ObservtionReport.ObserverSignDate.CastLocaleDate(), CellFont);
        //        cell.AddElement(chunk);
        //        table.AddCell(cell);


        //        cell = new PdfPCell { Border = 0 };
        //        if (!string.IsNullOrEmpty(ObservtionReport.DSContractorSignId.FilePath))
        //        {
        //            Image constimage = Image.GetInstance(_commonProvider.FilePath(ObservtionReport.DSContractorSignId.FilePath));
        //            cell.AddElement(new Chunk(constimage, 0, 0));
        //        }
        //        table.AddCell(cell);


        //        cell = new PdfPCell { Border = 0 };
        //        if (!string.IsNullOrEmpty(ObservtionReport.DSObserverSignId.FilePath))
        //        {
        //            Image observimage = Image.GetInstance(_commonProvider.FilePath(ObservtionReport.DSObserverSignId.FilePath));
        //            cell.AddElement(new Chunk(observimage, 0, 0));
        //        }
        //        table.AddCell(cell);


        //        cell = new PdfPCell { Border = 0 };
        //        cell.AddElement(new Phrase(ObservtionReport.DSContractorSignId.SignByUserName + " (" + ObservtionReport.DSContractorSignId.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
        //        table.AddCell(cell);


        //        cell = new PdfPCell { Border = 0 };
        //        cell.AddElement(new Phrase(ObservtionReport.DSObserverSignId.SignByUserName + " (" + ObservtionReport.DSObserverSignId.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
        //        table.AddCell(cell);


        //        pdfDoc.Add(table);
        //        //pdfWriter.PageEvent = new PDFFooter();
        //        pdfWriter.CloseStream = false;
        //        pdfDoc.Close();
        //        string fileName = "ObserverReport_" + ObservtionReport.ProjectIds.Replace(",", "_") + "_" + ObservtionReport.TICRAReportId + ".pdf";
        //        CreatePdfFile(pdfDoc, fileName, ms);
        //    }
        //    return View();
        //}


        [HttpPost]
        [ActionName("ICRAProjectObserverReport")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectObserverReport(string projectIds, string reportId)
        {
            TICRAReports ObservtionReport = new TICRAReports();
            var resstr = _pdfService.PrintProjectObserverReportInbytes(projectIds, reportId, ref ObservtionReport);
            byte[] fileContent = Convert.FromBase64String(resstr);
            string fileName = "ObserverReport_" + ObservtionReport.ProjectIds.Replace(",", "_") + "_" + ObservtionReport.TICRAReportId + ".pdf";

            Response.Clear();
            MemoryStream ms = new MemoryStream(fileContent);
            byte[] data = ms.ToArray();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);
            Response.Body.WriteAsync(data, 0, data.Length);
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ICRAPermit(int icraId, string PDFName, int? projectId, int? tpcraquestid)
        {
            TIcraLog objTIcraLog = _pdfService.getTicralog(icraId);
            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
            if (projectId > 0 && tpcraquestid > 0)
            {
                objQuestionTPCRA = _pcraService.GetQuestionTPCRA(projectId, tpcraquestid);
            }
            else
            {
                objQuestionTPCRA = null;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                _pdfService.PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "";
                if (projectId > 0 && tpcraquestid > 0)
                    fileName = PDFName + "_" + objQuestionTPCRA.PCRANumber + ".pdf";
                else
                    fileName = PDFName + "_" + objTIcraLog.PermitNo + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PCRAPermitWorksheet(int projectId, int tpcraquestid, int icraId, string PDFName, int hasattachment)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
                objQuestionTPCRA = _pcraService.GetQuestionTPCRA(projectId, tpcraquestid);
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();

                PrintICRAPermitWorksheet(objQuestionTPCRA.TicraId, objQuestionTPCRA, pdfDoc, pdfWriter, hasattachment, 1);

                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = PDFName + "_" + objQuestionTPCRA.PCRANumber + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ICRAPermitWorksheet(int icraId, string PDFName, int hasattachment)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                TIcraLog objTIcraLog = _pdfService.getTicralog(icraId);
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PrintICRAPermitWorksheet(icraId, null, pdfDoc, pdfWriter, hasattachment);

                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = PDFName + "_" + objTIcraLog.PermitNo + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }
        private void PrintICRAPermitWorksheet(int icraId, TPCRAQuestion objQuestionTPCRA, Document pdfDoc, PdfWriter pdfwriter, int hasattachment, int? PermitType = 0)
        {
            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            TIcraLog objTIcraLog = _pdfService.getTicralog(icraId);
            //if (PermitType == 1)
            //{

            //    _pdfService.PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            //    pdfDoc.NewPage();
            //}
            _pdfService.PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            pdfDoc.NewPage();
            Paragraph para = new Paragraph("Infection Control Risk Assessment \n Matrix of Precautions for Construction & Renovation", TitleFontS)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);
            Paragraph line = new Paragraph();
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            int childicrasteps = 0;
            for (int i = 0; i < objTIcraLog.TIcraSteps.Where(x => x.Step.ParentICRAStepId == 0).ToList().Count; i++)
            {
                childicrasteps = i + 1;
                if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 1)
                {
                    float[] widths = new float[] { 40f, 60f };
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Type of Construction Project Activity (Type A-D)", ParagraphFontS);
                    pdfDoc.Add(para);
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f
                    };
                    table.SetWidths(widths);
                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
                    {
                        string activity = objTIcraLog.ConstructionTypes[j].Description + "\n" + "Includes, but is not limited to: \n";
                        foreach (var item in objTIcraLog.ConstructionTypes[j].ConstructionActivity)
                        {
                            activity = activity + "• " + item.Activity + "\n";
                        }
                        PdfPCell cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
                        PdfPCell cell2 = new PdfPCell(new Phrase(activity, CellFontS));
                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
                        {
                            cell1.BackgroundColor = Gray;
                            cell2.BackgroundColor = Gray;
                        }
                        table.AddCell(cell1);
                        table.AddCell(cell2);
                    }
                    pdfDoc.Add(table);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        SpacingAfter = 5f
                    };
                    pdfDoc.Add(para);
                }
                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 2)
                {
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Patient Risk Groups that will be affected. If more than one risk group will be affected, select the higher risk group:", ParagraphFontS);
                    pdfDoc.Add(para);
                    table = new PdfPTable(4)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f
                    };
                    for (int k = 0; k < objTIcraLog.ConstructionRisks.Count; k++)
                    {
                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[k].RiskName, CellFontS));
                        if (objTIcraLog.ConstructionRisks[k].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
                        {
                            cell.BackgroundColor = Gray;
                        }
                        table.AddCell(cell);
                    }
                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
                    {
                        para = new Paragraph();
                        foreach (var item in objTIcraLog.ConstructionRisks[j].RiskArea)
                        {
                            para.Add(new Phrase("• " + item.Name + "\n", ParagraphFontS));
                        }
                        table.AddCell(para);
                    }
                    pdfDoc.Add(table);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        SpacingAfter = 5f
                    };
                    pdfDoc.Add(para);

                }
                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 3)
                {
                    if (objTIcraLog.Version > 0)
                    {
                        var classlist = _constructionService.GetConstructionClass().Where(x => x.Version == objTIcraLog.Version || x.Version == 0).ToList();
                        string clstrr = string.Join(",", classlist.Select(x => x.ClassName));
                        string clslevel = classlist.First().ClassName.ToUpper().Replace("CLASS", "") + "-" + classlist.Last().ClassName.ToUpper().Replace("CLASS", "");
                        para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Patient Risk Group (Low, Medium, High, Highest) with the planned Construction Project Type (A, B, C, D) on the following matrix, to find the Class of Precautions (" + clstrr.ToUpper().Replace("CLASS", "") + ") or level of infection control activities required. Class " + clslevel + " or Color-Coded Precautions are delineated on the following page.\nIC Matrix - Class of Precautions: Construction Project by Patient Risk", ParagraphFontS)
                        {
                            SpacingBefore = 5f,
                            //SpacingAfter = 30f
                        };
                    }
                    pdfDoc.Add(para);

                    pdfDoc.NewPage();
                    para = new Paragraph("\nConstruction Project Type ", ParagraphFontS)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 10f
                    };
                    pdfDoc.Add(para);                    
                    table = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f
                    };
                    //table.SpacingAfter = 30f;
                    table.AddCell(new Phrase("Patient Risk Group", CellFontS));
                    int? ConstructionTypeId = 0;
                    int? ConstructionRiskId = 0;
                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
                    {

                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
                        {
                            cell.BackgroundColor = Gray;
                            ConstructionTypeId = objTIcraLog.ConstructionTypeId;
                        }
                        table.AddCell(cell);
                    }
                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
                    {
                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
                        if (objTIcraLog.ConstructionRisks[j].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
                        {
                            cell.BackgroundColor = Gray;
                            ConstructionRiskId = objTIcraLog.ConstructionRiskId;
                        }

                        table.AddCell(cell);
                        //table.AddCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
                        for (int k = 0; k < objTIcraLog.ConstructionTypes.Count; k++)
                        {
                            var str = string.Empty;
                            var className = string.Empty;
                            var ClassId = string.Empty;

                            var data = objTIcraLog.ICRAMatixPrecautions.Where(x => x.ConstructionRiskId == objTIcraLog.ConstructionRisks[j].ConstructionRiskId && x.ConstructionTypeId == objTIcraLog.ConstructionTypes[k].ConstructionTypeId && (x.Version == objTIcraLog.Version || x.Version == 0)).ToList();
                            if (data.Count > 0)
                            {
                                str = string.Join(",", data.Select(x => x.ConstructionClass.ClassName));
                                className = data.FirstOrDefault().ConstructionClass.ClassName.Replace(" ", "").ToLower();

                            }

                            cell = new PdfPCell(new Phrase(str, CellFontS));

                            if (objTIcraLog.ConstructionTypes[k].ConstructionTypeId == ConstructionTypeId && objTIcraLog.ConstructionRisks[j].ConstructionRiskId == ConstructionRiskId)
                            {
                                cell.BackgroundColor = BaseColor.BLUE;
                            }
                            table.AddCell(cell);
                        }
                    }
                    pdfDoc.Add(table);
                    para = new Paragraph("Note: Infection Control approval will be required when the Construction Activity and Risk Level indicate that Class III or Class IV control procedures are necessary.", ParagraphFontS);
                    pdfDoc.Add(para);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        SpacingAfter = 5f
                    };
                    pdfDoc.Add(para);

                }
                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 4)
                {
                    para = new Paragraph("Step 4:Identify the areas surrounding the project area, assessing potential impact # " + objTIcraLog.ProjectNo, ParagraphFontS);
                    pdfDoc.Add(para);

                    table = new PdfPTable(6)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f
                    };
                    //table.SpacingAfter = 30f;
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        //string risklist = "";
                        string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames : string.Empty;
                        //risklist = risklist.TrimEnd(',') + riskarea;

                        table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + riskarea, CellFontS));
                    }
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string riskName = objTIcraLog.AreasSurroundings[k].ConstructionRisk != null ? objTIcraLog.AreasSurroundings[k].ConstructionRisk.RiskName : string.Empty;
                        table.AddCell(new Phrase("Risk Group : " + riskName, CellFontS));
                    }
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string others = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].Comment) ? "Others :" + objTIcraLog.AreasSurroundings[k].Comment : "Others:" + string.Empty;
                        table.AddCell(new Phrase(others, CellFontS));
                    }
                    pdfDoc.Add(table);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        SpacingAfter = 5f
                    };
                    pdfDoc.Add(para);
                }
                else
                {
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ": " + "\n", ParagraphFontS);
                    pdfDoc.Add(para);

                    para = new Paragraph(Convert.ToChar(65) + ": " + objTIcraLog.TIcraSteps[i].Step.Steps + "\n", ParagraphFontS);
                    pdfDoc.Add(para);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment, ParagraphFontS)
                    {
                        SpacingBefore = 5f

                    };
                    pdfDoc.Add(para);
                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
                    pdfDoc.Add(line);
                }
                for (int j = 0; j < objTIcraLog.TIcraSteps.Where(x => x.Step.ParentICRAStepId == objTIcraLog.TIcraSteps[i].ICRAStepId).ToList().Count; j++)
                {
                    para = new Paragraph($"{Convert.ToChar(j + 66)}" + ": " + objTIcraLog.TIcraSteps[childicrasteps].Step.Steps + "\n", ParagraphFontS);
                    pdfDoc.Add(para);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[childicrasteps].Comment, ParagraphFontS)
                    {
                        SpacingBefore = 5f
                    };
                    pdfDoc.Add(para);
                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
                    pdfDoc.Add(line);
                    childicrasteps++;
                }

            }

            //if (PermitType != 1)
            //{
            //    pdfDoc.NewPage();
            //    _pdfService.PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            //}




            bool isattachemnets = false;
            if (hasattachment == 1)
            {
                if (PermitType == 1)
                {
                    if (objTIcraLog.TICRAFiles.Count > 0 || objQuestionTPCRA.DrawingAttachments.Count > 0)
                    {
                        pdfDoc.NewPage();
                        isattachemnets = true;
                    }
                }
                else
                {
                    if (objTIcraLog.TICRAFiles.Count > 0 || objTIcraLog.DrawingAttachments.Count > 0)
                    {
                        pdfDoc.NewPage();
                        isattachemnets = true;
                    }
                }
                if (isattachemnets)
                {
                    if (objTIcraLog.TICRAFiles.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 10f
                        };


                        table.AddCell(_pdfService.AddNewCell("Attachment(s):", ParagraphFontS));
                        for (int fileid = 0; fileid < objTIcraLog.TICRAFiles.Count(); fileid++)
                        {

                            _pdfService.AddAttachmentCell(objTIcraLog.TICRAFiles[fileid].FilePath, objTIcraLog.TICRAFiles[fileid].FileName, pdfwriter, table);
                        }
                        pdfDoc.Add(table);
                    }
                    if (PermitType == 1)
                    {
                        if (objQuestionTPCRA.DrawingAttachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0
                            };
                            table.AddCell(_pdfService.AddNewCell("Drawings Attached:", ParagraphFontS));
                            for (int k = 0; k < objQuestionTPCRA.DrawingAttachments.Count; k++)
                            {
                                //table.AddCell(_pdfService.AddNewCell(objQuestionTPCRA.DrawingAttachments[k].FullFileName, ParagraphFontS));
                                //table.AddCell(_pdfService.AddNewCell("", ParagraphFontS));
                                _pdfService.AddAttachmentCell(objQuestionTPCRA.DrawingAttachments[k].ImagePath, objQuestionTPCRA.DrawingAttachments[k].FullFileName, pdfwriter, table);
                            }
                            pdfDoc.Add(table);
                        }
                    }
                    else
                    {
                        if (objTIcraLog.DrawingAttachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0
                            };
                            table.AddCell(_pdfService.AddNewCell("Drawings Attached:", ParagraphFontS));
                            for (int k = 0; k < objTIcraLog.DrawingAttachments.Count; k++)
                            {
                                //table.AddCell(_pdfService.AddNewCell(objTIcraLog.DrawingAttachments[k].FullFileName, ParagraphFontS));
                                //table.AddCell(_pdfService.AddNewCell("", ParagraphFontS));

                                _pdfService.AddAttachmentCell(objTIcraLog.DrawingAttachments[k].ImagePath, objTIcraLog.DrawingAttachments[k].FullFileName, pdfwriter, table);
                            }
                            pdfDoc.Add(table);
                        }
                    }
                }
            }
        }



        #endregion

        #region FireDrill Reports


        [HttpPost]
        [ActionName("FireDrillReport")]
        [ValidateAntiForgeryToken]
        public ActionResult FireDrillReport(int texerciseId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                TExercises objExercise = new TExercises();
                if (texerciseId > 0)
                {
                    objExercise = _exercisesService.GetExercises(texerciseId).FirstOrDefault(); //.FirstOrDefault(x => x.TExerciseId == texerciseId);
                }
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                SetHeader(out table);
                pdfDoc.Add(table);

                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //pdfDoc.Add(line);

                Paragraph para = new Paragraph("FIRE DRILL DOCUMENTATION", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };

                //table.AddCell(new Phrase("Department", CellFont));
                table.AddCell(new Phrase("Shift:", TitleFontS));
                table.AddCell(new Phrase(objExercise.ShiftId.ToString(), CellFont));

                table.AddCell(new Phrase("Date:", TitleFontS));
                table.AddCell(new Phrase(objExercise.Date.Value.ToString("ddd, MMM d, yyyy"), CellFont));

                table.AddCell(new Phrase("Time:", TitleFontS));
                if (objExercise.Time.HasValue)
                {
                    DateTime time = DateTime.Today.Add(objExercise.Time.Value);
                    string StartTime = time.ToString("hh:mm tt");
                    table.AddCell(new Phrase(StartTime, CellFont));
                }
                else { table.AddCell(""); }


                if (objExercise.IsAudible == false)
                {
                    objExercise.TExerciseQuestionnaires = objExercise.TExerciseQuestionnaires.Where(x => x.FireDrillQuestionnaires.FireDrillCategory.CategoryName != "Fire Alarm Equipment & Response").ToList();
                }

                table.AddCell(new Phrase("Building:", TitleFontS));
                table.AddCell(new Phrase(objExercise.Building.BuildingName, CellFont));

                table.AddCell(new Phrase("Location:", TitleFontS));
                table.AddCell(new Phrase(objExercise.NearBy, CellFont));

                table.AddCell(new Phrase("Drill Conducted By:", TitleFontS));
                table.AddCell(new Phrase(objExercise.ConductedBy, CellFont));

                table.AddCell(new Phrase("Observers:", TitleFontS));
                table.AddCell(new Phrase(objExercise.Observers, CellFont));

                table.AddCell(new Phrase("Unannounced:", TitleFontS));
                table.AddCell(new Phrase(objExercise.Announced ? "Yes" : "No", CellFont));

                table.AddCell(new Phrase("Audible Notification:", TitleFontS));
                table.AddCell(new Phrase(objExercise.IsAudible ? "Yes" : "No", CellFont));

                table.AddCell(new Phrase("", CellFont));
                table.AddCell(new Phrase("", CellFont));


                pdfDoc.Add(table);

                para = new Paragraph("Evaluation:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);
                var widths = new float[] { 32f, 6f, 6f, 16f };
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };
                table.SetWidths(widths);
                table.AddCell(new Phrase("Questions", CellFont));
                table.AddCell(new Phrase("Status", CellFont));
                table.AddCell(new Phrase("Score (out of 5)", CellFont));
                table.AddCell(new Phrase("Comment", CellFont));
                var previouscategoryname = "";
                int overallScore = 0;
                foreach (var item in objExercise.TExerciseQuestionnaires)
                {
                    var currentcategoryname = item.FireDrillQuestionnaires.FireDrillCategory.CategoryName;
                    if (currentcategoryname != null && currentcategoryname != "" && previouscategoryname == "")
                    {
                        PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellBoldFont));
                        Categoryname.Colspan = 4;
                        table.AddCell(Categoryname);
                        previouscategoryname = currentcategoryname;
                    }
                    else if (currentcategoryname != previouscategoryname)
                    {
                        PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellBoldFont));
                        Categoryname.Colspan = 4;
                        table.AddCell(Categoryname);
                        previouscategoryname = currentcategoryname;
                    }
                    BDO.Enums.FireDrillQuesStatus enums = (BDO.Enums.FireDrillQuesStatus)item.Status;
                    table.AddCell(new Phrase(item.FireDrillQuestionnaires.Questionnaries, CellFont));
                    table.AddCell(new Phrase(enums.ToString(), CellFont));
                    table.AddCell(new Phrase(item.Ratings.HasValue ? item.Ratings.Value.ToString() : "", CellFont));
                    table.AddCell(new Phrase(item.Comments, CellFont));
                    overallScore = overallScore + (item.Ratings.HasValue ? Convert.ToInt32(item.Ratings.Value) : 0);
                }
                pdfDoc.Add(table);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };
                table.AddCell(new Phrase("Overall Score ", CellFont));
                table.AddCell(new Phrase(overallScore.ToString(), CellFont));
                table.AddCell(new Phrase("Final Comment", CellFont));
                table.AddCell(new Phrase(objExercise.Comment, CellFont));

                pdfDoc.Add(table);

                //pdfDoc.NewPage();
                para = new Paragraph("Evaluation Signature", CellFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };

                pdfDoc.Add(para);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 20f
                };
                //table.AddCell(new Phrase("Evaluation Signature", CellFont));
                foreach (var item in objExercise.DigitalSignature)
                {
                    PdfPCell cell = new PdfPCell();
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonModelFactory.FilePath(item.FilePath));
                    //image.ScaleAbsolute(150, 30);
                    //cell.AddElement(image);
                    cell.AddElement(new Chunk(image, 0, 0));
                    table.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cell.AddElement(new Phrase(item.SignByUserName + " ( " + item.SignByEmail + " )", CellFontS));
                    cell.AddElement(new Phrase("(" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontDatetimeblue));
                    table.AddCell(cell);
                }
                pdfDoc.Add(table);

                para = new Paragraph("Evaluation Documents:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                var evaluationDocuments = objExercise.TExerciseFiles.Where(x => x.DrillFileType == BDO.Enums.DrillFileType.Evaluation && x.DocumentType == 0).ToList();
                if (evaluationDocuments.Count > 0)
                {
                    foreach (var item in evaluationDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFont)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);
                    }
                    pdfDoc.NewPage();

                    foreach (var item in evaluationDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFontBold)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);

                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 20f
                        };

                        var reader = new PdfReader(new Uri(_commonModelFactory.FilePath(item.FilePath)));
                        PdfReader.unethicalreading = true;
                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            var page = pdfWriter.GetImportedPage(reader, i);
                            Image img = Image.GetInstance(page);
                            PdfPCell cell = new PdfPCell(img, true);
                            cell.FixedHeight = 690f;
                            table.AddCell(cell);
                        }
                        pdfDoc.Add(table);
                        pdfDoc.NewPage();
                        //reader.Close();
                    }

                }
                pdfDoc.NewPage();

                para = new Paragraph("Critique:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };
                table.AddCell(new Phrase("Critique Date "));
                table.AddCell(new Phrase(objExercise.CritiqueDate.HasValue ? objExercise.CritiqueDate.Value.ToString("ddd, MMM d, yyyy") : objExercise.Date.Value.ToString("ddd, MMM d, yyyy")));
                table.AddCell(new Phrase("Comments/Critique"));
                table.AddCell(new Phrase(objExercise.CritiquesComment));

                pdfDoc.Add(table);

                para = new Paragraph("Issue And Action:", CellFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };

                table.AddCell(new Phrase("Issue", CellFont));
                table.AddCell(new Phrase("Recommendation/Action to be Taken", CellFont));

                foreach (var item in objExercise.TExerciseActions)
                {
                    table.AddCell(new Phrase(item.Issue, CellFont));
                    table.AddCell(new Phrase(item.Action, CellFont));
                }
                pdfDoc.Add(table);

                para = new Paragraph("Critique Signature", CellFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 20f
                };

                //table.AddCell(new Phrase("Critique Signature", CellFont));
                foreach (var item in objExercise.CritiqueDigitalSignature)
                {
                    PdfPCell cell = new PdfPCell();
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonModelFactory.FilePath(item.FilePath));
                    //image.ScaleAbsolute(150, 30);
                    //cell.AddElement(image);
                    cell.AddElement(new Chunk(image, 0, 0));
                    table.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    //cell.AddElement(new Phrase(item.SignByUserName + " ( " + item.SignByEmail + " )" + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    //table.AddCell(cell);
                    cell.AddElement(new Phrase(item.SignByUserName + " ( " + item.SignByEmail + " )", CellFontS));
                    cell.AddElement(new Phrase("(" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontDatetimeblue));
                    table.AddCell(cell);

                }
                pdfDoc.Add(table);

                para = new Paragraph("Critique Documents:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                var critiqueDocuments = objExercise.TExerciseFiles.Where(x => x.DrillFileType == BDO.Enums.DrillFileType.Critique && x.DocumentType == 0).ToList();

                if (critiqueDocuments.Count > 0)
                {
                    foreach (var item in critiqueDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFont)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);
                    }
                    pdfDoc.NewPage();

                    foreach (var item in critiqueDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFontBold)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);

                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 20f
                        };
                        var reader = new PdfReader(new Uri(_commonModelFactory.FilePath(item.FilePath)));
                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            var page = pdfWriter.GetImportedPage(reader, i);
                            Image img = Image.GetInstance(page);
                            PdfPCell cell = new PdfPCell(img, true);
                            cell.FixedHeight = 690f;
                            table.AddCell(cell);
                        }
                        pdfDoc.Add(table);
                        pdfDoc.NewPage();
                        //reader.Close();
                    }
                }

                pdfDoc.NewPage();

                para = new Paragraph("Education:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };
                table.AddCell(new Phrase("Education Date "));
                table.AddCell(new Phrase(objExercise.EducationDate.HasValue ? objExercise.EducationDate.Value.ToString("ddd, MMM d, yyyy") : objExercise.Date.Value.ToString("ddd, MMM d, yyyy")));
                table.AddCell(new Phrase("Comments"));
                table.AddCell(new Phrase(objExercise.EducationComment));

                pdfDoc.Add(table);

                para = new Paragraph("Education Signature", CellFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };

                pdfDoc.Add(para);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 20f
                };
                //table.AddCell(new Phrase("Education Signature", CellFont));
                foreach (var item in objExercise.EducationDigitalSignature)
                {
                    PdfPCell cell = new PdfPCell();
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonModelFactory.FilePath(item.FilePath));
                    //image.ScaleAbsolute(150, 30);
                    //cell.AddElement(image);
                    cell.AddElement(new Chunk(image, 0, 0));
                    table.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    //cell.AddElement(new Phrase(item.SignByUserName + " ( " + item.SignByEmail + " )" + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    //table.AddCell(cell);
                    cell.AddElement(new Phrase(item.SignByUserName + " ( " + item.SignByEmail + " )", CellFontS));
                    cell.AddElement(new Phrase("(" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontDatetimeblue));
                    table.AddCell(cell);

                }
                pdfDoc.Add(table);

                para = new Paragraph("Education Documents:", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                pdfDoc.Add(para);

                var educationDocuments = objExercise.TExerciseFiles.Where(x => x.DrillFileType == BDO.Enums.DrillFileType.Education && x.DocumentType == 0).ToList();

                if (educationDocuments.Count > 0)
                {
                    foreach (var item in critiqueDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFont)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);
                    }
                    pdfDoc.NewPage();

                    foreach (var item in educationDocuments)
                    {
                        para = new Paragraph(item.FileName, CellFontBold)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        pdfDoc.Add(para);
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 20f
                        };
                        var reader = new PdfReader(new Uri(_commonModelFactory.FilePath(item.FilePath)));
                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            var page = pdfWriter.GetImportedPage(reader, i);
                            Image img = Image.GetInstance(page);
                            PdfPCell cell = new PdfPCell(img, true);
                            cell.FixedHeight = 690f;
                            table.AddCell(cell);
                        }
                        pdfDoc.Add(table);
                        pdfDoc.NewPage();
                    }
                }
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "Firedrill_" + objExercise.TExerciseId + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        //private Document CreateFiredrillMatrix(int buildingTypeId, int quarterNo, int year, Stream streamOutput)
        //{
        //    var quater = ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterNo, year, false).FirstOrDefault();

        //    Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
        //    pdfWriter.PageEvent = new PDFFooter();
        //    pdfDoc.Open();
        //    PdfPTable table;
        //    SetHeader(out table);
        //    pdfDoc.Add(table);


        //    Paragraph para = new Paragraph("FIRE DRILL MATRIX", TitleFont)
        //    {
        //        Alignment = Element.ALIGN_CENTER
        //    };
        //    pdfDoc.Add(para);

        //    para = new Paragraph("Year: " + quater.Year, CellFontBoldSmall)
        //    {
        //        Alignment = Element.ALIGN_RIGHT
        //    };
        //    pdfDoc.Add(para);
        //    int quaterno = 0;
        //    //string quatername = "";
        //    if (TempData["quaterNo"] != null)
        //    {
        //        quaterno = Convert.ToInt32(TempData["quaterNo"]);

        //        if (quaterno == 1)
        //        {
        //            // quatername = "Q1(Jan-Mar)";
        //            para = new Paragraph("Q1 (Jan-Mar)", CellFontSmall1)
        //            {
        //                Alignment = Element.ALIGN_RIGHT
        //            };
        //        }
        //        else if (quaterno == 2)
        //        {
        //            //quatername = "Q2(Apr-Jun)";
        //            para = new Paragraph("Q2 (Apr-Jun)", CellFontSmall1)
        //            {
        //                Alignment = Element.ALIGN_RIGHT
        //            };
        //        }
        //        else if (quaterno == 3)
        //        {
        //            //quatername = " Q3(Jul - Sep)";
        //            para = new Paragraph("Q3 (Jul-Sep)", CellFontSmall1)
        //            {
        //                Alignment = Element.ALIGN_RIGHT
        //            };
        //        }
        //        else if (quaterno == 4)
        //        {
        //            //quatername = "Q4(Oct-Dec)";
        //            para = new Paragraph("Q4 (Oct-Dec)", CellFontSmall1)
        //            {
        //                Alignment = Element.ALIGN_RIGHT
        //            };
        //        }
        //    }

        //    pdfDoc.Add(para);

        //    //table = new PdfPTable(7)
        //    //{
        //    //    WidthPercentage = 100,
        //    //    HorizontalAlignment = 0,
        //    //    SpacingBefore = 20f,
        //    //};
        //    //table.SetWidths(new float[] { 17f, 15f, 15f, 8f, 15f, 15f, 15f });
        //    //table.AddCell(new PdfPCell(new Phrase("Planned Drills", CellFontBoldSmall)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //table.AddCell(new PdfPCell(new Phrase("Total Drills:" + quater.TotalPlanned, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    ////table.AddCell(new PdfPCell(new Phrase("Total Unannounced:" + quater.TotalUnAnnounced, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //Image Img = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.UnAnnouncedIcon));
        //    //Img.ScaleAbsolute(20f, 20f);
        //    //if (quater.TotalUnAnnounced > 0 && quater.TotalPlanned > 0)
        //    //{
        //    //    var data = quater.TotalUnAnnounced * 100 / quater.TotalPlanned;
        //    //    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(data) + "(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

        //    //}
        //    //else
        //    //{

        //    //    table.AddCell(new PdfPCell(new Phrase("(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //}
        //    //PdfPCell cellnoborder = new PdfPCell(new Phrase("", CellFont))
        //    //{
        //    //    Border = 0,
        //    //};
        //    //table.AddCell(cellnoborder);

        //    //table.AddCell(new PdfPCell(new Phrase("Actual Drills", CellFontBoldSmall)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //table.AddCell(new PdfPCell(new Phrase("Total Drills:" + quater.TotalDone, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    ////table.AddCell(new PdfPCell(new Phrase("Total Unannounced:" + quater.TotalUnAnnouncedDone, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //Img = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.UnAnnouncedIcon));
        //    //Img.ScaleAbsolute(20f, 20f);
        //    //if (quater.TotalUnAnnouncedDone > 0 && quater.TotalDone > 0)
        //    //{
        //    //    var donePer = quater.TotalUnAnnouncedDone * 100 / quater.TotalDone;
        //    //    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(donePer) + "(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

        //    //}
        //    //else
        //    //{

        //    //    table.AddCell(new PdfPCell(new Phrase("(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //}


        //    //pdfDoc.Add(table);
        //    var shift = ShiftRepository.GetShift().Where(x => x.IsActive == true).ToList();
        //    if (shift.Count == 4)
        //    {

        //        table = new PdfPTable(6)
        //        {
        //            WidthPercentage = 100,
        //            HorizontalAlignment = 0,
        //            SpacingBefore = 20f,
        //            SpacingAfter = 30f
        //        };
        //        table.SetWidths(new float[] { 10f, 10f, 20f, 20f, 20f, 20f });
        //    }
        //    else
        //    {
        //        table = new PdfPTable(5)
        //        {
        //            WidthPercentage = 100,
        //            HorizontalAlignment = 0,
        //            SpacingBefore = 20f,
        //            SpacingAfter = 30f
        //        };
        //        table.SetWidths(new float[] { 15f, 10f, 25f, 25f, 25f });
        //    }

        //    table.AddCell(new Phrase("", CellFont));
        //    table.AddCell(new Phrase("", CellFont));

        //    //table.AddCell(new PdfPCell(new Phrase("Shift 1")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //table.AddCell(new PdfPCell(new Phrase("Shift 2")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    //table.AddCell(new PdfPCell(new Phrase("Shift 3")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    foreach (var _shift in shift)
        //    {
        //        table.AddCell(new PdfPCell(new Phrase(_shift.Name)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    }


        //    PdfPCell cell = new PdfPCell();
        //    Image image1 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.VendorBuildingIcon));
        //    image1.ScaleAbsolute(20f, 20f);

        //    Image image2 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.CalendarIcon));
        //    image2.ScaleAbsolute(20f, 20f);

        //    PdfPCell cell2 = new PdfPCell();

        //    Image image3 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.ClockIcon));
        //    image3.ScaleAbsolute(20f, 20f);

        //    //Image image4 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.AnnounceIcon));
        //    //image4.ScaleAbsolute(20f, 20f);

        //    Image image5 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.TickIcon));
        //    image5.ScaleAbsolute(20f, 20f);

        //    Image image6 = Image.GetInstance(Server.MapPath(HCF.Web.Models.ImagePathModel.DashboardIcon));
        //    image6.ScaleAbsolute(20f, 20f);

        //    //Shift 1

        //    Paragraph icon = new Paragraph();
        //    icon.Add(new Chunk(image1, -5, 0));
        //    cell.AddElement(image1);
        //    table.AddCell(cell);

        //    table.AddCell(new Phrase("", CellFont));


        //    Paragraph p = new Paragraph();
        //    p.Add(new Chunk(image2, 05, 0));
        //    p.Add(new Chunk(image3, 15, 0));
        //    //p.Add(new Chunk(image4, 35, 0));
        //    p.Add(new Chunk(image5, 45, 0));
        //    //p.Add(new Chunk(image6, -1, 0));
        //    cell2.AddElement(p);
        //    foreach (var _shift in shift)
        //    {
        //        table.AddCell(cell2);
        //    }

        //    //table.AddCell(cell2);
        //    //table.AddCell(cell2);

        //    //pdfDoc.Add(table);
        //    for (var i = 0; i < quater.Buildings.Count(); i++)
        //    {
        //        addCell(table, quater.Buildings[i].BuildingName, quater.Buildings[i].FireDrillTypes.Count);
        //        for (var kk = 0; kk < quater.Buildings[i].FireDrillTypes.Count; kk++)
        //        {
        //            var ii = quater.Buildings[i].FireDrillTypes[kk].Id;
        //            addCell(table, quater.Buildings[i].FireDrillTypes[kk].FireDrillType, 1);
        //            for (var j = 0; j < quater.Buildings[i].Shifts.Count(); j++)
        //            {
        //                string date = string.Empty;
        //                var drill = quater.Buildings[i].Shifts[j].Exercises.Where(x => x.Name == quater.Buildings[i].FireDrillTypes[kk].FireDrillType).FirstOrDefault();
        //                if (drill != null)
        //                {
        //                    if (drill.Date.HasValue && drill.Date != null)
        //                    {
        //                        date = drill.Date.Value.ToString("ddd, MMM d, yyyy");
        //                        //string IsAnnounced = drill.Announced ? "Yes" : "No";
        //                        string IsDone = drill.Status == 0 ? "Plan" : "Done";
        //                        string firedrillstring = $"{date + " "} {HCF.Web._commonModelFactory.ConvertToAMPM(drill.Time) + " "} {IsDone} {"\n"} {drill.NearBy}";
        //                        addCell(table, firedrillstring, 1);
        //                    }
        //                    else { addCell(table, "", 1); }
        //                }

        //                //if (quater.Buildings[i].Shifts[j].Exercises[ii].Date.HasValue && quater.Buildings[i].Shifts[j].Exercises[ii].Date != null)
        //                //{
        //                //    date = quater.Buildings[i].Shifts[j].Exercises[ii].Date.Value.ToString("ddd, MMM d, yyyy");
        //                //    string IsAnnounced = quater.Buildings[i].Shifts[j].Exercises[ii].Announced ? "Yes" : "No";
        //                //    string IsDone = quater.Buildings[i].Shifts[j].Exercises[ii].Status == 0 ? "Plan" : "Done";
        //                //    string firedrillstring = $"{date + " "} {quater.Buildings[i].Shifts[j].Exercises[ii].StartTime + " "} {IsAnnounced + " "} {IsDone} {"\n"} {quater.Buildings[i].Shifts[j].Exercises[ii].NearBy}";
        //                //    addCell(table, firedrillstring, 1);
        //                //}
        //                //else { addCell(table, "", 1); }
        //                //addCell(table, "Wed, Mar 18, 2020. 03:20 YES PLAN ?", 1);                     
        //            }
        //        }
        //    }
        //    pdfDoc.Add(table);
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();
        //    return pdfDoc;
        //}

        //public string PrintFiredrillMatrixInbytes(int buildingTypeId, int quarterNo, int year)
        //{
        //    var mem = new MemoryStream();
        //    Document pdfDoc = new Document();
        //    pdfDoc = CreateFiredrillMatrix(buildingTypeId, quarterNo, year, mem);
        //    var pdf = mem.ToArray();
        //    return Convert.ToBase64String(pdf);
        //}

        //public ActionResult PrintFiredrillMatrix(int buildingTypeId, int quarterNo, int year)
        //{
        //    Document pdfDoc = new Document();
        //    pdfDoc = CreateFiredrillMatrix(buildingTypeId, quarterNo, year, Response.OutputStream);
        //    string fileName = System.Web.HttpUtility.UrlEncode("FireDrillMatrix_" + "Q" + quarterNo + "_" + year + ".pdf");
        //    CreatePdfFile(pdfDoc, fileName);
        //    return View();
        //}

        public ActionResult PrintFiredrillMatrix(string buildingIds, string locationGroupIds, string mode, int year, int printtimeformat)
        {
            Enum.TryParse(mode, out FireDrillMode enumMode);
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                pdfDoc = GenerateFiredrillMatrix(buildingIds, locationGroupIds, enumMode.ToString(), year, printtimeformat, ms);
                string fileName = System.Web.HttpUtility.UrlEncode("FireDrillMatrix_" + "Y_" + year + ".pdf");
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        private Document GenerateFiredrillMatrix(string buildingIds, string locationGroupIds, string mode, int year, int timeformat, Stream outputStream)
        {
            var quater = _exercisesService.GetQuarterSettings(buildingIds, locationGroupIds, mode, year, false);

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, outputStream);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            SetHeader(out table);
            pdfDoc.Add(table);

            Paragraph para = new Paragraph("FIRE DRILL MATRIX", TitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            para = new Paragraph("Year: " + quater.Year, CellFontBoldSmall)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            pdfDoc.Add(para);

            table = new PdfPTable(6)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };
            table.SetWidths(new float[] { 10f, 10f, 20f, 20f, 20f, 20f });

            addCell(table, " ", 1, 2);
            for (var kk = 1; kk < 5; kk++)
            {
                BDO.Enums.QuarterNo enums = (BDO.Enums.QuarterNo)kk;
                addCell(table, string.Format("Quarter {0} ({1})", kk, enums.GetDescription()), 1, 1);
            }
            for (var i = 0; i < quater.Buildings.Count; i++)
            {
                addCell(table, quater.Buildings[i].BuildingName, 1, 6);
                for (var kk = 0; kk < quater.Buildings[i].Shifts.Count; kk++)
                {
                    for (int m = 0; m < quater.Buildings[i].Shifts[kk].FireDrillTypes.Count; m++)
                    {
                        if (quater.Buildings[i].Shifts[kk].StartTime != null && quater.Buildings[i].Shifts[kk].EndTime != null)
                        {
                            addCell(table, $"{quater.Buildings[i].Shifts[kk].Name} ({quater.Buildings[i].Shifts[kk].StartTime.ConvertToString()} -{quater.Buildings[i].Shifts[kk].EndTime.ConvertToString()})", 1, 1);
                        }
                        else
                        {
                            addCell(table, quater.Buildings[i].Shifts[kk].Name, 1, 1);
                        }
                        addCell(table, quater.Buildings[i].Shifts[kk].FireDrillTypes[m].FireDrillType, 1, 1);
                        var kkk = 0;
                        for (kkk = 1; kkk < 5; kkk++)
                        {
                            string date = string.Empty;
                            string dateday = string.Empty;
                            var drill = quater.Buildings[i].Shifts[kk].Exercises.Where(x => x.Name == quater.Buildings[i].Shifts[kk].FireDrillTypes[m].FireDrillType && x.QuarterNo == kkk).FirstOrDefault();
                            if (drill != null)
                            {
                                BDO.Enums.FiredrillDocStatus enums = (BDO.Enums.FiredrillDocStatus)drill.FiredrillDocStatus;
                                if (drill.Date.HasValue && drill.Date != null)
                                {
                                    dateday = drill.Date.Value.ToString("ddd");
                                    date = drill.Date.Value.ToString("MMM d, yyyy");
                                    //string IsAnnounced = drill.Announced ? "Yes" : "No";
                                    string IsDone = drill.Status == 0 ? "Scheduled" : "Completed";
                                    string firedrillstring = $" {dateday + "\n"} {date + "\n"} {ChangeTimeformat(timeformat, drill.Time) + "\n"} {IsDone + "\n"} {drill.NearBy + "\n"} {enums.GetDescription()}";
                                    addCell(table, firedrillstring, 1, 1);
                                }
                                else { addCell(table, "", 1, 1); }
                            }
                        }
                    }
                }

            }
            pdfDoc.Add(table);
            //pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }


        private string ChangeTimeformat(int timeformat, TimeSpan? Time) // 1 Means 12 hours format , 0-means 24 hours format
        {
            if (timeformat == 1)
            {
                return _commonModelFactory.ConvertToAMPM(Time);
            }
            else { return Time.ToString(); }
        }

        private static void addCell(PdfPTable table, string text, int rowspan, int colspan)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.Rowspan = rowspan;
            cell.Colspan = colspan;
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

        #endregion

        #region Document Report

        public ActionResult DocumentReport()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var datavalues = _session.combinevaluearr.ToString();
                List<string> filesCol = new List<string>();//(List<string>)Session["attachedfilepaths"];
                string[] Data = datavalues.Split(',');
                string fromdate = Data[0].ToString() + "," + Data[1].ToString();
                string todate = Data[2].ToString() + "," + Data[3].ToString();
                int documenttype = Convert.ToInt32(Data[4]);
                //InspectionEPDocs objExercise = new InspectionEPDocs();
                var objExercise = _transactionService.GetDocumentReport(UserSession.CurrentUser.UserId);
                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate) && documenttype != 0)
                {
                    objExercise = objExercise.Where(x => x.DtEffectiveDate.Value.Date >= Convert.ToDateTime(fromdate).Date
                    && x.DtEffectiveDate.Value.Date <= Convert.ToDateTime(todate).Date && x.UploadDocTypeId == documenttype).OrderByDescending(x => x.CreatedDate).ToList();
                }
                else
                {
                    objExercise = objExercise.Where(x => x.DtEffectiveDate.Value.Date >= Convert.ToDateTime(fromdate).Date
                   && x.DtEffectiveDate.Value.Date <= Convert.ToDateTime(todate).Date).OrderByDescending(x => x.CreatedDate).ToList();
                }
                if (objExercise.Count > 0)
                {
                    foreach (var model in objExercise)
                    {
                        filesCol.Add(model.Path);
                    }
                }
                //var a = Guid.NewGuid();
                //System.IO.FileStream fs = null; //
                //fs = new FileStream(a.ToString() + ".pdf", FileMode.Create);
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                SetHeader(out table);
                pdfDoc.Add(table);
                Paragraph para = new Paragraph("Document Report", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                var widths = new float[] { 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f };
                table = new PdfPTable(8)
                {
                    WidthPercentage = 100,
                };
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 10f;
                table.SetWidths(widths);
                PdfPCell Documentname = new PdfPCell(new Phrase("Document Name", CellBoldFont));
                Documentname.Colspan = 2;
                table.AddCell(Documentname);
                table.AddCell(new Phrase("Next Due", CellBoldFont));
                PdfPCell Filename = new PdfPCell(new Phrase("File Name", CellBoldFont));
                Filename.Colspan = 2;
                table.AddCell(Filename);
                PdfPCell DocumentType = new PdfPCell(new Phrase("Document Type", CellBoldFont));
                DocumentType.Colspan = 1;
                table.AddCell(DocumentType);
                PdfPCell UploadDate = new PdfPCell(new Phrase("Uploaded Date", CellBoldFont));
                UploadDate.Colspan = 1;
                table.AddCell(UploadDate);
                PdfPCell StandardEp = new PdfPCell(new Phrase("Standard, EP", CellBoldFont));
                StandardEp.Colspan = 1;
                table.AddCell(StandardEp);
                for (int i = 0; i < objExercise.Count; i++)
                {
                    PdfPCell docname = new PdfPCell(new Phrase(objExercise[i].DocumentType == null ? "" : objExercise[i].DocumentType.Name));
                    docname.Colspan = 2;
                    table.AddCell(docname);
                    table.AddCell(new Phrase(objExercise[i].ExpiredDate.ToFormatDate()));
                    PdfPCell filename = new PdfPCell(new Phrase(objExercise[i].DocumentName));
                    filename.Colspan = 2;
                    table.AddCell(filename);
                    if (objExercise[i].UploadDocTypeId == 106)
                    {
                        table.AddCell(new Phrase("Policies"));
                    }
                    else if (objExercise[i].UploadDocTypeId == 107)
                    {
                        table.AddCell(new Phrase("Report"));
                    }
                    else if (objExercise[i].UploadDocTypeId == 108)
                    {
                        table.AddCell(new Phrase("Misc.Docs"));
                    }
                    else
                    {
                        table.AddCell(new Phrase("Asset Report"));
                    }
                    table.AddCell(new Phrase(objExercise[i].CreatedDate.ToFormatDate()));
                    table.AddCell(new Phrase(objExercise[i].EPDetail == null ? "" : objExercise[i].EPDetail.StandardEp));
                }
                pdfDoc.Add(table);
                for (int j = 0; j < objExercise.Count; j++)
                {
                    pdfDoc.NewPage();
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,

                        //SpacingBefore = 20f,
                        //SpacingAfter = 20f
                    };
                    string url = Path.GetFileName(filesCol[j]);
                    if (Path.GetExtension(url).ToLower() == ".pdf")
                    {
                        PdfPCell chunkcell = new PdfPCell(new Phrase(objExercise[j].DocumentName.Replace('_', ' ').Replace('-', ' ').ToString(), TitleFontS))
                        {
                            Border = 0,
                            Colspan = 1,
                            FixedHeight = 20f

                        };
                        table.AddCell(chunkcell);
                        var reader = new PdfReader(new Uri(_commonModelFactory.FilePath(filesCol[j])));
                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            var page = pdfWriter.GetImportedPage(reader, i);
                            Image img = Image.GetInstance(page);
                            PdfPCell cell = new PdfPCell(img, true)
                            {
                                FixedHeight = 700f
                            };
                            table.AddCell(cell);
                        }
                    }
                    else if (Path.GetExtension(url).ToLower() == ".jpg" || Path.GetExtension(url).ToLower() == ".png")
                    {
                        PdfPCell chunkcell = new PdfPCell(new Phrase(objExercise[j].DocumentName.Replace('_', ' ').Replace('-', ' ').ToString(), TitleFontS))
                        {
                            Border = 0,
                            Colspan = 1,
                            FixedHeight = 20f,

                        };
                        table.AddCell(chunkcell);
                        Image image = Image.GetInstance(new Uri(_commonModelFactory.FilePath(filesCol[j])));
                        //image.ScaleToFit(250f, 250f);
                        //image.setScaleToFitHeight(false);
                        PdfPCell cell = new PdfPCell(image, true)
                        {
                            FixedHeight = 700f
                        };
                        //cell.AddElement(image);
                        table.AddCell(cell);
                    }
                    else if (Path.GetExtension(url).ToLower() == ".xlsx" || Path.GetExtension(url).ToLower() == ".xlsm" || Path.GetExtension(url).ToLower() == ".xml")
                    {
                    }
                    else { }
                    pdfDoc.Add(table);
                }
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "Document Type" + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        #endregion



        #region ILSM Reports

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateTilsmReports(int tilsmId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var pdfDoc = new Document();
                var mem = new MemoryStream();
                TIlsm viewModel = new TIlsm();
                TIlsm objTilsm = _ilsmService.GetIlsmInspection(tilsmId);
                if (objTilsm != null)
                {
                    pdfDoc = CreateTilsmReport(tilsmId, ms, objTilsm);
                    string fileName = "ILSM_" + objTilsm.IncidentId + ".pdf";
                    CreatePdfFile(pdfDoc, fileName, ms);
                    SaveInDatabase(tilsmId, viewModel, fileName);
                }
            }
            return View();
        }

        private void SaveInDatabase(int tilsmId, TIlsm viewModel, string fileName)
        {
            viewModel.TIlsmId = tilsmId;
            viewModel.FileName = fileName;
            viewModel.FilesContent = CreateTilsmReportsbytes(tilsmId);
            string postData = _commonModelFactory.JsonSerialize<TIlsm>(viewModel);
            string uri = APIUrls.Goal_GenerateIlsmReport;
            int StatusCode = Convert.ToInt32(BDO.Enums.HttpReponseStatusCode.Success);
            _httpService.CallPostMethod(postData, uri, ref StatusCode);
        }

        [HttpGet]
        public string CreateTilsmReportsbytes(int tilsmId)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TIlsm objTilsm = _ilsmService.GetIlsmInspection(tilsmId);
            pdfDoc = CreateTilsmReport(tilsmId, mem, objTilsm);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        private Document CreateTilsmReport(int tilsmId, Stream streamOutput, TIlsm objTilsm)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            string footerTitle = $"{"Incident #"}: {objTilsm.IncidentId}";
            pdfWriter.PageEvent = new PDFFooter(footerTitle);
            pdfDoc.Open();
            //Table
            PdfPTable table;
            SetHeader(out table);
            pdfDoc.Add(table);
            Paragraph para = new Paragraph("ILSM Report", TitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f,
            };
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            PdfPCell cell3 = new PdfPCell
            {
                Border = 0,
                Colspan = 2,
            };
            cell3.AddElement(line);
            table.AddCell(cell3);

            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f
            };
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cellt = new PdfPCell(new Phrase(Localize.Resource.IncidentNo + ": " + objTilsm.IncidentId, CellFontBold))
            {

            };
            table.AddCell(cellt);
            cellt = new PdfPCell(new Phrase("ILSM Date" + ": " + string.Format("{0} {1}", objTilsm.llsmDate != default ? $"{objTilsm.llsmDate.ToClientTime().ToFormatDate()}" : "NA", (objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : ""), CellFontBold)))
            {
            };
            table.AddCell(cellt);

            //table.AddCell(new Phrase(Localize.Resource.IncidentNo + ": " + objTilsm.IncidentId, CellFontBold));
            //table.AddCell(new Phrase(Localize.Resource.CreatedDate + ": " + objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

            pdfDoc.Add(table);

            float[] widths = new float[] { 30f, 70f };
            table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
            table.SetWidths(widths);

            table.AddCell(new Phrase(Localize.Resource.Name, CellFont));
            table.AddCell(new Phrase(objTilsm.Notes, CellFont));

            table.AddCell(new Phrase(Localize.Resource.StandardEP, CellFont));
            if (objTilsm.TicraId.HasValue)
                table.AddCell(new Phrase("Construction ILSM ", CellFont));
            else if (objTilsm.SourceInspection.EPDetails != null)
                table.AddCell(new Phrase(objTilsm.SourceInspection.EPDetails.StandardEp, CellFont));
            else
                table.AddCell(new Phrase("Manual ILSM ", CellFont));

            //table.AddCell(new Phrase(Localize.Resource.Location, CellFont));

            //table.AddCell(new Phrase(objTilsm.Floor != null ? $"{objTilsm.Floor.Building.BuildingName},{objTilsm.Floor.FloorName}"
            //    : "NA", CellFont));
            table.AddCell(new Phrase("User", CellFont));
            table.AddCell(new Phrase(objTilsm.Inspector != null ? $"{objTilsm.Inspector.FullName}" : "NA", CellFont));
            //table.AddCell(new Phrase("Date", CellFont));
            //table.AddCell(new Phrase(objTilsm.llsmDate != null ? $"{objTilsm.llsmDate.ToFormatDate()}": "NA", CellFont));
            //table.AddCell(new Phrase("Time", CellFont));
            //table.AddCell(new Phrase(objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : "NA", CellFont));

            table.AddCell(new Phrase(Localize.Resource.Status, CellFont));
            table.AddCell(new Phrase(_commonModelFactory.GetIlsmStatus((int)objTilsm.Status), CellFont));

            //table.AddCell(new Phrase(Localize.Resource.ILSMDate, CellFont));
            //table.AddCell(new Phrase(objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

            table.AddCell(new Phrase("Completed Date", CellFont));
            table.AddCell(new Phrase((int)objTilsm.Status == 1 ? objTilsm.CompletedDate.HasValue ? objTilsm.CompletedDate.Value.ToClientTime().ToFormatDate() : "NA" : "NA", CellFont));

            pdfDoc.Add(table);

            para = new Paragraph("Location(s):", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };

            pdfDoc.Add(para);

            table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 5f,
                SpacingAfter = 10f
            };
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            if (objTilsm.Buildings.Count > 0)
            {
                foreach (var building in objTilsm.Buildings)
                {
                    table.AddCell(new Phrase($"{building.BuildingName}", CellFont));
                    foreach (var floor in building.Floor)
                    {
                        table.AddCell(new Phrase($"    {floor.FloorName}", CellFont));
                    }
                }
            }

            pdfDoc.Add(table);

            para = new Paragraph("Comments:", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdfDoc.Add(para);

            table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 5f,
                SpacingAfter = 10f
            };
            if (objTilsm.IlsmComments.Count > 0)
            {
                foreach (var cmt in objTilsm.IlsmComments)
                {
                    table.AddCell(new Phrase(
                        $"{cmt.Comment} ({cmt.AddedByUserProfile.FullName} {"on "} {cmt.AddedDate.CastLocaleDate()})", CellFont));

                }
            }
            //else
            //{
            //    PdfPCell cell = new PdfPCell(new Phrase("N/A", CellFont))
            //    {
            //        Border = 0,
            //        Colspan = 2,
            //    };
            //    table.AddCell(cell);
            //}

            pdfDoc.Add(table);

            para = new Paragraph("Deficiencies:", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdfDoc.Add(para);

            table = new PdfPTable(1)
            {
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 5f,
                SpacingAfter = 10f
            };

            if (objTilsm.Deficiencies.Count > 0)
            {
                foreach (var def in objTilsm.Deficiencies.Select(x => x.Steps).ToList())
                {
                    foreach (var step in def)
                    {
                        table.AddCell(new Phrase($"{step.Step}", CellFont));
                    }
                }
            }
            else
            {
                PdfPCell cell = new PdfPCell(new Phrase("N/A", CellFont))
                {
                    Border = 0,
                    Colspan = 2,
                };
                table.AddCell(cell);
            }
            pdfDoc.Add(table);

            widths = new float[] { 70f, 30f };
            table = new PdfPTable(2)
            {
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                //SpacingBefore = 10f,
                SpacingAfter = 10f
            };
            table.SetWidths(widths);

            List<int> epsID = new List<int>();
            foreach (var item in objTilsm.MainGoal.Where(x => x.EPDetailId.HasValue).OrderBy(x => x.Goal).ToList())
            {
                if (!epsID.Contains(item.EPDetailId.Value))
                {
                    epsID.Add(item.EPDetailId.Value);
                    var standardEP = objTilsm.TriggerEps.Where(x => x.EPDetailId == item.EPDetailId).FirstOrDefault().StandardEP;
                    PdfPCell cell = new PdfPCell(new Phrase(standardEP, CellFont))
                    {
                        Border = 0,
                        Colspan = 2,
                    };
                    table.AddCell(cell);
                }
                PdfPCell cell1 = new PdfPCell(new Phrase(item.Goal, CellFontBold))
                {
                    Border = 0,
                    Colspan = 2,
                };
                table.AddCell(cell1);
                foreach (var step in item.Steps)
                {
                    //if (step.Status != -3)
                    //{
                    table.AddCell(new Phrase(step.Step, CellFont));
                    table.AddCell(new Phrase(@Enum.GetName(typeof(BDO.Enums.InspectionStatus), step.Status), CellFont));
                    foreach (var cmt in step.StepsComments)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(
                            $"{cmt.Comment} ({cmt.AddedByUserProfile.FullName} {"on "} {cmt.AddedDate.CastLocaleDate()})", CellFont))
                        {
                            Border = 0,
                            Colspan = 2,
                        };
                        table.AddCell(cell);
                    }
                    //}
                    //else
                    //{
                    //    table.AddCell(new Phrase());
                    //    table.AddCell(new Phrase(@Enum.GetName(typeof(BDO.Enums.InspectionStatus), step.Status), CellFont));
                    //}
                    PdfPCell imagecell = new PdfPCell() { Colspan = 2, Border = 0 };
                    PdfPTable tableWork = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = 0 };
                    //widths = new float[] { 20f, 20f, 20f, 20f, 20f };
                    //tableWork.SetTotalWidth(widths);
                    tableWork.DefaultCell.Border = Rectangle.NO_BORDER;
                    var TInspectionFiles = step.TInspectionFiles.Where(x => x.IsDeleted == false).ToList();
                    var blankcellcount = TInspectionFiles.Count > 0 ? (TInspectionFiles.Count > 5 ? (TInspectionFiles.Count % 5) : (5 - TInspectionFiles.Count)) : 0;
                    foreach (var files in TInspectionFiles)
                    {
                        PdfPCell nobordercell = new PdfPCell() { Border = 0 };
                        Image image = Image.GetInstance(_commonProvider.GetS3StaticImage("~/images/icons/document_icon.png"));
                        image.ScaleAbsolute(10, 15);
                        Chunk cImage = new Chunk(image, 0, 0, false);
                        cImage.SetAnchor(_commonModelFactory.FilePath(files.FilePath));
                        nobordercell.AddElement(cImage);
                        tableWork.AddCell(nobordercell);
                    }
                    if (blankcellcount > 0)
                    {
                        for (int i = 0; i < blankcellcount; i++)
                        {
                            tableWork.AddCell(string.Empty);
                        }
                    }
                    imagecell.AddElement(tableWork);
                    table.AddCell(imagecell);
                }
                line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.DottedLineSeparator()));
                cell3 = new PdfPCell
                {
                    Border = 0,
                    Colspan = 2,
                };
                cell3.Border = Rectangle.TOP_BORDER;
                cell3.AddElement(line);

                table.AddCell(cell3);
            }
            pdfDoc.Add(table);

            if (objTilsm.TicraId.HasValue)
            {

            }
            else if (objTilsm.SourceInspection.TInspectionActivity.Count > 0)
            {
                para = new Paragraph("Assets Information", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(3)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };
                foreach (var item in objTilsm.SourceInspection.TInspectionActivity)
                {
                    table.AddCell(new Phrase(item.TFloorAssets.AssetNo, CellFont));
                    table.AddCell(new Phrase(item.TFloorAssets.Name, CellFont));
                    table.AddCell(new Phrase(@Enum.GetName(typeof(BDO.Enums.InspectionStatus), item.Status), CellFont));
                }
                pdfDoc.Add(table);
            }
            //pdfWriter.PageEvent = new PDFFooter();
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        #endregion

        #region Ceiling Permit

        public static byte[] GetEmbeddedFileData(string pdfFileName, string embeddedFileName)
        {
            byte[] attachedFileBytes = null;
            var reader = new iTextSharp.text.pdf.PdfReader(pdfFileName);
            if (reader != null)
            {
                var root = reader.Catalog;
                if (root != null)
                {
                    var names = root.GetAsDict(iTextSharp.text.pdf.PdfName.NAMES);
                    if (names != null)
                    {
                        var embeddedFiles = names.GetAsDict(iTextSharp.text.pdf.PdfName.EMBEDDEDFILES);
                        if (embeddedFiles != null)
                        {
                            var namesArray = embeddedFiles.GetAsArray(iTextSharp.text.pdf.PdfName.NAMES);
                            if (namesArray != null)
                            {
                                int n = namesArray.Size;
                                for (int i = 0; i < n; i++)
                                {
                                    i++;
                                    var fileArray = namesArray.GetAsDict(i);
                                    var file = fileArray.GetAsDict(iTextSharp.text.pdf.PdfName.EF);
                                    foreach (iTextSharp.text.pdf.PdfName key in file.Keys)
                                    {
                                        string attachedFileName = fileArray.GetAsString(key).ToString();
                                        if (attachedFileName == embeddedFileName)
                                        {
                                            var stream = (iTextSharp.text.pdf.PRStream)iTextSharp.text.pdf.PdfReader.GetPdfObject(file.GetAsIndirectObject(key));
                                            attachedFileBytes = iTextSharp.text.pdf.PdfReader.GetStreamBytes(stream);
                                            break;
                                        }
                                    }
                                    if (attachedFileBytes != null) break;
                                }
                            }
                        }
                    }
                }
                reader.Close();
            }
            return attachedFileBytes;
        }
        public ActionResult CeilingPermit(int? ceilingPermitId, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                CeilingPermit objceilingPermit = new CeilingPermit();

                if (ceilingPermitId > 0)
                {
                    objceilingPermit = _permitService.GetCeilingPermit(ceilingPermitId.Value);

                }

                if (objceilingPermit != null)
                {
                    pdfDoc = _pdfService.CreateCeilingPermit(ceilingPermitId.Value, ms, objceilingPermit, IsAttachmentIncluded.Value);
                    string fileName = System.Web.HttpUtility.UrlEncode(objceilingPermit.PermitNo + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }


            }

            return View();
        }
        #endregion

        #region Fire System Bypass Permit
        public ActionResult FireSystemByPassPermitReports(int tfsbPermitId, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var mem = new MemoryStream();
                TFireSystemByPassPermit objTFireSystemByPassPermit = _permitService.GetFSBPermit(tfsbPermitId);
                if (objTFireSystemByPassPermit != null)
                {
                    pdfDoc = _pdfService.CreateFireSystemByPassPermit(tfsbPermitId, ms, objTFireSystemByPassPermit, IsAttachmentIncluded.Value);
                    //string fileName = "FSBP_" + objTFireSystemByPassPermit.TFSBPermitId + ".pdf";
                    string fileName = System.Web.HttpUtility.UrlEncode(objTFireSystemByPassPermit.PermitNo + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }
            }
            return View();
        }
        #endregion

        public int getRandomNumber()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        #region TinspectionActivity

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateTinspectionActivityReports(Guid? activityId, string submit, bool isrouteInsp = false)
        {
            var pdfDoc = new Document();
            string fileName = "InspectionReports_" + activityId + ".pdf";
            var tfiles = new TFiles();
            var mem = new MemoryStream();
            //var tfiles = new TFiles();
            var inspection = _transactionService.GetActivityHistory(activityId.Value);
            if (inspection != null && inspection.TInspectionActivity.FirstOrDefault() != null && inspection.TInspectionActivity.FirstOrDefault().ReportFile != null)
            {
                tfiles = inspection.TInspectionActivity.FirstOrDefault().ReportFile;
            }
            else
            {
                //pdfDoc = CreateTinspectionActivityReports(activityId, Response.OutputStream, isrouteInsp, inspection);
                var base64EncodePdf = CreateTinspectionActivityReportsbytes(activityId, isrouteInsp);
                if (!string.IsNullOrEmpty(base64EncodePdf))
                {
                    TFiles files = new TFiles
                    {
                        ActivityId = activityId.Value,
                        FileName = fileName,
                        FileContent = base64EncodePdf
                    };
                    tfiles = _commonService.SaveTFile(files, UserSession.CurrentUser.UserId, "inspectionreports");
                }
                //CreatePdfFile(pdfDoc, fileName);
            }

            //if (tfiles != null && !string.IsNullOrEmpty(tfiles.FilePath))
            //{
            //    FileInfo fileInfo = new FileInfo(HCF.Web._commonModelFactory.CommonFilePath(tfiles.FilePath));
            //    if (fileInfo.Exists)
            //    {
            //        Response.Clear();
            //        Response.AddHeader("Content-Disposition", "inline;attachment; filename=" + fileInfo.Name);
            //        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            //        Response.ContentType = "application/octet-stream";
            //        Response.Flush();
            //        Response.WriteFile(fileInfo.FullName);
            //        Response.End();
            //    }
            //}

            tfiles.FileContent = "";

            return Json(new { Result = _commonModelFactory.FilePath(tfiles.FilePath), buttonType = submit, tfiles = tfiles });
        }


        [HttpGet]
        public string CreateTinspectionActivityReportsbytes(Guid? activityId, bool isrouteInsp = false)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            var inspection = _transactionService.GetActivityHistory(activityId.Value);
            pdfDoc = CreateTinspectionActivityReports(activityId, mem, isrouteInsp, inspection);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }


        public Document CreateTinspectionActivityReports(Guid? activityId, Stream streamOutput, bool isrouteInsp, Inspection inspection)
        {
            var objTInspectionActivity = new TInspectionActivity();
            var inspectiondocs = new List<InspectionEPDocs>();
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            //Table
            PdfPTable table;
            SetHeader(out table);
            pdfDoc.Add(table);
            Paragraph para = new Paragraph("Inspection Report", TitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            if (!isrouteInsp)
            {

                //if (inspection != null && inspection.InspectionId != 0)
                //{
                //    inspectiondocs = Transaction.GetInspectionDocs(inspection.InspectionId).ToList();
                //}
                if (inspection != null)
                {
                    table = new PdfPTable(4);
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 10f;

                    table.AddCell(new Phrase(Localize.Resource.StandardEP, CellFont));
                    table.AddCell(new Phrase(inspection.EPDetails.StandardEp, CellFont));
                    table.AddCell("");
                    table.AddCell("");
                    //table.AddCell(new Phrase(Localize.Resource.Score, CellFont));
                    //table.AddCell(new Phrase(inspection.EPDetails.Scores.Name, CellFont));
                    objTInspectionActivity = inspection.TInspectionActivity[0];
                    if (objTInspectionActivity == null || objTInspectionActivity.ActivityType != 2)
                    {
                        table.AddCell(new Phrase(Localize.Resource.InspectorName, CellFont));
                        table.AddCell(new Phrase(inspection.TInspectionActivity[0].UserProfile.FullName, CellFont));
                        table.AddCell(new Phrase(Localize.Resource.InspectionDate, CellFont));
                        table.AddCell(new Phrase(inspection.TInspectionActivity[0].ActivityInspectionDate.Value.ToClientTime().ToFormatDateTime(), CellFont));
                    }
                    table.AddCell(new Phrase(Localize.Resource.Description, CellFont));
                    PdfPCell cell = new PdfPCell(new Phrase(inspection.EPDetails.Description, CellFont))
                    {
                        Border = 0,
                        Colspan = 4
                    };
                    table.AddCell(cell);
                    table.AddCell(new Phrase(Localize.Resource.Comment, CellFont));
                    PdfPCell commentcell = new PdfPCell(new Phrase(inspection.TInspectionActivity[0].Comment == "" ? "NA" : inspection.TInspectionActivity[0].Comment, CellFont))
                    {
                        Border = 0,
                        Colspan = 4
                    };
                    table.AddCell(commentcell);
                    //PdfPCell plancell = new PdfPCell(new Phrase(inspection.TInspectionActivity[0].TInspectionDetail[0].MainGoal.Goal == "" ? " " : inspection.TInspectionActivity[0].TInspectionDetail[0].MainGoal.Goal, CellFont))
                    //{
                    //    Border = 0,
                    //    Colspan = 4
                    //};
                    //table.AddCell(plancell);
                    pdfDoc.Add(table);

                    //if (inspectiondocs.Count > 0)
                    //{
                    //    for (int j = 0; j < inspectiondocs.Count; j++)
                    //    {
                    //        pdfDoc.NewPage();
                    //        table = new PdfPTable(1)
                    //        {
                    //            WidthPercentage = 100,
                    //            HorizontalAlignment = 0,

                    //            //SpacingBefore = 20f,
                    //            //SpacingAfter = 20f
                    //        };
                    //        //Chunk docchunk = new Chunk(inspection.TInspectionFiles[j].FileName.ToString(), TitleFontS);
                    //        //pdfDoc.Add(docchunk);
                    //        string url = Path.GetFileName(inspectiondocs[j].Path);
                    //        if (Path.GetExtension(url).ToLower() == ".pdf")
                    //        {
                    //            //Chunk docchunk = new Chunk(inspectiondocs[j].DocumentName.ToString(), TitleFontS);
                    //            PdfPCell chunkcell = new PdfPCell(new Phrase(inspectiondocs[j].DocumentName.ToString(), CellFont))
                    //            {
                    //                Border = 0,
                    //                Colspan = 1,
                    //                FixedHeight = 20f,

                    //            };
                    //            table.AddCell(chunkcell);
                    //            var reader = new PdfReader(new Uri(_commonModelFactory.FilePath(inspectiondocs[j].Path)));
                    //            for (var i = 1; i <= reader.NumberOfPages; i++)
                    //            {
                    //                var page = pdfWriter.GetImportedPage(reader, i);
                    //                Image img = Image.GetInstance(page);
                    //                PdfPCell pdfcell = new PdfPCell(img, true)
                    //                {
                    //                    FixedHeight = 700f,
                    //                };
                    //                table.AddCell(pdfcell);
                    //            }
                    //        }
                    //        else if (Path.GetExtension(url).ToLower() == ".jpg" || Path.GetExtension(url).ToLower() == ".png")
                    //        {
                    //            //Chunk docchunk = new Chunk(inspectiondocs[j].DocumentName.ToString(), TitleFontS);
                    //            PdfPCell chunkcell = new PdfPCell(new Phrase(inspectiondocs[j].DocumentName.ToString(), CellFont))
                    //            {
                    //                Border = 0,
                    //                Colspan = 1,
                    //                FixedHeight = 20f
                    //            };
                    //            table.AddCell(chunkcell);
                    //            Image image = Image.GetInstance(new Uri(_commonModelFactory.FilePath(inspectiondocs[j].Path)));
                    //            //image.setScaleToFitHeight(false);
                    //            PdfPCell imgcell = new PdfPCell(image, true)
                    //            {
                    //                FixedHeight = 700f,
                    //            };
                    //            table.AddCell(imgcell);
                    //        }
                    //        //else if (Path.GetExtension(url).ToLower() == ".xlsx" || Path.GetExtension(url).ToLower() == ".xlsm" || Path.GetExtension(url).ToLower() == ".xml")
                    //        //{
                    //        //}
                    //        else { }
                    //        pdfDoc.Add(table);
                    //    }
                    //}

                    objTInspectionActivity = inspection.TInspectionActivity[0];
                }
            }
            else
            {
                objTInspectionActivity = _insActivityService.GetRouteInspectionActivity(activityId.Value);
            }
            if (objTInspectionActivity != null)
            {
                CreateTinspectionActivity(pdfDoc, objTInspectionActivity);
            }
            //pdfWriter.PageEvent = new PDFFooter();
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        private static void CreateTinspectionActivity(Document pdfDoc, TInspectionActivity objTInspectionActivity)
        {
            PdfPCell cell;
            PdfPTable table;
            //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            if (objTInspectionActivity.ActivityType == 2)
            {
                table = new PdfPTable(9)
                {
                    WidthPercentage = 100
                };
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 3f;
                table.SpacingAfter = 3f;
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                cell = new PdfPCell
                {
                    Border = 0,
                    Colspan = 9,
                };
                cell.AddElement(line);
                table.AddCell(cell);

                //table.AddCell(new Phrase(Localize.Resource.InspectorName +":", CellFont));
                cell = new PdfPCell(new Phrase(Localize.Resource.InspectorName + ":", CellFont))
                {
                    Border = 0,
                    Colspan = 2
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(objTInspectionActivity.UserProfile.Name, CellFont))
                {
                    Border = 0,
                    Colspan = 2
                };
                table.AddCell(cell);
                //table.AddCell(new Phrase(Localize.Resource.InspectionDate +":", CellFont));
                cell = new PdfPCell(new Phrase(Localize.Resource.InspectionDate + ":", CellFont))
                {
                    Border = 0,
                    Colspan = 2
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(objTInspectionActivity.ActivityInspectionDate.Value.ToClientTime().ToFormatDateTime(), CellFont))
                {
                    Border = 0,
                    Colspan = 3
                };
                table.AddCell(cell);
                //table.AddCell("");
                //table.AddCell(new Phrase(Localize.Resource.AssetName +":", CellFont));
                cell = new PdfPCell(new Phrase(Localize.Resource.AssetName + ":" + "   " + objTInspectionActivity.TFloorAssets.Assets.Name.CastToNA(), CellFont))
                {
                    Border = 0,
                    Colspan = 3
                };
                table.AddCell(cell);
                //table.AddCell(new Phrase(Localize.Resource.AssetName + ":" + objTInspectionActivity.TFloorAssets.Assets.Name.CastToNA(), CellFont));
                table.AddCell(new Phrase(Localize.Resource.AssetNo + ":", CellFont));
                table.AddCell(new Phrase(objTInspectionActivity.TFloorAssets.AssetNo, CellFont));
                table.AddCell(new Phrase(Localize.Resource.NearBy + ":", CellFont));
                table.AddCell(new Phrase(objTInspectionActivity.TFloorAssets.NearBy.CastToNA(), CellFont));
                table.AddCell(new Phrase(Localize.Resource.Frequency + ":", CellFont));
                table.AddCell(new Phrase(objTInspectionActivity.Frequency.DisplayName.CastToNA(), CellFont));

                line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                cell = new PdfPCell
                {
                    Border = 0,
                    Colspan = 9,
                };
                cell.AddElement(line);
                table.AddCell(cell);
                pdfDoc.Add(table);
            }

            if (objTInspectionActivity.InsType == 1 || objTInspectionActivity.InsType == 0)
            {
                float[] widths = new float[] { 40f, 60f };
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    //table.DefaultCell.Border = Rectangle.NO_BORDER;
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };
                table.SetWidths(widths);
                foreach (var detail in objTInspectionActivity.TInspectionDetail)
                {
                    cell = new PdfPCell(new Phrase(detail.MainGoal.Goal, CellFont))
                    {
                        Border = 0,
                        Colspan = 2,
                    };
                    table.AddCell(cell);
                    foreach (var steps in detail.InspectionSteps.ToList())
                    {
                        string statustext = steps.Status == 1 ? Localize.Resource.CompliantText : steps.Status == 0 ? Localize.Resource.NonCompliantText : "Pending";
                        if (steps.Steps != null && steps.Steps.StepType == 2)
                            statustext = steps.InputValue;

                        table.AddCell(new Phrase(steps.Steps.Step, CellFont));
                        table.AddCell(new Phrase(statustext, CellFont));

                    }
                }
                pdfDoc.Add(table);
            }
        }

        #endregion

        #region Fire Watch Report

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FireWatchReports(string sFromdate, string sTodate)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                int timeSlotPeriod = UserSession.CurrentOrg.FireWatchTimeSlot > 0 ? UserSession.CurrentOrg.FireWatchTimeSlot : 4;
                DateTime FromDatetime = Convert.ToDateTime(sFromdate);
                DateTime ToDatetime = Convert.ToDateTime(sTodate);
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();

                PdfPTable table;
                SetHeader(out table);
                pdfDoc.Add(table);

                Paragraph para = new Paragraph("FIRE WATCH REPORT", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                double noofdays = (ToDatetime - FromDatetime).TotalDays;

                for (int i = 0; i <= noofdays; i++)
                {
                    DateTime starteddate = new DateTime(FromDatetime.Year, FromDatetime.Month, FromDatetime.Day);
                    //if (!string.IsNullOrEmpty(timeSpan))
                    //{
                    //    selecteddate = HCF.Utility.Conversion.ConvertToDateTime(double.Parse(timeSpan.Substring(0, timeSpan.Length - 3))).ToUTCTime();
                    //}
                    DateTime selecteddate = starteddate.AddDays(i);
                    var timeSlots = _fireWatchService.Get_FirewatchlogbyTimeSlot(selecteddate.ToUTCTime(), timeSlotPeriod);

                    para = new Paragraph("Report Date :" + selecteddate.ToShortDateString(), CellFontBold)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };
                    pdfDoc.Add(para);

                    table = new PdfPTable(6)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,
                        SpacingAfter = 30f
                    };


                    table.AddCell(new Phrase("Start", CellFont));
                    table.AddCell(new Phrase("Finish", CellFont));
                    table.AddCell(new Phrase("Officer", CellFont));
                    table.AddCell(new Phrase("Areas", CellFont));
                    table.AddCell(new Phrase("Round Completed", CellFont));
                    table.AddCell(new Phrase("Comments", CellFont));

                    foreach (TimeSlots oTimeSlot in timeSlots)
                    {
                        table.AddCell(new Phrase(oTimeSlot.Start.ToClientTime().ToString("t"), CellFont));
                        table.AddCell(new Phrase(oTimeSlot.End.ToClientTime().ToString("t"), CellFont));
                        foreach (var item in oTimeSlot.FireWatchLog)
                        {
                            if (item.FWLogId > 0)
                            {
                                table.AddCell(new Phrase(item.InspectorName, CellFont));
                                table.AddCell(new Phrase(item.Area, CellFont));
                                table.AddCell(new Phrase(item.RoundInspDate.Value.ToClientTime().ToString("t"), CellFont));
                                table.AddCell(new Phrase(item.Comment, CellFont));
                            }
                            else
                            {
                                table.AddCell("");
                                table.AddCell("");
                                table.AddCell("");
                                table.AddCell("");
                            }
                        }
                    }
                    pdfDoc.Add(table);
                    //pdfWriter.PageEvent = new PDFFooter();
                    pdfDoc.NewPage();
                }

                //pdfDoc.Add(table);

                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();

                string fileName = "FireWatchReport_" + DateTime.UtcNow.Ticks + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }
        public ActionResult PrintFireWatchPDF(int? logId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                SetHeader(out table);
                pdfDoc.Add(table);

                Paragraph para = new Paragraph(new Chunk("Instructions:", CellFontBoldBlueSmall));
                para.Alignment = Element.ALIGN_LEFT;
                pdfDoc.Add(para);
                para = new Paragraph(new Chunk("This form is to be completed during any planned (4+ Hours) or unplanned outage of the smoke detection, fire alarm, or suppression system. The Fire Watch Form must be filled out for each Fire Watch Event.  The Fire Watch Checklist will be completed each tour for the duration of the Fire Watch. And. All fields and all sections must be completed at the appropriate time.", CellFont));
                pdfDoc.Add(para);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,

                };

                table.DefaultCell.Border = Rectangle.NO_BORDER;

                var objScheduledLogs = new ScheduledLogs();
                objScheduledLogs = _fireWatchService.GetScheduledLogsDetail(logId).FirstOrDefault();
                if (objScheduledLogs != null)
                {
                    objScheduledLogs.StartDate = objScheduledLogs.StartDate.HasValue
                        ? objScheduledLogs.StartDate.Value.ToClientTime()
                        : objScheduledLogs.StartDate;
                    objScheduledLogs.Enddate = objScheduledLogs.Enddate.HasValue
                        ? objScheduledLogs.Enddate.Value.ToClientTime()
                        : objScheduledLogs.Enddate;


                    table.AddCell(_pdfService.AddNewCell("Building Name:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(objScheduledLogs.Buildings.BuildingName, CellFont, false));
                    pdfDoc.Add(table);
                    table = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f,
                    };

                    float[] widths = new float[] { 22.5f, 25f, 5f, 22.5f, 25f };
                    table.SetTotalWidth(widths);
                    table.DefaultCell.Border = Rectangle.NO_BORDER;

                    var StartDate = "";
                    var Enddate = "";
                    if (objScheduledLogs.StartDate.HasValue && objScheduledLogs.StartDate != null)
                    {
                        StartDate = objScheduledLogs.StartDate.Value.ToString("MMM d, yyyy");
                    }



                    if (objScheduledLogs.Enddate.HasValue && objScheduledLogs.Enddate != null)
                    {
                        Enddate = objScheduledLogs.Enddate.Value.ToString("MMM d, yyyy");
                    }



                    table.AddCell(_pdfService.AddNewCell("Effective Date:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(StartDate, CellFont, false));
                    table.AddCell("");
                    table.AddCell(_pdfService.AddNewCell("Continue Until Date:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(Enddate, CellFont, false));
                    table.AddCell(_pdfService.AddNewCell("Effective Time:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(_commonModelFactory.ConvertToAMPM(objScheduledLogs.StartDate), CellFont, false));
                    table.AddCell("");
                    table.AddCell(_pdfService.AddNewCell("Continue Until Time:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(_commonModelFactory.ConvertToAMPM(objScheduledLogs.Enddate), CellFont, false));
                    table.AddCell(_pdfService.AddNewCell("Area:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(objScheduledLogs.Area, CellFont, false));
                    table.AddCell("");
                    table.AddCell(_pdfService.AddNewCell("Comment:", CellFontBoldBlack, false));
                    table.AddCell(_pdfService.AddNewCell(objScheduledLogs.Comment, CellFont, false));
                    pdfDoc.Add(table);

                    Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                    pdfDoc.Add(line1);

                    para = new Paragraph(new Chunk("Notifications:", CellFontBoldBlueSmall));
                    para.Alignment = Element.ALIGN_LEFT;
                    pdfDoc.Add(para);


                    table = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f,

                    };

                    widths = new float[] { 23.5f, 26f, 2f, 23.5f, 26f };
                    table.SetTotalWidth(widths);
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    for (int i = 0; i < objScheduledLogs.TFirewatchNotificationType.Count(); i++)
                    {

                        if (!string.IsNullOrEmpty(objScheduledLogs.TFirewatchNotificationType[i].InitNotificationTime.ToString()))
                        {
                            objScheduledLogs.TFirewatchNotificationType[i].InitTime = TimeSpan.Parse(objScheduledLogs.TFirewatchNotificationType[i].InitNotificationTime.ToString());
                        }

                        objScheduledLogs.TFirewatchNotificationType[i].InitNotificationTime = objScheduledLogs.TFirewatchNotificationType[i].InitTime.ToString();
                        if (objScheduledLogs.TFirewatchNotificationType[i].InitTime.HasValue)
                        {
                            DateTime inittime = DateTime.Today.Add(objScheduledLogs.TFirewatchNotificationType[i].InitTime.Value);
                            objScheduledLogs.TFirewatchNotificationType[i].InitNotificationTime = inittime.ToString("hh:mm tt");

                        }



                        if (!string.IsNullOrEmpty(objScheduledLogs.TFirewatchNotificationType[i].EndNotificationTime.ToString()))
                        {
                            objScheduledLogs.TFirewatchNotificationType[i].EndTime = TimeSpan.Parse(objScheduledLogs.TFirewatchNotificationType[i].EndNotificationTime.ToString());
                        }

                        objScheduledLogs.TFirewatchNotificationType[i].EndNotificationTime = objScheduledLogs.TFirewatchNotificationType[i].EndTime.ToString();
                        if (objScheduledLogs.TFirewatchNotificationType[i].EndTime.HasValue)
                        {
                            DateTime endtime = DateTime.Today.Add(objScheduledLogs.TFirewatchNotificationType[i].EndTime.Value);
                            objScheduledLogs.TFirewatchNotificationType[i].EndNotificationTime = endtime.ToString("hh:mm tt");

                        }


                        var InitNotificationDate = "";
                        var EndNotificationDate = "";
                        if (objScheduledLogs.TFirewatchNotificationType[i].InitNotificationDate.HasValue && objScheduledLogs.TFirewatchNotificationType[i].InitNotificationDate != null)
                        {
                            InitNotificationDate = objScheduledLogs.TFirewatchNotificationType[i].InitNotificationDate.Value.ToString("MMM d, yyyy"); ;
                        }



                        if (objScheduledLogs.TFirewatchNotificationType[i].EndNotificationDate.HasValue && objScheduledLogs.TFirewatchNotificationType[i].EndNotificationDate != null)
                        {
                            EndNotificationDate = objScheduledLogs.TFirewatchNotificationType[i].EndNotificationDate.Value.ToString("MMM d, yyyy");
                        }

                        table.AddCell(_pdfService.AddNewCell(objScheduledLogs.TFirewatchNotificationType[i].FirewatchNotificationName, CellFontBoldBlack, false));
                        table.AddCell(_pdfService.AddNewCell(objScheduledLogs.TFirewatchNotificationType[i].Name, CellFont, false));
                        table.AddCell("");
                        table.AddCell("");
                        table.AddCell("");
                        table.AddCell(_pdfService.AddNewCell("Initial Notification Date:", CellFont, false));
                        table.AddCell(_pdfService.AddNewCell(InitNotificationDate, CellFont, false));
                        table.AddCell("");
                        table.AddCell(_pdfService.AddNewCell("End Notification Date:", CellFont, false));
                        table.AddCell(_pdfService.AddNewCell(EndNotificationDate, CellFont, false));
                        table.AddCell(_pdfService.AddNewCell("Initial Notification Time:", CellFont, false));
                        table.AddCell(_pdfService.AddNewCell(objScheduledLogs.TFirewatchNotificationType[i].InitNotificationTime, CellFont, false));
                        table.AddCell("");
                        table.AddCell(_pdfService.AddNewCell("End Notification Time:", CellFont, false));
                        table.AddCell(_pdfService.AddNewCell(objScheduledLogs.TFirewatchNotificationType[i].EndNotificationTime, CellFont, false));
                        table.AddCell("  ");
                        table.AddCell("  ");
                        table.AddCell(" ");
                        table.AddCell("  ");
                        table.AddCell("  ");

                    }
                    pdfDoc.Add(table);
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };

                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(_pdfService.AddNewCell("Public Safety personnel INITIALLY filling out form Public Safety personnel FINALIZING form", CellFontBoldBlack, false));
                    pdfDoc.Add(table);
                    table = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f,

                    };

                    widths = new float[] { 23.5f, 25f, 4f, 23.5f, 25f };
                    table.SetTotalWidth(widths);
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(_pdfService.AddNewCell("Print:", CellFont, false));
                    table.AddCell(_pdfService.AddNewCell(objScheduledLogs.PrintInitial, CellFont, false));
                    table.AddCell("");
                    table.AddCell(_pdfService.AddNewCell("Print:", CellFont, false));
                    table.AddCell(_pdfService.AddNewCell(objScheduledLogs.PrintFinal, CellFont, false));
                    //table.AddCell(_pdfService.AddNewCell("Sign:", CellFont, false));
                    pdfDoc.Add(table);

                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,

                    };
                    int twocolcount = 0;
                    if (objScheduledLogs.DSSign1Signature != null && objScheduledLogs.DSSign1Signature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = _pdfService.CreateSignSectionCell("Sign:", objScheduledLogs.DSSign1Signature, string.Empty, 45f);
                        table.AddCell(cellmain);
                        twocolcount++;

                        //PdfPCell cell = new PdfPCell()
                        //{
                        //    Border = 0,
                        //};
                        //Image image = Image.GetInstance(_commonModelFactory.FilePath(objScheduledLogs.DSSign1Signature.FilePath));
                        //image.ScaleAbsolute(60, 60);
                        //cell.AddElement(image);
                        //table.AddCell(cell);
                        //cell = new PdfPCell()
                        //{
                        //    Border = 0,
                        //};
                        //cell.Colspan = 3;
                        //cell.AddElement(new Phrase(objScheduledLogs.DSSign1Signature.SignByUserName + " (" + objScheduledLogs.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                        //table.AddCell(cell);

                    }
                    if (twocolcount == 0)
                    {
                        table.AddCell(_pdfService.AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                    //table.AddCell(_pdfService.AddNewCell("Sign:", CellFont, false));
                    if (objScheduledLogs.DSSign2Signature != null && objScheduledLogs.DSSign2Signature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = _pdfService.CreateSignSectionCell("Sign:", objScheduledLogs.DSSign2Signature, string.Empty, 45f);
                        table.AddCell(cellmain);
                        twocolcount++;
                        //PdfPCell cell = new PdfPCell()
                        //{
                        //    Border = 0,
                        //};
                        //Image image = Image.GetInstance(_commonModelFactory.FilePath(objScheduledLogs.DSSign2Signature.FilePath));
                        //image.ScaleAbsolute(60, 60);
                        //cell.AddElement(image);
                        //table.AddCell(cell);
                        //cell = new PdfPCell()
                        //{
                        //    Border = 0,
                        //};
                        //cell.Colspan = 3;
                        //cell.AddElement(new Phrase(objScheduledLogs.DSSign2Signature.SignByUserName + " (" + objScheduledLogs.DSSign2Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                        //table.AddCell(cell);

                    }
                    if (twocolcount == 2)
                    {
                        table.AddCell(_pdfService.AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                    pdfDoc.Add(table);
                }
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = System.Web.HttpUtility.UrlEncode("FireWatch_" + logId + ".pdf");
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();

        }
        #endregion

        #region  Daily Maintenance log Report

        public ActionResult DailyMaintenanceReport(int pmlogId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var pdfDoc = new Document(SetPaperType(), 25, 25, 25, 35);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                //BaseColor myColor = BaseColor.LIGHT_GRAY;


                PdfPTable table;
                SetHeader(out table);
                pdfDoc.Add(table);
                var para = new Paragraph("PLANT OPERATIONS DAILY MAINTENANCE LOG", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                var pmLogs = _roundsService.PMDailyLog(pmlogId);
                var cell = new PdfPCell();


                table = new PdfPTable(8)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                };

                var fileName2 = $"{pmLogs.Building.BuildingName}_{DateTime.Now.ToFormatDate().Replace(",", "").Replace(" ", "_")}.pdf";
                PdfPCell building = new PdfPCell(new Phrase("Building:", CellBoldFont));
                building.Colspan = 2;
                building.Border = Rectangle.NO_BORDER;
                table.AddCell(building);
                PdfPCell BuildingName = new PdfPCell(new Phrase((pmLogs.Building != null) ? pmLogs.Building.BuildingName : "NA", CellFont));
                BuildingName.Colspan = 6;
                BuildingName.Border = Rectangle.NO_BORDER;
                table.AddCell(BuildingName);
                PdfPCell DateandTime = new PdfPCell(new Phrase("Date & Time:", CellBoldFont));
                DateandTime.Colspan = 2;
                DateandTime.Border = Rectangle.NO_BORDER;
                table.AddCell(DateandTime);
                var stringdata = pmLogs.Date.ToFormatDate() + ',' + Conversion.ConvertToAmPm(pmLogs.Time);
                PdfPCell Datetime = new PdfPCell(new Phrase(stringdata, CellFont));
                Datetime.Colspan = 6;
                Datetime.Border = Rectangle.NO_BORDER;
                table.AddCell(Datetime);
                PdfPCell Name = new PdfPCell(new Phrase("Name:", CellBoldFont));
                Name.Colspan = 2;
                Name.Border = Rectangle.NO_BORDER;
                table.AddCell(Name);
                PdfPCell name = new PdfPCell(new Phrase(pmLogs.Name, CellFont));
                name.Colspan = 6;
                name.Border = Rectangle.NO_BORDER;
                table.AddCell(name);
                //table.AddCell(new Phrase("TIME:", CellBoldFont));
                //table.AddCell(new Phrase(Conversion.ConvertToAmPm(pmLogs.Time), CellFont));
                pdfDoc.Add(table);

                var widths = new float[] { 8f, 36f, 8f, 8f, 8f, 8f, 8f, 8f, 8f };
                table = new PdfPTable(9)
                {
                    WidthPercentage = 100,
                };
                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 10f;
                table.SetWidths(widths);
                PdfPCell serialNum = new PdfPCell(new Phrase("S.No.", CellBoldFont));
                serialNum.Colspan = 1;
                table.AddCell(serialNum);
                table.AddCell(new Phrase("Item", CellBoldFont));
                table.AddCell(new Phrase("Value", CellBoldFont));
                //table.AddCell(new Phrase(""));
                PdfPCell Expectedvalue = new PdfPCell(new Phrase("Expected Value", CellBoldFont));
                Expectedvalue.Colspan = 2;
                table.AddCell(Expectedvalue);
                //table.AddCell(new Phrase(""));
                PdfPCell formatType = new PdfPCell(new Phrase("Format", CellBoldFont));
                formatType.Colspan = 2;
                table.AddCell(formatType);
                PdfPCell Commentsheading = new PdfPCell(new Phrase("Comments", CellBoldFont));
                Commentsheading.Colspan = 2;
                table.AddCell(Commentsheading);
                int seialno = 1;
                for (int i = 0; i < pmLogs.Questionnaires.Count; i++)
                {
                    //PdfPCell serial = new PdfPCell(new Phrase(""));
                    //serial.Colspan = 1;
                    //table.AddCell(serial);
                    //table.AddCell(new Phrase(pmLogs.Questionnaires[i].Goal));                
                    //if (pmLogs.Questionnaires[i].QuestionnaireOptions.Count == 0)
                    //{
                    //    cell = new PdfPCell
                    //    {
                    //        //Border = 0,
                    //        Colspan = 4,
                    //    };
                    //    table.AddCell(cell);
                    //}
                    //else if (pmLogs.Questionnaires[i].QuestionnaireOptions.Count == 1)
                    //{
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    cell = new PdfPCell
                    //    {
                    //        //Border = 0,
                    //        Colspan = 1,
                    //    };
                    //    table.AddCell(cell);
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    cell = new PdfPCell
                    //    {
                    //        //Border = 0,
                    //        Colspan = 1,
                    //    };
                    //    table.AddCell(cell);
                    //}
                    //else if (pmLogs.Questionnaires[i].QuestionnaireOptions.Count == 2)
                    //{
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[1].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(""));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[1].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(""));
                    //}
                    //else if (pmLogs.Questionnaires[i].QuestionnaireOptions.Count == 3)
                    //{
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[1].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[2].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[0].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[1].QuestionnaireHeaderType.Name));
                    //    table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireOptions[2].QuestionnaireHeaderType.Name));
                    //}
                    //table.AddCell(new Phrase(""));
                    //PdfPCell Comments = new PdfPCell(new Phrase(""));
                    //Comments.Colspan = 2;
                    //table.AddCell(Comments);
                    for (int j = 0; j < pmLogs.Questionnaires[i].QuestionnaireSteps.Count; j++)
                    {
                        if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps.FirstOrDefault(x => x.IsDeleted == false) != null)
                        {
                            PdfPCell snum = new PdfPCell(new Phrase(seialno.ToString(), CellBoldFont));
                            snum.Colspan = 1;
                            table.AddCell(snum);
                            table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].Step));
                            if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps.Where(x => x.IsDeleted == false).Count() == 0)
                            {
                                cell = new PdfPCell
                                {
                                    //Border = 0,
                                    Colspan = 5,
                                };
                                table.AddCell(cell);
                            }
                            else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps.Where(x => x.IsDeleted == false).Count() == 1)
                            {
                                table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value));
                                //cell = new PdfPCell
                                //{
                                //    //Border = 0,
                                //    Colspan = 1,
                                //};
                                //table.AddCell(cell);
                                PdfPCell RecommendedText = new PdfPCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText));
                                RecommendedText.Colspan = 2;
                                table.AddCell(RecommendedText);
                                PdfPCell format = new PdfPCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format));
                                format.Colspan = 2;
                                table.AddCell(format);
                                //cell = new PdfPCell
                                //{
                                //    //Border = 0,
                                //    Colspan = 1,
                                //};
                                //table.AddCell(cell);
                            }
                            else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps.Where(x => x.IsDeleted == false).Count() == 2)
                            {
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null))
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null)
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null)
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else
                                {
                                    table.AddCell(new Phrase("N/A"));
                                }
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value));
                                //table.AddCell(new Phrase(""));
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null))
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null)
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null)
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else
                                {
                                    PdfPCell recommend = new PdfPCell(new Phrase("N/A"));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText));
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null))
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null)
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null)
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else
                                {
                                    PdfPCell format = new PdfPCell(new Phrase("N/A"));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format));
                                //table.AddCell(new Phrase(""));
                            }
                            else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps.Where(x => x.IsDeleted == false).Count() == 3)
                            {
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != null))
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null))
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != null))
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != null))
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value != null)
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value != null)
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value != null)
                                {
                                    var values = pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value;
                                    table.AddCell(new Phrase(values));
                                }
                                else
                                {
                                    table.AddCell(new Phrase("N/A"));
                                }

                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Value));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[1].Value));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[2].Value));
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != null))
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != null))
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != null))
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != null))
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText != null)
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText != null)
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != null)
                                {
                                    var recommendvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText;
                                    PdfPCell recommend = new PdfPCell(new Phrase(recommendvalue));
                                    recommend.Colspan = 2;
                                    table.AddCell(recommend);
                                }
                                else
                                {
                                    table.AddCell(new Phrase("N/A"));
                                }

                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].RecommendedText));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].RecommendedText));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText));
                                if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != null))
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != null))
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != null))
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if ((pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != null) && (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != null))
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format + " , " + pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format != null)
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format != null)
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else if (pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].RecommendedText != "" && pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format != null)
                                {
                                    var formatvalue = pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format;
                                    PdfPCell format = new PdfPCell(new Phrase(formatvalue));
                                    format.Colspan = 2;
                                    table.AddCell(format);
                                }
                                else
                                {
                                    table.AddCell(new Phrase("N/A"));
                                }


                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[0].Format));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[1].Format));
                                //table.AddCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].QuestionnaireStepDetail[2].Format));
                            }
                            //table.AddCell(new Phrase("F", CellFont));
                            PdfPCell Comment = new PdfPCell(new Phrase(pmLogs.Questionnaires[i].QuestionnaireSteps[j].PMLogSteps[0].Comments, CellFont));
                            Comment.Colspan = 2;
                            table.AddCell(Comment);
                            seialno++;
                        }
                    }
                }

                pdfDoc.Add(table);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.AddCell(new Phrase("COMMENTS:  \n" + pmLogs.Comments, CellFont));
                pdfDoc.Add(table);


                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.AddCell(new Phrase("ACTION TAKEN:  \n" + pmLogs.ActionTaken, CellFont));
                pdfDoc.Add(table);


                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.AddCell(new Phrase("REVIEWED BY: \n" + (pmLogs.ReviewedByUser != null ? pmLogs.ReviewedByUser.FullName : string.Empty), CellFont));
                pdfDoc.Add(table);

                //pdfWriter.PageEvent = new PDFFooter();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();

                //string fileName = "PM_" + DateTime.Now.ToFormatDate().Replace(",", "").Replace(" ", "_") + ".pdf";
                CreatePdfFile(pdfDoc, "PM_" + fileName2.Replace(" ", "_"), ms);

            }
            return View();
        }

        #endregion

        #region EpAssignmentReportEmail
        public ActionResult SendEPReport(int userId, int catId, string to, string cc, string subject, string user, string date, string selecteduser)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Success = true;
            var base64EncodePdf = "";
            base64EncodePdf = CreateEPReportDoument(userId, catId);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = "";
            switch (selecteduser)
            {
                case "-1":
                    filename = "All_EPs_" + date.Replace(":", "") + ".pdf";
                    break;
                case "0":
                    filename = "Not_Assigned_EPs_" + date.Replace(":", "") + ".pdf";
                    break;
                default:
                    filename = "EPs_Of_" + user + "_" + date.Replace(":", "") + ".pdf";
                    break;
            }
            var toUserName = "";

            string path = _commonProvider.GetContentRootPath("wwwroot");
            string emailBody = System.IO.File.ReadAllText(string.Format(path + "{0}", @"/Templates/EpAssignmentReport.html"));
            emailBody = emailBody.Replace("{UserName}", toUserName);
            emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
            emailBody = emailBody.Replace("{Body}", "Please find attached report.");


            _emailProcessor.SendMail(to, subject, cc, emailBody, "CRx_NoReply@hcfcompliance.com", filebytes, filename);
            return Json(response);
        }

        private string CreateEPReportDoument(int userId, int catId)
        {
            var ReportTitleTemp = "EP Assignment Report{0}";
            var ReportTitle = "";
            var epdetails = _epService.GetEpAssignment().ToList();
            if (userId != 0)
            {
                if (userId != -1)
                {
                    epdetails = epdetails.Where(x => x.EPUsers.Where(y => y.UserId == userId).Any()).ToList();
                }
                else
                {
                    ReportTitle = string.Format(ReportTitleTemp, " (All EPs)");
                }
                if (catId != -1)
                {
                    epdetails = epdetails.Where(x => x.Standard.CategoryId == catId).ToList();
                }

                if (userId != -1)
                {
                    var userName = _userService.GetUserProfile(userId).FullName;
                    ReportTitle = string.Format(ReportTitleTemp, " (" + userName + " , EP Count: " + epdetails.Count + ")");
                }
            }
            else
            {
                if (userId != -1)
                {
                    ReportTitle = string.Format(ReportTitleTemp, " (Unassigned EPs)");
                    epdetails = epdetails.Where(x => !x.EPUsers.Any()).ToList();
                }
                else
                {
                    ReportTitle = string.Format(ReportTitleTemp, " (All EPs)");
                }
                if (catId != -1)
                {
                    epdetails = epdetails.Where(x => x.Standard.CategoryId == catId).ToList();
                }
            }

            epdetails = epdetails.Where(x => x.IsActive).ToList();


            var mem = new MemoryStream();
            var pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, mem);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();

            //Table
            PdfPTable table;
            //PdfPCell cell;
            //Chunk chunk;
            SetHeader(out table);
            //Add table to document
            pdfDoc.Add(table);

            var para = new Paragraph(ReportTitle, TitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //pdfDoc.Add(line);

            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 10f
            };

            pdfDoc.Add(table);


            table.AddCell(new Phrase("Standard, EP", CellFont));
            table.AddCell(new Phrase("Document Type", CellFont));
            table.AddCell(new Phrase("Assets", CellFont));
            table.AddCell(new Phrase("Description", CellFont));

            foreach (var item in epdetails)
            {
                table.AddCell(new Phrase(item.StandardEp, CellFont));
                table.AddCell(new Phrase((item.DocumentType != null && item.DocumentType.Any()) ? item.DocumentType[0].Name : "", CellFont));
                table.AddCell(new Phrase(string.Join(",", item.Assets.Select(x => x.Name).ToList()), CellFont));
                table.AddCell(new Phrase(item.Description, CellFont));
            }

            pdfDoc.Add(table);



            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //pdfDoc.Add(line);

            //pdfWriter.PageEvent = new PDFFooter();
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }
        #endregion

        #region SenduserEP
        public ActionResult SendUserEP(int userId, string to, string cc, string subject)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Success = true;
            var base64EncodePdf = "";
            base64EncodePdf = CreateUserEPDoument(userId);
            byte[] filebytes = Convert.FromBase64String(base64EncodePdf);
            var filename = "UserAssignEps_Report.pdf";

            var toUserName = "";

            string path = _commonProvider.GetContentRootPath("wwwroot");
            string emailBody = System.IO.File.ReadAllText(string.Format(path + "{0}", @"/Templates/EpAssignmentReport.html"));
            emailBody = emailBody.Replace("{UserName}", toUserName);
            emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
            emailBody = emailBody.Replace("{Body}", "Please find attached report.");


            _emailProcessor.SendMail(to, subject, cc, emailBody, "sunita.rawat@inficaretech.com", filebytes, filename);
            return Json(response);
        }

        private string CreateUserEPDoument(int userId)
        {
            var ReportTitleTemp = " User Assigned EPs ";
            var ReportTitle = "";
            var epdetails = _epService.GetEpAssignment().ToList();

            var user = _userService.GetUserProfile(userId);
            var userName = user?.FullName;
            var userEmail = user?.Email;
            if (userId != 0)
            {
                if (userId != -1)
                {
                    epdetails = epdetails.Where(x => x.EPUsers.Where(y => y.UserId == userId).Any()).ToList();
                    //ReportTitle = string.Format(ReportTitleTemp, " (" + userName + "(" + userEmail + ")" + " , EP Count: " + epdetails.Count + ")");
                }
                else
                {
                    ReportTitle = string.Format(ReportTitleTemp, " (All EPs)");
                }
            }
            epdetails = epdetails.Where(x => x.IsActive).ToList();


            var mem = new MemoryStream();
            var pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, mem);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();

            PdfPTable table;
            PdfPCell cell;
            SetHeader(out table);
            pdfDoc.Add(table);
            Paragraph para = new Paragraph("User Assign Eps", UnderlineTitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 10f
            };

            cell = new PdfPCell(new Phrase("User Name: " + userName, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 30f,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Email: " + userEmail, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 30f,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.AddCell(cell);

            table.AddCell(new Phrase("Standard, EP", CellFont));
            table.AddCell(new Phrase("Description", CellFont));

            foreach (var item in epdetails)
            {
                table.AddCell(new Phrase(item.StandardEp, CellFont));
                table.AddCell(new Phrase(item.Description, CellFont));
            }


            pdfDoc.Add(table);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //pdfWriter.PageEvent = new PDFFooter();
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }
        #endregion

        #region PCRA PDF
        public ActionResult PrintPCRAPdf(int? projectId, int? tPCRAQuesId, string mode, int? doctype, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
                objQuestionTPCRA = _pcraService.GetQuestionTPCRA(projectId, tPCRAQuesId, null, doctype != null && doctype == 1 ? false : true);
                if (objQuestionTPCRA != null)
                {
                    pdfDoc = _pdfService.CreatePrintPCRAPdf(projectId, tPCRAQuesId, ms, objQuestionTPCRA, doctype, IsAttachmentIncluded.Value);

                    var docname = (doctype.HasValue && doctype.Value > 0 ? "CRA_" : "PCRA_");
                    string fileName = System.Web.HttpUtility.UrlEncode(docname + objQuestionTPCRA.PCRANumber + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);

                }
            }
            return View();
        }

        #endregion

        #region MOP

        public ActionResult MethodofprocedurePdf(int? id, bool IsAttachmentIncluded)
        {
            Document pdfDoc = new Document();
            TMOP mop = new TMOP();
            if (id > 0)
            {
                mop = _permitService.GetMethodofProcedure(id.Value);

            }

            if (mop != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pdfDoc = _pdfService.CreateMOPPermit(id.Value, ms, mop, IsAttachmentIncluded);
                    // string fileName = System.Web.HttpUtility.UrlEncode("MOP_" + id + ".pdf");
                    string fileName = System.Web.HttpUtility.UrlEncode(mop.PermitNo + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }

            }
            else
            {
                ErrorMessage = ConstMessage.Not_exist;
                return RedirectToAction("MethodofProcedure");
            }

            return View();
        }
        #endregion

        #region Lifesafety

        public ActionResult LifeSafetyFormPdf(string formId, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var mem = new MemoryStream();
                TLifeSafetyDevicesForms newForm = _permitService.GetLifeSafetyForm(Guid.Parse(formId));
                if (newForm != null)
                {
                    pdfDoc = _pdfService.CreateLifeSafetyPermit(formId, ms, newForm, IsAttachmentIncluded.Value);
                    // string fileName = System.Web.HttpUtility.UrlEncode("LSD" + (newForm.FormType == 1 ? "A_" : "R_") + formId + ".pdf");
                    string fileName = System.Web.HttpUtility.UrlEncode(newForm.PermitNo + (newForm.FormType == 1 ? "_A" : "_R") + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);

                }
            }
            return View();
        }

        #endregion

        #region HotWorkPermit
        public ActionResult HotWorkPermitPdf(int? thotworkpermitid, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                THotWorkPermits objhotWorkPermits = new THotWorkPermits();
                if (thotworkpermitid > 0)
                {
                    objhotWorkPermits = _hotWorkPermitService.GetTHotWorksPermit(thotworkpermitid.Value);

                }

                if (objhotWorkPermits != null)
                {
                    pdfDoc = _pdfService.CreateHWPPermit(thotworkpermitid.Value, ms, objhotWorkPermits, IsAttachmentIncluded.Value);
                    string fileName = System.Web.HttpUtility.UrlEncode(objhotWorkPermits.PermitNo + ".pdf");
                    //string fileName = System.Web.HttpUtility.UrlEncode("HWP_" + thotworkpermitid.Value + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }

            }
            return View();
        }
        #endregion

        #region FMC

        public ActionResult FMCPermitPdf(int? id)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                TFacilitiesMaintenanceOccurrencePermit objTFMC = new TFacilitiesMaintenanceOccurrencePermit();
                if (id > 0)
                {
                    objTFMC = _permitService.GetFacilitiesMaintenanceOccurrenceAsync(id.Value);

                }

                if (objTFMC != null)
                {
                    pdfDoc = _pdfService.CreateFMCPermit(id.Value, ms, objTFMC);
                    //string fileName = System.Web.HttpUtility.UrlEncode("FMC_" + id.Value + ".pdf");
                    string fileName = System.Web.HttpUtility.UrlEncode(objTFMC.PermitNo + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }
            }
            return View();
        }
        #endregion

        #region PrintUserEpPDF
        public ActionResult PrintUserEpPDF(int userid)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                if (userid != 0)
                {
                    pdfDoc = CreateUserEPAssignment(userid, ms);
                    string fileName = System.Web.HttpUtility.UrlEncode("UserEP_" + userid + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }
            }
            return View();
        }

        private Document CreateUserEPAssignment(int userId, Stream streamOutput)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            var epdetails = _epService.GetEpAssignment().ToList();

            var userProfile = _userService.GetUserProfile(userId);
            var userName = userProfile?.FullName;
            var userEmail = userProfile?.Email;
            if (userId != 0)
            {
                if (userId != -1)
                {
                    epdetails = epdetails.Where(x => x.EPUsers.Where(y => y.UserId == userId).Any()).ToList();
                    SetHeaderBlue(out table, userName + "(" + userEmail + ")");
                }
                else
                {
                    SetHeaderBlue(out table, "(All EPs)");
                }

                pdfDoc.Add(table);
                Paragraph para = new Paragraph("User Assign Eps", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };

                cell = new PdfPCell(new Phrase("User Name: " + userName, CellFont))
                {
                    Colspan = 2,
                    MinimumHeight = 30f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Email:" + userEmail, CellFont))
                {
                    Colspan = 2,
                    MinimumHeight = 30f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);

                table.AddCell(new Phrase("Standard, EP", CellFont));
                table.AddCell(new Phrase("Description", CellFont));

                foreach (var item in epdetails)
                {
                    table.AddCell(new Phrase(item.StandardEp, CellFont));
                    table.AddCell(new Phrase(item.Description, CellFont));
                }

                pdfDoc.Add(table);
            }
            //pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        #endregion

        #region BBI


        private Document CreatedBBIPDf(Stream streamOutput, List<Site> sites)
        {

            Document pdfDoc = new Document(SetPaperType(), 8, 0, 25, 24);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;

            SetHeaderBlue(out table, "BBI Tracking");


            //SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            PdfPTable tableapprove = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };
            pdfDoc.Add(table);

            if (sites != null && sites.Count > 0)
            {
                table = new PdfPTable(11)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f

                };

                float[] widths = new float[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 8, 2 };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.LEFT_BORDER;
                foreach (var item in sites.Where(x => x.IsActive && x.Buildings.Where(y => y.IsActive).Any()).OrderBy(x => x.Name))
                {
                    table.AddCell(_pdfService.AddNewCell("\n", smallfontbold, false, 11));
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(new Phrase("Site", smallfontbold));
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase("Site Code", smallfontbold));
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase("Address", smallfontbold));
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase("City/State", smallfontbold));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false, 0, 1));

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase(item.Name, smallfont));
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase(item.Code, smallfont));
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    cell.AddElement(new Phrase(item.Address, smallfont));
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    if ((item.CityName != null && item.CityName != "") && (item.StateName != null && item.StateName != ""))
                        cell.AddElement(new Phrase(item.CityName + "," + item.StateName, smallfont));
                    else
                        cell.AddElement(new Phrase(item.CityName + item.StateName, smallfont));

                    cell.Colspan = 2;
                    table.AddCell(cell);
                    table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false, 0, 1));


                    table.AddCell(_pdfService.AddNewCell("Building ", smallfontbold, false, 0, 1));
                    table.AddCell(_pdfService.AddNewCell("Building Code", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("Primary Occup. Type", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("% Sqr. ft.", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("% Renovated", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("Sq. Ft.", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("Beds", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("Sprinkled", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("Gov. Env. Susp.", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("# Open SPFI", smallfontbold, false));
                    table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false, 0, 1));
                    foreach (var building in item.Buildings.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                    {
                        table.AddCell(_pdfService.AddNewCell(building.BuildingName, smallfont, false, 0, 1));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingCode, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingType.Name, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null ? building.BuildingDetails.PercentageSqrFt : string.Empty, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null && building.BuildingDetails.PercentageRenovated != null && building.BuildingDetails.PercentageRenovated != "" ? building.BuildingDetails.PercentageRenovated + "%" : string.Empty, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null ? building.BuildingDetails.SquareFtRange : string.Empty, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null ? building.BuildingDetails.Beds : string.Empty, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null && building.BuildingDetails.Sprinkled ? "Y" : "N", smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null && building.BuildingDetails.GovEnvSusp ? "Y" : "N", smallfont, false));
                        table.AddCell(_pdfService.AddNewCell(building.BuildingDetails != null ? building.BuildingDetails.OpenSPFI : string.Empty, smallfont, false));
                        table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false, 0, 1));


                    }
                    cell = new PdfPCell
                    {
                        Colspan = 10,
                        Border = Rectangle.TOP_BORDER
                    };
                    table.AddCell(cell);
                    table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false));
                }



            }
            //PdfPCell cell2 = new PdfPCell();
            //cell2.Colspan = 10;
            //cell2.Border = Rectangle.TOP_BORDER;
            //table.AddCell(cell2);
            //table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false));
            pdfDoc.Add(table);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        public ActionResult PrintBBI()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var sites = _siteService.GetBBISitesBuildings();
                //  List<Site> sites = UnityHelper.Container.Resolve<ISiteService>().GetBBISitesBuildings();
                if (sites != null && sites.Count > 0)
                {
                    pdfDoc = CreatedBBIPDf(ms, sites);
                    string fileName = HttpUtility.UrlEncode("BBI_" + DateTime.Now.ToShortDateString() + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                    return View();
                }
            }
            return RedirectToRoute("bbi");
        }
        #endregion

        #region PrintPDFNonTrack


        private Document CreateNonTrackEp(Stream streamOutput, List<StandardEps> eps)
        {
            List<HCF.BDO.Category> categories = eps.GroupBy(x => x.CategoryId).Select(x => new HCF.BDO.Category { CategoryId = x.FirstOrDefault().CategoryId, Name = x.FirstOrDefault().CategoryName }).ToList();
            Document pdfDoc = new Document(SetPaperType(), 8, 8, 25, 24);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;

            SetHeaderBlue(out table, "Non Track EPs");


            //SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            PdfPTable tableapprove = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };
            pdfDoc.Add(table);

            if (eps != null && eps.Count > 0)
            {
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f

                };

                float[] widths = new float[] { 40f, 60f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.LEFT_BORDER;

                foreach (var category in categories)
                {
                    //table.AddCell(_pdfService.AddNewCell("\n", smallfontbold, false, 11));
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(new Phrase(category.Name, smallfontbold));
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    foreach (var item in eps.Where(x => x.CategoryId == category.CategoryId).GroupBy(x => x.StDescID).Select(x => x.FirstOrDefault()).ToList())
                    {
                        cell = new PdfPCell();
                        cell.AddElement(new Phrase(item.TJCStandard, smallfontbold));
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        cell.AddElement(new Phrase("EP#", smallfontbold));
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        cell.AddElement(new Phrase("Description", smallfontbold));
                        table.AddCell(cell);

                        //cell = new PdfPCell();
                        //cell.AddElement(new Phrase("Tracked?", smallfontbold));
                        //table.AddCell(cell);
                        foreach (var ep in eps.Where(x => x.StDescID == item.StDescID).ToList())
                        {
                            cell = new PdfPCell();
                            cell.AddElement(new Phrase(ep.StandardEP, smallfont));
                            table.AddCell(cell);

                            cell = new PdfPCell();
                            cell.AddElement(new Phrase(ep.Description, smallfont));
                            table.AddCell(cell);

                            //cell = new PdfPCell();
                            //cell.AddElement(new Phrase("", smallfont));
                            //table.AddCell(cell);
                        }

                    }
                }


            }
            //PdfPCell cell2 = new PdfPCell();
            //cell2.Colspan = 10;
            //cell2.Border = Rectangle.TOP_BORDER;
            //table.AddCell(cell2);
            //table.AddCell(_pdfService.AddNewCell("  ", smallfontbold, false));
            pdfDoc.Add(table);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        public ActionResult PrintPDFNonTrack(string CategoryId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var mem = new MemoryStream();
                var eps = _epService.GetStandardEPs().Where(x => x.IsApplicable == false).ToList();
                if (!string.IsNullOrEmpty(CategoryId) && Convert.ToInt32(CategoryId) != 0)
                    eps = eps.Where(x => x.CategoryId == Convert.ToInt32(CategoryId)).ToList();

                if (eps != null && eps.Count > 0)
                {
                    pdfDoc = CreateNonTrackEp(ms, eps);
                    string fileName = System.Web.HttpUtility.UrlEncode("NonTrackEp_" + DateTime.Now.ToShortDateString() + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                }
            }
            return View();
        }


        #endregion

        #region Assets Inspection Reports
        public ActionResult CreateAssetsInspectionReport(string selAssetId, string selEPDetailId, string fromDate = "", string toDate = "")
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                PdfPCell cell = new PdfPCell();
                SetHeader(out table);
                pdfDoc.Add(table);

                if (!string.IsNullOrEmpty(selAssetId))
                {
                    string[] assetId = selAssetId.Split(',');
                    var activityStartingdate = fromDate;
                    var activityEndingdate = toDate;

                    List<TFloorAssets> AllAssetsTfloorAssets = new List<TFloorAssets>();
                    if (TempData["AssetsReportdata"] != null)
                        AllAssetsTfloorAssets = TempData.Get<List<TFloorAssets>>("AssetsReportdata");  //(List<TFloorAssets>)TempData["AssetsReportdata"];

                    for (int i = 0; i < assetId.Length; i++)
                    {
                        var assets = new Assets();
                        if (!string.IsNullOrEmpty(assetId[i]) && Convert.ToInt32(assetId[i]) > 0)
                            assets = _assetService.GetAllAsset().FirstOrDefault(x => x.AssetId == Convert.ToInt32(assetId[i]));

                        var assetEps = _assetService.GetAssetEp(Convert.ToInt32(assetId[i])).Where(x => x.StandardEp != null).ToList();
                        List<TFloorAssets> tfloorAssets = new List<TFloorAssets>();
                        tfloorAssets = AllAssetsTfloorAssets.Where(x => x.AssetId == assets.AssetId).ToList();

                        if (tfloorAssets.Count > 0)
                        {
                            for (int j = 0; j < assetEps.Count; j++)
                            {

                                var ep = new EPDetails();
                                Phrase phrase = new Phrase();
                                int selEpdetailId = 0;
                                if (!string.IsNullOrEmpty(assetEps[j].EPDetailId.ToString()) && Convert.ToInt32(assetEps[j].EPDetailId) > 0)
                                    selEpdetailId = Convert.ToInt32(assetEps[j].EPDetailId);

                                ep = _epService.GetEpDescription(selEpdetailId);

                                Paragraph para = new Paragraph("Inspection Summary", TitleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER
                                };
                                pdfDoc.Add(para);

                                table = new PdfPTable(6)
                                {
                                    WidthPercentage = 100,
                                    HorizontalAlignment = 0,
                                    SpacingBefore = 20f,
                                    SpacingAfter = 30f
                                };

                                table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;

                                table.AddCell(new Phrase("Asset Type", CellFontBoldBlue));
                                table.AddCell(new Phrase("Inventory Total", CellFontBoldBlue));
                                table.AddCell(new Phrase("Total Tested", CellFontBoldBlue));
                                table.AddCell(new Phrase("Pass", CellFontBoldBlue));
                                table.AddCell(new Phrase("Fail", CellFontBoldBlue));
                                table.AddCell(new Phrase("% Passed", CellFontBoldBlue));

                                table.DefaultCell.BackgroundColor = BaseColor.WHITE;

                                var totalAssets = tfloorAssets.Count;
                                var totaltested = 0;
                                var totalPass = 0;
                                var totalFailed = 0;
                                var totalpassedPercent = 0;
                                if (totalAssets > 0)
                                {
                                    var latestTfloorAssetActivity = tfloorAssets.SelectMany(x => x.TInspectionActivity).OrderByDescending(x => x.ActivityInspectionDate).GroupBy(test => test.FloorAssetId)
                                           .Select(grp => grp.First());

                                    totaltested = latestTfloorAssetActivity.Count(y => y.IsCurrent);
                                    totalPass = latestTfloorAssetActivity.Count(y => y.Status == 1 && y.IsCurrent);
                                    totalFailed = latestTfloorAssetActivity.Count(y => y.Status == 0 && y.IsCurrent);
                                    totalpassedPercent = (totalPass * 100 / totalAssets);
                                }

                                table.AddCell(new Phrase(assets.Name, CellFont));
                                table.AddCell(new Phrase(tfloorAssets.Count.ToString(), CellFont));
                                table.AddCell(new Phrase(totaltested.ToString(), CellFont));
                                table.AddCell(new Phrase(totalPass.ToString(), CellFont));
                                table.AddCell(new Phrase(totalFailed.ToString(), CellFont));
                                table.AddCell(new Phrase($"{totalpassedPercent.ToString()} %", CellFont));

                                pdfDoc.Add(table);

                                table = new PdfPTable(2)
                                {
                                    WidthPercentage = 100,
                                    HorizontalAlignment = 0,
                                    SpacingBefore = 20f,
                                    SpacingAfter = 10f
                                };
                                table.DefaultCell.Border = Rectangle.NO_BORDER;

                                phrase = new Phrase();
                                phrase.Add(new Chunk("Activity Dates: ", CellFontBold));
                                phrase.Add(new Chunk($"{activityStartingdate} - {activityEndingdate}", CellFont));
                                cell = new PdfPCell(new Phrase(phrase))
                                {
                                    Colspan = 2,
                                    MinimumHeight = 30f,
                                    VerticalAlignment = Element.ALIGN_MIDDLE,
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Border = Rectangle.NO_BORDER
                                };
                                table.AddCell(cell);

                                if (selEpdetailId > 0)
                                {
                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("Standard: ", CellFontBold));
                                    phrase.Add(new Chunk($"{ep.Standard.TJCStandard} - {ep.Standard.DisplayDescription}", CellFont));
                                    cell = new PdfPCell(new Phrase(phrase))
                                    {
                                        Colspan = 2,
                                        MinimumHeight = 30f,
                                        VerticalAlignment = Element.ALIGN_MIDDLE,
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Border = Rectangle.NO_BORDER
                                    };
                                    table.AddCell(cell);

                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("EP: ", CellFontBold));
                                    phrase.Add(new Chunk($"{ep.EPNumber} - {ep.Description}", CellFont));
                                    cell = new PdfPCell(new Phrase(phrase))
                                    {
                                        Colspan = 2,
                                        MinimumHeight = 30f,
                                        VerticalAlignment = Element.ALIGN_MIDDLE,
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Border = Rectangle.NO_BORDER
                                    };
                                    table.AddCell(cell);

                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("CoP: ", CellFontBold));
                                    phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.RequirementName)), CellFont));
                                    table.AddCell(new Phrase(phrase));

                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("CMS Tags: ", CellFontBold));
                                    phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.CopStdesc.TagCode)), CellFont));
                                    table.AddCell(new Phrase(phrase));

                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("Name of Activity: ", CellFontBold));
                                    phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName} Inspection of {assets.Name}", CellFont));
                                    cell = new PdfPCell(new Phrase(phrase))
                                    {
                                        Colspan = 2,
                                        MinimumHeight = 30f,
                                        VerticalAlignment = Element.ALIGN_MIDDLE,
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Border = Rectangle.NO_BORDER
                                    };
                                    table.AddCell(cell);

                                    phrase = new Phrase();
                                    phrase.Add(new Chunk("Frequency of Activity: ", CellFontBold));
                                    phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName}", CellFont));
                                    cell = new PdfPCell(new Phrase(phrase))
                                    {
                                        Colspan = 2,
                                        MinimumHeight = 30f,
                                        VerticalAlignment = Element.ALIGN_MIDDLE,
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Border = Rectangle.NO_BORDER
                                    };
                                    table.AddCell(cell);

                                    pdfDoc.Add(table);

                                    var floorassetsEPs = tfloorAssets.Select(x => x.EPDetails.Where(y => y.EPDetailId == selEpdetailId)).FirstOrDefault();

                                    var inspectorInformtaion = floorassetsEPs.SelectMany(x => x.TInspectionActivity).OrderBy(x => x.ActivityInspectionDate).Select(y => y.UserProfile).GroupBy(test => test.UserId)
                                                .Select(grp => grp.First()).ToList();
                                    var isanyVendorUser = inspectorInformtaion.Any(x => x.IsVendorUser);


                                    para = new Paragraph("Inspector Contact Information", TitleFont)
                                    {
                                        Alignment = Element.ALIGN_CENTER
                                    };
                                    pdfDoc.Add(para);

                                    table = new PdfPTable(3)
                                    {
                                        WidthPercentage = 100,
                                        HorizontalAlignment = 0,
                                        SpacingBefore = 20f,
                                        SpacingAfter = 30f
                                    };
                                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                                    // Need looping here                         

                                    foreach (var item in inspectorInformtaion)
                                    {

                                        phrase = new Phrase();
                                        phrase.Add(new Chunk("Name: ", CellFontBold));
                                        phrase.Add(new Chunk(item.Name, CellFont));
                                        table.AddCell(new Phrase(phrase));

                                        phrase = new Phrase();
                                        phrase.Add(new Chunk("Phone: ", CellFontBold));
                                        phrase.Add(new Chunk(item.PhoneNumber, CellFont));
                                        table.AddCell(new Phrase(phrase));

                                        phrase = new Phrase();
                                        phrase.Add(new Chunk("Email: ", CellFontBold));
                                        phrase.Add(new Chunk(item.Email, CellFont));
                                        table.AddCell(new Phrase(phrase));

                                        phrase = new Phrase();
                                        phrase.Add(new Chunk("Company: ", CellFontBold));
                                        phrase.Add(new Chunk(item.IsVendorUser ? "" : UserSession.CurrentOrg.Name, CellFont));
                                        table.AddCell(new Phrase(phrase));

                                        phrase = new Phrase();
                                        phrase.Add(new Chunk("Address: ", CellFontBold));
                                        phrase.Add(new Chunk(item.IsVendorUser ? "" : UserSession.CurrentOrg.Address, CellFont));
                                        cell = new PdfPCell(phrase)
                                        {
                                            Colspan = 2,
                                            MinimumHeight = 30f,
                                            Border = Rectangle.NO_BORDER
                                        };
                                        table.AddCell(cell);
                                        // Need looping here                            
                                    }
                                    pdfDoc.Add(table);

                                    pdfDoc.NewPage();

                                    para = new Paragraph("Inspection Details", TitleFont)
                                    {
                                        Alignment = Element.ALIGN_CENTER
                                    };
                                    pdfDoc.Add(para);
                                    table = new PdfPTable(7)
                                    {
                                        WidthPercentage = 100,
                                        HorizontalAlignment = 0,
                                        SpacingBefore = 20f,
                                        SpacingAfter = 30f
                                    };
                                    table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    float[] widths = new float[] { 5f, 25f, 25f, 20f, 8f, 7f, 10f };
                                    table.SetTotalWidth(widths);

                                    table.AddCell(new Phrase("#", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Asset #", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Building, Floor", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Location", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Inspector", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Date", CellFontBoldBlue));
                                    table.AddCell(new Phrase("Result", CellFontBoldBlue));
                                    int indexcount = 0;
                                    table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                                    foreach (var tfloorAsset in tfloorAssets.Select((value, index) => new { value, index }))
                                    {
                                        string buildingfloor = string.Format("{0}{1}", !string.IsNullOrEmpty(tfloorAsset.value.Floor.FloorName) ? $", {tfloorAsset.value.Floor.FloorName}" : "", tfloorAsset.value.Floor.Building.BuildingName);
                                        table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                                        table.AddCell(new Phrase(tfloorAsset.value.AssetNo, CellFont));
                                        table.AddCell(new Phrase(!string.IsNullOrEmpty(buildingfloor) ? buildingfloor : "NA", CellFont));
                                        table.AddCell(new Phrase(tfloorAsset.value.NearBy, CellFont));
                                        var userlist = floorassetsEPs.SelectMany(x => x.TInspectionActivity).Select(x => x.UserProfile).ToList();
                                        var name = userlist.Where(x => x.UserId == tfloorAsset.value.CreatedBy).ToList();
                                        var Name = "";
                                        if (name.Count > 0) { Name = name[0].Name.ToString(); }
                                        table.AddCell(new Phrase(Name, CellFont));
                                        var Tinspectiondata = floorassetsEPs.Select(x => x.TInspectionActivity).ToList();
                                        var createddate = new List<DateTime?>();
                                        if (Tinspectiondata.Any())
                                        {
                                            createddate = Tinspectiondata[0].Select(x => x.CreatedDate).ToList();
                                        }

                                        var date = "";
                                        if (createddate.Count > 0 && indexcount < createddate.Count)
                                        {
                                            date = Convert.ToDateTime(createddate[indexcount]).ToShortDateString();
                                        }

                                        table.AddCell(new Phrase(date, CellFont));
                                        var statusdata = new List<int>();
                                        statusdata = Tinspectiondata[0].Select(x => x.Status).ToList();
                                        int status = 0;
                                        if (statusdata.Count > 0 && indexcount < statusdata.Count)
                                        {
                                            status = statusdata[indexcount];
                                        }
                                        var statusString = "";
                                        if (status == 1)
                                        {
                                            statusString = "Compliance";
                                        }
                                        else if (status == 0)
                                        {
                                            statusString = "Non-compliance";
                                        }
                                        else { statusString = ""; }
                                        table.AddCell(new Phrase(statusString, CellFont));
                                        indexcount++;
                                    }
                                    pdfDoc.Add(table);
                                    table = new PdfPTable(2)
                                    {
                                        WidthPercentage = 75,
                                        HorizontalAlignment = Element.ALIGN_CENTER,
                                        SpacingBefore = 20f,
                                        SpacingAfter = 10f
                                    };

                                    table.AddCell(new Phrase("Inspector", CellFont));
                                    table.AddCell(new Phrase("Digitally Signed On", CellFont));

                                    foreach (var user in inspectorInformtaion.Where(x => !x.IsVendorUser))
                                    {
                                        var latestinspectiondate = floorassetsEPs.SelectMany(x => x.TInspectionActivity).OrderBy(x => x.ActivityInspectionDate).Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate).ToFormatDate();
                                        table.AddCell(new Phrase(user.FullName, CellFont));
                                        table.AddCell(new Phrase(latestinspectiondate, CellFont));
                                    }
                                    pdfDoc.Add(table);

                                    if (isanyVendorUser)
                                    {
                                        table = new PdfPTable(2)
                                        {
                                            WidthPercentage = 75,
                                            HorizontalAlignment = Element.ALIGN_CENTER,
                                            SpacingBefore = 20f,
                                            SpacingAfter = 10f
                                        };

                                        table.AddCell(new Phrase("Owner/Representative", CellFont));
                                        table.AddCell(new Phrase("Digitally Signed On", CellFont));

                                        foreach (var user in inspectorInformtaion.Where(x => x.IsVendorUser))
                                        {
                                            var latestinspectiondate = floorassetsEPs.SelectMany(x => x.TInspectionActivity).OrderBy(x => x.ActivityInspectionDate).Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate).ToFormatDate();
                                            table.AddCell(new Phrase(user.FullName, CellFont));
                                            table.AddCell(new Phrase(latestinspectiondate, CellFont));
                                        }
                                        pdfDoc.Add(table);
                                    }
                                    pdfDoc.NewPage();
                                }
                            }
                            pdfDoc.Add(table);
                            pdfDoc.NewPage();
                        }
                    }
                }
                TempData.Keep();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "AssetsInspectionReport_" + DateTime.UtcNow.ToShortDateString() + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }


        #endregion

        #region RBI Assets Inspection Reports

        [HttpPost]
        [ActionName("RBIReports")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRouteBasedAssetsInspectionReport(int? selAssetId, int? selEpdetailId, string selReportType, int? selYear, int? selMonth)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var Year = selYear.HasValue ? selYear.Value : DateTime.Now.Year;
                var Month = selMonth.HasValue ? selMonth.Value : DateTime.Now.Month;

                var tfloorAssets = new List<TFloorAssets>();
                var ep = new EPDetails();
                var assets = new Assets();

                if (selEpdetailId.HasValue && selEpdetailId.Value > 0)
                    ep = _epService.GetEpDescription(Convert.ToInt32(selEpdetailId));

                if (selAssetId.HasValue && selAssetId.Value > 0)
                    assets = _assetService.GetAllAsset().FirstOrDefault(x => x.AssetId == selAssetId.Value);

                //if (TempData["RBIReportsdata"] != null)
                //    TempData.Get<List<TFloorAssets>>("RBIReportsdata");
                //else
                tfloorAssets = _fireExtinguisherService.Get_ExtinguisherAssetsReport(Year, 0, selAssetId, selEpdetailId);
                var rec = tfloorAssets.Where(x => x.AssetSubCategory.AscName != null).GroupBy(y => y.AssetSubCategory.AscName, (key, g) => new { AscId = key, Count = g.Count(), Name = g.FirstOrDefault().AssetSubCategory.AscName });

                Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 25);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                PdfPCell cell = new PdfPCell();
                Phrase phrase = new Phrase();
                SetHeader(out table);
                pdfDoc.Add(table);

                Paragraph para = new Paragraph("Inspection Summary", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(7)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;

                table.AddCell(new Phrase("Asset Type", CellFontBoldBlue));
                table.AddCell(new Phrase("Inventory Total", CellFontBoldBlue));
                table.AddCell(new Phrase("Total Tested", CellFontBoldBlue));
                table.AddCell(new Phrase("Pass", CellFontBoldBlue));
                table.AddCell(new Phrase("Fail", CellFontBoldBlue));
                table.AddCell(new Phrase("% Inspected", CellFontBoldBlue));
                table.AddCell(new Phrase("% Passed", CellFontBoldBlue));

                table.DefaultCell.BackgroundColor = BaseColor.WHITE;

                var totalAssets = tfloorAssets.Count;
                var totaltested = 0;
                var totalPass = 0;
                var totalFailed = 0;
                var totalinspectedPercent = 0;
                var totalpassedPercent = 0;
                if (totalAssets > 0)
                {
                    var latestTfloorAssetActivity = new List<TInspectionActivity>();
                    if (selReportType.ToLower() != "sm")
                    {
                        latestTfloorAssetActivity = tfloorAssets.SelectMany(x => x.TInspectionActivity).OrderByDescending(x => x.ActivityInspectionDate).GroupBy(test => test.FloorAssetId)
                                       .Select(grp => grp.First()).ToList();
                    }
                    else
                    {
                        latestTfloorAssetActivity = tfloorAssets.SelectMany(x => x.TInspectionActivity).Where(x => x.ActivityInspectionDate.Value.Month == Month).OrderByDescending(x => x.ActivityInspectionDate).GroupBy(test => test.FloorAssetId)
                                       .Select(grp => grp.First()).ToList();
                    }

                    totaltested = latestTfloorAssetActivity.Count();
                    totalPass = latestTfloorAssetActivity.Count(y => y.Status == 1);
                    totalFailed = latestTfloorAssetActivity.Count(y => y.Status == 0);
                    totalinspectedPercent = (totaltested * 100 / totalAssets);
                    totalpassedPercent = (totalPass * 100 / totalAssets);
                }
                var activityStartingdate = tfloorAssets.SelectMany(x => x.TInspectionActivity).Min(x => x.ActivityInspectionDate).ToFormatDate();
                var activityEndingdate = tfloorAssets.SelectMany(x => x.TInspectionActivity).Max(x => x.ActivityInspectionDate).ToFormatDate();

                table.AddCell(new Phrase(assets.Name, CellFont));
                table.AddCell(new Phrase(tfloorAssets.Count.ToString(), CellFont));
                table.AddCell(new Phrase(totaltested.ToString(), CellFont));
                table.AddCell(new Phrase(totalPass.ToString(), CellFont));
                table.AddCell(new Phrase(totalFailed.ToString(), CellFont));
                table.AddCell(new Phrase($"{totalinspectedPercent.ToString()} %", CellFont));
                table.AddCell(new Phrase($"{totalpassedPercent.ToString()} %", CellFont));

                pdfDoc.Add(table);

                var reccount = rec.Count(x => x.Name != "");
                if (reccount > 0)
                {
                    para = new Paragraph("Fire Extinguisher Count by Type", TitleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    pdfDoc.Add(para);

                    table = new PdfPTable(reccount)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,
                        SpacingAfter = 10f
                    };
                    table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    foreach (var item in rec.Where(x => x.Name != "").OrderBy(x => x.Name))
                    {
                        table.AddCell(new Phrase(item.Name, CellFontBoldBlue));
                    }
                    table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    foreach (var item in rec.Where(x => x.Name != "").OrderBy(x => x.Name))
                    {
                        table.AddCell(new Phrase(item.Count.ToString(), CellFont));
                    }
                    pdfDoc.Add(table);
                }

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };
                table.DefaultCell.Border = Rectangle.NO_BORDER;

                phrase = new Phrase();
                phrase.Add(new Chunk("Activity Dates: ", CellFontBold));
                phrase.Add(new Chunk($"{activityStartingdate} - {activityEndingdate}", CellFont));
                cell = new PdfPCell(new Phrase(phrase))
                {
                    Colspan = 2,
                    MinimumHeight = 30f,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Border = Rectangle.NO_BORDER
                };
                table.AddCell(cell);

                if (selEpdetailId.HasValue && selEpdetailId.Value > 0)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Standard: ", CellFontBold));
                    phrase.Add(new Chunk($"{ep.Standard.TJCStandard} - {ep.Standard.DisplayDescription}", CellFont));
                    cell = new PdfPCell(new Phrase(phrase))
                    {
                        Colspan = 2,
                        MinimumHeight = 30f,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    };
                    table.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("EP: ", CellFontBold));
                    phrase.Add(new Chunk($"{ep.EPNumber} - {ep.Description}", CellFont));
                    cell = new PdfPCell(new Phrase(phrase))
                    {
                        Colspan = 2,
                        MinimumHeight = 30f,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    };
                    table.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("CoP: ", CellFontBold));
                    phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.RequirementName)), CellFont));
                    table.AddCell(new Phrase(phrase));

                    phrase = new Phrase();
                    phrase.Add(new Chunk("CMS Tags: ", CellFontBold));
                    phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.CopStdesc.TagCode)), CellFont));
                    table.AddCell(new Phrase(phrase));

                    phrase = new Phrase();
                    phrase.Add(new Chunk("Name of Activity: ", CellFontBold));
                    phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName} Inspection of {assets.Name}", CellFont));
                    cell = new PdfPCell(new Phrase(phrase))
                    {
                        Colspan = 2,
                        MinimumHeight = 30f,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    };
                    table.AddCell(cell);

                    phrase = new Phrase();
                    phrase.Add(new Chunk("Frequency of Activity: ", CellFontBold));
                    phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName}", CellFont));
                    cell = new PdfPCell(new Phrase(phrase))
                    {
                        Colspan = 2,
                        MinimumHeight = 30f,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    };
                    table.AddCell(cell);
                }

                pdfDoc.Add(table);

                var inspectorInformtaion = new List<UserProfile>();
                if (selReportType.ToLower() != "sm")
                {
                    inspectorInformtaion = tfloorAssets.SelectMany(x => x.TInspectionActivity).OrderByDescending(x => x.ActivityInspectionDate).Select(y => y.UserProfile).GroupBy(test => test.UserId)
                      .Select(grp => grp.First()).ToList();
                }
                else
                {
                    inspectorInformtaion = tfloorAssets.SelectMany(x => x.TInspectionActivity).Where(x => x.ActivityInspectionDate.Value.Month == Month).OrderByDescending(x => x.ActivityInspectionDate).Select(y => y.UserProfile).GroupBy(test => test.UserId)
                     .Select(grp => grp.First()).ToList();
                }
                var isanyVendorUser = inspectorInformtaion.Any(x => x.IsVendorUser);

                para = new Paragraph("Inspector Contact Information", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                table = new PdfPTable(6)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(new Phrase("Initials", CellFontBoldBlue));
                table.AddCell(new Phrase("Inspector Name", CellFontBoldBlue));
                table.AddCell(new Phrase("Company", CellFontBoldBlue));
                table.AddCell(new Phrase("Address", CellFontBoldBlue));
                table.AddCell(new Phrase("Phone Number", CellFontBoldBlue));
                table.AddCell(new Phrase("Email", CellFontBoldBlue));
                table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                foreach (var item in inspectorInformtaion)
                {
                    table.AddCell(new Phrase(string.Format("{0}{1}", item.FirstName[0], string.IsNullOrEmpty(item.LastName) ? (char?)null : item.LastName[0], CellFont)));
                    table.AddCell(new Phrase(item.Name, CellFont));
                    table.AddCell(new Phrase(item.IsVendorUser ? "" : UserSession.CurrentOrg.Name, CellFont));
                    table.AddCell(new Phrase(item.IsVendorUser ? "" : UserSession.CurrentOrg.Address, CellFont));
                    table.AddCell(new Phrase(item.PhoneNumber, CellFont));
                    table.AddCell(new Phrase(item.Email, CellFont));
                }

                pdfDoc.Add(table);

                pdfDoc.NewPage();

                if (selReportType.ToLower() != "sm" && selReportType.ToLower() != "sa")  // Not added images for single month or annual 
                {

                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f
                    };
                    cell = new PdfPCell
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Border = Rectangle.NO_BORDER
                    };

                    System.Net.ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Image image = Image.GetInstance(_commonProvider.GetWebUrlPath(Models.ImagePathModel.RBIInspectionInfoIcon));
                    image.ScaleAbsolute(800, 600);
                    cell.AddElement(image);

                    table.AddCell(cell);
                    pdfDoc.Add(table);
                }

                para = new Paragraph($"{assets.Name} Inspection Details", TitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);

                if (selReportType.ToLower() == "sm" || selReportType.ToLower() == "sa")
                {
                    table = CreateRBIReportsForSingle(selReportType, Year, Month, tfloorAssets, pdfDoc);
                }
                else
                {
                    table = CreateRBIReports(selReportType, Year, Month, tfloorAssets, pdfDoc);
                }

                table = new PdfPTable(3)
                {
                    WidthPercentage = 75,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };
                table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(new Phrase("#", CellFontBoldBlue));
                table.AddCell(new Phrase("Tag/BarCode", CellFontBoldBlue));
                table.AddCell(new Phrase("Comment", CellFontBoldBlue));
                table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                foreach (var tfloorAsset in tfloorAssets.Select((value, index) => new { value, index }))
                {
                    var activity = tfloorAsset.value.TInspectionActivity.OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                    if (activity != null && (!string.IsNullOrEmpty(activity.Comment)))
                    {
                        table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                        table.AddCell(new Phrase(tfloorAsset.value.SerialNo, CellFont));
                        table.AddCell(new Phrase(activity.Comment, CellFont));
                    }
                }

                pdfDoc.Add(table);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 75,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f,
                    SpacingAfter = 10f
                };

                table.AddCell(new Phrase("Inspector", CellFont));
                table.AddCell(new Phrase("Digitally Signed On", CellFont));

                foreach (var user in inspectorInformtaion.Where(x => !x.IsVendorUser))
                {
                    var latestinspectiondate = string.Empty;
                    var latestinspection = tfloorAssets.SelectMany(x => x.TInspectionActivity).OrderByDescending(x => x.ActivityInspectionDate).Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
                    if (latestinspection != null)
                        latestinspectiondate = latestinspection.Value.ToString("ddd, MMM d, yyyy");
                    table.AddCell(new Phrase(user.FullName, CellFont));
                    table.AddCell(new Phrase(latestinspectiondate, CellFont));
                }

                pdfDoc.Add(table);

                if (isanyVendorUser)
                {
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 75,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        SpacingBefore = 20f,
                        SpacingAfter = 10f
                    };

                    table.AddCell(new Phrase("Owner/Representative ", CellFont));
                    table.AddCell(new Phrase("Digitally Signed On", CellFont));

                    foreach (var user in inspectorInformtaion.Where(x => x.IsVendorUser))
                    {
                        var latestinspectiondate = string.Empty;
                        var latestinspection = tfloorAssets.SelectMany(x => x.TInspectionActivity).OrderByDescending(x => x.ActivityInspectionDate).Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
                        if (latestinspection != null)
                            latestinspectiondate = latestinspection.Value.ToString("ddd, MMM d, yyyy");
                        table.AddCell(new Phrase(user.FullName, CellFont));
                        table.AddCell(new Phrase(latestinspectiondate, CellFont));
                    }
                    pdfDoc.Add(table);
                }

                TempData.Keep();
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = $"RouteBasedAssets_{assets.Name}" + DateTime.UtcNow.ToShortDateString() + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        private PdfPTable CreateRBIReportsForSingle(string selReportType, int year, int month, List<TFloorAssets> tfloorAssets, Document pdfDoc)
        {
            PdfPTable table = new PdfPTable(8)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };
            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            float[] widths = new float[] { 10f, 10f, 10f, 10f, 10f, 10f, 20f, 20f };
            table.SetTotalWidth(widths);

            table.AddCell(new Phrase("#", CellFontBoldBlue));
            table.AddCell(new Phrase("Tag/BarCode", CellFontBoldBlue));
            table.AddCell(new Phrase("Building,Floor", CellFontBoldBlue));
            table.AddCell(new Phrase("Location", CellFontBoldBlue));
            table.AddCell(new Phrase("Type", CellFontBoldBlue));
            table.AddCell(new Phrase("Inspector", CellFontBoldBlue));
            table.AddCell(new Phrase("Date", CellFontBoldBlue));
            table.AddCell(new Phrase("Result", CellFontBoldBlue));

            table.DefaultCell.BackgroundColor = BaseColor.WHITE;

            foreach (var tfloorAsset in tfloorAssets.Select((value, index) => new { value, index }))
            {
                table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.SerialNo, CellFont));
                table.AddCell(new Phrase("", CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.Stop.StopName, CellFont));
                if (tfloorAsset.value.AssetSubCategory.AscId > 0)
                    table.AddCell(new Phrase(tfloorAsset.value.AssetSubCategory.CustomAscName(tfloorAsset.value.FireExtinguisherType.FeType), CellFont));
                else
                    table.AddCell(new Phrase(string.Empty, CellFont));
                if (tfloorAsset.value.TInspectionActivity != null && tfloorAsset.value.TInspectionActivity.Count > 0)
                {
                    var activity = new TInspectionActivity();
                    var monthno = !(string.IsNullOrEmpty(month.ToString())) ? Convert.ToInt32(month) : 0;
                    var selyear = year;
                    if (selReportType == "SM")
                    {
                        activity = tfloorAsset.value.TInspectionActivity.Where(x => x.ActivityInspectionDate.Value.Month == monthno).OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                    }
                    else
                    {
                        activity = tfloorAsset.value.TInspectionActivity.Where(x => x.ActivityInspectionDate.Value.Year == year).OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                    }
                    if (activity != null)
                    {
                        table.AddCell(new Phrase(activity.UserProfile.Name, CellFont));
                        table.AddCell(new Phrase(activity.ActivityInspectionDate.Value.Date.ToFormatDate(), CellFont));
                        table.AddCell(new Phrase(Enum.GetName(typeof(BDO.Enums.InspectionStatus), activity.Status), CellFont));
                    }
                    else
                    {
                        table.AddCell(new Phrase(string.Empty, CellFont));
                        table.AddCell(new Phrase(string.Empty, CellFont));
                        table.AddCell(new Phrase(string.Empty, CellFont));
                    }
                }
                else
                {
                    table.AddCell(new Phrase(string.Empty, CellFont));
                    table.AddCell(new Phrase(string.Empty, CellFont));
                    table.AddCell(new Phrase(string.Empty, CellFont));
                }
            }
            pdfDoc.Add(table);
            return table;
        }

        private PdfPTable CreateRBIReports(string selReportType, int Year, int Month, List<TFloorAssets> tfloorAssets, Document pdfDoc)
        {
            PdfPTable table = new PdfPTable(7)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };
            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            float[] widths = new float[] { 5f, 10f, 10f, 5f, 5f, 5f, 60f };
            table.SetTotalWidth(widths);

            table.AddCell(new Phrase("#", CellFontBoldBlue));
            table.AddCell(new Phrase("Tag/BarCode", CellFontBoldBlue));
            table.AddCell(new Phrase("Location", CellFontBoldBlue));
            table.AddCell(new Phrase("Stop Code", CellFontBoldBlue));
            table.AddCell(new Phrase("Route", CellFontBoldBlue));
            table.AddCell(new Phrase("Type", CellFontBoldBlue));
            table.AddCell(routeReportHeader(selReportType, Year, Month));

            table.DefaultCell.BackgroundColor = BaseColor.WHITE;

            foreach (var tfloorAsset in tfloorAssets.Select((value, index) => new { value, index }))
            {
                table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.SerialNo, CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.Stop.StopName, CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.Stop.StopCode, CellFont));
                if (tfloorAsset.value.Routes.Count > 0)
                    table.AddCell(new Phrase(tfloorAsset.value.Routes.FirstOrDefault().RouteNo, CellFont));
                else
                    table.AddCell(new Phrase(string.Empty, CellFont));
                if (tfloorAsset.value.AssetSubCategory.AscId > 0)
                    table.AddCell(new Phrase(tfloorAsset.value.AssetSubCategory.CustomAscName(tfloorAsset.value.FireExtinguisherType.FeType), CellFont));
                else
                    table.AddCell(new Phrase(string.Empty, CellFont));
                table.AddCell(routeReportData(tfloorAsset.value.TInspectionActivity, selReportType, Year, Month));
            }
            pdfDoc.Add(table);
            return table;
        }

        public ActionResult GenerateInspectionReport(int epdetailId, int inspectionId)
        {
            EPDetails epdetails = new EPDetails();
            int userId = UserSession.CurrentUser.UserId;
            var resstr = _pdfService.PrintAssetsReportsInbytes(userId, epdetailId, ref epdetails);
            byte[] fileContent = Convert.FromBase64String(resstr);
            string fileName = $"AssetsReport_{inspectionId }_{DateTime.Now.ToShortDateString()}" + ".pdf";

            Response.Clear();
            MemoryStream ms = new MemoryStream(fileContent);
            byte[] data = ms.ToArray();
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=" + fileName);
            Response.Body.WriteAsync(data, 0, data.Length);

            return View();


        }

        public PdfPTable routeReportHeader(string reportType, int year, int month)
        {
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            if (reportType == "M")
            {
                table = new PdfPTable(12)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = Rectangle.RIGHT_BORDER
                };
                table.AddCell(new Phrase("Jan", CellFontBold));
                table.AddCell(new Phrase("Feb", CellFontBold));
                table.AddCell(new Phrase("Mar", CellFontBold));
                table.AddCell(new Phrase("Apr", CellFontBold));
                table.AddCell(new Phrase("May", CellFontBold));
                table.AddCell(new Phrase("Jun", CellFontBold));
                table.AddCell(new Phrase("Jul", CellFontBold));
                table.AddCell(new Phrase("Aug", CellFontBold));
                table.AddCell(new Phrase("Sep", CellFontBold));
                table.AddCell(new Phrase("Oct", CellFontBold));
                table.AddCell(new Phrase("Nov", CellFontBold));
                table.AddCell(new Phrase("Dec", CellFontBold));
            }
            else if (reportType == "Q")
            {
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                table.AddCell(new Phrase("Q1(J-M)", CellFontBold));
                table.AddCell(new Phrase("Q2(A-J)", CellFontBold));
                table.AddCell(new Phrase("Q3(J-S)", CellFontBold));
                table.AddCell(new Phrase("Q4(O-D)", CellFontBold));
            }
            else
            {
                var weeks = _commonModelFactory.GetWeeks(year, month);
                table = new PdfPTable(weeks.Count)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                foreach (var item in weeks.OrderBy(x => x.weekNum))
                {
                    table.AddCell(new Phrase($"{item.weekStart.ToString("MMM d")} - {item.weekFinish.ToString("MMM d")}", CellFontBold));
                }
            }
            return table;
        }

        private PdfPTable routeReportData(List<TInspectionActivity> inspectionActivity, string selReportType, int year, int month)
        {
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();

            var weeks = _commonModelFactory.GetWeeks(year, month);
            var count = selReportType == "M" ? 12 : selReportType == "Q" ? 4 : weeks.Count;
            var quarterCount = 1;
            table = new PdfPTable(count)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            table.DefaultCell.Border = Rectangle.RIGHT_BORDER;
            for (int i = 1; i <= count; i++)
            {
                var activity = new TInspectionActivity();
                string inspDateInfo = string.Empty;

                if (selReportType == "M")
                {
                    activity = inspectionActivity.Where(x => x.ActivityInspectionDate.Value.Month == i).OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                    if (activity != null)
                    {
                        inspDateInfo = string.Format("{0}{1} ({2})- {3}", activity.UserProfile.FirstName[0], string.IsNullOrEmpty(activity.UserProfile.LastName) ? (char?)null : activity.UserProfile.LastName[0], activity.ActivityInspectionDate.Value.Date.Day, Enum.GetName(typeof(BDO.Enums.Status), activity.Status).ToUpper()[0]);
                    }
                }
                else if (selReportType == "Q")
                {
                    var startdate = new DateTime(year, quarterCount, 1);
                    var endDate = startdate.AddMonths(3).AddMinutes(-1);
                    quarterCount += 3;
                    activity = inspectionActivity.Where(x => x.ActivityInspectionDate.Value.Date >= startdate && x.ActivityInspectionDate.Value.Date <= endDate).OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                    if (activity != null)
                    {
                        inspDateInfo = string.Format("{0}{1} ({2})- {3}", activity.UserProfile.FirstName[0], string.IsNullOrEmpty(activity.UserProfile.LastName) ? (char?)null : activity.UserProfile.LastName[0], activity.ActivityInspectionDate.Value.ToString("MMM d"), Enum.GetName(typeof(BDO.Enums.Status), activity.Status).ToUpper()[0]);
                    }
                }
                else
                {
                    var week = weeks.FirstOrDefault(x => x.weekNum == i);
                    if (week != null)
                    {
                        var startdate = week.weekStart;
                        var endDate = week.weekFinish;
                        activity = inspectionActivity.Where(x => x.ActivityInspectionDate.Value.Date >= startdate && x.ActivityInspectionDate.Value.Date <= endDate).OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                        if (activity != null)
                        {
                            inspDateInfo = string.Format("{0}{1} ({2})- {3}", activity.UserProfile.FirstName[0], string.IsNullOrEmpty(activity.UserProfile.LastName) ? (char?)null : activity.UserProfile.LastName[0], activity.ActivityInspectionDate.Value.Day, Enum.GetName(typeof(BDO.Enums.Status), activity.Status).ToUpper()[0]);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(inspDateInfo))
                    table.AddCell(new Phrase(inspDateInfo, CellFont));
                else
                    table.AddCell(new Phrase("", CellFont));
            }
            return table;
        }



        #endregion

        #region Tracking Assets
        public ActionResult CreateTrackingAssetsReport(string assetIds, string campusid, string buildingId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 35);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.Red);
                SetHeader(out table);
                pdfDoc.Add(table);
                Paragraph para = new Paragraph("Tracking Asset Report", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);
                table.DefaultCell.Border = 0;
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };

                cell = new PdfPCell(new Phrase("Campus Name"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Building Name"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Asset Type"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Asset #"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);

                var result = _assetService.GetTrackingAssetCreation(assetIds, campusid, buildingId).ToList();
                var assetData = (List<Assets>)result[3];
                var siteData = (List<Site>)result[0];
                var buildingData = (List<Buildings>)result[1];
                var floorData = (List<Floor>)result[2];
                var floorassetData = (List<TFloorAssets>)result[4];

                string siteName = "";
                if (assetData != null)
                {
                    foreach (var sitedata in siteData)
                    {
                        if (siteData.Count > 0)
                        {
                            siteName = sitedata.Name;
                            var sitebuildingdata = buildingData.Where(x => x.SiteCode == sitedata.Code).ToList<Buildings>();
                            if (sitebuildingdata.Count > 0)
                            {
                                foreach (var buildingdata in sitebuildingdata)
                                {
                                    foreach (var assetdata in assetData)
                                    {

                                        var Floordata = floorData.Where(x => x.BuildingId == buildingdata.BuildingId).ToList<Floor>();
                                        if (Floordata.Count > 0)
                                        {

                                            foreach (var florrdata in Floordata)
                                            {
                                                var flooraasetdata = floorassetData.Where(x => x.FloorId == florrdata.FloorId && x.AssetId == assetdata.AssetId).ToList<TFloorAssets>();
                                                if (flooraasetdata.Count > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(siteName))
                                                    {
                                                        table.AddCell(new Phrase(siteName));
                                                        siteName = "";
                                                    }
                                                    else
                                                    {
                                                        table.AddCell(new Phrase(""));
                                                    }

                                                    table.AddCell(new Phrase(buildingdata.BuildingName));
                                                    table.AddCell(new Phrase(assetdata.Name));
                                                    table.AddCell(new Phrase(flooraasetdata.FirstOrDefault().AssetNo));

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "TrackingAssets" + DateTime.UtcNow.ToShortDateString() + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }
        #endregion

        #region Document Dashboard
        public ActionResult CreateDocumentDashBoardReport(int selectedUser, int inprogress = 0, int dueWitndays = 0, int pastDueordef = 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 35);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.Red);
                SetHeader(out table);
                pdfDoc.Add(table);
                Paragraph para = new Paragraph("Document Report", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);
                table.DefaultCell.Border = 0;
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                };
                table.AddCell(_pdfService.AddNewCell("Users:", CellFont, false));
                if (selectedUser == 0)
                {
                    table.AddCell(_pdfService.AddNewCell("All", CellFontBoldBlueSmall, false));
                }
                else
                {
                    UserProfile userprofile = _userService.GetUserList(selectedUser);
                    table.AddCell(_pdfService.AddNewCell(userprofile.FullName, CellFontBoldBlueSmall, false));
                }
                table.AddCell(_pdfService.AddNewCell("Document Status:", CellFont, false));
                if (inprogress == 0 && dueWitndays == 0 && pastDueordef == 0)
                    table.AddCell(_pdfService.AddNewCell("All", CellFontBoldBlueSmall, false));
                else if (inprogress == 1 && dueWitndays == 0 && pastDueordef == 0)
                    table.AddCell(_pdfService.AddNewCell("InProgress", CellFontBoldBlueSmall, false));
                else if (inprogress == 0 && dueWitndays == 1 && pastDueordef == 0)
                    table.AddCell(_pdfService.AddNewCell("DueWithInDays", CellFontBoldBlueSmall, false));
                else if (inprogress == 0 && dueWitndays == 0 && pastDueordef == 1)
                    table.AddCell(_pdfService.AddNewCell("Deficient", CellFontBoldBlueSmall, false));

                cell = new PdfPCell(new Phrase("Document Status : "));

                pdfDoc.Add(table);
                table = new PdfPTable(5)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };

                cell = new PdfPCell(new Phrase("Document"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Standard,EP"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Last Review"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Next Review"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Responsible Party"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);

                var docs = _transactionService.GetPolicyBinders(UserSession.CurrentUser.UserId, selectedUser, null);
                if (inprogress > 0)
                {
                    docs = docs.Where(x => x.DocStatus == 2).ToList();
                }
                if (pastDueordef > 0)
                {
                    docs = docs.Where(x => x.DocStatus != 1 && x.DocStatus != 2).ToList();
                }
                if (dueWitndays > 0)
                {
                    docs = docs.Where(x => x.DueWithInDays <= 0 || x.DueWithInDays <= dueWitndays).ToList();
                }
                docs = docs.Where(x => x.EPDocument.Any()).OrderBy(x => x.Name).ToList();

                foreach (var item in docs)
                {
                    table.AddCell(new Phrase(item.Name));
                    List<string> grouplist = new List<string>();
                    foreach (var epDocs in item.EPDocument.Where(x => x.EPDetailId > 0).OrderBy(x => x.StandardEPs.StandardEP))
                    {
                        grouplist.Add(epDocs.StandardEPs.StandardEP);
                    }
                    table.AddCell(new Phrase(string.Join(",", grouplist.Distinct())));
                    if (item.LastUploadedDate.HasValue)
                    {
                        table.AddCell(new Phrase(Convert.ToDateTime(item.LastUploadedDate).ToString("MMM d, yyyy")));
                    }
                    else { table.AddCell(new Phrase(" ")); }
                    if (item.NextDueDateDate.HasValue)
                    {
                        table.AddCell(new Phrase(Convert.ToDateTime(item.NextDueDateDate).ToString("MMM d, yyyy")));
                    }
                    else { table.AddCell(new Phrase(" ")); }

                    if (item.DocUserProfiles.Any())
                    {
                        var users = item.DocUserProfiles.Where(x => !x.IsCRxUser).Distinct().ToList();
                        var showCounts = 3;
                        var totalCount = users.Count;
                        var usersText = string.Join(",", users.Take(showCounts).Select(x => x.FullName));
                        if (users != null || !string.IsNullOrEmpty(usersText))
                        {
                            if (totalCount > showCounts)
                            {
                                table.AddCell(new Phrase(usersText + "+" + (totalCount - showCounts)));
                            }
                            else
                            {
                                table.AddCell(new Phrase(usersText));
                            }
                        }
                        else
                        {
                            table.AddCell(new Phrase(" "));
                        }

                    }
                    else
                    {
                        table.AddCell(new Phrase(" "));
                    }

                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "DocumentDashboard" + DateTime.UtcNow.ToShortDateString() + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        #endregion
        #region Round
        public ActionResult RoundInspectionPdf(int floorId, string roundid, int? groupround = 0)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var mem = new MemoryStream();
               
                    pdfDoc = _pdfService.RoundInspection(floorId, roundid,ms, groupround.Value);
                    //string fileName = "FSBP_" + objTFireSystemByPassPermit.TFSBPermitId + ".pdf";
                    string fileName = System.Web.HttpUtility.UrlEncode("RoundInspection.pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);
                
            }
            return View();


        }
        #endregion

        #region AssetDeviceChange

        public ActionResult AssetDeviceChangeFormPdf(string formId, bool? IsAttachmentIncluded = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document();
                var mem = new MemoryStream();
                TAssetDeviceChangeForms newForm = _permitService.GetAssetChangeDeviceFormsById(Guid.Parse(formId));
                if (newForm != null)
                {
                    pdfDoc = _pdfService.CreateAssetDeviceChangePermit(formId, ms, newForm, IsAttachmentIncluded.Value);
                    // string fileName = System.Web.HttpUtility.UrlEncode("LSD" + (newForm.FormType == 1 ? "A_" : "R_") + formId + ".pdf");
                    string fileName = System.Web.HttpUtility.UrlEncode(newForm.PermitNo + (newForm.FormType == 1 ? "_A" : "_R") + ".pdf");
                    CreatePdfFile(pdfDoc, fileName, ms);

                }
            }
            return View();
        }

        #endregion
        public class PDFFooter : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate footerTemplate;
            BaseFont bf = null;

            private string FileTitle;

            public PDFFooter(string fileTitle)
            {
                FileTitle = fileTitle;
            }
            public PDFFooter()
            { }

            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                footerTemplate = cb.CreateTemplate(50, 25);
                //PdfPTable tabFot = new PdfPTable(new float[] { 1F })
                //{
                //    SpacingAfter = 10F
                //};
                //PdfPCell cell;
                //tabFot.TotalWidth = 300F;
                //cell = new PdfPCell(new Phrase(FooterGeneratedBy()))
                //{
                //    Border = Rectangle.NO_BORDER
                //};
                //tabFot.AddCell(cell);
                //tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
            }

            // write on start of each page
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                // DateTime horario = DateTime.Now;
                base.OnEndPage(writer, document);

                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //document.Add(line);

                //string text = "Page " + writer.PageNumber + " of " ;

                PdfPTable tabFot = new PdfPTable(2);
                PdfPCell cell;
                tabFot.TotalWidth = 950;
                //tabFot.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase(FooterMsg(), CellFontSmall))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                //tabFot.AddCell(cell);
                //PdfPCell cell;
                //tabFot.TotalWidth = 300F;
                string lbltext = !string.IsNullOrEmpty(FileTitle) ? FileTitle : string.Empty;
                cell = new PdfPCell(new Phrase(lbltext, CellFontSmall))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                tabFot.AddCell(cell);
                tabFot.WriteSelectedRows(0, -1, 10, document.Bottom + 10, writer.DirectContent);
                {
                    string generated = FooterMsg();
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 8);
                    cb.SetTextMatrix(document.PageSize.GetLeft(25), document.PageSize.GetBottom(25));
                    cb.ShowText(generated);
                    cb.EndText();
                    string text = "Page " + writer.PageNumber + " of ";
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 8);
                    cb.SetTextMatrix(document.PageSize.GetRight(325), document.PageSize.GetBottom(25));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 8);
                    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(325) + len, document.PageSize.GetBottom(25));
                }

            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                footerTemplate.BeginText();
                footerTemplate.SetFontAndSize(bf, 8);
                footerTemplate.SetTextMatrix(0, 0);
                footerTemplate.ShowText((writer.PageNumber).ToString());
                footerTemplate.EndText();
            }

            public string FooterGeneratedBy()
            {
                return $"{Localize.Resource.PrintGeneratedByTitle}: {Localize.Resource.PrintGeneratedFromText}";
            }

            public string FooterGeneratedDate()
            {
                return $"{Localize.Resource.PrintGeneratedDateTitle}: {DateTime.Now.ToShortDateString()}";
            }

            public string FooterMsg()
            {
                return
                    $"{Localize.Resource.PrintGeneratedFromTitle} {Localize.Resource.PrintGeneratedFromText} {"on"} {DateTime.Now.ToFormatDate()}";
            }
        }

        #region Compilance Dashboard
        public ActionResult CreateCompilanceDashBoardReport(int? userid, string sortOrder = "", string orderbySort = "", int? categoryId = null, int? status = null, int? noofdays = null, int? duemonth = null, int? dueyear = null, string searchtext = "", bool isScroll = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 35);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
                pdfWriter.PageEvent = new PDFFooter();
                pdfDoc.Open();
                PdfPTable table;
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.Red);
                SetHeader(out table);
                pdfDoc.Add(table);
                Paragraph para = new Paragraph("Compilance Report", UnderlineTitleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(para);
                table.DefaultCell.Border = 0;
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                };
                table.AddCell(_pdfService.AddNewCell("Status:", CellFont, false));
                if (status == null)
                    table.AddCell(_pdfService.AddNewCell("All", CellFontBoldBlueSmall, false));
                else if (status == 2)
                    table.AddCell(_pdfService.AddNewCell("InProgress", CellFontBoldBlueSmall, false));
                else if (status == 0)
                    table.AddCell(_pdfService.AddNewCell("PastDue", CellFontBoldBlueSmall, false));
                cell = new PdfPCell(new Phrase("Status : "));

                pdfDoc.Add(table);
                table = new PdfPTable(10)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 20f
                };
                table.SetWidths(new float[] { 8f, 6f, 5f, 6f, 5f, 8f, 7f, 7f, 5f, 6f });
                cell = new PdfPCell(new Phrase("Standard,EP"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("CoP"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Score"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Binder"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Asset"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Description"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("InspectionDate"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Users"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Frequency"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("DocRequired"));
                cell.BackgroundColor = new BaseColor(System.Drawing.Color.LightBlue);
                table.AddCell(cell);


                var epDetails = new List<EPDetails>();
                var objrequest = new Request();
                objrequest.PageIndex = 0;
                objrequest.PageSize = 999999;
                objrequest.SortOrder = "StandardEP";
                objrequest.OrderbySort = "ASC";
                objrequest.SearchcBy = null;
                objrequest.FilterByUserUserId = null;
                int Userid = 0;
                if (userid == 0 || userid == null)
                {
                    Userid = UserSession.CurrentUser.UserId;
                }
                else
                {
                    Userid = (int)userid;
                }
                var httpResponse = _epService.GetDashBoardEp(objrequest, Userid, categoryId, status, noofdays, duemonth, dueyear);
                if (httpResponse.Result.EPDetails != null)
                {
                    epDetails = httpResponse.Result.EPDetails;
                }

                foreach (var item in epDetails)
                {
                    table.AddCell(new Phrase(item.StandardEp));

                    if (item.CopDetails.Count > 0)
                    {
                        List<string> grouplist = new List<string>();

                        foreach (var cop in item.CopDetails)
                        {
                            grouplist.Add(cop.RequirementName);

                        }
                        table.AddCell(new Phrase(string.Join(",", grouplist.Distinct())));
                    }
                    else
                    {
                        table.AddCell(new Phrase(""));
                    }

                    table.AddCell(new Phrase(item.Scores.Name.Replace("Risk", "")));
                    if (item.Binders.Count > 0)
                    {
                        foreach (var binder in item.Binders.GroupBy(x => x.BinderId, (key, g) => g.OrderBy(e => e.BinderName).First()))
                        {
                            table.AddCell(new Phrase(binder.BinderName));
                        }
                    }
                    else
                    {
                        table.AddCell(new Phrase(""));
                    }

                    if (item.Assets.Count > 0)
                    {
                        foreach (var asset in item.Assets)
                        {
                            table.AddCell(new Phrase(asset.Name));
                        }
                    }
                    else
                    {
                        table.AddCell(new Phrase(""));
                    }
                    Phrase phrase = new Phrase();
                    phrase.Add(new Chunk($"{item.Description}", CellFont));
                    cell = new PdfPCell(new Phrase(phrase))
                    {
                        Colspan = 1,
                        MinimumHeight = 30f,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    table.AddCell(cell);


                    if (item.Inspection.DueDate.HasValue)
                    {
                        if (item.Inspection.DueDate == DateTime.MinValue)
                        {
                            table.AddCell(new Phrase(""));
                        }
                        else
                        {
                            table.AddCell(new Phrase(item.Inspection.DueDate.ToFormatDate()));
                        }



                    }


                    var users = item.EPUsers.Where(x => !x.IsCRxUser).ToList();

                    var usersText = string.Join(",", users.Select(x => x.FullName));
                    table.AddCell(new Phrase(usersText));

                    table.AddCell(new Phrase(item.GetFrequencyName()));
                    table.AddCell(new Phrase(item.IsDocRequired ? "Yes" : "No"));

                }
                pdfDoc.Add(table);
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                string fileName = "ComplianceDashboard" + DateTime.UtcNow.ToShortDateString() + ".pdf";
                CreatePdfFile(pdfDoc, fileName, ms);
            }
            return View();
        }

        #endregion
    }

}
