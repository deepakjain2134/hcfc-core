using System;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
   
    [Route("api/ilsm")]  
    [ApiController]
    public class IlsmApiController : ApiController
    {
        private readonly ILoggingService _loggingService; private readonly IlsmService _ilsmService;

        public IlsmApiController(ILoggingService loggingService, IlsmService ilsmService)
        {
            _loggingService = loggingService;
            _ilsmService = ilsmService;
        }

        #region ILSM
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage IlsmlinkToWo(string tilsmId, string issueId)
        {
           var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _ilsmService.ILSMlinkToWO(Convert.ToInt32(tilsmId), Convert.ToInt32(issueId));
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {

                };
            }
            else { _objHttpResponseMessage.Success = false; }
            return _objHttpResponseMessage;
        }

        //public HttpResponseMessage UpdateStatusIlsm(TIlsm tilsm)
        //{
        //    _objHttpResponseMessage = new HttpResponseMessage();
        //    var status = IlsmRepository.UpdateILSMStatus(tilsm);
        //    if (status)
        //    {
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result = new Result
        //        {

        //        };
        //    }
        //    else { _objHttpResponseMessage.Success = false; }
        //    return _objHttpResponseMessage;
        //}


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage AddAdditionalTilsmEp(string epdetailId, string tilsmId, bool isActive)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _ilsmService.SaveAdditionalTilsmEP(Convert.ToInt32(epdetailId), Convert.ToInt32(tilsmId), isActive);
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {

                };
            }
            else { _objHttpResponseMessage.Success = false; }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateIlsm(TIlsm objtilsm)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _ilsmService.UpdateIlsm(objtilsm);
            if (status > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {

                };
            }
            else { _objHttpResponseMessage.Success = false; }
            return _objHttpResponseMessage;
        }

        #endregion
    }
}