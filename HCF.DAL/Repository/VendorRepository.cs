using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly IUsersRepository _usersRepository;
        public VendorRepository(ISqlHelper sqlHelper, IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVendors"></param>
        /// <returns></returns>
        public int AddVendor(Vendors newVendors, bool isMasterVendor, int masterVendorId, Guid? invitationId, string siteAssignees)
        {
            int newId;
            const string sql = StoredProcedures.Insert_Vendors; // "dbo.Insert_Vendors";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CreatedBy", newVendors.CreatedBy);
                command.Parameters.AddWithValue("@Name", newVendors.Name);
                command.Parameters.AddWithValue("@RegistrationNo", newVendors.RegistrationNo);
                command.Parameters.AddWithValue("@Address", newVendors.Address);
                command.Parameters.AddWithValue("@IsActive", newVendors.IsActive);
                command.Parameters.AddWithValue("@Email", newVendors.Email);
                command.Parameters.AddWithValue("@PhoneNo", newVendors.PhoneNo);
                command.Parameters.AddWithValue("@CellNo", newVendors.CellNo);
                command.Parameters.AddWithValue("@isMasterVendor", isMasterVendor);
                command.Parameters.AddWithValue("@masterVendorId", masterVendorId);
                command.Parameters.AddWithValue("@BuildingIds", newVendors.BuildingIds);
                command.Parameters.AddWithValue("@invitationId", invitationId);
                command.Parameters.AddWithValue("@siteAssignees", siteAssignees);
                command.Parameters.AddWithValue("@drawingIds", newVendors.DrpDrawingsIds != null ? string.Join(",", newVendors.DrpDrawingsIds) : null);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vendors> GetVendor(int? vendorId)
        {
            List<Vendors> vendors=new List<Vendors> ();
            string sql = StoredProcedures.Get_Vendors;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vendorId", vendorId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        vendors = _sqlHelper.ConvertDataTable<Vendors>(ds.Tables[0]);
                        var vendorUser = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[2]);
                        var vendorCertificates = _sqlHelper.ConvertDataTable<Certificates>(ds.Tables[1]);
                        foreach (var vendor in vendors)
                        {
                            vendor.Users = vendorUser.Where(x => x.VendorId == vendor.VendorId).ToList();
                            vendor.VendorCertificates = vendorCertificates.Where(m => m.VendorId == vendor.VendorId).ToList();
                            foreach (var item in vendor.VendorCertificates)
                            {
                                item.UserProfile = _usersRepository.GetUsersList(item.CreatedBy);
                                item.ValidUpToTimeSpan = Conversion.ConvertToTimeSpan(item.ValidUpTo);
                            }
                        }
                    }
                }
            }
            return vendors;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vendors> GetVendors()
        {
            var vendors = new List<Vendors>();
            string sql = StoredProcedures.Get_Vendors;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            var vendor = new Vendors
                            {
                                Address = item["Address"].ToString(),
                                BuildingIds = item["BuildingIds"].ToString(),
                                CellNo = item["CellNo"].ToString(),
                                ClientNo = Conversion.TryCastInt32(item["ClientNo"].ToString()),
                                ContactDetails = item["ContactDetails"].ToString(),
                                CreatedBy = Conversion.TryCastInt32(item["CreatedBy"].ToString()),
                                CreatedDate = Convert.ToDateTime(item["CreatedDate"].ToString()),
                                //CustomName = item["CustomName"].ToString(),
                                DrpDrawingsIds = Conversion.TryCastIntArray(item["DrawingIds"].ToString()),
                                Email = item["Email"].ToString(),
                                IsActive = Convert.ToBoolean(item["IsActive"].ToString()),
                                //IsAdded = Conversion.TryCastBoolean(item["IsAdded"].ToString()),
                                MessageToContractor = item["MessageToContractor"].ToString(),
                                Name = item["Name"].ToString(),
                                PhoneNo = item["PhoneNo"].ToString(),
                                RegistrationNo = item["RegistrationNo"].ToString(),
                                //UploadLink = item["UploadLink"].ToString(),
                                VendorId = Conversion.TryCastInt32(item["VendorId"].ToString())
                            };
                            vendors.Add(vendor);
                        }
                    }
               
                }
            }
            return vendors;
        }

        public int SaveVendorInvitation(VendorOrganizations org)
        {
            int newId;
            const string sql = StoredProcedures.Insert_VendorInvitation; // "dbo.Insert_Vendors";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationId", org.InvitationId);
                command.Parameters.AddWithValue("@OrgKey", org.OrgKey);
                command.Parameters.AddWithValue("@VendorId", org.VendorId);
                command.Parameters.AddWithValue("@RequestedBy", org.RequestedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public List<Vendors> GetVendorByPrefix(string prefix)
        {
            var lists = new List<Vendors>();
            const string sql = StoredProcedures.Get_VendorByPrefix;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@prefix", prefix);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var vendor = new Vendors
                        {
                            Name = item["Name"].ToString(),
                            VendorId = Convert.ToInt32(item["VendorId"].ToString()),
                            Email = item["Email"].ToString(),
                            RegistrationNo = item["RegistrationNo"].ToString(),
                            PhoneNo = item["PhoneNo"].ToString(),
                            CellNo = item["CellNo"].ToString(),
                            Address = item["Address"].ToString()
                        };
                        if (!string.IsNullOrEmpty(item["ClientNo"].ToString()))
                            vendor.ClientNo = Convert.ToInt32(item["ClientNo"].ToString());
                        vendor.IsAdded = Convert.ToBoolean(item["IsAdded"].ToString());
                        lists.Add(vendor);
                    }
            }
            return lists;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVendors"></param>
        /// <returns></returns>
        public bool UpdateVendor(Vendors newVendors, string siteAssigneeType)
        {
            const string sql = StoredProcedures.Update_Vendor; // "Update_Vendor";
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", newVendors.VendorId);
                command.Parameters.AddWithValue("@Name", newVendors.Name);
                command.Parameters.AddWithValue("@RegistrationNo", newVendors.RegistrationNo);
                command.Parameters.AddWithValue("@Address", newVendors.Address);
                command.Parameters.AddWithValue("@IsActive", newVendors.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newVendors.CreatedBy);
                command.Parameters.AddWithValue("@PhoneNo", newVendors.PhoneNo);
                command.Parameters.AddWithValue("@BuildingIds", newVendors.BuildingIds);
                command.Parameters.AddWithValue("@Email", newVendors.Email);
                command.Parameters.AddWithValue("@CellNo", newVendors.CellNo);
                command.Parameters.AddWithValue("@siteAssigneeType", siteAssigneeType);
                command.Parameters.AddWithValue("@drawingIds", newVendors.DrpDrawingsIds != null ? string.Join(",", newVendors.DrpDrawingsIds) : null);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }
        //public  bool SaveBuildingIds(string vendorId, string buildingsId)
        //{
        //    int vendorid = Convert.ToInt32(vendorId);
        //    const string sql = StoredProcedures.Update_VendorbuildingsId; // "Update_Vendor";
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@VendorId", vendorid);
        //        command.Parameters.AddWithValue("@BuildingsId", buildingsId);
        //        return _sqlHelper.ExecuteNonQuery(command);
        //    }
        //}
        //public  Vendors GetVendorsBuildings(int vendorId)
        //{
        //    Vendors lists;
        //    const string sql = StoredProcedures.Get_VendorsBuildings;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@Vendorid", vendorId);
        //        var dt = _sqlHelper.GetDataTable(command);
        //        lists = _sqlHelper.ConvertDataTable<Vendors>(dt).FirstOrDefault();
        //    }
        //    return lists;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public bool DeleteCertificates(int vendorId)
        {
            const string sql = StoredProcedures.Delete_Certificates;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", vendorId);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        //public  List<Vendors> GetVendorDetails()
        //{
        //    var vendors = new List<Vendors>();
        //    const string sql = StoredProcedures.Get_Vendor;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        var ds = _sqlHelper.GetDataSet(command);
        //        vendors = _sqlHelper.ConvertDataTable<Vendors>(ds.Tables[0]);
        //        return vendors;
        //    }
        //}

        public bool UpdateContactDetails(string contactDetails, int vendorId, string messageToContractor, int Status, int organizationId = 1)
        {
            const string sql = StoredProcedures.Update_ContactDetails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Vendorid", vendorId);
                command.Parameters.AddWithValue("@ContactDetails", contactDetails);
                command.Parameters.AddWithValue("@OrganizationId", organizationId);
                command.Parameters.AddWithValue("@MessageToContractor", messageToContractor);
                command.Parameters.AddWithValue("@Status", Status);

                var success = _sqlHelper.ExecuteNonQuery(command);
                return success;
            }
        }

        public VendorRecordsCount GetVendorDashboardData(int vendorId, int userId)
        {

            VendorRecordsCount lists;
            const string sql = StoredProcedures.Get_VendorDashboardData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Vendorid", vendorId);
                command.Parameters.AddWithValue("@UserId", userId);
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<VendorRecordsCount>(dt).FirstOrDefault();
            }
            return lists;
        }
        public object IsVendorExist(string OrganizationId, string Email)
        {
            const string sql = StoredProcedures.Get_IsVendorExist;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Orgkey", OrganizationId);
                command.Parameters.AddWithValue("@Email", Email);
                return _sqlHelper.GetScalarValue(command);
            }
        }

        public Vendors GetVendorbyOrgid(string OrganizationId)
        {
            Vendors lists;
            const string sql = StoredProcedures.Get_VendorbyOrgid;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Orgkey", OrganizationId);
                var dt = _sqlHelper.GetDataTable(command);
                lists = _sqlHelper.ConvertDataTable<Vendors>(dt).FirstOrDefault();
            }
            return lists;
        }


        #region VendorResource
        public int AddVendorResources(VendorResource vmodel)
        {
            int newId;
            const string sql = StoredProcedures.Insert_VendorResource;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorResId", vmodel.VendorResId);
                command.Parameters.AddWithValue("@VendorId", vmodel.VendorId);
                command.Parameters.AddWithValue("@UploadLink", vmodel.UploadLink);
                command.Parameters.AddWithValue("@CustomName", vmodel.CustomName);
                command.Parameters.AddWithValue("@EffectiveDate", vmodel.EffectiveDate);
                command.Parameters.AddWithValue("@TFileId", vmodel.TFileId);
                command.Parameters.AddWithValue("@IsActive", vmodel.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", vmodel.CreatedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId > 0 ? newId : 0;
        }
        public List<Vendors> GetVendorResources(int? vendorId)
        {
            string _File = "";
            List<Vendors> vendors;
            int? _vendorId = (vendorId != null) ? (int?)vendorId : -1;
            string sql = StoredProcedures.Get_VendorResources;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", _vendorId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    vendors = _sqlHelper.ConvertDataTable<Vendors>(ds.Tables[0]);
                    var vendorFiles = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[1]);
                    var vendorResources = _sqlHelper.ConvertDataTable<VendorResource>(ds.Tables[2]);

                    foreach (var v in vendors)
                    {
                        if (!string.IsNullOrEmpty(v.TFileId))
                        {
                            _File = v.TFileId;
                            _File = _File.TrimEnd(',');
                            int[] strFile = _File.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                            v.File = vendorFiles.Where(x => strFile.Contains(x.TFileId)).ToList();
                        }
                        v.VendorResource = vendorResources.Where(x => x.VendorId == vendorId).ToList();
                    }
                }
            }
            return vendors;
        }
        public List<VendorResource> GetVendorResource(int? vendorId)
        {
            List<VendorResource> vendorResource = new List<VendorResource>();
            string sql = StoredProcedures.Get_VendorsResource;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", vendorId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if(ds!=null && ds.Tables.Count>0)
                    {
                        var tFiles = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[1]);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            VendorResource resource = new VendorResource();
                            resource.VendorResId = Convert.ToInt32(row["VendorResId"].ToString());
                            if (!string.IsNullOrEmpty(row["VendorId"].ToString()))
                            {
                                resource.VendorId = Convert.ToInt32(row["VendorId"].ToString());
                                resource.Vendor = new Vendors();
                                resource.Vendor.Name = row["VendorName"].ToString();
                                resource.Vendor.VendorId = resource.VendorId.Value;
                            }
                            resource.CustomName = row["CustomName"].ToString();
                            resource.UploadLink = row["UploadLink"].ToString();
                            resource.IsActive = Convert.ToBoolean(row["IsActive"].ToString());
                            if (!string.IsNullOrEmpty(row["EffectiveDate"].ToString()))
                                resource.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString());

                            if (!string.IsNullOrEmpty(row["TFileId"].ToString()))
                            {
                                int[] fileIds = (row["TFileId"].ToString() ?? "").Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray();
                                resource.File = tFiles.Where(x => fileIds.Contains(x.TFileId)).ToList();
                            }
                            vendorResource.Add(resource);
                        }
                    }
                   
                }
            }
            return vendorResource;
        }

        public List<Vendors> GetVendorResourcesDashboard(int? vendorId)
        {
            string _File = "";
            List<Vendors> vendors;
            int? _vendorId = (vendorId != null) ? (int?)vendorId : -1;
            string sql = StoredProcedures.Get_VendorResources_Dashboard;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", _vendorId);
                using (DataSet ds = _sqlHelper.GetDataSet(command))
                {
                    if (ds != null && ds.Tables.Count>0)
                    {
                        vendors = _sqlHelper.ConvertDataTable<Vendors>(ds.Tables[0]);
                        var vendorFiles = _sqlHelper.ConvertDataTable<TFiles>(ds.Tables[1]);

                        foreach (var v in vendors)
                        {
                            if (!string.IsNullOrEmpty(v.TFileId))
                            {
                                _File = v.TFileId;
                                _File = _File.TrimEnd(',');
                                int[] strFile = _File.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                v.File = vendorFiles.Where(x => strFile.Contains(x.TFileId)).ToList();
                            }
                        }
                    }
                    else
                    {
                        vendors = null;
                    }
                }
            }
            return vendors;
        }
        #endregion
    }
}
