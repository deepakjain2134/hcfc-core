using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class FrequencyService : IFrequencyService
    {
        private readonly IFrequencyRepository _frequencyRepository;
        public FrequencyService(IFrequencyRepository frequencyRepository)
        {
            _frequencyRepository = frequencyRepository;
        }
        public  List<FrequencyMaster> GetFrequency()
        {
            return _frequencyRepository.GetFrequency();
        }

        public  FrequencyMaster GetFrequency(int id)
        {
            return _frequencyRepository.GetFrequency(id);
        }

        public  int Save(FrequencyMaster frequencyMaster)
        {
            return _frequencyRepository.Save(frequencyMaster);
        }

        public  bool Update(FrequencyMaster frequencyMaster)
        {
            return _frequencyRepository.Update(frequencyMaster);
        }
    }
}
