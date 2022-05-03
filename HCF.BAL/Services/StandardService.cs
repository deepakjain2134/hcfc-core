using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class StandardService : IStandardService
    {
        private readonly IStandardRepository _standardRepository;
        public StandardService(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        public  Standard GetEpHistory(int epdetailId)
        {
            return _standardRepository.GetEpHistory(epdetailId);
        }

        public  List<Standard> Get()
        {
            return _standardRepository.Get();
        }

        public  Standard Get(int stdescId)
        {
            return _standardRepository.Get().FirstOrDefault(x=>x.StDescID== stdescId);
        }

        public  int AddStandard(Standard newdata)
        {
            return _standardRepository.Save(newdata);
        }

        public  List<Standard> GetStandards()
        {
            return _standardRepository.GetStandards();
        }


        public  List<Standard> DocumentStandardReports()
        {
            return _standardRepository.DocumentStandardReports();
        }

        public  Standard GetStandard(int stdescId)
        {
            return _standardRepository.GetStandard(stdescId);
        }

        public  bool UpdateStandard(Standard newdata)
        {
            return _standardRepository.UpdateStandard(newdata);
        }

        public  List<Standard> GetEPlists()
        {
            return _standardRepository.GetEPlists();
        }

        public  List<EPDetails> GetUserEPs(int userId)
        {
            return _standardRepository.GetUserEPs(userId);
        }
    }
}
