//using HCF.BDO;
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class Transaction
//    {
//        #region Transaction

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspection"></param>
//        /// <returns></returns>
//        public static int AddInspection(Inspection inspection)
//        {
//            return DAL.InspectionRepository.AddInspection(inspection);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionDetails"></param>
//        /// <returns></returns>
//        public static int AddInspectionDetails(TInspectionDetail inspectionDetails)
//        {
//            return DAL.Transaction.AddInspectionDetails(inspectionDetails);
//        }

//        public static int AddInspectionDetails(TInspectionDetail inspectionDetails, string steps)
//        {
//            return DAL.Transaction.AddInspectionDetails(inspectionDetails, steps);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="activity"></param>
//        /// <returns></returns>
//        public static int AddTInspectionActivity(TInspectionActivity activity)
//        {
//            return DAL.InspectionActivityRepository.Save(activity);
//        }



//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionfiles"></param>
//        /// <returns></returns>
//        public static int AddInspectionFiles(TInspectionFiles inspectionfiles)
//        {
//            return DAL.Transaction.AddInspectionFiles(inspectionfiles);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionSteps"></param>
//        /// <returns></returns>
//        public static int AddInspectionSteps(InspectionSteps inspectionSteps)
//        {
//            return DAL.InsStepsRepository.AddInspectionSteps(inspectionSteps);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionDocs"></param>
//        /// <returns></returns>
//        public static int AddInspectionDocs(InspectionEPDocs inspectionDocs)
//        {
//            return DAL.Transaction.AddInspectionDocs(inspectionDocs);
//        }

//        public static IEnumerable<InspectionEPDocs> GetInspectionDocs(int inspectionId)
//        {
//            return DAL.Transaction.GetInspectionEPDocs(inspectionId, 0);
//        }

//        //public static IEnumerable<InspectionEPDocs> GetEPInspectionDocs(int epdetailid)
//        //{
//        //    return DAL.Transaction.GetInspectionEPDocs(0, epdetailid);
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionId"></param>
//        /// <returns></returns>
//        private static List<TInspectionFiles> GetInspectionEpFiles(int inspectionId)
//        {
//            return DAL.Transaction.GetInspectionEpFiles(inspectionId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetId"></param>
//        /// <param name="inspectionId"></param>
//        /// <returns></returns>
//        private static List<TInspectionFiles> GetInspectionAssetFiles(int floorAssetId, int inspectionId)
//        {
//            return DAL.Transaction.GetInspectionAssetFiles(floorAssetId, inspectionId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="activityId"></param>
//        /// <returns></returns>
//        private static List<TInspectionFiles> GetInspectionFiles(Guid activityId)
//        {
//            return DAL.Transaction.GetInspectionFiles(activityId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="mode"></param>
//        /// <param name="ePdetailId"></param>
//        /// <param name="floorAssetId"></param>
//        /// <param name="mainGoalId"></param>
//        /// <param name="inspectionDetailId"></param>
//        /// <param name="frequencyId"></param>
//        /// <returns></returns>
//        private static List<Steps> GetStepsTransactionalRecords(int mode, int ePdetailId, int? floorAssetId, int? mainGoalId, int? inspectionDetailId, int? frequencyId)
//        {
//            return DAL.Transaction.GetStepsTransactionalRecords(mode, ePdetailId, floorAssetId, mainGoalId, inspectionDetailId, frequencyId);
//        }

//        private static List<Steps> GetStepsTransactionalRecords(int inspectionDetailId)
//        {
//            return DAL.Transaction.GetStepsTransactionalRecords(inspectionDetailId);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetId"></param>
//        /// <param name="assetsId"></param>
//        /// <param name="epDetailId"></param>
//        /// <param name="inspectionId"></param>
//        /// <param name="mode"></param>
//        /// <param name="frequencyId"></param>
//        /// <param name="inspectionGroupId"></param>
//        /// <returns></returns>
//        public static List<EPSteps> GetGoalStepsByActivity(int? floorAssetId, int? assetsId, int epDetailId, int inspectionId, int mode, int frequencyId, int? inspectionGroupId = null)
//        {
//            if (mode == Convert.ToInt32(Enums.Mode.DOC))
//                return GetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId, inspectionGroupId);
//            else if (mode == Convert.ToInt32(Enums.Mode.EP))
//                return GetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId, inspectionGroupId);
//            else
//                return GetAssetStepActivity(floorAssetId, epDetailId, ref inspectionId, mode, frequencyId);

