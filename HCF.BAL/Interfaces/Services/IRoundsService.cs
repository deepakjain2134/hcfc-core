using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IRoundsService
    {
        bool AddQuestionToCustomRound(int roundQuestionnaireId, int roundCatId, bool status);
        int AddRoundCategory(RoundCategory newRoundCategory);
        int AddRoundInspection(TRounds newroundinps, string floorIds, string userIds);
        int SaveRoundFloorInspection(int FloorId, int TRoundId, int UserId);
        int AddRoundInspector(TRoundUsers roundUsers);
        int SetArchiveRound(string dbname);

        int AddRoundItemsInspection(TRoundsQuestionnaires newroundinpsitm);
        int AddRoundLocation(TRoundLocations roundLocation);
        int AddRoundsQuestionnaires(RoundsQuestionnaires questionnaires);
        List<TRounds> GetBuildingRound(int? buildingId);
        List<RoundCategory> GetCommonRoundCategory();
        List<TRounds> GetConsolidatedRoundsReport();
        List<TRounds> GetCurrentRoundStatus(int roundId);
        TRounds GetFloorRoundsQuestionnaires(int floorid, int tRoundId);
        List<TRoundsQuestionnaires> GetMatrixData(DateTime startDate, DateTime endDate);
        object GetPCRAActionPlans(Func<object, object, bool> referenceEquals);
        List<RoundCategory> GetRoundCategories();
        List<RoundCategory> GetRoundCategories_Data();
        List<RoundCategory> GetRoundCategory(int catId);
        List<TRounds> GetRoundDetails();
        TRounds GetRoundDetails(int roundId);
        List<Buildings> GetRoundLocations(int roundId);
        List<TRounds> GetRoundsCategories(int userId);
        List<TRounds> GetRoundsCategories(int userid, int floorid, int? wingId, int? rid);
        List<TRounds> GetRoundsHistory(int roundId, int floorid);
        List<TRounds> GetRoundsHistory(string roundId, int floorid, int buildingId);
        TRounds GetRoundsQuestionnaires(int floorid, int tRoundId, int userId);
        TRounds GetRoundsQuestionOnFloorId(int floorid, int roundCatId, int userId);
        List<TRoundUsers> GetRoundUsers(int roundId);
        TRounds GetRoundWODetails(int roundId);
        List<TimeSlots> GetTimeSlots(DateTime selectedDate, int timeSlotPeriod = 4);
        List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(int troundId);
        List<TRoundUsers> GetTRoundUsers(int tRoundId);
        TPMLogs PMDailyLog(int? pmLogId);
        object PMDailyLogs(DateTime? FromDate, DateTime? ToDate);
        List<RoundCategory> RoundCommonQuestionnaries();
        int SavePMDailyLog(TPMLogs logs);
        bool SaveRoundFailStatus(int status, int roundId);
        bool SaveRoundInspection(TRoundInspections inspection);
        bool SaveRoundInspectionSteps(int floorId, int userId, int status, string Comment, TRoundsQuestionnaires inspection, int roundCatId);
        bool SaveRoundInspStatus(TRoundInspections rounds);
        bool SaveRoundStatus(TRounds rounds, int userId);
        bool Update_RoundSchduleDatesOnRoundCat(RoundCategory newRoundCategory);
        TRounds GetRoundDetailsbyScheduleDateId(int? rscheduleDateId);

        bool SaveTRoundUserCategories(int TRoundId, int UserId, string RoundCatIds);

    }
}