using HCF.BDO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public interface IDepartmentRepository
    {
        List<Department> GetDepartment();
        Task<List<Department>> GetDepartmentAsync();
        int Save(Department department);
    }
}