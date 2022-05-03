using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int Save(WoStatus status)
        {
            return _statusRepository.Save(status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WoStatus> GetStatus()
        {
            return _statusRepository.GetStatus();
        }
    }


    #region Shift

    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        public ShiftService(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public int Save(Shift shift)
        {
            return _shiftRepository.Save(shift);
        }


        public List<Shift> GetShift()
        {
            return _shiftRepository.GetShift();
        }
    }

    #endregion
}
