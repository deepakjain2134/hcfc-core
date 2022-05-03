using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IFrequencyRepository
    {
        List<FrequencyMaster> GetEpFrequency(int epDetailId);
        List<FrequencyMaster> GetFrequency();
        FrequencyMaster GetFrequency(int frequencyId);
        int Save(FrequencyMaster frequencyMaster);
        bool Update(FrequencyMaster frequencyMaster);
    }
}