using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class IlsmService :IIlsmService
    {
        private readonly ICommonService _commonService;
        private readonly IEmailService _emailService; 
        private readonly ITransactionService _transactionservice;
        private readonly IIlsmRepository _ilsmRepository;
        private readonly IInsStepsRepository _insStepsRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly ITransaction _transactionRepository;

        public IlsmService(IStepsRepository stepsRepository, ITransaction transactionRepository,ICommonService commonService, IEmailService emailService, IInsStepsRepository insStepsRepository,
           IIlsmRepository ilsmRepository,
            ITransactionService transactionservice)
        {
            _stepsRepository = stepsRepository;
            _transactionRepository = transactionRepository;
            _insStepsRepository = insStepsRepository;
            _transactionservice = transactionservice;
            _commonService = commonService;
            _emailService = emailService;
            _ilsmRepository = ilsmRepository;



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="affectedEps"></param>
        /// <returns></returns>
        public  bool UpdateIlsmMatrix(AffectedEPs affectedEps)
        {
            return _ilsmRepository.UpdateIlsmMatrix(affectedEps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public  bool UpdateIlsmScore(Steps steps)
        {
            return _ilsmRepository.UpdateIlsmScore(steps);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="stepId"></param>
       /// <param name="ilsmStepId"></param>
       /// <returns></returns>
        public  bool AddStepWithIlLsmStep(int stepId, int ilsmStepId)
        {
            return _ilsmRepository.AddStepWithIlLsmStep(stepId, ilsmStepId);
        }

        public  List<TIlsm> GetIlsm()
        {
            return _ilsmRepository.GetIlsm();
        }

        public  List<TIlsm> GetIlsmAsync()
        {
            return  _ilsmRepository.GetIlsmAsync();
        }

        public  TIlsm GetIlsm(int tilsmId)
        {
            return _ilsmRepository.GetAllIlsm(null,null,tilsmId).FirstOrDefault();
        }

        public  List<TIlsm> GetAllIlsm(DateTime? fromDate = null, DateTime? toDate = null)
        {
            return _ilsmRepository.GetAllIlsm(fromDate, toDate);
        }

        public  TIlsm GetIlsmInspection(int tilsmId)
        {
            var tilsminspection = _ilsmRepository.GetIlsmInspection(tilsmId);

            if (tilsminspection.TinspectionActivity.Any())
            {
                var currentActivity = tilsminspection.TinspectionActivity.FirstOrDefault(x => x.IsCurrent);
                var stepscomments = _stepsRepository.GetStepsComments(currentActivity.ActivityId, tilsmId);
                var files = _transactionRepository.GetIlsmInspectionFiles(currentActivity.ActivityId);
                currentActivity.TInspectionFiles = files.Where(x => x.StepsId == 0 || x.StepsId == null).ToList(); //DAL.Transaction.GetIlsmInspectionFiles(currentActivity.ActivityId);
                tilsminspection.IlsmComments = new List<TComment>();
                tilsminspection.IlsmComments = stepscomments.Where(x => x.StepId == 0).ToList();
                tilsminspection.EpStepsComplete = currentActivity.Status == 1;

                var goals = new List<MainGoal>();
                foreach (var details in currentActivity.TInspectionDetail)
                {
                    MainGoal goal;
                    goal = details.MainGoal;

                    goal.InspectionDetailId = details.InspectionDetailId;
                    goal.Steps = new List<Steps>();
                    var steps = details.InspectionSteps;
                    foreach (var step in steps)
                    {
                        Steps maingoalStep;
                        maingoalStep = step.Steps;
                        maingoalStep.Status = step.Status;
                        maingoalStep.FileName = step.FileName;
                        maingoalStep.Comments = step.Comments;
                        maingoalStep.FilePath = step.FilePath;
                        maingoalStep.StepsComments = new List<TComment>();
                        maingoalStep.StepsComments = stepscomments.Where(x => x.StepId == step.StepsId).ToList();
                        maingoalStep.TInspectionFiles = new List<TInspectionFiles>();
                        maingoalStep.TInspectionFiles = files.Where(x => x.StepsId == step.StepsId).ToList();
                        goal.Steps.Add(maingoalStep);
                    }
                    goals.Add(goal);
                }

                tilsminspection.MainGoal = goals;//.OrderBy(x => x.MainGoalId).ToList();
                tilsminspection.TInspectionFiles = currentActivity.TInspectionFiles;
                tilsminspection.Comment = currentActivity.Comment;
            }
            else
            {
                var temactivity = new TInspectionActivity
                {
                    ActivityId = Guid.Empty
                };
                tilsminspection.TinspectionActivity = new List<TInspectionActivity>();
                tilsminspection.TinspectionActivity.Add(temactivity);
            }
            return tilsminspection;
        }

        public  TIlsm GetIlsmDetails(int tilsmId)
        {
            var tilsminspection = _ilsmRepository.GetIlsmDetails(tilsmId);
            if (tilsminspection.TinspectionActivity.Any())
            {
                var currentActivity = tilsminspection.TinspectionActivity.FirstOrDefault(x => x.IsCurrent);
                //List<TComment> stepscomments = StepsRepository.GetStepsComments(currentActivity.ActivityId, tilsmId);
                if (currentActivity != null)
                {
                    currentActivity.TInspectionFiles = _transactionRepository
                        .GetIlsmInspectionFiles(currentActivity.ActivityId)
                        .Where(x => x.StepsId == 0 || x.StepsId == null).ToList();
                    tilsminspection.TInspectionFiles = currentActivity.TInspectionFiles;
                }
            }
            return tilsminspection;
        }

        public  bool ILSMlinkToWO(int tilsmId, int issueId)
        {
            return _ilsmRepository.IlsMlinkToWo(tilsmId, issueId);
        }

        public  bool UpdateILSMStatus(TIlsm tilsmId)
        {
            return _ilsmRepository.UpdateIlsmStatus(tilsmId);
        }

        public  bool SaveAdditionalTilsmEP(int epDetailId, int tilsmId, bool isActive)
        {
            return _ilsmRepository.SaveAdditionalTilsmEp(epDetailId, tilsmId, isActive);
        }

        public  void UpdateIlsmStatus(Guid? activityId = null)
        {
            //Change the deficiency code of tinspectionSteps
            UpdateTinspectionSteps();

            //Change the Substatus code of inspection & inspectionActivity
            UpdateTinspectionActivity();

            //Check and update the ilsm status
            //List<Inspection> list = HCF.BAL.InspectionRepository.GetIlsmInspection();

            var currentDt = _transactionservice.GetIlsmInspections(activityId);

            foreach (DataRow row in currentDt.Rows)
            {
                int tilsmId = 0;
                int floorId = Convert.ToInt32(row["FloorId"].ToString());
                int assetId = Convert.ToInt32(row["AssetId"].ToString());
                if (!string.IsNullOrEmpty(row["TilsID"].ToString()))
                    tilsmId = Convert.ToInt32(row["TilsID"].ToString());

                //if (tilsmId == 0)
                //{
                    var tilsm = new TIlsm
                    {
                        TIlsmId = tilsmId,
                        Status = (BDO.Enums.ILSMStatus)Convert.ToInt32(BDO.Enums.Status.Fail),
                        AssetId = assetId,
                        FloorId = floorId
                    };
                    tilsmId = InsertTilsm(tilsm);

                if (tilsmId > 0)
                {
                    var notificationMatrix = _commonService.MailNotification(BDO.Enums.NotificationCategory.ILSM.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString());
                    if (notificationMatrix != null)
                    {
                        SendIlsmMail(tilsmId, notificationMatrix);
                    }
                }
                // }
            }
        }

        private  void SendIlsmMail(int tilsmId, NotificationMapping obj)
        {
            var objtilsm = GetIlsmInspection(tilsmId);
            _emailService.SendIlsmMail(objtilsm, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM), obj);
        }


        private  void UpdateTinspectionSteps()
        {
            var dtRadate = DateTime.UtcNow;
            var tinssteps = _insStepsRepository.GetInspectionSteps().Where(x => x.IsRA && x.Status == 0 && x.DeficiencyCode == BDO.Enums.InspectionSubStatus.DE.ToString() 
            && (x.DeficiencyStatus == BDO.Enums.StatusCode.ACTIV.ToString())).ToList();
            foreach (var item in tinssteps.Where(x=>x.DRTime.HasValue && x.DeficiencyDate.HasValue).ToList())
            {
                if (item.DeficiencyDate != null)
                {
                    if (item.DRTime != null)
                    {
                        var raData = item.DeficiencyDate.Value.AddHours(item.DRTime.Value);
                        if (DateTime.UtcNow > raData)
                        {
                            if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.TimeToResolveToRA.ToString(), BDO.Enums.NotificationEvent.OnChanged.ToString()))
                            {
                                _emailService.SendTimeToResolveToRAMail(item.TInsStepsId, Convert.ToInt32(BDO.Enums.NotificationCategory.TimeToResolveToRA));
                            }
                            item.DeficiencyCode = BDO.Enums.InspectionSubStatus.RA.ToString();
                            item.RaDate = dtRadate;
                            _insStepsRepository.Update(item, 4);
                        }
                    }
                }
            }
        }

        private  void UpdateTinspectionActivity()
        {
            _transactionservice.MarkInspectionRa();
            //var data = InspectionActivityRepository.GetCurrentActivities();
            //var inspectionActivity = data //InspectionActivityRepository.GetCurrentActivities()
            //    .Where(x => x.SubStatus == Enums.InspectionSubStatus.RA.ToString() && x.IsCurrent && x.Status != 2)
            //    .GroupBy(x => x.InspectionId).ToList();

            //foreach (var item in inspectionActivity)
            //{
            //    if (item.Key.HasValue)
            //    {
            //        int RACount = 0;
            //        int inspectionId = item.Key.Value;
            //        var tinspeActivity = data.Where(x => x.InspectionId == item.Key.Value).ToList();
            //        //var tinspeActivity = DAL.InspectionActivityRepository
            //        //    .GetInspectionActivity(item.InspectionId).Where(x => x.SubStatus == Enums.InspectionSubStatus.RA.ToString() && x.IsCurrent && x.Status != 2).ToList();

            //        RACount = tinspeActivity.Sum(x => x.RaScore.Value);
            //        if (RACount > 0)
            //        {
            //            Inspection inspection = new Inspection();
            //            inspection.InspectionId = inspectionId;
            //            inspection.SubStatus = Enums.InspectionSubStatus.RA.ToString();
            //            InspectionRepository.Update(inspection);
            //        }
            //    }
            //}           
        }

        public  string GetIlsmwoDescription(int tilsmId)
        {
            return _ilsmRepository.GetIlsmwoDescription(tilsmId);
        }

        public  string CompleteIlsmIncident(int tilsmId)
        {
            //return _ilsmRepository.CompleteIlsmIncident(tilsmId);
            string text = _ilsmRepository.CompleteIlsmIncident(tilsmId);
            if (text == "ILSM is now closed")
            {
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.ILSM.ToString(), BDO.Enums.NotificationEvent.OnClosed.ToString()))
                {
                    _emailService.SendIlsmCloseMail(tilsmId, Convert.ToInt32(BDO.Enums.NotificationCategory.ILSM));
                }
            }
            return text;
        }

        public  TIlsm GetCurrentTilsm(Guid activityId)
        {
            return _ilsmRepository.GetCurrentTilsm(activityId);
        }

        public  bool SaveTilsmReport(TIlsm newTilsm)
        {
            return _ilsmRepository.SaveTilsmReport(newTilsm);
        }

        public  bool IcraLinkToIslm(int icraId,int lsmId,int createdBy)
        {
            return _ilsmRepository.IcraLinkToIslm(icraId, lsmId, createdBy);
        }


        private  int InsertTilsm(TIlsm tilsm)
        {
            return _ilsmRepository.InsertTilsm(tilsm);
        }

        public  DataTable InsertConstructionTilsm(TIlsm tilsm, string epDetails)
        {
            return _ilsmRepository.InsertConstructionTilsm(tilsm, epDetails);
        }

        public  int UpdateIlsm(TIlsm objTilsm)
        {
            return _ilsmRepository.UpdateIlsm(objTilsm);
        }

        public bool ILSMlinkToObservationReport(string tilsmId, string ticraReportIds)
        {
            return _ilsmRepository.ILSMlinkToObservationReport(tilsmId, ticraReportIds);
        }
    }
}
