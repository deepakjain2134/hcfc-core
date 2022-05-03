using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IExercisesRepository
    {
        int CreateExercises(TExercises newExercises);
        bool DeleteExercises(int exerciseId);
        bool DeleteTExerciseActions(int texerciseId, int texerciseActionId);
        List<FireDrillCategory> GetCommonFireDrillCategory();
        List<TExercises> GetExerciseFiles(int? exerciseId);
        List<TExercises> GetExerciseFiles(string buildingIds, int quarterno);
        List<TExercises> GetExercises(int? exerciseId);
        DateTime getFirstdayofNextQuarter(DateTime myDate, int quarterNo);
        void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles, string mode);
        List<FireDrillCategory> GetFireDrillCategory();
        List<FireDrillCategory> GetFiredrillQues();
        FireDrillQuestionnaires GetFireDrillQuestion(int questionId);
        FireDrillQuestionnaires GetFireDrillQuestionnaire(int questionId);
        List<FireDrillCategory> GetFiredrillQuestionnaries();
        List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string siteCode);
        //List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todayDrill);
        QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year, bool todaydrill);
        List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId);
        int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster);
        int InsertTExerciseActions(TExerciseActions newObject);
        int InsertTExerciseFiles(TExerciseFiles newExercises);
        int InsertTExerciseQuestionnaires(TExerciseQuestionnaires newObject);
        //void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterno);
        void PlanDrill(TExercises newExercises, int year, int quarterno);
        int Save(FireDrillCategory newFiredrillCategory);
        int Savefiredrill(FireDrillCategory newFiredrillCategory);
        bool UpdateExercises(TExercises newExercises);
        bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings);


    }
}