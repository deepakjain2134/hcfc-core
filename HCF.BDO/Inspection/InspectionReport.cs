using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO
{
    [DataContract]
    public class InspectionReport
    {
        [DataMember(EmitDefaultValue = false)]
        public string StandardEp
        {
            get => SetStandardEp();
            set { }
        }

        [DataMember(EmitDefaultValue = false)]
        public int EPDetailId { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public bool IsAssetsRequired { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsDocRequired { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int StDescID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string EPNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Standard Standard { get; set; }        

        [DataMember(EmitDefaultValue = false)]
        public List<UserProfile> EPUsers { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Vendors> Vendors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<EPFrequency> EPFrequency { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Assets> Assets { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Year> Years { get; set; }

        public StandardEps StandardEps { get; set; }



        public InspectionReport()
        {           
            EPUsers = new List<UserProfile>();
            EPFrequency = new List<EPFrequency>();
            Assets = new List<Assets>();
            Years = new List<Year>();
            Vendors = new List<Vendors>();
        }

        private string SetStandardEp()
        {
            if (Standard != null)
            {
                return (Standard.TJCStandard ?? string.Empty) + " , " + (EPNumber ?? string.Empty);
            }
            else return string.Empty;
        }
    }


    [DataContract]
    public class Year
    {
        [DataMember(EmitDefaultValue = false)]
        public List<Months> Months { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int YearId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }
    }

    [DataContract]
    public class Months
    {
        [DataMember(EmitDefaultValue = false)]
        public List<Inspection> Inspections { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int MonthId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        //[DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }
    }


}