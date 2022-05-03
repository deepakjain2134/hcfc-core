using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class GoalDocumentDashBoardViewViewComponent : ViewComponent
    {
        private readonly ITransactionService _transactionService;
        public GoalDocumentDashBoardViewViewComponent(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var docs = _transactionService.GetPolicyBinders(
                UserSession.CurrentUser.UserId,
               0, null);
            return await Task.FromResult(View(docs));
        }
    }
}
