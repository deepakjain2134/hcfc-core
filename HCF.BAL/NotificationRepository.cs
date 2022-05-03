//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public class NotificationRepository
//    {   
//        public static int AddNotificationEmails(NotificationEmails objectData)
//        {
//            return DAL.NotificationRepository.AddNotificationEmails(objectData);
//        }        
       
//        public static string GetNotificationEmail(int notificationTypeId)
//        {
//            var emails= DAL.NotificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId == notificationTypeId);

//            return emails != null ? emails.NotificationEmail : "";
//        }

//        public static List<NotificationCategory> GetNotificationCategory()
//        {
//            return DAL.NotificationRepository.GetNotificationCategory();

//        }

//        public static List<NotificationMapping> GetNotificationMatrix()
//        {
//            return DAL.NotificationRepository.GetNotificationMatrix();
//        }

//        public static bool UpdateNotificationMatrix(NotificationMapping objNotificationMapping)
//        {
//            return DAL.NotificationRepository.UpdateNotificationMatrix(objNotificationMapping);
//        }

//        public static List<NotificationCategory> GetNotificationMatrixByCategory()
//        {
//            return DAL.NotificationRepository.GetNotificationMatrixByCategory();
//        }

//        public static NotificationEmails GetNotificationEmailByCat(int notiCatId)
//        {
//            return DAL.NotificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId== notiCatId);
//        }
//        public static NotificationEmails GetNotificationEmailByCatID(int notiCatId, string BuildingId,string FloorId,string SiteId,int? EventId=1)
//        {
//            return DAL.NotificationRepository.GetNotificationEmailsAddress(notiCatId, BuildingId, FloorId, SiteId, EventId);
//        }

//        #region Notification Settings


//        public static List<NotificationCategory> GetNotifications(string mode, string id)
//        {
//            return DAL.NotificationRepository.GetNotifications(mode, id);

//        }

//        public static NotificationEmails GetNotifications(string notiCatId, string eventId, string mode, string id)
//        {
//            return DAL.NotificationRepository.GetNotifications(notiCatId, eventId, mode, id);

//        }

//        private static NotificationEmails GetNotification(int notiCatId, int? eventId, string mode, string id)
//        {
//            return DAL.NotificationRepository.GetNotification(notiCatId, eventId, mode, id);
//        }

//        public static NotificationEmails GetNotificationBySite(int notiCatId, int? eventId, string siteId)
//        {
//            return DAL.NotificationRepository.GetNotification(notiCatId, eventId, "s", siteId);
//        }

//        public static NotificationEmails GetNotificationByBuilding(int notiCatId, int? eventId, string buildingId)
//        {
//            return DAL.NotificationRepository.GetNotification(notiCatId, eventId, "b", buildingId);
//        }


//        #endregion
//    }
//}