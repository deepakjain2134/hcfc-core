//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace HCF.Web.Helpers
//{
//    public class PermanentRedirectResult : ActionResult
//    {
//        public string Url { get; private set; }

//        public PermanentRedirectResult(string url)
//        {
//            this.Url = url;
//        }

//        public override void ExecuteResult(ControllerContext context)
//        {
//            var response = context.HttpContext.Response;
//            response.StatusCode = 401;
//            response.Status = "401  Unauthorized client";
//            response.RedirectLocation = Url;
//            response.End();
//        }
//    }
//}