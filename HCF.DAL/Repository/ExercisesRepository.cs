using HCF.BDO;
using HCF.BDO.Enums;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace HCF.DAL
{
    public class ExercisesRepository : IExercisesRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IDigitalSignRepository _digitalSignRepository;
        private readonly IBuildingsRepository _buildingsRepository;
        private readonly Utility.IHCFSession _hCFSession;
        private readonly ILocationRepository _locationRepository;
        public ExercisesRepository(IHCFSession hCFSession, ISqlHelper sqlHelper, IDigitalSignRepository digitalSignRepository,
            IBuildingsRepository buildingsRepository, ILocationRepository locationRepository)
        {
            _hCFSession = hCFSession;
            _sqlHelper = sqlHelper;
            _buildingsRepository = buildingsRepository;
            _digitalSignRepository = digitalSignRepository;
            _locationRepository = locationRepository;
        }

        #region Exercises

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newExercises"></param>
        /// <returns></returns>
        public int CreateExercises(TExercises newExercises)
        {
            int newId;
            if (newExercises.Date == null)
            {
                newExercises.Date = Conversion.ConvertToDateTime(newExercises.DateTimeSpan);
            }
            newExercises.CritiqueDate = Conversion.ConvertToDateTime(newExercises.CritiqueDateTimeSpan);
            newExercises.EducationDate = Conversion.ConvertToDateTime(newExercises.EducationDateTimeSpan);
            const string sql = StoredProcedures.Insert_Exercises;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuarterId", newExercises.QuarterId);
                command.Parameters.AddWithValue("@Announced", newExercises.Announced);
                command.Parameters.AddWithValue("@CreatedBy", newExercises.CreatedBy);
                command.Parameters.AddWithValue("@Date", newExercises.Date);
                command.Parameters.AddWithValue("@Name", newExercises.Name);
                command.Parameters.AddWithValue("@Time", newExercises.Time);
                command.Parameters.AddWithValue("@Status", newExercises.Status);
                command.Parameters.AddWithValue("@ShiftId", newExercises.ShiftId);
                command.Parameters.AddWithValue("@BuildingId", newExercises.BuildingId);
                command.Parameters.AddWithValue("@SignIds", newExercises.SignIds);
                command.Parameters.AddWithValue("@DrillType", newExercises.DrillType);
                command.Parameters.AddWithValue("@Comment", newExercises.Comment);
                command.Parameters.AddWithValue("@NearBy", newExercises.NearBy);
                command.Parameters.AddWithValue("@Observers", newExercises.Observers);
                command.Parameters.AddWithValue("@ConductedBy", newExercises.ConductedBy);
                command.Parameters.AddWithValue("@CritiquesComment", newExercises.CritiquesComment);
                command.Parameters.AddWithValue("@EducationComment", newExercises.EducationComment);
                command.Parameters.AddWithValue("@CritiqueSignIds", newExercises.CritiqueSignIds);
                command.Parameters.AddWithValue("@EducationSignIds", newExercises.EducationSignIds);
                command.Parameters.AddWithValue("@CritiqueDate", newExercises.CritiqueDate);
                command.Parameters.AddWithValue("@EducationDate", newExercises.EducationDate);
                command.Parameters.AddWithValue("@IsAdditional", newExercises.IsAdditional);
                command.Parameters.AddWithValue("@year", newExercises.Year);
                command.Parameters.AddWithValue("@QuarterNo", newExercises.QuarterNo);
                command.Parameters.AddWithValue("@IsAudible", newExercises.IsAudible);
                command.Parameters.AddWithValue("@FiredrillDocStatus", newExercises.FiredrillDocStatus);
                command.Parameters.AddWithValue("@LocationGroupId", newExercises.LocationGroupId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        /// <summary>
        ///    A
        /// </summary>
        /// <returns></returns>
        public List<TExercises> GetExercises(int? exerciseId)
        {
            var lists = new List<TExercises>();
            const string sql = StoredProcedures.Get_Exercises;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (exerciseId != null)
                    command.Parameters.AddWithValue("@Exerciseid", Convert.ToInt32(exerciseId));
                var ds = _sqlHelper.GetDataSet(command);
                var files = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[1]);
                //var quarterMasters = _sqlHelper.ConvertDataTable<QuarterMaster>(ds.Tables[2]);
                var texerciseActions = _sqlHelper.ConvertDataTable<TExerciseActions>(ds.Tables[2]);
                var objShifts = _sqlHelper.ConvertDataTable<Shift>(ds.Tables[3]);
                var digitalSignature = _digitalSignRepository.GetDigitalSignature().ToList();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    var list = new TExercises
                    {
                        Announced = Convert.ToBoolean(row["Announced"].ToString()),
                        //BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
                        Date = Convert.ToDateTime(row["Date"].ToString()),
                        NearBy = row["NearBy"].ToString(),
                        Comment = row["Comment"].ToString(),
                        ConductedBy = row["ConductedBy"].ToString(),
                        Observers = row["Observers"].ToString(),
                        CritiquesComment = row["CritiquesComment"].ToString(),
                        EducationComment = row["EducationComment"].ToString(),
                        FiredrillDocStatus = Convert.ToInt32(row["FiredrillDocStatus"].ToString())
                    };
                    if (!string.IsNullOrEmpty(row["BuildingId"].ToString()))
                        list.BuildingId = Convert.ToInt32(row["BuildingId"].ToString());

                    if (!string.IsNullOrEmpty(row["LocationGroupId"].ToString()))
                        list.LocationGroupId = Convert.ToInt32(row["LocationGroupId"].ToString());

                    if (!string.IsNullOrEmpty(row["CritiqueDate"].ToString()))
                        list.CritiqueDate = Convert.ToDateTime(row["CritiqueDate"].ToString());
                    else
                        list.CritiqueDate = Convert.ToDateTime(row["Date"].ToString());
                    if (!string.IsNullOrEmpty(row["EducationDate"].ToString()))
                        list.EducationDate = Convert.ToDateTime(row["EducationDate"].ToString());
                    else
                        list.EducationDate = Convert.ToDateTime(row["Date"].ToString());
                    list.DateTimeSpan = Conversion.ConvertToTimeSpan(list.Date);
                    list.TExerciseId = Convert.ToInt32(row["TExerciseId"].ToString());
                    list.ShiftId = Convert.ToInt32(row["ShiftId"].ToString());
                    if (!string.IsNullOrEmpty(row["ShiftId"].ToString()))
                    {
                        list.Shift = new Shift();
                        list.Shift = objShifts.FirstOrDefault(x => x.ShiftId == list.ShiftId);
                    }

                    if (!string.IsNullOrEmpty(row["Time"].ToString()))
                    {
                        list.Time = TimeSpan.Parse(row["Time"].ToString());
                    }
                    list.SignIds = row["SignIds"].ToString();
                    list.CritiqueSignIds = row["CritiqueSignIds"].ToString();
                    list.EducationSignIds = row["EducationSignIds"].ToString();
                    list.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
                    if (!string.IsNullOrEmpty(row["Status"].ToString()))
                    {
                        list.Status = Convert.ToInt32(row["Status"].ToString());
                    }
                    if (!string.IsNullOrEmpty(row["IsAudible"].ToString()))
                    {
                        list.IsAudible = Convert.ToBoolean(row["IsAudible"].ToString());
                    }
                    list.Name = row["Name"].ToString();
                    list.Year = Convert.ToInt32(row["Year"].ToString());
                    list.QuarterId = Convert.ToInt32(row["QuarterId"].ToString());
                    list.DrillType = Convert.ToInt32(row["DrillType"].ToString());
                    if (!string.IsNullOrEmpty(row["QuarterNo"].ToString()))
                    {
                        list.QuarterNo = Convert.ToInt32(row["QuarterNo"].ToString());
                    }
                    //list.Comments = row["Comments"].ToString();
                    list.StartTime = list.Time.ToString();
                    if (list.Time.HasValue)
                    {
                        DateTime time = DateTime.Today.Add(list.Time.Value);
                        list.StartTime = time.ToString("hh:mm tt");
                        //var hours = list.Time.Value.Hours;
                        //var minutes = list.Time.Value.Minutes;
                        //list.StartTime = string.Format("{0}:{1}", hours, minutes);
                    }
                    if (!string.IsNullOrEmpty(row["IsAdditional"].ToString()))
                        list.IsAdditional = Convert.ToBoolean(row["IsAdditional"].ToString());
                    //list.QuarterMaster = quarterMasters.FirstOrDefault(x => x.QuarterId == list.QuarterId);
                    list.Building = new Buildings();
                    if (list.BuildingId != null && list.BuildingId > 0)
                        list.Building = _buildingsRepository.GetBuildings().FirstOrDefault(x => x.BuildingId == list.BuildingId);
                    else if (list.LocationGroupId != null && list.LocationGroupId > 0)
                    {
                        var locationGroups = _locationRepository.GetLocationGroup().FirstOrDefault(x => x.LocationGroupId == list.LocationGroupId);
                        list.Building = new Buildings()
                        {
                            BuildingName = locationGroups.Name,
                            LocationGroupId = locationGroups.LocationGroupId,
                            IsActive = locationGroups.IsActive
                        };
                    }

                    list.TExerciseFiles = files.Where(x => x.TExerciseId == list.TExerciseId).ToList();
                    list.TExerciseFiles = list.TExerciseFiles.Count > 0 ? list.TExerciseFiles : new List<TExerciseFiles>();
                    list.TExerciseQuestionnaires = GetTExerciseQuestionnaires(list.TExerciseId).ToList();
                    //  list.DigitalSignature = DigitalSignRepository.GetDigitalSignature().Where(x => list.SignIds.Contains(x.DigSignatureId.ToString())).ToList();
                    if (!string.IsNullOrEmpty(list.SignIds))
                    {
                        string[] SignIDS = list.SignIds.Split(',');
                        list.DigitalSignature = digitalSignature.Where(x => SignIDS.Contains(x.DigSignatureId.ToString())).ToList();  // DigitalSignRepository.GetDigitalSignature(list.SignIds).ToList();
                    }
                    if (!string.IsNullOrEmpty(list.CritiqueSignIds))
                    {
                        string[] SignIDS = list.CritiqueSignIds.Split(',');
                        list.CritiqueDigitalSignature = digitalSignature.Where(x => SignIDS.Contains(x.DigSignatureId.ToString())).ToList();  // DigitalSignRepository.GetDigitalSignature(list.CritiqueSignIds).ToList();
                    }
                    if (!string.IsNullOrEmpty(list.EducationSignIds))
                    {
                        string[] SignIDS = list.EducationSignIds.Split(',');
                        list.EducationDigitalSignature = digitalSignature.Where(x => SignIDS.Contains(x.DigSignatureId.ToString())).ToList();  //DigitalSignRepository.GetDigitalSignature(list.EducationSignIds).ToList();
                    }
                    list.TExerciseActions = texerciseActions.Where(x => x.TExerciseId == list.TExerciseId).ToList();
                    lists.Add(list);
                }
            }
            return lists;
        }


        public List<TExercises> GetExerciseFiles(int? exerciseId)
        {
            var lists = new List<TExercises>();
            const string sql = StoredProcedures.Get_Exercises;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (exerciseId != null)
                    command.Parameters.AddWithValue("@Exerciseid", Convert.ToInt32(exerciseId));
                var ds = _sqlHelper.GetDataSet(command);
                var files = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[1]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var list = new TExercises
                    {
                        TExerciseId = Convert.ToInt32(row["TExerciseId"].ToString()),
                    };
                    list.TExerciseFiles = files.Where(x => x.TExerciseId == list.TExerciseId).ToList();
                    list.TExerciseFiles = list.TExerciseFiles.Count > 0 ? list.TExerciseFiles : new List<TExerciseFiles>();
                    lists.Add(list);
                }
            }
            return lists;
        }
        public List<TExercises> GetExerciseFiles(string BuildingIds, int quarterno)
        {
            var lists = new List<TExercises>();
            const string sql = StoredProcedures.Get_ExercisesFiles;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingIds", BuildingIds);
                command.Parameters.AddWithValue("@Quarterno", quarterno > 0 ? quarterno : (object)DBNull.Value);
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<TExercises>(ds.Tables[0]);
                List<TExerciseFiles> texercisefiles = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[0]);
                List<UserProfile> users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]);
                List<Shift> shift = _sqlHelper.ConvertDataTable<Shift>(ds.Tables[2]);
                foreach (var item in lists)
                {
                    item.Shift = shift.FirstOrDefault(x => x.ShiftId == item.ShiftId);
                    item.TExerciseFiles = texercisefiles.Where(x => x.TExerciseId == item.TExerciseId).ToList();
                    item.CreatedByUser = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                }
            }
            return lists;
        }

        public bool DeleteTExerciseActions(int texerciseId, int texerciseActionId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_TExerciseActions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@texerciseActionId", texerciseActionId);
                command.Parameters.AddWithValue("@texerciseId", texerciseId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        //private  IEnumerable<TExercises> GetExercises(int quarterId, int shiftId, int buildingId, int drillType)
        //{
        //    var lists = new List<TExercises>();
        //    const string sql = Utility.StoredProcedures.Get_Exercises;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@QuarterId", quarterId);
        //        command.Parameters.AddWithValue("@ShiftId", shiftId);
        //        command.Parameters.AddWithValue("@BuildingId", buildingId);
        //        command.Parameters.AddWithValue("@DrillType", drillType);
        //        var ds = _sqlHelper.GetDataSet(command);
        //        var files = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[1]);
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            var list = new TExercises
        //            {
        //                Announced = Convert.ToBoolean(row["Announced"].ToString()),
        //                BuildingId = Convert.ToInt32(row["BuildingId"].ToString()),
        //                Date = Convert.ToDateTime(row["Date"].ToString()),
        //                Comment = row["Comment"].ToString(),
        //                NearBy = row["NearBy"].ToString(),
        //                ConductedBy = row["ConductedBy"].ToString(),
        //                Observers = row["Observers"].ToString(),
        //                CritiquesComment = row["CritiquesComment"].ToString(),
        //                EducationComment = row["EducationComment"].ToString(),
        //                Name = row["EducationComment"].ToString()
        //            };
        //            list.CritiqueDate = Convert.ToDateTime(!string.IsNullOrEmpty(row["CritiqueDate"].ToString()) ? row["CritiqueDate"].ToString() : row["Date"].ToString());


        //            list.EducationDate = Convert.ToDateTime(!string.IsNullOrEmpty(row["EducationDate"].ToString()) ? row["EducationDate"].ToString() : row["Date"].ToString());

        //            list.DateTimeSpan = Utility.Conversion.ConvertToTimeSpan(list.Date);
        //            list.TExerciseId = Convert.ToInt32(row["TExerciseId"].ToString());
        //            list.DrillType = Convert.ToInt32(row["DrillType"].ToString());
        //            list.ShiftId = Convert.ToInt32(row["ShiftId"].ToString());
        //            if (!string.IsNullOrEmpty(row["Time"].ToString()))
        //                list.Time = TimeSpan.Parse(row["Time"].ToString());
        //            list.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
        //            if (!string.IsNullOrEmpty(row["Status"].ToString()))
        //                list.Status = Convert.ToInt32(row["Status"].ToString());
        //            list.SignIds = row["SignIds"].ToString();
        //            list.CritiqueSignIds = row["CritiqueSignIds"].ToString();
        //            list.EducationSignIds = row["EducationSignIds"].ToString();
        //            list.Name = row["Name"].ToString();
        //            list.QuarterId = Convert.ToInt32(row["QuarterId"].ToString());
        //            list.StartTime = list.Time.ToString();
        //            if (list.Time.HasValue)
        //            {
        //                var time = DateTime.Today.Add(list.Time.Value);
        //                list.StartTime = time.ToString("hh:mm tt");
        //            }
        //            list.TExerciseFiles = files.Where(x => x.TExerciseId == list.TExerciseId).ToList();
        //            list.TExerciseFiles = list.TExerciseFiles.Count > 0 ? list.TExerciseFiles : new List<TExerciseFiles>();
        //            list.TExerciseQuestionnaires = GetTExerciseQuestionnaires(list.TExerciseId).ToList();
        //            // list.DigitalSignature = DigitalSignRepository.GetDigitalSignature().Where(x => list.SignIds.Contains(x.DigSignatureId.ToString())).ToList();
        //            if (!string.IsNullOrEmpty(list.SignIds))
        //            {
        //                list.DigitalSignature = DigitalSignRepository.GetDigitalSignature(list.SignIds).ToList();
        //            }
        //            if (!string.IsNullOrEmpty(list.CritiqueSignIds))
        //            {
        //                list.CritiqueDigitalSignature = DigitalSignRepository.GetDigitalSignature(list.CritiqueSignIds).ToList();
        //            }
        //            if (!string.IsNullOrEmpty(list.EducationSignIds))
        //            {
        //                list.EducationDigitalSignature = DigitalSignRepository.GetDigitalSignature(list.EducationSignIds).ToList();
        //            }

        //            if (!string.IsNullOrEmpty(row["IsAdditional"].ToString()))
        //                list.IsAdditional = Convert.ToBoolean(row["IsAdditional"].ToString());

        //            lists.Add(list);
        //        }
        //    }
        //    return lists;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns></returns>
        public bool DeleteExercises(int exerciseId)
        {
            bool status;

            const string sql = StoredProcedures.Delete_Exercises;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@exerciseId", exerciseId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newExercises"></param>
        /// <returns></returns>
        public bool UpdateExercises(TExercises newExercises)
        {
            bool status;
            if (newExercises.Date == null)
            {
                newExercises.Date = Conversion.ConvertToDateTime(newExercises.DateTimeSpan);
            }
            //newExercises.Date = Utility.Conversion.ConvertToDateTime(newExercises.DateTimeSpan);
            newExercises.CritiqueDate = Conversion.ConvertToDateTime(newExercises.CritiqueDateTimeSpan);
            newExercises.EducationDate = Conversion.ConvertToDateTime(newExercises.EducationDateTimeSpan);
            const string sql = StoredProcedures.Update_Exercises;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuarterId", newExercises.QuarterId);
                command.Parameters.AddWithValue("@Announced", newExercises.Announced);
                command.Parameters.AddWithValue("@CreatedBy", newExercises.CreatedBy);
                command.Parameters.AddWithValue("@Date", newExercises.Date);
                command.Parameters.AddWithValue("@Name", newExercises.Name);
                command.Parameters.AddWithValue("@Time", newExercises.Time);
                command.Parameters.AddWithValue("@ShiftId", newExercises.ShiftId);
                command.Parameters.AddWithValue("@Status", newExercises.Status);
                command.Parameters.AddWithValue("@BuildingId", newExercises.BuildingId);
                command.Parameters.AddWithValue("@ExerciseId", newExercises.TExerciseId);
                command.Parameters.AddWithValue("@SignIds", newExercises.SignIds);
                command.Parameters.AddWithValue("@Comment", newExercises.Comment);
                command.Parameters.AddWithValue("@NearBy", newExercises.NearBy);
                command.Parameters.AddWithValue("@Observers", newExercises.Observers);
                command.Parameters.AddWithValue("@ConductedBy", newExercises.ConductedBy);
                command.Parameters.AddWithValue("@CritiquesComment", newExercises.CritiquesComment);
                command.Parameters.AddWithValue("@EducationComment", newExercises.EducationComment);
                command.Parameters.AddWithValue("@CritiqueSignIds", newExercises.CritiqueSignIds);
                command.Parameters.AddWithValue("@EducationSignIds", newExercises.EducationSignIds);
                command.Parameters.AddWithValue("@CritiqueDate", newExercises.CritiqueDate);
                command.Parameters.AddWithValue("@EducationDate", newExercises.EducationDate);
                command.Parameters.AddWithValue("@FireDrillMode", newExercises.FireDrillMode);
                command.Parameters.AddWithValue("@year", newExercises.Year);
                command.Parameters.AddWithValue("@QuarterNo", newExercises.QuarterNo);
                command.Parameters.AddWithValue("@IsAudible", newExercises.IsAudible);
                command.Parameters.AddWithValue("@FiredrillDocStatus", newExercises.FiredrillDocStatus);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        //public void PlanDrill(int buildingTypeId, TExercises newExercises, int year, int quarterno)
        //{
        //    int count = 0;
        //    var nextQuaterDrills = new TExercises();
        //    for (int i = quarterno; i <= 4; i++)
        //    {
        //        count = count + 1;
        //        if (buildingTypeId == 2) { year = year + 1; } else { quarterno = i + 1; }
        //        if (quarterno > 4)
        //        {
        //            quarterno = 0;
        //            year = year + 1;
        //            for (int j = quarterno; j <= 4 - count; j++)
        //            {
        //                quarterno = j + 1;
        //                nextQuaterDrills = PlanFireDrill(buildingTypeId, quarterno, year, newExercises, count, nextQuaterDrills);
        //                count = count + 1;
        //            }
        //            break;
        //        }

        //        nextQuaterDrills = PlanFireDrill(buildingTypeId, quarterno, year, newExercises, count, nextQuaterDrills);
        //        if (count == 3 && quarterno == 4) { break; }
        //        //nextQuaterDrills = PlanFireDrill(BuildingTypeId, quarterno + 1, year, newExercises, count, nextQuaterDrills);
        //        if (buildingTypeId == 2) { break; }
        //    }
        //}

        //private TExercises PlanFireDrill(int buildingTypeId, int quaterId, int year, TExercises newExercises, int count, TExercises nextQuaterDrills)
        //{
        //    var getnextquarter = GetQuarterSettings(buildingTypeId, quaterId, year, false);
        //    var nextQuarter = getnextquarter.Select(x => x.QuarterId).FirstOrDefault();
        //    var exercise = GetExercises(null).Where(x => x.QuarterId == Convert.ToInt32(nextQuarter) && x.IsDeleted == false && x.BuildingId == newExercises.BuildingId && x.ShiftId == newExercises.ShiftId).ToList();
        //    if (nextQuarter == 0)
        //    {
        //        QuarterMaster quarterMaster = new QuarterMaster
        //        {
        //            QuarterNo = quaterId,
        //            BuildingTypeId = buildingTypeId,
        //            Year = year
        //        };
        //        nextQuarter = InsertQuarterMasterSettings(quarterMaster);
        //    }

        //    var exercises = new TExercises
        //    {
        //        ShiftId = newExercises.ShiftId,
        //        BuildingId = newExercises.BuildingId,
        //        DrillType = newExercises.DrillType,
        //        Announced = newExercises.Announced,
        //        Status = 0,
        //        Year = newExercises.Year,
        //        QuarterNo = newExercises.QuarterNo,
        //        Name = newExercises.Name,
        //        IsAdditional = newExercises.IsAdditional,
        //        CreatedBy = newExercises.CreatedBy,
        //        QuarterId = Convert.ToInt32(nextQuarter)
        //    };

        //    #region days and date calculations
        //    var shift = GetShift().FirstOrDefault(x => x.ShiftId == newExercises.ShiftId);


        //    if (nextQuaterDrills.QuarterId != 0)
        //    { exercises.Time = nextQuaterDrills.Time + TimeSpan.FromHours(1); }
        //    else { exercises.Time = newExercises.Time + TimeSpan.FromHours(1); }

        //    string Time = "";
        //    if (nextQuaterDrills.Time != null)
        //    {
        //        Time = nextQuaterDrills.Time.ToString();
        //    }
        //    else { Time = newExercises.Time.ToString(); }
        //    if (Time.Split(':')[0] == "00")
        //    {
        //        var time0 = Time.Split(':')[0].Replace("00", "12");
        //        string time1 = Time.Split(':')[1];
        //        string time2 = Time.Split(':')[2];
        //        Time = time0 + ":" + time1 + ":" + time2;
        //    }
        //    newExercises.Time = TimeSpan.Parse(Time);

        //    if (shift != null && (shift.StartTime == null || shift.EndTime == null))
        //    {
        //        TimeSpan? startTime = newExercises.Time + TimeSpan.FromHours(-4);
        //        TimeSpan? endTime = newExercises.Time + TimeSpan.FromHours(4);
        //        if (newExercises.Time >= startTime && newExercises.Time <= endTime)
        //        {
        //            exercises.Time = newExercises.Time + TimeSpan.FromHours(1);
        //        }
        //        else
        //        {
        //            exercises.Time = newExercises.Time + TimeSpan.FromHours(-1);
        //        }
        //    }
        //    if (exercises.Time >= shift.EndTime)
        //    {
        //        if (nextQuaterDrills.QuarterId != 0)
        //        { exercises.Time = nextQuaterDrills.Time + TimeSpan.FromHours(1); }
        //        else { exercises.Time = shift.StartTime + TimeSpan.FromHours(.50); }
        //    }

        //    string Time24 = exercises.Time.ToString();
        //    if (Time24.Split(':')[0] == "1.00")
        //    {
        //        var time0 = Time24.Split(':')[0].Replace("1.00", "00");
        //        string time1 = Time24.Split(':')[1];
        //        string time2 = Time24.Split(':')[2];
        //        Time24 = time0 + ":" + time1 + ":" + time2;
        //        exercises.Time = TimeSpan.Parse(Time24);
        //    }
        //    else
        //    {
        //        if (Time24.Split(':')[0] == "00")
        //        {
        //            var time0 = Time24.Split(':')[0].Replace("00", "12");
        //            string time1 = Time24.Split(':')[1];
        //            string time2 = Time24.Split(':')[2];
        //            Time24 = time0 + ":" + time1 + ":" + time2;
        //        }
        //        exercises.Time = TimeSpan.Parse(Time24);
        //    }
        //    #endregion

        //    DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(newExercises.Date));
        //    int currentweek = GetWeekNumberOfMonth(Convert.ToDateTime(newExercises.Date));

        //    if (buildingTypeId == 2) { exercises.Date = newExercises.Date + TimeSpan.FromDays(370); }
        //    else
        //    {
        //        if (count == 1) { exercises.Date = newExercises.Date + TimeSpan.FromDays(91); }
        //        else if (count == 2) { exercises.Date = newExercises.Date + TimeSpan.FromDays(181); }
        //        else { exercises.Date = newExercises.Date + TimeSpan.FromDays(271); }
        //    }

        //    if (currentweek == 5 && (count == 2 || count == 3))
        //    {
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(6);
        //    }
        //    int nextweek = GetWeekNumberOfMonth(Convert.ToDateTime(exercises.Date));

        //    if (nextQuaterDrills.QuarterId != 0)
        //    {
        //        var date = nextQuaterDrills.Date;
        //        currentweek = GetWeekNumberOfMonth(Convert.ToDateTime(date));
        //    }

        //    if (currentweek == nextweek)
        //    {
        //        var month = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(7);
        //        var monthafterinc = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        if (monthafterinc != month && buildingTypeId == 1)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(-14);
        //        }
        //        if (monthafterinc != month && buildingTypeId == 2)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(-8);
        //        }

        //    }

        //    if (currentweek == 5 && nextweek == 4 || currentweek == 1 && nextweek == 5)
        //    {
        //        var month = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(7);
        //        var monthafterinc = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        if (monthafterinc != month && buildingTypeId == 1)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(-14);
        //        }
        //        if (monthafterinc != month && buildingTypeId == 2)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(-8);
        //        }
        //    }

        //    var monthbeforeinc = "0";//Convert.ToDateTime(newExercises.Date).ToString("MM");
        //    if (newExercises.TExerciseId > 0 && nextQuaterDrills.TExerciseId == 0)
        //    {
        //        monthbeforeinc = Convert.ToDateTime(newExercises.Date).ToString("MM");
        //    }
        //    if (newExercises.TExerciseId > 0 && nextQuaterDrills.Date != null)
        //    {
        //        monthbeforeinc = Convert.ToDateTime(nextQuaterDrills.Date).ToString("MM");
        //    }
        //    var curmonth = Convert.ToDateTime(exercises.Date).ToString("MM");
        //    int val = Convert.ToInt32(monthbeforeinc) - Convert.ToInt32(curmonth);
        //    if (Math.Abs(val) < 3)
        //    {
        //        if (Math.Abs(val) <= 2)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(8);
        //        }
        //        else
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(-8);
        //        var afterchangemonth = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        int monthdiff = Convert.ToInt32(afterchangemonth) - Convert.ToInt32(monthbeforeinc);
        //        if (Math.Abs(monthdiff) < 3)
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(10);

        //    }

        //    if (Math.Abs(val) == 4)
        //    {
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(-8);
        //    }
        //    if (Math.Abs(val) > 4 && Math.Abs(val) < 5)
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(10);


        //    if (Math.Abs(val) == 8)
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(-7);
        //    if (Math.Abs(val) == 9)
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(5);

        //    var beforemonthchanged = Convert.ToDateTime(nextQuaterDrills.Date).ToString("MM");
        //    var aftermonthchanged = Convert.ToDateTime(exercises.Date).ToString("MM");

        //    int value = Convert.ToInt32(beforemonthchanged) - Convert.ToInt32(aftermonthchanged);
        //    if (Math.Abs(value) == 8)
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(-5);
        //    if (Math.Abs(value) == 9)
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(5);
        //    if (nextQuaterDrills.Date != null)
        //        day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(nextQuaterDrills.Date));
        //    DayOfWeek nextday = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(exercises.Date));
        //    exercises.Date = day == nextday ? exercises.Date + TimeSpan.FromDays(-1) : exercises.Date;
        //    nextday = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(exercises.Date));


        //    if (nextday == DayOfWeek.Sunday || nextday == DayOfWeek.Saturday)
        //    {
        //        var month = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        exercises.Date = exercises.Date + TimeSpan.FromDays(-2);
        //        nextday = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(exercises.Date));
        //        exercises.Date = day == nextday ? exercises.Date + TimeSpan.FromDays(-2) : exercises.Date;
        //        var monthafterinc = Convert.ToDateTime(exercises.Date).ToString("MM");
        //        if (monthafterinc != month)
        //        {
        //            exercises.Date = exercises.Date + TimeSpan.FromDays(5);
        //        }
        //    }


        //    if (exercise.Count > 0)
        //    {
        //        foreach (var eexerise in exercise)
        //        {
        //            if (eexerise.TExerciseId != 0 && eexerise.Name == exercises.Name)
        //            {
        //                exercises.TExerciseId = eexerise.TExerciseId;
        //                UpdateExercises(exercises);
        //            }
        //            else
        //                CreateExercises(exercises);
        //        }
        //    }
        //    else
        //    {
        //        CreateExercises(exercises);
        //    }
        //    var savedExercises = exercises;
        //    nextQuaterDrills = savedExercises;
        //    return nextQuaterDrills;
        //}


        // need to call from shift repo
        public List<BDO.Shift> GetShift()
        {
            List<Shift> lists = new List<BDO.Shift>();
            const string sql = StoredProcedures.Get_Shift;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<Shift>(dt);
            }
            return lists;
        }
        int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        public void PlanDrill(TExercises newExercises, int year, int quarterno)
        {
            var nextQuaterDrills = new TExercises();
            for (int i = 0; i < 3; i++)
            {
                if (quarterno >= 4)
                {
                    quarterno = 0;
                    year = year + 1;
                }
                quarterno = quarterno + 1;
                nextQuaterDrills = PlanFireDrill(quarterno, year, newExercises, nextQuaterDrills);
            }
        }

        private TExercises PlanFireDrill(int quarterno, int year, TExercises newExercises, TExercises nextQuaterDrills)
        {
            var exercises = new TExercises
            {
                ShiftId = newExercises.ShiftId,
                BuildingId = newExercises.BuildingId,
                LocationGroupId = newExercises.LocationGroupId,
                DrillType = newExercises.DrillType,
                Announced = newExercises.Announced,
                Status = 0,
                QuarterNo = quarterno,
                Name = newExercises.Name,
                IsAdditional = newExercises.IsAdditional,
                FiredrillDocStatus = Convert.ToInt32(FiredrillDocStatus.DocumentationPending),
                CreatedBy = newExercises.CreatedBy
            };

            #region days and date calculations

            var shift = GetShift().FirstOrDefault(x => x.ShiftId == newExercises.ShiftId);
            int currentWeek = 0;
            int nextweek = 0;
            DayOfWeek currentDay = 0;
            DayOfWeek nextDay = 0;
            if (nextQuaterDrills.TExerciseId > 0)
            {
                exercises.Time = nextQuaterDrills.Time + TimeSpan.FromHours(1);
                exercises.Date = (nextQuaterDrills.Date.Value.AddMonths(3).AddDays(1));
                currentWeek = GetWeekNumberOfMonth(Convert.ToDateTime(nextQuaterDrills.Date));
                nextweek = GetWeekNumberOfMonth(Convert.ToDateTime(exercises.Date));
                currentDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(nextQuaterDrills.Date));
            }
            else
            {
                exercises.Time = newExercises.Time + TimeSpan.FromHours(1);
                exercises.Date = (newExercises.Date.Value.AddMonths(3).AddDays(1));
                currentWeek = GetWeekNumberOfMonth(Convert.ToDateTime(newExercises.Date));
                nextweek = GetWeekNumberOfMonth(Convert.ToDateTime(exercises.Date));
                currentDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(newExercises.Date));
            }
            if (exercises.Time >= shift.EndTime)
            {
                exercises.Time = shift.StartTime + TimeSpan.FromHours(.25);
            }
            if (currentWeek == nextweek)
            {
                exercises.Date = exercises.Date.Value.AddDays(5);
            }
            nextDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(exercises.Date));
            if (currentDay == nextDay)
            {
                exercises.Date = exercises.Date.Value.AddDays(1);
            }
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(Convert.ToDateTime(exercises.Date));
            if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                exercises.Date = exercises.Date.Value.AddDays(2);
            }
            #endregion

            exercises.CritiqueDateTimeSpan = Conversion.ConvertToTimeSpan(exercises.Date);
            exercises.EducationDateTimeSpan = Conversion.ConvertToTimeSpan(exercises.Date);
            exercises.Year = exercises.Date.Value.Year;
            exercises.TExerciseId = CreateExercises(exercises);
            nextQuaterDrills = exercises;
            return nextQuaterDrills;
        }

        #endregion

        #region TExercises Files

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newExercises"></param>
        /// <returns></returns>
        public int InsertTExerciseFiles(TExerciseFiles newExercises)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TExercise;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ExerciseId", newExercises.TExerciseId);
                command.Parameters.AddWithValue("@CreatedBy", newExercises.CreatedBy);
                command.Parameters.AddWithValue("@FileName", newExercises.FileName);
                command.Parameters.AddWithValue("@FilePath", newExercises.FilePath);
                command.Parameters.AddWithValue("@DrillFileType", newExercises.DrillFileType);
                command.Parameters.AddWithValue("@DocumentType", newExercises.DocumentType);
                command.Parameters.AddWithValue("@IsActive", newExercises.IsActive);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;

        }



        #endregion

        #region Exercise Settings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newQuarterMaster"></param>
        /// <returns></returns>
        public int InsertQuarterMasterSettings(QuarterMaster newQuarterMaster)
        {
            int newId;
            const string sql = StoredProcedures.Insert_QuarterMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PlannedDrills", newQuarterMaster.TotalPlanned ?? 0);
                command.Parameters.AddWithValue("@UnAnnouncedDrills", newQuarterMaster.TotalUnAnnounced ?? 0);
                command.Parameters.AddWithValue("@Year", newQuarterMaster.Year);
                command.Parameters.AddWithValue("@QuarterNo", newQuarterMaster.QuarterNo);
                command.Parameters.AddWithValue("@BuildingTypeId", newQuarterMaster.BuildingTypeId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quarterMasterSettings"></param>
        /// <returns></returns>
        public bool UpdateQuarterMasterSettings(QuarterMaster quarterMasterSettings)
        {
            bool status;
            const string sql = StoredProcedures.Update_QuarterMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuarterId", quarterMasterSettings.QuarterId);
                command.Parameters.AddWithValue("@PlannedDrills", quarterMasterSettings.TotalPlanned);
                command.Parameters.AddWithValue("@UnAnnouncedDrills", quarterMasterSettings.TotalUnAnnounced);
                command.Parameters.AddWithValue("@StartDate", Conversion.ConvertToDateTime(quarterMasterSettings.StartDateTimeSpan));
                command.Parameters.AddWithValue("@EndDate", Conversion.ConvertToDateTime(quarterMasterSettings.EndDateTimeSpan));

                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }

        #endregion

        #region Quarter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingTypeId"></param>
        /// <param name="quarterNo"></param>
        /// <param name="year"></param>
        /// <param name="todayDrill"></param>
        /// <returns></returns>
        //public List<QuarterMaster> GetQuarterSettings(int buildingTypeId, int quarterNo, int year, bool todayDrill)
        //{
        //    List<QuarterMaster> quarterMasterSettings;
        //    const string sql = StoredProcedures.Get_QuarterMaster;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@BuildingTypeId", buildingTypeId);
        //        command.Parameters.AddWithValue("@QuarterNo", quarterNo);
        //        command.Parameters.AddWithValue("@Year", year);
        //        var ds = _sqlHelper.GetDataSet(command);
        //        quarterMasterSettings = _sqlHelper.ConvertDataTable<QuarterMaster>(ds.Tables[0]);
        //        var objShifts = _sqlHelper.ConvertDataTable<Shift>(ds.Tables[1]);
        //        var drillTypes = _sqlHelper.ConvertDataTable<FireDrillTypes>(ds.Tables[2]);
        //        var tExerciseses = _sqlHelper.ConvertDataTable<TExercises>(ds.Tables[3]);
        //        var tExercisesfiles = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[4]);
        //        if (todayDrill)
        //        {
        //            var exercises = new List<TExercises>();
        //            string currentdate = DateTime.Now.ToString("dd/MM/yyy");
        //            foreach (var exercise in tExerciseses)
        //            {
        //                exercise.TExerciseFiles = tExercisesfiles.Where(x => x.TExerciseId == exercise.TExerciseId).ToList();
        //                var dt = DateTime.Parse(exercise.Date.ToString()); //_exercise.Date.ToString();
        //                string exercisedate = dt.ToString("dd/MM/yyyy"); //= Date.ToString("dd/MM/yyy");    //DateTime.ParseExact(_exercise.Date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
        //                if (currentdate == exercisedate && exercise.Status == 0)
        //                {
        //                    exercises.Add(exercise);
        //                }
        //            }
        //            tExerciseses.Clear();
        //            tExerciseses = exercises;
        //        }
        //        var buildings = _buildingsRepository.GetBuildings().ToList();
        //        if (quarterMasterSettings.Count == 0)
        //        {
        //            var objQuarterMaster = new QuarterMaster
        //            {
        //                QuarterId = 0,
        //                BuildingTypeId = buildingTypeId,
        //                QuarterNo = quarterNo,
        //                Year = year,
        //                TotalDone = 0,
        //                TotalPlanned = 0,
        //                TotalUnAnnounced = 0,
        //                TotalUnAnnouncedDone = 0,
        //                FireDrillTypes = drillTypes
        //            };
        //            quarterMasterSettings.Add(objQuarterMaster);
        //        }
        //        foreach (var q1 in quarterMasterSettings)
        //        {
        //            q1.FireDrillTypes = drillTypes;
        //            var q2 = q1;
        //            q1.Buildings = buildings
        //                .Where(x => x.BuildingTypeId == q2.BuildingTypeId && x.IsActive).ToList();
        //            for (var ii = 0; ii < q1.Buildings.Count; ii++)
        //            {
        //                q1.Buildings[ii].Shifts = new List<Shift>();
        //                q1.Buildings[ii].FireDrillTypes = new List<FireDrillTypes>();
        //                q1.Buildings[ii].FireDrillTypes.AddRange(q1.FireDrillTypes);
        //                var data = tExerciseses.Where(x => x.IsAdditional && x.BuildingId == q1.Buildings[ii].BuildingId).ToList().GroupBy(x => x.Name)
        //                    .Select(x => x.FirstOrDefault());
        //                foreach (var firExercisese in data)
        //                {
        //                    if (firExercisese != null)
        //                    {
        //                        var newFireDrill = new FireDrillTypes
        //                        {

        //                            Id = firExercisese.DrillType,
        //                            IsAdded = true,
        //                            FireDrillType = firExercisese.Name,
        //                            IsActive = true
        //                        };
        //                        q1.Buildings[ii].FireDrillTypes.Add(newFireDrill);
        //                    }
        //                }
        //                BindShiftData(q1, ii, objShifts, q1.Buildings[ii].FireDrillTypes, tExerciseses, tExercisesfiles);

        //            }
        //        }
        //    }
        //    return quarterMasterSettings;
        //}

        public QuarterMaster GetQuarterSettings(string buildingIds, string locationGroupIds, string mode, int year, bool todaydrill)
        {
            QuarterMaster quarterMasterSettings = new QuarterMaster();
            int[] buildArray;
            const string sql = StoredProcedures.Get_QuarterMaster;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingTypeId", 0);
                command.Parameters.AddWithValue("@QuarterNo", 0);
                command.Parameters.AddWithValue("@Year", year);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    //quarterMasterSettings = _sqlHelper.ConvertDataTable<QuarterMaster>(ds.Tables[0]);
                    var objShifts = _sqlHelper.ConvertDataTable<Shift>(ds.Tables[1]);
                    var drillTypes = _sqlHelper.ConvertDataTable<FireDrillTypes>(ds.Tables[2]);
                    var tExerciseses = _sqlHelper.ConvertDataTable<TExercises>(ds.Tables[3]);
                    var tExercisesfiles = _sqlHelper.ConvertDataTable<TExerciseFiles>(ds.Tables[4]);
                    if (todaydrill)
                    {
                        var exercises = new List<TExercises>();
                        string currentdate = DateTime.Now.ToString("dd/MM/yyy");
                        foreach (var exercise in tExerciseses)
                        {
                            exercise.TExerciseFiles = tExercisesfiles.Where(x => x.TExerciseId == exercise.TExerciseId).ToList();
                            var dt = DateTime.Parse(exercise.Date.ToString());
                            string exercisedate = dt.ToString("dd/MM/yyyy");
                            if (currentdate == exercisedate && exercise.Status == 0)
                            {
                                exercises.Add(exercise);
                            }
                        }
                        tExerciseses.Clear();
                        tExerciseses = exercises;
                    }

                    var buildings = new List<Buildings>();
                    if (FireDrillMode.ByBuildings.ToString() == mode)
                    {
                        if (buildingIds.Contains("-1") && todaydrill)
                            buildArray = tExerciseses.Where(x => x.BuildingId != null).Select(x => x.BuildingId.Value).ToArray();
                        else
                            buildArray = buildingIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        buildings = _buildingsRepository.GetBuildings().Where(x => buildArray.Contains(x.BuildingId)).ToList();
                    }
                    else
                    {
                        if (locationGroupIds.Contains("-1") && todaydrill)
                            buildArray = tExerciseses.Where(x => x.LocationGroupId != null).Select(x => x.LocationGroupId.Value).ToArray();
                        else
                            buildArray = locationGroupIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                        var locationGroups = _locationRepository.GetLocationGroup().Where(x => buildArray.Contains(x.LocationGroupId)).ToList();
                        foreach (var item in locationGroups)
                        {
                            Buildings objbuilding = new Buildings();
                            objbuilding.BuildingName = item.Name;
                            objbuilding.LocationGroupId = item.LocationGroupId;
                            objbuilding.IsActive = item.IsActive;
                            buildings.Add(objbuilding);
                        }
                    }
                    quarterMasterSettings.Year = year;
                    quarterMasterSettings.FireDrillTypes = drillTypes;
                    quarterMasterSettings.Buildings = buildings.Where(x => x.IsActive).ToList();
                    for (var ii = 0; ii < quarterMasterSettings.Buildings.Count; ii++)
                    {
                        quarterMasterSettings.Buildings[ii].Shifts = new List<Shift>();
                        BindShiftDatas(quarterMasterSettings, ii, objShifts, drillTypes, tExerciseses, tExercisesfiles, mode);
                    }
                }
            }
            return quarterMasterSettings;
            //throw new NotImplementedException();
        }

        private void BindShiftDatas(QuarterMaster q1, int ii, List<Shift> objShifts, List<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles, string mode)
        {
            foreach (var shift in objShifts)
            {
                var shift1 = new Shift
                {
                    ShiftId = shift.ShiftId,
                    Name = shift.Name,
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime
                };
                shift1.FireDrillTypes = new List<FireDrillTypes>();
                shift1.FireDrillTypes.AddRange(fireDrillTypes);

                var data = exercises.ToList().GroupBy(x => x.Name).Select(x => x.FirstOrDefault());
                if (FireDrillMode.ByBuildings.ToString() == mode)
                {
                    data = exercises.Where(x => x.IsAdditional && x.BuildingId == q1.Buildings[ii].BuildingId && x.ShiftId == shift1.ShiftId).ToList().GroupBy(x => x.Name)
                           .Select(x => x.FirstOrDefault());
                }
                else
                {
                    data = exercises.Where(x => x.IsAdditional && x.LocationGroupId == q1.Buildings[ii].LocationGroupId && x.ShiftId == shift1.ShiftId).ToList().GroupBy(x => x.Name)
                               .Select(x => x.FirstOrDefault());
                }

                foreach (var firExercisese in data)
                {
                    if (firExercisese != null)
                    {
                        var newFireDrill = new FireDrillTypes
                        {
                            Id = firExercisese.DrillType,
                            IsAdded = true,
                            FireDrillType = firExercisese.Name,
                            IsActive = true
                        };
                        shift1.FireDrillTypes.Add(newFireDrill);
                    }
                }
                var firedrillLocationId = FireDrillMode.ByBuildings.ToString() == mode ? q1.Buildings[ii].BuildingId : q1.Buildings[ii].LocationGroupId.Value;
                GetExercises(shift1, shift1.ShiftId, firedrillLocationId, shift1.FireDrillTypes, exercises, tExercisesfiles, mode);
                q1.Buildings[ii].Shifts.Add(shift1);
            }
        }

        public void GetExercises(Shift newShift, int shiftId, int firedrillLocationId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles, string mode)
        {
            newShift.Exercises = new List<TExercises>();
            for (int i = 1; i < 5; i++)
            {
                foreach (var fireDrillType in fireDrillTypes.OrderBy(x => x.Sequence))
                {
                    var ex = new HCF.BDO.TExercises();
                    if (FireDrillMode.ByBuildings.ToString() == mode)
                        ex = exercises.FirstOrDefault(x => x.QuarterNo == i && x.ShiftId == shiftId && x.BuildingId == firedrillLocationId && String.Equals(x.Name, fireDrillType.FireDrillType, StringComparison.CurrentCultureIgnoreCase));
                    else
                        ex = exercises.FirstOrDefault(x => x.QuarterNo == i && x.ShiftId == shiftId && x.LocationGroupId == firedrillLocationId && String.Equals(x.Name, fireDrillType.FireDrillType, StringComparison.CurrentCultureIgnoreCase));

                    if (ex != null)
                    {
                        DateTime firstDayOfNextQuarter = getFirstdayofNextQuarter(ex.Date.Value, ex.QuarterNo);
                        ex.TExerciseQuestionnaires = GetTExerciseQuestionnaires(ex.TExerciseId).ToList();
                        ex.TExerciseFiles = tExercisesfiles.Where(x => x.TExerciseId == ex.TExerciseId).ToList();
                        var drillcompleted = ex.TExerciseFiles.Where(x => x.DocumentType == 110).ToList();
                        ex.FiredrillDocStatus = drillcompleted.Count > 0 ? Convert.ToInt32(FiredrillDocStatus.DocumentationCompleted) : firstDayOfNextQuarter > ex.Date.Value ? Convert.ToInt32(FiredrillDocStatus.DocumentationPastdue) : ex.TExerciseQuestionnaires.Any(x => x.TExerciseQuestId > 0) ? Convert.ToInt32(FiredrillDocStatus.DocumentationInprogress) : Convert.ToInt32(FiredrillDocStatus.DocumentationPending);
                        newShift.Exercises.Add(ex);
                    }
                    else
                    {
                        ex = new TExercises
                        {
                            TExerciseId = 0,
                            Announced = false,
                            Status = 0,
                            BuildingId = FireDrillMode.ByBuildings.ToString() == mode ? firedrillLocationId : null,
                            LocationGroupId = FireDrillMode.ByLocationGroup.ToString() == mode ? firedrillLocationId : null,
                            QuarterNo = i,
                            ShiftId = shiftId,
                            DrillType = fireDrillType.Id,
                            FireDrillType = fireDrillType,
                            IsAdditional = fireDrillType.IsAdded,
                            FiredrillDocStatus = Convert.ToInt32(FiredrillDocStatus.DocumentationPending)
                        };

                        ex.Name = fireDrillType.FireDrillType;
                        ex.TExerciseQuestionnaires = new List<TExerciseQuestionnaires>(); //GetTExerciseQuestionnaires(ex.TExerciseId);
                        newShift.Exercises.Add(ex);
                    }
                }
            }
        }


        public DateTime getFirstdayofNextQuarter(DateTime myDate, int quarterNo)
        {
            return new DateTime(myDate.Year, (3 * quarterNo) - 2, 1);
        }



        public List<QuarterMaster> GetQuarterMaster(int buildingTypeId, int quarterNo, int year, int? buildingId, string siteCode)
        {
            const string sql = StoredProcedures.Get_QuarterMasterCount;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingTypeId", buildingTypeId);
                command.Parameters.AddWithValue("@QuarterNo", quarterNo);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@BuildingId", buildingId.HasValue ? buildingId : null);
                command.Parameters.AddWithValue("@SiteCode", (!string.IsNullOrEmpty(siteCode) ? siteCode : null));
                var ds = _sqlHelper.GetDataSet(command);
                var quarterMasterSettings = _sqlHelper.ConvertDataTable<QuarterMaster>(ds.Tables[0]);
                if (quarterMasterSettings.Count == 0)
                {
                    var objQuarterMaster = new QuarterMaster
                    {
                        QuarterId = 0,
                        BuildingTypeId = buildingTypeId,
                        QuarterNo = quarterNo,
                        Year = year,
                        TotalDone = 0,
                        TotalPlanned = 0,
                        TotalUnAnnounced = 0,
                        TotalUnAnnouncedDone = 0
                    };
                    quarterMasterSettings.Add(objQuarterMaster);
                }
                return quarterMasterSettings;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="ii"></param>
        /// <param name="objShifts"></param>
        /// <param name="fireDrillTypes"></param>
        /// <param name="exercises"></param>
        /// <param name="tExercisesfiles"></param>
        private void BindShiftData(QuarterMaster q1, int ii, IEnumerable<Shift> objShifts, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles)
        {
            foreach (var shift in objShifts)
            {
                var shift1 = new Shift
                {
                    ShiftId = shift.ShiftId,
                    Name = shift.Name,
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime
                };
                if (q1.QuarterId != null)
                    GetExercise(shift1, q1.QuarterId.Value, shift1.ShiftId, q1.Buildings[ii].BuildingId, fireDrillTypes, exercises, tExercisesfiles);
                q1.Buildings[ii].Shifts.Add(shift1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newShift"></param>
        /// <param name="quarterId"></param>
        /// <param name="shiftId"></param>
        /// <param name="buildingId"></param>
        /// <param name="fireDrillTypes"></param>
        /// <param name="building"></param>
        /// <param name="exercises"></param>
        /// <param name="tExercisesfiles"></param>
        private void GetExercise(Shift newShift, int quarterId, int shiftId, int buildingId, IEnumerable<FireDrillTypes> fireDrillTypes, List<TExercises> exercises, List<TExerciseFiles> tExercisesfiles)
        {
            newShift.Exercises = new List<TExercises>();
            foreach (var fireDrillType in fireDrillTypes.OrderBy(x => x.Sequence))
            {
                var ex = exercises.FirstOrDefault(x => x.QuarterId == quarterId && x.ShiftId == shiftId && x.BuildingId == buildingId
                                                       && String.Equals(x.Name, fireDrillType.FireDrillType, StringComparison.CurrentCultureIgnoreCase)); //GetExercises(quarterId, shiftId, buildingId, fireDrillType.Id).FirstOrDefault(x=>x.IsAdditional== fireDrillType.IsAdded);

                if (ex != null)
                {
                    ex.TExerciseQuestionnaires = GetTExerciseQuestionnaires(ex.TExerciseId).ToList();
                    ex.TExerciseFiles = tExercisesfiles.Where(x => x.TExerciseId == ex.TExerciseId).ToList();
                    newShift.Exercises.Add(ex);
                }
                else
                {
                    ex = new TExercises
                    {
                        TExerciseId = 0,
                        Announced = false,
                        Status = 0,
                        BuildingId = buildingId,
                        QuarterId = quarterId,
                        ShiftId = shiftId,
                        DrillType = fireDrillType.Id,
                        FireDrillType = fireDrillType,
                        IsAdditional = fireDrillType.IsAdded
                    };

                    ex.Name = fireDrillType.FireDrillType;
                    ex.TExerciseQuestionnaires = new List<TExerciseQuestionnaires>(); //GetTExerciseQuestionnaires(ex.TExerciseId);
                    newShift.Exercises.Add(ex);
                }
            }
        }

        #endregion

        #region Fire Drill Common Questionnaires

        public int InsertFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
        {
            int newId;
            const string sql = StoredProcedures.Insert_FireDrillQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FireDrillQuesId", newFireDrillQuestionnaires.FireDrillQuesId);
                command.Parameters.AddWithValue("@FireDrillCatId", newFireDrillQuestionnaires.FireDrillCatId);
                command.Parameters.AddWithValue("@Questionnaries", newFireDrillQuestionnaires.Questionnaries);
                command.Parameters.AddWithValue("@IsActive", newFireDrillQuestionnaires.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newFireDrillQuestionnaires.CreatedBy);
                if (newFireDrillQuestionnaires.IsCommQues)
                    command.Parameters.AddWithValue("@Applicable", newFireDrillQuestionnaires.Applicable);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                if (newFireDrillQuestionnaires.IsCommQues)
                    newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
                else
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public bool UpdateFiredrillQuestionnaries(FireDrillQuestionnaires newFireDrillQuestionnaires)
        {
            bool status;
            const string sql = StoredProcedures.Update_FiredrillQuestionnaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FireDrillQuesId", newFireDrillQuestionnaires.FireDrillQuesId);
                command.Parameters.AddWithValue("@FireDrillCatId", newFireDrillQuestionnaires.FireDrillCatId);
                command.Parameters.AddWithValue("@Questionnaries", newFireDrillQuestionnaires.Questionnaries);
                command.Parameters.AddWithValue("@IsActive", newFireDrillQuestionnaires.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newFireDrillQuestionnaires.CreatedBy);
                if (newFireDrillQuestionnaires.IsCommQues)
                {
                    command.Parameters.AddWithValue("@Applicable", newFireDrillQuestionnaires.Applicable);
                    status = _sqlHelper.CommonExecuteNonQuery(command);
                }
                else
                    status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        public List<FireDrillCategory> GetFiredrillQuestionnaries()
        {
            List<FireDrillCategory> lists;
            const string sql = StoredProcedures.Get_FiredrillQuestionnaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                lists = _sqlHelper.ConvertDataTable<FireDrillCategory>(ds.Tables[0]);
                var fireDrillQuestionnaires = _sqlHelper.ConvertDataTable<FireDrillQuestionnaires>(ds.Tables[1]);
                foreach (var ques in fireDrillQuestionnaires)
                {
                    ques.IsActive = ques.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                }
                foreach (var item in lists)
                {
                    item.FireDrillQuestionnaires = fireDrillQuestionnaires.Where(x => x.FireDrillCatId == item.FiredrillCatId).ToList();
                }
            }
            return lists;
        }

        //public  FireDrillCategory GetFiredrillQuestionnaries(int fireDrillCategoryId)
        //{
        //    return GetFiredrillQuestionnaries().FirstOrDefault(x => x.FiredrillCatId == fireDrillCategoryId);
        //}

        public FireDrillQuestionnaires GetFireDrillQuestionnaire(int questionId)
        {
            var lists = new FireDrillQuestionnaires();
            const string sql = StoredProcedures.Get_FiredrillQuestionnaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(command);
                if (ds != null)
                {
                    var fireDrillQuestionnaires = _sqlHelper.ConvertDataTable<FireDrillQuestionnaires>(ds.Tables[1]);
                    foreach (var ques in fireDrillQuestionnaires)
                    {
                        ques.IsActive = ques.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                    }
                    lists = fireDrillQuestionnaires.FirstOrDefault(x => x.FireDrillQuesId == questionId);
                }
            }
            return lists;
        }

        #endregion

        #region TExerciseQuestionnaires      

        public int InsertTExerciseQuestionnaires(TExerciseQuestionnaires newObject)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TExerciseQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TExerciseQuestId", newObject.TExerciseQuestId);
                command.Parameters.AddWithValue("@Comments", newObject.Comments);
                command.Parameters.AddWithValue("@TExerciseId", newObject.TExerciseId);
                command.Parameters.AddWithValue("@FireDrillQuesId", newObject.FireDrillQuesId);
                command.Parameters.AddWithValue("@Status", newObject.Status);
                command.Parameters.AddWithValue("@Ratings", newObject.Ratings);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public List<TExerciseQuestionnaires> GetTExerciseQuestionnaires(int exerciseId)
        {
            var lists = new List<TExerciseQuestionnaires>();
            const string sql = StoredProcedures.Get_ExerciseQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@exerciseId", exerciseId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = DtToTExerciseQuestionnaires(ds.Tables[0]);
                }
            }
            return lists;
        }

        private List<TExerciseQuestionnaires> DtToTExerciseQuestionnaires(DataTable dt)
        {
            var lists = new List<TExerciseQuestionnaires>();
            foreach (DataRow item in dt.Rows)
            {
                var list = new TExerciseQuestionnaires
                {
                    Comments = item["Comments"].ToString(),
                    TExerciseId = !string.IsNullOrEmpty(item["TExerciseId"].ToString()) ? Convert.ToInt32(item["TExerciseId"].ToString()) : 0,
                    FireDrillQuesId = Convert.ToInt32(item["FireDrillQuesId"].ToString()),
                    Status = !string.IsNullOrEmpty(item["Status"].ToString()) ? Convert.ToInt32(item["Status"].ToString()) : -1,
                    Ratings = !string.IsNullOrEmpty(item["Ratings"].ToString()) ? Convert.ToInt32(item["Ratings"].ToString()) : default(int?),
                    TExerciseQuestId = !string.IsNullOrEmpty(item["TExerciseQuestId"].ToString()) ? Convert.ToInt32(item["TExerciseQuestId"].ToString()) : 0,
                    FireDrillQuestionnaires = new FireDrillQuestionnaires
                    {
                        Questionnaries = item["Questionnaries"].ToString(),
                        FireDrillQuesId = Convert.ToInt32(item["FireDrillQuesId"].ToString()),
                        FireDrillCatId = Convert.ToInt32(item["FireDrillCatId"].ToString()),
                        FireDrillCategory = new FireDrillCategory
                        {
                            FiredrillCatId = Convert.ToInt32(item["FiredrillCatId"].ToString()),
                            CategoryName = item["CategoryName"].ToString()
                        }
                    }
                };
                lists.Add(list);
            }
            return lists;
        }

        #endregion

        #region TExerciseActions
        public int InsertTExerciseActions(TExerciseActions newObject)
        {
            int newId;
            const string sql = StoredProcedures.Insert_TExerciseActions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TExerciseActionId", newObject.TExerciseActionId);
                command.Parameters.AddWithValue("@TExerciseId", newObject.TExerciseId);
                command.Parameters.AddWithValue("@Issue", newObject.Issue);
                command.Parameters.AddWithValue("@Action", newObject.Action);
                command.Parameters.AddWithValue("@IsDeleted", newObject.IsDeleted);
                command.Parameters.AddWithValue("@CreatedBy", newObject.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

        #region Fire Dill ques
        public List<FireDrillCategory> GetFiredrillQues()
        {
            List<FireDrillCategory> lists;
            const string sql = StoredProcedures.Get_FiredrillQuestionnaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                lists = _sqlHelper.ConvertDataTable<FireDrillCategory>(ds.Tables[0]);
                var fireDrillQuestionnaires = _sqlHelper.ConvertDataTable<FireDrillQuestionnaires>(ds.Tables[1]);
                foreach (var item in fireDrillQuestionnaires)
                {
                    if (item.IsCommQues)
                        item.IsActive = item.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                }
                //lists = lists.GroupBy(x => x.CategoryName).Select(grp => grp.First()).Distinct().ToList();
                foreach (var item in lists)
                {
                    item.FireDrillQuestionnaires = fireDrillQuestionnaires.Where(x => x.FireDrillCatId == item.FiredrillCatId).ToList();
                }
            }
            return lists;
        }

        public FireDrillQuestionnaires GetFireDrillQuestion(int questionId)
        {
            var lists = new FireDrillQuestionnaires();
            const string sql = StoredProcedures.Get_FiredrillQuestionnaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    var fireDrillQuestionnaires = _sqlHelper.ConvertDataTable<FireDrillQuestionnaires>(ds.Tables[1]);
                    foreach (var item in fireDrillQuestionnaires)
                    {
                        if (item.IsCommQues)
                            item.IsActive = item.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                    }
                    lists = fireDrillQuestionnaires.FirstOrDefault(x => x.FireDrillQuesId == questionId);
                }
            }
            return lists;
        }
        #endregion

        #region FireDril Common categ
        public List<FireDrillCategory> GetCommonFireDrillCategory()
        {
            List<FireDrillCategory> firedrillCategories;
            const string sql = StoredProcedures.GetCommonFireDrillCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetCommonDataSet(command))
                {
                    firedrillCategories = _sqlHelper.ConvertDataTable<FireDrillCategory>(ds.Tables[0]);
                }
            }
            return firedrillCategories;
        }

        public int Save(FireDrillCategory newFiredrillCategory)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_FiredrillCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FiredrilCatId", newFiredrillCategory.FiredrillCatId);
                command.Parameters.AddWithValue("@CategoryName", newFiredrillCategory.CategoryName);
                command.Parameters.AddWithValue("@IsActive", newFiredrillCategory.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newFiredrillCategory.CreatedBy);
                command.Parameters.AddWithValue("@Description", newFiredrillCategory.Description);
                if (newFiredrillCategory.IsCommonCat == true)
                    command.Parameters.AddWithValue("@Applicable", newFiredrillCategory.Applicable);

                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                if (newFiredrillCategory.IsCommonCat == true)
                    newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
                else
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        #endregion

        #region FireDril categ
        public List<FireDrillCategory> GetFireDrillCategory()
        {
            List<FireDrillCategory> firedrillCategories;
            const string sql = StoredProcedures.GetFireDrillCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    firedrillCategories = _sqlHelper.ConvertDataTable<FireDrillCategory>(ds.Tables[0]);
                }
            }
            return firedrillCategories;
        }

        public int Savefiredrill(FireDrillCategory newFiredrillCategory)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_FiredrillCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FiredrilCatId", newFiredrillCategory.FiredrillCatId);
                command.Parameters.AddWithValue("@CategoryName", newFiredrillCategory.CategoryName);
                command.Parameters.AddWithValue("@IsActive", newFiredrillCategory.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newFiredrillCategory.CreatedBy);
                command.Parameters.AddWithValue("@Description", newFiredrillCategory.Description);

                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                if (newFiredrillCategory.IsCommonCat == true)
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
                else
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion

    }
}
