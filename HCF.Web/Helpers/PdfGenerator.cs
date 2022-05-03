//using HCF.BAL;
//using HCF.BAL.Ioc;
//using HCF.BDO;
//using HCF.Utility;
//using HCF.Web.Base;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;

//using static HCF.Utility.Enums;


//namespace HCF.Web.Helpers
//{
//    public class PdfGenerator
//    {
//        static PdfGenerator()
//        {

//        }
//        // private static IServiceProvider provider = null;
//        #region Common
//        private static PdfPCell getCell(String text, int alignment, Font font)
//        {
//            PdfPCell cell = new PdfPCell(new Phrase(text, font))
//            {
//                Border = 0,
//                Padding = 0,
//                HorizontalAlignment = alignment

//            };
//            return cell;
//        }
//        public static PdfPTable AddAttachmentCell(string filepath, string Filename, PdfWriter pdfWriter, PdfPTable table)
//        {
//            var bucketName = Convert.ToString(UserSession.CurrentOrg.ClientNo);
//            FileInfo fi = new FileInfo(filepath);
//            PdfPCell cell = new PdfPCell()
//            {
//                Border = 0,
//            };
//            Font link = FontFactory.GetFont("Arial", 8.5f, Font.UNDERLINE, BaseColor.BLUE);
//            var c = new Chunk(Filename, link);
//            c.SetAnchor(AppSettings.WebUrlPath + "/Common/FilePreview?imgSrc=" + bucketName + "/" + filepath.Replace("~/", ""));
//            cell.AddElement(c);
//            table.AddCell(cell);

//            return table;

//        }
//        public static PdfPCell AddNewCell(string text, Font fontstyle, bool alignmentrighttype = false, int? Colspan = 0, int? Border = 0, int? cellBackColor = 0, bool paddingoff = false)
//        {
//            Chunk chunk1 = new Chunk(text, fontstyle);
//            PdfPCell nobordercell = new PdfPCell()
//            {
//                Border = 0,
//            };
//            if (paddingoff)
//            {
//                nobordercell.PaddingBottom = 0;
//                nobordercell.PaddingTop = 0;
//            }

//            if (
//                Colspan > 0)
//            {
//                nobordercell.Colspan = Colspan.Value;
//                nobordercell.Padding = 0;

//            }
//            nobordercell.UseVariableBorders = true;
//            nobordercell.BorderColor = BaseColor.WHITE;
//            if (Colspan == 0)
//            {
//                nobordercell.Border = 0;
//            }
//            if (Border == 1)
//            {
//                nobordercell = new PdfPCell()
//                {
//                    Border = Rectangle.LEFT_BORDER,
//                };
//                nobordercell.UseVariableBorders = true;
//                Border = Rectangle.LEFT_BORDER;
//                nobordercell.BorderColorRight = BaseColor.BLACK;
//                nobordercell.BorderColorBottom = BaseColor.BLACK;
//                nobordercell.BorderColorTop = BaseColor.BLACK;
//                nobordercell.BorderColorLeft = BaseColor.BLACK;
//                //nobordercell.BorderColorLeft = BaseColor.BLACK;
//                //nobordercell.BorderColorBottom = BaseColor.BLACK;
//            }
//            if (cellBackColor > 0)
//            {
//                nobordercell.BackgroundColor = BaseColor.BLACK;

//            }
//            nobordercell.HorizontalAlignment = alignmentrighttype ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
//            nobordercell.AddElement(chunk1);
//            return nobordercell;
//        }
//        public static PdfPCell CreateSignSectionCell(string Title, DigitalSignature digitalsign, float? linewidth = 45.0F)
//        {
//            PdfPCell cellmain = new PdfPCell()
//            {
//                Border = 0,
//            };

//            PdfPTable digitable = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0

//            };

//            if (digitalsign != null && digitalsign.DigSignatureId > 0)
//            {
//                if (!string.IsNullOrEmpty(Title))
//                {
//                    digitable.AddCell(AddNewCell(Title, CellBoldFont, false));
//                    digitable.AddCell(AddNewCell("", CellBoldFont, false));
//                }

//                PdfPCell cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Image image = Image.GetInstance(Base.Common.FilePath(digitalsign.FilePath));
//                cell.AddElement(new Chunk(image, 0, -10));
//                cell.PaddingBottom = 0;
//                cell.PaddingTop = 0;
//                digitable.AddCell(cell);
//                cell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, linewidth.Value, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//                cell.AddElement(line);
//                cell.UseAscender = true;
//                cell.PaddingBottom = 0;
//                cell.PaddingTop = 0;
//                digitable.AddCell(cell);


//                digitable.AddCell(AddNewCell(digitalsign != null && digitalsign.SignByUserName != null ? digitalsign.SignByUserName + " ( " + digitalsign.SignByEmail + ")" : " ", CellFontS, false, 0, 0, 0, true));
//                digitable.AddCell(AddNewCell(digitalsign != null && digitalsign.LocalSignDateTime != null ? "(" + digitalsign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue, false, 0, 0, 0, true));
//            }

//            cellmain.AddElement(digitable);
//            cellmain.Colspan = 1;

//            return cellmain;

//        }
//        #endregion

//        #region Ceiling Permit

//        public static Document CreateCeilingPermit(int ceilingPermitId, Stream streamOutput, CeilingPermit objceilingPermit, bool IsAttachmentIncluded)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table = new PdfPTable(1);
//            PdfPCell cell = new PdfPCell();
//            string approverName = objceilingPermit.FinalInspectionByUser != null ? objceilingPermit.FinalInspectionByUser.Name : string.Empty;
//            if (objceilingPermit.Status == 2 || objceilingPermit.Status == -1)
//            {
//                SetHeaderBlue(out table, "Ceiling Permit (CP)");
//            }
//            else
//            {
//                SetStatusHeaderBlue(out table, "Ceiling Permit (CP)", Enum.GetName(typeof(Enums.ApprovalStatus), objceilingPermit.Status).ToString().ToUpper(), approverName, ($"{objceilingPermit.ApprovedDate:MMM d, yyyy}"));

//            }
//            pdfDoc.Add(table);

//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingAfter = 10f,
//                SpacingBefore = 10f
//            };
//            float[] widths = new float[] { 20f, 60f, 20f };
//            table.SetWidths(widths);
//            table.AddCell(getCell("", PdfPCell.ALIGN_LEFT, CellFont));
//            table.AddCell(getCell("Above the Ceiling Permit Request", PdfPCell.ALIGN_CENTER, UnderlineTitleFont));
//            //table.AddCell(getCell("Text in the middle", PdfPCell.ALIGN_CENTER,CellFont));
//            table.AddCell(getCell("Permit #: " + objceilingPermit.PermitNo, PdfPCell.ALIGN_RIGHT, CellFont));
//            pdfDoc.Add(table);
//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingAfter = 10f
//            };
//            cell = new PdfPCell(new Phrase("Project #: " + (objceilingPermit.ProjectId > 0 ? objceilingPermit.Project.ProjectNumber : string.Empty), CellFont))
//            {

//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Project Name: " + (objceilingPermit.ProjectId > 0 ? objceilingPermit.Project.ProjectName : string.Empty), CellFont))
//            {

//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Work area:  " + objceilingPermit.WorkArea, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Scope of work:  " + objceilingPermit.Scope, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Building(s):  " + objceilingPermit.BuildingName, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Floor(s):  " + objceilingPermit.FloorName, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Zone(s):  " + objceilingPermit.Zones, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Start time:  " + Base.Common.ConvertToAMPM(objceilingPermit.StartTime), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("End time:  " + Base.Common.ConvertToAMPM(objceilingPermit.EndTime), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Work start date:  " + (objceilingPermit.StartDate.HasValue ? objceilingPermit.StartDate.Value.ToFormatDate() : string.Empty), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Estimated completion date:  " + (objceilingPermit.CompletionDate.HasValue ? objceilingPermit.CompletionDate.Value.ToFormatDate() : string.Empty), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Responsible party for sealing the penetrations: " + objceilingPermit.Responsible, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Type of sealant used:  " + objceilingPermit.TypeofSealant, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            string uLApproved = (objceilingPermit.ULApproved) ? "Yes" : "No";
//            cell = new PdfPCell(new Phrase("UL approved for use?: " + uLApproved, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Requested By: " + (objceilingPermit.RequestedbyUser != null ? objceilingPermit.RequestedbyUser.Name : string.Empty), CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Organization/Dept:  " + objceilingPermit.RequestedDept, CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Telephone #:  " + objceilingPermit.PhoneNo, CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Date of request: " + (objceilingPermit.RequestedDate.HasValue ? objceilingPermit.RequestedDate.Value.ToFormatDate() : string.Empty), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Additional items (circle): " + (objceilingPermit.AdditionalItems.HasValue ? Enum.GetName(typeof(Enums.AdditionalItems), objceilingPermit.AdditionalItems).ToString() : string.Empty), CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);


//            cell = new PdfPCell(new Phrase("I UNDERSTAND THAT PAYMENT WILL NOT BE RELEASED UNTIL ALL PENETRATIONS HAVE BEEN SEALED, INSPECTED, AND APPROVED BY A REPRESENTITIVE FROM THE PLANT OPERATIONS DEPARTMENT.  I FURTHER CERTIFY THAT I AM AUTHORIZED TO SIGN FOR MY ORGANIZATION.", CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);



//            if (objceilingPermit.DSRequesterSign != null && objceilingPermit.DSRequesterSign.DigSignatureId > 0)
//            {

//                PdfPTable table1 = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                PdfPCell cellmain = new PdfPCell()
//                {
//                    Border = 0
//                };
//                cellmain = CreateSignSectionCell("Requestor Signature:", objceilingPermit.DSRequesterSign, 30f);
//                table1.AddCell(cellmain);
//                cellmain = new PdfPCell()
//                {
//                    Colspan = 2
//                };
//                cellmain.AddElement(table1);
//                table.AddCell(cellmain);
//            }
//            cell = new PdfPCell(new Phrase("Pictures/verification of sealed penetrations:" + (objceilingPermit.IsPenetrationsVerified != null && objceilingPermit.IsPenetrationsVerified == 1 ? "Yes" : "No"), CellFont))
//            {
//                Colspan = 2,
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Approval Status:  " + Enum.GetName(typeof(Enums.ApprovalStatus), objceilingPermit.Status).ToString().ToUpper(), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            if (objceilingPermit.Status == 2)
//            {
//                cell = new PdfPCell(new Phrase("", CellFont))
//                {
//                    MinimumHeight = 15f,
//                    VerticalAlignment = Element.ALIGN_MIDDLE
//                };
//            }
//            else if (objceilingPermit.Status == 1)
//            {
//                cell = new PdfPCell(new Phrase("", CellFont))
//                {
//                    MinimumHeight = 15f,
//                    VerticalAlignment = Element.ALIGN_MIDDLE
//                };
//            }
//            else
//            {
//                var label = objceilingPermit.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
//                cell = new PdfPCell(new Phrase(label + objceilingPermit.Reason, CellFont))
//                {
//                    MinimumHeight = 15f,
//                    VerticalAlignment = Element.ALIGN_MIDDLE
//                };

//            }
//            table.AddCell(cell);
//            cell = new PdfPCell(new Phrase("Date: " + (objceilingPermit.ApprovedDate.HasValue ? objceilingPermit.ApprovedDate.Value.ToFormatDate() : string.Empty), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);

//            cell = new PdfPCell(new Phrase("Final Authorization Made By:  " + (objceilingPermit.FinalInspectionByUser != null ? objceilingPermit.FinalInspectionByUser.Name : string.Empty), CellFont))
//            {
//                MinimumHeight = 15f,
//                VerticalAlignment = Element.ALIGN_MIDDLE
//            };
//            table.AddCell(cell);
//            if (objceilingPermit.Status == 1)
//            {
//                if (objceilingPermit.DSApproverSign != null && objceilingPermit.DSApproverSign.DigSignatureId > 0)
//                {

//                    PdfPTable table1 = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0
//                    };
//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Colspan = 2
//                    };
//                    cellmain = CreateSignSectionCell("Approver Signature:", objceilingPermit.DSApproverSign, 30f);
//                    table1.AddCell(cellmain);
//                    cellmain = new PdfPCell()
//                    {
//                        Colspan = 2
//                    };
//                    cellmain.AddElement(table1);
//                    table.AddCell(cellmain);

//                }
//            }

//            pdfDoc.Add(table);
//            bool isattach = false;
//            if (IsAttachmentIncluded)
//            {
//                if (objceilingPermit.DrawingAttachments.Count > 0 || objceilingPermit.Attachments.Count > 0)
//                {
//                    pdfDoc.NewPage();
//                    isattach = true;
//                }
//                if (isattach)
//                {
//                    if (objceilingPermit.DrawingAttachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 10f
//                        };
//                        table.AddCell(AddNewCell("Drawings Attached :", CellFontBoldBlack));
//                        for (int i = 0; i < objceilingPermit.DrawingAttachments.Count; i++)
//                        {
//                            AddAttachmentCell(objceilingPermit.DrawingAttachments[i].ImagePath, objceilingPermit.DrawingAttachments[i].FullFileName, pdfWriter, table);
//                        }
//                        pdfDoc.Add(table);
//                    }
//                    if (objceilingPermit.Attachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 10f
//                        };
//                        table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
//                        for (int i = 0; i < objceilingPermit.Attachments.Count; i++)
//                        {
//                            AddAttachmentCell(objceilingPermit.Attachments[i].FilePath, objceilingPermit.Attachments[i].FileName, pdfWriter, table);
//                        }
//                        pdfDoc.Add(table);
//                    }
//                }



//            }



//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }


//        public static string CeilingPermitInbytes(int? ceilingPermitId, bool IsAttachmentIncluded)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            CeilingPermit objceilingPermit = new CeilingPermit();

//            objceilingPermit = UnityContextFactory<IPermitService>.CreateContext().GetCeilingPermit(ceilingPermitId.Value);
//            pdfDoc = CreateCeilingPermit(ceilingPermitId.Value, mem, objceilingPermit, IsAttachmentIncluded);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }


//        #endregion

//        #region LifeSafety
//        public static string LifeSafetyFormInbytes(string formId)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();

//            TLifeSafetyDevicesForms newForm = UnityContextFactory<IPermitService>.CreateContext().GetLifeSafetyForm(Guid.Parse(formId));

//            pdfDoc = CreateLifeSafetyPermit(formId, mem, newForm, true);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        public static Document CreateLifeSafetyPermit(string formId, Stream streamOutput, TLifeSafetyDevicesForms newForm, bool IsAttachmentIncluded)
//        {
//            if (newForm.LifeSafetyDeviceList.Count == 0)
//            {
//                LifeSafetyDeviceList device = new LifeSafetyDeviceList();
//                newForm.LifeSafetyDeviceList.Add(device);
//            }

//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            string approverName = newForm.ApprovedByUser != null ? newForm.ApprovedByUser.Name : string.Empty;
//            if (newForm.Status == 2 || newForm.Status == -1)
//            {
//                SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
//            }
//            else
//            {

//                SetStatusHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"), Enum.GetName(typeof(Enums.ApprovalStatus), newForm.Status).ToString().ToUpper(), approverName, ($"{newForm.SignDate:MMM d, yyyy}"));

//            }
//            //SetHeaderBlue(out table, "Life Safety Devices –" + (newForm.FormType == 1 ? " Addition Form" : " Removal Form"));
//            PdfPTable tableapprove = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0

//            };
//            pdfDoc.Add(table);

//            if (newForm != null)
//            {
//                table = new PdfPTable(6)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 5f

//                };

//                float[] widths = new float[] { 25f, 25f, 5f, 20f, 5f, 20f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;

//                Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                ImgCheckBox.ScaleAbsolute(10f, 10f);

//                Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

//                PdfPCell nobordercell = new PdfPCell()
//                {
//                    Border = 0,
//                };


//                Chunk chunk1 = new Chunk();
//                table.AddCell(AddNewCell("Section 1: Basic Information", CellFontBoldBlueSmall, false, 6));
//                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.PermitNo, CellFont, false));
//                if (newForm.IsMOPSubmission)
//                {
//                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);


//                }
//                else
//                {
//                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                }
//                table.AddCell(AddNewCell("MOP Submission", CellFont, false));
//                if (newForm.IsFinalSubmission)
//                {

//                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                }
//                else
//                {
//                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);
//                }
//                table.AddCell(AddNewCell("Final Submission", CellFont, false));
//                PdfPCell cellattachment = new PdfPCell()
//                {
//                    Border = 0,
//                };

//                pdfDoc.Add(table);
//                table = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingAfter = 10f

//                };


//                widths = new float[] { 25f, 25f, 25f, 25f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.ProjectName, CellFont, false));
//                table.AddCell(AddNewCell("Project #:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.Project.ProjectNumber, CellFont, false));
//                table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell($"{newForm.DateOfRequest:MMM d, yyyy}", CellFont, false));
//                table.AddCell(AddNewCell("Requestor:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.RequestorUser != null ? newForm.RequestorUser.Name : string.Empty, CellFont, false));
//                table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.Contractor, CellFont, false));
//                table.AddCell(AddNewCell("Building(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.BuildingName, CellFont, false));
//                table.AddCell(AddNewCell("Floor(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.FloorName, CellFont, false));
//                table.AddCell(AddNewCell("Zone(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.Zones, CellFont, false));
//                table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.EmailAddress, CellFont, false));
//                table.AddCell(AddNewCell("Phone:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.PhoneNumber, CellFont, false));
//                table.AddCell(AddNewCell("Device Types:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.DeviceType, CellFont, false, 3));
//                table.AddCell(AddNewCell("Brief Description of Work:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(newForm.Description, CellFont, false, 3));

//                pdfDoc.Add(table);
//                //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//                //pdfDoc.Add(line);
//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingAfter = 10f

//                };

//                table.AddCell(AddNewCell("Section 2: Life Safety Device List:", CellFontBoldBlueSmall, false));
//                pdfDoc.Add(table);
//                table = new PdfPTable(7)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 5f,
//                    SpacingAfter = 5f

//                };
//                widths = new float[] { 5f, 15f, 15f, 25f, 15f, 20f, 15f };
//                table.SetTotalWidth(widths);
//                table.AddCell(new Phrase("#", CellFontBoldBlack));
//                table.AddCell(new Phrase("Device #", CellFontBoldBlack));
//                table.AddCell(new Phrase("Building", CellFontBoldBlack));
//                table.AddCell(new Phrase("Location / FACP Alias ", CellFontBoldBlack));
//                table.AddCell(new Phrase("Device Type ", CellFontBoldBlack));
//                table.AddCell(new Phrase("Make / Model", CellFontBoldBlack));
//                table.AddCell(new Phrase(newForm.FormType == 1 ? "Date Added " : "Date Removed", CellFontBoldBlack));
//                nobordercell = new PdfPCell();
//                for (int i = 0; i < newForm.LifeSafetyDeviceList.Count; i++)
//                {
//                    //var type = newForm.FormType ? newForm.FormType.ToString() : string.Empty;
//                    //var sequence = this.ViewData.ContainsKey("sequence") ? this.ViewData["sequence"].ToString() : string.Empty;

//                    table.AddCell(new Phrase(string.Format("{0}", i + 1), CellFont));
//                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].DeviceNumber, CellFont));
//                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Building, CellFont));
//                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Location, CellFont));
//                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].AssetType, CellFont));
//                    table.AddCell(new Phrase(newForm.LifeSafetyDeviceList[i].Make, CellFont));
//                    table.AddCell(new Phrase(newForm.FormType == 1 ? newForm.LifeSafetyDeviceList[i].DateAdded.ToFormatDate() : newForm.LifeSafetyDeviceList[i].DateRemoved.ToFormatDate(), CellFont));
//                }
//                pdfDoc.Add(table);
//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingAfter = 5f

//                };

//                table.AddCell(AddNewCell("SECTION 3:  Approval  [Hospital Employee’s Only] ", CellFontBoldBlueSmall, false));
//                pdfDoc.Add(table);

//                if (newForm.Status == 2)
//                {
//                    table = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 5f,



//                    };
//                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell("Requested", CellFont, false));
//                }
//                else if (newForm.Status == 1)
//                {
//                    table = new PdfPTable(3)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 5f,



//                    };
//                    widths = new float[] { 60f, 20f, 20f };
//                    table.SetTotalWidth(widths);

