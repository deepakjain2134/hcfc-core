using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IObservationReport
    {
        int AddObservationReportCheckPoint(ICRAObsReportCheckPoints modelData);
        int AddTICRAReport(TICRAReports modelData);
        int AddTICRAReportCheckPoint(TICRAReportsCheckPoints modelData);
        bool DeleteFile(int fileId);
        TICRAReports GetICRAProjectObservationReport(string projectIds, string reportId = null);
        TIcraLog GetIcraReport(int icraId, int reportId);
        string GetPermitNumber(int TICRAId, string permittype);
        List<ICRAObsReportCheckPoints> GetReportCheckPoint();
        TIcraLog GetTICRAReport(int icraId);
        int SaveProjectReport(TICRAReports modelData);
        bool SaveObservationReport(TICRAReports newTICRAReports);
    }
}