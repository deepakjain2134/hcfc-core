//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace HCF.Web.Filters
//{
//    public class HandleUnauthorizedRequest: AuthorizeAttribute
//    {
//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            HttpContext ctx = HttpContext.Current;
//            // check if session is supported  
//            if (ctx.Session != null)
//            {
//                // check if a new session id was generated  
//                if (ctx.Session[Utility.SessionKey.User] == null || ctx.Session.IsNewSession || Base.UserSession.CurrentUser.UserId==0)
//                {
//                    //Check is Ajax request  
//                    if (filterContext.HttpContext.Request.IsAjaxRequest())
//                    {
//                        filterContext.HttpContext.Response.ClearContent();
//                        filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
//                    }
//                    // check if a new session id was generated  
//                    else
//                    {
//                        filterContext.Result = new RedirectResult("~/Account/Login");
//                    }
//                }
//            }
//            base.HandleUnauthorizedRequest(filterContext);
//        }
//    }
//}