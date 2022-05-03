using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class HomeUserRecordsViewComponent :ViewComponent
    {
        private readonly IUserService _userService;
        public HomeUserRecordsViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userData = _userService.GetUserRecords(UserSession.CurrentUser.UserId);
            return await Task.FromResult(View(userData));
        }
    }
}
