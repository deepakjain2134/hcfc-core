using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class DashboardDetails
    {
        [DataMember]
        public int EpDetailId { get; set; }
        [DataMember]
        public int StDescId { get; set; }
        [DataMember]
        public int? UserId { get; set; }
        [DataMember]
        [Display(Name = "Standard , EP")]
        public string StandardEP { get; set; }
        [DataMember]
        public bool IslmFail { get; set; }
        [DataMember]
        public bool IsRAFail { get; set; }
        [DataMember]
        public bool IsTrigger { get; set; }
        [DataMember]
        public string Score { get; set; }
        [DataMember]
        public int ScoreId { get; set; }
        [DataMember]
        public string Assets { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? IsInspReady { get; set; }
        [DataMember]
        public string AssetId { get; set; }
        [DataMember]
        public string FrequencyDisplayName { get; set; }
        [DataMember]
        public int FrequencyId { get; set; }
        [DataMember]
        public int Days { get; set; }

        [DataMember]
        public int? AssigneeId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? IsAllDocumentUploaded { get; set; }

        [DataMember]
        public string DueDate { get; set; }

        [DataMember]
        public bool IsDeficiency { get; set; }

        [DataMember]
        [Display(Name = "Inspection Date")]
        public string LastInspectionDate { get; set; }

        [DataMember]
        public string DOC { get; set; }

        [DataMember]
        public string GraceDate { get; set; }

        [DataMember]
        public string GracePeriod { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDocRequired { get; set; }

        [DataMember]
        public string StatusText { get; set; }

        [DataMember]
        public int InspectionId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string Code { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        //public List<InspectionEPDocs> inspectionDocs { get; set; }

        [DataMember]
        public int ReportId { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string SubStatus { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
                
        [DataMember]
        [Display(Name = "User Name")]
        public string UserFullName
        {
            get =>
                (FirstName ?? "") + " " +
                (!string.IsNullOrEmpty(LastName) ? LastName.Substring(0, 1) : string.Empty);
            set { }
        }
    }
}