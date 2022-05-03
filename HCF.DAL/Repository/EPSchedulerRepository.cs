using HCF.BDO;
using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class EPSchedulerRepository :IEPSchedulerRepository
    {
        
        private readonly ISqlHelper _sqlHelper;
        public EPSchedulerRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public  int Save(EPScheduler scheduler)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Scheduler;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CreatedBy", scheduler.CreatedBy);
                command.Parameters.AddWithValue("@Day", scheduler.Day);
                command.Parameters.AddWithValue("@EpDetaild", scheduler.EpDetailld);               
                command.Parameters.AddWithValue("@Month", scheduler.Month);
                command.Parameters.AddWithValue("@Year", scheduler.Year);               
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<EPScheduler> GetEpSchedulers()
        {
            var ePScheduler = new List<EPScheduler>();
            const string sql = StoredProcedures.Get_Scheduler;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;               
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                    ePScheduler = _sqlHelper.ConvertDataTable<EPScheduler>(ds.Tables[0]);
               
            }
            return ePScheduler;
        }
    }
}
