using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BDO
{
    [DataContract]
    public class TAssetDeviceChangeForms
    {
        public TAssetDeviceChangeForms()
        {
            AssetDeviceList = new List<AssetDeviceList>();
            RequestorUser = new UserProfile();
            DrawingAttachments = new List<DrawingpathwayFiles>();
            Attachments = new List<TFiles>();
            AssetDevicePathSettings = new List<AssetDevicePathSettings>();
            TPermitWorkFlowDetails = new List<TPermitWorkFlowDetails>();
        }

        [DataMember]
        public TIcraProject Project { get; set; }

        [DataMember]
        public string ProjectName
        {
            get => GetProjectName();
            set { }
        }

        private string GetProjectName()
        {
            if (Project != null)
                return string.Format("{0} ({1})", Project.ProjectName, Project.ProjectNumber);
            else
                return "";
        }

        [DataMember]
        [Key]
        public Guid AdcFormId { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public int FormType { get; set; }

        [DataMember]
        public int AdcFormNo { get; set; }

        [DataMember]
        public bool IsSignDeleted { get; set; }

        [DataMember]
        public bool IsCurrentDeleted { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? Requestor { get; set; }

        [DataMember]
        public UserProfile RequestorUser { get; set; }

        [DataMember]
        public DateTime DateOfRequest { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Contractor { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PrintName { get; set; }


        public bool IsCurrent { get; set; }
        [DataMember]
        public string ParentAdcFormId { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public string PermitNo { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember] public string BuildingId { get; set; }
        [DataMember] public string BuildingName { get; set; }

        [DataMember] public string FloorId { get; set; }
        [DataMember] public string FloorName { get; set; }

        [DataMember] public string Zones { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        public List<AssetDeviceList> AssetDeviceList { get; set; }

        [DataMember]
        public string TDrawingFields { get; set; }
        [DataMember]
        public List<DrawingpathwayFiles> DrawingAttachments { get; set; }
        [DataMember]
        public int DeviceType { get; set; }
        [DataMember]
        public int Path { get; set; }
        [DataMember]
        public List<TFiles> Attachments { get; set; }
        [DataMember]
        public string AttachFiles { get; set; }
        [DataMember]
        public string TFileIds { get; set; }
        public TIcraProject TIcraProject { get; set; }
        public List<TPermitWorkFlowDetails> TPermitWorkFlowDetails { get; set; }
        public List<UserProfile> lstUserProfile { get; set; }
        public List<AssetDevicePathSettings> AssetDevicePathSettings { get; set; }
    }
    [DataContract]
    public class AssetDeviceList
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Guid AdcFormId { get; set; }
        [DataMember]
        public string DeviceNumber { get; set; }
        [DataMember]
        public string CMMSAssetNumber { get; set; }
        [DataMember]
        public string FireAlarmDeviceNumber { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string RoomNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DateAdded { get; set; }

        [DataMember]
        public DateTime? DateRemoved { get; set; }

        [DataMember]
        public DateTime? DateRelocation { get; set; }

        [DataMember]
        public TFloorAssets FloorAsset { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string ModelNumber { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public int? IsDeleted { get; set; }

        [DataMember]
        public string AssetType { get; set; }

    }
    public class AssetDevices
    {
        [DataMember]
        public int AssetFormId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int DeviceType { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }


    }

    public class AssetDevicePathSettings
    {
        [DataMember]
        public int PathId { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public bool Description { get; set; }
        [DataMember]
        public bool IsActive { get; set; }



    }
}