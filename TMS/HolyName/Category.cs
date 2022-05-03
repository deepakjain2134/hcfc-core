using System.Collections.Generic;

namespace TMS
{
    public static class Category
    {
        public static List<HCF.BDO.Category> GetTmsCategory(string modulecode)
        {
            var categories = new List<HCF.BDO.Category>();
            var client = new HolyNameTmsService.HolyNameCategoryClient();
            var lists = client.GetCodesCategory(modulecode, 11, false);
            var category = new HCF.BDO.Category();
            foreach (var list in lists)
            {
                category.Code = list.codeField;
                category.Name = list.descriptionField;
                category.Description = list.descriptionField;
                category.IsActive = true;
                categories.Add(category);
            }
            return categories;
        }

        public static List<HCF.BDO.Category> GetBrukeTmsCategory(string modulecode)
        {
            var categories = new List<HCF.BDO.Category>();
            var client = new HolyNameTmsService.BurkeCategoryClient();
            var lists = client.GetBurkeCodesCategory(modulecode, 1, false);
            foreach (var list in lists)
            {
                var category = new HCF.BDO.Category
                {
                    Code = list.codeField,
                    Name = list.descriptionField,
                    Description = list.descriptionField,
                    IsActive = true
                };
                categories.Add(category);
            }
            return categories;
        }

    }
}