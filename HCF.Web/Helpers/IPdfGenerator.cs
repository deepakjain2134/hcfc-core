//using HCF.BAL;
//using HCF.BAL.Ioc;
//using HCF.BDO;
//using HCF.Utility;
//using HCF.Web.Base;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace HCF.Web.Helpers
//{
//    public interface IPdfGenerator
//    {
//        PdfPTable AddAttachmentCell(string filepath, string Filename, PdfWriter pdfWriter, PdfPTable table);
//        PdfPCell AddNewCell(string text, Font fontstyle, bool alignmentrighttype = false, int? Colspan = 0, int? Border = 0, int? cellBackColor = 0, bool paddingoff = false);
//        string CeilingPermitInbytes(int? ceilingPermitId, bool IsAttachmentIncluded);
//        Document CreateCeilingPermit(int ceilingPermitId, Stream streamOutput, CeilingPermit objceilingPermit, bool IsAttachmentIncluded);
//        Document CreateFireSystemByPassPermit(int tfsbPermitId, Stream streamOutput, TFireSystemByPassPermit objTFireSystemByPassPermit, bool IsAttachmentIncluded);
//        Document CreateFMCPermit(int id, Stream streamOutput, TFacilitiesMaintenanceOccurrencePermit objFMC);
//        Document CreateHWPPermit(int id, Stream streamOutput, THotWorkPermits objhotWorkPermits, bool IsAttachmentIncluded);
//        Document CreateICRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? PermitType = 0);
//        Document CreateLifeSafetyPermit(string formId, Stream streamOutput, TLifeSafetyDevicesForms newForm, bool IsAttachmentIncluded);
//        Document CreateMOPPermit(int id, Stream streamOutput, TMOP mop, bool IsAttachmentIncluded);
//        Document CreatePCRAPermit(int icraId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int PermitType);
//        Document CreatePrintPCRAPdf(int? projectId, int? tPCRAQuesId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? doctype, bool IsAttachmentIncluded);
//        PdfPCell CreateSignSectionCell(string Title, DigitalSignature digitalsign, float? linewidth = 45);
//        string CreateTilsmReportsbytes(int tilsmId);
//        string FireSystemByPassPermitInbytes(int tfsbPermitId);
//        string FMCPermitInbytes(int? id);
//        TIcraLog getTicralog(int TicraId);
//        string HWPPermitInbytes(int? thotworkpermitid);
//        string LifeSafetyFormInbytes(string formId);
//        string MOPPermitInbytes(int mopid);
//        string PCRAPermitWorksheetBytes(int projectId, int tpcraquestid, int icraId, string PDFName);
//        string PrintPCRAPdfInbytes(int? projectId, int? tPCRAQuesId, string mode, int? doctype);
//        void PrintPermit(TIcraLog objTIcraLog, Document pdfDoc, TPCRAQuestion objQuestionTPCRA);
//    }
//}