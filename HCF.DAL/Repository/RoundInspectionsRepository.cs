using HCF.BDO;
using HCF.Utility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using HCF.BDO.Enums;

namespace HCF.DAL
{
    public class RoundInspectionsRepository : IRoundInspectionsRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IRounds _roundRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IFloorRepository _buildingsRepository;
             
        public RoundInspectionsRepository(ISqlHelper sqlHelper, IRounds roundRepository, IUsersRepository usersRepository, IFloorRepository floorRepository)
        {
            _usersRepository = usersRepository;
            _sqlHelper = sqlHelper;
            _roundRepository = roundRepository;
            _buildingsRepository = floorRepository;
        }
        public int AddRoundInspection(WorkOrder newWorkOrder)
        {
            int newId;
            const string sql = StoredProcedures.Insert_RoundInspection;
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
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int UpdateRoundInspection(WorkOrder newWorkOrder)
        {
            int issueId;
            const string sql = StoredProcedures.Update_RoundInspections;  //Update_WorkOrder;
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
                issueId = newWorkOrder.IssueId;
                _sqlHelper.ExecuteNonQuery(command);
            }
            return issueId;
        }

        #region RoundGroup

        public int Save(RoundGroup roundUser)
        {
            int newId;
            const string sql = StoredProcedures.Insert_RoundUserGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundUserGroupId", roundUser.RoundGroupId);
                command.Parameters.AddWithValue("@Name", roundUser.Name);
                command.Parameters.AddWithValue("@Description", roundUser.Description);
                command.Parameters.AddWithValue("@IsActive", roundUser.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", roundUser.CreatedBy);
                command.Parameters.AddWithValue("@IsRecurringRound", roundUser.IsRecurringRound);
                command.Parameters.AddWithValue("@RoundType", roundUser.RoundType);
                command.Parameters.AddWithValue("@StartDate", roundUser.StartDate);
                command.Parameters.AddWithValue("@EndDate", roundUser.EndDate);
                command.Parameters.AddWithValue("@Frequency", roundUser.Frequency);
                command.Parameters.AddWithValue("@Ocurrence", roundUser.Ocurrence);
                command.Parameters.AddWithValue("@ReoccurEvery", roundUser.ReoccurEvery);
                command.Parameters.AddWithValue("@Dayno", roundUser.Dayno);
                command.Parameters.AddWithValue("@Monthno", roundUser.Monthno);
                command.Parameters.AddWithValue("@Weekno", roundUser.Weekno);
                command.Parameters.AddWithValue("@Yearno", roundUser.Yearno);
                command.Parameters.AddWithValue("@The", roundUser.The);
                command.Parameters.AddWithValue("@RecurFor", roundUser.RecurFor);
                command.Parameters.AddWithValue("@Time", roundUser.Time);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public List<RoundGroup> GetRoundGroupList()
        {
            var rounds = new List<RoundGroup>();
            var sql = StoredProcedures.Get_RoundGroupList;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                    rounds = _sqlHelper.ConvertDataTable<RoundGroup>(dt.Tables[0]);
            }
            return rounds;
        }


        public List<RoundGroup> GetRoundUserGroup()
        {
            var roundUserGroup = new List<RoundGroup>();
            var sql = StoredProcedures.Get_RoundUserGroup;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    roundUserGroup = _sqlHelper.ConvertDataTable<RoundGroup>(dt.Tables[0]);
                    var roundCategories = _sqlHelper.ConvertDataTable<RoundCategory>(dt.Tables[1]);
                    var roundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(dt.Tables[2]);
                    var roundSchedules = _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[3]);
                    var roundGroupLocations = _sqlHelper.ConvertDataTable<RoundGroupLocations>(dt.Tables[6]);
                    foreach (var item in roundUserGroup)
                    {
                        item.RoundGroupUsers = roundGroupUsers.Where(x => x.RoundGroupId == item.RoundGroupId).ToList();
                        item.RoundGroupLocations = roundGroupLocations.Where(x => x.RoundGroupId == item.RoundGroupId).ToList();
                        item.RoundSchedules = roundSchedules.Where(x => x.RoundGroupId == item.RoundGroupId).ToList();
                        var allroundCategories = string.Join(",", item.RoundGroupUsers.Where(x => !string.IsNullOrEmpty(x.RoundCategories)).Select(x => x.RoundCategories));
                        if (!string.IsNullOrEmpty(allroundCategories))
                        {
                            int[] roundTypes = allroundCategories.Split(',').Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => int.Parse(x)).Distinct().ToArray();
                            if (roundTypes.Length > 0)
                                item.RoundCategory = roundCategories.Where(x => roundTypes.Contains(x.RoundCatId)).ToList();
                        }
                    }
                }
            }
            return roundUserGroup;
        }


        public int InsertOrUpdateGroup(RoundGroupUsers roundGroupUsers)
        {
            int newId;
            const string sql = StoredProcedures.Insert_RoundGroupUserDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", roundGroupUsers.UserId);
                command.Parameters.AddWithValue("@RoundGroupID", roundGroupUsers.RoundGroupId);
                command.Parameters.AddWithValue("@Status", roundGroupUsers.IsActive);
                if (roundGroupUsers.RoundCategories != "")
                    command.Parameters.AddWithValue("@RoundCategories", roundGroupUsers.RoundCategories);
                else
                    command.Parameters.AddWithValue("@RoundCategories", ""); // if remove all the categories                 
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int InsertOrUpdateTRoundUsers(TRoundUsers roundGroupUsers)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TRoundGroupUser;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", roundGroupUsers.RoundUserId);
                command.Parameters.AddWithValue("@TRoundId", roundGroupUsers.TRoundId);
                command.Parameters.AddWithValue("@Status", roundGroupUsers.IsActive);
                command.Parameters.AddWithValue("@AddedBy", roundGroupUsers.AddedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int RemoveReplaceVolunteer(RoundVolunteer roundGroupUsers)
        {
            int newId;
            const string sql = StoredProcedures.Update_roundVolunteer;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", roundGroupUsers.Userid);
                command.Parameters.AddWithValue("@RoundGroupID", roundGroupUsers.RoundGroupId);
                command.Parameters.AddWithValue("@Status", roundGroupUsers.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public int InsertOrUpdateGroupUserType(RoundSchedules roundGroup, string dates)
        {
            int newId;
            const string sql = StoredProcedures.Insert_GroupUserType;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundCatID", roundGroup.RoundCatId);
                command.Parameters.AddWithValue("@RoundGroupID", roundGroup.RoundGroupId);
                command.Parameters.AddWithValue("@FrequencyId", roundGroup.FrequencyId);
                command.Parameters.AddWithValue("@StartInitialDate", roundGroup.StartInitialDate);
                command.Parameters.AddWithValue("@dates", dates);
                command.Parameters.AddWithValue("@EndDate", roundGroup.EndDate);
                command.Parameters.AddWithValue("@Occurence", roundGroup.Occurence);
                command.Parameters.AddWithValue("@ReoccurEvery", roundGroup.@ReoccurEvery);
                command.Parameters.AddWithValue("@Dayno", roundGroup.Dayno);
                command.Parameters.AddWithValue("@Monthno", roundGroup.Monthno);
                command.Parameters.AddWithValue("@Weekno", roundGroup.Weekno);
                command.Parameters.AddWithValue("@Yearno", roundGroup.Yearno);
                command.Parameters.AddWithValue("@The", roundGroup.The);
                command.Parameters.AddWithValue("@RecurFor", roundGroup.RecurFor);
                command.Parameters.AddWithValue("@Categories", roundGroup.Categories);
                command.Parameters.AddWithValue("@Time", roundGroup.Time);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

                if (newId == 0)
                {
                    //HCF.Utility.ErrorLog.LogMsg(roundGroup.StartInitialDate.ToString());
                    // HCF.Utility.ErrorLog.LogMsg(roundGroup.RoundGroupId.ToString());
                    // HCF.Utility.ErrorLog.LogMsg(dates);
                }
            }
            return newId;
        }

        //public  int SaveUserCategories(RoundGroupUsers roundGroup)
        //{
        //    int newId;
        //    const string sql = StoredProcedures.Insert_GroupUserType;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@RoundCatID", roundGroup.RoundCatId);
        //        command.Parameters.AddWithValue("@RoundGroupID", roundGroup.RoundGroupId);
        //        command.Parameters.AddWithValue("@FrequencyId", roundGroup.FrequencyId);
        //        command.Parameters.AddWithValue("@StartInitialDate", Convert.ToDateTime(roundGroup.StartInitialDate));
        //        //if (roundGroup.UserId != 0)
        //        //{
        //        //    command.Parameters.AddWithValue("@UserID", roundGroup.UserId);
        //        //}
        //        command.Parameters.Add("@newId", SqlDbType.Int);
        //        command.Parameters["@newId"].Direction = ParameterDirection.Output;
        //        newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
        //    }
        //    return newId;
        //}

        public List<RoundSchedules> GetTGroupRound(int id)
        {
            var troundGroups = new List<RoundSchedules>();
            var sql = StoredProcedures.Get_TRoundGroup;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                    troundGroups = (id != 0) ? _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[1]) : _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[0]);
                foreach (var group in troundGroups)
                {
                    var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(group.StartInitialDate)).ToString();
                    group.day = day.Substring(0, 3);
                }

            }
            return troundGroups;
        }
        public RoundGroup GetRoundUserGroup(int roundGroupId, string users, int? roundtype)
        {
            var roundGroups = new RoundGroup();
            var sql = StoredProcedures.Get_RoundUserGroup;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@roundGroupId", roundGroupId);
                cmd.Parameters.AddWithValue("@volunteers", users);
                cmd.Parameters.AddWithValue("@roundtype", roundtype.Value);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    roundGroups = _sqlHelper.ConvertDataTable<RoundGroup>(dt.Tables[0]).FirstOrDefault();
                    if (roundGroups != null)
                    {
                        roundGroups.RoundCategory = _sqlHelper.ConvertDataTable<RoundCategory>(dt.Tables[1]);
                        roundGroups.GroupUsers = _sqlHelper.ConvertDataTable<UserProfile>(dt.Tables[2]);
                        if (roundtype != null && roundtype.Value == 1)
                            roundGroups.GroupUsers = roundGroups.GroupUsers.Where(x => x.UserAdditionalRoleIds.Contains("1")).ToList();
                        roundGroups.RoundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(dt.Tables[2]);
                        roundGroups.RoundSchedules = _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[3]);
                        roundGroups.RoundInspections = _sqlHelper.ConvertDataTable<TRounds>(dt.Tables[4]);
                        //var roundScheduleDates = _sqlHelper.ConvertDataTable<RoundScheduleDates>(dt.Tables[5]);

                        var roundScheduleDates = ConvertDataTableToScheduleDates(dt.Tables[5]);

                        var roundVolunteer = _sqlHelper.ConvertDataTable<RoundVolunteer>(dt.Tables[7]);

                        foreach (var roundSchedules in roundGroups.RoundSchedules)
                        {
                            roundSchedules.RoundCategory =
                                roundGroups.RoundCategory.FirstOrDefault(x => x.RoundCatId == roundSchedules.RoundCatId);
                            roundSchedules.RoundScheduleDates =
                                roundScheduleDates.Where(x => x.RoundScheduleId == roundSchedules.TRoundGroupId).ToList();
                        }

                        foreach (var user in roundGroups.GroupUsers)
                        {
                            var Schedules = roundGroups.RoundSchedules.Where(x => x.RoundGroupId == roundGroupId).ToList();
                            var gusers = roundGroups.RoundGroupUsers.Where(x => x.UserId == user.UserId).ToList();
                            List<string> roundcategory = new List<string>();
                            foreach (var volunteercategory in gusers)
                            {
                                foreach (var cate in volunteercategory.RoundCategories.Split(','))
                                {
                                    if (!string.IsNullOrEmpty(cate)) { roundcategory.Add(cate); }
                                }
                            }
                            foreach (var sche in Schedules)
                            {
                                foreach (var cate in roundcategory)
                                {
                                    if (sche.RoundCatId.ToString() == cate) { user.ISExistsRoundSchedules = true; }
                                }
                            }

                        }

                        foreach (var user in roundGroups.RoundGroupUsers)
                        {
                            user.userProfile = roundGroups.GroupUsers.Where(x => x.UserId == user.UserId).FirstOrDefault();
                            user.roundVolunteers = roundVolunteer.Where(x => x.ReplacedFrom == user.UserId).ToList();
                            foreach (var replaceVoln in user.roundVolunteers)
                            {
                                replaceVoln.userProfile = _usersRepository.GetUsersList(replaceVoln.Userid);
                            }
                        }

                        // roundGroups.RoundInspections = _sqlHelper.ConvertDataTable<TRounds>(dt.Tables[4]);
                    }
                }
            }
            return roundGroups;
        }

        private static List<RoundScheduleDates> ConvertDataTableToScheduleDates(DataTable dt)
        {
            var roundScheduleDates = new List<RoundScheduleDates>();
            foreach (DataRow item in dt.Rows)
            {
                RoundScheduleDates dates = new RoundScheduleDates();
                dates.Id = Convert.ToInt32(item["Id"].ToString());
                dates.RoundDate = Convert.ToDateTime(item["RoundDate"].ToString());
                dates.RoundScheduleId = Convert.ToInt32(item["RoundScheduleId"].ToString());
                if (!string.IsNullOrEmpty(item["TRoundId"].ToString()))
                    dates.TRoundId = Convert.ToInt32(item["TRoundId"].ToString());
                roundScheduleDates.Add(dates);
            }

            return roundScheduleDates;
        }

        public RoundGroup GetTRoundUserGroup(int troundId)
        {
            var roundGroups = new RoundGroup();
            roundGroups.RoundGroupUsers = new List<RoundGroupUsers>();
            const string sql = StoredProcedures.Get_RoundsUserGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@troundId", troundId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        RoundGroupUsers roundGroupUsers = new RoundGroupUsers();
                        roundGroupUsers.UserId = Convert.ToInt32(item["UserId"].ToString());
                        //roundGroupUsers.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                        roundGroupUsers.RoundCategories = item["RoundCategories"].ToString();
                        roundGroupUsers.IsActive = Convert.ToBoolean(item["IsActive"].ToString());
                        roundGroupUsers.userProfile = new UserProfile(roundGroupUsers.UserId, item["FirstName"].ToString(),
                            item["LastName"].ToString(), item["Email"].ToString()
                            );
                        roundGroups.RoundGroupUsers.Add(roundGroupUsers);
                    }
                }
            }
            return roundGroups;
        }


        public List<RoundSchedules> GetTGroupRoundGroup(int id, string name)
        {
            var troundGroups = new List<RoundSchedules>();
            var sql = StoredProcedures.Get_TRoundGroup;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                    troundGroups = (name == "GroupName") ? _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[2]) : _sqlHelper.ConvertDataTable<RoundSchedules>(dt.Tables[3]);
            }
            return troundGroups;
        }

        public List<RoundGroup> GetUsers(int id)
        {
            var users = new List<RoundGroup>();
            const string sql = StoredProcedures.HCF_GetGroupUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    users = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[0]);
                }
            }
            return users;
        }


        public List<RoundGroup> GetRoundGroup(int id)
        {
            var rgroup = new List<RoundGroup>();
            const string sql = StoredProcedures.HCF_GetRGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    rgroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[0]);
                }
            }
            return rgroup;
        }


        public List<RoundGroup> GetUserRoundGroup(int userid)
        {
            var roundgroup = new List<RoundGroup>();
            const string sql = StoredProcedures.Get_UserRoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    roundgroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[0]);
                    var roundcategory = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[1]);
                    var roundvolunteer = _sqlHelper.ConvertDataTable<RoundVolunteer>(ds.Tables[2]);
                    var schedules = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[3]);
                    foreach (var rgroup in roundgroup)
                    {
                        if (!string.IsNullOrEmpty(rgroup.Roundcategories))
                        {
                            foreach (var id in rgroup.Roundcategories.Split(','))
                                rgroup.RoundCategory.Add(roundcategory.Where(x => x.RoundCatId == Convert.ToInt32(id)).FirstOrDefault());

                        }
                        rgroup.roundVolunteers = roundvolunteer;
                        rgroup.RoundSchedules = schedules.Where(x => x.RoundGroupId == rgroup.RoundGroupId).ToList();
                    }
                }
            }
            return roundgroup;
        }

        public RoundGroup GetUser_Volunteer_RoundGroup(int groupid, int userid)
        {
            var roundgroup = new RoundGroup();
            const string sql = StoredProcedures.Get_RoundVolunteer_UserDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundGroupId", groupid);
                command.Parameters.AddWithValue("@userid", userid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    roundgroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[0]).FirstOrDefault();
                    var categories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[1]);
                    roundgroup.RoundSchedules = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[2]);
                    roundgroup.roundVolunteers = _sqlHelper.ConvertDataTable<RoundVolunteer>(ds.Tables[3]);
                    var rgroups = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[4]);
                    roundgroup.roundVolunteersAssigned = _sqlHelper.ConvertDataTable<RoundVolunteer>(ds.Tables[5]);
                    roundgroup.RoundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(ds.Tables[6]);

                    var users = roundgroup.RoundGroupUsers.Where(x => x.UserId == userid && x.RoundGroupId == groupid).ToList();
                    if (users.Count > 0)
                    {
                        List<string> roundcategory = new List<string>();
                        foreach (var user in users)
                        {
                            foreach (var category in user.RoundCategories.Split(','))
                            {
                                if (!string.IsNullOrEmpty(category))
                                {
                                    roundcategory.Add(category);
                                }
                            }
                        }
                        foreach (var cate in roundcategory)
                        {
                            var categ = categories.Where(x => x.RoundCatId == Convert.ToInt32(cate)).FirstOrDefault();
                            roundgroup.RoundCategory.Add(categ);
                        }
                    }

                    roundgroup.roundVolunteers = roundgroup.roundVolunteers.Where(x => x.ReplacementType != 3).ToList();
                    roundgroup.roundVolunteersAssigned = roundgroup.roundVolunteersAssigned.Where(x => x.ReplacementType != 3).ToList();
                    foreach (var volunteer in roundgroup.roundVolunteers)
                    {
                        List<int> roundcategories = new List<int>();
                        foreach (var category in volunteer.RoundCategories.Split(','))
                        {
                            roundcategories.Add(Convert.ToInt32(category));
                        }
                        foreach (var item in roundcategories)
                        {
                            var list = categories.Where(x => x.RoundCatId == item).FirstOrDefault();
                            volunteer.roundCategory.Add(list);
                        }
                    }

                    foreach (var vol in roundgroup.roundVolunteersAssigned)
                    {
                        List<int> roundcategories = new List<int>();
                        foreach (var category in vol.RoundCategories.Split(','))
                        {
                            roundcategories.Add(Convert.ToInt32(category));
                        }
                        foreach (var item in roundcategories)
                        {
                            var list = categories.Where(x => x.RoundCatId == item).FirstOrDefault();
                            vol.roundCategory.Add(list);
                        }
                        vol.roundGroups = rgroups.Where(x => x.RoundGroupId == vol.RoundGroupId).ToList();
                    }

                }
            }
            return roundgroup;
        }

        public RoundGroup GetRoundGroupDetails(int groupid)
        {
            var roundgroup = new RoundGroup();
            const string sql = StoredProcedures.Get_RoundVolunteer_UserDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundGroupId", groupid);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    roundgroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[0]).FirstOrDefault();
                    var categories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[1]);
                    roundgroup.RoundSchedules = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[2]);
                    roundgroup.roundVolunteers = _sqlHelper.ConvertDataTable<RoundVolunteer>(ds.Tables[3]);
                    var rgroups = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[4]);
                    roundgroup.RoundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(ds.Tables[6]);
                    roundgroup.RoundGroupLocations = _sqlHelper.ConvertDataTable<RoundGroupLocations>(ds.Tables[7]);
                    //roundgroup.GroupUsers = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[8]);
                    foreach (DataRow row in ds.Tables[8].Rows)
                    {
                        var user =
                            new UserProfile
                            {
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UserGuid = Guid.Parse(row["UserProfileId"].ToString()),
                                UserId = Convert.ToInt32(row["UserId"].ToString()),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString()),
                                Email = row["Email"].ToString(),
                                UserName = row["UserName"].ToString(),
                                FileName = row["FileName"].ToString(),
                                IsActive = Convert.ToBoolean(row["IsActive"].ToString()),
                                IsSystemUser = Convert.ToBoolean(row["IsSystemUser"].ToString()),
                                UserAdditionalRoleIds = Conversion.TryCastString(row["UserAdditionalRoleIds"].ToString()),
                            };
                        roundgroup.GroupUsers.Add(user);
                    }
                    roundgroup.GroupUsers = roundgroup.GroupUsers.Where(x => x.UserAdditionalRoleIds.Contains("1")).ToList();
                    var replacedUsers = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[9]);

                    foreach (var user in roundgroup.RoundGroupUsers)
                    {
                        user.userProfile = roundgroup.GroupUsers.Where(x => x.UserId == user.UserId).FirstOrDefault();
                    }
                    foreach (var replacedVol in roundgroup.roundVolunteers)
                    {
                        replacedVol.userProfile = replacedUsers.Where(x => x.UserId == replacedVol.Userid).FirstOrDefault();
                    }

                }
            }
            return roundgroup;
        }


        public List<ActivityData> GetNotifyDetails_RoundsDetails()
        {
            var activities = new List<ActivityData>();
            const string sql = StoredProcedures.GetNotifyDetails_Rounds;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    activities = _sqlHelper.ConvertDataTable<ActivityData>(ds.Tables[0]);
                    var rgroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[1]);
                    var rGroupusers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(ds.Tables[2]);
                    var userprofile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                    var roundcategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[3]);
                    var rgroupLocations = _sqlHelper.ConvertDataTable<RoundGroupLocations>(ds.Tables[4]);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[4]);
                    var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[4]);
                    var replaceVolunteers = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[5]);
                    var schdules = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[6]);
                    foreach (var floordetails in rgroupLocations)
                    {
                        floordetails.floors = floors.Where(x => x.FloorId == floordetails.FloorId).FirstOrDefault();
                    }
                    foreach (var activity in activities)
                    {
                        var userid = "";
                        var categories = "";
                        var rgroupName = "";
                        DateTime startDate, endDate;
                        foreach (var data in activity.Data.Split(';'))
                        {
                            if (data.Contains("UserID="))
                            {
                                userid = data.Replace("UserID=", "");
                                if (activity.Type == UserActivityType.TemporaryReplaced.ToString() || activity.Type == UserActivityType.PermanentlyReplaced.ToString())
                                {
                                    activity.user = replaceVolunteers.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
                                }
                                else
                                {
                                    activity.user = userprofile.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
                                }

                            }
                            if (data.Contains("CategoriesGroupId="))
                            {
                                categories = data.Replace("CategoriesGroupId=", "");
                                var rcategory = roundcategories.Where(x => x.RoundCatId == Convert.ToInt32(categories)).ToList().Count > 0 ? roundcategories.Where(x => x.RoundCatId == Convert.ToInt32(categories)).FirstOrDefault().CategoryName : string.Empty;
                                activity.RoundCatId = Convert.ToInt32(categories);
                                activity.roundcategories = rcategory;
                            }
                            if (data.Contains("RoundGroupId="))
                            {
                                rgroupName = data.Replace("RoundGroupId=", "");
                                if (rgroup.Any(x => x.RoundGroupId == Convert.ToInt32(rgroupName)))
                                {
                                    activity.rGroupName = rgroup.Where(x => x.RoundGroupId == Convert.ToInt32(rgroupName)).FirstOrDefault().Name;
                                    activity.roundGroupId = rgroup.Where(x => x.RoundGroupId == Convert.ToInt32(rgroupName)).FirstOrDefault().RoundGroupId;
                                }
                            }
                            if (data.Contains("StartDate="))
                            {
                                startDate = Convert.ToDateTime(data.ToString().Replace("StartDate=", "").Replace(" 12:00:00 AM", ""));
                                activity.StartDate = startDate.ToString("dd/MM/yyyy");
                            }
                            if (data.Contains("EndDate="))
                            {
                                endDate = Convert.ToDateTime(data.ToString().Replace("EndDate=", ""));
                                activity.EndDate = endDate.ToString("dd/MM/yyyy");
                            }
                        }
                        if (activity.Type == UserActivityType.Added.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was " + activity.Type + " to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        if (activity.Type == UserActivityType.Removed.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was " + activity.Type + " from " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        if (activity.Type == UserActivityType.TemporaryReplaced.ToString())
                        {
                            if (activity.StartDate == activity.EndDate)
                            {
                                activity.DataLine = activity.user.FullName + " was temporily replaced to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group for next round only " + activity.StartDate;
                            }
                            else
                                activity.DataLine = activity.user.FullName + " was temporily replaced to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group from " + activity.StartDate + " to " + activity.EndDate;
                        }
                        if (activity.Type == UserActivityType.PermanentlyReplaced.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was permanently replaced to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        activity.frequency = schdules.Where(x => x.RoundGroupId == activity.roundGroupId && x.RoundCatId == activity.RoundCatId).FirstOrDefault().FrequencyId.ToString();

                    }
                }
            }
            return activities;
        }

        #endregion

        #region RoundGroupLocations

        public int SaveRoundGroupLocations(RoundGroupLocations objroundGroupLocations)
        {
            int newId;
            const string sql = StoredProcedures.Insert_RoundGroupLocations;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundGroupId", objroundGroupLocations.RoundGroupId);
                command.Parameters.AddWithValue("@BuildingId", objroundGroupLocations.BuildingId);
                command.Parameters.AddWithValue("@FloorId", objroundGroupLocations.FloorId);
                command.Parameters.AddWithValue("@CreatedBy", objroundGroupLocations.CreatedBy);
                command.Parameters.AddWithValue("@IsActive", objroundGroupLocations.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveTRoundLocations(TRoundLocations objTRoundLocations)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TRoundLocations;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", objTRoundLocations.TRoundId);
                command.Parameters.AddWithValue("@FloorId", objTRoundLocations.FloorId);
                command.Parameters.AddWithValue("@AddedBy", objTRoundLocations.AddedBy);
                command.Parameters.AddWithValue("@IsActive", objTRoundLocations.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<Buildings> GetRoundGroupLocations(int roundGroupId, int troundId)
        {
            // var groupLocation = new List<RoundGroupLocations>();
            var buildings = new List<Buildings>();
            const string sql = StoredProcedures.Get_RoundGroupLocations;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundGroupId", roundGroupId);
                command.Parameters.AddWithValue("@troundId", troundId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[0]);
                    buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]).GroupBy(test => test.BuildingId).Select(grp => grp.First()).ToList();
                    foreach (var item in buildings)
                    {
                        item.Floor = new List<Floor>();
                        item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                    }
                }
            }
            return buildings;
        }

        public List<RoundScheduleDates> GetRoundScheduleDates(int? locationGroupId, int? userId, DateTime startDate, DateTime endDate)
        {
            var sql = StoredProcedures.Get_RoundByDate;
            var roundScheduleDate = new List<RoundScheduleDates>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoundDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@locationGroupId", (locationGroupId != null && locationGroupId.Value > 0) ? locationGroupId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@userId", (userId != null && userId.Value > 0) ? userId.Value : DBNull.Value);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Tables[0].Rows)
                    {
                        var cate = new RoundScheduleDates
                        {
                            RoundDate = Convert.ToDateTime(item["RoundDate"].ToString()),
                            RoundScheduleId = Convert.ToInt32(item["TRoundGroupId"].ToString())
                        };
                        roundScheduleDate.Add(cate);
                    }
                }
                return roundScheduleDate;
            }
        }

        //var roundScheduleDate = new List<RoundScheduleDates>();
        ////var roundScheduleDates = new List<RoundScheduleDates>();
        ////var roundGroupUsers = new List<RoundGroupUsers>();
        //const string sql = StoredProcedures.Get_RoundCountonCalender;
        //using (var command = new SqlCommand(sql))
        //{
        //    command.CommandType = CommandType.StoredProcedure;
        //    if (userId != 0)
        //        command.Parameters.AddWithValue("@userId", userId);

        //    if (locationGroupId != 0)
        //        command.Parameters.AddWithValue("@roundGroup", locationGroupId);

        //    command.Parameters.AddWithValue("@startDate", startDate);
        //    command.Parameters.AddWithValue("@endDate", endDate);
        //    var ds = _sqlHelper.GetDataSet(command);

        //    if (ds != null)
        //    {
        //        roundScheduleDate = _sqlHelper.ConvertDataTable<RoundScheduleDates>(ds.Tables[1]);
        //        //roundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(ds.Tables[2]);
        //        //roundScheduleDates = roundScheduleDate;


        //        //var individuallist = roundScheduleDate;
        //        // roundScheduleDates = individuallist.Where(x => x.RoundType != 2).ToList();
        //        //var grouplist = roundScheduleDate.Where(x => x.RoundType == 2).GroupBy(g => new { g.RoundDate, g.RoundGroupId }).Select(g => g.First()); ;
        //        //List<RoundScheduleDates> grouplist = (List<RoundScheduleDates>)roundScheduleDate.Where(x => x.RoundType == 2).GroupBy(g => new { g.RoundGroupId }).Select(g => g.First());
        //        //roundScheduleDates.AddRange(grouplist.ToList());
        //    }
        //}
        //return roundScheduleDate;


        public List<RoundGroup> GetRoundByDate(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp, DateTime endDate)
        {            
            var roundGroups = new List<RoundGroup>();
            var sql = StoredProcedures.Get_RoundByDate;          
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoundDate", roundDate);
                cmd.Parameters.AddWithValue("@locationGroupId", (locationGroupId != null && locationGroupId.Value > 0) ? locationGroupId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@userId", (userId != null && userId.Value > 0) ? userId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@endDate",endDate);
                var dt = _sqlHelper.GetDataSet(cmd);
                
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {

                    var floors = _buildingsRepository.GetFloorsList();
                    var roundCategory = new List<RoundCategory>();
                    foreach (DataRow item in dt.Tables[1].Rows)
                    {
                        RoundCategory cate = new RoundCategory();
                        cate.CategoryName = item["CategoryName"].ToString();
                        cate.RoundCatId = Convert.ToInt32(item["RoundCatId"].ToString());
                        roundCategory.Add(cate);
                    }

                    var roundLocations = new List<RoundGroupLocations>();
                    foreach (DataRow item in dt.Tables[3].Rows)
                    {
                        var floorIds = item["FloorIds"].ToString().Split(',');
                        foreach (var floorId in floorIds)
                        {
                            if (!string.IsNullOrEmpty(floorId))
                            {
                                var roundGroupLocations = new RoundGroupLocations();
                                roundGroupLocations.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                                roundGroupLocations.FloorId = Convert.ToInt32(floorId);
                                roundGroupLocations.floors = floors.FirstOrDefault(x => x.FloorId == roundGroupLocations.FloorId);
                                roundLocations.Add(roundGroupLocations);
                            }
                        }
                    }

                    var roundGroupUsers = new List<RoundGroupUsers>();
                    foreach (DataRow item in dt.Tables[2].Rows)
                    {
                        RoundGroupUsers cate = new RoundGroupUsers();

                        cate.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                        cate.UserId = Convert.ToInt32(item["UserId"].ToString());
                        cate.RoundCatId = Convert.ToInt32(item["RoundCatId"].ToString());
                        cate.userProfile = new UserProfile(cate.UserId, item["FirstName"].ToString()
                            , item["LastName"].ToString(), item["Email"].ToString());
                        roundGroupUsers.Add(cate);
                    }



                    List<RoundSchedules> roundSchedules = new List<RoundSchedules>();
                    foreach (DataRow item in dt.Tables[0].Rows)
                    {
                        RoundSchedules roundGroup = new RoundSchedules();
                        roundGroup.RoundCategories = new List<RoundCategory>();
                        roundGroup.RoundScheduleDates = new List<RoundScheduleDates>();                      

                        if (!string.IsNullOrEmpty(item["Categories"].ToString()))
                        {
                            roundGroup.Categories = item["Categories"].ToString();
                            string[] catArray = roundGroup.Categories.Split(',');
                            foreach (var cat in catArray)
                            {

                                var cate = roundCategory.FirstOrDefault(x => x.RoundCatId == Convert.ToInt32(cat));
                                if (cate != null)
                                    roundGroup.RoundCategories.Add(cate);
                            }
                        }

                        

                        roundGroup.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                        roundGroup.TRoundGroupId = Convert.ToInt32(item["TRoundGroupId"].ToString());

                        roundGroup.RoundGroup = new RoundGroup();
                        roundGroup.RoundGroup.RoundGroupId = roundGroup.RoundGroupId;
                        roundGroup.RoundGroup.Name = item["Name"].ToString();
                        roundGroup.RoundGroup.RoundType = Convert.ToInt32(item["RoundType"].ToString());
                        roundGroup.Names = item["Names"].ToString();
                        roundGroup.RoundGroupNames = new List<string>();
                        foreach (var name in roundGroup.Names.Split(','))
                        {
                            if (!string.IsNullOrEmpty(name))
                                roundGroup.RoundGroupNames.Add(name);
                        }

                        roundGroup.RoundType = roundGroup.RoundGroup.RoundType;

                  
                        roundGroup.RoundGroupUsers = SetRoundGroupUses(roundGroup.Categories, roundGroup.RoundGroup.RoundGroupId, roundGroupUsers, roundGroup.RoundGroup.RoundType);

                        RoundScheduleDates dates = new RoundScheduleDates();

                        if (!string.IsNullOrEmpty(item["TRoundId"].ToString()))
                        {
                            dates.TRoundId = Convert.ToInt32(item["TRoundId"].ToString());
                            dates.Tround = new TRounds();
                            dates.Tround.TRoundId = dates.TRoundId.Value;
                            if (!string.IsNullOrEmpty(item["Status"].ToString()))
                            {
                                dates.Tround.Status = Convert.ToInt32(item["Status"].ToString());
                                dates.Status = dates.Tround.Status;
                            }
                            if (!string.IsNullOrEmpty(item["IsClosed"].ToString()))
                                dates.Tround.IsClosed = Convert.ToBoolean(item["IsClosed"].ToString());

                        }
                        if (!string.IsNullOrEmpty(item["Id"].ToString()))
                        {
                            dates.Id = Convert.ToInt32(item["Id"].ToString());
                            dates.RoundDate = Convert.ToDateTime(item["RoundDate"].ToString());
                        }

                        roundGroup.RoundScheduleDates.Add(dates);

                        dates.Locations = roundLocations.Where(x => x.RoundGroupId == roundGroup.RoundGroup.RoundGroupId).ToList();
                        roundSchedules.Add(roundGroup);
                    }

                    roundGroups = roundSchedules.GroupBy(x => x.RoundGroupId).Select(x => x.FirstOrDefault().RoundGroup).ToList();
                    foreach (var item in roundGroups)
                    {
                        item.RoundSchedules = roundSchedules.Where(x => x.RoundGroupId == item.RoundGroupId).ToList();                       
                        item.RoundGroupUsers = roundGroupUsers.Where(x => x.RoundGroupId == item.RoundGroupId).ToList();
                        item.GroupUsers = item.RoundGroupUsers.Select(x => x.userProfile).ToList();
                    } 
                }
                return roundGroups;
            }
        }

        private List<RoundGroupUsers> SetRoundGroupUses(string ategories, int groupRoundId, List<RoundGroupUsers> lists, int roundType)
        {
            string[] arrays = ategories.Split(',');
            List<RoundGroupUsers> users = new List<RoundGroupUsers>();
            if (roundType == 1)
                users = lists.Where(x => arrays.Contains(Convert.ToString(x.RoundCatId)) && x.RoundGroupId == groupRoundId).ToList();
            else
                users = lists.Where(x => x.RoundGroupId == groupRoundId).ToList();
            return users;
        }

        public List<RoundGroup> GetRoundByDateApp(DateTime roundDate, int? locationGroupId, int? userId, bool? RequestFromApp, int? day, DateTime endDate)
        {
            return GetRoundByDate(roundDate, locationGroupId, userId, false, endDate);
            ////var roundGroup = new List<RoundGroup>();
            //var roundGroups = new List<RoundGroup>();
            //var sql = string.Empty;
            //if (RequestFromApp.HasValue && RequestFromApp.Value)
            //    sql = StoredProcedures.Get_RoundByDate_App;
            //else
            //    sql = StoredProcedures.Get_RoundByDate;
            //var IsRoundVolunteer = false;
            //var IsInspectionDone = false;
            //bool showgroupround = false;
            //using (var cmd = new SqlCommand())
            //{
            //    cmd.CommandText = sql;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@RoundDate", roundDate);
            //    if (locationGroupId != 0)
            //    {
            //        cmd.Parameters.AddWithValue("@locationGroupId", locationGroupId);
            //    }
            //    if (userId != 0)
            //    {
            //        if (userId == -1)
            //        {
            //            userId = 0;
            //            cmd.Parameters.AddWithValue("@day", day);
            //        }
            //        cmd.Parameters.AddWithValue("@userId", userId);
            //    }
            //    if (userId != 0)
            //    {
            //        cmd.Parameters.AddWithValue("@day", day);
            //    }
            //    var dt = _sqlHelper.GetDataSet(cmd);
            //    if (dt != null && dt.Tables[0].Rows.Count > 0)
            //    {
            //        if (userId != 0)
            //        {
            //            var userdetails = _usersRepository.GetUsersList(Convert.ToInt32(userId));
            //            if (!string.IsNullOrEmpty(userdetails.UserAdditionalRoleIds))
            //                IsRoundVolunteer = userdetails.UserAdditionalRoleIds.Contains("1");
            //        }
            //        //          DataTable tblIndividualRounds = dt.Tables[0].AsEnumerable()
            //        //.Where(row => row.Field<int>("RoundType") == 1)
            //        //.CopyToDataTable();

            //        DataTable dtrounds = dt.Tables[0];
            //        IEnumerable<DataRow> query = null;
            //        if (locationGroupId.HasValue && locationGroupId > 0)
            //        {
            //            query =
            //             from round in dtrounds.AsEnumerable()
            //             where round.Field<int>("RoundType") == 1 && round.Field<int>("RoundGroupId") == locationGroupId
            //             select round;
            //        }
            //        else
            //        {
            //            query =
            //            from round in dtrounds.AsEnumerable()
            //            where round.Field<int>("RoundType") == 1
            //            select round;
            //        }


            //        if (query.Any())
            //        {


            //            DataTable tblIndividualRounds = query.CopyToDataTable<DataRow>();

            //            foreach (DataRow dataRow in tblIndividualRounds.Rows)
            //            {
            //                TRounds rounds = new TRounds();
            //                showgroupround = false;
            //                var roundGroup = new RoundGroup
            //                {
            //                    Name = dataRow["Name"].ToString(),
            //                    RoundGroupId = Convert.ToInt32(dataRow["RoundGroupId"].ToString()),
            //                    Names = dataRow["Names"].ToString(),
            //                    RoundCategory = new List<RoundCategory>(),
            //                    RoundType = Convert.ToInt32(dataRow["RoundType"]),
            //                    RoundDate = Convert.ToDateTime(dataRow["RoundDate"])
            //                };
            //                roundGroup.RoundGroupNames = new List<string>();
            //                foreach (var name in roundGroup.Names.Split(','))
            //                {
            //                    roundGroup.RoundGroupNames.Add(name);

            //                }

            //                var categories = new RoundCategory
            //                {
            //                    CategoryName = dataRow["CategoryName"].ToString(),
            //                    RoundCatId = Convert.ToInt32(dataRow["RoundCatId"].ToString())
            //                };
            //                roundGroup.RoundCategory.Add(categories);
            //                //roundGroups.Add(roundGroup);
            //                roundGroup.RoundSchedules = new List<RoundSchedules>();

            //                var tRoundGroup = new RoundSchedules
            //                {
            //                    RoundCatId = Convert.ToInt32(dataRow["RoundCatId"].ToString()),
            //                    TRoundGroupId = Convert.ToInt32(dataRow["TRoundGroupId"].ToString())
            //                };


            //                var roundScheduleDates = new RoundScheduleDates()
            //                {
            //                    RoundDate = Convert.ToDateTime(dataRow["RoundDate"].ToString()),
            //                    Id = Convert.ToInt32(dataRow["Id"].ToString()),
            //                    RoundScheduleId = Convert.ToInt32(dataRow["TRoundGroupId"].ToString())
            //                };
            //                if (!string.IsNullOrEmpty(dataRow["Status"].ToString()))
            //                    roundScheduleDates.Status = Convert.ToInt32(dataRow["Status"].ToString());

            //                if (!string.IsNullOrEmpty(dataRow["TRoundId"].ToString()))
            //                    roundScheduleDates.TRoundId = Convert.ToInt32(dataRow["TRoundId"].ToString());

            //                if (roundScheduleDates.TRoundId != null)
            //                    rounds = _roundRepository.GetRoundDetails(Convert.ToInt32(roundScheduleDates.TRoundId));

            //                if (rounds != null)
            //                {
            //                    foreach (var insp in rounds.Inspections.Where(x => x.UserId == userId))
            //                    {
            //                        insp.RoundCatId = rounds.RoundCatId.HasValue && rounds.RoundCatId > 0 ? rounds.RoundCatId.Value : 0;
            //                        roundGroup.Inspections.Add(insp);
            //                    }
            //                    //roundGroup.Inspections = rounds.Inspections;
            //                    if (roundGroup.Inspections.Count > 0)
            //                    {
            //                        IsInspectionDone = true;
            //                        roundGroup.InspectionDone = IsInspectionDone;
            //                    }
            //                }


            //                tRoundGroup.RoundScheduleDates = new List<RoundScheduleDates> { roundScheduleDates };
            //                // roundGroup.RoundSchedules.Add(tRoundGroup);

            //                roundGroup.RoundSchedules.Add(tRoundGroup);
            //                roundGroup.isRoundVolunteer = IsRoundVolunteer;
            //                var GroupUsers = new List<UserProfile>();
            //                if (roundGroups != null && roundGroups.Count > 0 && roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId && x.RoundCategory.FirstOrDefault().RoundCatId == roundGroup.RoundCategory.FirstOrDefault().RoundCatId).Any())
            //                {
            //                    GroupUsers = roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).FirstOrDefault().GroupUsers;
            //                    roundGroup.Locations = roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).FirstOrDefault().Locations;
            //                }
            //                else
            //                {
            //                    GroupUsers = GetUserByRoundCategory(roundGroup.RoundGroupId, categories.RoundCatId);
            //                    roundGroup.Locations = GetRoundGroupLocations(Convert.ToInt32(roundGroup.RoundGroupId),0);
            //                }

            //                foreach (var users in GroupUsers)
            //                {
            //                    if (userId > 0)
            //                    {
            //                        if (users.UserId == userId)
            //                        {
            //                            showgroupround = true;
            //                        }
            //                    }                               
            //                    if (roundGroup.Names.Contains(users.FullName))
            //                    {
            //                        roundGroup.GroupUsers.Add(users);
            //                    }
            //                }
            //                if (roundGroup.GroupUsers != null && roundGroup.GroupUsers.Count > 0)
            //                {
            //                    roundGroup.GroupUsers = roundGroup.GroupUsers.GroupBy(x => x.UserId).Select(x => x.FirstOrDefault()).ToList();
            //                }

            //                if (roundGroup.GroupUsers.Any(x => x.UserAdditionalRoleIds != null && x.UserAdditionalRoleIds.Contains("1")))
            //                    IsRoundVolunteer = true;
            //                roundGroup.isRoundVolunteer = IsRoundVolunteer;




            //                if (userId != 0)
            //                {
            //                    if (roundGroup.Inspections.Any(x => x.UserId == userId))
            //                    {
            //                        IsInspectionDone = true;
            //                        roundGroup.InspectionDone = IsInspectionDone;
            //                    }
            //                    if (userId > 0)
            //                    {
            //                        if (showgroupround)
            //                        {
            //                            roundGroups.Add(roundGroup);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        roundGroups.Add(roundGroup);
            //                    }
            //                    //if (RequestFromApp.HasValue && RequestFromApp.Value)
            //                    //{
            //                    //    if (showgroupround)
            //                    //    {
            //                    //        roundGroups.Add(roundGroup);
            //                    //    }
            //                    //}
            //                    //else
            //                    //{
            //                    //    roundGroups.Add(roundGroup);
            //                    //}

            //                }
            //                else
            //                    roundGroups.Add(roundGroup);
            //            }

            //        }

            //        //for group round

            //        //           DataTable tblGroupRounds = dt.Tables[0].AsEnumerable()
            //        //.Where(row => row.Field<int>("RoundType") == 2)
            //        //.GroupBy(r => new { Col1 = r["RoundGroupId"] })
            //        //.Select(g => g.OrderBy(r => r["RoundGroupId"]).First())
            //        //.CopyToDataTable();
            //        dtrounds = dt.Tables[0];

            //        if (locationGroupId.HasValue && locationGroupId > 0)
            //        {
            //            query =
            //             from round in dtrounds.AsEnumerable()
            //             where round.Field<int>("RoundType") == 2 && !string.IsNullOrEmpty(round.Field<string>("Names")) && round.Field<int>("RoundGroupId") == locationGroupId
            //             select round;
            //        }
            //        else
            //        {
            //            query =
            //           from round in dtrounds.AsEnumerable()
            //           where round.Field<int>("RoundType") == 2 && !string.IsNullOrEmpty(round.Field<string>("Names"))
            //           select round;
            //        }
            //        if (query.Any())
            //        {
            //            DataTable tblRounds = query.CopyToDataTable<DataRow>();
            //            //query = query.GroupBy(row => row.Field<int>("RoundGroupId")).Select(grp => grp.First());
            //            query = query.GroupBy(row => new { V = row.Field<int>("RoundGroupId"), DateTime = row.Field<DateTime>("RoundDate") }).Select(grp => grp.First());
            //            DataTable tblGroupRounds = query.CopyToDataTable<DataRow>();
            //            foreach (DataRow dataRow in tblGroupRounds.Rows)
            //            {
            //                showgroupround = false;
            //                TRounds rounds = new TRounds();
            //                var roundGroup = new RoundGroup
            //                {
            //                    Name = dataRow["Name"].ToString(),
            //                    RoundGroupId = Convert.ToInt32(dataRow["RoundGroupId"].ToString()),
            //                    Names = dataRow["Names"].ToString(),
            //                    RoundCategory = new List<RoundCategory>(),
            //                    RoundType = Convert.ToInt32(dataRow["RoundType"]),
            //                    RoundDate = Convert.ToDateTime(dataRow["RoundDate"])
            //                };
            //                roundGroup.RoundGroupNames = new List<string>();
            //                foreach (var name in roundGroup.Names.Split(','))
            //                {
            //                    roundGroup.RoundGroupNames.Add(name);
            //                }

            //                var GroupUsers = new List<UserProfile>();
            //                if (roundGroups != null && roundGroups.Count > 0 && roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).Any())
            //                {
            //                    roundGroup.RoundCategory = roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).FirstOrDefault().RoundCategory;
            //                    roundGroup.GroupUsers = roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).FirstOrDefault().GroupUsers;
            //                    roundGroup.Locations = roundGroups.Where(x => x.RoundGroupId == roundGroup.RoundGroupId).FirstOrDefault().Locations;
            //                    if (roundGroup != null && roundGroup.GroupUsers != null && roundGroup.GroupUsers.Count > 0)
            //                    {
            //                        if (roundGroup.GroupUsers.Where(x => x.UserId == userId).Any())
            //                        {
            //                            showgroupround = true;
            //                        }
            //                    }

            //                }
            //                else
            //                {
            //                    DataTable tblcategory = dt.Tables[0].AsEnumerable()
            //.Where(row => row.Field<int>("RoundGroupId") == Convert.ToInt32(dataRow["RoundGroupId"].ToString()) && row.Field<DateTime>("RoundDate") == Convert.ToDateTime(dataRow["RoundDate"].ToString()))
            //.CopyToDataTable();
            //                    foreach (DataRow dataRow1 in tblcategory.Rows)
            //                    {
            //                        var categories = new RoundCategory
            //                        {
            //                            CategoryName = dataRow1["CategoryName"].ToString(),
            //                            RoundCatId = Convert.ToInt32(dataRow1["RoundCatId"].ToString()),
            //                            IsActive = Convert.ToBoolean(dataRow1["CatStatus"].ToString())
            //                        };
            //                        if (categories != null && categories.RoundCatId > 0 && categories.IsActive)
            //                        {
            //                            roundGroup.RoundCategory.Add(categories);
            //                        }
            //                    }

            //                    foreach (var cat in roundGroup.RoundCategory.GroupBy(x => x.RoundCatId).Select(x => x.First()).ToList())
            //                    {

            //                        GroupUsers = GetUserByRoundCategory(roundGroup.RoundGroupId, cat.RoundCatId);
            //                        roundGroup.Locations = GetRoundGroupLocations(Convert.ToInt32(roundGroup.RoundGroupId),0);

            //                        foreach (var users in GroupUsers)
            //                        {
            //                            if (userId > 0)
            //                            {
            //                                if (users.UserId == userId)
            //                                {
            //                                    showgroupround = true;
            //                                }
            //                            }
            //                            if (roundGroup.Names.Contains(users.FullName))
            //                            {
            //                                roundGroup.GroupUsers.Add(users);
            //                            }
            //                        }
            //                    }
            //                }


            //                //roundGroups.Add(roundGroup);
            //                roundGroup.RoundSchedules = new List<RoundSchedules>();
            //                foreach (DataRow rowval in tblRounds.Rows)
            //                {
            //                    if ((Convert.ToInt32(dataRow["RoundGroupId"].ToString()) == Convert.ToInt32(rowval["RoundGroupId"].ToString())) && (Convert.ToDateTime(dataRow["RoundDate"].ToString()) == Convert.ToDateTime(rowval["RoundDate"].ToString())))
            //                    {
            //                        var tRoundGroup = new RoundSchedules
            //                        {
            //                            RoundCatId = Convert.ToInt32(rowval["RoundCatId"].ToString()),
            //                            TRoundGroupId = Convert.ToInt32(rowval["TRoundGroupId"].ToString())
            //                        };


            //                        var roundScheduleDates = new RoundScheduleDates()
            //                        {
            //                            RoundDate = Convert.ToDateTime(rowval["RoundDate"].ToString()),
            //                            Id = Convert.ToInt32(rowval["Id"].ToString()),
            //                            RoundScheduleId = Convert.ToInt32(rowval["TRoundGroupId"].ToString())
            //                        };
            //                        if (!string.IsNullOrEmpty(rowval["Status"].ToString()))
            //                            roundScheduleDates.Status = Convert.ToInt32(rowval["Status"].ToString());

            //                        if (!string.IsNullOrEmpty(rowval["TRoundId"].ToString()))
            //                            roundScheduleDates.TRoundId = Convert.ToInt32(rowval["TRoundId"].ToString());

            //                        if (roundScheduleDates.TRoundId != null)
            //                            rounds = _roundRepository.GetRoundDetails(Convert.ToInt32(roundScheduleDates.TRoundId));

            //                        if (rounds != null)
            //                        {
            //                            //roundGroup.Inspections = rounds.Inspections;
            //                            foreach (var insp in rounds.Inspections.Where(x => x.UserId == userId))
            //                            {
            //                                insp.RoundCatId = rounds.RoundCatId.HasValue && rounds.RoundCatId > 0 ? rounds.RoundCatId.Value : 0;
            //                                roundGroup.Inspections.Add(insp);
            //                            }

            //                            if (roundGroup.Inspections.Count > 0)
            //                            {
            //                                IsInspectionDone = true;
            //                                roundGroup.InspectionDone = IsInspectionDone;
            //                            }
            //                        }




            //                        tRoundGroup.RoundScheduleDates = new List<RoundScheduleDates> { roundScheduleDates };
            //                        // roundGroup.RoundSchedules.Add(tRoundGroup);

            //                        roundGroup.RoundSchedules.Add(tRoundGroup);
            //                    }
            //                }
            //                roundGroup.isRoundVolunteer = IsRoundVolunteer;
            //                if (roundGroup.GroupUsers != null && roundGroup.GroupUsers.Count > 0)
            //                {
            //                    roundGroup.GroupUsers = roundGroup.GroupUsers.GroupBy(x => x.UserId).Select(x => x.FirstOrDefault()).ToList();
            //                }


            //                if (roundGroup.GroupUsers.Any(x => x.UserAdditionalRoleIds != null && x.UserAdditionalRoleIds.Contains("1")))
            //                    IsRoundVolunteer = true;
            //                roundGroup.isRoundVolunteer = IsRoundVolunteer;


            //                //roundGroup.Locations = GetRoundGroupLocations(Convert.ToInt32(roundGroup.RoundGroupId));
            //                if (userId != 0)
            //                {
            //                    if (roundGroup.Inspections.Any(x => x.UserId == userId))
            //                    {
            //                        IsInspectionDone = true;
            //                        roundGroup.InspectionDone = IsInspectionDone;
            //                    }
            //                    if (userId > 0)
            //                    {
            //                        if (showgroupround)
            //                        {
            //                            roundGroups.Add(roundGroup);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        roundGroups.Add(roundGroup);
            //                    }
            //                }
            //                else
            //                    roundGroups.Add(roundGroup);

            //            }
            //        }


            //    }

            //    return roundGroups;
            //}

        }
        
        
        public List<ActivityData> GetRoundRecentChangesByDate(string roundDate, int? locationGroupId, int? userId)
        {
            var listActivity = new List<ActivityData>();
            var sql = StoredProcedures.Get_RoundRecentChangesByDate;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoundDate", roundDate);
                if (locationGroupId != 0)
                {
                    cmd.Parameters.AddWithValue("@locationGroupId", locationGroupId);
                }
                if (userId != 0)
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                }
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    listActivity = _sqlHelper.ConvertDataTable<ActivityData>(dt.Tables[0]);
                    var roundGroup = _sqlHelper.ConvertDataTable<RoundGroup>(dt.Tables[1]);
                    var roundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(dt.Tables[2]);
                    var userprofiles = _sqlHelper.ConvertDataTable<UserProfile>(dt.Tables[2]);
                    var roundcategories = _sqlHelper.ConvertDataTable<RoundCategory>(dt.Tables[3]);
                    var roundreplacedusers = _sqlHelper.ConvertDataTable<UserProfile>(dt.Tables[4]);
                    foreach (var activity in listActivity)
                    {
                        var userid = "";
                        var categories = "";
                        var rgroupName = "";
                        foreach (var data in activity.Data.Split(';'))
                        {
                            if (data.Contains("UserID="))
                            {
                                userid = data.Replace("UserID=", "");
                                if (activity.Type == UserActivityType.TemporaryReplaced.ToString() || activity.Type == UserActivityType.PermanentlyReplaced.ToString())
                                {
                                    activity.user = roundreplacedusers.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
                                }
                                else
                                {
                                    activity.user = userprofiles.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
                                }

                            }
                            if (data.Contains("CategoriesGroupId="))
                            {
                                categories = data.Replace("CategoriesGroupId=", "");
                                var rcategory = roundcategories.Where(x => x.RoundCatId == Convert.ToInt32(categories)).FirstOrDefault().CategoryName;
                                activity.RoundCatId = Convert.ToInt32(categories);
                                activity.roundcategories = rcategory;
                            }
                            if (data.Contains("RoundGroupId="))
                            {
                                rgroupName = data.Replace("RoundGroupId=", "");
                                if (roundGroup.Where(x => x.RoundGroupId == Convert.ToInt32(rgroupName)).FirstOrDefault() != null)
                                {
                                    activity.rGroupName = roundGroup.Where(x => x.RoundGroupId == Convert.ToInt32(rgroupName)).FirstOrDefault().Name;
                                    activity.roundGroupId = roundGroup.Where(x => x.RoundGroupId == Convert.ToInt32(rgroupName)).FirstOrDefault().RoundGroupId;
                                }
                            }

                        }
                        if (activity.Type == UserActivityType.Added.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was " + activity.Type + " to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        if (activity.Type == UserActivityType.Removed.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was " + activity.Type + " from " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        if (activity.Type == UserActivityType.TemporaryReplaced.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was temporily replaced to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                        if (activity.Type == UserActivityType.PermanentlyReplaced.ToString())
                        {
                            activity.DataLine = activity.user.FullName + " was permanently replaced to " + activity.roundcategories + " rounds with the " + activity.rGroupName + " group.";
                        }
                    }
                }

            }
            return listActivity;
        }

        private List<UserProfile> GetUserByRoundCategory(int roundGroupId, int roundCatId)
        {
            var userProfile = new List<UserProfile>();
            const string sql = StoredProcedures.Get_RoundUserCatGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundGroupId", roundGroupId);
                command.Parameters.AddWithValue("@RoundCatId", roundCatId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                }
            }
            return userProfile;
        }


        private List<UserProfile> GetVolunteerByRoundCategory(int roundGroupId, int roundCatId)
        {
            var userProfile = new List<UserProfile>();
            const string sql = StoredProcedures.Get_RoundVolunteerCatGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundGroupId", roundGroupId);
                command.Parameters.AddWithValue("@RoundCatId", roundCatId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    userProfile = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                }
            }
            return userProfile;
        }

        public RoundGroup GetRoundGroupUsers()
        {
            var Rgu = new RoundGroup();
            var sql = StoredProcedures.Get_RoundGroupUsers;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    Rgu.roundVolunteers = _sqlHelper.ConvertDataTable<RoundVolunteer>(dt.Tables[0]);
                    Rgu.RoundGroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(dt.Tables[1]);
                }
            }
            return Rgu;
        }

        public List<RoundGroup> GetRoundInspectionByDate(int userId)
        {
            var roundGroups = new List<RoundGroup>();
            var sql = StoredProcedures.Get_RoundInspectionByDate;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                var dt = _sqlHelper.GetDataSet(cmd);
                if (dt != null)
                {
                    foreach (DataRow dataRow in dt.Tables[0].Rows)
                    {
                        var roundGroup = new RoundGroup
                        {
                            Name = dataRow["Name"].ToString(),
                            RoundGroupId = Convert.ToInt32(dataRow["RoundGroupId"].ToString()),
                            RoundCategory = new List<RoundCategory>(),
                            RoundID = Convert.ToInt32(dataRow["Id"].ToString()),
                            RoundDate = Convert.ToDateTime(dataRow["RoundDate"].ToString())
                        };
                        var categories = new RoundCategory
                        {
                            CategoryName = dataRow["CategoryName"].ToString(),
                            RoundCatId = Convert.ToInt32(dataRow["RoundCatId"].ToString())
                        };
                        roundGroup.RoundCategory.Add(categories);
                        roundGroups.Add(roundGroup);
                        roundGroup.RoundSchedules = new List<RoundSchedules>();

                        var tRoundGroup = new RoundSchedules
                        {
                            RoundCatId = Convert.ToInt32(dataRow["RoundCatId"].ToString()),
                            TRoundGroupId = Convert.ToInt32(dataRow["TRoundGroupId"].ToString()),
                            RoundDate = Convert.ToDateTime(dataRow["RoundDate"].ToString()),
                            Id = Convert.ToInt32(dataRow["Id"].ToString())
                        };
                        if (!string.IsNullOrEmpty(dataRow["Status"].ToString()))
                            tRoundGroup.Status = Convert.ToInt32(dataRow["Status"].ToString());

                        if (!string.IsNullOrEmpty(dataRow["TRoundId"].ToString()))
                            tRoundGroup.TRoundId = Convert.ToInt32(dataRow["TRoundId"].ToString());


                        var roundScheduleDates = new RoundScheduleDates()
                        {
                            RoundDate = Convert.ToDateTime(dataRow["RoundDate"].ToString()),
                            Id = Convert.ToInt32(dataRow["Id"].ToString()),
                            IsActive = Convert.ToBoolean(dataRow["IsActive"].ToString()),
                            IsDone = Convert.ToBoolean(dataRow["IsDone"].ToString())
                        };
                        if (!string.IsNullOrEmpty(dataRow["Status"].ToString()))
                            roundScheduleDates.Status = Convert.ToInt32(dataRow["Status"].ToString());

                        if (!string.IsNullOrEmpty(dataRow["TRoundId"].ToString()))
                            roundScheduleDates.TRoundId = Convert.ToInt32(dataRow["TRoundId"].ToString());


                        tRoundGroup.RoundScheduleDates = new List<RoundScheduleDates>(); //EditableList<RoundScheduleDates> { roundScheduleDates };

                        roundGroup.RoundSchedules.Add(tRoundGroup);

                        roundGroup.GroupUsers = GetUserByRoundCategory(roundGroup.RoundGroupId, categories.RoundCatId);
                    }
                }
            }
            return roundGroups;
        }





        public bool UserReplacevolunteer(RoundVolunteer roundVolunteer)
        {
            bool value = false;
            var sql = StoredProcedures.User_ReplaceVolunteer;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", roundVolunteer.Userid);
                cmd.Parameters.AddWithValue("@roundGroup", roundVolunteer.RoundGroupId);
                cmd.Parameters.AddWithValue("@replacementType", roundVolunteer.ReplacementType);
                cmd.Parameters.AddWithValue("@startdate", roundVolunteer.StartDate);
                cmd.Parameters.AddWithValue("@enddate", roundVolunteer.EndDate);
                cmd.Parameters.AddWithValue("@replacedbyUser", roundVolunteer.RoundVolunteerID);
                cmd.Parameters.AddWithValue("@roundCategories", roundVolunteer.RoundCategories);
                cmd.Parameters.AddWithValue("@RoundSchedulesIds", roundVolunteer.RoundSchedulesIds);
                value = _sqlHelper.ExecuteNonQuery(cmd);
            }
            return value;
        }

        #endregion

        public List<RoundScheduleDates> GetRoundScheduleDatesIDS(int locationGroupId, int userId)
        {
            var roundScheduleDates = new List<RoundScheduleDates>();
            const string sql = StoredProcedures.Get_RoundUserGroups;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@roundGroup", locationGroupId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    roundScheduleDates = _sqlHelper.ConvertDataTable<RoundScheduleDates>(ds.Tables[1]);
                }
            }
            return roundScheduleDates;
        }
        public bool IsmailNotified(Guid Id, string Type)
        {
            bool value = false;
            var sql = StoredProcedures.Update_ActivityIsMailNotified;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@Type", Type);
                value = _sqlHelper.ExecuteNonQuery(cmd);
            }
            return value;
        }
      

        public List<RoundGroupUsers> roundUserGroups(int userId)
        {
            var roundgroupuser = new List<RoundGroupUsers>();
            const string sql = StoredProcedures.Get_RoundsUserGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        RoundGroupUsers roundGroupUsers = new RoundGroupUsers();
                        roundGroupUsers.UserId = Convert.ToInt32(item["UserId"].ToString());
                        roundGroupUsers.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                        roundGroupUsers.RoundCategories = item["RoundCategories"].ToString();
                        roundGroupUsers.IsActive = Convert.ToBoolean(item["IsActive"].ToString());
                        roundGroupUsers.userProfile = new UserProfile(roundGroupUsers.UserId, item["FirstName"].ToString(),
                            item["LastName"].ToString(), item["Email"].ToString()
                            );
                        roundgroupuser.Add(roundGroupUsers);
                    }
                }
            }
            return roundgroupuser;
        }

        public IEnumerable<RoundGroupUsers> GetRoundUserGroups(int roundGroupId)
        {
            var roundgroupuser = new List<RoundGroupUsers>();
            const string sql = StoredProcedures.Get_RoundsUserGroup;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundGroupId", roundGroupId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        RoundGroupUsers roundGroupUsers = new RoundGroupUsers();
                        roundGroupUsers.UserId = Convert.ToInt32(item["UserId"].ToString());
                        roundGroupUsers.RoundGroupId = Convert.ToInt32(item["RoundGroupId"].ToString());
                        roundGroupUsers.RoundCategories = item["RoundCategories"].ToString();
                        roundGroupUsers.IsActive = Convert.ToBoolean(item["IsActive"].ToString());
                        roundGroupUsers.userProfile = new UserProfile(roundGroupUsers.UserId, item["FirstName"].ToString(),
                            item["LastName"].ToString(), item["Email"].ToString()
                            );
                        roundgroupuser.Add(roundGroupUsers);
                    }
                }
            }
            return roundgroupuser;
        }


        public List<UserFloorRoundCategory> UserFloorRoundCategory(int userId, int floorId, int troundId)
        {
            var records = new List<UserFloorRoundCategory>();
            const string sql = StoredProcedures.Get_UserFloorRoundCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@troundId", troundId);
                var ds = _sqlHelper.GetDataTable(command);
                if (ds != null)
                {
                    foreach (DataRow item in ds.Rows)
                    {
                        UserFloorRoundCategory cat = new UserFloorRoundCategory();
                        cat.RoundCatId = Convert.ToInt32(item["RoundCatId"].ToString());
                        cat.IsActive = Convert.ToBoolean(item["IsActive"].ToString());
                        cat.IsAdditional = Convert.ToBoolean(item["IsAdditional"].ToString());
                        cat.CategoryName = item["CategoryName"].ToString();
                        records.Add(cat);
                    }
                }
            }
            return records;
        }
    }

}
