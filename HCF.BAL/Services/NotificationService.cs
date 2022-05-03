using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public  int AddNotificationEmails(NotificationEmails objectData)
        {
            return _notificationRepository.AddNotificationEmails(objectData);
        }        
       
        public  string GetNotificationEmail(int notificationTypeId)
        {
            var emails= _notificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId == notificationTypeId);

            return emails != null ? emails.NotificationEmail : "";
        }

        public  List<NotificationCategory> GetNotificationCategory()
        {
            return _notificationRepository.GetNotificationCategory();

        }

        public  List<NotificationMapping> GetNotificationMatrix()
        {
            return _notificationRepository.GetNotificationMatrix();
        }

        public  bool UpdateNotificationMatrix(NotificationMapping objNotificationMapping)
        {
            return _notificationRepository.UpdateNotificationMatrix(objNotificationMapping);
        }

        public  List<NotificationCategory> GetNotificationMatrixByCategory()
        {
            return _notificationRepository.GetNotificationMatrixByCategory();
        }

        public  NotificationEmails GetNotificationEmailByCat(int notiCatId)
        {
            return _notificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId== notiCatId);
        }
        public  NotificationEmails GetNotificationEmailByCatID(int notiCatId, string BuildingId,string FloorId,string SiteId,int? EventId=1)
        {
            return _notificationRepository.GetNotificationEmailsAddress(notiCatId, BuildingId, FloorId, SiteId, EventId);
        }

        #region Notification Settings


        public  List<NotificationCategory> GetNotifications(string mode, string id)
        {
            return _notificationRepository.GetNotifications(mode, id);

        }

        public  NotificationEmails GetNotifications(string notiCatId, string eventId, string mode, string id)
        {
            return _notificationRepository.GetNotifications(notiCatId, eventId, mode, id);

        }

        private  NotificationEmails GetNotification(int notiCatId, int? eventId, string mode, string id)
        {
            return _notificationRepository.GetNotification(notiCatId, eventId, mode, id);
        }

        public  NotificationEmails GetNotificationBySite(int notiCatId, int? eventId, string siteId)
        {
            return _notificationRepository.GetNotification(notiCatId, eventId, "s", siteId);
        }

        public  NotificationEmails GetNotificationByBuilding(int notiCatId, int? eventId, string buildingId)
        {
            return _notificationRepository.GetNotification(notiCatId, eventId, "b", buildingId);
        }


        #endregion
    }
}