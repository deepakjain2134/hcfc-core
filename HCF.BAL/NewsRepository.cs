//using HCF.BDO;
//using System.Collections.Generic;
//using System.Linq;

//namespace HCF.BAL
//{
//    public class NewsRepository
//    {
//        // private readonly INewsRepository _newsRepository;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newNews"></param>
//        /// <returns></returns>
//        public static bool AddNews(News newNews)
//        {            //return _newsRepository.Add(newNews);
//            return DAL.NewsRepository.AddNews(newNews);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="news"></param>
//        /// <returns></returns>
//        public static bool UpdateNews(News news)
//        {
//            return DAL.NewsRepository.UpdateNews(news);
//        }

//        public static bool DeleteNews(int newsId)
//        {
//            return DAL.NewsRepository.DeleteNews(newsId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newsId"></param>
//        /// <returns></returns>
//        public static News GetNews(int newsId)
//        {
//            return DAL.NewsRepository.GetNewsById(newsId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public static List<News> GetAllNews()
//        {
//            return DAL.NewsRepository.GetAllNews();
//        }


//        public static IEnumerable<KeyValuePair<string, string>> NewsTitles()
//        {
//            var newsData = GetAllNews().Where(x => x.IsExpired == false && x.IsReleaseNotes && x.Published).ToList();
//            var news = new List<KeyValuePair<string, string>>();
//            if (newsData.Count > 0)
//            {
//                foreach (var item in newsData)
//                {
//                    var newsItem = new KeyValuePair<string, string>(item.Id.ToString(), item.Title);
//                    news.Add(newsItem);
//                }
//            }
//            return news;
//        }
//    }
//}
