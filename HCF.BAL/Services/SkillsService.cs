using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class SkillsService : ISkillsService
    {
        private readonly ISkillsRepository _skillsRepository;
        public SkillsService(ISkillsRepository skillsRepository)
        {
            _skillsRepository = skillsRepository;
        }
        public  int Save(Skills skills)
        {
            return _skillsRepository.Save(skills);
        }

        public  List<Skills> GetSkills()
        {
            return _skillsRepository.GetSkills();
        }

        public Skills GetSkills(string skillCode)
        {
            return _skillsRepository.GetSkills(skillCode);
        }
    }

}
