using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IExercisesService
    {
        int AddFireDrilCategory(FireDrillCategory newFiredrillCategory);
        int AddFireDrilCommonCategory(FireDrillCategory newFiredrillCategory);
        int CreateExercises(TExercises newExercises);
        bool DeleteExercises(int exerciseId);
        bool DeleteTExerciseActionsbyTExerciseId(int texerciseId);
        bool DeleteTExerciseActionsTExerciseActionId(int texerciseActionId);
        List<FireDrillCategory> GetCommonFireDrillCategory();
        List<TExercises> GetExerciseFiles(int? exerciseId);
        DateTime getFirstdayofNextQuarter(DateTime myDate, int quarterNo);
        List<TExercises> GetExercises(int? exerciseId);
        void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles, string mode);
        List<FireDrillCategory> GetFireDrillCategory();
        List<FireDrillCategory> GetFiredrillQues();
        FireDrillQuestionnaires GetFiredrillQues(int questionId);
        FireDrillQuestionnaires GetFiredrillQuestionnarie(int questionId);
        List<FireDrillCategory> GetFiredrillQuestionnaries();
        List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string sitecode);
        //List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todaydrill);
        QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year, bool todaydrill = false);
        List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId);
        int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster);
        int InsertTExerciseActions(TExerciseActions exerActions);
        int InsertTExerciseFiles(TExerciseFiles exercise);
        int InsertTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires);
        //void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterNo);
        void PlanDrill(TExercises newExercises, int year, int quarterNo);
        List<TExercises> GetExerciseFiles(string buildingIds, int quarterno);
        bool UpdateExercises(TExercises newExercises);
        bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings);
    }
}