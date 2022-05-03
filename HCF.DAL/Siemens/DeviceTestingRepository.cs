using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.Utility;

namespace HCF.DAL
{
    public class DeviceTestingRepository : IDeviceTestingRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public DeviceTestingRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public async Task<List<TDeviceTesting>> Get()
        {
            List<TDeviceTesting> list = new List<TDeviceTesting>();
            const string sql = StoredProcedures.Get_TDeviceTesting;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = await _sqlHelper.GetDataTableAsync(command);
                if (ds != null)
                {
                    list = _sqlHelper.ConvertDataTable<TDeviceTesting>(ds);
                    var wos = new List<WorkOrder>();
                    foreach (DataRow item in ds.Rows)
                    {
                        if (!string.IsNullOrEmpty(item["IssueId"].ToString()))
                        {
                            var wo = new WorkOrder
                            {
                                WorkOrderId = Convert.ToInt32(item["WorkOrderId"].ToString()),
                                WorkOrderNumber = item["WorkOrderNumber"].ToString(),
                                IssueId = Convert.ToInt32(item["IssueId"].ToString()),
                                StatusCode = item["StatusCode"].ToString(),
                                SubStatusCode = item["SubStatusCode"].ToString()
                            };
                            wos.Add(wo);
                        }
                    }
                    foreach (var item in list.Where(x => x.IssueId.HasValue))
                        item.WorkOrder = wos.FirstOrDefault(x => x.IssueId == item.IssueId);

                }
            }
            return await Task.FromResult(list);
        }

        public int Save(TDeviceTesting deviceTesting)
        {
            int newId;
            try
            {
                const string sql = StoredProcedures.Insert_TDeviceTesting;
                using (var command = new SqlCommand(sql))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Equipment", deviceTesting.Equipment);
                    command.Parameters.AddWithValue("@Address", deviceTesting.Address);
                    command.Parameters.AddWithValue("@XMLType", deviceTesting.XMLType);
                    command.Parameters.AddWithValue("@XMLUsage", deviceTesting.XMLUsage);
                    command.Parameters.AddWithValue("@Location", deviceTesting.Location);
                    command.Parameters.AddWithValue("@NFPAClassification", deviceTesting.NFPAClassification);
                    command.Parameters.AddWithValue("@HealthClassification", deviceTesting.HealthClassification);
                    command.Parameters.AddWithValue("@TestMethod", deviceTesting.TestMethod);
                    command.Parameters.AddWithValue("@TestResult", deviceTesting.TestResult);
                    command.Parameters.AddWithValue("@TestDate", deviceTesting.TestDate);
                    command.Parameters.AddWithValue("@DevReading", deviceTesting.DevReading);
                    command.Parameters.AddWithValue("@DevReadingDate", deviceTesting.DevReadingDate);
                    command.Parameters.AddWithValue("@FactorySetting", deviceTesting.FactorySetting);
                    command.Parameters.AddWithValue("@DPM", deviceTesting.DPM);
                    command.Parameters.AddWithValue("@MakeModel", deviceTesting.MakeModel);
                    command.Parameters.AddWithValue("@Note", deviceTesting.Note);
                    command.Parameters.AddWithValue("@Comment", deviceTesting.Comment);
                    command.Parameters.AddWithValue("@Site", deviceTesting.Site);
                    command.Parameters.AddWithValue("@Building", deviceTesting.Building);
                    command.Parameters.AddWithValue("@Floor", deviceTesting.Floor);
                    command.Parameters.AddWithValue("@Barcode", deviceTesting.Barcode);
                    command.Parameters.AddWithValue("@Panel", deviceTesting.Panel);
                    command.Parameters.AddWithValue("@Module", deviceTesting.Module);
                    command.Parameters.AddWithValue("@Devices", deviceTesting.Devices);
                    command.Parameters.AddWithValue("@CreatedDate", deviceTesting.CreatedDate.Year == 1 ? DateTime.Now : deviceTesting.CreatedDate);
                    command.Parameters.Add("@Id", SqlDbType.Int);
                    command.Parameters["@Id"].Direction = ParameterDirection.Output;
                    newId = _sqlHelper.ExecuteNonQuery(command, "@Id");
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
                newId = 0;
            }
            return newId;
        }

        public int UpdateIssueID(WorkOrder workOrder)
        {
            int newId;
            const string sql = StoredProcedures.Update_TDeviceTesting;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", workOrder.IssueId);
                command.Parameters.AddWithValue("@description", workOrder.Description);
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
    }
}
