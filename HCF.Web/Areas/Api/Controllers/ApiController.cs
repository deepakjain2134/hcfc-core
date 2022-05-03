using HCF.Web.Areas.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceFilter(typeof(ActionWebApiFilter))]
    //[ApiController]
    public class ApiController : Controller
    {
          
    }     
}
