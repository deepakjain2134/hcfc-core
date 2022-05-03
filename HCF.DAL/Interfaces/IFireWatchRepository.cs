using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFireWatchRepository
    {
        bool CloseScheduleLog(int id, int closedBy);
        List<FireWatchLog> GetFireWatchLog(DateTime? roundInspdate);
        ScheduledLogs GetFirewatchNotificationType();
        List<ScheduledLogs> GetScheduledLogs(int? id);
        List<ScheduledLogs> GetScheduledLogsDetail(int? id);
        int Save(FireWatchLog objFireWatchLog);
        int Save(ScheduledLogs objScheduledLogs);
        int SaveFirewatchNotification(TFirewatchNotificationType t);
        bool Update(ScheduledLogs objScheduledLogs);

        List<TimeSlots> GetTimeSlots(DateTime selectedDate, int timeSlotPeriod);
    }
}