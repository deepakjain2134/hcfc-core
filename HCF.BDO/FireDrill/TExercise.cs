using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HCF.BDO.Extensions;

namespace HCF.BDO
{
    [DataContract]
    public class TExerciseFiles
    {
        [DataMember][Key]
        public int TExeriseFileId { get; set; }

        [DataMember]
        public int TExerciseId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FilesContent { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string TFileIds { get; set; }

        [DataMember]
        public int DocumentType { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]

        //public int DrillFileType { get; set; }

        public Enums.DrillFileType DrillFileType { get; set; }

        public long CreatedDateTimeSpan
        {
            get => Conversion.ConvertToTimeSpan(CreatedDate);
            set { }
        }

    }    
}