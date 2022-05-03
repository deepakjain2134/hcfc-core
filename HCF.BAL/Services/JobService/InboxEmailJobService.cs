//using HCF.BAL.Interfaces.Services;
//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text.RegularExpressions;
//using HCF.Utility;
//using Lucene.Net.Messages;

//namespace HCF.BAL.Services.JobService
//{
//    public partial class InboxEmailJobService
//    {

//        //private static readonly string Timer2 = System.Configuration.ConfigurationManager.AppSettings["timer"];
//        private static readonly string Count = Utility.AppSettings.Fetchmsgcount; //System.Configuration.ConfigurationManager.AppSettings["fetchmsgcount"];
//        private static readonly string FileBaseUrl = Utility.AppSettings.AWSAccessKey; //System.Configuration.ConfigurationManager.AppSettings["AWSbaseurl"];

//        private readonly ILoggingService _loggingService;

//        public InboxEmailJobService(ILoggingService loggingService)
//        {
//            _loggingService = loggingService;
//        }

//        public void FetchMail()
//        {
//            var emails = GetMails();
//            var clientEmail = emails.Where(x => x.IsActive && x.ClientNo > 0).ToList();
//            foreach (var email in emails.Where(x => x.IsActive && x.ClientNo == null))
//            {
//                try
//                {
//                    SaveMails(email, clientEmail);
//                }
//                catch (Exception ex)
//                {
//                    _loggingService.Error(ex);
//                    //this.EventLog.WriteEntry("FetchMail for " + email.EmailId + " : " + ex.Message);
//                }
//            }
//        }

//        private List<PopEmails> GetMails()
//        {
//            var lists = new List<PopEmails>();
//            string query = "Get_PopEmails";
//            string sqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["CommonConnection"].ConnectionString; //System.Configuration.ConfigurationManager.AppSettings["CommonConnection"];
//            using (SqlConnection conn = new SqlConnection(sqlConnection))
//            {
//                using (var cmd = new SqlCommand(query, conn))
//                {
//                    conn.Open();
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    using (var dataAdapter = new SqlDataAdapter(cmd))
//                    {
//                        using (var dataTable = new DataTable())
//                        {

//                            dataAdapter.Fill(dataTable);
//                            foreach (DataRow row in dataTable.Rows)
//                            {
//                                var list = new PopEmails();
//                                list.EmailId = row["EmailId"].ToString();
//                                list.Password = row["Password"].ToString();
//                                list.PopServer = row["PopServer"].ToString();
//                                list.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
//                                list.DbName = row["DbName"].ToString();
//                                if (!string.IsNullOrEmpty(row["ClientNo"].ToString()))
//                                {
//                                    list.ClientNo = Convert.ToInt32(row["ClientNo"].ToString());
//                                }
//                                list.UseSSL = Convert.ToBoolean(row["UseSSL"].ToString());
//                                list.Port = Convert.ToInt32(row["Port"].ToString());
//                                lists.Add(list);
//                            }
//                        }
//                    }
//                }
//            }
//            return lists.Where(x => x.IsActive).ToList();
//        }

//        private void SaveMails(PopEmails email, List<PopEmails> clientEmails)
//        {
//            string pop3Server = email.PopServer;
//            string emailAddress = email.EmailId;
//            string password = email.Password;

//            var client = new Pop3Client();
//            try
//            {
//                // List<string> seenUids = new List<string>();
//                client.Connect(pop3Server, email.Port, email.UseSSL); //UseSSL true or false
//                client.Authenticate(emailAddress, password);
//                var messageCount = client.GetMessageCount();
//                //var Messages = new List<Message>(messageCount);
//                int counter = 0;
//                for (int i = messageCount; i >= 1; i--)
//                {
//                    Message message = client.GetMessage(i);
//                    SaveClientMail(message, emailAddress, clientEmails);
//                    //new Thread(delegate ()
//                    //{

//                    //    SaveClientMail(message, emailAddress, clientEmails);
//                    //}).Start();
//                    counter++;
//                    if (counter > Convert.ToInt32(Count))
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                _loggingService.Error("Fetch Mail for " + email.EmailId + " ->  " + ex.Message);
//            }
//            finally
//            {
//                if (client.Connected)
//                    client.Dispose();
//            }
//        }

//        private void SaveClientMail(Message msg, string to, List<PopEmails> allEmails)
//        {
//            var emailList = msg.Headers.Cc.Select(eMailAddress => eMailAddress.Address).ToList();
//            emailList.AddRange(msg.Headers.To.Select(eMailAddress => eMailAddress.Address));
//            emailList.AddRange(msg.Headers.Bcc.Select(eMailAddress => eMailAddress.Address));


//            //var allTos = string.Join(",", emailLis);

//            var clientEmails = allEmails
//                .Where(t => emailList.Contains(t.EmailId))
//                .ToList();

//            foreach (var clientEmail in clientEmails)
//            {
//                //Console.WriteLine(clientEmail.EmailId);
//                SaveMail(msg, to, clientEmail);
//            }
//        }

//        private void SaveMail(Message msg, string to, PopEmails emails)
//        {
//            string folderName = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
//            string bucketName = System.Configuration.ConfigurationManager.AppSettings["BucketName"];
//            var files = new List<Files>();
//            //string msgfilePath = string.Format(@"\{0}\{1}\mail.eml", to, msg.Headers.MessageId);
//            //FileInfo msgfile = new FileInfo(folderName + msgfilePath); 
//            string cc = "";
//            if (msg.Headers.Cc.Any())
//                cc = string.Join(",", msg.Headers.Cc.Select(x => x.Address));

//            string bcc = "";
//            if (msg.Headers.Bcc.Any())
//                bcc = string.Join(",", msg.Headers.Bcc.Select(x => x.Address));

