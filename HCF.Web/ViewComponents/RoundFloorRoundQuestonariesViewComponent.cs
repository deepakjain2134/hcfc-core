using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class RoundFloorRoundQuestonariesViewComponent : ViewComponent
    {
        private static IRoundsService _roundService;
        public RoundFloorRoundQuestonariesViewComponent(IRoundsService roundService)
        {
            _roundService = roundService;

        }
        public async Task<IViewComponentResult> InvokeAsync(int rId, int floorId,int seq)
        {
            var rounds = _roundService.GetFloorRoundsQuestionnaires(floorId, rId);
            ViewBag.Seq = seq;
            return await Task.FromResult(View(rounds));
        }
    }
}
