
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;
using System;
using HCF.Utility;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly ISqlHelper _sqlHelper;

        private readonly IEpRepository _epRepository;
        public DocumentsRepository(ISqlHelper sqlHelper, IEpRepository epRepository)
        {
            _sqlHelper = sqlHelper;
            _epRepository = epRepository;
        }



        #region Local Documents

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  Documents GetDocument(int docRepoId)
        {
            Documents documents;
            const string sql = Utility.StoredProcedures.Get_Documents;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@docRepoId", docRepoId);
                command.Parameters.AddWithValue("@clientNo", 0);
                command.Parameters.AddWithValue("@fileId", 0);
                var ds = _sqlHelper.GetCommonDataSet(command);
                documents = _sqlHelper.ConvertDataTable<Documents>(ds.Tables[0]).FirstOrDefault();
                var attatchments = _sqlHelper.ConvertDataTable<FilesRepository>(ds.Tables[1]);
                if (documents != null)
                {
                    documents.Attachments = new List<FilesRepository>();
                    documents.Attachments = attatchments.Where(x => x.DocumentRepoId == documents.DocumentRepoId).ToList();
                }
            }
            return documents;
        }


        public  List<Documents> GetInboxMails(int clientNo)
        {
            List<Documents> documents=new List<Documents>();
            const string sql = StoredProcedures.Get_Documents;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientNo", clientNo);
                command.Parameters.AddWithValue("@docRepoId", 0);
                command.Parameters.AddWithValue("@fileId", 0);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    documents = _sqlHelper.ConvertDataTable<Documents>(ds.Tables[0]);

                    var attatchments = new List<FilesRepository>();
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        var attatchment = new FilesRepository
                        {
                            DocumentId = Convert.ToInt32(item["DocumentId"].ToString()),
                            DocumentRepoId = Convert.ToInt32(item["DocumentRepoId"].ToString()),
                            Extension = item["Extension"].ToString(),
                            FileName = item["FileName"].ToString(),
                            FilePath = item["FilePath"].ToString(),
                            Id = Convert.ToInt32(item["Id"].ToString()),
                            IsRejected = Convert.ToBoolean(item["IsRejected"].ToString()),
                            IsUsed = Convert.ToBoolean(item["IsUsed"].ToString())
                        };

                        if (!string.IsNullOrEmpty(item["FileSize"].ToString()))
                            attatchment.FileSize = Convert.ToInt64(item["FileSize"].ToString());
                        if (!string.IsNullOrEmpty(item["DocTypeId"].ToString()))
                            attatchment.DocTypeId = Convert.ToInt32(item["DocTypeId"].ToString());

                        attatchments.Add(attatchment);
                    }
                    foreach (var doc in documents)
                    {
                        doc.Attachments = new List<FilesRepository>();
                        doc.Attachments = attatchments.Where(x => x.DocumentRepoId == doc.DocumentRepoId).ToList();
                    }
                }
               
            }
            return documents;
        }


        public  List<Documents> GetPolicyDocuments(int clientNo)
        {
            List<Documents> documents;
            const string sql = StoredProcedures.Get_Documents;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientNo", clientNo);
                command.Parameters.AddWithValue("@docRepoId", 0);
                command.Parameters.AddWithValue("@fileId", 0);
                var ds = _sqlHelper.GetDataSet(command);
                documents = _sqlHelper.ConvertDataTable<Documents>(ds.Tables[0]);

                var attatchments = _sqlHelper.ConvertDataTable<FilesRepository>(ds.Tables[1]);

                var documentMaster = _sqlHelper.ConvertDataTable<DocumentMaster>(ds.Tables[2]);

                Documents objDocuments = new Documents
                {
                    DocumentRepoId = 0,
                    Attachments = new List<FilesRepository>()
                };
                List<FilesRepository> objAttachments = new List<FilesRepository>();
                foreach (DocumentMaster item in documentMaster)
                {
                    var objAttachment = new FilesRepository()
                    {
                        Id = (item.AttachmentId.HasValue) ? item.AttachmentId.Value : 0,
                        DocumentId = item.DocumentId,
                        FileName = item.FileName,
                        DocTypeId = item.DocTypeId,
                        FilePath = item.FilePath,
                        CreatedDate = item.UploadedDate,

                    };
                    objAttachments.Add(objAttachment);
                }
                objDocuments.Attachments = objAttachments;
                documents.Add(objDocuments);
            }
            return documents;
        }

        public  DocumentMaster GetDocumentFile(int documentId)
        {
            DocumentMaster file = new DocumentMaster();
            const string sql = StoredProcedures.Get_DocumentMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@documentId", documentId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                    file = _sqlHelper.ConvertDataTable<DocumentMaster>(ds.Tables[0]).FirstOrDefault();
            }
            return file;
        }

        public  FilesRepository GetAttacheMentFile(int fileId)
        {
            FilesRepository file = new FilesRepository();
            const string sql = StoredProcedures.Get_Documents;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@fileId", fileId);
                command.Parameters.AddWithValue("@clientNo", 0);
                command.Parameters.AddWithValue("@docRepoId", 0);
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                    file = _sqlHelper.ConvertDataTable<FilesRepository>(ds.Tables[0]).Where(x => x.Id == fileId).FirstOrDefault();
            }
            return file;
        }

        public  int SaveDocumentMaster(DocumentMaster documentMaster)
        {
            int newId;
            const string sql = StoredProcedures.Insert_DocumentMaster;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AttachmentId", documentMaster.AttachmentId > 0 ? documentMaster.AttachmentId : null);
                command.Parameters.AddWithValue("@DocTypeId", documentMaster.DocTypeId);
                command.Parameters.AddWithValue("@FileName", documentMaster.FileName);
                command.Parameters.AddWithValue("@FilePath", documentMaster.FilePath);
                command.Parameters.AddWithValue("@IsDeleted", documentMaster.IsDeleted);
                command.Parameters.AddWithValue("@UploadedBy", documentMaster.UploadedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        public  int UpdateAttachDocType(FilesRepository file)
        {
            const string sql = StoredProcedures.Update_Attachmentdoc;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AttachmentId", file.Id);
                command.Parameters.AddWithValue("@DocTypeId", file.DocTypeId);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return file.Id;
        }



        public  List<DocumentMaster> GetDocumentMaster(int docTypeId)
        {
            List<DocumentMaster> files = new List<DocumentMaster>();
            const string sql = StoredProcedures.Get_DocumentMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@docTypeId", docTypeId > 0 ? docTypeId : (object)DBNull.Value);

                var ds = _sqlHelper.GetDataSet(command);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    files = _sqlHelper.ConvertDataTable<DocumentMaster>(ds.Tables[0]);
                    var docTypes = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);

                    foreach (var item in files)
                    {
                        item.DocumentType = docTypes.FirstOrDefault(x => x.DocTypeId == item.DocTypeId);
                    }
                }

            }
            return files;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public  int InsertDocument(Documents newDocument)
        {
            int createdDocumentRepoId;
            const string sql = StoredProcedures.Insert_Mail;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ParentDocumentRepoId", newDocument.ParentDocumentRepoId);
                command.Parameters.AddWithValue("@MessageId", newDocument.MessageId);
                command.Parameters.AddWithValue("@IsReplied", newDocument.IsReplied);
                command.Parameters.AddWithValue("@Subject", newDocument.Subject);
                command.Parameters.AddWithValue("@Sender", newDocument.Sender);
                command.Parameters.AddWithValue("@Details", newDocument.Details);
                command.Parameters.AddWithValue("@ClientNo", newDocument.ClientNo);
                command.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                _sqlHelper.ExecuteNonQuery(command);
                createdDocumentRepoId = (int)command.Parameters["@id"].Value;
            }
            return createdDocumentRepoId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentRepoId"></param>
        /// <returns></returns>
        public  bool DeleteDocument(int documentRepoId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_Document;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@documentRepoId", documentRepoId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  bool DeleteDocumentMaster(int documentId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_DocumentMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@documentId", documentId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region Attachments

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="docId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateAttachment(string ids, int docId, int type)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Update_Attachment;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ids", ids);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@docId", docId);
                status = _sqlHelper.CommonExecuteNonQuery(command);
            }
            return status;
        }

        public bool UpdateAttachment(Documents newDoc, int type)
        {
            var ids = string.Join(",", newDoc.Attachments.Select(x => x.Id));
            var status = UpdateAttachment(ids, newDoc.DocumentRepoId, type);
            return status;
        }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="newDocument"></param>
            /// <returns></returns>
            public  int InsertAttachment(FilesRepository newDocument)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_Attachments;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DocumentRepoId", newDocument.DocumentRepoId);
                command.Parameters.AddWithValue("@Extension", newDocument.Extension);
                command.Parameters.AddWithValue("@FileName", newDocument.FileName);
                command.Parameters.AddWithValue("@FilePath", newDocument.FilePath);
                command.Parameters.AddWithValue("@CreatedBy", newDocument.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateAttachment"></param>
        /// <returns></returns>
        public  bool UpdateAttachmentStatus(FilesRepository updateAttachment)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Update_Attachment_IsUsed_Status;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", updateAttachment.Id);
                command.Parameters.AddWithValue("@Status", updateAttachment.IsUsed);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }





        #endregion

        #region GetDeficiencyDoc

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Inspection> GetDeficiencyDoc()
        {
            List<Inspection> lists;
            const string sql = Utility.StoredProcedures.Get_DeficiencyDoc;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                var docs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(ds.Tables[2]);
                foreach (var list in lists)
                {
                    list.EPDetails = _epRepository.GetEpShortDescription(list.EPDetailId);
                    list.TInspectionActivity = activity.Where(x => x.InspectionId == list.InspectionId).ToList();
                    foreach (var act in list.TInspectionActivity)
                    {
                        act.InspectionDocs = docs.Where(x => x.ActivityId == act.ActivityId).ToList();
                    }
                }
            }
            return lists;
        }

        #endregion

        #region Binder Folder 

        public  int SaveBinderFolder(BinderFolders objbinderfolders)
        {
            int newId;
            const string sql = StoredProcedures.Insert_BinderFolders;

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FolderId", objbinderfolders.FolderId);
                command.Parameters.AddWithValue("@FolderName", objbinderfolders.FolderName);
                command.Parameters.AddWithValue("@CommonName", objbinderfolders.CommonName);
                command.Parameters.AddWithValue("@IsPublic", objbinderfolders.IsPublic);
                command.Parameters.AddWithValue("@TFileId", objbinderfolders.TFileId);
                command.Parameters.AddWithValue("@ValidUpTo", objbinderfolders.ValidUpTo);
                command.Parameters.AddWithValue("@ParentFolderId", objbinderfolders.ParentFolderId);
                command.Parameters.AddWithValue("@CreatedBy", objbinderfolders.CreatedBy);
                command.Parameters.AddWithValue("@IsSurveyPrepBinder", objbinderfolders.IsSurveyPrepBinder);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

        public  List<BinderFolders> GetBinderFolders(int? folderId)
        {
            var items = new List<BinderFolders>();
            const string sql = StoredProcedures.Get_BinderFolders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@folderId", (folderId.HasValue) ? folderId.Value : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        BinderFolders binderFolders = new BinderFolders();
                        binderFolders.CommonName = item["CommonName"].ToString();

                        if (!string.IsNullOrEmpty(item["CreatedBy"].ToString()))
                            binderFolders.CreatedBy = Convert.ToInt32(item["CreatedBy"].ToString());

                        binderFolders.CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString());
                        binderFolders.FolderId = Convert.ToInt32(item["FolderId"].ToString());
                        binderFolders.FolderName = item["FolderName"].ToString();
                        binderFolders.IsSurveyPrepBinder = Convert.ToBoolean(item["IsSurveyPrepBinder"].ToString());
                        binderFolders.IsActive = Convert.ToBoolean(item["IsActive"].ToString());
                        binderFolders.IsDeleted = Convert.ToBoolean(item["IsDeleted"].ToString());
                        binderFolders.IsPublic = Convert.ToBoolean(item["IsPublic"].ToString());
                        if (!string.IsNullOrEmpty(item["ParentFolderId"].ToString()))
                            binderFolders.ParentFolderId = Convert.ToInt32(item["ParentFolderId"].ToString());

                        if (!string.IsNullOrEmpty(item["ValidUpTo"].ToString()))
                            binderFolders.ValidUpTo = Convert.ToDateTime(item["ValidUpTo"].ToString());

                        if (!string.IsNullOrEmpty(item["TFileId"].ToString()))
                            binderFolders.TFileId = Convert.ToInt32(item["TFileId"].ToString());

                        if (!string.IsNullOrEmpty(item["CreatedBy"].ToString()))
                        {
                            binderFolders.CreatedBy = Convert.ToInt32(item["CreatedBy"].ToString());
                            binderFolders.CreatedByUserProfile = new UserProfile();
                            binderFolders.CreatedByUserProfile.UserId = Convert.ToInt32(item["UserId"].ToString());
                            binderFolders.CreatedByUserProfile.FirstName = item["FirstName"].ToString();
                            binderFolders.CreatedByUserProfile.LastName = item["LastName"].ToString();
                        }

                        items.Add(binderFolders);
                    }
                    var tFiles = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[1]);
                    foreach (var item in items)
                    {
                        if (item.TFileId.HasValue && item.TFileId > 0)
                            item.File = tFiles.FirstOrDefault(x => x.TFileId == item.TFileId);
                        item.ChildBinderFolders = items.Where(x => x.ParentFolderId.HasValue && x.ParentFolderId.Value == item.FolderId).ToList();
                    }
                }
            }
            return items;
        }

        public  bool DeleteBinderFolder(int folderId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_BinderFolder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@folderId", folderId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        public  HttpResponseMessage GetAHJDocumentNotificationdata()
        {
            HttpResponseMessage _objHttpResponseMessage = new HttpResponseMessage();
            const string sql = StoredProcedures.Get_AHJDocumentNotificationdata;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var files = _sqlHelper.ConvertDataTable<BinderFolders>(ds.Tables[0]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    foreach (var item in files)
                    {
                        item.CreatedByUserProfile = new UserProfile();
                        item.CreatedByUserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }
                    _objHttpResponseMessage.Result = new Result
                    {
                        BinderFolders = files
                    };
                }
            }
            return _objHttpResponseMessage;
        }

        #endregion

        #region AHJ document Space status

       public async Task<DocumentSpaceStatus> GetDocumentSpaceStatus(Guid orgkey)
        {
            DocumentSpaceStatus documents=new DocumentSpaceStatus();
            const string sql = StoredProcedures.Get_DocuemntSpaceStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orgkey", orgkey);               
                var dt =await _sqlHelper.GetDataTableAsync(command);
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(item["RemainingSpace"].ToString()))
                            documents.RemainingSpace = Convert.ToInt64(item["RemainingSpace"].ToString());
                        if (!string.IsNullOrEmpty(item["TotalSpace"].ToString()))
                            documents.TotalSpace = Convert.ToInt64(item["TotalSpace"].ToString());
                        if (!string.IsNullOrEmpty(item["UsedSpace"].ToString()))
                            documents.UsedSpace = Convert.ToInt64(item["UsedSpace"].ToString());
                    }
                }
            }
            return documents;
        }
        #endregion
    }
}