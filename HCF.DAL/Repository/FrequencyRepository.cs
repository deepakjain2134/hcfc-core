using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class FrequencyRepository : IFrequencyRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public FrequencyRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<FrequencyMaster> GetFrequency()
        {
            List<FrequencyMaster> lists = new List<FrequencyMaster>();
            const string sql = StoredProcedures.Get_FrequencyMaster; // "dbo.Get_FrequencyMaster";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Days"].ToString()))
                        {                            
                            var list = new FrequencyMaster
                            {
                                Days = Convert.ToInt32(row["Days"].ToString()),
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                DisplayName = row["DisplayName"].ToString(),
                                GracePeriod = Convert.ToInt32(row["GracePeriod"].ToString()),
                                IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                                Type = row["DisplayName"].ToString()
                            };
                            lists.Add(list);
                        }
                    }
                }
            }
            return lists;
        }

        public  bool Update(FrequencyMaster frequencyMaster)
        {
            bool status = true;
            const string sql = StoredProcedures.Insert_FrequencyMaster; // "dbo.CreateUser";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FrequencyId", frequencyMaster.FrequencyId);
                command.Parameters.AddWithValue("@DisplayName", frequencyMaster.DisplayName);
                command.Parameters.AddWithValue("@GracePeriod", frequencyMaster.GracePeriod);
                command.Parameters.AddWithValue("@Type", frequencyMaster.Type);
                command.Parameters.AddWithValue("@Days", frequencyMaster.Days);
                command.Parameters.AddWithValue("@IsActive", frequencyMaster.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                status = _sqlHelper.CommonExecuteNonQuery(command);
            }

            return status;
        }

        public  int Save(FrequencyMaster frequencyMaster)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FrequencyMaster; // "dbo.CreateUser";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FrequencyId", frequencyMaster.FrequencyId);
                command.Parameters.AddWithValue("@DisplayName", frequencyMaster.DisplayName);
                command.Parameters.AddWithValue("@GracePeriod", frequencyMaster.GracePeriod);
                command.Parameters.AddWithValue("@Days", frequencyMaster.Days);
                command.Parameters.AddWithValue("@Version",1.0 );
                command.Parameters.AddWithValue("@Value", 1);
                command.Parameters.AddWithValue("@Type", frequencyMaster.Type);
                command.Parameters.AddWithValue("@CreatedBy", frequencyMaster.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", frequencyMaster.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public  List<FrequencyMaster> GetEpFrequency(int epDetailId)
        {
            List<FrequencyMaster> list = new List<FrequencyMaster>();
            const string sql = StoredProcedures.Get_EPFrequency;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epDetailId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    list = _sqlHelper.ConvertDataTable<FrequencyMaster>(dt);
            }
            return list;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        public  FrequencyMaster GetFrequency(int frequencyId)
        {
            return GetFrequency().FirstOrDefault(x => x.FrequencyId == frequencyId);
        }
    }
}
