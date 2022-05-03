using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface INotificationService
    {
        int AddNotificationEmails(NotificationEmails objectData);
        NotificationEmails GetNotificationByBuilding(int notiCatId, int? eventId, string buildingId);
        NotificationEmails GetNotificationBySite(int notiCatId, int? eventId, string siteId);
        List<NotificationCategory> GetNotificationCategory();
        string GetNotificationEmail(int notificationTypeId);
        NotificationEmails GetNotificationEmailByCat(int notiCatId);
        NotificationEmails GetNotificationEmailByCatID(int notiCatId, string BuildingId, string FloorId, string SiteId, int? EventId = 1);
        List<NotificationMapping> GetNotificationMatrix();
        List<NotificationCategory> GetNotificationMatrixByCategory();
        List<NotificationCategory> GetNotifications(string mode, string id);
        NotificationEmails GetNotifications(string notiCatId, string eventId, string mode, string id);
        bool UpdateNotificationMatrix(NotificationMapping objNotificationMapping);
    }
}