//                    if (newForm.ApprovedByUser != null && newForm.PermitAuthorizedSignature != null && newForm.PermitAuthorizedSignature.DigSignatureId > 0)
//                    {
//                        PdfPCell cellmain = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        cellmain = CreateSignSectionCell("Approved By:", newForm.PermitAuthorizedSignature);
//                        table.AddCell(cellmain);
//                    }
//                    else
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                    }
//                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell("Approved", CellFont, false));
//                }
//                else
//                {
//                    table = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 5f,



//                    };
//                    table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Rejected" : "On Hold/Pending", CellFont, false));
//                    table.AddCell(AddNewCell("Approval By:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(newForm.ApprovedByUser != null ? newForm.ApprovedByUser.Name : string.Empty, CellFont, false));
//                    table.AddCell(AddNewCell(newForm.Status == 0 ? "Reason(s) of Rejection" : "Reason(s) of on Hold/Pending", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(newForm.Reason, CellFont, false));
//                }
//                pdfDoc.Add(table);

//                bool isattach = false;
//                if (IsAttachmentIncluded)
//                {
//                    if (newForm.DrawingAttachments.Count > 0 || newForm.Attachments.Count > 0)
//                    {
//                        pdfDoc.NewPage();
//                        isattach = true;
//                    }
//                    if (isattach)
//                    {
//                        if (newForm.Attachments.Count > 0)
//                        {
//                            table = new PdfPTable(1)
//                            {
//                                WidthPercentage = 100,
//                                HorizontalAlignment = 0,
//                                SpacingAfter = 10f

//                            };

//                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
//                            for (int i = 0; i < newForm.Attachments.Count; i++)
//                            {
//                                PdfGenerator.AddAttachmentCell(newForm.Attachments[i].FilePath, newForm.Attachments[i].FileName, pdfWriter, table);


//                            }
//                            pdfDoc.Add(table);
//                        }
//                        if (newForm.DrawingAttachments.Count > 0)
//                        {
//                            table = new PdfPTable(1)
//                            {
//                                WidthPercentage = 100,
//                                HorizontalAlignment = 0,
//                                SpacingAfter = 10f

//                            };

//                            table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
//                            for (int i = 0; i < newForm.DrawingAttachments.Count; i++)
//                            {
//                                PdfGenerator.AddAttachmentCell(newForm.DrawingAttachments[i].ImagePath, newForm.DrawingAttachments[i].FullFileName, pdfWriter, table);
//                            }
//                            pdfDoc.Add(table);
//                        }
//                    }
//                }
//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;

//        }
//        #endregion

//        #region Fire System Bypass Permit

//        [HttpGet]
//        public static string FireSystemByPassPermitInbytes(int tfsbPermitId)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TFireSystemByPassPermit objTFireSystemByPassPermit = UnityContextFactory<IPermitService>.CreateContext().GetFSBPermit(tfsbPermitId);
//            pdfDoc = CreateFireSystemByPassPermit(tfsbPermitId, mem, objTFireSystemByPassPermit, true);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        public static Document CreateFireSystemByPassPermit(int tfsbPermitId, Stream streamOutput, TFireSystemByPassPermit objTFireSystemByPassPermit, bool IsAttachmentIncluded)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            SetHeaderBlue(out table, "Fire System Bypass Permit [FSBP] ");

//            Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//            ImgCheckBox.ScaleAbsolute(10f, 10f);

//            Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//            ImgUnCheckBox.ScaleAbsolute(10f, 10f);

//            pdfDoc.Add(table);

//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 20f
//            };
//            table.DefaultCell.Border = Rectangle.NO_BORDER;
//            table.AddCell(new Phrase("Section 1: Basic Information", CellFontBoldBlueSmall));

//            pdfDoc.Add(table);

//            if (objTFireSystemByPassPermit != null && objTFireSystemByPassPermit.TFSBPermitId > 0)
//            {
//                table = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                float[] widths = new float[] { 25f, 25f, 25f, 25f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.PermitNo, CellFont, false));
//                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.ProjectName, CellFont));
//                table.AddCell(AddNewCell("Date [of Request]:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(($"{objTFireSystemByPassPermit.RequestedDate:MMM d, yyyy}"), CellFont));
//                table.AddCell(AddNewCell("Requestor:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.RequestorByUser != null ? objTFireSystemByPassPermit.RequestorByUser.Name : string.Empty, CellFont));
//                table.AddCell(AddNewCell("Phone Number:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.PhoneNo, CellFont));
//                table.AddCell(AddNewCell("Organization:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.Company, CellFont));
//                table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.Email, CellFont));
//                table.AddCell(AddNewCell("On-Site Contact:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.OnSiteContact, CellFont));
//                table.AddCell(AddNewCell("On-Site Phone:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(objTFireSystemByPassPermit.OnSitePhone, CellFont, false, 3));

//                table.AddCell(AddNewCell(($"Start Date: {objTFireSystemByPassPermit.StartDate:MMM d, yyyy}"), CellFont));
//                table.AddCell(AddNewCell(($"Start Time: {Base.Common.ConvertToAMPM(objTFireSystemByPassPermit.StartTime)}"), CellFont));
//                table.AddCell(AddNewCell(($"End Date: {objTFireSystemByPassPermit.EndDate:MMM d, yyyy}"), CellFont));
//                table.AddCell(AddNewCell(($"End Time: {Base.Common.ConvertToAMPM(objTFireSystemByPassPermit.EndTime)}"), CellFont));

//                pdfDoc.Add(table);

//                //table = new PdfPTable(1)
//                //{
//                //    WidthPercentage = 100,
//                //    HorizontalAlignment = 0
//                //};
//                //table.DefaultCell.Border = Rectangle.NO_BORDER;

//                //pdfDoc.Add(table);
//                PdfPTable tablemain = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                tablemain.DefaultCell.Border = Rectangle.NO_BORDER;
//                widths = new float[] { 50f, 50f };
//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 50,
//                    HorizontalAlignment = 0
//                };
//                widths = new float[] { 20f, 30f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                Chunk chunk1 = new Chunk();
//                PdfPCell nobordercell = new PdfPCell();


//                PdfPTable tblFiresystem = new PdfPTable(2)
//                {
//                    WidthPercentage = 50,
//                    HorizontalAlignment = 0
//                };
//                widths = new float[] { 20f, 30f };
//                tblFiresystem.SetTotalWidth(widths);
//                table.AddCell(AddNewCell("Building(s) Affected:", CellFontBoldBlack));
//                table.AddCell(AddNewCell("", CellFontBoldBlack));
//                tblFiresystem.AddCell(AddNewCell("Applicable  Fire systems :", CellFontBoldBlack));
//                tblFiresystem.AddCell(AddNewCell("", CellFontBoldBlack));
//                tblFiresystem.AddCell(AddNewCell("Building(s)", CellFontBoldBlack));
//                tblFiresystem.AddCell(AddNewCell("Applicable systems:", CellFontBoldBlack));
//                for (int i = 0; i < objTFireSystemByPassPermit.TFSBPBuildingDetails.Count; i++)
//                {
//                    nobordercell = new PdfPCell() { Border = 0, };
//                    if (objTFireSystemByPassPermit.TFSBPBuildingDetails[i].Status)
//                    {
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table.AddCell(nobordercell);
//                    }
//                    else
//                    {
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table.AddCell(nobordercell);
//                    }
//                    table.AddCell(AddNewCell(objTFireSystemByPassPermit.TFSBPBuildingDetails[i].SiteBuildingName, CellFont));


//                    string firesystem = string.Empty;

//                    tblFiresystem.AddCell(AddNewCell(objTFireSystemByPassPermit.TFSBPBuildingDetails[i].BuildingName, CellFont));
//                    for (int j = 0; j < objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes.Count; j++)
//                    {
//                        if (objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].CheckVal.HasValue && objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].CheckVal.Value == 1)
//                        {
//                            firesystem = firesystem + objTFireSystemByPassPermit.TFSBPBuildingDetails[i].fireSystemTypes[j].Name + ",";
//                        }
//                        else
//                        {
//                        }

//                    }
//                    tblFiresystem.AddCell(AddNewCell(firesystem.TrimEnd(','), CellFont));

//                }
//                if (objTFireSystemByPassPermit.TFSBPBuildingDetails.Count % 4 == 1)
//                {
//                    table.AddCell("");
//                    table.AddCell("");
//                }
//                tablemain.AddCell(table);
//                tablemain.AddCell(tblFiresystem);
//                pdfDoc.Add(tablemain);
//                //pdfDoc.Add(table);
//                //pdfDoc.Add(tblFiresystem);




//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };

//                table.AddCell(new Phrase("Device/Points Affected:", CellFontBoldBlack));
//                table.AddCell(new Phrase(objTFireSystemByPassPermit.DevicePointsAffected, CellFont));

//                table.AddCell(new Phrase("Department(s)/ZonesAffected:", CellFontBoldBlack));
//                table.AddCell(new Phrase(objTFireSystemByPassPermit.DepartmentZonesAffected, CellFont));
//                pdfDoc.Add(table);

//                table = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                widths = new float[] { 25f, 5f, 55f, 15f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                table.AddCell(AddNewCell("Brief Description of Work:", CellFontBoldBlack));

//                nobordercell = new PdfPCell() { Border = 0, };
//                if (objTFireSystemByPassPermit.IsSystemReprogrammingRequired)
//                {
//                    chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);
//                }
//                else
//                {
//                    chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);
//                }
//                table.AddCell(AddNewCell("System Reprogramming Required and Scheduled for [Date]:", CellFontBoldBlack));
//                table.AddCell(AddNewCell(($"{objTFireSystemByPassPermit.ScheduledDate:MMM d, yyyy}"), CellFontBoldBlack));
//                pdfDoc.Add(table);

//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };
//                table.AddCell(new Phrase(!string.IsNullOrEmpty(objTFireSystemByPassPermit.Description) ? objTFireSystemByPassPermit.Description : "", CellFont));

//                pdfDoc.Add(table);

//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };
//                table.AddCell(AddNewCell("CHILDRENS HOSPITAL MEDICAL CENTER EMPLOYEE’S ONLY ", CellRedtext, false, 2));
//                table.AddCell(AddNewCell(" ", CellRedtext, false, 2));
//                PdfPTable ilsmtable = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                };
//                ilsmtable.DefaultCell.Border = Rectangle.BOX;
//                widths = new float[] { 20f, 80f };
//                ilsmtable.SetTotalWidth(widths);
//                // ilsmtable.DefaultCell.Border = Rectangle.NO_BORDER;
//                ilsmtable.AddCell(AddNewCell("SECTION 2: ILSM Required Checklist", CellFontBoldBlueSmall, false, 2));
//                ilsmtable.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
//                for (int i = 0; i < objTFireSystemByPassPermit.ILSMRequiredChecklist.Count; i++)
//                {
//                    nobordercell = new PdfPCell();
//                    if (objTFireSystemByPassPermit.ILSMRequiredChecklist[i].RespondStatus)
//                    {
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        ilsmtable.AddCell(nobordercell);
//                    }
//                    else
//                    {
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        ilsmtable.AddCell(nobordercell);
//                    }
//                    ilsmtable.AddCell(new Phrase(objTFireSystemByPassPermit.ILSMRequiredChecklist[i].FSBPForms.Name, CellFont));

//                }
//                ilsmtable.AddCell(AddNewCell("Fire Watch Required if entire fire alarm or sprinkler system is disabled more than 4 hours ", CellFontSmall1, false, 2));
//                ilsmtable.AddCell(AddNewCell(" ", CellRedtext, false, 2));
//                table.AddCell(ilsmtable);

//                PdfPTable StampTable;
//                string approverName = objTFireSystemByPassPermit.ApprovedByUser != null ? objTFireSystemByPassPermit.ApprovedByUser.Name : string.Empty;
//                CreateApprovalStamp(out StampTable, Enum.GetName(typeof(Enums.ApprovalStatus), objTFireSystemByPassPermit.ApprovalStatus), approverName, ($"{objTFireSystemByPassPermit.ApprovedDate:MMM d, yyyy}"), objTFireSystemByPassPermit);

//                table.AddCell(StampTable);

//                PdfPTable table1 = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                //widths = new float[] { 20f, 50f, 15f, 15f };
//                //table1.SetTotalWidth(widths);
//                //table1.DefaultCell.Border = Rectangle.NO_BORDER;
//                table1.AddCell(AddNewCell("SECTION 4: Bypass Engineering Actions", CellFontBoldBlueSmall, false, 4));
//                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
//                table1.AddCell(new Phrase("Date in Bypass:", CellFont));
//                table1.AddCell(new Phrase($"{objTFireSystemByPassPermit.BypassEngActionDate:MMM d, yyyy}", CellFont));
//                table1.AddCell(new Phrase("Time in Bypass", CellFont));
//                table1.AddCell(new Phrase(Web.Base.Common.ConvertToAMPM(objTFireSystemByPassPermit.BypassEngActionTime), CellFont));
//                for (int i = 0; i < objTFireSystemByPassPermit.BypassEngineeringActions.Count; i++)
//                {
//                    nobordercell = new PdfPCell();
//                    if (objTFireSystemByPassPermit.BypassEngineeringActions[i].RespondStatus)
//                    {
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table1.AddCell(nobordercell);
//                    }
//                    else
//                    {
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table1.AddCell(nobordercell);
//                    }
//                    table1.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].FSBPForms.Name, CellFont));
//                    table1.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].TimeinbyPass, CellFont));
//                    table1.AddCell(new Phrase(string.Empty, CellFont));
//                }
//                table1.AddCell(AddNewCell("Fac./ Engineering Rep:", CellFontBoldSmall2));
//                if (objTFireSystemByPassPermit.DSBypassEngActionSign != null && objTFireSystemByPassPermit.DSBypassEngActionSign.DigSignatureId > 0)
//                {
//                    //PdfPTable tableimg = new PdfPTable(1)
//                    //{
//                    //    WidthPercentage = 100,
//                    //    HorizontalAlignment = 0
//                    //};
//                    //PdfPCell cellimg = new PdfPCell()
//                    //{
//                    //    Border = 0,
//                    //};
//                    //PdfPCell cell = new PdfPCell()
//                    //{
//                    //    Border = 0,
//                    //};
//                    //Image image = Image.GetInstance(Base.Common.FilePath(objTFireSystemByPassPermit.DSBypassEngActionSign.FilePath));
//                    ////image.ScaleAbsolute(40, 40);
//                    //cellimg.AddElement(new Chunk(image, 0, 0));
//                    //tableimg.AddCell(cellimg);
//                    //cell.AddElement(tableimg);
//                    //table1.AddCell(cell);
//                    //cell = new PdfPCell()
//                    //{
//                    //    Border = 0,
//                    //};
//                    //cell.Colspan = 2;
//                    //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSBypassEngActionSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSBypassEngActionSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
//                    //table1.AddCell(cell);



//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSBypassEngActionSign);
//                    cellmain.Colspan = 3;
//                    table1.AddCell(cellmain);

//                }
//                else
//                {
//                    table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                }
//                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                table1.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                table.AddCell(table1);

//                PdfPTable table2 = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                //widths = new float[] { 20f, 50f, 15f, 15f };
//                //table2.SetTotalWidth(widths);
//                // table2.DefaultCell.Border = Rectangle.NO_BORDER;
//                table2.AddCell(AddNewCell("SECTION 5: Bypass Return Engineering Actions", CellFontBoldBlueSmall, false, 4));
//                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
//                table2.AddCell(new Phrase("Date Out Bypass:", CellFont));
//                table2.AddCell(new Phrase($"{objTFireSystemByPassPermit.BypassReturnEngActionDate:MMM d, yyyy}", CellFont));
//                table2.AddCell(new Phrase("Time Out Bypass", CellFont));
//                table2.AddCell(new Phrase(Web.Base.Common.ConvertToAMPM(objTFireSystemByPassPermit.BypassReturnEngActionTime), CellFont));
//                for (int i = 0; i < objTFireSystemByPassPermit.BypassReturnEngineeringActions.Count; i++)
//                {
//                    nobordercell = new PdfPCell();
//                    if (objTFireSystemByPassPermit.BypassReturnEngineeringActions[i].RespondStatus)
//                    {
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table2.AddCell(nobordercell);
//                    }
//                    else
//                    {
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        table2.AddCell(nobordercell);
//                    }
//                    table2.AddCell(new Phrase(objTFireSystemByPassPermit.BypassReturnEngineeringActions[i].FSBPForms.Name, CellFont));
//                    table2.AddCell(new Phrase(objTFireSystemByPassPermit.BypassEngineeringActions[i].TimeinbyPass, CellFont));
//                    table2.AddCell(new Phrase(string.Empty, CellFont));
//                }
//                table2.AddCell(AddNewCell("Fac./ Engineering Rep:", CellFontBoldSmall2));
//                if (objTFireSystemByPassPermit.DSBypassReturnEngActionSign != null && objTFireSystemByPassPermit.DSBypassReturnEngActionSign.DigSignatureId > 0)
//                {

//                    //PdfPCell cell = new PdfPCell()
//                    //{
//                    //    Border = 0,
//                    //};
//                    //Image image = Image.GetInstance(Base.Common.FilePath(objTFireSystemByPassPermit.DSBypassReturnEngActionSign.FilePath));
//                    ////image.ScaleAbsolute(40, 40);
//                    //cell.AddElement(new Chunk(image, 0, 0));
//                    //table2.AddCell(cell);
//                    //cell = new PdfPCell()
//                    //{
//                    //    Border = 0,
//                    //};
//                    //cell.Colspan = 2;
//                    //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSBypassReturnEngActionSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSBypassReturnEngActionSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
//                    //table2.AddCell(cell);
//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSBypassReturnEngActionSign);
//                    cellmain.Colspan = 3;
//                    table2.AddCell(cellmain);

//                }
//                else
//                {
//                    table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                }
//                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                table2.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                table.AddCell(table2);

//                pdfDoc.Add(table);

//                bool isattach = false;
//                if (IsAttachmentIncluded)
//                {

//                    if (objTFireSystemByPassPermit.DrawingAttachments.Count > 0 || objTFireSystemByPassPermit.Attachments.Count > 0)
//                    {
//                        pdfDoc.NewPage();
//                        isattach = true;
//                    }

//                    if (isattach)
//                    {
//                        if (objTFireSystemByPassPermit.DrawingAttachments.Count > 0)
//                        {
//                            table = new PdfPTable(1)
//                            {
//                                WidthPercentage = 100,
//                                HorizontalAlignment = 0,
//                                SpacingBefore = 10f
//                            };
//                            table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
//                            for (int i = 0; i < objTFireSystemByPassPermit.DrawingAttachments.Count; i++)
//                            {
//                                //table.AddCell(AddNewCell(objceilingPermit.DrawingAttachments[i].FullFileName, CellFont));
//                                //table.AddCell(AddNewCell("", CellFont));
//                                PdfGenerator.AddAttachmentCell(objTFireSystemByPassPermit.DrawingAttachments[i].ImagePath, objTFireSystemByPassPermit.DrawingAttachments[i].FullFileName, pdfWriter, table);


//                            }
//                            pdfDoc.Add(table);
//                        }
//                        if (objTFireSystemByPassPermit.Attachments.Count > 0)
//                        {
//                            table = new PdfPTable(1)
//                            {
//                                WidthPercentage = 100,
//                                HorizontalAlignment = 0,
//                                SpacingBefore = 10f
//                            };
//                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
//                            for (int i = 0; i < objTFireSystemByPassPermit.Attachments.Count; i++)
//                            {
//                                PdfGenerator.AddAttachmentCell(objTFireSystemByPassPermit.Attachments[i].FilePath, objTFireSystemByPassPermit.Attachments[i].FileName, pdfWriter, table);

//                            }
//                            pdfDoc.Add(table);
//                        }
//                    }

//                }



//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }

//        private static void CreateApprovalStamp(out PdfPTable stampTable, string Status, string ApproveBy, string Date, TFireSystemByPassPermit objTFireSystemByPassPermit)
//        {
//            stampTable = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingAfter = 5f

//            };
//            float[] widths = new float[] { 25, 300, 25 };
//            stampTable.SetTotalWidth(widths);
//            stampTable.AddCell(AddNewCell("SECTION 3:  Approval Stamp or Signature/Date  ", CellFontBoldBlueSmall, false, 3));
//            stampTable.AddCell(AddNewCell("     ", CellFontBoldBlueSmall, false, 3));
//            stampTable.AddCell(" ");
//            PdfPTable table2 = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingAfter = 5f

