using HCF.BDO;
using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public class BuildVersionRepository : IBuildVersionRepository
    {      
        private readonly ISqlHelper _sqlHelper;

        //private readonly IRepository<BuildVersion> _repository;

        public BuildVersionRepository(ISqlHelper sqlHelper)
        {           
            _sqlHelper = sqlHelper;
           // _repository = repository;
        }

        public async Task<IEnumerable<BuildVersion>> GetAsync()
        {
            return null;
            //return await _repository.FindAll().OrderBy(ow => ow.Version).ToListAsync();
        }

        public List<BuildVersion> GetBuildVersions()
        {
            var lists = new List<BuildVersion>();
            const string sql = StoredProcedures.Get_BuildVersions;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)                
                    lists = _sqlHelper.ConvertDataTable<BuildVersion>(ds.Tables[0]);                
            }
            return lists;
        }
    }
}
