using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IIlsmRepository
    {
        bool AddStepWithIlLsmStep(int stepId, int ilsmStepsId);
        string CompleteIlsmIncident(int tilsId);
        List<TIlsm> Get();
        List<TIlsm> GetAllIlsm(DateTime? fromdate = null, DateTime? todate = null, int? tilsmId = null);
        TIlsm GetCurrentTilsm(Guid activityId);
        List<TIlsm> GetIlsm();
        List<TIlsm> GetIlsmAsync();
        TIlsm GetIlsmDetails(int tilsmId);
        TIlsm GetIlsmInspection(int tilsmId);
        string GetIlsmwoDescription(int tilsmId);
        bool IcraLinkToIslm(int icraId, int lsmId, int createdBy);
        bool IlsMlinkToWo(int tilsmId, int issueId);
        DataTable InsertConstructionTilsm(TIlsm ilsm, string epDetailIds);
        int InsertTilsm(TIlsm tilsm);
        bool SaveAdditionalTilsmEp(int epdetailId, int tilsmId, bool isActive);
        bool SaveTilsmReport(TIlsm newTilsm);
        int UpdateIlsm(TIlsm objtilsm);
        bool UpdateIlsmMatrix(AffectedEPs affectedEps);
        bool UpdateIlsmScore(Steps steps);
        bool UpdateIlsmStatus(TIlsm tilsm);
        bool ILSMlinkToObservationReport(string tilsmId, string ticraReportIds);
    }
}