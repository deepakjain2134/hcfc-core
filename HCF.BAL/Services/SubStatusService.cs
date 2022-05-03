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
    public class SubStatusService : ISubStatusService
    {
        private readonly ISubStatusRepository _subStatusRepository;
        public SubStatusService(ISubStatusRepository subStatusRepository)
        {
            _subStatusRepository = subStatusRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="substatus"></param>
        /// <returns></returns>
        public int Save(SubStatus substatus)
        {
            return _subStatusRepository.Save(substatus);
        }


        public List<SubStatus> GetSubStatus()
        {
            return _subStatusRepository.GetSubStatus();
        }
    }
}
