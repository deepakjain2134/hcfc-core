using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public class NewsRepository : INewsRepository
    {
        #region ctor 
        private readonly ISqlHelper _sqlHelper;
       // private readonly IRepository<News> _repository;

        public NewsRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
            //_repository = repository;
        }


        #endregion

        public bool UpdateNews(News news)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Update_News;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", news.Id);
                command.Parameters.AddWithValue("@Title", news.Title);
                command.Parameters.AddWithValue("@Short", news.Short);
                command.Parameters.AddWithValue("@Description", news.Description);
                command.Parameters.AddWithValue("@Published", news.Published);
                command.Parameters.AddWithValue("@StartDate", news.StartDate);
                command.Parameters.AddWithValue("@EndDate", news.EndDate);
                command.Parameters.AddWithValue("@IsReleaseNotes", news.IsReleaseNotes);
                command.Parameters.AddWithValue("@CreatedBy", news.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return (newId > 0) ? true : false;
        }

        public async Task<List<News>> GetAllNews()
        {
            List<News> objAudit = new List<News>();
            const string sql = StoredProcedures.Get_News;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = await _sqlHelper.GetCommonDataTableAsync(command);
                if (dt != null)
                    objAudit = _sqlHelper.ConvertDataTable<News>(dt);
            }
            return objAudit;
        }

        public async Task<News> GetNewsById(int newsId)
        {
            var list = await GetAllNews();
            return list.Where(x => x.Id == newsId).FirstOrDefault();
        }

        public bool DeleteNews(int newsId)
        {
            const string sql = Utility.StoredProcedures.Delete_News;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", newsId);
                return _sqlHelper.CommonExecuteNonQuery(command);
            }
        }

        public async Task<List<News>> GetNewsByIds(int[] newsIds)
        {
            var list = await GetAllNews();
            return list.Where(news => newsIds.Contains(news.Id)).ToList();
        }

        public bool AddNews(News newNews)
        {
            int newId;
            const string sql = StoredProcedures.Insert_News;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title", newNews.Title);
                command.Parameters.AddWithValue("@StartDate", newNews.StartDate);
                command.Parameters.AddWithValue("@EndDate", newNews.EndDate);
                command.Parameters.AddWithValue("@Description", newNews.Description);
                command.Parameters.AddWithValue("@CreatedBy", newNews.CreatedBy);
                command.Parameters.AddWithValue("@Published", newNews.Published);
                command.Parameters.AddWithValue("@IsReleaseNotes", newNews.IsReleaseNotes);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            return (newId > 0) ? true : false;

        }
    }
}