using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Areas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;



namespace HCF.Web.Areas.Api.Controllers
{

    [Route("api/rounds")]
    [ApiController]
    public class RoundsApiController : ApiController
    {
        #region ctor

        private readonly ILoggingService _loggingService;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;
        private readonly IApiCommon _apiCommon;
        private readonly IDigitalSignService _digitalSignService;
        private readonly IRoundsService _roundsService;
        private readonly IRoundInspectionsService _roundInspectionsService;
        public RoundsApiController(IDigitalSignService digitalSignService, IApiCommon apiCommon, ILoggingService loggingService, IEmailService emailService,
            ICommonService commonService, IRoundsService roundsService, IRoundInspectionsService roundInspectionsService)
        {
            _digitalSignService = digitalSignService;
            _apiCommon = apiCommon;
            _commonService = commonService;
            _loggingService = loggingService;
            _emailService = emailService;
            _roundsService = roundsService;
            _roundInspectionsService = roundInspectionsService;
        }

        #endregion

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddRoundsQuestionnaires")]
        public HttpResponseMessage AddRoundsQuestionnaires(RoundsQuestionnaires questionnaires)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            questionnaires.RouQuesId = _roundsService.AddRoundsQuestionnaires(questionnaires);
            if (questionnaires.RouQuesId > 0)
            {
                _objHttpResponseMessage = new HttpResponseMessage
                {
                    Success = true,
                    Result = new Result
                    {
                        RoundsQuestionnaires = questionnaires
                    },
                    Message = ConstMessage.Success
                };
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addRoundInspection/{isComplete}")]
        public HttpResponseMessage AddRoundInspection(TRounds newRoundInsp, bool isComplete)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            newRoundInsp.Status = isComplete ? CalculateRoundStatus(newRoundInsp, isComplete) : Convert.ToInt32(BDO.Enums.Status.In_Progress);


            newRoundInsp.TRoundId = _roundsService.AddRoundInspection(newRoundInsp, "", "");
            if (newRoundInsp.TRoundId > 0)
            {
                foreach (var inspRoundItems in newRoundInsp.TRoundsQuestionnaires)
                {
                    inspRoundItems.TRoundId = newRoundInsp.TRoundId;
                    inspRoundItems.TRQuesId = _roundsService.AddRoundItemsInspection(inspRoundItems);
                }
                if (newRoundInsp.DigitalSignature != null)
                {
                    if (!string.IsNullOrEmpty(newRoundInsp.DigitalSignature.FileName))
                    {
                        newRoundInsp.DigitalSignature.TRoundId = newRoundInsp.TRoundId;
                        newRoundInsp.DigitalSignature.CreatedBy = newRoundInsp.CreatedBy;
                        if (!string.IsNullOrEmpty(newRoundInsp.DigitalSignature.FileContent))
                            newRoundInsp.DigitalSignature.FilePath = _apiCommon.SaveFile(newRoundInsp.DigitalSignature.FileContent,
                                newRoundInsp.DigitalSignature.FileName,
                                "DigitalSignPath", newRoundInsp.TRoundId.ToString()).FilePath;
                        else if (!string.IsNullOrEmpty(newRoundInsp.DigitalSignature.FilePath))
                            newRoundInsp.DigitalSignature.FilePath = newRoundInsp.DigitalSignature.FilePath;
                        newRoundInsp.DigitalSignature.DigSignatureId =
                            _digitalSignService.Save(newRoundInsp.DigitalSignature);
                    }
                }
                //if (newRoundInsp.Status.Value == 0)
                //    Email.RoundStatusMail(newRoundInsp);
                if (newRoundInsp.Status == 0)
                {
                    if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.RoundsInspection.ToString(), BDO.Enums.NotificationEvent.OnFailed.ToString()))
                    {
                        _emailService.SendRoundFailMail(newRoundInsp, Convert.ToInt32(BDO.Enums.NotificationCategory.RoundsInspection));
                    }
                }
                _objHttpResponseMessage.Message = ConstMessage.Round_Inspection_Added_Successfully;
                _objHttpResponseMessage.Result = new Result
                {
                    TRound = newRoundInsp
                };
                _objHttpResponseMessage.Success = true;
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.Error;
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddRoundsCommQuestonaries")]
        public HttpResponseMessage AddRoundsCommonQuestionnaires(RoundsQuestionnaires questionnaires)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            questionnaires.RouQuesId = _roundsService.AddRoundsQuestionnaires(questionnaires);
            if (questionnaires.RouQuesId > 0)
            {
                _objHttpResponseMessage = new HttpResponseMessage
                {
                    Success = true,
                    Result = new Result
                    {
                        RoundsQuestionnaires = questionnaires
                    },
                    Message = ConstMessage.Success
                };
            }
            return _objHttpResponseMessage;
        }

