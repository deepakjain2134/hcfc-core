using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IScheduleService
    {
        int AddSchedule(Schedules newSchedule);
        int AddScheduleFrequency(ScheduleFrequency newScheduleFrequency);
        List<Schedules> GetSchedule();
        Schedules GetSchedule(int scheduleId);
        Schedules GetSchedules(int epId, int? floorAssetId);
        int SaveAssetsSchedule(int scheduleId, string floorAssetIds, int epId, int activityType);
        bool UpdateSchedule(int scheduleId, bool isActive);
    }
}