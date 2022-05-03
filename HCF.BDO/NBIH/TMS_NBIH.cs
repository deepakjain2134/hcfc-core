using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.BDO.NBIH
{
    public class CustomField
    {
        public int FieldId { get; set; }
        public int ParentFieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public object ValueDescription { get; set; }
        public bool IsCode { get; set; }
    }

    public class TMSNBIAssetsResults
    {
        public int AssetID { get; set; }
        public string AssetNumber { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryDescription { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryDescription { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string SkillCode { get; set; }
        public string SkillDescription { get; set; }
        public string PriorityCode { get; set; }
        public string PriorityDescription { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public string ShopCode { get; set; }
        public string ShopDescription { get; set; }
        //public DateTime? DatePurchased { get; set; }
        //public DateTime? DateReceived { get; set; }
        //public DateTime? DateAccepted { get; set; }
        public double OriginalCost { get; set; }
        public double ReplacementCost { get; set; }
        public string IDNumber { get; set; }
        public string ItemIdentifier { get; set; }
        public string AreaCode { get; set; }
        public string AreaDescription { get; set; }
        public bool ServiceManual { get; set; }
        public string ManualLocation { get; set; }
        public string NetworkDevice { get; set; }
        public string MeterTypeCode { get; set; }
        public string MeterTypeDescription { get; set; }
        public double MeterReading { get; set; }
        public string PhysicalConditionCode { get; set; }
        public string PhysicalConditionDescription { get; set; }
        public string Owner { get; set; }
        public string SupportStatusCode { get; set; }
        public string SupportStatusDescription { get; set; }
        //public DateTime? DateRetired { get; set; }
        public string WarrantyCompanyName { get; set; }
        public string WarrantyCompanyCode { get; set; }
        //public DateTime? WarrantyStartDate { get; set; }
        //public DateTime? WarrantyEndDate { get; set; }
        public string OriginalPONumber { get; set; }
        public string Department { get; set; }
        public string BuildingDescription { get; set; }
        public string Room { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string UDF7 { get; set; }
        public string UDF8 { get; set; }
        public string UDF9 { get; set; }
        public string UDF10 { get; set; }
        public string ExtendedDescription { get; set; }
        public string Options { get; set; }
        public string SoftwareTest { get; set; }
        public double CostBasis { get; set; }
        public double SalvageValue { get; set; }
        public double LifeExpectancy { get; set; }
        //public DateTime? StartingDate { get; set; }
        public int FirstMonth { get; set; }
        public string RiskNumber { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string HIPAACode { get; set; }
        public string HIPAADescription { get; set; }
        public DateTime? NextPMDate { get; set; }
        public int SegmentID { get; set; }
        public int FieldId { get; set; }
        public bool ValidateForDataImport { get; set; }
        public int KeyID { get; set; }
        public List<CustomField> CustomFields { get; set; }
    }

    public class TMSNBIPriorityResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public bool IsNewHierarchicalCode { get; set; }
        public int SegmentID { get; set; }
        public int FieldId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public bool ValidateForDataImport { get; set; }
        public int KeyID { get; set; }
        public object CustomFields { get; set; }

    }

    public class TMSNBIAccountResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
        public int SegmentID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentDescription { get; set; }
        public object CustomFields { get; set; }


    }

    public class TMSNBISitesResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public int SegmentID { get; set; }



    }

    public class TMSNBITypeResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public int SegmentID { get; set; }
        public int Escalate { get; set; }
        public string ModuleCode { get; set; }

    }

    public class TMSNBIStatusResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public bool IsNewHierarchicalCode { get; set; }
        public int SegmentID { get; set; }
        public int FieldId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public bool ValidateForDataImport { get; set; }
        public int KeyID { get; set; }
        public object CustomFields { get; set; }
    }

    public class TMSNBIBuildingsResults
    {
        public string SiteCode { get; set; }
        public string SiteDescription { get; set; }
        public int NumberOfBeds { get; set; }
        public double SquareFootage { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public bool IsNewHierarchicalCode { get; set; }
        public int SegmentID { get; set; }
        public int FieldId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public bool ValidateForDataImport { get; set; }
        public int KeyID { get; set; }
        public object CustomFields { get; set; }
    }

    public class TMSNBIResourceResults
    {
        public int ResourceID { get; set; }
        public string ResourceNumber { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string WorkPhone { get; set; }
        public string WorkPager { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public string ShopCode { get; set; }
        public string ShopDescription { get; set; }
        public string AreaCode { get; set; }
        public string AreaDescription { get; set; }
        public string SkillCode { get; set; }
        public string SkillDescription { get; set; }
        public string SkillLevelCode { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftDescription { get; set; }
        public double ChargeRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastReviewDate { get; set; }
        public string Email { get; set; }
        public string PagerEmail { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string UserField4 { get; set; }
        public string UserField5 { get; set; }
        public string UserField6 { get; set; }
        public string UserField7 { get; set; }
        public string UserField8 { get; set; }
        public string UserField9 { get; set; }
        public string UserField10 { get; set; }
        public string Notes { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int SegmentID { get; set; }
        public int FieldId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public bool ValidateForDataImport { get; set; }
        public int KeyID { get; set; }
        public List<object> CustomFields { get; set; }
    }

    public class TMSNBISubStatusResults
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
        public int SegmentID { get; set; }
        public string ModuleCode { get; set; }
    }

    public class TMSNBISLocationResults
    {
        public int ID { get; set; }
        public int SegmentID { get; set; }
        public string SiteCode { get; set; }
        public string SiteDescription { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingDescription { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Show { get; set; }
        public bool ShowInQuery { get; set; }
    }

    public class TMSNBISRequesterResults
    {
        public int SegmentID { get; set; }
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public string PagerNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int RequesterID { get; set; }
        public string RequesterName { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public bool ShowInQuery { get; set; }
    }

    public class TMSNBIWorkOrderResults
    {
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string ActionCode { get; set; }
        public string ActionDescription { get; set; }
        public string AssetGroupNumber { get; set; }
        public string AssetNumber { get; set; }
        public string BuildingCode { get; set; }
        public string BuildingName { get; set; }
        public string CauseCode { get; set; }
        public string CauseDescription { get; set; }
        public string CompletionComments { get; set; }
        public DateTime DateAvailable { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateNeeded { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Description { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public double MeterReading { get; set; }
        public string PriorityCode { get; set; }
        public string PriorityDescription { get; set; }
        public string ProblemCode { get; set; }
        public string ProblemDescription { get; set; }
        public string ReferenceNumber { get; set; }
        public string RequesterComments { get; set; }
        public string RequesterEmail { get; set; }
        public string RequesterName { get; set; }
        public string RequesterPager { get; set; }
        public string RequesterPhone { get; set; }
        public int SegmentID { get; set; }
        public string ShopCode { get; set; }
        public string ShopDescription { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
        public string SkillCode { get; set; }
        public string SkillDescription { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string SubStatusCode { get; set; }
        public string SubStatusDescription { get; set; }
        public double TotalCost { get; set; }
        public double TotalHours { get; set; }
        public double TotalMaterialCharges { get; set; }
        public double TotalTimeCharges { get; set; }
        public string TypeCode { get; set; }

        public string TypeDescription { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string UDF6 { get; set; }
        public string UDF7 { get; set; }
        public string UDF8 { get; set; }
        public string UDF9 { get; set; }
        public string UDF10 { get; set; }
        public DateTime? WeekScheduled { get; set; }
        public int WorkOrderID { get; set; }
        public int WorkOrderNumber { get; set; }



    }
}