using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
 
    [Route("api/offline")]
    [ApiController]
    public class OfflineApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        public OfflineApiController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
    }
}