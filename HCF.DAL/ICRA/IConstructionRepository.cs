using System;
using System.Collections.Generic;
using System.Data;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IConstructionRepository
    {
        int AddConstClassActivity(ConstructionClassActivity modelData);
        int AddConstructionClass(ConstructionClass modelData);
        int AddConstructionRisk(ConstructionRisk modelData);
        int AddConstructionType(ConstructionType modelData);
        int AddConstuctionActivity(ConstructionActivity modelData);
        int AddICRAMatixPrecautions(int constructionRiskId, int constructionTypeId, string classIdss, int createdby, int version);
        int AddICRARiskArea(ICRARiskArea modelData);
        int AddICRASteps(ICRASteps modelData);
        int AddInspectionIcra(TIcraLog modelData);
        int AddRiskAreaToArea(int constructionRiskId, string riskAreaIds, int createdBy);
        int AddTICRASteps(TIcraSteps modelData);
        List<TICRAFiles> ConvertdtTICRAFiles(DataTable dataTable);
        bool DeleteICRADrawings(int TicraId, string fileIds);
        bool DeleteRiskAreaToArea(int riskAreaIds);
        List<TIcraLog> GetAllInspectionIcraSteps(int icraId, DateTime? fromdate = null, DateTime? todate = null, int? statusId = null, int? constructionrikId = null, int? version = null);
        AllPermitReport GetAllPermitCount(string from, string todate, string permittype);
        List<ConstRiskDeptMap> GetConstRiskDeptMap();
        ConstructionActivity GetConstructionActivity(int? constActivityId);
        List<ConstructionClass> GetConstructionClass();
        ConstructionClass GetConstructionClass(int? constructionclassid);
        List<ConstructionClassCategrory> GetConstructionClassCategory();
        List<ConstructionRisk> GetConstructionRisk();
        ConstructionRisk GetConstructionRisk(int? constructionriskid);
        IEnumerable<ConstructionType> GetConstructionType();
        ConstructionType GetConstructionType(int? constructionTypeId);
        List<Audit> GetIcraHistory(int icraId);
        List<ICRAMatixPrecautions> GetICRAMatixPrecautions();
        List<TICRAReports> GetICRAProjectReport(string projectIds = null);
        List<ICRARiskArea> GetICRARiskArea();
        ICRARiskArea GetICRARiskArea(int riskAreaId);
        List<TIcraLog> GetIcras(int? icraId);
        List<ICRASteps> GetICRASteps();
        ICRASteps GetICRASteps(int icrastepId);
        List<TIcraLog> GetIcrasWithIlsm(string searchParam, bool IsICRA);
        TIcraLog GetInspectionIcraSteps(int? icraId);
        List<ProjectType> GetProjectType();
        List<TICRAFiles> GetTICRAFiles(int icraId);
        int SaveAreaSurrounding(TICRAAreasNearBy tICRAAreasNearBy);
        int SaveTICRAFiles(TICRAFiles modelData);
        bool UpdateICRARiskArea(ICRARiskArea modelData);
    }
}