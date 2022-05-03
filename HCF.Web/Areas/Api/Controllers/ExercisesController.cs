using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Hcf.Api.Application;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Areas.Api.Controllers
{
    [Route("api/exec")]
    [ApiController]
    public class ExercisesApiController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;
        private readonly IExercisesService _exercisesService;
        private readonly IApiCommon _apiCommon;
        private readonly IDigitalSignService _digitalSignService;
        private readonly IOrganizationService _organizationService;
        private readonly IPdfService _pdfService;

        public ExercisesApiController(IPdfService pdfService, IDigitalSignService digitalSignService, IApiCommon apiCommon, ILoggingService loggingService, IEmailService emailService, ICommonService commonService, IExercisesService exercisesService, IOrganizationService organizationService)
        {
            _pdfService = pdfService;
            _digitalSignService = digitalSignService;
            _apiCommon = apiCommon;
            _loggingService = loggingService;
            _emailService = emailService;
            _commonService = commonService;
            _exercisesService = exercisesService;
            _organizationService = organizationService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("AddTExercises")]
        public HttpResponseMessage AddTExercises(QuarterMaster newQuarterMaster)
        {
            var exercises = _exercisesService.GetExercises(null);
            var org = _organizationService.GetOrganization();
            var _objHttpResponseMessage = new HttpResponseMessage();
            foreach (var newExercises in newQuarterMaster.Exercises)
            {
                newExercises.Date = Conversion.ConvertToDateTime(newExercises.DateTimeSpan);
                if (!string.IsNullOrEmpty(newExercises.StartTime))
                {
                    var dateTime = DateTime.ParseExact(newExercises.StartTime, "hh:mm tt", CultureInfo.InvariantCulture);
                    var span = dateTime.TimeOfDay;
                    newExercises.Time = span;//TimeSpan.Parse(newExercises.StartTime);
                }
                else
                    newExercises.Time = null;

                List<int> signIds;
                List<int> critiquesignIds;
                List<int> educationsignIds;
                if (newExercises.Status == null)
                    newExercises.Status = 0;

                if (newExercises.TExerciseId > 0)
                {
                    var selectedexercise = exercises.Where(x => x.TExerciseId == newExercises.TExerciseId).FirstOrDefault();
                    newExercises.Name = selectedexercise.Name;
                    if (selectedexercise.NearBy == "") { selectedexercise.NearBy = null; }
                    if (newExercises.DigitalSignature.Count > 0)
                    {
                        signIds = SaveDigitalSignature(newExercises);
                        newExercises.SignIds = string.Join(",", signIds);
                    }
                    if (newExercises.CritiqueDigitalSignature.Count > 0)
                    {
                        critiquesignIds = SaveCritiqueDigitalSignature(newExercises);
                        newExercises.CritiqueSignIds = string.Join(",", critiquesignIds);
                    }
                    if (newExercises.EducationDigitalSignature.Count > 0)
                    {
                        educationsignIds = SaveEducationDigitalSignature(newExercises);
                        newExercises.EducationSignIds = string.Join(",", educationsignIds);
                    }
                    if (selectedexercise.Date != newExercises.Date || selectedexercise.Time != newExercises.Time || selectedexercise.NearBy != newExercises.NearBy || selectedexercise.Status != newExercises.Status
                        || newExercises.DigitalSignature.Count > 0 || newExercises.CritiqueDigitalSignature.Count > 0 || newExercises.EducationDigitalSignature.Count > 0)
                    {
                        _exercisesService.UpdateExercises(newExercises);
                    }
                    if (newExercises.Status == 1 && newExercises.Status != selectedexercise.Status)
                    {
                        if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Firedrill.ToString(), BDO.Enums.NotificationEvent.OnClosed.ToString()))
                        {
                            _emailService.SendFireDrillMailOnDone(newExercises, Convert.ToInt32(BDO.Enums.NotificationCategory.Firedrill));
                        }
                    }
                }
                else
                {
                    signIds = SaveDigitalSignature(newExercises);
                    newExercises.SignIds = string.Join(",", signIds);
                    critiquesignIds = SaveCritiqueDigitalSignature(newExercises);
                    newExercises.CritiqueSignIds = string.Join(",", critiquesignIds);
                    educationsignIds = SaveEducationDigitalSignature(newExercises);
                    newExercises.EducationSignIds = string.Join(",", educationsignIds);
                    var newExerciseId = _exercisesService.CreateExercises(newExercises);
                    newExercises.TExerciseId = newExerciseId;
                    if (newExerciseId > 0)
                    {
                        if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Firedrill.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString()))
                        {
                            _emailService.SendFireDrillCreatedMail(newExercises, Convert.ToInt32(BDO.Enums.NotificationCategory.Firedrill));
                        }
                    }
                }
                if (newExercises.TExerciseFiles != null && newExercises.TExerciseFiles.Count > 0)
                {
                    foreach (var exfiles in newExercises.TExerciseFiles)
                    {
                        exfiles.CreatedBy = newExercises.CreatedBy;
                    }
                    _apiCommon.SaveExercisesFiles(newExercises.TExerciseFiles, newExercises.TExerciseId);
                }
                if (newExercises.TExerciseQuestionnaires != null && newExercises.TExerciseQuestionnaires.Count > 0)
                {
                    foreach (var texerciseQuestionnaires in newExercises.TExerciseQuestionnaires)
                    {
                        texerciseQuestionnaires.TExerciseId = newExercises.TExerciseId;
                        SaveTExerciseQuestionnaires(texerciseQuestionnaires);
                    }
                }
                if (newExercises.TExerciseActions != null && newExercises.TExerciseActions.Count > 0)
                {
                    _exercisesService.DeleteTExerciseActionsbyTExerciseId(newExercises.TExerciseId);
                    foreach (var exerActions in newExercises.TExerciseActions)
                    {
                        if (!string.IsNullOrEmpty(exerActions.Issue))
                        {
                            exerActions.TExerciseId = newExercises.TExerciseId;
                            exerActions.CreatedBy = newExercises.CreatedBy;
                            SaveTExerciseActions(exerActions);
                        }
                    }
                }
            }
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Message = ConstMessage.Success;
            _objHttpResponseMessage.Result = new Result { QuarterMasterSetting = newQuarterMaster };
            return _objHttpResponseMessage;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private int SaveTExerciseActions(TExerciseActions exerActions)
        {
            return _exercisesService.InsertTExerciseActions(exerActions);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private List<int> SaveDigitalSignature(TExercises newExercises)
        {
            var signIds = new List<int>();
            if (newExercises.DigitalSignature != null)
            {
                foreach (var item in newExercises.DigitalSignature)
                {
                    if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FileContent) && item.DigSignatureId == 0)
                    {
                        item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "DigitalSignPath", "ExerciseDigSign").FilePath;
                        item.DigSignatureId = _digitalSignService.Save(item);
                        //if (!item.IsDeleted)
                        //    signIds.Add(signId);
                    }
                    else if (item.DigSignatureId > 0)
                    {
                        if (item.IsDeleted)
                            _digitalSignService.UpdateDigitalSignature(item);
                        // if (!item.IsDeleted)
                        //    signIds.Add(item.DigSignatureId);
                    }
                }
                signIds = newExercises.DigitalSignature.Where(x => x.IsDeleted == false && x.DigSignatureId > 0).Select(x => x.DigSignatureId).ToList();
            }
            return signIds;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private List<int> SaveCritiqueDigitalSignature(TExercises newExercises)
        {
            var signIds = new List<int>();
            if (newExercises.CritiqueDigitalSignature != null)
            {
                foreach (var item in newExercises.CritiqueDigitalSignature)
                {
                    if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FileContent) && item.DigSignatureId == 0)
                    {
                        item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "DigitalSignPath", "ExerciseDigSign").FilePath;
                        item.DigSignatureId = _digitalSignService.Save(item);
                        //if (!item.IsDeleted)
                        //    signIds.Add(signId);
                    }
                    else if (item.DigSignatureId > 0)
                    {
                        if (item.IsDeleted)
                            _digitalSignService.UpdateDigitalSignature(item);
                        // if (!item.IsDeleted)
                        //    signIds.Add(item.DigSignatureId);
                    }
                }
                signIds = newExercises.CritiqueDigitalSignature.Where(x => x.IsDeleted == false && x.DigSignatureId > 0).Select(x => x.DigSignatureId).ToList();
            }
            return signIds;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private List<int> SaveEducationDigitalSignature(TExercises newExercises)
        {
            var signIds = new List<int>();
            if (newExercises.EducationDigitalSignature != null)
            {
                foreach (var item in newExercises.EducationDigitalSignature)
                {
                    if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.FileContent) && item.DigSignatureId == 0)
                    {
                        item.FilePath = _apiCommon.SaveFile(item.FileContent, item.FileName, "DigitalSignPath", "ExerciseDigSign").FilePath;
                        item.DigSignatureId = _digitalSignService.Save(item);
                        //if (!item.IsDeleted)
                        //    signIds.Add(signId);
                    }
                    else if (item.DigSignatureId > 0)
                    {
                        if (item.IsDeleted)
                            _digitalSignService.UpdateDigitalSignature(item);
                        // if (!item.IsDeleted)
                        //    signIds.Add(item.DigSignatureId);
                    }
                }
                signIds = newExercises.EducationDigitalSignature.Where(x => x.IsDeleted == false && x.DigSignatureId > 0).Select(x => x.DigSignatureId).ToList();
            }
            return signIds;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        private void SaveTExerciseQuestionnaires(TExerciseQuestionnaires texerciseQuestionnaires)
        {
            _exercisesService.InsertTExerciseQuestionnaires(texerciseQuestionnaires);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetExercises()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var lists = _exercisesService.GetExercises(null);
            if (lists.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { Exercises = lists };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        /// <summary>
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage DeleteExercise(string exerciseId)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var status = _exercisesService.DeleteExercises(Convert.ToInt32(exerciseId));
            _objHttpResponseMessage.Success = status;
            _objHttpResponseMessage.Message = status ? ConstMessage.Delete_Successfully : ConstMessage.Error;
            return _objHttpResponseMessage;
        }


        //Reminder Emails before 1 days 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetFiredrillNotification")]
        public HttpResponseMessage GetFiredrillNotification()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TExercises> reminderExercises = new List<TExercises>();
            List<TExercises> dueDateExercises = new List<TExercises>();
            DateTime today = DateTime.Now;
            var exercise = _exercisesService.GetExercises(null).Where(x => x.IsDeleted == false).ToList();
            foreach (var exerise in exercise)
            {
                DateTime date = Conversion.ConvertToDateTime(exerise.DateTimeSpan);
                var numberOfDays = Convert.ToInt32((date - today).TotalDays);
                if (numberOfDays == 1)
                {
                    reminderExercises.Add(exerise);
                }
                else if (numberOfDays < 0)
                {
                    dueDateExercises.Add(exerise);
                }
            }
            //send Reminder Email
            if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Firedrill.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString()))
            {
                foreach (var item in reminderExercises)
                {
                    _emailService.SendFireDrillReminderMail(item, Convert.ToInt32(BDO.Enums.NotificationCategory.Firedrill));
                }
            }

            // Send Email on Due Date Passed
            if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Firedrill.ToString(), BDO.Enums.NotificationEvent.OnDueDate.ToString()))
            {
                foreach (var item in dueDateExercises)
                {
                    _emailService.SendFireDrillOnDueDatePassed(item, Convert.ToInt32(BDO.Enums.NotificationCategory.Firedrill));
                }
            }
            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("generateFiredrillDoc")]
        public HttpResponseMessage GenerateFiredrillReport(TExercises exercises)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var base64EncodePdf = _pdfService.GenerateFireDrillReportInbytes(exercises.TExerciseId);
            if (!string.IsNullOrEmpty(base64EncodePdf))
            {
                var files = new TExerciseFiles();
                var fileName = "Firedrill_" + exercises.Building.BuildingName.Trim() + "_Q" + exercises.QuarterNo + "_" + exercises.Shift.Name.ToString() + ".pdf";
                var fName = CommonUtility.CreateFileName(fileName);
                _objHttpResponseMessage.Success = true;
                string path = _apiCommon.SaveFile(base64EncodePdf, fName, "Firedrill", exercises.BuildingId.ToString()).FilePath;
                files.TExerciseId = exercises.TExerciseId;
                files.CreatedBy = exercises.CreatedBy;
                files.FileName = fileName;
                files.FilePath = path;
                files.DrillFileType = BDO.Enums.DrillFileType.Evaluation;
                files.DocumentType = Convert.ToInt32(BDO.Enums.FileType.FireDrillReport);
                files.IsActive = true;
                int val = _exercisesService.InsertTExerciseFiles(files);
                _objHttpResponseMessage.Result = new Result { result = base64EncodePdf };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }
            return _objHttpResponseMessage;
        }


        #region Exercise Settings

        /// <summary>
        /// </summary>
        /// <param name="newQuarterMaster"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage AddQuarterSettings(QuarterMaster newQuarterMaster)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (newQuarterMaster.QuarterId > 0)
            {
                _exercisesService.UpdateQuarterMasterSettings(newQuarterMaster);
            }
            else
            {
                var newQuarterId = _exercisesService.InsertQuarterMasterSettings(newQuarterMaster);
                newQuarterMaster.QuarterId = newQuarterId;
            }
            if (newQuarterMaster.QuarterId > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Message = ConstMessage.Success;
                _objHttpResponseMessage.Result = new Result { QuarterMasterSetting = newQuarterMaster };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
                _objHttpResponseMessage.Message = ConstMessage.Record_Already_Exists;
            }
            return _objHttpResponseMessage;
        }


        /// <summary>
        /// </summary>
        /// <param name="buildingTypeId"></param>
        /// <param name="quarterNo"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        //public HttpResponseMessage GetQuarterSettings(string buildingTypeId, string quarterNo, string year)
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();
        //    var lists = _exercisesService.GetQuarterSettings(Convert.ToInt32(buildingTypeId), Convert.ToInt32(quarterNo), Convert.ToInt32(year), false);
        //    if (lists.Count > 0)
        //    {
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result = new Result { QuarterMasterSettings = lists };
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
        //    }
        //    return _objHttpResponseMessage;
        //}

        #endregion

        #region Fire Drill Questionnaries

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// 
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("addFiredrillQuestionnaries")]
        public HttpResponseMessage AddFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            if (newFireDrillQuestionnaires.FireDrillQuesId > 0)
            {
                _exercisesService.UpdateFiredrillQuestionnaries(newFireDrillQuestionnaires);
                _objHttpResponseMessage.Message = ConstMessage.Success_Updated;
            }
            else
            {
                var newFireDrillQuesId = _exercisesService.InsertFiredrillQuestionnaries(newFireDrillQuestionnaires);
                newFireDrillQuestionnaires.FireDrillQuesId = newFireDrillQuesId;
                _objHttpResponseMessage.Message = ConstMessage.Success;
            }
            _objHttpResponseMessage.Success = true;
            _objHttpResponseMessage.Result = new Result();
            return _objHttpResponseMessage;
        }




        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public HttpResponseMessage GetFiredrillQuestionnaries()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var lists = _exercisesService.GetFiredrillQuestionnaries();
            if (lists.Count > 0)
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { FireDrillCategory = lists };
            }
            else
            {
                _objHttpResponseMessage.Message = ConstMessage.No_Records_Found;
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region Create Documents

        //public HttpResponseMessage CreateFiredrillMatrix(string buildingTypeId, string quarterNo, string year)
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();
        //    //  string fileContent = string.Empty;            
        //    var base64EncodePdf = _pdfService.PrintFiredrillMatrixInbytes(Convert.ToInt32(buildingTypeId), Convert.ToInt32(quarterNo), Convert.ToInt32(year));
        //    if (!string.IsNullOrEmpty(base64EncodePdf))
        //    {
        //        _objHttpResponseMessage.Success = true;
        //        _objHttpResponseMessage.Result = new Result { result = base64EncodePdf };
        //    }
        //    else
        //    {
        //        _objHttpResponseMessage.Success = false;
        //    }

        //    return _objHttpResponseMessage;
        //}



        //Create document method

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public HttpResponseMessage CreateDocument()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            string fileName = "SampleFile.doc";
            string filePath = string.Empty;
            // CreateWordDocument(out filePath);
            var stream = new FileStream(Path.Combine(Path.GetTempPath(), "SaveFile.doc"), FileMode.Open);
            byte[] byteArray = ReadFully(stream);//new byte[stream.Length];
            var fileContent = Convert.ToBase64String(byteArray);
            if (!string.IsNullOrEmpty(fileContent))
                filePath = _apiCommon.SaveFile(fileContent, fileName, "exercise", "FireDrillForm").FilePath;
            if (!string.IsNullOrEmpty(filePath))
            {
                _objHttpResponseMessage.Success = true;
                _objHttpResponseMessage.Result = new Result { result = filePath };
            }
            else
            {
                _objHttpResponseMessage.Success = false;
            }

            return _objHttpResponseMessage;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }



        #endregion

        #region generate Firedrill Doc



        #endregion
    }
}