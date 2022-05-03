using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFireWatchService
    {
        bool CloseSchedulelog(int Id, int closedBy);
        List<FireWatchLog> GetFireWatchLog();
        List<FireWatchLog> GetFireWatchLog(DateTime roundInspdate);
        ScheduledLogs GetFirewatchNotificationType();
        List<ScheduledLogs> GetScheduledLogs(int? id);
        List<ScheduledLogs> GetScheduledLogsDetail(int? id);
        List<TimeSlots> Get_FirewatchlogbyTimeSlot(DateTime dt, int timeSlotPeriod = 4);
        int Save(ScheduledLogs objScheduledLogs);
        int SaveFireWatchLog(FireWatchLog obj);
        int SaveFirewatchNotification(TFirewatchNotificationType firewatch);
        bool Update(ScheduledLogs objScheduledLogs);
    }
}