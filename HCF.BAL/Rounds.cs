//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class Rounds
//    {
//        #region Rounds


//        public static List<TRoundUsers> GetTRoundUsers(int tRoundId)
//        {
//            return DAL.Rounds.GetTRoundUsers(tRoundId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newroundinps"></param>
//        /// <returns></returns>
//        public static int AddRoundInspection(TRounds newroundinps)
//        {
//            return DAL.Rounds.AddRoundInspection(newroundinps);
//        }

//        public static List<TRounds> GetBuildingRound(int? buildingId)
//        {
//            return DAL.Rounds.GetRoundDetails(buildingId);
//        }

//        public static List<TRounds> GetRoundDetails()
//        {
//            return DAL.Rounds.GetRoundDetails(null);
//        }


//        public static TRounds GetRoundDetails(int roundId)
//        {
//            return DAL.Rounds.GetRoundDetails(roundId);
//        }

//        public static TRounds GetRoundWODetails(int roundId)
//        {
//            return DAL.Rounds.GetRoundWoDetails(roundId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newroundinpsitm"></param>
//        /// <returns></returns>
//        public static int AddRoundItemsInspection(TRoundsQuestionnaires newroundinpsitm)
//        {
//            return DAL.Rounds.AddRoundItemsInspection(newroundinpsitm);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userid"></param>
//        /// <param name="floorid"></param>
//        /// <param name="wingId"></param>
//        /// <returns></returns>
//        public static List<TRounds> GetRoundsCategories(int userid, int floorid, int? wingId, int? rid)
//        {
//            return DAL.Rounds.GetFloorRound(floorid, rid.Value);
//        }

//        public static TRounds GetRoundsQuestionnaires(int floorid, int tRoundId, int userId)
//        {
//            return DAL.Rounds.GetRoundsQuestionnaires(floorid, tRoundId, userId);
//        }

//        public static TRounds GetRoundsQuestionOnFloorId(int floorid, int roundCatId, int userId)
//        {
//            return DAL.Rounds.GetRoundsQuestionOnFloorId(floorid, roundCatId, userId);
//        }

//        public static TRounds GetFloorRoundsQuestionnaires(int floorid, int tRoundId)
//        {
//            return DAL.Rounds.GetRoundsQuestionnaires(floorid, tRoundId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="roundId"></param>
//        /// <param name="floorid"></param>
//        /// <returns></returns>
//        public static List<TRounds> GetRoundsHistory(int roundId, int floorid)
//        {
//            return DAL.Rounds.GetRoundsHistory(roundId, floorid);
//        }

//        public static object GetPCRAActionPlans(Func<object, object, bool> referenceEquals)
//        {
//            throw new NotImplementedException();
//        }

//        public static List<Buildings> GetRoundLocations(int roundId)
//        {
//            return DAL.Rounds.GetRoundLocations(roundId);
//        }

//        //public static List<UserProfile> GetRoundUsers(int roundId)
//        //{
//        //    return DAL.Rounds.GetRoundUsers(roundId);
//        //}

//        public static List<TRoundUsers> GetRoundUsers(int roundId)
//        {
//            return DAL.Rounds.GetRoundUsers(roundId);
//        }
//        public static int AddRoundLocation(TRoundLocations roundLocation)
//        {
//            return DAL.Rounds.AddRoundLocation(roundLocation);
//        }

//        public static int AddRoundInspector(TRoundUsers roundUsers)
//        {
//            return DAL.Rounds.AddRoundInspector(roundUsers);
//        }

//        public static List<TRounds> GetCurrentRoundStatus(int roundId)
//        {
//            return DAL.Rounds.GetCurrentRoundStatus(roundId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="roundId"></param>
//        /// <param name="floorid"></param>
//        /// <param name="buildingId"></param>
//        /// <returns></returns>
//        public static List<TRounds> GetRoundsHistory(string roundId, int floorid, int buildingId)
//        {
//            return DAL.Rounds.GetRoundsHistory(roundId, floorid, buildingId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public static List<TRounds> GetRoundsCategories(int userId)
//        {
//            return DAL.Rounds.GetRoundsCategories(userId);
//        }

//        public static List<RoundCategory> GetRoundCategory(int catId)
//        {
//            return DAL.Rounds.GetRoundCategory(catId);
//        }

//        public static List<RoundCategory> GetRoundCategories()
//        {
//            return GetRoundCategory(0);
//        }

//        public static List<RoundCategory> GetRoundCategories_Data()
//        {
//            return DAL.Rounds.GetRoundCategories_Data();
//        }


