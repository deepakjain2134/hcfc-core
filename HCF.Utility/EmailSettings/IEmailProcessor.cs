using System.Collections.Generic;

namespace HCF.Utility
{
    public interface IEmailProcessor
    {
        bool SendMail(string to, string subject, string cc, string body, string from, List<string> attachments = null);
        bool SendMail(string to, string subject, string cc, string body, string from, byte[] fileBytes, string fileName);
    }
}