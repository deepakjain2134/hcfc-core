using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class Rounds : IRounds
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IDigitalSignRepository _digitalSignRepository;
        private readonly IBuildingsRepository _buildingsRepository;
        private readonly IQuestionnairesRepository _questionnairesRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IHCFSession _hCFSession;

        public Rounds(IHCFSession hCFSession, IUsersRepository usersRepository, IFloorRepository floorRepository, ISqlHelper sqlHelper, IDigitalSignRepository digitalSignRepository, IBuildingsRepository buildingsRepository, IQuestionnairesRepository questionnairesRepository)
        {
            _hCFSession = hCFSession;
            _usersRepository = usersRepository;
            _floorRepository = floorRepository;
            _buildingsRepository = buildingsRepository;
            _sqlHelper = sqlHelper;
            _digitalSignRepository = digitalSignRepository;
            _questionnairesRepository = questionnairesRepository;
        }

        #region Rounds

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public List<TRounds> GetFloorRound(int floorId, int roundId)
        {
            List<DigitalSignature> digitalSignatures;
            List<TRounds> floorRounds = new List<TRounds>();
            const string sql = StoredProcedures.Get_TRounds;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@roundId", roundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    floorRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]);
                    digitalSignatures = _sqlHelper.ConvertDataTable<DigitalSignature>(ds.Tables[1]);
                    foreach (var rnd in floorRounds.Where(x => x.IsCurrent))
                    {
                        if (rnd.Status != 2)
                        {
                            rnd.TRoundId = 0;
                            rnd.WingId = 0;
                            rnd.Comment = string.Empty;
                        }
                        rnd.TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
                        rnd.TRoundsQuestionnaires = GetTRoundsQuestionnaires(rnd.TRoundId);
                        if (rnd.TRoundId > 0)
                            rnd.DigitalSignature = digitalSignatures.Where(x => x.TRoundId == rnd.TRoundId).FirstOrDefault();
                    }
                }
            }
            return floorRounds.Where(x => x.IsCurrent).ToList();
        }

        public TRounds GetRoundsQuestionnaires(int floorId, int tRoundId, int userId)
        {
            TRounds tRounds = new TRounds();
            var roundInspection = new List<TRoundInspections>();

            const string sql = StoredProcedures.Get_TRoundsQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@tRoundId", tRoundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);

                if (ds != null && ds.Tables.Count > 0)
                {
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();

                    if (tRounds != null)
                    {
                        if (string.IsNullOrEmpty(tRounds.RoundCategories))
                            tRounds.RoundCategories = Convert.ToString(tRounds.RoundCatId);

                        tRounds.Inspections = new List<TRoundInspections>();
                        var troundsQuestionnaires = new List<TRoundsQuestionnaires>();
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {

                            var tquestion = new TRoundsQuestionnaires
                            {
                                TRQuesId = Convert.ToInt32(item["TRQuesId"].ToString()),
                                //RoundInspId = Guid.Parse(item["RoundInspId"].ToString()),
                                TRoundId = Convert.ToInt32(item["TRoundId"].ToString()),
                                RouQuesId = Convert.ToInt32(item["RouQuesId"].ToString()),
                                Status = Convert.ToInt32(item["Status"].ToString()),
                                FileName = item["FileName"].ToString(),
                                FilePath = item["FilePath"].ToString(),
                                Comment = item["Comment"].ToString()
                            };
                            if (!string.IsNullOrEmpty(item["RoundInspId"].ToString()))
                            {
                                tquestion.RoundInspId = Guid.Parse(item["RoundInspId"].ToString());
                            }

                            troundsQuestionnaires.Add(tquestion);

                        }

                        //troundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[1]);

                        var roundsQuestionnaires = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[1]);
                        var currentUser = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).FirstOrDefault();
                        //var tRoundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);
                        var tRoundUsers = new List<TRoundUsers>();
                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            var trounuser = new TRoundUsers
                            {
                                TRoundId = Convert.ToInt32(item["TRoundId"].ToString()),
                                IsActive = Convert.ToBoolean(item["IsActive"].ToString()),
                                RoundUserId = Convert.ToInt32(item["RoundUserId"].ToString())
                            };
                            if (!string.IsNullOrEmpty(item["IsClosedForMe"].ToString()))
                                trounuser.IsClosedForMe = Convert.ToBoolean(item["IsClosedForMe"].ToString());
                            tRoundUsers.Add(trounuser);
                        }
                        tRounds.TRoundUsers = tRoundUsers.Where(x => x.TRoundId == tRounds.TRoundId).ToList();

                        var currentFloor = _floorRepository.ConvertToFloor(ds.Tables[3]);

                        roundInspection = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[4]);
                        var inspection = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[6]);
                        //if (roundInspection == null || roundInspection.Count == 0)
                        //{
                        //roundInspection = new List<TRoundInspections>();
                        string[] categories = tRounds.RoundCategories.Split(',');
                        foreach (var item in categories)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                //roundInspection = new TRoundInspections(floorId, userId);
                                var inspe = new TRoundInspections(floorId, userId);
                                inspe.Status = -1;
                                inspe.RoundCatId = Convert.ToInt32(item);
                                if (!roundInspection.Any(x => x.RoundCatId == inspe.RoundCatId))
                                    roundInspection.Add(inspe);
                            }
                        }
                        // }

                        foreach (var item in troundsQuestionnaires)
                        {
                            item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId);
                            if (item.RouQuesId > 0)
                            {
                                item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId);
                                if (item.RoundsQuestionnaires != null)
                                    item.RoundsQuestionnaires.IsActive = true;
                            }
                            else
                            {
                                item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId && x.IsActive);
                                if (item.RoundsQuestionnaires != null && item.RoundsQuestionnaires.IsCommonRoundQues)
                                {
                                    if (!string.IsNullOrEmpty(item.RoundsQuestionnaires.Applicable))
                                    {
                                        item.RoundsQuestionnaires.IsActive = item.RoundsQuestionnaires.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                                    }
                                    else
                                    {
                                        item.RoundsQuestionnaires.IsActive = false;
                                    }
                                }
                            }
                        }

                        //foreach (var item1 in troundsQuestionnaires)
                        //{
                        //    if (item1.RoundsQuestionnaires != null && !item1.RoundsQuestionnaires.IsActive && troundsQuestionnaires.Count > 0)
                        //    {
                        //        troundsQuestionnaires = troundsQuestionnaires.Where(x => x.RouQuesId != item1.RouQuesId).ToList();
                        //    }
                        //}

                        tRounds.TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
                        foreach (var inspectionObj in roundInspection)
                        {
                            // remove this case after updating the RoundCatId in TRoundInspections table start
                            if (inspectionObj.RoundCatId == 0)
                            {
                                inspectionObj.RoundCatId = (tRounds.RoundCatId.HasValue) ? tRounds.RoundCatId.Value : 0;
                                inspectionObj.RoundCategory = tRounds.CategoryName;
                            }
                            // // remove this case after updating the RoundCatId in TRoundInspections table start
                            inspectionObj.User = currentUser;
                            inspectionObj.Floor = currentFloor;

                            inspectionObj.Questionnaires = troundsQuestionnaires.Where(x =>
                                    x.RoundsQuestionnaires.RoundCatId == inspectionObj.RoundCatId
                                    && x.RoundsQuestionnaires.IsOneTimeStep == false
                                    && x.RoundsQuestionnaires.IsActive
                             ).ToList();

                            var tempQuestions = troundsQuestionnaires.Where(x =>
                                      (x.RoundInspId == inspectionObj.RoundInspId && x.RoundsQuestionnaires.IsOneTimeStep) && x.RoundInspId != Guid.Empty
                             ).ToList();

                            inspectionObj.Questionnaires.AddRange(tempQuestions);
                        }
                    }
                }
                tRounds.Inspections = roundInspection;//.Add(roundInspection);
            }
            return tRounds;
        }

        public TRounds GetRoundsQuestionOnFloorId(int floorId, int roundCatId, int userId)
        {
            TRounds tRounds = new TRounds();
            var roundInspection = new TRoundInspections();

            const string sql = StoredProcedures.Get_TRoundsQuestionnairesByFloorId;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@roundCatId", roundCatId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);

                if (ds != null && ds.Tables.Count > 0)
                {
                    tRounds.Inspections = new List<TRoundInspections>();
                    var troundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[0]);
                    var roundsQuestionnaires = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[0]);
                    var currentUser = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[1]).FirstOrDefault();
                    var currentFloor = _floorRepository.ConvertToFloor(ds.Tables[2]);
                    //roundInspection = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[3]).FirstOrDefault();

                    if (roundInspection == null)
                        roundInspection = new TRoundInspections(floorId, userId);

                    foreach (var item in troundsQuestionnaires)
                    {
                        item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId);
                    }
                    tRounds.TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
                    roundInspection.User = currentUser;
                    roundInspection.Floor = currentFloor;
                    roundInspection.Questionnaires = troundsQuestionnaires;
                }

            }
            tRounds.Inspections.Add(roundInspection);
            return tRounds;
        }

        public TRounds GetRoundsQuestionnaires(int floorId, int tRoundId)
        {
            TRounds tRounds = new TRounds();
            var roundInspections = new List<TRoundInspections>();

            const string sql = StoredProcedures.Get_FloorRoundsQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@tRoundId", tRoundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var roundcategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[6]);
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();
                    if (tRounds != null)
                    {
                        tRounds.Inspections = new List<TRoundInspections>();

                        string[] categoryName = tRounds.RoundCategories.Split(';');
                        var query = string.Join(",", roundcategories.Where(c => categoryName.Contains(c.RoundCatId.ToString())).Select(x => x.CategoryName));
                        tRounds.CategoryName = query;

                        var troundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[1]);
                        var roundsQuestionnaires = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[1]);
                        var currentUser = new List<UserProfile>(); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                        var currentFloor = _floorRepository.ConvertToFloor(ds.Tables[3]);

                        foreach (DataRow item in ds.Tables[2].Rows)
                        {
                            var user = new UserProfile
                            {
                                UserId = Convert.ToInt32(item["UserId"].ToString()),
                                FirstName = item["FirstName"].ToString(),
                                LastName = item["LastName"].ToString(),
                                Email = item["Email"].ToString()
                            };

                            if (!currentUser.Any(x => x.UserId == user.UserId))
                                currentUser.Add(user);
                        }

                        roundInspections = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[4]);

                        var roundWorkOrders = _sqlHelper.ConvertDataTable<TRoundWorkOrders>(ds.Tables[5]);
                        tRounds.RoundWorkOrders = roundWorkOrders;

                        //foreach (var item in troundsQuestionnaires)
                        //{
                        //    item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId);
                        //}

                        foreach (var item in troundsQuestionnaires)
                        {
                            item.RoundsQuestionnaires = roundsQuestionnaires.FirstOrDefault(x => x.RouQuesId == item.RouQuesId);
                            if (item.RoundsQuestionnaires != null && item.RoundsQuestionnaires.IsCommonRoundQues)
                            {
                                if (!string.IsNullOrEmpty(item.RoundsQuestionnaires.Applicable))
                                {
                                    item.RoundsQuestionnaires.IsActive = item.RoundsQuestionnaires.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                                }
                                else
                                {
                                    item.RoundsQuestionnaires.IsActive = false;
                                }
                            }
                        }

                        //foreach (var item1 in troundsQuestionnaires)
                        //{
                        //    if (item1.RoundsQuestionnaires != null && !item1.RoundsQuestionnaires.IsActive && troundsQuestionnaires.Count > 0)
                        //    {
                        //        troundsQuestionnaires = troundsQuestionnaires.Where(x => x.RouQuesId != item1.RouQuesId).ToList();
                        //    }
                        //}

                        foreach (var roundInspection in roundInspections)
                        {
                            roundInspection.User = currentUser.FirstOrDefault(x => x.UserId == roundInspection.UserId);
                            roundInspection.Floor = currentFloor;
                            tRounds.TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
                            roundInspection.Questionnaires = troundsQuestionnaires.Where(x => x.RoundInspId == roundInspection.RoundInspId).ToList();
                        }
                    }
                }
                tRounds.Inspections = new List<TRoundInspections>();
                tRounds.Inspections = roundInspections;
            }
            return tRounds;
        }



        public List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(DateTime startDate, DateTime endDate)
        {
            var troundsQuestionnaires = new List<TRoundsQuestionnaires>();
            const string sql = StoredProcedures.Get_RoundMatrixData;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    troundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[0]);
                }
            }
            return troundsQuestionnaires;
        }

        public bool SaveRoundInspStatus(TRoundInspections roundInsp)
        {
            const string sql = StoredProcedures.Save_RoundInspStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", roundInsp.FloorId);
                command.Parameters.AddWithValue("@UserId", roundInsp.UserId);
                command.Parameters.AddWithValue("@Status", roundInsp.Status > 0 ? roundInsp.Status : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TRoundId", roundInsp.TRoundId);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }

        public bool SaveRoundStatus(TRounds rounds, int userId)
        {
            const string sql = StoredProcedures.Save_RoundStatus;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", rounds.TRoundId);
                command.Parameters.AddWithValue("@Status", rounds.Status);
                command.Parameters.AddWithValue("@RoundUserId", userId);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }


        public bool SaveRoundFailStatus(int status, int roundId)
        {
            const string sql = StoredProcedures.Save_RoundStatusForfail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", roundId);
                command.Parameters.AddWithValue("@Status", status);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }
        public bool SaveRoundInspection(TRoundInspections inspection)
        {
            const string sql = StoredProcedures.Save_RoundInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", inspection.FloorId);
                command.Parameters.AddWithValue("@UserId", inspection.UserId);
                command.Parameters.AddWithValue("@TRoundId", inspection.TRoundId);
                command.Parameters.AddWithValue("@Comment", inspection.Comment);
                command.Parameters.AddWithValue("@IsOpen", inspection.IsOpen);
                command.Parameters.AddWithValue("@Status", inspection.Status);
                command.Parameters.AddWithValue("@roundCatId", inspection.RoundCatId);
                command.Parameters.AddWithValue("@IsAdditional", inspection.IsAdditional);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }

        public bool SaveRoundInspectionSteps(int floorId, int userId, int status, string comment, TRoundsQuestionnaires roundQuestionnaires, int roundCatId)
        {

            const string sql = StoredProcedures.Save_RoundInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", floorId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@TRoundId", roundQuestionnaires.TRoundId);
                command.Parameters.AddWithValue("@Comment", comment);
                command.Parameters.AddWithValue("@RouQuesId", roundQuestionnaires.RouQuesId);
                command.Parameters.AddWithValue("@StepComment", roundQuestionnaires.Comment);
                command.Parameters.AddWithValue("@StepStatus", roundQuestionnaires.Status);
                command.Parameters.AddWithValue("@fileName", roundQuestionnaires.FileName);
                command.Parameters.AddWithValue("@filePath", roundQuestionnaires.FilePath);
                command.Parameters.AddWithValue("@roundCatId", roundCatId);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }



        public List<TRounds> GetRoundDetails(int? buildingId)
        {
            List<TRounds> tRounds = new List<TRounds>();
            const string sql = StoredProcedures.Get_RoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@buildingId", buildingId.HasValue && buildingId.Value > 0 ? buildingId.Value : (object)DBNull.Value);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);

                if (ds != null && ds.Tables.Count > 0)
                {
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                    var roundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);
                    if (tRounds != null)
                    {
                        foreach (var tround in tRounds)
                        {
                            // set users
                            tround.RoundDateTimeSpan = Conversion.ConvertToTimeSpan(tround.RoundDate.ToUniversalTime());
                            //string freqexpr = "TRoundId = '" + tround.TRoundId + "'";
                            //DataRow[] freqRowsUsers = ds.Tables[2].Select(freqexpr);
                            //List<UserProfile> users = new List<UserProfile>();
                            //if (freqRowsUsers.Count() > 0)
                            //{
                            //    List<int> userArray = new List<int>();
                            //    foreach (DataRow item in freqRowsUsers)
                            //    {
                            //        userArray.Add(Convert.ToInt32(item["UserId"].ToString()));
                            //    }
                            //    users = roundUsers.ToList().Where(x => userArray.Contains(x.UserId)).ToList();
                            //}
                            //tround.Inspectors = users;
                            tround.TRoundUsers = new List<TRoundUsers>();
                            var roundUser = roundUsers.Where(x => x.TRoundId == tround.TRoundId).ToList();
                            foreach (var item in roundUser)
                            {
                                item.User = users.FirstOrDefault(x => x.UserId == item.RoundUserId);
                            }
                            tround.TRoundUsers = roundUser;

                            // set locations

                            tround.Locations = new List<Buildings>();
                            string freqexprLocations = "TRoundId = '" + tround.TRoundId + "'";
                            DataRow[] freqRowsLocations = ds.Tables[1].Select(freqexprLocations);
                            //List<Buildings> locations = new List<Buildings>();
                            if (freqRowsLocations.Any())
                            {
                                var dt = freqRowsLocations.CopyToDataTable();
                                tround.Locations = _buildingsRepository.ConvertToBuildings(dt);
                            }
                        }
                    }
                }

            }
            return tRounds;
        }

        public TRounds GetRoundDetails(int roundId)
        {
            TRounds tRounds = new TRounds();
            const string sql = StoredProcedures.Get_RoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@roundId", roundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();
                    var roundcategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[10]);
                    if (tRounds != null)
                    {
                        tRounds.RoundDateTimeSpan = Conversion.ConvertToTimeSpan(tRounds.RoundDate.ToUniversalTime());
                        var roundWorkOrders = _sqlHelper.ConvertDataTable<TRoundWorkOrders>(ds.Tables[5]);
                        var workOrders = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[6]);
                        var roundLocations = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[1]).GroupBy(x => x.BuildingId).Select(x => x.First()).ToList();
                        var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[1]).ToList();
                        var roundILSM = _sqlHelper.ConvertDataTable<TIlsm>(ds.Tables[7]).ToList();
                        var roundscheduled = new List<RoundSchedules>();
                        if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                        {
                            roundscheduled = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[8]).ToList();
                            tRounds.TroundGroups = roundscheduled;
                        }
                        var roundGroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[9]).FirstOrDefault();
                        tRounds.RoundGroup = roundGroup;

                        string[] categoryName = tRounds.RoundCategories.Split(';');
                        var query = string.Join(",", roundcategories.Where(c => categoryName.Contains(c.RoundCatId.ToString())).Select(x => x.CategoryName));

                        tRounds.CategoryName = query;

                        //List<string> categoryName = new List<string>();
                        //foreach (var cate in tRounds.RoundCategories.Split(','))
                        //{
                        //    if (!string.IsNullOrEmpty(cate)) { categoryName.Add(cate); }
                        //}

                        foreach (var item in roundLocations)
                        {
                            item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                        }
                        //List<UserProfile> roundUsers = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                        tRounds.Locations = roundLocations;
                        //tRounds.Inspectors = roundUsers;

                        var roundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                        foreach (var item in roundUsers)
                        {
                            item.User = users.FirstOrDefault(x => x.UserId == item.RoundUserId);
                        }
                        tRounds.TRoundUsers = roundUsers;
                        tRounds.Ilsms = roundILSM;
                        var roundInspections = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[3]);
                        var roundInspUsers = _usersRepository.ConvertToUser(ds.Tables[3]); //_sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[3]);

                        var roundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[4]);
                        foreach (var item in roundInspections)
                        {
                            item.Questionnaires = roundsQuestionnaires.Where(x => x.RoundInspId == item.RoundInspId).ToList();
                            item.User = roundInspUsers.FirstOrDefault(x => x.UserId == item.UserId);
                        }
                        tRounds.RoundWorkOrders = new List<TRoundWorkOrders>();
                        tRounds.RoundWorkOrders = roundWorkOrders;
                        foreach (var item in tRounds.RoundWorkOrders)
                        {
                            item.WorkOrder = workOrders.FirstOrDefault(x => x.IssueId == item.IssueId);
                        }
                        tRounds.Inspections = roundInspections;

                    }
                }
            }
            return tRounds;
        }

        public TRounds GetRoundWoDetails(int roundId)
        {
            var tRounds = new TRounds();
            const string sql = StoredProcedures.Get_RoundWODetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@roundId", roundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();
                    if (tRounds != null)
                    {
                        var roundWorkOrders = _sqlHelper.ConvertDataTable<TRoundWorkOrders>(ds.Tables[4]);
                        var workOrders = _sqlHelper.ConvertDataTable<WorkOrder>(ds.Tables[5]);
                        var roundLocations = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[1]).GroupBy(x => x.BuildingId).Select(x => x.First()).ToList();
                        var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[1]).ToList();
                        foreach (var item in roundLocations)
                        {
                            item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                        }
                        //List<UserProfile> roundUsers = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                        tRounds.Locations = roundLocations;
                        //tRounds.Inspectors = roundUsers;

                        var roundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);
                        var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                        foreach (var item in roundUsers)
                        {
                            item.User = users.FirstOrDefault(x => x.UserId == item.RoundUserId);
                        }
                        tRounds.TRoundUsers = roundUsers;
                        var questionnaires = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[3]).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList();
                        var roundInspections = _sqlHelper.ConvertDataTable<TRoundInspections>(ds.Tables[3]).GroupBy(x => x.RoundInspId).Select(x => x.First()).ToList();
                        //  List<TRoundsQuestionnaires> roundsQuestionnaires = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(ds.Tables[3]).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList();
                        foreach (var item in questionnaires)
                        {
                            foreach (var wo in workOrders.Where(x => x.IssueId == item.IssueId))
                            {
                                if (!string.IsNullOrEmpty(wo.WorkOrderNumber))
                                {
                                    item.IsWODone = wo.WorkOrderNumber != "0" ? true : false;
                                }

                            }
                        }
                        foreach (var item in roundInspections)
                        {
                            item.Questionnaires = GetRoundQuestionnairesGroupBy(ds.Tables[3], item.FloorId, item.RoundInspId); //roundsQuestionnaires.Where(x => x.RoundInspId == item.RoundInspId).ToList();
                            foreach (var itemQuestionnaires in item.Questionnaires)
                            {
                                itemQuestionnaires.RoundsQuestionnaires = questionnaires.FirstOrDefault(x => x.RouQuesId == itemQuestionnaires.RouQuesId);
                            }

                            item.Floor = floors.FirstOrDefault(x => x.FloorId == item.FloorId);
                            if (item.Floor != null)
                            {
                                var building = roundLocations.FirstOrDefault(x => x.BuildingId == item.Floor.BuildingId);
                                if (building != null)
                                {
                                    item.Floor.Building = new Buildings
                                    {
                                        BuildingCode = building.BuildingCode,
                                        BuildingName = building.BuildingName,
                                        BuildingId = building.BuildingId,
                                        BuildingTypeId = building.BuildingTypeId
                                    };
                                }
                            }
                            item.InspectByUser = users.FirstOrDefault(x => x.UserId == item.UserId);
                        }
                        tRounds.RoundWorkOrders = new List<TRoundWorkOrders>();
                        tRounds.RoundWorkOrders = roundWorkOrders;
                        foreach (var item in tRounds.RoundWorkOrders)
                        {
                            item.WorkOrder = workOrders.FirstOrDefault(x => x.IssueId == item.IssueId);
                        }


                        tRounds.Inspections = roundInspections;
                    }
                }
            }
            return tRounds;
        }

        public TRounds GetRoundDetailsbyScheduleDateId(int? rscheduleDateId)
        {
            var tRounds = new TRounds();
            const string sql = StoredProcedures.Get_RoundDetailsbyScheduleDateId;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@rscheduleDateId", rscheduleDateId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();
                    var roundSchedules = _sqlHelper.ConvertDataTable<RoundSchedules>(ds.Tables[1]);
                    var roundGroup = _sqlHelper.ConvertDataTable<RoundGroup>(ds.Tables[2]).FirstOrDefault();
                    var roundsgroupUsers = _sqlHelper.ConvertDataTable<RoundGroupUsers>(ds.Tables[3]);
                    var roundGroupLocation = _sqlHelper.ConvertDataTable<RoundGroupLocations>(ds.Tables[4]);
                    tRounds.RoundGroup = roundGroup;
                    tRounds.RoundGroup.RoundSchedules = new List<RoundSchedules>();
                    tRounds.RoundGroup.RoundSchedules = roundSchedules;
                    tRounds.RoundGroup.RoundGroupUsers = roundsgroupUsers;
                    tRounds.RoundGroup.RoundGroupLocations = roundGroupLocation;
                }
            }
            return tRounds;
        }

        public bool SaveTRoundUserCategories(int tRoundId, int userId, string roundCatIds)
        {

            const string sql = StoredProcedures.Insert_TRoundUserCategories;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", tRoundId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RoundCatIds", roundCatIds);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return true;
        }


        private List<TRoundsQuestionnaires> GetRoundQuestionnairesGroupBy(DataTable dt, int floorId, Guid roundInspId)
        {
            var lists = new List<TRoundsQuestionnaires>();
            string freqexpr = "FloorId = '" + floorId + "'";
            DataRow[] freqRows = dt.Select(freqexpr);
            if (freqRows.Any())
            {
                DataTable filerDt = freqRows.CopyToDataTable();
                lists = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(filerDt).GroupBy(x => x.RouQuesId).Select(x => x.First()).ToList();
            }
            return lists.Where(x=>x.RoundInspId== roundInspId).ToList();
        }
        public int AddRoundLocation(TRoundLocations roundLocation)
        {
            int newId;
            const string sql = StoredProcedures.Update_RoundLocations;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", roundLocation.TRoundId);
                command.Parameters.AddWithValue("@FloorId", roundLocation.FloorId);
                command.Parameters.AddWithValue("@IsActive", roundLocation.IsActive);
                command.Parameters.AddWithValue("@AddedBy", roundLocation.AddedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public int AddRoundInspector(TRoundUsers roundUsers)
        {
            int newId;
            const string sql = StoredProcedures.Update_RoundInspectors;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", roundUsers.TRoundId);
                command.Parameters.AddWithValue("@IsActive", roundUsers.IsActive);
                command.Parameters.AddWithValue("@RoundUserId", roundUsers.RoundUserId);
                command.Parameters.AddWithValue("@AddedBy", roundUsers.AddedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<Buildings> GetRoundLocations(int roundId)
        {
            List<Buildings> roundLocations = new List<Buildings>();
            const string sql = StoredProcedures.Get_RoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@roundId", roundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    roundLocations = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[1]).GroupBy(x => x.BuildingId).Select(x => x.First()).ToList();

                    var floors = _sqlHelper.ConvertDataTable<Floor>(ds.Tables[1]).ToList();
                    foreach (var item in roundLocations)
                    {
                        item.Floor = floors.Where(x => x.BuildingId == item.BuildingId).ToList();
                    }
                }
            }
            return roundLocations;
        }

        public List<TRoundUsers> GetRoundUsers(int roundId)
        {
            var roundUsers = new List<TRoundUsers>();
            const string sql = StoredProcedures.Get_RoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@roundId", roundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    roundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]).GroupBy(x => x.UserId).Select(x => x.First()).ToList();
                    foreach (var item in roundUsers)
                    {
                        item.User = users.FirstOrDefault(x => x.UserId == item.RoundUserId);
                    }
                }
            }
            return roundUsers.Where(x => x.IsActive).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TRounds> GetRoundsCategories(int userId)
        {
            var roundsCategorys = new List<TRounds>();
            const string sql = StoredProcedures.Get_RoundsCategories;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Userid", userId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    roundsCategorys = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]);
                }
            }
            return roundsCategorys;
        }

        public List<RoundCategory> GetRoundCategory(int catId)
        {
            var roundCategories = new List<RoundCategory>();
            const string sql = StoredProcedures.GetRoundCategory;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@catId", catId > 0 ? catId : (object)DBNull.Value);
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        roundCategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[0]);
                        var data = new List<RoundsQuestionnaires>();
                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            var roundsQuestionnaire = new RoundsQuestionnaires
                            {
                                RouQuesId = Convert.ToInt32(item["RouQuesId"].ToString()),
                                RoundCatId = Convert.ToInt32(item["RoundCatId"].ToString()),
                                RoundStep = item["RoundStep"].ToString(),
                                IsShared = Convert.ToBoolean(item["IsShared"].ToString()),
                                IsActive = Convert.ToBoolean(item["IsActive"].ToString())
                            };

                            if (!string.IsNullOrEmpty(item["RiskType"].ToString()))
                                roundsQuestionnaire.RiskType = (BDO.Enums.RiskScore)Convert.ToInt32(item["RiskType"].ToString());

                            if (!string.IsNullOrEmpty(item["ParentRouQuesId"].ToString()))
                                roundsQuestionnaire.ParentRouQuesId = Convert.ToInt32(item["ParentRouQuesId"].ToString());

                            //if (!string.IsNullOrEmpty(item["IsOneTimeStep"].ToString()))
                            //    roundsQuestionnaire.IsOneTimeStep = Convert.ToBoolean(item["IsOneTimeStep"].ToString());

                            if (!string.IsNullOrEmpty(item["ShortDescription"].ToString()))
                                roundsQuestionnaire.ShortDescription = item["ShortDescription"].ToString();

                            if (!string.IsNullOrEmpty(item["RiskStepCode"].ToString()))
                                roundsQuestionnaire.RiskStepCode = item["RiskStepCode"].ToString();

                            roundsQuestionnaire.IsCommonRoundQues = Convert.ToBoolean(item["IsCommonRoundQues"].ToString());
                            roundsQuestionnaire.Applicable = item["Applicable"].ToString();
                            data.Add(roundsQuestionnaire);
                        }

                        //foreach (var item in data)
                        //{
                        //    if (item.IsCommonRoundQues)
                        //        item.IsActive = item.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                        //    else
                        //        item.IsActive = true;
                        //}
                        foreach (var item in roundCategories)
                            item.RoundItems = data.Where(x => x.RoundCatId == item.RoundCatId).ToList();
                    }
                }
            }
            return roundCategories;
        }

        public List<RoundCategory> GetRoundCategories_Data()
        {
            return GetRoundCategory(0);
            //var roundCategories=new List<RoundCategory>();
            //const string sql = StoredProcedures.GetRoundCategory_Data;
            //using (var command = new SqlCommand(sql))
            //{
            //    command.Parameters.AddWithValue("@catId", (object)DBNull.Value);
            //    command.CommandType = CommandType.StoredProcedure;
            //    using (var ds = _sqlHelper.GetDataSet(command))
            //    {
            //        if (ds != null && ds.Tables.Count > 0)
            //        {
            //            roundCategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[0]);
            //            var data = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[1]);
            //            foreach (var item in data)
            //            {
            //                if (item.IsCommonRoundQues)
            //                {
            //                    if(!string.IsNullOrEmpty(item.Applicable))
            //                    {
            //                        item.IsActive = item.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
            //                    }
            //                    else
            //                    {
            //                        item.IsActive = false;
            //                    }
            //                }

            //            }
            //            foreach (var item in roundCategories)
            //                item.RoundItems = data.Where(x => x.RoundCatId == item.RoundCatId).ToList();
            //        }
            //    }
            //}
            //return roundCategories;
        }

        public bool AddQuestionToCustomRound(int roundQuestionnaireId, int roundCatId, bool status)
        {
            int newId = 1;
            const string sql = StoredProcedures.AddQuestionToCustomRound;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@rouQuesId", roundQuestionnaireId);
                command.Parameters.AddWithValue("@roundCatId", roundCatId);
                command.Parameters.AddWithValue("@status", status);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return (newId > 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionRound"></param>
        /// <returns></returns>
        public int AddRoundInspection(TRounds inspectionRound, string floorIds, string userIds)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionRound; //"Insert_InspectionRound";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", inspectionRound.TRoundId);
                command.Parameters.AddWithValue("@FloorId", inspectionRound.FloorId);
                command.Parameters.AddWithValue("@RoundCatId", inspectionRound.RoundCatId);
                command.Parameters.AddWithValue("@Status", inspectionRound.Status);
                command.Parameters.AddWithValue("@Comment", inspectionRound.Comment);
                command.Parameters.AddWithValue("@CreatedBy", inspectionRound.CreatedBy);
                command.Parameters.AddWithValue("@RoundName", inspectionRound.RoundName);
                command.Parameters.AddWithValue("@RoundDate", inspectionRound.RoundDate);
                command.Parameters.AddWithValue("@RoundScheduleId", inspectionRound.RoundScheduleId);
                command.Parameters.AddWithValue("@IsAdditional", inspectionRound.IsAdditional);
                command.Parameters.AddWithValue("@roundCategories", inspectionRound.RoundCategories);
                command.Parameters.AddWithValue("@users", userIds);
                command.Parameters.AddWithValue("@locationIds ", floorIds);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveRoundFloorInspection(int FloorId, int TRoundId, int UserId)
        {
            int newId;
            const string sql = StoredProcedures.SaveRoundFloorInspection;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRoundId", TRoundId);
                command.Parameters.AddWithValue("@FloorId", FloorId);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionRoundItems"></param>
        /// <returns></returns>
        public int AddRoundItemsInspection(TRoundsQuestionnaires inspectionRoundItems)
        {
            int newId;
            const string sql = StoredProcedures.Insert_InspectionRoundItems; // "Insert_InspectionRoundItems";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TRQuesId", inspectionRoundItems.TRQuesId);
                command.Parameters.AddWithValue("@TRoundId", inspectionRoundItems.TRoundId);
                command.Parameters.AddWithValue("@RouQuesId", inspectionRoundItems.RouQuesId);
                command.Parameters.AddWithValue("@Status", inspectionRoundItems.Status);
                command.Parameters.AddWithValue("@Comment", inspectionRoundItems.Comment);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SetArchiveRound(string dbname)
        {
            const string sql = StoredProcedures.SetArchiveRound; // "Insert_InspectionRoundItems";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@DB", dbname);
                command.CommandType = CommandType.StoredProcedure;
                bool retval = _sqlHelper.CommonExecuteNonQuery(command);
                return retval ? 1 : 0;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundId"></param>
        /// <param name="floorId"></param>
        /// <param name="assetId"></param>
        /// <param name="floorassetId"></param>
        /// <returns></returns>
        public List<TRounds> GetRoundsHistory(int? roundId, int? floorId, int assetId = 0, int floorassetId = 0)
        {
            var sings = _digitalSignRepository.GetDigitalSignature();
            var rounds = new List<TRounds>();
            const string sql = StoredProcedures.Get_TRounds; // "Get_TRounds";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundId", roundId > 0 ? roundId : null);
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? floorId : null);
                command.Parameters.AddWithValue("@assetId", assetId);
                command.Parameters.AddWithValue("@floorassetId", floorassetId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    rounds = _sqlHelper.ConvertDataTable<TRounds>(dt);
                    foreach (var tRound in rounds.Where(x => x.Status != 2).ToList())
                    {
                        tRound.UserProfile = _usersRepository.GetUsersList(Convert.ToInt32(tRound.CreatedBy));
                        tRound.Floors = _floorRepository.GetFloor(Convert.ToInt32(tRound.FloorId));
                        if (tRound.TRoundId > 0)
                        {
                            tRound.TRoundsQuestionnaires = GetTRoundsQuestionnaires(tRound.TRoundId);
                            tRound.RoundFiles = GetRoundFiles(tRound.TRoundId);
                        }
                        var sign = sings.FirstOrDefault(x => x.TRoundId == tRound.TRoundId);
                        if (sign != null)
                            tRound.DigitalSignature = sign;
                    }
                }
            }
            return rounds.Where(x => x.Status != 2).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public List<TRounds> GetCurrentRoundStatus(int roundId)
        {
            var rounds = new List<TRounds>();
            const string sql = StoredProcedures.Get_TRounds; // "Get_TRounds";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundId", roundId);
                command.Parameters.AddWithValue("@floorId", 0);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null && dt.Rows.Count > 0)
                    rounds = _sqlHelper.ConvertDataTable<TRounds>(dt);

            }
            return rounds.Where(x => x.IsCurrent).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tRoundId"></param>
        /// <returns></returns>
        public List<TRoundsQuestionnaires> GetTRoundsQuestionnaires(int tRoundId)
        {
            var files = new List<TRoundsQuestionnaires>();
            const string sql = StoredProcedures.Get_RoundsQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tRoundId", tRoundId);
                var dt = _sqlHelper.GetDataSet(command);
                if (dt != null && dt.Tables.Count > 0)
                {
                    files = _sqlHelper.ConvertDataTable<TRoundsQuestionnaires>(dt.Tables[0]);
                    var rQuestionnaires = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(dt.Tables[0]);
                    foreach (var tRoundQuestion in files)
                        tRoundQuestion.RoundsQuestionnaires = rQuestionnaires.FirstOrDefault(x => x.RouQuesId == tRoundQuestion.RouQuesId);
                }
            }
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tRoundId"></param>
        /// <returns></returns>
        private List<TRoundFiles> GetRoundFiles(int tRoundId)
        {
            var files = new List<TRoundFiles>();
            const string sql = StoredProcedures.Get_RoundFiles; //"Get_RoundFiles";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tRoundId", tRoundId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                    files = _sqlHelper.ConvertDataTable<TRoundFiles>(dt);
            }
            return files;
        }

        //<summary>

        //</summary>
        //<param name="roundId"></param>
        //<param name="floorId"></param>
        //<param name="buildingId"></param>
        //<returns></returns>
        public List<TRounds> GetRoundsHistory(string roundId, int floorId, int buildingId)
        {
            var sings = _digitalSignRepository.GetDigitalSignature();
            List<TRounds> rounds;
            const string sql = StoredProcedures.Get_CompleteTRounds; // "Get_TRounds";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@roundsId", roundId);
                command.Parameters.AddWithValue("@floorId", floorId > 0 ? (object)floorId : null);
                command.Parameters.AddWithValue("@buildingId", buildingId > 0 ? (object)buildingId : null);
                var dt = _sqlHelper.GetDataTable(command);
                rounds = _sqlHelper.ConvertDataTable<TRounds>(dt);
                foreach (var tRound in rounds)
                {
                    tRound.UserProfile = _usersRepository.GetUsersList(Convert.ToInt32(tRound.CreatedBy));
                    tRound.Floors = _floorRepository.GetFloor(Convert.ToInt32(tRound.FloorId));
                    if (tRound.TRoundId > 0)
                    {
                        tRound.TRoundsQuestionnaires = GetTRoundsQuestionnaires(tRound.TRoundId);
                        tRound.RoundFiles = GetRoundFiles(tRound.TRoundId);
                    }
                    var sign = sings.FirstOrDefault(x => x.TRoundId == tRound.TRoundId);
                    if (sign != null)
                        tRound.DigitalSignature = sign;
                }
            }
            return rounds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TRounds> GetConsolidatedRoundsReport()
        {
            List<TRounds> roundsCategory;
            const string sql = StoredProcedures.Get_ConsolidatedRoundsReport;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                roundsCategory = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]);
                foreach (var rnd in roundsCategory)
                {
                    if (rnd.TRoundId > 0)
                    {
                        rnd.TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
                        rnd.TRoundsQuestionnaires = GetTRoundsQuestionnaires(rnd.TRoundId);
                    }
                }
            }
            return roundsCategory;
        }

        #endregion 

        #region PM Daily Logs

        public TPMLogs PmDailyLog(int? pmlogId)
        {
            var logs = new TPMLogs();
            var allQuestionnaires = _questionnairesRepository.GetQuestionnaires(null);//QuestionnairesRepository.GetActiveQuestionnaires(null);           
            if (pmlogId.HasValue)
            {
                logs = PmDailyLogs(pmlogId, null, null).FirstOrDefault();
                if (logs != null)
                {
                    var queststepId = logs.LogSteps.GroupBy(x => x.QuestionnaireStepId).Select(g => g.First().QuestionnaireStepId.Value).ToList();
                    var questionnaires = new List<Questionnaire>();
                    foreach (var item in allQuestionnaires)
                    {
                        item.QuestionnaireSteps = item.QuestionnaireSteps.Where(x => queststepId.Contains(x.QuestionnaireStepId)).ToList();
                        //bool res = item.QuestionnaireSteps.Where(x => x.IsActive).ToList().Any(x => queststepId.Contains(x.QuestionnaireStepId));
                        //if (res)
                        //{
                        //    questionnaires.Add(item);
                        //}
                    }
                    logs.Questionnaires = allQuestionnaires;
                }

                foreach (var item in logs.Questionnaires)
                {
                    foreach (var questionnaireSteps in item.QuestionnaireSteps)
                    {
                        foreach (var logSteps in questionnaireSteps.PMLogSteps)
                        {
                            var data = logs.LogSteps.FirstOrDefault(x => x.QuestionnaireStepId == questionnaireSteps.QuestionnaireStepId &&
                            x.QuestionnaireHeaderTypeId == logSteps.QuestionnaireHeaderTypeId);
                            if (data != null)
                            {
                                logSteps.Value = data.Value;
                                logSteps.Comments = data.Comments;
                                logSteps.IsDeleted = data.IsDeleted;
                                //logSteps = data;
                            }
                            // lo
                        }
                    }
                }
            }
            else
            {
                allQuestionnaires = allQuestionnaires.Where(x => x.IsActive).ToList();
                foreach (var item in allQuestionnaires)
                {
                    item.QuestionnaireSteps = item.QuestionnaireSteps.Where(x => x.IsActive).ToList();
                }
                logs.Questionnaires = allQuestionnaires;
            }

            if (!string.IsNullOrEmpty(logs.Time.ToString()))
            {
                var time = DateTime.Today.Add(logs.Time);
                logs.POTime = time.ToString("hh:mm tt");
            }

            //if (pmlogId.HasValue)
            //    logs = PMDailyLogs(pmlogId).FirstOrDefault();
            //if (!string.IsNullOrEmpty(logs.Time.ToString()))
            //{
            //    DateTime time = DateTime.Today.Add(logs.Time);
            //    logs.POTime = time.ToString("hh:mm tt");
            //}
            //logs.Questionnaires = questionnaires;
            //if (pmlogId.HasValue)
            //{
            //    foreach (var item in logs.Questionnaires)
            //    {
            //        foreach (var questionnaireSteps in item.QuestionnaireSteps)
            //        {
            //            foreach (var logSteps in questionnaireSteps.PMLogSteps)
            //            {
            //                var data = logs.LogSteps.FirstOrDefault(x => x.QuestionnaireStepId == questionnaireSteps.QuestionnaireStepId &&
            //                x.QuestionnaireHeaderTypeId == logSteps.QuestionnaireHeaderTypeId);
            //                if (data != null)
            //                    logSteps.Value = data.Value;
            //            }
            //        }
            //    }
            //}

            return logs;
        }

        public List<TPMLogs> PmDailyLogs(int? logId, DateTime? FromDate, DateTime? ToDate)
        {
            var logs = new List<TPMLogs>();
            string sql = StoredProcedures.Get_PMDailyLogs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PMLogId", logId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    logs = _sqlHelper.ConvertDataTable<TPMLogs>(ds.Tables[0]);
                    var buildings = _sqlHelper.ConvertDataTable<Buildings>(ds.Tables[0]);
                    var users = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
                    var logSteps = _sqlHelper.ConvertDataTable<TPMLogSteps>(ds.Tables[1]);
                    if (FromDate != null && ToDate != null)
                    {
                        logs = logs.Where(a => a.Date >= FromDate && a.Date <= ToDate).ToList();
                    }
                    foreach (var item in logs)
                    {
                        item.CreatedByUser = users.FirstOrDefault(x => x.UserId == item.CreatedBy);
                        if (item.ReviewedBy > 0)
                        { item.ReviewedByUser = _usersRepository.GetUsersList(item.ReviewedBy); }
                        item.LogSteps = logSteps.Where(x => x.PMLogId == item.PMLogId).ToList();

                        item.Building = buildings.FirstOrDefault(x => x.BuildingId == item.BuildingId);
                    }
                }
            }
            return logs;
        }

        public int SavePmDailyLog(TPMLogs logs)
        {
            int newId;
            const string sql = StoredProcedures.Insert_PMDailyLog;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BuildingId", logs.BuildingId);
                command.Parameters.AddWithValue("@Name", logs.Name);
                command.Parameters.AddWithValue("@Date", logs.Date);
                command.Parameters.AddWithValue("@Time", logs.Time);
                command.Parameters.AddWithValue("@Comments", logs.Comments);
                command.Parameters.AddWithValue("@ActionTaken", logs.ActionTaken);
                command.Parameters.AddWithValue("@ReviewedBy", logs.ReviewedBy);
                command.Parameters.AddWithValue("@CreatedBy", logs.CreatedBy);
                command.Parameters.AddWithValue("@ParentLogId", logs.ParentLogId);
                command.Parameters.AddWithValue("@Status", logs.Status);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SavePmLogSteps(TPMLogSteps pMLogSteps)
        {
            int newId;
            const string sql = StoredProcedures.Insert_PMLogSteps;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PMLogId", pMLogSteps.PMLogId);
                command.Parameters.AddWithValue("@QuestionnaireStepId", pMLogSteps.QuestionnaireStepId);
                command.Parameters.AddWithValue("@QuestionnaireHeaderTypeId", pMLogSteps.QuestionnaireHeaderTypeId);
                command.Parameters.AddWithValue("@Value", pMLogSteps.Value);
                command.Parameters.AddWithValue("@Comments", pMLogSteps.Comments);
                command.Parameters.AddWithValue("@IsDeleted", pMLogSteps.IsDeleted);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;

        }

        public List<TRoundUsers> GetTRoundUsers(int tRoundId)
        {
            List<TRoundUsers> roundUsers = new List<TRoundUsers>();
            const string sql = StoredProcedures.Get_RoundDetails;
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@roundId", tRoundId);
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var tRounds = _sqlHelper.ConvertDataTable<TRounds>(ds.Tables[0]).FirstOrDefault();
                    if (tRounds != null)
                    {
                        roundUsers = _sqlHelper.ConvertDataTable<TRoundUsers>(ds.Tables[2]);

                    }
                }
            }
            return roundUsers;
        }

        #endregion

        #region

        public List<RoundCategory> RoundCommonQuestionnaries()
        {
            var roundCategories = new List<RoundCategory>();
            const string sql = StoredProcedures.Get_RoundCommonQuestionaries;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetCommonDataSet(command))
                {
                    roundCategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[0]);
                    var data = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[1]);
                    foreach (var ques in data)
                    {
                        ques.IsActive = ques.Applicable.Contains(_hCFSession.ClientNo.ToString()) ? true : false;
                    }
                    foreach (var item in roundCategories)
                        item.RoundItems = data.Where(x => x.RoundCatId == item.RoundCatId).ToList();
                }
            }
            return roundCategories;
        }

        public List<RoundCategory> GetCommonRoundCategory()
        {
            List<RoundCategory> roundCategories;
            const string sql = StoredProcedures.GetCommonRoundCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var ds = _sqlHelper.GetCommonDataSet(command))
                {
                    roundCategories = _sqlHelper.ConvertDataTable<RoundCategory>(ds.Tables[0]);
                    var data = _sqlHelper.ConvertDataTable<RoundsQuestionnaires>(ds.Tables[1]);
                    foreach (var item in roundCategories)
                        item.RoundItems = data.Where(x => x.RoundCatId == item.RoundCatId).ToList();
                }
            }
            return roundCategories;
        }
        #endregion
    }
}