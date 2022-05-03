using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.Infrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class Organization : EntityBase
    {
        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        [Key]
        public int OrganizationId { get; set; }

        [DataMember]
        [NotMapped]
        public string UserDomain { get; set; }

        [DataMember]
        [NotMapped]
        public string CRxInboxMailId { get; set; }

        [DataMember]
        public string Timezone { get; set; }

        public string Code { get; set; }

        [DataMember]
        [NotMapped]
        public string NoticeText { get; set; }

        [DataMember]
        public bool IsTmsWo { get; set; }

        [DataMember]
        public int ClientNo { get; set; }

        [DataMember]
        public Guid Orgkey { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string City { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string State { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public StateMaster StateMaster { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Country { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Zip { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        
       // [ForeignKey("UserProfile")]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public virtual UserProfile User { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<Menus> Services { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Category> Category { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Status> Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Score> Scores { get; set; }

        [DataMember]
        public bool IsOutpatient { get; set; }

        [DataMember]
        public string DashBoadImagePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string FilesContent { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<UserGroup> UserGroups { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<BuildingType> BuildingTypes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Roles> Roles { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LogoPath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LogoName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public string LogoBase64 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<PopEmails> PopEmails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DbName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Priority> Priority { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Actions> Action { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Cause> Cause { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Department> Department { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Skills> Skills { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Types> Types { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<WoStatus> WoStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<SubStatus> SubStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Item> Item { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Problem> Problem { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Guid? ParentOrgKey { get; set; }

        [DataMember(EmitDefaultValue = false)] 
        public DateTime? InitialCRxIncidentDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TotalBed { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal? HospitalArea { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? WOSysType { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? LastTJCSurvey { get; set; }

        public string Address
        {
            get => (City + ", ") + (State + " ") + Zip + " ";
            set { }
        }




        [DataMember]
        public int SegmentId { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public List<Organization> ChildOrgs { get; set; }

        [DataMember]
        public List<Site> Sites { get; set; }

        [DataMember]
        public List<NotificationCategory> NotificationCategorys { get; set; }

        [DataMember]
        public List<NotificationEvent> NotificationEvents { get; set; }

        [DataMember]
        [NotMapped]
        public int InspectionType { get; set; }

        [DataMember]
        public List<Manufactures> Manufactures { get; set; }

        [DataMember]
        public List<InspResult> InspResults { get; set; }

        [DataMember]
        public List<InspStatus> InspStatus { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int FireWatchTimeSlot { get; set; }

        [DataMember]
        public int HospitalTypeId { get; set; }

        public HospitalType HospitalType { get; set; }

        [DataMember]
        [NotMapped]
        public string ContactDetails { get; set; }
        [DataMember]
        [NotMapped]
        public string MessageToContractor { get; set; }

        [NotMapped]
        public DateTime? LastWoTmsSync { get; set; }

        [DataMember]
        [NotMapped]
        public bool PlanFireDrillsForMe { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsTaggingEnabled { get; set; }

        [DataMember]
        public bool DefaultEPInspection { get; set; }

        [DataMember]
        public DateTime? NonTrackedEPReportDate { get; set; }

        public Organization()
        {
            Services = new List<Menus>();
        }
    }


    [DataContract]
    public class HospitalType
    {
        [DataMember]
        public int HospitalTypeId { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        //[DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        public DateTime CreateDate { get; set; }

    }
   
    [DataContract]
    public class Status
    {

        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string StatusName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Key { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

    }

    [DataContract]
    public class Actions
    {
        [DataMember]
        [Key]
        public int ActionId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember] public bool PlanFireDrillsForMe { get; set; }

    }

    [DataContract]
    public class Cause
    {
        [DataMember]
        public int CauseId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

    }

    [DataContract]
    public class Item
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

    }

    [DataContract]
    public class Problem
    {
        [DataMember]
        public int ProblemId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }

}