//        }

//        public static List<EPSteps> GetFloorAssetsActivity(int epDetailId, int inspectionId, Guid activityId, int floorAssetId)
//        {
//            var maingoals = new List<MainGoal>();
//            int mode = Convert.ToInt32(Enums.Mode.ASSET);
//            var epdocs = GoalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId).Take(1).ToList();
//            var activity = DAL.InspectionActivityRepository.GetInspectionActivity(activityId).FirstOrDefault();
//            // if (activity != null)         
//            //activity.TInspectionDetail = DAL.InsDetailRepository.GetTInspectionDetails(activity.ActivityId);            

//            //InspectionActivityRepository.GetInspectionActivity(inspectionId).Where(x => x.ActivityId == activityId).FirstOrDefault();
//            foreach (EPSteps epdoc in epdocs)
//            {
//                if (activity != null)
//                {
//                    epdoc.TInspectionActivity = activity;
//                    epdoc.ActivityStatus = activity.Status;
//                }
//                //epdoc.EPDetails = EpRepository.GetEpDescription(epDetailId);
//                epdoc.FloorAssetId = floorAssetId;
//                epdoc.TFloorAssets = FloorAssetRepository.GetFloorAsset(floorAssetId);

//                if (activity != null && (activity.Status == 0 && activity.SubStatus != "NA"))
//                {
//                    maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(activity.ActivityId);
//                    if (maingoals.Count > 0)
//                    {
//                       // epdoc.InspectionFiles = floorAssetId > 0 ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
//                       // epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
//                        foreach (var goal in maingoals)
//                        {
//                            var steps = GetStepsTransactionalRecords(goal.InspectionDetailId.Value);
//                            //var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, activity.FrequencyId);
//                            goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyCode != Enums.DeficiencyCode.NA.ToString()).ToList();
//                        }
//                    }

//                    maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
//                    epdoc.Type = Enums.InspectionMode.Fail.ToString();
//                    epdoc.ActivityId = activity.ActivityId;
//                }

//                epdoc.InspectionGroupId = epdoc.TFloorAssets.InspectionGroupId;
//                epdoc.MainGoal = maingoals;
//            }
//            return epdocs;
//        }

//        private static List<EPSteps> GetAssetStepActivity(int? floorAssetId, int epDetailId, ref int inspectionId, int mode, int frequencyId)
//        {
           
//            var inspection = new Inspection();
//            List<MainGoal> maingoals;       
//            var epdocs = GoalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId).Take(1).ToList(); 
//            frequencyId = frequencyId > 0 ? frequencyId : epdocs?.FirstOrDefault()?.FrequencyId ?? 0;

//            var allactivity = DAL.InspectionActivityRepository.GetFloorAssetInspectionActivity(floorAssetId.Value)
//                .Where(x => x.FrequencyId == frequencyId || x.FrequencyId == null).OrderByDescending(x => x.ActivityInspectionDate).ToList();
//            allactivity = allactivity.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
//            // List<TInspectionActivity> allactivity = InspectionActivityRepository.GetInspectionActivity(inspectionId).Where(x => x.FloorAssetId == floorAssetId.Value && (x.FrequencyId == frequencyId || x.FrequencyId == null)).ToList();
//            var activity = allactivity.OrderByDescending(x=>x.ActivityInspectionDate).FirstOrDefault();
//            var preactivity = allactivity.Where(x => x.Status != Convert.ToInt32(Enums.Status.In_Progress)).OrderByDescending(x => x.CreatedDate).Take(4).ToList();
//            //TInspectionActivity activity = InspectionActivityRepository.GetInspectionActivity(inspectionId).FirstOrDefault(x => x.FloorAssetId == floorAssetId.Value && x.IsCurrent);
//            foreach (var epdoc in epdocs)
//            {
//                epdoc.ActivityType = Convert.ToInt32(Enums.Mode.ASSET);
//                //if (epdoc.InspectionId > 0)
//                //{
//                //    epdoc.EpTransStatus = DAL.EpRepository.EpTransStatus(epdoc.EPDetailId, epdoc.Inspection.InspectionId > 0 ? epdoc.Inspection : null);
//                //    epdoc.EpTranStatus = DAL.EpRepository.EpTransStatusInt(epdoc.EPDetailId, epdoc.Inspection.InspectionId > 0 ? epdoc.Inspection : null);
//                //}
//                //else
//                //{
//                //    epdoc.EpTransStatus = "P";
//                //    epdoc.EpTranStatus = -1;
//                //}
//                if (activity != null)
//                {
//                    epdoc.TInspectionActivity = activity;
//                    epdoc.ActivityStatus = activity.Status;
//                }
//                //epdoc.EPDetails = EpRepository.GetEpDescription(epDetailId); 
//                epdoc.EPDetails = DAL.Transaction.GetCurrentEp(epDetailId);
//                if (floorAssetId.HasValue)
//                {
//                    epdoc.FloorAssetId = floorAssetId.Value;
//                    epdoc.TFloorAssets = FloorAssetRepository.GetFloorAssetDescription(floorAssetId.Value);
//                    epdoc.InspectionGroupId = epdoc.TFloorAssets.InspectionGroupId;
//                }


