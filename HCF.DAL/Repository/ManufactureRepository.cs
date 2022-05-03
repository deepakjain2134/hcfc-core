using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public  class ManufactureRepository :IManufactureRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public ManufactureRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  List<Manufactures> GetManufactures()
        {
            var list = new List<Manufactures>();
            const string sql = StoredProcedures.Get_Manufacture;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    list = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[0]);
            }
            return list;
        }

        public  int Save(Manufactures newManufacture)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Manufacture;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ManufactureName", newManufacture.ManufactureName);
                command.Parameters.AddWithValue("@IsActive", newManufacture.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newManufacture.CreatedBy);
                command.Parameters.AddWithValue("@ManufactureId", newManufacture.ManufactureId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
