using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public class SubStatusRepository : ISubStatusRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public SubStatusRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public int Save(BDO.SubStatus substatus)
        {
            int newId;
            const string sql = StoredProcedures.Insert_subStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SubStatusId", substatus.SubStatusId);
                command.Parameters.AddWithValue("@WoStatusId", substatus.WoStatusId);
                command.Parameters.AddWithValue("@Name", substatus.Name);
                command.Parameters.AddWithValue("@Code", substatus.Code);
                command.Parameters.AddWithValue("@Description", substatus.Description);
                command.Parameters.AddWithValue("@IsActive", substatus.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<BDO.SubStatus> GetSubStatus()
        {
            List<HCF.BDO.SubStatus> lists = new List<BDO.SubStatus>();
            const string sql = StoredProcedures.Get_SubStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<HCF.BDO.SubStatus>(dt);
            }
            return lists;
        }
    }
}