//                if (activity == null)
//                {
//                    epdoc.InspectionFiles = null;
//                    epdoc.Type = Enums.InspectionMode.New.ToString();
//                    var ClientNo = HcfSession.ClientNo;
//                    maingoals = DAL.MainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId, ClientNo);
//                    epdoc.CurrentStatus = Enums.InspectionSubStatus.NA.ToString();
//                    epdoc.ActivityId = Guid.Empty;

//                }
//                else
//                {
//                    epdoc.TInspectionActivity = activity;
//                    if (activity.Status == 2)
//                    {
//                        epdoc.Type = Enums.InspectionMode.InProgress.ToString();
//                    }
//                    else if (activity.IsWorkOrder != 1 && activity.Status == 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.Fail.ToString();
//                    }
//                    else if (activity.SubStatus == "RA" && activity.OpenWOCount == 0 && activity.IsWorkOrder == 1 && epdoc.TFloorAssets.OpenIlsmsCount > 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.ILSMPending.ToString();
//                    }
//                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount == 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.New.ToString();
//                    }
//                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount > 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.WOInProgress.ToString();
//                    }
//                    else if (epdoc.TFloorAssets.OpenWorkOrdersCount > 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.WOInProgress.ToString();
//                    }
//                    else if (epdoc.TFloorAssets.OpenIlsmsCount > 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.ILSMPending.ToString();
//                    }
//                    else
//                    {
//                        epdoc.Type = Enums.InspectionMode.New.ToString();
//                        //epdoc.TInspectionActivity = new TInspectionActivity();
//                    }
//                    if(epdoc.Type=="New")
//                    {
//                        epdoc.TInspectionActivity.Comment = "";
//                    }
//                    epdoc.CurrentStatus = activity.SubStatus;

//                    if (epdoc.Type == Enums.InspectionMode.New.ToString())
//                    {
//                        epdoc.InspectionFiles = null;
//                        epdoc.ActivityId = Guid.Empty;
//                        var ClientNo = HcfSession.ClientNo;
//                        maingoals = DAL.MainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId, ClientNo);
//                    }
//                    else
//                    {
//                        epdoc.ActivityId = activity.ActivityId;
//                        maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(mode, Convert.ToInt32(epDetailId), Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(Enums.Status.In_Progress), frequencyId);
//                        if (maingoals.Count > 0)
//                        {
//                            epdoc.InspectionFiles = GetInspectionFiles(activity.ActivityId);
//                            epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
//                            foreach (var goal in maingoals)
//                            {
//                                var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, frequencyId);
//                                if (epdoc.Type == Enums.InspectionMode.Fail.ToString() || epdoc.Type == Enums.InspectionMode.WOInProgress.ToString() || epdoc.Type == Enums.InspectionMode.ILSMPending.ToString())
//                                {
//                                    goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != Enums.StatusCode.CMPLT.ToString()).ToList();
//                                }
//                                else
//                                {
//                                    goal.Steps = steps;
//                                }
//                            }
//                        }
//                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
//                        epdoc.ActivityId = activity.ActivityId;
//                    }
//                }
//                epdoc.MainGoal = maingoals.Where(x => x.Steps.Any()).ToList();
//                if (epdoc.MainGoal.Any(x => x.Steps.Any(y => y.StepType == 2)))
//                {
//                    if (inspection != null)
//                    {
//                        epdoc.Inspection = inspection;
//                        epdoc.Inspection.TInspectionActivity = new List<TInspectionActivity>();
//                        epdoc.Inspection.TInspectionActivity = preactivity;
//                    }
//                }
//            }
//            return epdocs;
//        }

