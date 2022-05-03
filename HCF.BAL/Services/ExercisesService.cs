using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
namespace HCF.BAL
{
    public  class ExercisesService : IExercisesService
    {
        private readonly IExercisesRepository _exercisesRepository;
        public ExercisesService(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository;
        }
        #region Exercises

        public  int CreateExercises(TExercises newExercises)
        {
            return _exercisesRepository.CreateExercises(newExercises);
        }

        public  bool UpdateExercises(TExercises newExercises)
        {
            return _exercisesRepository.UpdateExercises(newExercises);
        }

        public  List<TExercises> GetExercises(int? exerciseId)
        {
            return _exercisesRepository.GetExercises(exerciseId);
        }

        public  List<TExercises> GetExerciseFiles(int? exerciseId)
        {
            return _exercisesRepository.GetExerciseFiles(exerciseId);
        }

        public  bool DeleteExercises(int exerciseId)
        {
            return _exercisesRepository.DeleteExercises(exerciseId);
        }

        public  int InsertTExerciseFiles(TExerciseFiles exercise)
        {
            return _exercisesRepository.InsertTExerciseFiles(exercise);
        }

        public  List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId)
        {
            return _exercisesRepository.GetTExerciseQuestionnaires(exerciseId);
        }

        //public  void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterNo)
        //{
        //    _exercisesRepository.PlanDrill(buildingTypeId, newExercises, year, quarterNo);
        //}

        public  void PlanDrill(TExercises newExercises, int year, int quarterNo)
        {
            _exercisesRepository.PlanDrill(newExercises, year, quarterNo);
        }

        public QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year,  bool todaydrill = false)
        {
            return _exercisesRepository.GetQuarterSettings(buildingIds, locationGroupIds, mode, year, todaydrill);
        }

        public void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles, string mode)
        {
            _exercisesRepository.GetExercises(newShift, shiftId, buildingId, fireDrillTypes, exercises, tExercisesfiles, mode);
        }

        public List<TExercises> GetExerciseFiles(string buildingIds, int quarterno)
        {
            return _exercisesRepository.GetExerciseFiles(buildingIds, quarterno);
        }

        public DateTime getFirstdayofNextQuarter(DateTime myDate, int quarterNo)
        {
            return _exercisesRepository.getFirstdayofNextQuarter(myDate, quarterNo);
        }

        #endregion Exercises

        #region Exercise Settings

        public  int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster)
        {
            return _exercisesRepository.InsertQuarterMasterSettings(newQuarterMaster);
        }

        public  bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings)
        {
            return _exercisesRepository.UpdateQuarterMasterSettings(quarterMasterSettings);
        }

        //public  List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todaydrill)
        //{
        //    return _exercisesRepository.GetQuarterSettings(buildingTypeId, quarterNo, year, todaydrill);
        //}

        public  List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string sitecode)
        {
            return _exercisesRepository.GetQuarterMaster(buildingTypeId, quarterNo, year, buildingId, sitecode);
        }        

        #endregion

        #region Fire Drill common Questionnaires

        public  int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
        {
            return _exercisesRepository.InsertFiredrillQuestionnaries(newFireDrillQuestionnaires);
        }

        public  bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
        {
            return _exercisesRepository.UpdateFiredrillQuestionnaries(newFireDrillQuestionnaires);
        }

        public  List<FireDrillCategory> GetFiredrillQuestionnaries()
        {
            return _exercisesRepository.GetFiredrillQuestionnaries();
        }

        public  FireDrillQuestionnaires GetFiredrillQuestionnarie(int questionId)
        {
            return _exercisesRepository.GetFireDrillQuestionnaire(questionId);
        }

        #endregion

        #region TExerciseQuestionnaires

        public  int InsertTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires)
        {
            return _exercisesRepository.InsertTExerciseQuestionnaires(texerciseQuestionnaires);
        }

        public  int InsertTExerciseActions(TExerciseActions exerActions)
        {
            return _exercisesRepository.InsertTExerciseActions(exerActions);
        }

        public  bool DeleteTExerciseActionsbyTExerciseId(int texerciseId)
        {
            return _exercisesRepository.DeleteTExerciseActions(texerciseId, 0);
        }
        public  bool DeleteTExerciseActionsTExerciseActionId(int texerciseActionId)
        {
            return _exercisesRepository.DeleteTExerciseActions(0, texerciseActionId);
        }
        #endregion

        #region FireDrill Ques
        public  List<FireDrillCategory> GetFiredrillQues()
        {
            return _exercisesRepository.GetFiredrillQues();
        }


        public  FireDrillQuestionnaires GetFiredrillQues(int questionId)
        {
            return _exercisesRepository.GetFireDrillQuestion(questionId);
        }

        #endregion

        #region FireDrill Common Categ
        public  List<FireDrillCategory> GetCommonFireDrillCategory()
        {
            return _exercisesRepository.GetCommonFireDrillCategory();
        }

        public  int AddFireDrilCommonCategory(FireDrillCategory newFiredrillCategory)
        {
            return _exercisesRepository.Save(newFiredrillCategory);
        }
        #endregion

        #region FireDrill Categ
        public  List<FireDrillCategory> GetFireDrillCategory()
        {
            return _exercisesRepository.GetFireDrillCategory();
        }

        public  int AddFireDrilCategory(FireDrillCategory newFiredrillCategory)
        {
            return _exercisesRepository.Savefiredrill(newFiredrillCategory);
        }
        #endregion
    }
}
