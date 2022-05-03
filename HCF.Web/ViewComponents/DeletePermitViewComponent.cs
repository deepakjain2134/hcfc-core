using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class DeletePermitViewComponent: ViewComponent
    {
        public DeletePermitViewComponent()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {            
            return await Task.FromResult(View());
        }
    }
}
