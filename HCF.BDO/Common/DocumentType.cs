using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
    [DataContract]
    public class DocumentType
    {
        [DataMember]
        [Key]
        public int DocTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int DocStatus { get; set; }

        [DataMember]
        [DisplayName("Status")]
        public bool IsActive { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int CreatedBy { get; set; }

        [DataMember]
        [DisplayName("Doc Category")]
        public Enums.DocCategory DocCategoryID { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Path { get; set; }

        public string OldName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }

        [DataMember]
        //[DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public DateTime? LastUploadedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? NextDueDateDate { get; set; }

        public string FileUrl => "";//$"{AppSettings.CommonFileBasePath}{Path.Replace("~/", string.Empty)}";

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<EpDocuments> EPDocument { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<TInspectionActivity> TInspectionActivity { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<TDocInspections> TDocInspections { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Status
        {
            get => GetStatus();
            set{ }

        }

        //public int DueWithInDays
        //{
        //    get => GetDueWithInDays();
        //    set{}

        //}

        public int DueWithInDays { get; set; }

        private int GetStatus()
        {
            if (TDocInspections != null && TDocInspections.ToList().Count > 0)
            {
                var tDocInspections = TDocInspections.ToList().FirstOrDefault(x => x.IsCurrent);
                return tDocInspections?.Status ?? -1;
            }
            else
                return -1;
        }

        private int GetDueWithInDays()
        {
            const int diff = 0;
            if (TInspectionActivity != null && TInspectionActivity.ToList().Count > 0)
            {
                var activity = TInspectionActivity.OrderByDescending(x => x.ActivityInspectionDate).FirstOrDefault();
                if (activity?.InspectionEPDocs != null)
                {
                    return activity.InspectionEPDocs.DueWithInDays;
                }
            }
            return diff;
        }

        [DataMember(EmitDefaultValue = false)]
        [NotMapped]
        public List<UserProfile> DocUserProfiles { get; set; }

        public DocumentType()
        {
            DocUserProfiles=new List<UserProfile>();

        }
    }


    [DataContract]
    public class TDocInspections
    {
        [DataMember][Key]
        public int DocInspectionId { get; set; }

        [DataMember]
        public int DocTypeId { get; set; }

        [DataMember]
        public DateTime? ReviewDate { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

    }

}