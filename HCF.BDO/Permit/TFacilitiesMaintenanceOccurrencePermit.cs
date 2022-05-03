using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TFacilitiesMaintenanceOccurrencePermit
    {
        [DataMember]
        public int  TfmcId { get; set; }
        [DataMember]
        public string PermitNo { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string AssetId { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public int? DepartmentId { get; set; }
          [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public String PhoneNo { get; set; }
        [DataMember]
        public string Shift { get; set; }
        [DataMember]
        public string ShiftName { get; set; }
        [DataMember]
        public DateTime? DateOfOccurence { get; set; }
        [DataMember]
        public string TimeOfOccurence { get; set; }
        [DataMember]
        public DateTime? ReportDate { get; set; }
        [DataMember]
        public string ClassificationType { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string OccurenceDetails { get; set; }
        [DataMember]
        public string ActionTaken { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public bool AddedToEOC { get; set; }
        [DataMember]
        public int CompletelyResolved { get; set; }

        [DataMember]
        public DateTime? CompleteDate { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public int? UpdatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public int? RequesterSignId { get; set; }

        [DataMember]
        public int? ApproverSignId { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign1Signature { get; set; }
        [DataMember]
        public string Company { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DigitalSignature DSSign2Signature { get; set; }

        [DataMember]
        public UserProfile PermitRequestUser { get; set; }
        [DataMember]
        public UserProfile AuthorizedByUser { get; set; }

        [DataMember]
        public int? RequestedBy { get; set; }
        [DataMember]
        public int? ApprovedBy { get; set; }
        [DataMember]
        public DateTime? RequesterDate { get; set; }
        [DataMember]
        public DateTime? ApprovedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public List<ClassificationType> lstClassificationType { get; set; }
        
        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public TFacilitiesMaintenanceOccurrencePermit()
        {
            lstClassificationType = new List<ClassificationType>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
        }
    }

    [DataContract]
    public class ClassificationType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public int? IsChecked { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

    }

}