using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public int Save(Department department)
        {
            return _departmentRepository.Save(department);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartment()
        {
            return _departmentRepository.GetDepartment();
        }
        public async Task<List<Department>> GetDepartmentAsync()
        {
            return await _departmentRepository.GetDepartmentAsync();
        }
    }
}
