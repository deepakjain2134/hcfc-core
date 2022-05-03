using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.DAL
{
    public interface IScheduleRepository
    {
        int AddSchedule(Schedules newSchedule);
        int AddScheduleFrequency(ScheduleFrequency newScheduleFrequency);
        List<Schedules> GetSchedules(int schedulesId);
        List<Schedules> GetSchedules(int? epId, int? floorAssetId);
        Schedules GetSchedulesbyEPAndFloorAssetId(Guid activityId);
        Schedules GetSchedulesbyEPAndFloorAssetId(int epdetailId);
        Schedules GetSchedulesbyEPAndFloorAssetId(int epdetailId, int floorAssetsId);
        int SaveAssetsSchedule(int scheduleId, string floorAssetIds, int epId, int activityType);
        bool UpdateSchedule(int scheduleId, bool isActive);
    }
}