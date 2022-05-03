using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class CommonRepository : ICommonRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public CommonRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region Org 

        public  Organization GetMasterOrg(Guid orgKey)
        {
            List<Organization> orgs = new();
            const string sql = StoredProcedures.Hcf_GetOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Orgkey", orgKey);
                var ds = _sqlHelper.GetCommonDataSet(cmd);
                orgs = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
            }

            //foreach (Organization newOrg in orgs.Where(x => x.ParentOrgKey == null))
            //    newOrg.ChildOrgs = orgs.Where(x => x.ParentOrgKey == newOrg.Orgkey).ToList();

            return orgs.FirstOrDefault(x => x.Orgkey == orgKey);
        }        

        #endregion

        public  List<Audit> GetAuditConfiguration()
        {
            List<Audit> objAudit = new();
            const string sql = StoredProcedures.Get_AuditConfiguration;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                objAudit = _sqlHelper.ConvertDataTable<Audit>(ds.Tables[0]);
            }
            return objAudit;
        }

        public  List<T> GetTableData<T>(string sql)
        {
            List<T> data = new();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    data = _sqlHelper.ConvertDataTable<T>(ds.Tables[0]);
            }
            return data;
        }
     
        public List<T> GetTableDataCommon<T>(string sql)
        {
            List<T> data = new();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                    data = _sqlHelper.ConvertDataTable<T>(ds.Tables[0]);
            }
            return data;
        }

        #region        

        public List<TFiles> GetRecentFiles(int userId)
        {
            List<TFiles> lists = new();
            const string sql = StoredProcedures.Get_RecentFiles;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var tfiles = new TFiles();
                        if (!string.IsNullOrEmpty(item["FileSize"].ToString()))
                            tfiles.FileSize =Convert.ToInt64(item["FileSize"].ToString());
                        if (!string.IsNullOrEmpty(item["UploadedBy"].ToString()))
                            tfiles.UploadedBy = Convert.ToInt32(item["UploadedBy"].ToString());
                        if (!string.IsNullOrEmpty(item["TFileId"].ToString()))
                            tfiles.TFileId = Convert.ToInt32(item["TFileId"].ToString());
                        tfiles.FileName = item["FileName"].ToString();
                        tfiles.FilePath = item["FilePath"].ToString();
                        tfiles.UploadedDate = Convert.ToDateTime(item["UploadedDate"].ToString());
                      
                        var user = new UserProfile
                        {
                            FirstName = item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString(),
                            UserId = Convert.ToInt32(item["UserId"].ToString())
                        };
                        tfiles.UploadedByUser = user;
                        lists.Add(tfiles);
                    }
                }
            }
            return lists;
        }

        public List<TFiles> FindUploadeFiles(string key,string fileName,int userId)
        {
            List<TFiles> lists = new();
            const string sql = StoredProcedures.Get_FindUploadedFiles;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Md5Digest", key);
                cmd.Parameters.AddWithValue("@fileNames", fileName); 
                cmd.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(cmd);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var tfiles = new TFiles();
                        if (!string.IsNullOrEmpty(item["FileSize"].ToString()))
                            tfiles.FileSize = Convert.ToInt64(item["FileSize"].ToString());                        
                        if (!string.IsNullOrEmpty(item["TFileId"].ToString()))
                            tfiles.TFileId = Convert.ToInt32(item["TFileId"].ToString());
                        tfiles.FileName = item["FileName"].ToString();
                        tfiles.FilePath = item["FilePath"].ToString();
                        tfiles.MD5Digest = item["Md5Digest"].ToString();
                        tfiles.UploadedDate = Convert.ToDateTime(item["UploadedDate"].ToString());

                        //var user = new UserProfile
                        //{
                        //    FirstName = item["FirstName"].ToString(),
                        //    LastName = item["LastName"].ToString(),
                        //    UserId = Convert.ToInt32(item["UserId"].ToString())
                        //};
                        //tfiles.UploadedByUser = user;
                        lists.Add(tfiles);
                    }
                }
            }
            return lists;
        }


        #endregion      

        #region Drawingspathway 

        public  List<DrawingpathwayFiles> GetUploadedDrawings(string files)
        {
            List<DrawingpathwayFiles> tfiles = new();
            foreach (var item in files.Split(','))
            {
                string fileId = Convert.ToString(item.Split('_')[0]); 
                var tfile = GetUploadedDrawing(fileId).FirstOrDefault();
                if (tfile != null)
                    tfiles.Add(tfile);
            }
            return tfiles;
        }

        private IEnumerable<DrawingpathwayFiles> GetUploadedDrawing(string floorPlanId)
        {
            List<DrawingpathwayFiles> DrawingpathwayFiles = new();
            const string sql = StoredProcedures.GetUploadedDrawings;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@floorPlanId", floorPlanId);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                    DrawingpathwayFiles = _sqlHelper.ConvertDataTable<DrawingpathwayFiles>(dt.Tables[0]);
            }
            return DrawingpathwayFiles;

        }

        #endregion
    }
}