//            };
//            //table2.DefaultCell.Border = 1;
//            widths = new float[] { 45, 10, 45 };
//            table2.SetTotalWidth(widths);
//            PdfPCell cell = new PdfPCell
//            {
//                Border = 0
//            };
//            cell.Colspan = 3;
//            if (!string.IsNullOrEmpty(Status) && Status.ToUpper() == "PENDINGSUBMISSION")
//            {
//                Status = "PENDING";
//            }
//            cell.AddElement(new Phrase(!string.IsNullOrEmpty(Status) ? "                " + Status.ToUpper() : " ", CellStatusFont));
//            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
//            table2.AddCell(cell);
//            PdfPCell cell2 = new PdfPCell();
//            cell2.UseVariableBorders = true;
//            cell2.BorderColor = BaseColor.BLACK;
//            cell2.BorderWidth = 1f;
//            cell2.Border = PdfPCell.BOTTOM_BORDER;
//            cell2.Colspan = 3;
//            table2.AddCell(cell2);
//            if (objTFireSystemByPassPermit.ApprovalStatus != 1)
//            {
//                cell = new PdfPCell
//                {
//                    Border = 0
//                };
//                cell.AddElement(new Phrase(!string.IsNullOrEmpty(ApproveBy) ? ApproveBy : " ", CellFontNormalBlueSmall));
//                table2.AddCell(cell);
//                cell = new PdfPCell
//                {
//                    Border = 0
//                };
//                cell.AddElement(new Phrase("|", CellFontNormalBlueSmall));
//                table2.AddCell(cell);
//                cell = new PdfPCell
//                {
//                    Border = 0
//                };
//                cell.AddElement(new Phrase(!string.IsNullOrEmpty(Date) ? Date : " ", CellFontNormalBlueSmall));
//                table2.AddCell(cell);
//            }

//            if (objTFireSystemByPassPermit.DSFSBPApproverSign != null && objTFireSystemByPassPermit.DSFSBPApproverSign.DigSignatureId > 0)
//            {
//                //cell = new PdfPCell()
//                //{
//                //    Border = 0,
//                //};
//                //Image image = Image.GetInstance(Base.Common.FilePath(objTFireSystemByPassPermit.DSFSBPApproverSign.FilePath));
//                ////image.ScaleAbsolute(40, 40);
//                //cell.AddElement(new Chunk(image, 0, 0));
//                //table2.AddCell(cell);

//                //cell = new PdfPCell
//                //{
//                //    Border = 0
//                //};
//                //cell.Colspan = 2;
//                //cell.AddElement(new Phrase(objTFireSystemByPassPermit.DSFSBPApproverSign.SignByUserName + " (" + objTFireSystemByPassPermit.DSFSBPApproverSign.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFontSmall2));
//                //table2.AddCell(cell);
//                if (objTFireSystemByPassPermit.ApprovalStatus == 1)
//                {
//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    cellmain = CreateSignSectionCell(string.Empty, objTFireSystemByPassPermit.DSFSBPApproverSign);
//                    cellmain.Colspan = 3;
//                    table2.AddCell(cellmain);
//                }


//            }

//            cell2 = new PdfPCell
//            {
//                BorderWidth = 2f
//            };

//            cell2.AddElement(table2);
//            stampTable.AddCell(cell2);
//            stampTable.AddCell(" ");


//        }

//        #endregion

//        #region Method of procedure
//        [HttpGet]
//        public static string MOPPermitInbytes(int mopid)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TMOP mop = UnityContextFactory<IPermitService>.CreateContext().GetMethodofProcedure(mopid);
//            pdfDoc = CreateMOPPermit(mopid, mem, mop, true);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }
//        public static Document CreateMOPPermit(int id, Stream streamOutput, TMOP mop, bool IsAttachmentIncluded)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            //
//            if (mop != null)
//            {
//                string approverName = mop.ApprovedByUser != null ? mop.ApprovedByUser.Name : string.Empty;
//                if (mop.Status == 2 || mop.Status == -1)
//                {
//                    SetHeaderBlue(out table, "Method of Procedure (MOP)");
//                }
//                else
//                {
//                    SetStatusHeaderBlue(out table, "Method of Procedure (MOP)", mop.Status == 3 ? "Hold" : Enum.GetName(typeof(Enums.ApprovalStatus), mop.Status).ToString().ToUpper(), approverName, ($"{mop.ApproveDate:MMM d, yyyy}"));

//                }

//            }
//            else
//            {
//                SetHeaderBlue(out table, "Method of Procedure (MOP)");
//            }
//            PdfPTable tableapprove = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0

//            };
//            pdfDoc.Add(table);

//            if (mop != null)
//            {
//                table = new PdfPTable(6)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 20f,

//                };

//                var date = "";
//                var startdate = "";
//                var enddate = "";
//                if (mop.Date.HasValue && mop.Date != null)
//                {
//                    date = mop.Date.Value.ToString("MMM d, yyyy");
//                }
//                if (mop.StartDate.HasValue && mop.StartDate != null)
//                {
//                    startdate = mop.StartDate.Value.ToString("MMM d, yyyy");
//                }
//                if (mop.EndDate.HasValue && mop.EndDate != null)
//                {
//                    enddate = mop.EndDate.Value.ToString("MMM d, yyyy");
//                }
//                float[] widths = new float[] { 15f, 20f, 15f, 30f, 15f, 15f };
//                table.SetTotalWidth(widths);
//                table.DefaultCell.Border = Rectangle.NO_BORDER;
//                table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.PermitNo, CellFont, false));
//                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.ProjectName, CellFont, false));
//                table.AddCell(AddNewCell("Project #:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.Project.ProjectNumber, CellFont, false));

//                //table.AddCell(AddNewCell("", CellFont, false, 4));
//                //table.AddCell(AddNewCell("", CellFont, false, 4));
//                table.AddCell(AddNewCell("Request Date:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(date, CellFont, false));
//                table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.Contractor, CellFont, false));
//                table.AddCell(AddNewCell("Supervisor:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.Supervisor, CellFont, false)); ;
//                table.AddCell(AddNewCell("Building(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.BuildingName, CellFont, false));
//                table.AddCell(AddNewCell("Floor(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.FloorName, CellFont, false, 3));
//                table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(startdate, CellFont, false));
//                table.AddCell(AddNewCell("Start Time:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell($"{mop.StartTime:HH:MM}", CellFont, false));
//                table.AddCell(AddNewCell("Zone(s):", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.Zones, CellFont, false));
//                table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(enddate, CellFont, false));
//                table.AddCell(AddNewCell("End Time:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell($"{mop.EndTime:HH:MM}", CellFont, false, 3));
//                table.AddCell(AddNewCell("Request By:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.RequestBy != null && mop.RequestByUser != null && mop.RequestByUser.Name != null ? mop.RequestByUser.Name : " ", CellFont, false));
//                table.AddCell(AddNewCell("Email Adress:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(mop.EmailAddress, CellFont, false, 3));
//                pdfDoc.Add(table);
//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                PdfPCell nobordercell = new PdfPCell()
//                {
//                    Border = 0,
//                };
//                Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                ImgCheckBox.ScaleAbsolute(10f, 10f);

//                Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

//                //CreateSignSection(out table, string.Empty, mop.DSSign2Signature);
//                if (mop.RequestByUser != null && mop.DSSign2Signature != null && mop.DSSign2Signature.DigSignatureId > 0)
//                {
//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cellmain = CreateSignSectionCell(string.Empty, mop.DSSign2Signature, 30f);
//                    table.AddCell(cellmain);

//                }

//                //if (mop.DSSign2Signature != null && mop.DSSign2Signature.DigSignatureId > 0)
//                //{
//                //    PdfPCell cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    Image image = Image.GetInstance(Base.Common.FilePath(mop.DSSign2Signature.FilePath));
//                //    cell.AddElement(new Chunk(image, 0, -10));
//                //    cell.PaddingBottom = 0;
//                //    cell.PaddingTop = 0;
//                //    table.AddCell(cell);
//                //    cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//                //    cell.AddElement(line);
//                //    cell.UseAscender = true;
//                //    cell.PaddingBottom = 0;
//                //    cell.PaddingTop = 0;
//                //    table.AddCell(cell);

//                //    table.AddCell(AddNewCell(mop.RequesterSignatureId != null && mop.DSSign2Signature != null && mop.DSSign2Signature.SignByUserName != null ? mop.DSSign2Signature.SignByUserName + " ( " + mop.DSSign2Signature.SignByEmail + ")" : " ", CellFontS, false));
//                //    table.AddCell(AddNewCell(mop.RequesterSignatureId != null && mop.DSSign2Signature != null && mop.DSSign2Signature.LocalSignDateTime != null ? "(" + mop.DSSign2Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue));
//                //}
//                //else
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 1));
//                //}
//                pdfDoc.Add(table);

//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                table.AddCell(AddNewCell("Section 1:  Brief Description of Work", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(mop.Description == "" ? "N/A" : mop.Description, CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell("Section 2: Systems Impacted ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                PdfPTable tbldetail = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0

//                };
//                widths = new float[] { 10f, 10f, 35f, 45f };
//                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
//                tbldetail.SetWidths(widths);
//                tbldetail.AddCell(new Phrase("Yes", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("No", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Type", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Description", CellFontBoldBlueSmall));
//                nobordercell = new PdfPCell();
//                for (int i = 0; i < mop.SystemsImpacted.Count; i++)
//                {
//                    if (mop.SystemsImpacted[i].RespondStatus)
//                    {
//                        nobordercell = new PdfPCell();
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);

//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell = new PdfPCell();
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);

//                    }
//                    else
//                    {
//                        nobordercell = new PdfPCell();
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell = new PdfPCell();
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);
//                    }
//                    tbldetail.AddCell(new Phrase(mop.SystemsImpacted[i].AdditionalForms.FormName, CellFont));
//                    tbldetail.AddCell(new Phrase(mop.SystemsImpacted[i].AdditionalForms.Description, CellFont));
//                }

//                PdfPCell cellsystemimacted = new PdfPCell()
//                {
//                    Border = 0
//                };

//                cellsystemimacted.AddElement(tbldetail);
//                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
//                table.AddCell(cellsystemimacted);
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell("Section 3:  Additional Forms Required ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                tbldetail = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0

//                };
//                widths = new float[] { 10f, 10f, 35f, 45f };
//                tbldetail.SetWidths(widths);
//                tbldetail.AddCell(new Phrase("Yes", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("No", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Form", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Description", CellFontBoldBlueSmall));
//                nobordercell = new PdfPCell();
//                for (int i = 0; i < mop.AdditionalForms.Count; i++)
//                {
//                    if (mop.AdditionalForms[i].RespondStatus)
//                    {
//                        nobordercell = new PdfPCell();
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);

//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell = new PdfPCell();
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);

//                    }
//                    else
//                    {
//                        nobordercell = new PdfPCell();
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell = new PdfPCell();
//                        nobordercell.AddElement(chunk1);
//                        tbldetail.AddCell(nobordercell);
//                    }
//                    tbldetail.AddCell(new Phrase(mop.AdditionalForms[i].AdditionalForms.FormName, CellFont));
//                    tbldetail.AddCell(new Phrase(mop.AdditionalForms[i].AdditionalForms.Description, CellFont));
//                }

//                cellsystemimacted = new PdfPCell()
//                {
//                    Border = 0
//                };
//                cellsystemimacted.AddElement(tbldetail);
//                table.AddCell(cellsystemimacted);
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell("Section 4: Step by Step Procedure [Discuss, in technical detail, the work to be performed – include times]", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(mop.ProcedureSteps == "" ? "N/A" : mop.ProcedureSteps, CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell("Section 5: System Impact Area(s) [List all areas that will be affected by this project] # ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                tbldetail = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0

//                };
//                widths = new float[] { 10f, 90f };
//                tbldetail.SetWidths(widths);
//                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
//                tbldetail.AddCell(new Phrase("#", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Department / Floor / Zone / Room ", CellFontBoldBlueSmall));

//                nobordercell = new PdfPCell();
//                for (int i = 0; i < mop.SystemImpactArea.Count; i++)
//                {
//                    tbldetail.AddCell(new Phrase(Convert.ToString(i + 1), CellFont));
//                    tbldetail.AddCell(new Phrase(mop.SystemImpactArea[i].AreaName, CellFont));

//                }

//                cellsystemimacted = new PdfPCell()
//                {
//                    Border = 0
//                };
//                cellsystemimacted.AddElement(tbldetail);
//                table.AddCell(cellsystemimacted);
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" Section 6: Project Contact List [provide off-shift contacts, if applicable] ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                tbldetail = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                widths = new float[] { 10f, 30f, 30f, 30f };
//                tbldetail.SetWidths(widths);
//                //tbldetail.DefaultCell.Border = Rectangle.NO_BORDER;
//                tbldetail.AddCell(new Phrase("#", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Name", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Phone", CellFontBoldBlueSmall));
//                tbldetail.AddCell(new Phrase("Email", CellFontBoldBlueSmall));
//                nobordercell = new PdfPCell();
//                for (int i = 0; i < mop.ProjectContactList.Count; i++)
//                {
//                    tbldetail.AddCell(Convert.ToString(i + 1));
//                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].Name == "" ? " " : mop.ProjectContactList[i].Name, CellFont));
//                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].Phone == "" ? " " : mop.ProjectContactList[i].Phone, CellFont));
//                    tbldetail.AddCell(new Phrase(mop.ProjectContactList[i].EmailAddress == "" ? " " : mop.ProjectContactList[i].EmailAddress, CellFont));
//                }
//                cellsystemimacted = new PdfPCell()
//                {
//                    Border = 0
//                };
//                cellsystemimacted.AddElement(tbldetail);
//                table.AddCell(cellsystemimacted);
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell("Section 7: Follow-Up Required", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(mop.RequiredFollowUp == "" ? "N/A" : mop.RequiredFollowUp, CellFont, false));
//                table.AddCell(AddNewCell("Section 8:  Notifications Necessary ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(mop.NotificationsNecessary == "" ? "N/A" : mop.NotificationsNecessary, CellFont, false));
//                table.AddCell(AddNewCell("Section 9:  Additional Comments, Questions, and/or Concerns ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(mop.AdditionalComments == "" ? "N/A" : mop.AdditionalComments, CellFont, false));
//                table.AddCell(AddNewCell("Section 9:  Approval ", CellFontBoldBlueSmall, false));

//                if (mop.Status == 2)
//                {
//                    table.AddCell(AddNewCell("Status:Requested", CellFont, false));

//                }
//                else if (mop.Status == 1)
//                {
//                    table.AddCell(AddNewCell("Status:Approved", CellFont, false));
//                    //table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
//                    //table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
//                }
//                else if (mop.Status == 3)
//                {
//                    var label = "Reason(s) for Hold/Pending: ";
//                    var labelstatus = "Status:Hold ";
//                    table.AddCell(AddNewCell(labelstatus, CellFont, false));
//                    table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
//                    table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
//                    table.AddCell(AddNewCell(label + mop.RejectReason, CellFont, false));
//                }
//                else
//                {
//                    var label = mop.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
//                    var labelstatus = mop.Status == 0 ? "Status:Rejected " : "Status:Pending ";
//                    table.AddCell(AddNewCell(labelstatus, CellFont, false));
//                    table.AddCell(AddNewCell("Approve By:" + mop.ApproveBy != null && mop.ApprovedByUser != null && mop.ApprovedByUser.Name != null ? mop.ApprovedByUser.Name : " ", CellFont, false));
//                    table.AddCell(AddNewCell("Approval Date:" + ($"{mop.ApproveDate:MMM d, yyyy}"), CellFont, false));
//                    table.AddCell(AddNewCell(label + mop.RejectReason, CellFont, false));
//                }

//                pdfDoc.Add(table);
//                table = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0
//                };
//                if (mop.Status == 1)
//                {
//                    //if (mop.DSSign1Signature != null && mop.DSSign1Signature.DigSignatureId > 0)
//                    //{
//                    //    PdfPCell cell = new PdfPCell()
//                    //    {
//                    //        Border = 0,
//                    //    };
//                    //    Image image = Image.GetInstance(Base.Common.FilePath(mop.DSSign1Signature.FilePath));
//                    //    //image.ScaleAbsolute(80, 80);
//                    //    cell.AddElement(new Chunk(image, 0, 0));
//                    //    table.AddCell(cell);
//                    //    //cell = new PdfPCell()
//                    //    //{
//                    //    //    Border = 0,
//                    //    //};
//                    //    //cell.AddElement(new Phrase(mop.DSSign1Signature.SignByUserName + " (" + mop.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                    //    //table.AddCell(cell);

//                    //    table.AddCell(AddNewCell(mop.ApproverSignatureId != null && mop.DSSign1Signature != null && mop.DSSign1Signature.SignByUserName != null ? mop.DSSign1Signature.SignByUserName + " ( " + mop.DSSign1Signature.SignByEmail + ")" : " ", CellFontS, false));
//                    //    table.AddCell(AddNewCell(mop.ApproverSignatureId != null && mop.DSSign1Signature != null && mop.DSSign1Signature.LocalSignDateTime != null ? "(" + mop.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")" : string.Empty, CellFontDatetimeblue));
//                    //}
//                    //else
//                    //{
//                    //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                    //}

//                    //CreateSignSection(out table,string.Empty, mop.DSSign1Signature);

//                    if (mop.ApprovedByUser != null && mop.DSSign1Signature != null && mop.DSSign1Signature.DigSignatureId > 0)
//                    {
//                        PdfPCell cellmain = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        cellmain = CreateSignSectionCell(string.Empty, mop.DSSign1Signature, 30f);
//                        table.AddCell(cellmain);

//                    }
//                }
//                pdfDoc.Add(table);
//            }

//            bool isattach = false;
//            if (IsAttachmentIncluded)
//            {
//                if (mop.DrawingAttachments.Count > 0 || mop.Attachments.Count > 0)
//                {
//                    pdfDoc.NewPage();
//                    isattach = true;
//                }
//                if (isattach)
//                {
//                    if (mop.DrawingAttachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 10f
//                        };
//                        table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
//                        for (int i = 0; i < mop.DrawingAttachments.Count; i++)
//                        {

//                            PdfGenerator.AddAttachmentCell(mop.DrawingAttachments[i].ImagePath, mop.DrawingAttachments[i].FullFileName, pdfWriter, table);

//                        }
//                        pdfDoc.Add(table);
//                    }
//                    if (mop.Attachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 10f
//                        };

//                        table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack));
//                        for (int i = 0; i < mop.Attachments.Count; i++)
//                        {

//                            PdfGenerator.AddAttachmentCell(mop.Attachments[i].FilePath, mop.Attachments[i].FileName, pdfWriter, table);
//                        }
//                        pdfDoc.Add(table);
//                    }
//                }



//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }
//        #endregion

//        #region HWP
//        [HttpGet]
//        public static string HWPPermitInbytes(int? thotworkpermitid)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            THotWorkPermits objhotWorkPermits = UnityContextFactory<IHotWorkPermitService>.CreateContext().GetTHotWorksPermit(thotworkpermitid.Value);
//            pdfDoc = CreateHWPPermit(thotworkpermitid.Value, mem, objhotWorkPermits, true);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }
//        public static Document CreateHWPPermit(int id, Stream streamOutput, THotWorkPermits objhotWorkPermits, bool IsAttachmentIncluded)
//        {

//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;

//            if (objhotWorkPermits != null)
//            {
//                string approverName = objhotWorkPermits.AuthorizedByUser != null ? objhotWorkPermits.AuthorizedByUser.Name : string.Empty;
//                if (objhotWorkPermits.Status == 2 || objhotWorkPermits.Status == -1)
//                {
//                    SetHeaderBlue(out table, "Hot Work Permit (HWP)");
//                }
//                else
//                {
//                    SetStatusHeaderBlue(out table, "Hot Work Permit (HWP)", Enum.GetName(typeof(Enums.ApprovalStatus), objhotWorkPermits.Status).ToString().ToUpper(), approverName, ($"{objhotWorkPermits.PermitAuthorizedByDate:MMM d, yyyy}"));

//                }

//                pdfDoc.Add(table);
//                {

//                    table = new PdfPTable(6)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f,

//                    };
//                    float[] widths = new float[] { 19f, 18f, 19f, 16f, 15f, 13f };
//                    table.SetTotalWidth(widths);
//                    table.DefaultCell.Border = Rectangle.NO_BORDER;
//                    table.AddCell(AddNewCell("Permit #:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.PermitNo, CellFont, false));
//                    table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.ProjectName, CellFont, false));
//                    table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(($"{objhotWorkPermits.StartDate:MMM d,yyyy}"), CellFont, false));
//                    if (objhotWorkPermits.StartTime != null)
//                    {
//                        if (!string.IsNullOrEmpty(objhotWorkPermits.StartTime.ToString()))
//                        {
//                            objhotWorkPermits.StartTimeVal = TimeSpan.Parse(objhotWorkPermits.StartTime.ToString());
//                        }

//                        objhotWorkPermits.StartTime = objhotWorkPermits.StartTime.ToString();
//                        if (objhotWorkPermits.StartTimeVal.HasValue)
//                        {
//                            DateTime starttime = DateTime.Today.Add(objhotWorkPermits.StartTimeVal.Value);
//                            objhotWorkPermits.StartTime = starttime.ToString("hh:mm tt");

//                        }

//                    }

//                    if (objhotWorkPermits.EndTime != null)
//                    {
//                        if (!string.IsNullOrEmpty(objhotWorkPermits.EndTime.ToString()))
//                        {
//                            objhotWorkPermits.EndTimeVal = TimeSpan.Parse(objhotWorkPermits.EndTime.ToString());
//                        }

//                        objhotWorkPermits.EndTime = objhotWorkPermits.EndTime.ToString();
//                        if (objhotWorkPermits.EndTimeVal.HasValue)
//                        {
//                            DateTime endttime = DateTime.Today.Add(objhotWorkPermits.EndTimeVal.Value);
//                            objhotWorkPermits.EndTime = endttime.ToString("hh:mm tt");

//                        }


//                    }
//                    table.AddCell(AddNewCell("Start Time:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.StartTime, CellFont, false));
//                    table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(($"{objhotWorkPermits.EndDate:MMM d,yyyy}"), CellFont, false));
//                    table.AddCell(AddNewCell("End Time:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.EndTime, CellFont, false));
//                    table.AddCell(AddNewCell("Permit Requestor :", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.RequestorByUser != null && !string.IsNullOrEmpty(objhotWorkPermits.RequestorByUser.Name) ? objhotWorkPermits.RequestorByUser.Name : "", CellFont, false, 3));

//                    table.AddCell(AddNewCell("Organization:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.Company) ? objhotWorkPermits.Company : string.Empty, CellFont, false));
//                    table.AddCell(AddNewCell("Email Address:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.EmailAddress) ? objhotWorkPermits.EmailAddress : UserSession.CurrentUser.Email, CellFont, false, 3));
//                    table.AddCell(AddNewCell("Phone Number :", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(!string.IsNullOrEmpty(objhotWorkPermits.PhoneNumber) ? objhotWorkPermits.PhoneNumber : UserSession.CurrentUser.PhoneNumber, CellFont, false));
//                    table.AddCell(AddNewCell("On-Site Contact:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.OnSiteContact, CellFont, false, 3));
//                    table.AddCell(AddNewCell("On-Site Phone:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.OnSitePhone, CellFont, false));
//                    table.AddCell(AddNewCell("Building:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.BuildingName, CellFont, false, 3));
//                    table.AddCell(AddNewCell("Floor:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.FloorName, CellFont, false));
//                    table.AddCell(AddNewCell("Zone:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.Zones, CellFont, false));
//                    table.AddCell(AddNewCell("Room:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.Rooms, CellFont, false));
//                    table.AddCell(AddNewCell("Types of Work:", CellFontBoldBlack, false, 6));
//                    int TotalColumn = objhotWorkPermits.ConstructionWorkType.Count() + objhotWorkPermits.ConstructionWorkType.Count();
//                    PdfPTable tableWork = new PdfPTable(TotalColumn)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };
//                    List<float> list = new List<float>();



//                    for (int i = 0; i < TotalColumn; i++)
//                    {
//                        if (i == 7)
//                            list.Add(30f);
//                        else if (i == 9)
//                            list.Add(15f);
//                        else if (i % 2 == 0)
//                            list.Add(5f);
//                        else
//                            list.Add(12f);

//                    }
//                    widths = list.ToArray();
//                    tableWork.SetTotalWidth(widths);
//                    tableWork.DefaultCell.Border = Rectangle.NO_BORDER;
//                    Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                    ImgCheckBox.ScaleAbsolute(10f, 10f);

//                    Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


//                    PdfPCell nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    for (int i = 0; i < objhotWorkPermits.ConstructionWorkType.Count(); i++)
//                    {
//                        //if (i % 3 == 0 && i != 0)
//                        //{
//                        //    tableWork.AddCell(AddNewCell("", CellFontS, false, objhotWorkPermits.ConstructionWorkType.Count() - 3));
//                        //}
//                        nobordercell = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        if (objhotWorkPermits.ConstructionWorkType[i].IsChecked == 1)
//                        {
//                            Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
//                            nobordercell.AddElement(chunk1);
//                            tableWork.AddCell(nobordercell);

//                        }
//                        else
//                        {
//                            Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
//                            nobordercell.AddElement(chunk1);
//                            tableWork.AddCell(nobordercell);
//                        }
//                        tableWork.AddCell(AddNewCell(objhotWorkPermits.ConstructionWorkType[i].Name, CellFontS, false));

//                    }

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    nobordercell.Colspan = 6;
//                    nobordercell.AddElement(tableWork);
//                    table.AddCell(nobordercell);
//                    table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(objhotWorkPermits.Description, CellFont, false, 5));
//                    //table.AddCell(AddNewCell("Attach Drawings:", CellFontBoldBlack, false));
//                    //foreach (var drawings in objhotWorkPermits.DrawingAttachments)
//                    //{
//                    //    table.AddCell(AddNewCell(drawings.FullFileName, CellFont, false, 5));
//                    //    table.AddCell(AddNewCell("", CellFont, false));
//                    //}

//                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
//                    table.AddCell(AddNewCell("ITEMS REQUIRED TO PERFORM HOT WORK:", CellFontBoldBlack, false, 8));
//                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
//                    tableWork = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };
//                    widths = new float[] { 30f, 70f };
//                    tableWork.SetTotalWidth(widths);
//                    string text = "";
//                    for (var i = 0; i < objhotWorkPermits.THotWorkItems.OrderBy(x => x.ItemId).Count(); i++)
//                    {
//                        text = "";
//                        if (objhotWorkPermits.THotWorkItems[i].ParentId == 0)
//                        {
//                            tableWork.AddCell(new Phrase(objhotWorkPermits.THotWorkItems[i].Item, CellFontBoldBlack));



//                            foreach (var childitem in objhotWorkPermits.THotWorkItems.Where(x => x.ParentId == objhotWorkPermits.THotWorkItems[i].ItemId))
//                            {
//                                text += "   • " + childitem.Item;

//                            }
//                            tableWork.AddCell(new Phrase(text, CellFont));
//                        }
//                    }
//                    PdfPCell cell1 = new PdfPCell();
//                    cell1.Colspan = 8;
//                    cell1.AddElement(tableWork);
//                    table.AddCell(cell1);
//                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
//                    table.AddCell(AddNewCell("       ", CellFont, false, 6));
//                    tableWork = new PdfPTable(3)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };
//                    widths = new float[] { 15f, 5f, 80f };
//                    tableWork.SetTotalWidth(widths);

//                    PdfPCell cellWithRowspan = new PdfPCell(new Phrase("Agreement", CellRedtext));
//                    cellWithRowspan.Rowspan = 3;
//                    tableWork.AddCell(cellWithRowspan);
//                    nobordercell = new PdfPCell();
//                    if (objhotWorkPermits.IsVerifyHotWorkPerformed == true)
//                    {
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);

//                    }
//                    else
//                    {
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);
//                    }
//                    tableWork.AddCell(new Phrase("I verify that the following steps at the location where the hot work is being performed and adjacent areas have been inspected and applicable precautions have been checked and completed as necessary.", CellRedtext));

//                    nobordercell = new PdfPCell();
//                    if (objhotWorkPermits.IsVerifyObservedrevisited == true)
//                    {
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);