//        //public static List<RoundCategory> GetRoundCat(int userid)
//        //{
//        //    return DAL.Rounds.GetRoundCat(0,userid);
//        //}

//        public static bool AddQuestionToCustomRound(int roundQuestionnaireId, int roundCatId, bool status)
//        {
//            return DAL.Rounds.AddQuestionToCustomRound(roundQuestionnaireId, roundCatId, status);
//        }

//        public static bool SaveRoundInspection(TRoundInspections inspection)
//        {
//            return DAL.Rounds.SaveRoundInspection(inspection);
//        }

//        public static bool SaveRoundInspectionSteps(int floorId, int userId, int status, string Comment, TRoundsQuestionnaires inspection)
//        {
//            return DAL.Rounds.SaveRoundInspectionSteps(floorId, userId, status, Comment, inspection);
//        }

//        public static bool SaveRoundStatus(TRounds rounds)
//        {
//            return DAL.Rounds.SaveRoundStatus(rounds);
//        }

//        public static bool SaveRoundInspStatus(TRoundInspections rounds)
//        {
//            return DAL.Rounds.SaveRoundInspStatus(rounds);
//        }

//        public static bool SaveRoundFailStatus(int status, int roundId)
//        {
//            return DAL.Rounds.SaveRoundFailStatus(status, roundId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="troundId"></param>
//        /// <returns></returns>
//        public static List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(int troundId)
//        {
//            return DAL.Rounds.GetTRoundsQuestionnaires(troundId);
//        }

//        public static List<TRounds> GetConsolidatedRoundsReport()
//        {
//            return DAL.Rounds.GetConsolidatedRoundsReport();
//        }

//        #endregion

//        #region RoundsQuestionnaires


//        public static int AddRoundCategory(RoundCategory newRoundCategory)
//        {
//            return DAL.RoundsQuesRepository.Save(newRoundCategory);
//        }

//        public static bool Update_RoundSchduleDatesOnRoundCat(RoundCategory newRoundCategory)
//        {
//            return DAL.RoundsQuesRepository.Update_RoundSchduleDatesOnRoundCat(newRoundCategory);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="questionnaires"></param>
//        /// <returns></returns>
//        public static int AddRoundsQuestionnaires(RoundsQuestionnaires questionnaires)
//        {
//            return DAL.RoundsQuesRepository.Save(questionnaires);
//        }




//        public static List<TRoundsQuestionnaires> GetMatrixData(DateTime startDate, DateTime endDate)
//        {
//            return DAL.Rounds.GetTRoundsQuestionnaires(startDate, endDate);
//        }

//        #endregion

//        #region Time Slots

//        public static List<TimeSlots> GetTimeSlots(DateTime selectedDate, int timeSlotPeriod = 4)
//        {
//            return DAL.Rounds.GetTimeSlots(selectedDate, timeSlotPeriod);
//        }



//        #endregion


//        #region PMDailyLogs

//        public static TPMLogs PMDailyLog(int? pmLogId)
//        {
//            return DAL.Rounds.PmDailyLog(pmLogId);
//        }


//        public static object PMDailyLogs(DateTime? FromDate, DateTime? ToDate)
//        {
//            return DAL.Rounds.PmDailyLogs(null, FromDate, ToDate);
//        }

//        public static int SavePMDailyLog(TPMLogs logs)
//        {
//            int logId = DAL.Rounds.SavePmDailyLog(logs);
//            if (logId > 0)
//            {
//                foreach (var questionnaires in logs.Questionnaires)
//                {
//                    foreach (var item in questionnaires.QuestionnaireSteps.Where(x=>x.IsDeleted==false))
//                    {
//                        foreach (var logSteps in item.PMLogSteps)
//                        {
//                            logSteps.PMLogId = logId;
//                            logSteps.QuestionnaireStepId = item.QuestionnaireStepId;
//                            logSteps.IsDeleted = logSteps.IsDeleted;
//                            DAL.Rounds.SavePmLogSteps(logSteps);
//                        }
//                    }
//                }
//            }
//            return logId;
//        }



//        #endregion


//        #region RoundCommonQues
//        public static List<RoundCategory> RoundCommonQuestionnaries()
//        {
//            return DAL.Rounds.RoundCommonQuestionnaries();
//        }

//        public static List<RoundCategory> GetCommonRoundCategory()
//        {
//            return DAL.Rounds.GetCommonRoundCategory();
//        }
//        #endregion

//    }
//}
