using System;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class TDeviceTesting
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public int? IssueId { get; set; }

        [DataMember] public string Equipment { get; set; }

        [DataMember] public string Address { get; set; }

        [DataMember] public string XMLType { get; set; }

        [DataMember] public string XMLUsage { get; set; }

        [DataMember] public string Location { get; set; }

        [DataMember] public string NFPAClassification { get; set; }

        [DataMember] public string HealthClassification { get; set; }

        [DataMember] public string TestMethod { get; set; }

        [DataMember] public string TestResult { get; set; }

        [DataMember] public DateTime? TestDate { get; set; }

        [DataMember] public string DevReading { get; set; }

        [DataMember] public DateTime? DevReadingDate { get; set; }

        [DataMember] public string FactorySetting { get; set; }

        [DataMember] public string DPM { get; set; }

        [DataMember] public string MakeModel { get; set; }

        [DataMember] public string Note { get; set; }

        [DataMember] public string Comment { get; set; }

        [DataMember] public string Site { get; set; }

        [DataMember] public string Building { get; set; }

        [DataMember] public string Floor { get; set; }

        [DataMember] public string Barcode { get; set; }

        [DataMember] public string Panel { get; set; }

        [DataMember] public string Module { get; set; }

        [DataMember] public string Devices { get; set; }

        [DataMember] public DateTime CreatedDate { get; set; }

        [DataMember]
        public WorkOrder WorkOrder { get; set; }

    }

}