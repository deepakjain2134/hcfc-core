using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using AutoMapper;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Areas.Api.Filters;
using HCF.Web.Areas.Api.Models;
using HCF.Web.Areas.Api.Models.Request;
using HCF.Web.Areas.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HCF.Web.Areas.Api.Controllers
{
    [Route("api/master")]
    [ApiController]
    [ServiceFilter(typeof(ActionWebApiFilter))]
    [ApiExplorerSettings(GroupName = "Master")]
    public class MasterApiController : ApiController
    {
        private readonly IStatusService _statusService;
        private readonly ISubStatusService _subStatusService;
        private readonly IMapper _mapper;

        public MasterApiController(IStatusService statusService, ISubStatusService subStatusService, IMapper mapper)
        {
            _statusService = statusService;
            _subStatusService = subStatusService;
            _mapper = mapper;
        }

        #region WO Status

        /// <summary>
        /// Returns a list of status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("status")]
        public ActionResult GetWOStatus()
        {
            List<WoStatus> status = _statusService.GetStatus().ToList();
            List<StatusResponseModel> statusViewModel = _mapper.Map<List<StatusResponseModel>>(status);
            if (statusViewModel.Count > 0)
            {
                var data = new Response<List<StatusResponseModel>>(statusViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Creates status with the details specified in the request body
        /// </summary>
        /// <param name="newWoStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status")]
        public ActionResult SaveWOStatus([FromBody] SaveStatusRequestModel RequestModel)
        {
            List<WoStatus> status = _statusService.GetStatus().Where(x => x.Code == RequestModel.Code).ToList();
            if (status.Count > 0)
            {
                return Ok(new Response<ConstMessage>(string.Format(ConstMessage.Already_Exists, "Status Code")));
            }
            else
            {
                try
                {
                    BDO.WoStatus newWoStatus = _mapper.Map<BDO.WoStatus>(RequestModel);
                    newWoStatus.ModuleCode = "WO";
                    newWoStatus.WoStatusId = _statusService.Save(newWoStatus);
                    if (newWoStatus.WoStatusId > 0)
                    {
                        var data = new Response<Int32>(newWoStatus.WoStatusId, ConstMessage.Saved_Successfully);
                        return Ok(data);
                    }
                }
                catch
                {
                    return Ok(new Response<ConstMessage>(ConstMessage.Error));
                }
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Updates status with the details specified in the request body
        /// </summary>
        /// <param name="newWoStatus"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("status/{statusCode}")]
        public ActionResult SaveWOStatus([FromBody] UpdateStatusRequestModel RequestModel, string statusCode)
        {
            BDO.WoStatus newWoStatus = _mapper.Map<BDO.WoStatus>(RequestModel);
            newWoStatus.Code = statusCode;
            newWoStatus.WoStatusId = _statusService.Save(newWoStatus);
            if (newWoStatus.WoStatusId > 0)
            {
                var data = new Response<Int32>(newWoStatus.WoStatusId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(string.Format(ConstMessage.Not_Found, "Status Code")));
        }

        /// <summary>
        /// Returns specified status
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("status/{statusCode}")]
        public ActionResult GetWOStatus(string statusCode)
        {
            WoStatus status = _statusService.GetStatus().FirstOrDefault(x => x.Code == statusCode);
            StatusResponseModel statusViewModel = _mapper.Map<StatusResponseModel>(status);
            if (statusViewModel != null && statusViewModel.StatusId > 0)
            {
                var data = new Response<StatusResponseModel>(statusViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        #endregion WO Status


        #region WO SubStatus

        /// <summary>
        /// Returns a list of substatus based on status code
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("status/{statusCode}/substatus")]
        public ActionResult GetStatusSubStatus(string statusCode)
        {
            List<SubStatus> substatus = _subStatusService.GetSubStatus().Where(x => x.StatusCode == statusCode).ToList();
            List<SubStatusRepsonseModel> substatusViewModel = _mapper.Map<List<SubStatusRepsonseModel>>(substatus);
            if (substatusViewModel.Count > 0)
            {
                var data = new Response<List<SubStatusRepsonseModel>>(substatusViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.No_Records_Found));
        }

        /// <summary>
        /// Returns specified substatus
        /// </summary>
        /// <param name="subStatusCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("substatus/{subStatusCode}")]
        public ActionResult GetSubStatus(string subStatusCode)
        {
            SubStatus substatus = _subStatusService.GetSubStatus().FirstOrDefault(x => x.Code == subStatusCode);
            SubStatusRepsonseModel substatusViewModel = _mapper.Map<SubStatusRepsonseModel>(substatus);
            if (substatusViewModel != null && substatusViewModel.SubStatusId > 0)
            {
                var data = new Response<SubStatusRepsonseModel>(substatusViewModel);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Creates substatus with the details specified in the request body
        /// </summary>
        /// <param name="newWoStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("substatus")]
        public ActionResult SaveSubstaus([FromBody] SaveSubstatusRequestModel RequestModel)
        {
            BDO.SubStatus newSubStatus = _mapper.Map<BDO.SubStatus>(RequestModel);
            newSubStatus.SubStatusId = _subStatusService.Save(newSubStatus);
            if (newSubStatus.WoStatusId > 0)
            {
                var data = new Response<Int32>(newSubStatus.SubStatusId, ConstMessage.Saved_Successfully);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        /// <summary>
        /// Updates substatus with the details specified in the request body
        /// </summary>
        /// <param name="newWoStatus"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("substatus/{subStatusCode}")]
        public ActionResult SaveSubstaus([FromBody] UpdateSubstatusRequestModel RequestModel, string subStatusCode)
        {
            BDO.SubStatus newSubStatus = _mapper.Map<BDO.SubStatus>(RequestModel);
            newSubStatus.Code = subStatusCode;
            newSubStatus.SubStatusId = _subStatusService.Save(newSubStatus);
            if (newSubStatus.WoStatusId > 0)
            {
                var data = new Response<Int32>(newSubStatus.SubStatusId, ConstMessage.Success_Updated);
                return Ok(data);
            }
            return Ok(new Response<ConstMessage>(ConstMessage.Internal_Server_Error));
        }

        #endregion WO SubStatus
    }
}
