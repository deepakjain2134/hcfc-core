//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;


//namespace HCF.Web.Controllers
//{
//    public class BasePublicController : Controller
//    {
//        protected string ErrorMessage
//        {
//            get => TempData["ErrorMessage"] == null ? string.Empty : TempData["ErrorMessage"].ToString();
//            set => TempData["ErrorMessage"] = value;
//        }

//        protected string SuccessMessage
//        {
//            get => TempData["SuccessMessage"] == null ? String.Empty : TempData["SuccessMessage"].ToString();
//            set => TempData["SuccessMessage"] = value;

//        }
            
//        protected string WarningMessage
//        {
//            get => TempData["WarningMessage"] == null ? String.Empty : TempData["WarningMessage"].ToString();
//            set => TempData["WarningMessage"] = value;
//        }
//    }
//}