using HCF.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Areas.Core.Controllers
{
    public class BasePublicController : Controller
    {
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

        protected string Error
        {
            get => TempData[Constants.MessageError] == null ? string.Empty : TempData[Constants.MessageError].ToString();
            set => TempData[Constants.MessageError] = value;
        }


        protected string Success
        {
            get => TempData[Constants.MessageSuccess] == null ? string.Empty : TempData[Constants.MessageError].ToString();
            set => TempData[Constants.MessageSuccess] = value;
        }
    }
}
