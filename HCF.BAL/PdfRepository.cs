//using System;
//using System.IO;
//using System.Linq;
//using System.Net;
//using HCF.BDO;
//using HCF.Utility;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

//namespace HCF.BAL
//{
//    public static class PdfRepository
//    {
//        #region Color
//        //
//        //public static readonly System.Drawing.Color navybluecolor = System.Drawing.Color.FromArgb(0, 0, 128);
//        //public static readonly BaseColor navyblue = new BaseColor(navybluecolor);
//        public static readonly System.Drawing.Color GrayColor = System.Drawing.Color.FromArgb(231, 231, 231);
//        public static readonly BaseColor Gray = new BaseColor(GrayColor);

//        public static readonly System.Drawing.Color CyanColor = System.Drawing.Color.FromArgb(0, 162, 232);
//        public static readonly BaseColor CYAN = new BaseColor(CyanColor);

//        #endregion

//        #region Font
//        public static readonly Font CellFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
//        public static readonly Font CellBoldFont = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
//        public static readonly Font smallfont = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);
//        public static readonly Font smallfontbold = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
//        public static readonly Font CellFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
//        public static readonly Font UnderlineTitleBoldFont = new Font(Font.FontFamily.HELVETICA, 11.5f, Font.BOLD | Font.UNDERLINE);
//        public static readonly Font UnderlineCellBoldFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD | Font.UNDERLINE);
//        public static readonly Font UnderlineCellFont = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.UNDERLINE);
//        public static readonly Font CellFontSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
//        public static readonly Font CellFontDatetimeblue = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, CYAN);
//        public static readonly Font CellFontBold = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD);
//        public static readonly Font TitleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.NORMAL);
//        public static readonly Font TitleFontS = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
//        public static readonly Font UnderlineTitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.UNDERLINE);
//        public static readonly Font ParagraphFont = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL);
//        public static readonly Font ParagraphFontS = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL);
//        public static readonly Font Strikethru = new Font(Font.FontFamily.HELVETICA, 11, Font.STRIKETHRU);
//        public static readonly Font CellFontBoldSmall = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD);
//        public static readonly Font CellFontSmall1 = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.NORMAL);
//        public static readonly Font CellFontSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.NORMAL);
//        public static readonly Font CellFontBoldSmall2 = new Font(Font.FontFamily.HELVETICA, 6.5f, Font.BOLD, BaseColor.BLACK);
//        public static readonly Font CellFontBoldBlue = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
//        public static readonly Font CellFontBoldBlack = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD);
//        public static readonly Font CellFontBoldBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.BLACK);
//        public static readonly Font CellFontNormalBlueSmall = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, BaseColor.BLACK);
//        public static readonly Font CellStatusFont = new Font(Font.FontFamily.HELVETICA, 14f, Font.BOLD, BaseColor.RED);
//        public static readonly Font CellRedtext = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.RED);
//        public static readonly Font CellFontWhiteBold = new Font(Font.FontFamily.HELVETICA, 10.5f, Font.BOLD, BaseColor.WHITE);
//        public static readonly Font TitleFontHWP = new Font(Font.FontFamily.HELVETICA, 76f, Font.NORMAL, BaseColor.WHITE);
//        public static readonly Font TitleFont2HWP = new Font(Font.FontFamily.HELVETICA, 35f, Font.BOLD, BaseColor.BLACK);


//        #endregion

//        #region  Common Methods
//        public static void SetHeader(out PdfPTable table)
//        {
//            BDO.Organization CurrentOrg = DAL.OrganizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Utility.HcfSession.ClientNo);
//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                //0=Left, 1=Centre, 2=Right
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,
//                //SpacingAfter = 10f
//            };

//            //Cell no 1
//            PdfPCell cell = new PdfPCell
//            {
//                Border = 0
//            };
//            ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            Image image = Image.GetInstance(CurrentOrg.LogoUrl);
//            image.ScaleAbsolute(100, 80);
//            cell.AddElement(image);
//            table.AddCell(cell);

//            Chunk chunk = new Chunk("" + CurrentOrg.Name, FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK));
//            Chunk chunk1 = new Chunk("" + CurrentOrg.Address + "", FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK));
//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.AddElement(chunk);
//            cell.AddElement(chunk1);

//            table.AddCell(cell);

//            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//            //PdfPCell cell3 = new PdfPCell
//            //{
//            //    Border = 0,
//            //    Colspan = 2,
//            //};
//            //cell3.AddElement(line);
//            //table.AddCell(cell3);
//        }
//        public static void SetHeaderBlue(out PdfPTable table, string Heading)
//        {
//            BDO.Organization CurrentOrg = DAL.OrganizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Utility.HcfSession.ClientNo);
//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                //0=Left, 1=Centre, 2=Right
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,
//                //SpacingAfter = 10f
//            };

//            float[] widths = new float[] { 15f, 5f, 80f };
//            table.SetTotalWidth(widths);

//            //Cell no 1
//            PdfPCell cell = new PdfPCell
//            {
//                Border = 0
//            };
//            System.Net.ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            Image image = Image.GetInstance(CurrentOrg.LogoUrl);
//            image.ScaleAbsolute(80, 80);
//            image.SetAbsolutePosition(10, 0);
//            cell.AddElement(image);
//            table.AddCell(cell);

//            PdfPCell cell2 = new PdfPCell
//            {
//                Border = PdfPCell.RIGHT_BORDER,
//                BorderColor = BaseColor.BLACK,
//                BorderWidth = 3f
//            };
//            table.AddCell(cell2);

//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.Padding = 0;
//            Chunk chunk1 = new Chunk("   " + Heading + "", CellFontBoldBlue);
//            var para = new Paragraph(chunk1)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            cell.AddElement(chunk1);
//            table.AddCell(cell);

//            cell2 = new PdfPCell();
//            cell2.UseVariableBorders = true;
//            cell2.Colspan = 4;
//            cell2.Border = PdfPCell.TOP_BORDER;
//            cell2.BorderColor = BaseColor.BLACK;
//            cell2.BorderWidth = 3f;
//            cell2.Padding = 2;
//            table.AddCell(cell2);


//        }

//        public static void SetStatusHeaderBlue(out PdfPTable table, string Heading, string Status, string ApproveBy, string Date)
//        {
//            ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            BDO.Organization CurrentOrg = DAL.OrganizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Utility.HcfSession.ClientNo);
//            table = new PdfPTable(4)
//            {
//                WidthPercentage = 100,
//                //0=Left, 1=Centre, 2=Right
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,
//                //SpacingAfter = 10f
//            };

