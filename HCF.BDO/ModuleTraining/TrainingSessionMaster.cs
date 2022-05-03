using HCF.BDO.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO.ModuleTraining
{
    [DataContract]
    public class TrainingSessionMaster
    {
        [Key]
        [DataMember]
        public long ModuleSessionId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [DataMember]
        public int Sequence { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsCurrent { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        public List<OrgTrainingSessions> TrainingSessions { get; set; }

        public OrgTrainingSessions TrainingSession
        {
            get { return GetCurrentTrainingSession(); }
            set { }
        }

        private OrgTrainingSessions GetCurrentTrainingSession()
        {
            if(TrainingSessions.Count > 0) 
                return TrainingSessions.FirstOrDefault(x => x.IsCurrent);
            else
            return new OrgTrainingSessions();
        }

        public TrainingSessionStatus GetOrgTrainingStatus()
        {
            if (TrainingSessions.Count > 0 )
            {
                return TrainingSessions.FirstOrDefault(x=>x.IsCurrent).Status;
            }
            else
                return TrainingSessionStatus.NotStarted;

        }
        public TrainingSessionMaster()
        {
            TrainingSessions = new List<OrgTrainingSessions>();
        }

    }
}
