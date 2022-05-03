using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO
{
    public class UserFloorRoundCategory
    {
        public int RoundCatId { get; set; }
        public string CategoryName { get; set; }
        public bool IsAdditional { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
