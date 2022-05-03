using HCF.BDO.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TInspectionReport
    {
        [DataMember]
        public int ReportId { get; set; }

        [DataMember]
        public string ReportName { get; set; }

        [DataMember]
        public string SignImageName { get; set; }

        [DataMember]
        public string SignPath { get; set; }

        [DataMember]
        public string SignContent { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FilesContent { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }


        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public UserProfile Signby { get; set; }

        [DataMember]
        public string BinderName { get; set; }

        [DataMember]
        public List<TInsReportDetails> TInsReportDetails { get; set; }
    }
}