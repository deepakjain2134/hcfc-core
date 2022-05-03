using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public  class BinderRepository: IBinderRepository
    {
        private readonly ISqlHelper _sqlHelper;

        private readonly IEpRepository _epRepository;
        public BinderRepository(ISqlHelper sqlHelper, IEpRepository epRepository)
        {
            _epRepository = epRepository;
            _sqlHelper = sqlHelper;
        }


        public  Binders Get(int binderid)
        {
            return GetAllBinders(binderid).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  List<Binders> GetAllBinders(int? binderId)
        {
            var lists = new List<Binders>();
            const string sql = StoredProcedures.Get_Binders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", binderId);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    var epBinders = _sqlHelper.ConvertDataTable<EpBinder>(dt.Tables[1]);
                    lists = _sqlHelper.ConvertDataTable<Binders>(dt.Tables[0]);
                    foreach (Binders binder in lists)
                    {
                        binder.EpBinders = epBinders.Where(x => x.BinderId == binder.BinderId && x.IsActive == true).ToList();
                    }
                }
            }
            return lists;
        }

        public  List<Binders> GetAllBinders()
        {
            return GetAllBinders(null);
        }
        
        private  List<EPDetails> GetBindersEPs(int binderId)
        {
            var epdetails = new List<EPDetails>();
            const string sql = StoredProcedures.GetBindersEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", binderId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            Standard = new Standard
                            {
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString()
                            },
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            IsApplicable=Convert.ToBoolean(row["IsApplicable"].ToString()),
                            EpTransStatus= row["SubStatus"].ToString()
                        };

                        ep.DocumentType = new List<DocumentType>();
                        var assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                        var AssetSortOrder = "Name ASC";
                        var drfoundRows = ds.Tables[1].Select(assetExpression, AssetSortOrder);
                        for (var i = 0; i < drfoundRows.Length; i++)
                        {
                            var asset = new DocumentType
                            {
                                Name = Convert.ToString(drfoundRows[i]["Name"]),
                                DocTypeId = Convert.ToInt32(drfoundRows[i]["DocTypeId"])

                            };
                            ep.DocumentType.Add(asset);
                        }
                       
                        //if (ep != null)
                        //{
                        //    ep.EpTransStatus = _epRepository.EpTransStatus(ep.EPDetailId, null);
                        //}
                        epdetails.Add(ep); 
                    }
                }
            }
            return epdetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Binders> GetBindersList()
        {
            var lists = new List<Binders>();
            const string sql = StoredProcedures.Get_Binders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", null);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                    lists = _sqlHelper.ConvertDataTable<Binders>(dt.Tables[0]);
                foreach (var item in lists)
                {
                    item.StandardEps = new List<StandardEps>();
                    string expression = "BinderId = '" + item.BinderId + "'";
                    string sortOrder = "StandardEP ASC";
                    DataRow[] foundRows = dt.Tables[2].Select(expression, sortOrder);
                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        var standardEps = new StandardEps
                        {
                            EPDetailId = Convert.ToInt32(foundRows[i]["EPDetailId"]),
                            StandardEP = Convert.ToString(foundRows[i]["StandardEP"]).Trim(),
                            Description = Convert.ToString(foundRows[i]["Description"]),
                            TJCDescription = Convert.ToString(foundRows[i]["TJCDescription"]),
                            StDescID = Convert.ToInt32(foundRows[i]["StDescID"]),
                            TJCStandard = Convert.ToString(foundRows[i]["TJCStandard"])
                        };
                        item.StandardEps.Add(standardEps);
                    }
                }
                foreach (var item in lists)
                {
                    item.StandardEps = item.StandardEps.OrderBy(x => x.StandardEP).ToList();
                }
            }
            return lists;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Binders> GetBinders()
        {
            var epdetails = _epRepository.GetEps();
            var allBinders = GetAllBinders(null);
            var binders = allBinders.Where(x => x.ParentBinderId.HasValue).ToList();
            foreach (Binders binder in binders)
            {                
                binder.EpDetails = GetBindersEPs(binder.BinderId);
                foreach (var epBinder in binder.EpBinders)
                {
                    epBinder.EpDetails = epdetails.Where(x => x.EPDetailId == epBinder.EPDetailId).FirstOrDefault();
                }
            }
            return binders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        public  Binders GetBinder(int binderId)
        {
            var binder = GetAllBinders(binderId).FirstOrDefault();
            if (binder != null)
            {
                binder.EpDetails = GetBindersEPs(binder.BinderId);
                foreach (EpBinder epBinder in binder.EpBinders)
                {
                    if (epBinder != null)
                    {
                        epBinder.EpDetails = binder.EpDetails.Where(x => x != null && x.EPDetailId == epBinder.EPDetailId).FirstOrDefault();// EpRepository.GetEpDescription(epBinder.EPDetailId.Value); //
                    }
                }
            }
            return binder;
        }

        public  List<StandardEps> GetBinderStandardEps()
        {
            var objStandardEps = new List<StandardEps>();
            const string sql = StoredProcedures.Get_BinderStandardEps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    objStandardEps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[0]);
            }
            return objStandardEps;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public  int AddBinder(Binders modelData, string epBinders)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Binders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", modelData.BinderName);
                command.Parameters.AddWithValue("@Description", modelData.Description);
                command.Parameters.AddWithValue("@FileName", modelData.FileName);
                command.Parameters.AddWithValue("@FilePath", modelData.FilePath);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@ParentBinderId", modelData.ParentBinderId > 0 ? modelData.ParentBinderId : null);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                command.Parameters.AddWithValue("@DisplayName", modelData.DisplayName);
                command.Parameters.AddWithValue("@epBinders", epBinders);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public  Binders UpdateBinder(Binders modelData, string epBinders)
        {
            const string sql = StoredProcedures.Update_Binders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BinderId", modelData.BinderId);
                command.Parameters.AddWithValue("@Name", modelData.BinderName);
                command.Parameters.AddWithValue("@Description", modelData.Description);
                command.Parameters.AddWithValue("@BinderCode", modelData.BinderCode);
                command.Parameters.AddWithValue("@FileName", modelData.FileName);
                command.Parameters.AddWithValue("@FilePath", modelData.FilePath);
                command.Parameters.AddWithValue("@IsActive", modelData.IsActive);
                command.Parameters.AddWithValue("@epIds", epBinders);
                command.Parameters.AddWithValue("@CreatedBy", modelData.CreatedBy);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return modelData;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private  List<EpBinder> ConvertTableToEps(DataTable dataTable)
        {
            var epBinders = new List<EpBinder>();
            foreach (DataRow row in dataTable.Rows)
            {
                EpBinder epBinder = new EpBinder
                {
                    EPBinderId = Convert.ToInt32(row["EPBinderId"].ToString()),
                    BinderId = Convert.ToInt32(row["BinderId"].ToString()),
                    EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                    EpDetails = new EPDetails()
                    {
                        EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                        Description = row["Description"].ToString(),
                        EPNumber = row["EPNumber"].ToString(),
                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                        Standard = new Standard
                        {
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            TJCStandard = row["TJCStandard"].ToString()
                        }
                    },
                };
                epBinders.Add(epBinder);
            }
            return epBinders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        public  List<Binders> GetEpBinder(int epdetailId)
        {
            var binders = new List<Binders>();
            const string sql = StoredProcedures.Get_EpBinder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    binders = _sqlHelper.ConvertDataTable<Binders>(ds.Tables[0]);
            }
            return binders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binderId"></param>
        /// <returns></returns>
        public List<EPsDocument> GetBinderDocument(int? binderId, string Year, int? DocTypeId)
        {
            var binderDocument = new List<EPsDocument>();
            const string sql = StoredProcedures.Get_BinderDocs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", binderId ==-1 ? binderId : binderId > 0 ? binderId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Year", !String.IsNullOrEmpty(Year)? Year : (object)DBNull.Value);
                command.Parameters.AddWithValue("@DocTypeId", DocTypeId == -1 ? DocTypeId : DocTypeId > 0 ? DocTypeId : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    binderDocument = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[0]);
            }
            return binderDocument.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public async Task<List<EPsDocument>> GetBinderDocumentAsync(int? binderId)
        {
            var binderDocument = new List<EPsDocument>();
            const string sql = StoredProcedures.Get_BinderDocs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", binderId > 0 ? binderId : (object)DBNull.Value);
                var ds = await _sqlHelper.GetDataTableAsync(command);
                if (ds != null)
                    binderDocument = _sqlHelper.ConvertDataTable<EPsDocument>(ds);               
            }
            return binderDocument.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public  Binders GetEPBinderDocument(int? binderId)
        {
            var binderDocument = new Binders();
            const string sql = StoredProcedures.Get_BinderDocs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@binderId", binderId > 0 ? binderId : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    binderDocument.EPdocument = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[0]);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<EPsDocument> reportbinderDocument = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[1]);
                    binderDocument.EPdocument.AddRange(reportbinderDocument);
                }
            }
            return binderDocument;
        }       
    }
}