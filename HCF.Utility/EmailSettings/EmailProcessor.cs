using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Linq;
using System.Threading;
using HCF.Utility.EmailSettings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace HCF.Utility
{
    public class EmailProcessor : IEmailProcessor
    {
        private readonly ILogger<EmailProcessor> _logger;
        private readonly MailSettings _mailSettings;
        static MemoryStream _ms = new MemoryStream();
     

        public EmailProcessor(ILogger<EmailProcessor> logger, IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;           
        }


        public bool SendMail(string to, string subject, string cc, string body, string from, List<string> attachments = null)
        {
            var mail = new MailMessage();

            if (!string.IsNullOrEmpty(from))
                mail.From = new MailAddress(from);

            string[] toMailIds = !string.IsNullOrEmpty(to) ? to.Split(',') : null;

            if(toMailIds!=null)
            {
                foreach (var item in toMailIds)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.To.Add(item);
                }
            }
            

            if (!string.IsNullOrEmpty(cc))
            {
                string[] mailIds = cc.Split(',');
                foreach (var item in mailIds)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.CC.Add(item);
                }
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            if (attachments != null && attachments.Count > 0)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment.StartsWith("http://") || attachment.StartsWith("https://"))
                    {
                        var fileAttachment = GetAttachments(attachment);
                        if (fileAttachment != null)
                            mail.Attachments.Add(fileAttachment);
                    }
                    else
                    {
                        var newFile = new FileInfo(attachment);
                        mail.Attachments.Add(new Attachment(newFile.FullName));
                    }
                }
            }
            SendEmailInBackgroundThread(mail);
            return true;
        }

        public bool SendMail(string to, string subject, string cc, string body, string from, byte[] fileBytes, string fileName)
        {
            var mail = new MailMessage();

            if (!string.IsNullOrEmpty(from))
                mail.From = new MailAddress(from);

            string[] toMailIds = to.Split(',');
            foreach (var item in toMailIds)
            {
                if (!string.IsNullOrEmpty(item))
                    mail.To.Add(item);
            }

            if (!string.IsNullOrEmpty(cc))
            {
                string[] mailIds = cc.Split(',');
                foreach (var item in mailIds)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.CC.Add(item);
                }
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            var attachment = new Attachment(new MemoryStream(fileBytes), fileName);
            mail.Attachments.Add(attachment);
            SendEmailInBackgroundThread(mail);
            return true;
            //return SendSync(mail);
        }

        private void SendSync(object mailMsg)
        {
            MailMessage mail = (MailMessage)mailMsg;
            try
            {
                // var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                var username = _mailSettings.Mail; //smtpSection.Network.UserName;
                var password = _mailSettings.Password;// smtpSection.Network.Password;

                mail.From = new MailAddress(_mailSettings.Mail);

                var smtpServer = new SmtpClient
                {
                    Host = _mailSettings.Host, //smtpSection.Network.Host,
                    Port = _mailSettings.Port, //smtpSection.Network.Port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = _mailSettings.EnableSsl //smtpSection.Network.EnableSsl
                };

                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // ErrorLog.LogError(ex);
                // status =false;
            }
        }

        private Attachment GetAttachments(string url)
        {
            var filename = url.Split('/').Last();
            Attachment attachment;
            try
            {
                _ms = new MemoryStream();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse httpWResp = (HttpWebResponse)req.GetResponse())
                using (Stream responseStream = httpWResp.GetResponseStream())
                    if (responseStream != null)
                        responseStream.CopyTo(_ms);
                _ms.Seek(0, SeekOrigin.Begin);
                attachment = new Attachment(_ms, filename, MediaTypeNames.Application.Octet);
                return attachment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        private void SendEmailInBackgroundThread(MailMessage mailMessage)
        {
            var bgThread = new Thread(new ParameterizedThreadStart(SendSync))
            {
                IsBackground = true
            };
            bgThread.Start(mailMessage);
        }
    }
}