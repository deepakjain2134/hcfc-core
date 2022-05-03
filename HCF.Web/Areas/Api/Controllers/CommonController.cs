using System;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
    
    [Route("api/common")]
    [ApiController]
    public class CommonApiController : ApiController
    {
      
        private readonly IBuildVersionService _buildVersionService;
        private readonly ICommonService _commonService;
        private readonly IOrganizationService _organizationService;

        public CommonApiController(IOrganizationService organizationService, IBuildVersionService buildVersionService, ICommonService commonService)
        {
            _organizationService = organizationService;
            _commonService = commonService;          
            _buildVersionService = buildVersionService;
        }

        #region Org

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetOrganizations(string orgKey)
        {
            return !string.IsNullOrEmpty(orgKey) ? GetOrg(orgKey) : GetOrg();
        }

        #endregion

        #region Methods

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private HttpResponseMessage GetOrg()
        {
          var _objHttpResponseMessage = new HttpResponseMessage();
            var orgMasters = _organizationService.GetMasterOrg();
            if (orgMasters.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    Organizations = orgMasters
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetOrg(string orgKey)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var key = Guid.Parse(orgKey);
            var org = _organizationService.GetMasterOrg().FirstOrDefault(x=>x.Orgkey == key);
            if (org != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    Organization = org
                };
            }
            else
            {
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Build Versions

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetBuildLists()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var versions = _buildVersionService.GetBuildVersions();
            if (versions.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    BuildVersions = versions
                };
            }
            else
                _objHttpResponseMessage.Message = HCF.Utility.ConstMessage.No_Records_Found;

            return _objHttpResponseMessage;
        }


        #endregion
    }
}