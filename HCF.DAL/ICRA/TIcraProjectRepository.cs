using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public  class TIcraProjectRepository : ITIcraProjectRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ITFilesRepository _tFilesRepository;
        private readonly ICommonRepository _commonRepository;
        public TIcraProjectRepository(ISqlHelper sqlHelper, ITFilesRepository tFilesRepository, ICommonRepository commonRepository)
        {
            _sqlHelper = sqlHelper;
            _tFilesRepository = tFilesRepository;
            _commonRepository = commonRepository;
        }

        public async Task<List<TIcraProject>> GetAllTIcraProject()
        {
            var TIcraProjectList = new List<TIcraProject>();
            const string sql = StoredProcedures.Crud_TIcraProject;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectID", 0);
                command.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectNumber", 0);
                command.Parameters.AddWithValue("@Description", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectManager", 0);
                command.Parameters.AddWithValue("@Architect", 0);
                command.Parameters.AddWithValue("@Status",0);
                command.Parameters.AddWithValue("@CreatedBy", 0);
                command.Parameters.AddWithValue("@ProjectType", DBNull.Value);
                command.Parameters.AddWithValue("@TDrawingFields", DBNull.Value);
                command.Parameters.AddWithValue("@TFileIds", DBNull.Value);
                command.Parameters.AddWithValue("@Mode", 0); // 0=>Select,1=>Insert,2=>Update,3=>Delete
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                using (DataTable ds =await _sqlHelper.GetDataTableAsync(command))
                {
                    if (ds != null )
                    {
                        TIcraProjectList = _sqlHelper.ConvertDataTable<TIcraProject>(ds);
                        foreach (TIcraProject item in TIcraProjectList)
                        {
                            item.SubProject = TIcraProjectList.Where(x => x.ParentProjectId == item.ProjectId).ToList();
                            if (!string.IsNullOrEmpty(item.TFileIds))
                                item.Attachments = _tFilesRepository.GetTFiles(item.TFileIds);

                            if (!string.IsNullOrEmpty(item.TDrawingFields))
                                item.DrawingAttachments = _commonRepository.GetUploadedDrawings(item.TDrawingFields);
                        }
                    }
                }               
            }
            return await Task.FromResult(TIcraProjectList.ToList());
        }
        public  List<TIcraProject> GetAllActiveTIcraProject(bool? hideInActive = true)
        {
            List<TIcraProject> TIcraProjectList = new List<TIcraProject>();
            const string sql = StoredProcedures.Crud_TIcraProject;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectID", 0);
                command.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectNumber", 0);
                command.Parameters.AddWithValue("@Description", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectManager", 0);
                command.Parameters.AddWithValue("@Architect", 0);
                command.Parameters.AddWithValue("@Status", 0);
                command.Parameters.AddWithValue("@CreatedBy", 0);
                command.Parameters.AddWithValue("@ProjectType", DBNull.Value);
                command.Parameters.AddWithValue("@TDrawingFields", DBNull.Value);
                command.Parameters.AddWithValue("@TFileIds", DBNull.Value);
                command.Parameters.AddWithValue("@Mode", 0); // 0=>Select,1=>Insert,2=>Update,3=>Delete
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        TIcraProjectList = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                        if (hideInActive.Value)
                            TIcraProjectList = TIcraProjectList.Where(x => x.Status).ToList();
                    }

                }


                foreach (var item in TIcraProjectList)
                {
                    item.SubProject = TIcraProjectList.Where(x => x.ParentProjectId == item.ProjectId).ToList();
                }
            }
            return TIcraProjectList;
        }

        public  List<TIcraProject> GetCountAllActiveTIcraProject(int UserID ,int? vendorID, bool? HideInActive=true)
        {
            List<TIcraProject> TIcraProjectList = new List<TIcraProject>();
            const string sql = StoredProcedures.Crud_TIcraProject;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectID", 0); 
                command.Parameters.AddWithValue("@VendorID", vendorID);
                command.Parameters.AddWithValue("@Userid", UserID);
                command.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectNumber", 0);
                command.Parameters.AddWithValue("@Description", DBNull.Value);
                command.Parameters.AddWithValue("@ProjectManager", 0);
                command.Parameters.AddWithValue("@Architect", 0);
                command.Parameters.AddWithValue("@Status", 0);
                command.Parameters.AddWithValue("@CreatedBy", 0);
                command.Parameters.AddWithValue("@ProjectType", DBNull.Value);
                command.Parameters.AddWithValue("@TDrawingFields", DBNull.Value);
                command.Parameters.AddWithValue("@TFileIds", DBNull.Value);
                command.Parameters.AddWithValue("@Mode", 0); // 0=>Select,1=>Insert,2=>Update,3=>Delete
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        TIcraProjectList = _sqlHelper.ConvertDataTable<TIcraProject>(ds.Tables[0]);
                        if (HideInActive.Value)
                            TIcraProjectList = TIcraProjectList.Where(x => x.Status).ToList();
                    }
                }

                foreach (var item in TIcraProjectList)
                {
                    item.SubProject = TIcraProjectList.Where(x => x.ParentProjectId == item.ProjectId).ToList();
                }
            }
            return TIcraProjectList;
        }


        public  bool DeleteProjectFiles(int ProjectId, string fileIds,int type)
        {
            bool status;
            const string sql = StoredProcedures.Delete_ProjectFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectId", ProjectId);
                command.Parameters.AddWithValue("@fileIds", fileIds);
                command.Parameters.AddWithValue("@type", type);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  int CrudTIcraProject(TIcraProject item, int mode)
        {
            int newId;
            const string sql = StoredProcedures.Crud_TIcraProject;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProjectID", item.ProjectId);
                command.Parameters.AddWithValue("@ProjectName", item.ProjectName);
                command.Parameters.AddWithValue("@ProjectNumber", item.ProjectNumber);
                command.Parameters.AddWithValue("@ProjectLocation", item.ProjectLocation);
                command.Parameters.AddWithValue("@ProjectContractor", item.ProjectContractor);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@ProjectManager", item.ProjectManager);
                command.Parameters.AddWithValue("@Architect", item.Architect);
                command.Parameters.AddWithValue("@Status", item.Status);
                command.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                command.Parameters.AddWithValue("@ProjectStartDate", item.ProjectStartDate);
                command.Parameters.AddWithValue("@ProjectEndDate", item.ProjectEndDate);
                command.Parameters.AddWithValue("@ParentProjectId", item.ParentProjectId);
                command.Parameters.AddWithValue("@ProjectType", item.ProjectType);
                command.Parameters.AddWithValue("@TDrawingFields", item.TDrawingFields);
                command.Parameters.AddWithValue("@TFileIds", item.TFileIds);
                command.Parameters.AddWithValue("@Mode", mode); // 0=>Select,1=>Insert,2=>Update,3=>Delete 
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
