using HCF.BAL.Interfaces;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Services
{
    public class FireDrillService : IFireDrillService
    {
        private readonly IExercisesRepository _exercisesRepository;
        public FireDrillService(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository;
        }

       

        #region Fire Drills
        public int CreateExercises(TExercises newExercises)
        {
            return _exercisesRepository.CreateExercises(newExercises);
        }

        public bool UpdateExercises(TExercises newExercises)
        {
            return _exercisesRepository.UpdateExercises(newExercises);
        }

        public List<TExercises> GetExercises(int? exerciseId)
        {
            return _exercisesRepository.GetExercises(exerciseId);
        }

        public List<TExercises> GetExerciseFiles(int? exerciseId)
        {
            return _exercisesRepository.GetExerciseFiles(exerciseId);
        }

        public bool DeleteExercises(int exerciseId)
        {
            return _exercisesRepository.DeleteExercises(exerciseId);
        }

        public int InsertTExerciseFiles(TExerciseFiles exercise)
        {
            return _exercisesRepository.InsertTExerciseFiles(exercise);
        }

        public List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId)
        {
            return _exercisesRepository.GetTExerciseQuestionnaires(exerciseId);
        }

        public void PlanDrill(TExercises newExercises, int year, int quarterNo)
        {
            _exercisesRepository.PlanDrill(newExercises, year, quarterNo);
        }

        public QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year, bool todaydrill = false)
        {
            return _exercisesRepository.GetQuarterSettings(buildingIds, locationGroupIds, mode, year, todaydrill);
        }

        public void GetExercises(Shift newShift, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles)
        {
            _exercisesRepository.GetExercises(newShift, shiftId, buildingId, fireDrillTypes, exercises, tExercisesfiles);
        }

        public List<TExercises> GetExerciseFiles(string buildingIds, int quarterno)
        {
            return _exercisesRepository.GetExerciseFiles(buildingIds, quarterno);
        }

        #endregion Fire Drills

        #region TExercise Questionnaires

        public int InsertTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires)
        {
            return _exercisesRepository.InsertTExerciseQuestionnaires(texerciseQuestionnaires);
        }

        #endregion TExercise Questionnaires

        #region TExercise Actions

        public int InsertTExerciseActions(TExerciseActions exerActions)
        {
            return _exercisesRepository.InsertTExerciseActions(exerActions);
        }

        public bool DeleteTExerciseActionsbyTExerciseId(int texerciseId)
        {
            return _exercisesRepository.DeleteTExerciseActions(texerciseId, 0);
        }
        public bool DeleteTExerciseActionsTExerciseActionId(int texerciseActionId)
        {
            return _exercisesRepository.DeleteTExerciseActions(0, texerciseActionId);
        }

        #endregion TExercise Actions
    }
}