//                    }
//                    else
//                    {
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);
//                    }
//                    tableWork.AddCell(new Phrase("I verify the area was observed for thirty [30] minutes and then revisited one [1] hour, and that the Hot Work Permit was returned to Engineering at the completion of work", CellRedtext));

//                    nobordercell = new PdfPCell();
//                    if (objhotWorkPermits.IsVerifyAttach == true)
//                    {
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);

//                    }
//                    else
//                    {
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tableWork.AddCell(nobordercell);
//                    }
//                    tableWork.AddCell(new Phrase("I verify the Hot Work Notice [attached] is posted at the jobsite. ", CellRedtext));
//                    nobordercell = new PdfPCell();
//                    nobordercell.Colspan = 6;
//                    nobordercell.AddElement(tableWork);
//                    table.AddCell(nobordercell);
//                    table.AddCell(AddNewCell("  ", CellFontBoldBlack, false, 6));
//                    pdfDoc.Add(table);
//                    table = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,



//                    };
//                    widths = new float[] { 50f, 50f };
//                    table.SetTotalWidth(widths);
//                    int twocolcount = 0;
//                    if (objhotWorkPermits.PermitRequestor != null && objhotWorkPermits.DSSign1Signature != null && objhotWorkPermits.DSSign1Signature.DigSignatureId > 0)
//                    {
//                        PdfPCell cellmain1 = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        cellmain1 = CreateSignSectionCell("Permit Request By:", objhotWorkPermits.DSSign1Signature);
//                        table.AddCell(cellmain1);
//                        twocolcount++;

//                    }

//                    if (twocolcount == 0)
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                        twocolcount++;
//                    }

//                    PdfPTable tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,



//                    };
//                    widths = new float[] { 30f, 70f };
//                    tableinner.SetTotalWidth(widths);
//                    PdfPCell cellinner = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    tableinner.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
//                    if (objhotWorkPermits.Status == 2)
//                    {
//                        tableinner.AddCell(AddNewCell("Requested", CellFont, false));

//                    }
//                    else if (objhotWorkPermits.Status == 1)
//                    {
//                        tableinner.AddCell(AddNewCell("Approved", CellFont, false));
//                    }
//                    else
//                    {
//                        var labelstatus = objhotWorkPermits.Status == 0 ? "Rejected " : "Pending ";
//                        tableinner.AddCell(AddNewCell(labelstatus, CellFont, false));

//                        var label = objhotWorkPermits.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
//                        tableinner.AddCell(AddNewCell(label, CellFontBoldBlack, false));
//                        tableinner.AddCell(AddNewCell(objhotWorkPermits.Reason, CellFont, false));
//                    }

//                    //cellinner.AddElement(tableinner);
//                    //table.AddCell(cellinner);
//                    PdfPTable tableinner1 = new PdfPTable(1)
//                    {
//                        HorizontalAlignment = 0

//                    };
//                    widths = new float[] { 100f };
//                    tableinner1.SetTotalWidth(widths);

//                    cellinner = new PdfPCell()
//                    {
//                        Border = 0,
//                        PaddingLeft = 0

//                    };

//                    if (objhotWorkPermits.Status == 1)
//                    {
//                        if (objhotWorkPermits.AuthorizedByUser != null && objhotWorkPermits.DSSign2Signature != null && objhotWorkPermits.DSSign2Signature.DigSignatureId > 0)
//                        {
//                            PdfPCell cellmain1 = new PdfPCell()
//                            {
//                                Border = 0,
//                            };
//                            cellmain1 = CreateSignSectionCell("Approve By:", objhotWorkPermits.DSSign2Signature);
//                            tableinner1.AddCell(cellmain1);
//                        }

//                    }
//                    cellinner.Colspan = 2;
//                    cellinner.AddElement(tableinner1);
//                    tableinner.AddCell(cellinner);


//                    cellinner = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cellinner.AddElement(tableinner);
//                    table.AddCell(cellinner);
//                    pdfDoc.Add(table);


//                    bool isattach = false;
//                    if (IsAttachmentIncluded)
//                    {

//                        if (objhotWorkPermits.DrawingAttachments.Count > 0)
//                        {
//                            pdfDoc.NewPage();
//                            isattach = true;
//                        }
//                        if (isattach)
//                        {
//                            if (objhotWorkPermits.DrawingAttachments.Count > 0)
//                            {
//                                table = new PdfPTable(1)
//                                {
//                                    WidthPercentage = 100,
//                                    HorizontalAlignment = 0,
//                                    SpacingBefore = 10f
//                                };
//                                table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack));
//                                for (int i = 0; i < objhotWorkPermits.DrawingAttachments.Count; i++)
//                                {

//                                    PdfGenerator.AddAttachmentCell(objhotWorkPermits.DrawingAttachments[i].ImagePath, objhotWorkPermits.DrawingAttachments[i].FullFileName, pdfWriter, table);

//                                }
//                                pdfDoc.Add(table);
//                            }
//                        }

//                    }

//                    string Noticetext = UnityContextFactory<IOrganizationService>.CreateContext().GetOrganization().NoticeText;
//                    if (!string.IsNullOrEmpty(Noticetext))
//                    {
//                        pdfDoc.SetPageSize(PageSize.A4.Rotate());
//                        pdfDoc.NewPage();

//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0

//                        };


//                        Paragraph para = new Paragraph("NOTICE", TitleFontHWP)
//                        {
//                            Alignment = Element.ALIGN_CENTER,



//                        };
//                        nobordercell = new PdfPCell()
//                        {
//                            HorizontalAlignment = Element.ALIGN_CENTER,
//                            Border = 0,
//                            PaddingBottom = 15f,
//                            PaddingTop = -25f,
//                            PaddingLeft = -25f,
//                            PaddingRight = -25f


//                        };
//                        nobordercell.BackgroundColor = BaseColor.BLACK;
//                        nobordercell.AddElement(para);
//                        table.AddCell(nobordercell);


//                        para = new Paragraph("HOT WORK IN PROGRESS", TitleFont2HWP)
//                        {
//                            Alignment = Element.ALIGN_CENTER
//                        };
//                        nobordercell = new PdfPCell()
//                        {
//                            HorizontalAlignment = Element.ALIGN_MIDDLE,
//                            Border = 0
//                        };
//                        nobordercell.BackgroundColor = BaseColor.WHITE;
//                        nobordercell.AddElement(para);
//                        table.AddCell(nobordercell);
//                        table.AddCell(AddNewCell(" \n", CellFontBoldBlueSmall, false));

//                        PdfPCell tableCell = new PdfPCell()
//                        {
//                            Border = 0,
//                            HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL
//                        };

//                        string styles = "h2 ,p { text-align: center;font-size:21px; }";
//                        foreach (IElement element in XMLWorkerHelper.ParseToElementList(Noticetext, styles))
//                        {
//                            tableCell.AddElement(element);
//                        }
//                        table.AddCell(tableCell);
//                        pdfDoc.Add(table);
//                    }

//                    pdfWriter.CloseStream = false;
//                    pdfDoc.Close();

//                }
//            }
//            return pdfDoc;
//        }
//        #endregion

//        #region FMC
//        [HttpGet]
//        public static string FMCPermitInbytes(int? id)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TFacilitiesMaintenanceOccurrencePermit objTFMC = new TFacilitiesMaintenanceOccurrencePermit();
//            objTFMC = UnityContextFactory<IPermitService>.CreateContext().GetFacilitiesMaintenanceOccurrence(id.Value);

//            pdfDoc = CreateFMCPermit(id.Value, mem, objTFMC);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        public static Document CreateFMCPermit(int id, Stream streamOutput, TFacilitiesMaintenanceOccurrencePermit objFMC)
//        {

//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 26);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;

//            if (objFMC != null)
//            {
//                // SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form");
//                string approverName = objFMC.AuthorizedByUser != null ? objFMC.AuthorizedByUser.Name : string.Empty;
//                SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)");
//                //if (objFMC.Status == 2)
//                //{
//                //    SetHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)");
//                //}
//                //else
//                //{
//                //    SetStatusHeaderBlue(out table, "Facilities Maintenance Occurrence Form(FMC)", Enum.GetName(typeof(Enums.ApprovalStatus), objFMC.Status).ToString().ToUpper(), approverName, ($"{objFMC.ApprovedDate:MMM d, yyyy}"));

//                //}

//                pdfDoc.Add(table);
//                {

//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f,

//                    };
//                    PdfPTable tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    float[] widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    PdfPCell cellelement = new PdfPCell();
//                    Paragraph para = new Paragraph("ORIGINATOR", UnderlineTitleBoldFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER
//                    };
//                    cellelement.AddElement(para);
//                    cellelement.Colspan = 1;
//                    cellelement.AddElement(new Paragraph(" ", UnderlineCellFont));
//                    cellelement.BackgroundColor = Gray;
//                    table.AddCell(cellelement);
//                    tableinner.AddCell(new Phrase("Report #:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(string.Empty, UnderlineCellFont));
//                    tableinner.AddCell(new Phrase(objFMC.PermitNo, CellFont));
//                    tableinner.AddCell(new Phrase(string.Empty, CellFontBoldBlack));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Name:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase("Asset ID(if applicable):", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(objFMC.Name, CellFont));
//                    tableinner.AddCell(new Phrase(objFMC.AssetId.ToString(), CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Position:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase("Department:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(objFMC.Position, CellFont));
//                    tableinner.AddCell(new Phrase(objFMC.DepartmentName, CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Phone:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase("Shift:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(objFMC.PhoneNo, CellFont));
//                    tableinner.AddCell(new Phrase(objFMC.Shift, CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Date/Time Occurrence:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase("Report Date:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(((objFMC.DateOfOccurence == null || objFMC.DateOfOccurence.ToString() == "") && (objFMC.TimeOfOccurence == null || objFMC.TimeOfOccurence == "") ? "" : $"{objFMC.DateOfOccurence:MMM d, yyyy}" + " @ " + $"{objFMC.TimeOfOccurence:HH:MM}"), CellFont));
//                    tableinner.AddCell(new Phrase($"{objFMC.ReportDate:MMM d, yyyy}", CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Organization:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(string.Empty, UnderlineCellFont));
//                    tableinner.AddCell(new Phrase(objFMC.Company, CellFont));
//                    tableinner.AddCell(new Phrase(string.Empty, CellFontBoldBlack));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Drawings Attached:", UnderlineCellBoldFont));
//                    foreach (var drawings in objFMC.DrawingAttachments)
//                    {
//                        tableinner.AddCell(new Phrase(drawings.FullFileName, CellFont));
//                    }
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    cellelement = new PdfPCell();
//                    para = new Paragraph("DESCRIPTION", UnderlineTitleBoldFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER
//                    };
//                    cellelement.Colspan = 1;
//                    cellelement.AddElement(para);
//                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
//                    cellelement.BackgroundColor = Gray;
//                    table.AddCell(cellelement);
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(new Phrase("Classification", UnderlineCellBoldFont));
//                    table.AddCell(cellelement);
//                    int TotalColumn = objFMC.lstClassificationType.Count() + objFMC.lstClassificationType.Count();
//                    PdfPTable tableWork = new PdfPTable(10)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };
//                    widths = new float[] { 5f, 15f, 5f, 15f, 5f, 15f, 5f, 15f, 5f, 15f };
//                    tableWork.SetTotalWidth(widths);
//                    tableWork.DefaultCell.Border = Rectangle.NO_BORDER;
//                    Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                    ImgCheckBox.ScaleAbsolute(10f, 10f);

//                    Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


//                    PdfPCell nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    for (int i = 0; i < objFMC.lstClassificationType.Count(); i++)
//                    {

//                        nobordercell = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        if (objFMC.lstClassificationType[i].IsChecked == 1)
//                        {
//                            Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
//                            nobordercell.AddElement(chunk1);
//                            tableWork.AddCell(nobordercell);

//                        }
//                        else
//                        {
//                            Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
//                            nobordercell.AddElement(chunk1);
//                            tableWork.AddCell(nobordercell);
//                        }
//                        tableWork.AddCell(AddNewCell(objFMC.lstClassificationType[i].Name, CellFontS, false));

//                    }

//                    nobordercell = new PdfPCell();
//                    nobordercell.Colspan = 4;
//                    nobordercell.AddElement(tableWork);
//                    table.AddCell(nobordercell);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Location:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(objFMC.Location, CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 50f, 50f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase("Describe Occurrence:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(objFMC.OccurenceDetails, CellFont));
//                    tableinner.AddCell(new Phrase(" ", CellFont));

