using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ISkillsService
    {
        List<Skills> GetSkills();
        Skills GetSkills(string skillCode);
        int Save(Skills skills);
    }
}