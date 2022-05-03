using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class PermitFSBPBuildingDDLViewComponent :ViewComponent
    {
        public PermitFSBPBuildingDDLViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(bool isMultiple, string selectdValue)
        {
            var selectPicker = new MultiSelectPicker { IsMultiple = isMultiple, SelectedValue = selectdValue };
            return await Task.FromResult(View(selectPicker));
        }
    }
}
