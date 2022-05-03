using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface INewsService
    {
        bool AddNews(News newNews);
        bool DeleteNews(int newsId);
        Task<List<News>> GetAllNews();
        Task<News> GetNews(int newsId);
        Task<List<KeyValuePair<string, string>>> NewsTitles();
        bool UpdateNews(News news);
    }
}