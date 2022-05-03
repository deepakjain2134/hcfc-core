using System;
using System.Collections.Generic;
using System.Data;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ITransactionService
    {
        int AddInspection(Inspection inspection);

        int AddPreviousInspection(Inspection inspection);
        int AddInspectionDetails(TInspectionDetail inspectionDetails);
        int AddInspectionDetails(TInspectionDetail inspectionDetails, string steps);
        int AddInspectionDocs(InspectionEPDocs inspectionDocs);
        int AddInspectionFiles(TInspectionFiles inspectionfiles);
        int AddInspectionSteps(InspectionSteps inspectionSteps);
        int AddTInspectionActivity(TInspectionActivity activity);
        int AddTInspectionCampus(Buildings Campus, int InspectionId, int Epdetailid);
        bool DeleteEpDocs(Guid activityId);
        bool DeleteTInspectionFiles(int tinsFileId);
        Inspection GetActivityHistory(Guid activityId);
        List<EPSteps> GetAllEpDocs(int userId);
        List<EPSteps> GetAllEpDocs(int userId, int epDetailId);
        List<Inspection> GetAllInspections();
        List<Inspection> GetComplianceReport();
        List<TInspectionActivity> GetComplianceReports(string assetids, string buildingId, string floorId, string fromdate, string todate, string status = null);
        EPDetails GetCurrentEp(int epId);
        List<Inspection> GetDeficiencies(string activityId);
        EPDetails GetDocsbyEp(int userId, int epdetailId);
        List<InspectionEPDocs> GetDocumentHistory(int fileId);
        List<InspectionEPDocs> GetDocumentReport(int userId);
        List<InspectionEPDocs> GetEpDocByBinder(int binderId);
        EPDetails GetEpInspectionActivity(int epDetailId, int inspectionId);
        List<InspectionEPDocs> GetEpInspectionDoc(int epDetIlId);
        EPDetails GetEpInspectionHistory(int epDetailId);
        IEnumerable<Inspection> GetEpsInspections(int epDetailId);
        IEnumerable<Inspection> GetEpsCampusInspections(int epDetailId); 
        List<EPSteps> GetFloorAssetsActivity(int epDetailId, int inspectionId, Guid activityId, int floorAssetId);
        List<EPSteps> GetGoalStepsByActivity(int? floorAssetId, int? assetsId, int epDetailId, int inspectionId, int mode, int frequencyId, int? inspectionGroupId = null);
        List<Inspection> GetInspection(string subStatus);
        IEnumerable<InspectionEPDocs> GetInspectionDocs(int inspectionId);
        List<EPsDocument> GetInspectionDocuments(int userId);
        EPDetails GetInspectionHistory(int userId, int epDetailId, int inspectionId);
        List<Inspection> GetInspectionsforWorkOrder(string activityId);
        List<DocumentType> GetPolicyBinders(int userId, int selectedUser, int? epDetailId);
        //List<EPDetails> GetRecentActivity(int userId);
        List<InspectionEPDocs> GetRelationEpInspectionDoc(int epDetIlId);
        TInspectionActivity GetTInspectionActivity(Guid activityId);
        int InsertUpdateEpConfig(EpConfig epConfig, int mode, string dates);
        DocumentType PolicyBinderHistory(int userId);
        List<InspectionEPDocs> RepositoryDashboard(int userId);
        bool UpdateInspectionSubstatus(int inspectionId, Guid activityId, string subStatus);

        bool MarkInsdocStatus(int inspectionId, int isComplete);
        DataTable GetIlsmInspections(Guid? activityId);
        bool MarkInspectionRa();
        List<EPsDocument> GetEpsDocument(int epId);
        int Save(InspectionGroup objInspectionGroup);
        List<InspectionGroup> GetInspectionGroup();
        bool UpdateInspectionGroup(InspectionGroup objInspectionGroup);
        List<Inspection> GetInspectionsForCalendar(int userId, DateTime? startDate, DateTime? endDate);
        List<TInspectionActivity> GetInspections(int floorAssetId, int epId);
        calenderViewDashBoard GetDashboardCalendar(int userId);
        List<EPDetails> GetArchivedEPsInspection(int userId);
        void SendDueDateEmail();
        EPDetails GetEpByInspectionId(int epId, int Inspectionid);

        int UndoInspection(Inspection inspection);
    }
}