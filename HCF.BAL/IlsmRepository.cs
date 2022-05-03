//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace HCF.BAL
//{
//    public static class IlsmRepository
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="affectedEps"></param>
//        /// <returns></returns>
//        public static bool UpdateIlsmMatrix(AffectedEPs affectedEps)
//        {
//            return DAL.IlsmRepository.UpdateIlsmMatrix(affectedEps);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="steps"></param>
//        /// <returns></returns>
//        public static bool UpdateIlsmScore(Steps steps)
//        {
//            return DAL.IlsmRepository.UpdateIlsmScore(steps);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="stepId"></param>
//        /// <param name="ilsmStepId"></param>
//        /// <returns></returns>
//        //public static bool AddStepWithIlLsmStep(int stepId, int ilsmStepId)
//        //{
//        //    return DAL.IlsmRepository.AddStepWithIlLsmStep(stepId, ilsmStepId);
//        //}

//        public static List<TIlsm> GetIlsm()
//        {
//            return DAL.IlsmRepository.GetIlsm();
//        }

//        //public static async Task<List<TIlsm>> GetIlsmAsync()
//        //{
//        //    return await DAL.IlsmRepository.GetIlsmAsync().ConfigureAwait(false);
//        //}

//        //public static TIlsm GetIlsm(int tilsmId)
//        //{
//        //    return DAL.IlsmRepository.GetAllIlsm(null,null,tilsmId).FirstOrDefault();
//        //}

//        public static List<TIlsm> GetAllIlsm(DateTime? fromDate = null, DateTime? toDate = null)
//        {
//            return DAL.IlsmRepository.GetAllIlsm(fromDate, toDate);
//        }

//        public static TIlsm GetIlsmInspection(int tilsmId)
//        {
//            var tilsminspection = DAL.IlsmRepository.GetIlsmInspection(tilsmId);

//            if (tilsminspection.TinspectionActivity.Any())
//            {
//                var currentActivity = tilsminspection.TinspectionActivity.FirstOrDefault(x => x.IsCurrent);
//                var stepscomments = DAL.StepsRepository.GetStepsComments(currentActivity.ActivityId, tilsmId);
//                var files = DAL.Transaction.GetIlsmInspectionFiles(currentActivity.ActivityId);
//                currentActivity.TInspectionFiles = files.Where(x => x.StepsId == 0 || x.StepsId == null).ToList(); //DAL.Transaction.GetIlsmInspectionFiles(currentActivity.ActivityId);
//                tilsminspection.IlsmComments = new List<TComment>();
//                tilsminspection.IlsmComments = stepscomments.Where(x => x.StepId == 0).ToList();
//                tilsminspection.EpStepsComplete = currentActivity.Status == 1;

//                var goals = new List<MainGoal>();
//                foreach (var details in currentActivity.TInspectionDetail)
//                {
//                    MainGoal goal;
//                    goal = details.MainGoal;

//                    goal.InspectionDetailId = details.InspectionDetailId;
//                    goal.Steps = new List<Steps>();
//                    var steps = details.InspectionSteps;
//                    foreach (var step in steps)
//                    {
//                        Steps maingoalStep;
//                        maingoalStep = step.Steps;
//                        maingoalStep.Status = step.Status;
//                        maingoalStep.FileName = step.FileName;
//                        maingoalStep.Comments = step.Comments;
//                        maingoalStep.FilePath = step.FilePath;
//                        maingoalStep.StepsComments = new List<TComment>();
//                        maingoalStep.StepsComments = stepscomments.Where(x => x.StepId == step.StepsId).ToList();
//                        maingoalStep.TInspectionFiles = new List<TInspectionFiles>();
//                        maingoalStep.TInspectionFiles = files.Where(x => x.StepsId == step.StepsId).ToList();
//                        goal.Steps.Add(maingoalStep);
//                    }
//                    goals.Add(goal);
//                }

//                tilsminspection.MainGoal = goals;//.OrderBy(x => x.MainGoalId).ToList();
//                tilsminspection.TInspectionFiles = currentActivity.TInspectionFiles;
//                tilsminspection.Comment = currentActivity.Comment;
//            }
//            else
//            {
//                var temactivity = new TInspectionActivity
//                {
//                    ActivityId = Guid.Empty
//                };
//                tilsminspection.TinspectionActivity = new List<TInspectionActivity>();
//                tilsminspection.TinspectionActivity.Add(temactivity);
//            }
//            return tilsminspection;
//        }

