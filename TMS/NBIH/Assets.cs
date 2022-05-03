using HCF.BDO;
using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using NbiAssetsWS;
using System.Threading.Tasks;

namespace TMS.NBIH
{
    public static class Assets
    {
        public static List<TMSNBIAssetsResults> GetTmsAssets(int day)
        {
            DateTime createdDate = DateTime.Now;

            AssetCriteria[] assetCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                assetCriteria = AssetCriteria(createdDate);
            }
            else
            {
                assetCriteria = new AssetCriteria[1];
                var c1 = new AssetCriteria
                {
                    Field = FIELDS.DATE_CREATED,
                    FieldValue = createdDate.AddDays(-4000),
                    Operator = OPERATORS.GREATER_OR_EQUAL
                };
                assetCriteria[0] = c1;
            }
            AuthenticationHeader authHeader = SetNbiHeader();
            var client = new AssetWSClient(AssetWSClient.EndpointConfiguration.AssetWS);
            Asset[] assetsList = client.QueryAsync(authHeader, assetCriteria).Result;
            List<HCF.BDO.NBIH.TMSNBIAssetsResults> TMS_Results = ConverttoResult(assetsList);
            return TMS_Results;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }

        private static List<TMSNBIAssetsResults> ConverttoResult(Asset[] assetsList)
        {
            List<TMSNBIAssetsResults> lists = new List<TMSNBIAssetsResults>();
            foreach (var item in assetsList)
            {
                HCF.BDO.NBIH.TMSNBIAssetsResults obj = new TMSNBIAssetsResults();
                obj.AssetID = item.AssetID;
                obj.AssetNumber = item.AssetNumber;
                obj.Description = item.Description;
                obj.ManufacturerName = item.ManufacturerName;
                obj.ManufacturerCode = item.ManufacturerCode;
                obj.ModelNumber = item.ModelNumber;
                obj.SerialNumber = item.SerialNumber;
                obj.CategoryCode = item.CategoryCode;
                obj.CategoryDescription = item.CategoryDescription;
                obj.SubCategoryCode = item.SubCategoryCode;
                obj.SubCategoryDescription = item.SubCategoryDescription;
                obj.TypeCode = item.TypeCode;
                obj.TypeDescription = item.TypeDescription;
                obj.AccountCode = item.AccountCode;
                obj.AccountDescription = item.AccountDescription;
                obj.StatusCode = item.StatusCode;
                obj.StatusDescription = item.StatusDescription;
                obj.SkillCode = item.SkillCode;
                obj.SkillDescription = item.SkillDescription;
                obj.PriorityCode = item.PriorityCode;
                obj.PriorityDescription = item.PriorityDescription;
                obj.SiteCode = item.SiteCode;
                obj.SiteName = item.SiteName;
                obj.BuildingCode = item.BuildingCode;
                obj.BuildingName = item.BuildingName;
                obj.LocationCode = item.LocationCode;
                obj.LocationDescription = item.LocationDescription;
                obj.ShopCode = item.ShopCode;
                obj.ShopDescription = item.ShopDescription;
                //obj.DatePurchased = item.DatePurchased;
                //obj.DateReceived = item.DateReceived;
                //obj.DateAccepted = item.DateAccepted;
                obj.OriginalCost = Convert.ToDouble(item.OriginalCost);
                obj.ReplacementCost = Convert.ToDouble(item.ReplacementCost);
                obj.IDNumber = item.IDNumber;
                obj.ItemIdentifier = item.ItemIdentifier;
                obj.AreaCode = item.AreaCode;
                obj.AreaDescription = item.AreaDescription;
                obj.ServiceManual = item.ServiceManual;
                obj.ManualLocation = item.ManualLocation;
                obj.NetworkDevice = item.NetworkDevice;
                obj.MeterTypeCode = item.MeterTypeCode;
                obj.MeterTypeDescription = item.MeterTypeDescription;
                obj.MeterReading = item.MeterReading;
                obj.PhysicalConditionCode = item.PhysicalConditionCode;
                obj.PhysicalConditionDescription = item.PhysicalConditionDescription;
                obj.Owner = item.Owner;
                obj.SupportStatusCode = item.SupportStatusCode;
                obj.SupportStatusDescription = item.SupportStatusDescription;
                //obj.DateRetired = item.DateRetired;
                obj.WarrantyCompanyName = item.WarrantyCompanyName;
                obj.WarrantyCompanyCode = item.WarrantyCompanyCode;
                //obj.WarrantyStartDate = item.WarrantyStartDate;
                //obj.WarrantyEndDate = item.WarrantyEndDate;
                obj.OriginalPONumber = item.OriginalPONumber;
                obj.Department = item.Department;
                obj.BuildingDescription = item.BuildingDescription;
                obj.Room = item.Room;
                obj.UDF1 = item.UDF1;
                obj.UDF2 = item.UDF2;
                obj.UDF3 = item.UDF3;
                obj.UDF4 = item.UDF4;
                obj.UDF5 = item.UDF5;
                obj.UDF6 = item.UDF6;
                obj.UDF7 = item.UDF7;
                obj.UDF8 = item.UDF8;
                obj.UDF9 = item.UDF9;
                obj.UDF10 = item.UDF10;
                obj.ExtendedDescription = item.ExtendedDescription;
                obj.Options = item.Options;
                obj.SoftwareTest = item.SoftwareTest;
                obj.CostBasis = Convert.ToDouble(item.CostBasis);
                obj.SalvageValue = Convert.ToDouble(item.SalvageValue);
                obj.LifeExpectancy = item.LifeExpectancy;
                //obj.StartingDate = item.StartingDate;
                obj.FirstMonth = item.FirstMonth;
                obj.RiskNumber = item.RiskNumber;
                obj.DateCreated = item.DateCreated;
                obj.DateUpdated = item.DateUpdated;
                obj.HIPAACode = item.HIPAACode;
                obj.HIPAADescription = item.HIPAADescription;
                obj.NextPMDate = item.NextPMDate;
                obj.SegmentID = item.SegmentID;
                obj.FieldId = item.FieldId;
                obj.ValidateForDataImport = item.ValidateForDataImport;
                obj.KeyID = item.KeyID;
                lists.Add(obj);
            }
            return lists;


        }

        private static AssetCriteria[] AssetCriteria(DateTime modifyDate)
        {
            var assetCriteria = new AssetCriteria[2];
            var c1 = new AssetCriteria
            {
                Field = FIELDS.DATE_CREATED,
                FieldValue = modifyDate,
                Operator = OPERATORS.GREATER_OR_EQUAL,
                LogicalOperator = LOGICAL_OPERATORS.OR
            };
            assetCriteria[0] = c1;

            var c2 = new AssetCriteria
            {
                Field = FIELDS.DATE_UPDATED,
                FieldValue = modifyDate,
                Operator = OPERATORS.GREATER_OR_EQUAL
            };
            c1.LogicalOperator = LOGICAL_OPERATORS.OR;
            assetCriteria[1] = c2;
            return assetCriteria;
        }
    }
}
