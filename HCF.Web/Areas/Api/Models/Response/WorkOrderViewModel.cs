using HCF.BDO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HCF.Web.Areas.Api.Models.Response
{
    public class WorkOrderViewModel
    {

        [DataMember]
        [Key]
        public int? IssueId { get; set; }

        [DataMember]
        [DisplayName("WorkOrder #")]
        public string WorkOrderNumber { get; set; }

        [DisplayName("Priority Code")]
        [DataMember]
        public string PriorityCode { get; set; }

        [DisplayName("Type Code")]
        [DataMember]
        public string TypeCode { get; set; }

        [DisplayName("Building Code")]
        [DataMember]
        public string BuildingCode { get; set; }

        [DisplayName("Status Code")]
        [DataMember]
        public string StatusCode { get; set; }

        [DisplayName("SubStatus Code")]
        [DataMember]
        public string SubStatusCode { get; set; }

        [DisplayName("Requester Email")]
        [DataMember]
        public string RequesterEmail { get; set; }

        [DisplayName("Requester Name")]
        [DataMember]
        public string RequesterName { get; set; }


        [DisplayName("Requester Phone")]
        [DataMember]
        public string RequesterPhone { get; set; }



        [DisplayName("Deficiency Code")]
        [DataMember]
        public string DeficiencyCode { get; set; }


        [Required]
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public DateTime? DateCompleted { get; set; }

        [DataMember]
        public DateTime? LastUpdated { get; set; }


        [DisplayName("Requester Comments")]
        [DataMember]
        public string RequesterComments { get; set; }

        [DisplayName("Completion Comments")]
        [DataMember]
        public string CompletionComments { get; set; }

        public List<WorkOrderFloorAssetsViewModel> Assets { get; set; }



    }
    [DataContract]
    public class WorkOrderFloorAssetsViewModel
    {


        [DataMember]
        public int? IssueId { get; set; }

        [DataMember]
        public int? CMMSReference { get; set; }

    }
}
