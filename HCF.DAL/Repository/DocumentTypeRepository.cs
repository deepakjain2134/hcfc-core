using HCF.BDO;

using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public  class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public DocumentTypeRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ePDetailId"></param>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentTypes(int ePDetailId)
        {
            List<DocumentType> docTypes = new List<DocumentType>();
            const string sql = StoredProcedures.Get_EpDocumentTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", ePDetailId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    docTypes = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
                }

            }
            return docTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentType()
        {
            List<DocumentType> documentTypes = new List<DocumentType>();
            const string sql = Utility.StoredProcedures.Get_DocumentTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    documentTypes = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
                    var binders = _sqlHelper.ConvertDataTable<Binders>(ds.Tables[1]);
                    //foreach (var documentType in documentTypes)
                    //{
                    //    //if (documentType.BinderId.HasValue)
                    //    //{
                    //    //    documentType.Binder = binders.Single(x => x.BinderId == documentType.BinderId);
                    //    //}
                    //}
                }
            }
            return documentTypes;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<DocumentType> GetDocumentTypes()
        {
            var documentTypes = new List<DocumentType>();
            const string sql = StoredProcedures.Get_DocumentTypes;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    documentTypes = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);

                }
            }
            return documentTypes;

        }



        public  List<DocumentType> GetDocumentTypesMaster()
        {
            var documentTypes = new List<DocumentType>();
            const string sql = StoredProcedures.Get_DocumentTypesMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    documentTypes = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);

                }
            }
            return documentTypes;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="docTypeId"></param>
        /// <returns></returns>
        public  DocumentType GetDocumentType(int docTypeId)
        {
            return GetDocumentType().FirstOrDefault(x => x.DocTypeId == docTypeId);
        }

        public  DocumentType GetDocumentTypeMaster(int docTypeId)
        {
            // return GetDocumentTypes().Single(x => x.DocTypeId == docTypeId);
            return GetDocumentTypesMaster().Where(x => x.DocTypeId == docTypeId).FirstOrDefault();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newDocumentType"></param>
        /// <returns></returns>
        public  int AddDocumntType(DocumentType newDocumentType)
        {
            int docTypeId;
            const string sql = Utility.StoredProcedures.Insert_DocumentType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DocTypeId", newDocumentType.DocTypeId);
                command.Parameters.AddWithValue("@CreatedBy", newDocumentType.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newDocumentType.IsActive);
                command.Parameters.AddWithValue("@Name", newDocumentType.Name);
                command.Parameters.AddWithValue("@Path", newDocumentType.Path);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                docTypeId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            // return (docTypeId > 0) ? true : false;
            return docTypeId;
        }

        public  int InsertDocumentTypeMaster(DocumentType newDocumentType)
        {
            int docTypeId;
            const string sql = Utility.StoredProcedures.Insert_DocumentTypeMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DocTypeId", newDocumentType.DocTypeId);
                command.Parameters.AddWithValue("@CreatedBy", newDocumentType.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", newDocumentType.IsActive);
                command.Parameters.AddWithValue("@Name", newDocumentType.Name);
                command.Parameters.AddWithValue("@Path", newDocumentType.Path);
                command.Parameters.AddWithValue("@DocCategoryID", (int)newDocumentType.DocCategoryID);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                docTypeId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
            }
            // return (docTypeId > 0) ? true : false;
            return docTypeId;
        }


        public  int UpdateDocumntTypeMaster(DocumentType documentType)
        {
            bool status;
            string sql = Utility.StoredProcedures.Update_DocumentTypeMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DocTypeId", documentType.DocTypeId);
                command.Parameters.AddWithValue("@Name", documentType.Name);
                command.Parameters.AddWithValue("@Path", documentType.Path);
                command.Parameters.AddWithValue("@IsActive", documentType.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", documentType.CreatedBy);
                command.Parameters.AddWithValue("@DocCategoryID", (int)documentType.DocCategoryID);
                status = _sqlHelper.CommonExecuteNonQuery(command);
            }
            return documentType.DocTypeId;
        }

        public  bool DeleteDocumentTypeMaster(int docTypeId)
        {
            string sql = StoredProcedures.Delete_DocumentTypeMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DocTypeId", docTypeId);
                return _sqlHelper.CommonExecuteNonQuery(command);
            }

        }
        
    }
}
