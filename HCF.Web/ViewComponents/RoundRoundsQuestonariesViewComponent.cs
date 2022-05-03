//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using HCF.BAL;
//using HCF.Web.Base;

//namespace HCF.Web.ViewComponents
//{
//    public class RoundRoundsQuestonariesViewComponent : ViewComponent
//    {
//        private static IRoundsService _roundService;
//        public RoundRoundsQuestonariesViewComponent(IRoundsService roundService)
//        {
//            _roundService = roundService;
//        }
//        public async Task<IViewComponentResult> InvokeAsync(int floorId, int rId)
//        {
//            var rounds = _roundService.GetRoundsQuestionnaires(floorId, rId, UserSession.CurrentUser.UserId);
//            return await Task.FromResult(View(rounds));
//        }
//    }
//}