//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);
//                    cellelement = new PdfPCell();
//                    para = new Paragraph("ACTION TAKEN – RESOLUTIONS", UnderlineCellBoldFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER
//                    };
//                    cellelement.Colspan = 1;
//                    cellelement.AddElement(para);
//                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
//                    cellelement.BackgroundColor = Gray;
//                    table.AddCell(cellelement);

//                    tableinner = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(new Phrase(!string.IsNullOrEmpty(objFMC.ActionTaken) ? objFMC.ActionTaken : string.Empty, CellFont));
//                    cellelement = new PdfPCell
//                    {
//                        Colspan = 1
//                    };
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);

//                    cellelement = new PdfPCell();
//                    para = new Paragraph("\nCOMMENTS", UnderlineCellBoldFont)
//                    {
//                        Alignment = Element.ALIGN_CENTER
//                    };
//                    cellelement.Colspan = 1;
//                    cellelement.AddElement(para);
//                    cellelement.AddElement(new Chunk(" ", UnderlineCellFont));
//                    cellelement.BackgroundColor = Gray;
//                    table.AddCell(cellelement);
//                    tableinner = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    cellelement = new PdfPCell();
//                    tableinner.AddCell(new Phrase("Comments:", UnderlineCellBoldFont));
//                    tableinner.AddCell(new Phrase(!string.IsNullOrEmpty(objFMC.Comments) ? objFMC.Comments : string.Empty, CellFont));
//                    cellelement = new PdfPCell();
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);

//                    //table.AddCell(AddNewCell("Location:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(objFMC.Location, CellFont, false));
//                    //table.AddCell(AddNewCell("Describe Occurrence:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(objFMC.OccurenceDetails, CellFont, false));
//                    //table.AddCell(AddNewCell("Action Taken:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(objFMC.ActionTaken, CellFont, false));
//                    //table.AddCell(AddNewCell("Comments:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(objFMC.Comments, CellFont, false));

//                    tableinner = new PdfPTable(4)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingAfter = 20f,

//                    };
//                    widths = new float[] { 25f, 25f, 25f, 25f };
//                    tableinner.SetTotalWidth(widths);
//                    tableinner.DefaultCell.Border = Rectangle.NO_BORDER;
//                    tableinner.AddCell(AddNewCell("Completely Resolved :", UnderlineCellBoldFont));
//                    tableinner.AddCell(AddNewCell((objFMC.CompletelyResolved == 0) ? "No" : "Yes", CellFont));
//                    tableinner.AddCell(AddNewCell("Date :", UnderlineCellBoldFont));
//                    tableinner.AddCell(AddNewCell(($"{objFMC.CompleteDate:MMM d, yyyy}"), CellFont));
//                    //tableinner.AddCell(new Phrase((objFMC.CompletelyResolved == 0) ? "No" : "Yes", CellFont));
//                    //tableinner.AddCell(new Phrase(($"{objFMC.CompleteDate:MMM d, yyyy}"), CellFont));
//                    cellelement = new PdfPCell();


//                    PdfPTable commentBo = new PdfPTable(4)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };

//                    ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                    ImgCheckBox.ScaleAbsolute(10f, 10f);

//                    ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                    ImgUnCheckBox.ScaleAbsolute(10f, 10f);


//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    PdfPTable tablechild = new PdfPTable(4)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0

//                    };
//                    widths = new float[] { 5f, 25f, 5f, 65f };
//                    tablechild.SetWidths(widths);

//                    if (objFMC.AddedToEOC)
//                    {
//                        Chunk chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tablechild.AddCell(nobordercell);
//                        tablechild.AddCell(AddNewCell("Added to EOC Dashboard", CellFont, false));
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        nobordercell.AddElement(chunk1);
//                        tablechild.AddCell(nobordercell);
//                        tablechild.AddCell(AddNewCell("Do not include in EOC Dashboard ", CellFont, false));
//                    }
//                    else
//                    {
//                        Chunk chunk1 = new Chunk(ImgUnCheckBox, 0, 0);
//                        nobordercell.AddElement(chunk1);
//                        tablechild.AddCell(nobordercell);
//                        tablechild.AddCell(AddNewCell("Added to EOC Dashboard", CellFont, false));
//                        chunk1 = new Chunk(ImgCheckBox, 0, 0);
//                        nobordercell = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        nobordercell.AddElement(chunk1);
//                        tablechild.AddCell(nobordercell);
//                        tablechild.AddCell(AddNewCell("Do not include in EOC Dashboard ", CellFont, false));
//                    }
//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    nobordercell.AddElement(tablechild);
//                    nobordercell.Colspan = 4;
//                    tableinner.AddCell(nobordercell);
//                    cellelement.AddElement(tableinner);
//                    table.AddCell(cellelement);

//                    //table.AddCell(AddNewCell("Permit Requestor:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(objFMC.PermitRequestUser != null && !string.IsNullOrEmpty(objFMC.PermitRequestUser.Name) ? objFMC.PermitRequestUser.Name : "", CellFont, false));
//                    //table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
//                    //table.AddCell(AddNewCell(($"{objFMC.RequesterDate:MMM d, yyyy}"), CellFont, false));
//                    //if (objFMC.DSSign1Signature != null && objFMC.DSSign1Signature.DigSignatureId > 0)
//                    //{
//                    //    table.AddCell(AddNewCell("  ", CellFontBoldBlack, false));
//                    //    PdfPCell cell = new PdfPCell()
//                    //    {
//                    //        Border = 0,
//                    //    };
//                    //    Image image = Image.GetInstance(Base.Common.FilePath(objFMC.DSSign1Signature.FilePath));
//                    //    image.ScaleAbsolute(60, 60);
//                    //    cell.AddElement(image);
//                    //    cell.Colspan = 3;
//                    //    table.AddCell(cell);

//                    //}
//                    //else
//                    //{
//                    //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
//                    //}

//                    //table.AddCell(AddNewCell("Approval Status:", CellFontBoldBlack, false));
//                    //if (objFMC.Status == 2)
//                    //{
//                    //    table.AddCell(AddNewCell("Requested", CellFont, false, 3));

//                    //}
//                    //else if (objFMC.Status == 1)
//                    //{
//                    //    table.AddCell(AddNewCell("Approved", CellFont, false, 3));
//                    //    table.AddCell(AddNewCell("Approve By:", CellFontBoldBlack, false));
//                    //    table.AddCell(AddNewCell(objFMC.AuthorizedByUser != null && objFMC.AuthorizedByUser.Name != null ? objFMC.AuthorizedByUser.Name : " ", CellFont, false));
//                    //    table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
//                    //    table.AddCell(AddNewCell(($"{objFMC.ApprovedDate:MMM d, yyyy}"), CellFont, false));
//                    //}
//                    //else
//                    //{
//                    //    var label = objFMC.Status == 0 ? "Reason(s) for Rejection: " : "Reason(s) for Hold/Pending: ";
//                    //    var labelstatus = objFMC.Status == 0 ? "Rejected " : "Pending ";
//                    //    table.AddCell(AddNewCell(labelstatus, CellFont, false, 3));
//                    //    table.AddCell(AddNewCell("Approve By:", CellFontBoldBlack, false));
//                    //    table.AddCell(AddNewCell(objFMC.AuthorizedByUser != null && objFMC.AuthorizedByUser.Name != null ? objFMC.AuthorizedByUser.Name : " ", CellFont, false));
//                    //    table.AddCell(AddNewCell("Date:", CellFontBoldBlack, false));
//                    //    table.AddCell(AddNewCell(($"{objFMC.ApprovedDate:MMM d, yyyy}"), CellFont, false));
//                    //    table.AddCell(AddNewCell(label, CellFontBoldBlack, false));
//                    //    table.AddCell(AddNewCell(objFMC.Reason, CellFont, false, 3));
//                    //}
//                    //if (objFMC.Status == 1)
//                    //{
//                    //    if (objFMC.DSSign2Signature != null && objFMC.DSSign2Signature.DigSignatureId > 0)
//                    //    {
//                    //        table.AddCell(AddNewCell("  ", CellFontBoldBlack, false));
//                    //        PdfPCell cell = new PdfPCell()
//                    //        {
//                    //            Border = 0,
//                    //        };
//                    //        cell.Colspan = 3;
//                    //        Image image = Image.GetInstance(Base.Common.FilePath(objFMC.DSSign2Signature.FilePath));
//                    //        image.ScaleAbsolute(80, 80);
//                    //        cell.AddElement(image);
//                    //        table.AddCell(cell);

//                    //    }
//                    //    else
//                    //    {
//                    //        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
//                    //    }
//                    //}
//                    //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 4));
//                    pdfDoc.Add(table);
//                    pdfWriter.CloseStream = false;
//                    pdfDoc.Close();

//                }
//            }
//            return pdfDoc;
//        }

//        #endregion

//        #region PCRA
//        public static void PrintPermit(TIcraLog objTIcraLog, Document pdfDoc, TPCRAQuestion objQuestionTPCRA)
//        {
//            PdfPTable table = new PdfPTable(1);
//            PdfPCell cell = new PdfPCell();
//            System.Net.ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f
//            };
//            cell = new PdfPCell(new Phrase(UserSession.CurrentOrg.Name + " Infection Control Construction Permit", TitleFontS))
//            {
//                Colspan = 2,
//                HorizontalAlignment = Element.ALIGN_CENTER
//            };
//            table.AddCell(cell);
//            pdfDoc.Add(table);

//            if (objQuestionTPCRA != null)
//            {


//                table = new PdfPTable(4)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };
//                float[] width = new float[] { 5f, 35f, 5f, 55f };
//                table.SetTotalWidth(width);
//                objQuestionTPCRA.CheckBoxRiskAssesmentType = new List<EnumModel>();
//                foreach (RiskAssesmentType RiskAssesmentType in Enum.GetValues(typeof(RiskAssesmentType)))
//                {
//                    objQuestionTPCRA.CheckBoxRiskAssesmentType.Add(new EnumModel() { RiskAssesmentType = RiskAssesmentType, IsSelected = objQuestionTPCRA.RiskAssessmentType == (int)RiskAssesmentType ? true : false });
//                }
//                Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//                ImgCheckBox.ScaleAbsolute(10f, 10f);

//                Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//                ImgUnCheckBox.ScaleAbsolute(10f, 10f);

//                PdfPCell nobordercell = new PdfPCell();
//                Chunk chunk1 = new Chunk(ImgCheckBox, 0, 2);
//                for (int i = 0; i < objQuestionTPCRA.CheckBoxRiskAssesmentType.Count; i++)
//                {

//                    HCF.Utility.Enums.RiskAssesmentType enums = (HCF.Utility.Enums.RiskAssesmentType)objQuestionTPCRA.CheckBoxRiskAssesmentType[i].RiskAssesmentType;
//                    nobordercell = new PdfPCell() { Border = 0, };
//                    if (objQuestionTPCRA.CheckBoxRiskAssesmentType[i].IsSelected)
//                    {
//                        chunk1 = new Chunk(ImgCheckBox, 0, 2);
//                        nobordercell.AddElement(chunk1);
//                        table.AddCell(nobordercell);
//                        table.AddCell(AddNewCell(enums.GetDescription(), CellFontS, false));
//                    }
//                    else
//                    {
//                        chunk1 = new Chunk(ImgUnCheckBox, 0, 2);
//                        nobordercell.AddElement(chunk1);
//                        table.AddCell(nobordercell);
//                        table.AddCell(AddNewCell(enums.GetDescription(), CellFontS, false));
//                    }




//                }
//                cell = new PdfPCell(new Phrase("CRA #: " + objQuestionTPCRA.PCRANumber, CellFontS))
//                {
//                    Colspan = 4,
//                    Border = 0,
//                    HorizontalAlignment = Element.ALIGN_LEFT
//                };
//                table.AddCell(cell);

//                pdfDoc.Add(table);
//            }
//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f
//            };

//            table.AddCell(new Phrase("Project Name: " + objTIcraLog.ProjectName, CellFontS));
//            cell = new PdfPCell(new Phrase("Permit #: " + objTIcraLog.PermitNo, CellFontS))
//            {
//                Colspan = 2,
//                HorizontalAlignment = Element.ALIGN_RIGHT
//            };
//            table.AddCell(cell);

//            //table.AddCell(new Phrase("Project Start Date: " + (objTIcraLog.StartDate.HasValue ? objTIcraLog.StartDate.Value.ToFormatDate() : string.Empty), CellFontS));

//            table.AddCell(new Phrase("Location of Construction: " + objTIcraLog.Location, CellFontS));
//            table.AddCell(new Phrase("Project Start Date: " + (objTIcraLog.StartDate.HasValue ? objTIcraLog.StartDate.Value.ToFormatDate() : string.Empty), CellFontS));

//            table.AddCell(new Phrase("Project Coordinator: " + objTIcraLog.ProjectCoordinator, CellFontS));
//            table.AddCell(new Phrase("Estimated Duration: " + objTIcraLog.EstimatedDuration, CellFontS));

//            table.AddCell(new Phrase("Contractor Performing Work: " + objTIcraLog.Contractor, CellFontS));
//            table.AddCell(new Phrase("Permit Expiration Date: " + (objTIcraLog.CompletionDate.HasValue ? objTIcraLog.CompletionDate.Value.ToFormatDate() : string.Empty), CellFontS));

//            table.AddCell(new Phrase("Supervisor: " + objTIcraLog.Supervisor, CellFontS));
//            table.AddCell(new Phrase("Telephone: " + objTIcraLog.Telephone, CellFontS));
//            if (objQuestionTPCRA != null && objQuestionTPCRA.TicraId > 0)
//            {
//                table.AddCell(new Phrase("Scope: " + objTIcraLog.Scope, CellFontS));
//                table.AddCell(new Phrase("Phone Number: " + objQuestionTPCRA.Phone, CellFontS));
//                table.AddCell(new Phrase("Date Submitted: " + objQuestionTPCRA.DateSubmitted.Value.ToFormatDate(), CellFontS));
//                table.AddCell(new Phrase("Email Address : " + objQuestionTPCRA.EmailAddress, CellFontS));
//                table.AddCell(new Phrase("Building : " + objQuestionTPCRA.BuildingName, CellFontS));
//                table.AddCell(new Phrase("Floor : " + objQuestionTPCRA.FloorName, CellFontS));

//                cell = new PdfPCell(new Phrase("Brief Description of Work : " + objQuestionTPCRA.WorkDescription, CellFontS))
//                {
//                    Colspan = 2,
//                    HorizontalAlignment = Element.ALIGN_LEFT
//                };
//                table.AddCell(cell);
//                pdfDoc.Add(table);
//                //table = new PdfPTable(7)
//                //{
//                //    WidthPercentage = 100,
//                //    HorizontalAlignment = 0
//                //};
//                //table.AddCell(new Phrase("S.No", CellFontS));
//                //table.AddCell(new Phrase("Above", CellFontS));
//                //table.AddCell(new Phrase("Below", CellFontS));
//                //table.AddCell(new Phrase("North", CellFontS));
//                //table.AddCell(new Phrase("South", CellFontS));
//                //table.AddCell(new Phrase("East", CellFontS));
//                //table.AddCell(new Phrase("West", CellFontS));
//                //for (int i = 0; i < objQuestionTPCRA.TDepartmentNearConstruction.Count; i++)
//                //{
//                //    table.AddCell(new Phrase((i+1).ToString(), CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].Above, CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].below, CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].North, CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].South, CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].East, CellFontS));
//                //    table.AddCell(new Phrase(objQuestionTPCRA.TDepartmentNearConstruction[i].West, CellFontS));

//                //}
//                //pdfDoc.Add(table);
//            }
//            else
//            {
//                cell = new PdfPCell(new Phrase("Scope: " + objTIcraLog.Scope, CellFontS))
//                {
//                    Colspan = 2,
//                    HorizontalAlignment = Element.ALIGN_LEFT
//                };
//                table.AddCell(cell);
//                pdfDoc.Add(table);
//            }
//            float[] widths = new float[] { 15f, 35f, 15f, 35f };
//            table = new PdfPTable(4)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0
//            };
//            table.SetWidths(widths);
//            table.AddCell(new Phrase("YES/NO", CellFontS));
//            table.AddCell(new Phrase("CONSTRUCTION ACTIVITY", CellFontS));
//            table.AddCell(new Phrase("YES/NO", CellFontS));
//            table.AddCell(new Phrase("INFECTION CONTROL RISK GROUP", CellFontS));
//            bool addtotable = false;
//            for (int i = 0; i < objTIcraLog.ConstructionTypes.Count; i++)
//            {
//                string typetext = objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId ? "YES" : "NO";
//                PdfPCell cell1 = new PdfPCell(new Phrase(typetext, CellFontS));
//                PdfPCell cell2 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[i].TypeName + ": " + objTIcraLog.ConstructionTypes[i].Description, CellFontS));
//                if (objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                {
//                    cell1.BackgroundColor = Gray;
//                    cell2.BackgroundColor = Gray;
//                    table.AddCell(cell1);
//                    table.AddCell(cell2);
//                    addtotable = true;
//                }


//                string risktext = objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId ? "YES" : "NO";
//                PdfPCell cell3 = new PdfPCell(new Phrase(risktext, CellFontS));
//                PdfPCell cell4 = new PdfPCell();
//                for (int j = i; j <= i; j++)
//                {
//                    cell4 = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[i].GroupName + ": " + objTIcraLog.ConstructionRisks[i].RiskName, CellFontS));
//                }
//                if (objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//                {
//                    cell3.BackgroundColor = Gray;
//                    cell4.BackgroundColor = Gray;
//                    table.AddCell(cell3);
//                    table.AddCell(cell4);
//                    addtotable = true;
//                }

//            }
//            if (addtotable)
//            {
//                pdfDoc.Add(table);
//            }

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0
//            };
//            widths = new float[] { 40f, 60f };
//            table.SetWidths(widths);
//            for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
//            {
//                string activity = objTIcraLog.ConstructionTypes[j].Description + "\n" + "Includes, but is not limited to: \n";
//                foreach (var item in objTIcraLog.ConstructionTypes[j].ConstructionActivity)
//                {
//                    activity = activity + "• " + item.Activity + "\n";
//                }
//                PdfPCell cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
//                PdfPCell cell2 = new PdfPCell(new Phrase(activity, CellFontS));
//                if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                {
//                    cell1.BackgroundColor = Gray;
//                    cell2.BackgroundColor = Gray;
//                    table.AddCell(cell1);
//                    table.AddCell(cell2);
//                }

//            }
//            pdfDoc.Add(table);
//            //for (int i = 0; i < objTIcraLog.ConstructionTypes.Count; i++)
//            //{
//            //    string typetext = objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId ? "YES" : "NO";
//            //    PdfPCell cell1 = new PdfPCell(new Phrase(typetext, CellFontS));
//            //    PdfPCell cell2 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[i].TypeName + ": " + objTIcraLog.ConstructionTypes[i].Description, CellFontS));
//            //    if (objTIcraLog.ConstructionTypes[i].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//            //    {
//            //        cell1.BackgroundColor = Gray;
//            //        cell2.BackgroundColor = Gray;
//            //    }
//            //    table.AddCell(cell1);
//            //    table.AddCell(cell2);

//            //    string risktext = objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId ? "YES" : "NO";
//            //    PdfPCell cell3 = new PdfPCell(new Phrase(risktext, CellFontS));
//            //    PdfPCell cell4 = new PdfPCell();
//            //    for (int j = i; j <= i; j++)
//            //    {
//            //        cell4 = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[i].GroupName + ": " + objTIcraLog.ConstructionRisks[i].RiskName, CellFontS));
//            //    }
//            //    if (objTIcraLog.ConstructionRisks[i].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//            //    {
//            //        cell3.BackgroundColor = Gray;
//            //        cell4.BackgroundColor = Gray;
//            //    }
//            //    table.AddCell(cell3);
//            //    table.AddCell(cell4);
//            //}
//            //pdfDoc.Add(table);

