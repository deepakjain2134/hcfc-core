using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.BDO.Enums;
using HCF.Utility;
using HCF.Utility.AppConfig;
using HCF.Web.Filters;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HCF.Web.Controllers
{
    [Route("error")]
    public class MessageController : Controller
    {

        #region Ctor

        private readonly ILoggingService _baselogger;
        private readonly IEmailService _emailService;      
        private readonly IBrowserDetector _browserDetector;
        private readonly IHCFSession _hcfsession;            

        public MessageController(IHCFSession hcfsession,ILoggingService logger, IEmailService emailService, IBrowserDetector browserDetector)
        {         
            _baselogger = logger;
            _emailService = emailService;
            _browserDetector = browserDetector;
            _hcfsession = hcfsession;
        }


        #endregion

        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            string originalPath = "unknown";
            if (HttpContext.Items.ContainsKey("originalPath"))
            {
                originalPath = HttpContext.Items["originalPath"] as string;
            }
            _baselogger.Error($"Page Not Found:{originalPath}");

            return View();
        }

        [Route("500")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            var exceptions = new AppException();
            try
            {
                var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
                ViewData["statusCode"] = exceptions.StatusCode = HttpContext.Response.StatusCode;
                ViewData["message"] = exceptions.AppErrorMessage = exception.Error.Message;
                ViewData["stackTrace"] = exceptions.AppStackTrace = exception.Error.StackTrace;
                ViewData["errorPath"] = exceptions.ErrorPath = exceptionHandlerPathFeature.Path;

                var userName = Base.UserSession.CurrentUser.FullName;
                SendErrorMail("logged-in -> " + userName + " (Reported By -> System)", Convert.ToString(HttpContext.Response.StatusCode),
                    exception.Error.Message,
                    exception.Error.StackTrace,
                    0,
                    exceptionHandlerPathFeature.Path);

                if (Base.UserSession.CurrentUser.UserId == 0)
                {
                    return RedirectToAction("LogOff", "Account");
                }
            }
            catch (Exception ex)
            {
                _baselogger.Error(ex);
            }
            return View(exceptions);
        }

        [ActionValidate(isRequired: false)]
        public ActionResult RedirectToError(string ctrName, string actionName)
        {
            TempData.Peek("HandleErrorInfo");
            return View("_error");
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SendError(string statusCode, string message, string stackTrace, int count, string errorPath)
        {
            var userName = Base.UserSession.CurrentUser.FullName;
            LogEntry logEntry = SendErrorMail(userName, statusCode, message, stackTrace, count, errorPath);
            return Json(logEntry);
        }

        private LogEntry SendErrorMail(string userName,
            string statusCode, string message,
            string stackTrace, int count,
            string errorPath)
        {
            var hospitalName = _hcfsession.ClientDbName;
            var logEntry = new LogEntry
            {
                UserName = userName,
                DbName = hospitalName,
                ErrorMessage = message,
                ErrorCode = statusCode,
                StackTrace = stackTrace,
                Module = string.Format("{0}",errorPath),
                OsName = _browserDetector.Browser.OS,
                BrowserName = string.Format("{0} ({1})", _browserDetector.Browser.Name, _browserDetector.Browser.Version),                
                DeviceType = _browserDetector.Browser.DeviceType
            };
            if (count == 0)
                _emailService.ReportError(logEntry);
            return logEntry;
        }
    }
}