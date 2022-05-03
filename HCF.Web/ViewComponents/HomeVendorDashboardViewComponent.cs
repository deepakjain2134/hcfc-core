using HCF.BAL.Interfaces.Services;
using HCF.Web.Base;
using HCF.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class HomeVendorDashboardViewComponent : ViewComponent
    {
        private readonly IVendorService _vendorService;
        public HomeVendorDashboardViewComponent(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string data)
        {
            int vendorId = UserSession.CurrentUser.VendorId.Value;
            var viewModel = new VendorDashboardViewModel
            {
                VendorRecordsCount = _vendorService.GetVendorDashboardData(vendorId, UserSession.CurrentUser.UserId),
                vendorRes = _vendorService.GetVendorResourcesDashboard(vendorId)
            };
            return await Task.FromResult(View(viewModel));
        }
    }
}
