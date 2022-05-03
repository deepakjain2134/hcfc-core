using HCF.BDO.ModuleTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL
{
   public interface ITrainingSessionRepository
    {
        int SaveOrgTrainingSession(OrgTrainingSessions orgTrainingSession);

        int SaveTrainingSession(TrainingSessionMaster trainingSession);

        IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo, int ModuleSessionId);

        IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo);
      
        bool AddModuleTraining(TrainingSessionMaster trainingSession);

        bool UpdateData(TrainingSessionMaster trainingSession);
    }
}