//            float[] widths1 = new float[] { 10f, 45f, 45f };
//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0
//            };
//            table.SetWidths(widths1);
//            for (int i = 0; i < objTIcraLog.ConstructionClasses.Count; i++)
//            {
//                int Activitycount = objTIcraLog.ConstructionClasses[i].ConstructionClassActivity.Count;
//                int count = Activitycount % 2 == 0 ? ((Activitycount / 2)) : ((Activitycount / 2) + 1);
//                string activity = string.Empty;
//                string activity1 = string.Empty;
//                PdfPCell cell1;
//                if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == 3)
//                {
//                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName + '\n' + (objTIcraLog.Class3Date.HasValue ? objTIcraLog.Class3Date.Value.ToFormatDate() : string.Empty) + '\n' + objTIcraLog.Class3Initial, CellFontS));
//                }
//                else if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == 4)
//                {
//                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName + '\n' + (objTIcraLog.Class4Date.HasValue ? objTIcraLog.Class4Date.Value.ToFormatDate() : string.Empty) + '\n' + objTIcraLog.Class4Initial, CellFontS));
//                }
//                else
//                {
//                    cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionClasses[i].ClassName, CellFontS));
//                }



//                PdfPCell cell2 = new PdfPCell();
//                PdfPCell cell3 = new PdfPCell();
//                for (int j = 0; j < count; j++)
//                {
//                    if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId && (!string.IsNullOrEmpty(objTIcraLog.ActivityLists) && !objTIcraLog.ActivityLists.Split(',').Contains(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].ConstClassActivityId.ToString())))
//                        cell2.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", Strikethru));
//                    else
//                        cell2.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", CellFontS));
//                    //activity = activity + "•" + objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n";
//                }
//                for (int j = count; j < objTIcraLog.ConstructionClasses[i].ConstructionClassActivity.Count; j++)
//                {
//                    if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId && (!string.IsNullOrEmpty(objTIcraLog.ActivityLists) && !objTIcraLog.ActivityLists.Split(',').Contains(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].ConstClassActivityId.ToString())))
//                        cell3.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", Strikethru));
//                    else
//                        cell3.AddElement(new Phrase(objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n", CellFontS));
//                    //activity1 = activity1 + "•" + objTIcraLog.ConstructionClasses[i].ConstructionClassActivity[j].Activity + "\n";
//                }
//                if (objTIcraLog.ConstructionClasses[i].ConstructionClassId == objTIcraLog.ConstructionClassId)
//                {
//                    cell1.BackgroundColor = Gray;
//                    cell2.BackgroundColor = Gray;
//                    cell3.BackgroundColor = Gray;
//                    table.AddCell(cell1);
//                    table.AddCell(cell2);
//                    table.AddCell(cell3);
//                }

//            }
//            cell = new PdfPCell(new Phrase("Additional Requirements: " + objTIcraLog.Comment, CellFontS))
//            {
//                Colspan = 3
//            };
//            table.AddCell(cell);

//            pdfDoc.Add(table);

//            float[] widths3 = new float[] { 50f, 25f, 25f };
//            table = new PdfPTable(3)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0
//            };
//            table.SetWidths(widths3);
//            //cell = new PdfPCell(new Phrase(string.Empty))
//            //{
//            //    Colspan = 3
//            //};
//            //table.AddCell(cell);
//            HCF.Utility.Enums.ApprovalStatus enums1 = (HCF.Utility.Enums.ApprovalStatus)objTIcraLog.Status;
//            table.AddCell(new Phrase("Status : " + (objTIcraLog.Status == 3 ? "Hold" : enums1.GetDescription()), CellFontS));
//            table.AddCell(new Phrase("Date: " + (objTIcraLog.Date1.HasValue ? objTIcraLog.Date1.Value.ToFormatDate() : string.Empty) + " Initials: " + objTIcraLog.Initial1, CellFontS));
//            table.AddCell(new Phrase("Date: " + (objTIcraLog.Date2.HasValue ? objTIcraLog.Date2.Value.ToFormatDate() : string.Empty) + " Initials: " + objTIcraLog.Initial2, CellFontS));
//            pdfDoc.Add(table);

//            if (objQuestionTPCRA != null && objQuestionTPCRA.TicraId > 0)
//            {
//            }
//            else
//            {
//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };
//                int twocolcount = 0;
//                foreach (var permitworkflow in objTIcraLog.TPermitWorkFlowDetails)
//                {
//                    if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
//                    {
//                        PdfPCell cellmain = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, 45f);
//                        table.AddCell(cellmain);

//                        twocolcount++;
//                    }

//                    if (twocolcount == 0)
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                        twocolcount++;
//                    }

//                    if (twocolcount % 2 == 0)
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                        twocolcount++;
//                    }

//                }

//                if (twocolcount % 2 != 0)
//                {
//                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                }
//                pdfDoc.Add(table);

//                //table = new PdfPTable(2)
//                //{
//                //    WidthPercentage = 100,
//                //    HorizontalAlignment = 0
//                //};
//                //int twocolcount = 0;

//                //if (objTIcraLog.PermitRequestByUser != null && objTIcraLog.DSPermitRequestBy != null && objTIcraLog.DSPermitRequestBy.DigSignatureId > 0)
//                //{
//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Permit Request By:", objTIcraLog.DSPermitRequestBy, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;
//                //}
//                //if (twocolcount == 0)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}

//                //if (objTIcraLog.PermitReviewerBy != null && objTIcraLog.DSPermitReviewerBy != null && objTIcraLog.DSPermitReviewerBy.DigSignatureId > 0)
//                //{

//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Reviewer 1:", objTIcraLog.DSPermitReviewerBy, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;
//                //}
//                //if (twocolcount == 2)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}
//                //if (objTIcraLog.PermitAuthorizedBy != null && objTIcraLog.DSPermitAuthorizedBy != null && objTIcraLog.DSPermitAuthorizedBy.DigSignatureId > 0)
//                //{
//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Reviewer 2:", objTIcraLog.DSPermitAuthorizedBy, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;
//                //}

//                //if (twocolcount % 2 == 0)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}
//                //if (twocolcount == 1)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //}

//                //pdfDoc.Add(table);
//            }
//            if (objQuestionTPCRA != null)
//            {

//                //table = new PdfPTable(2)
//                //{
//                //    WidthPercentage = 100,
//                //    HorizontalAlignment = 0,
//                //    SpacingBefore = 10f
//                //};
//                //int twocolcount = 0;
//                //if (objQuestionTPCRA.ContractorId > 0 && objQuestionTPCRA.DSContractorSignature != null && objQuestionTPCRA.DSContractorSignature.DigSignatureId > 0)
//                //{
//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Contractor/ Requester:", objQuestionTPCRA.DSContractorSignature, 45f);
//                //    table.AddCell(cellmain);

//                //    twocolcount++;
//                //}
//                //if (twocolcount == 0)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}
//                //if (objQuestionTPCRA.InfectionControlId > 0 && objQuestionTPCRA.DSInfectionControlSignature != null && objQuestionTPCRA.DSInfectionControlSignature.DigSignatureId > 0)
//                //{

//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Infection Control: ", objQuestionTPCRA.DSInfectionControlSignature, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;
//                //}
//                //if (twocolcount == 2)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}
//                //if (objQuestionTPCRA.FacilityId > 0 && objQuestionTPCRA.DSFacilitySignature != null && objQuestionTPCRA.DSFacilitySignature.DigSignatureId > 0)
//                //{
//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Facilities: ", objQuestionTPCRA.DSFacilitySignature, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;
//                //}
//                //if (twocolcount % 2 == 0)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //    twocolcount++;
//                //}

//                //if (objQuestionTPCRA.SafetyId > 0 && objQuestionTPCRA.DSSafetySignature != null && objQuestionTPCRA.DSSafetySignature.DigSignatureId > 0)
//                //{

//                //    PdfPCell cellmain = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cellmain = CreateSignSectionCell("Safety:", objQuestionTPCRA.DSSafetySignature, 45f);
//                //    table.AddCell(cellmain);
//                //    twocolcount++;

//                //}
//                //if (twocolcount % 2 != 0)
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //}
//                //pdfDoc.Add(table);


//                table = new PdfPTable(2)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };
//                int twocolcount = 0;
//                foreach (var permitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
//                {
//                    if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
//                    {
//                        PdfPCell cellmain = new PdfPCell()
//                        {
//                            Border = 0,
//                        };
//                        cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, 45f);
//                        table.AddCell(cellmain);

//                        twocolcount++;
//                    }

//                    if (twocolcount == 0)
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                        twocolcount++;
//                    }

//                    if (twocolcount % 2 == 0)
//                    {
//                        table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                        twocolcount++;
//                    }

//                }

//                if (twocolcount % 2 != 0)
//                {
//                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                }
//                pdfDoc.Add(table);
//            }

//        }

//        [HttpGet]
//        public static string PCRAPermitWorksheetBytes(int projectId, int tpcraquestid, int icraId, string PDFName)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
//            objQuestionTPCRA = UnityContextFactory<IPCRAService>.CreateContext().GetQuestionTPCRA(projectId, tpcraquestid);

//            if (icraId > 0)
//            {
//                pdfDoc = CreateICRAPermit(icraId, mem, objQuestionTPCRA, 0);
//            }
//            else
//            {
//                pdfDoc = CreatePCRAPermit(objQuestionTPCRA.TicraId, mem, objQuestionTPCRA, 1);
//            }

//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }
//        public static Document CreatePCRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int PermitType)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            TIcraLog objTIcraLog = getTicralog(icraId);

//            PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
//            Paragraph para = new Paragraph("Infection Control Risk Assessment \n Matrix of Precautions for Construction & Renovation", TitleFontS)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);
//            Paragraph line = new Paragraph();
//            PdfPTable table = new PdfPTable(1);
//            PdfPCell cell = new PdfPCell();
//            for (int i = 0; i < objTIcraLog.TIcraSteps.Count; i++)
//            {
//                if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 1)
//                {
//                    float[] widths = new float[] { 40f, 60f };
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Type of Construction Project Activity (Type A-D)", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    table.SetWidths(widths);
//                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
//                    {
//                        string activity = objTIcraLog.ConstructionTypes[j].Description + "\n" + "Includes, but is not limited to: \n";
//                        foreach (var item in objTIcraLog.ConstructionTypes[j].ConstructionActivity)
//                        {
//                            activity = activity + "• " + item.Activity + "\n";
//                        }
//                        PdfPCell cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
//                        PdfPCell cell2 = new PdfPCell(new Phrase(activity, CellFontS));
//                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                        {
//                            cell1.BackgroundColor = Gray;
//                            cell2.BackgroundColor = Gray;
//                        }
//                        table.AddCell(cell1);
//                        table.AddCell(cell2);
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);
//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 2)
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Patient Risk Groups that will be affected. If more than one risk group will be affected, select the higher risk group:", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(4)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    for (int k = 0; k < objTIcraLog.ConstructionRisks.Count; k++)
//                    {
//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[k].RiskName, CellFontS));
//                        if (objTIcraLog.ConstructionRisks[k].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//                        {
//                            cell.BackgroundColor = Gray;
//                        }
//                        table.AddCell(cell);
//                    }
//                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
//                    {
//                        para = new Paragraph();
//                        foreach (var item in objTIcraLog.ConstructionRisks[j].RiskArea)
//                        {
//                            para.Add(new Phrase("• " + item.Name + "\n", ParagraphFontS));
//                        }
//                        table.AddCell(para);
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);

//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 3)
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Patient Risk Group (Low, Medium, High, Highest) with the planned Construction Project Type (A, B, C, D) on the following matrix, to find the Class of Precautions (I, II, III or IV) or level of infection control activities required. Class I-IV or Color-Coded Precautions are delineated on the following page.\nIC Matrix - Class of Precautions: Construction Project by Patient Risk", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        //SpacingAfter = 30f
//                    };
//                    pdfDoc.Add(para);
//                    para = new Paragraph("\nConstruction Project Type ", ParagraphFontS)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        //SpacingBefore = 30f
//                    };
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(5)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    //table.SpacingAfter = 30f;
//                    table.AddCell(new Phrase("Patient Risk Group", CellFontS));
//                    int? ConstructionTypeId = 0;
//                    int? ConstructionRiskId = 0;
//                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
//                    {

//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
//                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                        {
//                            cell.BackgroundColor = Gray;
//                            ConstructionTypeId = objTIcraLog.ConstructionTypeId;
//                        }
//                        table.AddCell(cell);
//                    }
//                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
//                    {
//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
//                        if (objTIcraLog.ConstructionRisks[j].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//                        {
//                            cell.BackgroundColor = Gray;
//                            ConstructionRiskId = objTIcraLog.ConstructionRiskId;
//                        }

//                        table.AddCell(cell);
//                        //table.AddCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
//                        for (int k = 0; k < objTIcraLog.ConstructionTypes.Count; k++)
//                        {
//                            var str = string.Empty;
//                            var className = string.Empty;
//                            var ClassId = string.Empty;

//                            var data = objTIcraLog.ICRAMatixPrecautions.Where(x => x.ConstructionRiskId == objTIcraLog.ConstructionRisks[j].ConstructionRiskId && x.ConstructionTypeId == objTIcraLog.ConstructionTypes[k].ConstructionTypeId).ToList();
//                            if (data.Count > 0)
//                            {
//                                str = string.Join(",", data.Select(x => x.ConstructionClass.ClassName));
//                                className = data.FirstOrDefault().ConstructionClass.ClassName.Replace(" ", "").ToLower();

//                            }

//                            cell = new PdfPCell(new Phrase(str, CellFontS));

//                            if (objTIcraLog.ConstructionTypes[k].ConstructionTypeId == ConstructionTypeId && objTIcraLog.ConstructionRisks[j].ConstructionRiskId == ConstructionRiskId)
//                            {
//                                cell.BackgroundColor = BaseColor.BLUE;
//                            }
//                            table.AddCell(cell);
//                        }
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Note: Infection Control approval will be required when the Construction Activity and Risk Level indicate that Class IIIor Class IVcontrol procedures are necessary.", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);

//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 4)
//                {
//                    para = new Paragraph("Step 4:Identify the areas surrounding the project area, assessing potential impact # " + objTIcraLog.ProjectNo, ParagraphFontS);
//                    pdfDoc.Add(para);

//                    table = new PdfPTable(6)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 10f
//                    };
//                    //table.SpacingAfter = 30f;

//                    //for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
//                    //{
//                    //    string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames : string.Empty;
//                    //    table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + riskarea, CellFontS));
//                    //}
//                    string risklist = "";
//                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
//                    {
//                        string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames + "," : string.Empty;
//                        risklist = risklist + riskarea;
//                        table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + risklist, CellFontS));
//                    }
//                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
//                    {
//                        string riskName = objTIcraLog.AreasSurroundings[k].ConstructionRisk != null ? objTIcraLog.AreasSurroundings[k].ConstructionRisk.RiskName : string.Empty;
//                        table.AddCell(new Phrase("Risk Group : " + riskName, CellFontS));
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);
//                }
//                else
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ": " + objTIcraLog.TIcraSteps[i].Step.Steps + "\n", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment, ParagraphFontS)
//                    {
//                        SpacingBefore = 5f

//                    };
//                    pdfDoc.Add(para);
//                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
//                    pdfDoc.Add(line);
//                }
//            }
//            if (PermitType == 1)
//            {
//                //para = new Paragraph("Section Approval:", ParagraphFontS);
//                //pdfDoc.Add(para);
//                table = new PdfPTable(3)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 10f
//                };

