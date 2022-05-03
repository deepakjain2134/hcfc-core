using System.Collections.Generic;

namespace TMS
{
    public static class Type
    {
        public static List<HCF.BDO.Types> GetTmsTypes(string modulecode)
        {
            List<HCF.BDO.Types> types = new List<HCF.BDO.Types>();
            HolyNameTmsService.HolyNameTypeClient client = new HolyNameTmsService.HolyNameTypeClient();
            HolyNameTmsService.Type1[] lists = client.GetCodesType(modulecode, 11, true);
            foreach (HolyNameTmsService.Type1 list in lists)
            {
                HCF.BDO.Types type = new HCF.BDO.Types();
                type.Code = list.codeField;
                type.Name = list.descriptionField;
                type.ModuleCode = list.moduleCodeField;
                type.Description = list.descriptionField;
                type.IsActive = true;
                types.Add(type);
            }
            return types;
        }

        public static List<HCF.BDO.Types> GetBrukeTmsTypes(string modulecode)
        {
            List<HCF.BDO.Types> types = new List<HCF.BDO.Types>();
            HolyNameTmsService.BurkeTypeClient client = new HolyNameTmsService.BurkeTypeClient();
            HolyNameTmsService.Type[] lists = client.GetBurkeCodesType(modulecode, 1, true);
            foreach (HolyNameTmsService.Type list in lists)
            {
                HCF.BDO.Types type = new HCF.BDO.Types();
                type.Code = list.codeField;
                type.Name = list.descriptionField;
                type.ModuleCode = list.moduleCodeField;
                type.Description = list.descriptionField;
                type.IsActive = true;
                types.Add(type);
            }
            return types;
        }
    }
}
