using System.Collections.Generic;

namespace TMS
{
    public static class Skill
    {
        public static List<HCF.BDO.Skills> GetTmsSkill()
        {
            List<HCF.BDO.Skills> skills = new List<HCF.BDO.Skills>();
            HolyNameTmsService.HolyNameSkillClient client = new HolyNameTmsService.HolyNameSkillClient();
            HolyNameTmsService.Skill[] lists = client.GetCodesSkill(11, true);
            foreach (HolyNameTmsService.Skill list in lists)
            {
                HCF.BDO.Skills skill = new HCF.BDO.Skills();
                skill.Code = list.codeField;
                skill.Name = list.descriptionField;
                skill.Description = list.descriptionField;
                skill.IsActive = true;
                skills.Add(skill);
            }
            return skills;
        }

        public static List<HCF.BDO.Skills> GetBrukeTmsSkill()
        {
            List<HCF.BDO.Skills> skills = new List<HCF.BDO.Skills>();
            HolyNameTmsService.BurkeSkillClient client = new HolyNameTmsService.BurkeSkillClient();
            //HolyNameTmsService.Skill[] lists = client.GetBurkeCodesSkill(1,true);
            //foreach (HolyNameTmsService.Skill list in lists)
            //{
            //    HCF.BDO.Skills skill = new HCF.BDO.Skills();
            //    skill.Code = list.codeField;
            //    skill.Name = list.descriptionField;
            //    skill.Description = list.descriptionField;
            //    skill.IsActive = true;
            //    skills.Add(skill);
            //}
            return skills;
        }
    }
}
