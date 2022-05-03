using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class WorkOrderfiles
    {
        [DataMember][System.ComponentModel.DataAnnotations.Key]
        public int WorkOrderFileId { get; set; }

        [DataMember]
        public int IssueId { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FileContent { get; set; }
    }
}