using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.Core.Models
{
    public class UserNavigation
    {

        [DataMember]
        public string CurrentAbsolutePath { get; set; }

        [DataMember]
        public string Screen { get; set; }

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public string PageUrl { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UrlReferrer { get; set; }

        [DataMember]
        public string PageMode { get; set; }

        [DataMember]
        public string SortOrder { get; set; }

        [DataMember]
        public string OrderbySort { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

    }
}