//        private static List<EPSteps> GetStepActivity(int? floorAssetId, int epDetailId, ref int inspectionId, int mode, int frequencyId, int? inspectionGroupId = null)
//        {
//            // int insStatus = -1;
//            var epmode = mode;
//            var inspection = new Inspection(); 
//            if (inspectionId == 0)
//            {
//                //inspection = GetEpsInspections(epDetailId).FirstOrDefault(x => x.InspectionGroupId == inspectionGroupId && x.IsCurrent); // GetInspection(epDetailId, inspectionId); //1 // change for multiple inspection for one EP so filter by inpection group id 
//                inspection = DAL.InspectionRepository.GetEPCurrentInspection(epDetailId).FirstOrDefault(); ///tinspection //GetEpsInspections(epDetailId).FirstOrDefault(x => x.IsCurrent);
//                if (inspection != null)
//                {
//                    //  insStatus = inspection.Status;
//                    if (inspection.Status == Convert.ToInt32(Enums.Status.In_Progress))
//                    {
//                        inspectionId = inspection.InspectionId;
//                    }
//                    //else { inspectionId = 0; }
//                }
//            }

//            List<MainGoal> maingoals;
//            List<MainGoal> _maingoals;
//            // int ePdetailsId = Convert.ToInt32(epDetailId);
//            var epdocs = GoalMaster.GetEpTransInfo(Convert.ToInt32(epDetailId), inspectionId, inspectionGroupId).Take(1).ToList(); //2
//            frequencyId = frequencyId > 0 ? frequencyId : epdocs?.FirstOrDefault()?.FrequencyId ?? 0;
//            foreach (EPSteps epdoc in epdocs)
//            {
//                epdoc.ActivityType = mode;
//                //epdoc.EpTransStatus = EpRepository.EpTransStatus(epdoc.EPDetailId);

//                //if (epdoc.InspectionId > 0)
//                //{
//                //    epdoc.EpTransStatus = DAL.EpRepository.EpTransStatus(epdoc.EPDetailId, epdoc.Inspection.InspectionId > 0 ? epdoc.Inspection : null);
//                //    epdoc.EpTranStatus = DAL.EpRepository.EpTransStatusInt(epdoc.EPDetailId, epdoc.Inspection.InspectionId > 0 ? epdoc.Inspection : null);
//                //}
//                //else
//                //{
//                //    epdoc.EpTransStatus = "P";
//                //    epdoc.EpTranStatus = -1;
//                //}
//                // epdoc.EPDetails =inspection?.EPDetails ?? EpRepository.GetEpDescription(epDetailId); //3
               
               
                
//                epdoc.EPDetails = GetCurrentEp(epDetailId);

//                if (inspection != null)
//                {
//                    inspection.EPDetails = epdoc.EPDetails;

//                }
//                var allactivity = DAL.InspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == mode).OrderByDescending(x => x.TInsActivityId).ToList();
//                var allactivityComment = DAL.InspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == 1).OrderByDescending(x => x.TInsActivityId).ToList();
//                //List<TInspectionActivity> Allactivity = HCF.BAL.InspectionActivityRepository.GetAllInspectionActivity(inspectionId).Where(x => x.ActivityType == 3).OrderByDescending(x => x.TInsActivityId).ToList();
//                var activity = allactivity.FirstOrDefault(x => x.IsCurrent);
                
//                if ((activity == null || activity.Status != 2))
//                {
//                    epdoc.InspectionFiles = null;
//                    epdoc.Type = Enums.InspectionMode.New.ToString();
//                    maingoals = DAL.MainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId,null);
//                    epdoc.CurrentStatus = Enums.InspectionSubStatus.NA.ToString();
//                    if(activity==null)
//                    {
//                        epdoc.ActivityId = Guid.Empty;
//                    }
//                    else
//                    {
//                        epdoc.TInspectionActivity = activity;
//                    }
//                    if (activity != null)
//                    {
//                        //epdoc.Comment = allactivity.FirstOrDefault(x => x.IsCurrent).Comment;
//                        if (allactivityComment.Count > 0)
//                            epdoc.Comment = allactivityComment.Where(x => x.IsCurrent && (x.Comment != "" || x.Comment != null)).FirstOrDefault().Comment;
//                    }
//                    else
//                    {
//                        if(allactivityComment.Count>0)
//                        epdoc.Comment = allactivityComment.Where(x => x.IsCurrent &&( x.Comment!="" ||x.Comment!=null)).FirstOrDefault().Comment; 
//                    }
                        
