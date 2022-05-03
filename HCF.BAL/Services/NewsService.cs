using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public  class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
           // _newsRepository = new DAL.NewsRepository();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newNews"></param>
        /// <returns></returns>
        public  bool AddNews(News newNews)
        {            //return _newsRepository.Add(newNews);
            return _newsRepository.AddNews(newNews);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public  bool UpdateNews(News news)
        {
            return _newsRepository.UpdateNews(news);
        }

        public  bool DeleteNews(int newsId)
        {
            return _newsRepository.DeleteNews(newsId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public async Task<News> GetNews(int newsId)
        {
            return await _newsRepository.GetNewsById(newsId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<News>> GetAllNews()
        {
            return await _newsRepository.GetAllNews();
        }
        public async Task<List<KeyValuePair<string, string>>> NewsTitles()
        {
            var newsData = await GetAllNews();
            var list= newsData.ToList().Where(x => x.IsExpired == false && x.IsReleaseNotes && x.Published).ToList(); ;
            var news = new List<KeyValuePair<string, string>>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var newsItem = new KeyValuePair<string, string>(item.Id.ToString(), item.Title);
                    news.Add(newsItem);
                }
            }
            return news;
        }
    }
}
