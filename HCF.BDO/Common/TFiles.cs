using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HCF.BDO
{
    [DataContract]
    public class TFiles
    {
        [DataMember][Key]
        public int TFileId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public long FileSize { get; set; }

        [DataMember] public Enums.FileType FileType { get; set; }

        [DataMember]
        public int? AttachmentId { get; set; }

        [DataMember]
        public string Name
        {
            get => SetFileName();
            set { }
        }

        private string SetFileName()
        {
            if (!string.IsNullOrEmpty(FileName) && FileName.Contains('.'))
                return FileName.TrimEnd('.').Remove(FileName.LastIndexOf('.'));
            else
                return "";
        }

        [DataMember]
        public string FileContent { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string ModuleCode { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public DateTime UploadedDate { get; set; }

        [DataMember]
        public int UploadedBy { get; set; }

        [DataMember]
        public Guid? ActivityId { get; set; }

        public UserProfile UploadedByUser { get; set; }

        public string FilExtension
        {
            get => SetFilExtension();
            set { }
        }

        private string SetFilExtension()
        {
            if (!string.IsNullOrEmpty(FileName) && FileName.Contains('.'))
                return FileName.Remove(FileName.LastIndexOf('.') + 1);
            else
                return string.Empty;
        }

        public string TblName { get; set; }

        [DataMember]
        public string MD5Digest { get; set; }

        [DataMember]
        public IEnumerable<TFiles> ReleatedFiles { get; set; }
    }



    //[DataContract]
    //public class FileHistory
    //{
    //    [DataMember]
    //    public int FileIdHid { get; set; }

    //    [DataMember]
    //    public string FileName { get; set; }

    //    [DataMember]
    //    public string FilePath { get; set; }

    //    [DataMember]
    //    public DateTime UploadedDate { get; set; }

    //    [DataMember]
    //    public long UploadedDateTimeSpan
    //    {
    //        get => Conversion.ConvertToTimeSpan(UploadedDate);
    //        set { }
    //    }

    //    public string TblName { get; set; }
       

    //    [DataMember]
    //    public int UploadedBy { get; set; }

    //    [DataMember]
    //    public UserProfile UploadedByUser { get; set; }

    //    //[DataMember]
    //    public string FilExtension
    //    {
    //        get => SetFilExtension();
    //        set { }
    //    }

    //    private string SetFilExtension()
    //    {
    //        if (!string.IsNullOrEmpty(FileName) && FileName.Contains('.'))
    //            return FileName.Remove(FileName.LastIndexOf('.') + 1);
    //        else
    //            return "";
    //    }

    //    [DataMember]
    //    public int? EPDetailId { get; set; }

    //    [DataMember]
    //    public long FileSize { get; set; }
    //}

}