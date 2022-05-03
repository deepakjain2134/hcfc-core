using HCF.BDO;

using HCF.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class EPGroupsRepsitory : IEPGroupsRepsitory
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;
        //private readonly IUsersRepository _usersRepository;
        public EPGroupsRepsitory(ISqlHelper sqlHelper, ICommonRepository commonRepository)
        {
            _sqlHelper = sqlHelper;
            _commonRepository = commonRepository;
        }

        public List<EPGroups> GetEPGroups(int? EPGroupId)
        {
            List<EPGroups> ePGroups = new List<EPGroups>();
            const string sql = StoredProcedures.Get_EPGroups;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPGroupId", EPGroupId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    ePGroups = _sqlHelper.ConvertDataTable<EPGroups>(ds.Tables[0]);
                    var standardeps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                    foreach (var item in ePGroups)
                    {
                        item.StandardEps = standardeps.Where(x => x.EPGroupId == item.EPGroupId).ToList();
                    }
                }
            }
            return ePGroups;
            //return CommonRepository.GetTableDataCommon<EPGroups>(StoredProcedures.Get_EPGroups);
        }

        public List<EPGroupsDetail> GetEPGroupsDetail()
        {
            return _commonRepository.GetTableDataCommon<EPGroupsDetail>(StoredProcedures.Get_EPGroupsDetail);
        }

        public List<EPGroups> GetVendorEPGroups(int vendorId)
        {
            List<EPGroups> ePGroups = new List<EPGroups>();
            const string sql = StoredProcedures.Get_VendorEPGroups;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", vendorId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    ePGroups = _sqlHelper.ConvertDataTable<EPGroups>(ds.Tables[0]);
                    List<StandardEps> standardeps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                    foreach (var item in ePGroups)
                    {
                        item.StandardEps = standardeps.Where(x => x.EPGroupId == item.EPGroupId).ToList();
                    }
                }
            }
            return ePGroups;
        }

        public int SaveVendorEPGroups(int vendorId, int groupId, bool status)
        {
            int newId = 0;
            const string sql = StoredProcedures.Insert_VendorEPGroups;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vendorId", vendorId);
                command.Parameters.AddWithValue("@groupId", groupId);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<EPGroups> GetEPGroupNameList()
        {
            return _commonRepository.GetTableDataCommon<EPGroups>(StoredProcedures.Get_EPGroupsNameList);
        }

        public EPGroups GetEPGroupName(int epgroupId)
        {
            return GetEPGroupNameList().Where(x => x.EPGroupId == epgroupId).FirstOrDefault();
        }

        public int AddEPGroupsName(EPGroups ePGroups)
        {
            //int newId = 0;
            const string sql = StoredProcedures.Insert_EPGroupsName;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPGroupId", ePGroups.EPGroupId);
                command.Parameters.AddWithValue("@EPGroupName", ePGroups.EPGroupName);
                command.Parameters.AddWithValue("@IsActive", ePGroups.IsActive);
                _sqlHelper.CommonExecuteNonQuery(command);
            }
            return 0;
        }

        public int RemoveEP(int? epgroupId, int? epdetailid)
        {
            //int newId = 0;
            const string sql = StoredProcedures.Updade_EPsAssignList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPGroupId", epgroupId);
                command.Parameters.AddWithValue("@EPDetailId", epdetailid);
                _sqlHelper.CommonExecuteNonQuery(command);
            }
            return 0;
        }

        public List<EPGroups> AssignEpsList(int? epgroupId)
        {
            List<EPGroups> ePGroups = new List<EPGroups>();
            const string sql = StoredProcedures.Get_EPsAssignList;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EPGroupId", epgroupId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    ePGroups = _sqlHelper.ConvertDataTable<EPGroups>(ds.Tables[0]);
                    List<StandardEps> standardeps = _sqlHelper.ConvertDataTable<StandardEps>(ds.Tables[1]);
                    foreach (var item in ePGroups)
                    {
                        item.StandardEps = standardeps;
                    }
                }
            }
            return ePGroups;
        }

        public int AssignEPsGroup(int epdetailId, int epGruopId, bool status)
        {
            //int newId = 0;
            const string sql = StoredProcedures.Insert_AssignEPs;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@epdetailId", epdetailId);
                command.Parameters.AddWithValue("@epgroupId", epGruopId);
                command.Parameters.AddWithValue("@status", status);
                //command.Parameters.Add("@newId", SqlDbType.Int);
                //command.Parameters["@newId"].Direction = ParameterDirection.Output;
                _sqlHelper.CommonExecuteNonQuery(command);
            }
            return 0;
        }

    }
}