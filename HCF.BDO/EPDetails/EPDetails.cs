using HCF.BDO.Enums;
using HCF.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;


namespace HCF.BDO
{
    [DataContract]
    public class EPDetails : EntityBase
    {
        [NotMapped]
        public override long Id { get; protected set; }

        public EPDetails()
        {
            EPUsers = new List<UserProfile>();
            Inspections = new List<Inspection>();
            MainGoal = new List<MainGoal>();
            TInspectionActivity = new List<TInspectionActivity>();
            Assets = new List<Assets>();
            InspectionEPDocs = new List<InspectionEPDocs>();         
            DocumentType = new List<DocumentType>();
            Schedules = new List<Schedules>();
            EPDescriptions = new List<EPDescriptions>();
            EPFrequency = new List<EPFrequency>();
            CopDetails = new List<CopDetails>();
            Inspection = new Inspection();
            EPsDocument = new List<EPsDocument>();
        }

        public List<EPsDocument> EPsDocument { get; set; }

       // public RiskScore RiskScore { get; set; }

        [NotMapped]
        public DateTime? InitialInspDate { get; set; }

        [NotMapped]
        public bool IsInspectionDateFixed
        {
            get => InitialInspDate.HasValue;
            set { }
        }

        [NotMapped]
        public bool IsUpComingEp
        {
            get => SetIsUpComingEp();
            set { }
        }

        public DateTime? GetFloorAssetLastInspectionDate(List<Inspection> inspectionList, int floorAssetId, int epId)
        {
            if (inspectionList == null) return null;
            var inpection = inspectionList.FirstOrDefault(x => x.EPDetailId == epId && x.IsCurrent);
            if (inpection != null)
            {
                var inspectionActivity = inpection.TInspectionActivity?.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.IsCurrent);
                if (inspectionActivity != null && inspectionActivity.ActivityInspectionDate != null)
                    return inspectionActivity.ActivityInspectionDate.Value;
            }
            return null;
        }

        public DateTime? GetFloorAssetNextInspectionDate(List<Inspection> inspectionList, int floorAssetId, int epId)
        {
            if (inspectionList.Count>0 &&  floorAssetId>0  && epId >0)
            {
                var inpection = inspectionList.FirstOrDefault(x => x.EPDetailId == epId && x.IsCurrent);
                if (inpection == null && inpection.TInspectionActivity != null)
                {

                    var inspectionactivity = inpection.TInspectionActivity.FirstOrDefault(x => x.FloorAssetId == floorAssetId && x.IsCurrent);
                    if (inspectionactivity != null && inspectionactivity.DueDate.HasValue)
                        return inspectionactivity.DueDate.Value;
                }
            }
            return null;
        }

        public bool SetIsUpComingEp()
        {
            bool status = false;
            if (EffectiveDate != null)
            {
                if (EffectiveDate> DateTime.UtcNow)
                    return true;
            }
            return status;
        }

        [NotMapped]
        public int Sequence
        {
            
            get => SetSequence();
            set { }
        }
        private int SetSequence()
        {
            int sequence = 0;
            int res = 0; int res2 = 0;
            try
            {
                string newEPNumber = EPNumber?.ToLower().Replace("EP".ToLower(), "").TrimStart(new Char[] { '0' });
                int charCounter = Regex.Matches(newEPNumber, @"[a-zA-Z]").Count;
                if (charCounter > 0)
                {
                    Regex re = new Regex(@"(\d+)([a-zA-Z]+)");
                    Match result = re.Match(newEPNumber.ToLower());
                    if (!string.IsNullOrEmpty(result.Groups[1].Value))
                        res = Convert.ToInt32(result.Groups[1].Value) * 10;
                    if (!string.IsNullOrEmpty(result.Groups[2].Value))
                    {
                        foreach (char c in result.Groups[2].Value)
                            res2 = (int)c;
                        res2 = res2 - 96;
                    }
                }
                else
                    res = Convert.ToInt32(newEPNumber) * 10;
            }
            catch (Exception ex)
            {
                //Utility.ErrorLog.LogMsg("BDO.EPDetails" + ex.Message);
            }
            sequence = res + res2;
            return sequence;
        }

