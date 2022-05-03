using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Module.Core.Areas.Core.Controllers;
using HCF.Module.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Module.News.Areas.News.Controllers
{
    [Authorize]
    [Area("News")]
    public class NewsItemController : BaseController
    {
        #region Ctor
        private readonly INewsService _newsService;     
        private readonly IWorkContext _workContext;       
        private readonly INavigationSession _uISession;
        

        public NewsItemController(INewsService newsService, IWorkContext workContext, INavigationSession uISession)
        {
            _workContext = workContext;          
            _newsService = newsService;           
            _uISession = uISession;
        }

        #endregion

        [HttpGet("news")]
        public async Task<ActionResult> News()
        {
           
            var currentUser = await _workContext.GetCurrentUser();
            _uISession.AddCurrentPage("Repository_News", 0, "News");
            var list = await _newsService.GetAllNews();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].Description = RemoveHTMLTags(list[i].Description);
            } 
            return View(list);
        }

        [HttpGet("create/news")]
        public async Task<ActionResult> AddNews(int? newsId)
        {
            var news = new BDO.News();
            _uISession.AddCurrentPage("Repository_AddNews", 0, (newsId.HasValue) ? "Edit News" : "Add News");
            if (newsId.HasValue)
                news = await _newsService.GetNews(newsId.Value);

            return View(news);
        }

        [HttpPost("create/news")]
        public async Task<ActionResult> AddNews(BDO.News news)
        {
            var success = false;
            var currentUser = await _workContext.GetCurrentUser();
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(news.Description))
                {
                    news.IsReleaseNotes = true;
                    news.CreatedBy = currentUser.UserId;
                    news.Title = ReplaceSpacesWithOneSpace(news.Title);
                    if (news.Id > 0)
                        success = _newsService.UpdateNews(news);
                    else
                        success = _newsService.AddNews(news);

                    if (success)
                        SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                    else
                        ErrorMessage = "This title already exists. Use a different title to proceed.";
                    return RedirectToAction("News");
                }
                else
                    ErrorMessage = "Please enter description";
            }
            return View(news);
        }

        [HttpGet("latest-releases/{type}/{newsId}")]
        public async Task<ActionResult> NewsDescription(int? newsId, string type = "0")
        {

            var newsList = await _newsService.GetAllNews();
            ViewBag.CurrentNews = newsList.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            if (newsId.HasValue)
            {
                var news = newsList.FirstOrDefault(x => x.Id == newsId.Value);
                if (news != null)
                    //newsList = newsList.Where(x => x.IsReleaseNotes == news.IsReleaseNotes).ToList();
                    newsList = newsList.Where(x => (type == "0" ? x.Published : x.IsReleaseNotes)).ToList();
                ViewBag.CurrentNews = news;
            }
            if (!string.IsNullOrEmpty(type) && !newsId.HasValue)
            {
                newsList = newsList.Where(x => (type == "0" ? x.Published : x.IsReleaseNotes)).ToList();
                if (newsList.Count() == 0)
                {
                    var newItem = new BDO.News
                    {
                        Title = "",
                        Description = "No Record Found"
                    };
                    ViewBag.CurrentNews = newItem;
                }
                else
                {
                    ViewBag.CurrentNews = newsList.FirstOrDefault();
                }
            }
            return View(newsList);
        }

        public async Task<ActionResult> GetReleaseNotes(int id)
        {
            var news = await _newsService.GetNews(id);
            return PartialView("_ReleaseNotesView", news);
        }

        #region Private methods

        private string ReplaceSpacesWithOneSpace(string text)
        {
            var regex = new Regex(@"\s+");
            var str = text.Trim();
            str = regex.Replace(str, " ");
            return str;
        }

        private string RemoveHTMLTags(string text)
        {
            try
            {
                bool isHtml = true;
                //var regex = new Regex("(</?([^>/]*)/?>|&nbsp;)");
                var regex = new Regex(@"(<{1}([/]?)(\w*\W*)([/]?)>{1}|&nbsp;)");
                text = regex.Replace(text.Trim(), "");
                while (isHtml)
                {
                    if (text.Contains("<"))
                    {
                        var txt = text.Substring(text.IndexOf("<"), text.IndexOf(">") - text.IndexOf("<") + 1);
                        text = text.Replace(txt, "");
                    }
                    else
                    {
                        isHtml = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
            return text;

        }

        #endregion
    }
}