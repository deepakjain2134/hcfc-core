using HCF.BDO;

namespace HCF.DAL
{
    public interface IRoundsQuesRepository
    {
        int Save(RoundCategory newRoundCategory);
        int Save(RoundsQuestionnaires questionnaires);
        bool Update_RoundSchduleDatesOnRoundCat(RoundCategory newRoundCategory);
    }
}