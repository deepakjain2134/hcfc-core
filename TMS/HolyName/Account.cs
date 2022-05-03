using System.Collections.Generic;
using System.Linq;

namespace TMS
{
    public static class Account
    {
        public static List<HCF.BDO.Department> GetTmsAccount()
        {
            var client = new HolyNameTmsService.HolyNameAccountClient();
            var lists = client.GetCodesAccount(11, false);
            return lists.Select(list => new HCF.BDO.Department
            {
                Code = list.codeField,
                Name = list.descriptionField
            }).ToList();
        }

        public static List<HCF.BDO.Department> GetBrukeTmsAccount()
        {
            var client = new HolyNameTmsService.BurkeAccountClient();
            var lists = client.GetBurkeCodesAccount(1, false);

            return lists.Select(list => new HCF.BDO.Department
            {
                Code = list.codeField,
                Name = list.descriptionField
            }).ToList();
        }
    }
}
