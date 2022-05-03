using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.Utility;
using System.Data.SqlClient;
using System.Data;


namespace HCF.DAL
{
    public class EpDocumentRepository:IEpDocumentRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public EpDocumentRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public int Save(EpDocuments epDocuments)
        {
            int newId;
            const string sql = StoredProcedures.Insert_EPDocument;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EpDetailId", epDocuments.EPDetailId);
                command.Parameters.AddWithValue("@DocTypeId", epDocuments.DocTypeId);
                command.Parameters.AddWithValue("@CreatedBy", epDocuments.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", epDocuments.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
