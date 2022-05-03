using HCF.BDO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public interface IPdfService
    {
        Document CreateAssetsInspectionReport(int userId, int? selEpdetailId, Stream streamOutput, ref EPDetails epDetail);
        string GenerateFireDrillReportInbytes(int TExerciseId);
        string PrintAssetsReportsInbytes(int userId, int epdetailId, ref EPDetails epdetails);
        //string PrintFiredrillMatrixInbytes(int buildingTypeId, int quarterNo, int year);
        void SetHeader(out PdfPTable table);
        void SetHeaderBlue(out PdfPTable table, string Heading);
        Rectangle SetLandScapePaperType();
        Rectangle SetPaperType();
        void SetStatusHeaderBlue(out PdfPTable table, string Heading, string Status, string ApproveBy, string Date);
        PdfPCell AddNewCell(string text, Font fontstyle, bool alignmentrighttype = false, int? Colspan = 0, int? Border = 0, int? cellBackColor = 0, bool paddingoff = false);
        PdfPCell CreateSignSectionCell(string Title, DigitalSignature digitalsign, string comment, float? linewidth = 45.0F);
        PdfPTable AddAttachmentCell(string filepath, string Filename, PdfWriter pdfWriter, PdfPTable table);
        #region Ceiling Permit

        Document CreateCeilingPermit(int ceilingPermitId, Stream streamOutput, CeilingPermit objceilingPermit, bool IsAttachmentIncluded);
        string CeilingPermitInbytes(int? ceilingPermitId, bool IsAttachmentIncluded);

        #endregion

        #region LifeSafety
        string LifeSafetyFormInbytes(string formId);
        Document CreateLifeSafetyPermit(string formId, Stream streamOutput, TLifeSafetyDevicesForms newForm, bool IsAttachmentIncluded);
        #endregion

        #region Fire System Bypass Permit
        string FireSystemByPassPermitInbytes(int tfsbPermitId);
        Document CreateFireSystemByPassPermit(int tfsbPermitId, Stream streamOutput, TFireSystemByPassPermit objTFireSystemByPassPermit, bool IsAttachmentIncluded);
        #endregion

        #region Method of procedure
        string MOPPermitInbytes(int mopid);
        Document CreateMOPPermit(int id, Stream streamOutput, TMOP mop, bool IsAttachmentIncluded);
        #endregion

        #region HWP
        string HWPPermitInbytes(int? thotworkpermitid);
        Document CreateHWPPermit(int id, Stream streamOutput, THotWorkPermits objhotWorkPermits, bool IsAttachmentIncluded);
        #endregion

        #region FMC
        string FMCPermitInbytes(int? id);
        Document CreateFMCPermit(int id, Stream streamOutput, TFacilitiesMaintenanceOccurrencePermit objFMC);

        #endregion

        #region PCRA
        void PrintPermit(TIcraLog objTIcraLog, Document pdfDoc, TPCRAQuestion objQuestionTPCRA);
        string PCRAPermitWorksheetBytes(int projectId, int tpcraquestid, int icraId, string PDFName);
        string PrintPCRAPdfInbytes(int? projectId, int? tPCRAQuesId, string mode, int? doctype);
        TIcraLog getTicralog(int TicraId);
        Document CreatePrintPCRAPdf(int? projectId, int? tPCRAQuesId, Stream streamOutput, TPCRAQuestion objQuestionTPCRA, int? doctype, bool IsAttachmentIncluded);

        #endregion

        #region Tilsm Reports


        string CreateTilsmReportsbytes(int tilsmId);
        #endregion

        #region Observation Reports 

        public string PrintProjectObserverReportInbytes(string projectIds, string reportId, ref TICRAReports ObservtionReport);
        #endregion
        #region Round
        Document RoundInspection(int floorId, string roundid, Stream streamOutput, int? groupround = 0);
        #endregion

        #region AssetDeviceChange
        string AssetDeviceChangeFormInbytes(string formId);
        Document CreateAssetDeviceChangePermit(string formId, Stream streamOutput, TAssetDeviceChangeForms newForm, bool IsAttachmentIncluded);
        #endregion
    }
}
