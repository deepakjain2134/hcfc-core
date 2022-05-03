using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCF.BDO
{
[DataContract]
    public class TIcraProject
    {
        [DataMember][Key]
        public int ProjectId { get; set; }

        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string ProjectNumber { get; set; }

        [DataMember]
        public string ProjectLocation { get; set; }

        [DataMember]
        public string ProjectContractor { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public int? ParentProjectId { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string ProjectManager { get; set; }
        [DataMember]
        public string Architect { get; set; }

        [DataMember]
        [NotMapped]
        public List<TIcraProject> SubProject { get; set; }

        [DataMember]
        public int? TIcraReportId { get; set; }

        [DataMember]
        public DateTime? ProjectStartDate { get; set; }

        [DataMember]
        public int? ProjectType { get; set; }
        [DataMember]
        public string  ProjectTypeName { get; set; }

        [DataMember]
        public DateTime? ProjectEndDate { get; set; }

        [DataMember]
        public int ICRACount { get; set; }
        [DataMember]
        public int PCRACount { get; set; }
        [DataMember]
        public int FSBPCount { get; set; }
        [DataMember]
        public int MOPCount { get; set; }
        [DataMember]
        public int HWPCount { get; set; }
        [DataMember]
        public int LSDCount { get; set; }
        [DataMember]
        public int TPaperPermitCount { get; set; }
        [DataMember]
        public int CRACount { get; set; }

        [DataMember]
        public int CPCount { get; set; }
        public string mode { get; set; }
        [DataMember]
        public string TFileIds { get; set; }
        [DataMember]
        public string TDrawingFields { get; set; }

        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public string AttachDrawingFiles { get; set; }
        [DataMember]
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        public List<TIcraProject> icraProjectList { get; set; }

        public TIcraProject()
        {

            Attachments = new List<TFiles>();
            DrawingAttachments = new List<DrawingpathwayFiles>();
        }
    }

   
}