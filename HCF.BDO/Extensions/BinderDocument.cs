using HCF.BDO.Extensions;
using System;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace HCF.BDO
{
    [DataContract]
    public class EPsDocument
    {

        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public Guid EPsDocumentId { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int FileId { get; set; }


        [DataMember]
        public Guid ActivityId { get; set; }

        [DataMember]
        public DateTime? DtEffectiveDate { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string BinderName { get; set; }

        [DataMember]
        public int? BinderId { get; set; }

        [DataMember]
        public int? DocTypeId { get; set; }

        [DataMember]
        public string StandardEp { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string DeviceNo { get; set; }
       
        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int EpdetailId { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FullName
        {
            get => (FirstName ?? string.Empty) + " " + (!string.IsNullOrEmpty(LastName) ? LastName.Substring(0, 1) : string.Empty);
            set { }
        }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember]
        public long UploadDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

        [DataMember]
        public long DtEffectiveDateTimeSpan
        {
            get => DtEffectiveDate == null ? 0 : Conversion.ConvertToTimeSpan(DtEffectiveDate);
            set { }
        }

        public DateTime? DueDate { get; set; }

        [DataMember]
        public int? UploadDocTypeId { get; set; }

        public bool isShowNextDate => ShowNextDate();

        private bool ShowNextDate()
        {
            bool status = false;
            if (UploadDocTypeId.HasValue)
            {
                if (UploadDocTypeId.Value == (int)Enums.UploadDocTypes.Policy || UploadDocTypeId.Value == (int)Enums.UploadDocTypes.Report)
                    status = true;
            }
            return status;
        }

        public bool IsRequiredDoc => SetRequiredDoc();

        private bool SetRequiredDoc()
        {
            var status = true;
            if (DocTypeId.HasValue)
            {
                if (DocTypeId.Value == (int)Enums.UploadDocTypes.Policy ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.Report ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.AssetsReport ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.MiscDocuments ||
                    DocTypeId.Value == (int)Enums.UploadDocTypes.SampleDocument)
                    status = false;
            }
            return status;
        }

        [DataMember]
        public long DueDateDateTimeSpan
        {
            get => DueDate == null ? 0 : Conversion.ConvertToTimeSpan(DueDate);
            set { }
        }

        [DataMember]
        public bool IsDeleted { get; set; }

        public int? DueWithIn { get; set; }
    }
}