using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
using HCF.BAL.Interfaces;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
    
    [Route("api/goal")]
   [ApiController]
    public class GoalApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IUserService _userService;
        private readonly IStandardService _standardService;
        private readonly IStepsService _stepService;
        private readonly IIlsmService _ilsmService;
        private readonly IDocumentsService _documentsService;
        private readonly IMainGoalService _mainGoalService;
        private readonly IEpService _epService;
        private readonly IFrequencyService _frequencyService;
        private readonly IGoalMasterService _goalMasterService;
        private readonly ITransactionService _transactionService;

        public GoalApiController(IEpService epService, IMainGoalService mainGoalService, IDocumentsService documentsService, IlsmService ilsmService, ILoggingService loggingService, IUserService userService, IStandardService standardService, IStepsService stepService, IFrequencyService frequencyService, IGoalMasterService goalMasterService, ITransactionService transactionService)
        {
            _epService = epService;
            _mainGoalService = mainGoalService;
            _documentsService = documentsService;
            _ilsmService = ilsmService;
            _loggingService = loggingService;
            _userService = userService;
            _standardService = standardService;
            _stepService = stepService;
            _frequencyService = frequencyService;
            _goalMasterService = goalMasterService;
            _transactionService = transactionService;
        }

        #region ILSMMeasures

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //public HttpResponseMessage GetIlsmMeasures()
        //{
        //    objHttpResponseMessage = new HttpResponseMessage
        //    {
        //        Result = new Result
        //        {
        //            ILSMMeasures = IlsmRepository.GetIlsmMeasures()
        //        },
        //        Success = true
        //    };
        //    if (objHttpResponseMessage.Result.ILSMMeasures != null &&
        //        objHttpResponseMessage.Result.ILSMMeasures.Count == 0)
        //    {
        //        objHttpResponseMessage.Success = false;
        //        objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return objHttpResponseMessage;
        //}

        #endregion

        #region Binders

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetBinders()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var binders = _documentsService.GetBinders();
            if (binders.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Binders = binders };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Standard

        /// <summary>
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public int AddStandard(HCF.BDO.Standard newdata)
        {
            return _standardService.AddStandard(newdata);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetStandards()
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    Standards = _standardService.GetStandards()
                },
                Success = true
            };

            if (_objHttpResponseMessage.Result.Standards.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="stdescId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HCF.BDO.Standard GetStandard(int stdescId)
        {
            return _standardService.GetStandard(stdescId);
        }

        /// <summary>
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public bool UpdateStandard(HCF.BDO.Standard newdata)
        {
            return _standardService.UpdateStandard(newdata);
        }

        #endregion

        #region EPDetails

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpAssignment()
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    EPDetails = _epService.GetEpAssignment()
                },
                Success = true
            };

            if (_objHttpResponseMessage.Result.EPDetails.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetUsersEpsLists(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    EPDetails = _standardService.GetUserEPs(Convert.ToInt32(userId))
                },
                Success = true
            };
            if (_objHttpResponseMessage.Result.EPDetails.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage SaveUserEps(EPUsers objEpusers)
        {
            var objHttpResponseMessage = new HttpResponseMessage();
            var type = "E";
            var res = _goalMasterService.SaveUserEPs(Convert.ToInt32(objEpusers.UserId), Convert.ToInt32(objEpusers.EPDetailId), objEpusers.EPDetailIds, Convert.ToInt32(objEpusers.CreatedBy), type, objEpusers.Status);
            if (res > 0)
            {
                objHttpResponseMessage.Success = true;
                objHttpResponseMessage.Result = new Result() { EPDetails = _standardService.GetUserEPs(Convert.ToInt32(objEpusers.UserId)) };
                objHttpResponseMessage.Message = ConstMessage.Success_Updated;
            }
            else
            {
                objHttpResponseMessage.Success = true;
                objHttpResponseMessage.Message = ConstMessage.Error;
            }
            return objHttpResponseMessage;
        }



        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpAssignmentUser()
        {
            var _objHttpResponseMessage =
                new HttpResponseMessage
                {
                    Result = new Result { EPDetails = _epService.GetEpAssignmentUser() },
                    Success = true
                };
            if (_objHttpResponseMessage.Result.EPDetails.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage OngoingEpDetails()
        {
            var _objHttpResponseMessage =
                new HttpResponseMessage
                {
                    Result = new Result { EPDetails = _epService.OngoingEpDetails() },
                    Success = true
                };
            if (_objHttpResponseMessage.Result.EPDetails.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetDeficientEPs(string userId)
        {
            //GetUpdatedWorkOrders();

            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    EPDetails = _epService.GetDeficientEPs(Convert.ToInt32(userId))
                },
                Success = true
            };
            if (_objHttpResponseMessage.Result.EPDetails.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetStandardEPs(string epDetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    StandardEp = _epService.GetStandardEPs(Convert.ToInt32(epDetailId))
                },
                Success = true
            };
            if (_objHttpResponseMessage.Result.StandardEp == null)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            else
            {
                _objHttpResponseMessage.Success = true;
            }
            return _objHttpResponseMessage;
        }


        #endregion

        #region Frequency

        /// <summary>
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateEpFrequency(EPFrequency newdata)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result()
            };
            var res = _goalMasterService.UpdateEpFrequency(newdata);
            if (res > 0)
            {
                newdata.EpFrequencyId = res;
                _objHttpResponseMessage.Result.EPFrequency = newdata;
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Frequency_Notupadted_Inprogress;
            }
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFrequency()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var list = _frequencyService.GetFrequency();
            if (list.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    FrequencyMaster = list
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion


        #region MainGoal/Steps

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetMainGoalSteps(string epDetailId, string activityType)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result
                {
                    MainGoals = _mainGoalService.GetAllMainGoal(null, null).ToList()
                },
                Success = true
            };
            if (_objHttpResponseMessage.Result.MainGoals == null)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            else
            {
                _objHttpResponseMessage.Success = true;
            }
            return _objHttpResponseMessage;
        }

        #endregion MainGoal/Steps

        #region Steps

        /// <summary>
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public int Addsteps(Steps newdata)
        {
            var step = _stepService.Save(newdata);
            return step.StepsId;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public List<Steps> GetSteps()
        {
            return _stepService.GetSteps();
        }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRaSteps()
        {
            var raSteps = _stepService.GetRASetps();
            var _objHttpResponseMessage = new HttpResponseMessage { Result = new Result() { Steps = raSteps } };
            if (raSteps.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
            }
            return _objHttpResponseMessage;
        }




        #endregion

        #region AffectedEPs

        /// <summary>
        /// </summary>
        /// <param name="newAffectedEps"></param>
        /// <returns></returns>
        //public HttpResponseMessage SaveAffectedEps(AffectedEPs newAffectedEps)
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var result = GoalMaster.AddAffectedEPs(newAffectedEps);
        //    if (result > 0)
        //    {
        //        objHttpResponseMessage.Success = true;
        //        objHttpResponseMessage.Message = ConstMessage.Sucess;
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Success = false;
        //        objHttpResponseMessage.Message = ConstMessage.Error;
        //    }
        //    return objHttpResponseMessage;
        //}

        /// <summary>
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAffectedEPs(string epDetailId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result { AffectedEPs = _goalMasterService.GetAffectedEPs(Convert.ToInt32(epDetailId)) },
                Success = true
            };
            if (_objHttpResponseMessage.Result.AffectedEPs.Count == 0)
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region steps

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpDocSteps(string epDetailId, string inspectionId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var mode = Convert.ToInt32(BDO.Enums.Mode.DOC);
            var epdocs = _transactionService.GetGoalStepsByActivity(null, null, Convert.ToInt32(epDetailId), Convert.ToInt32(inspectionId), mode, 0);
            foreach (var item in epdocs.Where(item => item.MainGoal.Count > 0))
            {
                if (item.MainGoal.Count > 0)
                    foreach (var goal in item.MainGoal)
                        goal.Steps = goal.Steps.Where(m => m.IsCurrent).ToList();
                item.InspectionDocs = _transactionService.GetEpInspectionDoc(Convert.ToInt32(epDetailId));
                foreach (var inspectiondoc in item.InspectionDocs)
                {
                    inspectiondoc.EffectiveDate = Conversion.ConvertToTimeSpan(inspectiondoc.DtEffectiveDate);
                    inspectiondoc.UserProfile = _userService.GetUserProfile(inspectiondoc.CreatedBy);
                }
            }
            if (epdocs.Count <= 0)
                return _objHttpResponseMessage;
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result { EPDocument = epdocs };
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="inspectionId"></param>
        /// <param name="inspectionGroupId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetEpSteps(string epDetailId, string inspectionId, string inspectionGroupId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var mode = Convert.ToInt32(BDO.Enums.Mode.EP);
            var epdocs = _transactionService.GetGoalStepsByActivity(null, null, Convert.ToInt32(epDetailId),
                Convert.ToInt32(inspectionId), mode, 0, Convert.ToInt32(inspectionGroupId));
            if (epdocs != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    EPDocument = epdocs
                };
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetAllEpWhichHaveDocRequire(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var epdocs = _transactionService.GetAllEpDocs(Convert.ToInt32(userId));
            if (epdocs != null)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { EPDocument = epdocs };
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region ILSM Mapping

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetIlsmEPs()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var ilsmEPs = _epService.GetIlsmEPs();
            if (ilsmEPs.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { EPDetails = ilsmEPs };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetIlsmMatrix()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var ilsmMatrix = _mainGoalService.GetILSMMainGoal().Where(x => x.ActivityType == 0).ToList();
            var affectedEPs = _goalMasterService.GetAffectedEPs();
            if (ilsmMatrix.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { MainGoals = ilsmMatrix, AffectedEPs = affectedEPs };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateIlsmMatrix(AffectedEPs affectedEPs)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _ilsmService.UpdateIlsmMatrix(affectedEPs);
            _objHttpResponseMessage.Success = status;
            _objHttpResponseMessage.Result = new Result();
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage UpdateIlsmScore(List<Steps> steps)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = false;
            foreach (var item in steps)
            {
                status = _ilsmService.UpdateIlsmScore(item);
            }
            if (status)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Steps = _stepService.GetRASetps() };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region SOC

        //public HttpResponseMessage GetSoc()
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var soc = GoalMaster.GetSoc();
        //    if (soc.Count > 0)
        //    {
        //        objHttpResponseMessage.Success = true;
        //        objHttpResponseMessage.Result = new Result { SOC = soc };
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return objHttpResponseMessage;
        //}

        //public HttpResponseMessage GetSocMatrix()
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var socMatrix = GoalMaster.GetSocMatrix();
        //    if (socMatrix.Count > 0)
        //    {
        //        objHttpResponseMessage.Success = true;
        //        objHttpResponseMessage.Result = new Result { SOC = socMatrix };
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return objHttpResponseMessage;
        //}

        //public HttpResponseMessage UpdateSocMatrix(SOCMatrix socMatrix)
        //{
        //    objHttpResponseMessage = new HttpResponseMessage();
        //    var status = GoalMaster.UpdateSocMatrix(socMatrix);
        //    if (status)
        //    {
        //        objHttpResponseMessage.Success = true;
        //        objHttpResponseMessage.Result = new Result { SOC = GoalMaster.GetSocMatrix() };
        //    }
        //    else
        //    {
        //        objHttpResponseMessage.Success = false;
        //    }
        //    return objHttpResponseMessage;
        //}

        #endregion

        #region Risk_management


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetRiskManagementCount(string userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var objRisk = _goalMasterService.GetRiskCount(Convert.ToInt32(userId));
            _objHttpResponseMessage.Result = new Result { risk = objRisk };
            _objHttpResponseMessage.Success = true;
            return _objHttpResponseMessage;
        }

        #endregion
    }
}