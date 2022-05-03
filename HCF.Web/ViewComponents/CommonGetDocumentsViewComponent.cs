using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonGetDocumentsViewComponent : ViewComponent
    {
        private readonly IDocumentsService _documentsService;
        public CommonGetDocumentsViewComponent(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var docs = _documentsService.GetInboxMails(UserSession.CurrentOrg.ClientNo);
            return await Task.FromResult(View(docs));
        }
    }
}
