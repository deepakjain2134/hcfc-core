using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;


namespace HCF.DAL
{
    public  class SearchRepository : ISearchRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorAssetRepository _floorAssetRepository;
        public SearchRepository(ISqlHelper sqlHelper, IFloorAssetRepository floorAssetRepository)
        {
            _sqlHelper = sqlHelper;
            _floorAssetRepository = floorAssetRepository;
        }


        #region Search Filter

        public  int AddSearchFilter(SearchFilter objSearchFilter)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_SearchFilter;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchType", objSearchFilter.SearchType);
                command.Parameters.AddWithValue("@FilterName", objSearchFilter.FilterName);
                command.Parameters.AddWithValue("@OwnerID", objSearchFilter.OwnerID);
                command.Parameters.AddWithValue("@DeviceNo", objSearchFilter.DeviceNo);
                command.Parameters.AddWithValue("@SerialNo", objSearchFilter.SerialNo);
                command.Parameters.AddWithValue("@BuildingId", objSearchFilter.BuildingId);
                command.Parameters.AddWithValue("@AssetId", objSearchFilter.AssetId);
                command.Parameters.AddWithValue("@StdescId", objSearchFilter.StdescId);
                command.Parameters.AddWithValue("@EpDetailId", objSearchFilter.EpDetailId);
                command.Parameters.AddWithValue("@ScoreId", objSearchFilter.ScoreId);             
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  SearchFilter GetSearchFilter(Guid filterId )
        {
            SearchFilter searchfilter = new SearchFilter();
            const string sql = Utility.StoredProcedures.Get_SearchFilter;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@OwnerID", OwnerID);
                command.Parameters.AddWithValue("@filterId", filterId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    searchfilter = _sqlHelper.ConvertDataTable<SearchFilter>(dt).FirstOrDefault();
            }
            return searchfilter;
        }

        public  List<SearchFilter> GetSearchFilter(int userId)
        {
            List<SearchFilter> searchfilter = new List<SearchFilter>();
            const string sql = Utility.StoredProcedures.Get_SearchFilter;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OwnerID", userId);
                DataTable dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    searchfilter = _sqlHelper.ConvertDataTable<SearchFilter>(dt);
            }
            return searchfilter;
        }
        #endregion


        #region Search Result

        public  SearchFilter GetSearchResult(SearchFilter objSearchFilter)
        {
            const string sql = Utility.StoredProcedures.Get_SearchResult;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchType", objSearchFilter.SearchType);
                command.Parameters.AddWithValue("@DeviceNo", objSearchFilter.DeviceNo);
                command.Parameters.AddWithValue("@SerialNo", objSearchFilter.SerialNo);
                command.Parameters.AddWithValue("@BuildingId", objSearchFilter.BuildingId);
                command.Parameters.AddWithValue("@StdescId", objSearchFilter.StdescId);
                command.Parameters.AddWithValue("@EpDetailId", objSearchFilter.EpDetailId);
                command.Parameters.AddWithValue("@ScoreId", objSearchFilter.ScoreId);
                command.Parameters.AddWithValue("@AssetId", objSearchFilter.AssetId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    switch (objSearchFilter.SearchType)
                    {
                        case 2:
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                List<TFloorAssets> tfloorAssets = new List<TFloorAssets>();
                                tfloorAssets = _floorAssetRepository.BindFloorAssets(ds.Tables[0]);
                                objSearchFilter.TFloorAssets = tfloorAssets;//_sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]);
                            }

                            break;
                        }
                        case 1:
                        {
                            var epDetails = new List<EPDetails>();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                EPDetails ep = new EPDetails
                                {
                                    EPNumber = row["EPNumber"].ToString(),
                                    Description = row["Description"].ToString(),
                                    EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                                    StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                    Standard = new BDO.Standard
                                    {
                                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                        TJCStandard = row["TJCStandard"].ToString(),
                                        Category = new BDO.Category
                                        {
                                            CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                            Code = row["Code"].ToString()
                                        }
                                    },
                                    Scores = new BDO.Score
                                    {
                                        Name = row["ScoreName"].ToString()
                                    },
                                    IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString())
                                };
                                epDetails.Add(ep);
                            }
                            objSearchFilter.EPDetails = epDetails;
                            break;
                        }
                    }
            }
            return objSearchFilter;
        }
        #endregion
    }
}
