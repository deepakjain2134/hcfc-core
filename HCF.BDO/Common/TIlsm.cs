using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HCF.BDO.Enums;

namespace HCF.BDO
{
    [DataContract]
    public class TIlsm
    {
        [DataMember]
        [Key]
        public int TIlsmId { get; set; }

        [DataMember]
        public DateTime llsmDate { get; set; }

        //[DataMember]
        public TimeSpan? llsmTime { get; set; }


        [DataMember]
        public string llsmStartTime { get; set; }


        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int? ClosedBy { get; set; }

        [DataMember]
        public int? ReopenBy { get; set; }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Notes { get; set; }     

        [DataMember]
        public int InspectionId { get; set; }

        [DataMember]
        [DisplayName("Incident Id")]
        public string IncidentId { get; set; }

        [DataMember]
        public ILSMStatus Status { get; set; }

        //[DataMember]
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        //[DataMember]
        //public long CreatedDateTimeSpan
        //{
        //    get => CreatedDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(CreatedDate);
        //    set { }
        //}


        //[DataMember]
        [DisplayName("Completed")]
        public DateTime? CompletedDate { get; set; }

        //[DataMember]
        //public long CompletedDateTimeSpan
        //{
        //    get => CreatedDate == default(DateTime) ? 0 : Utility.Conversion.ConvertToTimeSpan(CompletedDate);
        //    set { }
        //}


        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public Inspection SourceInspection { get; set; }

        [DataMember]
        public List<TInspectionActivity> TinspectionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MainGoal> MainGoal { get; set; }

        [DataMember]
        public List<TInspectionFiles> TInspectionFiles { get; set; }

        [DataMember]
        public List<StandardEps> TriggerEps { get; set; }

        [DataMember]
        public List<TIlsmEP> TIlsmEP { get; set; }

        [DataMember]
        public List<TFloorAssets> FloorAssets { get; set; }

        [DataMember]
        public bool EpStepsComplete { get; set; }

        [DataMember]
        public int? AssetId { get; set; }

        [DataMember]
        public int? EpDetailId { get; set; }

        [DataMember]
        public int? FloorId { get; set; }

        [DataMember]
        public Floor Floor { get; set; }

        [DataMember]
        public List<TIlsmfloorAssets> TIlsmfloorAssets { get; set; }

        [DataMember]
        public string BuildingIds { get; set; }

        [DataMember]
        public string FloorIds { get; set; }


        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public List<TComment> IlsmComments { get; set; }

        [DataMember]
        public int? TicraId { get; set; }

        [DataMember]
        public string ConstIlsmStepId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FilesContent { get; set; }

        [DataMember]
        public List<Assets> Assets
        {
            get => SetILSMAssets();
            set { }
        }

        [NotMapped]
        public List<UserProfile> Responsible
        {
            get => SetILSMResponsible();
            set { }
        }

        private List<UserProfile> SetILSMResponsible()
        {
            List<UserProfile> userProfile = new List<UserProfile>();
            if (TIlsmfloorAssets != null && TIlsmfloorAssets.Count > 0)
            {
                List<UserProfile> ilsmUserProfiles = TIlsmfloorAssets.Where(x => x.TInspectionActivity != null).Select(x => x.TInspectionActivity.UserProfile).ToList();
                userProfile = ilsmUserProfiles.GroupBy(test => test.UserId)
                   .Select(grp => grp.First())
                   .ToList();
            }
            return userProfile;
        }

        private List<Assets> SetILSMAssets()
        {
            List<Assets> assets = new List<Assets>();
            if (TIlsmfloorAssets != null && TIlsmfloorAssets.Count > 0)
            {
                List<TFloorAssets> floorAssets = TIlsmfloorAssets.Where(x => x.TInspectionActivity != null).Select(x => x.TInspectionActivity.TFloorAssets).ToList();
                assets = floorAssets.Select(x => x.Assets).GroupBy(test => test.AssetId)
                   .Select(grp => grp.First())
                   .ToList();
            }
            return assets;
        }

        [DataMember]
        public int? IssueId { get; set; }

        [NotMapped]
        [DataMember]
        public List<WorkOrder> WorkOrders { get; set; }

        [DataMember]
        public bool CanClose { get; set; }

        [DataMember]
        public string CompletionComment { get; set; }

        [DataMember]
        [NotMapped]
        public List<TIcraLog> ICRALists { get; set; }

        [NotMapped]
        public UserProfile Inspector { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MainGoal> Deficiencies { get; set; }

        [DataMember]
        public int WoCount { get; set; }

        [DataMember]
        public int ICRAsCount { get; set; }

        [DataMember]
        public int DocumentCount { get; set; }

        [DataMember]
        public int ObservationReportCount { get; set; }

        [DataMember]
        public int? TRoundId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Buildings> Buildings { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember] public int DeletedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> AllTilsmEPs { get; set; }        

        [DataMember]
        public List<TICRAReports> TICRAReports { get; set; }

        public TIlsm()
        {
            SourceInspection = new Inspection();
            TinspectionActivity = new List<TInspectionActivity>();
            MainGoal = new List<MainGoal>();
            Assets = new List<Assets>();
            Responsible = new List<UserProfile>();
            WorkOrders = new List<WorkOrder>();
            IlsmComments = new List<TComment>();
            ICRALists = new List<TIcraLog>();
            TIlsmfloorAssets = new List<TIlsmfloorAssets>();
            Inspector = new UserProfile();
            Deficiencies = new List<MainGoal>();
            Buildings = new List<Buildings>();
            AllTilsmEPs = new List<EPDetails>();
            TICRAReports = new List<TICRAReports>();
        }
    }

    [DataContract]
    public class TIlsmfloorAssets
    {
        [DataMember]
        [Key]
        public int TIlsmfloorAssetId { get; set; }

        [DataMember]
        public int? TilsmId { get; set; }

        [DataMember]
        public int? FloorAssetId { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public TInspectionActivity TInspectionActivity { get; set; }

    }


    [DataContract]
    public class TIlsmEP
    {
        [DataMember]
        public int TIlsmEPId { get; set; }

        [DataMember]
        public int TilsmId { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public bool IsAdditional { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public StandardEps StandardEp { get; set; }


    }
}