//            string toEmails = "";
//            if (msg.Headers.To.Any())
//                toEmails = string.Join(",", msg.Headers.To.Select(x => x.Address));

//            //List<PopEmails> Images = clientEmails
//            //    .Where(t => t.EmailId.Contains(allTos))
//            //    .ToList(); 

//            var body = msg.FindAllTextVersions()[0].GetBodyAsText();

//            int documentRepoId = SaveMailToDataBase(msg.Headers.MessageId, msg.Headers.Subject, msg.Headers.From.Address, msg.Headers.DateSent.ToUniversalTime(), body, to, cc, bcc, toEmails, "", emails.DbName, emails.ClientNo.ToString());
//            if (documentRepoId > 0)
//            {
//                var status = AmazonFileUpload.CreateBucket(emails.ClientNo.ToString());
//                foreach (var attachment in msg.FindAllAttachments())
//                {
//                    var file = new Files { MessageId = msg.Headers.MessageId };
//                    if (attachment.ContentDisposition != null && attachment.ContentDisposition.DispositionType == "attachment")
//                    {
//                        file.FileName = RenameFileName(attachment.FileName);
//                        var msgHeader = getUniqueHeaderValue(msg.Headers.MessageId); //msg.Headers.MessageId
//                        var filePath = $"{folderName}/{msgHeader}/{file.FileName}"; //System.IO.Path.Combine(folderName, to + "/" + msg.Headers.MessageId);  
//                        try
//                        {
//                            //string filePath = Path.Combine(pathString, RenameFileName(attachment.FileName));                              
//                            if (status)
//                            {
//                                string filesContent = Convert.ToBase64String(attachment.Body);
//                                AmazonFileUpload.SaveFile(bucketName, filesContent, filePath, file.FileName, emails.ClientNo.ToString());
//                                file.FilePath = filePath;
//                                files.Add(file);
//                                if (msg.Headers.Subject.ToLower().Contains("Siemens".ToLower()))
//                                {
//                                    SaveSiemensdeficiencientAssets(filePath, emails.ClientNo.ToString(), emails.DbName);
//                                }
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            _loggingService.Error(ex);
//                        }
//                    }
//                }
//                SaveAttachments(documentRepoId, files, emails.DbName);
//            }
//        }

//        private string getUniqueHeaderValue(string messageId)
//        {
//            string[] messageHeader = messageId.Split('@');
//            var uniqueCode = messageHeader.Length > 0 ? messageHeader[0] : Guid.NewGuid().ToString();
//            return uniqueCode;
//        }

//        private void SaveSiemensdeficiencientAssets(string filePath, string clientNo, string dbName)
//        {
//            var stringPath = $"{FileBaseUrl}/{clientNo}/{filePath}";
//            Siemens.SaveData.SaveRecords(stringPath.ToLower(), dbName);
//        }

//        private void SaveAttachments(int documentRepoId, List<Files> files, string dbName)
//        {
//            foreach (var file in files.Where(x => x.FileName != null).ToList())
//            {
//                var query = "Insert_Attachments";
//                var sqlConnection = string.Format(System.Configuration.ConfigurationManager.ConnectionStrings["CommonConnection"].ConnectionString, dbName);
//                using (var conn = new SqlConnection(sqlConnection))
//                {
//                    using (var cmd = new SqlCommand(query, conn))
//                    {
//                        conn.Open();
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@FileName", RenameFileName(file.FileName));
//                        cmd.Parameters.AddWithValue("@FilePath", file.FilePath);
//                        cmd.Parameters.AddWithValue("@DocumentRepoId", documentRepoId);
//                        cmd.ExecuteNonQuery();
//                    }
//                }
//            }
//        }

//        private string RenameFileName(string fileName)
//        {
//            var newFileName = fileName.Trim();
//            newFileName = Regex.Replace(newFileName, @"\s+", "_");
//            return newFileName;
//        }

//        private int SaveMailToDataBase(string messageId, string subject, string @from, DateTime dateSent, string body, string to, string cc, string bcc, string toEmails, string msgfilePath, string dbName, string clientNo)
//        {
//            var documentRepoId = 0;
//            try
//            {
//                var query = "Insert_Mail";
//                var sqlConnection = string.Format(System.Configuration.ConfigurationManager.ConnectionStrings["CommonConnection"].ConnectionString, dbName);
//                using (var conn = new SqlConnection(sqlConnection))
//                {
//                    using (var cmd = new SqlCommand(query, conn))
//                    {
//                        conn.Open();
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.AddWithValue("@MessageId", messageId);
//                        cmd.Parameters.AddWithValue("@Subject", subject);
//                        cmd.Parameters.AddWithValue("@Sender", @from);
//                        cmd.Parameters.AddWithValue("@to", toEmails);
//                        cmd.Parameters.AddWithValue("@Bcc", bcc);
//                        cmd.Parameters.AddWithValue("@Cc", cc);
//                        cmd.Parameters.AddWithValue("@Details", body);
//                        cmd.Parameters.AddWithValue("@ReceivedDate", dateSent);
//                        cmd.Parameters.AddWithValue("@EmailId", to);
//                        cmd.Parameters.AddWithValue("@MailFilePath", msgfilePath);
//                        cmd.Parameters.AddWithValue("@ClientNo", clientNo);
//                        cmd.Parameters.Add("@newId", SqlDbType.Int);
//                        cmd.Parameters["@newId"].Direction = ParameterDirection.Output;
//                        cmd.ExecuteNonQuery();
//                        documentRepoId = Convert.ToInt32(cmd.Parameters["@newId"].Value);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _loggingService.Error(ex);
//            }
//            return documentRepoId;
//        }
//    }
//}