using HCF.BDO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{
    public class RoundGroupApp
    {
        [DataMember]
        [Key]
        public bool RoundGroup
        {
            get
            {
                return SetRoundType();
            }
            set
            {
                RoundGroup = value;
            }

        }
        [DataMember]
        [Key]
        public int RoundGroupId { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RoundName { get; set; }

        [DataMember]
        public DateTime RoundDate { get; set; }

        [DataMember]
        public long RoundDatetimespan
        {
            get => HCF.Utility.Conversion.ConvertToTimeSpan(RoundDate);
            set { }
        }


        [DataMember]
        public bool isRoundVolunteer { get; set; }


        [DataMember]
        public bool InspectionDone { get; set; }

        [DataMember]
        public String Names { get; set; }
        [DataMember]
        public List<string> RoundGroupNames { get; set; }
        [DataMember]
        public List<UserDetaileApp> GroupUsers { get; set; }
        [DataMember]
        public List<RoundCategory> RoundCategory { get; set; }
        [DataMember]
        public List<RoundSchedules> RoundSchedules { get; set; }

        [DataMember]
        public List<BuildingsApp> Locations { get; set; }
        [DataMember]

        public List<TRoundInspections> Inspections { get; set; }

        [DataMember]
        public int RoundType { get; set; }

        private bool SetRoundType()
        {

            return (RoundType == 2 ? true : false);
        }


        public RoundGroupApp()
        {
             GroupUsers = new List<UserDetaileApp>();
            RoundCategory = new List<RoundCategory>();
            RoundSchedules = new List<RoundSchedules>();
            Locations = new List<BuildingsApp>();
            Inspections = new List<TRoundInspections>();
        }
    }


}