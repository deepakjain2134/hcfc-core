using System;
using System.Collections.Generic;

namespace TMS
{
    public static class Resource
    {
        #region HolyName
        public static List<HCF.BDO.UserProfile> GetResources(int day)
        {

            DateTime createdDate = DateTime.Now;
            HolyNameTmsService.ResourceCriteria[] resourceCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                resourceCriteria = ResourceCriteria(createdDate);
            }
            else
            {
                resourceCriteria = new HolyNameTmsService.ResourceCriteria[1];
                HolyNameTmsService.ResourceCriteria c1 = new HolyNameTmsService.ResourceCriteria();
                c1.fieldField = HolyNameTmsService.FIELDS1.DATE_CREATED;
                c1.fieldValueField = createdDate;
                c1.operatorField = HolyNameTmsService.OPERATORS1.LESS_OR_EQUAL;
                resourceCriteria[0] = c1;
            }

            List<HCF.BDO.UserProfile> users = new List<HCF.BDO.UserProfile>();
            HolyNameTmsService.HolyNameResourceClient resouceClient = new HolyNameTmsService.HolyNameResourceClient();         
            HolyNameTmsService.Resource[] resources = resouceClient.SearchResource(resourceCriteria);
            foreach (HolyNameTmsService.Resource resource in resources)
            {
                HCF.BDO.UserProfile user = new HCF.BDO.UserProfile();
                user.FirstName = resource.firstNameField;
                user.LastName = resource.lastNameField;
                user.SkillCode = resource.skillCodeField;
                user.AccountCode = resource.accountCodeField;
                user.StatusCode = resource.statusCodeField;
                user.ResourceId = resource.resourceIDField;
                user.ResourceNumber = resource.resourceNumberField;
                user.Email = resource.emailField;
                user.TypeCode = resource.typeCodeField;
                users.Add(user);
            }
            return users;
        }

        private static HolyNameTmsService.ResourceCriteria[] ResourceCriteria(DateTime modifyDate)
        {
            HolyNameTmsService.ResourceCriteria[] resourceCriteria = new HolyNameTmsService.ResourceCriteria[2];
            HolyNameTmsService.ResourceCriteria c1 = new HolyNameTmsService.ResourceCriteria();
            c1.fieldField = HolyNameTmsService.FIELDS1.DATE_CREATED;
            c1.fieldValueField = modifyDate;
            c1.operatorField = HolyNameTmsService.OPERATORS1.GREATER_OR_EQUAL;
            c1.logicalOperatorField = HolyNameTmsService.LOGICAL_OPERATORS1.OR;
            resourceCriteria[0] = c1;

            HolyNameTmsService.ResourceCriteria c2 = new HolyNameTmsService.ResourceCriteria();
            c2.fieldField = HolyNameTmsService.FIELDS1.DATE_UPDATED;
            c2.fieldValueField = modifyDate;
            c2.operatorField = HolyNameTmsService.OPERATORS1.GREATER_OR_EQUAL;
            c1.logicalOperatorField = HolyNameTmsService.LOGICAL_OPERATORS1.OR;
            resourceCriteria[1] = c2;
            return resourceCriteria;
        }

        #endregion
        
        #region Burke
        public static List<HCF.BDO.UserProfile> GetBurkeResources(int day)
        {

            DateTime createdDate = DateTime.Now;
            HolyNameTmsService.ResourceCriteria1[] resourceCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                resourceCriteria = BrukeResourceCriteria(createdDate);
            }
            else
            {
                resourceCriteria = new HolyNameTmsService.ResourceCriteria1[1];
                HolyNameTmsService.ResourceCriteria1 c1 = new HolyNameTmsService.ResourceCriteria1();
                c1.fieldField = HolyNameTmsService.FIELDS7.DATE_CREATED;
                c1.fieldValueField = createdDate;
                c1.operatorField = HolyNameTmsService.OPERATORS7.LESS_OR_EQUAL;
                resourceCriteria[0] = c1;
            }

            List<HCF.BDO.UserProfile> users = new List<HCF.BDO.UserProfile>();
            HolyNameTmsService.BurkeResourceClient resouceClient = new HolyNameTmsService.BurkeResourceClient();
            HolyNameTmsService.Resource2[] resources = resouceClient.SearchBurkeResource(resourceCriteria);
            foreach (HolyNameTmsService.Resource2 resource in resources)
            {
                HCF.BDO.UserProfile user = new HCF.BDO.UserProfile();
                user.FirstName = resource.firstNameField;
                user.LastName = resource.lastNameField;
                user.SkillCode = resource.skillCodeField;
                user.AccountCode = resource.accountCodeField;
                user.StatusCode = resource.statusCodeField;
                user.ResourceId = resource.resourceIDField;
                user.ResourceNumber = resource.resourceNumberField;
                user.Email = resource.emailField;
                user.TypeCode = resource.typeCodeField;
                users.Add(user);
            }
            return users;
        }

        private static HolyNameTmsService.ResourceCriteria1[] BrukeResourceCriteria(DateTime modifyDate)
        {
            HolyNameTmsService.ResourceCriteria1[] resourceCriteria = new HolyNameTmsService.ResourceCriteria1[2];
            HolyNameTmsService.ResourceCriteria1 c1 = new HolyNameTmsService.ResourceCriteria1();
            c1.fieldField = HolyNameTmsService.FIELDS7.DATE_CREATED;
            c1.fieldValueField = modifyDate;
            c1.operatorField = HolyNameTmsService.OPERATORS7.GREATER_OR_EQUAL;
            c1.logicalOperatorField = HolyNameTmsService.LOGICAL_OPERATORS7.OR;
            resourceCriteria[0] = c1;

            HolyNameTmsService.ResourceCriteria1 c2 = new HolyNameTmsService.ResourceCriteria1();
            c2.fieldField = HolyNameTmsService.FIELDS7.DATE_UPDATED;
            c2.fieldValueField = modifyDate;
            c2.operatorField = HolyNameTmsService.OPERATORS7.GREATER_OR_EQUAL;
            c1.logicalOperatorField = HolyNameTmsService.LOGICAL_OPERATORS7.OR;
            resourceCriteria[1] = c2;
            return resourceCriteria;
        }

        #endregion
    }
}
