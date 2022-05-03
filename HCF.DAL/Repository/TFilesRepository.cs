using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;

using HCF.Utility;

namespace HCF.DAL
{
    public  class TFilesRepository: ITFilesRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public TFilesRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  int Save(TFiles item)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TFileId", item.TFileId);
                command.Parameters.AddWithValue("@FileName", item.FileName);
                command.Parameters.AddWithValue("@FilePath", item.FilePath);
                command.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                command.Parameters.AddWithValue("@Comment", item.Comment);
                command.Parameters.AddWithValue("@UploadedBy", item.UploadedBy);
                command.Parameters.AddWithValue("@AttachmentId", item.AttachmentId);
                command.Parameters.AddWithValue("@FileType", item.FileType);
                command.Parameters.AddWithValue("@FileSize", item.FileSize);
                command.Parameters.AddWithValue("@ActivityId", item.ActivityId);
                command.Parameters.AddWithValue("@MD5Digest", item.MD5Digest);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  TFiles GetFile(int fileId, string tbName)
        {
            return GetFiles(Convert.ToString(fileId), tbName).FirstOrDefault();
        }

        private IEnumerable<TFiles> GetFiles(string fileId,string tbName="")
        {
            var lists = new List<TFiles>();
            const string sql = StoredProcedures.Get_Files;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("fileId", fileId);
                cmd.Parameters.AddWithValue("tbName", !string.IsNullOrEmpty(tbName) ? tbName : (object)DBNull.Value);

                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<TFiles>(dt);
            }
            return lists;
        }

        public  List<TFiles> GetTFiles(string files)
        {
            var tfiles = new List<TFiles>();
            foreach (var item in files.Split(','))
            {
                int fileId = Convert.ToInt32(item.Split('_')[0]);
                string tblName = "";
                if (item.Split('_').Length > 1)
                {
                    tblName = item.Split('_')[1];
                }
                var tfile = GetFile(fileId, tblName);
                if (tfile != null)
                    tfiles.Add(tfile);
            }
            return tfiles;
        }
    }
}
