using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IRounds
    {
        bool AddQuestionToCustomRound(int roundQuestionnaireId, int roundCatId, bool status);
        int AddRoundInspection(TRounds inspectionRound, string floorIds, string userIds);
        int SaveRoundFloorInspection(int FloorId, int TRoundId, int UserId);
        int AddRoundInspector(TRoundUsers roundUsers);
        int AddRoundItemsInspection(TRoundsQuestionnaires inspectionRoundItems);
        int SetArchiveRound(string dbname);
        int AddRoundLocation(TRoundLocations roundLocation);
        List<RoundCategory> GetCommonRoundCategory();
        List<TRounds> GetConsolidatedRoundsReport();
        List<TRounds> GetCurrentRoundStatus(int roundId);
        List<TRounds> GetFloorRound(int floorId, int roundId);
        List<RoundCategory> GetRoundCategories_Data();
        List<RoundCategory> GetRoundCategory(int catId);
        TRounds GetRoundDetails(int roundId);
        List<TRounds> GetRoundDetails(int? buildingId);
        List<Buildings> GetRoundLocations(int roundId);
        List<TRounds> GetRoundsCategories(int userId);
        List<TRounds> GetRoundsHistory(int? roundId, int? floorId, int assetId = 0, int floorassetId = 0);
        List<TRounds> GetRoundsHistory(string roundId, int floorId, int buildingId);
        TRounds GetRoundsQuestionnaires(int floorId, int tRoundId);
        TRounds GetRoundsQuestionnaires(int floorId, int tRoundId, int userId);
        TRounds GetRoundsQuestionOnFloorId(int floorId, int roundCatId, int userId);
        List<TRoundUsers> GetRoundUsers(int roundId);
        TRounds GetRoundWoDetails(int roundId);
        List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(DateTime startDate, DateTime endDate);
        List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(int tRoundId);
        List<TRoundUsers> GetTRoundUsers(int tRoundId);
        TPMLogs PmDailyLog(int? pmlogId);
        List<TPMLogs> PmDailyLogs(int? logId, DateTime? FromDate, DateTime? ToDate);
        List<RoundCategory> RoundCommonQuestionnaries();
        int SavePmDailyLog(TPMLogs logs);
        int SavePmLogSteps(TPMLogSteps pMLogSteps);
        bool SaveRoundFailStatus(int status, int roundId);
        bool SaveRoundInspection(TRoundInspections inspection);
        bool SaveRoundInspectionSteps(int floorId, int userId, int status, string comment, TRoundsQuestionnaires roundQuestionnaires,int roundCatId);
        bool SaveRoundInspStatus(TRoundInspections roundInsp);
        bool SaveRoundStatus(TRounds rounds, int userId);
        TRounds GetRoundDetailsbyScheduleDateId(int? rscheduleDateId);
        bool SaveTRoundUserCategories(int tRoundId, int userId, string roundCatIds);
    }
}