//                    if (maingoals.Count > 0 & inspectionId > 0)
//                    {
//                        var epsteps = maingoals.FirstOrDefault(x => x.ActivityType == 1);
//                        mode = epsteps.ActivityType;
//                        _maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(Utility.Enums.Status.In_Progress), frequencyId);//4
//                        if (_maingoals.Count > 0)
//                        {
//                            epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
//                            epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
//                            foreach (var goal in maingoals.Where(x => x.ActivityType == 1))
//                            {
//                                var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, _maingoals.FirstOrDefault().InspectionDetailId, frequencyId);
//                                if (epdoc.Type == Enums.InspectionMode.Fail.ToString() || epdoc.Type == Enums.InspectionMode.WOInProgress.ToString())
//                                {
//                                    goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != Enums.StatusCode.CMPLT.ToString()&& x.IsActive == true).ToList();
//                                }
//                                else
//                                {
//                                    goal.Steps = steps.Where(x => x.IsActive == true&& x.IsCurrent == true&&x.StepType==1).ToList();
//                                }
//                            }

//                        }
//                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
//                    }




//                }
//                else
//                {
//                    if (activity != null)
//                    {
//                        //epdoc.Comment = allactivity.FirstOrDefault(x => x.IsCurrent).Comment;
//                        if (allactivityComment.Count > 0)
//                            epdoc.Comment = allactivityComment.Where(x => x.IsCurrent && (x.Comment != "" || x.Comment != null)).FirstOrDefault().Comment;
//                    }
                        
//                    epdoc.TInspectionActivity = activity;
//                    if (activity.Status == 2)
//                    {
//                        epdoc.Type = Enums.InspectionMode.InProgress.ToString();
//                    }
//                    else if (activity.IsWorkOrder != 1 && activity.Status == 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.Fail.ToString();
//                    }
//                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount == 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.New.ToString();
//                    }
//                    else if (activity.IsWorkOrder == 1 && activity.OpenWOCount > 0)
//                    {
//                        epdoc.Type = Enums.InspectionMode.WOInProgress.ToString();
//                    }
//                    else
//                    {
//                        epdoc.Type = Enums.InspectionMode.New.ToString();
//                    }
//                    epdoc.CurrentStatus = activity.SubStatus;

//                    epdoc.InspectionFiles = null;
//                    epdoc.ActivityId = Guid.Empty;
//                    //maingoals = MainGoalRepository.GetGoalAndSteps(epDetailId, mode, floorAssetId, frequencyId);

//                    epdoc.InspectionFiles = null;
//                    epdoc.ActivityId = Guid.Empty;
//                    maingoals = DAL.MainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId, null);
//                    if (epdoc.Type == Enums.InspectionMode.New.ToString())
//                    {

//                        epdoc.InspectionFiles = null;
//                        epdoc.ActivityId = Guid.Empty;
//                        maingoals = DAL.MainGoalRepository.GetMainGoalByActivity(epDetailId, mode, floorAssetId, frequencyId, null);
//                    }
//                    else
//                    {
//                        epdoc.ActivityId = activity.ActivityId;
//                        if (activity.ActivityType == 1)
//                        {
//                            maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(Utility.Enums.Status.In_Progress), frequencyId);//4
//                            if (maingoals.Count > 0)
//                            {
//                                epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
//                                epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
//                                foreach (var goal in maingoals)
//                                {
//                                    var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, goal.InspectionDetailId, frequencyId);
//                                    if (epdoc.Type == Enums.InspectionMode.Fail.ToString() || epdoc.Type == Enums.InspectionMode.WOInProgress.ToString())
//                                    {
//                                        goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != Enums.StatusCode.CMPLT.ToString()&& x.IsActive == true && x.IsCurrent == true).ToList();
//                                    }
//                                    else
//                                    {
//                                        goal.Steps = steps.Where(x => x.IsActive == true && x.IsCurrent == true && x.StepType==1).ToList();
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            var epsteps = maingoals.FirstOrDefault(x => x.ActivityType == 1);
//                            mode=epsteps.ActivityType;
//                             _maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId), Convert.ToInt32(Utility.Enums.Status.In_Progress), frequencyId);//4
//                            if (_maingoals.Count > 0)
//                            {
//                                epdoc.InspectionFiles = floorAssetId != null ? GetInspectionAssetFiles(Convert.ToInt32(floorAssetId), Convert.ToInt32(inspectionId)) : GetInspectionEpFiles(Convert.ToInt32(inspectionId));
//                                epdoc.InspectionFiles = epdoc.InspectionFiles.Count > 0 ? epdoc.InspectionFiles : null;
//                                foreach (var goal in maingoals.Where(x => x.ActivityType == 1))
//                                {
//                                    var steps = GetStepsTransactionalRecords(mode, epDetailId, Convert.ToInt32(floorAssetId), goal.MainGoalId, _maingoals.FirstOrDefault().InspectionDetailId, frequencyId);
//                                    if (epdoc.Type == Enums.InspectionMode.Fail.ToString() || epdoc.Type == Enums.InspectionMode.WOInProgress.ToString())
//                                    {
//                                        goal.Steps = steps.Where(x => x.Status == 0 && x.DeficiencyStatus != Enums.StatusCode.CMPLT.ToString()&& x.IsActive == true).ToList();
//                                    }
//                                    else
//                                    {
//                                        goal.Steps = steps.Where(x => x.IsActive == true && x.IsCurrent == true && x.StepType == 1).ToList();
//                                    }
//                                }
                               
