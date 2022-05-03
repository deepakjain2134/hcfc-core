using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class FilesRepository
    {
        [DataMember]
        [Key]
        public int Id { get; set; }               

        [DataMember]
        public int? DocumentRepoId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public bool IsUsed { get; set; }

        [DataMember]
        public bool IsRejected { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public int? DocTypeId { get; set; }

        [DataMember]
        public int? AttachPolicyCount { get; set; }

        //public string FileUrl => ""; //;$"{AppSettings.S3FileBasePath}{HcfSession.ClientNo + "/"}{FilePath.Replace("~/", string.Empty)}";

        [DataMember]
        public int  DocumentId { get; set; }

        [DataMember]
        public long FileSize { get; set; }

    }
}