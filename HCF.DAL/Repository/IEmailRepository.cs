//using System;
//using System.Collections.Generic;
//using HCF.BDO;

//namespace HCF.DAL
//{
//    public interface IEmailRepository
//    {
//        List<PopEmails> GetEmails();
//        List<PopEmails> GetInboxEmails();
//        bool SaveEmail(PopEmails mailSetting);
//        Guid? SaveEmailGuid(string to, string ccc, string subject, string body, string sentBy, int value=0);
//        bool SaveEmail(string domain, string primaryKeyValue, string recipient, string subject, string body, string sentBy);
//        int SaveEmailSettings(PopEmails emails);
//        void UpdateSentMail(Guid outgoingMailId);
//    }
//}