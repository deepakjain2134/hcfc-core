using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;
using System.Linq;

namespace HCF.BDO
{
    public class RoundGroup
    {

        [DataMember]
        [Key]
        public int RoundGroupId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int IsActive { get; set; }
        //[DataMember]
        public int CreatedBy { get; set; }
        //[DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int? Frequency { get; set; }

        [DataMember]
        public int? ReoccurEvery { get; set; }

        [DataMember]
        public int? Monthno { get; set; }
        [DataMember]
        public int? Dayno { get; set; }
        [DataMember]
        public int? Yearno { get; set; }

        [DataMember]
        public int? Weekno { get; set; }

        [DataMember]
        public int? The { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string RoundName { get; set; }

        [DataMember]
        public DateTime RoundDate { get; set; }
        [DataMember]
        public int RecurFor { get; set; }
        [DataMember]
        public long RoundDatetimespan
        {
            get => Conversion.ConvertToTimeSpan(RoundDate);
            set { }
        }

        [DataMember]
        public int RoundID { get; set; }

        [DataMember]
        public bool isRoundVolunteer { get; set; }
        [DataMember]
        public string Roundcategories { get; set; }

        [DataMember]
        public bool InspectionDone { get; set; }

        [DataMember]
        public String Names { get; set; }

        public List<string> RoundGroupNames { get; set; }

        public List<UserProfile> GroupUsers { get; set; }
        public List<RoundCategory> RoundCategory { get; set; }
        public List<RoundSchedules> RoundSchedules { get; set; }
        public List<TRounds> RoundInspections { get; set; }

        public List<RoundGroupLocations> RoundGroupLocations { get; set; }

        public List<RoundGroupUsers> RoundGroupUsers { get; set; }

        public List<Buildings> Locations { get; set; }

        public List<TRoundUsers> TRoundUsers { get; set; }
        public List<TRoundInspections> Inspections { get; set; }

        public List<TRoundInspections> FloorInpections { get; set; }
        //public List<RoundScheduleDates> RoundScheduleDates { get; set; }

        public List<RoundVolunteer> roundVolunteers { get; set; }
        public List<RoundVolunteer> roundVolunteersAssigned { get; set; }

        [DataMember]
        public int RoundType { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }


        [DataMember]
        public bool IsRecurringRound { get; set; }


        [DataMember]
        public string Ocurrence { get; set; }


        public Dictionary<int, string> Months { get; set; }
        public Dictionary<int, string> DayName { get; set; }
        public Dictionary<int, string> SequenceOcuurence { get; set; }

        public string RoundStatus
        {
            get => SetRoundStatus();
            set { }
        }
        public int RoundStatusID
        {
            get => SetRoundStatusID();
            set { }
        }
        private string SetRoundStatus()
        {
            string statusKey = string.Empty;
            if (RoundSchedules.Any() && RoundSchedules.FirstOrDefault().RoundScheduleDates.Any())
            {
                var firstInsp = RoundSchedules.FirstOrDefault().RoundScheduleDates.OrderBy(x => x.RoundStatus).FirstOrDefault();
                if (firstInsp != null)
                    statusKey = firstInsp.RoundStatus;
            }
            return statusKey;
        }
        private int SetRoundStatusID()
        {
            int statusKey = 0;
            if (RoundSchedules.Any() && RoundSchedules.FirstOrDefault().RoundScheduleDates.Any())
            {
                var firstInsp = RoundSchedules.FirstOrDefault().RoundScheduleDates.OrderBy(x => x.Status).FirstOrDefault();
                if (firstInsp != null)
                {
                    if (firstInsp.Status.HasValue)
                    {
                        statusKey = firstInsp.Status.Value;
                    }
                    else if (!firstInsp.Status.HasValue && Convert.ToDateTime(firstInsp.RoundDate).Date >= DateTime.Now.Date)
                    {
                        statusKey = -1;//Due
                    }
                    else
                    {
                        statusKey = -2;
                    }

                }

            }
            return statusKey;
        }

        public string RoundStatusText
        {
            get => GetRoundStatusText(SetRoundStatus());
            set { }
        }

        public TimeSpan? Time { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        private string GetRoundStatusText(string v)
        {
            if (string.IsNullOrEmpty(v))
                return "Due";
            else if (v == "status_pass")
                return "Complete";
            else if (v == "status_inprogress")
                return "In-Progress";
            else if (v == "status_inprogress")
                return "In-Progress";
            else if (v == "grey")
                return "Not Done";
            else if (v == "status_fail")
                return "Deficiency";

            return "Due";
        }

        [DataMember]
        public string RoundsCategorieList
        {
            get => SetRoundCategories();
            set { }
        }

        private string SetRoundCategories()
        {
            if (Roundcategories != null)
            {
                if (RoundCategory.Any())
                {
                    if (RoundType == 2)
                    {
                        return string.Join(",", RoundCategory.Distinct().Select(x => x.CategoryName));
                    }
                    else
                    {
                        return RoundCategory.Distinct().FirstOrDefault().CategoryName;
                    }
                }
            }
            return string.Empty;
        }

        public RoundGroup()
        {
            GroupUsers = new List<UserProfile>();
            RoundCategory = new List<RoundCategory>();
            RoundSchedules = new List<RoundSchedules>();
            RoundInspections = new List<TRounds>();
            RoundGroupLocations = new List<RoundGroupLocations>();
            RoundGroupUsers = new List<RoundGroupUsers>();
            Locations = new List<Buildings>();
            TRoundUsers = new List<TRoundUsers>();
            Inspections = new List<TRoundInspections>();
            FloorInpections = new List<TRoundInspections>();
            roundVolunteers = new List<RoundVolunteer>();
            roundVolunteersAssigned = new List<RoundVolunteer>();
            // RoundScheduleDates = new List<RoundScheduleDates>();
        }
    }

    [DataContract]
    public class RoundGroupUsers
    {
        [DataMember]
        public int RguId { get; set; }

        [DataMember]
        public int RoundGroupId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string RoundCategories { get; set; }
        [DataMember]
        public bool IsReplaced { get; set; }
        [DataMember]
        public int ReplacedBy { get; set; }

        [DataMember]
        public int ReplacedFrom { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public UserProfile userProfile { get; set; }
        [DataMember]
        public List<RoundVolunteer> roundVolunteers { get; set; }

        public int RoundCatId { get; set; }
        public RoundGroupUsers()
        {
            roundVolunteers = new List<RoundVolunteer>();
            userProfile = new UserProfile();
        }

    }

    [DataContract]
    public class RoundSchedules
    {
        [DataMember]
        public string Names { get; set; }
        public List<string> RoundGroupNames { get; set; }

        [DataMember]
        public string Categories { get; set; }

        [DataMember]
        public int TRoundGroupId { get; set; }

        [DataMember]
        public int RoundGroupId { get; set; }

        [DataMember]
        public int RoundCatId { get; set; }

        [DataMember]
        public RoundCategory RoundCategory { get; set; }

        [DataMember]
        public List<RoundCategory> RoundCategories { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Enums.Frequency FrequencyId { get; set; }

        [DataMember]
        public DateTime StartInitialDate { get; set; }


        [DataMember]
        public string day { get; set; }

        [DataMember]
        public string Occurence { get; set; }

        [DataMember]
        public int? ReoccurEvery { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public int? Monthno { get; set; }
        [DataMember]
        public int? Dayno { get; set; }
        [DataMember]
        public int? Yearno { get; set; }

        [DataMember]
        public int? Weekno { get; set; }
        [DataMember]
        public int RecurFor { get; set; }
        [DataMember]
        public int? The { get; set; }

        [DataMember]
        public DateTime RoundDate { get; set; }

        [DataMember]
        public long RoundDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(RoundDate);
            set { }
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public int? TRoundId { get; set; }

        [DataMember]
        public int? RoundType { get; set; }

        [DataMember]
        public string RoundStatus
        {
            get => SetRoundStatus();
            set { }
        }

        private string SetRoundStatus()
        {

            if (Status.HasValue)
            {
                if (Status == 1)
                    return "status_pass";
                else if (Status == 2)
                    return "status_inprogress";
                else if (Status == 0)
                    return "status_fail";
            }
            else
            {
                if (RoundDate.Date >= DateTime.Now.Date)
                    return "status_grace_in_days_sysadmin";
                else if (DateTime.Now.Date >= RoundDate.Date)
                    return "grey";
            }

            return string.Empty;
        }

        [DataMember]
        public bool CanInspect
        {
            get => CheckCanInspect();
            set { }
        }

        private bool CheckCanInspect()
        {
            if (Status == 0 || Status == 1)
                return false;

            if (RoundDate.Date > DateTime.Now.Date)
                return false;

            if (DateTime.Now.Date >= RoundDate.Date)
                return true;
            return false;
        }

        public RoundGroup RoundGroup { get; set; }

        [DataMember]
        public List<RoundScheduleDates> RoundScheduleDates { get; set; }

        public List<RoundGroupUsers> RoundGroupUsers { get; set; }

        public RoundSchedules()
        {
            RoundScheduleDates = new List<RoundScheduleDates>();
            RoundGroupUsers = new List<RoundGroupUsers>();
        }

        public TimeSpan? Time { get; set; }

        [DataMember]
        public string RoundsCategorieList
        {
            get => SetRoundCategories();
            set { }
        }

        private string SetRoundCategories()
        {
            if (RoundCategories != null)
            {
                if (RoundCategories.Any())
                {
                    if (RoundType == 2)
                    {
                        return string.Join(",", RoundCategories.Distinct().Select(x => x.CategoryName));
                    }
                    else
                    {
                        return RoundCategories.Distinct().FirstOrDefault().CategoryName;
                    }
                }
            }
            return string.Empty;
        }
    }


    [DataContract]
    public class RoundGroupLocations
    {
        [DataMember]
        public int RoundGroupLocationId { get; set; }

        [DataMember]
        public int? RoundGroupId { get; set; }

        [DataMember]
        public RoundGroup RoundGroup { get; set; }

        [DataMember]
        public int? BuildingId { get; set; }

        [DataMember]
        public Buildings Building { get; set; }
        [DataMember]
        public Floor floors { get; set; }

        [DataMember]
        public int? FloorId { get; set; }


        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }


        [DataMember]
        public bool IsActive { get; set; }
    }

    [DataContract]
    public class RoundScheduleDates
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int RoundScheduleId { get; set; }

        [DataMember]
        public DateTime RoundDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDone { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public int? TRoundId { get; set; }
        [DataMember]
        public int RoundType { get; set; }
        //[DataMember]
        //public TRounds RoundInspection { get; set; }

        [DataMember]
        public string RoundStatus => SetRoundStatus();

        private string SetRoundStatus()
        {

            if (Status.HasValue)
            {
                if (Status == 1)
                    return "status_pass";
                else if (Status == 2)
                    return "status_inprogress";
                else if (Status == 0)
                    return "status_fail";
            }
            else
            {
                if (RoundDate.Date >= DateTime.Now.Date)
                    return "status_grace_in_days_sysadmin";
                else if (DateTime.Now.Date >= RoundDate.Date)
                    return "grey";
            }

            return string.Empty;
        }

        [DataMember]
        public bool CanInspect => CheckCanInspect();

        private bool CheckCanInspect()
        {
            if (Status == 0 || Status == 1)
                return false;

            if (RoundDate.Date > DateTime.Now.Date)
                return false;

            if (DateTime.Now.Date >= RoundDate.Date)
                return true;

            return false;
        }

        [DataMember]
        public int RoundGroupId { get; set; }
        [DataMember]
        public int RoundCatId { get; set; }

        public List<UserProfile> GroupUsers { get; set; }
        public RoundScheduleDates()
        {
            GroupUsers = new List<UserProfile>();
        }

        public List<RoundGroupLocations> Locations { get; set; }

        public TRounds Tround { get; set; }
    }

    [DataContract]
    public class RoundVolunteer
    {
        public string SelectedChecks { get; set; }

        [DataMember]
        public int RoundVolunteerID { get; set; }
        [DataMember]
        public int RoundGroupId { get; set; }
        [DataMember]
        public int Userid { get; set; }
        [DataMember]
        public int ReplacementType { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public DateTime EffectiveDate { get; set; }

        public List<RoundGroup> roundGroups { get; set; }
        [DataMember]
        public string RoundCategories { get; set; }
        [DataMember]
        public string RoundSchedulesIds { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }


        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ReplacedFrom { get; set; }

        public List<RoundCategory> roundCategory { get; set; }

        public UserProfile userProfile { get; set; }
        public RoundVolunteer()
        {
            roundGroups = new List<RoundGroup>();
            roundCategory = new List<RoundCategory>();
            userProfile = new UserProfile();
        }

        public string ReplacingUserName { get; set; }

        public List<UserProfile> userList { get; set; }


    }
}