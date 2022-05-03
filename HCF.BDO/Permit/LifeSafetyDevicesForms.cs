using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class TLifeSafetyDevicesForms
    {
        public TLifeSafetyDevicesForms()
        {
            LifeSafetyDeviceList = new List<LifeSafetyDeviceList>();
            RequestorUser = new UserProfile();
            ApprovedByUser = new UserProfile();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            Attachments = new List<TFiles>();
        }      

        [DataMember]
        public TIcraProject Project { get; set; }

        [DataMember]
        public string ProjectName
        {
            get => GetProjectName();
            set { }
        }

        private string GetProjectName()
        {
            if (Project != null)
                return string.Format("{0} ({1})", Project.ProjectName,Project.ProjectNumber);
            else
                return "";
        }

        [DataMember][Key]
        public Guid LsdFormId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int Status { get; set; }
        
        [DataMember]
        public int FormType { get; set; }

        [DataMember]
        public int LsdFormNo { get; set; }

        [DataMember]
        public bool IsSignDeleted { get; set; }

        [DataMember]
        public bool IsCurrentDeleted { get; set; }


        [DataMember]
        public bool IsMOPSubmission { get; set; }

        [DataMember]
        public bool IsFinalSubmission { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? Requestor { get; set; }

        [DataMember]
        public UserProfile RequestorUser { get; set; }

        [DataMember]
        public DateTime DateOfRequest { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Contractor { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PrintName { get; set; }

        [DataMember]
        public int? ApprovedBy { get; set; }

        [DataMember]
        public UserProfile ApprovedByUser { get; set; }

        [DataMember]
        public int? PermitAuthorizedSignatureId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature PermitAuthorizedSignature { get; set; }

        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public DateTime? SignDate { get; set; }
        [DataMember]
        public bool IsCurrent { get; set; }
        [DataMember]
        public string ParentLsdFormId { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public string PermitNo { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember] public string BuildingId { get; set; }
        [DataMember] public string BuildingName { get; set; }

        [DataMember] public string FloorId { get; set; }
        [DataMember] public string FloorName { get; set; }

        [DataMember] public string Zones { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        public List<LifeSafetyDeviceList> LifeSafetyDeviceList { get; set; }

        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        [DataMember]
        public string DeviceType { get; set; }

        [DataMember]
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public string TFileIds { get; set; }
        public TIcraProject TIcraProject { get; set; }
    }
    [DataContract]
    public class LifeSafetyDeviceList
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Guid LsdFormId { get; set; }
        [DataMember]
        public string DeviceNumber { get; set; }
        [DataMember]
        public string Building { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string AssetType { get; set; }
        [DataMember]
        public string Make { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DateAdded { get; set; }

        [DataMember]
        public DateTime? DateRemoved { get; set; }

        [DataMember]
        public TFloorAssets FloorAsset { get; set; }
        [DataMember]
        public string BuildingId { get; set; }

        [DataMember]
        public int? IsDeleted { get; set; }

    }
}