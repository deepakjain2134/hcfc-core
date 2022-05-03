using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace HCF.Web.Controllers
{
    public class LogController : BaseController
    {
        protected readonly ILoggingService LoggingService;

        public LogController(ILoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        public ActionResult Index()
        {
            if (!UserSession.CurrentUser.IsSystemUser)
                return RedirectToRoute("error404");

            IList<LogEntry> logs = new List<LogEntry>();

            try
            {
                logs = LoggingService.ListLogFile();
            }
            catch (Exception ex)
            {
                var err = $"Unable to access logs: {ex.Message}";
                TempData[Constants.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };

                LoggingService.Error(err);
            }

            return View(new ListLogViewModel { LogFiles = logs });
        }

        public ActionResult ClearLog()
        {
            LoggingService.ClearLogFiles();

            TempData[Constants.MessageViewBagName] = new GenericMessageViewModel
            {
                Message = "Log File Cleared",
                MessageType = GenericMessages.success
            };
            return RedirectToAction("Index");
        }
    }
}