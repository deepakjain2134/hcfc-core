using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HCF.DAL
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public NotificationRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public  List<NotificationEmails> GetNotificationEmails()
        {
            List<NotificationEmails> emails = new List<NotificationEmails>();
            const string sql = StoredProcedures.Get_NotificationEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    emails = _sqlHelper.ConvertDataTable<NotificationEmails>(dt);
                }
            }
            return emails;
        }
        
        public  NotificationEmails GetNotificationEmailsAddress(int catId, string BuildingId, string FloorId, string SiteID, int? EventId = 1)
        {
            List<NotificationEmails> NotificationEmails = new List<NotificationEmails>();
            NotificationEmails objemails = new NotificationEmails();

            string TOEmails = "";
            string CCEmails = "";
            bool IsAlreadyDefaultSet = false;
            const string sql = StoredProcedures.Get_FilterNotificationEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NotificationCatId", catId);
                command.Parameters.AddWithValue("@NotificationEventId", EventId == null ? 1 : EventId);
                command.Parameters.AddWithValue("@FloorId", 0);
                command.Parameters.AddWithValue("@BuildingId", BuildingId);
                var dt = _sqlHelper.GetDataTable(command);
                if (dt != null)
                {
                    NotificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(dt);
                    if (NotificationEmails != null && NotificationEmails.Count > 0)
                    {

                        if (!string.IsNullOrEmpty(BuildingId))
                        {
                            foreach (var building in BuildingId.Split(','))
                            {
                                foreach (var emails in NotificationEmails)
                                {
                                    if (emails.BuildingId == Convert.ToInt32(building))
                                    {
                                        TOEmails += emails.NotificationEmail + ",";
                                        CCEmails += emails.NotificationCCEmail + ",";
                                        break;
                                    }
                                    else if (emails.BuildingId == 0)// for default if  particular building data not found only one time
                                    {
                                        if (IsAlreadyDefaultSet == false)
                                        {
                                            TOEmails += emails.NotificationEmail + ",";
                                            CCEmails += emails.NotificationCCEmail + ",";
                                            IsAlreadyDefaultSet = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var emails in NotificationEmails)
                            {
                                if (emails.BuildingId == 0)// for default if  particular building data not found only one time
                                {
                                    if (IsAlreadyDefaultSet == false)
                                    {
                                        TOEmails += emails.NotificationEmail + ",";
                                        CCEmails += emails.NotificationCCEmail + ",";
                                        IsAlreadyDefaultSet = true;
                                    }
                                }

                            }
                        }

                        objemails.NotificationCatId = catId;
                        objemails.NotificationEventId = EventId;
                        objemails.NotificationEmail = !string.IsNullOrEmpty(TOEmails) ? TOEmails.TrimEnd(',') : string.Empty;
                        objemails.NotificationCCEmail = !string.IsNullOrEmpty(CCEmails) ? CCEmails.TrimEnd(',') : string.Empty;
                    }
                }
            }
            return objemails;
        }

        public  int AddNotificationEmails(NotificationEmails objectData)
        {
            int newId;
            const string sql = StoredProcedures.Add_NotificationEmails;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", objectData.Id);
                command.Parameters.AddWithValue("@NotificationCatId", objectData.NotificationCatId);
                command.Parameters.AddWithValue("@NotificationCCEmail", objectData.NotificationCCEmail);
                command.Parameters.AddWithValue("@NotificationEventId", objectData.NotificationEventId > 0 ? objectData.NotificationEventId : (object)null);
                command.Parameters.AddWithValue("@Type", objectData.Type);
                command.Parameters.AddWithValue("@IsActive", objectData.IsActive);
                command.Parameters.AddWithValue("@NotificationEmail", objectData.NotificationEmail);
                command.Parameters.AddWithValue("@BuildingId", objectData.BuildingId > 0 ? objectData.BuildingId : (object)null);
                command.Parameters.AddWithValue("@SiteId", objectData.SiteId > 0 ? objectData.SiteId : (object)null);
                command.Parameters.AddWithValue("@FloorId", objectData.FloorId > 0 ? objectData.FloorId : (object)null);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        
        public  List<NotificationMapping> GetNotificationMatrix()
        {
            List<NotificationMapping> lists = new List<NotificationMapping>();
            const string sql = StoredProcedures.Get_NotificationMatrix;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<NotificationEmails> notificationEmails = new List<NotificationEmails>();
                    if (ds.Tables[1]!=null && ds.Tables[1].Rows.Count>0)
                    {
                       notificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(ds.Tables[1]);
                    }

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            var list = new NotificationMapping
                            {
                                NotificationMappingId = Convert.ToInt32(row["NotificationMappingId"].ToString()),
                                Status = !string.IsNullOrEmpty(row["Status"].ToString()) ? Convert.ToInt32(row["Status"].ToString()) : -1,
                                NotificationCatId = Convert.ToInt32(row["NotificationCatId"].ToString()),
                                NotificationEventId = string.IsNullOrWhiteSpace(row["NotificationEventId"].ToString())
                                    ? 0
                                    : Convert.ToInt32(row["NotificationEventId"].ToString()),
                                NotificationCategory = new NotificationCategory
                                {
                                    CategoryName = row["CategoryName"].ToString(),
                                    EnumValue = row["EnumValue"].ToString(),
                                    CategoryDescription = row["CategoryDescription"].ToString(),
                                    NotificationCatId = Convert.ToInt32(row["NotificationCatId"].ToString()),
                                    NotifyEmails = notificationEmails.Where(x => x.NotificationCatId == Convert.ToInt32(row["NotificationCatId"].ToString())).FirstOrDefault()

                                },
                                NotificationEvent = new NotificationEvent
                                {
                                    NotificationEventId = Convert.ToInt32(row["NotificationEventId"].ToString()),
                                    EventDescription = row["EventDescription"].ToString(),
                                    EventName = row["EventName"].ToString()
                                }
                            };
                            lists.Add(list);
                        }

                    }
                }

            }

            return lists;
        }

        public  bool UpdateNotificationMatrix(NotificationMapping objNotificationMapping)
        {
            const string sql = StoredProcedures.Update_NotificationMatrix;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NotificationCatId", objNotificationMapping.NotificationCatId);
                command.Parameters.AddWithValue("@NotificationEventId", objNotificationMapping.NotificationEventId);
                command.Parameters.AddWithValue("@Status", objNotificationMapping.Status);
                return _sqlHelper.ExecuteNonQuery(command);
            }
        }

        public  List<NotificationCategory> GetNotificationMatrixByCategory()
        {
            var lists = GetNotificationMatrix();
            var categoryLists = GetNotificationCategory();
            foreach (var item in categoryLists)
                item.NotificationMappings = lists.Where(x => x.NotificationCatId == item.NotificationCatId).ToList();
            return categoryLists;
        }
      
        public  List<NotificationCategory> GetNotificationCategory()
        {
            List<NotificationCategory> notificationCategory;
            const string sql = StoredProcedures.Get_NotificationCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                notificationCategory = _sqlHelper.ConvertDataTable<NotificationCategory>(ds.Tables[0]);
                var notificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(ds.Tables[1]);
                foreach (var item in notificationCategory)
                {
                    item.NotifyEmails = notificationEmails.Where(x => x.NotificationCatId == item.NotificationCatId).FirstOrDefault();
                }
            }
            return notificationCategory;
        }

        public  List<NotificationCategory> GetNotifications(string mode, string id)
        {
            List<NotificationCategory> notificationCategory = new List<NotificationCategory>();
            const string sql = StoredProcedures.Get_Notifications;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    notificationCategory = _sqlHelper.ConvertDataTable<NotificationCategory>(ds.Tables[0]);
                    var notificationEvents = _sqlHelper.ConvertDataTable<NotificationEvent>(ds.Tables[1]);
                    var notificationMapping = _sqlHelper.ConvertDataTable<NotificationMapping>(ds.Tables[1]);
                    foreach (var item in notificationMapping)
                        item.NotificationEvent = notificationEvents.FirstOrDefault(x => x.NotificationEventId == item.NotificationEventId);


                    var notificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(ds.Tables[2]);

                    if (!string.IsNullOrEmpty(mode) && !string.IsNullOrEmpty(id))
                        notificationEmails = FilterEmails(notificationEmails, id, mode);
                    else if (!string.IsNullOrEmpty(mode) && mode == "c")
                        notificationEmails = FilterEmails(notificationEmails, id, mode);


                    foreach (var item in notificationCategory)
                    {
                        item.NotificationMappings = notificationMapping.Where(x => x.NotificationCatId == item.NotificationCatId).ToList();
                        foreach (var notificationMappings in item.NotificationMappings)
                        {
                            if (notificationMappings.NotificationEvent != null)
                            {
                                notificationMappings.NotificationEmails =
                                    notificationEmails.Where(x => x.NotificationCatId == item.NotificationCatId
                                    && x.NotificationEventId == notificationMappings.NotificationEventId).FirstOrDefault();
                            }
                        }
                        item.NotifyEmails = notificationEmails.Where(x => x.NotificationCatId == item.NotificationCatId && x.NotificationEventId == null).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    }
                }
            }
            return notificationCategory;
        }

        public  NotificationEmails GetNotifications(string catId, string eventId, string mode, string searchId)
        {
            List<NotificationEmails> notificationEmails = new List<NotificationEmails>();
            const string sql = StoredProcedures.Get_Notifications;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    List<NotificationCategory> notificationCategories = _sqlHelper.ConvertDataTable<NotificationCategory>(ds.Tables[0]);
                    List<NotificationEvent> notificationEvents = _sqlHelper.ConvertDataTable<NotificationEvent>(ds.Tables[1]);
                    notificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(ds.Tables[2]);
                    foreach (var item in notificationEmails)
                    {
                        item.NotifyCategory = notificationCategories.FirstOrDefault(x => x.NotificationCatId == item.NotificationCatId);
                        if (item.NotificationEventId.HasValue)
                            item.NotificationEvent = notificationEvents.FirstOrDefault(x => x.NotificationEventId == item.NotificationEventId.Value);
                    }

                    if (!string.IsNullOrEmpty(catId) && Convert.ToInt32(catId) > 0)
                        notificationEmails = notificationEmails.Where(x => x.NotificationCatId == Convert.ToInt32(catId)).ToList();

                    if (!string.IsNullOrEmpty(eventId) && Convert.ToInt32(eventId) > 0)
                        notificationEmails = notificationEmails.Where(x => x.NotificationEventId == Convert.ToInt32(eventId)).ToList();
                    else
                        notificationEmails = notificationEmails.Where(x => x.NotificationEventId == null).ToList();

                    if (string.IsNullOrEmpty(mode))
                        mode = "c";

                    notificationEmails = FilterEmails(notificationEmails, searchId, mode);

                    if (notificationEmails.Count == 0)
                    {
                        NotificationEmails obj = new NotificationEmails();
                        obj.NotificationCatId = Convert.ToInt32(catId);
                        obj.NotificationEventId = Convert.ToInt32(eventId);
                        obj.NotifyCategory = notificationCategories.FirstOrDefault(x => x.NotificationCatId == obj.NotificationCatId);
                        if (obj.NotificationEventId.HasValue)
                            obj.NotificationEvent = notificationEvents.FirstOrDefault(x => x.NotificationEventId == obj.NotificationEventId.Value);
                        notificationEmails.Add(obj);
                    }

                }
            }
            return notificationEmails.OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public  NotificationEmails GetNotification(int notiCatId, int? eventId, string mode, string id)
        {
            int buildingId = 0;
            int siteId = 0;
            int floorId = 0;
            List<NotificationEmails> notificationEmails = new List<NotificationEmails>();
            const string sql = StoredProcedures.Get_NotificationMail;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    notificationEmails = _sqlHelper.ConvertDataTable<NotificationEmails>(ds.Tables[0]);
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        buildingId = Convert.ToInt32(item["BuildingId"].ToString());
                        floorId = Convert.ToInt32(item["FloorId"].ToString());
                        siteId = Convert.ToInt32(item["SiteId"].ToString());
                    }
                    notificationEmails = notificationEmails.Where(x => x.NotificationCatId == notiCatId).ToList();

                    var list = new List<NotificationEmails>();
                    notificationEmails = eventId.HasValue ? notificationEmails.Where(x => x.NotificationEventId == Convert.ToInt32(eventId)).ToList() : notificationEmails.Where(x => x.NotificationEventId == null).ToList();

                    if (floorId > 0)
                        list = FilterEmails(notificationEmails, floorId.ToString(), "f");

                    if (buildingId > 0 && list.Count == 0)
                        list = FilterEmails(notificationEmails, buildingId.ToString(), "b");

                    if (siteId > 0 && list.Count == 0)
                        list = FilterEmails(notificationEmails, siteId.ToString(), "s");

                    if (list.Count == 0)
                        list = FilterEmails(notificationEmails, "", "c");

                    if (string.IsNullOrEmpty(mode))
                        mode = "c";

                    notificationEmails = FilterEmails(notificationEmails, id, mode);
                }
            }
            return notificationEmails.FirstOrDefault();
        }

        private  List<NotificationEmails> FilterEmails(List<NotificationEmails> notificationEmails, string searchId, string mode)
        {
            List<NotificationEmails> notifications = new List<NotificationEmails>();
            if (mode == "c")
                return notificationEmails.Where(x => x.SiteId == null && x.BuildingId == null && x.FloorId == null).ToList();
            else if (mode == "b")
                return notificationEmails.Where(x => x.BuildingId.HasValue && x.BuildingId.Value == Convert.ToInt32(searchId)).ToList();
            else if (mode == "s")
                return notificationEmails.Where(x => x.SiteId.HasValue && x.SiteId.Value == Convert.ToInt32(searchId)).ToList();
            else if (mode == "f")
                return notificationEmails.Where(x => x.FloorId.HasValue && x.FloorId.Value == Convert.ToInt32(searchId)).ToList();
            return notifications;
        }       
    }
}
