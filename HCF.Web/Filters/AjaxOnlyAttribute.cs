using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;


namespace HCF.Web.Filters
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.IsChildAction)
            //    filterContext.HttpContext.Response.Redirect("/error404");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}