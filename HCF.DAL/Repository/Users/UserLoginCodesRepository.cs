using HCF.BDO;
using HCF.DAL.Interfaces;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL.Repository.Users
{
    public class UserLoginCodesRepository: IUserLoginCodesRepository
    {
        #region ctor

        private readonly ISqlHelper _sqlHelper;
        public UserLoginCodesRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #endregion

        public List<UserLoginCodes> GetUserLoginCodes(Guid orgKey)
        {
            var loginTokens = new List<UserLoginCodes>();
            const string sql = StoredProcedures.GetUserLoginCodes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orgKey", orgKey);
                var dt = _sqlHelper.GetCommonDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow item in dt.Tables[0].Rows)
                    {
                        var loginCodes = new UserLoginCodes
                        {
                            Code = Convert.ToString(item["code"].ToString()),
                            OrgKey = Guid.Parse(item["OrgKey"].ToString()),
                            IsUsed = Convert.ToBoolean(item["IsUsed"].ToString()),
                        };
                        loginTokens.Add(loginCodes);
                    }
                }
            }
            return loginTokens;
        }

        public int SaveUserLoginCodes(int noOfCodes, Guid orgKey, int createdBy,string codes)
        {
            int newId;
            string sql = StoredProcedures.SaveUserLoginCodes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@noOfCodes", noOfCodes);
                command.Parameters.AddWithValue("@orgKey", orgKey);
                command.Parameters.AddWithValue("@createdBy", createdBy);
                command.Parameters.AddWithValue("@codes", codes);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
