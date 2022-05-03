using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Areas.Core.ViewModels.CityMaster
{
    public class CityMasterViewModel
    {
        public int CityId { get; set; }

        public string CityName { get; set; }

        public string CityCode { get; set; }

        public int StateId { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string StateName { get; set; }       

        public int Zipcode { get; set; }
    }
}
