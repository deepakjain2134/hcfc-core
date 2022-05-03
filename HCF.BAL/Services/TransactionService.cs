using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HCF.BAL
{
    public class TransactionService : ITransactionService
    {
        private readonly IMainGoalService _mainGoalService;
        private readonly IInsStepsRepository _insStepsRepository;
        private readonly IGoalMaster _goalMaster;
        private readonly ITransaction _transaction;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        private readonly IInsDetailRepository _insDetailRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IMainGoalRepository _mainGoalRepository;
        private readonly IEpRepository _epRepository;
        private readonly IEmailService _emailService;

        public TransactionService(IEpRepository epRepository,IMainGoalRepository mainGoalRepository,IFloorAssetRepository floorAssetRepository,IInsDetailRepository insDetailRepository,IInspectionActivityRepository inspectionActivityRepository,IInspectionRepository inspectionRepository, IMainGoalService mainGoalService, IInsStepsRepository insStepsRepository, IGoalMaster goalMaster, ITransaction transaction, IEmailService emailService)
        {
            _epRepository = epRepository;
            _mainGoalRepository = mainGoalRepository;
            _floorAssetRepository = floorAssetRepository;
            _insDetailRepository = insDetailRepository;
            _inspectionActivityRepository = inspectionActivityRepository;
            _inspectionRepository = inspectionRepository;
            _transaction = transaction;
            _insStepsRepository = insStepsRepository;
            _mainGoalService = mainGoalService;
            _goalMaster = goalMaster;
            _emailService = emailService;
        }
        #region Transaction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns>
        public int AddInspection(Inspection inspection)
        {
            return _inspectionRepository.AddInspection(inspection);
        }

        public int AddPreviousInspection(Inspection inspection)
        {
            return _inspectionRepository.AddPreviousInspection(inspection);
        }
       
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionDetails"></param>
        /// <returns></returns>
        public int AddInspectionDetails(TInspectionDetail inspectionDetails)
        {
            return _transaction.AddInspectionDetails(inspectionDetails);
        }

        public int AddInspectionDetails(TInspectionDetail inspectionDetails, string steps)
        {
            return _transaction.AddInspectionDetails(inspectionDetails, steps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public int AddTInspectionActivity(TInspectionActivity activity)
        {
            return _inspectionActivityRepository.Save(activity);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionfiles"></param>
        /// <returns></returns>
        public int AddInspectionFiles(TInspectionFiles inspectionfiles)
        {
            return _transaction.AddInspectionFiles(inspectionfiles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionSteps"></param>
        /// <returns></returns>
        public int AddInspectionSteps(InspectionSteps inspectionSteps)
        {
            return _insStepsRepository.AddInspectionSteps(inspectionSteps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionDocs"></param>
        /// <returns></returns>
        public int AddInspectionDocs(InspectionEPDocs inspectionDocs)
        {
            return _transaction.AddInspectionDocs(inspectionDocs);
        }

        public IEnumerable<InspectionEPDocs> GetInspectionDocs(int inspectionId)
        {
            return _transaction.GetInspectionEPDocs(inspectionId, 0);
        }

        //public  IEnumerable<InspectionEPDocs> GetEPInspectionDocs(int epdetailid)
        //{
        //    return _transaction.GetInspectionEPDocs(0, epdetailid);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        private List<TInspectionFiles> GetInspectionEpFiles(int inspectionId)
        {
            return _transaction.GetInspectionEpFiles(inspectionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        private List<TInspectionFiles> GetInspectionAssetFiles(int floorAssetId, int inspectionId)
        {
            return _transaction.GetInspectionAssetFiles(floorAssetId, inspectionId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private List<TInspectionFiles> GetInspectionFiles(Guid activityId)
        {
            return _transaction.GetInspectionFiles(activityId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="ePdetailId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="mainGoalId"></param>
        /// <param name="inspectionDetailId"></param>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        private List<Steps> GetStepsTransactionalRecords(int mode, int ePdetailId, int? floorAssetId, int? mainGoalId, int? inspectionDetailId, int? frequencyId)
        {
            return _transaction.GetStepsTransactionalRecords(mode, ePdetailId, floorAssetId, mainGoalId, inspectionDetailId, frequencyId);
        }

        private List<Steps> GetStepsTransactionalRecords(int inspectionDetailId)
        {
            return _transaction.GetStepsTransactionalRecords(inspectionDetailId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorAssetId"></param>
        /// <param name="assetsId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="inspectionId"></param>
        /// <param name="mode"></param>
        /// <param name="frequencyId"></param>
        /// <param name="inspectionGroupId"></param>
        /// <returns></returns>
        public List<EPSteps> GetGoalStepsByActivity(int? floorAssetId, int? assetsId, int epDetailId, int inspectionId, int mode, int frequencyId, int? inspectionGroupId = null)
        {
            if (mode == Convert.ToInt32(BDO.Enums.Mode.DOC))
                return GetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId, inspectionGroupId);
            else if (mode == Convert.ToInt32(BDO.Enums.Mode.EP))
                return GetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId, inspectionGroupId);
            else
                return GetAssetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId);

        }

        public List<EPSteps> GetFloorAssetsActivity(int epDetailId, int inspectionId, Guid activityId, int floorAssetId)
        {
            var maingoals = new List<MainGoal>();
            int mode = Convert.ToInt32(BDO.Enums.Mode.ASSET);
            var epdocs = _goalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId, null).Take(1).ToList();
            var activity = _inspectionActivityRepository.GetInspectionActivity(activityId).FirstOrDefault();
               

            //InspectionActivityRepository.GetInspectionActivity(inspectionId).Where(x => x.ActivityId == activityId).FirstOrDefault();
            foreach (EPSteps epdoc in epdocs)
            {
                if (activity != null)
                {
                    epdoc.TInspectionActivity = activity;
                    epdoc.ActivityStatus = activity.Status;
                }
                //epdoc.EPDetails = EpRepository.GetEpDescription(epDetailId);
                epdoc.FloorAssetId = floorAssetId;
                epdoc.TFloorAssets = _floorAssetRepository.GetFloorAsset(floorAssetId);

                if (activity != null && (activity.Status == 0 && activity.SubStatus != "NA"))
                {
                    maingoals = _mainGoalRepository.GetGoalTransactionalRecords(activity.ActivityId);
                    if (maingoals.Count > 0)
                    {
                        // epdoc.InspectionFiles = floorAssetId > 0 ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
                        // epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
                        foreach (var goal in maingoals)
                        {
                            var steps = GetStepsTransactionalRecords(goal.InspectionDetailId.Value);
                            //var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, activity.FrequencyId);
                            goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyCode != BDO.Enums.DeficiencyCode.NA.ToString()).ToList();
                        }
                    }

                    maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
                    epdoc.Type = BDO.Enums.InspectionMode.Fail.ToString();
                    epdoc.ActivityId = activity.ActivityId;
                }

                epdoc.InspectionGroupId = epdoc.TFloorAssets.InspectionGroupId;
                epdoc.MainGoal = maingoals;
            }
            return epdocs;
        }

        private List<EPSteps> GetAssetStepActivity(int? floorAssetId, int epDetailId, ref int inspectionId, int mode, int frequencyId)
        {

            var inspection = new Inspection();
            List<MainGoal> maingoals;
            var epdocs = _goalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId, null).Take(1).ToList();

            frequencyId = frequencyId > 0 ? frequencyId : epdocs?.FirstOrDefault()?.FrequencyId ?? 0;

            var allactivity = _inspectionActivityRepository.GetFloorAssetInspectionActivity(floorAssetId.Value)
                .Where(x => x.FrequencyId == frequencyId || x.FrequencyId == null).OrderByDescending(x => x.ActivityInspectionDate).ToList();
            allactivity = allactivity.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
            // List<TInspectionActivity> allactivity = InspectionActivityRepository.GetInspectionActivity(inspectionId).Where(x => x.FloorAssetId == floorAssetId.Value && (x.FrequencyId == frequencyId || x.FrequencyId == null)).ToList();
            var activity = allactivity.OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
            var preactivity = allactivity.Where(x => x.Status != Convert.ToInt32(BDO.Enums.Status.In_Progress)).OrderByDescending(x => x.CreatedDate).Take(4).ToList();
            //TInspectionActivity activity = InspectionActivityRepository.GetInspectionActivity(inspectionId).FirstOrDefault(x => x.FloorAssetId == floorAssetId.Value && x.IsCurrent);
            foreach (var epdoc in epdocs)
            {
                //inspection = epdoc.Inspection;
                epdoc.ActivityType = Convert.ToInt32(BDO.Enums.Mode.ASSET);                
                if (activity != null)
                {
                    epdoc.TInspectionActivity = activity;
                    epdoc.ActivityStatus = activity.Status;
                }
                //epdoc.EPDetails = EpRepository.GetEpDescription(epDetailId); 
                if (inspectionId == 0)
                {
                    epdoc.EPDetails = GetCurrentEp(epDetailId);
                }
                else
                {
                    epdoc.EPDetails = GetEpByInspectionId(epDetailId, inspectionId);
                }

                //epdoc.EPDetails = _inspectionRepository.GetCurrentEp(epDetailId);
                if (floorAssetId.HasValue)
                {
                    epdoc.FloorAssetId = floorAssetId.Value;
                    epdoc.TFloorAssets = _floorAssetRepository.GetFloorAssetDescription(floorAssetId.Value);
                    epdoc.InspectionGroupId = epdoc.TFloorAssets.InspectionGroupId;
                }


                if (activity == null)
                {
                    epdoc.InspectionFiles = null;
                    epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                    var ClientNo = 12344;//HcfSession.ClientNo;
                    maingoals = _mainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId, ClientNo);
                    epdoc.CurrentStatus = BDO.Enums.InspectionSubStatus.NA.ToString();
                    epdoc.ActivityId = Guid.Empty;

                }
                else
                {
                    epdoc.TInspectionActivity = activity;
                    if (activity.Status == 2)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.InProgress.ToString();
                    }
                    else if (activity.IsWorkOrder != 1 && activity.Status == 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.Fail.ToString();
                    }
                    else if (activity.SubStatus == "RA" && activity.OpenWOCount == 0 && activity.IsWorkOrder == 1 && epdoc.TFloorAssets.OpenIlsmsCount > 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.ILSMPending.ToString();
                    }
                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount == 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                    }
                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount > 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.WOInProgress.ToString();
                    }
                    else if (epdoc.TFloorAssets.OpenWorkOrdersCount > 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.WOInProgress.ToString();
                    }
                    else if (epdoc.TFloorAssets.OpenIlsmsCount > 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.ILSMPending.ToString();
                    }
                    else
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                        //epdoc.TInspectionActivity = new TInspectionActivity();
                    }
                    if (epdoc.Type == "New")
                    {
                        epdoc.TInspectionActivity.Comment = "";
                    }
                    epdoc.CurrentStatus = activity.SubStatus;

                    if (epdoc.Type == BDO.Enums.InspectionMode.New.ToString())
                    {
                        epdoc.InspectionFiles = null;
                        epdoc.ActivityId = Guid.Empty;
                        var ClientNo = 123455;// HcfSession.ClientNo;
                        maingoals = _mainGoalService.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId, ClientNo);
                    }
                    else
                    {
                        epdoc.ActivityId = activity.ActivityId;
                        maingoals = _mainGoalService.GetGoalTransactionalRecords(mode, Convert.ToInt32(epDetailId), Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(BDO.Enums.Status.In_Progress), frequencyId);
                        if (maingoals.Count > 0)
                        {
                            epdoc.InspectionFiles = GetInspectionFiles(activity.ActivityId);
                            epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
                            foreach (var goal in maingoals)
                            {
                                var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, frequencyId);
                                if (epdoc.Type == BDO.Enums.InspectionMode.Fail.ToString() || epdoc.Type == BDO.Enums.InspectionMode.WOInProgress.ToString() || epdoc.Type == BDO.Enums.InspectionMode.ILSMPending.ToString())
                                {
                                    goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != BDO.Enums.StatusCode.CMPLT.ToString()).ToList();
                                }
                                else
                                {
                                    goal.Steps = steps;
                                }
                            }
                        }
                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
                        epdoc.ActivityId = activity.ActivityId;
                    }
                }
                epdoc.MainGoal = maingoals.Where(x => x.Steps.Any()).ToList();
                if (epdoc.MainGoal.Any(x => x.Steps.Any(y => y.StepType == 2)))
                {
                    if (inspection != null)
                    {
                        //epdoc.Inspection = inspection;
                        epdoc.Inspection.TInspectionActivity = new List<TInspectionActivity>();
                        epdoc.Inspection.TInspectionActivity = preactivity;
                    }
                }
            }
            return epdocs;
        }

        private List<EPSteps> GetStepActivity(int? floorAssetId, int epDetailId, ref int inspectionId, int mode, int frequencyId, int? inspectionGroupId = null)
        {
            // int insStatus = -1;
            var epmode = mode;
            var inspection = new Inspection();
            if (inspectionId == 0)
            {
                //inspection = GetEpsInspections(epDetailId).FirstOrDefault(x => x.InspectionGroupId == inspectionGroupId && x.IsCurrent); // GetInspection(epDetailId, inspectionId); //1 // change for multiple inspection for one EP so filter by inpection group id 
                inspection = _inspectionRepository.GetEPCurrentInspection(epDetailId).FirstOrDefault(); ///tinspection //GetEpsInspections(epDetailId).FirstOrDefault(x => x.IsCurrent);
                if (inspection != null)
                {
                    //  insStatus = inspection.Status;
                    if (inspection.Status == Convert.ToInt32(BDO.Enums.Status.In_Progress))
                    {
                        inspectionId = inspection.InspectionId;
                    }
                    //else { inspectionId = 0; }
                }
            }

            List<MainGoal> maingoals;
            List<MainGoal> _maingoals;
            // int ePdetailsId = Convert.ToInt32(epDetailId);
            var epdocs = _goalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId, inspectionGroupId).Take(1).ToList(); //2
            frequencyId = frequencyId > 0 ? frequencyId : epdocs?.FirstOrDefault()?.FrequencyId ?? 0;
            foreach (EPSteps epdoc in epdocs)
            {
                epdoc.ActivityType = mode;
                //epdoc.EpTransStatus = EpRepository.EpTransStatus(epdoc.EPDetailId);


                // epdoc.EPDetails =inspection?.EPDetails ?? EpRepository.GetEpDescription(epDetailId); //3


                if (inspectionId == 0) 
                {
                    epdoc.EPDetails = GetCurrentEp(epDetailId);
                }
                else
                {
                    epdoc.EPDetails = GetEpByInspectionId(epDetailId,inspectionId);
                }
                    

                if (inspection != null)
                {
                    inspection.EPDetails = epdoc.EPDetails;
                   
                }
                var allactivity = _inspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == mode).OrderByDescending(x => x.TInsActivityId).ToList();
                var allactivityComment = _inspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == 1 && x.Comment !="").OrderByDescending(x => x.TInsActivityId).ToList();
                //List<TInspectionActivity> Allactivity = HCF.BAL.InspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == 3).OrderByDescending(x => x.TInsActivityId).ToList();
                var activity = allactivity.FirstOrDefault(x => x.IsCurrent);

                if ((activity == null || activity.Status != 2))
                {
                    epdoc.InspectionFiles = null;
                    epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                    maingoals = _mainGoalService.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId, null);
                    epdoc.CurrentStatus = BDO.Enums.InspectionSubStatus.NA.ToString();
                    if (activity == null)
                    {
                        epdoc.ActivityId = Guid.Empty;
                    }
                    else
                    {
                        epdoc.TInspectionActivity = activity;
                    }
                    if (activity != null)
                    {
                        //epdoc.Comment = allactivity.FirstOrDefault(x => x.IsCurrent).Comment;
                        if (allactivityComment.Count > 0 && allactivityComment.Any(x => x.Comment != ""))
                            epdoc.Comment = allactivityComment.Where(x => x.IsCurrent && (x.Comment != "" || x.Comment != null)).FirstOrDefault().Comment;
                    }
                    else
                    {
                        if (allactivityComment.Count > 0 && allactivityComment.Any(x=>x.Comment!=""))
                            epdoc.Comment = allactivityComment.Where(x => x.IsCurrent && (x.Comment != "" || x.Comment != null)).FirstOrDefault().Comment;
                    }

                    if (maingoals.Count > 0 & inspectionId > 0)
                    {
                        var epsteps = maingoals.FirstOrDefault(x => x.ActivityType == 1);
                        mode = epsteps.ActivityType;
                        _maingoals = _mainGoalService.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(BDO.Enums.Status.In_Progress), frequencyId);//4
                        if (_maingoals.Count > 0)
                        {
                            epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
                            epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
                            foreach (var goal in maingoals.Where(x => x.ActivityType == 1))
                            {
                                var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, _maingoals.FirstOrDefault().InspectionDetailId, frequencyId);
                                if (epdoc.Type == BDO.Enums.InspectionMode.Fail.ToString() || epdoc.Type == BDO.Enums.InspectionMode.WOInProgress.ToString())
                                {
                                    goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != BDO.Enums.StatusCode.CMPLT.ToString() && x.IsActive == true).ToList();
                                }
                                else
                                {
                                    goal.Steps = steps.Where(x => x.IsActive == true && x.IsCurrent == true && x.StepType == 1).ToList();
                                }
                            }

                        }
                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
                    }




                }
                else
                {
                    if (activity != null)
                    {
                        //epdoc.Comment = allactivity.FirstOrDefault(x => x.IsCurrent).Comment;
                        if (allactivityComment.Count > 0)
                            epdoc.Comment = allactivityComment.Where(x => x.IsCurrent && (x.Comment != "" || x.Comment != null)).FirstOrDefault().Comment;
                    }

                    epdoc.TInspectionActivity = activity;
                    if (activity.Status == 2)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.InProgress.ToString();
                    }
                    else if (activity.IsWorkOrder != 1 && activity.Status == 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.Fail.ToString();
                    }
                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount == 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                    }
                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount > 0)
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.WOInProgress.ToString();
                    }
                    else
                    {
                        epdoc.Type = BDO.Enums.InspectionMode.New.ToString();
                    }
                    epdoc.CurrentStatus = activity.SubStatus;

                    epdoc.InspectionFiles = null;
                    epdoc.ActivityId = Guid.Empty;
                    //maingoals = _mainGoalService.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId);

                    epdoc.InspectionFiles = null;
                    epdoc.ActivityId = Guid.Empty;
                    maingoals = _mainGoalService.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId, null);
                    if (epdoc.Type == BDO.Enums.InspectionMode.New.ToString())
                    {

                        epdoc.InspectionFiles = null;
                        epdoc.ActivityId = Guid.Empty;
                        maingoals = _mainGoalService.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId, null);
                    }
                    else
                    {
                        epdoc.ActivityId = activity.ActivityId;
                        if (activity.ActivityType == 1)
                        {
                            maingoals = _mainGoalService.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(BDO.Enums.Status.In_Progress), frequencyId);//4
                            if (maingoals.Count > 0)
                            {
                                epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
                                epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
                                foreach (var goal in maingoals)
                                {
                                    var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, frequencyId);
                                    if (epdoc.Type == BDO.Enums.InspectionMode.Fail.ToString() || epdoc.Type == BDO.Enums.InspectionMode.WOInProgress.ToString())
                                    {
                                        goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != BDO.Enums.StatusCode.CMPLT.ToString() && x.IsActive == true && x.IsCurrent == true).ToList();
                                    }
                                    else
                                    {
                                        goal.Steps = steps.Where(x => x.IsActive == true && x.IsCurrent == true && x.StepType == 1).ToList();
                                    }
                                }
                            }
                        }
                        else
                        {
                            var epsteps = maingoals.FirstOrDefault(x => x.ActivityType == 1);
                            mode = epsteps.ActivityType;
                            _maingoals = _mainGoalService.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(BDO.Enums.Status.In_Progress), frequencyId);//4
                            if (_maingoals.Count > 0)
                            {
                                epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
                                epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
                                foreach (var goal in maingoals.Where(x => x.ActivityType == 1))
                                {
                                    var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, _maingoals.FirstOrDefault().InspectionDetailId, frequencyId);
                                    if (epdoc.Type == BDO.Enums.InspectionMode.Fail.ToString() || epdoc.Type == BDO.Enums.InspectionMode.WOInProgress.ToString())
                                    {
                                        goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != BDO.Enums.StatusCode.CMPLT.ToString() && x.IsActive == true).ToList();
                                    }
                                    else
                                    {
                                        goal.Steps = steps.Where(x => x.IsActive == true && x.IsCurrent == true && x.StepType == 1).ToList();
                                    }
                                }

                            }

                        }


                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
                        epdoc.ActivityId = activity.ActivityId;
                    }
                }
                if (inspection != null&& inspection.InspectionId>0)
                {
                    epdoc.Inspection = inspection;
                }
                if (epmode == 3)
                {
                    epdoc.MainGoal = maingoals.Where(x => x.ActivityType != 2 && x.ActivityType != 1 && x.Steps.Any()).OrderByDescending(x => x.ActivityType).ToList();

                }
                else
                {
                    epdoc.MainGoal = maingoals.Where(x => x.ActivityType != 2 && x.Steps.Any()).OrderByDescending(x => x.ActivityType).ToList();
                }



            }

            return epdocs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="activityId"></param>
        /// <param name="subStatus"></param>
        public bool UpdateInspectionSubstatus(int inspectionId, Guid activityId, string subStatus)
        {
            return _transaction.UpdateInspectionSubstatus(inspectionId, activityId, subStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subStatus"></param>
        /// <returns></returns>
        public List<Inspection> GetInspection(string subStatus)
        {
            return _inspectionRepository.GetInspection(subStatus);
        }


        public int UndoInspection(Inspection inspection)
        {
            return _inspectionRepository.UndoInspection(inspection);
        }

        #endregion

        #region EPHistory

        public EPDetails GetEpInspectionHistory(int epDetailId)
        {
            var epDetails = _epRepository.GetEpInspectionHistory(epDetailId);
            if (epDetails != null)
                epDetails.Inspections = _inspectionRepository.GetEpsInspections(epDetailId);
            return epDetails;
        }

        public EPDetails GetEpInspectionActivity(int epDetailId, int inspectionId)
        {
            var ep = _epRepository.GetEpInspectionHistory(epDetailId, inspectionId);
            if (ep != null)
            {
                var epActivity = _inspectionActivityRepository.GetAllInspectionActivity(inspectionId).FirstOrDefault(x => x.ActivityType == 1); //&& x.IsCurrent);
                if (epActivity != null)
                {
                    ep.Inspection = _inspectionRepository.GetActivityHistory(epActivity.ActivityId);
                }
            }
            return ep;
        }

        public EPDetails GetInspectionHistory(int userId, int epDetailId, int inspectionId)
        {
            return _inspectionRepository.GetInspectionHistory(userId, epDetailId, inspectionId);
        }

        public IEnumerable<Inspection> GetEpsInspections(int epDetailId)
        {
            return _inspectionRepository.GetEpsInspections(epDetailId);
        }


        public Inspection GetActivityHistory(Guid activityId)
        {
            return _inspectionRepository.GetActivityHistory(activityId);
        }

        public IEnumerable<Inspection> GetEpsCampusInspections(int epDetailId)
        {
            return _inspectionRepository.GetEpsCampusInspections(epDetailId);
        }
        #endregion

        #region IHCFRepository

        public List<InspectionEPDocs> RepositoryDashboard(int userId)
        {
            return _transaction.GetUserInsDoc(userId);
        }

        public List<InspectionEPDocs> GetEpDocByBinder(int binderId)
        {
            return _transaction.GetEpDocByBinder(binderId);
        }

        public EPDetails GetDocsbyEp(int userId, int epdetailId)
        {
            var epDetails = _epRepository.GetDocsbyEp(userId, epdetailId);
            if (epDetails != null)
                epDetails.Inspections = _inspectionRepository.GetEpInspectionDoc(userId, epdetailId);
            return epDetails;
        }

        #endregion

        #region Reports

        public List<InspectionEPDocs> GetDocumentReport(int userId)
        {
            return _transaction.GetDocumentReport(userId);
        }

        public List<InspectionEPDocs> GetDocumentHistory(int fileId)
        {
            return _transaction.GetDocumentHistory(fileId);
        }

        //public  EPDetails GetDocumentHistorybyEP(int userId, int epdetailId)
        //{
        //    return HCF._transaction.GetDocumentHistorybyEP(userId, epdetailId);
        //}

        #endregion

        #region TInspection

        public List<Inspection> GetAllInspections()
        {
            return _inspectionRepository.GetCurrentInspections();
        }

        #endregion

        #region TInspection Activity

        public List<EPSteps> GetAllEpDocs(int userId, int epDetailId)
        {
            var epDocs = _inspectionRepository.GetAllEpDocs(userId, epDetailId);
            return epDetailId > 0 ? epDocs.Where(x => x.EPDetailId == epDetailId).ToList() : epDocs;
        }


        public List<EPSteps> GetAllEpDocs(int userId)
        {
            var mode = Convert.ToInt32(BDO.Enums.Mode.DOC);
            var epdocs = _inspectionRepository.GetAllEpDocs(userId, 0);
            foreach (var epdoc in epdocs)
            {
                if (Convert.ToInt32(epdoc.InspectionId) > 0)
                {
                    var maingoals = _mainGoalService.GetGoalTransactionalRecords(mode, Convert.ToInt32(epdoc.EPDetailId), null,
                        Convert.ToInt32(epdoc.InspectionId), Convert.ToInt32(BDO.Enums.Status.In_Progress), 0);
                    if (maingoals.Count > 0)
                    {
                        epdoc.MainGoal = maingoals;
                        foreach (var goal in maingoals)
                        {
                            var steps = GetStepsTransactionalRecords(mode, epdoc.EPDetailId, null,
                                goal.MainGoalId, goal.InspectionDetailId, null);
                            goal.Steps = steps;
                        }
                    }
                    else
                    {
                        epdoc.InspectionFiles = null;
                        epdoc.MainGoal = _mainGoalService.GetGoalAndSteps(epdoc.EPDetailId, mode, null, 0, null);
                    }
                }
                else
                {
                    epdoc.InspectionFiles = null;
                    epdoc.MainGoal = _mainGoalService.GetGoalAndSteps(epdoc.EPDetailId, mode, null, 0, null);
                }
                if (epdoc.MainGoal != null)
                    if (epdoc.MainGoal.Count > 0)
                        foreach (var goal in epdoc.MainGoal)
                            goal.Steps = goal.Steps.Where(m => m.IsCurrent).ToList();

            }
            return epdocs;
        }

       

        public TInspectionActivity GetTInspectionActivity(Guid activityId)
        {
            return _inspectionActivityRepository.GetTInspectionActivity(activityId);

        }

        public List<Inspection> GetComplianceReport()
        {
            var inspections = GetComplianceInspections();
            foreach (var item in inspections)
            {
                foreach (var inspectionActivity in item.TInspectionActivity.Where(x => x.ActivityType == 2).ToList())
                {
                    if (inspectionActivity.TFloorAssets != null && inspectionActivity.TFloorAssets.Assets != null)
                    {
                        if (inspectionActivity.TFloorAssets.Assets.IsStepsOnReport)
                        {
                            inspectionActivity.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(inspectionActivity.ActivityId);
                        }
                    }
                }
            }
            return inspections;
        }

        public List<TInspectionActivity> GetComplianceReports(string assetids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            List<TInspectionActivity> inspectionActivities = GetComplianceReports(assetids);
            //if (!string.IsNullOrEmpty(assetids))
            //{
            //    inspectionActivities = inspectionActivities.Where(x => assetids.Contains(x.TFloorAssets.AssetId.ToString())).ToList();
            //}
            if (!string.IsNullOrEmpty(buildingId) && Convert.ToInt32(buildingId) > 0)
            {
                inspectionActivities = inspectionActivities.Where(x => x.TFloorAssets.Floor.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0)
            {
                inspectionActivities = inspectionActivities.Where(x => x.TFloorAssets.FloorId == Convert.ToInt32(floorId)).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                inspectionActivities = inspectionActivities.Where(x => x.Status == Convert.ToInt32(status)).ToList();
            }
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                inspectionActivities = inspectionActivities.Where(x => x.ActivityInspectionDate.Value.Date >= Convert.ToDateTime(fromdate).Date && x.ActivityInspectionDate.Value.Date <= Convert.ToDateTime(todate).Date).ToList();
            }
            return inspectionActivities;
        }


        private List<TInspectionActivity> GetComplianceReports(string assetids)
        {
            var inspectionActivities = _inspectionActivityRepository.GetComplianceRpeort(assetids);
            foreach (var inspectionActivity in inspectionActivities)
            {
                if (inspectionActivity.TFloorAssets != null && inspectionActivity.TFloorAssets.Assets != null)
                {
                    if (inspectionActivity.TFloorAssets.Assets.IsStepsOnReport)
                    {
                        inspectionActivity.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(inspectionActivity.ActivityId);
                    }
                }
            }
            return inspectionActivities;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Inspection> GetComplianceInspections()
        {
            var inspections = _inspectionRepository.GetInspections().Where(x => x.IsCurrent && (x.EPDetails.IsDocRequired || x.EPDetails.IsAssetsRequired)).ToList();
            foreach (var inspection in inspections)
            {
                inspection.TInspectionActivity = _inspectionActivityRepository.GetTInspectionActivity(inspection.InspectionId).Where(x => x.IsCurrent).ToList();
            }
            return inspections;
        }



        public bool DeleteTInspectionFiles(int tinsFileId)
        {
            return _transaction.DeleteTInspectionFiles(tinsFileId);
        }


        #endregion

        public List<InspectionEPDocs> GetEpInspectionDoc(int epDetIlId)
        {
            return _transaction.GetEpInspectionDoc(epDetIlId);
        }

        public List<Inspection> GetInspectionsforWorkOrder(string activityId)
        {
            return _inspectionRepository.GetInspectionsforWorkOrder(activityId);
        }


        public List<Inspection> GetDeficiencies(string activityId)
        {
            return _inspectionRepository.GetDeficiencies(activityId);
        }
        public List<EPsDocument> GetInspectionDocuments(int userId)
        {
            return _transaction.GetInspectionDocuments(userId);
        }

        public List<DocumentType> GetPolicyBinders(int userId, int selectedUser, int? epDetailId)
        {
            return _transaction.GetPolicyBinders(userId, selectedUser, epDetailId);
        }

        public DocumentType PolicyBinderHistory(int userId)
        {
            return _transaction.PolicyBinderHistory(userId);
        }

        public int InsertUpdateEpConfig(EpConfig epConfig, int mode, string dates)
        {
            return _transaction.InsertUpdateEpConfig(epConfig, mode, dates);
        }

        public bool DeleteEpDocs(Guid activityId)
        {
            return _transaction.DeleteEpDocs(activityId);
        }

        public EPDetails GetCurrentEp(int epId)
        {
            return _inspectionRepository.GetCurrentEp(epId);
        }
        public List<InspectionEPDocs> GetRelationEpInspectionDoc(int epDetIlId)
        {
            return _transaction.GetRelationEpInspectionDoc(epDetIlId);
        }
        public int AddTInspectionCampus(Buildings Campus, int InspectionId, int Epdetailid)
        {
            return _transaction.AddTinspectionCampus(Campus, InspectionId, Epdetailid);
        }

        public bool MarkInsdocStatus(int inspectionId, int isComplete)
        {
            return _inspectionRepository.MarkInsdocStatus(inspectionId, isComplete);
        }
        public DataTable GetIlsmInspections(Guid? activityId)
        {
            return _inspectionRepository.GetIlsmInspections(activityId);
        }
        public bool MarkInspectionRa()
        {
            return _inspectionRepository.MarkInspectionRa();
        }

        public List<EPsDocument> GetEpsDocument(int epId)
        {
            return _inspectionRepository.GetEPsDocument(epId);
        }
        public int Save(InspectionGroup objInspectionGroup)
        {
            return _inspectionRepository.Save(objInspectionGroup);
        }
        public List<InspectionGroup> GetInspectionGroup()
        {
            return _inspectionRepository.GetInspectionGroup();
        }
        public bool UpdateInspectionGroup(InspectionGroup objInspectionGroup)
        {
            return _inspectionRepository.UpdateInspectionGroup(objInspectionGroup);
        }
        public List<Inspection> GetInspectionsForCalendar(int userId, DateTime? startDate, DateTime? endDate)
        {
            return _inspectionRepository.GetInspectionsForCalendar(userId, startDate, endDate);
        }
        public List<TInspectionActivity> GetInspections(int floorAssetId, int epId)
        {
            return _inspectionRepository.GetInspections(floorAssetId, epId);
        }
        public calenderViewDashBoard GetDashboardCalendar(int userId)
        {
            return _inspectionRepository.GetDashboardCalendar(userId);
        }
        public List<EPDetails> GetArchivedEPsInspection(int userId)
        {
            return _inspectionRepository.GetArchivedEPsInspection(userId);

        }
        public void SendDueDateEmail()
        {
            _emailService.SendDueDateMail();
        }

        public EPDetails GetEpByInspectionId(int epId, int Inspectionid)
        {
            return _inspectionRepository.GetEpByInspectionId(epId, Inspectionid);
        }
    }
}
