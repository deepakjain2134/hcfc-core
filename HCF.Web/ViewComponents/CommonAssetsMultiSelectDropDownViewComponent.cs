using HCF.BAL.Interfaces.Services;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonAssetsMultiSelectDropDownViewComponent: ViewComponent
    {
        private readonly IAssetsService _assetService;
        public CommonAssetsMultiSelectDropDownViewComponent(IAssetsService assetService)
        {
            _assetService = assetService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var assets = _assetService.GetAssets(UserSession.CurrentUser.UserId);
            return await Task.FromResult(View(assets));
        }
    }
}
