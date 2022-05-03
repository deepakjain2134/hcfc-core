using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO.PCRA
{
    public class ProjectDetailsTICRA
    {
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectManager { get; set; }
        [DataMember]
        public string Architect { get; set; }
        [DataMember]
        public int ProjectNumber { get; set; }
        [DataMember]
        public string ProjectLocation { get; set; }
        [DataMember]
        public string ProjectContractor { get; set; }
        [DataMember]
        public string Description { get; set; }

    }
}