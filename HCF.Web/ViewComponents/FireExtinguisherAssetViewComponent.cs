using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class FireExtinguisherAssetViewComponent :ViewComponent
    {
        private readonly IAssetTypeService _assetTypeService;
        public FireExtinguisherAssetViewComponent(IAssetTypeService assetTypeService)
        {
            _assetTypeService = assetTypeService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string mode)
        {
            ViewBag.PageMode = mode;           
            var types = _assetTypeService.GetInternalAssetsByType(UserSession.CurrentUser.UserId);
            return await Task.FromResult(View(types));
        }
    }
}
