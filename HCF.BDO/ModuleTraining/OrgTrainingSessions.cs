using HCF.BDO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.ModuleTraining
{
    [DataContract]
    public class OrgTrainingSessions
    {
        [DataMember] 
        public int Id { get; set; }

        [DataMember] 
        public long ModuleSessionId { get; set; }

        [DataMember] 
        public Guid OrgKey { get; set; }

        [DataMember] 
        public DateTime? Date { get; set; }
        [DataMember]
        public int ClientNo { get; set; }



        [DataMember] public string Participants { get; set; }

        [DataMember] public bool IsCurrent { get; set; }

        [DataMember] public string Comments { get; set; }

        [DataMember] public int? CreatedBy { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }
        [DataMember]   public TrainingSessionStatus Status { get; set; }
 
        [DataMember]
        public TrainingSessionMaster TrainingSessionMaster  { get; set; }

        public List<UserProfile> allUsers { get; set; }

    }
}
