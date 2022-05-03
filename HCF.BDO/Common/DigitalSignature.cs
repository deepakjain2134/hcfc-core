using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class DigitalSignature
    {
        [DataMember][Key]
        public int DigSignatureId { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public string Signee { get; set; }

        [DataMember]
        public int? TRoundId { get; set; }

        [DataMember]
        public int? TConstId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsSignOverride { get; set; }
        [DataMember]
        public DateTime LocalSignDateTime { get; set; }
        [DataMember]
        public string SignByUserName { get; set; }

        [DataMember]
        public string SignByEmail { get; set; }
    }

}