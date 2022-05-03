using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.Utility;
using HCF.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCF.Web.ViewComponents
{
    public class UserUserPermitViewViewComponent : ViewComponent
    {
        public UserUserPermitViewViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(int? userId, int? vendorId, long[] selectedValues)
        {
            var listItems = new List<SelectListItem>();
            var lists = from BDO.Enums.PermitType e in Enum.GetValues(typeof(BDO.Enums.PermitType))
                        select new
                        {
                            Value = (int)e,
                            Text = e.GetDescription()
                        };

            foreach (var item in lists.OrderBy(x => x.Text))
            {

                var selItem = new SelectListItem
                {
                    Text = item.Text,
                    Value = Convert.ToString(item.Value)
                };
                listItems.Add(selItem);
            }

            var model = new UserPermitViewModel
            {
                PermitTypes = listItems,
                UserId = userId,
                VendorId = vendorId,
                SelectedPermitTypes = selectedValues
            };
            return await Task.FromResult(View(model));
        }
    }
}