//                            }

//                        }
                        
                        
//                        maingoals = maingoals.Where(x => x.Steps.Count > 0).ToList();
//                        epdoc.ActivityId = activity.ActivityId;
//                    }
//                }
//                if (inspection != null)
//                {
//                    epdoc.Inspection = inspection;
//                }
//                if(epmode == 3)
//                {
//                    epdoc.MainGoal = maingoals.Where(x => x.ActivityType != 2 && x.ActivityType != 1 && x.Steps.Any()).OrderByDescending(x => x.ActivityType).ToList();

//                }
//                else
//                {
//                    epdoc.MainGoal = maingoals.Where(x => x.ActivityType != 2 && x.Steps.Any()).OrderByDescending(x => x.ActivityType).ToList();
//                }
                
                
               
//            }
           
//            return epdocs;
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inspectionId"></param>
//        /// <param name="activityId"></param>
//        /// <param name="subStatus"></param>
//        public static bool UpdateInspectionSubstatus(int inspectionId, Guid activityId, string subStatus)
//        {
//            return DAL.Transaction.UpdateInspectionSubstatus(inspectionId, activityId, subStatus);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="subStatus"></param>
//        /// <returns></returns>
//        public static List<Inspection> GetInspection(string subStatus)
//        {
//            return DAL.InspectionRepository.GetInspection(subStatus);
//        }

//        #endregion

//        #region EPHistory

//        public static EPDetails GetEpInspectionHistory(int epDetailId)
//        {
//            return DAL.EpRepository.GetEpInspectionHistory(epDetailId);
//        }

//        public static EPDetails GetEpInspectionActivity(int epDetailId, int inspectionId)
//        {
//            return DAL.EpRepository.GetEpInspectionHistory(epDetailId, inspectionId);
//        }

//        public static EPDetails GetInspectionHistory(int userId, int epDetailId, int inspectionId)
//        {
//            return DAL.EpRepository.GetInspectionHistory(userId, epDetailId, inspectionId);
//        }

//        public static IEnumerable<Inspection> GetEpsInspections(int epDetailId)
//        {
//            return DAL.InspectionRepository.GetEpsInspections(epDetailId);
//        }

//        public static Inspection GetActivityHistory(Guid activityId)
//        {
//            return DAL.InspectionRepository.GetActivityHistory(activityId);
//        }

//        #endregion

//        #region IHCFRepository

//        public static List<InspectionEPDocs> RepositoryDashboard(int userId)
//        {
//            return DAL.Transaction.GetUserInsDoc(userId);
//        }

//        public static List<InspectionEPDocs> GetEpDocByBinder(int binderId)
//        {
//            return DAL.Transaction.GetEpDocByBinder(binderId);
//        }

//        public static EPDetails GetDocsbyEp(int userId, int epdetailId)
//        {
//            return DAL.EpRepository.GetDocsbyEp(userId, epdetailId);
//        }

//        #endregion

//        #region Reports

//        public static List<InspectionEPDocs> GetDocumentReport(int userId)
//        {
//            return DAL.Transaction.GetDocumentReport(userId);
//        }

