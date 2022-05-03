using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public  class FireWatchService : IFireWatchService
    {
        private readonly ICommonService _commonService;
        private readonly IFireWatchRepository _fireWatchRepository;

        public FireWatchService(ICommonService commonService, IFireWatchRepository fireWatchRepository)
        {
            _fireWatchRepository = fireWatchRepository;
               _commonService = commonService;           
        }
        #region Fire Watch

        public  int SaveFireWatchLog(FireWatchLog obj)
        {
            return _fireWatchRepository.Save(obj);
        }

        public  List<FireWatchLog> GetFireWatchLog()
        {
            return _fireWatchRepository.GetFireWatchLog(null);
        }

        public  List<FireWatchLog> GetFireWatchLog(DateTime roundInspdate)
        {
            return _fireWatchRepository.GetFireWatchLog(roundInspdate);
        }

        /// <summary>
        /// </summary>
        /// <param name="selecteddate"></param>
        /// <returns></returns>
        public  List<TimeSlots> Get_FirewatchlogbyTimeSlot(DateTime dt, int timeSlotPeriod = 4)
        {
            //DateTime dt = Convert.ToDateTime(selecteddate);
            DateTime date = dt;//dt.Add(DateTime.Now.TimeOfDay);
            List<TimeSlots> timeSlots = new List<TimeSlots>();
            timeSlots = _fireWatchRepository.GetTimeSlots(date, timeSlotPeriod);

            //List<FireWatchLog> logs = HCF.BAL.FireWatchRepository.GetFireWatchLog(date);
            List<ScheduledLogs> sclogs = GetScheduledLogs(null).Where(x => x.IsClosed == false).ToList();
            foreach (var item in timeSlots)
            {
                //  var listsLogs = HCF.BAL.FireWatchRepository.GetScheduledLogs(item.Start, item.End);
                var logs = GetFireWatchLog(item.Start);
                var firelog = logs.Where(x => x.RoundInspDate > item.Start && x.RoundInspDate < item.End).ToList();
                var sclog = sclogs.Where(x => (x.StartDate.Value >= item.Start && x.StartDate.Value <= item.End) || x.StartDate.Value <= item.Start && item.End >= x.StartDate.Value)
                     .Select(x => x.AreaComment);

                if (firelog.Count > 0) { item.FireWatchLog = firelog; }
                else
                {
                    item.FireWatchLog = new List<FireWatchLog>();
                    var fireWatch = new FireWatchLog
                    {
                        FinishTime = new TimeSpan(),
                        InspectorName = string.Empty,
                        Comment = string.Empty,
                        Area = string.Join(" ,\n ", sclog),
                        LogDate = date
                    };

                    fireWatch.LogDateText = fireWatch.LogDate.Value.ToString("MMM dd ,yyyy");
                    item.FireWatchLog.Add(fireWatch);
                }
            }
            return timeSlots;
        }

        #endregion


        #region ScheduledLogs


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objScheduledLogs"></param>
        /// <returns></returns>
        public  int Save(ScheduledLogs objScheduledLogs)
        {
            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) && objScheduledLogs.DSSign1Signature.DigSignatureId == 0)
            //    objScheduledLogs.Signature1Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
            //else
            //    objScheduledLogs.Signature1Id = objScheduledLogs.DSSign1Signature.DigSignatureId;

            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) && objScheduledLogs.DSSign2Signature.DigSignatureId == 0)
            //    objScheduledLogs.Signature2Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
            //else
            //    objScheduledLogs.Signature2Id = objScheduledLogs.DSSign2Signature.DigSignatureId;

            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) || objScheduledLogs.DSSign1Signature.DigSignatureId > 0)
                objScheduledLogs.Signature1Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) || objScheduledLogs.DSSign2Signature.DigSignatureId > 0)
                objScheduledLogs.Signature2Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
         

            return _fireWatchRepository.Save(objScheduledLogs);
        }



        /// <summary>
        /// </summary>
        /// <param name="objScheduledLogs"></param>
        /// <returns></returns>
        public  int SaveFirewatchNotification(TFirewatchNotificationType firewatch)
        {

            return _fireWatchRepository.SaveFirewatchNotification(firewatch);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objScheduledLogs"></param>
        public  bool Update(ScheduledLogs objScheduledLogs)
        {
            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) && objScheduledLogs.DSSign1Signature.DigSignatureId == 0)
            //    objScheduledLogs.Signature1Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
            //else
            //    objScheduledLogs.Signature1Id = objScheduledLogs.DSSign1Signature.DigSignatureId;

            //if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) && objScheduledLogs.DSSign2Signature.DigSignatureId == 0)
            //    objScheduledLogs.Signature2Id = CommonRepository.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());
            //else
            //    objScheduledLogs.Signature2Id = objScheduledLogs.DSSign2Signature.DigSignatureId;
            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign1Signature.FileContent) || objScheduledLogs.DSSign1Signature.DigSignatureId > 0)
                objScheduledLogs.Signature1Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign1Signature, objScheduledLogs.Id.ToString());
            if (!string.IsNullOrEmpty(objScheduledLogs.DSSign2Signature.FileContent) || objScheduledLogs.DSSign2Signature.DigSignatureId > 0)
                objScheduledLogs.Signature2Id = _commonService.SaveDigitalFile(objScheduledLogs.DSSign2Signature, objScheduledLogs.Id.ToString());


            return _fireWatchRepository.Update(objScheduledLogs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<ScheduledLogs> GetScheduledLogs(int? id)
        {
            return _fireWatchRepository.GetScheduledLogs(id);
        }
        public  List<ScheduledLogs> GetScheduledLogsDetail(int? id)
        {
            return _fireWatchRepository.GetScheduledLogsDetail(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  bool CloseSchedulelog(int Id, int closedBy)
        {
            return _fireWatchRepository.CloseScheduleLog(Id, closedBy);
        }

        public  ScheduledLogs GetFirewatchNotificationType()
        {
            return _fireWatchRepository.GetFirewatchNotificationType();
        }


        //public  List<ScheduledLogs> GetScheduledLogs(DateTime start, DateTime end)
        //{
        //    return _fireWatchRepository.GetScheduledLogs(start, end);
        //}

        #endregion

    }
}
