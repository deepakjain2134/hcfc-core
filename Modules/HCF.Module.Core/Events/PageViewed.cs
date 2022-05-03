using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Events
{
    public class PageViewed : INotification
    {
        public long PageId { get; set; }

        public string PageTypeId { get; set; }
    }
}
