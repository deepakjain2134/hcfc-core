using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO.NBIH;
using NbiResourceWS;

namespace TMS.NBIH
{
    public static class Resource
    {
        public static List<TMSNBIResourceResults> GetmsResources(int day)
        {
            DateTime createdDate = DateTime.Now;
            ResourceCriteria[] resourceCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                resourceCriteria = ResourceCriteria(createdDate);
            }
            else
            {
                resourceCriteria = new ResourceCriteria[1];
                ResourceCriteria c1 = new ResourceCriteria();
                c1.Field = FIELDS.DATE_CREATED;
                c1.FieldValue = createdDate.AddDays(-3000);
                c1.Operator = OPERATORS.GREATER_OR_EQUAL;
                resourceCriteria[0] = c1;
            }
            AuthenticationHeader authHeader = SetNbiHeader();
            var client = new ResourceWSClient(ResourceWSClient.EndpointConfiguration.ResourceWS);
            NbiResourceWS.Resource[] resourceList = client.QueryAsync(authHeader, resourceCriteria).Result;
            List<HCF.BDO.NBIH.TMSNBIResourceResults> TMS_Results = ConverttoResult(resourceList);
            return TMS_Results;

        }

        private static List<TMSNBIResourceResults> ConverttoResult(NbiResourceWS.Resource[] resourceList)
        {
            List<TMSNBIResourceResults> lists = new List<TMSNBIResourceResults>();
            foreach (var item in resourceList)
            {
                TMSNBIResourceResults obj = new TMSNBIResourceResults();
                obj.ResourceID = item.ResourceID;
                obj.ResourceNumber = item.ResourceNumber;
                obj.TypeCode = item.TypeCode;
                obj.TypeDescription = item.TypeDescription;
                obj.FirstName = item.FirstName;
                obj.LastName = item.LastName;
                obj.Name = item.Name;
                obj.Address1 = item.Address1;
                obj.Address2 = item.Address2;
                obj.City = item.City;
                obj.State = item.State;
                obj.Zip = item.Zip;
                obj.HomePhone = item.HomePhone;
                obj.EmergencyContact = item.EmergencyContact;
                obj.EmergencyContactPhone = item.EmergencyContactPhone;
                obj.WorkPhone = item.WorkPhone;
                obj.WorkPager = item.WorkPager;
                obj.AccountCode = item.AccountCode;
                obj.AccountDescription = item.AccountDescription;
                obj.SiteCode = item.SiteCode;
                obj.SiteName = item.SiteName;
                obj.BuildingCode = item.BuildingCode;
                obj.BuildingName = item.BuildingName;
                obj.LocationCode = item.LocationCode;
                obj.LocationDescription = item.LocationDescription;
                obj.ShopCode = item.ShopCode;
                obj.ShopDescription = item.ShopDescription;
                obj.AreaCode = item.AreaCode;
                obj.AreaDescription = item.AreaDescription;
                obj.SkillCode = item.SkillCode;
                obj.SkillDescription = item.SkillDescription;
                obj.SkillLevelCode = item.SkillLevelCode;
                obj.ShiftCode = item.ShiftCode;
                obj.ShiftDescription = item.ShiftDescription;
                obj.ChargeRate = item.ChargeRate;
                obj.StartDate = item.StartDate;
                obj.LastReviewDate = item.LastReviewDate;
                obj.Email = item.Email;
                obj.PagerEmail = item.PagerEmail;
                obj.StatusCode = item.StatusCode;
                obj.StatusDescription = item.StatusDescription;
                obj.UserField1 = item.UserField1;
                obj.UserField2 = item.UserField2;
                obj.UserField3 = item.UserField3;
                obj.UserField4 = item.UserField4;
                obj.UserField5 = item.UserField5;
                obj.UserField6 = item.UserField6;
                obj.UserField7 = item.UserField7;
                obj.UserField8 = item.UserField8;
                obj.UserField9 = item.UserField9;
                obj.UserField10 = item.UserField10;
                obj.Notes = item.Notes;
                obj.DateCreated = item.DateCreated.Value;
                obj.DateUpdated = item.DateUpdated.Value;
                obj.SegmentID = item.SegmentID;
                obj.FieldId = item.FieldId;
                obj.ModuleId = Convert.ToInt32(item.ModuleId);
                obj.ModuleCode = ((NbiResourceWS.MODULES)item.ModuleId).ToString();
                obj.ValidateForDataImport = item.ValidateForDataImport;
                obj.KeyID = item.KeyID;
                lists.Add(obj);
            }
            return lists;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }

        private static ResourceCriteria[] ResourceCriteria(DateTime modifyDate)
        {
            ResourceCriteria[] resourceCriteria = new ResourceCriteria[2];
            ResourceCriteria c1 = new ResourceCriteria();
            c1.Field = FIELDS.DATE_CREATED;
            c1.FieldValue = modifyDate;
            c1.Operator = OPERATORS.GREATER_OR_EQUAL;
            c1.LogicalOperator = LOGICAL_OPERATORS.OR;
            resourceCriteria[0] = c1;

            ResourceCriteria c2 = new ResourceCriteria();
            c2.Field = FIELDS.DATE_UPDATED;
            c2.FieldValue = modifyDate;
            c2.Operator = OPERATORS.GREATER_OR_EQUAL;
            c1.LogicalOperator = LOGICAL_OPERATORS.OR;
            resourceCriteria[1] = c2;
            return resourceCriteria;
        }
    }
}
