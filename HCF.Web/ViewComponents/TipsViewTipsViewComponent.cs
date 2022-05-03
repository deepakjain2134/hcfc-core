using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
namespace HCF.Web.ViewComponents
{
    public class TipsViewTipsViewComponent : ViewComponent
    {
        private readonly ITipsService _tipsService;
        public TipsViewTipsViewComponent(ITipsService tipsService)
        {
            _tipsService = tipsService;

        }
        public async Task<IViewComponentResult> InvokeAsync(string pageUrl)
        {
            var tips = _tipsService.GetTipsByClientNo(UserSession.CurrentOrg.ClientNo, pageUrl);
            return await Task.FromResult(View());
        }
    }
}
