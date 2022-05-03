using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TInspectionFiles
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int TInsFileId { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string FileType { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public string FileExtension
        {
            get { return !string.IsNullOrEmpty(FileName) ? FileName.Split('.')[1] : string.Empty; }
            set { }
        }

        [DataMember]
        public int? StepsId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

    }
}