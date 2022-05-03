using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IIlsmService
    {
        bool AddStepWithIlLsmStep(int stepId, int ilsmStepId);
        string CompleteIlsmIncident(int tilsmId);
        List<TIlsm> GetAllIlsm(DateTime? fromDate = null, DateTime? toDate = null);
        TIlsm GetCurrentTilsm(Guid activityId);
        List<TIlsm> GetIlsm();
        TIlsm GetIlsm(int tilsmId);
        List<TIlsm> GetIlsmAsync();
        TIlsm GetIlsmDetails(int tilsmId);
        TIlsm GetIlsmInspection(int tilsmId);
        string GetIlsmwoDescription(int tilsmId);
        bool IcraLinkToIslm(int icraId, int lsmId, int createdBy);
        bool ILSMlinkToWO(int tilsmId, int issueId);
        DataTable InsertConstructionTilsm(TIlsm tilsm, string epDetails);
        bool SaveAdditionalTilsmEP(int epDetailId, int tilsmId, bool isActive);
        bool SaveTilsmReport(TIlsm newTilsm);
        int UpdateIlsm(TIlsm objTilsm);
        bool UpdateIlsmMatrix(AffectedEPs affectedEps);
        bool UpdateIlsmScore(Steps steps);
        void UpdateIlsmStatus(Guid? activityId = null);
        bool UpdateILSMStatus(TIlsm tilsmId);
        bool ILSMlinkToObservationReport(string tilsmId, string ticraReportIds);
    }
}