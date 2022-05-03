using System;
using System.Runtime.Serialization;

namespace HCF.Web.Models
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