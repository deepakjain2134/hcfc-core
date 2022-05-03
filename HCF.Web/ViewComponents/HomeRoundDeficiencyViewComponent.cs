using System.Threading.Tasks;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class HomeRoundDeficiencyViewComponent:ViewComponent
    {
        private readonly IWorkOrderService _workorderService;
        public HomeRoundDeficiencyViewComponent(IWorkOrderService workorderService)
        {
            _workorderService = workorderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lists = _workorderService.GetRoundWorkOrders();
            return await Task.FromResult(View(lists));
        }
    }
}
