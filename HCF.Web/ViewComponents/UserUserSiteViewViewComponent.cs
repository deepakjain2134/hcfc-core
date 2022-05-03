using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.Web.ViewModels;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using HCF.BAL.Interfaces.Services;

namespace HCF.Web.ViewComponents
{
    public class UserUserSiteViewViewComponent :ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserSiteService _userSiteService;
        public UserUserSiteViewViewComponent(ISiteService siteService, IUserSiteService userSiteService)
        {
            _userSiteService = userSiteService;
            _siteService = siteService;

        }
        public async Task<IViewComponentResult> InvokeAsync(int? userId, int? vendorId)
        {
            var userSiteViewModel = new UserSiteViewModel
            {
                Sites = _siteService.GetSites().Where(x => x.IsActive).ToList(),
                UserId = userId,
                VendorId = vendorId
            };
            if (userSiteViewModel.UserId.HasValue && userSiteViewModel.UserId.Value > 0)
                userSiteViewModel.SiteMapping = _userSiteService.GetUserSiteMappings(userSiteViewModel.UserId.Value).ToList();

            if (userSiteViewModel.VendorId.HasValue && userSiteViewModel.VendorId.Value > 0)
                userSiteViewModel.SiteMapping = _userSiteService.GetVendorSiteMappings(userSiteViewModel.VendorId.Value).ToList();

            return await Task.FromResult(View(userSiteViewModel));
        }
    }
}
