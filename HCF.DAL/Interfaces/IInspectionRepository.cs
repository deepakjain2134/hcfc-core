using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.DAL
{
    public interface IInspectionRepository
    {
        //int AddPreviousInspection(Inspection inspection);
        List<Inspection> GetInspection(string subStatus);
        List<Inspection> GetInspections();
        List<Inspection> GetEpsInspections(int epDetailId);
        
        //Inspection GetEpInspection(int epDetailId);
        Inspection GetCurrentInspection(int epDetailId);
        List<Inspection> GetCurrentInspections();
        List<Inspection> GetEPCurrentInspection(int epDetailId);

        //Inspection EpStatus(int epDetailId);
        string EpTransStatus(int epDetailId, Inspection currentInspection = null);
        EPDetails GetCurrentEp(int epId);
        List<EPSteps> GetAllEpDocs(int userId, int epDetailId);
        TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp);
        List<EPDetails> GetFloorAssetEp(int floorAssetId);
        List<Inspection> GetEpInspectionDoc(int userId, int epdetailId);
        EPDetails GetInspectionHistory(int userId, int epdetailId, int inspectionId);
        int AddInspection(Inspection inspection);

        int AddPreviousInspection(Inspection inspection);
        Inspection GetActivityHistory(Guid activityId);
        List<EPDetails> GetArchivedEPsInspection(int userId);  
        calenderViewDashBoard GetDashboardCalendar(int userId);
        List<Inspection> GetDeficiencies(string activityId); 
        List<EPsDocument> GetEPsDocument(int epDetailId);        
        DataTable GetIlsmInspections(Guid? activityId);
        List<InspectionGroup> GetInspectionGroup();        
        List<TInspectionActivity> GetInspections(int floorAssetId, int epId);
        List<Inspection> GetInspectionsForCalendar(int userId, DateTime? startDate, DateTime? endDate);
        List<Inspection> GetInspectionsforWorkOrder(string activityId);        
        bool MarkInsdocStatus(int inspectionId, int isComplete);
        bool MarkInspectionRa();
        int Save(InspectionGroup objInspectionGroup);
        bool UpdateInspectionGroup(InspectionGroup objInspectionGroup);
        List<Inspection> GetEpsCampusInspections(int epDetailId);
        //Inspection GetWorkOrderSteps(Guid activityId);
        //Inspection GetInspection(int inspectionId);
        //Inspection GetInspection(int inspectionId, int epDetailId);
        //int GetEpDocStatus(int epDetailId);

        EPDetails GetEpByInspectionId(int epId, int InspectionId);
        int UndoInspection(Inspection inspection);
    }
}