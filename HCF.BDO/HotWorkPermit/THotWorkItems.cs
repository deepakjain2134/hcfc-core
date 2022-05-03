using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HCF.BDO
{
    [DataContract]
    public class THotWorkItems
    {
        [DataMember][Key]
        public int Id { get; set; }

        [DataMember]
        public int HotWorkPermitId { get; set; }

        [DataMember]
        public string  Item { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public int? ParentId { get; set; }


        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        public int UpdatedBy { get; set; }   


    }
}