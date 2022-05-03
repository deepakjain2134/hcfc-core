using System.Collections.Generic;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IDepartmentService
    {
        List<Department> GetDepartment();
        Task<List<Department>> GetDepartmentAsync();
        int Save(Department department);
    }
}