using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO;

using HCF.Utility;
using System.Text.RegularExpressions;

namespace HCF.DAL
{
    public class EmailSync : IEmailSync
    {
        private readonly ISqlHelper _sqlHelper;
       
        public EmailSync(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  List<PopEmail> GetMails()
        {
            List<PopEmail> lists = new List<PopEmail>();
            const string sql = StoredProcedures.Get_PopEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetCommonDataSet(command))
                {
                    lists = _sqlHelper.ConvertDataTable<PopEmail>(ds.Tables[0]);
                }
            }
            return lists.Where(x => x.IsActive).ToList();
        }

        private int SaveMailToDataBase(string MessageId, string Subject, string From, DateTime DateSent, List<Files> files, string body, string to, string cc, string bcc, string toEmails, string msgfilePath, string DbName, string ClientNo)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Mail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MessageId", MessageId);
                command.Parameters.AddWithValue("@Subject", Subject);
                command.Parameters.AddWithValue("@Sender", From);
                command.Parameters.AddWithValue("@to", toEmails);
                command.Parameters.AddWithValue("@Bcc", bcc);
                command.Parameters.AddWithValue("@Cc", cc);
                command.Parameters.AddWithValue("@Details", body);
                command.Parameters.AddWithValue("@ReceivedDate", DateSent);
                command.Parameters.AddWithValue("@EmailId", to);
                command.Parameters.AddWithValue("@MailFilePath", msgfilePath);
                command.Parameters.AddWithValue("@ClientNo", ClientNo);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public void SaveAttachments(int DocumentRepoId, List<EmailFiles> files, string DbName)
        {
            foreach (EmailFiles file in files.Where(x => x.fileName != null).ToList())
            {
                const string sql = StoredProcedures.Insert_Attachments;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FileName", RenameFileName(file.fileName));
                    command.Parameters.AddWithValue("@FilePath", file.filePath);
                    command.Parameters.AddWithValue("@DocumentRepoId", DocumentRepoId);
                }
            }
        }

        public string RenameFileName(string FileName)
        {
            string newFileName = "";
            newFileName = FileName.Trim();
            newFileName = Regex.Replace(newFileName, @"\s+", "_");
            return newFileName;
        }
    }
}
