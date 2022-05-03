using HCF.Utility;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public ErrorRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public bool SaveError(Exception ex, string functionName)
        {
            bool status = false;
            const string sql = StoredProcedures.Insert_ErrorLog; // "dbo.CreateUser";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Message", ex.Message);
                command.Parameters.AddWithValue("@Source", functionName);
                command.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                if (ex.InnerException != null)
                    command.Parameters.AddWithValue("@InnerMessage", ex.InnerException.Message);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
    }
}
