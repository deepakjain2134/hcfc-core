//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//namespace HCF.BAL
//{
//    public static class ExercisesRepository
//    {
//        #region Exercises

//        public static int CreateExercises(TExercises newExercises)
//        {
//            return DAL.ExercisesRepository.CreateExercises(newExercises);
//        }

//        public static bool UpdateExercises(TExercises newExercises)
//        {
//            return DAL.ExercisesRepository.UpdateExercises(newExercises);
//        }

//        public static List<TExercises> GetExercises(int? exerciseId)
//        {
//            return DAL.ExercisesRepository.GetExercises(exerciseId);
//        }

//        public static List<TExercises> GetExerciseFiles(int? exerciseId)
//        {
//            return DAL.ExercisesRepository.GetExerciseFiles(exerciseId);
//        }

//        public static bool DeleteExercises(int exerciseId)
//        {
//            return DAL.ExercisesRepository.DeleteExercises(exerciseId);
//        }

//        public static int InsertTExerciseFiles(TExerciseFiles exercise)
//        {
//            return DAL.ExercisesRepository.InsertTExerciseFiles(exercise);
//        }

//        public static List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId)
//        {
//            return DAL.ExercisesRepository.GetTExerciseQuestionnaires(exerciseId);
//        }

//        public static void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterNo)
//        {
//            DAL.ExercisesRepository.PlanDrill(buildingTypeId, newExercises, year, quarterNo);
//        }

//        public static void PlanDrill(TExercises newExercises, int year, int quarterNo)
//        {
//            DAL.ExercisesRepository.PlanDrill(newExercises, year, quarterNo);
//        }

//        #endregion Exercises

//        #region Exercise Settings

//        public static int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster)
//        {
//            return DAL.ExercisesRepository.InsertQuarterMasterSettings(newQuarterMaster);
//        }

//        public static bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings)
//        {
//            return DAL.ExercisesRepository.UpdateQuarterMasterSettings(quarterMasterSettings);
//        }

//        public static List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todaydrill)
//        {
//            return DAL.ExercisesRepository.GetQuarterSettings(buildingTypeId, quarterNo, year, todaydrill);
//        }

//        public static List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string sitecode)
//        {
//            return DAL.ExercisesRepository.GetQuarterMaster(buildingTypeId, quarterNo, year, buildingId, sitecode);
//        }

//        public static QuarterMaster GetQuarterSettings(string buildingIds, int year, bool todaydrill)
//        {
//            return DAL.ExercisesRepository.GetQuarterSettings(buildingIds, year, todaydrill);
//        }

//        public static void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles)
//        {
//            DAL.ExercisesRepository.GetExercises(newShift, shiftId, buildingId, fireDrillTypes, exercises, tExercisesfiles);
//        }

//        #endregion

//        #region Fire Drill common Questionnaires

//        public static int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
//        {
//            return DAL.ExercisesRepository.InsertFiredrillQuestionnaries(newFireDrillQuestionnaires);
//        }

//        public static bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
//        {
//            return DAL.ExercisesRepository.UpdateFiredrillQuestionnaries(newFireDrillQuestionnaires);
//        }

//        public static List<FireDrillCategory> GetFiredrillQuestionnaries()
//        {
//            return DAL.ExercisesRepository.GetFiredrillQuestionnaries();
//        }

//        public static FireDrillQuestionnaires GetFiredrillQuestionnarie(int questionId)
//        {
//            return DAL.ExercisesRepository.GetFireDrillQuestionnaire(questionId);
//        }

//        #endregion

//        #region TExerciseQuestionnaires

//        public static int InsertTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires)
//        {
//            return DAL.ExercisesRepository.InsertTExerciseQuestionnaires(texerciseQuestionnaires);
//        }

//        public static int InsertTExerciseActions(TExerciseActions exerActions)
//        {
//            return DAL.ExercisesRepository.InsertTExerciseActions(exerActions);
//        }

//        public static bool DeleteTExerciseActionsbyTExerciseId(int texerciseId)
//        {
//            return DAL.ExercisesRepository.DeleteTExerciseActions(texerciseId, 0);
//        }
//        public static bool DeleteTExerciseActionsTExerciseActionId(int texerciseActionId)
//        {
//            return DAL.ExercisesRepository.DeleteTExerciseActions(0, texerciseActionId);
//        }
//        #endregion

//        #region FireDrill Ques
//        public static List<FireDrillCategory> GetFiredrillQues()
//        {
//            return DAL.ExercisesRepository.GetFiredrillQues();
//        }


//        public static FireDrillQuestionnaires GetFiredrillQues(int questionId)
//        {
//            return DAL.ExercisesRepository.GetFireDrillQuestion(questionId);
//        }

//        #endregion

//        #region FireDrill Common Categ
//        public static List<FireDrillCategory> GetCommonFireDrillCategory()
//        {
//            return DAL.ExercisesRepository.GetCommonFireDrillCategory();
//        }

//        public static int AddFireDrilCommonCategory(FireDrillCategory newFiredrillCategory)
//        {
//            return DAL.ExercisesRepository.Save(newFiredrillCategory);
//        }
//        #endregion

//        #region FireDrill Categ
//        public static List<FireDrillCategory> GetFireDrillCategory()
//        {
//            return DAL.ExercisesRepository.GetFireDrillCategory();
//        }

//        public static int AddFireDrilCategory(FireDrillCategory newFiredrillCategory)
//        {
//            return DAL.ExercisesRepository.Savefiredrill(newFiredrillCategory);
//        }
//        #endregion
//    }
//}