//                PdfPTable tableContractor = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                };
//                PdfPTable tableinfectionist = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                };
//                PdfPTable tablesafety = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                };
//                PdfPTable tablefacility = new PdfPTable(1)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                };
//                tableContractor.AddCell(AddNewCell("Contractor/ Requester:", smallfontbold, false));
//                tableContractor.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
//                tablefacility.AddCell(AddNewCell("Facilities:", smallfontbold, false));
//                tablefacility.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
//                tableContractor.AddCell(AddNewCell(objQuestionTPCRA.ContractorId != null && objQuestionTPCRA.ContractorUser != null && objQuestionTPCRA.ContractorUser.Name != null ? objQuestionTPCRA.ContractorUser.Name : " ", ParagraphFontS, false));
//                tableContractor.AddCell(AddNewCell(($"{objQuestionTPCRA.ContractorSignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
//                tablefacility.AddCell(AddNewCell(objQuestionTPCRA.FacilityId != null && objQuestionTPCRA.FacilityUser != null && objQuestionTPCRA.FacilityUser.Name != null ? objQuestionTPCRA.FacilityUser.Name : " ", ParagraphFontS, false));
//                tablefacility.AddCell(AddNewCell(($"{objQuestionTPCRA.FacilitySignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
//                if (objQuestionTPCRA.DSContractorSignature != null && objQuestionTPCRA.DSContractorSignature.DigSignatureId > 0)
//                {
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    Image image = Image.GetInstance(Base.Common.FilePath(objQuestionTPCRA.DSContractorSignature.FilePath));
//                    //image.ScaleAbsolute(80, 80);
//                    cell.AddElement(new Chunk(image, 0, 0));
//                    tableContractor.AddCell(cell);
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cell.AddElement(new Phrase(objQuestionTPCRA.DSContractorSignature.SignByUserName + " (" + objQuestionTPCRA.DSContractorSignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                    tableContractor.AddCell(cell);

//                }
//                else
//                {
//                    tableContractor.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
//                }
//                if (objQuestionTPCRA.DSFacilitySignature != null && objQuestionTPCRA.DSFacilitySignature.DigSignatureId > 0)
//                {
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    Image image = Image.GetInstance(Base.Common.FilePath(objQuestionTPCRA.DSFacilitySignature.FilePath));
//                    //image.ScaleAbsolute(80, 80);
//                    cell.AddElement(new Chunk(image, 0, 0));
//                    tablefacility.AddCell(cell);
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cell.AddElement(new Phrase(objQuestionTPCRA.DSFacilitySignature.SignByUserName + " (" + objQuestionTPCRA.DSFacilitySignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                    tablefacility.AddCell(cell);

//                }
//                else
//                {
//                    tablefacility.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
//                }
//                tableinfectionist.AddCell(AddNewCell("Infection Control: ", smallfontbold, false));
//                tableinfectionist.AddCell(AddNewCell("Approval Date:", smallfontbold, false));
//                tablesafety.AddCell(AddNewCell("Safety:", smallfontbold, false));
//                tablesafety.AddCell(AddNewCell("Approval Date:", smallfontbold, false));

//                tableinfectionist.AddCell(AddNewCell(objQuestionTPCRA.InfectionControlId != null && objQuestionTPCRA.InfectionControlUser != null && objQuestionTPCRA.InfectionControlUser.Name != null ? objQuestionTPCRA.InfectionControlUser.Name : " ", ParagraphFontS, false));
//                tableinfectionist.AddCell(AddNewCell(($"{objQuestionTPCRA.InfectionControlSignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
//                tablesafety.AddCell(AddNewCell(objQuestionTPCRA.SafetyId != null && objQuestionTPCRA.SafetyUser != null && objQuestionTPCRA.SafetyUser.Name != null ? objQuestionTPCRA.SafetyUser.Name : " ", ParagraphFontS, false));
//                tablesafety.AddCell(AddNewCell(($"{objQuestionTPCRA.SafetySignatureDate:MMM d, yyyy}"), ParagraphFontS, false));
//                if (objQuestionTPCRA.DSInfectionControlSignature != null && objQuestionTPCRA.DSInfectionControlSignature.DigSignatureId > 0)
//                {
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    Image image = Image.GetInstance(Base.Common.FilePath(objQuestionTPCRA.DSInfectionControlSignature.FilePath));
//                    //image.ScaleAbsolute(80, 80);
//                    cell.AddElement(new Chunk(image, 0, 0));
//                    tableinfectionist.AddCell(cell);
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cell.AddElement(new Phrase(objQuestionTPCRA.DSInfectionControlSignature.SignByUserName + " (" + objQuestionTPCRA.DSInfectionControlSignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                    tableinfectionist.AddCell(cell);


//                }
//                else
//                {
//                    tableinfectionist.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
//                }
//                if (objQuestionTPCRA.DSSafetySignature != null && objQuestionTPCRA.DSSafetySignature.DigSignatureId > 0)
//                {
//                    cell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    cell.AddElement(new Phrase(objQuestionTPCRA.DSSafetySignature.SignByUserName + " (" + objQuestionTPCRA.DSSafetySignature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                    tablesafety.AddCell(cell);
//                }
//                else
//                {
//                    tablesafety.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false, 2));
//                }
//                pdfDoc.Add(table);
//            }



//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }


//        public static string PrintPCRAPdfInbytes(int? projectId, int? tPCRAQuesId, string mode, int? doctype)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TPCRAQuestion objQuestionTPCRA = new TPCRAQuestion();
//            objQuestionTPCRA = UnityContextFactory<IPCRAService>.CreateContext().GetQuestionTPCRA(projectId, tPCRAQuesId);
//            pdfDoc = CreatePrintPCRAPdf(projectId, tPCRAQuesId, mem, objQuestionTPCRA, doctype, true);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        public static Document CreatePrintPCRAPdf(int? projectId, int? tPCRAQuesId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? doctype, bool IsAttachmentIncluded)
//        {

//            System.Net.ServicePointManager.Expect100Continue = true;
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 30);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            PdfPTable table;
//            if (doctype.HasValue && doctype.Value == 1)
//                SetHeaderBlue(out table, "Construction Risk Assessment [CRA] ");
//            else
//                SetHeaderBlue(out table, "Pre-Construction Risk Assessment [PCRA] ");

//            pdfDoc.Add(table);

//            table = new PdfPTable(5)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,

//            };

//            float[] widths = new float[] { 17f, 30f, 6f, 17f, 30f };
//            table.SetTotalWidth(widths);
//            table.DefaultCell.Border = Rectangle.NO_BORDER;
//            TPCRAQuestion objFilteredQuestionTPCRA = new TPCRAQuestion();
//            objFilteredQuestionTPCRA = objQuestionTPCRA;
//            var data = objQuestionTPCRA.TPCRAQuestionDetails.Where(s => s.QuestionPCRA.IsActive == true).ToList();
//            objFilteredQuestionTPCRA.TPCRAQuestionDetails = new List<TPCRAQuestionDetails>();
//            objFilteredQuestionTPCRA.TPCRAQuestionDetails = data;

//            // table.DefaultCell.Border = Rectangle.NO_BORDER;
//            Chunk chunk1 = new Chunk();
//            var para = new Paragraph();
//            PdfPCell cell = new PdfPCell();
//            para.Alignment = Element.ALIGN_LEFT;
//            var projectData = new TIcraProject();
//            if (objQuestionTPCRA.ProjectId > 0)
//            {
//                projectData = UnityContextFactory<IPCRAService>.CreateContext().GetProjectDetails().FirstOrDefault(m => m.ProjectId == objQuestionTPCRA.ProjectId);

//                table.AddCell(AddNewCell("Project Name:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(projectData.ProjectName, CellFont, false));
//                table.AddCell("");
//                table.AddCell(AddNewCell("Project Number:", CellFontBoldBlack, false));
//                table.AddCell(AddNewCell(projectData.ProjectNumber, CellFont, false));

//                if (doctype.HasValue && doctype.Value == 1)
//                {
//                    TIcraLog objTIcraLog = getTicralog(objQuestionTPCRA.TicraId);
//                    if (objTIcraLog != null && objQuestionTPCRA.TicraId > 0)
//                    {
//                        table.AddCell(AddNewCell("Location of Construction:", CellFontBoldBlack, false));
//                        table.AddCell(AddNewCell(objTIcraLog.Location, CellFont, false));
//                        table.AddCell("");
//                        table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
//                        table.AddCell(AddNewCell(objQuestionTPCRA.ContractorId != null && objQuestionTPCRA.ContractorUser != null && objQuestionTPCRA.ContractorUser.Name != null ? objQuestionTPCRA.ContractorUser.Name : " ", CellFont, false));
//                        table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
//                        table.AddCell(AddNewCell((objTIcraLog.StartDate.HasValue ? objTIcraLog.StartDate.Value.ToFormatDate() : string.Empty), CellFont, false));
//                        table.AddCell("");
//                        table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
//                        table.AddCell(AddNewCell((objTIcraLog.CompletionDate.HasValue ? objTIcraLog.CompletionDate.Value.ToFormatDate() : string.Empty), CellFont, false));
//                        table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
//                        table.AddCell(AddNewCell(objQuestionTPCRA.WorkDescription, CellFont, false));
//                    }

//                }
//                else
//                {
//                    table.AddCell(AddNewCell("Project Location:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.ProjectLocation, CellFont, false));
//                    table.AddCell("");
//                    table.AddCell(AddNewCell("Architect:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.Architect, CellFont, false));
//                    table.AddCell(AddNewCell("Project Manager:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.ProjectManager, CellFont, false));
//                    table.AddCell("");
//                    table.AddCell(AddNewCell("Contractor:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.ProjectContractor, CellFont, false));
//                    table.AddCell(AddNewCell("Start Date:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.ProjectStartDate.ToFormatDate(), CellFont, false));
//                    table.AddCell("");
//                    table.AddCell(AddNewCell("End Date:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.ProjectEndDate.ToFormatDate(), CellFont, false));
//                    table.AddCell(AddNewCell("Description:", CellFontBoldBlack, false));
//                    table.AddCell(AddNewCell(projectData.Description, CellFont, false));
//                }


//                table.AddCell("");
//                table.AddCell("");
//                table.AddCell("");

//            }

//            pdfDoc.Add(table);
//            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

//            pdfDoc.Add(line1);
//            table = new PdfPTable(4)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,
//            };

//            widths = new float[] { 75f, 5f, 10f, 10f };
//            table.SetTotalWidth(widths);


//            PdfPTable table1 = new PdfPTable(5)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 0f,
//            };

//            widths = new float[] { 10f, 65f, 5f, 10f, 10f };
//            table1.SetTotalWidth(widths);
//            PdfPCell cellsubtable = new PdfPCell()
//            {
//                Border = 0,
//            };

//            Image ImgCheckBox = Image.GetInstance(Models.ImagePathModel.CheckBoxImage);
//            ImgCheckBox.ScaleAbsolute(10f, 10f);

//            Image ImgUnCheckBox = Image.GetInstance(Models.ImagePathModel.UnCheckBoxImage);
//            ImgUnCheckBox.ScaleAbsolute(10f, 10f);
//            int sectionlen = 0;
//            for (int i = 0; i < objQuestionTPCRA.TPCRAQuestionDetails.Count; i++)
//            {
//                sectionlen++;
//                table.AddCell(AddNewCell("Section " + sectionlen, CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell("YES", CellFontNormalBlueSmall, false));
//                table.AddCell(AddNewCell("NO", CellFontNormalBlueSmall, false));

//                //question starts
//                table.AddCell(AddNewCell(sectionlen + ". " + objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.Questions, CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));

//                PdfPCell nobordercell = new PdfPCell()
//                {
//                    Border = 0,
//                };

//                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuesStatus == 1)
//                {
//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };

//                    chunk1 = new Chunk(ImgCheckBox, 3, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                }
//                else if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuesStatus == 0)
//                {

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    chunk1 = new Chunk(ImgCheckBox, 1, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);


//                }
//                else
//                {

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);

//                    nobordercell = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
//                    nobordercell.AddElement(chunk1);
//                    table.AddCell(nobordercell);


//                }
//                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.IsOption == true)
//                {
//                    table1 = new PdfPTable(5)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 0f,
//                    };

//                    widths = new float[] { 3f, 72f, 5f, 10f, 10f };
//                    table1.SetTotalWidth(widths);
//                    cellsubtable = new PdfPCell(table1)
//                    {
//                        Border = 0,

//                    };
//                    cellsubtable.Colspan = 5;
//                    for (int j = 0; j < objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs.Select(s => s.Text).Count(); j++)
//                    {

//                        if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].IsReadOnly)
//                        {
//                            table1.AddCell(AddNewCell(" ", CellFont, false));
//                            table1.AddCell(AddNewCell(objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].Text, CellFont, true));
//                            table1.AddCell(AddNewCell(" ", CellFont, false));
//                            table1.AddCell(AddNewCell(" ", CellFont, false));
//                            table1.AddCell(AddNewCell(" ", CellFont, false));

//                        }
//                        else
//                        {
//                            table1.AddCell(AddNewCell(" ", CellFont, false));
//                            table1.AddCell(AddNewCell(objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].Text, CellFont, false));
//                            table1.AddCell(AddNewCell(" ", CellFont, false));
//                            if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.QuestionOptionPCRAs[j].OptionStatus)
//                            {
//                                nobordercell = new PdfPCell()
//                                {
//                                    Border = 0,
//                                };

//                                chunk1 = new Chunk(ImgCheckBox, 3, 0);
//                                nobordercell.AddElement(chunk1);
//                                table1.AddCell(nobordercell);

//                                nobordercell = new PdfPCell()
//                                {
//                                    Border = 0,
//                                };
//                                chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
//                                nobordercell.AddElement(chunk1);
//                                table1.AddCell(nobordercell);

//                            }

//                            else
//                            {

//                                nobordercell = new PdfPCell()
//                                {
//                                    Border = 0,
//                                };
//                                chunk1 = new Chunk(ImgUnCheckBox, 3, 0);
//                                nobordercell.AddElement(chunk1);
//                                table1.AddCell(nobordercell);

//                                nobordercell = new PdfPCell()
//                                {
//                                    Border = 0,
//                                };
//                                chunk1 = new Chunk(ImgUnCheckBox, 1, 0);
//                                nobordercell.AddElement(chunk1);
//                                table1.AddCell(nobordercell);


//                            }
//                        }



//                    }

//                    table.AddCell(cellsubtable);
//                }

//                if (objQuestionTPCRA.TPCRAQuestionDetails[i].QuestionPCRA.CanComment == true)
//                {
//                    table.AddCell(AddNewCell("Comment: " + objQuestionTPCRA.TPCRAQuestionDetails[i].Comment, CellFont, false));
//                    table.AddCell(AddNewCell(" ", CellFont, false));
//                    table.AddCell(AddNewCell(" ", CellFont, false));
//                    table.AddCell(AddNewCell(" ", CellFont, false));
//                }
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//            }
//            if (doctype.HasValue && doctype.Value == 1)
//            {
//            }
//            else
//            {
//                sectionlen = sectionlen + 1;
//                table.AddCell(AddNewCell("Section " + sectionlen + ":Approval", CellFontBoldBlueSmall, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//                table.AddCell(AddNewCell(" ", CellFont, false));
//            }
//            pdfDoc.Add(table);
//            if (doctype.HasValue && doctype.Value == 1)
//            {
//            }
//            else
//            {
//                //if(objQuestionTPCRA.Sign1User!=null)
//                //{ 
//                //}
//                //table = new PdfPTable(5)
//                //{
//                //    WidthPercentage = 100,
//                //    HorizontalAlignment = 0,
//                //    SpacingBefore = 10f,
//                //};

//                //widths = new float[] { 35f, 20f, 15f, 10f, 20f };

//                //table.AddCell(AddNewCell("Sign1 Name :", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell("Sign2 Name :", CellFontBoldBlueSmall, false));

//                //table.AddCell(AddNewCell((objQuestionTPCRA.Sign1User != null ? objQuestionTPCRA.Sign1User.Name : string.Empty), UnderlineCellFont, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell((objQuestionTPCRA.Sign2User != null ? objQuestionTPCRA.Sign2User.Name : string.Empty), UnderlineCellFont, false));

//                //table.AddCell(AddNewCell("Sign1 Date :", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell("Sign2 Date :", CellFontBoldBlueSmall, false));

//                //table.AddCell(AddNewCell(objQuestionTPCRA.Sign1Date.ToFormatDate(), UnderlineCellFont, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(objQuestionTPCRA.Sign2Date.ToFormatDate(), UnderlineCellFont, false));



//                //if (objQuestionTPCRA.DSSign1Signature != null && objQuestionTPCRA.DSSign1Signature.DigSignatureId > 0)
//                //{
//                //    cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    Image image = Image.GetInstance(Base.Common.FilePath(objQuestionTPCRA.DSSign1Signature.FilePath));
//                //    image.ScaleAbsolute(60, 60);
//                //    cell.AddElement(image);
//                //    table.AddCell(cell);
//                //    cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cell.AddElement(new Phrase(objQuestionTPCRA.DSSign1Signature.SignByUserName + " (" + objQuestionTPCRA.DSSign1Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                //    table.AddCell(cell);
//                //}
//                //else
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //}
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //if (objQuestionTPCRA.DSSign2Signature != null && objQuestionTPCRA.DSSign2Signature.DigSignatureId > 0)
//                //{
//                //    cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    Image image = Image.GetInstance(Base.Common.FilePath(objQuestionTPCRA.DSSign2Signature.FilePath));
//                //    image.ScaleAbsolute(60, 60);
//                //    cell.AddElement(image);
//                //    table.AddCell(cell);
//                //    cell = new PdfPCell()
//                //    {
//                //        Border = 0,
//                //    };
//                //    cell.AddElement(new Phrase(objQuestionTPCRA.DSSign2Signature.SignByUserName + " (" + objQuestionTPCRA.DSSign2Signature.LocalSignDateTime.ToString("MMM d, yyyy hh:mm tt") + ")", CellFont));
//                //    table.AddCell(cell);
//                //}
//                //else
//                //{
//                //    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                //}

//                //pdfDoc.Add(table);
//            }

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f
//            };
//            widths = new float[] { 8f, 92f };
//            table.SetWidths(widths);
//            PdfPCell cell1 = new PdfPCell();
//            table.AddCell(AddNewCell("Status:", CellFontBoldBlueSmall, false));
//            table.AddCell(AddNewCell(objQuestionTPCRA.ApprovalStatus == 3 ? "HOLD" : Enum.GetName(typeof(Enums.ApprovalStatus), objQuestionTPCRA.ApprovalStatus).ToString().ToUpper(), CellFont, false));
//            table.AddCell(AddNewCell("Reason:", CellFontBoldBlueSmall, false));
//            table.AddCell(AddNewCell(objQuestionTPCRA.RejectionReason, CellFont, false));
//            pdfDoc.Add(table);
//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f
//            };
//            int twocolcount = 0;
//            foreach (var permitworkflow in objQuestionTPCRA.TPermitWorkFlowDetails)
//            {
//                if (permitworkflow.LabelValue > 0 && permitworkflow.DSPermitSignature != null && permitworkflow.DSPermitSignature.DigSignatureId > 0)
//                {
//                    PdfPCell cellmain = new PdfPCell()
//                    {
//                        Border = 0,
//                    };
//                    cellmain = CreateSignSectionCell(permitworkflow.LabelText, permitworkflow.DSPermitSignature, 45f);
//                    table.AddCell(cellmain);

//                    twocolcount++;
//                }

//                if (twocolcount == 0)
//                {
//                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                    twocolcount++;
//                }

//                if (twocolcount % 2 == 0)
//                {
//                    table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//                    twocolcount++;
//                }

//            }

//            if (twocolcount % 2 != 0)
//            {
//                table.AddCell(AddNewCell(" ", CellFontBoldBlueSmall, false));
//            }

//            pdfDoc.Add(table);
//            bool isattach = false;
//            if (IsAttachmentIncluded)
//            {

//                TIcraLog objTIcra = new TIcraLog();
//                if (objQuestionTPCRA.DrawingAttachments.Count > 0 || objTIcra.TICRAFiles.Count > 0)
//                {
//                    pdfDoc.NewPage();
//                    isattach = true;
//                }
//                if (isattach)
//                {
//                    if (objQuestionTPCRA.DrawingAttachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 20f
//                        };

//                        table.AddCell(AddNewCell("Drawings Attached:", CellFontBoldBlack, false));
//                        foreach (var drawingsname in objQuestionTPCRA.DrawingAttachments)
//                        {
//                            PdfGenerator.AddAttachmentCell(drawingsname.ImagePath, drawingsname.FullFileName, pdfWriter, table);

//                        }


//                        pdfDoc.Add(table);
//                    }



//                    if (objTIcra.TICRAFiles.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0,
//                            SpacingBefore = 20f
//                        };

//                        if (objTIcra != null && objTIcra.TICRAFiles.Count > 0)
//                        {
//                            table.AddCell(AddNewCell("Attachment(s):", CellFontBoldBlack, false));
//                            foreach (var files in objTIcra.TICRAFiles)
//                            {
//                                PdfGenerator.AddAttachmentCell(files.FilePath, files.FileName, pdfWriter, table);
//                            }
//                        }
//                        pdfDoc.Add(table);
//                    }
//                }

//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }


//        #endregion

//        #region ICRA
//        public static Document CreateICRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? PermitType = 0)
//        {
//            TIcraLog objTIcraLog = getTicralog(icraId);
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 27);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            pdfWriter.PageEvent = new PDFFooter();
//            pdfDoc.Open();
//            if (PermitType == 1)
//            {

//                PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
//                pdfDoc.NewPage();
//            }
//            Paragraph para = new Paragraph("Infection Control Risk Assessment \n Matrix of Precautions for Construction & Renovation", TitleFontS)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);
//            Paragraph line = new Paragraph();
//            PdfPTable table = new PdfPTable(1);
//            PdfPCell cell = new PdfPCell();
//            int childicrasteps = 0;
//            for (int i = 0; i < objTIcraLog.TIcraSteps.Where(x => x.Step.ParentICRAStepId == 0).ToList().Count; i++)
//            {
//                childicrasteps = i + 1;
//                if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 1)
//                {
//                    float[] widths = new float[] { 40f, 60f };
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Type of Construction Project Activity (Type A-D)", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(2)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    table.SetWidths(widths);
//                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
//                    {
//                        string activity = objTIcraLog.ConstructionTypes[j].Description + "\n" + "Includes, but is not limited to: \n";
//                        foreach (var item in objTIcraLog.ConstructionTypes[j].ConstructionActivity)
//                        {
//                            activity = activity + "• " + item.Activity + "\n";
//                        }
//                        PdfPCell cell1 = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
//                        PdfPCell cell2 = new PdfPCell(new Phrase(activity, CellFontS));
//                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                        {
//                            cell1.BackgroundColor = Gray;
//                            cell2.BackgroundColor = Gray;
//                        }
//                        table.AddCell(cell1);
//                        table.AddCell(cell2);
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);
//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 2)
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Using the following table, identify the Patient Risk Groups that will be affected. If more than one risk group will be affected, select the higher risk group:", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(4)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    for (int k = 0; k < objTIcraLog.ConstructionRisks.Count; k++)
//                    {
//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[k].RiskName, CellFontS));
//                        if (objTIcraLog.ConstructionRisks[k].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//                        {
//                            cell.BackgroundColor = Gray;
//                        }
//                        table.AddCell(cell);
//                    }
//                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
//                    {
//                        para = new Paragraph();
//                        foreach (var item in objTIcraLog.ConstructionRisks[j].RiskArea)
//                        {
//                            para.Add(new Phrase("• " + item.Name + "\n", ParagraphFontS));
//                        }
//                        table.AddCell(para);
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);

//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 3)
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ":" + "\n" + "Patient Risk Group (Low, Medium, High, Highest) with the planned Construction Project Type (A, B, C, D) on the following matrix, to find the Class of Precautions (I, II, III or IV) or level of infection control activities required. Class I-IV or Color-Coded Precautions are delineated on the following page.\nIC Matrix - Class of Precautions: Construction Project by Patient Risk", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        //SpacingAfter = 30f
//                    };
//                    pdfDoc.Add(para);
//                    para = new Paragraph("\nConstruction Project Type ", ParagraphFontS)
//                    {
//                        Alignment = Element.ALIGN_CENTER,
//                        //SpacingBefore = 30f
//                    };
//                    pdfDoc.Add(para);
//                    table = new PdfPTable(5)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 20f
//                    };
//                    //table.SpacingAfter = 30f;
//                    table.AddCell(new Phrase("Patient Risk Group", CellFontS));
//                    int? ConstructionTypeId = 0;
//                    int? ConstructionRiskId = 0;
//                    for (int j = 0; j < objTIcraLog.ConstructionTypes.Count; j++)
//                    {

//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionTypes[j].TypeName, CellFontS));
//                        if (objTIcraLog.ConstructionTypes[j].ConstructionTypeId == objTIcraLog.ConstructionTypeId)
//                        {
//                            cell.BackgroundColor = Gray;
//                            ConstructionTypeId = objTIcraLog.ConstructionTypeId;
//                        }
//                        table.AddCell(cell);
//                    }
//                    for (int j = 0; j < objTIcraLog.ConstructionRisks.Count; j++)
//                    {
//                        cell = new PdfPCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
//                        if (objTIcraLog.ConstructionRisks[j].ConstructionRiskId == objTIcraLog.ConstructionRiskId)
//                        {
//                            cell.BackgroundColor = Gray;
//                            ConstructionRiskId = objTIcraLog.ConstructionRiskId;
//                        }

//                        table.AddCell(cell);
//                        //table.AddCell(new Phrase(objTIcraLog.ConstructionRisks[j].RiskName, CellFontS));
//                        for (int k = 0; k < objTIcraLog.ConstructionTypes.Count; k++)
//                        {
//                            var str = string.Empty;
//                            var className = string.Empty;
//                            var ClassId = string.Empty;

//                            var data = objTIcraLog.ICRAMatixPrecautions.Where(x => x.ConstructionRiskId == objTIcraLog.ConstructionRisks[j].ConstructionRiskId && x.ConstructionTypeId == objTIcraLog.ConstructionTypes[k].ConstructionTypeId).ToList();
//                            if (data.Count > 0)
//                            {
//                                str = string.Join(",", data.Select(x => x.ConstructionClass.ClassName));
//                                className = data.FirstOrDefault().ConstructionClass.ClassName.Replace(" ", "").ToLower();

//                            }

//                            cell = new PdfPCell(new Phrase(str, CellFontS));

//                            if (objTIcraLog.ConstructionTypes[k].ConstructionTypeId == ConstructionTypeId && objTIcraLog.ConstructionRisks[j].ConstructionRiskId == ConstructionRiskId)
//                            {
//                                cell.BackgroundColor = BaseColor.BLUE;
//                            }
//                            table.AddCell(cell);
//                        }
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Note: Infection Control approval will be required when the Construction Activity and Risk Level indicate that Class IIIor Class IVcontrol procedures are necessary.", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);

//                }
//                else if (objTIcraLog.TIcraSteps[i].Step.ICRAStepId == 4)
//                {
//                    para = new Paragraph("Step 4:Identify the areas surrounding the project area, assessing potential impact # " + objTIcraLog.ProjectNo, ParagraphFontS);
//                    pdfDoc.Add(para);

//                    table = new PdfPTable(6)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 10f
//                    };
//                    //table.SpacingAfter = 30f;
//                    string risklist = "";
//                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
//                    {
//                        string riskarea = !string.IsNullOrEmpty(objTIcraLog.AreasSurroundings[k].RiskAreaIdNames) ? objTIcraLog.AreasSurroundings[k].RiskAreaIdNames + "," : string.Empty;
//                        risklist = risklist + riskarea;
//                        table.AddCell(new Phrase(objTIcraLog.AreasSurroundings[k].AreasSurrounding.ToString() + ": " + risklist, CellFontS));
//                    }
//                    for (int k = 0; k < objTIcraLog.AreasSurroundings.Count; k++)
//                    {
//                        string riskName = objTIcraLog.AreasSurroundings[k].ConstructionRisk != null ? objTIcraLog.AreasSurroundings[k].ConstructionRisk.RiskName : string.Empty;
//                        table.AddCell(new Phrase("Risk Group : " + riskName, CellFontS));
//                    }
//                    pdfDoc.Add(table);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment + "\n", ParagraphFontS)
//                    {
//                        SpacingBefore = 5f,
//                        SpacingAfter = 5f
//                    };
//                    pdfDoc.Add(para);
//                }
//                else
//                {
//                    para = new Paragraph(objTIcraLog.TIcraSteps[i].Step.Alias + ": " + "\n", ParagraphFontS);
//                    pdfDoc.Add(para);

//                    para = new Paragraph(Convert.ToChar(65) + ": " + objTIcraLog.TIcraSteps[i].Step.Steps + "\n", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[i].Comment, ParagraphFontS)
//                    {
//                        SpacingBefore = 5f
//                    };
//                    pdfDoc.Add(para);
//                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
//                    pdfDoc.Add(line);
//                }
//                for (int j = 0; j < objTIcraLog.TIcraSteps.Where(x => x.Step.ParentICRAStepId == objTIcraLog.TIcraSteps[i].ICRAStepId).ToList().Count; j++)
//                {
//                    para = new Paragraph($"{Convert.ToChar(j + 66)}" + ": " + objTIcraLog.TIcraSteps[childicrasteps].Step.Steps + "\n", ParagraphFontS);
//                    pdfDoc.Add(para);
//                    para = new Paragraph("Comment: " + objTIcraLog.TIcraSteps[childicrasteps].Comment, ParagraphFontS)
//                    {
//                        SpacingBefore = 5f
//                    };
//                    pdfDoc.Add(para);
//                    line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
//                    pdfDoc.Add(line);
//                    childicrasteps++;
//                }
//            }


//            if (PermitType != 1)
//            {
//                pdfDoc.NewPage();
//                PrintPermit(objTIcraLog, pdfDoc, objQuestionTPCRA);
//            }
//            bool isattachemnets = false;
//            if (PermitType == 1)
//            {
//                if (objTIcraLog.TICRAFiles.Count > 0 || objQuestionTPCRA.DrawingAttachments.Count > 0)
//                {
//                    pdfDoc.NewPage();
//                    isattachemnets = true;
//                }
//            }
//            else
//            {
//                if (objTIcraLog.TICRAFiles.Count > 0 || objTIcraLog.DrawingAttachments.Count > 0)
//                {
//                    pdfDoc.NewPage();
//                    isattachemnets = true;
//                }
//            }
//            if (isattachemnets)
//            {
//                if (objTIcraLog.TICRAFiles.Count > 0)
//                {
//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0,
//                        SpacingBefore = 10f
//                    };
//                    table = new PdfPTable(1)
//                    {
//                        WidthPercentage = 100,
//                        HorizontalAlignment = 0
//                    };
//                    table.AddCell(AddNewCell("Attachment(s):", ParagraphFontS));
//                    for (int fileid = 0; fileid < objTIcraLog.TICRAFiles.Count(); fileid++)
//                    {
//                        //table.AddCell(AddNewCell(objQuestionTPCRA.DrawingAttachments[k].FullFileName, ParagraphFontS));
//                        //table.AddCell(AddNewCell("", ParagraphFontS));
//                        PdfGenerator.AddAttachmentCell(objTIcraLog.TICRAFiles[fileid].FilePath, objTIcraLog.TICRAFiles[fileid].FileName, pdfWriter, table);
//                    }
//                    pdfDoc.Add(table);
//                }

//                if (PermitType == 1)
//                {
//                    if (objQuestionTPCRA.DrawingAttachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0
//                        };
//                        table.AddCell(AddNewCell("Drawings Attached:", ParagraphFontS));
//                        for (int k = 0; k < objQuestionTPCRA.DrawingAttachments.Count; k++)
//                        {
//                            //table.AddCell(AddNewCell(objQuestionTPCRA.DrawingAttachments[k].FullFileName, ParagraphFontS));
//                            //table.AddCell(AddNewCell("", ParagraphFontS));
//                            PdfGenerator.AddAttachmentCell(objQuestionTPCRA.DrawingAttachments[k].ImagePath, objQuestionTPCRA.DrawingAttachments[k].FullFileName, pdfWriter, table);
//                        }
//                        pdfDoc.Add(table);
//                    }
//                }
//                else
//                {
//                    if (objTIcraLog.DrawingAttachments.Count > 0)
//                    {
//                        table = new PdfPTable(1)
//                        {
//                            WidthPercentage = 100,
//                            HorizontalAlignment = 0
//                        };
//                        table.AddCell(AddNewCell("Drawings Attached:", ParagraphFontS));
//                        for (int k = 0; k < objTIcraLog.DrawingAttachments.Count; k++)
//                        {
//                            //table.AddCell(AddNewCell(objTIcraLog.DrawingAttachments[k].FullFileName, ParagraphFontS));
//                            //table.AddCell(AddNewCell("", ParagraphFontS));

//                            PdfGenerator.AddAttachmentCell(objTIcraLog.DrawingAttachments[k].ImagePath, objTIcraLog.DrawingAttachments[k].FullFileName, pdfWriter, table);
//                        }
//                        pdfDoc.Add(table);
//                    }

//                }
//            }
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }
//        public static TIcraLog getTicralog(int TicraId)
//        {
//            TIcraLog objTIcraLog = new TIcraLog();
//            if (TicraId > 0)
//            {
//                objTIcraLog = UnityContextFactory<IConstructionService>.CreateContext().GetInspectionIcraSteps(TicraId);
//                if (objTIcraLog != null)
//                {
//                    objTIcraLog.ConstructionTypes = UnityContextFactory<IConstructionService>.CreateContext().GetConstructionType().Where(x => x.IsActive).ToList();
//                    objTIcraLog.ConstructionRisks = UnityContextFactory<IConstructionService>.CreateContext().GetConstructionRisk().Where(x => x.IsActive).ToList();
//                    objTIcraLog.ConstructionClasses = UnityContextFactory<IConstructionService>.CreateContext().GetConstructionClass().Where(x => x.IsActive).ToList();
//                    objTIcraLog.ICRAMatixPrecautions = UnityContextFactory<IConstructionService>.CreateContext().GetICRAMatixPrecautions().Where(x => x.IsActive).ToList();


