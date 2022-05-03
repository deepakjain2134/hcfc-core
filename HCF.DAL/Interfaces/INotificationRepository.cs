using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface INotificationRepository
    {
       
        int AddNotificationEmails(NotificationEmails objectData);
      
        NotificationEmails GetNotification(int notiCatId, int? eventId, string mode, string id);
        List<NotificationCategory> GetNotificationCategory();
        List<NotificationEmails> GetNotificationEmails();
      
        NotificationEmails GetNotificationEmailsAddress(int catId, string BuildingId, string FloorId, string SiteID, int? EventId = 1);
     
        List<NotificationMapping> GetNotificationMatrix();
        List<NotificationCategory> GetNotificationMatrixByCategory();
        List<NotificationCategory> GetNotifications(string mode, string id);
        NotificationEmails GetNotifications(string catId, string eventId, string mode, string searchId);
        bool UpdateNotificationMatrix(NotificationMapping objNotificationMapping);
    }
}