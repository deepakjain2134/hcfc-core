using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using HCF.Utility.AppConfig;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace HCF.BAL
{
    public class PdfService : IPdfService
    {
        private readonly IHCFSession _session;
        private readonly IAppSetting _appSetting;
        private readonly ICommonProvider _commonProvider;
        private readonly IOrganizationService _organizationService;
        private readonly IExercisesService _exercisesService;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ITransactionService _inspectionService;
        private readonly IPermitService _permitService;
        private readonly IHotWorkPermitService _hotWorkPermitService;
        private readonly IPCRAService _pCRAService;
        private readonly IConstructionService _constructionService;
        private readonly IIlsmService _ilsmService;
        private readonly IRoundInspectionsService _roundInspectionService;
        private readonly IRoundsService _roundService;

        public PdfService(IIlsmService ilsmService, IConstructionService constructionService, IPCRAService pCRAService, IHotWorkPermitService hotWorkPermitService, IPermitService permitService, ITransactionService inspectionService, IExercisesRepository exercisesRepository, IExercisesService exercisesService, IHCFSession session, IOrganizationService organizationService, IAppSetting appSetting, ICommonProvider commonProvider, IRoundInspectionsService roundInspectionService, IRoundsService roundService)
        {
            _ilsmService = ilsmService;
            _constructionService = constructionService;
            _pCRAService = pCRAService;
            _hotWorkPermitService = hotWorkPermitService;
            _permitService = permitService;
            _inspectionService = inspectionService;
            _exercisesRepository = exercisesRepository;
            _session = session;
            _commonProvider = commonProvider;
            _organizationService = organizationService;
            _appSetting = appSetting;
            _exercisesService = exercisesService;
            _roundInspectionService = roundInspectionService;
            _roundService = roundService;
        }
        //   BDO.Organization CurrentOrg = OrganizationRepository.GetOrganizations().FirstOrDefault(x => x.ClientNo == Utility.HcfSession.ClientNo);

        #region Color

        public static readonly System.Drawing.Color GrayColor = System.Drawing.Color.FromArgb(231, 231, 231);
        public static readonly BaseColor Gray = new BaseColor(GrayColor);

        public static readonly System.Drawing.Color CyanColor = System.Drawing.Color.FromArgb(0, 162, 232);
        public static readonly BaseColor CYAN = new BaseColor(CyanColor);

        #endregion

        #region Font
        public static readonly Font CellFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        public static readonly Font CellBoldFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
        public static readonly Font smallfont = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);
        public static readonly Font smallfontbold = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
        public static readonly Font CellFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        public static readonly Font UnderlineTitleBoldFont = new Font(Font.FontFamily.HELVETICA, 11.5f, Font.BOLD | Font.UNDERLINE);
        public static readonly Font UnderlineCellBoldFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD | Font.UNDERLINE);
        public static readonly Font UnderlineCellFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.UNDERLINE);
        public static readonly Font CellFontSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        public static readonly Font CellFontDatetimeblue = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, CYAN);
        public static readonly Font CellFontBold = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD);
        public static readonly Font TitleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.NORMAL);
        public static readonly Font TitleFontS = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
        public static readonly Font UnderlineTitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.UNDERLINE);
        public static readonly Font ParagraphFontlarge = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
        public static readonly Font ParagraphFont = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
        public static readonly Font ParagraphFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
        public static readonly Font Strikethru = new Font(Font.FontFamily.HELVETICA, 11, Font.STRIKETHRU);
        public static readonly Font CellFontBoldSmall = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
        public static readonly Font CellFontSmall1 = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.NORMAL);
        public static readonly Font CellFontSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.NORMAL);
        public static readonly Font CellFontBoldSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.BOLD, BaseColor.BLACK);
        public static readonly Font CellFontBoldBlue = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
        public static readonly Font CellFontBoldBlack = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
        public static readonly Font CellFontBoldBlackten = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD);
        public static readonly Font CellFontBoldBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.BLACK);
        public static readonly Font CellFontNormalBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, BaseColor.BLACK);
        public static readonly Font CellStatusFont = new Font(Font.FontFamily.HELVETICA, 14f, Font.BOLD, BaseColor.RED);
        public static readonly Font CellRedtext = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.RED);
        public static readonly Font CellFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD, BaseColor.WHITE);
        public static readonly Font TitleFontHWP = new Font(Font.FontFamily.HELVETICA, 76f, Font.NORMAL, BaseColor.WHITE);
        public static readonly Font TitleFont2HWP = new Font(Font.FontFamily.HELVETICA, 35f, Font.BOLD, BaseColor.BLACK);


        #endregion

        #region  Common Methods
        public void SetHeader(out PdfPTable table)
        {
            var CurrentOrg = _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(_session.ClientNo));
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
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Image image = Image.GetInstance(_commonProvider.GetS3StaticImage(CurrentOrg.LogoPath));
            image.ScaleAbsolute(100, 80);
            cell.AddElement(image);
            table.AddCell(cell);

            Chunk chunk = new Chunk("" + CurrentOrg.Name, FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK));
            Chunk chunk1 = new Chunk("" + CurrentOrg.Address + "", FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
            cell = new PdfPCell
            {
                Border = 0
            };
            cell.AddElement(chunk);
            cell.AddElement(chunk1);

            table.AddCell(cell);

            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //PdfPCell cell3 = new PdfPCell
            //{
            //    Border = 0,
            //    Colspan = 2,
            //};
            //cell3.AddElement(line);
            //table.AddCell(cell3);
        }
        private string GetLogoUrlPath(string LogoPath)
        {
            var imageBasepath = _appSetting.CommonFileBasePath;
            return !string.IsNullOrEmpty(LogoPath) ? $"{imageBasepath}{LogoPath.Replace("~/", string.Empty)}" : string.Empty;
        }
        private string GetFilePath(string filePath)
        {
            var imageBasepath = _appSetting.CommonFileBasePath;
            return !string.IsNullOrEmpty(filePath) ? $"{imageBasepath}{filePath.Replace("~/", string.Empty)}" : string.Empty;
        }
        public void SetHeaderBlue(out PdfPTable table, string Heading)
        {
            var CurrentOrg = _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(_session.ClientNo));
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

            Image image = Image.GetInstance(GetLogoUrlPath(CurrentOrg.LogoPath));
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

        public void SetStatusHeaderBlue(out PdfPTable table, string Heading, string Status, string ApproveBy, string Date)
        {
            var CurrentOrg = _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(_session.ClientNo));
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
            Image image = Image.GetInstance(_commonProvider.GetS3StaticImage(CurrentOrg.LogoPath));
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
        private static PdfPCell getCell(String text, int alignment, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                Border = 0,
                Padding = 0,
                HorizontalAlignment = alignment

            };
            return cell;
        }

        public PdfPCell CreateSignSectionCell(string Title, DigitalSignature digitalsign, string comment, float? linewidth = 45.0F)
        {
            PdfPCell cellmain = new PdfPCell()
            {
                Border = 0,
            };

            PdfPTable digitable = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };

            if (digitalsign != null && digitalsign.DigSignatureId > 0)
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    digitable.AddCell(AddNewCell(Title, CellBoldFont, false));
                    digitable.AddCell(AddNewCell("", CellBoldFont, false));
                }

                PdfPCell cell = new PdfPCell()
                {
                    Border = 0,
                };
                Image image = Image.GetInstance(_commonProvider.FilePath(digitalsign.FilePath));
                cell.AddElement(new Chunk(image, 0, -10));
                cell.PaddingBottom = 0;
                cell.PaddingTop = 0;
                digitable.AddCell(cell);
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, linewidth.Value, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                cell.AddElement(line);
                cell.UseAscender = true;
                cell.PaddingBottom = 0;
                cell.PaddingTop = 0;
                digitable.AddCell(cell);


                digitable.AddCell(AddNewCell(digitalsign != null && digitalsign.SignByUserName != null ? digitalsign.SignByUserName + " ( " + digitalsign.SignByEmail + ")" : " ", CellFontS, false, 0, 0, 0, true));
                digitable.AddCell(AddNewCell(digitalsign != null && digitalsign.LocalSignDateTime != null ? "(" + digitalsign.LocalSignDateTime.ToClientTime().ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue, false, 0, 0, 0, true));
                if (!string.IsNullOrEmpty(comment))
                {
                    digitable.AddCell(AddNewCell("Comments:" + comment, CellFontS, false, 0, 0, 0, true));
                }

            }

            cellmain.AddElement(digitable);
            cellmain.Colspan = 1;

            return cellmain;

        }
        public Rectangle SetPaperType()
        {
            return PageSize.LETTER;
        }

        private void AddCell(PdfPTable table, string text, int rowspan)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times))
            {
                Rowspan = rowspan,
                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            };
            table.AddCell(cell);
        }
        public PdfPCell AddNewCell(string text, Font fontstyle, bool alignmentrighttype = false, int? Colspan = 0, int? Border = 0, int? cellBackColor = 0, bool paddingoff = false)
        {
            Chunk chunk1 = new Chunk(text, fontstyle);
            PdfPCell nobordercell = new PdfPCell()
            {
                Border = 0,
            };
            if (paddingoff)
            {
                nobordercell.PaddingBottom = 0;
                nobordercell.PaddingTop = 0;
            }

            if (
                Colspan > 0)
            {
                nobordercell.Colspan = Colspan.Value;
                nobordercell.Padding = 0;

            }
            nobordercell.UseVariableBorders = true;
            nobordercell.BorderColor = BaseColor.WHITE;
            if (Colspan == 0)
            {
                nobordercell.Border = 0;
            }
            if (Border == 1)
            {
                nobordercell = new PdfPCell()
                {
                    Border = Rectangle.LEFT_BORDER,
                };
                nobordercell.UseVariableBorders = true;
                Border = Rectangle.LEFT_BORDER;
                nobordercell.BorderColorRight = BaseColor.BLACK;
                nobordercell.BorderColorBottom = BaseColor.BLACK;
                nobordercell.BorderColorTop = BaseColor.BLACK;
                nobordercell.BorderColorLeft = BaseColor.BLACK;
                //nobordercell.BorderColorLeft = BaseColor.BLACK;
                //nobordercell.BorderColorBottom = BaseColor.BLACK;
            }
            if (cellBackColor > 0)
            {
                nobordercell.BackgroundColor = BaseColor.BLACK;

            }
            nobordercell.HorizontalAlignment = alignmentrighttype ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
            nobordercell.AddElement(chunk1);
            return nobordercell;
        }

        public PdfPTable AddAttachmentCell(string filepath, string Filename, PdfWriter pdfWriter, PdfPTable table)
        {
            var bucketName = Convert.ToString(_session.ClientNo);
            FileInfo fi = new FileInfo(filepath);
            PdfPCell cell = new PdfPCell()
            {
                Border = 0,
            };
            Font link = FontFactory.GetFont("Arial", 8.5f, Font.UNDERLINE, BaseColor.BLUE);
            var c = new Chunk(Filename, link);
            c.SetAnchor(_appSetting.WebUrlPath + "Common/FilePreview?imgSrc=" + bucketName + "/" + filepath.Replace("~/", ""));
            cell.AddElement(c);
            table.AddCell(cell);

            return table;

        }
        #endregion

        #region  User Define Function

        #region  Firedrill Reports

        #region Generate Firedrill Reports

        public string GenerateFireDrillReportInbytes(int TExerciseId)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            pdfDoc = GenerateFireDrillReport(TExerciseId, mem);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        private Document GenerateFireDrillReport(int TExerciseId, Stream streamOutput)
        {
            TExercises objExercise = new TExercises();
            if (TExerciseId > 0)
            {
                objExercise = _exercisesService.GetExercises(TExerciseId).FirstOrDefault(); //.FirstOrDefault(x => x.TExerciseId == TExerciseId);
            }
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            SetHeader(out table);
            pdfDoc.Add(table);


            Paragraph para = new Paragraph("FIRE DRILL DOCUMENTATION", UnderlineTitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                    PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellFontBold));
                    Categoryname.Colspan = 4;
                    table.AddCell(Categoryname);
                    previouscategoryname = currentcategoryname;
                }
                else if (currentcategoryname != previouscategoryname)
                {
                    PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellFontBold));
                    Categoryname.Colspan = 4;
                    table.AddCell(Categoryname);
                    previouscategoryname = currentcategoryname;
                }
                HCF.BDO.Enums.FireDrillQuesStatus enums = (HCF.BDO.Enums.FireDrillQuesStatus)item.Status;
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
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                SpacingAfter = 30f
            };
            //table.AddCell(new Phrase("Evaluation Signature", CellFont));
            foreach (var item in objExercise.DigitalSignature)
            {
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                Image image = Image.GetInstance(_commonProvider.FilePath(item.FilePath));
                //image.ScaleAbsolute(150, 30);
                //cell.AddElement(image);
                cell.AddElement(new Chunk(image, 0, 0));
                table.AddCell(cell);
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                table.AddCell(cell);
            }
            pdfDoc.Add(table);

            para = new Paragraph("Evaluation Documents:", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdfDoc.Add(para);

            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Evaluation && x.DocumentType == 0).ToList().Count > 0)
            {
                pdfDoc.NewPage();

                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Evaluation && x.DocumentType == 0).ToList())
                {
                    para = new Paragraph(item.FileName, CellFont)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };
                    pdfDoc.Add(para);

                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,
                        SpacingAfter = 30f
                    };
                    var reader = new PdfReader(new Uri(_commonProvider.FilePath(item.FilePath)));
                    PdfReader.unethicalreading = true;
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = pdfWriter.GetImportedPage(reader, i);
                        Image img = Image.GetInstance(page);
                        PdfPCell cell = new PdfPCell(img, true);
                        table.AddCell(cell);
                    }
                    pdfDoc.Add(table);
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
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                SpacingAfter = 30f
            };

            //table.AddCell(new Phrase("Critique Signature", CellFont));
            foreach (var item in objExercise.CritiqueDigitalSignature)
            {
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                Image image = Image.GetInstance(_commonProvider.FilePath(item.FilePath));
                //image.ScaleAbsolute(150, 30);
                //cell.AddElement(image);
                cell.AddElement(new Chunk(image, 0, 0));
                table.AddCell(cell);
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                table.AddCell(cell);

            }
            pdfDoc.Add(table);

            para = new Paragraph("Critique Documents:", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdfDoc.Add(para);

            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Critique && x.DocumentType == 0).ToList().Count > 0)
            {
                pdfDoc.NewPage();
                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Critique && x.DocumentType == 0).ToList())
                {
                    para = new Paragraph(item.FileName, CellFont)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };

                    pdfDoc.Add(para);
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,
                        SpacingAfter = 30f
                    };
                    var reader = new PdfReader(new Uri(_commonProvider.FilePath(item.FilePath)));
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = pdfWriter.GetImportedPage(reader, i);
                        Image img = Image.GetInstance(page);
                        PdfPCell cell = new PdfPCell(img, true);
                        table.AddCell(cell);
                    }
                    pdfDoc.Add(table);
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
                SpacingBefore = 20f,
                SpacingAfter = 30f
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
                SpacingAfter = 30f
            };
            //table.AddCell(new Phrase("Education Signature", CellFont));
            foreach (var item in objExercise.EducationDigitalSignature)
            {
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                Image image = Image.GetInstance(_commonProvider.FilePath(item.FilePath));
                //image.ScaleAbsolute(150, 30);
                //cell.AddElement(image);
                cell.AddElement(new Chunk(image, 0, 0));
                table.AddCell(cell);
                cell = new PdfPCell()
                {
                    Border = 0,
                };
                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                table.AddCell(cell);

            }
            pdfDoc.Add(table);

            para = new Paragraph("Education Documents:", CellFont)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdfDoc.Add(para);

            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Education && x.DocumentType == 0).ToList().Count > 0)
            {
                pdfDoc.NewPage();
                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == HCF.BDO.Enums.DrillFileType.Education && x.DocumentType == 0).ToList())
                {
                    para = new Paragraph(item.FileName, CellFont)
                    {
                        Alignment = Element.ALIGN_LEFT
                    };

                    pdfDoc.Add(para);
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,
                        SpacingAfter = 30f
                    };
                    var reader = new PdfReader(new Uri(_commonProvider.FilePath(item.FilePath)));
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = pdfWriter.GetImportedPage(reader, i);
                        Image img = Image.GetInstance(page);
                        PdfPCell cell = new PdfPCell(img, true);
                        table.AddCell(cell);
                    }
                    pdfDoc.Add(table);
                }
            }

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;

        }

        #endregion

        #region Create Fire drill lMatrix

        //public string PrintFiredrillMatrixInbytes(int buildingTypeId, int quarterNo, int year)
        //{
        //    var mem = new MemoryStream();
        //    Document pdfDoc = new Document();
        //    pdfDoc = CreateFiredrillMatrix(buildingTypeId, quarterNo, year, mem);
        //    var pdf = mem.ToArray();
        //    return Convert.ToBase64String(pdf);
        //}

        //private Document CreateFiredrillMatrix(int buildingTypeId, int quarterNo, int year, Stream streamOutput)
        //{
        //    var quater = _exercisesRepository.GetQuarterSettings(buildingTypeId, quarterNo, year, false).FirstOrDefault();

        //    Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
        //    pdfWriter.PageEvent = new PDFFooter();
        //    pdfDoc.Open();
        //    SetHeader(out PdfPTable table);
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
        //    if (quarterNo > 0)
        //    {
        //        quaterno = Convert.ToInt32(quarterNo);

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
        //            // quatername = "Q4(Oct-Dec)";
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
        //    //Image Img = Image.GetInstance(HCF.BDO.ImagePathModel.UnAnnouncedIcon);
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
        //    //Img = Image.GetInstance(HCF.BDO.ImagePathModel.UnAnnouncedIcon);
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


        //    table = new PdfPTable(5)
        //    {
        //        WidthPercentage = 100,
        //        HorizontalAlignment = 0,
        //        SpacingBefore = 20f,
        //        SpacingAfter = 30f
        //    };
        //    table.SetWidths(new float[] { 15f, 10f, 25f, 25f, 25f });
        //    table.AddCell(new Phrase("", CellFont));
        //    table.AddCell(new Phrase("", CellFont));
        //    table.AddCell(new PdfPCell(new Phrase("Shift 1")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    table.AddCell(new PdfPCell(new Phrase("Shift 2")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
        //    table.AddCell(new PdfPCell(new Phrase("Shift 3")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });


        //    PdfPCell cell = new PdfPCell();
        //    Image image1 = Image.GetInstance(HCF.BDO.ImagePathModel.VendorBuildingIcon);
        //    image1.ScaleAbsolute(20f, 20f);

        //    Image image2 = Image.GetInstance(HCF.BDO.ImagePathModel.CalendarIcon);
        //    image2.ScaleAbsolute(20f, 20f);

        //    PdfPCell cell2 = new PdfPCell();

        //    Image image3 = Image.GetInstance(HCF.BDO.ImagePathModel.ClockIcon);
        //    image3.ScaleAbsolute(20f, 20f);

        //    Image image4 = Image.GetInstance(HCF.BDO.ImagePathModel.AnnounceIcon);
        //    image4.ScaleAbsolute(20f, 20f);

        //    Image image5 = Image.GetInstance(HCF.BDO.ImagePathModel.TickIcon);
        //    image5.ScaleAbsolute(20f, 20f);

        //    Image image6 = Image.GetInstance(HCF.BDO.ImagePathModel.DashboardIcon);
        //    image6.ScaleAbsolute(20f, 20f);

        //    //Shift 1

        //    Paragraph icon = new Paragraph
        //    {
        //        new Chunk(image1, -5, 0)
        //    };
        //    cell.AddElement(image1);
        //    table.AddCell(cell);

        //    table.AddCell(new Phrase("", CellFont));


        //    Paragraph p = new Paragraph
        //    {
        //        new Chunk(image2, 15, 0),
        //        new Chunk(image3, 35, 0),
        //        new Chunk(image4, 55, 0),
        //        new Chunk(image5, 70, 0)
        //    };
        //    //p.Add(new Chunk(image6, -1, 0));
        //    cell2.AddElement(p);
        //    table.AddCell(cell2);
        //    table.AddCell(cell2);
        //    table.AddCell(cell2);

        //    //pdfDoc.Add(table);
        //    for (var i = 0; i < quater.Buildings.Count(); i++)
        //    {
        //        AddCell(table, quater.Buildings[i].BuildingName, quater.FireDrillTypes.Count);
        //        for (var kk = 0; kk < quater.FireDrillTypes.Count; kk++)
        //        {
        //            var ii = quater.FireDrillTypes[kk].Id;
        //            AddCell(table, quater.FireDrillTypes[kk].FireDrillType, 1);
        //            for (var j = 0; j < quater.Buildings[i].Shifts.Count(); j++)
        //            {
        //                string date = string.Empty;
        //                if (quater.Buildings[i].Shifts[j].Exercises[ii].Date.HasValue && quater.Buildings[i].Shifts[j].Exercises[ii].Date != null)
        //                {
        //                    date = quater.Buildings[i].Shifts[j].Exercises[ii].Date.Value.ToString("ddd, MMM d, yyyy");
        //                    string IsAnnounced = quater.Buildings[i].Shifts[j].Exercises[ii].Announced ? "Yes" : "No";
        //                    string IsDone = quater.Buildings[i].Shifts[j].Exercises[ii].Status == 0 ? "Scheduled" : "Completed";
        //                    string firedrillstring = $"{date + " "} {quater.Buildings[i].Shifts[j].Exercises[ii].StartTime + " "} {IsAnnounced + " "} {IsDone} {"\n"} {quater.Buildings[i].Shifts[j].Exercises[ii].NearBy}";
        //                    AddCell(table, firedrillstring, 1);
        //                }
        //                else { AddCell(table, "", 1); }
        //                //addCell(table, "Wed, Mar 18, 2020. 03:20 YES PLAN ?", 1);                     
        //            }
        //        }
        //    }
        //    pdfDoc.Add(table);
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();
        //    return pdfDoc;
        //}

        #endregion Create Firedrill Matrix

        #endregion  Firedrill Reports

        #region Create Assets Reports

        public string PrintAssetsReportsInbytes(int userId, int epdetailId, ref EPDetails epdetails)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            //pdfDoc = CreateAssetsReports(userId, epdetailId, mem, ref epdetails);
            pdfDoc = CreateAssetsInspectionReport(userId, epdetailId, mem, ref epdetails);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        #endregion Create Assets Reports

        private Document CreateAssetsReports(int userId, int epdetailId, Stream streamOutput, ref EPDetails epDetail)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            SetHeader(out PdfPTable table);
            pdfDoc.Add(table);
            epDetail = _inspectionService.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(epdetailId), 0); //DAL.InspectionRepository.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(epdetailId), 0);
            if (epDetail.TInspectionActivity != null && epDetail.TInspectionActivity.Count > 0)
            {
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} : {1}", epDetail.Standard.TJCStandard, epDetail.Standard.TJCDescription))));
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} : {1}", epDetail.EPNumber, epDetail.Description))));

                pdfDoc.Add(table);
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,
                    SpacingAfter = 30f
                };
                table.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                //table.AddCell(new PdfPCell(new Phrase("Asset")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Asset #")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Status")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Comment")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Inspection Date")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                var assetsActivity = (from p in epDetail.TInspectionActivity.Where(x => x.ActivityType == 2)
                                      select new
                                      {
                                          //asset = p.TFloorAssets.Assets.Name,
                                          status = p.Status,
                                          assetNo = p.TFloorAssets.AssetNo,
                                          comment = p.Comment,
                                          inspectiondate = p.ActivityInspectionDate
                                      }).ToList().OrderBy(x => x.assetNo);
                foreach (var item in assetsActivity)
                {
                    string statustext = item.status == 1 ? "Compliant" : item.status == 0 ? "Non Compliant" : "Pending";
                    //table.AddCell(new Phrase(item.asset, CellFont));
                    table.AddCell(new Phrase(item.assetNo, CellFont));
                    table.AddCell(new Phrase(statustext, CellFont));
                    table.AddCell(new Phrase(item.inspectiondate.ToString(), CellFont));
                    table.AddCell(new Phrase(item.comment, CellFont));
                }
                pdfDoc.Add(table);
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        public Document CreateAssetsInspectionReport(int userId, int? selEpdetailId, Stream streamOutput, ref EPDetails epDetail)
        {
            var CurrentOrg = _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(_session.ClientNo));
            //var tfloorAssets = new List<TFloorAssets>();
            var ep = new EPDetails();
            //var assets = new Assets();

            if (selEpdetailId.HasValue && selEpdetailId.Value > 0)
                ep = _inspectionService.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(selEpdetailId), 0);
            //ep = DAL.InspectionRepository.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(selEpdetailId), 0);

            var assetsActivity = (from p in ep.TInspectionActivity.Where(x => x.ActivityType == 2)
                                  select new
                                  {
                                      assetName = p.TFloorAssets.Assets.Name,
                                      tfloorAssets = p.TFloorAssets,
                                      status = p.Status,
                                      assetNo = p.TFloorAssets.DeviceNo,
                                      comment = p.Comment,
                                      inspectiondate = p.ActivityInspectionDate != null ? p.ActivityInspectionDate : null,
                                      nearby = p.TFloorAssets.NearBy,
                                      inspectorName = p.UserProfile != null ? p.UserProfile.FullName : string.Empty
                                  }).ToList().OrderBy(x => x.assetNo).ToList();

            var assetTypeName = "";
            if (assetsActivity.Count > 0)
                assetTypeName = assetsActivity.FirstOrDefault().tfloorAssets.Assets.Name;


            Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
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

            var totalAssets = assetsActivity.Count;
            var totaltested = 0;
            var totalPass = 0;
            var totalFailed = 0;
            var totalpassedPercent = 0;
            if (totalAssets > 0)
            {
                var latestTfloorAssetActivity = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).OrderByDescending(x => x.ActivityInspectionDate).GroupBy(test => test.FloorAssetId)
                       .Select(grp => grp.First());

                totaltested = latestTfloorAssetActivity.Count(y => y.IsCurrent);
                totalPass = latestTfloorAssetActivity.Count(y => y.Status == 1 && y.IsCurrent);
                totalFailed = latestTfloorAssetActivity.Count(y => y.Status == 0 && y.IsCurrent);
                totalpassedPercent = (totalPass * 100 / totalAssets);
            }

            var activityStartingdate = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).Min(x => x.ActivityInspectionDate);
            var activityEndingdate = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).Max(x => x.ActivityInspectionDate);

            table.AddCell(new Phrase(assetTypeName, CellFont));
            table.AddCell(new Phrase(totalAssets.ToString(), CellFont));
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
            phrase.Add(new Chunk($"{(activityStartingdate.HasValue ? activityStartingdate.Value.ToString("ddd, MMM d, yyyy") : string.Empty)} - {(activityEndingdate.HasValue ? activityEndingdate.Value.ToString("ddd, MMM d, yyyy") : string.Empty)}", CellFont));
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
            phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName} Inspection of {assetTypeName}", CellFont));
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

            var inspectorInformtaion = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null).Select(y => y.UserProfile).GroupBy(test => test.UserId)
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
                phrase.Add(new Chunk(item.IsVendorUser ? "" : CurrentOrg.Name, CellFont));
                table.AddCell(new Phrase(phrase));

                phrase = new Phrase();
                phrase.Add(new Chunk("Address: ", CellFontBold));
                phrase.Add(new Chunk(item.IsVendorUser ? "" : CurrentOrg.Name, CellFont));
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

            table.DefaultCell.BackgroundColor = BaseColor.WHITE;

            foreach (var tfloorAsset in assetsActivity.Select((value, index) => new { value, index }))
            {
                string buildingfloor = string.Format("{0}{1}", tfloorAsset.value.tfloorAssets.Floor.Building.BuildingName, !string.IsNullOrEmpty(tfloorAsset.value.tfloorAssets.Floor.FloorName) ? $", {tfloorAsset.value.tfloorAssets.Floor.FloorName}" : "");
                table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.assetNo, CellFont));
                table.AddCell(new Phrase(!string.IsNullOrEmpty(buildingfloor) ? buildingfloor : "NA", CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.nearby, CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.inspectorName.ToString(), CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.inspectiondate.HasValue ? tfloorAsset.value.inspectiondate.Value.ToString("ddd, MMM d, yyyy") : string.Empty, CellFont));
                table.AddCell(new Phrase(tfloorAsset.value.status == 1 ? "Compliant" : tfloorAsset.value.status == 0 ? "Non Compliant" : "Pending", CellFont));
            }
            pdfDoc.Add(table);

            table = new PdfPTable(3)
            {
                WidthPercentage = 75,
                HorizontalAlignment = Element.ALIGN_CENTER,
                SpacingBefore = 20f,
                SpacingAfter = 10f
            };
            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(new Phrase("#", CellFontBoldBlue));
            table.AddCell(new Phrase("Asset #", CellFontBoldBlue));
            table.AddCell(new Phrase("Comment", CellFontBoldBlue));
            table.DefaultCell.BackgroundColor = BaseColor.WHITE;
            foreach (var tfloorAsset in assetsActivity.Select((value, index) => new { value, index }))
            {
                if ((!string.IsNullOrEmpty(tfloorAsset.value.comment)))
                {
                    table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
                    table.AddCell(new Phrase(tfloorAsset.value.assetNo, CellFont));
                    table.AddCell(new Phrase(tfloorAsset.value.comment, CellFont));
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
                var latestinspection = ep.TInspectionActivity.Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
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

                table.AddCell(new Phrase("Owner/Representative", CellFont));
                table.AddCell(new Phrase("Digitally Signed On", CellFont));

                foreach (var user in inspectorInformtaion.Where(x => x.IsVendorUser))
                {
                    var latestinspectiondate = string.Empty;
                    var latestinspection = ep.TInspectionActivity.Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
                    if (latestinspection != null)
                        latestinspectiondate = latestinspection.Value.ToString("ddd, MMM d, yyyy");
                    table.AddCell(new Phrase(user.FullName, CellFont));
                    table.AddCell(new Phrase(latestinspectiondate, CellFont));
                }
                pdfDoc.Add(table);
            }

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        #region Ceiling Permit

        public Document CreateCeilingPermit(int ceilingPermitId, Stream streamOutput, CeilingPermit objceilingPermit, bool IsAttachmentIncluded)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 35);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            string approverName = objceilingPermit.TPermitWorkFlowDetails != null && objceilingPermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.HasValue && objceilingPermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelValue.Value > 0 ? objceilingPermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().UserDetail.Name : string.Empty;
            DateTime? approvedt = objceilingPermit.TPermitWorkFlowDetails != null && objceilingPermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelSignDate != null ? objceilingPermit.TPermitWorkFlowDetails.OrderByDescending(x => x.Id).ToList().FirstOrDefault().LabelSignDate : (DateTime?)null;
            if (objceilingPermit.Status == 2 || objceilingPermit.Status == -1)
            {
                SetHeaderBlue(out table, "Ceiling Permit (CP)");
            }
            else
            {
                SetStatusHeaderBlue(out table, "Ceiling Permit (CP)", Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), objceilingPermit.Status).ToString().ToUpper(), approverName, ($"{approvedt:MMM d, yyyy}"));

            }
            pdfDoc.Add(table);

            table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 10f,
                SpacingBefore = 10f
            };
            float[] widths = new float[] { 20f, 60f, 20f };
            table.SetWidths(widths);
            table.AddCell(getCell("", PdfPCell.ALIGN_LEFT, CellFont));
            table.AddCell(getCell("Above the Ceiling Permit Request", PdfPCell.ALIGN_CENTER, UnderlineTitleFont));
            //table.AddCell(getCell("Text in the middle", PdfPCell.ALIGN_CENTER,CellFont));
            table.AddCell(getCell("Permit #: " + objceilingPermit.PermitNo, PdfPCell.ALIGN_RIGHT, CellFont));
            pdfDoc.Add(table);
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 10f
            };
            cell = new PdfPCell(new Phrase("Project #: " + (objceilingPermit.ProjectId > 0 ? objceilingPermit.Project.ProjectNumber : string.Empty), CellFont))
            {

                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Project Name: " + (objceilingPermit.ProjectId > 0 ? objceilingPermit.Project.ProjectName : string.Empty), CellFont))
            {

                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Work area:  " + objceilingPermit.WorkArea, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Scope of work:  " + objceilingPermit.Scope, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Building(s):  " + objceilingPermit.BuildingName, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Floor(s):  " + objceilingPermit.FloorName, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Zone(s):  " + objceilingPermit.Zones, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Start time:  " + _commonProvider.ConvertToAMPM(objceilingPermit.StartTime), CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("End time:  " + _commonProvider.ConvertToAMPM(objceilingPermit.EndTime), CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Work start date:  " + (objceilingPermit.StartDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objceilingPermit.StartDate.Value) : string.Empty), CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Estimated completion date:  " + (objceilingPermit.CompletionDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objceilingPermit.CompletionDate.Value) : string.Empty), CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Responsible party for sealing the penetrations: " + objceilingPermit.Responsible, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Type of sealant used:  " + objceilingPermit.TypeofSealant, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            string uLApproved = (objceilingPermit.ULApproved) ? "Yes" : "No";
            cell = new PdfPCell(new Phrase("UL approved for use?: " + uLApproved, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);



            cell = new PdfPCell(new Phrase("Organization/Dept:  " + objceilingPermit.RequestedDept, CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Telephone #:  " + objceilingPermit.PhoneNo, CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("Additional items (circle): " + (objceilingPermit.AdditionalItems.HasValue ? Enum.GetName(typeof(HCF.BDO.Enums.AdditionalItems), objceilingPermit.AdditionalItems).ToString() : string.Empty), CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("I UNDERSTAND THAT PAYMENT WILL NOT BE RELEASED UNTIL ALL PENETRATIONS HAVE BEEN SEALED, INSPECTED, AND APPROVED BY A REPRESENTITIVE FROM THE PLANT OPERATIONS DEPARTMENT.  I FURTHER CERTIFY THAT I AM AUTHORIZED TO SIGN FOR MY ORGANIZATION.", CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            foreach (var permitworkflow in objceilingPermit.TPermitWorkFlowDetails.Where(x => x.Sequence == 1))
            {
                cell = new PdfPCell(new Phrase(permitworkflow.LabelText + ": " + (permitworkflow.UserDetail != null ? permitworkflow.UserDetail.Name : string.Empty), CellFont))
                {

                    MinimumHeight = 15f,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Date of request: " + (permitworkflow.LabelSignDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(permitworkflow.LabelSignDate.Value) : string.Empty), CellFont))
                {
                    MinimumHeight = 15f,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(cell);
                if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                {
                    PdfPTable table2 = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0
                    };
                    PdfPCell cellmain2 = new PdfPCell()
                    {
                        Border = 0
                    };

                    cellmain2 = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 30f);
                    table2.AddCell(cellmain2);
                    cellmain2 = new PdfPCell()
                    {
                        Colspan = 2
                    };
                    cellmain2.AddElement(table2);
                    table.AddCell(cellmain2);


                }

            }

            cell = new PdfPCell(new Phrase("Pictures/verification of sealed penetrations:" + (objceilingPermit.IsPenetrationsVerified != null && objceilingPermit.IsPenetrationsVerified == 1 ? "Yes" : "No"), CellFont))
            {
                Colspan = 2,
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Approval Status:  " + Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), objceilingPermit.Status).ToString().ToUpper(), CellFont))
            {
                MinimumHeight = 15f,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            if (objceilingPermit.Status == 2)
            {
                cell = new PdfPCell(new Phrase("", CellFont))
                {
                    MinimumHeight = 15f,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
            }
            else if (objceilingPermit.Status == 1)
            {
                cell = new PdfPCell(new Phrase("", CellFont))
                {
                    MinimumHeight = 15f,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
            }
            else
            {
                var label = objceilingPermit.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
                cell = new PdfPCell(new Phrase(label + objceilingPermit.Reason, CellFont))
                {
                    MinimumHeight = 15f,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };

            }
            table.AddCell(cell);
            foreach (var permitworkflow in objceilingPermit.TPermitWorkFlowDetails.Where(x => x.Sequence != 1))
            {
                if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                {
                    cell = new PdfPCell(new Phrase(permitworkflow.LabelText + " Date: " + (permitworkflow.LabelSignDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(permitworkflow.LabelSignDate.Value) : string.Empty), CellFont))
                    {
                        MinimumHeight = 15f,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(permitworkflow.LabelText + ": " + (permitworkflow.UserDetail != null ? permitworkflow.UserDetail.Name : string.Empty), CellFont))
                    {
                        MinimumHeight = 15f,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    table.AddCell(cell);
                }
            }
            PdfPTable table1 = new PdfPTable(1);
            PdfPCell cellmain = new PdfPCell();
            int j = 0;
            foreach (var permitworkflow in objceilingPermit.TPermitWorkFlowDetails.Where(x => x.Sequence != 1))
            {


                if (permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                {


                    table1 = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0
                    };
                    cellmain = new PdfPCell();
                    if ((objceilingPermit.TPermitWorkFlowDetails.Where(x => x.Sequence != 1 && x.LabelSignId.HasValue && x.LabelSignId != 0).ToList().Count - 1) == j)
                    {
                        cellmain = new PdfPCell()
                        {
                            Colspan = 2
                        };
                    }

                    cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 30f);
                    table1.AddCell(cellmain);

                    if ((objceilingPermit.TPermitWorkFlowDetails.Where(x => x.Sequence != 1 && x.LabelSignId.HasValue && x.LabelSignId != 0).ToList().Count - 1) == j)
                    {
                        cellmain = new PdfPCell()
                        {
                            Colspan = 2
                        };
                    }
                    else
                    {
                        cellmain = new PdfPCell();
                    }
                    cellmain.AddElement(table1);
                    table.AddCell(cellmain);
                    j++;
                }



            }

            pdfDoc.Add(table);
            bool isattach = false;
            if (IsAttachmentIncluded)
            {
                if (objceilingPermit.DrawingAttachments.Count > 0 || objceilingPermit.Attachments.Count > 0)
                {
                    pdfDoc.NewPage();
                    isattach = true;
                }
                if (isattach)
                {
                    if (objceilingPermit.DrawingAttachments.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 10f
                        };
                        table.AddCell(AddNewCell("Drawings Attached :", CellFontBoldBlack));
                        for (int i = 0; i < objceilingPermit.DrawingAttachments.Count; i++)
                        {
                            AddAttachmentCell(objceilingPermit.DrawingAttachments[i].ImagePath, objceilingPermit.DrawingAttachments[i].FullFileName, pdfWriter, table);
                        }
                        pdfDoc.Add(table);
                    }
                    if (objceilingPermit.Attachments.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 10f
                        };
                        table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
                        for (int i = 0; i < objceilingPermit.Attachments.Count; i++)
                        {
                            AddAttachmentCell(objceilingPermit.Attachments[i].FilePath, objceilingPermit.Attachments[i].FileName, pdfWriter, table);
                        }
                        pdfDoc.Add(table);
                    }
                }



            }



            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }


        public string CeilingPermitInbytes(int? ceilingPermitId, bool IsAttachmentIncluded)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            CeilingPermit objceilingPermit = new CeilingPermit();

            objceilingPermit = _permitService.GetCeilingPermit(ceilingPermitId.Value);
            pdfDoc = CreateCeilingPermit(ceilingPermitId.Value, mem, objceilingPermit, IsAttachmentIncluded);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }


        #endregion

        #region LifeSafety
        public string LifeSafetyFormInbytes(string formId)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();

            TLifeSafetyDevicesForms newForm = _permitService.GetLifeSafetyForm(Guid.Parse(formId));

            pdfDoc = CreateLifeSafetyPermit(formId, mem, newForm, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        public Document CreateLifeSafetyPermit(string formId, Stream streamOutput, TLifeSafetyDevicesForms newForm, bool IsAttachmentIncluded)
        {
            if (newForm.LifeSafetyDeviceList.Count == 0)
            {
                LifeSafetyDeviceList device = new LifeSafetyDeviceList();
                newForm.LifeSafetyDeviceList.Add(device);
            }

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            string approverName = newForm.ApprovedByUser != null ? newForm.ApprovedByUser.Name : string.Empty;
            if (newForm.Status == 2 || newForm.Status == -1)
            {
                SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            }
            else
            {

                SetStatusHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"), Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), newForm.Status).ToString().ToUpper(), approverName, ($"{newForm.SignDate:MMM d, yyyy}"));

            }
            //SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            PdfPTable tableapprove = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };
            pdfDoc.Add(table);

            if (newForm != null)
            {
                table = new PdfPTable(6)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 5f

                };

                float[] widths = new float[] { 25f, 25f, 5f, 20f, 5f, 20f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;

                Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                ImgCheckBox.ScaleAbsolute(10f, 10f);

                Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

                PdfPCell nobordercell = new PdfPCell()
                {
                    Border = 0,
                };


                Chunk chunk1 = new Chunk();
                table.AddCell(AddNewCell("Section 1: Basic Information", CellFontBoldBlueSmall, false, 6));
                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.PermitNo, CellFont, false));
                if (newForm.IsMOPSubmission)
                {
                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);


                }
                else
                {
                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                }
                table.AddCell(AddNewCell("MOP Submission", CellFont, false));
                if (newForm.IsFinalSubmission)
                {

                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                }
                else
                {
                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);
                }
                table.AddCell(AddNewCell("Final Submission", CellFont, false));
                PdfPCell cellattachment = new PdfPCell()
                {
                    Border = 0,
                };

                pdfDoc.Add(table);
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 10f

                };


                widths = new float[] { 25f, 25f, 25f, 25f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.ProjectName, CellFont, false));
                table.AddCell(AddNewCell("Project #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Project.ProjectNumber, CellFont, false));
                table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell($"{newForm.DateOfRequest:MMM d, yyyy}", CellFont, false));
                table.AddCell(AddNewCell("Requestor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.RequestorUser != null ? newForm.RequestorUser.Name : string.Empty, CellFont, false));
                table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Contractor, CellFont, false));
                table.AddCell(AddNewCell("Building(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.BuildingName, CellFont, false));
                table.AddCell(AddNewCell("Floor(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.FloorName, CellFont, false));
                table.AddCell(AddNewCell("Zone(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Zones, CellFont, false));
                table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.EmailAddress, CellFont, false));
                table.AddCell(AddNewCell("Phone:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.PhoneNumber, CellFont, false));
                table.AddCell(AddNewCell("Device Types:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.DeviceType, CellFont, false, 3));
                table.AddCell(AddNewCell("Brief Description of Work:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Description, CellFont, false, 3));

                pdfDoc.Add(table);
                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //pdfDoc.Add(line);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 10f

                };

                table.AddCell(AddNewCell("Section 2: Life Safety Device List:", CellFontBoldBlueSmall, false));
                pdfDoc.Add(table);
                table = new PdfPTable(7)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 5f,
                    SpacingAfter = 5f

                };
                widths = new float[] { 5f, 15f, 15f, 25f, 15f, 20f, 15f };
                table.SetTotalWidth(widths);
                table.AddCell(new Phrase("#", CellFontBoldBlack));
                table.AddCell(new Phrase("Device #", CellFontBoldBlack));
                table.AddCell(new Phrase("Building", CellFontBoldBlack));
                table.AddCell(new Phrase("Location / FACP Alias ", CellFontBoldBlack));
                table.AddCell(new Phrase("Device Type ", CellFontBoldBlack));
                table.AddCell(new Phrase("Make / Model", CellFontBoldBlack));
                table.AddCell(new Phrase(newForm.FormType == 1 ? "Date Added " : "Date Removed", CellFontBoldBlack));
                nobordercell = new PdfPCell();
                for (int i = 0; i < newForm.LifeSafetyDeviceList.Count; i++)
                {
                    //var type = newForm.FormType ? newForm.FormType.ToString() : string.Empty;
                    //var sequence = this.ViewData.ContainsKey("sequence") ? this.ViewData["sequence"].ToString() : string.Empty;

                    table.AddCell(new Phrase(string.Format("{0}", i + 1), CellFont));
                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].DeviceNumber, CellFont));
                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Building, CellFont));
                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Location, CellFont));
                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].AssetType, CellFont));
                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Make, CellFont));
                    if (newForm.LifeSafetyDeviceList[i].DateAdded.HasValue || newForm.LifeSafetyDeviceList[i].DateRemoved.HasValue)
                        table.AddCell(new Phrase(newForm.FormType == 1 ? HCF.Utility.CommonUtility.ConvertToFormatDate(newForm.LifeSafetyDeviceList[i].DateAdded.Value) : HCF.Utility.CommonUtility.ConvertToFormatDate(newForm.LifeSafetyDeviceList[i].DateRemoved.Value), CellFont));
                    else
                        table.AddCell(new Phrase(string.Empty, CellFont));

                }
                pdfDoc.Add(table);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 5f

                };

                table.AddCell(AddNewCell("SECTION 3:  Approval  [Hospital Employee’s Only] ", CellFontBoldBlueSmall, false));
                pdfDoc.Add(table);

                if (newForm.Status == 2)
                {
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell("Requested", CellFont, false));
                }
                else if (newForm.Status == 1)
                {
                    table = new PdfPTable(3)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    widths = new float[] { 60f, 20f, 20f };
                    table.SetTotalWidth(widths);

                    if (newForm.ApprovedByUser != null && newForm.PermitAuthorizedSignature != null && newForm.PermitAuthorizedSignature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = CreateSignSectionCell("Approved By:", newForm.PermitAuthorizedSignature, string.Empty);
                        table.AddCell(cellmain);
                    }
                    else
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                    }
                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell("Approved", CellFont, false));
                }
                else
                {
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Rejected" : "On Hold/Pending", CellFont, false));
                    table.AddCell(AddNewCell("Approval By:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(newForm.ApprovedByUser != null ? newForm.ApprovedByUser.Name : string.Empty, CellFont, false));
                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Reason(s) of Rejection" : "Reason(s) of on Hold/Pending", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(newForm.Reason, CellFont, false));
                }
                pdfDoc.Add(table);

                bool isattach = false;
                if (IsAttachmentIncluded)
                {
                    if (newForm.DrawingAttachments.Count > 0 || newForm.Attachments.Count > 0)
                    {
                        pdfDoc.NewPage();
                        isattach = true;
                    }
                    if (isattach)
                    {
                        if (newForm.Attachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingAfter = 10f

                            };

                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
                            for (int i = 0; i < newForm.Attachments.Count; i++)
                            {
                                AddAttachmentCell(newForm.Attachments[i].FilePath, newForm.Attachments[i].FileName, pdfWriter, table);


                            }
                            pdfDoc.Add(table);
                        }
                        if (newForm.DrawingAttachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingAfter = 10f

                            };

                            table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
                            for (int i = 0; i < newForm.DrawingAttachments.Count; i++)
                            {
                                AddAttachmentCell(newForm.DrawingAttachments[i].ImagePath, newForm.DrawingAttachments[i].FullFileName, pdfWriter, table);
                            }
                            pdfDoc.Add(table);
                        }
                    }
                }
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;

        }
        #endregion

        #region Fire System Bypass Permit


        public string FireSystemByPassPermitInbytes(int tfsbPermitId)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TFireSystemByPassPermit objTFireSystemByPassPermit = _permitService.GetFSBPermit(tfsbPermitId);
            pdfDoc = CreateFireSystemByPassPermit(tfsbPermitId, mem, objTFireSystemByPassPermit, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        public Document CreateFireSystemByPassPermit(int tfsbPermitId, Stream streamOutput, TFireSystemByPassPermit objTFireSystemByPassPermit, bool IsAttachmentIncluded)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            SetHeaderBlue(out table, "Fire System Bypass Permit [FSBP] ");

            Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
            ImgCheckBox.ScaleAbsolute(10f, 10f);

            Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
            ImgUnCheckBox.ScaleAbsolute(10f, 10f);

            pdfDoc.Add(table);

            table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f
            };
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.AddCell(new Phrase("Section 1: Basic Information", CellFontBoldBlueSmall));

            pdfDoc.Add(table);

            if (objTFireSystemByPassPermit != null && objTFireSystemByPassPermit.TFSBPermitId > 0)
            {
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                float[] widths = new float[] { 25f, 25f, 25f, 25f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.PermitNo, CellFont, false));
                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.ProjectName, CellFont));
                table.AddCell(AddNewCell("Date [of Request]:", CellFontBoldBlack));
                table.AddCell(AddNewCell(($"{objTFireSystemByPassPermit.RequestedDate:MMM d, yyyy}"), CellFont));
                table.AddCell(AddNewCell("Requestor:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.RequestorByUser != null ? objTFireSystemByPassPermit.RequestorByUser.Name : string.Empty, CellFont));
                table.AddCell(AddNewCell("Phone Number:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.PhoneNo, CellFont));
                table.AddCell(AddNewCell("Organization:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.Company, CellFont));
                table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.Email, CellFont));
                table.AddCell(AddNewCell("On-Site Contact:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.OnSiteContact, CellFont));
                table.AddCell(AddNewCell("On-Site Phone:", CellFontBoldBlack));
                table.AddCell(AddNewCell(objTFireSystemByPassPermit.OnSitePhone, CellFont, false, 3));

                table.AddCell(AddNewCell(($"Start Date: {objTFireSystemByPassPermit.StartDate:MMM d, yyyy}"), CellFont));
                table.AddCell(AddNewCell(($"Start Time: {_commonProvider.ConvertToAMPM(objTFireSystemByPassPermit.StartTime)}"), CellFont));
                table.AddCell(AddNewCell(($"End Date: {objTFireSystemByPassPermit.EndDate:MMM d, yyyy}"), CellFont));
                table.AddCell(AddNewCell(($"End Time: {_commonProvider.ConvertToAMPM(objTFireSystemByPassPermit.EndTime)}"), CellFont));

                pdfDoc.Add(table);

                //table = new PdfPTable(1)
                //{
                //    WidthPercentage = 100,
                //    HorizontalAlignment = 0
                //};
                //table.DefaultCell.Border = Rectangle.NO_BORDER;

                //pdfDoc.Add(table);
                PdfPTable tablemain = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                tablemain.DefaultCell.Border = Rectangle.NO_BORDER;
                widths = new float[] { 50f, 50f };
                table = new PdfPTable(2)
                {
                    WidthPercentage = 50,
                    HorizontalAlignment = 0
                };
                widths = new float[] { 20f, 30f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                Chunk chunk1 = new Chunk();
                PdfPCell nobordercell = new PdfPCell();


                PdfPTable tblFiresystem = new PdfPTable(2)
                {
                    WidthPercentage = 50,
                    HorizontalAlignment = 0
                };
                widths = new float[] { 20f, 30f };
                tblFiresystem.SetTotalWidth(widths);
                table.AddCell(AddNewCell("Building(s) Affected:", CellFontBoldBlack));
                table.AddCell(AddNewCell("", CellFontBoldBlack));
                tblFiresystem.AddCell(AddNewCell("Applicable  Fire systems :", CellFontBoldBlack));
                tblFiresystem.AddCell(AddNewCell("", CellFontBoldBlack));
                tblFiresystem.AddCell(AddNewCell("Building(s)", CellFontBoldBlack));
                tblFiresystem.AddCell(AddNewCell("Applicable systems:", CellFontBoldBlack));
                for (int i = 0; i < objTFireSystemByPassPermit.TFSBPBuildingDetails.Count; i++)
                {
                    nobordercell = new PdfPCell() { Border = 0, };
                    if (objTFireSystemByPassPermit.TFSBPBuildingDetails[i].Status)
                    {
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table.AddCell(nobordercell);
                    }
                    else
                    {
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table.AddCell(nobordercell);
                    }
                    table.AddCell(AddNewCell(objTFireSystemByPassPermit.TFSBPBuildingDetails[i].SiteBuildingName, CellFont));


                    string firesystem = string.Empty;

                    tblFiresystem.AddCell(AddNewCell(objTFireSystemByPassPermit.TFSBPBuildingDetails[i].BuildingName, CellFont));
                    for (int j = 0; j < objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes.Count; j++)
                    {
                        if (objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].CheckVal.HasValue && objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].CheckVal.Value == 1)
                        {
                            firesystem = firesystem + objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].Name + ",";
                        }
                        else
                        {
                        }

                    }
                    tblFiresystem.AddCell(AddNewCell(firesystem.TrimEnd(','), CellFont));

                }
                if (objTFireSystemByPassPermit.TFSBPBuildingDetails.Count % 4 == 1)
                {
                    table.AddCell("");
                    table.AddCell("");
                }
                tablemain.AddCell(table);
                tablemain.AddCell(tblFiresystem);
                pdfDoc.Add(tablemain);
                //pdfDoc.Add(table);
                //pdfDoc.Add(tblFiresystem);




                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };

                table.AddCell(new Phrase("Device/Points Affected:", CellFontBoldBlack));
                table.AddCell(new Phrase(objTFireSystemByPassPermit.DevicePointsAffected, CellFont));

                table.AddCell(new Phrase("Department(s)/ZonesAffected:", CellFontBoldBlack));
                table.AddCell(new Phrase(objTFireSystemByPassPermit.DepartmentZonesAffected, CellFont));
                pdfDoc.Add(table);

                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                widths = new float[] { 25f, 5f, 55f, 15f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(AddNewCell("Brief Description of Work:", CellFontBoldBlack));

                nobordercell = new PdfPCell() { Border = 0, };
                if (objTFireSystemByPassPermit.IsSystemReprogrammingRequired)
                {
                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);
                }
                else
                {
                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);
                }
                table.AddCell(AddNewCell("System Reprogramming Required and Scheduled for [Date]:", CellFontBoldBlack));
                table.AddCell(AddNewCell(($"{objTFireSystemByPassPermit.ScheduledDate:MMM d, yyyy}"), CellFontBoldBlack));
                pdfDoc.Add(table);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };
                table.AddCell(new Phrase(!string.IsNullOrEmpty(objTFireSystemByPassPermit.Description) ? objTFireSystemByPassPermit.Description : "", CellFont));

                pdfDoc.Add(table);

                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };
                table.AddCell(AddNewCell("CHILDRENS HOSPITAL MEDICAL CENTER EMPLOYEE’S ONLY ", CellRedtext, false, 2));
                table.AddCell(AddNewCell(" ", CellRedtext, false, 2));
                PdfPTable ilsmtable = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                };
                ilsmtable.DefaultCell.Border = Rectangle.BOX;
                widths = new float[] { 20f, 80f };
                ilsmtable.SetTotalWidth(widths);
                // ilsmtable.DefaultCell.Border = Rectangle.NO_BORDER;
                ilsmtable.AddCell(AddNewCell("SECTION 2: ILSM Required Checklist", CellFontBoldBlueSmall, false, 2));
                ilsmtable.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
                for (int i = 0; i < objTFireSystemByPassPermit.ILSMRequiredChecklist.Count; i++)
                {
                    nobordercell = new PdfPCell();
                    if (objTFireSystemByPassPermit.ILSMRequiredChecklist[i].RespondStatus)
                    {
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        ilsmtable.AddCell(nobordercell);
                    }
                    else
                    {
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        ilsmtable.AddCell(nobordercell);
                    }
                    ilsmtable.AddCell(new Phrase(objTFireSystemByPassPermit.ILSMRequiredChecklist[i].FSBPForms.Name, CellFont));

                }
                ilsmtable.AddCell(AddNewCell("Fire Watch Required if entire fire alarm or sprinkler system is disabled more than 4 hours ", CellFontSmall1, false, 2));
                ilsmtable.AddCell(AddNewCell(" ", CellRedtext, false, 2));
                table.AddCell(ilsmtable);

                PdfPTable StampTable;
                string approverName = objTFireSystemByPassPermit.ApprovedByUser != null ? objTFireSystemByPassPermit.ApprovedByUser.Name : string.Empty;
                CreateApprovalStamp(out StampTable, Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), objTFireSystemByPassPermit.ApprovalStatus), approverName, ($"{objTFireSystemByPassPermit.ApprovedDate:MMM d, yyyy}"), objTFireSystemByPassPermit);

                table.AddCell(StampTable);

                PdfPTable table1 = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                //widths = new float[] { 20f, 50f, 15f, 15f };
                //table1.SetTotalWidth(widths);
                //table1.DefaultCell.Border = Rectangle.NO_BORDER;
                table1.AddCell(AddNewCell("SECTION 4: Bypass Engineering Actions", CellFontBoldBlueSmall, false, 4));
                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
                table1.AddCell(new Phrase("Date in Bypass:", CellFont));
                table1.AddCell(new Phrase($"{objTFireSystemByPassPermit.BypassEngActionDate:MMM d, yyyy}", CellFont));
                table1.AddCell(new Phrase("Time in Bypass", CellFont));
                table1.AddCell(new Phrase(_commonProvider.ConvertToAMPM(objTFireSystemByPassPermit.BypassEngActionTime), CellFont));
                for (int i = 0; i < objTFireSystemByPassPermit.BypassEngineeringActions.Count; i++)
                {
                    nobordercell = new PdfPCell();
                    if (objTFireSystemByPassPermit.BypassEngineeringActions[i].RespondStatus)
                    {
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table1.AddCell(nobordercell);
                    }
                    else
                    {
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table1.AddCell(nobordercell);
                    }
                    table1.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].FSBPForms.Name, CellFont));
                    table1.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].TimeinbyPass, CellFont));
                    table1.AddCell(new Phrase(string.Empty, CellFont));
                }
                table1.AddCell(AddNewCell("Fac./ Engineering Rep:", CellFontBoldSmall2));
                if (objTFireSystemByPassPermit.DSBypassEngActionSign != null && objTFireSystemByPassPermit.DSBypassEngActionSign.DigSignatureId > 0)
                {
                    //PdfPTable tableimg = new PdfPTable(1)
                    //{
                    //    WidthPercentage = 100,
                    //    HorizontalAlignment = 0
                    //};
                    //PdfPCell cellimg = new PdfPCell()
                    //{
                    //    Border = 0,
                    //};
                    //PdfPCell cell = new PdfPCell()
                    //{
                    //    Border = 0,
                    //};
                    //Image image = Image.GetInstance(CommonUtility.FilePath(objTFireSystemByPassPermit.DSBypassEngActionSign.FilePath));
                    ////image.ScaleAbsolute(40, 40);
                    //cellimg.AddElement(new Chunk(image, 0, 0));
                    //tableimg.AddCell(cellimg);
                    //cell.AddElement(tableimg);
                    //table1.AddCell(cell);
                    //cell = new PdfPCell()
                    //{
                    //    Border = 0,
                    //};
                    //cell.Colspan = 2;
                    //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSBypassEngActionSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSBypassEngActionSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
                    //table1.AddCell(cell);



                    PdfPCell cellmain = new PdfPCell()
                    {
                        Border = 0,
                    };

                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSBypassEngActionSign, string.Empty);
                    cellmain.Colspan = 3;
                    table1.AddCell(cellmain);

                }
                else
                {
                    table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                }
                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                table.AddCell(table1);

                PdfPTable table2 = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                //widths = new float[] { 20f, 50f, 15f, 15f };
                //table2.SetTotalWidth(widths);
                // table2.DefaultCell.Border = Rectangle.NO_BORDER;
                table2.AddCell(AddNewCell("SECTION 5: Bypass Return Engineering Actions", CellFontBoldBlueSmall, false, 4));
                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
                table2.AddCell(new Phrase("Date Out Bypass:", CellFont));
                table2.AddCell(new Phrase($"{objTFireSystemByPassPermit.BypassReturnEngActionDate:MMM d, yyyy}", CellFont));
                table2.AddCell(new Phrase("Time Out Bypass", CellFont));
                table2.AddCell(new Phrase(_commonProvider.ConvertToAMPM(objTFireSystemByPassPermit.BypassReturnEngActionTime), CellFont));
                for (int i = 0; i < objTFireSystemByPassPermit.BypassReturnEngineeringActions.Count; i++)
                {
                    nobordercell = new PdfPCell();
                    if (objTFireSystemByPassPermit.BypassReturnEngineeringActions[i].RespondStatus)
                    {
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table2.AddCell(nobordercell);
                    }
                    else
                    {
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        table2.AddCell(nobordercell);
                    }
                    table2.AddCell(new Phrase(objTFireSystemByPassPermit.BypassReturnEngineeringActions[i].FSBPForms.Name, CellFont));
                    table2.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].TimeinbyPass, CellFont));
                    table2.AddCell(new Phrase(string.Empty, CellFont));
                }
                table2.AddCell(AddNewCell("Fac./ Engineering Rep:", CellFontBoldSmall2));
                if (objTFireSystemByPassPermit.DSBypassReturnEngActionSign != null && objTFireSystemByPassPermit.DSBypassReturnEngActionSign.DigSignatureId > 0)
                {

                    //PdfPCell cell = new PdfPCell()
                    //{
                    //    Border = 0,
                    //};
                    //Image image = Image.GetInstance(CommonUtility.FilePath(objTFireSystemByPassPermit.DSBypassReturnEngActionSign.FilePath));
                    ////image.ScaleAbsolute(40, 40);
                    //cell.AddElement(new Chunk(image, 0, 0));
                    //table2.AddCell(cell);
                    //cell = new PdfPCell()
                    //{
                    //    Border = 0,
                    //};
                    //cell.Colspan = 2;
                    //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSBypassReturnEngActionSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSBypassReturnEngActionSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
                    //table2.AddCell(cell);
                    PdfPCell cellmain = new PdfPCell()
                    {
                        Border = 0,
                    };

                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSBypassReturnEngActionSign, string.Empty);
                    cellmain.Colspan = 3;
                    table2.AddCell(cellmain);

                }
                else
                {
                    table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                }
                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                table.AddCell(table2);

                pdfDoc.Add(table);

                bool isattach = false;
                if (IsAttachmentIncluded)
                {

                    if (objTFireSystemByPassPermit.DrawingAttachments.Count > 0 || objTFireSystemByPassPermit.Attachments.Count > 0)
                    {
                        pdfDoc.NewPage();
                        isattach = true;
                    }

                    if (isattach)
                    {
                        if (objTFireSystemByPassPermit.DrawingAttachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingBefore = 10f
                            };
                            table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
                            for (int i = 0; i < objTFireSystemByPassPermit.DrawingAttachments.Count; i++)
                            {
                                //table.AddCell(AddNewCell(objceilingPermit.DrawingAttachments[i].FullFileName, CellFont));
                                //table.AddCell(AddNewCell("", CellFont));
                                AddAttachmentCell(objTFireSystemByPassPermit.DrawingAttachments[i].ImagePath, objTFireSystemByPassPermit.DrawingAttachments[i].FullFileName, pdfWriter, table);


                            }
                            pdfDoc.Add(table);
                        }
                        if (objTFireSystemByPassPermit.Attachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingBefore = 10f
                            };
                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
                            for (int i = 0; i < objTFireSystemByPassPermit.Attachments.Count; i++)
                            {
                                AddAttachmentCell(objTFireSystemByPassPermit.Attachments[i].FilePath, objTFireSystemByPassPermit.Attachments[i].FileName, pdfWriter, table);

                            }
                            pdfDoc.Add(table);
                        }
                    }

                }



            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        private void CreateApprovalStamp(out PdfPTable stampTable, string Status, string ApproveBy, string Date, TFireSystemByPassPermit objTFireSystemByPassPermit)
        {
            stampTable = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 5f

            };
            float[] widths = new float[] { 25, 300, 25 };
            stampTable.SetTotalWidth(widths);
            stampTable.AddCell(AddNewCell("SECTION 3:  Approval Stamp or Signature/Date  ", CellFontBoldBlueSmall, false, 3));
            stampTable.AddCell(AddNewCell("     ", CellFontBoldBlueSmall, false, 3));
            stampTable.AddCell(" ");
            PdfPTable table2 = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 5f

            };
            //table2.DefaultCell.Border = 1;
            widths = new float[] { 45, 10, 45 };
            table2.SetTotalWidth(widths);
            PdfPCell cell = new PdfPCell
            {
                Border = 0
            };
            cell.Colspan = 3;
            if (!string.IsNullOrEmpty(Status) && Status.ToUpper() == "PENDINGSUBMISSION")
            {
                Status = "PENDING";
            }
            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Status) ? "                " + Status.ToUpper() : " ", CellStatusFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            table2.AddCell(cell);
            PdfPCell cell2 = new PdfPCell();
            cell2.UseVariableBorders = true;
            cell2.BorderColor = BaseColor.BLACK;
            cell2.BorderWidth = 1f;
            cell2.Border = PdfPCell.BOTTOM_BORDER;
            cell2.Colspan = 3;
            table2.AddCell(cell2);
            if (objTFireSystemByPassPermit.ApprovalStatus != 1)
            {
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
            }

            if (objTFireSystemByPassPermit.DSFSBPApproverSign != null && objTFireSystemByPassPermit.DSFSBPApproverSign.DigSignatureId > 0)
            {
                //cell = new PdfPCell()
                //{
                //    Border = 0,
                //};
                //Image image = Image.GetInstance(CommonUtility.FilePath(objTFireSystemByPassPermit.DSFSBPApproverSign.FilePath));
                ////image.ScaleAbsolute(40, 40);
                //cell.AddElement(new Chunk(image, 0, 0));
                //table2.AddCell(cell);

                //cell = new PdfPCell
                //{
                //    Border = 0
                //};
                //cell.Colspan = 2;
                //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSFSBPApproverSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSFSBPApproverSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
                //table2.AddCell(cell);
                if (objTFireSystemByPassPermit.ApprovalStatus == 1)
                {
                    PdfPCell cellmain = new PdfPCell()
                    {
                        Border = 0,
                    };

                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSFSBPApproverSign, string.Empty);
                    cellmain.Colspan = 3;
                    table2.AddCell(cellmain);
                }


            }

            cell2 = new PdfPCell
            {
                BorderWidth = 2f
            };

            cell2.AddElement(table2);
            stampTable.AddCell(cell2);
            stampTable.AddCell(" ");


        }

        #endregion

        #region Method of procedure

        public string MOPPermitInbytes(int mopid)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TMOP mop = _permitService.GetMethodofProcedure(mopid);
            pdfDoc = CreateMOPPermit(mopid, mem, mop, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }
        public Document CreateMOPPermit(int id, Stream streamOutput, TMOP mop, bool IsAttachmentIncluded)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            //
            if (mop != null)
            {
                string approverName = mop.ApprovedByUser != null ? mop.ApprovedByUser.Name : string.Empty;
                if (mop.Status == 2 || mop.Status == -1)
                {
                    SetHeaderBlue(out table, "Method of Procedure (MOP)");
                }
                else
                {
                    SetStatusHeaderBlue(out table, "Method of Procedure (MOP)", mop.Status == 3 ? "Hold" : Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), mop.Status).ToString().ToUpper(), approverName, ($"{mop.ApproveDate:MMM d, yyyy}"));

                }

            }
            else
            {
                SetHeaderBlue(out table, "Method of Procedure (MOP)");
            }
            PdfPTable tableapprove = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };
            pdfDoc.Add(table);

            if (mop != null)
            {
                table = new PdfPTable(6)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f,

                };

                var date = "";
                var startdate = "";
                var enddate = "";
                if (mop.Date.HasValue && mop.Date != null)
                {
                    date = mop.Date.Value.ToString("MMM d, yyyy");
                }
                if (mop.StartDate.HasValue && mop.StartDate != null)
                {
                    startdate = mop.StartDate.Value.ToString("MMM d, yyyy");
                }
                if (mop.EndDate.HasValue && mop.EndDate != null)
                {
                    enddate = mop.EndDate.Value.ToString("MMM d, yyyy");
                }
                float[] widths = new float[] { 15f, 20f, 15f, 30f, 15f, 15f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.PermitNo, CellFont, false));
                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.ProjectName, CellFont, false));
                table.AddCell(AddNewCell("Project #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.Project.ProjectNumber, CellFont, false));

                //table.AddCell(AddNewCell("", CellFont, false, 4));
                //table.AddCell(AddNewCell("", CellFont, false, 4));
                table.AddCell(AddNewCell("Request Date:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(date, CellFont, false));
                table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.Contractor, CellFont, false));
                table.AddCell(AddNewCell("Supervisor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.Supervisor, CellFont, false)); ;
                table.AddCell(AddNewCell("Building(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.BuildingName, CellFont, false));
                table.AddCell(AddNewCell("Floor(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.FloorName, CellFont, false, 3));
                table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(startdate, CellFont, false));
                table.AddCell(AddNewCell("Start Time:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell($"{mop.StartTime:HH:MM}", CellFont, false));
                table.AddCell(AddNewCell("Zone(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.Zones, CellFont, false));
                table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(enddate, CellFont, false));
                table.AddCell(AddNewCell("End Time:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell($"{mop.EndTime:HH:MM}", CellFont, false, 3));
                table.AddCell(AddNewCell("Request By:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.RequestBy != null && mop.RequestByUser != null && mop.RequestByUser.Name != null ? mop.RequestByUser.Name : " ", CellFont, false));
                table.AddCell(AddNewCell("Email Adress:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(mop.EmailAddress, CellFont, false, 3));
                pdfDoc.Add(table);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                PdfPCell nobordercell = new PdfPCell()
                {
                    Border = 0,
                };
                Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                ImgCheckBox.ScaleAbsolute(10f, 10f);

                Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

                //CreateSignSection(out table, string.Empty, mop.DSSign2Signature);
                if (mop.RequestByUser != null && mop.DSSign2Signature != null && mop.DSSign2Signature.DigSignatureId > 0)
                {
                    PdfPCell cellmain = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cellmain = CreateSignSectionCell(string.Empty, mop.DSSign2Signature, string.Empty, 30f);
                    table.AddCell(cellmain);

                }

                //if (mop.DSSign2Signature != null && mop.DSSign2Signature.DigSignatureId > 0)
                //{
                //    PdfPCell cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    Image image = Image.GetInstance(CommonUtility.FilePath(mop.DSSign2Signature.FilePath));
                //    cell.AddElement(new Chunk(image, 0, -10));
                //    cell.PaddingBottom = 0;
                //    cell.PaddingTop = 0;
                //    table.AddCell(cell);
                //    cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //    cell.AddElement(line);
                //    cell.UseAscender = true;
                //    cell.PaddingBottom = 0;
                //    cell.PaddingTop = 0;
                //    table.AddCell(cell);

                //    table.AddCell(AddNewCell(mop.RequesterSignatureId != null && mop.DSSign2Signature != null && mop.DSSign2Signature.SignByUserName != null ? mop.DSSign2Signature.SignByUserName + " ( " + mop.DSSign2Signature.SignByEmail + ")" : " ", CellFontS, false));
                //    table.AddCell(AddNewCell(mop.RequesterSignatureId != null && mop.DSSign2Signature != null && mop.DSSign2Signature.LocalSignDateTime != null ? "(" + mop.DSSign2Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue));
                //}
                //else
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 1));
                //}
                pdfDoc.Add(table);

                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                table.AddCell(AddNewCell("Section 1:  Brief Description of Work", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(mop.Description == "" ? "N/A" : mop.Description, CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell("Section 2: Systems Impacted ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                PdfPTable tbldetail = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0

                };
                widths = new float[] { 10f, 10f, 35f, 45f };
                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
                tbldetail.SetWidths(widths);
                tbldetail.AddCell(new Phrase("Yes", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("No", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Type", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Description", CellFontBoldBlueSmall));
                nobordercell = new PdfPCell();
                for (int i = 0; i < mop.SystemsImpacted.Count; i++)
                {
                    if (mop.SystemsImpacted[i].RespondStatus)
                    {
                        nobordercell = new PdfPCell();
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);

                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell = new PdfPCell();
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);

                    }
                    else
                    {
                        nobordercell = new PdfPCell();
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell = new PdfPCell();
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);
                    }
                    tbldetail.AddCell(new Phrase(mop.SystemsImpacted[i].AdditionalForms.FormName, CellFont));
                    tbldetail.AddCell(new Phrase(mop.SystemsImpacted[i].AdditionalForms.Description, CellFont));
                }

                PdfPCell cellsystemimacted = new PdfPCell()
                {
                    Border = 0
                };

                cellsystemimacted.AddElement(tbldetail);
                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(cellsystemimacted);
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell("Section 3:  Additional Forms Required ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                tbldetail = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0

                };
                widths = new float[] { 10f, 10f, 35f, 45f };
                tbldetail.SetWidths(widths);
                tbldetail.AddCell(new Phrase("Yes", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("No", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Form", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Description", CellFontBoldBlueSmall));
                nobordercell = new PdfPCell();
                for (int i = 0; i < mop.AdditionalForms.Count; i++)
                {
                    if (mop.AdditionalForms[i].RespondStatus)
                    {
                        nobordercell = new PdfPCell();
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);

                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell = new PdfPCell();
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);

                    }
                    else
                    {
                        nobordercell = new PdfPCell();
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell = new PdfPCell();
                        nobordercell.AddElement(chunk1);
                        tbldetail.AddCell(nobordercell);
                    }
                    tbldetail.AddCell(new Phrase(mop.AdditionalForms[i].AdditionalForms.FormName, CellFont));
                    tbldetail.AddCell(new Phrase(mop.AdditionalForms[i].AdditionalForms.Description, CellFont));
                }

                cellsystemimacted = new PdfPCell()
                {
                    Border = 0
                };
                cellsystemimacted.AddElement(tbldetail);
                table.AddCell(cellsystemimacted);
                table.AddCell(AddNewCell("Section 4: Step by Step Procedure [Discuss, in technical detail, the work to be performed – include times]", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(mop.ProcedureSteps == "" ? "N/A" : mop.ProcedureSteps, CellFont, false));
                table.AddCell(AddNewCell("Section 5: System Impact Area(s) [List all areas that will be affected by this project] # ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                tbldetail = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0

                };
                widths = new float[] { 10f, 90f };
                tbldetail.SetWidths(widths);
                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
                tbldetail.AddCell(new Phrase("#", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Department / Floor / Zone / Room ", CellFontBoldBlueSmall));

                nobordercell = new PdfPCell();
                for (int i = 0; i < mop.SystemImpactArea.Count; i++)
                {
                    tbldetail.AddCell(new Phrase(Convert.ToString(i + 1), CellFont));
                    tbldetail.AddCell(new Phrase(mop.SystemImpactArea[i].AreaName, CellFont));

                }

                cellsystemimacted = new PdfPCell()
                {
                    Border = 0
                };
                cellsystemimacted.AddElement(tbldetail);
                table.AddCell(cellsystemimacted);
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" Section 6: Project Contact List [provide off-shift contacts, if applicable] ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                tbldetail = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                widths = new float[] { 10f, 30f, 30f, 30f };
                tbldetail.SetWidths(widths);
                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
                tbldetail.AddCell(new Phrase("#", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Name", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Phone", CellFontBoldBlueSmall));
                tbldetail.AddCell(new Phrase("Email", CellFontBoldBlueSmall));
                nobordercell = new PdfPCell();
                for (int i = 0; i < mop.ProjectContactList.Count; i++)
                {
                    tbldetail.AddCell(Convert.ToString(i + 1));
                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].Name == "" ? " " : mop.ProjectContactList[i].Name, CellFont));
                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].Phone == "" ? " " : mop.ProjectContactList[i].Phone, CellFont));
                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].EmailAddress == "" ? " " : mop.ProjectContactList[i].EmailAddress, CellFont));
                }
                cellsystemimacted = new PdfPCell()
                {
                    Border = 0
                };
                cellsystemimacted.AddElement(tbldetail);
                table.AddCell(cellsystemimacted);
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell("Section 7: Follow-Up Required", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(mop.RequiredFollowUp == "" ? "N/A" : mop.RequiredFollowUp, CellFont, false));
                table.AddCell(AddNewCell("Section 8:  Notifications Necessary ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(mop.NotificationsNecessary == "" ? "N/A" : mop.NotificationsNecessary, CellFont, false));
                table.AddCell(AddNewCell("Section 9:  Additional Comments, Questions, and/or Concerns ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(mop.AdditionalComments == "" ? "N/A" : mop.AdditionalComments, CellFont, false));
                table.AddCell(AddNewCell("Section 9:  Approval ", CellFontBoldBlueSmall, false));

                if (mop.Status == 2)
                {
                    table.AddCell(AddNewCell("Status:Requested", CellFont, false));

                }
                else if (mop.Status == 1)
                {
                    table.AddCell(AddNewCell("Status:Approved", CellFont, false));
                    //table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
                    //table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
                }
                else if (mop.Status == 3)
                {
                    var label = "Reason(s) for Hold/Pending: ";
                    var labelstatus = "Status:Hold ";
                    table.AddCell(AddNewCell(labelstatus, CellFont, false));
                    table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
                    table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
                    table.AddCell(AddNewCell(label + mop.RejectReason, CellFont, false));
                }
                else
                {
                    var label = mop.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
                    var labelstatus = mop.Status == 0 ? "Status:Rejected " : "Status:Pending ";
                    table.AddCell(AddNewCell(labelstatus, CellFont, false));
                    table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
                    table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
                    table.AddCell(AddNewCell(label + mop.RejectReason, CellFont, false));
                }

                pdfDoc.Add(table);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0
                };
                if (mop.Status == 1)
                {
                    //if (mop.DSSign1Signature != null && mop.DSSign1Signature.DigSignatureId > 0)
                    //{
                    //    PdfPCell cell = new PdfPCell()
                    //    {
                    //        Border = 0,
                    //    };
                    //    Image image = Image.GetInstance(CommonUtility.FilePath(mop.DSSign1Signature.FilePath));
                    //    //image.ScaleAbsolute(80, 80);
                    //    cell.AddElement(new Chunk(image, 0, 0));
                    //    table.AddCell(cell);
                    //    //cell = new PdfPCell()
                    //    //{
                    //    //    Border = 0,
                    //    //};
                    //    //cell.AddElement(new Phrase(mop.DSSign1Signature.SignByUserName + " (" + mop.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    //    //table.AddCell(cell);

                    //    table.AddCell(AddNewCell(mop.ApproverSignatureId != null && mop.DSSign1Signature != null && mop.DSSign1Signature.SignByUserName != null ? mop.DSSign1Signature.SignByUserName + " ( " + mop.DSSign1Signature.SignByEmail + ")" : " ", CellFontS, false));
                    //    table.AddCell(AddNewCell(mop.ApproverSignatureId != null && mop.DSSign1Signature != null && mop.DSSign1Signature.LocalSignDateTime != null ? "(" + mop.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue));
                    //}
                    //else
                    //{
                    //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                    //}

                    //CreateSignSection(out table,string.Empty, mop.DSSign1Signature);

                    if (mop.ApprovedByUser != null && mop.DSSign1Signature != null && mop.DSSign1Signature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = CreateSignSectionCell(string.Empty, mop.DSSign1Signature, string.Empty, 30f);
                        table.AddCell(cellmain);

                    }
                }
                pdfDoc.Add(table);
            }

            bool isattach = false;
            if (IsAttachmentIncluded)
            {
                if (mop.DrawingAttachments.Count > 0 || mop.Attachments.Count > 0)
                {
                    pdfDoc.NewPage();
                    isattach = true;
                }
                if (isattach)
                {
                    if (mop.DrawingAttachments.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 10f
                        };
                        table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
                        for (int i = 0; i < mop.DrawingAttachments.Count; i++)
                        {

                            AddAttachmentCell(mop.DrawingAttachments[i].ImagePath, mop.DrawingAttachments[i].FullFileName, pdfWriter, table);

                        }
                        pdfDoc.Add(table);
                    }
                    if (mop.Attachments.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 10f
                        };

                        table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
                        for (int i = 0; i < mop.Attachments.Count; i++)
                        {

                            AddAttachmentCell(mop.Attachments[i].FilePath, mop.Attachments[i].FileName, pdfWriter, table);
                        }
                        pdfDoc.Add(table);
                    }
                }



            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        #endregion

        #region HWP

        public string HWPPermitInbytes(int? thotworkpermitid)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            THotWorkPermits objhotWorkPermits = _hotWorkPermitService.GetTHotWorksPermit(thotworkpermitid.Value);
            pdfDoc = CreateHWPPermit(thotworkpermitid.Value, mem, objhotWorkPermits, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }
        public Document CreateHWPPermit(int id, Stream streamOutput, THotWorkPermits objhotWorkPermits, bool IsAttachmentIncluded)
        {

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;

            if (objhotWorkPermits != null)
            {
                string approverName = objhotWorkPermits.AuthorizedByUser != null ? objhotWorkPermits.AuthorizedByUser.Name : string.Empty;
                if (objhotWorkPermits.Status == 2 || objhotWorkPermits.Status == -1)
                {
                    SetHeaderBlue(out table, "Hot Work Permit (HWP)");
                }
                else
                {
                    SetStatusHeaderBlue(out table, "Hot Work Permit (HWP)", Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), objhotWorkPermits.Status).ToString().ToUpper(), approverName, ($"{objhotWorkPermits.PermitAuthorizedByDate:MMM d, yyyy}"));

                }

                pdfDoc.Add(table);
                {

                    table = new PdfPTable(6)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,

                    };
                    float[] widths = new float[] { 19f, 18f, 19f, 16f, 15f, 13f };
                    table.SetTotalWidth(widths);
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.PermitNo, CellFont, false));
                    table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.ProjectName, CellFont, false));
                    table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(($"{objhotWorkPermits.StartDate:MMM d,yyyy}"), CellFont, false));
                    if (objhotWorkPermits.StartTime != null)
                    {
                        if (!string.IsNullOrEmpty(objhotWorkPermits.StartTime.ToString()))
                        {
                            objhotWorkPermits.StartTimeVal = TimeSpan.Parse(objhotWorkPermits.StartTime.ToString());
                        }

                        objhotWorkPermits.StartTime = objhotWorkPermits.StartTime.ToString();
                        if (objhotWorkPermits.StartTimeVal.HasValue)
                        {
                            DateTime starttime = DateTime.Today.Add(objhotWorkPermits.StartTimeVal.Value);
                            objhotWorkPermits.StartTime = starttime.ToString("hh:mm tt");

                        }

                    }

                    if (objhotWorkPermits.EndTime != null)
                    {
                        if (!string.IsNullOrEmpty(objhotWorkPermits.EndTime.ToString()))
                        {
                            objhotWorkPermits.EndTimeVal = TimeSpan.Parse(objhotWorkPermits.EndTime.ToString());
                        }

                        objhotWorkPermits.EndTime = objhotWorkPermits.EndTime.ToString();
                        if (objhotWorkPermits.EndTimeVal.HasValue)
                        {
                            DateTime endttime = DateTime.Today.Add(objhotWorkPermits.EndTimeVal.Value);
                            objhotWorkPermits.EndTime = endttime.ToString("hh:mm tt");

                        }


                    }
                    table.AddCell(AddNewCell("Start Time:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.StartTime, CellFont, false));
                    table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(($"{objhotWorkPermits.EndDate:MMM d,yyyy}"), CellFont, false));
                    table.AddCell(AddNewCell("End Time:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.EndTime, CellFont, false));
                    table.AddCell(AddNewCell("Permit Requestor :", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.RequestorByUser != null && !string.IsNullOrEmpty(objhotWorkPermits.RequestorByUser.Name) ? objhotWorkPermits.RequestorByUser.Name : "", CellFont, false, 3));

                    table.AddCell(AddNewCell("Organization:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.Company) ? objhotWorkPermits.Company : string.Empty, CellFont, false));
                    table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.EmailAddress) ? objhotWorkPermits.EmailAddress : string.Empty, CellFont, false, 3));
                    table.AddCell(AddNewCell("Phone Number :", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.PhoneNumber) ? objhotWorkPermits.PhoneNumber : string.Empty, CellFont, false));
                    table.AddCell(AddNewCell("On-Site Contact:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.OnSiteContact, CellFont, false, 3));
                    table.AddCell(AddNewCell("On-Site Phone:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.OnSitePhone, CellFont, false));
                    table.AddCell(AddNewCell("Building:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.BuildingName, CellFont, false, 3));
                    table.AddCell(AddNewCell("Floor:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.FloorName, CellFont, false));
                    table.AddCell(AddNewCell("Zone:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.Zones, CellFont, false));
                    table.AddCell(AddNewCell("Room:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.Rooms, CellFont, false));
                    table.AddCell(AddNewCell("Types of Work:", CellFontBoldBlack, false, 6));
                    int TotalColumn = objhotWorkPermits.ConstructionWorkType.Count() + objhotWorkPermits.ConstructionWorkType.Count();
                    PdfPTable tableWork = new PdfPTable(TotalColumn)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };
                    List<float> list = new List<float>();



                    for (int i = 0; i < TotalColumn; i++)
                    {
                        if (i == 7)
                            list.Add(30f);
                        else if (i == 9)
                            list.Add(15f);
                        else if (i % 2 == 0)
                            list.Add(5f);
                        else
                            list.Add(12f);

                    }
                    widths = list.ToArray();
                    tableWork.SetTotalWidth(widths);
                    tableWork.DefaultCell.Border = Rectangle.NO_BORDER;

                    Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                    ImgCheckBox.ScaleAbsolute(10f, 10f);

                    Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


                    PdfPCell nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };

                    for (int i = 0; i < objhotWorkPermits.ConstructionWorkType.Count(); i++)
                    {
                        //if (i % 3 == 0 && i != 0)
                        //{
                        //    tableWork.AddCell(AddNewCell("", CellFontS, false, objhotWorkPermits.ConstructionWorkType.Count() - 3));
                        //}
                        nobordercell = new PdfPCell()
                        {
                            Border = 0,
                        };
                        if (objhotWorkPermits.ConstructionWorkType[i].IsChecked == 1)
                        {
                            Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
                            nobordercell.AddElement(chunk1);
                            tableWork.AddCell(nobordercell);

                        }
                        else
                        {
                            Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
                            nobordercell.AddElement(chunk1);
                            tableWork.AddCell(nobordercell);
                        }
                        tableWork.AddCell(AddNewCell(objhotWorkPermits.ConstructionWorkType[i].Name, CellFontS, false));

                    }

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    nobordercell.Colspan = 6;
                    nobordercell.AddElement(tableWork);
                    table.AddCell(nobordercell);
                    table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(objhotWorkPermits.Description, CellFont, false, 5));
                    //table.AddCell(AddNewCell("Attach Drawings:", CellFontBoldBlack, false));
                    //foreach (var drawings in objhotWorkPermits.DrawingAttachments)
                    //{
                    //    table.AddCell(AddNewCell(drawings.FullFileName, CellFont, false, 5));
                    //    table.AddCell(AddNewCell("", CellFont, false));
                    //}

                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
                    table.AddCell(AddNewCell("ITEMS REQUIRED TO PERFORM HOT WORK:", CellFontBoldBlack, false, 8));
                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
                    tableWork = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };
                    widths = new float[] { 30f, 70f };
                    tableWork.SetTotalWidth(widths);
                    string text = "";
                    for (var i = 0; i < objhotWorkPermits.THotWorkItems.OrderBy(x => x.ItemId).Count(); i++)
                    {
                        text = "";
                        if (objhotWorkPermits.THotWorkItems[i].ParentId == 0)
                        {
                            tableWork.AddCell(new Phrase(objhotWorkPermits.THotWorkItems[i].Item, CellFontBoldBlack));



                            foreach (var childitem in objhotWorkPermits.THotWorkItems.Where(x => x.ParentId == objhotWorkPermits.THotWorkItems[i].ItemId))
                            {
                                text += "   • " + childitem.Item;

                            }
                            tableWork.AddCell(new Phrase(text, CellFont));
                        }
                    }
                    PdfPCell cell1 = new PdfPCell();
                    cell1.Colspan = 8;
                    cell1.AddElement(tableWork);
                    table.AddCell(cell1);
                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
                    tableWork = new PdfPTable(3)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };
                    widths = new float[] { 15f, 5f, 80f };
                    tableWork.SetTotalWidth(widths);

                    PdfPCell cellWithRowspan = new PdfPCell(new Phrase("Agreement", CellRedtext));
                    cellWithRowspan.Rowspan = 3;
                    tableWork.AddCell(cellWithRowspan);
                    nobordercell = new PdfPCell();
                    if (objhotWorkPermits.IsVerifyHotWorkPerformed == true)
                    {
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);

                    }
                    else
                    {
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);
                    }
                    tableWork.AddCell(new Phrase("I verify that the following steps at the location where the hot work is being performed and adjacent areas have been inspected and applicable precautions have been checked and completed as necessary.", CellRedtext));

                    nobordercell = new PdfPCell();
                    if (objhotWorkPermits.IsVerifyObservedrevisited == true)
                    {
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);

                    }
                    else
                    {
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);
                    }
                    tableWork.AddCell(new Phrase("I verify the area was observed for thirty [30] minutes and then revisited one [1] hour, and that the Hot Work Permit was returned to Engineering at the completion of work", CellRedtext));

                    nobordercell = new PdfPCell();
                    if (objhotWorkPermits.IsVerifyAttach == true)
                    {
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);

                    }
                    else
                    {
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tableWork.AddCell(nobordercell);
                    }
                    tableWork.AddCell(new Phrase("I verify the Hot Work Notice [attached] is posted at the jobsite. ", CellRedtext));
                    nobordercell = new PdfPCell();
                    nobordercell.Colspan = 6;
                    nobordercell.AddElement(tableWork);
                    table.AddCell(nobordercell);
                    table.AddCell(AddNewCell("  ", CellFontBoldBlack, false, 6));
                    pdfDoc.Add(table);
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,



                    };
                    widths = new float[] { 50f, 50f };
                    table.SetTotalWidth(widths);
                    int twocolcount = 0;
                    if (objhotWorkPermits.PermitRequestor != null && objhotWorkPermits.DSSign1Signature != null && objhotWorkPermits.DSSign1Signature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain1 = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain1 = CreateSignSectionCell("Permit Request By:", objhotWorkPermits.DSSign1Signature, string.Empty);
                        table.AddCell(cellmain1);
                        twocolcount++;

                    }

                    if (twocolcount == 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                    PdfPTable tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,



                    };
                    widths = new float[] { 30f, 70f };
                    tableinner.SetTotalWidth(widths);
                    PdfPCell cellinner = new PdfPCell()
                    {
                        Border = 0,
                    };
                    tableinner.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    if (objhotWorkPermits.Status == 2)
                    {
                        tableinner.AddCell(AddNewCell("Requested", CellFont, false));

                    }
                    else if (objhotWorkPermits.Status == 1)
                    {
                        tableinner.AddCell(AddNewCell("Approved", CellFont, false));
                    }
                    else
                    {
                        var labelstatus = objhotWorkPermits.Status == 0 ? "Rejected " : "Pending ";
                        tableinner.AddCell(AddNewCell(labelstatus, CellFont, false));

                        var label = objhotWorkPermits.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
                        tableinner.AddCell(AddNewCell(label, CellFontBoldBlack, false));
                        tableinner.AddCell(AddNewCell(objhotWorkPermits.Reason, CellFont, false));
                    }

                    //cellinner.AddElement(tableinner);
                    //table.AddCell(cellinner);
                    PdfPTable tableinner1 = new PdfPTable(1)
                    {
                        HorizontalAlignment = 0

                    };
                    widths = new float[] { 100f };
                    tableinner1.SetTotalWidth(widths);

                    cellinner = new PdfPCell()
                    {
                        Border = 0,
                        PaddingLeft = 0

                    };

                    if (objhotWorkPermits.Status == 1)
                    {
                        if (objhotWorkPermits.AuthorizedByUser != null && objhotWorkPermits.DSSign2Signature != null && objhotWorkPermits.DSSign2Signature.DigSignatureId > 0)
                        {
                            PdfPCell cellmain1 = new PdfPCell()
                            {
                                Border = 0,
                            };
                            cellmain1 = CreateSignSectionCell("Approve By:", objhotWorkPermits.DSSign2Signature, string.Empty);
                            tableinner1.AddCell(cellmain1);
                        }

                    }
                    cellinner.Colspan = 2;
                    cellinner.AddElement(tableinner1);
                    tableinner.AddCell(cellinner);


                    cellinner = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cellinner.AddElement(tableinner);
                    table.AddCell(cellinner);
                    pdfDoc.Add(table);


                    bool isattach = false;
                    if (IsAttachmentIncluded)
                    {

                        if (objhotWorkPermits.DrawingAttachments.Count > 0)
                        {
                            pdfDoc.NewPage();
                            isattach = true;
                        }
                        if (isattach)
                        {
                            if (objhotWorkPermits.DrawingAttachments.Count > 0)
                            {
                                table = new PdfPTable(1)
                                {
                                    WidthPercentage = 100,
                                    HorizontalAlignment = 0,
                                    SpacingBefore = 10f
                                };
                                table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
                                for (int i = 0; i < objhotWorkPermits.DrawingAttachments.Count; i++)
                                {

                                    AddAttachmentCell(objhotWorkPermits.DrawingAttachments[i].ImagePath, objhotWorkPermits.DrawingAttachments[i].FullFileName, pdfWriter, table);

                                }
                                pdfDoc.Add(table);
                            }
                        }

                    }

                    string Noticetext = _organizationService.GetOrganization().NoticeText;
                    if (!string.IsNullOrEmpty(Noticetext))
                    {
                        pdfDoc.SetPageSize(PageSize.A4.Rotate());
                        pdfDoc.NewPage();

                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0

                        };


                        Paragraph para = new Paragraph("NOTICE", TitleFontHWP)
                        {
                            Alignment = Element.ALIGN_CENTER,



                        };
                        nobordercell = new PdfPCell()
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = 0,
                            PaddingBottom = 15f,
                            PaddingTop = -25f,
                            PaddingLeft = -25f,
                            PaddingRight = -25f


                        };
                        nobordercell.BackgroundColor = BaseColor.BLACK;
                        nobordercell.AddElement(para);
                        table.AddCell(nobordercell);


                        para = new Paragraph("HOT WORK IN PROGRESS", TitleFont2HWP)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        nobordercell = new PdfPCell()
                        {
                            HorizontalAlignment = Element.ALIGN_MIDDLE,
                            Border = 0
                        };
                        nobordercell.BackgroundColor = BaseColor.WHITE;
                        nobordercell.AddElement(para);
                        table.AddCell(nobordercell);
                        table.AddCell(AddNewCell(" \n", CellFontBoldBlueSmall, false));

                        PdfPCell tableCell = new PdfPCell()
                        {
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL
                        };

                        string styles = "h2 ,p { text-align: center;font-size:21px; }";
                        foreach (IElement element in XMLWorkerHelper.ParseToElementList(Noticetext, styles))
                        {
                            tableCell.AddElement(element);
                        }
                        table.AddCell(tableCell);
                        pdfDoc.Add(table);
                    }

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                }
            }
            return pdfDoc;
        }
        #endregion

        #region FMC

        public string FMCPermitInbytes(int? id)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TFacilitiesMaintenanceOccurrencePermit objTFMC = new TFacilitiesMaintenanceOccurrencePermit();
            objTFMC = _permitService.GetFacilitiesMaintenanceOccurrence(id.Value);

            pdfDoc = CreateFMCPermit(id.Value, mem, objTFMC);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        public Document CreateFMCPermit(int id, Stream streamOutput, TFacilitiesMaintenanceOccurrencePermit objFMC)
        {

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 26);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;

            if (objFMC != null)
            {
                // SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form");
                string approverName = objFMC.AuthorizedByUser != null ? objFMC.AuthorizedByUser.Name : string.Empty;
                SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)");
                //if (objFMC.Status == 2)
                //{
                //    SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)");
                //}
                //else
                //{
                //    SetStatusHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)", Enum.GetName(typeof(Enums.ApprovalStatus), objFMC.Status).ToString().ToUpper(), approverName, ($"{objFMC.ApprovedDate:MMM d, yyyy}"));

                //}

                pdfDoc.Add(table);
                {

                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f,

                    };
                    PdfPTable tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    float[] widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell cellelement = new PdfPCell();
                    Paragraph para = new Paragraph("ORIGINATOR", UnderlineTitleBoldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    cellelement.AddElement(para);
                    cellelement.Colspan = 1;
                    cellelement.AddElement(new Paragraph(" ", UnderlineCellFont));
                    cellelement.BackgroundColor = Gray;
                    table.AddCell(cellelement);
                    tableinner.AddCell(new Phrase("Report #:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(string.Empty, UnderlineCellFont));
                    tableinner.AddCell(new Phrase(objFMC.PermitNo, CellFont));
                    tableinner.AddCell(new Phrase(string.Empty, CellFontBoldBlack));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Name:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase("Asset ID(if applicable):", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(objFMC.Name, CellFont));
                    tableinner.AddCell(new Phrase(objFMC.AssetId.ToString(), CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Position:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase("Department:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(objFMC.Position, CellFont));
                    tableinner.AddCell(new Phrase(objFMC.DepartmentName, CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Phone:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase("Shift:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(objFMC.PhoneNo, CellFont));
                    tableinner.AddCell(new Phrase(objFMC.Shift, CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Date/Time Occurrence:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase("Report Date:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(((objFMC.DateOfOccurence == null || objFMC.DateOfOccurence.ToString() == "") && (objFMC.TimeOfOccurence == null || objFMC.TimeOfOccurence == "") ? "" : $"{objFMC.DateOfOccurence:MMM d, yyyy}" + " @ " + $"{objFMC.TimeOfOccurence:HH:MM}"), CellFont));
                    tableinner.AddCell(new Phrase($"{objFMC.ReportDate:MMM d, yyyy}", CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Organization:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(string.Empty, UnderlineCellFont));
                    tableinner.AddCell(new Phrase(objFMC.Company, CellFont));
                    tableinner.AddCell(new Phrase(string.Empty, CellFontBoldBlack));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Drawings Attached:", UnderlineCellBoldFont));
                    foreach (var drawings in objFMC.DrawingAttachments)
                    {
                        tableinner.AddCell(new Phrase(drawings.FullFileName, CellFont));
                    }
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    cellelement = new PdfPCell();
                    para = new Paragraph("DESCRIPTION", UnderlineTitleBoldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    cellelement.Colspan = 1;
                    cellelement.AddElement(para);
                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
                    cellelement.BackgroundColor = Gray;
                    table.AddCell(cellelement);
                    cellelement = new PdfPCell();
                    cellelement.AddElement(new Phrase("Classification", UnderlineCellBoldFont));
                    table.AddCell(cellelement);
                    int TotalColumn = objFMC.lstClassificationType.Count() + objFMC.lstClassificationType.Count();
                    PdfPTable tableWork = new PdfPTable(10)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };
                    widths = new float[] { 5f, 15f, 5f, 15f, 5f, 15f, 5f, 15f, 5f, 15f };
                    tableWork.SetTotalWidth(widths);
                    tableWork.DefaultCell.Border = Rectangle.NO_BORDER;
                    Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                    ImgCheckBox.ScaleAbsolute(10f, 10f);

                    Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


                    PdfPCell nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };

                    for (int i = 0; i < objFMC.lstClassificationType.Count(); i++)
                    {

                        nobordercell = new PdfPCell()
                        {
                            Border = 0,
                        };
                        if (objFMC.lstClassificationType[i].IsChecked == 1)
                        {
                            Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
                            nobordercell.AddElement(chunk1);
                            tableWork.AddCell(nobordercell);

                        }
                        else
                        {
                            Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
                            nobordercell.AddElement(chunk1);
                            tableWork.AddCell(nobordercell);
                        }
                        tableWork.AddCell(AddNewCell(objFMC.lstClassificationType[i].Name, CellFontS, false));

                    }

                    nobordercell = new PdfPCell();
                    nobordercell.Colspan = 4;
                    nobordercell.AddElement(tableWork);
                    table.AddCell(nobordercell);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Location:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(objFMC.Location, CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 50f, 50f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase("Describe Occurrence:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(objFMC.OccurenceDetails, CellFont));
                    tableinner.AddCell(new Phrase(" ", CellFont));

                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);
                    cellelement = new PdfPCell();
                    para = new Paragraph("ACTION TAKEN – RESOLUTIONS", UnderlineCellBoldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    cellelement.Colspan = 1;
                    cellelement.AddElement(para);
                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
                    cellelement.BackgroundColor = Gray;
                    table.AddCell(cellelement);

                    tableinner = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(new Phrase(!string.IsNullOrEmpty(objFMC.ActionTaken) ? objFMC.ActionTaken : string.Empty, CellFont));
                    cellelement = new PdfPCell
                    {
                        Colspan = 1
                    };
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);

                    cellelement = new PdfPCell();
                    para = new Paragraph("COMMENTS", UnderlineCellBoldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    cellelement.Colspan = 1;
                    cellelement.AddElement(para);
                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
                    cellelement.BackgroundColor = Gray;
                    table.AddCell(cellelement);
                    tableinner = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    cellelement = new PdfPCell();
                    tableinner.AddCell(new Phrase("Comments:", UnderlineCellBoldFont));
                    tableinner.AddCell(new Phrase(!string.IsNullOrEmpty(objFMC.Comments) ? objFMC.Comments : string.Empty, CellFont));
                    cellelement = new PdfPCell();
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);

                    //table.AddCell(AddNewCell("Location:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(objFMC.Location, CellFont, false));
                    //table.AddCell(AddNewCell("Describe Occurrence:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(objFMC.OccurenceDetails, CellFont, false));
                    //table.AddCell(AddNewCell("Action Taken:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(objFMC.ActionTaken, CellFont, false));
                    //table.AddCell(AddNewCell("Comments:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(objFMC.Comments, CellFont, false));

                    tableinner = new PdfPTable(4)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingAfter = 20f,

                    };
                    widths = new float[] { 25f, 25f, 25f, 25f };
                    tableinner.SetTotalWidth(widths);
                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
                    tableinner.AddCell(AddNewCell("Completely Resolved :", UnderlineCellBoldFont));
                    tableinner.AddCell(AddNewCell((objFMC.CompletelyResolved == 0) ? "No" : "Yes", CellFont));
                    tableinner.AddCell(AddNewCell("Date :", UnderlineCellBoldFont));
                    tableinner.AddCell(AddNewCell(($"{objFMC.CompleteDate:MMM d, yyyy}"), CellFont));
                    //tableinner.AddCell(new Phrase((objFMC.CompletelyResolved == 0) ? "No" : "Yes", CellFont));
                    //tableinner.AddCell(new Phrase(($"{objFMC.CompleteDate:MMM d, yyyy}"), CellFont));
                    cellelement = new PdfPCell();


                    PdfPTable commentBo = new PdfPTable(4)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };

                    ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                    ImgCheckBox.ScaleAbsolute(10f, 10f);

                    ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };

                    PdfPTable tablechild = new PdfPTable(4)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0

                    };
                    widths = new float[] { 5f, 25f, 5f, 65f };
                    tablechild.SetWidths(widths);

                    if (objFMC.AddedToEOC)
                    {
                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tablechild.AddCell(nobordercell);
                        tablechild.AddCell(AddNewCell("Added to EOC Dashboard", CellFont, false));
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell = new PdfPCell()
                        {
                            Border = 0,
                        };
                        nobordercell.AddElement(chunk1);
                        tablechild.AddCell(nobordercell);
                        tablechild.AddCell(AddNewCell("Do not include in EOC Dashboard ", CellFont, false));
                    }
                    else
                    {
                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
                        nobordercell.AddElement(chunk1);
                        tablechild.AddCell(nobordercell);
                        tablechild.AddCell(AddNewCell("Added to EOC Dashboard", CellFont, false));
                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
                        nobordercell = new PdfPCell()
                        {
                            Border = 0,
                        };
                        nobordercell.AddElement(chunk1);
                        tablechild.AddCell(nobordercell);
                        tablechild.AddCell(AddNewCell("Do not include in EOC Dashboard ", CellFont, false));
                    }
                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    nobordercell.AddElement(tablechild);
                    nobordercell.Colspan = 4;
                    tableinner.AddCell(nobordercell);
                    cellelement.AddElement(tableinner);
                    table.AddCell(cellelement);

                    //table.AddCell(AddNewCell("Permit Requestor:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(objFMC.PermitRequestUser != null && !string.IsNullOrEmpty(objFMC.PermitRequestUser.Name) ? objFMC.PermitRequestUser.Name : "", CellFont, false));
                    //table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
                    //table.AddCell(AddNewCell(($"{objFMC.RequesterDate:MMM d, yyyy}"), CellFont, false));
                    //if (objFMC.DSSign1Signature != null && objFMC.DSSign1Signature.DigSignatureId > 0)
                    //{
                    //    table.AddCell(AddNewCell("  ", CellFontBoldBlack, false));
                    //    PdfPCell cell = new PdfPCell()
                    //    {
                    //        Border = 0,
                    //    };
                    //    Image image = Image.GetInstance(CommonUtility.FilePath(objFMC.DSSign1Signature.FilePath));
                    //    image.ScaleAbsolute(60, 60);
                    //    cell.AddElement(image);
                    //    cell.Colspan = 3;
                    //    table.AddCell(cell);

                    //}
                    //else
                    //{
                    //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
                    //}

                    //table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    //if (objFMC.Status == 2)
                    //{
                    //    table.AddCell(AddNewCell("Requested", CellFont, false, 3));

                    //}
                    //else if (objFMC.Status == 1)
                    //{
                    //    table.AddCell(AddNewCell("Approved", CellFont, false, 3));
                    //    table.AddCell(AddNewCell("Approve By:", CellFontBoldBlack, false));
                    //    table.AddCell(AddNewCell(objFMC.AuthorizedByUser != null && objFMC.AuthorizedByUser.Name != null ? objFMC.AuthorizedByUser.Name : " ", CellFont, false));
                    //    table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
                    //    table.AddCell(AddNewCell(($"{objFMC.ApprovedDate:MMM d, yyyy}"), CellFont, false));
                    //}
                    //else
                    //{
                    //    var label = objFMC.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
                    //    var labelstatus = objFMC.Status == 0 ? "Rejected " : "Pending ";
                    //    table.AddCell(AddNewCell(labelstatus, CellFont, false, 3));
                    //    table.AddCell(AddNewCell("Approve By:", CellFontBoldBlack, false));
                    //    table.AddCell(AddNewCell(objFMC.AuthorizedByUser != null && objFMC.AuthorizedByUser.Name != null ? objFMC.AuthorizedByUser.Name : " ", CellFont, false));
                    //    table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
                    //    table.AddCell(AddNewCell(($"{objFMC.ApprovedDate:MMM d, yyyy}"), CellFont, false));
                    //    table.AddCell(AddNewCell(label, CellFontBoldBlack, false));
                    //    table.AddCell(AddNewCell(objFMC.Reason, CellFont, false, 3));
                    //}
                    //if (objFMC.Status == 1)
                    //{
                    //    if (objFMC.DSSign2Signature != null && objFMC.DSSign2Signature.DigSignatureId > 0)
                    //    {
                    //        table.AddCell(AddNewCell("  ", CellFontBoldBlack, false));
                    //        PdfPCell cell = new PdfPCell()
                    //        {
                    //            Border = 0,
                    //        };
                    //        cell.Colspan = 3;
                    //        Image image = Image.GetInstance(CommonUtility.FilePath(objFMC.DSSign2Signature.FilePath));
                    //        image.ScaleAbsolute(80, 80);
                    //        cell.AddElement(image);
                    //        table.AddCell(cell);

                    //    }
                    //    else
                    //    {
                    //        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
                    //    }
                    //}
                    //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
                    pdfDoc.Add(table);
                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                }
            }
            return pdfDoc;
        }

        #endregion

        #region PCRA
        public void PrintPermit(TIcraLog objTIcraLog, Document pdfDoc, TPCRAQuestion objQuestionTPCRA)
        {
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f
            };
            BDO.Organization CurrentOrg = _organizationService.GetOrganizations().FirstOrDefault(x => x.ClientNo ==
             Convert.ToInt32(_session.ClientNo));
            cell = new PdfPCell(new Phrase(!string.IsNullOrEmpty(CurrentOrg.Name) ? CurrentOrg.Name : string.Empty + " Infection Control Construction Permit", TitleFontS))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
            pdfDoc.Add(table);

            if (objQuestionTPCRA != null)
            {


                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };
                float[] width = new float[] { 5f, 35f, 5f, 55f };
                table.SetTotalWidth(width);
                objQuestionTPCRA.CheckBoxRiskAssesmentType = new List<BDO.Enums.EnumModel>();
                foreach (HCF.BDO.Enums.RiskAssesmentType RiskAssesmentType in Enum.GetValues(typeof(BDO.Enums.RiskAssesmentType)))
                {
                    objQuestionTPCRA.CheckBoxRiskAssesmentType.Add(new BDO.Enums.EnumModel() { RiskAssesmentType = RiskAssesmentType, IsSelected = objQuestionTPCRA.RiskAssessmentType == (int)RiskAssesmentType ? true : false });
                }
                Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                ImgCheckBox.ScaleAbsolute(10f, 10f);

                Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

                PdfPCell nobordercell = new PdfPCell();
                Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
                for (int i = 0; i < objQuestionTPCRA.CheckBoxRiskAssesmentType.Count; i++)
                {

                    BDO.Enums.RiskAssesmentType enums = (BDO.Enums.RiskAssesmentType)objQuestionTPCRA.CheckBoxRiskAssesmentType[i].RiskAssesmentType;
                    nobordercell = new PdfPCell() { Border = 0, };
                    if (objQuestionTPCRA.CheckBoxRiskAssesmentType[i].IsSelected)
                    {
                        chunk1 = new Chunk(ImgCheckBox, 0, 2);
                        nobordercell.AddElement(chunk1);
                        table.AddCell(nobordercell);
                        table.AddCell(AddNewCell(enums.GetDescription(), CellFontS, false));
                    }
                    else
                    {
                        chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
                        nobordercell.AddElement(chunk1);
                        table.AddCell(nobordercell);
                        table.AddCell(AddNewCell(enums.GetDescription(), CellFontS, false));
                    }




                }
                cell = new PdfPCell(new Phrase("CRA #: " + objQuestionTPCRA.PCRANumber, CellFontS))
                {
                    Colspan = 4,
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);

                pdfDoc.Add(table);
            }
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f
            };

            table.AddCell(new Phrase("Project Name: " + objTIcraLog.ProjectName, CellFontS));
            cell = new PdfPCell(new Phrase("Permit #: " + objTIcraLog.PermitNo, CellFontS))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            table.AddCell(cell);

            //table.AddCell(new Phrase("Project Start Date: " + (objTIcraLog.StartDate.HasValue ? objTIcraLog.StartDate.Value.ToFormatDate() : string.Empty), CellFontS));

            table.AddCell(new Phrase("Location of Construction: " + objTIcraLog.Location, CellFontS));
            table.AddCell(new Phrase("Project Start Date: " + (objTIcraLog.StartDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.StartDate.Value) : string.Empty), CellFontS));

            table.AddCell(new Phrase("Project Coordinator: " + objTIcraLog.ProjectCoordinator, CellFontS));
            table.AddCell(new Phrase("Estimated Duration: " + objTIcraLog.EstimatedDuration, CellFontS));

            table.AddCell(new Phrase("Contractor Performing Work: " + objTIcraLog.Contractor, CellFontS));
            table.AddCell(new Phrase("Permit Expiration Date: " + (objTIcraLog.CompletionDate.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.CompletionDate.Value) : string.Empty), CellFontS));

            table.AddCell(new Phrase("Supervisor: " + objTIcraLog.Supervisor, CellFontS));
            table.AddCell(new Phrase("Telephone: " + objTIcraLog.Telephone, CellFontS));
            if (objQuestionTPCRA != null && objQuestionTPCRA.TicraId > 0)
            {
                table.AddCell(new Phrase("Scope: " + objTIcraLog.Scope, CellFontS));
                table.AddCell(new Phrase("Phone Number: " + objQuestionTPCRA.Phone, CellFontS));
                table.AddCell(new Phrase("Date Submitted: " + HCF.Utility.CommonUtility.ConvertToFormatDate(objQuestionTPCRA.DateSubmitted.Value), CellFontS));
                table.AddCell(new Phrase("Email Address : " + objQuestionTPCRA.EmailAddress, CellFontS));
                table.AddCell(new Phrase("Building : " + objQuestionTPCRA.BuildingName, CellFontS));
                table.AddCell(new Phrase("Floor : " + objQuestionTPCRA.FloorName, CellFontS));

                cell = new PdfPCell(new Phrase("Brief Description of Work : " + objQuestionTPCRA.WorkDescription, CellFontS))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                pdfDoc.Add(table);
                //table = new PdfPTable(7)
                //{
                //    WidthPercentage = 100,
                //    HorizontalAlignment = 0
                //};
                //table.AddCell(new Phrase("S.No", CellFontS));
                //table.AddCell(new Phrase("Above", CellFontS));
                //table.AddCell(new Phrase("Below", CellFontS));
                //table.AddCell(new Phrase("North", CellFontS));
                //table.AddCell(new Phrase("South", CellFontS));
                //table.AddCell(new Phrase("East", CellFontS));
                //table.AddCell(new Phrase("West", CellFontS));
                //for (int i = 0; i < objQuestionTPCRA.TDepartmentNearConstruction.Count; i++)
                //{
                //    table.AddCell(new Phrase((i+1).ToString(), CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].Above, CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].below, CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].North, CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].South, CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].East, CellFontS));
                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].West, CellFontS));

                //}
                //pdfDoc.Add(table);
            }
            else
            {
                cell = new PdfPCell(new Phrase("Scope: " + objTIcraLog.Scope, CellFontS))
                {
                    Colspan = 2,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                pdfDoc.Add(table);
            }
            float[] widths = new float[] { 15f, 35f, 15f, 35f };
            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            table.SetWidths(widths);
            table.AddCell(new Phrase("YES/NO", CellFontS));
            table.AddCell(new Phrase("CONSTRUCTION ACTIVITY", CellFontS));
            table.AddCell(new Phrase("YES/NO", CellFontS));
            table.AddCell(new Phrase("INFECTION CONTROL RISK GROUP", CellFontS));
            bool addtotable = false;
            for (int i = 0; i < objTIcraLog.ConstructionTypes.Count; i++)
            {
                string typetext = objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId ? "YES" : "NO";
                PdfPCell cell1 = new PdfPCell(new Phrase(typetext, CellFontS));
                PdfPCell cell2 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[i].TypeName + ": " + objTIcraLog.ConstructionTypes[i].Description, CellFontS));
                if (objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
                {
                    cell1.BackgroundColor = Gray;
                    cell2.BackgroundColor = Gray;
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                    addtotable = true;
                }


                string risktext = objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId ? "YES" : "NO";
                PdfPCell cell3 = new PdfPCell(new Phrase(risktext, CellFontS));
                PdfPCell cell4 = new PdfPCell();
                for (int j = i; j <= i; j++)
                {
                    cell4 = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[i].GroupName + ": " + objTIcraLog.ConstructionRisks[i].RiskName, CellFontS));
                }
                if (objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
                {
                    cell3.BackgroundColor = Gray;
                    cell4.BackgroundColor = Gray;
                    table.AddCell(cell3);
                    table.AddCell(cell4);
                    addtotable = true;
                }

            }
            if (addtotable)
            {
                pdfDoc.Add(table);
            }

            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            widths = new float[] { 40f, 60f };
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
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                }

            }
            pdfDoc.Add(table);
            //for (int i = 0; i < objTIcraLog.ConstructionTypes.Count; i++)
            //{
            //    string typetext = objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId ? "YES" : "NO";
            //    PdfPCell cell1 = new PdfPCell(new Phrase(typetext, CellFontS));
            //    PdfPCell cell2 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[i].TypeName + ": " + objTIcraLog.ConstructionTypes[i].Description, CellFontS));
            //    if (objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
            //    {
            //        cell1.BackgroundColor = Gray;
            //        cell2.BackgroundColor = Gray;
            //    }
            //    table.AddCell(cell1);
            //    table.AddCell(cell2);

            //    string risktext = objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId ? "YES" : "NO";
            //    PdfPCell cell3 = new PdfPCell(new Phrase(risktext, CellFontS));
            //    PdfPCell cell4 = new PdfPCell();
            //    for (int j = i; j <= i; j++)
            //    {
            //        cell4 = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[i].GroupName + ": " + objTIcraLog.ConstructionRisks[i].RiskName, CellFontS));
            //    }
            //    if (objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
            //    {
            //        cell3.BackgroundColor = Gray;
            //        cell4.BackgroundColor = Gray;
            //    }
            //    table.AddCell(cell3);
            //    table.AddCell(cell4);
            //}
            //pdfDoc.Add(table);

            float[] widths1 = new float[] { 10f, 45f, 45f };
            table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            table.SetWidths(widths1);
            for (int i = 0; i < objTIcraLog.ConstructionClasses.Count; i++)
            {
                int Activitycount = objTIcraLog.ConstructionClasses[i].ConstructionClassActivity.Count;
                int count = Activitycount % 2 == 0 ? ((Activitycount / 2)) : ((Activitycount / 2) + 1);
                string activity = string.Empty;
                string activity1 = string.Empty;
                PdfPCell cell1;
                if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == 3)
                {
                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName + '\n' + (objTIcraLog.Class3Date.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.Class3Date.Value) : string.Empty) + '\n' + objTIcraLog.Class3Initial, CellFontS));
                }
                else if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == 4)
                {
                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName + '\n' + (objTIcraLog.Class4Date.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.Class4Date.Value) : string.Empty) + '\n' + objTIcraLog.Class4Initial, CellFontS));
                }
                else
                {
                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName, CellFontS));
                }



                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();
                for (int j = 0; j < count; j++)
                {
                    if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId && (!string.IsNullOrEmpty(objTIcraLog.ActivityLists) && !objTIcraLog.ActivityLists.Split(',').Contains(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].ConstClassActivityId.ToString())))
                        cell2.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", Strikethru));
                    else
                        cell2.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", CellFontS));
                    //activity = activity + "•" + objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n";
                }
                for (int j = count; j < objTIcraLog.ConstructionClasses[i].ConstructionClassActivity.Count; j++)
                {
                    if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId && (!string.IsNullOrEmpty(objTIcraLog.ActivityLists) && !objTIcraLog.ActivityLists.Split(',').Contains(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].ConstClassActivityId.ToString())))
                        cell3.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", Strikethru));
                    else
                        cell3.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", CellFontS));
                    //activity1 = activity1 + "•" + objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n";
                }
                if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId)
                {
                    cell1.BackgroundColor = Gray;
                    cell2.BackgroundColor = Gray;
                    cell3.BackgroundColor = Gray;
                    table.AddCell(cell1);
                    table.AddCell(cell2);
                    table.AddCell(cell3);
                }

            }
            cell = new PdfPCell(new Phrase("Additional Requirements: " + objTIcraLog.Comment, CellFontS))
            {
                Colspan = 3
            };
            table.AddCell(cell);

            pdfDoc.Add(table);

            float[] widths3 = new float[] { 50f, 25f, 25f };
            table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            table.SetWidths(widths3);
            //cell = new PdfPCell(new Phrase(string.Empty))
            //{
            //    Colspan = 3
            //};
            //table.AddCell(cell);
            HCF.BDO.Enums.ApprovalStatus enums1 = (HCF.BDO.Enums.ApprovalStatus)objTIcraLog.Status;
            table.AddCell(new Phrase("Status : " + (objTIcraLog.Status == 3 ? "Hold" : enums1.GetDescription()), CellFontS));
            table.AddCell(new Phrase("Date: " + (objTIcraLog.Date1.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.Date1.Value) : string.Empty) + " Initials: " + objTIcraLog.Initial1, CellFontS));
            table.AddCell(new Phrase("Date: " + (objTIcraLog.Date2.HasValue ? HCF.Utility.CommonUtility.ConvertToFormatDate(objTIcraLog.Date2.Value) : string.Empty) + " Initials: " + objTIcraLog.Initial2, CellFontS));
            pdfDoc.Add(table);

            if (objQuestionTPCRA != null && objQuestionTPCRA.TicraId > 0)
            {
            }
            else
            {
                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };
                int twocolcount = 0;
                foreach (var permitworkflow in objTIcraLog.TPermitWorkFlowDetails)
                {
                    if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 45f);
                        table.AddCell(cellmain);

                        twocolcount++;
                    }

                    if (twocolcount == 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                    if (twocolcount % 2 == 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                }

                if (twocolcount % 2 != 0)
                {
                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                }
                pdfDoc.Add(table);

                //table = new PdfPTable(2)
                //{
                //    WidthPercentage = 100,
                //    HorizontalAlignment = 0
                //};
                //int twocolcount = 0;

                //if (objTIcraLog.PermitRequestByUser != null && objTIcraLog.DSPermitRequestBy != null && objTIcraLog.DSPermitRequestBy.DigSignatureId > 0)
                //{
                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Permit Request By:", objTIcraLog.DSPermitRequestBy, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;
                //}
                //if (twocolcount == 0)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}

                //if (objTIcraLog.PermitReviewerBy != null && objTIcraLog.DSPermitReviewerBy != null && objTIcraLog.DSPermitReviewerBy.DigSignatureId > 0)
                //{

                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Reviewer 1:", objTIcraLog.DSPermitReviewerBy, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;
                //}
                //if (twocolcount == 2)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}
                //if (objTIcraLog.PermitAuthorizedBy != null && objTIcraLog.DSPermitAuthorizedBy != null && objTIcraLog.DSPermitAuthorizedBy.DigSignatureId > 0)
                //{
                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Reviewer 2:", objTIcraLog.DSPermitAuthorizedBy, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;
                //}

                //if (twocolcount % 2 == 0)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}
                //if (twocolcount == 1)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //}

                //pdfDoc.Add(table);
            }
            if (objQuestionTPCRA != null)
            {

                //table = new PdfPTable(2)
                //{
                //    WidthPercentage = 100,
                //    HorizontalAlignment = 0,
                //    SpacingBefore = 10f
                //};
                //int twocolcount = 0;
                //if (objQuestionTPCRA.ContractorId > 0 && objQuestionTPCRA.DSContractorSignature != null && objQuestionTPCRA.DSContractorSignature.DigSignatureId > 0)
                //{
                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Contractor/ Requester:", objQuestionTPCRA.DSContractorSignature, 45f);
                //    table.AddCell(cellmain);

                //    twocolcount++;
                //}
                //if (twocolcount == 0)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}
                //if (objQuestionTPCRA.InfectionControlId > 0 && objQuestionTPCRA.DSInfectionControlSignature != null && objQuestionTPCRA.DSInfectionControlSignature.DigSignatureId > 0)
                //{

                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Infection Control: ", objQuestionTPCRA.DSInfectionControlSignature, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;
                //}
                //if (twocolcount == 2)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}
                //if (objQuestionTPCRA.FacilityId > 0 && objQuestionTPCRA.DSFacilitySignature != null && objQuestionTPCRA.DSFacilitySignature.DigSignatureId > 0)
                //{
                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Facilities: ", objQuestionTPCRA.DSFacilitySignature, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;
                //}
                //if (twocolcount % 2 == 0)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //    twocolcount++;
                //}

                //if (objQuestionTPCRA.SafetyId > 0 && objQuestionTPCRA.DSSafetySignature != null && objQuestionTPCRA.DSSafetySignature.DigSignatureId > 0)
                //{

                //    PdfPCell cellmain = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cellmain = CreateSignSectionCell("Safety:", objQuestionTPCRA.DSSafetySignature, 45f);
                //    table.AddCell(cellmain);
                //    twocolcount++;

                //}
                //if (twocolcount % 2 != 0)
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //}
                //pdfDoc.Add(table);


                table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };
                int twocolcount = 0;
                foreach (var permitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
                {
                    if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                    {
                        PdfPCell cellmain = new PdfPCell()
                        {
                            Border = 0,
                        };
                        cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 45f);
                        table.AddCell(cellmain);

                        twocolcount++;
                    }

                    if (twocolcount == 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                    if (twocolcount % 2 == 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                        twocolcount++;
                    }

                }

                if (twocolcount % 2 != 0)
                {
                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                }
                pdfDoc.Add(table);
            }

        }


        public string PCRAPermitWorksheetBytes(int projectId, int tpcraquestid, int icraId, string PDFName)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
            objQuestionTPCRA = _pCRAService.GetQuestionTPCRA(projectId, tpcraquestid);

            if (icraId > 0)
            {
                pdfDoc = CreateICRAPermit(icraId, mem, objQuestionTPCRA, 0);
            }
            else
            {
                pdfDoc = CreatePCRAPermit(objQuestionTPCRA.TicraId, mem, objQuestionTPCRA, 1);
            }

            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }
        public Document CreatePCRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int PermitType)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            TIcraLog objTIcraLog = getTicralog(icraId);

            PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            Paragraph para = new Paragraph("Infection Control Risk Assessment \n Matrix of Precautions for Construction & Renovation", TitleFontS)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);
            Paragraph line = new Paragraph();
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            for (int i = 0; i < objTIcraLog.TIcraSteps.Count; i++)
            {
                if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 1)
                {
                    float[] widths = new float[] { 40f, 60f };
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Type of Construction Project Activity (Type A-D)", ParagraphFontS);
                    pdfDoc.Add(para);
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 20f
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
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Patient Risk Group (Low, Medium, High, Highest) with the planned Construction Project Type (A, B, C, D) on the following matrix, to find the Class of Precautions (I, II, III or IV) or level of infection control activities required. Class I-IV or Color-Coded Precautions are delineated on the following page.\nIC Matrix - Class of Precautions: Construction Project by Patient Risk", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        //SpacingAfter = 30f
                    };
                    pdfDoc.Add(para);
                    pdfDoc.NewPage();
                    para = new Paragraph("\nConstruction Project Type ", ParagraphFontS)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        //SpacingBefore = 30f
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
                            //var data = objTIcraLog.ICRAMatixPrecautions.Where(x => x.ConstructionRiskId == objTIcraLog.ConstructionRisks[j].ConstructionRiskId && x.ConstructionTypeId == objTIcraLog.ConstructionTypes[k].ConstructionTypeId).ToList();
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
                    para = new Paragraph("Note: Infection Control approval will be required when the Construction Activity and Risk Level indicate that Class IIIor Class IVcontrol procedures are necessary.", ParagraphFontS);
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

                    //for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    //{
                    //    string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames : string.Empty;
                    //    table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + riskarea, CellFontS));
                    //}
                    string risklist = "";
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames + "," : string.Empty;
                        risklist = risklist + riskarea;
                        table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + risklist, CellFontS));
                    }
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string riskName = objTIcraLog.AreasSurroundings[k].ConstructionRisk != null ? objTIcraLog.AreasSurroundings[k].ConstructionRisk.RiskName : string.Empty;
                        table.AddCell(new Phrase("Risk Group : " + riskName, CellFontS));
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
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ": " + objTIcraLog.TIcraSteps[i].Step.Steps + "\n", ParagraphFontS);
                    pdfDoc.Add(para);
                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment, ParagraphFontS)
                    {
                        SpacingBefore = 5f

                    };
                    pdfDoc.Add(para);
                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
                    pdfDoc.Add(line);
                }
            }
            if (PermitType == 1)
            {
                //para = new Paragraph("Section Approval:", ParagraphFontS);
                //pdfDoc.Add(para);
                table = new PdfPTable(3)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 10f
                };

                PdfPTable tableContractor = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                };
                PdfPTable tableinfectionist = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                };
                PdfPTable tablesafety = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                };
                PdfPTable tablefacility = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                };
                tableContractor.AddCell(AddNewCell("Contractor/ Requester:", smallfontbold, false));
                tableContractor.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
                tablefacility.AddCell(AddNewCell("Facilities:", smallfontbold, false));
                tablefacility.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
                tableContractor.AddCell(AddNewCell(objQuestionTPCRA.ContractorId != null && objQuestionTPCRA.ContractorUser != null && objQuestionTPCRA.ContractorUser.Name != null ? objQuestionTPCRA.ContractorUser.Name : " ", ParagraphFontS, false));
                tableContractor.AddCell(AddNewCell(($"{objQuestionTPCRA.ContractorSignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
                tablefacility.AddCell(AddNewCell(objQuestionTPCRA.FacilityId != null && objQuestionTPCRA.FacilityUser != null && objQuestionTPCRA.FacilityUser.Name != null ? objQuestionTPCRA.FacilityUser.Name : " ", ParagraphFontS, false));
                tablefacility.AddCell(AddNewCell(($"{objQuestionTPCRA.FacilitySignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
                if (objQuestionTPCRA.DSContractorSignature != null && objQuestionTPCRA.DSContractorSignature.DigSignatureId > 0)
                {
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonProvider.FilePath(objQuestionTPCRA.DSContractorSignature.FilePath));
                    //image.ScaleAbsolute(80, 80);
                    cell.AddElement(new Chunk(image, 0, 0));
                    tableContractor.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cell.AddElement(new Phrase(objQuestionTPCRA.DSContractorSignature.SignByUserName + " (" + objQuestionTPCRA.DSContractorSignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    tableContractor.AddCell(cell);

                }
                else
                {
                    tableContractor.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
                }
                if (objQuestionTPCRA.DSFacilitySignature != null && objQuestionTPCRA.DSFacilitySignature.DigSignatureId > 0)
                {
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonProvider.FilePath(objQuestionTPCRA.DSFacilitySignature.FilePath));
                    //image.ScaleAbsolute(80, 80);
                    cell.AddElement(new Chunk(image, 0, 0));
                    tablefacility.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cell.AddElement(new Phrase(objQuestionTPCRA.DSFacilitySignature.SignByUserName + " (" + objQuestionTPCRA.DSFacilitySignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    tablefacility.AddCell(cell);

                }
                else
                {
                    tablefacility.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
                }
                tableinfectionist.AddCell(AddNewCell("Infection Control: ", smallfontbold, false));
                tableinfectionist.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
                tablesafety.AddCell(AddNewCell("Safety:", smallfontbold, false));
                tablesafety.AddCell(AddNewCell("Approval Date:", smallfontbold, false));

                tableinfectionist.AddCell(AddNewCell(objQuestionTPCRA.InfectionControlId != null && objQuestionTPCRA.InfectionControlUser != null && objQuestionTPCRA.InfectionControlUser.Name != null ? objQuestionTPCRA.InfectionControlUser.Name : " ", ParagraphFontS, false));
                tableinfectionist.AddCell(AddNewCell(($"{objQuestionTPCRA.InfectionControlSignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
                tablesafety.AddCell(AddNewCell(objQuestionTPCRA.SafetyId != null && objQuestionTPCRA.SafetyUser != null && objQuestionTPCRA.SafetyUser.Name != null ? objQuestionTPCRA.SafetyUser.Name : " ", ParagraphFontS, false));
                tablesafety.AddCell(AddNewCell(($"{objQuestionTPCRA.SafetySignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
                if (objQuestionTPCRA.DSInfectionControlSignature != null && objQuestionTPCRA.DSInfectionControlSignature.DigSignatureId > 0)
                {
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    Image image = Image.GetInstance(_commonProvider.FilePath(objQuestionTPCRA.DSInfectionControlSignature.FilePath));
                    //image.ScaleAbsolute(80, 80);
                    cell.AddElement(new Chunk(image, 0, 0));
                    tableinfectionist.AddCell(cell);
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cell.AddElement(new Phrase(objQuestionTPCRA.DSInfectionControlSignature.SignByUserName + " (" + objQuestionTPCRA.DSInfectionControlSignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    tableinfectionist.AddCell(cell);


                }
                else
                {
                    tableinfectionist.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
                }
                if (objQuestionTPCRA.DSSafetySignature != null && objQuestionTPCRA.DSSafetySignature.DigSignatureId > 0)
                {
                    cell = new PdfPCell()
                    {
                        Border = 0,
                    };

                    cell.AddElement(new Phrase(objQuestionTPCRA.DSSafetySignature.SignByUserName + " (" + objQuestionTPCRA.DSSafetySignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                    tablesafety.AddCell(cell);
                }
                else
                {
                    tablesafety.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
                }
                pdfDoc.Add(table);
            }



            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }


        public string PrintPCRAPdfInbytes(int? projectId, int? tPCRAQuesId, string mode, int? doctype)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
            objQuestionTPCRA = _pCRAService.GetQuestionTPCRA(projectId, tPCRAQuesId);
            pdfDoc = CreatePrintPCRAPdf(projectId, tPCRAQuesId, mem, objQuestionTPCRA, doctype, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }


        public Document CreatePrintPCRAPdf(int? projectId, int? tPCRAQuesId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? doctype, bool IsAttachmentIncluded)
        {

            System.Net.ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            if (doctype.HasValue && doctype.Value == 1)
                SetHeaderBlue(out table, "Construction Risk Assessment [CRA] ");
            else
                SetHeaderBlue(out table, "Pre-Construction Risk Assessment [PCRA] ");

            pdfDoc.Add(table);

            table = new PdfPTable(5)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f,

            };

            float[] widths = new float[] { 17f, 30f, 6f, 17f, 30f };
            table.SetTotalWidth(widths);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            TPCRAQuestion objFilteredQuestionTPCRA = new TPCRAQuestion();
            objFilteredQuestionTPCRA = objQuestionTPCRA;
            var data = objQuestionTPCRA.TPCRAQuestionDetails.Where(s => s.QuestionPCRA.IsActive == true).ToList();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = new List<TPCRAQuestionDetails>();
            objFilteredQuestionTPCRA.TPCRAQuestionDetails = data;

            // table.DefaultCell.Border = Rectangle.NO_BORDER;
            Chunk chunk1 = new Chunk();
            var para = new Paragraph();
            PdfPCell cell = new PdfPCell();
            para.Alignment = Element.ALIGN_LEFT;
            var projectData = new TIcraProject();
            if (objQuestionTPCRA.ProjectId > 0)
            {
                projectData = _pCRAService.GetProjectDetails().FirstOrDefault(m => m.ProjectId == objQuestionTPCRA.ProjectId);

                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(projectData.ProjectName, CellFont, false));
                table.AddCell("");
                table.AddCell(AddNewCell("Project Number:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(projectData.ProjectNumber, CellFont, false));

                if (doctype.HasValue && doctype.Value == 1)
                {
                    TIcraLog objTIcraLog = getTicralog(objQuestionTPCRA.TicraId);
                    if (objTIcraLog != null && objQuestionTPCRA.TicraId > 0)
                    {
                        table.AddCell(AddNewCell("Location of Construction:", CellFontBoldBlack, false));
                        table.AddCell(AddNewCell(objTIcraLog.Location, CellFont, false));
                        table.AddCell("");
                        table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
                        table.AddCell(AddNewCell(objQuestionTPCRA.ContractorId != null && objQuestionTPCRA.ContractorUser != null && objQuestionTPCRA.ContractorUser.Name != null ? objQuestionTPCRA.ContractorUser.Name : " ", CellFont, false));
                        table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
                        table.AddCell(AddNewCell((objTIcraLog.StartDate.HasValue ? _commonProvider.ConvertToFormatDate(objTIcraLog.StartDate.Value) : string.Empty), CellFont, false));
                        table.AddCell("");
                        table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
                        table.AddCell(AddNewCell((objTIcraLog.CompletionDate.HasValue ? _commonProvider.ConvertToFormatDate(objTIcraLog.CompletionDate.Value) : string.Empty), CellFont, false));
                        table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
                        table.AddCell(AddNewCell(objQuestionTPCRA.WorkDescription, CellFont, false));
                    }

                }
                else
                {
                    table.AddCell(AddNewCell("Project Location:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(projectData.ProjectLocation, CellFont, false));
                    table.AddCell("");
                    table.AddCell(AddNewCell("Architect:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(projectData.Architect, CellFont, false));
                    table.AddCell(AddNewCell("Project Manager:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(projectData.ProjectManager, CellFont, false));
                    table.AddCell("");
                    table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(projectData.ProjectContractor, CellFont, false));
                    table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(_commonProvider.ConvertToFormatDate(projectData.ProjectStartDate.Value), CellFont, false));
                    table.AddCell("");
                    table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
                    if (projectData.ProjectEndDate != null)
                    {
                        table.AddCell(AddNewCell(_commonProvider.ConvertToFormatDate(projectData.ProjectEndDate.Value), CellFont, false));
                    }
                    else
                    {
                        table.AddCell(AddNewCell(string.Empty, CellFont, false));
                    }

                    table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(projectData.Description, CellFont, false));
                }


                table.AddCell("");
                table.AddCell("");
                table.AddCell("");

            }

            pdfDoc.Add(table);
            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            pdfDoc.Add(line1);
            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f,
            };

            widths = new float[] { 75f, 5f, 10f, 10f };
            table.SetTotalWidth(widths);


            PdfPTable table1 = new PdfPTable(5)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 0f,
            };

            widths = new float[] { 10f, 65f, 5f, 10f, 10f };
            table1.SetTotalWidth(widths);
            PdfPCell cellsubtable = new PdfPCell()
            {
                Border = 0,
            };

            Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
            ImgCheckBox.ScaleAbsolute(10f, 10f);

            Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
            ImgUnCheckBox.ScaleAbsolute(10f, 10f);
            int sectionlen = 0;
            for (int i = 0; i < objQuestionTPCRA.TPCRAQuestionDetails.Count; i++)
            {
                sectionlen++;
                table.AddCell(AddNewCell("Section " + sectionlen, CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell("YES", CellFontNormalBlueSmall, false));
                table.AddCell(AddNewCell("NO", CellFontNormalBlueSmall, false));

                //question starts
                table.AddCell(AddNewCell(sectionlen + ". " + objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.Questions, CellFont, false));
                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));

                PdfPCell nobordercell = new PdfPCell()
                {
                    Border = 0,
                };

                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuesStatus == 1)
                {
                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };

                    chunk1 = new Chunk(ImgCheckBox, 3, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                }
                else if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuesStatus == 0)
                {

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    chunk1 = new Chunk(ImgCheckBox, 1, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);


                }
                else
                {

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);

                    nobordercell = new PdfPCell()
                    {
                        Border = 0,
                    };
                    chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
                    nobordercell.AddElement(chunk1);
                    table.AddCell(nobordercell);


                }
                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.IsOption == true)
                {
                    table1 = new PdfPTable(5)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 0f,
                    };

                    widths = new float[] { 3f, 72f, 5f, 10f, 10f };
                    table1.SetTotalWidth(widths);
                    cellsubtable = new PdfPCell(table1)
                    {
                        Border = 0,

                    };
                    cellsubtable.Colspan = 5;
                    for (int j = 0; j < objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs.Select(s => s.Text).Count(); j++)
                    {

                        if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].IsReadOnly)
                        {
                            table1.AddCell(AddNewCell(" ", CellFont, false));
                            table1.AddCell(AddNewCell(objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].Text, CellFont, true));
                            table1.AddCell(AddNewCell(" ", CellFont, false));
                            table1.AddCell(AddNewCell(" ", CellFont, false));
                            table1.AddCell(AddNewCell(" ", CellFont, false));

                        }
                        else
                        {
                            table1.AddCell(AddNewCell(" ", CellFont, false));
                            table1.AddCell(AddNewCell(objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].Text, CellFont, false));
                            table1.AddCell(AddNewCell(" ", CellFont, false));
                            if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].OptionStatus)
                            {
                                nobordercell = new PdfPCell()
                                {
                                    Border = 0,
                                };

                                chunk1 = new Chunk(ImgCheckBox, 3, 0);
                                nobordercell.AddElement(chunk1);
                                table1.AddCell(nobordercell);

                                nobordercell = new PdfPCell()
                                {
                                    Border = 0,
                                };
                                chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
                                nobordercell.AddElement(chunk1);
                                table1.AddCell(nobordercell);

                            }

                            else
                            {

                                nobordercell = new PdfPCell()
                                {
                                    Border = 0,
                                };
                                chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
                                nobordercell.AddElement(chunk1);
                                table1.AddCell(nobordercell);

                                nobordercell = new PdfPCell()
                                {
                                    Border = 0,
                                };
                                chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
                                nobordercell.AddElement(chunk1);
                                table1.AddCell(nobordercell);


                            }
                        }



                    }

                    table.AddCell(cellsubtable);
                }

                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.CanComment == true)
                {
                    table.AddCell(AddNewCell("Comment: " + objQuestionTPCRA.TPCRAQuestionDetails[i].Comment, CellFont, false));
                    table.AddCell(AddNewCell(" ", CellFont, false));
                    table.AddCell(AddNewCell(" ", CellFont, false));
                    table.AddCell(AddNewCell(" ", CellFont, false));
                }
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
            }
            if (doctype.HasValue && doctype.Value == 1)
            {
            }
            else
            {
                sectionlen = sectionlen + 1;
                table.AddCell(AddNewCell("Section " + sectionlen + ":Approval", CellFontBoldBlueSmall, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
                table.AddCell(AddNewCell(" ", CellFont, false));
            }
            pdfDoc.Add(table);
            if (doctype.HasValue && doctype.Value == 1)
            {
            }
            else
            {
                //if(objQuestionTPCRA.Sign1User!=null)
                //{ 
                //}
                //table = new PdfPTable(5)
                //{
                //    WidthPercentage = 100,
                //    HorizontalAlignment = 0,
                //    SpacingBefore = 10f,
                //};

                //widths = new float[] { 35f, 20f, 15f, 10f, 20f };

                //table.AddCell(AddNewCell("Sign1 Name :", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell("Sign2 Name :", CellFontBoldBlueSmall, false));

                //table.AddCell(AddNewCell((objQuestionTPCRA.Sign1User != null ? objQuestionTPCRA.Sign1User.Name : string.Empty), UnderlineCellFont, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell((objQuestionTPCRA.Sign2User != null ? objQuestionTPCRA.Sign2User.Name : string.Empty), UnderlineCellFont, false));

                //table.AddCell(AddNewCell("Sign1 Date :", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell("Sign2 Date :", CellFontBoldBlueSmall, false));

                //table.AddCell(AddNewCell(objQuestionTPCRA.Sign1Date.ToFormatDate(), UnderlineCellFont, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(objQuestionTPCRA.Sign2Date.ToFormatDate(), UnderlineCellFont, false));



                //if (objQuestionTPCRA.DSSign1Signature != null && objQuestionTPCRA.DSSign1Signature.DigSignatureId > 0)
                //{
                //    cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    Image image = Image.GetInstance(CommonUtility.FilePath(objQuestionTPCRA.DSSign1Signature.FilePath));
                //    image.ScaleAbsolute(60, 60);
                //    cell.AddElement(image);
                //    table.AddCell(cell);
                //    cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cell.AddElement(new Phrase(objQuestionTPCRA.DSSign1Signature.SignByUserName + " (" + objQuestionTPCRA.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                //    table.AddCell(cell);
                //}
                //else
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //}
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //if (objQuestionTPCRA.DSSign2Signature != null && objQuestionTPCRA.DSSign2Signature.DigSignatureId > 0)
                //{
                //    cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    Image image = Image.GetInstance(CommonUtility.FilePath(objQuestionTPCRA.DSSign2Signature.FilePath));
                //    image.ScaleAbsolute(60, 60);
                //    cell.AddElement(image);
                //    table.AddCell(cell);
                //    cell = new PdfPCell()
                //    {
                //        Border = 0,
                //    };
                //    cell.AddElement(new Phrase(objQuestionTPCRA.DSSign2Signature.SignByUserName + " (" + objQuestionTPCRA.DSSign2Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
                //    table.AddCell(cell);
                //}
                //else
                //{
                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                //}

                //pdfDoc.Add(table);
            }

            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f
            };
            widths = new float[] { 8f, 92f };
            table.SetWidths(widths);
            PdfPCell cell1 = new PdfPCell();
            table.AddCell(AddNewCell("Status:", CellFontBoldBlueSmall, false));
            table.AddCell(AddNewCell(objQuestionTPCRA.ApprovalStatus == 3 ? "HOLD" : Enum.GetName(typeof(BDO.Enums.ApprovalStatus), objQuestionTPCRA.ApprovalStatus).ToString().ToUpper(), CellFont, false));
            table.AddCell(AddNewCell("Reason:", CellFontBoldBlueSmall, false));
            table.AddCell(AddNewCell(objQuestionTPCRA.RejectionReason, CellFont, false));
            pdfDoc.Add(table);
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 10f
            };
            int twocolcount = 0;
            foreach (var permitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
            {
                if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                {
                    PdfPCell cellmain = new PdfPCell()
                    {
                        Border = 0,
                    };
                    cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 45f);
                    table.AddCell(cellmain);

                    twocolcount++;
                }

                if (twocolcount == 0)
                {
                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                    twocolcount++;
                }

                if (twocolcount % 2 == 0)
                {
                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                    twocolcount++;
                }

            }

            if (twocolcount % 2 != 0)
            {
                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
            }

            pdfDoc.Add(table);
            bool isattach = false;
            if (IsAttachmentIncluded)
            {

                TIcraLog objTIcra = new TIcraLog();
                if (objQuestionTPCRA.DrawingAttachments.Count > 0 || objTIcra.TICRAFiles.Count > 0)
                {
                    pdfDoc.NewPage();
                    isattach = true;
                }
                if (isattach)
                {
                    if (objQuestionTPCRA.DrawingAttachments.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 20f
                        };

                        table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack, false));
                        foreach (var drawingsname in objQuestionTPCRA.DrawingAttachments)
                        {
                            AddAttachmentCell(drawingsname.ImagePath, drawingsname.FullFileName, pdfWriter, table);

                        }


                        pdfDoc.Add(table);
                    }



                    if (objTIcra.TICRAFiles.Count > 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0,
                            SpacingBefore = 20f
                        };

                        if (objTIcra != null && objTIcra.TICRAFiles.Count > 0)
                        {
                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack, false));
                            foreach (var files in objTIcra.TICRAFiles)
                            {
                                AddAttachmentCell(files.FilePath, files.FileName, pdfWriter, table);
                            }
                        }
                        pdfDoc.Add(table);
                    }
                }

            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }


        #endregion

        #region ICRA
        public Document CreateICRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? PermitType = 0)
        {
            TIcraLog objTIcraLog = getTicralog(icraId);
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            //if (PermitType == 1)
            //{

            //    PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            //    pdfDoc.NewPage();
            //}

            PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
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
                        SpacingBefore = 20f
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
                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Patient Risk Group (Low, Medium, High, Highest) with the planned Construction Project Type (A, B, C, D) on the following matrix, to find the Class of Precautions (I, II, III or IV) or level of infection control activities required. Class I-IV or Color-Coded Precautions are delineated on the following page.\nIC Matrix - Class of Precautions: Construction Project by Patient Risk", ParagraphFontS)
                    {
                        SpacingBefore = 5f,
                        //SpacingAfter = 30f
                    };
                    pdfDoc.Add(para);
                    pdfDoc.NewPage();
                    para = new Paragraph("\nConstruction Project Type ", ParagraphFontS)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        //SpacingBefore = 30f
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
                            //var data = objTIcraLog.ICRAMatixPrecautions.Where(x => x.ConstructionRiskId == objTIcraLog.ConstructionRisks[j].ConstructionRiskId && x.ConstructionTypeId == objTIcraLog.ConstructionTypes[k].ConstructionTypeId).ToList();
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
                    para = new Paragraph("Note: Infection Control approval will be required when the Construction Activity and Risk Level indicate that Class IIIor Class IVcontrol procedures are necessary.", ParagraphFontS);
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
                    string risklist = "";
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames + "," : string.Empty;
                        risklist = risklist + riskarea;
                        table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + risklist, CellFontS));
                    }
                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
                    {
                        string riskName = objTIcraLog.AreasSurroundings[k].ConstructionRisk != null ? objTIcraLog.AreasSurroundings[k].ConstructionRisk.RiskName : string.Empty;
                        table.AddCell(new Phrase("Risk Group : " + riskName, CellFontS));
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
            //    PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
            //}
            bool isattachemnets = false;
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
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0
                    };
                    table.AddCell(AddNewCell("Attachment(s):", ParagraphFontS));
                    for (int fileid = 0; fileid < objTIcraLog.TICRAFiles.Count(); fileid++)
                    {
                        //table.AddCell(AddNewCell(objQuestionTPCRA.DrawingAttachments[k].FullFileName, ParagraphFontS));
                        //table.AddCell(AddNewCell("", ParagraphFontS));
                        AddAttachmentCell(objTIcraLog.TICRAFiles[fileid].FilePath, objTIcraLog.TICRAFiles[fileid].FileName, pdfWriter, table);
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
                        table.AddCell(AddNewCell("Drawings Attached:", ParagraphFontS));
                        for (int k = 0; k < objQuestionTPCRA.DrawingAttachments.Count; k++)
                        {
                            //table.AddCell(AddNewCell(objQuestionTPCRA.DrawingAttachments[k].FullFileName, ParagraphFontS));
                            //table.AddCell(AddNewCell("", ParagraphFontS));
                            AddAttachmentCell(objQuestionTPCRA.DrawingAttachments[k].ImagePath, objQuestionTPCRA.DrawingAttachments[k].FullFileName, pdfWriter, table);
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
                        table.AddCell(AddNewCell("Drawings Attached:", ParagraphFontS));
                        for (int k = 0; k < objTIcraLog.DrawingAttachments.Count; k++)
                        {
                            //table.AddCell(AddNewCell(objTIcraLog.DrawingAttachments[k].FullFileName, ParagraphFontS));
                            //table.AddCell(AddNewCell("", ParagraphFontS));

                            AddAttachmentCell(objTIcraLog.DrawingAttachments[k].ImagePath, objTIcraLog.DrawingAttachments[k].FullFileName, pdfWriter, table);
                        }
                        pdfDoc.Add(table);
                    }

                }
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        public TIcraLog getTicralog(int TicraId)
        {
            TIcraLog objTIcraLog = new TIcraLog();
            if (TicraId > 0)
            {
                objTIcraLog = _constructionService.GetInspectionIcraSteps(TicraId);
                if (objTIcraLog != null)
                {
                    objTIcraLog.ConstructionTypes = _constructionService.GetConstructionType().Where(x => x.IsActive).ToList();
                    objTIcraLog.ConstructionRisks = _constructionService.GetConstructionRisk().Where(x => x.IsActive).ToList();
                    objTIcraLog.ConstructionClasses = _constructionService.GetConstructionClass().Where(x => x.IsActive).ToList();
                    objTIcraLog.ICRAMatixPrecautions = _constructionService.GetICRAMatixPrecautions().Where(x => x.IsActive).ToList();


                    if (objTIcraLog.AreasSurroundings.Count == 0)
                    {
                        objTIcraLog.AreasSurroundings = new List<TICRAAreasNearBy>();
                        foreach (BDO.Enums.AreasSurrounding item in Enum.GetValues(typeof(BDO.Enums.AreasSurrounding)))
                        {
                            TICRAAreasNearBy newTICRAAreasNearBy = new TICRAAreasNearBy
                            {
                                AreasSurrounding = item,
                                AreasSurroundingId = (int)item
                            };
                            objTIcraLog.AreasSurroundings.Add(newTICRAAreasNearBy);
                        }
                    }
                }
            }

            return objTIcraLog;
        }


        #endregion

        #region Tilsm Reports


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

            PdfPCell cellt = new PdfPCell(new Phrase("IncidentNo" + ": " + objTilsm.IncidentId, CellFontBold))
            {

            };
            table.AddCell(cellt);
            cellt = new PdfPCell(new Phrase("ILSM Date" + ": " + string.Format("{0} {1}", (objTilsm.llsmDate != null ?
                $"{_commonProvider.ConvertToFormatDate(objTilsm.llsmDate.ToClientTime())}" : "NA"), (objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : ""), CellFontBold)))
            {
            };
            table.AddCell(cellt);

            //table.AddCell(new Phrase(Resources.Resource.IncidentNo + ": " + objTilsm.IncidentId, CellFontBold));
            //table.AddCell(new Phrase(Resources.Resource.CreatedDate + ": " + objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

            pdfDoc.Add(table);

            float[] widths = new float[] { 30f, 70f };
            table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
            table.SetWidths(widths);

            table.AddCell(new Phrase("Name", CellFont));
            table.AddCell(new Phrase(objTilsm.Notes, CellFont));

            table.AddCell(new Phrase("StandardEP", CellFont));
            if (objTilsm.TicraId.HasValue)
                table.AddCell(new Phrase("Construction ILSM ", CellFont));
            else if (objTilsm.SourceInspection.EPDetails != null)
                table.AddCell(new Phrase(objTilsm.SourceInspection.EPDetails.StandardEp, CellFont));
            else
                table.AddCell(new Phrase("Manual ILSM ", CellFont));

            //table.AddCell(new Phrase(Resources.Resource.Location, CellFont));

            //table.AddCell(new Phrase(objTilsm.Floor != null ? $"{objTilsm.Floor.Building.BuildingName},{objTilsm.Floor.FloorName}"
            //    : "NA", CellFont));
            table.AddCell(new Phrase("User", CellFont));
            table.AddCell(new Phrase(objTilsm.Inspector != null ? $"{objTilsm.Inspector.FullName}" : "NA", CellFont));
            //table.AddCell(new Phrase("Date", CellFont));
            //table.AddCell(new Phrase(objTilsm.llsmDate != null ? $"{objTilsm.llsmDate.ToFormatDate()}": "NA", CellFont));
            //table.AddCell(new Phrase("Time", CellFont));
            //table.AddCell(new Phrase(objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : "NA", CellFont));

            table.AddCell(new Phrase("Status", CellFont));
            table.AddCell(new Phrase(_commonProvider.GetIlsmStatus((int)objTilsm.Status), CellFont));

            //table.AddCell(new Phrase(Resources.Resource.ILSMDate, CellFont));
            //table.AddCell(new Phrase(objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

            table.AddCell(new Phrase("Completed Date", CellFont));
            table.AddCell(new Phrase((int)objTilsm.Status == 1 ? objTilsm.CompletedDate.HasValue ? _commonProvider.ConvertToFormatDate(objTilsm.CompletedDate.Value.ToClientTime()) : "NA" : "NA", CellFont));

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
                    if (step.Status != -3)
                    {
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
                    }
                    else
                    {
                        table.AddCell(new Phrase());
                        table.AddCell(new Phrase(@Enum.GetName(typeof(BDO.Enums.InspectionStatus), step.Status), CellFont));
                    }
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


        #region Observation Reports 

        public string PrintProjectObserverReportInbytes(string projectIds, string reportId, ref TICRAReports ObservtionReport)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();
            pdfDoc = GenerateProjectObserverReport(projectIds, reportId, mem, ref ObservtionReport);
            var pdf = mem.ToArray();
            var base64EncodePdf = Convert.ToBase64String(pdf);
            return base64EncodePdf;
        }

        public Document GenerateProjectObserverReport(string projectIds, string reportId, Stream streamOutput, ref TICRAReports ObservtionReport)
        {
            //TICRAReports ObservtionReport = new TICRAReports();
            if (!string.IsNullOrEmpty(projectIds))
            {
                //icraLog = _constructionService.GetICRAObservationReport(Convert.ToInt32(icraId), Convert.ToInt32(reportId));
                ObservtionReport = _constructionService.GetICRAProjectObservationReport(projectIds, reportId);

                projectIds = string.Join(",", projectIds.Split(',').Select(x => Convert.ToInt32(x)).OrderBy(x => x).ToList());

                ObservtionReport = ObservtionReport.RelatedReports.Where(x => x.ProjectIds == projectIds).FirstOrDefault();
            }

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();

            //Table
            PdfPTable table;
            PdfPCell cell;
            Chunk chunk;
            SetHeader(out table);
            //Add table to document
            pdfDoc.Add(table);

            Paragraph para = new Paragraph("ICRA OBSERVER REPORT", TitleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(para);

            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //pdfDoc.Add(line);
            table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingBefore = 20f,
                SpacingAfter = 30f
            };

            table.AddCell(new Phrase("Project Name: " + ObservtionReport.ProjectNames, CellFont));
            table.AddCell(new Phrase("Contractor: " + ObservtionReport.ProjectContractors, CellFont));//
            table.AddCell(new Phrase("Project Location: " + ObservtionReport.ProjectLocations, CellFont));
            table.AddCell(new Phrase("Date: " + ObservtionReport.ReportDate.ToFormatDate(), CellFont));

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

            foreach (var item in ObservtionReport.CheckPoints)
            {
                table.AddCell(new Phrase(item.CheckPointText, CellFont));
                table.AddCell(new Phrase((item.Status == 0) ? "Yes" : "", CellFont));
                table.AddCell(new Phrase((item.Status == 1) ? "No" : "", CellFont));
                table.AddCell(new Phrase((item.Status == 2) ? "N/A" : "", CellFont));
            }


            //table.AddCell("June 28");

            pdfDoc.Add(table);

            para = new Paragraph("Comment,\n" + ObservtionReport.Comment, ParagraphFont);
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

            cell = new PdfPCell { Border = 0 };
            chunk = new Chunk("Contractor: " + ObservtionReport.DSContractorSignId.SignByUserName + "\nDate: " + ObservtionReport.ContractorSignDate.CastLocaleDate(), CellFont);
            cell.AddElement(chunk);
            table.AddCell(cell);

            cell = new PdfPCell { Border = 0 };
            chunk = new Chunk("Observer: " + ObservtionReport.DSObserverSignId.SignByUserName + "\nDate: " + ObservtionReport.ObserverSignDate.CastLocaleDate(), CellFont);
            cell.AddElement(chunk);
            table.AddCell(cell);


            cell = new PdfPCell { Border = 0 };
            if (!string.IsNullOrEmpty(ObservtionReport.DSContractorSignId.FilePath))
            {
                Image constimage = Image.GetInstance(_commonProvider.FilePath(ObservtionReport.DSContractorSignId.FilePath));
                cell.AddElement(new Chunk(constimage, 0, 0));
            }
            table.AddCell(cell);


            cell = new PdfPCell { Border = 0 };
            if (!string.IsNullOrEmpty(ObservtionReport.DSObserverSignId.FilePath))
            {
                Image observimage = Image.GetInstance(_commonProvider.FilePath(ObservtionReport.DSObserverSignId.FilePath));
                cell.AddElement(new Chunk(observimage, 0, 0));
            }
            table.AddCell(cell);


            cell = new PdfPCell { Border = 0 };
            cell.AddElement(new Phrase(ObservtionReport.DSContractorSignId.SignByUserName + " (" + ObservtionReport.DSContractorSignId.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
            table.AddCell(cell);


            cell = new PdfPCell { Border = 0 };
            cell.AddElement(new Phrase(ObservtionReport.DSObserverSignId.SignByUserName + " (" + ObservtionReport.DSObserverSignId.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
            table.AddCell(cell);


            pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        #endregion

        #region Round

        public Document RoundInspection(int floorId, string roundid, Stream streamOutput, int? groupround = 0)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            //int strlength = roundid.TrimEnd(',').Split(',').Length;
            
            List<TRounds> lstround = new List<TRounds>();

            foreach (string id in roundid.TrimEnd(',').Split(','))
            {
                int rid = Convert.ToInt32(id);
                var rounds = _roundService.GetRoundDetails(rid);
                lstround.Add(rounds);

            }
            var currentRound = lstround.FirstOrDefault();
            SetHeaderBlue(out table, currentRound.RoundType == 1 ?  "Departmental Round Report" : "Group Round Report");
            pdfDoc.Add(table);


            table = new PdfPTable(4)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            float[] widths = new float[] { 25f, 25f, 10f, 40f };
            table.SetWidths(widths);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.AddCell(AddNewCell("Round Name:", CellFontBoldSmall, false));
            table.AddCell(AddNewCell(lstround.FirstOrDefault().RoundName.CastToNA("NA"), CellFontSmall, false, 3));
            table.AddCell(AddNewCell("Questionnaire(s):", CellFontBoldSmall, false));
            table.AddCell(AddNewCell(string.Join(", ", lstround.Select(x => x.CategoryName)), CellFontSmall, false, 3));
            table.AddCell(AddNewCell("Round Date:", CellFontBoldSmall, false));
            table.AddCell(AddNewCell(lstround.FirstOrDefault().RoundDate.ToFormatDate(), CellFontSmall, false));
            table.AddCell(AddNewCell("Status:", CellFontBoldSmall, false));

            table.AddCell(AddNewCell((lstround.FirstOrDefault().Status == 2) ? "In progress" : "Completed", CellFontSmall, false));
            table.AddCell(AddNewCell("Round Time:", CellFontBoldSmall, false));
            table.AddCell(AddNewCell(Convert.ToDateTime(lstround.FirstOrDefault().CreatedDate.ToClientTime()).ToString("hh:mm tt").ToString(), CellFontSmall, false, 3));
            table.AddCell(AddNewCell("Schedule Time:", CellFontBoldSmall, false));
            table.AddCell(AddNewCell(lstround.FirstOrDefault().RoundGroupTime(), CellFontSmall, false, 3));


            table.AddCell(AddNewCell("Location(s):", CellFontBoldSmall, false));
            string location = string.Empty;
            foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
            {
                location += building.BuildingName + " ";
                foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                {
                    location += floor.FloorName + ", ";
                }
            }
            table.AddCell(AddNewCell(location.Trim().TrimEnd(','), CellFontSmall, false, 3));
            table.AddCell(AddNewCell("Participant(s):", CellFontBoldSmall, false));
            string participant = string.Empty;
            foreach (var roundUsers in lstround.FirstOrDefault().TRoundUsers.Where(x => x.IsActive || x.InspStatus != -1).OrderBy(y => y.User.FullName).ToList())
            {
                if (roundUsers.User != null && roundUsers.User.UserId != 0)
                {
                    participant += roundUsers.User.Name + ", ";
                }

            }
            table.AddCell(AddNewCell(participant.Trim().TrimEnd(','), CellFontSmall, false, 3));
            table.AddCell(AddNewCell("ILSM(s):", CellFontBoldSmall, false));
            string ILSMS = "NA";
            var ilsmCount = lstround.SelectMany(x => x.Ilsms).Count();
            if (ilsmCount > 0)
            {
                ILSMS = string.Join(",", lstround.SelectMany(x => x.Ilsms).Select(x => x.IncidentId));
            }
            table.AddCell(AddNewCell(ILSMS, CellFontSmall, false, 3));
            pdfDoc.Add(table);
            table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,

            };
            int i = 0;
            int j = 0;
            //int mainheading = 0;
            //int mainheading1 = 0;
            //int mainheading2 = 0;

            var inspectionRoundList = new List<TRounds>();
            foreach (var rounditem in lstround)
            {
                foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                {
                    foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                    {
                        var rounds = _roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                        if (rounds != null)
                            inspectionRoundList.Add(rounds);
                    }
                }
            }
            var nonCompliantCount = inspectionRoundList.SelectMany(x => x.Inspections.SelectMany(y => y.Questionnaires.Where(z => z.Status == 0))).ToList().Count;
            var compliantWithCommentCount = inspectionRoundList.SelectMany(x => x.Inspections.SelectMany(y => y.Questionnaires.Where(z => !string.IsNullOrEmpty(z.Comment)))).ToList().Count;
            var nonApplicableCount = inspectionRoundList.SelectMany(x => x.Inspections.SelectMany(y => y.Questionnaires.Where(z => z.Status == -3))).ToList().Count;

            if (nonCompliantCount > 0)
            {
                table = SetStepHeading(pdfDoc, "Non Compliant items");
                foreach (var rounditem in lstround)
                {
                    foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                    {
                        foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                        {
                            var rounds = inspectionRoundList.FirstOrDefault(x => x.FloorId == floor.FloorId && x.TRoundId == rounditem.TRoundId); //_roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                            if (rounds != null && rounds.Inspections.Count > 0)
                            {
                                i = 0;
                                j = 0;
                                foreach (var item in rounds.Inspections.SelectMany(x => x.Questionnaires).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList())
                                {
                                    foreach (var item3 in rounds.Inspections.Distinct())
                                    {
                                        //for non compliance
                                        foreach (var item2 in item3.Questionnaires.Where(x => x.RouQuesId == item.RouQuesId).Where(x => x.Status == 0))
                                        {
                                            //if (mainheading == 0)
                                            //{
                                            //    table = new PdfPTable(1)
                                            //    {
                                            //        WidthPercentage = 100,
                                            //        HorizontalAlignment = 0,
                                            //        SpacingAfter = 10f
                                            //    };
                                            //    table.DefaultCell.Border = Rectangle.NO_BORDER;
                                            //    table.AddCell(AddNewCell("Non Compliant items:", ParagraphFontlarge, false, 0));
                                            //    pdfDoc.Add(table);
                                            //    mainheading = mainheading + 1;
                                            //}
                                            if (i == 0)
                                            {
                                                RoundBuildingTitle(pdfDoc, rounds);

                                                table = new PdfPTable(4)
                                                {
                                                    WidthPercentage = 100,
                                                    HorizontalAlignment = 0
                                                };
                                                float[] celwidths = new float[] { 20f, 30f, 30f, 20f };
                                                table.SetWidths(celwidths);
                                                table.AddCell(new Phrase("Inspected By", CellFontBoldSmall));
                                                table.AddCell(new Phrase("Questionnaire(s)", CellFontBoldSmall));
                                                table.AddCell(new Phrase("Comment", CellFontBoldSmall));
                                                table.AddCell(new Phrase("Follow up", CellFontBoldSmall));
                                                pdfDoc.Add(table);
                                                i = i + 1;
                                            }

                                            PdfPTable table1 = new PdfPTable(4)
                                            {
                                                WidthPercentage = 100,
                                                HorizontalAlignment = 0
                                            };
                                            float[] cellwidths = new float[] { 20f, 30f, 30f, 20f };
                                            table1.SetWidths(cellwidths);
                                            table1.AddCell(new Phrase(($"{(item3.User != null && item3.UserId > 0 ? item3.User.Name : string.Empty)}"), CellFontSmall));
                                            table1.AddCell(new Phrase(item.RoundsQuestionnaires.RoundStep, CellFontSmall));
                                            if (!string.IsNullOrEmpty(item2.Comment))
                                                table1.AddCell(new Phrase(($"{item2.Comment}"), CellFontSmall));
                                            else
                                                table1.AddCell(new Phrase(($""), CellFontSmall));

                                            if (lstround.FirstOrDefault().RoundWorkOrders != null && lstround.FirstOrDefault().RoundWorkOrders.Count > 0)
                                            {
                                                string workorderstr = string.Empty;
                                                //foreach (var workorder in lstround.FirstOrDefault().RoundWorkOrders)
                                                //{
                                                //    table1.AddCell(new Phrase("WO#:" + workorder.WorkOrder.WorkOrderNumber, CellFontSmall));

                                                //}
                                                foreach (var worder in rounditem.RoundWorkOrders)
                                                {
                                                    if (rounds.RoundWorkOrders.Any(x => x.IssueId == worder.IssueId))
                                                    {
                                                        if (worder.WorkOrder.WorkOrderNumber != "0")
                                                            workorderstr += "WO#:" + worder.WorkOrder.WorkOrderNumber + ", ";

                                                    }


                                                }
                                                string ilsmstr = string.Empty;

                                                foreach (var ilsm in rounditem.Ilsms)
                                                {

                                                    ilsmstr += "Ilsms#:" + ilsm.IncidentId + ", ";

                                                }
                                                string combinename = workorderstr.Trim().TrimEnd(',') + ", " + ilsmstr.Trim().TrimEnd(',');
                                                table1.AddCell(new Phrase(combinename.Trim().TrimEnd(','), CellFontSmall));

                                            }
                                            else
                                            {
                                                table1.AddCell(new Phrase("", CellFontSmall));
                                            }
                                            pdfDoc.Add(table1);


                                        }

                                    }

                                }


                            }

                            PdfPTable tbloverallcomment = new PdfPTable(3)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingBefore = 10f,
                                SpacingAfter = 10f,

                            };

                            if (rounds != null)
                            {
                                foreach (var objinsp in rounds.Inspections)
                                {
                                    if (!string.IsNullOrEmpty(objinsp.Comment))
                                    {
                                        tbloverallcomment.AddCell(new Phrase(objinsp.User != null && objinsp.User.UserId > 0 ? objinsp.User.Name : string.Empty, CellFontSmall));
                                        tbloverallcomment.AddCell(new Phrase("Overall Comment", CellFontSmall));
                                        tbloverallcomment.AddCell(new Phrase(objinsp.Comment, CellFontSmall));

                                    }
                                }
                            }
                            pdfDoc.Add(tbloverallcomment);
                        }
                    }
                }
            }

            //mainheading1 = 0;
            i = 0;
            j = 0;
            if (compliantWithCommentCount > 0)
            {
                table = SetStepHeading(pdfDoc, "Compliant Items With Comment");
                foreach (var rounditem in lstround)
                {
                    foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                    {
                        foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                        {
                            //var rounds = _roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                            var rounds = inspectionRoundList.FirstOrDefault(x => x.FloorId == floor.FloorId && x.TRoundId == rounditem.TRoundId);
                            if (rounds != null && rounds.Inspections.Count > 0)
                            {

                                i = 0;
                                j = 0;
                                foreach (var item in rounds.Inspections.SelectMany(x => x.Questionnaires).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList())
                                {

                                    foreach (var item3 in rounds.Inspections.Distinct())
                                    {


                                        table = new PdfPTable(1)
                                        {
                                            WidthPercentage = 100,
                                            HorizontalAlignment = 0
                                        };
                                        //for non applicable
                                        foreach (var item2 in item3.Questionnaires.Where(x => x.RouQuesId == item.RouQuesId).Where(x => x.Status == 1))
                                        {
                                            //if (mainheading1 == 0)
                                            //{
                                            //    table = new PdfPTable(1)
                                            //    {
                                            //        WidthPercentage = 100,
                                            //        HorizontalAlignment = 0,
                                            //        SpacingAfter = 10f
                                            //    };
                                            //    table.DefaultCell.Border = Rectangle.NO_BORDER;
                                            //    table.AddCell(AddNewCell("Compliant Items With Comment", ParagraphFontlarge, false));
                                            //    pdfDoc.Add(table);
                                            //    mainheading1 = mainheading1 + 1;
                                            //}
                                            if (j == 0)
                                            {
                                                RoundBuildingTitle(pdfDoc, rounds);
                                                table = RoundReportHeader(pdfDoc, ref j);
                                            }
                                            PdfPTable table2 = new PdfPTable(3)
                                            {
                                                WidthPercentage = 100,
                                                HorizontalAlignment = 0
                                            };
                                            float[] cellwidths = new float[] { 20f, 40f, 40f };
                                            table2.SetWidths(cellwidths);
                                            table2.AddCell(new Phrase(($"{(item3.User != null && item3.UserId > 0 ? item3.User.Name : string.Empty)}"), CellFontSmall));
                                            table2.AddCell(new Phrase(item.RoundsQuestionnaires.RoundStep, CellFontSmall));
                                            if (!string.IsNullOrEmpty(item2.Comment))
                                                table2.AddCell(new Phrase(($"{item2.Comment}"), CellFontSmall));
                                            else
                                                table2.AddCell(new Phrase((""), CellFontSmall));
                                            pdfDoc.Add(table2);

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //mainheading1 = 0;

            i = 0;
            j = 0;
            if (nonApplicableCount > 0)
            {
                table = SetStepHeading(pdfDoc, "Non Applicable items");
                foreach (var rounditem in lstround)
                {
                    foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                    {
                        foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                        {
                            //var rounds = _roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                            var rounds = inspectionRoundList.FirstOrDefault(x => x.FloorId == floor.FloorId && x.TRoundId == rounditem.TRoundId);
                            if (rounds != null && rounds.Inspections.Count > 0)
                            {
                                i = 0;
                                j = 0;
                                foreach (var item in rounds.Inspections.SelectMany(x => x.Questionnaires).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList())
                                {
                                    foreach (var item3 in rounds.Inspections.Distinct())
                                    {
                                        table = new PdfPTable(1)
                                        {
                                            WidthPercentage = 100,
                                            HorizontalAlignment = 0
                                        };
                                        //for non applicable
                                        foreach (var item2 in item3.Questionnaires.Where(x => x.RouQuesId == item.RouQuesId).Where(x => x.Status == -3))
                                        {
                                            //if (mainheading1 == 0)
                                            //{
                                            //    table = new PdfPTable(1)
                                            //    {
                                            //        WidthPercentage = 100,
                                            //        HorizontalAlignment = 0,
                                            //        SpacingAfter = 10f
                                            //    };
                                            //    table.DefaultCell.Border = Rectangle.NO_BORDER;
                                            //    table.AddCell(AddNewCell("Non Applicable items:", ParagraphFontlarge, false));
                                            //    pdfDoc.Add(table);
                                            //    mainheading1 = mainheading1 + 1;
                                            //}
                                            if (j == 0)
                                            {
                                                RoundBuildingTitle(pdfDoc, rounds);
                                                table = RoundReportHeader(pdfDoc, ref j);
                                            }
                                            PdfPTable table2 = new PdfPTable(3)
                                            {
                                                WidthPercentage = 100,
                                                HorizontalAlignment = 0
                                            };
                                            float[] cellwidths = new float[] { 20f, 40f, 40f };
                                            table2.SetWidths(cellwidths);
                                            table2.AddCell(new Phrase(($"{(item3.User != null && item3.UserId > 0 ? item3.User.Name : string.Empty)}"), CellFontSmall));
                                            table2.AddCell(new Phrase(item.RoundsQuestionnaires.RoundStep, CellFontSmall));
                                            if (!string.IsNullOrEmpty(item2.Comment))
                                                table2.AddCell(new Phrase(($"{item2.Comment}"), CellFontSmall));
                                            else
                                                table2.AddCell(new Phrase((""), CellFontSmall));
                                            pdfDoc.Add(table2);

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            table = SetStepHeading(pdfDoc, "Questionnaire(s) Used During Rounding");
            foreach (var rounditem in lstround)
            {
                foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                {
                    foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                    {
                        var rounds = inspectionRoundList.FirstOrDefault(x => x.FloorId == floor.FloorId && x.TRoundId == rounditem.TRoundId);
                        //var rounds = _roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                        i = 0;
                        if (rounds != null && rounds.Inspections.Count > 0)
                        {
                            RoundBuildingTitle(pdfDoc, rounds);
                            foreach (var item in rounds.Inspections.SelectMany(x => x.Questionnaires).Distinct().GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList())
                            {
                                PdfPTable table3 = new PdfPTable(1)
                                {
                                    WidthPercentage = 100,
                                    HorizontalAlignment = 0
                                };
                                //table3.AddCell(new Phrase(rounds.Inspections.FirstOrDefault().Floor.FloorBuildingLocation, CellFontBoldSmall));
                                table3.AddCell(new Phrase(item.RoundsQuestionnaires.RoundStep, CellFontSmall));
                                pdfDoc.Add(table3);

                            }
                        }
                    }
                }
            }


            //Print Images in Reports if available//
            pdfDoc.NewPage();
            table = SetStepHeading(pdfDoc, "Images");
            PdfPTable imagetable = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
            };
            var imagecount = 0;
            foreach (var rounditem in lstround)
            {
                foreach (var building in lstround.FirstOrDefault().Locations.Where(x => x.IsActive).OrderBy(x => x.BuildingName))
                {
                    foreach (var floor in building.Floor.OrderBy(x => x.Sequence))
                    {
                        var rounds = inspectionRoundList.Where(x => x.FloorId == floor.FloorId && x.TRoundId == rounditem.TRoundId); //_roundService.GetFloorRoundsQuestionnaires(floor.FloorId, rounditem.TRoundId);
                        foreach (var round in rounds)
                        {
                            foreach (var insp in round.Inspections)
                            {
                                foreach (var quest in insp.Questionnaires)
                                {
                                    if (!string.IsNullOrEmpty(quest.FilePath))
                                    {
                                        PdfPCell cell = new PdfPCell();
                                        cell = new PdfPCell()
                                        {
                                            Border = Rectangle.NO_BORDER,
                                        };
                                        Image questimage = Image.GetInstance(_commonProvider.FilePath(quest.FilePath));
                                        questimage.ScaleAbsolute(250, 120);
                                        cell.AddElement(new Chunk(questimage, 0, 10, true));
                                        //questimage.SetAbsolutePosition(10, 0);
                                        Paragraph para = new Paragraph($"{round.CategoryName}-{round.Inspections.FirstOrDefault().Floor.FloorBuildingLocation}-{quest.RoundsQuestionnaires.RoundStep}", TitleFontS)
                                        {
                                            Alignment = Element.ALIGN_CENTER
                                        };
                                        cell.AddElement(para);
                                        imagetable.AddCell(cell);
                                        imagecount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (imagecount % 2 != 0)
            {
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell()
                {
                    Border = Rectangle.NO_BORDER,
                };
                imagetable.AddCell(cell);
            }

            pdfDoc.Add(imagetable);

            //Print Images in Reports if available//

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }

        private static void RoundBuildingTitle(Document pdfDoc, TRounds rounds)
        {
            PdfPTable buildingtable = new PdfPTable(2)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 10f
            };
            buildingtable.DefaultCell.Border = Rectangle.NO_BORDER;
            buildingtable.AddCell(new Phrase(rounds.CategoryName, CellFontBoldSmall));
            buildingtable.AddCell(new Phrase(rounds.Inspections.FirstOrDefault().Floor.FloorBuildingLocation, CellFontBoldSmall));
            pdfDoc.Add(buildingtable);
        }

        private static PdfPTable RoundReportHeader(Document pdfDoc, ref int j)
        {
            PdfPTable table = new PdfPTable(3)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0
            };
            float[] celwidths = new float[] { 20f, 40f, 40f };
            table.SetWidths(celwidths);
            table.AddCell(new Phrase("Inspected By", CellFontBoldSmall));
            table.AddCell(new Phrase("Questionnaire(s)", CellFontBoldSmall));
            table.AddCell(new Phrase("Comment", CellFontBoldSmall));
            j = j + 1;
            pdfDoc.Add(table);
            return table;
        }

        private PdfPTable SetStepHeading(Document pdfDoc, string title)
        {
            PdfPTable table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0,
                SpacingAfter = 10f
            };
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.AddCell(AddNewCell(title, ParagraphFontlarge, false, 0));
            pdfDoc.Add(table);
            return table;
        }

        public Document RoundInspection_old(int floorId, string roundid, Stream streamOutput)
        {
            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            SetHeaderBlue(out table, "Round Inspection ");
            pdfDoc.Add(table);
            List<TRounds> lstround = new List<TRounds>();

            foreach (string id in roundid.TrimEnd(',').Split(','))
            {
                int rid = Convert.ToInt32(id);
                var rounds = _roundService.GetRoundDetails(rid);
                lstround.Add(rounds);

            }
            int i = 0;
            foreach (var rounditem in lstround)
            {
                var rounds = _roundService.GetFloorRoundsQuestionnaires(floorId, rounditem.TRoundId);

                if (rounds.Inspections.Count > 0)
                {
                    if (i == 0)
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0
                        };
                        table.DefaultCell.Border = Rectangle.NO_BORDER;
                        table.AddCell(new Phrase("Location: " + rounds.Inspections.FirstOrDefault().Floor.FloorLocation, CellFontBoldBlueSmall));

                        pdfDoc.Add(table);
                        List<int> stepsWorders = rounds.RoundWorkOrders.Where(x => x.TRoundId == rounds.TRoundId && x.FloorId == rounds.Inspections.FirstOrDefault().Floor.FloorId).SelectMany(x => x.StepId).ToList();
                        List<int> compstepsWorders = rounds.RoundWorkOrders.Where(x => x.TRoundId == rounds.TRoundId && x.StatusCode == "CMPLT" && x.FloorId == rounds.Inspections.FirstOrDefault().Floor.FloorId).SelectMany(x => x.StepId).ToList();
                        i = i + 1;
                    }
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0
                    };
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.AddCell(new Phrase(rounds.CategoryName + ":", CellFontBoldBlueSmall));
                    pdfDoc.Add(table);
                    foreach (var item in rounds.Inspections.SelectMany(x => x.Questionnaires).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList())
                    {
                        table = new PdfPTable(1)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0
                        };
                        table.DefaultCell.Border = Rectangle.NO_BORDER;
                        var className = "";
                        if (rounds.Inspections.SelectMany(x => x.Questionnaires.Where(a => a.RouQuesId == item.RouQuesId)).Any(y => y.Status == 0))
                        {
                            className = "error";
                        }
                        else
                        {
                        }
                        table.AddCell(new Phrase(item.RoundsQuestionnaires.RoundStep, CellFontBoldBlueSmall));
                        pdfDoc.Add(table);
                        PdfPTable table1 = new PdfPTable(2)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 0
                        };
                        table1.DefaultCell.Border = Rectangle.NO_BORDER;
                        foreach (var item3 in rounds.Inspections)
                        {
                            foreach (var item2 in item3.Questionnaires.Where(x => x.RouQuesId == item.RouQuesId))
                            {
                                if (item2.Status == 1)
                                {
                                    table1.AddCell(new Phrase("Compliant", CellFont));
                                }
                                else if (item2.Status == 0)
                                {
                                    table1.AddCell(new Phrase("Non Compliant", CellFont));
                                }
                                else if (item2.Status == -3)
                                {
                                    table1.AddCell(new Phrase("Non Applicable", CellFont));
                                }
                                else
                                {
                                    table1.AddCell(new Phrase("Pending", CellFont));
                                }


                                table1.DefaultCell.Border = Rectangle.NO_BORDER;
                                table1.AddCell(new Phrase(($"Added by {(item3.User != null && item3.User.UserId > 0 ? item3.User.FullName : string.Empty)}"), CellFont));

                                if (!string.IsNullOrEmpty(item2.Comment))
                                {
                                    table1.AddCell(new Phrase(($"comment:{item2.Comment}"), CellFont));
                                    table1.AddCell(new Phrase("", CellFont));
                                }
                            }
                        }
                        pdfDoc.Add(table1);
                    }
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0
                    };
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    foreach (var Inspection in rounds.Inspections.Where(x => !string.IsNullOrEmpty(x.Comment)))
                    {
                        table.AddCell(new Phrase(($"Comment by: {(Inspection.User != null && Inspection.User.UserId > 0 ? Inspection.User.FullName : string.Empty)}"), CellFont));
                        table.AddCell(new Phrase(Inspection.Comment, CellFont));

                    }

                }
                else
                {



                }


            }
            if (i == 0)
            {
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 20f
                };
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(new Phrase("No Inspection started", CellFontBoldBlueSmall));
                //pdfDoc.Add(table);
            }
            pdfDoc.Add(table);


            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;
        }
        #endregion

        #region AssetDeviceChange
        public string AssetDeviceChangeFormInbytes(string formId)
        {
            var mem = new MemoryStream();
            Document pdfDoc = new Document();

            TAssetDeviceChangeForms newForm = _permitService.GetAssetChangeDeviceFormsById(Guid.Parse(formId));

            pdfDoc = CreateAssetDeviceChangePermit(formId, mem, newForm, true);
            var pdf = mem.ToArray();
            return Convert.ToBase64String(pdf);
        }

        public Document CreateAssetDeviceChangePermit(string formId, Stream streamOutput, TAssetDeviceChangeForms newForm, bool IsAttachmentIncluded)
        {
            if (newForm.AssetDeviceList.Count == 0)
            {
                AssetDeviceList device = new AssetDeviceList();
                newForm.AssetDeviceList.Add(device);
            }

            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
            pdfWriter.PageEvent = new PDFFooter();
            pdfDoc.Open();
            PdfPTable table;
            string approverName = string.Empty;
            if (newForm.Status == 2 || newForm.Status == -1)
            {
                SetHeaderBlue(out table, "Asset Device Change –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            }
            else
            {

                SetStatusHeaderBlue(out table, "Asset Device Change  –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"), Enum.GetName(typeof(HCF.BDO.Enums.ApprovalStatus), newForm.Status).ToString().ToUpper(), approverName, ($"{newForm.CreatedDate:MMM d, yyyy}"));

            }
            //SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
            PdfPTable tableapprove = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = 0

            };
            pdfDoc.Add(table);

            if (newForm != null)
            {
                table = new PdfPTable(6)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 5f

                };

                float[] widths = new float[] { 25f, 25f, 5f, 20f, 5f, 20f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;

                Image ImgCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.CheckBoxImage));
                ImgCheckBox.ScaleAbsolute(10f, 10f);

                Image ImgUnCheckBox = Image.GetInstance(GetFilePath(ImagePathModel.UnCheckBoxImage));
                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

                PdfPCell nobordercell = new PdfPCell()
                {
                    Border = 0,
                };


                Chunk chunk1 = new Chunk();
                table.AddCell(AddNewCell("Section 1: Basic Information", CellFontBoldBlueSmall, false, 6));
                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.PermitNo, CellFont, false));

                pdfDoc.Add(table);
                table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 10f

                };


                widths = new float[] { 25f, 25f, 25f, 25f };
                table.SetTotalWidth(widths);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.ProjectName, CellFont, false));
                table.AddCell(AddNewCell("Project #:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Project.ProjectNumber, CellFont, false));
                table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell($"{newForm.DateOfRequest:MMM d, yyyy}", CellFont, false));
                table.AddCell(AddNewCell("Requestor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.RequestorUser != null ? newForm.RequestorUser.Name : string.Empty, CellFont, false));
                table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Contractor, CellFont, false));
                table.AddCell(AddNewCell("Building(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.BuildingName, CellFont, false));
                table.AddCell(AddNewCell("Floor(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.FloorName, CellFont, false));
                table.AddCell(AddNewCell("Zone(s):", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Zones, CellFont, false));
                table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.EmailAddress, CellFont, false));
                table.AddCell(AddNewCell("Phone:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.PhoneNumber, CellFont, false));
                table.AddCell(AddNewCell("Device Types:", CellFontBoldBlack, false));
                string devicetype = string.Empty;
                if (newForm.DeviceType == 0)
                {
                    devicetype = "Reserved";
                }
                else if (newForm.DeviceType == 1)
                {
                    devicetype = "Lifesafety Device";
                }
                else if (newForm.DeviceType == 2)
                {
                    devicetype = "Facility Device";
                }
                table.AddCell(AddNewCell(devicetype, CellFont, false, 3));
                table.AddCell(AddNewCell("Brief Description of Work:", CellFontBoldBlack, false));
                table.AddCell(AddNewCell(newForm.Description, CellFont, false, 3));

                pdfDoc.Add(table);
                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                //pdfDoc.Add(line);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 10f

                };

                table.AddCell(AddNewCell("Section 2: Asset Device Change List:", CellFontBoldBlueSmall, false));
                pdfDoc.Add(table);
                table = new PdfPTable(7)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingBefore = 5f,
                    SpacingAfter = 5f

                };
                widths = new float[] { 5f, 15f, 15f, 25f, 15f, 20f, 15f };
                table.SetTotalWidth(widths);
                table.AddCell(new Phrase("#", CellFontBoldBlack));
                table.AddCell(new Phrase("Device #", CellFontBoldBlack));
                table.AddCell(new Phrase("CMMSAsset #", CellFontBoldBlack));
                table.AddCell(new Phrase("Serial #", CellFontBoldBlack));
                table.AddCell(new Phrase("Manufacturer", CellFontBoldBlack));
                table.AddCell(new Phrase("Room #", CellFontBoldBlack));
                table.AddCell(new Phrase(newForm.FormType == 1 ? "Date Added " : "Date Removed", CellFontBoldBlack));
                nobordercell = new PdfPCell();
                for (int i = 0; i < newForm.AssetDeviceList.Count; i++)
                {
                    //var type = newForm.FormType ? newForm.FormType.ToString() : string.Empty;
                    //var sequence = this.ViewData.ContainsKey("sequence") ? this.ViewData["sequence"].ToString() : string.Empty;

                    table.AddCell(new Phrase(string.Format("{0}", i + 1), CellFont));
                    table.AddCell(new Phrase(newForm.AssetDeviceList[i].DeviceNumber, CellFont));
                    table.AddCell(new Phrase(newForm.AssetDeviceList[i].CMMSAssetNumber, CellFont));
                    table.AddCell(new Phrase(newForm.AssetDeviceList[i].SerialNumber, CellFont));
                    table.AddCell(new Phrase(newForm.AssetDeviceList[i].Manufacturer, CellFont));
                    table.AddCell(new Phrase(newForm.AssetDeviceList[i].RoomNumber, CellFont));
                    if (newForm.AssetDeviceList[i].DateAdded.HasValue || newForm.AssetDeviceList[i].DateRemoved.HasValue)
                        table.AddCell(new Phrase(newForm.FormType == 1 ? HCF.Utility.CommonUtility.ConvertToFormatDate(newForm.AssetDeviceList[i].DateAdded.Value) : HCF.Utility.CommonUtility.ConvertToFormatDate(newForm.AssetDeviceList[i].DateRemoved.Value), CellFont));
                    else
                        table.AddCell(new Phrase(string.Empty, CellFont));

                }
                pdfDoc.Add(table);
                table = new PdfPTable(1)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = 0,
                    SpacingAfter = 5f

                };

                table.AddCell(AddNewCell("SECTION 3:  Approval  [Hospital Employee’s Only] ", CellFontBoldBlueSmall, false));

                foreach (var path in newForm.AssetDevicePathSettings)
                {
                    table = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f

                    };

                    table.AddCell(AddNewCell(path.Path + ":" + path.DeviceType + ":", CellFontBoldBlueSmall, false));
                    pdfDoc.Add(table);

                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 10f
                    };
                    int twocolcount = 0;
                    foreach (var permitworkflow in newForm.TPermitWorkFlowDetails.Where(x => x.PathId == path.PathId))
                    {
                        if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
                        {
                            PdfPCell cellmain = new PdfPCell()
                            {
                                Border = 0,
                            };
                            cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, permitworkflow.Comment, 45f);
                            table.AddCell(cellmain);

                            twocolcount++;
                        }

                        if (twocolcount == 0)
                        {
                            table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                            twocolcount++;
                        }

                        if (twocolcount % 2 == 0)
                        {
                            table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                            twocolcount++;
                        }

                    }

                    if (twocolcount % 2 != 0)
                    {
                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
                    }

                    pdfDoc.Add(table);
                }

                if (newForm.Status == 2)
                {
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell("Requested", CellFont, false));
                }
                else if (newForm.Status == 1)
                {
                    table = new PdfPTable(3)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    widths = new float[] { 60f, 20f, 20f };
                    table.SetTotalWidth(widths);


                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell("Approved", CellFont, false));
                }
                else
                {
                    table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 0,
                        SpacingBefore = 5f,



                    };
                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Rejected" : "On Hold/Pending", CellFont, false));

                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Reason(s) of Rejection" : "Reason(s) of on Hold/Pending", CellFontBoldBlack, false));
                    table.AddCell(AddNewCell(newForm.Reason, CellFont, false));
                }
                pdfDoc.Add(table);

                bool isattach = false;
                if (IsAttachmentIncluded)
                {
                    if (newForm.DrawingAttachments.Count > 0 || newForm.Attachments.Count > 0)
                    {
                        pdfDoc.NewPage();
                        isattach = true;
                    }
                    if (isattach)
                    {
                        if (newForm.Attachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingAfter = 10f

                            };

                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
                            for (int i = 0; i < newForm.Attachments.Count; i++)
                            {
                                AddAttachmentCell(newForm.Attachments[i].FilePath, newForm.Attachments[i].FileName, pdfWriter, table);


                            }
                            pdfDoc.Add(table);
                        }
                        if (newForm.DrawingAttachments.Count > 0)
                        {
                            table = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                HorizontalAlignment = 0,
                                SpacingAfter = 10f

                            };

                            table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
                            for (int i = 0; i < newForm.DrawingAttachments.Count; i++)
                            {
                                AddAttachmentCell(newForm.DrawingAttachments[i].ImagePath, newForm.DrawingAttachments[i].FullFileName, pdfWriter, table);
                            }
                            pdfDoc.Add(table);
                        }
                    }
                }
            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            return pdfDoc;

        }
        #endregion

        #endregion

        #region Footer 

        public class PDFFooter : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate footerTemplate;
            BaseFont bf = null;

            private readonly string FileTitle;

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
                footerTemplate = cb.CreateTemplate(50, 50);
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
                tabFot.AddCell(cell);
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
                return $"{ConstMessage.PrintGeneratedByTitle}: {ConstMessage.PrintGeneratedFromText}";
            }

            public string FooterGeneratedDate()
            {
                return $"{ConstMessage.PrintGeneratedDateTitle}: {DateTime.Now.ToShortDateString()}";
            }

            public string FooterMsg()
            {
                return $"{ConstMessage.PrintGeneratedFromText} {" on "} {CommonUtility.ConvertToFormatDate(DateTime.Now)}";
            }
        }

        #endregion

        #region Set Paper size

        public Rectangle SetLandScapePaperType()
        {
            return PageSize.A4.Rotate();
        }

        #endregion
    }
}
