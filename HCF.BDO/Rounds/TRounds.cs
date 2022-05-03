using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TRounds
    {

        [DataMember]
        [Key]
        public int TRoundId { get; set; }

        [DataMember]
        public int? RoundScheduleId { get; set; }

        [DataMember]
        public RoundGroup RoundGroup { get; set; }

        public string RoundGroupTime()
        {
            if (RoundGroup != null && RoundGroup.Time.HasValue)
            {
                var datetime = new DateTime(RoundGroup.Time.Value.Ticks);
                //var span = datetime.TimeOfDay;
                //RoundGroup.Time = span;
                return datetime.ToString("hh:mm:tt"); //.Value.ToString("hh:mm:tt").ToString();
            }
            else
                return "NA";
        }

        [DataMember]
        public string RoundCategories { get; set; }

        [DataMember]
        public string RoundName { get; set; }

        [DataMember]
        public bool IsAdditional { get; set; }

        [DataMember]
        public DateTime RoundDate { get; set; }

        [DataMember]
        public long RoundDateTimeSpan { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public int? WingId { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public int Status { get; set; }

        //[DataMember]
        //public int? AssetId { get; set; }

        //[DataMember]
        //public int? FloorAssetsId { get; set; }

        [DataMember]
        public int? RoundCatId { get; set; }

        public DateTime CreatedDate { get; set; }

        //[DataMember]
        //public long CreatedDateTimeSpan
        //{
        //    get => Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Floor Floors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TRoundFiles> RoundFiles { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public int TypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? StartDate { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public DateTime? EndDate { get; set; }


        #region Round Report Model parameter
        [DataMember(EmitDefaultValue = false)]
        public Decimal CompliantCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Questions { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int NumberOfOccurences { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int Occurences { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int RoundPercentage { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string RoundInspStatus { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public int WorkOrder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int NumberOfOccurencesRisk { get; set; }

        public string Risk { get; set; }
        public string QuestionsRisk { get; set; }
        public decimal CompliantCountBuilding { get; set; }
        public string BuildingName { get; set; }
        public string RoundStatus { get; set; }
        public int RoundCount { get; set; }
        public int RiskCount { get; set; }
        public string RiskType { get; set; }
        public int Assets { get; set; }
        public string AssetStatus { get; set; }

        #endregion

        [DataMember(EmitDefaultValue = false)]
        public List<TRoundsQuestionnaires> TRoundsQuestionnaires { get; set; }

        [DataMember]
        public DigitalSignature DigitalSignature { get; set; }


        [DataMember]
        public List<TRoundLocations> RoundLocations { get; set; }

        [DataMember]
        public List<TRoundUsers> TRoundUsers { get; set; }

        [DataMember]
        public List<Buildings> Locations { get; set; }

        //[DataMember]
        //public List<UserProfile> Inspectors { get; set; }

        [DataMember]
        public List<TRoundInspections> Inspections { get; set; }

        [DataMember]
        public List<RoundSchedules> TroundGroups { get; set; }

        //[DataMember]
        //public List<RoundSchedules> TroundGroup { get; set; }

        [DataMember]
        public List<TRoundWorkOrders> RoundWorkOrders { get; set; }

        [DataMember]
        public List<TIlsm> Ilsms { get; set; }
        public TRounds()
        {
            TRoundsQuestionnaires = new List<TRoundsQuestionnaires>();
            //Inspectors = new List<UserProfile>();
            Locations = new List<Buildings>();
            Inspections = new List<TRoundInspections>();
            RoundWorkOrders = new List<TRoundWorkOrders>();
            TRoundUsers = new List<TRoundUsers>();
            RoundLocations = new List<TRoundLocations>();
            Ilsms = new List<TIlsm>();
        }

        public bool IsInpectionReady => SetIsInpectionReady();

        private bool SetIsInpectionReady()
        {
            return Locations.Count > 0 && TRoundUsers.Count > 0;
        }


        public int RoundType => SetRoundType();

        private int SetRoundType()
        {
            if (TroundGroups != null && TroundGroups.Count > 0)
                return TroundGroups.FirstOrDefault().RoundType.Value;
            else return 0;
        }


        //public bool IsUserAuthorize(int userId)
        //{
        //    return TRoundUsers.Any(x => x.RoundUserId == userId);
        //}

        public int DefaultFloorId => SetDefaultFloorId();

        private int SetDefaultFloorId()
        {
            int floorId = 0;
            if (Locations.Count > 0 && Locations.FirstOrDefault().Floor != null)
                floorId = Locations.FirstOrDefault().Floor.FirstOrDefault().FloorId;
            return floorId;
        }

        public int GetNextFloor(int floorId)
        {
            int nextFloorId = 0;
            if (Locations.Count > 0 && Locations.FirstOrDefault().Floor != null)
            {
                var nextFloor = Locations.SkipWhile(x => x != x.Floor.Where(y => y.FloorId == floorId)).Skip(1).DefaultIfEmpty(Locations[0]).FirstOrDefault();
                nextFloorId = nextFloor != null ? nextFloor.Floor.FirstOrDefault().FloorId : Locations.FirstOrDefault().Floor.FirstOrDefault().FloorId;
            }
            return nextFloorId;
        }

        public bool IsRoundOpenForUser(int userId)
        {
            var existingUser = TRoundUsers.FirstOrDefault(x => x.RoundUserId == userId);
            if (existingUser != null)
            {
                if (Status == 2 && existingUser.IsClosedForMe == false)
                    return true;
            }
            return false;
        }
        public bool IsFloorRoundOpenForUser(int userId, int floorId)
        {
            if (IsRoundOpenForUser(userId))
            {
                var floorRoundInspection = Inspections.FirstOrDefault(x => x.UserId == userId && x.FloorId == floorId);
                if (floorRoundInspection != null)
                    return floorRoundInspection.IsOpen;
                else
                    return true;
            }
            return false;
        }


        //public bool CanEdit => SetCanEdit();

        //public bool SetCanEdit()
        //{
        //    return Status != 1;
        //}
    }

    [DataContract]
    public class TRoundInspections
    {
        [DataMember] public Guid RoundInspId { get; set; }
        [DataMember] public int RoundCatId { get; set; }
        [DataMember] public int TRoundId { get; set; }

        [DataMember] public int UserId { get; set; }

        [DataMember] public int FloorId { get; set; }

        [DataMember] public string Comment { get; set; }

        [DataMember] public int Status { get; set; }

        [DataMember] public bool IsOpen { get; set; }

        [DataMember] public DateTime StartDate { get; set; }

        [DataMember] public DateTime CompleteDate { get; set; }

        [DataMember]
        public UserProfile User { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        public int TotalCountQ { get; set; }

        [DataMember]
        public int InspStatus
        {
            get => SetInspectionStatus();
            set { }
        }

        private int SetInspectionStatus()
        {
            // var total = TotalCountQ;
            var failCount = Questionnaires.Count(x => x.Status == 0);
            //var passCount = Questionnaires.Count(x => x.Status == 1);
            if (IsOpen)
            {
                return 2;
            }
            else
            {
                return failCount > 0 ? 0 : 1;
            }
        }

        [DataMember]
        public List<TRoundsQuestionnaires> Questionnaires { get; set; }

        public TRoundInspections()
        {
            Questionnaires = new List<TRoundsQuestionnaires>();
        }

        public TRoundInspections(int floorId, int userId)
        {
            FloorId = floorId;
            UserId = userId;
            IsOpen = true;
        }

        [DataMember]
        public string RoundCategory { get; set; }

        [DataMember]
        public bool IsAdditional { get; set; }
        public string GetCategoryName()
        {
            if (Questionnaires.Any())
                return Questionnaires.FirstOrDefault().RoundsQuestionnaires?.CategoryName;
            else
                return string.Empty;
        }

        public UserProfile InspectByUser { get; set; }

    }

    [DataContract]
    public class TRoundLocations
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TRoundId { get; set; }

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public int AddedBy { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        [DataMember]
        public UserProfile AddedByUser { get; set; }

        [DataMember]
        public List<TRoundWorkOrders> RoundWorkOrders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TRoundsQuestionnaires> TRoundsQuestionnaires { get; set; }

        public int GetQuestionnaireStatus(int questionsId)
        {
            return TRoundsQuestionnaires.ToList().Any(x => x.RouQuesId == questionsId && x.Status == 0) ? 0 : 1;
        }

        public string GetQuestionnaireComment(int questionsId)
        {
            return string.Join(",", TRoundsQuestionnaires.Where(x => x.RouQuesId == questionsId && !string.IsNullOrEmpty(x.Comment)).Select(x => x.Comment));
        }

        public TRoundLocations()
        {
            RoundWorkOrders = new List<TRoundWorkOrders>();
        }
    }

    [DataContract]
    public class TRoundUsers
    {
        [DataMember]
        public string RoundCatIds { get; set; }

        [DataMember]
        public bool IsClosedForMe { get; set; }
        [DataMember]
        public int AddedBy { get; set; }

        //[DataMember] public int Id { get; set; }

        [DataMember] public int TRoundId { get; set; }

        [DataMember] public int RoundUserId { get; set; }

        [DataMember] public bool IsActive { get; set; }

        //[DataMember] public bool IsCreator { get; set; }

        //[DataMember] public DateTime AddedDate { get; set; }

        [DataMember] public UserProfile User { get; set; }

        [DataMember] public int? InspStatus { get; set; }

    }

    [DataContract]
    public class TRoundWorkOrders
    {
        [DataMember]
        [Key]
        public int RoundIssueId { get; set; }

        [DataMember]
        public int TRoundId { get; set; }

        [DataMember]
        public int FloorId { get; set; }

        [DataMember]
        public int IssueId { get; set; }

        [DataMember]
        public string StepsId { get; set; }

        [DataMember]
        public string questionIdText { get; set; }

        [DataMember]
        public string questionResponseComment { get; set; }
        [DataMember]
        public string RoundName { get; set; }

        public List<int> StepId => GetStepsId();

        private List<int> GetStepsId()
        {
            List<int> array = new List<int>();
            if (!string.IsNullOrEmpty(StepsId))
                array = StepsId.Split(',').Select(Int32.Parse).ToList();
            return array;
        }

        [DataMember]
        public WorkOrder WorkOrder { get; set; }

        public string StatusCode { get; set; }

    }

}