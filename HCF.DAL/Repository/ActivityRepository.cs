using HCF.BDO;
using HCF.DAL.Contexts;
using HCF.DAL.Interfaces;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL.Repository
{
   
    public class ActivityRepository: IActivityRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public ActivityRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public Activity Insert(Activity newActivity)
        {
            int newId = 0;
            string sql = StoredProcedures.Insert_RoundActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Data", newActivity.Data);
                command.Parameters.AddWithValue("@Timestamp", newActivity.Timestamp);
                command.Parameters.AddWithValue("@Type", newActivity.Type);
                command.Parameters.AddWithValue("@EffectiveDate", newActivity.EffectiveDate);
                command.Parameters.AddWithValue("@CreatedDate", newActivity.CreatedBy);
                command.Parameters.AddWithValue("@CreatedBy", newActivity.CreatedBy);
                command.Parameters.AddWithValue("@IsMailed", newActivity.IsMailed);
                command.Parameters.AddWithValue("@NotificationDate", newActivity.NotificationDate==null?DBNull.Value: newActivity.NotificationDate);
                command.Parameters.AddWithValue("@IsEnabled", newActivity.IsEnabled);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
      return newActivity;
        }
    }
}