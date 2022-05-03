using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL.Interfaces.Services
{
    public interface ICommonService
    {
        List<Audit> GetAuditConfiguration();
        TFiles GetFile(int fileId);
        TFiles GetFile(int fileId, string tbName);
       
        Organization GetMasterOrg(Guid orgKey);
      
        List<TFiles> GetRecentFiles(int userId);
       
        List<TFiles> GetTFiles(string files);
        List<DrawingpathwayFiles> GetUploadedDrawings(string files);
        bool IsSendMail(string Category, string Event);
        NotificationMapping MailNotification(string Category, string Event);
        int SaveDigitalFile(DigitalSignature objDigitalSignature, string folderName);
        TFiles SaveTFile(TFiles item, int uploadedBy, string folderName);
        List<TFiles> SaveTFiles(List<TFiles> tFiles, int uploadedBy);
        void SendConfigurationEmail();
        Task<bool> UpdateFileSize(TFiles item);
        List<TFiles> FindUploadeFiles(string key, string fileName, int userId);
    }
}