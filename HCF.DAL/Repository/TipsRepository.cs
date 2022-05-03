using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace HCF.DAL
{
    public  class TipsRepository:ITipsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public TipsRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public  List<Tips> GetTipsByClientNo(int clientNo,string routeUrl)
        {
            string sql = StoredProcedures.Get_Tips;
            List<Tips> tipsList = new List<Tips>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientNo", clientNo);
                command.Parameters.AddWithValue("@routeUrl", routeUrl);
                var dt = _sqlHelper.GetCommonDataTable(command);
                tipsList = _sqlHelper.ConvertDataTable<Tips>(dt);
            }
            return tipsList;
        }

        public  bool ApproveTip(int tipId, int approveStatus)
        {
            bool status = true;

            string sql = StoredProcedures.Update_ApprovalOfTips; //"Update Tips set IsApproved=@ApproveStatus where TipId=@tipId";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tipId", tipId);
                command.Parameters.AddWithValue("@ApproveStatus", approveStatus);
                _sqlHelper.CommonExecuteNonQuery(command);
            }
            return status;
        }

        public  List<Tips> GetAllTipsByClientNo(int clientNo)
        {
            var sql = StoredProcedures.Get_All_Tips;
            var tipsList = new List<Tips>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientNo", clientNo);
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var tip = new Tips
                        {
                            TipId = Convert.ToInt32(row["TipId"].ToString()),
                            Title = row["Title"].ToString(),
                            Description = row["Description"].ToString(),
                            RouteUrl = row["RouteUrl"].ToString(),
                            RouteName = row["RouteName"].ToString(),
                            ContributorName = row["ContributorName"].ToString(),
                            ContributorOrg = row["ContributorOrg"].ToString(),
                            ContributorPosition = row["ContributorPosition"].ToString(),
                            ShowContributorName = Convert.ToBoolean(row["ShowContributorName"].ToString()),
                            ShowContributorOrg = Convert.ToBoolean(row["ShowContributorOrg"].ToString()),
                            ShowContributorPosition = Convert.ToBoolean(row["ShowContributorPosition"].ToString()),
                            IsApproved = Convert.ToInt32(row["IsApproved"].ToString()),
                            IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                            IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString()),
                            TipType = (HCF.BDO.Enums.TipTypes)Convert.ToInt32(row["TipType"].ToString()), // Convert.ToInt32(row["TipType"].ToString());
                            ClientNo = Convert.ToInt32(row["TipId"].ToString()),
                            Services = new Menus()
                        };
                        if (!string.IsNullOrEmpty(row["Id"].ToString()))
                        {
                            tip.Services.Id = Convert.ToInt32(row["Id"].ToString());
                            tip.Services.Alias = row["Alias"].ToString();
                            tip.Services.Name = row["Name"].ToString();
                            tip.Services.PageUrl = row["PageUrl"].ToString();
                            tip.Services.Seq = Convert.ToInt32(row["Seq"].ToString());
                            tip.Services.Type = Convert.ToInt32(row["Type"].ToString());
                            tip.Services.Status = Convert.ToBoolean(row["IsActive"].ToString());
                            tip.Services.ParentId = Convert.ToInt32(row["ParentId"].ToString());
                            tip.Services.ChildCount = !string.IsNullOrEmpty(row["ChildCount"].ToString())?Convert.ToInt32(row["ChildCount"].ToString()):0;
                        }
                        tipsList.Add(tip);
                    }
                }
            }
            return tipsList;
        }

        public  bool InsertOrUpdateTips(Tips tip)
        {
            string sql = StoredProcedures.Insert_Update_Tip;
            List<Tips> tipsList = new List<Tips>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tipId", tip.TipId);
                command.Parameters.AddWithValue("@title", tip.Title);
                command.Parameters.AddWithValue("@description", tip.Description);
                command.Parameters.AddWithValue("@source", tip.Source);
                command.Parameters.AddWithValue("@routeUrl", tip.RouteUrlPath);
                command.Parameters.AddWithValue("@createdBy", tip.CreatedBy);
                command.Parameters.AddWithValue("@clientNo", tip.ClientNo);
                command.Parameters.AddWithValue("@contributorName", tip.ContributorName);
                command.Parameters.AddWithValue("@contributorOrg", tip.ContributorOrg);
                command.Parameters.AddWithValue("@contributorPosition", tip.ContributorPosition);
                command.Parameters.AddWithValue("@showContributorName", tip.ShowContributorName);
                command.Parameters.AddWithValue("@showContributorOrg", tip.ShowContributorOrg);
                command.Parameters.AddWithValue("@showContributorPosition", tip.ShowContributorPosition);
                command.Parameters.AddWithValue("@isApproved", tip.IsApproved);
                command.Parameters.AddWithValue("@routename", tip.RouteName);
                command.Parameters.AddWithValue("@type", tip.TipType);
                command.Parameters.AddWithValue("@updatedBy", tip.UpdatedBy);
                command.Parameters.AddWithValue("@isActive", tip.IsActive);
                var id = _sqlHelper.CommonExecuteNonQuery(command);
                return id;
            }
        }
    }
}
