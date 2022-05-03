using HCF.BDO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface INewsRepository
    {
        bool AddNews(News newNews);
        bool DeleteNews(int newsId);
        bool UpdateNews(News news);
        Task<List<News>> GetAllNews();
        Task<News> GetNewsById(int newsId);
        Task<List<News>> GetNewsByIds(int[] newsIds);
       
    }
}