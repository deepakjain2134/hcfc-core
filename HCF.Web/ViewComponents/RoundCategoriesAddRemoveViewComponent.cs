using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HCF.BAL;


namespace HCF.Web.ViewComponents
{
    public class RoundCategoriesAddRemoveViewComponent : ViewComponent
    {
        private readonly IRoundInspectionsService _roundInspectionService;
        public RoundCategoriesAddRemoveViewComponent(IRoundInspectionsService roundInspectionService)
        {
            _roundInspectionService = roundInspectionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int floorId, int troundId, int userId)
        {
            if (userId == 0)
                userId = Base.UserSession.CurrentUser.UserId;

            ViewModels.Round.RoundViewModel model = new ViewModels.Round.RoundViewModel();
            
            model.UserFloorRoundCategory = _roundInspectionService.UserFloorRoundCategory(userId, floorId, troundId);
            model.FloorId = floorId;
            model.TRoundId = troundId;

            return await Task.FromResult(View(model));
        }
    }
}
