using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO
{
   public class DocumentSpaceStatus
    {
        public long TotalSpace { get; set; }

        public long UsedSpace { get; set; }

        public int DocumentType { get; set; }
        public long RemainingSpace { get; set; }
    }
}
