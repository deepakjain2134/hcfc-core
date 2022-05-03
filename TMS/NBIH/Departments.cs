using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiDepartmentsWS;

namespace TMS.NBIH
{
    public static class Departments
    {
        public static NbiDepartmentsWS.Department[] GetDepartments()
        {
            DepartmentCriteria[] deptCriteria;

            deptCriteria = new DepartmentCriteria[1];
            DepartmentCriteria c1 = new DepartmentCriteria();
            c1.Field = FIELDS.DEPARTMENT_NAME;
            c1.FieldValue = "a";
            c1.Operator = OPERATORS.ISLIKE;
            deptCriteria[0] = c1;

            AuthenticationHeader authHeader = SetNbiHeader();
            var client = new DepartmentWSClient(DepartmentWSClient.EndpointConfiguration.DepartmentWS);
            NbiDepartmentsWS.Department[] deptList = client.QueryAsync(authHeader, deptCriteria).Result;
            //List<HCF.BDO.NBIH.TMSNBIResourceResults> TMS_Results = ConverttoResult(resourceList);
            return deptList;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }
    }
}
