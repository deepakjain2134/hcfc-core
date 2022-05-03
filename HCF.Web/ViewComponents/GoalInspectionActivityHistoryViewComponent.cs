using HCF.BAL;
using HCF.BDO;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class GoalInspectionActivityHistoryViewComponent :ViewComponent
    {
        private readonly IEpService _epService;
        private readonly IInsActivityService _inspectionActivityService;

        public GoalInspectionActivityHistoryViewComponent(IEpService epService,
            IInsActivityService inspectionActivityService)
        {
            _inspectionActivityService = inspectionActivityService;
            _epService = epService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? epId, int? inspectionId)
        {
           var activity = new List<TInspectionActivity>();
            if (epId.HasValue && inspectionId.HasValue)
            {
                ViewBag.EPDetailId = epId.Value;
                activity = _inspectionActivityService.GetInspectionActivityDetails(inspectionId.Value);
            }
            var model = new InspectionActivityHistoryViewModel
            {
                activity = activity,
                EPDetails = _epService.GetEpDescription(epId)
            };
            return await Task.FromResult(View(model));
        }
    }
}
