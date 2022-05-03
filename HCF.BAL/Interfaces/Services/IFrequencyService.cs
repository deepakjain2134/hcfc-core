using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IFrequencyService
    {
        List<FrequencyMaster> GetFrequency();
        FrequencyMaster GetFrequency(int id);
        int Save(FrequencyMaster frequencyMaster);
        bool Update(FrequencyMaster frequencyMaster);
    }
}