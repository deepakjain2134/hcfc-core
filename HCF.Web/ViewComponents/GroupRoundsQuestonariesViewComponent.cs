using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class GroupRoundsQuestonariesViewComponent : ViewComponent
    {
        private static IRoundsService _roundService;
        public GroupRoundsQuestonariesViewComponent(IRoundsService roundService)
        {
            _roundService = roundService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int floorId, int rId,string seq,int troundId)
        {
            ViewBag.sequennce = seq;
            var rounds = _roundService.GetRoundsQuestionnaires(floorId, rId, UserSession.CurrentUser.UserId);
            return await Task.FromResult(View(rounds));
        }
    }
}