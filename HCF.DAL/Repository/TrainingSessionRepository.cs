using HCF.BDO.ModuleTraining;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class TrainingSessionRepository : ITrainingSessionRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public TrainingSessionRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo, int ModuleSessionId)
        {
            var lists = new List<TrainingSessionMaster>();
            var sessions = new List<OrgTrainingSessions>();
            string sql = StoredProcedures.Get_TrainingSessionsDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientNo", ClientNo);
                command.Parameters.AddWithValue("@ModuleSessionId", ModuleSessionId);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<TrainingSessionMaster>(ds.Tables[0]);
                    //var sessions = _sqlHelper.ConvertDataTable<OrgTrainingSessions>(ds.Tables[1]);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        OrgTrainingSessions session = new OrgTrainingSessions
                        {
                            Id = Convert.ToInt32(item["Id"].ToString()),
                            ModuleSessionId = Convert.ToInt32(item["ModuleSessionId"].ToString()),
                            OrgKey = Guid.Parse(item["OrgKey"].ToString()),
                            Date = Convert.ToDateTime(item["Date"].ToString()),
                            Participants = item["Participants"].ToString(),
                            IsCurrent = Convert.ToBoolean(item["IsCurrent"].ToString()),
                            Comments = item["Comments"].ToString(),
                            CreatedBy = Convert.ToInt32(item["CreatedBy"].ToString()),
                            CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString()),
                            Status = (HCF.BDO.Enums.TrainingSessionStatus)Enum.Parse(typeof(HCF.BDO.Enums.TrainingSessionStatus), item["Status"].ToString()),
                           

                        };
                        sessions.Add(session);
                    }
                    foreach (var org in lists)
                    {
                       org.TrainingSessions =  sessions.Where(x => x.ModuleSessionId == org.ModuleSessionId).ToList();
                    }
                }
            }
            return lists;
        }    
        public IEnumerable<TrainingSessionMaster> GetTrainingSessions(int ClientNo)
        {
            var lists = new List<TrainingSessionMaster>();
            var sessions = new List<OrgTrainingSessions>();
            string sql = StoredProcedures.Get_TrainingSessionsDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientNo", ClientNo);
                command.Parameters.AddWithValue("@ModuleSessionId", DBNull.Value); 
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<TrainingSessionMaster>(ds.Tables[0]);
                    //var sessions = _sqlHelper.ConvertDataTable<OrgTrainingSessions>(ds.Tables[1]);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        OrgTrainingSessions session = new OrgTrainingSessions
                        {
                            Id = Convert.ToInt32(item["Id"].ToString()),
                            ModuleSessionId = Convert.ToInt32(item["ModuleSessionId"].ToString()),
                            OrgKey = Guid.Parse(item["OrgKey"].ToString()),
                            Date = Convert.ToDateTime(item["Date"].ToString()),
                            Participants = item["Participants"].ToString(),
                            IsCurrent = Convert.ToBoolean(item["IsCurrent"].ToString()),
                            Comments = item["Comments"].ToString(),
                            CreatedBy = Convert.ToInt32(item["CreatedBy"].ToString()),
                            CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString()),
                            Status = (HCF.BDO.Enums.TrainingSessionStatus)Enum.Parse(typeof(HCF.BDO.Enums.TrainingSessionStatus), item["Status"].ToString()),
                        };
                        sessions.Add(session);
                    }
                        foreach (var org in lists)
                    {
                        org.TrainingSessions = sessions.Where(x => x.ModuleSessionId == org.ModuleSessionId).ToList();
                       
                    }
                }
            }
            return lists;
        }
        public bool AddModuleTraining(TrainingSessionMaster trainingSession)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Update_TrainingSessionMaster;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@ModuleSessionId", trainingSession.ModuleSessionId);
                command.Parameters.AddWithValue("@Name", trainingSession.Name);
                command.Parameters.AddWithValue("@ShortDescription", trainingSession.ShortDescription);
                command.Parameters.AddWithValue("@Description", trainingSession.Description);
                command.Parameters.AddWithValue("@CreatedBy", trainingSession.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", trainingSession.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return (newId > 0);
        }

        public bool UpdateData(TrainingSessionMaster trainingSession)
        { 
             int newId;
             const string sql = Utility.StoredProcedures.Insert_Update_TrainingSessionMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ModuleSessionId", trainingSession.ModuleSessionId);
                command.Parameters.AddWithValue("@Name", trainingSession.Name);
                command.Parameters.AddWithValue("@ShortDescription", trainingSession.ShortDescription);
                command.Parameters.AddWithValue("@Description", trainingSession.Description);       
                command.Parameters.AddWithValue("@CreatedBy", trainingSession.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", trainingSession.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
             return (newId > 0);
        }

        public int SaveOrgTrainingSession(OrgTrainingSessions orgTrainingSession)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_Update_OrgTrainingSessions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ModuleSessionId", orgTrainingSession.ModuleSessionId);
                command.Parameters.AddWithValue("@id", orgTrainingSession.Id);
                command.Parameters.AddWithValue("@ClientNo", orgTrainingSession.ClientNo);
                command.Parameters.AddWithValue("@Date",orgTrainingSession.Date);
                command.Parameters.AddWithValue("@Status", orgTrainingSession.Status.ToString());
                command.Parameters.AddWithValue("@Participants", orgTrainingSession.Participants);
                command.Parameters.AddWithValue("@Comments", orgTrainingSession.Comments);
                command.Parameters.AddWithValue("@CreatedBy", orgTrainingSession.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return (newId > 0) ? 1 : -1;

        }

        public int SaveTrainingSession(TrainingSessionMaster trainingSession)
        {
            throw new NotImplementedException();
        }
    }
}
