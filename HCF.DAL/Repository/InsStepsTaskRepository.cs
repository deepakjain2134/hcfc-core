using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class InsStepsTaskRepository : IInsStepsTaskRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public InsStepsTaskRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveObject"></param>
        /// <returns></returns>
        public  int Save(TInsStepsTask saveObject)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TInsStepsTask;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ActivityId", saveObject.ActivityId);
                command.Parameters.AddWithValue("@Comment", saveObject.Comment);
                command.Parameters.AddWithValue("@CreatedBy", saveObject.CreatedBy);
                command.Parameters.AddWithValue("@StepsId", saveObject.StepsId);
                command.Parameters.AddWithValue("@FileName", saveObject.FileName);
                command.Parameters.AddWithValue("@FilePath", saveObject.FilePath);
                command.Parameters.AddWithValue("@Status", saveObject.DeficiencyStatus);
                command.Parameters.AddWithValue("@TInsStepsId", saveObject.TInsStepsId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="tInsStepsId"></param>
        /// <returns></returns>
        private  List<TInsStepsTask> GetTInsStepsTask(Guid? activityId, int? tInsStepsId)
        {
            var lists = new List<TInsStepsTask>();
            const string sql = StoredProcedures.Get_TInsStepsTask;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@tInsStepsId", tInsStepsId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    lists = _sqlHelper.ConvertDataTable<TInsStepsTask>(dt);
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<TInsStepsTask> GetTInsStepsTask()
        {
            return GetTInsStepsTask(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public  List<TInsStepsTask> GetTInsStepsTask(Guid activityId)
        {
            return GetTInsStepsTask(activityId, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tInsStepsId"></param>
        /// <returns></returns>
        public  List<TInsStepsTask> GetTInsStepsTask(int tInsStepsId)
        {
            return GetTInsStepsTask(null, tInsStepsId);
        }
    }
}
