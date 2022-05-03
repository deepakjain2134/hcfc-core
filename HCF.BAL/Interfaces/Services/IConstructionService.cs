using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IConstructionService
    {
        int AddConstClassActivity(ConstructionClassActivity modelData);
        int AddConstructionClass(ConstructionClass modelData);
        int AddConstructionRisk(ConstructionRisk modelData);
        int AddConstructionType(ConstructionType modelData);
        int AddConstuctionActivity(ConstructionActivity modelData);
        int AddIcra(TIcraLog modelData, string permittype);
        int AddICRAMatixPrecautions(int constructionRiskId, int constructionTypeId, string classIdss, int createdby, int version);
        int AddICRARiskArea(ICRARiskArea iCRARiskArea);
        int AddICRASteps(ICRASteps modelData);
        int AddInspectionIcra(TIcraLog modelData);
        int AddObservationReportCheckPoint(ICRAObsReportCheckPoints checkPoints);
        int AddRiskAreaToArea(int constructionRiskId, string riskAreaIds, int CreatedBy);
        int AddTICRASteps(TIcraSteps modelData);
        bool DeleteFile(int fileId);
        bool DeleteICRADrawings(int TicraId, string fileIds);
        bool DeleteRiskAreaToArea(int riskAreaId);
        List<TIcraLog> GetAllInspectionIcraSteps(int icraId, DateTime? fromdate = null, DateTime? todate = null, int? statusId = null, int? constructionrikId = null, int? version = null);
        AllPermitReport GetAllPermitCount(string from, string todate, string permittype);
        List<ConstRiskDeptMap> GetConstRiskDeptMap();
        List<ConstructionClass> GetConstructionClass();
        ConstructionClass GetConstructionClass(int? constructionclassid);
        List<ConstructionClassCategrory> GetConstructionClassCategory();
        List<ConstructionRisk> GetConstructionRisk();
        ConstructionRisk GetConstructionRisk(int? constructionriskid);
        IEnumerable<ConstructionType> GetConstructionType();
        ConstructionType GetConstructionType(int? ConstructionTypeId);
        List<Audit> GetIcraHistory(int icraId);
        List<ICRAMatixPrecautions> GetICRAMatixPrecautions();
        TIcraLog GetICRAObservationReport(int icraId);
        TIcraLog GetICRAObservationReport(int icraId, int reportId);
        TICRAReports GetICRAProjectObservationReport(string projectIds);
        TICRAReports GetICRAProjectObservationReport(string projectIds, string reportId);
        List<TICRAReports> GetICRAProjectReport();
        List<ICRARiskArea> GetICRARiskAra();
        ICRARiskArea GetICRARiskAra(int riskAreaId);
        List<ICRARiskArea> GetICRARiskAraWithOutRisk();
        List<TIcraLog> GetIcras();
        TIcraLog GetIcras(int icraId);
        List<ICRASteps> GetICRASteps();
        ICRASteps GetICRASteps(int icrastepId);
        List<TIcraLog> GetIcrasWithIlsm(string searchParam, bool? IsICRA = true);
        TIcraLog GetInspectionIcraSteps(int? icraId);
        string GetPermitNumber(int TICRAId, string permittype);
        List<ProjectType> GetProjectType();
        List<ICRAObsReportCheckPoints> GetReportCheckPoints();
        ICRAObsReportCheckPoints GetReportCheckPoints(int checkPointId);
        List<TICRAFiles> GetTICRAFiles(int icraid);
        int SaveAreaSurrounding(TICRAAreasNearBy tICRAAreasNearBy);
        int SaveDigitalFile(DigitalSignature dSPermitAuthorizedBy, string projectNo);
        int SaveProjectReport(TICRAReports observtionReport, int userId);
        int SaveReport(TICRAReports observtionReport, int userId);
        int SaveReportCheckPoints(TICRAReportsCheckPoints checkPoints);
        void SaveTICRAAttachments(List<TICRAFiles> tICRAFiles, int tICRAid);
        void SaveTICRAFiles(List<TICRAFiles> tICRAFiles, int tICRAid);
        int SaveTICRAFiles(TICRAFiles item);
        bool UpdateICRARiskAra(ICRARiskArea iCRARiskArea);
        bool SaveObservationReport(TICRAReports newTICRAReports);
    }
}