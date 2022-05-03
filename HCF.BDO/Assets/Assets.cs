using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


namespace HCF.BDO
{
    [DataContract]
    public class Assets
    {
        public Assets()
        {
            StandardEps = new List<StandardEps>();
            Users = new List<UserProfile>();
            EPdetails = new List<EPDetails>();
            TFloorAssets = new List<TFloorAssets>();
            AssetFrequency = new List<FrequencyMaster>();
        }
        [DataMember]
        public int EPDetailId { get; set; }

        [DataMember]
        public List<MainGoal> MainGoals { get; set; }

        [DataMember]
        [Key]
        public int AssetId { get; set; }

        [DataMember]
        public int AssetTypeId { get; set; }

        [DataMember]
        [Display(Name = "Asset")]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsActive { get; set; }

        [DataMember]
        public string IconPath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileContent { get; set; }

        public string Version { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        [DataMember]
        public bool IsStepsOnReport { get; set; }

        [DataMember]
        public bool IsRouteInsp { get; set; }

        [DataMember]
        public int Count { get; set; }

        public int StepsCount { get; set; }

        [DataMember]
        [Display(Name="Asset Code")]
        public string AssetCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public UserProfile UserProfile { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public AssetType AssetType { get; set; }

        [DataMember]
        [NotMapped]
        public IEnumerable<UserProfile> Users { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Vendors> Vendors { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<TFloorAssets> TFloorAssets { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public TFloorAssets TFloorAsset { get; set; }

        [NotMapped]
        [DataMember(EmitDefaultValue = false)]
        public List<EPDetails> EPdetails { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<AssetsSubCategory> AssetSubCategory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<StandardEps> StandardEps { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FrequencyMaster> AssetFrequency { get; set; }

        
    }

    [DataContract]
    public class FireExtinguisherTypes
    {
        [DataMember][Key]
        public int FETypeId { get; set; }

        [DataMember]
        public int AscId { get; set; }

        [DataMember]
        public string FeType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

    }
}