//            float[] widths = new float[] { 15f, 5f, 65f, 25f };
//            table.SetTotalWidth(widths);

//            //Cell no 1
//            PdfPCell cell = new PdfPCell
//            {
//                Border = 0
//            };
//            Image image = Image.GetInstance(CurrentOrg.LogoUrl);
//            image.ScaleAbsolute(80, 80);
//            image.SetAbsolutePosition(10, 0);
//            cell.AddElement(image);
//            table.AddCell(cell);

//            PdfPCell cell2 = new PdfPCell
//            {
//                Border = PdfPCell.RIGHT_BORDER,
//                BorderColor = BaseColor.BLACK,
//                BorderWidth = 3f
//            };
//            table.AddCell(cell2);

//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.Padding = 0;
//            Chunk chunk1 = new Chunk("   " + Heading + "", CellFontBoldBlue);
//            var para = new Paragraph(chunk1);
//            para.Alignment = Element.ALIGN_LEFT;
//            cell.AddElement(chunk1);
//            table.AddCell(cell);
//            //cell = new PdfPCell
//            //{
//            //    Border = 0
//            //};
//            //cell.Padding = 0;
//            //cell.Colspan = 3;
//            //Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//            //  table.AddCell(line1);

//            PdfPTable table2 = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingAfter = 5f

//            };
//            //table2.DefaultCell.Border = 1;
//            widths = new float[] { 40, 5, 45 };
//            table2.SetTotalWidth(widths);
//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.Colspan = 3;
//            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Status) ? "       " + Status : " ", CellStatusFont));
//            table2.AddCell(cell);
//            cell2 = new PdfPCell();
//            cell2.UseVariableBorders = true;
//            cell2.BorderColor = BaseColor.BLACK;
//            cell2.BorderWidth = 1f;
//            cell2.Border = PdfPCell.BOTTOM_BORDER;
//            cell2.Colspan = 3;
//            table2.AddCell(cell2);
//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.AddElement(new Phrase(!string.IsNullOrEmpty(ApproveBy) ? ApproveBy : " ", CellFontNormalBlueSmall));
//            table2.AddCell(cell);
//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.AddElement(new Phrase("|", CellFontNormalBlueSmall));
//            table2.AddCell(cell);
//            cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Date) ? Date : " ", CellFontNormalBlueSmall));
//            table2.AddCell(cell);
//            cell2 = new PdfPCell();
//            cell2.Border = PdfPCell.BOX;
//            cell2.BorderWidth = 2f;

//            cell2.AddElement(table2);

//            table.AddCell(cell2);

//            cell2 = new PdfPCell
//            {
//                UseVariableBorders = true,
//                Colspan = 4,
//                Border = PdfPCell.TOP_BORDER,
//                BorderColor = BaseColor.BLACK,
//                BorderWidth = 3f
//            };
//            table.AddCell(cell2);


//        }

//        public static Rectangle SetPaperType()
//        {
//            return PageSize.LETTER;
//        }

//        private static void AddCell(PdfPTable table, string text, int rowspan)
//        {
//            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
//            iTextSharp.text.Font times = new Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

//            PdfPCell cell = new PdfPCell(new Phrase(text, times))
//            {
//                Rowspan = rowspan,
//                HorizontalAlignment = PdfPCell.ALIGN_CENTER,
//                VerticalAlignment = PdfPCell.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//        }

//        #endregion       

//        #region Generate Firedrill Reports

//        public static string GenerateFireDrillReportInbytes(int TExerciseId)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            pdfDoc = GenerateFireDrillReport(TExerciseId, mem);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        private static Document GenerateFireDrillReport(int TExerciseId, Stream streamOutput)
//        {
//            TExercises objExercise = new TExercises();
//            if (TExerciseId > 0)
//            {
//                objExercise = DAL.ExercisesRepository.GetExercises(TExerciseId).FirstOrDefault(); //.FirstOrDefault(x => x.TExerciseId == TExerciseId);
//            }
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            SetHeader(out table);
//            pdfDoc.Add(table);


//            Paragraph para = new Paragraph("FIRE DRILL DOCUMENTATION", UnderlineTitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(4)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };

//            //table.AddCell(new Phrase("Department", CellFont));
//            table.AddCell(new Phrase("Shift:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.ShiftId.ToString(), CellFont));

//            table.AddCell(new Phrase("Date:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.Date.Value.ToString("ddd, MMM d, yyyy"), CellFont));

//            table.AddCell(new Phrase("Time:", TitleFontS));
//            if (objExercise.Time.HasValue)
//            {
//                DateTime time = DateTime.Today.Add(objExercise.Time.Value);
//                string StartTime = time.ToString("hh:mm tt");
//                table.AddCell(new Phrase(StartTime, CellFont));
//            }
//            else { table.AddCell(""); }


//            if (objExercise.IsAudible == false)
//            {
//                objExercise.TExerciseQuestionnaires = objExercise.TExerciseQuestionnaires.Where(x => x.FireDrillQuestionnaires.FireDrillCategory.CategoryName != "Fire Alarm Equipment & Response").ToList();
//            }

//            table.AddCell(new Phrase("Building:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.Building.BuildingName, CellFont));

//            table.AddCell(new Phrase("Location:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.NearBy, CellFont));

//            table.AddCell(new Phrase("Drill Conducted By:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.ConductedBy, CellFont));

//            table.AddCell(new Phrase("Observers:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.Observers, CellFont));

//            table.AddCell(new Phrase("Unannounced:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.Announced ? "Yes" : "No", CellFont));

//            table.AddCell(new Phrase("Audible Notification:", TitleFontS));
//            table.AddCell(new Phrase(objExercise.IsAudible ? "Yes" : "No", CellFont));

//            table.AddCell(new Phrase("", CellFont));
//            table.AddCell(new Phrase("", CellFont));


//            pdfDoc.Add(table);

