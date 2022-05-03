using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace HCF.DAL
{
    public class StandardRepository :IStandardRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly IEpRepository _epRepository;

        public StandardRepository(ISqlHelper sqlHelper, IEpRepository epRepository, ICommonRepository commonRepository, IInspectionRepository inspectionRepository)
        {
            _epRepository = epRepository;
            _inspectionRepository = inspectionRepository;
            _commonRepository = commonRepository;
               _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newGoalMaster"></param>
        /// <returns></returns>
        public  int Save(Standard standard)
        {
            int newId;
            const string sql = StoredProcedures.Insert_GoalMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", standard.CategoryId);
                command.Parameters.AddWithValue("@TJCStandard", standard.TJCStandard);
                command.Parameters.AddWithValue("@TJCDescription", standard.TJCDescription);
                command.Parameters.AddWithValue("@CreatedBy", standard.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", standard.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  List<Standard> Get()
        {
            return _commonRepository.GetTableData<Standard>(StoredProcedures.Get_GoalMaster.ToString());
        }

        public List<Standard> GetEPlists()
        {
            List<Standard> lists = new List<Standard>();
            const string sql = StoredProcedures.Get_EPlists; //"dbo.Get_GoalMaster";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                List<EPDetails> ePdetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[1]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var list = new Standard
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                        TJCDescription = row["TJCDescription"].ToString(),
                        TJCStandard = row["TJCStandard"].ToString(),
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        Category = new Category { Name = row["CategoryName"].ToString() },
                        EPDetails = ePdetails.Where(x => x.StDescID == Convert.ToInt32(row["StDescID"].ToString())).ToList(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString())
                    };
                    lists.Add(list);
                }
            }
            return lists;
        }

        public List<EPDetails> GetUserEPs(int userId)
        {
            List<EPDetails> lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_EPlists; //"dbo.Get_GoalMaster";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[2]);
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        public Standard GetEpHistory(int epDetailId)
        {
            Standard standard;
            string sql = StoredProcedures.Get_EPTranHistory;// "dbo.Get_EPTranHistory";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                standard = _sqlHelper.ConvertDataTable<Standard>(ds.Tables[0]).FirstOrDefault();
                List<EPDetails> ePDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
                if (standard != null)
                {
                    standard.EPDetails = new List<EPDetails>();
                    standard.EPDetails = ePDetails;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                        List<TInspectionActivity> activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[5]);
                        List<TInspectionFiles> tInspectionFiles = _sqlHelper.ConvertDataTable<TInspectionFiles>(ds.Tables[6]);
                        foreach (var ep in standard.EPDetails)
                        {
                            ep.Inspections = _inspectionRepository.GetEpsInspections(ep.EPDetailId).Where(x => x.Status.Value.ToString() != BDO.Enums.Status.In_Progress.ToString()).ToList(); //Inspection.Where(x => x.EPDetailId == ep.EPDetailId).ToList();
                            foreach (var inspection in ep.Inspections)
                            {
                                inspection.TInspectionActivity = activity.Where(x => x.InspectionId == inspection.InspectionId && x.ActivityType == 1).ToList();
                                foreach (var tInspectionActivity in inspection.TInspectionActivity)
                                {
                                    tInspectionActivity.UserProfile = users.FirstOrDefault(x => x.UserId == tInspectionActivity.CreatedBy);
                                    tInspectionActivity.TInspectionFiles = new List<TInspectionFiles>();
                                    tInspectionActivity.TInspectionFiles = tInspectionFiles.Where(x => x.ActivityId == tInspectionActivity.ActivityId).ToList();
                                }
                            }
                        }
                    }
                }
            }
            return standard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stdescId"></param>
        /// <returns></returns>
        public Standard GetStandard(int stdescId)
        {
            return GetStandards().FirstOrDefault(x => x.StDescID == stdescId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="standard"></param>
        /// <returns></returns>
        public bool UpdateStandard(Standard standard)
        {
            const string sql = StoredProcedures.Update_Standard; //"Update_Standard";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StDescID", standard.StDescID);
                command.Parameters.AddWithValue("@CategoryId", standard.CategoryId);
                command.Parameters.AddWithValue("@TJCStandard", standard.TJCStandard);
                command.Parameters.AddWithValue("@TJCDescription", standard.TJCDescription);
                command.Parameters.AddWithValue("@IsActive", standard.IsActive);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Standard> GetStandards()
        {
            List<Standard> lists = new List<Standard>();
            const string sql = StoredProcedures.Get_GoalMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                List<EPDetails> ePdetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[1]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var list = new Standard
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                        TJCDescription = row["TJCDescription"].ToString(),
                        TJCStandard = row["TJCStandard"].ToString(),
                        UserProfile = new UserProfile { UserName = row["UserName"].ToString() },
                        CreateDate = Convert.ToDateTime(row["CreateDate"].ToString()),
                        Category = new Category { Name = row["CategoryName"].ToString() },
                        EPDetails = ePdetails.Where(x => x.StDescID == Convert.ToInt32(row["StDescID"].ToString())).ToList(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString())
                    };
                    lists.Add(list);
                }
            }
            return lists;
        }

        public  List<Standard> DocumentStandardReports()
        {
            List<Standard> lists = new List<Standard>();
            const string sql = StoredProcedures.Get_DocumentStandardReports; //"dbo.Get_GoalMaster";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                List<EPDetails> ePdetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[1]);
                foreach (var item in ePdetails)
                {
                    item.EPFrequency = _epRepository.GetEpFrequency(item.EPDetailId);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var list = new Standard
                    {
                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                        StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                        TJCDescription = row["TJCDescription"].ToString(),
                        TJCStandard = row["TJCStandard"].ToString(),
                        EPDetails = ePdetails.Where(x => x.StDescID == Convert.ToInt32(row["StDescID"].ToString())).ToList()
                    };

                    lists.Add(list);
                }
            }
            return lists;

        }
    }
}
