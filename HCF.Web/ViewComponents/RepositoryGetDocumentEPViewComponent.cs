using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class RepositoryGetDocumentEPViewComponent : ViewComponent
    {
        private readonly ITransactionService _transactionService;
        public RepositoryGetDocumentEPViewComponent(ITransactionService transactionService)
        {
            _transactionService = transactionService;

        }
        public async Task<IViewComponentResult> InvokeAsync(string mode, int selectedUser, int? epdetailId, int? dueWitndays, int? inprogress, int pastDueordef = 0)
        {
            ViewBag.PolicyView = mode;
            var docs = _transactionService.GetPolicyBinders(UserSession.CurrentUser.UserId, selectedUser, epdetailId);
            if (epdetailId.HasValue && epdetailId > 0)
                docs = docs.Where(x => x.EPDocument.Any(y => y.EPDetailId == epdetailId.Value)).ToList();
            ViewBag.DueDays = dueWitndays;
            if (dueWitndays.HasValue && dueWitndays.Value > 0)
            {
                docs = docs.Where(x => x.DueWithInDays > 0 && x.DueWithInDays <= Convert.ToInt32(dueWitndays.Value)).ToList();
            }
            if (inprogress.HasValue && inprogress.Value > -1)
            {
                docs = docs.Where(x => x.DocStatus == Convert.ToInt32(inprogress.Value)).ToList();
            }
            if (pastDueordef > 0)
            {
                docs = docs.Where(x => x.DocStatus != 1 && x.DocStatus != 2).ToList();
            }
            return await Task.FromResult(View(docs));
        }
    }
}
