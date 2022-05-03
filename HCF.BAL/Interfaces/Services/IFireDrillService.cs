using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL.Interfaces
{
    public partial interface IFireDrillService
    {

        #region Fire Drills 

        int CreateExercises(TExercises newExercises);

        bool UpdateExercises(TExercises newExercises);

        List<TExercises> GetExercises(int? exerciseId);

        List<TExercises> GetExerciseFiles(int? exerciseId);

        bool DeleteExercises(int exerciseId);

        int InsertTExerciseFiles(TExerciseFiles exercise);

        List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId);

        void PlanDrill(TExercises newExercises, int year, int quarterNo);

        QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year, bool todaydrill = false);

        void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles);

        List<TExercises> GetExerciseFiles(string buildingIds, int quarterno);

        #endregion Fire Drills 

        #region TExercise Questionnaires

        int InsertTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires);

        #endregion TExercise Questionnaires

        #region TExercise Actions

        int InsertTExerciseActions(TExerciseActions exerActions);

        bool DeleteTExerciseActionsbyTExerciseId(int texerciseId);

        bool DeleteTExerciseActionsTExerciseActionId(int texerciseActionId);

        #endregion TExercise Actions

        //int AddFireDrilCategory(FireDrillCategory newFiredrillCategory);
        //int AddFireDrilCommonCategory(FireDrillCategory newFiredrillCategory);

        //List<FireDrillCategory> GetCommonFireDrillCategory();
      
        //List<FireDrillCategory> GetFireDrillCategory();
        //List<FireDrillCategory> GetFiredrillQues();
        //FireDrillQuestionnaires GetFiredrillQues(int questionId);
        //FireDrillQuestionnaires GetFiredrillQuestionnarie(int questionId);
        //List<FireDrillCategory> GetFiredrillQuestionnaries();
        //List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string sitecode);
        //List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todaydrill);


        //int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        //int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster);

        //

        //void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterNo);


        //bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires);
        //bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings);
    }
}