using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HCF.BAL;

namespace HCF.Web.ViewComponents
{
    public class RoundGetRoundByDateViewComponent : ViewComponent
    {
        public static IRoundInspectionsService _roundInspectionsService;
        public RoundGetRoundByDateViewComponent(IRoundInspectionsService roundInspectionsService)
        {
            _roundInspectionsService = roundInspectionsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string start, int? locationGroupId, int? userId)
        {
            var roundDate = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : DateTime.Now.Date;
            //userId = userId.HasValue && userId.Value > 0 ? userId : HCF.Web.Base.UserSession.CurrentUser.UserId;
            var roundGroups = _roundInspectionsService.GetRoundByDate(roundDate, locationGroupId, userId, false);
            return await Task.FromResult(View(roundGroups));
        }
    }
}
