using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class DocumentSpaceStatusBarViewComponent : ViewComponent
    {
        private readonly IDocumentsService _documentsService;
        public DocumentSpaceStatusBarViewComponent(IDocumentsService documentsService)
        {
            _documentsService = documentsService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var documentStatus =await _documentsService.GetDocumentSpaceStatus(Base.UserSession.CurrentOrg.Orgkey);
            return await Task.FromResult(View(documentStatus));
        }
    }
}
