using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ISkillsRepository
    {
        List<Skills> GetSkills();
        Skills GetSkills(string skillCode);
        int Save(Skills skills);
    }
}