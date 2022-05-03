using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class SearchSearchResultsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            SearchFilter searchLists = new SearchFilter();
            return await Task.FromResult(View(searchLists));
        }
    }
}
