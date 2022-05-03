using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models
{
    [DataContract]
    public class HttpResponseMessageApp
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ResultApp Result { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int TotalResult { get; set; }

    }

    [DataContract]
    public class ResultApp
    {




        [DataMember(EmitDefaultValue = false)]
        public List<UserProfileApp> Users { get; set; }



        [DataMember(EmitDefaultValue = false)]
        public UserProfileApp User { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<BuildingsApp> Buildings { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<RoundGroupApp> Rounds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<MenuApp> Menus { get; set; }
    }

    public class Files
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }

        public string MessageId { get; set; }

    }
}