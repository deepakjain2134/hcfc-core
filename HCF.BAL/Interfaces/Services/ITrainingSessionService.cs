using HCF.BDO.ModuleTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public interface ITrainingSessionService
    {
        int SaveOrgTrainingSession(OrgTrainingSessions orgTrainingSession);

        int SaveTrainingSession(TrainingSessionMaster trainingSession);

        IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo, int ModuleSessionId);

        IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo); 
        public bool UpdateData(TrainingSessionMaster trainingSession);
        public bool AddModuleTraining(TrainingSessionMaster model);
    }
}
