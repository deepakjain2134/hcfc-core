using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class StepsRepository: IStepsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUsersRepository _usersRepository;


        public StepsRepository(ISqlHelper sqlHelper, IUsersRepository usersRepository)
        {
            _sqlHelper = sqlHelper;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public  Steps Save(Steps step)
        {
            const string sql = StoredProcedures.Insert_Step; // "dbo.Insert_Step";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MainGoalId", step.MainGoalId);
                command.Parameters.AddWithValue("@Step", step.Step);
                command.Parameters.AddWithValue("@Sequence", step.Sequence);
                command.Parameters.AddWithValue("@IsActive", step.IsActive);
                command.Parameters.AddWithValue("@StepsId", step.StepsId);
                command.Parameters.AddWithValue("@CreatedBy", step.CreatedBy);
                command.Parameters.AddWithValue("@StepType", step.StepType);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                step.StepsId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return step;
        }

        /// <summary>
        /// get all steps 
        /// </summary>
        /// <returns></returns>
        public  List<Steps> GetAllSteps()
        {
            List<Steps> steps = new List<Steps>();
            const string sql = StoredProcedures.Get_Steps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    steps = _sqlHelper.ConvertDataTable<Steps>(dt);
            }

            return steps;
        }

        public  bool Update(Steps step)
        {
            bool status;
            const string sql = StoredProcedures.Update_Step;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IsActive", step.IsActive);
                command.Parameters.AddWithValue("@StepsId", step.StepsId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        

        public  List<Steps> GetSteps()
        {
            return GetAllSteps().Where(x => x.IsActive).ToList();
        }


        public  List<TComment> GetStepsComments(Guid activityId, int tIlsmId)
        {
            var stepscomments = new List<TComment>();
            List<UserProfile> userProfiles;
            const string sql = StoredProcedures.Get_StepsComment;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@TIlsmId ", tIlsmId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    stepscomments = _sqlHelper.ConvertDataTable<TComment>(ds.Tables[0]);
                    userProfiles = _usersRepository.ConvertToUser(ds.Tables[1]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    foreach (var item in stepscomments)
                    {
                        item.AddedDateTimeSpan = Conversion.ConvertToTimeSpan(item.AddedDate);
                        if (item.AddedBy > 0)
                        {
                            item.AddedByUserProfile = userProfiles.FirstOrDefault(x => x.UserId == item.AddedBy);
                        }
                    }
                }
            }

            return stepscomments;
        }

        /// <summary>
        /// get epsteps by mail goal 
        /// </summary>
        /// <param name="mainGoalId"></param>
        /// <returns></returns>
        public  List<Steps> GetSteps(int mainGoalId)
        {
            var steps = GetSteps().Where(x => x.MainGoalId == mainGoalId).ToList();
            return steps;
        }

        /// <summary>
        /// get steps Details by stepsId
        /// </summary>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public  Steps GetStep(int stepId)
        {
            var step = GetAllSteps().SingleOrDefault(x => x.StepsId == stepId);
            return step;
        }

        /// <summary>
        ///  get only ra steps
        /// </summary>
        /// <param name="isRaSteps"></param>
        /// <returns></returns>
        public  List<Steps> GetSteps(bool isRaSteps)
        {
            var steps = GetSteps().Where(x => x.IsRA).ToList();
            return steps;
        }

        /// <summary>
        /// get steps by main goal and ra steps only
        /// </summary>
        /// <param name="isRaSteps"></param>
        /// <returns></returns>
        //public  List<Steps> GetSteps(int mainGoalId, bool isRaSteps)
        //{
        //    List<Steps> steps = GetSteps().Where(x => x.IsRA && x.MainGoalId == mainGoalId).ToList();
        //    return steps;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Steps> SetTransSteps()
        {
            List<Steps> lists;

            const string sql = StoredProcedures.Get_Steps; // "dbo.Get_Steps";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<Steps>(dt);
                foreach (var item in lists)
                {
                    item.Status = -1;
                    item.Comments = "";
                    item.FilePath = "";
                    item.Ischecked = 0;
                    item.DRTime = 0;
                    item.IsCurrent = true;
                }
            }
            return lists;
        }

    }
}