//            para = new Paragraph("Evaluation:", UnderlineTitleFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);
//            var widths = new float[] { 32f, 6f, 6f, 16f };
//            table = new PdfPTable(4)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.SetWidths(widths);
//            table.AddCell(new Phrase("Questions", CellFont));
//            table.AddCell(new Phrase("Status", CellFont));
//            table.AddCell(new Phrase("Score (out of 5)", CellFont));
//            table.AddCell(new Phrase("Comment", CellFont));
//            var previouscategoryname = "";
//            int overallScore = 0;
//            foreach (var item in objExercise.TExerciseQuestionnaires)
//            {
//                var currentcategoryname = item.FireDrillQuestionnaires.FireDrillCategory.CategoryName;
//                if (currentcategoryname != null && currentcategoryname != "" && previouscategoryname == "")
//                {
//                    PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellFontBold));
//                    Categoryname.Colspan = 4;
//                    table.AddCell(Categoryname);
//                    previouscategoryname = currentcategoryname;
//                }
//                else if (currentcategoryname != previouscategoryname)
//                {
//                    PdfPCell Categoryname = new PdfPCell(new Phrase(currentcategoryname, CellFontBold));
//                    Categoryname.Colspan = 4;
//                    table.AddCell(Categoryname);
//                    previouscategoryname = currentcategoryname;
//                }
//                HCF.Utility.Enums.FireDrillQuesStatus enums = (Enums.FireDrillQuesStatus)item.Status;
//                table.AddCell(new Phrase(item.FireDrillQuestionnaires.Questionnaries, CellFont));
//                table.AddCell(new Phrase(enums.ToString(), CellFont));
//                table.AddCell(new Phrase(item.Ratings.HasValue ? item.Ratings.Value.ToString() : "", CellFont));
//                table.AddCell(new Phrase(item.Comments, CellFont));
//                overallScore = overallScore + (item.Ratings.HasValue ? Convert.ToInt32(item.Ratings.Value) : 0);
//            }
//            pdfDoc.Add(table);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.AddCell(new Phrase("Overall Score ", CellFont));
//            table.AddCell(new Phrase(overallScore.ToString(), CellFont));
//            table.AddCell(new Phrase("Final Comment", CellFont));
//            table.AddCell(new Phrase(objExercise.Comment, CellFont));

//            pdfDoc.Add(table);

//            //pdfDoc.NewPage();
//            para = new Paragraph("Evaluation Signature", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };

//            pdfDoc.Add(para);

//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            //table.AddCell(new Phrase("Evaluation Signature", CellFont));
//            foreach (var item in objExercise.DigitalSignature)
//            {
//                PdfPCell cell = new PdfPCell();
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Image image = Image.GetInstance(Common.FilePath(item.FilePath));
//                //image.ScaleAbsolute(150, 30);
//                //cell.AddElement(image);
//                cell.AddElement(new Chunk(image, 0, 0));
//                table.AddCell(cell);
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                table.AddCell(cell);
//            }
//            pdfDoc.Add(table);

//            para = new Paragraph("Evaluation Documents:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Evaluation && x.DocumentType == 0).ToList().Count > 0)
//            {
//                pdfDoc.NewPage();

//                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Evaluation && x.DocumentType == 0).ToList())
//                {
//                    para = new Paragraph(item.FileName, CellFont)
//                    {
//                        Alignment = Element.ALIGN_LEFT
//                    };
//                    pdfDoc.Add(para);

//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f,
//                        SpacingAfter = 30f
//                    };
//                    var reader = new PdfReader(new Uri(Common.FilePath(item.FilePath)));
//                    PdfReader.unethicalreading = true;
//                    for (var i = 1; i <= reader.NumberOfPages; i++)
//                    {
//                        var page = pdfWriter.GetImportedPage(reader, i);
//                        Image img = Image.GetInstance(page);
//                        PdfPCell cell = new PdfPCell(img, true);
//                        table.AddCell(cell);
//                    }
//                    pdfDoc.Add(table);
//                    //reader.Close();
//                }
//            }

//            pdfDoc.NewPage();

//            para = new Paragraph("Critique:", UnderlineTitleFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.AddCell(new Phrase("Critique Date "));
//            table.AddCell(new Phrase(objExercise.CritiqueDate.HasValue ? objExercise.CritiqueDate.Value.ToString("ddd, MMM d, yyyy") : objExercise.Date.Value.ToString("ddd, MMM d, yyyy")));
//            table.AddCell(new Phrase("Comments/Critique"));
//            table.AddCell(new Phrase(objExercise.CritiquesComment));

//            pdfDoc.Add(table);

//            para = new Paragraph("Issue And Action:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };

//            table.AddCell(new Phrase("Issue", CellFont));
//            table.AddCell(new Phrase("Recommendation/Action to be Taken", CellFont));

//            foreach (var item in objExercise.TExerciseActions)
//            {
//                table.AddCell(new Phrase(item.Issue, CellFont));
//                table.AddCell(new Phrase(item.Action, CellFont));
//            }
//            pdfDoc.Add(table);

//            para = new Paragraph("Critique Signature", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };

//            //table.AddCell(new Phrase("Critique Signature", CellFont));
//            foreach (var item in objExercise.CritiqueDigitalSignature)
//            {
//                PdfPCell cell = new PdfPCell();
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Image image = Image.GetInstance(Common.FilePath(item.FilePath));
//                //image.ScaleAbsolute(150, 30);
//                //cell.AddElement(image);
//                cell.AddElement(new Chunk(image, 0, 0));
//                table.AddCell(cell);
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                table.AddCell(cell);

//            }
//            pdfDoc.Add(table);

//            para = new Paragraph("Critique Documents:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Critique && x.DocumentType == 0).ToList().Count > 0)
//            {
//                pdfDoc.NewPage();
//                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Critique && x.DocumentType == 0).ToList())
//                {
//                    para = new Paragraph(item.FileName, CellFont)
//                    {
//                        Alignment = Element.ALIGN_LEFT
//                    };

//                    pdfDoc.Add(para);
//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f,
//                        SpacingAfter = 30f
//                    };
//                    var reader = new PdfReader(new Uri(Common.FilePath(item.FilePath)));
//                    for (var i = 1; i <= reader.NumberOfPages; i++)
//                    {
//                        var page = pdfWriter.GetImportedPage(reader, i);
//                        Image img = Image.GetInstance(page);
//                        PdfPCell cell = new PdfPCell(img, true);
//                        table.AddCell(cell);
//                    }
//                    pdfDoc.Add(table);
//                    //reader.Close();
//                }
//            }

//            pdfDoc.NewPage();

//            para = new Paragraph("Education:", UnderlineTitleFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.AddCell(new Phrase("Education Date "));
//            table.AddCell(new Phrase(objExercise.EducationDate.HasValue ? objExercise.EducationDate.Value.ToString("ddd, MMM d, yyyy") : objExercise.Date.Value.ToString("ddd, MMM d, yyyy")));
//            table.AddCell(new Phrase("Comments"));
//            table.AddCell(new Phrase(objExercise.EducationComment));

