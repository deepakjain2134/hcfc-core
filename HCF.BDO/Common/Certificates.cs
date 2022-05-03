using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class Certificates
    {
        [DataMember]  
        [Key]
        public int CertificateId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? UserId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? VendorId { get; set; }

        [DisplayName("Certificate Name")]
        [DataMember]
        public string CertificateName { get; set; }

        
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DisplayName("Expiration Date")]
        [DataMember(EmitDefaultValue = false)]
        public DateTime? ValidUpTo { get; set; }
       
        [DataMember]
        public long ValidUpToTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(ValidUpTo);
            set { }
        }

        [DataMember]
        public bool? IsExpired { get; set; }

        [DataMember]        
        public bool? IsDeleted { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
        [DisplayName("Issue Date")]
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public DateTime? IssueDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember]
        public int? CertificateType { get; set; }     

        public string UploadedBy { get; set; }

        public string View { get; set; }
    }
}