using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class InspectionActivityRepository : IInspectionActivityRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IInsDetailRepository _insDetailRepository;
        private readonly IAssetsRepository _assetsRepository;
        private readonly ITransaction _transaction;
        private readonly IFrequencyRepository _frequencyRepository;

        public InspectionActivityRepository(IFrequencyRepository frequencyRepository, ITransaction transaction, ISqlHelper sqlHelper, IFloorAssetRepository floorAssetRepository,
            IFloorRepository floorRepository, IUsersRepository usersRepository, IInsDetailRepository insDetailRepository, IAssetsRepository assetsRepository)
        {
            _frequencyRepository = frequencyRepository;
            _transaction = transaction;
            _insDetailRepository = insDetailRepository;
            _assetsRepository = assetsRepository;
            _floorAssetRepository = floorAssetRepository;
            _sqlHelper = sqlHelper;
            _floorRepository = floorRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionActivity"></param>
        /// <returns></returns>
        public int Save(TInspectionActivity inspectionActivity)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", inspectionActivity.InspectionId);
                command.Parameters.AddWithValue("@ActivityId", inspectionActivity.ActivityId);
                command.Parameters.AddWithValue("@FloorAssetId", inspectionActivity.FloorAssetId > 0 ? inspectionActivity.FloorAssetId : null);
                command.Parameters.AddWithValue("@Status", inspectionActivity.Status);
                command.Parameters.AddWithValue("@Comment", inspectionActivity.Comment is null ? "" : inspectionActivity.Comment);
                command.Parameters.AddWithValue("@CreatedBy", inspectionActivity.CreatedBy);
                command.Parameters.AddWithValue("@ActivityType", inspectionActivity.ActivityType);
                command.Parameters.AddWithValue("@IsDeficiency", inspectionActivity.IsDeficiency);
                command.Parameters.AddWithValue("@DocTypeId", inspectionActivity.DocTypeId);
                command.Parameters.AddWithValue("@DtEffectiveDate", inspectionActivity.DtEffectiveDate);
                command.Parameters.AddWithValue("@DeficiencyDate", inspectionActivity.DeficiencyDate);
                command.Parameters.AddWithValue("@SubStatus", inspectionActivity.SubStatus);
                command.Parameters.AddWithValue("@IsReInspection", inspectionActivity.IsReInspection);
                command.Parameters.AddWithValue("@FrequencyId", inspectionActivity.FrequencyId);
                command.Parameters.AddWithValue("@InsType", inspectionActivity.InsType);
                command.Parameters.AddWithValue("@ScheduleDueDate", inspectionActivity.ScheduleDueDate);
                command.Parameters.AddWithValue("@RaScore", inspectionActivity.RaScore);
                command.Parameters.AddWithValue("@ActivityInspectionDate", inspectionActivity.ActivityInspectionDate);
                command.Parameters.AddWithValue("@InspStatusCode", inspectionActivity.InspStatusCode);
                command.Parameters.AddWithValue("@EPDetailId", inspectionActivity.EPDetailId);
                command.Parameters.AddWithValue("@TinsDocId", inspectionActivity.TinsDocId);
                command.Parameters.AddWithValue("@WorkOrderId", inspectionActivity.WorkOrders?.Count > 0 ? inspectionActivity.WorkOrders.FirstOrDefault().WorkOrderId : 0);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public List<TFloorAssets> GetAssetsReports(string assetids)
        {
            var floorAssets = _floorAssetRepository.GetTFloorAssets().Where(x => assetids.Contains(x.AssetId.ToString())).ToList();
            var inspectionActivity = GetComplianceRpeort(assetids);
            foreach (var floorAsset in floorAssets)
            {
                SetEpAssetActivity(inspectionActivity, floorAsset);
            }

            return floorAssets;
        }

        public List<FloorAssetsWorkOrderCount> GetFloorAssetsWorkOrderCount()
        {
            var lists = new List<FloorAssetsWorkOrderCount>();
            const string sql = StoredProcedures.GET_FloorAssetsWorkOrderCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<FloorAssetsWorkOrderCount>(ds.Tables[0]);
            }
            return lists;
        }

        public List<TFloorAssets> InventoryInspectionAssetsReport(Request objRequest,
            string ids, string buildingId, string floorId, string fromdate, string todate, string status = null)
        {
            DateTime from = Convert.ToDateTime(fromdate);
            DateTime To = Convert.ToDateTime(todate);
            var floorAssets = _floorAssetRepository.GetFloorAssetByAsset(objRequest, ids);
            var epdetails = _assetsRepository.GetAssetEPs(ids);
            // get list of workordercount
            var floorAssetWOCount = GetFloorAssetsWorkOrderCount();

            if (!string.IsNullOrEmpty(buildingId) && Convert.ToInt32(buildingId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.Floor.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.FloorId == Convert.ToInt32(floorId)).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                //floorAssets = floorAssets.Where(x => x.StatusCode == status && x.CreatedDate >= from && x.CreatedDate <= To).ToList();
                floorAssets = floorAssets.Where(x => x.StatusCode == status).ToList();
            }
            if (!string.IsNullOrEmpty(fromdate) && floorAssets.Count > 0)
            {
                var inspectionActivity = GetAssetInspectionActivityFromToDate(fromdate, todate).ToList();
                foreach (var floorAsset in floorAssets)
                {
                    floorAsset.EPDetails = epdetails.Where(x => x.AssetId == floorAsset.AssetId).FirstOrDefault()?.EPdetails;
                    var floorAssetWO = floorAssetWOCount.FirstOrDefault(x => x.FloorAssetId == floorAsset.FloorAssetsId);
                    if (floorAssetWO != null)
                        floorAsset.OpenWorkOrdersCount = floorAssetWO.OpenWO;
                    if (floorAsset.EPDetails != null && floorAsset.EPDetails.Count > 0)
                    {
                        SetEpAssetActivity(inspectionActivity, floorAsset, fromdate, todate);
                    }
                }
            }
            return floorAssets;
        }


        public List<Assets> GetAssetsDashBoard(int userId, int? floorAssetId, int Inprogress = 0, int pastDue = 0, int dueWitndays = 0,
        string assetIds = null, string buildingIds = null, string floorIds = null)
        {
            List<Assets> asstsList = _assetsRepository.GetAssetMaster(userId);
            const string sql = StoredProcedures.Get_AssetsDashBoard;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.Parameters.AddWithValue("@Inprogress", Inprogress);
                command.Parameters.AddWithValue("@pastDue", pastDue);
                command.Parameters.AddWithValue("@dueWitndays", dueWitndays);
                command.Parameters.AddWithValue("@assetId", assetIds);
                command.Parameters.AddWithValue("@buildingId", buildingIds);
                command.Parameters.AddWithValue("@floorId", floorIds);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var floorAssets = new List<TFloorAssets>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //TFloorAssets floorAsset = new TFloorAssets;

                        var floorAsset = new TFloorAssets
                        {
                            Name = row["Name"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            DeviceNo = row["DeviceNo"].ToString(),
                            FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                            InspStatus = Convert.ToString(row["InspStatus"].ToString())
                        };
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                            floorAsset.FloorId = Convert.ToInt32(row["FloorId"].ToString());

                        floorAssets.Add(floorAsset);
                    }
                    //var floorAssets = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]);
                    if (!string.IsNullOrEmpty(assetIds))
                    {
                        int[] assetArray = assetIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        floorAssets = floorAssets.ToList().Where(x => x.AssetId.HasValue && assetArray.Contains(x.AssetId.Value)).ToList();
                    }
                    var floors = _floorRepository.GetAssetLocations().Where(x => x.IsActive).ToList();  //FloorRepository.ConvertToFloors(ds.Tables[0]);

                    var inspectionActivities = ConvertToInspectionActivities(ds.Tables[3]);

                    //  var inspectionActivities = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[2]);

                    foreach (var item in floorAssets)
                    {
                        if (item.FloorId.HasValue)
                            item.Floor = floors.FirstOrDefault(x => x.FloorId == item.FloorId);
                        item.TInspectionActivity = inspectionActivities.Where(x => x.FloorAssetId == item.FloorAssetsId).ToList();
                    }

                    if (!string.IsNullOrEmpty(buildingIds))
                    {
                        int[] buildingArray = buildingIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        floorAssets = floorAssets.ToList().Where(x => x.Floor != null && buildingArray.Contains(x.Floor.BuildingId)).ToList();
                    }

                    foreach (var item in asstsList)
                    {
                        item.TFloorAssets = floorAssets.Where(x => x.AssetId == item.AssetId).ToList();
                    }
                }
            }
            return asstsList;
        }
        private void SetEpAssetActivity(List<TInspectionActivity> inspectionActivity, TFloorAssets floorAsset, string fromDate = null, string toDate = null)
        {
            //var epDetails = EpRepository.GetEpByAssets(floorAsset.Assets.AssetId);
            foreach (var eps in floorAsset.EPDetails)
            {
                eps.TInspectionActivity = inspectionActivity.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId && x.EPDetailId == eps.EPDetailId).OrderByDescending(x => x.ActivityInspectionDate).ToList();
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    var assetInspectionActivity = eps.TInspectionActivity.Take(5).ToList();//Where(x => x.ActivityInspectionDate.Value.Date >= Convert.ToDateTime(fromdate).Date && x.ActivityInspectionDate.Value.Date <= Convert.ToDateTime(todate).Date).Take(5).ToList();
                    eps.TInspectionActivity = assetInspectionActivity;
                }
                // floorAsset.EPDetails.Add(eps);
            }
        }

        public List<TFloorAssets> GetFilterAssetsReports(string assetids, string buildingId, string floorId, string fromdate, string todate)
        {
            List<TFloorAssets> floorAssets = GetAssetsReports(assetids);
            if (!string.IsNullOrEmpty(assetids))
            {
                floorAssets = floorAssets.Where(x => assetids.Contains(x.AssetId.ToString())).ToList();
            }
            if (!string.IsNullOrEmpty(buildingId) && Convert.ToInt32(buildingId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.Floor.BuildingId == Convert.ToInt32(buildingId)).ToList();
            }
            if (!string.IsNullOrEmpty(floorId) && Convert.ToInt32(floorId) > 0)
            {
                floorAssets = floorAssets.Where(x => x.FloorId == Convert.ToInt32(floorId)).ToList();
            }
            //if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            //{
            //    floorAssets = floorAssets.Where(x => x.ActivityInspectionDate.Value.Date >= Convert.ToDateTime(fromdate).Date && x.ActivityInspectionDate.Value.Date <= Convert.ToDateTime(todate).Date).ToList();
            //}
            return floorAssets;
        }





        public List<TInspectionActivity> GetMatrixData(DateTime startDate, DateTime endDate)
        {
            var data = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_RoundMatrixData;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    data = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);

            }
            return data;
        }

        private List<TInspectionActivity> GetTInspectionActivity(Guid? activityId, int? inspectionId, int? activitytype, int? floorAssetId, bool isActivityUnique)
        {
            List<TInspectionActivity> activity;
            const string sql = StoredProcedures.Get_TInspectionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                command.Parameters.AddWithValue("@activitytype", activitytype);
                command.Parameters.AddWithValue("@floorAssetId", floorAssetId);
                command.CommandTimeout = 1000;
                var ds = _sqlHelper.GetDataSet(command);
                activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                if (isActivityUnique)
                {
                    activity = activity.GroupBy(test => test.ActivityId).Select(grp => grp.First()).ToList();
                }
                var users = _usersRepository.ConvertToUser(ds.Tables[0]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                foreach (var list in activity)
                {
                    if (list.FixedDueDate.HasValue)
                        list.DueDate = list.FixedDueDate;

                    if (list.CreatedBy > 0)
                        list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);
                }
            }
            return activity;
        }


        private IEnumerable<TInspectionActivity> GetAllRouteInspectionActivity(Guid? activityId)
        {
            var activities = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_RouteInspectionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                var ds = _sqlHelper.GetDataSet(command);
                var files = _sqlHelper.ConvertDataTable<TInspectionFiles>(ds.Tables[1]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var activity = new TInspectionActivity
                    {
                        ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                        ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString()),
                        ActivityType = Convert.ToInt32(row["ActivityType"].ToString()),
                        FloorAssetId = Convert.ToInt32(row["FloorAssetId"].ToString()),
                        InsType = Convert.ToInt32(row["InsType"].ToString()),
                        FrequencyId = Convert.ToInt32(row["FrequencyId"].ToString()),
                        Frequency = new FrequencyMaster { DisplayName = row["DisplayName"].ToString() }
                    };
                    activities.Add(activity);
                }
                var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                foreach (var list in activities)
                {
                    if (list.FloorAssetId != null)
                        list.TFloorAssets = _floorAssetRepository.GetFloorAsset(list.FloorAssetId.Value);

                    if (activityId != null)
                        list.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(activityId.Value);

                    if (list.FixedDueDate.HasValue)
                        list.DueDate = list.FixedDueDate;

                    if (list.CreatedBy > 0)
                        list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);

                    list.TInspectionFiles = files.Where(x => x.ActivityId == list.ActivityId).ToList();
                }
            }
            return activities;
        }

        public TInspectionActivity GetRouteInspectionActivity(Guid activityId)
        {
            return GetAllRouteInspectionActivity(activityId).FirstOrDefault();
        }


        public List<TInspectionActivity> GetInspectionActivity(Guid activityId)
        {
            return GetTInspectionActivity(activityId, null, null, null, true);
        }


        public List<TInspectionActivity> GetInspectionActivity(int inspectionId)
        {
            return GetTInspectionActivity(null, inspectionId, null, null, true);
        }

        public List<TInspectionActivity> GetAssetInspectionActivity()
        {
            return GetTInspectionActivity(null, null, 2, null, true);
        }

        public List<TInspectionActivity> GetAssetInspectionActivityFromToDate(string fromDate = null, string toDate = null)
        {
            var activity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_InspectionActivityFromToDate;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@startDate", fromDate);
                command.Parameters.AddWithValue("@endDate", toDate);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                    var users = _usersRepository.ConvertToUser(ds.Tables[0]);
                    foreach (var list in activity)
                    {
                        if (list.CreatedBy > 0)
                            list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);
                    }
                }
            }
            return activity;
        }

        public List<TInspectionActivity> GetCurrentActivities()
        {
            return GetTInspectionActivity(null, null, null, null, false).Where(x => x.IsCurrent).ToList();
        }


        public List<TInspectionActivity> GetFloorAssetInspectionActivity(int floorAssetId)
        {
            return GetTInspectionActivity(null, null, 2, floorAssetId, true);
        }

        public List<TInspectionActivity> GetInspectionActivityDetails(int inspectionId)
        {
            var list = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_InspectionActivityDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inspectionId", inspectionId);
                var ds = _sqlHelper.GetDataSet(command);
                var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var act = new TInspectionActivity
                    {
                        ActivityId = Guid.Parse(row["ActivityId"].ToString())
                    };
                    act = activity.FirstOrDefault(x => x.ActivityId == act.ActivityId);
                    if (act.FixedDueDate.HasValue)
                        act.DueDate = act.FixedDueDate;

                    switch (act.ActivityType)
                    {
                        case 3:
                            {
                                act.InspectionEPDocs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(ds.Tables[0]).FirstOrDefault();
                                //act.InspectionEPDocs = new InspectionEPDocs { ActivityId = act.ActivityId};
                                act.InspectionEPDocs = new InspectionEPDocs { ActivityId = act.ActivityId, DocTypeId = act.DocTypeId, DtEffectiveDate = act.DtEffectiveDate, CreatedDate = act.CreatedDate.Value };
                                if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                                    act.InspectionEPDocs.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());
                                if (act.InspectionEPDocs.DocTypeId > 0)
                                {
                                    act.InspectionEPDocs.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());
                                    act.InspectionEPDocs.DocumentName = row["DocumentName"].ToString();
                                }
                                act.InspectionEPDocs.DocumentType = new DocumentType()
                                {
                                    Name = row["DocumentTypeName"].ToString()
                                };
                                break;
                            }
                        case 2:
                            {
                                act.TFloorAssets = new TFloorAssets();
                                act.TFloorAssets.FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString());
                                act.TFloorAssets.DeviceNo = row["DeviceNo"].ToString();
                                act.TFloorAssets.SerialNo = row["SerialNo"].ToString();
                                act.TFloorAssets.Name = row["FloorAssetName"].ToString();
                                if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                                {
                                    act.TFloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                                    act.TFloorAssets.Floor = new Floor()
                                    {
                                        FloorName = row["FloorName"].ToString(),
                                        BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                        FloorId = act.TFloorAssets.FloorId.Value,
                                        Building = new Buildings()
                                        {
                                            BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                            BuildingName = row["BuildingName"].ToString()
                                        }
                                    };

                                }

                                break;
                            }
                    }
                    if (act.CreatedBy > 0)
                        act.UserProfile = users.FirstOrDefault(x => x.UserId == act.CreatedBy);

                    list.Add(act);
                }
            }
            return list;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="epDetailId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TInspectionActivity> GetCurrentTInspectionActivity(int? inspectionId, int epDetailId, int userId)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_CurrentTInspectionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", inspectionId);
                command.Parameters.AddWithValue("@EPDetailId", epDetailId);
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var activity = new TInspectionActivity
                        {
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            InspectionId = Convert.ToInt32(row["InspectionId"].ToString()),
                            Status = Convert.ToInt32(row["Status"].ToString()),
                            SubStatus = row["SubStatus"].ToString(),
                            ActivityType = Convert.ToInt32(row["ActivityType"].ToString()),
                            ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString()),
                            Comment = row["Comment"].ToString()
                        };
                        activity.CreatedDate = activity.ActivityInspectionDate;
                        if (!string.IsNullOrEmpty(row["CreatedBy"].ToString()))
                        {
                            activity.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        }
                        if (activity.DueDate != null)
                        {
                            activity.DueDate = Convert.ToDateTime(row["DueDate"].ToString());
                        }
                        //activity.DueDate = Convert.ToDateTime(row["DueDate"].ToString());
                        activity.InspectionEPDocs = new InspectionEPDocs
                        {
                            ActivityId = Guid.Parse(row["ActivityId"].ToString()),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                            ActivityType = Convert.ToInt32(row["ActivityType"].ToString()),
                            DocumentName = row["DocumentName"].ToString(),
                            Path = row["Path"].ToString()
                        };
                        if (!string.IsNullOrEmpty(row["DtEffectiveDate"].ToString()))
                        {
                            activity.InspectionEPDocs.DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString());
                            activity.DtEffectiveDate = Convert.ToDateTime(row["DtEffectiveDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["DocTypeId"].ToString()))
                        {
                            activity.InspectionEPDocs.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());
                            if (activity.InspectionEPDocs.DocTypeId.HasValue)
                            {
                                activity.InspectionEPDocs.DocumentType = new DocumentType
                                {
                                    Name = row["Name"].ToString()
                                };
                            }
                        }
                        if (!string.IsNullOrEmpty(row["UploadDocTypeId"].ToString()))
                        {
                            activity.InspectionEPDocs.UploadDocTypeId = Convert.ToInt32(row["UploadDocTypeId"].ToString());
                        }
                        activity.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                        if (!string.IsNullOrEmpty(row["UserId"].ToString()))
                        {
                            activity.UserProfile = new UserProfile
                            {
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString(),
                                PhoneNumber = row["PhoneNumber"].ToString(),
                                Email = row["Email"].ToString(),
                                VendorId = (!string.IsNullOrEmpty(row["VendorId"].ToString())) ? Convert.ToInt32(row["VendorId"].ToString()) : (int?)null
                            };
                        }
                        lists.Add(activity);
                    }
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var tfloorAssets = new TFloorAssets
                        {
                            FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),
                            DeviceNo = row["DeviceNo"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            IsRouteInsp = Convert.ToBoolean(row["IsRouteInsp"].ToString()),
                            NearBy = row["NearBy"].ToString(),
                            Assets = new Assets()
                            {
                                Name = row["AssetsName"].ToString()
                            }
                        };
                        if (!string.IsNullOrEmpty(row["InspectionGroupId"].ToString()))
                        {
                            tfloorAssets.InspectionGroupId = Convert.ToInt32(row["InspectionGroupId"].ToString());
                        }
                        tfloorAssets.InspectionCount = Convert.ToInt32(row["InspectionCount"].ToString());
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            tfloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            tfloorAssets.Floor = new Floor()
                            {
                                FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                                FloorName = row["FloorName"].ToString(),
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                Building = new Buildings()
                                {
                                    BuildingCode = row["BuildingCode"].ToString(),
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                    SiteName = row["SiteName"].ToString(),
                                    SiteCode = row["SiteCode"].ToString()
                                }
                            };
                        }
                        var activity = new TInspectionActivity();
                        activity.ActivityType = 2;
                        if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                        {
                            // inspection
                            activity.ActivityId = Guid.Parse(row["ActivityId"].ToString());
                            activity.InspectionId = Convert.ToInt32(row["InspectionId"].ToString());
                            activity.FloorAssetId = Convert.ToInt32(row["FloorAssetsId"].ToString());
                            activity.TFloorAssets = new TFloorAssets();
                            activity.TFloorAssets = tfloorAssets;
                            activity.Status = Convert.ToInt32(row["Status"].ToString());
                            activity.SubStatus = row["SubStatus"].ToString();
                            activity.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                            activity.ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString());
                            activity.CreatedDate = activity.ActivityInspectionDate;
                            activity.Comment = row["Comment"].ToString();
                            if (!string.IsNullOrEmpty(row["CreatedBy"].ToString()))
                            {
                                activity.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["FixedDueDate"].ToString()))
                            {
                                activity.DueDate = Convert.ToDateTime(row["FixedDueDate"].ToString());
                            }
                            if (!string.IsNullOrEmpty(row["UserId"].ToString()))
                            {
                                activity.UserProfile = new UserProfile
                                {
                                    UserId = Convert.ToInt32(row["UserId"].ToString()),
                                    FirstName = row["FirstName"].ToString(),
                                    LastName = row["LastName"].ToString(),
                                    PhoneNumber = row["PhoneNumber"].ToString(),
                                    Email = row["Email"].ToString(),
                                    VendorId = (!string.IsNullOrEmpty(row["VendorId"].ToString())) ? Convert.ToInt32(row["VendorId"].ToString()) : (int?)null
                                };
                            }
                        }
                        else
                        {
                            // no inspection                           
                            activity.TFloorAssets = new TFloorAssets();
                            activity.TFloorAssets = tfloorAssets;
                            activity.Status = -1;
                            activity.FloorAssetId = tfloorAssets.FloorAssetsId;
                            activity.CreatedDate = null;
                            activity.SubStatus = "NS";
                            //activity.CreatedDateTimeSpan = 0;
                            activity.UserProfile = new UserProfile();
                        }
                        lists.Add(activity);
                    }
                }


            }
            return lists;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDetailId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TInspectionActivity> GetInspectionActivityByEpAssets(int epDetailId, int floorAssetId, int userId)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_TInspectionActivityByEPAssets; //  "dbo.Get_TInspectionActivityByEPAssets";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epDetailId);
                command.Parameters.AddWithValue("@floorassetId", floorAssetId);
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                    var users = _usersRepository.ConvertToUser(ds.Tables[0]);
                    var frequencyMaster = _sqlHelper.ConvertDataTable<FrequencyMaster>(ds.Tables[0]);
                    foreach (var list in lists)
                    {
                        if (list.FixedDueDate.HasValue)
                            list.DueDate = list.FixedDueDate;
                        // if (list.CreatedBy)
                        list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);
                        if (list.FrequencyId.HasValue)
                            list.Frequency = frequencyMaster.FirstOrDefault(x => x.FrequencyId == list.FrequencyId);
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epdetailId"></param>
        /// <param name="floorassetId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TInspectionActivity> GetFloorAssetInspectionActivity(int floorassetId, int epdetailId, int userId)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_TInspectionActivityByEPAssets; //  "dbo.Get_TInspectionActivityByEPAssets";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (epdetailId > 0)
                {
                    command.Parameters.AddWithValue("@epdetailId", epdetailId);
                }
                command.Parameters.AddWithValue("@floorassetId", floorassetId);
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = ConvertToInspectionActivities(ds.Tables[0]); //_sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                    var users = _usersRepository.ConvertToUser(ds.Tables[0]);
                    var frequencyMaster = _sqlHelper.ConvertDataTable<FrequencyMaster>(ds.Tables[0]);
                    var inspectionEPDocs = _sqlHelper.ConvertDataTable<InspectionEPDocs>(ds.Tables[1]);
                    var issueIds = _sqlHelper.ConvertDataTable<WorkOrderFloorAssets>(ds.Tables[2]);

                    foreach (var list in lists)
                    {
                        if (list.CreatedBy > 0)
                            list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);
                        if (list.FrequencyId.HasValue)
                            list.Frequency = frequencyMaster.FirstOrDefault(x => x.FrequencyId == list.FrequencyId);
                        if (list.TinsDocId.HasValue)
                            list.InspectionDocs = inspectionEPDocs.Where(x => x.TInsDocs == list.TinsDocId).ToList();
                        var workOrders = issueIds.Where(x => x.ActivityId == list.ActivityId).ToList();
                        if (workOrders.Count > 0)
                        {
                            list.IsWorkOrder = 1;
                            foreach (var item in workOrders)
                            {
                                WorkOrder wo = new WorkOrder
                                {
                                    IssueId = item.IssueId.Value
                                };
                                list.WorkOrders.Add(wo);
                            }
                        }
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public List<TInspectionActivity> GetTInspectionActivity(int inspectionId)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_CompleteTInspectionActivity; // "dbo.Get_IncomTInspectionActivity";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", inspectionId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var tfloorAssets = new TFloorAssets
                        {
                            FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["Name"].ToString(),
                            DeviceNo = row["DeviceNo"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            InspectionCount = Convert.ToInt32(row["InspectionCount"].ToString()),
                            Assets = new Assets()
                            {
                                AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                                Name = row["AssetName"].ToString(),
                                IsStepsOnReport = Convert.ToBoolean(row["IsStepsOnReport"].ToString()),
                            }
                        };
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            tfloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            tfloorAssets.Floor = new Floor()
                            {

                                FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                                FloorName = row["FloorName"].ToString(),
                                BuildingId = Convert.ToInt32(row["BuildingID"].ToString()),
                                Building = new Buildings()
                                {
                                    BuildingCode = row["BuildingCode"].ToString(),
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingId = Convert.ToInt32(row["BuildingID"].ToString())
                                }

                            };
                        }
                        var activity = new TInspectionActivity();
                        if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                        {
                            // inspection
                            activity.ActivityType = Convert.ToInt32(row["ActivityType"].ToString());
                            activity.ActivityId = Guid.Parse(row["ActivityId"].ToString());
                            activity.Comment = row["Comment"].ToString();
                            activity.IsSubmit = Convert.ToBoolean(row["IsSubmit"].ToString());
                            activity.TInsActivityId = Convert.ToInt32(row["TInsActivityId"].ToString());
                            activity.InspectionId = Convert.ToInt32(row["InspectionId"].ToString());
                            activity.FloorAssetId = Convert.ToInt32(row["FloorAssetId"].ToString());
                            activity.TFloorAssets = new TFloorAssets();
                            activity.TFloorAssets = tfloorAssets;
                            activity.Status = Convert.ToInt32(row["Status"].ToString());
                            activity.SubStatus = row["SubStatus"].ToString();
                            if (!string.IsNullOrEmpty(row["IsWorkOrder"].ToString()))
                            {
                                activity.IsWorkOrder = Convert.ToInt32(row["IsWorkOrder"].ToString());
                            }
                            activity.OpenWOCount = Convert.ToInt32(row["OpenWOCount"].ToString());
                            activity.UserProfile = new UserProfile
                            {
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString()
                            };
                            activity.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                            activity.ActivityInspectionDate = Convert.ToDateTime(row["ActivityInspectionDate"].ToString());

                            if (!string.IsNullOrEmpty(row["RaScore"].ToString()))
                                activity.IsWorkOrder = Convert.ToInt32(row["RaScore"].ToString());

                            activity.CreatedDate = activity.ActivityInspectionDate;
                        }
                        lists.Add(activity);
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public TInspectionActivity GetTInspectionActivity(Guid activityId)
        {
            var inspectionActivity = GetInspectionActivity(activityId).FirstOrDefault();
            if (inspectionActivity != null)
            {
                if (inspectionActivity.FloorAssetId.HasValue)
                {
                    inspectionActivity.TFloorAssets = _floorAssetRepository.GetFloorAsset(inspectionActivity.FloorAssetId.Value);
                }
                inspectionActivity.TInspectionDetail = _insDetailRepository.GetTInspectionDetails(inspectionActivity.ActivityId);
                inspectionActivity.UserProfile = _usersRepository.GetUsersList(Convert.ToInt32(inspectionActivity.CreatedBy));
                inspectionActivity.TInspectionFiles = _transaction.GetInspectionFiles(activityId);

            }
            return inspectionActivity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        //public  List<TInspectionActivity> GetInspectionActivity(int inspectionId, int activityType)
        //{
        //    return GetInspectionActivity(inspectionId).Where(x => x.ActivityType == activityType && x.IsCurrent).ToList();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="floorAssetId"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        //public  List<TInspectionActivity> GetInspectionActivity(int inspectionId, int floorAssetId, int activityType)
        //{
        //    return GetInspectionActivity(inspectionId).Where(x => x.ActivityType == activityType && x.IsCurrent && x.FloorAssetId == floorAssetId).ToList();
        //}


        ///// <summary>
        ///// Created by avinash on date 07-12-2017 
        ///// </summary>
        ///// <param name="inspectionId"></param>
        ///// <param name="activityType"></param>
        ///// <returns></returns>
        //public  List<TInspectionActivity> GetInspectionActivitys(int floorAssetId, int activityType)
        //{
        //    return GetAssetInspectionActivity().Where(x => x.IsCurrent && x.FloorAssetId == floorAssetId).ToList();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="floorAssetId"></param>
        /// <returns></returns>
        //public  TInspectionActivity GetInspectionActivity(Guid activityId, int floorAssetId)
        //{
        //    return GetTInspectionActivity(activityId);
        //    //if (activity.ActivityId != Guid.Empty && activity.FloorAssetId.HasValue)
        //    //    activity.TFloorAssets = DAL.FloorAssetRepository.GetFloorAssetDescription(activity.FloorAssetId.Value);
        //    // return activity;
        //}

        //public  bool Update(TInspectionActivity activity)
        //{
        //    bool status;
        //    const string sql = StoredProcedures.Update_InspectionActivity;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@ActivityId", activity.ActivityId);
        //        command.Parameters.AddWithValue("@SubStatus", activity.SubStatus);
        //        command.Parameters.AddWithValue("@RaScore", activity.RaScore);
        //        command.Parameters.AddWithValue("@RaDate", activity.RaDate);
        //        status = _sqlHelper.ExecuteNonQuery(command);
        //    }
        //    return status;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TInspectionActivity> GetDeficientAssets(int userId, string id, string Taggedid)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_DeficientFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (userId > 0)
                    command.Parameters.AddWithValue("@userId", userId);
                if (!string.IsNullOrEmpty(id))
                    command.Parameters.AddWithValue("@activityid", id);
                if (!string.IsNullOrEmpty(Taggedid) && Taggedid != "0")
                    command.Parameters.AddWithValue("@tagId", Taggedid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    // var temActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);

                    var temActivity = ConvertDtToInspectionActivity(ds.Tables[0]);
                    var tInspectionDetail = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[1]);
                    var mainGoals = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[1]);
                    var inspectionSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[2]);
                    var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[2]);
                    var workOrders = ConvertDtToWorkOrderList(ds.Tables[3]); //_sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[3]);
                    var workorderfiles = _sqlHelper.ConvertDataTable<WorkOrderfiles>(ds.Tables[4]);

                    foreach (var wo in workOrders)
                    {
                        wo.WorkOrderFiles = workorderfiles.Where(x => x.IssueId == wo.IssueId).ToList();
                    }
                    foreach (var item in tInspectionDetail)
                    {
                        item.MainGoal = mainGoals.FirstOrDefault(x => x.MainGoalId == item.MainGoalId);
                    }

                    foreach (var inspectionStep in inspectionSteps)
                    {
                        inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                        if (inspectionStep.IssueId.HasValue)
                            inspectionStep.WorkOrders = workOrders.Where(x => x.IssueId == inspectionStep.IssueId).Distinct().ToList();
                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var activity = new TInspectionActivity
                        {
                            TInsActivityId = Convert.ToInt32(row["TInsActivityId"].ToString())
                        };

                        activity = temActivity.FirstOrDefault(x => x.TInsActivityId == activity.TInsActivityId);
                        if (activity != null)
                        {

                            if (!string.IsNullOrEmpty(row["EPDetailId"].ToString()))
                                activity.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());

                            activity.UserProfile = new UserProfile
                            {
                                UserId = Convert.ToInt32(row["CreatedBy"].ToString()),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString()
                            };
                            var tFloorAssets = new TFloorAssets
                            {
                                FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString()),
                                DeviceNo = row["DeviceNo"].ToString(),
                                SerialNo = row["SerialNo"].ToString(),
                                TmsReference = row["TmsReference"].ToString(),
                                Name = row["Name"].ToString(),
                                Assets = new Assets
                                {
                                    AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                                    Name = row["AssetsName"].ToString(),
                                    IsRouteInsp = Convert.ToBoolean(row["IsRouteInsp"].ToString())
                                },
                                AssetId = Convert.ToInt32(row["AssetId"].ToString())
                            };
                            if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                            {
                                tFloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                                tFloorAssets.Floor = new Floor
                                {
                                    FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                                    FloorName = row["FloorName"].ToString(),
                                    FloorCode = row["FloorCode"].ToString(),
                                    BuildingId = !string.IsNullOrEmpty(row["BuildingId"].ToString())
                                        ? Convert.ToInt32(row["BuildingId"].ToString())
                                        : 0,
                                    Building = new Buildings
                                    {
                                        BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                                        BuildingName = row["BuildingName"].ToString(),
                                        BuildingCode = row["BuildingCode"].ToString()
                                    }
                                };
                            }

                            activity.TFloorAssets = tFloorAssets;
                            activity.TInspectionDetail = tInspectionDetail.Where(x => x.ActivityId == activity.ActivityId).ToList();
                            foreach (var item in activity.TInspectionDetail)
                            {
                                item.InspectionSteps = inspectionSteps
                                    .Where(x => x.InspectionDetailId == item.InspectionDetailId).ToList();
                            }

                            var items = activity.TInspectionDetail.SelectMany(foo => foo.InspectionSteps).ToList();
                            if (items != null && items.Any(x => x.IssueId == null))
                                activity.IsWorkOrder = 0;
                            else if (items != null && items.Any(x => x.IssueId != null))
                                activity.IsWorkOrder = 1;

                            activity.Ilsm = new TIlsm();
                            if (!string.IsNullOrEmpty(row["IncidentId"].ToString()))
                            {
                                activity.Ilsm.IncidentId = row["IncidentId"].ToString();
                                activity.Ilsm.TIlsmId = Convert.ToInt32(row["TIlsmId"].ToString());
                            }




                            lists.Add(activity);
                        }
                    }
                }
            }
            lists = lists.Where(x => x.TInspectionDetail.Count != 0).ToList();
            return lists;
        }

        private List<WorkOrder> ConvertDtToWorkOrderList(DataTable dataTable)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            foreach (DataRow row in dataTable.Rows)
            {
                WorkOrder WO = new WorkOrder();
                WO.IssueId = Convert.ToInt32(row["IssueId"].ToString());
                WO.CRxCode = row["CRxCode"].ToString();
                WO.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());
                WO.WorkOrderNumber = row["WorkOrderNumber"].ToString();
                WO.AccountCode = row["AccountCode"].ToString();
                WO.SubStatusCode = row["SubStatusCode"].ToString();
                WO.StatusCode = row["StatusCode"].ToString();
                WO.DeficiencyCode = row["DeficiencyCode"].ToString();
                if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                    WO.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());
                WO.RequesterName = row["RequesterName"].ToString();
                WO.CompletionComments = row["CompletionComments"].ToString();
                WO.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
                workOrders.Add(WO);
            }

            return workOrders;
        }

        private List<TInspectionActivity> ConvertDtToInspectionActivity(DataTable dataTable)
        {
            List<TInspectionActivity> activities = new List<TInspectionActivity>();
            foreach (DataRow row in dataTable.Rows)
            {
                TInspectionActivity activity = new TInspectionActivity();
                activity.ActivityId = Guid.Parse(row["ActivityId"].ToString());
                //activity.Status = Convert.ToInt32(row["Status"].ToString());
                activity.ActivityType = Convert.ToInt32(row["ActivityType"].ToString());
                activity.TInsActivityId = Convert.ToInt32(row["TInsActivityId"].ToString());
                activity.SubStatus = row["SubStatus"].ToString();
                activity.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                activity.IsCurrent = Convert.ToBoolean(row["IsCurrent"].ToString());
                activity.Comment = row["Comment"].ToString();
                activity.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());

                if (!string.IsNullOrEmpty(row["RaScore"].ToString()))
                    activity.RaScore = Convert.ToInt32(row["RaScore"].ToString());

                if (!string.IsNullOrEmpty(row["IsWorkOrder"].ToString()))
                    activity.IsWorkOrder = Convert.ToInt32(row["IsWorkOrder"].ToString());

                if (!string.IsNullOrEmpty(row["InsType"].ToString()))
                    activity.InsType = Convert.ToInt32(row["InsType"].ToString());

                activity.ActivityInspectionDate = !string.IsNullOrEmpty(row["ActivityInspectionDate"].ToString()) ? Convert.ToDateTime(row["ActivityInspectionDate"].ToString()) : activity.CreatedDate;

                if (!string.IsNullOrEmpty(row["InspectionId"].ToString()))
                    activity.InspectionId = Convert.ToInt32(row["InspectionId"].ToString());
                if (!string.IsNullOrEmpty(row["FloorAssetId"].ToString()))
                    activity.FloorAssetId = Convert.ToInt32(row["FloorAssetId"].ToString());
                if (!string.IsNullOrEmpty(row["Status"].ToString()))
                    activity.Status = Convert.ToInt32(row["Status"].ToString());
                if (!string.IsNullOrEmpty(row["DocTypeId"].ToString()))
                    activity.DocTypeId = Convert.ToInt32(row["DocTypeId"].ToString());


                activities.Add(activity);
            }

            return activities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TInspectionActivity> GetComplianceRpeort(string assetids)
        {
            List<TInspectionActivity> activity;
            const string sql = StoredProcedures.Get_ComplianceRpeort;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@assetids", assetids);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                var tfloorAssets = _floorAssetRepository.BindFloorAssets(ds.Tables[1]);
                var standardEPs = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[0]);
                foreach (var list in activity)
                {
                    if (list.FixedDueDate.HasValue)
                        list.DueDate = list.FixedDueDate;

                    if (list.CreatedBy > 0)
                        list.UserProfile = users.FirstOrDefault(x => x.UserId == list.CreatedBy);

                    list.TFloorAssets = tfloorAssets.FirstOrDefault(x => x.FloorAssetsId == list.FloorAssetId);
                    list.StandardEps = standardEPs.FirstOrDefault(x => x.EPDetailId == list.EPDetailId);

                    //if (list.FrequencyId.HasValue)
                    //    list.Frequency = frequencyMaster.FirstOrDefault(x => x.FrequencyId == list.FrequencyId);
                }
            }
            return activity;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TInspectionActivity GetInspectionActivitybyTinsStepsId(int insStepsId)
        {
            var lists = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_InspectionActivitybyTinsStepsId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tinsStepsId", insStepsId);
                var ds = _sqlHelper.GetDataSet(command);
                var temActivity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                var tInspectionDetail = _sqlHelper.ConvertDataTable<TInspectionDetail>(ds.Tables[1]);
                var maingoal = _sqlHelper.ConvertDataTable<MainGoal>(ds.Tables[1]);
                var inspectionSteps = _sqlHelper.ConvertDataTable<InspectionSteps>(ds.Tables[2]);
                var steps = _sqlHelper.ConvertDataTable<Steps>(ds.Tables[2]);
                foreach (var item in tInspectionDetail)
                {
                    item.MainGoal = maingoal.FirstOrDefault(x => x.MainGoalId == item.MainGoalId);
                }
                foreach (var inspectionStep in inspectionSteps)
                {
                    inspectionStep.Steps = steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var activity = new TInspectionActivity
                    {
                        TInsActivityId = Convert.ToInt32(row["TInsActivityId"].ToString())
                    };

                    activity = temActivity.FirstOrDefault(x => x.TInsActivityId == activity.TInsActivityId);
                    if (activity != null)
                    {
                        activity.EPDetailId = Convert.ToInt32(row["EPDetailId"].ToString());
                        activity.IsWorkOrder = Convert.ToInt32(row["IsWorkOrder"].ToString());
                        activity.UserProfile = new UserProfile
                        {
                            UserId = Convert.ToInt32(row["TInsActivityId"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString()
                        };
                        var tFloorAssets = new TFloorAssets();
                        tFloorAssets.FloorAssetsId = Convert.ToInt32(row["FloorAssetsId"].ToString());
                        tFloorAssets.DeviceNo = row["DeviceNo"].ToString();
                        tFloorAssets.SerialNo = row["SerialNo"].ToString();
                        tFloorAssets.Name = row["Name"].ToString();
                        tFloorAssets.TmsReference = row["TmsReference"].ToString();
                        tFloorAssets.Assets = new Assets
                        {
                            AssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            Name = row["AssetsName"].ToString()
                        };
                        tFloorAssets.AssetId = Convert.ToInt32(row["AssetId"].ToString());
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            tFloorAssets.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            tFloorAssets.Floor = new Floor
                            {
                                FloorId = Convert.ToInt32(row["FloorId"].ToString()),
                                FloorName = row["FloorName"].ToString(),
                                Building = new Buildings
                                {
                                    BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                                    BuildingName = row["BuildingName"].ToString()
                                }
                            };
                        }

                        activity.TFloorAssets = tFloorAssets;
                        activity.TInspectionDetail =
                            tInspectionDetail.Where(x => x.ActivityId == activity.ActivityId).ToList();
                        foreach (var item in activity.TInspectionDetail)
                        {
                            item.InspectionSteps = inspectionSteps
                                .Where(x => x.InspectionDetailId == item.InspectionDetailId).ToList();
                        }

                        lists.Add(activity);
                    }
                }
            }
            return lists.FirstOrDefault();
        }

        public List<TInspectionActivity> GetAllInspectionActivity(int? inspectionId)
        {
            List<TInspectionActivity> activity = new List<TInspectionActivity>();
            const string sql = StoredProcedures.Get_AllInspectionActivity;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectionId", inspectionId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[0]);
                    //List<UserProfile> userProfiles = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    var userProfiles = _usersRepository.ConvertToUser(ds.Tables[0]);
                    foreach (var item in activity)
                    {
                        item.UserProfile = userProfiles.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }
                }
            }
            return activity;
        }


        public TComment SaveTComment(TComment comment)
        {
            const string sql = StoredProcedures.Insert_TComment;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TCommentId", comment.TCommentId);
                command.Parameters.AddWithValue("@TIlsmId", comment.TIlsmId);
                command.Parameters.AddWithValue("@StepId", comment.StepId);
                command.Parameters.AddWithValue("@AddedBy", comment.AddedBy);
                command.Parameters.AddWithValue("@AddeDate", comment.AddedDate);
                command.Parameters.AddWithValue("@Comments", comment.Comment);
                command.Parameters.AddWithValue("@IsDeleted", comment.IsDeleted);
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    comment.TCommentId = Convert.ToInt32(ds.Tables[0].Rows[0]["TCommentId"].ToString());
                    var userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]).FirstOrDefault();
                    if (userProfile != null)
                    {
                        comment.AddedDateTimeSpan = comment.AddedDate.HasValue ? Conversion.ConvertToTimeSpan(comment.AddedDate.Value) : 0;
                        comment.AddedByUserProfile = userProfile;
                    }
                }
            }
            return comment;

        }


        public List<TInspectionActivity> ConvertToInspectionActivities(DataTable dt)
        {
            List<TInspectionActivity> inspectionActivities = new List<TInspectionActivity>();
            List<FrequencyMaster> frequencies = new List<FrequencyMaster>();
            frequencies = _frequencyRepository.GetFrequency();
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    TInspectionActivity inspectionActivity = new TInspectionActivity
                    {
                        ActivityId = Guid.Parse(item["ActivityId"].ToString())
                    };

                    if (!string.IsNullOrEmpty(item["InspectionId"].ToString()))
                        inspectionActivity.InspectionId = Convert.ToInt32(item["InspectionId"].ToString());
                    inspectionActivity.Comment = item["Comment"].ToString();
                    inspectionActivity.ActivityInspectionDate = Convert.ToDateTime(item["ActivityInspectionDate"].ToString());
                    inspectionActivity.ActivityType = Convert.ToInt32(item["ActivityType"].ToString());
                    inspectionActivity.CreatedBy = Convert.ToInt32(item["CreatedBy"].ToString());

                    if (!string.IsNullOrEmpty(item["CreatedDate"].ToString()))
                        inspectionActivity.CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString());

                    if (dt.Columns.Contains("IssueId"))
                    {
                        if (!string.IsNullOrEmpty(item["IssueId"].ToString()))
                            inspectionActivity.IssueId = Convert.ToInt32(item["IssueId"].ToString());
                    }

                    if (!string.IsNullOrEmpty(item["EPDetailId"].ToString()))
                        inspectionActivity.EPDetailId = Convert.ToInt32(item["EPDetailId"].ToString());

                    if (!string.IsNullOrEmpty(item["TinsDocId"].ToString()))
                        inspectionActivity.TinsDocId = Convert.ToInt32(item["TinsDocId"].ToString());

                    if (!string.IsNullOrEmpty(item["FrequencyId"].ToString()))
                    {
                        inspectionActivity.FrequencyId = Convert.ToInt32(item["FrequencyId"].ToString());
                        inspectionActivity.Frequency = frequencies.FirstOrDefault(x => x.FrequencyId == inspectionActivity.FrequencyId);
                    }

                    if (!string.IsNullOrEmpty(item["FloorAssetId"].ToString()))
                        inspectionActivity.FloorAssetId = Convert.ToInt32(item["FloorAssetId"].ToString());

                    if (!string.IsNullOrEmpty(item["IlsmDate"].ToString()))
                        inspectionActivity.IlsmDate = Convert.ToDateTime(item["IlsmDate"].ToString());

                    inspectionActivity.IsCurrent = Convert.ToBoolean(item["IsCurrent"].ToString());
                    inspectionActivity.SubStatus = item["SubStatus"].ToString();
                    inspectionActivity.Status = Convert.ToInt32(item["Status"].ToString());
                    inspectionActivity.TInsActivityId = Convert.ToInt32(item["TInsActivityId"].ToString());
                    //inspectionActivity.IsSubmit = Convert.ToBoolean(item["TInsActivityId"]);
                    if (!string.IsNullOrEmpty(item["DueDate"].ToString()))
                        inspectionActivity.DueDate = Convert.ToDateTime(item["DueDate"].ToString());

                    if (dt.Columns.Contains("FirstName"))
                    {
                        inspectionActivity.UserProfile = new UserProfile
                        {
                            FirstName = item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString(),
                            UserId = inspectionActivity.CreatedBy
                        };
                    }

                    inspectionActivities.Add(inspectionActivity);
                }
            }
            return inspectionActivities;
        }
    }
}