//        public static TIlsm GetIlsmDetails(int tilsmId)
//        {
//            var tilsminspection = DAL.IlsmRepository.GetIlsmDetails(tilsmId);
//            if (tilsminspection.TinspectionActivity.Any())
//            {
//                var currentActivity = tilsminspection.TinspectionActivity.FirstOrDefault(x => x.IsCurrent);
//                //List<TComment> stepscomments = StepsRepository.GetStepsComments(currentActivity.ActivityId, tilsmId);
//                if (currentActivity != null)
//                {
//                    currentActivity.TInspectionFiles = DAL.Transaction
//                        .GetIlsmInspectionFiles(currentActivity.ActivityId)
//                        .Where(x => x.StepsId == 0 || x.StepsId == null).ToList();
//                    tilsminspection.TInspectionFiles = currentActivity.TInspectionFiles;
//                }
//            }
//            return tilsminspection;
//        }

//        public static bool ILSMlinkToWO(int tilsmId, int issueId)
//        {
//            return DAL.IlsmRepository.IlsMlinkToWo(tilsmId, issueId);
//        }

//        public static bool UpdateILSMStatus(TIlsm tilsmId)
//        {
//            return DAL.IlsmRepository.UpdateIlsmStatus(tilsmId);
//        }

//        public static bool SaveAdditionalTilsmEP(int epDetailId, int tilsmId, bool isActive)
//        {
//            return DAL.IlsmRepository.SaveAdditionalTilsmEp(epDetailId, tilsmId, isActive);
//        }

//        //public static void UpdateIlsmStatus(Guid? activityId = null)
//        //{
//        //    //Change the deficiency code of tinspectionSteps
//        //    UpdateTinspectionSteps();

//        //    //Change the Substatus code of inspection & inspectionActivity
//        //    UpdateTinspectionActivity();

//        //    //Check and update the ilsm status
//        //    //List<Inspection> list = HCF.BAL.InspectionRepository.GetIlsmInspection();

//        //    var currentDt = InspectionRepository.GetIlsmInspections(activityId);

//        //    foreach (DataRow row in currentDt.Rows)
//        //    {
//        //        int tilsmId = 0;
//        //        int floorId = Convert.ToInt32(row["FloorId"].ToString());
//        //        int assetId = Convert.ToInt32(row["AssetId"].ToString());
//        //        if (!string.IsNullOrEmpty(row["TilsID"].ToString()))
//        //            tilsmId = Convert.ToInt32(row["TilsID"].ToString());

//        //        //if (tilsmId == 0)
//        //        //{
//        //        var tilsm = new TIlsm
//        //        {
//        //            TIlsmId = tilsmId,
//        //            Status = (Enums.ILSMStatus)Convert.ToInt32(Enums.Status.Fail),
//        //            AssetId = assetId,
//        //            FloorId = floorId
//        //        };
//        //        tilsmId = InsertTilsm(tilsm);

//        //        if (tilsmId > 0)
//        //        {
//        //            var notificationMatrix = CommonRepository.MailNotification(Enums.NotificationCategory.ILSM.ToString(), Enums.NotificationEvent.OnCreated.ToString());
//        //            if (notificationMatrix != null)
//        //            {
//        //                SendIlsmMail(tilsmId, notificationMatrix);
//        //            }
//        //        }
//        //        // }
//        //    }
//        //}

//        //private static void SendIlsmMail(int tilsmId, NotificationMapping obj)
//        //{
//        //    var objtilsm = GetIlsmInspection(tilsmId);
//        //    Email.SendIlsmMail(objtilsm, Convert.ToInt32(Enums.NotificationCategory.ILSM), obj);
//        //}


