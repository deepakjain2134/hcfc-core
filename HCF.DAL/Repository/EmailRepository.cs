using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class EmailRepository : IEmailRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly HCF.Utility.IHCFSession _hCFSession;

        public EmailRepository(ISqlHelper sqlHelper, IHCFSession hCFSession)
        {
            _sqlHelper = sqlHelper;
            _hCFSession = hCFSession;
        }


        public  bool SaveEmail(string domain, string primaryKeyValue, string recipient, string subject, string body, string sentBy)
        {
            const string sql = "dbo.CreateEmail";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Domain", domain);
                command.Parameters.AddWithValue("@PKValue", primaryKeyValue);
                command.Parameters.AddWithValue("@EmailAddress", recipient);
                command.Parameters.AddWithValue("@Subject", subject);
                command.Parameters.AddWithValue("@Message", body);
                command.Parameters.AddWithValue("@SentBy", sentBy);

                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<PopEmails> GetEmails()
        {
            List<PopEmails> emails = new List<PopEmails>();
            string sql = StoredProcedures.Get_PopEmails;//
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    emails = _sqlHelper.ConvertDataTable<PopEmails>(dt);
            }
            return emails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailSetting"></param>
        /// <returns></returns>
        public  bool SaveEmail(PopEmails mailSetting)
        {
            const string sql = StoredProcedures.Save_PopEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailId", mailSetting.EmailId);
                command.Parameters.AddWithValue("@Password", mailSetting.Password);
                command.Parameters.AddWithValue("@PopServer", mailSetting.PopServer);
                command.Parameters.AddWithValue("@IsActive", mailSetting.IsActive);
                command.Parameters.AddWithValue("@Id", mailSetting.Id);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }


        public  System.Guid? SaveEmailGuid(string to, string ccc, string subject, string body, string sentBy)
        {
            try
            {
                const string sql = StoredProcedures.Insert_OutgoingEmails;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@to", to);
                    command.Parameters.AddWithValue("@ccc", ccc);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@Message", body);
                    command.Parameters.AddWithValue("@SentBy", sentBy);
                    DataTable dt = _sqlHelper.GetDataTable(command);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)                       
                            return System.Guid.Parse(dt.Rows[0][0].ToString());                      
                       
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
                return null;
            }
        }


        public  void UpdateSentMail(Guid outgoingMailId)
        {
            const string sql = StoredProcedures.Insert_OutgoingEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@outgoingMailId", outgoingMailId);
                _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public  int SaveEmailSettings(PopEmails emails)
        {
            int newId = 0;
            const string sql = StoredProcedures.Insert_InboxEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmailId", emails.EmailId);
                command.Parameters.AddWithValue("@PopServer", emails.PopServer);
                command.Parameters.AddWithValue("@Password", emails.Password);
                command.Parameters.AddWithValue("@IsActive", emails.IsActive);
                command.Parameters.AddWithValue("@Port", emails.Port);
                command.Parameters.AddWithValue("@UseSSL", emails.UseSSL);
                command.Parameters.AddWithValue("@ClientNo", emails.ClientNo);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public List<PopEmails> GetInboxEmails()
        {
            var emails = new List<PopEmails>();
            string sql = StoredProcedures.Get_PopEmails;//
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null)
                    emails = _sqlHelper.ConvertDataTable<PopEmails>(dt);
            }
            return emails.Where(x => x.ClientNo ==Convert.ToInt32(_hCFSession.ClientNo)).ToList();
        }

        public Guid? SaveEmailGuid(string to, string ccc, string subject, string body, string sentBy, int value = 0)
        {
            throw new NotImplementedException();
        }

        public Guid? SaveEmail(string to, string ccc, string subject, string body, string sentBy)
        {
            try
            {
                const string sql = StoredProcedures.Insert_OutgoingEmails;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@to", to);
                    command.Parameters.AddWithValue("@ccc", ccc);
                    command.Parameters.AddWithValue("@Subject", subject);
                    command.Parameters.AddWithValue("@Message", body);
                    command.Parameters.AddWithValue("@SentBy", sentBy);
                    DataTable dt = _sqlHelper.GetDataTable(command);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                            return System.Guid.Parse(dt.Rows[0][0].ToString());

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
               // ErrorLog.LogError(ex);
                return null;
            }
        }
    }
}
