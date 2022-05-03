using HCF.BDO;
using HCF.Web.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels.Round
{
    public class RoundViewModel
    {
        public string ReplacingUserName { get; set; }

        public IList<UserProfile> userList { get; set; }

        public RoundVolunteer roundVolunteer { get; set; }
        public IList<RoundGroup> roundLocationGroup { get; set; }

        public IList<RoundGroup> Roundgrouplist { get; set; }

        public RoundGroup roundGroup { get; set; }

        public IList<Buildings> buildings { get; set; }

        public IList<RoundCategory> roundCategory { get; set; }

        public TRounds rounds { get; set; }

        public int FloorId { get; set; }

        public int TRoundId { get; set; }

        public List<UserFloorRoundCategory> UserFloorRoundCategory { get; set; }

        public CustomSelectPicker UserSelectPicker { get; set; }
    }
}