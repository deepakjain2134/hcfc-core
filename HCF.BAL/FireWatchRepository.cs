//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HCF.BDO;


//namespace HCF.BAL
//{
//    public static class FireWatchRepository
//    {
//        #region Fire Watch

//        public static int SaveFireWatchLog(FireWatchLog obj)
//        {
//            return DAL.FireWatchRepository.Save(obj);
//        }

//        public static List<FireWatchLog> GetFireWatchLog()
//        {
//            return DAL.FireWatchRepository.GetFireWatchLog(null);
//        }

//        public static List<FireWatchLog> GetFireWatchLog(DateTime roundInspdate)
//        {
//            return DAL.FireWatchRepository.GetFireWatchLog(roundInspdate);
//        }

//        /// <summary>
//        /// </summary>
//        /// <param name="selecteddate"></param>
//        /// <returns></returns>
//        public static List<TimeSlots> Get_FirewatchlogbyTimeSlot(DateTime dt, int timeSlotPeriod = 4)
//        {
//            //DateTime dt = Convert.ToDateTime(selecteddate);
//            DateTime date = dt;//dt.Add(DateTime.Now.TimeOfDay);
//            List<TimeSlots> timeSlots = new List<TimeSlots>();
//            timeSlots = Rounds.GetTimeSlots(date, timeSlotPeriod);

//            //List<FireWatchLog> logs = HCF.BAL.FireWatchRepository.GetFireWatchLog(date);
//            List<ScheduledLogs> sclogs = GetScheduledLogs(null).Where(x => x.IsClosed == false).ToList();
//            foreach (var item in timeSlots)
//            {
//                //  var listsLogs = HCF.BAL.FireWatchRepository.GetScheduledLogs(item.Start, item.End);
//                var logs = GetFireWatchLog(item.Start);
//                var firelog = logs.Where(x => x.RoundInspDate > item.Start && x.RoundInspDate < item.End).ToList();
//                var sclog = sclogs.Where(x => (x.StartDate.Value >= item.Start && x.StartDate.Value <= item.End) || x.StartDate.Value <= item.Start && item.End >= x.StartDate.Value)
//                     .Select(x => x.AreaComment);

//                if (firelog.Count > 0) { item.FireWatchLog = firelog; }
//                else
//                {
//                    item.FireWatchLog = new List<FireWatchLog>();
//                    var fireWatch = new FireWatchLog
//                    {
//                        FinishTime = new TimeSpan(),
//                        InspectorName = string.Empty,
//                        Comment = string.Empty,
//                        Area = string.Join(" ,\n ", sclog),
//                        LogDate = date
//                    };

//                    fireWatch.LogDateText = fireWatch.LogDate.Value.ToString("MMM dd ,yyyy");
//                    item.FireWatchLog.Add(fireWatch);
//                }
//            }
//            return timeSlots;
//        }

//        #endregion


//        #region ScheduledLogs


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="objScheduledLogs"></param>
//        /// <returns></returns>
//        public static int Save(ScheduledLogs objScheduledLogs)
//        {
//            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) && objScheduledLogs.DSSign1Signature.DigSignatureId == 0)
//            //    objScheduledLogs.Signature1Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
//            //else
//            //    objScheduledLogs.Signature1Id = objScheduledLogs.DSSign1Signature.DigSignatureId;

//            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) && objScheduledLogs.DSSign2Signature.DigSignatureId == 0)
//            //    objScheduledLogs.Signature2Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
//            //else
//            //    objScheduledLogs.Signature2Id = objScheduledLogs.DSSign2Signature.DigSignatureId;

//            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) || objScheduledLogs.DSSign1Signature.DigSignatureId > 0)
//                objScheduledLogs.Signature1Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
//            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) || objScheduledLogs.DSSign2Signature.DigSignatureId > 0)
//                objScheduledLogs.Signature2Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
         

//            return DAL.FireWatchRepository.Save(objScheduledLogs);
//        }



//        /// <summary>
//        /// </summary>
//        /// <param name="objScheduledLogs"></param>
//        /// <returns></returns>
//        public static int SaveFirewatchNotification(TFirewatchNotificationType firewatch)
//        {

//            return DAL.FireWatchRepository.SaveFirewatchNotification(firewatch);
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="objScheduledLogs"></param>
//        public static bool Update(ScheduledLogs objScheduledLogs)
//        {
//            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) && objScheduledLogs.DSSign1Signature.DigSignatureId == 0)
//            //    objScheduledLogs.Signature1Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
//            //else
//            //    objScheduledLogs.Signature1Id = objScheduledLogs.DSSign1Signature.DigSignatureId;

//            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) && objScheduledLogs.DSSign2Signature.DigSignatureId == 0)
//            //    objScheduledLogs.Signature2Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
//            //else
//            //    objScheduledLogs.Signature2Id = objScheduledLogs.DSSign2Signature.DigSignatureId;
//            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) || objScheduledLogs.DSSign1Signature.DigSignatureId > 0)
//                objScheduledLogs.Signature1Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
//            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) || objScheduledLogs.DSSign2Signature.DigSignatureId > 0)
//                objScheduledLogs.Signature2Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());


//            return DAL.FireWatchRepository.Update(objScheduledLogs);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<ScheduledLogs> GetScheduledLogs(int? id)
//        {
//            return DAL.FireWatchRepository.GetScheduledLogs(id);
//        }
//        public static List<ScheduledLogs> GetScheduledLogsDetail(int? id)
//        {
//            return DAL.FireWatchRepository.GetScheduledLogsDetail(id);
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Id"></param>
//        /// <returns></returns>
//        public static bool CloseSchedulelog(int Id, int closedBy)
//        {
//            return DAL.FireWatchRepository.CloseScheduleLog(Id, closedBy);
//        }

//        public static ScheduledLogs GetFirewatchNotificationType()
//        {
//            return DAL.FireWatchRepository.GetFirewatchNotificationType();
//        }


//        //public static List<ScheduledLogs> GetScheduledLogs(DateTime start, DateTime end)
//        //{
//        //    return DAL.FireWatchRepository.GetScheduledLogs(start, end);
//        //}

//        #endregion

//    }
//}
