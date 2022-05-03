using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HCF.BDO
{

    [DataContract]
    public class TCeilingPermitDetail
    {
        [DataMember]
        public int TCeilingPermitDetailId { get; set; }

        [DataMember]
        public int? CeilingPermitId { get; set; }

        [DataMember]
        public int? CeilingFormId { get; set; }

        [DataMember]
        public bool RespondStatus { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public TPermitLinkForms TPermitLinkForms { get; set; }
        public List<TPermitMapping> TPermitMapping { get; set; }
        public TCeilingPermitDetail()
        {
            TPermitMapping = new List<TPermitMapping>();

        }
    }
}