//        public static List<InspectionEPDocs> GetDocumentHistory(int fileId)
//        {
//            return DAL.Transaction.GetDocumentHistory(fileId);
//        }

//        //public static EPDetails GetDocumentHistorybyEP(int userId, int epdetailId)
//        //{
//        //    return HCF.DAL.Transaction.GetDocumentHistorybyEP(userId, epdetailId);
//        //}

//        #endregion

//        #region TInspection

//        public static List<Inspection> GetAllInspections()
//        {
//            return DAL.InspectionRepository.GetCurrentInspections();
//        }

//        #endregion

//        #region TInspection Activity

//        public static List<EPSteps> GetAllEpDocs(int userId, int epDetailId)
//        {
//            var epDocs = DAL.Transaction.GetAllEpDocs(userId, epDetailId);
//            return epDetailId > 0 ? epDocs.Where(x => x.EPDetailId == epDetailId).ToList() : epDocs;
//        }


//        public static List<EPSteps> GetAllEpDocs(int userId)
//        {
//            var mode = Convert.ToInt32(Enums.Mode.DOC);
//            var epdocs = DAL.Transaction.GetAllEpDocs(userId, 0);
//            foreach (var epdoc in epdocs)
//            {
//                if (Convert.ToInt32(epdoc.InspectionId) > 0)
//                {
//                    var maingoals = DAL.MainGoalRepository.GetGoalTransactionalRecords(mode, Convert.ToInt32(epdoc.EPDetailId), null,
//                        Convert.ToInt32(epdoc.InspectionId), Convert.ToInt32(Enums.Status.In_Progress), 0);
//                    if (maingoals.Count > 0)
//                    {
//                        epdoc.MainGoal = maingoals;
//                        foreach (var goal in maingoals)
//                        {
//                            var steps = GetStepsTransactionalRecords(mode, epdoc.EPDetailId, null,
//                                goal.MainGoalId, goal.InspectionDetailId, null);
//                            goal.Steps = steps;
//                        }
//                    }
//                    else
//                    {
//                        epdoc.InspectionFiles = null;
//                        epdoc.MainGoal = DAL.MainGoalRepository.GetMainGoalByActivity(epdoc.EPDetailId, mode, null, 0,null);
//                    }
//                }
//                else
//                {
//                    epdoc.InspectionFiles = null;
//                    epdoc.MainGoal = DAL.MainGoalRepository.GetMainGoalByActivity(epdoc.EPDetailId, mode, null, 0,null);
//                }
//                if (epdoc.MainGoal != null)
//                    if (epdoc.MainGoal.Count > 0)
//                        foreach (var goal in epdoc.MainGoal)
//                            goal.Steps = goal.Steps.Where(m => m.IsCurrent).ToList();

//            }
//            return epdocs;
//        }

//        public static List<EPDetails> GetRecentActivity(int userId)
//        {
//            return DAL.EpRepository.GetRecentActivity(userId);
//        }

//        public static TInspectionActivity GetTInspectionActivity(Guid activityId)
//        {
//            return DAL.InspectionActivityRepository.GetTInspectionActivity(activityId);

//        }

//        public static List<Inspection> GetComplianceReport()
//        {
//            var inspections = GetComplianceInspections();
//            foreach (var item in inspections)
//            {
//                foreach (var inspectionActivity in item.TInspectionActivity.Where(x => x.ActivityType == 2).ToList())
//                {
//                    if (inspectionActivity.TFloorAssets != null && inspectionActivity.TFloorAssets.Assets != null)
//                    {
//                        if (inspectionActivity.TFloorAssets.Assets.IsStepsOnReport)
//                        {
//                            inspectionActivity.TInspectionDetail = DAL.InsDetailRepository.GetTInspectionDetails(inspectionActivity.ActivityId);
//                        }
//                    }
//                }
//            }
//            return inspections;
//        }