//                    if (objTIcraLog.AreasSurroundings.Count == 0)
//                    {
//                        objTIcraLog.AreasSurroundings = new List<TICRAAreasNearBy>();
//                        foreach (Enums.AreasSurrounding item in Enum.GetValues(typeof(Enums.AreasSurrounding)))
//                        {
//                            TICRAAreasNearBy newTICRAAreasNearBy = new TICRAAreasNearBy
//                            {
//                                AreasSurrounding = item,
//                                AreasSurroundingId = (int)item
//                            };
//                            objTIcraLog.AreasSurroundings.Add(newTICRAAreasNearBy);
//                        }
//                    }
//                }
//            }

//            return objTIcraLog;
//        }
//        #endregion

//        #region Tilsm Reports

//        [HttpGet]
//        public static string CreateTilsmReportsbytes(int tilsmId)
//        {
//            var mem = new MemoryStream();
//            Document pdfDoc = new Document();
//            TIlsm objTilsm = UnityContextFactory<IIlsmService>.CreateContext().GetIlsmInspection(tilsmId);
//            pdfDoc = CreateTilsmReport(tilsmId, mem, objTilsm);
//            var pdf = mem.ToArray();
//            return Convert.ToBase64String(pdf);
//        }

//        private static Document CreateTilsmReport(int tilsmId, Stream streamOutput, TIlsm objTilsm)
//        {
//            Document pdfDoc = new Document(SetPaperType(), 25, 25, 25, 25);
//            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, streamOutput);
//            string footerTitle = $"{"Incident #"}: {objTilsm.IncidentId}";
//            pdfWriter.PageEvent = new PDFFooter(footerTitle);
//            pdfDoc.Open();
//            //Table
//            PdfPTable table;
//            SetHeader(out table);
//            pdfDoc.Add(table);
//            Paragraph para = new Paragraph("ILSM Report", TitleFont)
//            {
//                Alignment = Element.ALIGN_CENTER
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f,
//            };
//            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//            PdfPCell cell3 = new PdfPCell
//            {
//                Border = 0,
//                Colspan = 2,
//            };
//            cell3.AddElement(line);
//            table.AddCell(cell3);

//            table = new PdfPTable(2)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 10f
//            };
//            table.DefaultCell.Border = Rectangle.NO_BORDER;

//            PdfPCell cellt = new PdfPCell(new Phrase(Resources.Resource.IncidentNo + ": " + objTilsm.IncidentId, CellFontBold))
//            {

//            };
//            table.AddCell(cellt);
//            cellt = new PdfPCell(new Phrase("ILSM Date" + ": " + string.Format("{0} {1}", (objTilsm.llsmDate != null ? $"{objTilsm.llsmDate.ToClientTime().ToFormatDate()}" : "NA"), (objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : ""), CellFontBold)))
//            {
//            };
//            table.AddCell(cellt);

//            //table.AddCell(new Phrase(Resources.Resource.IncidentNo + ": " + objTilsm.IncidentId, CellFontBold));
//            //table.AddCell(new Phrase(Resources.Resource.CreatedDate + ": " + objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

//            pdfDoc.Add(table);

//            float[] widths = new float[] { 30f, 70f };
//            table = new PdfPTable(2);
//            table.DefaultCell.Border = Rectangle.NO_BORDER;
//            table.WidthPercentage = 100;
//            table.HorizontalAlignment = 0;
//            table.SpacingBefore = 10f;
//            table.SpacingAfter = 10f;
//            table.SetWidths(widths);

//            table.AddCell(new Phrase(Resources.Resource.Name, CellFont));
//            table.AddCell(new Phrase(objTilsm.Notes, CellFont));

//            table.AddCell(new Phrase(Resources.Resource.StandardEP, CellFont));
//            if (objTilsm.TicraId.HasValue)
//                table.AddCell(new Phrase("Construction ILSM ", CellFont));
//            else if (objTilsm.SourceInspection.EPDetails != null)
//                table.AddCell(new Phrase(objTilsm.SourceInspection.EPDetails.StandardEp, CellFont));
//            else
//                table.AddCell(new Phrase("Manual ILSM ", CellFont));

//            //table.AddCell(new Phrase(Resources.Resource.Location, CellFont));

//            //table.AddCell(new Phrase(objTilsm.Floor != null ? $"{objTilsm.Floor.Building.BuildingName},{objTilsm.Floor.FloorName}"
//            //    : "NA", CellFont));
//            table.AddCell(new Phrase("User", CellFont));
//            table.AddCell(new Phrase(objTilsm.Inspector != null ? $"{objTilsm.Inspector.FullName}" : "NA", CellFont));
//            //table.AddCell(new Phrase("Date", CellFont));
//            //table.AddCell(new Phrase(objTilsm.llsmDate != null ? $"{objTilsm.llsmDate.ToFormatDate()}": "NA", CellFont));
//            //table.AddCell(new Phrase("Time", CellFont));
//            //table.AddCell(new Phrase(objTilsm.llsmStartTime != null ? $"{objTilsm.llsmStartTime}" : "NA", CellFont));

//            table.AddCell(new Phrase(Resources.Resource.Status, CellFont));
//            table.AddCell(new Phrase(Base.Common.GetIlsmStatus((int)objTilsm.Status), CellFont));

//            //table.AddCell(new Phrase(Resources.Resource.ILSMDate, CellFont));
//            //table.AddCell(new Phrase(objTilsm.CreatedDate.ToClientTime().ToFormatDate(), CellFont));

//            table.AddCell(new Phrase("Completed Date", CellFont));
//            table.AddCell(new Phrase((int)objTilsm.Status == 1 ? objTilsm.CompletedDate.HasValue ? objTilsm.CompletedDate.Value.ToClientTime().ToFormatDate() : "NA" : "NA", CellFont));

//            pdfDoc.Add(table);

//            para = new Paragraph("Location(s):", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };

//            pdfDoc.Add(para);

//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 5f,
//                SpacingAfter = 10f
//            };
//            table.DefaultCell.Border = Rectangle.NO_BORDER;
//            if (objTilsm.Buildings.Count > 0)
//            {
//                foreach (var building in objTilsm.Buildings)
//                {
//                    table.AddCell(new Phrase($"{building.BuildingName}", CellFont));
//                    foreach (var floor in building.Floor)
//                    {
//                        table.AddCell(new Phrase($"    {floor.FloorName}", CellFont));
//                    }
//                }
//            }

//            pdfDoc.Add(table);

//            para = new Paragraph("Comments:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(1)
//            {
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 5f,
//                SpacingAfter = 10f
//            };
//            if (objTilsm.IlsmComments.Count > 0)
//            {
//                foreach (var cmt in objTilsm.IlsmComments)
//                {
//                    table.AddCell(new Phrase(
//                        $"{cmt.Comment} ({cmt.AddedByUserProfile.FullName} {"on "} {cmt.AddedDate.CastLocaleDate()})", CellFont));

//                }
//            }
//            //else
//            //{
//            //    PdfPCell cell = new PdfPCell(new Phrase("N/A", CellFont))
//            //    {
//            //        Border = 0,
//            //        Colspan = 2,
//            //    };
//            //    table.AddCell(cell);
//            //}

//            pdfDoc.Add(table);

//            para = new Paragraph("Deficiencies:", CellFont)
//            {
//                Alignment = Element.ALIGN_LEFT
//            };
//            pdfDoc.Add(para);

//            table = new PdfPTable(1)
//            {
//                //table.DefaultCell.Border = Rectangle.NO_BORDER;
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                SpacingBefore = 5f,
//                SpacingAfter = 10f
//            };

//            if (objTilsm.Deficiencies.Count > 0)
//            {
//                foreach (var def in objTilsm.Deficiencies.Select(x => x.Steps).ToList())
//                {
//                    foreach (var step in def)
//                    {
//                        table.AddCell(new Phrase($"{step.Step}", CellFont));
//                    }
//                }
//            }
//            else
//            {
//                PdfPCell cell = new PdfPCell(new Phrase("N/A", CellFont))
//                {
//                    Border = 0,
//                    Colspan = 2,
//                };
//                table.AddCell(cell);
//            }
//            pdfDoc.Add(table);

//            widths = new float[] { 70f, 30f };
//            table = new PdfPTable(2)
//            {
//                //table.DefaultCell.Border = Rectangle.NO_BORDER;
//                WidthPercentage = 100,
//                HorizontalAlignment = 0,
//                //SpacingBefore = 10f,
//                SpacingAfter = 10f
//            };
//            table.SetWidths(widths);

//            List<int> epsID = new List<int>();
//            foreach (var item in objTilsm.MainGoal.Where(x => x.EPDetailId.HasValue).OrderBy(x => x.Goal).ToList())
//            {
//                if (!epsID.Contains(item.EPDetailId.Value))
//                {
//                    epsID.Add(item.EPDetailId.Value);
//                    var standardEP = objTilsm.TriggerEps.Where(x => x.EPDetailId == item.EPDetailId).FirstOrDefault().StandardEP;
//                    PdfPCell cell = new PdfPCell(new Phrase(standardEP, CellFont))
//                    {
//                        Border = 0,
//                        Colspan = 2,
//                    };
//                    table.AddCell(cell);
//                }
//                PdfPCell cell1 = new PdfPCell(new Phrase(item.Goal, CellFontBold))
//                {
//                    Border = 0,
//                    Colspan = 2,
//                };
//                table.AddCell(cell1);
//                foreach (var step in item.Steps)
//                {
//                    if (step.Status != -3)
//                    {
//                        table.AddCell(new Phrase(step.Step, CellFont));
//                        table.AddCell(new Phrase(@Enum.GetName(typeof(Enums.InspectionStatus), step.Status), CellFont));
//                        foreach (var cmt in step.StepsComments)
//                        {
//                            PdfPCell cell = new PdfPCell(new Phrase(
//                                $"{cmt.Comment} ({cmt.AddedByUserProfile.FullName} {"on "} {cmt.AddedDate.CastLocaleDate()})", CellFont))
//                            {
//                                Border = 0,
//                                Colspan = 2,
//                            };
//                            table.AddCell(cell);
//                        }
//                    }
//                    else
//                    {
//                        table.AddCell(new Phrase());
//                        table.AddCell(new Phrase(@Enum.GetName(typeof(Enums.InspectionStatus), step.Status), CellFont));
//                    }
//                }
//                line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.DottedLineSeparator()));
//                cell3 = new PdfPCell
//                {
//                    Border = 0,
//                    Colspan = 2,
//                };
//                cell3.Border = Rectangle.TOP_BORDER;
//                cell3.AddElement(line);

//                table.AddCell(cell3);
//            }
//            pdfDoc.Add(table);

//            if (objTilsm.TicraId.HasValue)
//            {

//            }
//            else if (objTilsm.SourceInspection.TInspectionActivity.Count > 0)
//            {
//                para = new Paragraph("Assets Information", TitleFont)
//                {
//                    Alignment = Element.ALIGN_CENTER
//                };
//                pdfDoc.Add(para);

//                table = new PdfPTable(3)
//                {
//                    WidthPercentage = 100,
//                    HorizontalAlignment = 0,
//                    SpacingBefore = 20f,
//                    SpacingAfter = 10f
//                };
//                foreach (var item in objTilsm.SourceInspection.TInspectionActivity)
//                {
//                    table.AddCell(new Phrase(item.TFloorAssets.AssetNo, CellFont));
//                    table.AddCell(new Phrase(item.TFloorAssets.Name, CellFont));
//                    table.AddCell(new Phrase(@Enum.GetName(typeof(Enums.InspectionStatus), item.Status), CellFont));
//                }
//                pdfDoc.Add(table);
//            }
//            //pdfWriter.PageEvent = new PDFFooter();
//            pdfWriter.CloseStream = false;
//            pdfDoc.Close();
//            return pdfDoc;
//        }

//        #endregion

//    }
//}