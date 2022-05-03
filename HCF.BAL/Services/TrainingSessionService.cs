using HCF.BAL.Interfaces.Services;
using HCF.BDO.ModuleTraining;
using HCF.DAL;
using HCF.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Services
{
    public class TrainingSessionService : ITrainingSessionService
    {
        private readonly ITrainingSessionRepository _trainingSessionRepository;
       
        public TrainingSessionService(ITrainingSessionRepository trainingSessionRepository)
        {
            _trainingSessionRepository = trainingSessionRepository;

        }
        public IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo)
        {
            return _trainingSessionRepository.GetTrainingSessions(ClientNo);
        }
        public IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo, int ModuleSessionId)
        {
            return _trainingSessionRepository.GetTrainingSessions(ClientNo, ModuleSessionId);
        }
        public int SaveOrgTrainingSession(OrgTrainingSessions orgTrainingSession)
        {
            return _trainingSessionRepository.SaveOrgTrainingSession(orgTrainingSession);
        }
        public int SaveTrainingSession(TrainingSessionMaster trainingSession)
        {
            return _trainingSessionRepository.SaveTrainingSession(trainingSession);
        }
        public bool UpdateData(TrainingSessionMaster trainingSession)
        {
            return _trainingSessionRepository.UpdateData(trainingSession);
        }
        public bool AddModuleTraining(TrainingSessionMaster model)
        {
            return _trainingSessionRepository.UpdateData(model);
        }    
    }
}