using HCF.BAL.Interfaces.Services;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class AssetsAssetViewComponent : ViewComponent
    {
        private readonly IAssetsService _assetService;
        public AssetsAssetViewComponent(IAssetsService assetService)
        {
            _assetService = assetService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserSession.CurrentUser.UserId;
            var types = _assetService.GetAssetsByType(userId, 0);
            var filterType = types.Where(y => y.Assets.Any(x => x.Count > 0 && x.StandardEps.Any())).ToList();
            return await Task.FromResult(View(filterType));
        }
    }
}
