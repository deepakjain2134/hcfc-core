using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HCF.Web.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly ISiteService _siteService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(ISiteService siteService, INotificationService notificationService, IUserService userService ) 
        {
            _siteService = siteService;
            _notificationService = notificationService;
            _userService = userService;
        }

        public ActionResult NotificationEmails()
        {
            UISession.AddCurrentPage("Notification_NotificationEmails", 0, "Notification Emails");
            var notificationTypes = _notificationService.GetNotificationCategory().Where(x => x.IsActive == false)
                .ToList();
            return View(notificationTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NotificationMatrix()
        {
            UISession.AddCurrentPage("notification_NotificationMatrix", 0, "Notification Configuration");
            var NotificationConfig = _notificationService.GetNotificationMatrix();
            return View(NotificationConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationCatId"></param>
        /// <param name="notificationEventId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateNotificationMatrix(int notificationCatId, int notificationEventId, bool newValue)
        {
            var _status = !newValue ? 0 : 1;
            var objNotificationMapping = new NotificationMapping { Status = _status, NotificationCatId = notificationCatId, NotificationEventId = notificationEventId };
            var status = _notificationService.UpdateNotificationMatrix(objNotificationMapping);
            return Json(status);
        }


        [HttpGet]
        public ActionResult GetEmails(string notiCatId)
        {
            var notificationEmails = _notificationService.GetNotificationEmailByCat(Convert.ToInt32(notiCatId)) ??
                                     new NotificationEmails
                                     {
                                         NotificationCatId = Convert.ToInt32(notiCatId),
                                         BuildingId = 0,
                                         SiteId = 0,
                                         FloorId = 0,
                                         NotificationEventId = 0
                                     };
            ViewBag.Users = _userService.GetUsers().Where(x => !x.IsCRxUser).ToList();

            return PartialView("_addNotificationEmails", notificationEmails);
        }


        [HttpPost]
        public JsonResult SaveNotificationCatEmail(NotificationEmails notificationEmails)
        {
            notificationEmails.IsActive = true;
            _notificationService.AddNotificationEmails(notificationEmails);
            return Json(new { data = notificationEmails.NotificationEmail });
        }


        #region Notification Settings 

        public ActionResult NotificationSettings()
        {
            UISession.AddCurrentPage("notification_notificationsettings", 0, "Notification Setting");
            List<Site> sites = _siteService.GetSitesBuildings();
            return View(sites);
        }

        public ActionResult NotifyMatrix(string mode, string id)
        {
            List<NotificationCategory> data = _notificationService.GetNotifications(mode, id);
            return PartialView("_NotificationMatrix", data);
        }

        [HttpGet]
        public ActionResult GetNotifyEmails(string notiCatId, string eventId, string mode, string id)
        {
            var notificationEmails = _notificationService.GetNotifications(notiCatId, eventId, mode, id);
            if (notificationEmails != null)
            {
                if (mode == "b" && notificationEmails.BuildingId == null)
                    notificationEmails.BuildingId = Convert.ToInt32(id);
                else if (mode == "s" && notificationEmails.SiteId == null)
                    notificationEmails.SiteId = Convert.ToInt32(id);
                else if (mode == "f" && notificationEmails.FloorId == null)
                    notificationEmails.FloorId = Convert.ToInt32(id);
            }
            ViewBag.Users = _userService.GetUsers().Where(x => !x.IsCRxUser).ToList();

            if (notificationEmails == null)
            {
                notificationEmails = new NotificationEmails();
            }
            return PartialView("_addNotificationEmails", notificationEmails);
        }
        #endregion
    }
}