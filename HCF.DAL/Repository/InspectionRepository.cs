using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Formats.Asn1.AsnWriter;

namespace HCF.DAL
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUsersRepository _usersRepository;
        private readonly IEpRepository _epRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        private readonly IInsDetailRepository _insDetailRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;


        public InspectionRepository(
            ISqlHelper sqlHelper,
            IUsersRepository usersRepository,
            IAssetsRepository assetsRepository,
            IFloorAssetRepository floorAssetRepository,
            IStepsRepository stepsRepository,
            IInsDetailRepository insDetailRepository,
            IInspectionActivityRepository inspectionActivityRepository,
            IEpRepository epRepository,
            IDocumentTypeRepository documentTypeRepository)
        {
            _inspectionActivityRepository = inspectionActivityRepository;
            _documentTypeRepository = documentTypeRepository;
            _insDetailRepository = insDetailRepository;
            _stepsRepository = stepsRepository;
            _floorAssetRepository = floorAssetRepository;
            _assetsRepository = assetsRepository;
            _usersRepository = usersRepository;
            _epRepository = epRepository;
            _sqlHelper = sqlHelper;
        }


        #region InspectionRepository



        public EPDetails GetCurrentEp(int epId)
        {
            DataSet ds;
            var epDetails = new List<EPDetails>();
            var sql = StoredProcedures.Get_EPDetailsCurrentStaus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epId", epId);
                ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var docType = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),

                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                    Code = row["Code"].ToString()
                                }
                            },
                            Scores = new Score
                            {
                                Name = row["ScoreName"].ToString()
                            },
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            DocStatus = Convert.ToInt32(row["DocStatus"].ToString()),
                            Inspection = new Inspection
                            {
                                InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                                InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString()),
                                CreatedDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                DueDate = string.IsNullOrEmpty(row["DueDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["DueDate"].ToString()),
                                GraceDate = string.IsNullOrEmpty(row["GraceDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["GraceDate"].ToString()),
                                Status = Convert.ToInt32(row["Status"].ToString()),
                                SubStatus = row["SubStatus"].ToString(),
                            },
                        };

                        if (docType != null)
                            ep.DocumentType = docType;

                        if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
                            ep.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

                        if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
                            ep.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

                        if (ep.Inspection.InspectionId > 0)
                            ep.EpTransStatus = EpTransStatus(ep.EPDetailId, ep.Inspection);
                        else
                            ep.EpTransStatus = "P";

                        ep.EPFrequency = new List<EPFrequency>(); //EPUsers(ep.EPDetailId);


                        // set ep frequency
                        if (!string.IsNullOrEmpty(row["FrequencyId"].ToString()))
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                    DisplayName = row["FrequencyDisplayName"].ToString(),
                                    Days = Convert.ToInt32(row["Days"].ToString()),
                                },
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                            };
                            ep.EPFrequency.Add(epfreq);
                        }
                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            ep.Assets = new List<Assets>();
                            var assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                            var AssetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[2].Select(assetExpression, AssetSortOrder);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    IsRouteInsp = Conversion.TryCastBoolean(drfoundRows[i]["IsRouteInsp"])
                                };
                                ep.Assets.Add(asset);
                            }
                        }

                        ep.EPUsers = new List<UserProfile>(); //EPUsers(ep.EPDetailId);
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[3].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var user = new UserProfile
                            {
                                Email = Convert.ToString(foundRows[i]["Email"]),
                                FirstName = Convert.ToString(foundRows[i]["FirstName"]),
                                LastName = Convert.ToString(foundRows[i]["LastName"]),
                                UserId = Convert.ToInt32(foundRows[i]["UserId"])
                            };
                            ep.EPUsers.Add(user);
                        }

                        // set epBinders
                        ep.Binders = new List<Binders>();
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                        {
                            var binder = new Binders
                            {
                                BinderId = Convert.ToInt32(row["BinderId"]),
                                BinderName = row["BinderName"].ToString()
                            };
                            ep.Binders.Add(binder);
                        }
                        ep.IsCustomFrequency = Convert.ToBoolean(row["IsCustomFrequency"]);

                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                            ep.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

                        epDetails.Add(ep);

                    }



                }
            }

            return epDetails.FirstOrDefault();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns> 
        public  int AddInspection(Inspection inspection)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Inspection; // "Insert_Inspection";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", inspection.InspectionId);
                command.Parameters.AddWithValue("@EPDetailId", inspection.EPDetailId);
                command.Parameters.AddWithValue("@Status", inspection.Status);
                command.Parameters.AddWithValue("@InspectionDate", DateTime.UtcNow);
                command.Parameters.AddWithValue("@DocStatus", inspection.DocStatus);
                command.Parameters.AddWithValue("@InspectionGroupId", inspection.InspectionGroupId);
                command.Parameters.AddWithValue("@TCycleId", inspection.TCycleId);
                command.Parameters.AddWithValue("@IsPreviousCycle", inspection.IsPreviousCycle==null?0:inspection.IsPreviousCycle);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }


        public int AddPreviousInspection(Inspection inspection)
        {
            int newId;
            const string sql = StoredProcedures.Insert_PrevInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", inspection.EPDetailId);
                command.Parameters.AddWithValue("@Status", inspection.Status);
                command.Parameters.AddWithValue("@InspectionDate", inspection.StartDate);
                command.Parameters.AddWithValue("@UpdatedBy", inspection.LastUpdatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }

      
        

        public List<EPSteps> GetAllEpDocs(int userId, int epDetailId)
        {
            List<EPSteps> objEpDocs = new List<EPSteps>();
            string sql = StoredProcedures.Get_UserDashboardData;// "Get_UserDashboardData";

            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@epdetailId", epDetailId > 0 ? epDetailId : (object)null);
                var dt = _sqlHelper.GetDataTable(command);
                foreach (DataRow rows in dt.Rows)
                {
                    EPSteps epdoc = new EPSteps
                    {
                        EPDetailId = Convert.ToInt32(rows["EPDetailId"].ToString()),
                        Status = Convert.ToInt32(rows["Status"].ToString()),
                        ScoreId = Convert.ToInt32(rows["ScoreId"].ToString()),
                        InspectionId = Convert.ToInt32(rows["InspectionId"].ToString()),
                        Score = rows["ScoreName"].ToString(),
                        ScoreName = rows["ScoreName"].ToString(),
                        SampleDocPath = rows["SampleDocPath"].ToString(),
                        DocName = rows["DocName"].ToString(),
                        StandardEP = rows["StandardEP"].ToString(),
                        InspectionGroupId = Convert.ToInt32(rows["InspectionGroupId"].ToString()),
                        DocInspectionGroupId = Convert.ToInt32(rows["DocInspectionGroupId"].ToString()),
                        DoctypeId = rows["DoctypeId"].ToString() == null || rows["DoctypeId"].ToString() == "" ? default(int) : Convert.ToInt32(rows["DoctypeId"].ToString()),
                        DOCStatus = Convert.ToInt32(rows["DOC"].ToString()),
                        Description = rows["Description"].ToString(),
                        IsDocRequired = Convert.ToBoolean(rows["IsDocRequired"].ToString())
                    };
                    if (epdoc.InspectionId > 0)
                    {
                        epdoc.EpTransStatus = EpTransStatus(epdoc.EPDetailId);
                        epdoc.EpTranStatus = EpTransStatusInt(epdoc.EPDetailId);
                    }
                    else
                    {
                        epdoc.EpTransStatus = "P";
                        epdoc.EpTranStatus = -1;
                    }
                    objEpDocs.Add(epdoc);
                }
            }
            return objEpDocs;
            //return objEpDocs.Where(x => x.IsDocRequired).ToList();
        }

        public  List<TInspectionActivity> GetInspections(int floorAssetId, int epId)
        {
            var activities = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_LastInspections;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@epId", epId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    activities = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                    var insDetails = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[1]);
                    var inspectionSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[2]);
                    foreach (var item in insDetails)
                    {
                        item.InspectionSteps = inspectionSteps.Where(x => x.InspectionDetailId == item.InspectionDetailId).ToList();

                    }
                    foreach (var itemActivity in activities)
                    {
                        itemActivity.TInspectionDetail = insDetails.Where(x => x.ActivityId == itemActivity.ActivityId).ToList();
                    }

                }
            }
            return activities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns>
        public  bool MarkInspectionRa()
        {
            bool status;
            const string sql = StoredProcedures.Update_Inspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@InspectionId", inspection.InspectionId);
                //command.Parameters.AddWithValue("@SubStatus", inspection.SubStatus);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epDetaild"></param>
        /// <returns></returns>
        private  List<Inspection> GetAllInspections(int? inspectionId, int? epDetaild)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_Inspections;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", epDetaild);
                command.Parameters.AddWithValue("@inspectionId", inspectionId > 0 ? inspectionId : null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var inspection = new Inspection
                        {
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                            IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString()),
                            InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            SubStatus = row["SubStatus"].ToString(),
                            StartDate=Convert.ToDateTime(row["StartDate"].ToString()),
                            GraceDate= Convert.ToDateTime(row["GraceDate"].ToString())
                        };
                        if (!string.IsNullOrEmpty(row["DueDate"].ToString()))
                            inspection.DueDate = Convert.ToDateTime(row["DueDate"].ToString());
                        if (!string.IsNullOrEmpty(row["Status"].ToString()))
                            inspection.Status = Convert.ToInt32(row["Status"].ToString());

                        if (!string.IsNullOrEmpty(row["DocStatus"].ToString()))
                            inspection.DocStatus = Convert.ToInt32(row["DocStatus"].ToString());


                        inspection.EPDetails = new EPDetails();
                        inspection.EPDetails.EPDetailId = inspection.EPDetailId;
                        inspection.EPDetails.Standard = new Standard();
                        inspections.Add(inspection);
                    }

                    //inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                    //var epDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
                    //var standard = _sqlHelper.ConvertDataTable<Standard>(ds.Tables[0]);
                    //var Users = _usersRepository.ConvertToUser(ds.Tables[0]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);


                    //foreach (var epDetail in epDetails)
                    //    epDetail.Standard = standard.FirstOrDefault(x => x.StDescID == epDetail.StDescID);
                    //foreach (var item in inspections)
                    //    item.EPDetails = epDetails.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    //foreach (var item in inspections)
                    //    item.UserProfile = Users.FirstOrDefault(x => x.UserId == item.LastUpdatedBy);

                }
            }
            return inspections;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  Inspection GetCurrentInspection(int epDetailId)
        {
            return GetAllInspections(null, epDetailId).FirstOrDefault(x => x.IsCurrent);
        }

        public  List<EPDetails> GetFloorAssetEp(int floorAssetId)
        {

            var lists = new List<EPDetails>();
            const string sql = StoredProcedures.Get_AssetsEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@assetId", null);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        var ep =_epRepository.GetEpDescription(Convert.ToInt32(row["EPDetailId"].ToString()));
                        ep.IsAssetSteps = Convert.ToInt32(row["IsAssetSteps"].ToString());
                        ep.EpTransStatus = EpTransStatus(ep.EPDetailId);
                        ep.EpTranStatus = EpTransStatusInt(ep.EPDetailId);
                        lists.Add(ep);
                    }
                }
            }
            return lists;

        }

        public  string EpTransStatus(int epDetailId, Inspection currentInspection = null)
        {
            var epTransSt = "P";
            if (currentInspection == null)
                currentInspection = GetCurrentInspection(epDetailId);
            if (currentInspection != null)
            {
                var nextDueDate = currentInspection.DueDate;
                var dueDate = Convert.ToDateTime(currentInspection.DueDate).Date.AddDays(1);
                var graceDate = new DateTime();
                if (currentInspection.GraceDate.HasValue)
                    graceDate = currentInspection.GraceDate.Value.Date.AddDays(1);


                if (currentInspection.EPDetails != null && currentInspection.EPDetails.InitialInspDate.HasValue)
                {
                    nextDueDate = ScheduleDateTime.GetScheduleFixedDate(currentInspection.EPDetails.FrequencyId, currentInspection.EPDetails.InitialInspDate.Value, 0);
                    dueDate = nextDueDate.Value;
                    graceDate = nextDueDate.Value;
                }

                if (currentInspection.Status == 0)
                    epTransSt = "F";
                else if (nextDueDate == null)
                    epTransSt = "I";
                else
                {


                    var currentDate = DateTime.UtcNow.Date;//.AddDays(1);
                    if (currentDate.Date >= dueDate.Date && currentDate.Date <= graceDate.Date)
                        epTransSt = "G";
                    else if (currentDate > graceDate)
                    {
                        epTransSt = "D";
                        MarkInspectionDueDatePassed(currentInspection.InspectionId);
                    }
                    else
                    {
                        if (currentInspection.Status != null)
                        {
                            switch (currentInspection.Status.Value)
                            {
                                case 2:
                                    epTransSt = "I";
                                    break;
                                case 1:
                                    epTransSt = "C";
                                    break;
                            }
                        }
                    }
                }
            }

            return epTransSt;
        }



        public  int EpTransStatusInt(int epDetailId, Inspection currentInspection = null)
        {
            var epTransSt = -1;
            if (currentInspection == null)
                currentInspection = GetCurrentInspection(epDetailId);// .GetInspection(epDetailId);         
            if (currentInspection != null)
            {
                if (currentInspection.Status == 0)
                    epTransSt = 0;
                else
                {
                    var dueDate = Convert.ToDateTime(currentInspection.DueDate).Date.AddDays(1);
                    var graceDate = new DateTime();
                    if (currentInspection.GraceDate.HasValue)
                    {
                        graceDate = currentInspection.GraceDate.Value.Date.AddDays(1);
                    }
                    var currentDate = DateTime.UtcNow.Date.AddDays(1);
                    if (currentDate.Ticks > dueDate.Ticks && currentDate.Ticks < graceDate.Ticks)
                        epTransSt = -1;
                    else if (graceDate < currentDate)
                        epTransSt = -1;
                    else
                    {
                        if (currentInspection.Status != null)
                            switch (currentInspection.Status.Value)
                            {
                                case 2:
                                    epTransSt = 2;
                                    break;
                                case 1:
                                    epTransSt = 1;
                                    break;
                            }
                    }
                }
            }

            return epTransSt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Inspection> GetInspections()
        {
            return GetAllInspections(null, null);
        }

        //public  Inspection EpStatus(int epDetailId)
        //{
        //    var currentInspection = GetEpInspection(epDetailId);
        //    return currentInspection;
        //}

        //public  Inspection GetEpInspection(int epDetailId)
        //{
        //    var inspections = new Inspection();
        //    const string sql = StoredProcedures.Get_EpInspection;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@epDetailId", epDetailId);
        //        var ds = _sqlHelper.GetDataSet(command);
        //        if (ds != null)
        //        {
        //            inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]).FirstOrDefault();
        //        }
        //    }
        //    return inspections;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public  List<Inspection> GetEpsInspections(int epDetailId)
        {
            return GetAllInspections(null, epDetailId).ToList();
        }
       
        


        public EPDetails GetInspectionHistory(int userId, int epdetailId, int inspectionId)
        {
            var newEpDetails =_epRepository.GetEpShortDescription(epdetailId) ?? new EPDetails();

            newEpDetails.Inspection = GetAssetCurrentInspection(0, epdetailId).FirstOrDefault() ?? new Inspection();

            inspectionId = newEpDetails.Inspection != null ? newEpDetails.Inspection.InspectionId : default(int);
            //newEpDetails.Inspection.TInspectionActivity = InspectionActivityRepository.GetCurrentTInspectionActivity(null, newEpDetails.EPDetailId, userId).ToList();
            newEpDetails.Inspection.TInspectionActivity = _inspectionActivityRepository.GetCurrentTInspectionActivity(inspectionId, newEpDetails.EPDetailId, userId).ToList();
            // Added by avinash varshney on date 20-20-2018 for showing all the activities without inspectionId 
            newEpDetails.TInspectionActivity = newEpDetails.Inspection.TInspectionActivity; //InspectionActivityRepository.GetCurrentTInspectionActivity(inspectionId, newEpDetails.EPDetailId, userId).ToList();
            //
            if (newEpDetails.Inspection.Status != null && newEpDetails.Inspection.Status != 2)
                newEpDetails.Inspection.TInspectionActivity = newEpDetails.Inspection.TInspectionActivity.Where(x => x.ActivityId != new Guid()).ToList();

            return newEpDetails;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        //public  Inspection GetInspection(int inspectionId)
        //{
        //    return GetAllInspections(inspectionId, null).FirstOrDefault();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        //public  Inspection GetInspection(int inspectionId, int epDetailId)
        //{
        //    return GetAllInspections(inspectionId, epDetailId).FirstOrDefault();
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="subStatus"></param>
        /// <returns></returns>
        public  List<Inspection> GetInspection(string subStatus)
        {
            return GetAllInspections(null, null).Where(x => x.SubStatus == subStatus).ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  DataTable GetIlsmInspections(Guid? activityId)
        {
            var dt = new DataTable();
            const string sql = StoredProcedures.Get_IlsmInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", (activityId.HasValue) ? activityId.Value : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    dt = ds.Tables[0];
            }
            return dt;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="epdetailId"></param>
        /// <returns></returns>
        public  List<Inspection> GetEpInspectionDoc(int userId, int epdetailId)
        {
            var lists = GetEpsInspections(epdetailId);
            const string sql = StoredProcedures.Get_EPInspectionDoc; // "dbo.GetEPInspectionDoc";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                var ds = _sqlHelper.GetDataSet(command);
                //lists = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                foreach (var list in lists)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string expression = "InspectionId = " + list.InspectionId;
                        DataRow[] foundRows = ds.Tables[1].Select(expression);
                        DataTable newDocs = foundRows.CopyToDataTable();
                        List<InspectionEPDocs> docs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(newDocs);
                        foreach (var inspectionDoc in docs)
                        {
                            if (inspectionDoc.DocTypeId.HasValue)
                                inspectionDoc.DocumentType = _documentTypeRepository.GetDocumentType(inspectionDoc.DocTypeId.Value);

                            //if (inspectionDoc.DtEffectiveDate.HasValue)
                            //    inspectionDoc.DtEffectiveDateTimeSpan = Conversion.ConvertToTimeSpan(inspectionDoc.DtEffectiveDate.Value);

                            inspectionDoc.UserProfile = users.FirstOrDefault(x => x.UserId == inspectionDoc.CreatedBy);

                            if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["AssetsId"].ToString()))
                            {
                                int? assetId = Convert.ToInt32(ds.Tables[1].Rows[0]["AssetsId"].ToString());
                                if (assetId.Value > 0)
                                {
                                    inspectionDoc.Assets = _assetsRepository.GetAsset(assetId.Value);
                                }
                            }
                        }
                        list.InspectionDocs = docs;
                    }
                }
            }
            return lists;
        }



        //public  int GetEpDocStatus(int epDetailId)
        //{
        //    int status = 0;
        //    const string sql = StoredProcedures.Get_EpDocStatus;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@EPDetailId", epDetailId);
        //        var ds = _sqlHelper.GetDataSet(command);
        //        if (ds != null)
        //            status = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //    }
        //    return status;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Inspection> GetCurrentInspections()
        {
            var inspections = GetInspections().Where(x => x.IsCurrent).ToList();
            foreach (var inspection in inspections)
            {
                inspection.TInspectionActivity = _inspectionActivityRepository.GetTInspectionActivity(inspection.InspectionId).Where(x => x.IsCurrent).ToList();
            }
            return inspections;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private  List<InspectionEPDocs> ConvertInspectionEpDocs(DataTable dt)
        {
            var inspectionDocs = new List<InspectionEPDocs>();
            //_sqlHelper.ConvertDataTable<InspectionEPDocs>(dt).ToList();
            var docType = _sqlHelper.ConvertDataTable<DocumentType>(dt).ToList();
            var binders = _sqlHelper.ConvertDataTable<Binders>(dt).ToList();
            foreach (DataRow dataRow in dt.Rows)
            {
                InspectionEPDocs inspDocs = new InspectionEPDocs();
                inspDocs.ActivityId = Guid.Parse(dataRow["ActivityId"].ToString());
                inspDocs.Path = dataRow["Path"].ToString();
                inspDocs.DocumentName = dataRow["DocumentName"].ToString();
                inspDocs.TInsDocs = Convert.ToInt32(dataRow["TInsDocs"].ToString());
                if (!string.IsNullOrEmpty(dataRow["FileId"].ToString()))
                    inspDocs.FileId = Convert.ToInt32(dataRow["FileId"].ToString());
                if (!string.IsNullOrEmpty(dataRow["UploadDocTypeId"].ToString()))
                    inspDocs.UploadDocTypeId = Convert.ToInt32(dataRow["UploadDocTypeId"].ToString());
                if (!string.IsNullOrEmpty(dataRow["DocTypeId"].ToString()))
                    inspDocs.DocTypeId = Convert.ToInt32(dataRow["DocTypeId"].ToString());

                if (!string.IsNullOrEmpty(dataRow["DtEffectiveDate"].ToString()))
                    inspDocs.DtEffectiveDate = Convert.ToDateTime(dataRow["DtEffectiveDate"].ToString());

                inspDocs.IsDeleted = Convert.ToBoolean(dataRow["IsDeleted"].ToString());

                if (inspDocs.BinderId.HasValue)
                    inspDocs.Binder = binders.FirstOrDefault(x => x.BinderId == inspDocs.BinderId);

                if (inspDocs.DocTypeId.HasValue)
                    inspDocs.DocumentType = docType.FirstOrDefault(x => x.DocTypeId == inspDocs.DocTypeId);

                inspectionDocs.Add(inspDocs);
            }
            return inspectionDocs;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        //public  Inspection GetWorkOrderSteps(Guid activityId)
        //{
        //    Inspection inpection;
        //    var tInspectionDetail = _insDetailRepository.TInspectionDetail(activityId).ToList();
        //    var mainGoals = _mainGoalRepository.GetMainGoal();
        //    var steps = _stepsRepository.GetSteps();
        //    List<InspectionSteps> tSteps;
        //    const string sql = StoredProcedures.Get_GetworkOrderSteps_ActivityId;
        //    List<TInspectionFiles> files;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@activityId", activityId);
        //        var ds = _sqlHelper.GetDataSet(command);
        //        inpection = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]).FirstOrDefault();
        //        tSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[1]);
        //        files = _sqlHelper.ConvertDataTable<TInspectionFiles>(ds.Tables[2]);
        //    }
        //    if (inpection != null)
        //    {
        //        inpection.EPDetails = _epRepository.GetEpDescription(inpection.EPDetailId);
        //        inpection.TInspectionActivity = _inspectionActivityRepository.GetInspectionActivity(inpection.InspectionId).Where(x => x.ActivityId == activityId).ToList();
        //        foreach (var inspectionDetail in tInspectionDetail)
        //        {
        //            //To add Remaining work Order steps that does not have work order Id
        //            inspectionDetail.InspectionSteps = tSteps.Where(x => x.InspectionDetailId == inspectionDetail.InspectionDetailId).ToList();
        //            inspectionDetail.MainGoal = new MainGoal();
        //            inspectionDetail.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);  //mainGoal.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);
        //            foreach (var inspectionStep in inspectionDetail.InspectionSteps)
        //            {
        //                inspectionStep.Steps = new Steps();
        //                inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
        //            }
        //        }
        //        foreach (var act in inpection.TInspectionActivity)
        //        {
        //            act.TInspectionDetail = tInspectionDetail;
        //            if (act.FloorAssetId != null)
        //                act.TFloorAssets = _floorAssetRepository.GetFloorAsset(act.FloorAssetId.Value);

        //            else if (act.ActivityType == 3)
        //                act.InspectionDocs = _transaction.GetInspectionDocs(act.ActivityId);

        //            act.TInspectionFiles = files.Where(x => x.ActivityId == act.ActivityId).ToList();
        //        }
        //    }
        //    return inpection;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public  Inspection GetActivityHistory(Guid activityId)
        {
            var inspection = new Inspection();
            var tInspectionActivity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_Inspection_ActivityId;
            var files = new List<TInspectionFiles>();
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    inspection = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]).FirstOrDefault();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        inspection.SubStatus = row["SubStatusEP"].ToString();
                        inspection.Status = Convert.ToInt32(row["EPStatus"].ToString());
                    }
                    tInspectionActivity = _inspectionActivityRepository.ConvertToInspectionActivities(ds.Tables[1]); //_sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    files = _sqlHelper.ConvertDataTable<TInspectionFiles>(ds.Tables[5]);
                }
                if (tInspectionActivity.Count > 0 && inspection == null)
                {
                    inspection = new Inspection()
                    {
                        InspectionId = 0
                    };
                }
                if (inspection != null)
                {
                    inspection.EPDetails = _epRepository.GetEpDescription(inspection.EPDetailId);
                    if (ds != null)
                    {
                        inspection.TInspectionActivity = tInspectionActivity; //_sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);

                        var details = _insDetailRepository.GetTInspectionDetails(activityId);
                        var tFiles = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[6]).FirstOrDefault();
                        foreach (var act in inspection.TInspectionActivity)
                        {
                            if (tFiles != null && tFiles.TFileId > 0)
                            {
                                act.ReportFile = tFiles;
                            }
                            act.UserProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]).FirstOrDefault();
                            act.WorkOrders =
                                _sqlHelper
                                    .ConvertDataTable<WorkOrder
                                    >(ds.Tables[3]);
                            act.TInspectionDetail = details;
                            if (act.FloorAssetId != null)
                                act.TFloorAssets = _floorAssetRepository.GetFloorAsset(act.FloorAssetId.Value);



                            if (act.TinsDocId.HasValue)
                            {
                                act.InspectionDocs = ConvertInspectionEpDocs(ds.Tables[4]);
                                act.InspectionDocs = act.InspectionDocs.Where(x => x.TInsDocs == act.TinsDocId).ToList();
                                if (act.InspectionDocs != null && act.InspectionDocs.Count > 0)
                                    act.InspectionEPDocs = act.InspectionDocs.FirstOrDefault(x => x.ActivityId == activityId);
                                //act.TInspectionFiles = files.Where(x => x.ActivityId == act.ActivityId).ToList();
                            }
                            else
                            {
                                act.InspectionDocs = ConvertInspectionEpDocs(ds.Tables[4]);
                                act.InspectionDocs = act.InspectionDocs.Where(x => x.ActivityId == act.ActivityId).ToList();
                                if (act.InspectionDocs != null && act.InspectionDocs.Count > 0)
                                    act.InspectionEPDocs = act.InspectionDocs.FirstOrDefault(x => x.ActivityId == activityId);
                            }
                        }
                    }
                }
            }
            return inspection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="isComplete"></param>
        /// <returns></returns>
        public  bool MarkInsdocStatus(int inspectionId, int isComplete)
        {
            bool status;
            const string sql = StoredProcedures.Update_EPdocStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                command.Parameters.AddWithValue("@isComplete", isComplete);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <returns></returns>
        public List<EPsDocument> GetEPsDocument(int epDetailId)
        {
            var binderDocument = new List<EPsDocument>();
            const string sql = StoredProcedures.Get_EpDocs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetIlId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    binderDocument = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[0]);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        var data = _sqlHelper.ConvertDataTable<EPsDocument>(ds.Tables[1]);
                        binderDocument.AddRange(data);
                    }
                }

            }
            return binderDocument.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Inspection> GetEpsCampusInspections(int epDetailId)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_EpsCampusInspections;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epDetailId", epDetailId);               
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                    var epDetails = _sqlHelper.ConvertDataTable<EPDetails>(ds.Tables[0]);
                    var Campus = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]);
                    var standard = _sqlHelper.ConvertDataTable<Standard>(ds.Tables[0]);
                    var Users = _usersRepository.ConvertToUser(ds.Tables[0]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);


                    foreach (var epDetail in epDetails)
                        epDetail.Standard = standard.FirstOrDefault(x => x.StDescID == epDetail.StDescID);
                    foreach (var item in inspections)
                        item.EPDetails = epDetails.FirstOrDefault(x => x.EPDetailId == item.EPDetailId);
                    foreach (var item in inspections)
                        item.UserProfile = Users.FirstOrDefault(x => x.UserId == item.LastUpdatedBy);
                    foreach (var item in inspections)
                        item.Campus = Campus.Where(x => x.EPDetailId == item.EPDetailId).ToList();

                }
            }
            return inspections;
        }

        #endregion

        #region Inspection WorkOrder

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public  List<Inspection> GetInspectionsforWorkOrder(string activityId)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_InspectionsforworkOrderbyActivityId; //
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                var tInspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                var tInspectionDetail = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[2]);
                var tSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[3]);
                var mainGoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[4]);
                var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[5]);
                var files = _sqlHelper.ConvertDataTable<TInspectionFiles>(ds.Tables[6]);
                if (tInspectionActivity.Count > 0 && inspections.Count == 0)
                {
                    var insp = new Inspection()
                    {
                        InspectionId = 0
                    };
                    inspections.Add(insp);
                }
                foreach (var inspection in inspections)
                {
                    inspection.EPDetails = _epRepository.GetEpDescription(inspection.EPDetailId);
                    inspection.TInspectionActivity = tInspectionActivity.Where(x => x.InspectionId == inspection.InspectionId).ToList();
                    foreach (var tinsActivity in inspection.TInspectionActivity)
                    {
                        tinsActivity.EPDetails = new List<EPDetails>();
                        if (tinsActivity.FloorAssetId != null)
                        {
                            tinsActivity.TFloorAssets = _floorAssetRepository.GetFloorAsset(tinsActivity.FloorAssetId.Value);
                        }
                        if (tinsActivity.ActivityType == 2)
                        { tinsActivity.EPDetails = GetInspectedEpFromInspectionForActivity(inspections); } //EpRepository.GetFloorAssetEp(tinsActivity.FloorAssetId.Value).Where(x => x.EPFrequency.FirstOrDefault().FrequencyId == tinsActivity.FrequencyId).ToList(); } // for assets get multiple EP from floorAssetID 
                        else
                        { tinsActivity.EPDetails.Add(inspection.EPDetails); } // in case of ep is assign inspection EP in Fail steps.

                        tinsActivity.TInspectionFiles = files.Where(x => x.ActivityId == tinsActivity.ActivityId).ToList();
                        tinsActivity.TInspectionDetail = tInspectionDetail.Where(x => x.ActivityId == tinsActivity.ActivityId).ToList();
                        foreach (var inspectionDetail in tinsActivity.TInspectionDetail)
                        {
                            inspectionDetail.InspectionSteps = tSteps.Where(x => x.InspectionDetailId == inspectionDetail.InspectionDetailId && x.Status == 0 && x.IssueId == null).ToList();
                            inspectionDetail.MainGoal = new MainGoal();
                            inspectionDetail.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);  //mainGoal.FirstOrDefault(x => x.MainGoalId == inspectionDetail.MainGoalId);
                            foreach (var inspectionStep in inspectionDetail.InspectionSteps)
                            {
                                inspectionStep.Steps = new Steps();
                                inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                            }
                        }
                        tinsActivity.TInspectionDetail = tInspectionDetail.Where(x => x.ActivityId == tinsActivity.ActivityId && x.InspectionSteps.Any(y => y.Status == 0 && y.IssueId == null)).ToList();
                    }
                }
            }
            return inspections;
        }


        public  List<Inspection> GetDeficiencies(string activityId)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_InspectionsforworkOrderbyActivityId; //
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                var tInspectionActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                if (tInspectionActivity.Count > 0 && inspections.Count == 0)
                {
                    var insp = new Inspection()
                    {
                        InspectionId = 0
                    };
                    inspections.Add(insp);
                }
                foreach (var inspection in inspections)
                {
                    inspection.TInspectionActivity = tInspectionActivity.Where(x => x.InspectionId == inspection.InspectionId).ToList();
                    foreach (var tinsActivity in inspection.TInspectionActivity)
                    {
                        if (tinsActivity.FloorAssetId != null)
                        {
                            tinsActivity.TFloorAssets = _floorAssetRepository.GetFloorAsset(tinsActivity.FloorAssetId.Value);
                        }
                    }
                }
            }
            return inspections;
        }

        private  List<EPDetails> GetInspectedEpFromInspectionForActivity(IEnumerable<Inspection> inspections)
        {
            var lists = new List<EPDetails>();
            foreach (var ep in inspections)
            {
                var epDetail = _epRepository.GetEpDescription(ep.EPDetailId);
                if (epDetail != null)
                {
                    epDetail.EpTransStatus = EpTransStatus(ep.EPDetailId, null);
                    epDetail.EpTranStatus = EpTransStatusInt(ep.EPDetailId, null);
                    lists.Add(epDetail);
                }
            }
            return lists;
        }

        public int UndoInspection(Inspection inspection)
        {
            int newId;
            const string sql = StoredProcedures.UndoInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPDetailId", inspection.EPDetailId);
                command.Parameters.AddWithValue("@InspectionId", inspection.InspectionId);
                command.Parameters.AddWithValue("@ActivityId", inspection.TInspectionActivity.FirstOrDefault().ActivityId);                
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
        #endregion

        #region Inspection Group

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objInspectionGroup"></param>
        /// <returns></returns>
        public  int Save(InspectionGroup objInspectionGroup)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionGroupId", objInspectionGroup.InspectionGroupId);
                command.Parameters.AddWithValue("@GroupName", objInspectionGroup.GroupName);
                command.Parameters.AddWithValue("@InspectionDate", objInspectionGroup.InspectionDate.Value.ToUniversalTime());
                command.Parameters.AddWithValue("@IsActive", objInspectionGroup.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", objInspectionGroup.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  bool UpdateInspectionGroup(InspectionGroup objInspectionGroup)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionGroupId", objInspectionGroup.InspectionGroupId);
                command.Parameters.AddWithValue("@GroupName", objInspectionGroup.GroupName);
                command.Parameters.AddWithValue("@InspectionDate", objInspectionGroup.InspectionDate.Value.ToUniversalTime());
                command.Parameters.AddWithValue("@IsActive", objInspectionGroup.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", objInspectionGroup.CreatedBy);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        public  List<InspectionGroup> GetInspectionGroup()
        {
            List<InspectionGroup> lists = new List<InspectionGroup>();
            string sql = StoredProcedures.Get_InspectionGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<InspectionGroup>(ds.Tables[0]);
            }
            return lists;
        }

        internal  bool MarkInspectionDueDatePassed(int inspectionId)
        {
            bool status;
            const string sql = StoredProcedures.Update_InspectionDueDatePassed;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region Inspection calendar 

        public  List<Inspection> GetInspectionsForCalendar(int userId, DateTime? startDate, DateTime? endDate)
        {
            List<Inspection> inspections = new List<Inspection>();
            string sql = StoredProcedures.Get_InspectionsForCalendar;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                command.Parameters.AddWithValue("@userId", userId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Inspection inspection = new Inspection
                        {
                            InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                            DueDate = Convert.ToDateTime(row["DueDate"].ToString()),
                            SubStatus = row["SubStatus"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["LastUpdatedDate"].ToString()))
                        {
                            inspection.LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"].ToString());
                        }
                        inspection.InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString());
                        inspection.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        inspection.EPDetails = new EPDetails()
                        {
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),

                            Standard = new Standard()
                            {
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString()
                            }                           
                        };
                        if (!string.IsNullOrEmpty(row["ScoreId"].ToString()))
                        {
                            inspection.EPDetails.Scores = new Score
                            {
                                Id = Convert.ToInt32(row["ScoreId"].ToString()),
                                Name = row["Name"].ToString()
                            };
                        }


                        inspection.EPDetails.EPUsers = new List<UserProfile>();
                        string expression = "EPDetailId = '" + inspection.EPDetails.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[1].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var user = new UserProfile
                            {
                                Email = Convert.ToString(foundRows[i]["Email"]),
                                FirstName = Convert.ToString(foundRows[i]["FirstName"]),
                                LastName = Convert.ToString(foundRows[i]["LastName"]),
                                UserId = Convert.ToInt32(foundRows[i]["UserId"])
                            };
                            inspection.EPDetails.EPUsers.Add(user);
                        }
                        inspections.Add(inspection);
                    }
                }
            }
            return inspections;
        }

        #endregion

        #region dashboard ep review calender
        public  calenderViewDashBoard GetDashboardCalendar(int userId)
        {
            calenderViewDashBoard calenderViewDash = new calenderViewDashBoard();
            string sql = StoredProcedures.Get_InspectionsForDashBoardCalendar;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var lists = _sqlHelper.ConvertDataTable<TExercises>(ds.Tables[1]);
                    var roundScheduleDates = _sqlHelper.ConvertDataTable<RoundScheduleDates>(ds.Tables[2]);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Inspection inspection = new Inspection
                        {
                            InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                            DueDate = Convert.ToDateTime(row["DueDate"].ToString()),
                            SubStatus = row["SubStatus"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["LastUpdatedDate"].ToString()))
                        {
                            inspection.LastUpdatedDate = Convert.ToDateTime(row["LastUpdatedDate"].ToString());
                        }
                        inspection.InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString());
                        inspection.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());

                        calenderViewDash.inspections.Add(inspection);
                    }
                    calenderViewDash.RoundSchedules = roundScheduleDates;
                    calenderViewDash.Exercises = lists;
                }
            }
            return calenderViewDash;
        }


        public  List<Inspection> GetEPCurrentInspection(int epDetailId)
        {
            return GetAssetCurrentInspection(0, epDetailId);
        }

        internal  List<Inspection> GetAssetCurrentInspection(int floorAssetsId, int epDetailId)
        {
            var inspections = new List<Inspection>();
            const string sql = StoredProcedures.Get_AssetCurrentInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetsId);
                command.Parameters.AddWithValue("@ePDetailId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                    var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    foreach (var item in activity)
                    {
                        item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }
                    foreach (var item in inspections)
                    {
                        item.TInspectionActivity = activity.Where(x => x.InspectionId == item.InspectionId).ToList();
                    }

                }
            }
            return inspections;
        }

        internal  List<TInspectionActivity> GetAssetCurrentInspectionActivity(int floorAssetsId, int epDetailId)
        {
            var inspectionActivities = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_AssetCurrentInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorAssetsId", floorAssetsId);
                command.Parameters.AddWithValue("@ePDetailId", epDetailId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    //inspections = _sqlHelper.ConvertDataTable<Inspection>(ds.Tables[0]);
                    inspectionActivities = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    foreach (var item in inspectionActivities)
                    {
                        item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }
                    //foreach (var item in inspections)
                    //{
                    //    item.TInspectionActivity = activity.Where(x => x.InspectionId == item.InspectionId).ToList();
                    //}

                }
            }
            return inspectionActivities;
        }


        public  TFloorAssets GetAssetInsHistory(int floorAssetsId, int epDetailId, int selectedEp)
        {
            var floorAssets =_floorAssetRepository.GetFloorAsset(floorAssetsId);
            if (floorAssets?.AssetId != null)
            {
                var ePDetails = _epRepository.GetEpByAssets(floorAssets.AssetId.Value).Where(x => x.IsExpired == false).OrderByDescending(x => x.CreatedDate).ToList();
                if (epDetailId > 0)
                    ePDetails = ePDetails.Where(x => x.EPDetailId == epDetailId).ToList();


                foreach (var ePDetail in ePDetails)
                {
                    //ePDetail.Schedule = ScheduleRepository.GetSchedulesbyEPAndFloorAssetId(ePDetail.EPDetailId, floorAssets.FloorAssetsId);

                    ePDetail.Inspections = GetAssetCurrentInspection(floorAssets.FloorAssetsId, ePDetail.EPDetailId);
                    //ePDetail.Inspections = InspectionRepository.GetEpsInspections(ePDetail.EPDetailId).Where(x => x.InspectionGroupId == floorAssets.InspectionGroupId).ToList();
                    var inspectionActivity = GetAssetCurrentInspectionActivity(floorAssets.FloorAssetsId, ePDetail.EPDetailId); //InspectionActivityRepository.GetInspectionActivityByEpAssets(ePDetail.EPDetailId, floorAssetsId, 4).OrderByDescending(x => x.CreatedDate).ToList();
                    if (inspectionActivity.Count > 0)
                    {
                        ePDetail.TInspectionActivity = inspectionActivity.Take(1).ToList();
                    }
                    ePDetail.Inspection = ePDetail.Inspections.FirstOrDefault(x => x.IsCurrent);
                    //foreach (var inspection in ePDetail.TInspectionActivity)
                    //{
                    //    //inspection.InspectionDocs = Transaction.GetInspectionDocs(inspection.ActivityId);
                    //}
                }
                //floorAssets.EPDetails = ePDetails.OrderBy(x => x.EPDetailId != selectedEp).ToList();
                floorAssets.EPDetails = epDetailId > 0 ? ePDetails.Where(m => m.EPDetailId == epDetailId).ToList() : ePDetails.OrderBy(x => x.EPDetailId != selectedEp).ToList();
            }
            return floorAssets;
        }



        #endregion

        #region Inspection Eps Archived

        public  List<EPDetails> GetArchivedEPsInspection(int userId)
        {
            var lists = new List<EPDetails>();
            string sql = StoredProcedures.Get_ArchivedEPsInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var inspection = new Inspection();

                        if (!string.IsNullOrEmpty(row["InspectionId"].ToString()))
                            inspection.InspectionId = Convert.ToInt32(row["InspectionId"].ToString());
                        if (!string.IsNullOrEmpty(row["LastInspectionDate"].ToString()))
                            inspection.LastUpdatedDate = Convert.ToDateTime(row["LastInspectionDate"].ToString());
                        

                        var eP = new EPDetails
                        {
                            Inspection = inspection,
                            Description= row["Description"].ToString(),
                            EPNumber = row["EPNumber"].ToString(),
                            EPDetailId =Convert.ToInt32(row["EPDetailId"].ToString()),
                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                              
                            },
                            Scores = new Score
                            {
                                Name = row["ScoreName"].ToString()
                            },

                        };
                        if (!string.IsNullOrEmpty(row["EPArchivedDate"].ToString()))
                            eP.LastUpdatedDate = Convert.ToDateTime(row["EPArchivedDate"].ToString());
                        eP.Binders = new List<Binders>();
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                        {
                            var binder = new Binders
                            {
                                BinderId = Convert.ToInt32(row["BinderId"]),
                                BinderName = row["BinderName"].ToString()
                            };
                            eP.Binders.Add(binder);
                        }
                        eP.Assets = new List<Assets>();
                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString())&& Convert.ToBoolean(row["IsAssetsRequired"].ToString())==true)
                        {
                                                  
                           
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(row["AssetName"]),
                                    AssetId = Convert.ToInt32(row["AssetId"]),
                                };
                                eP.Assets.Add(asset);
                            
                        }
                        lists.Add(eP);
                    }
                }
                return lists;
            }
        }

        //public int GetEpDocStatus(int epDetailId)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion


        public EPDetails GetEpByInspectionId(int epId , int InspectionId)
        {
            DataSet ds;
            var epDetails = new EPDetails();
            var sql = StoredProcedures.Get_EPDetailsByInspectionID;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epId", epId);
                command.Parameters.AddWithValue("@InspectionId", InspectionId);
                ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var docType = _sqlHelper.ConvertDataTable<DocumentType>(ds.Tables[0]);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var ep = new EPDetails
                        {
                            EPNumber = row["EPNumber"].ToString(),
                            Description = row["Description"].ToString(),
                            EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString()),
                            StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                            Priority = Convert.ToInt32(row["Priority"].ToString()),
                            IsAssetsRequired = Convert.ToBoolean(row["IsAssetsRequired"].ToString()),

                            Standard = new Standard
                            {
                                CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                StDescID = Convert.ToInt32(row["StDescID"].ToString()),
                                TJCStandard = row["TJCStandard"].ToString(),
                                Category = new Category
                                {
                                    CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                                    Code = row["Code"].ToString()
                                }
                            },
                            Scores = new Score
                            {
                                Name = row["ScoreName"].ToString()
                            },
                            IsDocRequired = Convert.ToBoolean(row["IsDocRequired"].ToString()),
                            DocStatus = Convert.ToInt32(row["DocStatus"].ToString()),
                            Inspection = new Inspection
                            {
                                InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                                InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString()),
                                CreatedDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["StartDate"].ToString()),
                                DueDate = string.IsNullOrEmpty(row["DueDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["DueDate"].ToString()),
                                GraceDate = string.IsNullOrEmpty(row["GraceDate"].ToString()) ? default(DateTime) : Convert.ToDateTime(row["GraceDate"].ToString()),
                                Status = Convert.ToInt32(row["Status"].ToString()),
                                SubStatus = row["SubStatus"].ToString(),
                            },
                        };

                        if (docType != null)
                            ep.DocumentType = docType;

                        if (!string.IsNullOrEmpty(row["EPStatus"].ToString()))
                            ep.EPStatus = Convert.ToInt16(row["EPStatus"].ToString());

                        if (!string.IsNullOrEmpty(row["InitialInspDate"].ToString()))
                            ep.InitialInspDate = Convert.ToDateTime(row["InitialInspDate"].ToString());

                        if (ep.Inspection.InspectionId > 0)
                            ep.EpTransStatus = EpTransStatus(ep.EPDetailId, ep.Inspection);
                        else
                            ep.EpTransStatus = "P";

                        ep.EPFrequency = new List<EPFrequency>(); //EPUsers(ep.EPDetailId);


                        // set ep frequency
                        if (!string.IsNullOrEmpty(row["FrequencyId"].ToString()))
                        {
                            var epfreq = new EPFrequency
                            {
                                Frequency = new FrequencyMaster
                                {
                                    FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                                    DisplayName = row["FrequencyDisplayName"].ToString(),
                                    Days = Convert.ToInt32(row["Days"].ToString()),
                                },
                                FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),

                            };
                            ep.EPFrequency.Add(epfreq);
                        }
                        if (!string.IsNullOrEmpty(row["IsAssetsRequired"].ToString()))
                        {
                            ep.Assets = new List<Assets>();
                            var assetExpression = "EPDetailId = '" + ep.EPDetailId + "'";
                            var AssetSortOrder = "Name ASC";
                            var drfoundRows = ds.Tables[2].Select(assetExpression, AssetSortOrder);
                            for (int i = 0; i < drfoundRows.Length; i++)
                            {
                                var asset = new Assets
                                {
                                    Name = Convert.ToString(drfoundRows[i]["Name"]),
                                    AssetId = Convert.ToInt32(drfoundRows[i]["AssetId"]),
                                    IsRouteInsp = Conversion.TryCastBoolean(drfoundRows[i]["IsRouteInsp"])
                                };
                                ep.Assets.Add(asset);
                            }
                        }

                        ep.EPUsers = new List<UserProfile>(); //EPUsers(ep.EPDetailId);
                        string expression = "EPDetailId = '" + ep.EPDetailId + "'";
                        string sortOrder = "FirstName ASC";
                        var foundRows = ds.Tables[3].Select(expression, sortOrder);
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            var user = new UserProfile
                            {
                                Email = Convert.ToString(foundRows[i]["Email"]),
                                FirstName = Convert.ToString(foundRows[i]["FirstName"]),
                                LastName = Convert.ToString(foundRows[i]["LastName"]),
                                UserId = Convert.ToInt32(foundRows[i]["UserId"])
                            };
                            ep.EPUsers.Add(user);
                        }

                        // set epBinders
                        ep.Binders = new List<Binders>();
                        if (!string.IsNullOrEmpty(row["BinderId"].ToString()))
                        {
                            var binder = new Binders
                            {
                                BinderId = Convert.ToInt32(row["BinderId"]),
                                BinderName = row["BinderName"].ToString()
                            };
                            ep.Binders.Add(binder);
                        }
                        ep.IsCustomFrequency = Convert.ToBoolean(row["IsCustomFrequency"]);

                        if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                            ep.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

                        epDetails=ep;

                    }



                }
            }

            return epDetails;
        }
        
    }
}