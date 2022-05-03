using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class THotWorkPermits
    {
        [DataMember][Key]
        public int THotWorkPermitId { get; set; }

        [DataMember]
        public string PermitNo { get; set; }

        [DataMember]
        public int ProjectId { get; set; }


        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string ProjectNumber { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }


        [DataMember]
        public TimeSpan? StartTimeVal { get; set; }

        [DataMember]
        public TimeSpan? EndTimeVal { get; set; }
        [DataMember]
        public string AttachDrawingFiles { get; set; }
        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [DataMember]
        public string PermitRequestor { get; set; }
        [DataMember]
        public UserProfile RequestorByUser { get; set; }
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string OnSiteContact { get; set; }

        [DataMember]
        public string OnSitePhone { get; set; }

        [DataMember]
        public string BuildingId { get; set; }

        [DataMember]
        public string BuildingName { get; set; }

        [DataMember]
        public string FloorId { get; set; }

        [DataMember]
        public string FloorName { get; set; }

        [DataMember]
        public string Zones { get; set; }

        [DataMember]
        public string Rooms { get; set; }

        [DataMember]
        public string WorkType { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int? PermitAuthorizedBy{ get; set; }
        [DataMember]
        public UserProfile AuthorizedByUser { get; set; }

        [DataMember]
        public DateTime? PermitAuthorizedByDate { get; set; }

        [DataMember]
        public int? PermitRequestBy { get; set; }


        [DataMember]
        public DateTime? PermitRequestByDate { get; set; }

        [DataMember]
        public bool? IsVerifyHotWorkPerformed { get; set; }

        [DataMember]
        public bool? IsVerifyObservedrevisited { get; set; }

        [DataMember]
        public bool? IsVerifyAttach { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int UpdatedBy { get; set; }

        [DataMember]
        public DateTime UpdatedDate { get; set; }


        [DataMember]
        public int? PermitRequestorSignatureId { get; set; }

        [DataMember]
        public int? PermitAuthorizedSignatureId { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign1Signature { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign2Signature { get; set; }


        [DataMember]
        public List<THotWorkItems> THotWorkItems { get; set; }

        [DataMember]
        public List<ConstructionWorkType> ConstructionWorkType { get; set; }
        [DataMember]
        public UserProfile PermitRequestUser { get; set; }

        [DataMember]
        public int Status { get; set; }
        public TIcraProject Project { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public TIcraProject TIcraProject { get; set; }
        public THotWorkPermits()
        {
            THotWorkItems = new List<THotWorkItems>();
            ConstructionWorkType = new List<ConstructionWorkType>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
        }

       
    }
}