using HCF.BDO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class EPSteps
    {

        [DataMember]
        public bool IsAllowed { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public int EPDetailId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int StDescID { get; set; }

        [DataMember]
        public int InspectionId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? IsInspReady { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? FloorAssetId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? DoctypeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? DOCStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BinderId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BinderName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Standard,EP")]
        public string StandardEP { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ScoreName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Score { get; set; }

        [DataMember]
        public DateTime? LastInspectionDate { get; set; }

        [DataMember]
        public double? LastInspectionTimeSpan
        {
            get => LastInspectionDate == default(DateTime) ? 0 : Conversion.ConvertToTimeSpan(LastInspectionDate);
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public int? ScoreId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Sample Doc")]
        public string SampleDocPath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [Display(Name = "Doc Type")]
        public string DocName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MainGoal> MainGoal { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public InspectionEPDocs InspectionDoc { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<InspectionEPDocs> InspectionDocs { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionFiles> InspectionFiles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TFloorAssets TFloorAssets { get; set; }

        [DataMember]
        public bool IsAssetEP { get; set; }

        [DataMember]
        public int FloorAssetsCount { get; set; }

        [DataMember]
        public int ActivityType { get; set; }

        [DataMember]
        public bool IsDocRequired { get; set; }

        [DataMember]
        public EPDetails EPDetails { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TInspectionActivity TInspectionActivity { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int ActivityStatus { get; set; }

        [DataMember]
        public Inspection Inspection { get; set; }

        [DataMember]
        public Inspection LastInspection { get; set; }

        [DataMember]
        public List<InspectionReport> LastInspectionSummary { get; set; }

        [DataMember]
        public int InspectionGroupId { get; set; }

        [DataMember]
        public int DocInspectionGroupId { get; set; }

        [DataMember]
        public int? FrequencyId { get; set; }

        [DataMember]
        public int? FrequencyInDays { get; set; }

        [DataMember]
        public int? UploadDocTypeId { get; set; }

        [DataMember]
        public string CurrentStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string EpTransStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int EpTranStatus { get; set; }

        [DataMember]
        public int? DocCategoryID { get; set; }

        public string InspectorName
        {
            get => GetInspectorName();
            set { }
        }

        public string InspectionComment
        {
            get => GetComment();
            set { }
        }

        private string GetInspectorName()
        {
            if (TInspectionActivity != null && TInspectionActivity.UserProfile != null)
                return TInspectionActivity.UserProfile.Name;
            else
                return string.Empty;
        }

        private string GetComment()
        {
            if (TInspectionActivity != null && TInspectionActivity.UserProfile != null)
                return TInspectionActivity.Comment;
            else
                return string.Empty;
        }

        public DateTime? LastDocumentUploadDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPSteps> EPStep { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? IsAllAssetsdone { get; set; }
        public int? IsDocDashboard { get; set; }
        public string Comment { get; set; }

        public int? IsCurrentCycle { get; set; }

        public int? IsPreviousCycle { get; set; }

        public int? PreviousCycleInspectionId { get; set; }
         public Guid TCycleId { get; set; }
        

        public TInspectionActivity LastEPInspection { get; set; }

        [DataMember]
        public bool IsRelation { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Buildings> Campus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TInspectionActivity> InspectionActivities { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<InspectionEPDocs> epdocument { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public EPDetails epdetials { get; set; }
       
        public List<Inspection> CampusInspection { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CampusStatus
        {
            get => GetCampusStatus();
            set { }
        }
        private string GetCampusStatus()
        {
            if (CampusInspection != null && CampusInspection.Count > 0)
            {
                if (CampusInspection.Any(x => x.SubStatus == "RA" || x.SubStatus == "DE" || x.SubStatus == "PD"))
                {
                    return "PD";
                }
                else if(CampusInspection.Any(x => x.SubStatus == "IN"))
                {
                    return "IN";
                }
                else if (CampusInspection.Any(x => x.SubStatus == "GP"))
                {
                    return "IN";
                }
                else if (CampusInspection.Any(x => x.SubStatus == "NA"))
                {
                    return "NA";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }
        public EPSteps()
        {
            MainGoal = new List<MainGoal>();
            Inspection = new Inspection();
            EPStep = new List<EPSteps>();
            LastInspection = new Inspection();
            LastInspectionSummary = new List<InspectionReport>();
            Campus = new List<Buildings>();
            InspectionActivities = new List<TInspectionActivity>();
            epdocument = new List<InspectionEPDocs>();
            epdetials = new EPDetails();
            CampusInspection= new List<Inspection>();
        }


    }
}