//            pdfDoc.Add(table);

//            para = new Paragraph("Education Signature", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };

//            pdfDoc.Add(para);
//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            //table.AddCell(new Phrase("Education Signature", CellFont));
//            foreach (var item in objExercise.EducationDigitalSignature)
//            {
//                PdfPCell cell = new PdfPCell();
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Image image = Image.GetInstance(Common.FilePath(item.FilePath));
//                //image.ScaleAbsolute(150, 30);
//                //cell.AddElement(image);
//                cell.AddElement(new Chunk(image, 0, 0));
//                table.AddCell(cell);
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                cell.AddElement(new Phrase(item.SignByUserName + " (" + item.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                table.AddCell(cell);

//            }
//            pdfDoc.Add(table);

//            para = new Paragraph("Education Documents:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            if (objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Education && x.DocumentType == 0).ToList().Count > 0)
//            {
//                pdfDoc.NewPage();
//                foreach (var item in objExercise.TExerciseFiles.Where(x => x.DrillFileType == Enums.DrillFileType.Education && x.DocumentType == 0).ToList())
//                {
//                    para = new Paragraph(item.FileName, CellFont)
//                    {
//                        Alignment = Element.ALIGN_LEFT
//                    };

//                    pdfDoc.Add(para);
//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f,
//                        SpacingAfter = 30f
//                    };
//                    var reader = new PdfReader(new Uri(Common.FilePath(item.FilePath)));
//                    for (var i = 1; i <= reader.NumberOfPages; i++)
//                    {
//                        var page = pdfWriter.GetImportedPage(reader, i);
//                        Image img = Image.GetInstance(page);
//                        PdfPCell cell = new PdfPCell(img, true);
//                        table.AddCell(cell);
//                    }
//                    pdfDoc.Add(table);
//                }
//            }

//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;

//        }

//        #endregion

//        #region Create Fire drill lMatrix

//        public static string PrintFiredrillMatrixInbytes(int buildingTypeId, int quarterNo, int year)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            pdfDoc = CreateFiredrillMatrix(buildingTypeId, quarterNo, year, mem);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        private static Document CreateFiredrillMatrix(int buildingTypeId, int quarterNo, int year, Stream streamOutput)
//        {
//            var quater = DAL.ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterNo, year, false).FirstOrDefault();

//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            SetHeader(out PdfPTable table);
//            pdfDoc.Add(table);


//            Paragraph para = new Paragraph("FIRE DRILL MATRIX", TitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);

//            para = new Paragraph("Year: " + quater.Year, CellFontBoldSmall)
//            {
//                Alignment = Element.ALIGN_RIGHT
//            };
//            pdfDoc.Add(para);
//            int quaterno = 0;
//            //string quatername = "";
//            if (quarterNo > 0)
//            {
//                quaterno = Convert.ToInt32(quarterNo);

//                if (quaterno == 1)
//                {
//                    // quatername = "Q1(Jan-Mar)";
//                    para = new Paragraph("Q1 (Jan-Mar)", CellFontSmall1)
//                    {
//                        Alignment = Element.ALIGN_RIGHT
//                    };
//                }
//                else if (quaterno == 2)
//                {
//                    //quatername = "Q2(Apr-Jun)";
//                    para = new Paragraph("Q2 (Apr-Jun)", CellFontSmall1)
//                    {
//                        Alignment = Element.ALIGN_RIGHT
//                    };
//                }
//                else if (quaterno == 3)
//                {
//                    //quatername = " Q3(Jul - Sep)";
//                    para = new Paragraph("Q3 (Jul-Sep)", CellFontSmall1)
//                    {
//                        Alignment = Element.ALIGN_RIGHT
//                    };
//                }
//                else if (quaterno == 4)
//                {
//                    // quatername = "Q4(Oct-Dec)";
//                    para = new Paragraph("Q4 (Oct-Dec)", CellFontSmall1)
//                    {
//                        Alignment = Element.ALIGN_RIGHT
//                    };
//                }
//            }

//            pdfDoc.Add(para);

//            //table = new PdfPTable(7)
//            //{
//            //    WidthPercentage = 100,
//            //    HorizontalAlignment = 0,
//            //    SpacingBefore = 20f,
//            //};
//            //table.SetWidths(new float[] { 17f, 15f, 15f, 8f, 15f, 15f, 15f });
//            //table.AddCell(new PdfPCell(new Phrase("Planned Drills", CellFontBoldSmall)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //table.AddCell(new PdfPCell(new Phrase("Total Drills:" + quater.TotalPlanned, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            ////table.AddCell(new PdfPCell(new Phrase("Total Unannounced:" + quater.TotalUnAnnounced, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //Image Img = Image.GetInstance(HCF.BDO.ImagePathModel.UnAnnouncedIcon);
//            //Img.ScaleAbsolute(20f, 20f);
//            //if (quater.TotalUnAnnounced > 0 && quater.TotalPlanned > 0)
//            //{
//            //    var data = quater.TotalUnAnnounced * 100 / quater.TotalPlanned;
//            //    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(data) + "(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

//            //}
//            //else
//            //{

//            //    table.AddCell(new PdfPCell(new Phrase("(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //}
//            //PdfPCell cellnoborder = new PdfPCell(new Phrase("", CellFont))
//            //{
//            //    Border = 0,
//            //};
//            //table.AddCell(cellnoborder);

//            //table.AddCell(new PdfPCell(new Phrase("Actual Drills", CellFontBoldSmall)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //table.AddCell(new PdfPCell(new Phrase("Total Drills:" + quater.TotalDone, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            ////table.AddCell(new PdfPCell(new Phrase("Total Unannounced:" + quater.TotalUnAnnouncedDone, CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //Img = Image.GetInstance(HCF.BDO.ImagePathModel.UnAnnouncedIcon);
//            //Img.ScaleAbsolute(20f, 20f);
//            //if (quater.TotalUnAnnouncedDone > 0 && quater.TotalDone > 0)
//            //{
//            //    var donePer = quater.TotalUnAnnouncedDone * 100 / quater.TotalDone;
//            //    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(donePer) + "(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });

//            //}
//            //else
//            //{

//            //    table.AddCell(new PdfPCell(new Phrase("(%)" + new Chunk(Img, 35, 0), CellFontSmall1)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            //}

//            //pdfDoc.Add(table);


//            table = new PdfPTable(5)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.SetWidths(new float[] { 15f, 10f, 25f, 25f, 25f });
//            table.AddCell(new Phrase("", CellFont));
//            table.AddCell(new Phrase("", CellFont));
//            table.AddCell(new PdfPCell(new Phrase("Shift 1")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            table.AddCell(new PdfPCell(new Phrase("Shift 2")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//            table.AddCell(new PdfPCell(new Phrase("Shift 3")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });


//            PdfPCell cell = new PdfPCell();
//            Image image1 = Image.GetInstance(HCF.BDO.ImagePathModel.VendorBuildingIcon);
//            image1.ScaleAbsolute(20f, 20f);

//            Image image2 = Image.GetInstance(HCF.BDO.ImagePathModel.CalendarIcon);
//            image2.ScaleAbsolute(20f, 20f);

//            PdfPCell cell2 = new PdfPCell();

//            Image image3 = Image.GetInstance(HCF.BDO.ImagePathModel.ClockIcon);
//            image3.ScaleAbsolute(20f, 20f);

//            Image image4 = Image.GetInstance(HCF.BDO.ImagePathModel.AnnounceIcon);
//            image4.ScaleAbsolute(20f, 20f);

//            Image image5 = Image.GetInstance(HCF.BDO.ImagePathModel.TickIcon);
//            image5.ScaleAbsolute(20f, 20f);

//            Image image6 = Image.GetInstance(HCF.BDO.ImagePathModel.DashboardIcon);
//            image6.ScaleAbsolute(20f, 20f);

//            //Shift 1

//            Paragraph icon = new Paragraph
//            {
//                new Chunk(image1, -5, 0)
//            };
//            cell.AddElement(image1);
//            table.AddCell(cell);

//            table.AddCell(new Phrase("", CellFont));


//            Paragraph p = new Paragraph
//            {
//                new Chunk(image2, 15, 0),
//                new Chunk(image3, 35, 0),
//                new Chunk(image4, 55, 0),
//                new Chunk(image5, 70, 0)
//            };
//            //p.Add(new Chunk(image6, -1, 0));
//            cell2.AddElement(p);
//            table.AddCell(cell2);
//            table.AddCell(cell2);
//            table.AddCell(cell2);

//            //pdfDoc.Add(table);
//            for (var i = 0; i < quater.Buildings.Count(); i++)
//            {
//                AddCell(table, quater.Buildings[i].BuildingName, quater.FireDrillTypes.Count);
//                for (var kk = 0; kk < quater.FireDrillTypes.Count; kk++)
//                {
//                    var ii = quater.FireDrillTypes[kk].Id;
//                    AddCell(table, quater.FireDrillTypes[kk].FireDrillType, 1);
//                    for (var j = 0; j < quater.Buildings[i].Shifts.Count(); j++)
//                    {
//                        string date = string.Empty;
//                        if (quater.Buildings[i].Shifts[j].Exercises[ii].Date.HasValue && quater.Buildings[i].Shifts[j].Exercises[ii].Date != null)
//                        {
//                            date = quater.Buildings[i].Shifts[j].Exercises[ii].Date.Value.ToString("ddd, MMM d, yyyy");
//                            string IsAnnounced = quater.Buildings[i].Shifts[j].Exercises[ii].Announced ? "Yes" : "No";
//                            string IsDone = quater.Buildings[i].Shifts[j].Exercises[ii].Status == 0 ? "Plan" : "Done";
//                            string firedrillstring = $"{date + " "} {quater.Buildings[i].Shifts[j].Exercises[ii].StartTime + " "} {IsAnnounced + " "} {IsDone} {"\n"} {quater.Buildings[i].Shifts[j].Exercises[ii].NearBy}";
//                            AddCell(table, firedrillstring, 1);
//                        }
//                        else { AddCell(table, "", 1); }
//                        //addCell(table, "Wed, Mar 18, 2020. 03:20 YES PLAN ?", 1);                     
//                    }
//                }
//            }
//            pdfDoc.Add(table);
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }

//        #endregion Create Firedrill Matrix      

//        #region Create Assets Reports

//        public static string PrintAssetsReportsInbytes(int userId, int epdetailId, ref EPDetails epdetails)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            //pdfDoc = CreateAssetsReports(userId, epdetailId, mem, ref epdetails);
//            pdfDoc = CreateAssetsInspectionReport(userId, epdetailId, mem, ref epdetails);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        #endregion Create Assets Reports

//        #region

//        private static Document CreateAssetsReports(int userId, int epdetailId, Stream streamOutput, ref EPDetails epDetail)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            SetHeader(out PdfPTable table);
//            pdfDoc.Add(table);
//            epDetail = DAL.EpRepository.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(epdetailId), 0);
//            if (epDetail.TInspectionActivity != null && epDetail.TInspectionActivity.Count > 0)
//            {
//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 20f,
//                    SpacingAfter = 30f
//                };
//                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} : {1}", epDetail.Standard.TJCStandard, epDetail.Standard.TJCDescription))));
//                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} : {1}", epDetail.EPNumber, epDetail.Description))));

//                pdfDoc.Add(table);
//                table = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 20f,
//                    SpacingAfter = 30f
//                };
//                table.SetWidths(new float[] { 25f, 25f, 25f, 25f });
//                //table.AddCell(new PdfPCell(new Phrase("Asset")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//                table.AddCell(new PdfPCell(new Phrase("Asset #")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//                table.AddCell(new PdfPCell(new Phrase("Status")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//                table.AddCell(new PdfPCell(new Phrase("Comment")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//                table.AddCell(new PdfPCell(new Phrase("Inspection Date")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
//                var assetsActivity = (from p in epDetail.TInspectionActivity.Where(x => x.ActivityType == 2)
//                                      select new
//                                      {
//                                          //asset = p.TFloorAssets.Assets.Name,
//                                          status = p.Status,
//                                          assetNo = p.TFloorAssets.AssetNo,
//                                          comment = p.Comment,
//                                          inspectiondate = p.ActivityInspectionDate
//                                      }).ToList().OrderBy(x => x.assetNo);
//                foreach (var item in assetsActivity)
//                {
//                    string statustext = item.status == 1 ? "Compliant" : item.status == 0 ? "Non Compliant" : "Pending";
//                    //table.AddCell(new Phrase(item.asset, CellFont));
//                    table.AddCell(new Phrase(item.assetNo, CellFont));
//                    table.AddCell(new Phrase(statustext, CellFont));
//                    table.AddCell(new Phrase(item.inspectiondate.ToString(), CellFont));
//                    table.AddCell(new Phrase(item.comment, CellFont));
//                }
//                pdfDoc.Add(table);
//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }

//        public static Document CreateAssetsInspectionReport(int userId, int? selEpdetailId, Stream streamOutput, ref EPDetails epDetail)
//        {
//            BDO.Organization CurrentOrg = DAL.OrganizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == HcfSession.ClientNo);
//            //var tfloorAssets = new List<TFloorAssets>();
//            var ep = new EPDetails();
//            //var assets = new Assets();

//            if (selEpdetailId.HasValue && selEpdetailId.Value > 0)
//                ep = DAL.EpRepository.GetInspectionHistory(Convert.ToInt32(userId), Convert.ToInt32(selEpdetailId), 0);

//            var assetsActivity = (from p in ep.TInspectionActivity.Where(x => x.ActivityType == 2)
//                                  select new
//                                  {
//                                      assetName = p.TFloorAssets.Assets.Name,
//                                      tfloorAssets = p.TFloorAssets,
//                                      status = p.Status,
//                                      assetNo = p.TFloorAssets.DeviceNo,
//                                      comment = p.Comment,
//                                      inspectiondate = p.ActivityInspectionDate != null ? p.ActivityInspectionDate : null,
//                                      nearby = p.TFloorAssets.NearBy,
//                                      inspectorName = p.UserProfile != null ? p.UserProfile.FullName : string.Empty
//                                  }).ToList().OrderBy(x => x.assetNo).ToList();

//            var assetTypeName = "";
//            if (assetsActivity.Count > 0)
//                assetTypeName = assetsActivity.FirstOrDefault().tfloorAssets.Assets.Name;


//            Document pdfDoc = new Document(SetLandScapePaperType(), 10, 10, 25, 25);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            PdfPCell cell = new PdfPCell();
//            Phrase phrase = new Phrase();
//            SetHeader(out table);
//            pdfDoc.Add(table);

//            Paragraph para = new Paragraph("Inspection Summary", TitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(6)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };

//            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;

//            table.AddCell(new Phrase("Asset Type", CellFontBoldBlue));
//            table.AddCell(new Phrase("Inventory Total", CellFontBoldBlue));
//            table.AddCell(new Phrase("Total Tested", CellFontBoldBlue));
//            table.AddCell(new Phrase("Pass", CellFontBoldBlue));
//            table.AddCell(new Phrase("Fail", CellFontBoldBlue));
//            table.AddCell(new Phrase("% Passed", CellFontBoldBlue));

//            table.DefaultCell.BackgroundColor = BaseColor.WHITE;

//            var totalAssets = assetsActivity.Count;
//            var totaltested = 0;
//            var totalPass = 0;
//            var totalFailed = 0;
//            var totalpassedPercent = 0;
//            if (totalAssets > 0)
//            {
//                var latestTfloorAssetActivity = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).OrderByDescending(x => x.ActivityInspectionDate).GroupBy(test => test.FloorAssetId)
//                       .Select(grp => grp.First());

//                totaltested = latestTfloorAssetActivity.Count(y => y.IsCurrent);
//                totalPass = latestTfloorAssetActivity.Count(y => y.Status == 1 && y.IsCurrent);
//                totalFailed = latestTfloorAssetActivity.Count(y => y.Status == 0 && y.IsCurrent);
//                totalpassedPercent = (totalPass * 100 / totalAssets);
//            }

//            var activityStartingdate = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).Min(x => x.ActivityInspectionDate);
//            var activityEndingdate = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null && x.IsCurrent).Max(x => x.ActivityInspectionDate);

//            table.AddCell(new Phrase(assetTypeName, CellFont));
//            table.AddCell(new Phrase(totalAssets.ToString(), CellFont));
//            table.AddCell(new Phrase(totaltested.ToString(), CellFont));
//            table.AddCell(new Phrase(totalPass.ToString(), CellFont));
//            table.AddCell(new Phrase(totalFailed.ToString(), CellFont));
//            table.AddCell(new Phrase($"{totalpassedPercent.ToString()} %", CellFont));
//            pdfDoc.Add(table);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 10f
//            };
//            table.DefaultCell.Border = Rectangle.NO_BORDER;

//            phrase = new Phrase();
//            phrase.Add(new Chunk("Activity Dates: ", CellFontBold));
//            phrase.Add(new Chunk($"{(activityStartingdate.HasValue ? activityStartingdate.Value.ToString("ddd, MMM d, yyyy") : string.Empty)} - {(activityEndingdate.HasValue ? activityEndingdate.Value.ToString("ddd, MMM d, yyyy") : string.Empty)}", CellFont));
//            cell = new PdfPCell(new Phrase(phrase))
//            {
//                Colspan = 2,
//                MinimumHeight = 30f,
//                VerticalAlignment = Element.ALIGN_MIDDLE,
//                HorizontalAlignment = Element.ALIGN_LEFT,
//                Border = Rectangle.NO_BORDER
//            };
//            table.AddCell(cell);

//            phrase = new Phrase();
//            phrase.Add(new Chunk("Standard: ", CellFontBold));
//            phrase.Add(new Chunk($"{ep.Standard.TJCStandard} - {ep.Standard.DisplayDescription}", CellFont));
//            cell = new PdfPCell(new Phrase(phrase))
//            {
//                Colspan = 2,
//                MinimumHeight = 30f,
//                VerticalAlignment = Element.ALIGN_MIDDLE,
//                HorizontalAlignment = Element.ALIGN_LEFT,
//                Border = Rectangle.NO_BORDER
//            };
//            table.AddCell(cell);

//            phrase = new Phrase();
//            phrase.Add(new Chunk("EP: ", CellFontBold));
//            phrase.Add(new Chunk($"{ep.EPNumber} - {ep.Description}", CellFont));
//            cell = new PdfPCell(new Phrase(phrase))
//            {
//                Colspan = 2,
//                MinimumHeight = 30f,
//                VerticalAlignment = Element.ALIGN_MIDDLE,
//                HorizontalAlignment = Element.ALIGN_LEFT,
//                Border = Rectangle.NO_BORDER
//            };
//            table.AddCell(cell);

//            phrase = new Phrase();
//            phrase.Add(new Chunk("CoP: ", CellFontBold));
//            phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.RequirementName)), CellFont));
//            table.AddCell(new Phrase(phrase));

//            phrase = new Phrase();
//            phrase.Add(new Chunk("CMS Tags: ", CellFontBold));
//            phrase.Add(new Chunk(string.Join(",", ep.CopDetails.Select(x => x.CopStdesc.TagCode)), CellFont));
//            table.AddCell(new Phrase(phrase));

//            phrase = new Phrase();
//            phrase.Add(new Chunk("Name of Activity: ", CellFontBold));
//            phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName} Inspection of {assetTypeName}", CellFont));
//            cell = new PdfPCell(new Phrase(phrase))
//            {
//                Colspan = 2,
//                MinimumHeight = 30f,
//                VerticalAlignment = Element.ALIGN_MIDDLE,
//                HorizontalAlignment = Element.ALIGN_LEFT,
//                Border = Rectangle.NO_BORDER
//            };
//            table.AddCell(cell);

//            phrase = new Phrase();
//            phrase.Add(new Chunk("Frequency of Activity: ", CellFontBold));
//            phrase.Add(new Chunk($"{ep.EPFrequency.FirstOrDefault().Frequency.DisplayName}", CellFont));
//            cell = new PdfPCell(new Phrase(phrase))
//            {
//                Colspan = 2,
//                MinimumHeight = 30f,
//                VerticalAlignment = Element.ALIGN_MIDDLE,
//                HorizontalAlignment = Element.ALIGN_LEFT,
//                Border = Rectangle.NO_BORDER
//            };
//            table.AddCell(cell);
//            pdfDoc.Add(table);

//            var inspectorInformtaion = ep.TInspectionActivity.Where(x => x.ActivityInspectionDate != null).Select(y => y.UserProfile).GroupBy(test => test.UserId)
//                                    .Select(grp => grp.First()).ToList();
//            var isanyVendorUser = inspectorInformtaion.Any(x => x.IsVendorUser);

//            para = new Paragraph("Inspector Contact Information", TitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.DefaultCell.Border = Rectangle.NO_BORDER;

//            foreach (var item in inspectorInformtaion)
//            {

//                phrase = new Phrase();
//                phrase.Add(new Chunk("Name: ", CellFontBold));
//                phrase.Add(new Chunk(item.Name, CellFont));
//                table.AddCell(new Phrase(phrase));

//                phrase = new Phrase();
//                phrase.Add(new Chunk("Phone: ", CellFontBold));
//                phrase.Add(new Chunk(item.PhoneNumber, CellFont));
//                table.AddCell(new Phrase(phrase));

//                phrase = new Phrase();
//                phrase.Add(new Chunk("Email: ", CellFontBold));
//                phrase.Add(new Chunk(item.Email, CellFont));
//                table.AddCell(new Phrase(phrase));

//                phrase = new Phrase();
//                phrase.Add(new Chunk("Company: ", CellFontBold));
//                phrase.Add(new Chunk(item.IsVendorUser ? "" : CurrentOrg.Name, CellFont));
//                table.AddCell(new Phrase(phrase));

//                phrase = new Phrase();
//                phrase.Add(new Chunk("Address: ", CellFontBold));
//                phrase.Add(new Chunk(item.IsVendorUser ? "" : CurrentOrg.Name, CellFont));
//                cell = new PdfPCell(phrase)
//                {
//                    Colspan = 2,
//                    MinimumHeight = 30f,
//                    Border = Rectangle.NO_BORDER
//                };
//                table.AddCell(cell);
//                // Need looping here                            
//            }
//            pdfDoc.Add(table);
//            pdfDoc.NewPage();

//            para = new Paragraph("Inspection Details", TitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);
//            table = new PdfPTable(7)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f,
//                SpacingAfter = 30f
//            };
//            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
//            float[] widths = new float[] { 5f, 25f, 25f, 20f, 8f, 7f, 10f };
//            table.SetTotalWidth(widths);

//            table.AddCell(new Phrase("#", CellFontBoldBlue));
//            table.AddCell(new Phrase("Asset #", CellFontBoldBlue));
//            table.AddCell(new Phrase("Building, Floor", CellFontBoldBlue));
//            table.AddCell(new Phrase("Location", CellFontBoldBlue));
//            table.AddCell(new Phrase("Inspector", CellFontBoldBlue));
//            table.AddCell(new Phrase("Date", CellFontBoldBlue));
//            table.AddCell(new Phrase("Result", CellFontBoldBlue));

//            table.DefaultCell.BackgroundColor = BaseColor.WHITE;

//            foreach (var tfloorAsset in assetsActivity.Select((value, index) => new { value, index }))
//            {
//                string buildingfloor = string.Format("{0}{1}", tfloorAsset.value.tfloorAssets.Floor.Building.BuildingName, !string.IsNullOrEmpty(tfloorAsset.value.tfloorAssets.Floor.FloorName) ? $", {tfloorAsset.value.tfloorAssets.Floor.FloorName}" : "");
//                table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
//                table.AddCell(new Phrase(tfloorAsset.value.assetNo, CellFont));
//                table.AddCell(new Phrase(!string.IsNullOrEmpty(buildingfloor) ? buildingfloor : "NA", CellFont));
//                table.AddCell(new Phrase(tfloorAsset.value.nearby, CellFont));
//                table.AddCell(new Phrase(tfloorAsset.value.inspectorName.ToString(), CellFont));
//                table.AddCell(new Phrase(tfloorAsset.value.inspectiondate.HasValue ? tfloorAsset.value.inspectiondate.Value.ToString("ddd, MMM d, yyyy") : string.Empty, CellFont));
//                table.AddCell(new Phrase(tfloorAsset.value.status == 1 ? "Compliant" : tfloorAsset.value.status == 0 ? "Non Compliant" : "Pending", CellFont));
//            }
//            pdfDoc.Add(table);

//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 75,
//                HorizontalAlignment = Element.ALIGN_CENTER,
//                SpacingBefore = 20f,
//                SpacingAfter = 10f
//            };
//            table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
//            table.AddCell(new Phrase("#", CellFontBoldBlue));
//            table.AddCell(new Phrase("Asset #", CellFontBoldBlue));
//            table.AddCell(new Phrase("Comment", CellFontBoldBlue));
//            table.DefaultCell.BackgroundColor = BaseColor.WHITE;
//            foreach (var tfloorAsset in assetsActivity.Select((value, index) => new { value, index }))
//            {
//                if ((!string.IsNullOrEmpty(tfloorAsset.value.comment)))
//                {
//                    table.AddCell(new Phrase(string.Format("{0}", tfloorAsset.index + 1), CellFont));
//                    table.AddCell(new Phrase(tfloorAsset.value.assetNo, CellFont));
//                    table.AddCell(new Phrase(tfloorAsset.value.comment, CellFont));
//                }
//            }

//            pdfDoc.Add(table);


//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 75,
//                HorizontalAlignment = Element.ALIGN_CENTER,
//                SpacingBefore = 20f,
//                SpacingAfter = 10f
//            };

//            table.AddCell(new Phrase("Inspector", CellFont));
//            table.AddCell(new Phrase("Digitally Signed On", CellFont));

//            foreach (var user in inspectorInformtaion.Where(x => !x.IsVendorUser))
//            {
//                var latestinspectiondate = string.Empty;
//                var latestinspection = ep.TInspectionActivity.Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
//                if (latestinspection != null)
//                    latestinspectiondate = latestinspection.Value.ToString("ddd, MMM d, yyyy");
//                table.AddCell(new Phrase(user.FullName, CellFont));
//                table.AddCell(new Phrase(latestinspectiondate, CellFont));
//            }

//            pdfDoc.Add(table);

//            if (isanyVendorUser)
//            {
//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 75,
//                    HorizontalAlignment = Element.ALIGN_CENTER,
//                    SpacingBefore = 20f,
//                    SpacingAfter = 10f
//                };

//                table.AddCell(new Phrase("Owner/Representative", CellFont));
//                table.AddCell(new Phrase("Digitally Signed On", CellFont));

//                foreach (var user in inspectorInformtaion.Where(x => x.IsVendorUser))
//                {
//                    var latestinspectiondate = string.Empty;
//                    var latestinspection = ep.TInspectionActivity.Where(x => x.CreatedBy == user.UserId).Max(x => x.ActivityInspectionDate);
//                    if (latestinspection != null)
//                        latestinspectiondate = latestinspection.Value.ToString("ddd, MMM d, yyyy");
//                    table.AddCell(new Phrase(user.FullName, CellFont));
//                    table.AddCell(new Phrase(latestinspectiondate, CellFont));
//                }
//                pdfDoc.Add(table);
//            }

//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }

//        #endregion

//        #region Footer 

//        public class PDFFooter : PdfPageEventHelper
//        {
//            PdfContentByte cb;
//            PdfTemplate footerTemplate;
//            BaseFont bf = null;

//            private readonly string FileTitle;

//            public PDFFooter(string fileTitle)
//            {
//                FileTitle = fileTitle;
//            }
//            public PDFFooter()
//            { }

//            // write on top of document
//            public override void OnOpenDocument(PdfWriter writer, Document document)
//            {
//                base.OnOpenDocument(writer, document);
//                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
//                cb = writer.DirectContent;
//                footerTemplate = cb.CreateTemplate(50, 50);
//                //PdfPTable tabFot = new PdfPTable(new float[] { 1F })
//                //{
//                //    SpacingAfter = 10F
//                //};
//                //PdfPCell cell;
//                //tabFot.TotalWidth = 300F;
//                //cell = new PdfPCell(new Phrase(FooterGeneratedBy()))
//                //{
//                //    Border = Rectangle.NO_BORDER
//                //};
//                //tabFot.AddCell(cell);
//                //tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
//            }

//            // write on start of each page
//            public override void OnStartPage(PdfWriter writer, Document document)
//            {
//                base.OnStartPage(writer, document);
//            }

//            // write on end of each page
//            public override void OnEndPage(PdfWriter writer, Document document)
//            {
//                // DateTime horario = DateTime.Now;
//                base.OnEndPage(writer, document);

//                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//                //document.Add(line);

//                //string text = "Page " + writer.PageNumber + " of " ;

//                PdfPTable tabFot = new PdfPTable(2);
//                PdfPCell cell;
//                tabFot.TotalWidth = 950;
//                //tabFot.WidthPercentage = 100;
//                cell = new PdfPCell(new Phrase(FooterMsg(), CellFontSmall))
//                {
//                    Border = Rectangle.NO_BORDER,
//                    HorizontalAlignment = Element.ALIGN_LEFT
//                };
//                tabFot.AddCell(cell);
//                //PdfPCell cell;
//                //tabFot.TotalWidth = 300F;
//                string lbltext = !string.IsNullOrEmpty(FileTitle) ? FileTitle : string.Empty;
//                cell = new PdfPCell(new Phrase(lbltext, CellFontSmall))
//                {
//                    Border = Rectangle.NO_BORDER,
//                    HorizontalAlignment = Element.ALIGN_LEFT
//                };
//                tabFot.AddCell(cell);
//                tabFot.WriteSelectedRows(0, -1, 10, document.Bottom + 10, writer.DirectContent);
//                {
//                    string text = "Page " + writer.PageNumber + " of ";
//                    cb.BeginText();
//                    cb.SetFontAndSize(bf, 8);
//                    cb.SetTextMatrix(document.PageSize.GetRight(325), document.PageSize.GetBottom(25));
//                    cb.ShowText(text);
//                    cb.EndText();
//                    float len = bf.GetWidthPoint(text, 8);
//                    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(325) + len, document.PageSize.GetBottom(25));
//                }

//            }

//            //write on close of document
//            public override void OnCloseDocument(PdfWriter writer, Document document)
//            {
//                base.OnCloseDocument(writer, document);
//                footerTemplate.BeginText();
//                footerTemplate.SetFontAndSize(bf, 8);
//                footerTemplate.SetTextMatrix(0, 0);
//                footerTemplate.ShowText((writer.PageNumber).ToString());
//                footerTemplate.EndText();
//            }

//            public string FooterGeneratedBy()
//            {
//                return $"{Utility.ConstMessage.PrintGeneratedByTitle}: {Utility.ConstMessage.PrintGeneratedFromText}";
//            }

//            public string FooterGeneratedDate()
//            {
//                return $"{Utility.ConstMessage.PrintGeneratedDateTitle}: {DateTime.Now.ToShortDateString()}";
//            }

//            public string FooterMsg()
//            {
//                return $"{Utility.ConstMessage.PrintGeneratedFromTitle} {Utility.ConstMessage.PrintGeneratedFromText} {"on"} {HCF.Utility.Common.ConvertToFormatDate(DateTime.Now)}";
//            }
//        }

//        #endregion

//        #region Set Paper size

//        public static Rectangle SetLandScapePaperType()
//        {
//            return PageSize.A4.Rotate();
//        }

//        #endregion
//    }
//}
