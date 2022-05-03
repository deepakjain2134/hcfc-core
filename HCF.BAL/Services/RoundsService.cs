using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class RoundsService : IRoundsService
    {
        #region Rounds

        private readonly IRounds _roundRepository;
        private readonly IRoundsQuesRepository _roundsQuesRepository;

        public RoundsService(IRounds roundRepository, IRoundsQuesRepository roundsQuesRepository)
        {
            _roundsQuesRepository = roundsQuesRepository;
               _roundRepository = roundRepository;
        }

        public  List<TRoundUsers> GetTRoundUsers(int tRoundId)
        {
            return _roundRepository.GetTRoundUsers(tRoundId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newroundinps"></param>
        /// <returns></returns>
        public  int AddRoundInspection(TRounds newroundinps, string floorIds, string userIds)
        {
            return _roundRepository.AddRoundInspection(newroundinps, floorIds, userIds);
        }
        public int SaveRoundFloorInspection(int FloorId, int TRoundId,int UserId)
        {
            return _roundRepository.SaveRoundFloorInspection(FloorId, TRoundId, UserId);
        }
        public  List<TRounds> GetBuildingRound(int? buildingId)
        {
            return _roundRepository.GetRoundDetails(buildingId);
        }

        public  List<TRounds> GetRoundDetails()
        {
            return _roundRepository.GetRoundDetails(null);
        }


        public  TRounds GetRoundDetails(int roundId)
        {
            return _roundRepository.GetRoundDetails(roundId);
        }

        public  TRounds GetRoundWODetails(int roundId)
        {
            return _roundRepository.GetRoundWoDetails(roundId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newroundinpsitm"></param>
        /// <returns></returns>
        public  int AddRoundItemsInspection(TRoundsQuestionnaires newroundinpsitm)
        {
            return _roundRepository.AddRoundItemsInspection(newroundinpsitm);
        }
        public int SetArchiveRound(string dbname)
        {
            return _roundRepository.SetArchiveRound(dbname);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="floorid"></param>
        /// <param name="wingId"></param>
        /// <returns></returns>
        public  List<TRounds> GetRoundsCategories(int userid, int floorid, int? wingId, int? rid)
        {
            return _roundRepository.GetFloorRound(floorid, rid.Value);
        }

        public  TRounds GetRoundsQuestionnaires(int floorid, int tRoundId, int userId)
        {
            return _roundRepository.GetRoundsQuestionnaires(floorid, tRoundId, userId);
        }

        public  TRounds GetRoundsQuestionOnFloorId(int floorid, int roundCatId, int userId)
        {
            return _roundRepository.GetRoundsQuestionOnFloorId(floorid, roundCatId, userId);
        }

        public  TRounds GetFloorRoundsQuestionnaires(int floorid, int tRoundId)
        {
            return _roundRepository.GetRoundsQuestionnaires(floorid, tRoundId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundId"></param>
        /// <param name="floorid"></param>
        /// <returns></returns>
        public  List<TRounds> GetRoundsHistory(int roundId, int floorid)
        {
            return _roundRepository.GetRoundsHistory(roundId, floorid);
        }

        public  object GetPCRAActionPlans(Func<object, object, bool> referenceEquals)
        {
            throw new NotImplementedException();
        }

        public  List<Buildings> GetRoundLocations(int roundId)
        {
            return _roundRepository.GetRoundLocations(roundId);
        }

        //public  List<UserProfile> GetRoundUsers(int roundId)
        //{
        //    return _roundRepository.GetRoundUsers(roundId);
        //}

        public  List<TRoundUsers> GetRoundUsers(int roundId)
        {
            return _roundRepository.GetRoundUsers(roundId);
        }
        public  int AddRoundLocation(TRoundLocations roundLocation)
        {
            return _roundRepository.AddRoundLocation(roundLocation);
        }

        public  int AddRoundInspector(TRoundUsers roundUsers)
        {
            return _roundRepository.AddRoundInspector(roundUsers);
        }

        public  List<TRounds> GetCurrentRoundStatus(int roundId)
        {
            return _roundRepository.GetCurrentRoundStatus(roundId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundId"></param>
        /// <param name="floorid"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public  List<TRounds> GetRoundsHistory(string roundId, int floorid, int buildingId)
        {
            return _roundRepository.GetRoundsHistory(roundId, floorid, buildingId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<TRounds> GetRoundsCategories(int userId)
        {
            return _roundRepository.GetRoundsCategories(userId);
        }

        public  List<RoundCategory> GetRoundCategory(int catId)
        {
            return _roundRepository.GetRoundCategory(catId);
        }

        public  List<RoundCategory> GetRoundCategories()
        {
            return GetRoundCategory(0);
        }

        public  List<RoundCategory> GetRoundCategories_Data()
        {
            return GetRoundCategory(0);
            // return _roundRepository.GetRoundCategories_Data();
        }


        //public  List<RoundCategory> GetRoundCat(int userid)
        //{
        //    return _roundRepository.GetRoundCat(0,userid);
        //}

        public  bool AddQuestionToCustomRound(int roundQuestionnaireId, int roundCatId, bool status)
        {
            return _roundRepository.AddQuestionToCustomRound(roundQuestionnaireId, roundCatId, status);
        }

        public  bool SaveRoundInspection(TRoundInspections inspection)
        {
            return _roundRepository.SaveRoundInspection(inspection);
        }

        public  bool SaveRoundInspectionSteps(int floorId, int userId, int status, string Comment, TRoundsQuestionnaires inspection, int roundCatId)
        {
            return _roundRepository.SaveRoundInspectionSteps(floorId, userId, status, Comment, inspection, roundCatId);
        }

        public  bool SaveRoundStatus(TRounds rounds, int userId)
        {
            return _roundRepository.SaveRoundStatus(rounds, userId);
        }

        public  bool SaveRoundInspStatus(TRoundInspections rounds)
        {
            return _roundRepository.SaveRoundInspStatus(rounds);
        }

        public  bool SaveRoundFailStatus(int status, int roundId)
        {
            return _roundRepository.SaveRoundFailStatus(status, roundId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="troundId"></param>
        /// <returns></returns>
        public  List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(int troundId)
        {
            return _roundRepository.GetTRoundsQuestionnaires(troundId);
        }

        public  List<TRounds> GetConsolidatedRoundsReport()
        {
            return _roundRepository.GetConsolidatedRoundsReport();
        }

        #endregion

        #region RoundsQuestionnaires


        public  int AddRoundCategory(RoundCategory newRoundCategory)
        {
            return _roundsQuesRepository.Save(newRoundCategory);
        }

        public  bool Update_RoundSchduleDatesOnRoundCat(RoundCategory newRoundCategory)
        {
            return _roundsQuesRepository.Update_RoundSchduleDatesOnRoundCat(newRoundCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionnaires"></param>
        /// <returns></returns>
        public  int AddRoundsQuestionnaires(RoundsQuestionnaires questionnaires)
        {
            return _roundsQuesRepository.Save(questionnaires);
        }




        public  List<TRoundsQuestionnaires> GetMatrixData(DateTime startDate, DateTime endDate)
        {
            return _roundRepository.GetTRoundsQuestionnaires(startDate, endDate);
        }

        public TRounds GetRoundDetailsbyScheduleDateId(int? rscheduleDateId)
        {
            return _roundRepository.GetRoundDetailsbyScheduleDateId(rscheduleDateId);
        }

        public bool SaveTRoundUserCategories(int tRoundId, int userId, string roundCatIds)
        {
            return _roundRepository.SaveTRoundUserCategories(tRoundId, userId, roundCatIds);
        }

        

        #endregion

        #region Time Slots

        public  List<TimeSlots> GetTimeSlots(DateTime selectedDate, int timeSlotPeriod = 4)
        {
            List<TimeSlots> slots = new List<TimeSlots>();

            DateTime oCurrentDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
            DateTime oTimeSlot = new DateTime(oCurrentDate.Year, oCurrentDate.Month, oCurrentDate.Day, oCurrentDate.Hour, oCurrentDate.Minute, oCurrentDate.Second);
            bool isNext = false;
            for (var iCounter = 1; iCounter <= 24 / timeSlotPeriod; iCounter++)
            {
                TimeSlots slot = new TimeSlots();
                slot.Start = oTimeSlot;

                slot.Sequence = iCounter;
                slot.DateText = selectedDate.ToString("MMM dd, yyyy");
                slot.End = oTimeSlot.AddHours(timeSlotPeriod).AddMinutes(-1);
                slot.StartTimeSpan = Utility.Conversion.ConvertToTimeSpan(slot.Start.ToUniversalTime());
                slot.EndTimeSpan = Utility.Conversion.ConvertToTimeSpan(slot.End.ToUniversalTime());

                slot.StartTimeText = slot.Start.ToString("t");
                slot.EndTimeText = slot.End.ToString("t");

                slot.UTCEndTime = slot.End.ToUniversalTime();
                slot.UTCStartTime = slot.Start.ToUniversalTime();
                slot.isNext = isNext;
                if (DateTime.UtcNow.Ticks > slot.UTCStartTime.Ticks && DateTime.UtcNow.Ticks < slot.UTCEndTime.Ticks)
                {
                    slot.isCurrent = true;
                }
                slots.Add(slot);
                if (DateTime.UtcNow.Ticks > slot.UTCStartTime.Ticks && DateTime.UtcNow.Ticks < slot.UTCEndTime.Ticks)
                {
                    isNext = true;
                }
                oTimeSlot = oTimeSlot.AddHours(timeSlotPeriod);
            }

            return slots;
        }



        #endregion


        #region PMDailyLogs

        public  TPMLogs PMDailyLog(int? pmLogId)
        {
            return _roundRepository.PmDailyLog(pmLogId);
        }


        public  object PMDailyLogs(DateTime? FromDate, DateTime? ToDate)
        {
            return _roundRepository.PmDailyLogs(null, FromDate, ToDate);
        }

        public  int SavePMDailyLog(TPMLogs logs)
        {
            int logId = _roundRepository.SavePmDailyLog(logs);
            if (logId > 0)
            {
                foreach (var questionnaires in logs.Questionnaires)
                {
                    foreach (var item in questionnaires.QuestionnaireSteps.Where(x=>x.IsDeleted==false))
                    {
                        foreach (var logSteps in item.PMLogSteps)
                        {
                            logSteps.PMLogId = logId;
                            logSteps.QuestionnaireStepId = item.QuestionnaireStepId;
                            logSteps.IsDeleted = logSteps.IsDeleted;
                            _roundRepository.SavePmLogSteps(logSteps);
                        }
                    }
                }
            }
            return logId;
        }



        #endregion


        #region RoundCommonQues
        public  List<RoundCategory> RoundCommonQuestionnaries()
        {
            return _roundRepository.RoundCommonQuestionnaries();
        }

        public  List<RoundCategory> GetCommonRoundCategory()
        {
            return _roundRepository.GetCommonRoundCategory();
        }
        #endregion

    }
}
