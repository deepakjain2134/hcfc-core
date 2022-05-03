using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ITransaction
    {
        List<EPDetails> GetDeficientEPs(int userId);
        List<InspectionEPDocs> GetInspectionDocs(Guid activityId);
        int AddInspectionDetails(TInspectionDetail inspectiondetails, string steps = "");
        int AddInspectionDocs(InspectionEPDocs inspectionDocs);
        int AddInspectionFiles(TInspectionFiles inspectionfiles);
        int AddTinspectionCampus(Buildings campus, int Inspectionid, int Epdetailid);
        bool DeleteEpDocs(Guid activityId);
        bool DeleteTInspectionFiles(int tinsFileId);
        List<InspectionEPDocs> GetDocumentHistory(int fileId);
        List<InspectionEPDocs> GetDocumentReport(int userId);
        List<InspectionEPDocs> GetEpDocByBinder(int binderId);
        List<InspectionEPDocs> GetEpInspectionDoc(int epDetIlId);
        List<TInspectionFiles> GetIlsmInspectionFiles(Guid activityId);
        List<TInspectionFiles> GetInspectionAssetFiles(int floorAssetId, int inspectionId);
        List<EPsDocument> GetInspectionDocuments(int userId);
        List<InspectionEPDocs> GetInspectionEPDocs();
        List<InspectionEPDocs> GetInspectionEPDocs(int inspectionId, int epDetailId);
        List<TInspectionFiles> GetInspectionEpFiles(int inspectionId);       
        List<TInspectionFiles> GetInspectionFiles(Guid activityId);
        List<DocumentType> GetPolicyBinders(int userId, int selectedUser, int? epdetailId);
        List<InspectionEPDocs> GetRelationEpInspectionDoc(int epDetIlId);
        List<Steps> GetStepsTransactionalRecords(int mode, int ePdetailId, int? floorAssetId, int? mainGoalId, int? inspectionDetailId, int? frequencyId);
        List<Steps> GetStepsTransactionalRecords(int? inspectionDetailId);
        List<InspectionEPDocs> GetUserInsDoc(int userId);
        int InsertUpdateEpConfig(EpConfig epconfig, int mode, string dates);
        DocumentType PolicyBinderHistory(int docTypeId);
        bool UpdateInspectionSubstatus(int inspectionId, Guid activityId, string subStatus);

        //List<TInspectionFiles> GetInspectionFiles(int inspectionId);
        //List<InspectionEPDocs> GetInspectionDocs(int inspectionId);
        //List<EpConfig> GetAllEpConfig();
        //List<EPSteps> GetAllEpDocs(int userId, int epDetailId);
        //EPDetails GetCurrentEp(int epId);
        //List<TInspectionFiles> GetInspectionFiles();
    }
}