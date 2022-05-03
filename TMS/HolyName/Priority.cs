using System.Collections.Generic;

namespace TMS
{
    public static class Priority
    {
        public static List<HCF.BDO.Priority> GetTmsPriorities(string modulecode)
        {
            List<HCF.BDO.Priority> priorities = new List<HCF.BDO.Priority>();
            HolyNameTmsService.HolyNamePriorityClient client = new HolyNameTmsService.HolyNamePriorityClient();
            HolyNameTmsService.Priority[] lists = client.GetCodesPriority(modulecode, 11, true);
            foreach (HolyNameTmsService.Priority list in lists)
            {
                HCF.BDO.Priority priority = new HCF.BDO.Priority();
                priority.Code = list.codeField;
                priority.Name = list.descriptionField;
                priority.ModuleCode = list.moduleCodeField;
                priority.Description = list.descriptionField;
                priority.IsActive = true;
                priorities.Add(priority);
            }
            return priorities;
        }

        public static List<HCF.BDO.Priority> GetBrukeTmsPriorities(string modulecode)
        {
            List<HCF.BDO.Priority> priorities = new List<HCF.BDO.Priority>();
            HolyNameTmsService.BurkePriorityClient client = new HolyNameTmsService.BurkePriorityClient();
            HolyNameTmsService.Priority1[] lists = client.GetBurkeCodesPriority(modulecode, 1, true);
            foreach (HolyNameTmsService.Priority1 list in lists)
            {
                HCF.BDO.Priority priority = new HCF.BDO.Priority();
                priority.Code = list.codeField;
                priority.Name = list.descriptionField;
                priority.ModuleCode = list.moduleCodeField;
                priority.Description = list.descriptionField;
                priority.IsActive = true;
                priorities.Add(priority);
            }
            return priorities;
        }
    }
}
