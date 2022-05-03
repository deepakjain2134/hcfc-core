using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace HCF.DAL
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IFloorRepository _floorRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;


        public WorkOrderRepository(ISqlHelper sqlHelper, IFloorRepository floorRepository, IUsersRepository usersRepository,
            IFloorAssetRepository floorAssetRepository, IInspectionActivityRepository inspectionActivityRepository
            )
        {
            _inspectionActivityRepository = inspectionActivityRepository;
            _floorRepository = floorRepository;
            _usersRepository = usersRepository;
            _floorAssetRepository = floorAssetRepository;
            _sqlHelper = sqlHelper;
        }


        #region Work Order

        private HttpResponseMessage _objHttpResponseMessage = new HttpResponseMessage();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newWorkOrder"></param>
        /// <returns></returns>
        public int Save(WorkOrder newWorkOrder)
        {
            int newId;
            const string sql = StoredProcedures.Insert_WorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountCode", newWorkOrder.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newWorkOrder.AccountDescription);
                command.Parameters.AddWithValue("@ActionCode", newWorkOrder.ActionCode);
                command.Parameters.AddWithValue("@ActionDescription", newWorkOrder.ActionDescription);
                command.Parameters.AddWithValue("@AssetGroupNumber", newWorkOrder.AssetGroupNumber);
                command.Parameters.AddWithValue("@AssetNumber", newWorkOrder.AssetNumber);
                command.Parameters.AddWithValue("@BuildingCode", newWorkOrder.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newWorkOrder.BuildingName);
                command.Parameters.AddWithValue("@CauseCode", newWorkOrder.CauseCode);
                command.Parameters.AddWithValue("@CauseDescription", newWorkOrder.CauseDescription);
                command.Parameters.AddWithValue("@CompletionComments", newWorkOrder.CompletionComments);
                command.Parameters.AddWithValue("@DateAvailable", newWorkOrder.DateAvailable);
                command.Parameters.AddWithValue("@DateCompleted", newWorkOrder.DateCompleted);
                command.Parameters.AddWithValue("@DateCreated", newWorkOrder.DateCreated ?? DateTime.UtcNow);
                command.Parameters.AddWithValue("@DateNeeded", newWorkOrder.DateNeeded);
                command.Parameters.AddWithValue("@DateUpdated", newWorkOrder.DateUpdated);
                command.Parameters.AddWithValue("@Description", newWorkOrder.Description);
                command.Parameters.AddWithValue("@ItemCode", newWorkOrder.ItemCode);
                command.Parameters.AddWithValue("@ItemDescription", newWorkOrder.ItemDescription);
                command.Parameters.AddWithValue("@LocationCode", newWorkOrder.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newWorkOrder.LocationDescription);
                command.Parameters.AddWithValue("@MeterReading", newWorkOrder.MeterReading);
                command.Parameters.AddWithValue("@PriorityCode", newWorkOrder.PriorityCode);
                command.Parameters.AddWithValue("@PriorityDescription", newWorkOrder.PriorityDescription);
                command.Parameters.AddWithValue("@ProblemCode", newWorkOrder.ProblemCode);
                command.Parameters.AddWithValue("@ProblemDescription", newWorkOrder.ProblemDescription);
                command.Parameters.AddWithValue("@ReferenceNumber", newWorkOrder.ReferenceNumber);
                command.Parameters.AddWithValue("@RequesterComments", newWorkOrder.RequesterComments);
                command.Parameters.AddWithValue("@RequesterEmail", newWorkOrder.RequesterEmail);
                command.Parameters.AddWithValue("@RequesterName", newWorkOrder.RequesterName);
                command.Parameters.AddWithValue("@RequesterPager", newWorkOrder.RequesterPager);
                command.Parameters.AddWithValue("@RequesterPhone", newWorkOrder.RequesterPhone);
                command.Parameters.AddWithValue("@SegmentId", newWorkOrder.SegmentID);
                command.Parameters.AddWithValue("@ShopCode", newWorkOrder.ShopCode);
                command.Parameters.AddWithValue("@ShopDescription", newWorkOrder.ShopDescription);
                command.Parameters.AddWithValue("@SiteCode", newWorkOrder.SiteCode);
                command.Parameters.AddWithValue("@SiteName", newWorkOrder.SiteName);
                command.Parameters.AddWithValue("@SkillCode", newWorkOrder.SkillCode);
                command.Parameters.AddWithValue("@SkillDescription", newWorkOrder.SkillDescription);
                command.Parameters.AddWithValue("@StatusCode", newWorkOrder.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newWorkOrder.StatusDescription);
                command.Parameters.AddWithValue("@SubStatusCode", newWorkOrder.SubStatusCode);
                command.Parameters.AddWithValue("@SubStatusDescription", newWorkOrder.SubStatusDescription);
                command.Parameters.AddWithValue("@TotalCost", newWorkOrder.TotalCost);
                command.Parameters.AddWithValue("@TotalHours", newWorkOrder.TotalHours);
                command.Parameters.AddWithValue("@TotalMaterialCharges", newWorkOrder.TotalMaterialCharges);
                command.Parameters.AddWithValue("@TotalTimeCharges", newWorkOrder.TotalTimeCharges);
                command.Parameters.AddWithValue("@TypeCode", newWorkOrder.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newWorkOrder.TypeDescription);
                command.Parameters.AddWithValue("@WeekScheduled", newWorkOrder.WeekScheduled);
                command.Parameters.AddWithValue("@WorkOrderId", newWorkOrder.WorkOrderId);
                command.Parameters.AddWithValue("@WorkOrderNumber", newWorkOrder.WorkOrderNumber);
                command.Parameters.AddWithValue("@ActivityId", newWorkOrder.ActivityId);
                command.Parameters.AddWithValue("@CreatedBy", newWorkOrder.CreatedBy);
                command.Parameters.AddWithValue("@DeficiencyCode", newWorkOrder.DeficiencyCode);
                command.Parameters.AddWithValue("@EpDetailId", newWorkOrder.EpDetailId);
                command.Parameters.AddWithValue("@IsDeficiency", newWorkOrder.IsDeficiency);
                command.Parameters.AddWithValue("@TimetoResolve", newWorkOrder.TimetoResolve);
                command.Parameters.AddWithValue("@ZoomLevel", newWorkOrder.ZoomLevel);
                command.Parameters.AddWithValue("@IssueId", newWorkOrder.IssueId);
                command.Parameters.AddWithValue("@Xcoordinate", newWorkOrder.Xcoordinate);
                command.Parameters.AddWithValue("@Ycoordinate", newWorkOrder.Ycoordinate);
                command.Parameters.AddWithValue("@FloorId", newWorkOrder.FloorId);
                command.Parameters.AddWithValue("@Data", newWorkOrder.Data);
                command.Parameters.AddWithValue("@TilsmId", newWorkOrder.TilsmId);
                command.Parameters.AddWithValue("@ActivityType", newWorkOrder.ActivityType);
                command.Parameters.AddWithValue("@ParentIssueId", newWorkOrder.ParentIssueId);
                command.Parameters.AddWithValue("@SourceWoId", newWorkOrder.SourceWoId);
                command.Parameters.AddWithValue("@TargetDate", newWorkOrder.TargetDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public WorkOrder GetWorkOrderByWorkOrderNumber(string workOrderNumber)
        {
            var workOrders = new WorkOrder();
            const string sql = StoredProcedures.Get_Workorders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    //DataTable selectedTable = ds.Tables[0].AsEnumerable()
                    //         .Where(r => r.Field<string>("WorkOrderNumber") == workOrderNumber)
                    //         .CopyToDataTable();
                    string expression = "WorkOrderNumber ='" + workOrderNumber + "'";
                    DataRow[] selectedRows = ds.Tables[0].Select(expression);

                    if (selectedRows.Count() > 0)
                    {
                        foreach (DataRow item in selectedRows)
                        {
                            if (!string.IsNullOrEmpty(item["WorkOrderId"].ToString()))
                                workOrders.WorkOrderId = Convert.ToInt32(item["WorkOrderId"].ToString());
                            if (!string.IsNullOrEmpty(item["IssueId"].ToString()))
                                workOrders.IssueId = Convert.ToInt32(item["IssueId"].ToString());
                            if (!string.IsNullOrEmpty(item["WorkOrderNumber"].ToString()))
                                workOrders.WorkOrderNumber = item["WorkOrderNumber"].ToString();
                        }
                    }
                }

            }
            return workOrders;
        }

        public int SaveTRounWorkOrder(TRoundWorkOrders objTRoundWorkOrders)
        {
            int newId;
            const string sql = StoredProcedures.Save_TRounWorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", objTRoundWorkOrders.TRoundId);
                command.Parameters.AddWithValue("@FloorId", objTRoundWorkOrders.FloorId);
                command.Parameters.AddWithValue("@StepsId", objTRoundWorkOrders.StepsId);
                command.Parameters.AddWithValue("@IssueId", objTRoundWorkOrders.IssueId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newWorkOrder"></param>
        /// <returns></returns>
        public int UpdatetWorkOrder(WorkOrder newWorkOrder)
        {
            int issueId;
            const string sql = StoredProcedures.Update_WorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountCode", newWorkOrder.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newWorkOrder.AccountDescription);
                command.Parameters.AddWithValue("@ActionCode", newWorkOrder.ActionCode);
                command.Parameters.AddWithValue("@ActionDescription", newWorkOrder.ActionDescription);
                command.Parameters.AddWithValue("@AssetGroupNumber", newWorkOrder.AssetGroupNumber);
                command.Parameters.AddWithValue("@AssetNumber", newWorkOrder.AssetNumber);
                command.Parameters.AddWithValue("@BuildingCode", newWorkOrder.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newWorkOrder.BuildingName);
                command.Parameters.AddWithValue("@CauseCode", newWorkOrder.CauseCode);
                command.Parameters.AddWithValue("@CauseDescription", newWorkOrder.CauseDescription);
                command.Parameters.AddWithValue("@CompletionComments", newWorkOrder.CompletionComments);
                command.Parameters.AddWithValue("@DateAvailable", newWorkOrder.DateAvailable);
                command.Parameters.AddWithValue("@DateCompleted", newWorkOrder.DateCompleted);
                command.Parameters.AddWithValue("@DateCreated", newWorkOrder.DateCreated.HasValue ? newWorkOrder.DateCreated : DateTime.UtcNow);
                command.Parameters.AddWithValue("@DateNeeded", newWorkOrder.DateNeeded);
                command.Parameters.AddWithValue("@DateUpdated", newWorkOrder.DateUpdated);
                command.Parameters.AddWithValue("@Description", newWorkOrder.Description);
                command.Parameters.AddWithValue("@ItemCode", newWorkOrder.ItemCode);
                command.Parameters.AddWithValue("@ItemDescription", newWorkOrder.ItemDescription);
                command.Parameters.AddWithValue("@LocationCode", newWorkOrder.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newWorkOrder.LocationDescription);
                command.Parameters.AddWithValue("@MeterReading", newWorkOrder.MeterReading);
                command.Parameters.AddWithValue("@PriorityCode", newWorkOrder.PriorityCode);
                command.Parameters.AddWithValue("@PriorityDescription", newWorkOrder.PriorityDescription);
                command.Parameters.AddWithValue("@ProblemCode", newWorkOrder.ProblemCode);
                command.Parameters.AddWithValue("@ProblemDescription", newWorkOrder.ProblemDescription);
                command.Parameters.AddWithValue("@ReferenceNumber", newWorkOrder.ReferenceNumber);
                command.Parameters.AddWithValue("@RequesterComments", newWorkOrder.RequesterComments);
                command.Parameters.AddWithValue("@RequesterEmail", newWorkOrder.RequesterEmail);
                command.Parameters.AddWithValue("@RequesterName", newWorkOrder.RequesterName);
                command.Parameters.AddWithValue("@RequesterPager", newWorkOrder.RequesterPager);
                command.Parameters.AddWithValue("@RequesterPhone", newWorkOrder.RequesterPhone);
                command.Parameters.AddWithValue("@SegmentId", newWorkOrder.SegmentID);
                command.Parameters.AddWithValue("@ShopCode", newWorkOrder.ShopCode);
                command.Parameters.AddWithValue("@ShopDescription", newWorkOrder.ShopDescription);
                command.Parameters.AddWithValue("@SiteCode", newWorkOrder.SiteCode);
                command.Parameters.AddWithValue("@SiteName", newWorkOrder.SiteName);
                command.Parameters.AddWithValue("@SkillCode", newWorkOrder.SkillCode);
                command.Parameters.AddWithValue("@SkillDescription", newWorkOrder.SkillDescription);
                command.Parameters.AddWithValue("@StatusCode", newWorkOrder.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newWorkOrder.StatusDescription);
                command.Parameters.AddWithValue("@SubStatusCode", newWorkOrder.SubStatusCode);
                command.Parameters.AddWithValue("@SubStatusDescription", newWorkOrder.SubStatusDescription);
                command.Parameters.AddWithValue("@TotalCost", newWorkOrder.TotalCost);
                command.Parameters.AddWithValue("@TotalHours", newWorkOrder.TotalHours);
                command.Parameters.AddWithValue("@TotalMaterialCharges", newWorkOrder.TotalMaterialCharges);
                command.Parameters.AddWithValue("@TotalTimeCharges", newWorkOrder.TotalTimeCharges);
                command.Parameters.AddWithValue("@TypeCode", newWorkOrder.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newWorkOrder.TypeDescription);
                command.Parameters.AddWithValue("@WeekScheduled", newWorkOrder.WeekScheduled);
                command.Parameters.AddWithValue("@WorkOrderId", newWorkOrder.WorkOrderId);
                command.Parameters.AddWithValue("@WorkOrderNumber", newWorkOrder.WorkOrderNumber);
                command.Parameters.AddWithValue("@UpdatedBy", newWorkOrder.UpdatedBy);
                command.Parameters.AddWithValue("@IssueId", newWorkOrder.IssueId);
                command.Parameters.AddWithValue("@SourceWoId", newWorkOrder.SourceWoId);
                command.Parameters.AddWithValue("@TargetDate", newWorkOrder.TargetDate);
                issueId = newWorkOrder.IssueId;
                _sqlHelper.ExecuteNonQuery(command);
            }
            return issueId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public WorkOrder GetWorkOrder(int issueId)
        {
            var workOrder = new WorkOrder();
            const string sql = StoredProcedures.Get_Workorders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@issueId", issueId);
                var ds = _sqlHelper.GetDataSet(command);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            workOrder.AccountCode = row["AccountCode"].ToString();
                            workOrder.Description = row["Description"].ToString();
                            workOrder.IssueId = Convert.ToInt32(row["IssueId"].ToString());
                            workOrder.StatusCode = row["StatusCode"].ToString();
                            workOrder.SubStatusCode = row["SubStatusCode"].ToString();
                            workOrder.IsDeficiency = Convert.ToBoolean(row["IsDeficiency"].ToString());
                            workOrder.RequesterPhone = row["RequesterPhone"].ToString();
                            workOrder.RequesterComments = row["RequesterComments"].ToString();
                            workOrder.RequesterName = row["RequesterName"].ToString();
                            workOrder.RequesterEmail = row["RequesterEmail"].ToString();
                            workOrder.TypeCode = row["TypeCode"].ToString();
                            workOrder.Data = row["Data"].ToString();
                            workOrder.SkillCode = row["SkillCode"].ToString();
                            workOrder.ItemCode = row["ItemCode"].ToString();
                            workOrder.ActionCode = row["ActionCode"].ToString();
                            workOrder.DeficiencyCode = row["DeficiencyCode"].ToString();
                            workOrder.ProblemCode = row["ProblemCode"].ToString();
                            workOrder.BuildingCode = row["BuildingCode"].ToString();
                            workOrder.SiteCode = row["SiteCode"].ToString();
                            workOrder.PriorityCode = row["PriorityCode"].ToString();
                            workOrder.CauseCode = row["CauseCode"].ToString();
                            workOrder.CompletionComments = row["CompletionComments"].ToString();
                            workOrder.AssetNumber = row["AssetNumber"].ToString();
                            workOrder.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                            workOrder.CRxCode = (row["CRxCode"] != null && !string.IsNullOrEmpty(row["CRxCode"].ToString())) ? row["CRxCode"].ToString() : "";
                            workOrder.CreatedDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.CreatedDate);
                            if (!string.IsNullOrEmpty(row["SegmentId"].ToString()))
                                workOrder.SegmentID = Convert.ToInt32(row["SegmentId"].ToString());

                            if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                            {
                                workOrder.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());
                                workOrder.DateCompletedDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.DateCompleted);
                            }

                            if (!string.IsNullOrEmpty(row["TargetDate"].ToString()))
                            {
                                workOrder.TargetDate = Convert.ToDateTime(row["TargetDate"].ToString());
                                workOrder.TargetDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.TargetDate);
                            }

                            if (!string.IsNullOrEmpty(row["DateCreated"].ToString()))
                                workOrder.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());

                            if (!string.IsNullOrEmpty(row["UpdatedDate"].ToString()))
                                workOrder.UpdateDate = Convert.ToDateTime(row["UpdatedDate"].ToString());

                            if (!string.IsNullOrEmpty(row["DateUpdated"].ToString()))
                                workOrder.DateUpdated = Convert.ToDateTime(row["DateUpdated"].ToString());

                            if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                                workOrder.ActivityId = Guid.Parse(row["ActivityId"].ToString());

                            if (!string.IsNullOrEmpty(row["TargetDate"].ToString()))
                                workOrder.TargetDate = Convert.ToDateTime(row["TargetDate"].ToString());


                            if (!string.IsNullOrEmpty(row["WorkOrderId"].ToString()))
                                workOrder.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());


                            if (!string.IsNullOrEmpty(row["TilsmId"].ToString()))
                                workOrder.TilsmId = Convert.ToInt32(row["TilsmId"].ToString());


                            workOrder.WorkOrderNumber = row["WorkOrderNumber"].ToString();
                            workOrder.SourceWoId = row["SourceWoId"].ToString();
                            //if (!string.IsNullOrEmpty(row["WorkOrderNumber"].ToString()))
                            //    workOrder.WorkOrderNumber = Convert.ToInt32(row["WorkOrderNumber"].ToString());

                            workOrder.TimetoResolve = !string.IsNullOrEmpty(row["TimetoResolve"].ToString()) ? Convert.ToInt32(row["TimetoResolve"].ToString()) : 0;

                            if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                                workOrder.FloorId = Convert.ToInt32(row["FloorId"].ToString());

                            workOrder.Xcoordinate = !string.IsNullOrEmpty(row["Xcoordinate"].ToString()) ? row["Xcoordinate"].ToString() : "0";

                            workOrder.Ycoordinate = !string.IsNullOrEmpty(row["Ycoordinate"].ToString()) ? row["Ycoordinate"].ToString() : "0";

                            workOrder.ZoomLevel = !string.IsNullOrEmpty(row["ZoomLevel"].ToString()) ? row["ZoomLevel"].ToString() : "0";

                            if (!string.IsNullOrEmpty(row["DeficiencyTime"].ToString()))
                                workOrder.DeficiencyTime = Convert.ToDateTime(row["DeficiencyTime"].ToString());

                            if (workOrder.FloorId.HasValue)
                                workOrder.Floor = _floorRepository.GetFloorDescription(workOrder.FloorId.Value);

                        }

                    var files = _sqlHelper.ConvertDataTable<WorkOrderfiles>(ds.Tables[3]);

                    // set ILSM 
                    var tIlsm = _sqlHelper.ConvertDataTable<TIlsm>(ds.Tables[4]);
                    if (workOrder.TilsmId.HasValue && tIlsm.Count > 0)
                        workOrder.Ilsm = tIlsm.FirstOrDefault(x => x.TIlsmId == workOrder.TilsmId.Value); ;

                    if (workOrder.IssueId > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            if (!string.IsNullOrEmpty(row["UserId"].ToString()))
                            {
                                var user = _usersRepository.GetUsersList(Convert.ToInt32(row["UserId"]));
                                if (user != null && user.UserId > 0)
                                    workOrder.Resources.Add(user);
                            }
                        }

                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            var floorAssets = _floorAssetRepository.GetFloorAssetDescription(Convert.ToInt32(row["FloorAssetId"]));
                            if (floorAssets != null)
                            {
                                workOrder.FloorAssets.Add(floorAssets);
                            }
                        }
                        workOrder.WorkOrderFiles = files.Where(x => x.IssueId == workOrder.IssueId).ToList();
                    }
                }
            }
            return workOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WorkOrder> GetWorkOrders()
        {
            var workOrders = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_Workorders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                ConvertToWorkOrders(workOrders, ds);

            }
            return workOrders;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newlists"></param>
        /// <param name="ds"></param>
        private void ConvertToWorkOrders(ICollection<WorkOrder> newlists, DataSet ds)
        {
            if (ds != null)
            {
                // List<WorkOrderfiles> files = _sqlHelper.ConvertDataTable<WorkOrderfiles>(ds.Tables[3]);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var workOrder = new WorkOrder
                    {
                        IssueId = Convert.ToInt32(row["IssueId"].ToString()),
                        AccountCode = row["AccountCode"].ToString(),
                        Description = row["Description"].ToString()
                    };
                    workOrder.IssueId = Convert.ToInt32(row["IssueId"].ToString());
                    workOrder.StatusCode = row["StatusCode"].ToString();
                    workOrder.SubStatusCode = row["SubStatusCode"].ToString();
                    workOrder.IsDeficiency = Convert.ToBoolean(row["IsDeficiency"].ToString());
                    workOrder.RequesterPhone = row["RequesterPhone"].ToString();
                    workOrder.RequesterComments = row["RequesterComments"].ToString();
                    workOrder.RequesterName = row["RequesterName"].ToString();
                    workOrder.RequesterEmail = row["RequesterEmail"].ToString();
                    workOrder.TypeCode = row["TypeCode"].ToString();
                    workOrder.Data = row["Data"].ToString();
                    workOrder.SkillCode = row["SkillCode"].ToString();
                    workOrder.ItemCode = row["ItemCode"].ToString();
                    workOrder.ActionCode = row["ActionCode"].ToString();
                    workOrder.DeficiencyCode = row["DeficiencyCode"].ToString();
                    workOrder.ProblemCode = row["ProblemCode"].ToString();
                    workOrder.BuildingCode = row["BuildingCode"].ToString();
                    workOrder.SiteCode = row["SiteCode"].ToString();
                    workOrder.PriorityCode = row["PriorityCode"].ToString();
                    workOrder.CauseCode = row["CauseCode"].ToString();
                    workOrder.CompletionComments = row["CompletionComments"].ToString();
                    workOrder.AssetNumber = row["AssetNumber"].ToString();
                    workOrder.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                    workOrder.CreatedDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.CreatedDate);
                    if (!string.IsNullOrEmpty(row["SegmentId"].ToString()))
                        workOrder.SegmentID = Convert.ToInt32(row["SegmentId"].ToString());

                    if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                        workOrder.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());

                    if (!string.IsNullOrEmpty(row["DateCreated"].ToString()))
                        workOrder.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());

                    if (!string.IsNullOrEmpty(row["UpdatedDate"].ToString()))
                        workOrder.UpdateDate = Convert.ToDateTime(row["UpdatedDate"].ToString());

                    if (!string.IsNullOrEmpty(row["DateUpdated"].ToString()))
                        workOrder.DateUpdated = Convert.ToDateTime(row["DateUpdated"].ToString());

                    if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                        workOrder.ActivityId = Guid.Parse(row["ActivityId"].ToString());


                    if (!string.IsNullOrEmpty(row["WorkOrderId"].ToString()))
                        workOrder.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());

                    workOrder.WorkOrderNumber = row["WorkOrderNumber"].ToString();

                    workOrder.TimetoResolve = !string.IsNullOrEmpty(row["TimetoResolve"].ToString()) ? Convert.ToInt32(row["TimetoResolve"].ToString()) : 0;

                    if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        workOrder.FloorId = Convert.ToInt32(row["FloorId"].ToString());

                    workOrder.Xcoordinate = !string.IsNullOrEmpty(row["Xcoordinate"].ToString()) ? row["Xcoordinate"].ToString() : "0";
                    workOrder.Ycoordinate = !string.IsNullOrEmpty(row["Ycoordinate"].ToString()) ? row["Ycoordinate"].ToString() : "0";
                    workOrder.ZoomLevel = !string.IsNullOrEmpty(row["ZoomLevel"].ToString()) ? row["ZoomLevel"].ToString() : "0";

                    if (!string.IsNullOrEmpty(row["DeficiencyTime"].ToString()))
                        workOrder.DeficiencyTime = Convert.ToDateTime(row["DeficiencyTime"].ToString());

                    if (workOrder.IssueId > 0)
                    {
                        workOrder.FloorAssets = new List<TFloorAssets>();
                        var expression = "IssueId = '" + workOrder.IssueId + "'";
                        const string sortOrder = "Name ASC";
                        var foundRows = ds.Tables[2].Select(expression, sortOrder);
                        foreach (var t in foundRows)
                        {
                            var floorAssets = new TFloorAssets
                            {
                                SerialNo = Convert.ToString(t["SerialNo"]),
                                DeviceNo = Convert.ToString(t["DeviceNo"]),
                                Name = Convert.ToString(t["Name"]),
                                FloorAssetsId = Convert.ToInt32(t["FloorAssetId"])
                            };
                            workOrder.FloorAssets.Add(floorAssets);
                        }
                    }
                    newlists.Add(workOrder);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetWorkOrdersPaging(Request objRequest, int? userId)
        {
            var pageCount = 0;
            var totalResult = 0;

            var newLists = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_Workorders_Paging_Proc;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mode", objRequest.Mode);
                command.Parameters.AddWithValue("@pageIndex", objRequest.PageIndex);
                command.Parameters.AddWithValue("@pageSize", objRequest.PageSize);
                command.Parameters.AddWithValue("@sortOrders", objRequest.SortOrder);
                command.Parameters.AddWithValue("@orderbySort", objRequest.OrderbySort);
                command.Parameters.AddWithValue("@searchcBy", objRequest.SearchcBy);
                command.Parameters.AddWithValue("@floorAssetId", objRequest.FloorAssetId);
                command.Parameters.AddWithValue("@activityId", objRequest.ActivityId == "" ? null : objRequest.ActivityId);
                command.Parameters.AddWithValue("@whereCondition", objRequest.WhereCondition == "" ? null : objRequest.WhereCondition);
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    //pageCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PageCount"].ToString());
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        pageCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalResult"].ToString());
                        totalResult = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalResult"].ToString());
                    }

                    var activity = _inspectionActivityRepository.ConvertToInspectionActivities(ds.Tables[5]); //_sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[5]);
                    var users = _usersRepository.ConvertToUser(ds.Tables[5]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[5]);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        var workOrder = new WorkOrder
                        {
                            IssueId = Convert.ToInt32(row["IssueId"].ToString()),
                            AccountCode = row["AccountCode"].ToString(),
                            Description = row["Description"].ToString()
                        };
                        workOrder.IssueId = Convert.ToInt32(row["IssueId"].ToString());
                        workOrder.StatusCode = row["StatusCode"].ToString();
                        workOrder.SubStatusCode = row["SubStatusCode"].ToString();
                        workOrder.IsDeficiency = Convert.ToBoolean(row["IsDeficiency"].ToString());
                        workOrder.RequesterPhone = row["RequesterPhone"].ToString();
                        workOrder.RequesterComments = row["RequesterComments"].ToString();
                        workOrder.RequesterName = row["RequesterName"].ToString();
                        workOrder.RequesterEmail = row["RequesterEmail"].ToString();
                        workOrder.TypeCode = row["TypeCode"].ToString();
                        workOrder.Data = row["Data"].ToString();
                        workOrder.SkillCode = row["SkillCode"].ToString();
                        workOrder.ItemCode = row["ItemCode"].ToString();
                        workOrder.ActionCode = row["ActionCode"].ToString();
                        workOrder.DeficiencyCode = row["DeficiencyCode"].ToString();
                        workOrder.ProblemCode = row["ProblemCode"].ToString();
                        workOrder.BuildingCode = row["BuildingCode"].ToString();
                        workOrder.SiteCode = row["SiteCode"].ToString();
                        workOrder.CRxCode = row["CrxCode"]?.ToString();
                        workOrder.PriorityCode = row["PriorityCode"].ToString();
                        if (!string.IsNullOrEmpty(row["CreatedBy"].ToString()))
                            workOrder.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                        workOrder.CauseCode = row["CauseCode"].ToString();
                        workOrder.CompletionComments = row["CompletionComments"].ToString();
                        workOrder.AssetNumber = row["AssetNumber"].ToString();
                        workOrder.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        workOrder.CreatedDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.CreatedDate);
                        if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                        {
                            workOrder.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());
                            workOrder.DateCompletedDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.DateCompleted);
                        }

                        if (!string.IsNullOrEmpty(row["TargetDate"].ToString()))
                        {
                            workOrder.TargetDate = Convert.ToDateTime(row["TargetDate"].ToString());
                            workOrder.TargetDateTimeSpan = Conversion.ConvertToTimeSpan(workOrder.TargetDate);
                        }

                        if (!string.IsNullOrEmpty(row["DateCreated"].ToString()))
                            workOrder.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());

                        if (!string.IsNullOrEmpty(row["UpdatedDate"].ToString()))
                            workOrder.UpdateDate = Convert.ToDateTime(row["UpdatedDate"].ToString());

                        if (!string.IsNullOrEmpty(row["DateUpdated"].ToString()))
                            workOrder.DateUpdated = Convert.ToDateTime(row["DateUpdated"].ToString());

                        if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
                            workOrder.ActivityId = Guid.Parse(row["ActivityId"].ToString());


                        if (!string.IsNullOrEmpty(row["WorkOrderId"].ToString()))
                            workOrder.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());

                        workOrder.WorkOrderNumber = row["WorkOrderNumber"].ToString();

                        workOrder.SourceWoId = row["SourceWoId"].ToString();

                        workOrder.TimetoResolve = !string.IsNullOrEmpty(row["TimetoResolve"].ToString()) ? Convert.ToInt32(row["TimetoResolve"].ToString()) : 0;

                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                            workOrder.FloorId = Convert.ToInt32(row["FloorId"].ToString());

                        workOrder.Xcoordinate = !string.IsNullOrEmpty(row["Xcoordinate"].ToString()) ? row["Xcoordinate"].ToString() : "0";
                        workOrder.Ycoordinate = !string.IsNullOrEmpty(row["Ycoordinate"].ToString()) ? row["Ycoordinate"].ToString() : "0";
                        workOrder.ZoomLevel = !string.IsNullOrEmpty(row["ZoomLevel"].ToString()) ? row["ZoomLevel"].ToString() : "0";

                        if (!string.IsNullOrEmpty(row["DeficiencyTime"].ToString()))
                            workOrder.DeficiencyTime = Convert.ToDateTime(row["DeficiencyTime"].ToString());

                        if (workOrder.FloorId.HasValue)
                            workOrder.Floor = _floorRepository.GetFloorDescription(workOrder.FloorId.Value);

                        if (row.Table.Columns.Contains("TilsmId"))
                        {
                            if (!string.IsNullOrEmpty(row["TilsmId"].ToString()))
                                workOrder.TilsmId = Convert.ToInt32(row["TilsmId"].ToString());
                        }
                        if (row.Table.Columns.Contains("WOBuildingName"))
                        {
                            if (!string.IsNullOrEmpty(row["WOBuildingName"].ToString()))
                                workOrder.BuildingName = row["WOBuildingName"].ToString();
                        }
                        if (workOrder.IssueId > 0)
                        {
                            var expression = "IssueId = '" + workOrder.IssueId + "'";
                            const string sortOrder = "Name ASC";
                            workOrder.Resources = new List<UserProfile>();
                            var resourcesFoundRows = ds.Tables[2].Select(expression, sortOrder);
                            foreach (var rows in resourcesFoundRows)
                            {
                                var user = new UserProfile
                                {
                                    FirstName = Convert.ToString(rows["FirstName"]),
                                    LastName = Convert.ToString(rows["LastName"]),
                                    Email = Convert.ToString(rows["Email"]),
                                    UserId = Convert.ToInt32(rows["UserId"])
                                };
                                workOrder.Resources.Add(user);
                            }


                            workOrder.FloorAssets = new List<TFloorAssets>();


                            var foundRows = ds.Tables[3].Select(expression, sortOrder);
                            foreach (var dataRow in foundRows)
                            {
                                var floorAssets = new TFloorAssets
                                {
                                    SerialNo = Convert.ToString(dataRow["SerialNo"]),
                                    DeviceNo = Convert.ToString(dataRow["DeviceNo"]),
                                    Name = Convert.ToString(dataRow["Name"]),
                                    FloorAssetsId = Convert.ToInt32(dataRow["FloorAssetId"]),
                                    Floor = new Floor()
                                };
                                if (!string.IsNullOrEmpty(dataRow["FloorId"].ToString()) && !string.IsNullOrEmpty(dataRow["BuildingId"].ToString()))
                                {
                                    floorAssets.FloorId = Convert.ToInt32(dataRow["FloorId"]);
                                    floorAssets.Floor = new Floor
                                    {
                                        Building = new Buildings()
                                    };
                                    floorAssets.Floor.FloorName = dataRow["FloorName"].ToString();
                                    //floorAssets.FloorName = dataRow["FloorName"].ToString();
                                    floorAssets.Floor.BuildingId = Convert.ToInt32(dataRow["BuildingId"]);
                                    floorAssets.Floor.Building.BuildingName = dataRow["BuildingName"].ToString();
                                    floorAssets.Floor.Building.BuildingCode = dataRow["BuildingCode"].ToString();
                                    //FloorRepository.GetFloor(Convert.ToInt32(dataRow["FloorId"]));
                                }
                                // only for offline purpose required by sunil IOS Team
                                //floorAssets.TInspectionActivity = activity.Where(x => x.FloorAssetId.HasValue && x.FloorAssetId.Value == floorAssets.FloorAssetsId && x.IssueId.Value == workOrder.IssueId && x.IsCurrent == true).ToList();
                                floorAssets.TInspectionActivity = activity.Where(x => x.FloorAssetId.HasValue && x.FloorAssetId.Value == floorAssets.FloorAssetsId && x.IssueId.Value == workOrder.IssueId).ToList();
                                foreach (var item in floorAssets.TInspectionActivity)
                                {
                                    item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                                }
                                workOrder.FloorAssets.Add(floorAssets);
                            }
                        }

                        newLists.Add(workOrder);
                    }
                }
            }
            _objHttpResponseMessage = new HttpResponseMessage
            {
                Result = new Result { WorkOrders = newLists },
                PageCount = pageCount,
                TotalResult = totalResult

            };
            return _objHttpResponseMessage;
        }

        private List<RoundIssueWorkOrderCategory> ConvertToRoundIssueWorkOrderCategory(DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
                dataTable = dataTable.AsEnumerable().Distinct(DataRowComparer.Default).CopyToDataTable();

            var list = new List<RoundIssueWorkOrderCategory>();
            foreach (DataRow item in dataTable.Rows)
            {
                
                    var roundCat = new RoundIssueWorkOrderCategory
                    {
                        RoundCatId = !string.IsNullOrEmpty(item["RoundCatId"].ToString()) ? Convert.ToInt32(item["RoundCatId"].ToString()) : 0,
                        IssueId = !string.IsNullOrEmpty(item["IssueId"].ToString()) ? Convert.ToInt32(item["IssueId"].ToString()) : 0,
                        RouQuesId = !string.IsNullOrEmpty(item["RouQuesId"].ToString()) ? Convert.ToInt32(item["RouQuesId"].ToString()) : 0,
                        CategoryName = item["CategoryName"].ToString(),
                        RoundGroupId = !string.IsNullOrEmpty(item["RoundGroupId"].ToString()) ? Convert.ToInt32(item["RoundGroupId"].ToString()) : 0,
                        RoundGroupName = item["RoundGroupName"].ToString(),
                    };
                list.Add(roundCat);               
            }
            return list.ToList();
        }


        /// <summary>
        /// get work order by according to deficiencyType 22 September  2017
        /// </summary>
        /// <returns></returns>
        public List<WorkOrder> GetRoundWorkOrders(int? buildingId = null, int? floorId = null)
        {
            var newlists = new List<WorkOrder>();
            string sql = StoredProcedures.Get_RoundWorkOrders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@buildingId", buildingId.HasValue && buildingId > 0 ? buildingId : null);
                command.Parameters.AddWithValue("@floorId", floorId.HasValue && floorId > 0 ? floorId : null);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    // var tilsm = IlsmRepository.Get().Where(x => x.FloorId == floorId).ToList();
                    var users = _usersRepository.ConvertToUser(ds.Tables[1]);
                    var workOrderRoundCategory = ConvertToRoundIssueWorkOrderCategory(ds.Tables[3]);


                    List<TaggedResource> taggedResources = new List<TaggedResource>();
                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        TaggedResource tagged = new TaggedResource();
                        tagged.TaggedId = Guid.Parse(row["TaggedId"].ToString());
                        tagged.Email = row["Email"].ToString();
                        if (!string.IsNullOrEmpty(row["IssueId"].ToString()))
                            tagged.IssueId = Convert.ToInt32(row["IssueId"].ToString());

                        taggedResources.Add(tagged);
                    }


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var wo = new WorkOrder
                        {
                            IssueId = Convert.ToInt32(row["IssueId"].ToString()),
                            TimetoResolve = Convert.ToInt32(row["TimetoResolve"].ToString()),
                            StatusCode = row["StatusCode"].ToString(),
                            IsDeficiency = Convert.ToBoolean(row["IsDeficiency"].ToString()),
                            DeficiencyCode = row["DeficiencyCode"].ToString(),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                            WorkOrderNumber = row["WorkOrderNumber"].ToString(),
                            CRxCode = row["CRxCode"].ToString()
                        };
                        //if (!string.IsNullOrEmpty(row["WorkOrderNumber"].ToString()))
                        //    wo.WorkOrderNumber = Convert.ToInt32(row["WorkOrderNumber"].ToString());
                        if (!string.IsNullOrEmpty(row["WorkOrderId"].ToString()))
                        {
                            wo.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());
                        }

                        if (!string.IsNullOrEmpty(row["CreatedDate"].ToString()))
                        {
                            wo.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        }

                        if (!string.IsNullOrEmpty(row["DateCompleted"].ToString()))
                        {
                            wo.DateCompleted = Convert.ToDateTime(row["DateCompleted"].ToString());
                        }

                        if (!string.IsNullOrEmpty(row["DateCreated"].ToString()))
                        {
                            wo.DateCreated = Convert.ToDateTime(row["DateCreated"].ToString());
                        }

                        wo.IsDeleted = Convert.ToBoolean(row["IsDeleted"].ToString());
                        wo.Description = row["Description"].ToString();

                        if (!string.IsNullOrEmpty(row["TilsmId"].ToString()))
                            wo.TilsmId = Convert.ToInt32(row["TilsmId"].ToString());

                        // if (wo.TilsmId.HasValue)
                        //     wo.ILSM = tilsm.FirstOrDefault(x => x.TIlsmId== wo.TilsmId.Value);

                        if (!string.IsNullOrEmpty(row["TRoundId"].ToString()))
                        {
                            wo.TRoundId = Convert.ToInt32(row["TRoundId"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["FloorId"].ToString()))
                        {
                            wo.FloorId = Convert.ToInt32(row["FloorId"].ToString());
                            wo.Floor = new Floor
                            {
                                FloorId = wo.FloorId.Value,
                                FloorName = row["FloorName"].ToString(),
                                ImagePath = row["ImagePath"].ToString(),
                                Building = new Buildings
                                {
                                    BuildingName = row["BuildingName"].ToString(),
                                    BuildingCode = row["BuildingCode"].ToString(),
                                    BuildingId = Convert.ToInt32(row["BuildingId"].ToString())
                                }
                            };
                        }
                        wo.RoundCategory = new RoundCategory();

                        if (!string.IsNullOrEmpty(row["RoundCatId"].ToString()))
                        {
                            wo.RoundCategory.RoundCatId = Convert.ToInt32(row["RoundCatId"].ToString());
                            wo.RoundCategory.CategoryName = row["CategoryName"].ToString();
                        }
                        wo.RoundIssueWorkOrderCategory = workOrderRoundCategory.Where(x => x.IssueId == wo.IssueId).ToList();
                        var taggedMaster = taggedResources.FirstOrDefault(x => x.IssueId == wo.IssueId);
                        if (taggedMaster != null)
                        {
                            wo.TaggedResource = taggedResources.Where(x => x.TaggedId == taggedMaster.TaggedId).ToList();
                        }

                        wo.UserProfile = new UserProfile();
                        wo.UserProfile = users.FirstOrDefault(x => x.UserId == wo.CreatedBy);
                        newlists.Add(wo);
                    }



                }
            }
            return newlists;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        private List<WorkOrder> GetWorkOrders(int? floorId)
        {
            var lists = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_RoundInspectionWO;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", floorId);
                var ds = _sqlHelper.GetDataSet(command);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var wo = new WorkOrder { IssueId = Convert.ToInt32(row["IssueId"].ToString()) };
                    if (wo != null)
                    {
                        wo = GetWorkOrder(wo.IssueId);
                        lists.Add(wo);
                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public List<WorkOrder> GetFloorWorkOrder(int floorId)
        {
            return GetWorkOrders(floorId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WorkOrder> GetFloorWorkOrder()
        {
            return GetWorkOrders(null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="stepsId"></param>
        /// <returns></returns>
        public List<WorkOrder> GetWorkOrder(Guid activityId, int stepsId)
        {
            var workOrders = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_GetStepWorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@activityId", activityId);
                command.Parameters.AddWithValue("@StepsId", stepsId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    workOrders = _sqlHelper.ConvertDataTable<WorkOrder>(dt);
            }
            return workOrders;
        }

        #endregion

        #region WokrOrder status
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SubStatus> GetSubStatus()
        {
            List<SubStatus> lists;
            const string sql = StoredProcedures.Get_SubStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<SubStatus>(ds.Tables[0]);
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SaveWorkOrderResource(int issueId, UserProfile user)
        {
            bool status;
            const string sql = StoredProcedures.Save_WorkOrderResource;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", issueId);
                command.Parameters.AddWithValue("@ResourceId", user.ResourceId);
                command.Parameters.AddWithValue("@UserId", user.UserId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public int SaveWorkOrderFiles(int issueId, WorkOrderfiles files)
        {
            int newId;
            const string sql = StoredProcedures.Insert_WorkOrderFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", files.IssueId);
                command.Parameters.AddWithValue("@FileName", files.FileName);
                command.Parameters.AddWithValue("@FilePath", files.FilePath);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="activityId"></param>
        /// <param name="inspectionSteps"></param>
        /// <returns></returns>
        public int SaveWorkOrderFailStep(int issueId, Guid activityId, InspectionSteps inspectionSteps)
        {
            int newId;
            const string sql = StoredProcedures.Insert_WorkOrderFailSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", issueId);
                command.Parameters.AddWithValue("@ActivityId", activityId);
                command.Parameters.AddWithValue("@StepsId", inspectionSteps.StepsId);
                command.Parameters.AddWithValue("@TInsStepsId", inspectionSteps.TInsStepsId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public List<TFloorAssets> GetIssuesFloorAssets(int issueId)
        {
            var floorAssets = new List<TFloorAssets>();
            const string sql = StoredProcedures.Get_IssuesFloorAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@issueId", issueId);
                var ds = _sqlHelper.GetDataSet(command);

                if (ds != null)
                {
                    var activity = _sqlHelper.ConvertDataTable<TInspectionActivity>(ds.Tables[1]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                    var workOrderFloorAssets = _sqlHelper.ConvertDataTable<WorkOrderFloorAssets>(ds.Tables[1]).Where(x => x.IssueId > 0).ToList();
                    foreach (var item in activity)
                    {
                        item.UserProfile = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                    }


                    var workOrders = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[2]);

                    foreach (var item in workOrderFloorAssets)
                        item.TinspectionActivity = activity.FirstOrDefault(x => x.ActivityId == item.ActivityId);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var floorAsset = _floorAssetRepository.BindFloorAsset(row);
                        floorAsset.TInspectionActivity = activity.Where(x => x.FloorAssetId != null && (x.FloorAssetId.Value == floorAsset.FloorAssetsId && x.IsCurrent)).ToList();
                        floorAsset.WorkOrders = workOrders;
                        foreach (var item in floorAsset.WorkOrders)
                        {
                            item.WorkOrderFloorAssets = workOrderFloorAssets.Where(x => x.FloorAssetId == floorAsset.FloorAssetsId).ToList();
                        }
                        floorAssets.Add(floorAsset);
                    }
                }
            }
            return floorAssets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="userIds"></param>
        /// <param name="floorAssetsIds"></param>
        /// <param name="tinsStepsId"></param>
        /// <param name="activityIds"></param>
        /// <param name="tmsAssetsIds"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        public bool SaveWorkOrderResources(int issueId, string userIds, string floorAssetsIds, string tinsStepsId, string activityIds, string tmsAssetsIds, string resourceIds)
        {
            bool status;
            const string sql = StoredProcedures.Save_WorkOrderAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", issueId);
                command.Parameters.AddWithValue("@userIds", string.IsNullOrEmpty(userIds) ? null : userIds);
                command.Parameters.AddWithValue("@floorAssetsIds", string.IsNullOrEmpty(floorAssetsIds) ? null : floorAssetsIds);
                command.Parameters.AddWithValue("@tinsStepsId", string.IsNullOrEmpty(tinsStepsId) ? null : tinsStepsId);
                command.Parameters.AddWithValue("@tmsAssetsIds", string.IsNullOrEmpty(tmsAssetsIds) ? null : tmsAssetsIds);
                command.Parameters.AddWithValue("@ResourceIds", string.IsNullOrEmpty(resourceIds) ? null : resourceIds);
                command.Parameters.AddWithValue("@ActivityIds", string.IsNullOrEmpty(activityIds) ? null : activityIds);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region Off line work order tms update

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WorkOrder> QueueWorkOrders()
        {
            var newLists = new List<WorkOrder>();
            const string sql = StoredProcedures.Get_QueueWorkOrders;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                ConvertToWorkOrders(newLists, ds);
            }
            return newLists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueId"></param>
        /// <param name="woId"></param>
        /// <param name="woNumber"></param>
        /// <param name="sourceWoId"></param>
        /// <returns></returns>
        public bool UpdateWorkOrderId(int issueId, int woId, int woNumber, string sourceWoId = null)
        {
            bool status;
            const string sql = StoredProcedures.Update_WorkOrderId;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", issueId);
                command.Parameters.AddWithValue("@WorkOrderId", woId);
                command.Parameters.AddWithValue("@WorkOrderNumber", woNumber);
                command.Parameters.AddWithValue("@SourceWoId", sourceWoId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        #endregion

        public int GetWorkOrderCount(string dbName, string date)
        {
            var count = 0;
            const string sql = StoredProcedures.Get_WoCountByDbName;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DbName", dbName);
                command.Parameters.AddWithValue("@date", date);
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null)
                    count = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return count;
        }

        public List<WorkOrder> LinkWorkorder(string assetno)
        {
            List<WorkOrder> lists;
            const string sql = StoredProcedures.Get_LinkWorkorder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", assetno);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[0]);
            }
            return lists;
        }

        public int UpdateTempWorkOrder(WorkOrder newWorkOrder)
        {
            int newId;
            const string sql = StoredProcedures.Update_TempWorkOrder;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IssueId", newWorkOrder.IssueId);
                command.Parameters.AddWithValue("@WorkOrderId", newWorkOrder.WorkOrderId);
                command.Parameters.AddWithValue("@WorkOrderNumber", newWorkOrder.WorkOrderNumber);
                command.Parameters.AddWithValue("@TempWorkOrderNumber", newWorkOrder.TempWorkOrderNumber);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


    }
}
