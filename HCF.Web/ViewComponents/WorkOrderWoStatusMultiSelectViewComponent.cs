using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;


namespace HCF.Web.ViewComponents
{
    public class WorkOrderWoStatusMultiSelectViewComponent : ViewComponent
    {
        private readonly IOrganizationService _organizationService;
        public WorkOrderWoStatusMultiSelectViewComponent(IOrganizationService organizationService)
        {
            _organizationService = organizationService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var woStatuses = new List<HCF.BDO.WoStatus>();
            var orgData = _organizationService.GetWOMastersData();
            var woStatuses = orgData.Result.WoStatus;
            return await Task.FromResult(View(woStatuses));
        }
    }
}
