using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.Areas.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/news")]
    //[Route("api/news")]
    [ApiController]
    public class NewsApiController : ApiController
    {
        private readonly INewsService _newsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        public NewsApiController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// get all the news related to new features and EPs
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("")]
        public ActionResult GetAllNews()
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    News = _newsService.GetAllNews().Result
                },
                Success = true
            };
            return Ok(_objHttpResponseMessage);
        }
    }
}