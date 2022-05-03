using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HCF.DAL
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public OrganizationRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public List<Organization> GetMasterOrg()
        {
            List<Organization> orgs = new();
            const string sql = StoredProcedures.Hcf_GetOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetCommonDataSet(cmd);
                orgs = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
            }
            return orgs.Where(x => x.ParentOrgKey == null).ToList();
        }

        public async Task<Organization> GetOrganizationAsync(Guid orgKey)
        {
            var orgs = new Organization();
            const string sql = StoredProcedures.Hcf_GetOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = await _sqlHelper.GetCommonDataTableAsync(cmd);
                if (dt != null)
                    orgs = _sqlHelper.ConvertDataTable<Organization>(dt).FirstOrDefault(x => x.Orgkey == orgKey);
            }
            return orgs;
        }

        #region Organization              

        public List<Organization> UserOrganizations(int userId)
        {
            List<Organization> orgs = new();
            const string sql = StoredProcedures.Get_UserOrganizations;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", userId);
                var dt = _sqlHelper.GetCommonDataTable(cmd);
                if (dt != null)
                    orgs = _sqlHelper.ConvertDataTable<Organization>(dt);
            }
            return orgs;
        }

        public DataSet GetAllOrganizationDashInfo()
        {
            DataSet ds;
            const string sql = StoredProcedures.Get_AllOrganizationDashInfo;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                ds = _sqlHelper.GetCommonDataSet(cmd);
            }
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentOrgKey"></param>
        /// <returns></returns>
        public List<Organization> GetMasterOrganization(Guid parentOrgKey)
        {
            var orgs = new List<Organization>();
            const string sql = StoredProcedures.Get_ChildOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ParentOrgKey", parentOrgKey);
                var ds = _sqlHelper.GetCommonDataSet(cmd);
                if (ds != null)
                    orgs = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
            }
            return orgs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetMasterOrganization()
        {
            var orgs = new List<Organization>();
            const string sql = StoredProcedures.Get_MasterOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                using(var ds = _sqlHelper.GetCommonDataSet(cmd))
                {
                    if (ds != null)
                    {
                        orgs = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[0]);
                        var popEmails = _sqlHelper.ConvertDataTable<PopEmails>(ds.Tables[1]);
                        foreach (var item in orgs)
                        {
                            item.PopEmails = popEmails.Where(x => x.ClientNo == item.ClientNo).ToList();
                            item.CRxInboxMailId = String.Join(",", item.PopEmails.Where(x => x.IsActive).Select(x => x.EmailId));
                        }
                    }
                }
            }
            return orgs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Organization GetOrganization()
        {
            var objOrganization = new Organization();
            const string sql = StoredProcedures.Get_Organization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objOrganization.Orgkey = new Guid(row["Orgkey"].ToString());
                        objOrganization.City = row["City"].ToString();
                        objOrganization.Country = row["Country"].ToString();
                        objOrganization.DashBoadImagePath = row["DashBoadImagePath"].ToString();
                        objOrganization.LogoName = row["logoName"].ToString();
                        objOrganization.LogoPath = row["LogoPath"].ToString();
                        objOrganization.LogoBase64 = row["LogoBase64"].ToString();
                        objOrganization.FileName = row["FileName"].ToString();
                        objOrganization.Country = row["Country"].ToString();
                        objOrganization.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                        objOrganization.Name = row["Name"].ToString();
                        objOrganization.IsTmsWo = Convert.ToBoolean(row["IsTmsWo"].ToString());
                        objOrganization.State = row["State"].ToString();
                        objOrganization.Zip = row["Zip"].ToString();

                        if (!string.IsNullOrEmpty(row["InitialCRxIncidentDate"].ToString()))
                            objOrganization.InitialCRxIncidentDate = Convert.ToDateTime(row["InitialCRxIncidentDate"].ToString());
                        else
                            objOrganization.InitialCRxIncidentDate = objOrganization.CreatedDate;

                        if (!string.IsNullOrEmpty(row["TotalBed"].ToString()))
                            objOrganization.TotalBed = Convert.ToInt32(row["TotalBed"].ToString());
                        if (!string.IsNullOrEmpty(row["HospitalArea"].ToString()))
                            objOrganization.HospitalArea = Convert.ToDecimal(row["HospitalArea"].ToString());
                        if (!string.IsNullOrEmpty(row["WOSysType"].ToString()))
                            objOrganization.WOSysType = Convert.ToInt32(row["WOSysType"].ToString());

                        objOrganization.FireWatchTimeSlot = Convert.ToInt32(row["FireWatchTimeSlot"].ToString());
                        objOrganization.IsOutpatient = Convert.ToBoolean(row["IsOutpatient"].ToString());
                        if (!string.IsNullOrEmpty(row["InspectionType"].ToString()))
                            objOrganization.InspectionType = Convert.ToInt32(row["InspectionType"].ToString());

                        objOrganization.LastWoTmsSync = Convert.ToDateTime(row["LastWoTmsSync"].ToString());
                        objOrganization.DbName = row["DbName"].ToString();

                        objOrganization.OrganizationId = Convert.ToInt32(row["OrganizationId"].ToString());
                        objOrganization.User = new UserProfile
                        {
                            UserName = row["UserName"].ToString(),
                            UserGuid = new Guid(row["UserProfileId"].ToString())
                        };
                        objOrganization.HospitalTypeId = Convert.ToInt32(row["HospitalTypeId"].ToString());
                        objOrganization.HospitalType = new HospitalType
                        {
                            Type = row["Type"].ToString(),
                            HospitalTypeId = objOrganization.HospitalTypeId
                        };
                        objOrganization.ContactDetails = row["ContactDetails"].ToString();
                        objOrganization.MessageToContractor = row["MessageToContractor"].ToString();
                        objOrganization.NoticeText = row["NoticeText"].ToString();

                        if (!string.IsNullOrEmpty(row["PlanFireDrillsForMe"].ToString()))
                            objOrganization.PlanFireDrillsForMe = Convert.ToBoolean(row["PlanFireDrillsForMe"].ToString());


                        if (!string.IsNullOrEmpty(row["IsTaggingEnabled"].ToString()))
                            objOrganization.IsTaggingEnabled = Convert.ToBoolean(row["IsTaggingEnabled"].ToString());

                        if (!string.IsNullOrEmpty(row["UserDomain"].ToString()))
                            objOrganization.UserDomain = row["UserDomain"].ToString();

                        if (!string.IsNullOrEmpty(row["StateName"].ToString()))
                            objOrganization.StateMaster = new StateMaster
                            {
                                StateName = row["StateName"].ToString()   
                            };
                        if (!string.IsNullOrEmpty(row["LastTJCSurvey"].ToString()))
                            objOrganization.LastTJCSurvey = Convert.ToDateTime(row["LastTJCSurvey"].ToString());
                        //else
                        //    objOrganization.InitialCRxIncidentDate = "";
                    }
                }
            }
            return objOrganization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newOrganization"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Organization UpdateOrganization(Organization newOrganization, string mode)
        {
            string sql = StoredProcedures.Update_Organization;
            using (var command = new SqlCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", newOrganization.Name);
                command.Parameters.AddWithValue("@City", newOrganization.City);
                command.Parameters.AddWithValue("@State", newOrganization.State);
                command.Parameters.AddWithValue("@Country", newOrganization.Country);
                command.Parameters.AddWithValue("@Zip", newOrganization.Zip); 
                command.Parameters.AddWithValue("@InitialCRxIncidentDate", newOrganization.InitialCRxIncidentDate);
                command.Parameters.AddWithValue("@TotalBed", newOrganization.TotalBed);
                command.Parameters.AddWithValue("@HospitalArea", newOrganization.HospitalArea);
                command.Parameters.AddWithValue("@WOSysType", newOrganization.WOSysType);
                command.Parameters.AddWithValue("@IsOutpatient", newOrganization.IsOutpatient);
                command.Parameters.AddWithValue("@OrganizationId", newOrganization.OrganizationId);
                command.Parameters.AddWithValue("@FileName", newOrganization.FileName);
                command.Parameters.AddWithValue("@DashBoadImagePath", newOrganization.DashBoadImagePath);
                command.Parameters.AddWithValue("@InspectionType", newOrganization.InspectionType);
                command.Parameters.AddWithValue("@FireWatchTimeSlot", newOrganization.FireWatchTimeSlot);
                command.Parameters.AddWithValue("@HospitalTypeId", newOrganization.HospitalTypeId);
                command.Parameters.AddWithValue("@NoticeText", newOrganization.NoticeText);
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@IsTaggingEnabled", newOrganization.IsTaggingEnabled);
                command.Parameters.AddWithValue("@LogoName", newOrganization.LogoName);
                command.Parameters.AddWithValue("@LogoBase64", newOrganization.LogoBase64);
                command.Parameters.AddWithValue("@LogoPath", newOrganization.LogoPath);
                command.Parameters.AddWithValue("@UserDomain", newOrganization.UserDomain);
                command.Parameters.AddWithValue("@LastTJCSurvey", newOrganization.LastTJCSurvey);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return newOrganization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Organization GetMasterData()
        {
            Organization org;
            const string sql = StoredProcedures.Get_MasterData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = _sqlHelper.GetDataSet(command);
                List<Status> list = _sqlHelper.ConvertDataTable<Status>(ds.Tables[0]);
                List<Score> scores = _sqlHelper.ConvertDataTable<Score>(ds.Tables[1]);
                List<UserGroup> userGroups = _sqlHelper.ConvertDataTable<UserGroup>(ds.Tables[2]);
                List<BuildingType> buildingTypes = _sqlHelper.ConvertDataTable<BuildingType>(ds.Tables[4]);
                org = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[5]).FirstOrDefault();
                List<Roles> roles = _sqlHelper.ConvertDataTable<Roles>(ds.Tables[6]);
                List<InspStatus> inspStatuses = _sqlHelper.ConvertDataTable<InspStatus>(ds.Tables[7]);
                List<NotificationCategory> notificationCategory = _sqlHelper.ConvertDataTable<NotificationCategory>(ds.Tables[8]);
                List<NotificationEvent> notificationEvent = _sqlHelper.ConvertDataTable<NotificationEvent>(ds.Tables[9]);
                List<Manufactures> manufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[10]);
                List<InspResult> inspResults = _sqlHelper.ConvertDataTable<InspResult>(ds.Tables[11]);


                if (org != null)
                {
                    org.Status = list;
                    org.Scores = scores;
                    org.UserGroups = userGroups;
                    org.Roles = roles;
                    org.BuildingTypes = buildingTypes;
                    org.NotificationCategorys = notificationCategory;
                    org.NotificationEvents = notificationEvent;
                    org.Manufactures = manufactures;
                    org.InspResults = inspResults;
                    org.InspStatus = inspStatuses;
                }
            }
            return org;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetMastersData()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage { Result = new Result() };
            const string sql = StoredProcedures.Get_MasterData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = _sqlHelper.GetDataSet(command);
                List<Status> list = _sqlHelper.ConvertDataTable<Status>(ds.Tables[0]);
                List<Score> scores = _sqlHelper.ConvertDataTable<Score>(ds.Tables[1]);
                List<UserGroup> userGroups = _sqlHelper.ConvertDataTable<UserGroup>(ds.Tables[2]);
                List<BuildingType> buildingTypes = _sqlHelper.ConvertDataTable<BuildingType>(ds.Tables[4]);
                List<Roles> roles = _sqlHelper.ConvertDataTable<Roles>(ds.Tables[6]);
                List<InspStatus> inspStatuses = _sqlHelper.ConvertDataTable<InspStatus>(ds.Tables[7]);
                List<NotificationCategory> notificationCategory = _sqlHelper.ConvertDataTable<NotificationCategory>(ds.Tables[8]);
                List<NotificationEvent> notificationEvent = _sqlHelper.ConvertDataTable<NotificationEvent>(ds.Tables[9]);
                List<Manufactures> manufactures = _sqlHelper.ConvertDataTable<Manufactures>(ds.Tables[10]);
                List<InspResult> inspResults = _sqlHelper.ConvertDataTable<InspResult>(ds.Tables[11]);
                List<FireDrillTypes> fireDrillTypes = _sqlHelper.ConvertDataTable<FireDrillTypes>(ds.Tables[12]);
                List<Site> sites = _sqlHelper.ConvertDataTable<Site>(ds.Tables[13]);
                List<SiteType> sitetype = _sqlHelper.ConvertDataTable<SiteType>(ds.Tables[14]);
                List<StateMaster> states = _sqlHelper.ConvertDataTable<StateMaster>(ds.Tables[15]);
                List<CityMaster> cities = _sqlHelper.ConvertDataTable<CityMaster>(ds.Tables[16]);
                httpResponseMessage.Result.Organization = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[5]).FirstOrDefault();
                httpResponseMessage.Result.Status = list;
                httpResponseMessage.Result.Score = scores;
                httpResponseMessage.Result.UserGroups = userGroups;
                httpResponseMessage.Result.Role = roles.ToList();
                httpResponseMessage.Result.BuildingTypes = buildingTypes;
                httpResponseMessage.Result.NotificationCategories = notificationCategory;
                httpResponseMessage.Result.NotificationEvents = notificationEvent;
                httpResponseMessage.Result.Manufactures = manufactures;
                httpResponseMessage.Result.InspResults = inspResults;
                httpResponseMessage.Result.InspStatus = inspStatuses;
                httpResponseMessage.Result.FireDrillTypes = fireDrillTypes;
                httpResponseMessage.Result.Sites = sites;
                httpResponseMessage.Result.SiteType = sitetype;
                httpResponseMessage.Result.CityMaster = cities;
                httpResponseMessage.Result.StateMaster = states;
            }
            return httpResponseMessage;
        }

        /// <summary>
        /// need to remove
        /// </summary>
        /// <returns></returns>
        public Organization GetWoMasterData()
        {
            Organization org;
            const string sql = StoredProcedures.Get_WoMasterData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = _sqlHelper.GetDataSet(command);
                List<Priority> priority = _sqlHelper.ConvertDataTable<Priority>(ds.Tables[0]);
                List<Department> department = _sqlHelper.ConvertDataTable<Department>(ds.Tables[1]);
                List<Actions> actions = _sqlHelper.ConvertDataTable<Actions>(ds.Tables[2]);
                List<Cause> cause = _sqlHelper.ConvertDataTable<Cause>(ds.Tables[3]);
                List<Problem> problem = _sqlHelper.ConvertDataTable<Problem>(ds.Tables[4]);
                List<Item> item = _sqlHelper.ConvertDataTable<Item>(ds.Tables[5]);
                List<Skills> skills = _sqlHelper.ConvertDataTable<Skills>(ds.Tables[6]);
                List<WoStatus> wostatus = _sqlHelper.ConvertDataTable<WoStatus>(ds.Tables[7]);
                List<SubStatus> substatus = _sqlHelper.ConvertDataTable<SubStatus>(ds.Tables[8]);
                List<Types> types = _sqlHelper.ConvertDataTable<Types>(ds.Tables[9]);
                org = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[10]).FirstOrDefault();
                List<Site> sites = _sqlHelper.ConvertDataTable<Site>(ds.Tables[11]);

                if (org != null)
                {
                    org.Priority = priority;
                    org.Department = department;
                    org.Action = actions;
                    org.Cause = cause;
                    org.Problem = problem;
                    org.Item = item;
                    org.Skills = skills;
                    org.WoStatus = wostatus;
                    org.SubStatus = substatus;
                    org.Types = types;
                    org.Sites = sites;
                }
            }
            return org;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetWoMastersData()
        {
            var httpResponseMessage = new HttpResponseMessage();
            const string sql = StoredProcedures.Get_WoMasterData;
            using (var command = new SqlCommand(sql))
            {
                httpResponseMessage.Result = new Result();
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                var priority = _sqlHelper.ConvertDataTable<Priority>(ds.Tables[0]);
                var department = _sqlHelper.ConvertDataTable<Department>(ds.Tables[1]);
                var actions = _sqlHelper.ConvertDataTable<Actions>(ds.Tables[2]);
                var cause = _sqlHelper.ConvertDataTable<Cause>(ds.Tables[3]);
                var problem = _sqlHelper.ConvertDataTable<Problem>(ds.Tables[4]);
                var item = _sqlHelper.ConvertDataTable<Item>(ds.Tables[5]);
                var skills = _sqlHelper.ConvertDataTable<Skills>(ds.Tables[6]);
                var woStatus = _sqlHelper.ConvertDataTable<WoStatus>(ds.Tables[7]);
                var subStatus = _sqlHelper.ConvertDataTable<SubStatus>(ds.Tables[8]);
                var types = _sqlHelper.ConvertDataTable<Types>(ds.Tables[9]);
                var sites = _sqlHelper.ConvertDataTable<Site>(ds.Tables[11]);
                httpResponseMessage.Result.Organization = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[10]).FirstOrDefault();

                httpResponseMessage.Result.Priorities = priority;
                httpResponseMessage.Result.Departments = department;
                httpResponseMessage.Result.Actions = actions;
                httpResponseMessage.Result.Cause = cause;
                httpResponseMessage.Result.Problem = problem;
                httpResponseMessage.Result.Item = item;
                httpResponseMessage.Result.Skills = skills;
                httpResponseMessage.Result.WoStatus = woStatus;
                httpResponseMessage.Result.SubStatus = subStatus;
                httpResponseMessage.Result.Types = types;
                httpResponseMessage.Result.Sites = sites;

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Score> GetScore()
        {
            var scores = new List<Score>();
            const string sql = StoredProcedures.Get_MasterData;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    scores = _sqlHelper.ConvertDataTable<Score>(ds.Tables[1]);
            }
            return scores.Where(x => x.IsActive).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Organization GetUserDashBoard(int userId)
        {
            var organization = new Organization();
            const string sql = StoredProcedures.Get_DashBoard;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        organization.Name = row["Name"].ToString();
                        organization.DashBoadImagePath = row["DashBoadImagePath"].ToString();
                        organization.FileName = row["FileName"].ToString();
                        organization.Country = row["Country"].ToString();
                        organization.State = row["State"].ToString();
                        organization.City = row["City"].ToString();
                        organization.Zip = row["Zip"].ToString();
                        organization.Services = new List<Menus>();
                        organization.Services = _sqlHelper.ConvertDataTable<Menus>(ds.Tables[2]);
                        organization.Category = new List<Category>();
                        organization.Category = _sqlHelper.ConvertDataTable<Category>(ds.Tables[1]);
                        organization.User = new UserProfile();
                    }
                }
            }
            return organization;
        }

        public Organization GetUserDashBoard(int userId, int menuId)
        {
            var organization = new Organization();
            const string sql = StoredProcedures.Get_UserMenu;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@menuId", menuId);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    organization = _sqlHelper.ConvertDataTable<Organization>(ds.Tables[1]).FirstOrDefault();
                    if (organization != null)
                        organization.Services = _sqlHelper.ConvertDataTable<Menus>(ds.Tables[0]);

                }
            }
            return organization;
        }

        public List<OrgServices> GetMenuOrganization(int id)
        {
            var organization = new List<OrgServices>();
            const string sql = StoredProcedures.HCFGet_Organization;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MenuID", id);
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    organization = _sqlHelper.ConvertDataTable<OrgServices>(ds.Tables[0]);
                }
            }
            return organization;
        }
        public bool IsPermitActiveByUserID(int userid, int permittype)
        {
            var IsExist = false;

            string sql = StoredProcedures.IsPermitActiveByUserID;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@permittype", permittype);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["IsExist"]) > 0)
                        IsExist = true;
                    else
                        IsExist = false;
                }

            }
            return IsExist;
        }
        public bool CheckMenuExist(string menuname, string orgkey)
        {
            var IsExist = false;

            string sql = StoredProcedures.CheckICRAPCRAMenuExist;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientId", orgkey);
                command.Parameters.AddWithValue("@MenuName", menuname);
                var dt = _sqlHelper.GetCommonDataTable(command);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["IsExist"]) > 0)
                        IsExist = true;
                    else
                        IsExist = false;
                }

            }
            return IsExist;
        }
        public int InsertMenuOrganization(OrgServices usermenu)
        {
            int newId;
            const string sql = StoredProcedures.Insert_UsermenuMapping;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MenuID", usermenu.MenuID);
                command.Parameters.AddWithValue("@OrganizationKey", usermenu.OrganizationKey);
                command.Parameters.AddWithValue("@CreatedBy", usermenu.Createdby);
                command.Parameters.AddWithValue("@Status", usermenu.Status);
                command.Parameters.AddWithValue("@ModuleId", usermenu.ModuleId);
                command.Parameters.AddWithValue("@ServiceMode", usermenu.ServiceMode);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        #endregion

        public Organization FireDrillSettings(Organization org)
        {
            const string sql = StoredProcedures.Insert_FireDrillSetting;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationId", org.OrganizationId);
                command.Parameters.AddWithValue("@PlanFireDrillsForMe", org.PlanFireDrillsForMe);

                _sqlHelper.ExecuteNonQuery(command);
            }
            return org;
        }

        public Organization PermitSettings(Organization org)
        {
            const string sql = StoredProcedures.Insert_PermitSettings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationId", org.OrganizationId);
                command.Parameters.AddWithValue("@NoticeText", org.NoticeText);
                _sqlHelper.ExecuteNonQuery(command);
            }
            return org;
        }

        public Organization UpdateOrganizationSettings(Organization org)
        {
            const string sql = StoredProcedures.Update_OrganizationSettings;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationId", org.OrganizationId);
                command.Parameters.AddWithValue("@NonTrackedEPReportDate", org.NonTrackedEPReportDate);

                _sqlHelper.ExecuteNonQuery(command);
            }
            return org;
        }
        public Organization GetMasterOrganization(string dbName)
        {
            var objOrganization = new Organization();
            const string sql = StoredProcedures.Get_MasterOrganization;
            using (var cmd = new SqlCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dbname", dbName);
                var dt = _sqlHelper.GetCommonDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objOrganization.Orgkey = new Guid(row["Orgkey"].ToString());
                        objOrganization.DashBoadImagePath = row["DashBoadImagePath"].ToString();
                        objOrganization.LogoName = row["logoName"].ToString();
                        objOrganization.LogoPath = row["LogoPath"].ToString();
                       // objOrganization.LogoBase64 = row["LogoBase64"].ToString();
                        objOrganization.FileName = row["FileName"].ToString();
                        objOrganization.Country = row["Country"].ToString();
                        objOrganization.ClientNo = Convert.ToInt32(row["ClientNo"].ToString());
                        objOrganization.Name = row["Name"].ToString();
                        objOrganization.IsTmsWo = Convert.ToBoolean(row["IsTmsWo"].ToString());
                        objOrganization.State = row["State"].ToString();
                        objOrganization.Zip = row["Zip"].ToString();
                        objOrganization.DbName = row["DbName"].ToString();
                        if (!string.IsNullOrEmpty(row["NonTrackedEPReportDate"].ToString()))
                            objOrganization.NonTrackedEPReportDate = Convert.ToDateTime(row["NonTrackedEPReportDate"].ToString());
                        //objOrganization.Orgkey = Convert.ToInt32(row["OrgKey"].ToString());

                    }
                }
            }
            return objOrganization;
        }

    }
  
}