        #region v2 version api for rounds
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/getAllRoundsInspectionv2")]
        public HttpResponseMessageApp getAllRoundsInspectionv2(string start, int? locationGroupId, int? userId, int? day)
        {
            var _objHttpResponseMessage = new HttpResponseMessageApp();
            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : DateTime.Now.Date;
            List<RoundGroup> lists = new List<RoundGroup>();
            //var lists = null;
            lists = _roundInspectionsService.GetRoundByDateApp(roundDate, locationGroupId, userId, true, day.HasValue && day.Value > 0 ? day.Value : 0, roundDate);
            if (lists.Count > 0)
            {
                var ress = JsonConvert.DeserializeObject<List<RoundGroupApp>>(JsonConvert.SerializeObject(lists));
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new ResultApp { Rounds = ress };

            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/getRoundsInspection")]
        public HttpResponseMessage getRoundsInspection(string start, int? locationGroupId, int? userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : DateTime.Now.Date;
            var lists = _roundInspectionsService.GetRoundByDate(roundDate, locationGroupId, userId, true);
            if (lists.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result
                {
                    RoundGroup = lists
                };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/getAllRoundsInspection")]
        public HttpResponseMessageApp getAllRoundsInspection(string start, int? locationGroupId, int? userId)
        {
            var _objHttpResponseMessage = new HttpResponseMessageApp();
            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : DateTime.Now.Date;
            var lists = _roundInspectionsService.GetRoundByDate(roundDate, locationGroupId, userId, true);
            if (lists.Count > 0)
            {
                var ress = JsonConvert.DeserializeObject<List<RoundGroupApp>>(JsonConvert.SerializeObject(lists));
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new ResultApp { Rounds = ress };

            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("app/roundQuestionaries")]
        public HttpResponseMessage roundQuestionaries()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var roundsCateg = _roundsService.GetRoundCategories_Data().Where(x => x.IsActive == true).ToList();

            foreach (var item in roundsCateg)
            {
                if (item != null && item.RoundItems != null && item.RoundItems.Count > 0)
                {
                    item.RoundItems = item.RoundItems.Where(x => x.IsActive == true).ToList();
                }
            }

            if (roundsCateg.Count > 0)
            {
                var ress = JsonConvert.DeserializeObject<List<RoundCategory>>(JsonConvert.SerializeObject(roundsCateg));
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { RoundCategorys = ress };

            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }

            return _objHttpResponseMessage;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("app/InspectRounds")]
        public HttpResponseMessage InspectRounds(List<TRounds> Rounds)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var IsSucess = false;
            string TRoundIds = string.Empty;
            foreach (var newRoundInsp in Rounds)
            {
                newRoundInsp.TRoundId = _roundsService.AddRoundInspection(newRoundInsp, "", "");
                if (newRoundInsp.TRoundId > 0)
                {

                    IsSucess = true;
                }
                else
                {
                    IsSucess = false;
                    _objHttpResponseMessage.Message = ConstMessage.Error;
                    _objHttpResponseMessage.Success = false;
                    return _objHttpResponseMessage;
                }
            }
            if (IsSucess)
            {
                _objHttpResponseMessage = new HttpResponseMessage
                {
                    Success = true,
                    Result = new Result
                    {
                        Rounds = Rounds
                    },
                    Message = ConstMessage.Success
                };
            }
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("app/SaveRoundInspection")]
        public HttpResponseMessage SaveRoundInspection(RoundInspections RoundInspections)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<int> roundcategories = new List<int>();
            List<int> insproundcategories = new List<int>();
            bool result = true;
            bool IsFirstInspect = false;
            bool closeforvolunteer = false;
            if (RoundInspections != null && !string.IsNullOrEmpty(RoundInspections.RoundScheduleId) && !string.IsNullOrEmpty(RoundInspections.RoundDate.ToString()) && RoundInspections.UserId > 0 && RoundInspections.Inspections != null && RoundInspections.Inspections.Count > 0)
            {
                if (RoundInspections != null && RoundInspections.Inspections != null && RoundInspections.Inspections.Count > 0)
                {
                    if (RoundInspections.Status == 3)
                    {
                        closeforvolunteer = true;
                        RoundInspections.Status = 1;

                    }
                    if (RoundInspections.Inspections.FirstOrDefault().TRoundId == 0)
                    {
                        IsFirstInspect = true;
                    }
                    if (RoundInspections != null && !string.IsNullOrEmpty(RoundInspections.AdditionalUserIds) && RoundInspections.RoundType == 2)
                    {
                        string[] UserIds = RoundInspections.AdditionalUserIds.Split(',');
                        foreach (string retUserId in UserIds)
                        {
                            if (!string.IsNullOrEmpty(retUserId))
                            {
                                if (retUserId != "")
                                {
                                    var roundGroupuser = new RoundGroupUsers
                                    {
                                        UserId = Convert.ToInt32(retUserId),
                                        RoundGroupId = RoundInspections.RoundGroupId,
                                        IsActive = true
                                    };
                                    _roundInspectionsService.InsertOrUpdateGroup(roundGroupuser);
                                }
                            }

                        }

                    }

                    if (RoundInspections.FloorId > 0)
                    {

                        RoundGroupLocations objroundGroupLocations = new RoundGroupLocations();
                        objroundGroupLocations.RoundGroupId = Convert.ToInt32(RoundInspections.RoundGroupId);
                        objroundGroupLocations.BuildingId = -1;
                        objroundGroupLocations.FloorId = Convert.ToInt32(RoundInspections.FloorId);
                        objroundGroupLocations.IsActive = true;
                        objroundGroupLocations.CreatedBy = RoundInspections.UserId;
                        var roundGroupLocationId = _roundInspectionsService.SaveRoundGroupLocations(objroundGroupLocations);
                    }
                    string[] scheduleid = RoundInspections.RoundScheduleId.Split(",");
                    if (RoundInspections.RoundType == 2)
                    {
                        //int extrascheduleid = RoundInspections.Inspections.Count() - scheduleid.Length;
                        Array.Resize(ref scheduleid, RoundInspections.Inspections.Count());
                    }
                    int i = 0;
                    if (scheduleid.Length > 0)
                    {



                        var roundgroupusers = _roundInspectionsService.roundUserGroups(0).Where(x => x.RoundGroupId == RoundInspections.RoundGroupId).FirstOrDefault();
                        if (roundgroupusers != null && !string.IsNullOrEmpty(roundgroupusers.RoundCategories))
                            roundcategories = roundgroupusers.RoundCategories.Split(',').Select(x => int.Parse(x)).ToList();
                        foreach (var inspection in RoundInspections.Inspections)
                        {

                            if (RoundInspections.RoundType == 2)
                            {
                                if (!string.IsNullOrEmpty(scheduleid[i]))
                                {
                                    if (Convert.ToString(scheduleid[i]) == "0" || Convert.ToString(scheduleid[i]) == "")
                                    {
                                        roundcategories.Add(Convert.ToInt32(Convert.ToInt32(inspection.RoundCatId)));
                                        var tRoundgroup = new RoundGroupUsers
                                        {
                                            RoundGroupId = RoundInspections.RoundGroupId,
                                            IsActive = true,
                                            UserId = 0,
                                            RoundCategories = string.Join(",", roundcategories.Distinct())
                                        };
                                        var Roundgroup = _roundInspectionsService.GetRoundUserGroup().Where(x => x.IsActive == 1 && x.RoundGroupId == RoundInspections.RoundGroupId).ToList();
                                        if (Roundgroup != null && Roundgroup.Count > 0)
                                        {
                                            var tRoundGroup = new RoundSchedules
                                            {
                                                RoundCatId = Convert.ToInt32(inspection.RoundCatId),
                                                RoundGroupId = RoundInspections.RoundGroupId,
                                                FrequencyId = (BDO.Enums.Frequency)Roundgroup.FirstOrDefault().Frequency,
                                                StartInitialDate = Convert.ToDateTime(Roundgroup.FirstOrDefault().StartDate),

                                            };
                                            var dates = "";

                                            var roundDates = GetRoundScheduleDates(tRoundGroup.FrequencyId, tRoundGroup.StartInitialDate, Roundgroup.FirstOrDefault().Ocurrence, Convert.ToDateTime(Roundgroup.FirstOrDefault().EndDate), Roundgroup.FirstOrDefault().ReoccurEvery);
                                            if (roundDates != null && roundDates.Count > 0)
                                                dates = string.Join(",", roundDates.Select(x => x.RoundDate).Distinct());
                                            int retval = _roundInspectionsService.InsertOrUpdateGroupUserType(tRoundGroup, dates);
                                            scheduleid[i] = Convert.ToString(retval);
                                        }

                                        _roundInspectionsService.InsertOrUpdateGroup(tRoundgroup);


                                    }


                                }
                            }

                            if (!string.IsNullOrEmpty(scheduleid[i]))
                            {

                                if (inspection.TRoundId == 0)
                                {

                                    TRounds newRoundInsp = new TRounds()
                                    {
                                        RoundCatId = inspection.RoundCatId,
                                        RoundScheduleId = Convert.ToInt32(scheduleid[i]),
                                        RoundDate = RoundInspections.RoundDate,
                                        CreatedBy = RoundInspections.UserId,
                                        RoundName = RoundInspections.RoundName,
                                        Status = RoundInspections.Status

                                    };

                                    inspection.TRoundId = _roundsService.AddRoundInspection(newRoundInsp, "", "");
                                }

                                if (inspection.TRoundId > 0)
                                {
                                    if (RoundInspections.FloorId > 0)
                                    {
                                        _roundsService.SaveRoundFloorInspection(RoundInspections.FloorId, inspection.TRoundId, RoundInspections.UserId);
                                    }

                                    inspection.Status = RoundInspections.Status;
                                    inspection.FloorId = RoundInspections.FloorId;
                                    inspection.UserId = RoundInspections.UserId;
                                    if (inspection.FloorId > 0)
                                    {
                                        if (inspection.Questionnaires != null && inspection.Questionnaires.Count > 0)
                                        {
                                            foreach (var question in inspection.Questionnaires)
                                            {
                                                question.TRoundId = inspection.TRoundId;
                                                int roundCatId = question.RoundsQuestionnaires != null ? question.RoundsQuestionnaires.RoundCatId : 0;
                                                var retval = _roundsService.SaveRoundInspectionSteps(RoundInspections.FloorId, RoundInspections.UserId, inspection.Status, inspection.Comment, question, roundCatId);
                                                if (!retval)
                                                {
                                                    result = false;
                                                }

                                                if (question.Status == 0)
                                                {

                                                    var failResult = _roundsService.SaveRoundFailStatus(question.Status, inspection.TRoundId);
                                                }
                                            }


                                        }
                                        else
                                        {

                                            if (RoundInspections.Status == 1 && !closeforvolunteer)
                                            {
                                                var rounds = new TRounds { TRoundId = inspection.TRoundId, Status = RoundInspections.Status };
                                                result = _roundsService.SaveRoundStatus(rounds, HCF.Web.Base.UserSession.CurrentUser.UserId);
                                            }
                                            else
                                            {
                                                if (closeforvolunteer)
                                                {
                                                    inspection.Status = 3;

                                                }
                                                inspection.IsOpen = true;
                                                result = _roundsService.SaveRoundInspection(inspection);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (IsFirstInspect)
                                        {

                                        }
                                        else
                                        {
                                            _objHttpResponseMessage.Message = "Floor Id value or key is missing in body";
                                            _objHttpResponseMessage.Success = false;
                                            return _objHttpResponseMessage;
                                        }

                                    }


                                    if (!result)
                                    {
                                        _objHttpResponseMessage.Message = ConstMessage.Error;
                                        _objHttpResponseMessage.Success = false;
                                        return _objHttpResponseMessage;
                                    }
                                }
                            }
                            else
                            {


                            }
                            i++;
                        }
                    }
                    else
                    {
                        _objHttpResponseMessage.Message = "RoundScheduleId value or key is missing in body or inspection list count not matching with RoundScheduleId values count";
                        _objHttpResponseMessage.Success = false;
                        return _objHttpResponseMessage;
                    }

                }
                else
                {

                    //string[] scheduleid = RoundInspections.RoundScheduleId.Split(",");
                    //foreach (string id in scheduleid)
                    //{
                    //    TRounds newRoundInsp = new TRounds()
                    //    {
                    //        RoundScheduleId = Convert.ToInt32(id),
                    //        RoundDate = RoundInspections.RoundDate,
                    //        CreatedBy = RoundInspections.UserId,
                    //        RoundName = RoundInspections.RoundName

                    //    };
                    //    _roundsService.AddRoundInspection(newRoundInsp);
                    //}


                }







            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.MandatoryFieldsError;
                _objHttpResponseMessage.Success = false;
                return _objHttpResponseMessage;
            }


            _objHttpResponseMessage = new HttpResponseMessage
            {
                Success = true,
                Message = ConstMessage.Success
            };
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private List<RoundScheduleDates> GetRoundScheduleDates(BDO.Enums.Frequency frequency, DateTime startDate, string Occurence, DateTime? endtime = null, int? recurecery = 1, int? recurfor = 1, int? month = null, int? year = null, int? week = null, int? day = null, int? The = null)
        {
            var roundScheduleDates = new List<RoundScheduleDates>();

            if (endtime != null && endtime.Value.ToString() == "1/1/0001 12:00:00 AM")
            {
                endtime = null;
            }
            var dates = ScheduleDateTime.GetDates((int)frequency, startDate, month.HasValue ? month.Value : 0, week.HasValue ? week.Value : 0, day.HasValue ? day.Value : 0, Occurence, endtime, recurecery.HasValue ? recurecery.Value : 1, The.HasValue ? The.Value : 0, recurfor.HasValue ? recurfor.Value : 1).Distinct().OrderBy(x => x.Date);
            foreach (var date in dates)
            {
                if (date.Date >= DateTime.Now.Date)
                {
                    var roundScheduleDate = new RoundScheduleDates();
                    if (date.DayOfWeek == DayOfWeek.Saturday && frequency != BDO.Enums.Frequency.Daily) { roundScheduleDate.RoundDate = date.AddDays(2); }
                    else if (date.DayOfWeek == DayOfWeek.Sunday && frequency != BDO.Enums.Frequency.Daily) { roundScheduleDate.RoundDate = date.AddDays(1); }
                    else if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) && (frequency == BDO.Enums.Frequency.Daily && (recurfor.HasValue && recurfor == 2)))
                    { }
                    else
                        roundScheduleDate.RoundDate = date;
                    roundScheduleDates.Add(roundScheduleDate);


                }


            }

            return roundScheduleDates;
        }
        #endregion
        #region Private methods

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        private static int CalculateRoundStatus(TRounds newRoundInsp, bool isComplete)
        {
            var totalCount = 0;
            var passCount = 0;
            int roundStatus;
            if (isComplete == false)
            {
                roundStatus = Convert.ToInt32(BDO.Enums.Status.In_Progress);
            }
            else
            {
                foreach (var roundsQuestionnaire in newRoundInsp.TRoundsQuestionnaires)
                {
                    totalCount += newRoundInsp.TRoundsQuestionnaires.Count;
                    passCount += newRoundInsp.TRoundsQuestionnaires.Count(m => m.Status == 1);
                }
                roundStatus = Convert.ToInt32(totalCount == passCount ? BDO.Enums.Status.Pass : BDO.Enums.Status.Fail);
            }
            return roundStatus;
        }

        #endregion
    }
}