//        //private static void UpdateTinspectionSteps()
//        //{
//        //    var dtRadate = DateTime.UtcNow;
//        //    var tinssteps = InsStepsRepository.GetInspectionSteps().Where(x => x.IsRA && x.Status == 0 && x.DeficiencyCode == Enums.InspectionSubStatus.DE.ToString()
//        //    && (x.DeficiencyStatus == Enums.StatusCode.ACTIV.ToString())).ToList();
//        //    foreach (var item in tinssteps.Where(x => x.DRTime.HasValue && x.DeficiencyDate.HasValue).ToList())
//        //    {
//        //        if (item.DeficiencyDate != null)
//        //        {
//        //            if (item.DRTime != null)
//        //            {
//        //                var raData = item.DeficiencyDate.Value.AddHours(item.DRTime.Value);
//        //                if (DateTime.UtcNow > raData)
//        //                {
//        //                    if (CommonRepository.IsSendMail(Enums.NotificationCategory.TimeToResolveToRA.ToString(), Enums.NotificationEvent.OnChanged.ToString()))
//        //                    {
//        //                        Email.SendTimeToResolveToRAMail(item.TInsStepsId, Convert.ToInt32(Enums.NotificationCategory.TimeToResolveToRA));
//        //                    }
//        //                    item.DeficiencyCode = Enums.InspectionSubStatus.RA.ToString();
//        //                    item.RaDate = dtRadate;
//        //                    InsStepsRepository.Update(item, 4);
//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        private static void UpdateTinspectionActivity()
//        {
//            //InspectionRepository.MarkInspectionRa();
//            //var data = InspectionActivityRepository.GetCurrentActivities();
//            //var inspectionActivity = data //InspectionActivityRepository.GetCurrentActivities()
//            //    .Where(x => x.SubStatus == Enums.InspectionSubStatus.RA.ToString() && x.IsCurrent && x.Status != 2)
//            //    .GroupBy(x => x.InspectionId).ToList();

//            //foreach (var item in inspectionActivity)
//            //{
//            //    if (item.Key.HasValue)
//            //    {
//            //        int RACount = 0;
//            //        int inspectionId = item.Key.Value;
//            //        var tinspeActivity = data.Where(x => x.InspectionId == item.Key.Value).ToList();
//            //        //var tinspeActivity = DAL.InspectionActivityRepository
//            //        //    .GetInspectionActivity(item.InspectionId).Where(x => x.SubStatus == Enums.InspectionSubStatus.RA.ToString() && x.IsCurrent && x.Status != 2).ToList();

//            //        RACount = tinspeActivity.Sum(x => x.RaScore.Value);
//            //        if (RACount > 0)
//            //        {
//            //            Inspection inspection = new Inspection();
//            //            inspection.InspectionId = inspectionId;
//            //            inspection.SubStatus = Enums.InspectionSubStatus.RA.ToString();
//            //            InspectionRepository.Update(inspection);
//            //        }
//            //    }
//            //}           
//        }

//        public static string GetIlsmwoDescription(int tilsmId)
//        {
//            return DAL.IlsmRepository.GetIlsmwoDescription(tilsmId);
//        }

//        //public static string CompleteIlsmIncident(int tilsmId)
//        //{
//        //    //return DAL.IlsmRepository.CompleteIlsmIncident(tilsmId);
//        //    string text = DAL.IlsmRepository.CompleteIlsmIncident(tilsmId);
//        //    if (text == "ILSM is now closed")
//        //    {
//        //        if (CommonRepository.IsSendMail(Enums.NotificationCategory.ILSM.ToString(), Enums.NotificationEvent.OnClosed.ToString()))
//        //        {
//        //            Email.SendIlsmCloseMail(tilsmId, Convert.ToInt32(Enums.NotificationCategory.ILSM));
//        //        }
//        //    }
//        //    return text;
//        //}

//        public static TIlsm GetCurrentTilsm(Guid activityId)
//        {
//            return DAL.IlsmRepository.GetCurrentTilsm(activityId);
//        }

//        public static bool SaveTilsmReport(TIlsm newTilsm)
//        {
//            return DAL.IlsmRepository.SaveTilsmReport(newTilsm);
//        }

//        public static bool IcraLinkToIslm(int icraId, int lsmId, int createdBy)
//        {
//            return DAL.IlsmRepository.IcraLinkToIslm(icraId, lsmId, createdBy);
//        }


//        private static int InsertTilsm(TIlsm tilsm)
//        {
//            return DAL.IlsmRepository.InsertTilsm(tilsm);
//        }

//        public static DataTable InsertConstructionTilsm(TIlsm tilsm, string epDetails)
//        {
//            return DAL.IlsmRepository.InsertConstructionTilsm(tilsm, epDetails);
//        }

//        public static int UpdateIlsm(TIlsm objTilsm)
//        {
//            return DAL.IlsmRepository.UpdateIlsm(objTilsm);
//        }
//    }
//}
