using HCF.BDO;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class DigitalSignRepository : IDigitalSignRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public DigitalSignRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDigitalSignature"></param>
        /// <returns></returns>
        public int AddDigitalSignature(DigitalSignature newDigitalSignature)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_Digitalsignature; 
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TConstId", newDigitalSignature.TConstId);
                command.Parameters.AddWithValue("@TRoundId", newDigitalSignature.TRoundId);
                command.Parameters.AddWithValue("@FileName", newDigitalSignature.FileName);
                command.Parameters.AddWithValue("@FilePath", newDigitalSignature.FilePath);
                command.Parameters.AddWithValue("@UserId", newDigitalSignature.UserId);
                command.Parameters.AddWithValue("@Signee", newDigitalSignature.Signee);
                command.Parameters.AddWithValue("@CreatedBy", newDigitalSignature.CreatedBy);
                command.Parameters.AddWithValue("@IsSignOverride", newDigitalSignature.IsSignOverride);
                //command.Parameters.AddWithValue("@LocalSignDateTime", (newDigitalSignature.LocalSignDateTime.ToString()== "1/1/0001 12:00:00 AM"?DateTime.Now: newDigitalSignature.LocalSignDateTime));
                command.Parameters.AddWithValue("@LocalSignDateTime", (newDigitalSignature.LocalSignDateTime != default(DateTime) ? newDigitalSignature.LocalSignDateTime : DateTime.Now));
                command.Parameters.AddWithValue("@DigSignatureId", newDigitalSignature.DigSignatureId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDigitalSignature"></param>
        /// <returns></returns>
        public bool UpdateDigitalSignature(DigitalSignature newDigitalSignature)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Update_Digitalsignature; 
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DigSignatureId", newDigitalSignature.DigSignatureId);
                command.Parameters.AddWithValue("@IsDeleted", newDigitalSignature.IsDeleted);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<DigitalSignature> GetDigitalSignature(int? tRoundId, string digSignatureId = null)
        {
            var signs = new List<DigitalSignature>();
            const string sql = Utility.StoredProcedures.Get_Digitalsignature;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@troundId", tRoundId);
                command.Parameters.AddWithValue("@DigSignatureId", digSignatureId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    signs = _sqlHelper.ConvertDataTable<DigitalSignature>(dt);
            }
            return signs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tRoundId"></param>
        /// <returns></returns>
        public DigitalSignature GetDigitalSignature(int tRoundId)
        {
            return GetDigitalSignature().FirstOrDefault(x => x.TRoundId == tRoundId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DigitalSignature> GetDigitalSignature(string digSignatureId = null)
        {
            return GetDigitalSignature(null, digSignatureId);
        }

        public int DeleteDigitalSign(int digSignatureId)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Delete_Digitalsignature; 
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DigSignatureId", digSignatureId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
