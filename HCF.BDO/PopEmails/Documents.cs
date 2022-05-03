using HCF.BDO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
namespace HCF.BDO
{
    [DataContract]
    public class Documents
    {
        public Documents()
        {
            Attachments = new List<FilesRepository>();
        }       

        [DataMember]
        [Key]
        public int DocumentRepoId { get; set; }

        [DataMember]
        public int ParentDocumentRepoId { get; set; }

        [DataMember]
        public bool? IsDeleted { get; set; }

        [DataMember]
        public bool? IsReplied { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        [Display(Name = "Description")]
        public string Details { get; set; }

        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        [Display(Name = "From")]
        public string Sender { get; set; }

        [DataMember]
        public string To { get; set; }

        //[DataMember]
        [Display(Name = "Date")]
        public DateTime ReceivedDate { get; set; }

        [DataMember]
        [Display(Name = "Date")]
        public long ReceivedDateTimespan
        {
            get => Conversion.ConvertToTimeSpan(ReceivedDate);
            set { }
        }

        //[DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }


        [DataMember]
        public int ClientNo { get; set; }


        [DataMember]
        public List<FilesRepository> Attachments { get; set; }

        [DataMember]
        public string RemovedAttach { get; set; }

    }


    [DataContract]
    public class DocumentMaster
    {
        [DataMember]
        public int DocumentId { get; set; }

        [DataMember]
        public int DocTypeId { get; set; }

        [DataMember]
        public int? AttachmentId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int UploadedBy { get; set; }

        //[DataMember]
        public DateTime UploadedDate { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

    }

}