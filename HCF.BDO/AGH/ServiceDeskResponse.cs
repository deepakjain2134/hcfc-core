using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HCF.BDO.AGH
{
    [DataContract]
    public class ResponseStatus
    {
        [DataMember(EmitDefaultValue = false)]
        public int status_code { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string status { get; set; }
    }
    [DataContract]
    public class SearchCriteria
    {
        [DataMember(EmitDefaultValue = false)]
        public string condition { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string field { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string value { get; set; }
    }

    public class ListInfo
    {
        public bool has_more_rows { get; set; }
        public int start_index { get; set; }
        public SearchCriteria search_criteria { get; set; }
        public int row_count { get; set; }
    }
    [DataContract]
    public class Template
    {
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
    }
    [DataContract]
    public class Category
    {
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
    }
    [DataContract]
    public class Mode
    {
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
    }
   
    [DataContract]
    public class Site
    {
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
    }

    [DataContract]
    public class Group
    {
        [DataMember(EmitDefaultValue = false)]
        public Site site { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
    }
    

    public class Department
    {
        public Site site { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    [DataContract]
    public class Requester
    {
        [DataMember(EmitDefaultValue = false)]
        public string email_id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_technician { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object sms_mail { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string phone { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object mobile { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string photo_url { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_vip_user { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Department department { get; set; }
    }

    public class CreatedTime
    {
        public string display_value { get; set; }
        public string value { get; set; }
    }
    [DataContract]
    public class Technician
    {
        [DataMember(EmitDefaultValue = false)]
        public string email_id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string cost_per_hour { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string phone { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string mobile { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string photo_url { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string sms_mail_id { get; set; }
    }

    public class CreatedBy
    {
        public string email_id { get; set; }
        public bool is_technician { get; set; }
        public object sms_mail { get; set; }
        public object phone { get; set; }
        public string name { get; set; }
        public object mobile { get; set; }
        public string id { get; set; }
        public string photo_url { get; set; }
        public bool is_vip_user { get; set; }
        public Department department { get; set; }
    }
    [DataContract]
    public class Status
    {
        [DataMember(EmitDefaultValue = false)]
        public bool in_progress { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string internal_name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool stop_timer { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string color { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
    }
    [DataContract]
    public class Request
    {
        [DataMember(EmitDefaultValue = false)]
        public Template template { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string display_id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string subject { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object notification_status { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string description { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object responded_time { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_read { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_service_request { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string id { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Group group { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Category category { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Mode mode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Requester requester { get; set; }
        [DataMember(EmitDefaultValue = false)]

        //public CreatedTime created_time { get; set; }
        //[DataMember(EmitDefaultValue = false)]

        public bool has_draft { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool has_attachments { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object approval_status { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object sla { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_overdue { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Technician technician { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public CreatedBy created_by { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object due_by_time { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_fcr { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object first_response_due_by_time { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Site site { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool is_first_response_overdue { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool has_notes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Status status { get; set; }

        public created_time created_time { get; set; }



    }

    public class created_time
    {
        public string display_value { get; set; }
        public long value { get; set; }
    }

    public class ServiceDeskRequestsResponse
    {
        public List<ResponseStatus> response_status { get; set; }
        public ListInfo list_info { get; set; }
        public List<Request> requests { get; set; }
    }
    public class ServiceDeskRequestResponse
    {
        public Request request { get; set; }
    }

    #region requestconditions
    public class RequestConditions
    {
        public class Value
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class MainSearchCriteria
        {
            public string field { get; set; }
            public string condition { get; set; }
        }

        public class SearchCriteria : MainSearchCriteria
        {
            public Value value { get; set; }
        }

        public class SearchCriteria2 : MainSearchCriteria
        {
            public string value { get; set; }
        }

        public class ListInfo
        {
            public int start_index { get; set; }
            public int row_count { get; set; }
            public string sort_field { get; set; }
            public string sort_order { get; set; }
            public MainSearchCriteria search_criteria { get; set; }
        }

        public class RequestCondition
        {
            public ListInfo list_info { get; set; }
        }
    }
    #endregion

}