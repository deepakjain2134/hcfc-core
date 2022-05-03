using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public  int AddSchedule(Schedules newSchedule)
        {
            return _scheduleRepository.AddSchedule(newSchedule);
        }

        public  int AddScheduleFrequency(ScheduleFrequency newScheduleFrequency)
        {
            return _scheduleRepository.AddScheduleFrequency(newScheduleFrequency);
        }

        public  Schedules GetSchedules(int epId, int? floorAssetId )
        {
            return _scheduleRepository.GetSchedules(epId,floorAssetId).FirstOrDefault();
        }


        public  List<Schedules> GetSchedule()
        {
            return _scheduleRepository.GetSchedules(0);
        }

        public  Schedules GetSchedule(int scheduleId)
        {
            return _scheduleRepository.GetSchedules(scheduleId).FirstOrDefault();//.FirstOrDefault(x=>x.ScheduleId== scheduleId);
        }

        public  int SaveAssetsSchedule(int scheduleId, string floorAssetIds, int epId, int activityType)
        {
            return _scheduleRepository.SaveAssetsSchedule(scheduleId, floorAssetIds, epId, activityType);
        }

        public  bool UpdateSchedule(int scheduleId, bool isActive)
        {
            return _scheduleRepository.UpdateSchedule(scheduleId, isActive);
        }
    }
}
