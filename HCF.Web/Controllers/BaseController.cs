using HCF.Utility;
using HCF.Utility.Configuration;
using HCF.Web.Base;
using HCF.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace HCF.Web.Controllers
{
    [ServiceFilter(typeof(BaseActionAsyncActionFilter))]
    [Authorize]
    public class BaseController : Controller
    {
        #region ctor        

        /// <summary>
        /// /
        /// </summary>
        public BaseController()
        {

            SetClientTimeZoneInfo();

            //if (UserSession.CurrentUser.UserId == 0)
            //    RedirectToRoute("login");

            //var dbName = ServiceLocator.ServiceProvider.GetService<IHCFSession>().Get<string>(SessionKey.ClientDBName);
            //if (string.IsNullOrEmpty(dbName))
            //    RedirectToAction(nameof(AuthController.LogOff), "Auth");
        }

        private void SetClientTimeZoneInfo()
        {
            try
            {
                var context = ServiceLocator.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;
                context.Request.Headers.TryGetValue("Cookie", out StringValues values);
                var cookies = values.ToString().Split(';').ToList();
                var result = cookies.Select(c => new { Key = c.Split('=')[0].Trim(), Value = c.Split('=')[1].Trim() }).ToList();
                var timeZoneInfo = result.FirstOrDefault(r => r.Key == "timezoneoffset");
                if (timeZoneInfo != null)
                {
                    ServiceLocator.ServiceProvider.GetService<IHCFSession>().Set(SessionKey.TimeZoneOffSet, timeZoneInfo.Value);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region
        protected string ErrorMessage
        {
            get => TempData[Constants.MessageViewBagError] == null ? string.Empty : TempData[Constants.MessageViewBagError].ToString();
            set => TempData[Constants.MessageViewBagError] = value;
        }

        protected string SuccessMessage
        {
            get => TempData[Constants.MessageViewBagSuccess] == null ? string.Empty : TempData[Constants.MessageViewBagSuccess].ToString();
            set => TempData[Constants.MessageViewBagSuccess] = value;
        }

        protected string WarningMessage
        {
            get => TempData[Constants.MessageViewBagWarning] == null ? string.Empty : TempData[Constants.MessageViewBagWarning].ToString();
            set => TempData[Constants.MessageViewBagWarning] = value;
        }

        #endregion
    }
}