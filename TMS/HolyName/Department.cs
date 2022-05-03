using System.Collections.Generic;

namespace TMS
{
    public static class Department
    {
        public static List<HCF.BDO.Department> GetDepartments()
        {
            List<HCF.BDO.Department> departments = new List<HCF.BDO.Department>();
            HolyNameTmsService.HolyNameDepartmentClient client = new HolyNameTmsService.HolyNameDepartmentClient();
            HolyNameTmsService.DepartmentCriteria[] criteria = new HolyNameTmsService.DepartmentCriteria[1];
            HolyNameTmsService.DepartmentCriteria c1 = new HolyNameTmsService.DepartmentCriteria();
            c1.fieldField = HolyNameTmsService.FIELDS.DEPARTMENT_CODE;
            c1.fieldValueField = "a";
            c1.operatorField = HolyNameTmsService.OPERATORS.ISLIKE;
            criteria[0] = c1;
                     
            HolyNameTmsService.Department[] holyDepartments = client.SearchDepartment(criteria);
            foreach (HolyNameTmsService.Department holydept in holyDepartments)
            {
                HCF.BDO.Department dept = new HCF.BDO.Department();                
                departments.Add(dept);
            }

            return departments;
        }

        public static List<HCF.BDO.Department> GetBurkeDepartments()
        {
            List<HCF.BDO.Department> departments = new List<HCF.BDO.Department>();
            HolyNameTmsService.BurkeDepartmentClient client = new HolyNameTmsService.BurkeDepartmentClient();
            HolyNameTmsService.DepartmentCriteria1[] criteria = new HolyNameTmsService.DepartmentCriteria1[1];
            HolyNameTmsService.DepartmentCriteria1 c1 = new HolyNameTmsService.DepartmentCriteria1();
            c1.fieldField = HolyNameTmsService.FIELDS3.DEPARTMENT_CODE;
            c1.fieldValueField = "a";
            c1.operatorField = HolyNameTmsService.OPERATORS3.ISLIKE;
            criteria[0] = c1;

            HolyNameTmsService.Department1[] holyDepartments = client.SearchBurkeDepartment(criteria);
            foreach (HolyNameTmsService.Department1 holydept in holyDepartments)
            {
                HCF.BDO.Department dept = new HCF.BDO.Department();
                departments.Add(dept);
            }

            return departments;
        }
    }
}