//        public static List<TInspectionActivity> GetComplianceReports(string assetids, string buildingId, string floorId, string fromdate, string todate, string status = null)
//        {
//            List<TInspectionActivity> inspectionActivities = GetComplianceReports(assetids);
//            //if (!string.IsNullOrEmpty(assetids))
//            //{
//            //    inspectionActivities = inspectionActivities.Where(x => assetids.Contains(x.TFloorAssets.AssetId.ToString())).ToList();
//            //}
//            if (!string.IsNullOrEmpty(buildingId) && Convert.ToInt32(buildingId) > 0)
//            {
//                inspectionActivities = inspectionActivities.Where(x => x.TFloorAssets.Floor.BuildingId == Convert.ToInt32(buildingId)).ToList();
//            }
//            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0)
//            {
//                inspectionActivities = inspectionActivities.Where(x => x.TFloorAssets.FloorId == Convert.ToInt32(floorId)).ToList();
//            }
//            if (!string.IsNullOrEmpty(status))
//            {
//                inspectionActivities = inspectionActivities.Where(x => x.Status == Convert.ToInt32(status)).ToList();
//            }
//            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
//            {
//                inspectionActivities = inspectionActivities.Where(x => x.ActivityInspectionDate.Value.Date >= Convert.ToDateTime(fromdate).Date && x.ActivityInspectionDate.Value.Date <= Convert.ToDateTime(todate).Date).ToList();
//            }
//            return inspectionActivities;
//        }


//        private static List<TInspectionActivity> GetComplianceReports(string assetids)
//        {
//            var inspectionActivities = DAL.InspectionActivityRepository.GetComplianceRpeort(assetids);
//            foreach (var inspectionActivity in inspectionActivities)
//            {
//                if (inspectionActivity.TFloorAssets != null && inspectionActivity.TFloorAssets.Assets != null)
//                {
//                    if (inspectionActivity.TFloorAssets.Assets.IsStepsOnReport)
//                    {
//                        inspectionActivity.TInspectionDetail = DAL.InsDetailRepository.GetTInspectionDetails(inspectionActivity.ActivityId);
//                    }
//                }
//            }
//            return inspectionActivities;
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        private static List<Inspection> GetComplianceInspections()
//        {
//            var inspections = DAL.InspectionRepository.GetInspections().Where(x => x.IsCurrent && (x.EPDetails.IsDocRequired || x.EPDetails.IsAssetsRequired)).ToList();
//            foreach (var inspection in inspections)
//            {
//                inspection.TInspectionActivity = DAL.InspectionActivityRepository.GetTInspectionActivity(inspection.InspectionId).Where(x => x.IsCurrent).ToList();
//            }
//            return inspections;
//        }



//        public static bool DeleteTInspectionFiles(int tinsFileId)
//        {
//            return DAL.Transaction.DeleteTInspectionFiles(tinsFileId);
//        }


//        #endregion

//        public static List<InspectionEPDocs> GetEpInspectionDoc(int epDetIlId)
//        {
//            return DAL.Transaction.GetEpInspectionDoc(epDetIlId);
//        }

//        public static List<Inspection> GetInspectionsforWorkOrder(string activityId)
//        {
//            return DAL.InspectionRepository.GetInspectionsforWorkOrder(activityId);
//        }
        

//        public static List<Inspection> GetDeficiencies(string activityId)
//        {
//            return DAL.InspectionRepository.GetDeficiencies(activityId);
//        }
//        public static List<EPsDocument> GetInspectionDocuments(int userId)
//        {
//            return DAL.Transaction.GetInspectionDocuments(userId);
//        }

//        public static List<DocumentType> GetPolicyBinders(int userId,int selectedUser, int? epDetailId)
//        {
//            return DAL.Transaction.GetPolicyBinders(userId, selectedUser, epDetailId);
//        }

//        public static DocumentType PolicyBinderHistory(int userId)
//        {
//            return DAL.Transaction.PolicyBinderHistory(userId);
//        }

//        public static int InsertUpdateEpConfig(EpConfig epConfig,int mode, string dates)
//        {
//            return DAL.Transaction.InsertUpdateEpConfig(epConfig, mode, dates);
//        }

//        public static bool DeleteEpDocs(Guid activityId)
//        {
//            return DAL.Transaction.DeleteEpDocs(activityId);
//        }

//        public static EPDetails GetCurrentEp(int epId)
//        {
//            return DAL.Transaction.GetCurrentEp(epId);
//        }
//        public static List<InspectionEPDocs> GetRelationEpInspectionDoc(int epDetIlId)
//        {
//            return DAL.Transaction.GetRelationEpInspectionDoc(epDetIlId);
//        }
//        public static int AddTInspectionCampus(Buildings Campus , int InspectionId, int Epdetailid)
//        {
//            return DAL.Transaction.AddTinspectionCampus(Campus,  InspectionId, Epdetailid);
//        }
//    }
//}
