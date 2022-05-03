using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace HCF.Web.Areas.Api.Models.Request
{
    public class SaveWorkOrderRequestModel
    {

        //[DataMember]
        //[Key]
        //public int? IssueId { get; set; }
        [Required]
        [DisplayName("Priority Code")]
        [DataMember(EmitDefaultValue = false)]
        public string PriorityCode { get; set; }
        [Required]
        [DisplayName("Type Code")]
        [DataMember(EmitDefaultValue = false)]
        public string TypeCode { get; set; }
        [Required]
        [DisplayName("Building Code")]
        [DataMember(EmitDefaultValue = false)]
        public string BuildingCode { get; set; }

        [Required]
        public string StatusCode { get; set; }

        [Required]
        [DisplayName("SubStatus Code")]
        [DataMember]
        public string SubStatusCode { get; set; }

        [Required]
        [DisplayName("Requester Email")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterEmail { get; set; }
        [Required]
        [DisplayName("Requester Name")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterName { get; set; }

        [Required]
        [DisplayName("Requester Phone")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterPhone { get; set; }
       

        [Required]
        [DisplayName("Deficiency Code")]
        [DataMember(EmitDefaultValue = false)]
        public string DeficiencyCode { get; set; }


        [Required]
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Requester Comments")]
        [DataMember(EmitDefaultValue = false)]
        public string RequesterComments { get; set; }

        public List<WorkOrderFloorAssetsRequestModel> Assets { get; set; }
    }

    public class UpdateWorkOrderRequestModel
    {

        //[DataMember]
        //[Key]
        //public int? IssueId { get; set; }
        [Required]
        [DisplayName("Priority Code")]
        [DataMember(EmitDefaultValue = false)]
        public string PriorityCode { get; set; }
        [Required]
        [DisplayName("Type Code")]
        [DataMember(EmitDefaultValue = false)]
        public string TypeCode { get; set; }
        [Required]
        [DisplayName("Building Code")]
        [DataMember(EmitDefaultValue = false)]
        public string BuildingCode { get; set; }

        [Required]
        public string StatusCode { get; set; }

        [Required]
        [DisplayName("SubStatus Code")]
        [DataMember]
        public string SubStatusCode { get; set; }

        [Required]
        [DisplayName("Deficiency Code")]
        [DataMember(EmitDefaultValue = false)]
        public string DeficiencyCode { get; set; }


        [Required]
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }


        [DisplayName("Completion Comments")]
        [DataMember]
        public string CompletionComments { get; set; }
    }

    public class UpdateTempWorkOrderRequestModel
    {

        //[DataMember]
        //[Key]
        //[Required]
        //public int IssueId { get; set; }

        //[DataMember]
        //[Required]
        //public int WorkOrderId { get; set; }

        [DataMember]
        [Required]
        [DisplayName("WorkOrder #")]
        public string WorkOrderNumber { get; set; }

        [DataMember]
        [Required]
        public string TempWorkOrderNumber { get; set; }
    }


    [DataContract]
    public class WorkOrderFloorAssetsRequestModel
    {
        [DataMember]
        [Required]
        public int? CMMSReference { get; set; }

        //[DataMember]
       
        //public int? AssetId { get; set; }

    }




}
