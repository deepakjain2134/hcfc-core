using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Module.Core.Areas.Core.Controllers
{
    public class BaseController : Controller
    {        
        public BaseController()
        {

        }

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
    }
}
