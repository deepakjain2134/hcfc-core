using System;
using System.Collections.Generic;
using System.Linq;
using Hcf.Api.Application;

using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/icra")]   
    [ApiController]
    public class ICRAApiController : ApiController
    {
        private readonly ILoggingService _loggingService;

        private readonly IConstructionService _constructionService;
        private readonly IEmailService _emailService;
        private readonly IApiCommon _apiCommon;
        private readonly IDigitalSignService _digitalSignService;

        public ICRAApiController(IDigitalSignService digitalSignService, IApiCommon apiCommon, ILoggingService loggingService, IConstructionService constructionService, IEmailService emailService)
        {
            _digitalSignService = digitalSignService;
            _apiCommon = apiCommon;
            _loggingService = loggingService;
            _emailService = emailService;
            _constructionService = constructionService;
        }

        #region APIs

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addConstructionType")]
        public ActionResult AddConstructionType(ConstructionType modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constructiontypeid = _constructionService.AddConstructionType(modelData);
            if (constructiontypeid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ConstructionTypeId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ConstructionType = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addConstuctionActivity")]
        public ActionResult AddConstructionActivity(ConstructionActivity modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constactivityid = _constructionService.AddConstuctionActivity(modelData);
            if (constactivityid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ConstActivityId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ConstructionActivity = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addConstructionRisk")]
        public ActionResult AddConstructionRisk(ConstructionRisk modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constructionriskid = _constructionService.AddConstructionRisk(modelData);
            if (constructionriskid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ConstructionRiskId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ConstructionRisk = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addConstructionClass")]
        public ActionResult AddConstructionClass(ConstructionClass modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constructionclassid = _constructionService.AddConstructionClass(modelData);
            if (constructionclassid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ConstructionClassId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ConstructionClass = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addConstClassActivity")]
        public ActionResult AddConstClassActivity(ConstructionClassActivity modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constclassactivityid = _constructionService.AddConstClassActivity(modelData);
            if (constclassactivityid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ConstClassActivityId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ConstructionClassActivity = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addICRASteps")]
        public ActionResult AddIcraSteps(ICRASteps modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var constclassactivityid = _constructionService.AddICRASteps(modelData);
            if (constclassactivityid > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = modelData.ICRAStepId > 0 ? ConstMessage.Success_Updated : ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result
                {
                    ICRASteps = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getInspectionIcraSteps/{ticraId}")]
        public ActionResult GetInspectionIcraSteps(string ticraId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var objTIcraLog = _constructionService.GetInspectionIcraSteps(Convert.ToInt32(ticraId));
            if (objTIcraLog != null)
            {

                objTIcraLog.ConstructionTypes = new List<ConstructionType>();
                objTIcraLog.ConstructionRisks = new List<ConstructionRisk>();
                objTIcraLog.ConstructionClasses = new List<ConstructionClass>();
                objTIcraLog.ICRAMatixPrecautions = new List<ICRAMatixPrecautions>();
                objTIcraLog.ConstructionTypes = _constructionService.GetConstructionType().ToList();
                objTIcraLog.ConstructionRisks = _constructionService.GetConstructionRisk().ToList();
                objTIcraLog.ConstructionClasses = _constructionService.GetConstructionClass();
                objTIcraLog.ICRAMatixPrecautions = _constructionService.GetICRAMatixPrecautions();
                if (objTIcraLog.AreasSurroundings.Count == 0)
                {
                    objTIcraLog.AreasSurroundings = new List<TICRAAreasNearBy>();
                    foreach (BDO.Enums.AreasSurrounding item in Enum.GetValues(typeof(BDO.Enums.AreasSurrounding)))
                    {
                        var newTicraAreasNearBy = new TICRAAreasNearBy
                        {
                            AreasSurrounding = item,
                            AreasSurroundingId = (int)item
                        };
                        objTIcraLog.AreasSurroundings.Add(newTicraAreasNearBy);
                    }
                }
                _objHttpResponseMessage.Result = new Result
                {
                    TIcraLog = objTIcraLog
                };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Internal_Server_Error;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getAllInspectionIcraSteps/{ticraId}")]
        public ActionResult GetAllInspectionIcraSteps(string ticraId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var objTIcraLog = _constructionService.GetAllInspectionIcraSteps(Convert.ToInt32(ticraId));
            if (objTIcraLog != null)
            {
                _objHttpResponseMessage.Result = new Result
                {
                    TIcraLogs = objTIcraLog
                };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Internal_Server_Error;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addInspectionIcra")]
        public ActionResult AddInspectionIcra(TIcraLog modelData)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var ticraId = _constructionService.AddIcra(modelData, "2");
            if (ticraId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = (modelData.TicraId > 0) ? ConstMessage.Success_Updated : $"{"Permit #"} {modelData.ProjectNo} {ConstMessage.Success}";
                _objHttpResponseMessage.Result = new Result
                {
                    TIcraLog = modelData
                };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("SendNotifyEmail/{userId}/{permitno}/{permittype}/{version}")]
        public ActionResult SendNotifyEmail(string userId, string permitNo, string permittype,int version)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (Convert.ToInt32(userId) > 0)
            {
                //permitNo = (!string.IsNullOrEmpty(permitNo) && permitNo == "undefined") ? "" : permitNo;
                var res = _emailService.SendICRANotifyEmail(Convert.ToInt32(userId), permitNo, permittype, version);
                _objHttpResponseMessage.Success = res;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("SendNotifyPCRAEmail/{userId}/{permitno}/{permittype}/{tpcraquesid}/{projectid}")]
        public ActionResult SendNotifyPCRAEmail(string userId, string permitNo, string permittype, string tpcraquesid, string projectid)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (Convert.ToInt32(userId) > 0)
            {
                //permitNo = (!string.IsNullOrEmpty(permitNo) && permitNo == "undefined") ? "" : permitNo;
                var res = _emailService.SendPCRANotifyEmail(Convert.ToInt32(userId), permitNo, permittype, Convert.ToInt32(tpcraquesid), Convert.ToInt32(projectid));
                _objHttpResponseMessage.Success = res;
            }
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addICRARiskArea")]
        public ActionResult AddIcraRiskArea(ICRARiskArea icraRiskArea)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (icraRiskArea.RiskAreaId > 0)
            {
                _constructionService.UpdateICRARiskAra(icraRiskArea);
                _objHttpResponseMessage.Message = ConstMessage.Success_Updated;
            }
            else
            {
                icraRiskArea.RiskAreaId = _constructionService.AddICRARiskArea(icraRiskArea);
                if (icraRiskArea.RiskAreaId > 0)
                {
                    _objHttpResponseMessage.Message = ConstMessage.Success;
                }
            }

            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result
            {
                ICRARiskArea = icraRiskArea
            };
            return Ok(_objHttpResponseMessage);
        }

        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("SaveSignedReport")]
        public ActionResult SaveSignedReport(List<TICRAFiles> ticraFiles)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var item in ticraFiles.Where(x => !string.IsNullOrEmpty(x.FileName)))
            {
                if (item.FileContent != null)
                    item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "TICRAFilesPath", item.TicraId.ToString()).FilePath;
                _constructionService.SaveTICRAFiles(item);
            }
            _objHttpResponseMessage.Success = true;
            return Ok(_objHttpResponseMessage);
        }

        #endregion


        #region Observation Reports 

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("SaveObservationReport")]
        public ActionResult SaveObservationReport(TICRAReports newTICRAReports)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (!string.IsNullOrEmpty(newTICRAReports.FilesContent))
            {
                Files newFile = _apiCommon.SaveFile(newTICRAReports.FilesContent, newTICRAReports.FileName, "ObservationReports", Convert.ToString(newTICRAReports.TICRAReportId));
                newTICRAReports.FileName = newFile.FileName;
                newTICRAReports.ReportPath = newFile.FilePath;
                bool status = true;
                if (status)
                {
                    _objHttpResponseMessage.Success = _constructionService.SaveObservationReport(newTICRAReports); ;
                    _objHttpResponseMessage.Result = new Result
                    {
                        TICRAReports = newTICRAReports
                    };
                }
            }
            return Ok(_objHttpResponseMessage);
        }

        #endregion

        #region Private Methods

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private int SaveDigitalFile(DigitalSignature dSPermitAuthorizedBy, string projectNo)
        {
            if (dSPermitAuthorizedBy != null)
            {
                if (!string.IsNullOrEmpty(dSPermitAuthorizedBy.FileName) && !string.IsNullOrEmpty(dSPermitAuthorizedBy.FileContent))
                {
                    dSPermitAuthorizedBy.FilePath = _apiCommon.SaveFile(dSPermitAuthorizedBy.FileContent,
                        dSPermitAuthorizedBy.FileName,
                        "DigitalSignPath", projectNo).FilePath;
                    dSPermitAuthorizedBy.DigSignatureId = _digitalSignService.Save(dSPermitAuthorizedBy);

                }
            }
            return dSPermitAuthorizedBy.DigSignatureId;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void SendEmailonRiskareaApproval(ICRARiskArea icraRiskArea)
        {
            var status = _emailService.SendEmailonRiskareaApproved(icraRiskArea);
            if (status)
            {
                icraRiskArea.IsSendEmail = true;
                _constructionService.UpdateICRARiskAra(icraRiskArea);
            }
        }

        #endregion

    }
}