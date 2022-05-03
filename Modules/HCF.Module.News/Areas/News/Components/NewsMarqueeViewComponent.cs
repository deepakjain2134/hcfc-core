using HCF.BAL;
using HCF.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HCF.Module.News.Areas.News.Components
{
    public class NewsMarqueeViewComponent : ViewComponent
    {

        private readonly INewsService _newsService;  
        public NewsMarqueeViewComponent(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _newsService.NewsTitles();
            return await Task.FromResult(View(this.GetViewPath(), data));
        }
    }
}
