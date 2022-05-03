using System.Collections.Generic;
using System.Linq;

namespace TMS
{
    public static class Site
    {
        public static List<HCF.BDO.Site> GetTmsSites()
        {
            var client = new HolyNameTmsService.HolyNameSiteClient();
            var lists = client.GetCodesSites(11, true);
            return lists.Select(list => new HCF.BDO.Site
            {
                Code = list.codeField,
                Name = list.descriptionField,
                Description = list.descriptionField,
                IsActive = true
            }).ToList();
        }

        public static List<HCF.BDO.Site> GetBurkeTmsSites()
        {
            var client = new HolyNameTmsService.BurkeSiteClient();
            var lists = client.GetBurkeSiteCodes(1, true);
            return lists.Select(list => new HCF.BDO.Site
            {
                Code = list.codeField,
                Name = list.descriptionField,
                Description = list.descriptionField,
                IsActive = true
            }).ToList();
        }
    }
}