        [DataMember]
        public int Priority { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string StandardEp
        {
            get => SetStandardEp();
            set { }
        }        

        [Key]
        [DataMember(EmitDefaultValue = false)]
        public int EPDetailId { get; set; }       

        [DataMember(EmitDefaultValue = false)]
        [ForeignKey(nameof(Standard))]
        public int StDescID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string EPNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CmsEPNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int ScoreId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string CmsDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool IsDocRequired { get; set; }

        [DataMember]
        public bool IsAssetsRequired { get; set; }

        [DataMember]
        public bool IsFrequencyChange { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsApplicable { get; set; }

        public string ReasonforNonTracked { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsExpired { get; set; }

        public string Version { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember]
        [Display(Name = "Effective Date")]
        public DateTime? EffectiveDate { get; set; }

        [DataMember]
        [Display(Name = "Effective Date")]
        public DateTime? LastUpdatedDate { get; set; }


        [DataMember]
        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [NotMapped]
        public EpStateCode EpState
        {
            get => SetEPState();
            set { }
        }

        private EpStateCode SetEPState()
        {
            if (EffectiveDate.HasValue && EffectiveDate.Value > DateTime.Now.Date)
                return EpStateCode.UpComing;
            else if (ExpiryDate.HasValue && ExpiryDate < DateTime.Now.Date)
                return EpStateCode.Expired;
            else if (!IsActive)
                return EpStateCode.TJCInActive;
            else if (!IsApplicable)
                return EpStateCode.NotApplicable;
            else if (!CanInspect())
                return EpStateCode.InspectionDisabled;
            else
               return EpStateCode.ACTIV;
        }        

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string EpTransStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int EpTranStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int EPStatus { get; set; }       

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int DocStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Standard Standard { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Score Scores { get; set; }

        [DataMember]
        public bool IsIlsmEP { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<MainGoal> MainGoal { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<DocumentType> DocumentType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<Binders> Binders { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<Assets> Assets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<UserProfile> EPUsers { get; set; }        

        [DataMember]
        [NotMapped]
        public List<Schedules> Schedules { get; set; }       

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public Inspection Inspection { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<Inspection> Inspections { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<TInspectionActivity> TInspectionActivity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<InspectionEPDocs> InspectionEPDocs { get; set; }      

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public DateTime? InspectionDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<TInspectionDates> InspectionDates { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<CopDetails> CopDetails { get; set; }

        private string SetStandardEp()
        {
            if (Standard != null)
            {
                return (Standard.TJCStandard ?? string.Empty) + "," + (EPNumber ?? string.Empty);
            }
            else return string.Empty;
        }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public int? IsAssetSteps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<EPsDocument> EpDocs { get; set; }

        [NotMapped]
        public int DueWithInDays { get; set; }

        [DataMember]
        [NotMapped]
        public bool InitialInpDateEditable { get; set; }

        [DataMember]
        public List<EPDescriptions> EPDescriptions { get; set; }

        public string GetFrequencyName()
        {
            var frequency = GetFrequency();
            if (frequency != null)
                return frequency.DisplayName;

            return string.Empty;
        }

        public FrequencyMaster GetFrequency()
        {
            if (Frequency != null)
                return Frequency;

            var epFrequency = EPFrequency.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (epFrequency != null && epFrequency.Frequency != null)
                return epFrequency.Frequency;

            return null;
        }

        [DataMember]
        [NotMapped]
        public FrequencyMaster Frequency { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<EPFrequency> EPFrequency { get; set; }

        public bool CanInspect()
        {
            if (Frequency != null && Frequency.Days == 0)
                return false;

            var epFrequency = EPFrequency.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (epFrequency?.Frequency != null && epFrequency.Frequency.Days == 0)
                return false;

            return true;
        }

        [NotMapped]
        public int FrequencyId { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]       
        public bool IsCustomFrequency { get; set; }
       
        [NotMapped]
        [DataMember(EmitDefaultValue = false)]        
        public bool IsNewEp { get; set; }

        public bool IsCurrent { get; set; }

        [DataMember]
        [NotMapped]
        public int CopDetailsId { get; set; }

        [DataMember]
        public bool IsRelation { get; set; }

        [DataMember]
        [NotMapped]
        public int PendingRelationEpCount { get; set; }

        [DataMember]
        [NotMapped]
        public string EPSearchText { get; set; }
    }


    [DataContract]
    public class StandardEps
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string StandardEP { get; set; }

        [DataMember]
        public string TJCStandard { get; set; }

        [DataMember]
        public string EPNumber { get; set; }

        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int StDescID { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsApplicable { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? EPGroupId { get; set; }

        [DataMember]
        public string TJCDescription { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MainGoal> MainGoals { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? BinderId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BinderName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public UserProfile UserProfile { get; set; }

        [DataMember]
        [NotMapped]
        public string EPSearchText { get; set; }
    }




    [DataContract]
    public class EPDescriptions
    {
        [DataMember][Key]
        public int EPDescriptionId { get; set; }

        [DataMember]
        public int? EPDetailId { get; set; }

        [DataMember]
        public int? HospitalTypeId { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Description { get; set; }

    }


}