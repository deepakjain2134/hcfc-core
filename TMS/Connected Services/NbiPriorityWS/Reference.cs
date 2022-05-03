﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NbiPriorityWS
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.frsoft.com/webservices", ConfigurationName="NbiPriorityWS.PriorityWSSoap")]
    public interface PriorityWSSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frsoft.com/webservices/Load", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SDKBase))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CustomField[]))]
        System.Threading.Tasks.Task<NbiPriorityWS.Priority> LoadAsync(NbiPriorityWS.AuthenticationHeader header, string moduleCode, string code, int segmentID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frsoft.com/webservices/Save", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SDKBase))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CustomField[]))]
        System.Threading.Tasks.Task<NbiPriorityWS.Priority> SaveAsync(NbiPriorityWS.AuthenticationHeader header, NbiPriorityWS.Priority sdkCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frsoft.com/webservices/GetCodes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SDKBase))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CustomField[]))]
        System.Threading.Tasks.Task<NbiPriorityWS.Priority[]> GetCodesAsync(NbiPriorityWS.AuthenticationHeader header, string moduleCode, int segmentID, bool includeShared);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public partial class AuthenticationHeader
    {
        
        private string serverField;
        
        private string databaseField;
        
        private string userNameField;
        
        private string passwordField;
        
        private string passKeyField;
        
        private string hostNameField;
        
        private string openObjectIdField;
        
        private bool useSSOAuthenticationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Server
        {
            get
            {
                return this.serverField;
            }
            set
            {
                this.serverField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Database
        {
            get
            {
                return this.databaseField;
            }
            set
            {
                this.databaseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string Password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string PassKey
        {
            get
            {
                return this.passKeyField;
            }
            set
            {
                this.passKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string HostName
        {
            get
            {
                return this.hostNameField;
            }
            set
            {
                this.hostNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string OpenObjectId
        {
            get
            {
                return this.openObjectIdField;
            }
            set
            {
                this.openObjectIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public bool UseSSOAuthentication
        {
            get
            {
                return this.useSSOAuthenticationField;
            }
            set
            {
                this.useSSOAuthenticationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public partial class CustomField
    {
        
        private int fieldIdField;
        
        private int parentFieldIdField;
        
        private string nameField;
        
        private object valueField;
        
        private string valueDescriptionField;
        
        private bool isCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int FieldId
        {
            get
            {
                return this.fieldIdField;
            }
            set
            {
                this.fieldIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int ParentFieldId
        {
            get
            {
                return this.parentFieldIdField;
            }
            set
            {
                this.parentFieldIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public object Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string ValueDescription
        {
            get
            {
                return this.valueDescriptionField;
            }
            set
            {
                this.valueDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public bool IsCode
        {
            get
            {
                return this.isCodeField;
            }
            set
            {
                this.isCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeBase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Priority))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public abstract partial class SDKBase
    {
        
        private int segmentIDField;
        
        private int fieldIdField;
        
        private MODULES moduleIdField;
        
        private bool validateForDataImportField;
        
        private int keyIDField;
        
        private CustomField[] customFieldsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int SegmentID
        {
            get
            {
                return this.segmentIDField;
            }
            set
            {
                this.segmentIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int FieldId
        {
            get
            {
                return this.fieldIdField;
            }
            set
            {
                this.fieldIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public MODULES ModuleId
        {
            get
            {
                return this.moduleIdField;
            }
            set
            {
                this.moduleIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool ValidateForDataImport
        {
            get
            {
                return this.validateForDataImportField;
            }
            set
            {
                this.validateForDataImportField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int KeyID
        {
            get
            {
                return this.keyIDField;
            }
            set
            {
                this.keyIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=5)]
        public CustomField[] CustomFields
        {
            get
            {
                return this.customFieldsField;
            }
            set
            {
                this.customFieldsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public enum MODULES
    {
        
        /// <remarks/>
        NA,
        
        /// <remarks/>
        TASKS,
        
        /// <remarks/>
        RESOURCES,
        
        /// <remarks/>
        ASSETS,
        
        /// <remarks/>
        MATERIALS,
        
        /// <remarks/>
        WORK_ORDERS,
        
        /// <remarks/>
        VENDORS,
        
        /// <remarks/>
        TIME_CHARGES,
        
        /// <remarks/>
        PM,
        
        /// <remarks/>
        PE,
        
        /// <remarks/>
        PURCHASING,
        
        /// <remarks/>
        REPORTING,
        
        /// <remarks/>
        REQUISITIONS,
        
        /// <remarks/>
        PM_GENERATE,
        
        /// <remarks/>
        WEB_REQUEST,
        
        /// <remarks/>
        WO_SURVEY,
        
        /// <remarks/>
        QUICK_CLOSE,
        
        /// <remarks/>
        EMAIL_RESPONSE,
        
        /// <remarks/>
        UPGRADE,
        
        /// <remarks/>
        GENERAL,
        
        /// <remarks/>
        DOCUMENT_UPLOAD,
        
        /// <remarks/>
        CONTRACTS,
        
        /// <remarks/>
        PO_GENERATE,
        
        /// <remarks/>
        REQUISITION_GENERATE,
        
        /// <remarks/>
        REQUESTERS,
        
        /// <remarks/>
        CUSTOMERS,
        
        /// <remarks/>
        BLANKET_ORDERS,
        
        /// <remarks/>
        SUB_TASKS,
        
        /// <remarks/>
        DISPATCH,
        
        /// <remarks/>
        DEPARTMENTS,
        
        /// <remarks/>
        UTILITY,
        
        /// <remarks/>
        SAFETY_INCIDENTS,
        
        /// <remarks/>
        INFO_CENTER,
        
        /// <remarks/>
        HIS_LINKS,
        
        /// <remarks/>
        HL7,
        
        /// <remarks/>
        SDK,
        
        /// <remarks/>
        AUTOPRINT,
        
        /// <remarks/>
        ASSET_GROUPS,
        
        /// <remarks/>
        TASK_GROUPS,
        
        /// <remarks/>
        MED_TESTER,
        
        /// <remarks/>
        HIPAA_SURVEY,
        
        /// <remarks/>
        AUTO_ASSIGNMENT,
        
        /// <remarks/>
        ECRI_ALERTS,
        
        /// <remarks/>
        PS,
        
        /// <remarks/>
        SOURCEBASE_CODES,
        
        /// <remarks/>
        UMDNS_CODES,
        
        /// <remarks/>
        CONTRACT_PAYMENTS,
        
        /// <remarks/>
        CUSTOMER_CONTACTS,
        
        /// <remarks/>
        DASHBOARD,
        
        /// <remarks/>
        DISCUSSIONS,
        
        /// <remarks/>
        MATERIAL_ISSUES,
        
        /// <remarks/>
        MATERIAL_RECEIPTS,
        
        /// <remarks/>
        DOWNTIMES,
        
        /// <remarks/>
        USER_FIELDS,
        
        /// <remarks/>
        WO_TASKS,
        
        /// <remarks/>
        WO_ASSIGNMENTS,
        
        /// <remarks/>
        ASSET_CONTACTS,
        
        /// <remarks/>
        ASSET_COMPONENTS,
        
        /// <remarks/>
        WO_ASSETS,
        
        /// <remarks/>
        PM_ASSETS,
        
        /// <remarks/>
        CONTRACT_ASSETS,
        
        /// <remarks/>
        ASSET_MATERIALS,
        
        /// <remarks/>
        PM_MATERIALS,
        
        /// <remarks/>
        PARTS_SOURCE,
        
        /// <remarks/>
        DATASOURCE,
        
        /// <remarks/>
        SAVED_QUERIES,
        
        /// <remarks/>
        REPORT_DEFINITIONS,
        
        /// <remarks/>
        PM_ASSIGNMENTS,
        
        /// <remarks/>
        PM_EXCEPTIONS,
        
        /// <remarks/>
        PM_PROCEDURES,
        
        /// <remarks/>
        TASK_MATERIALS,
        
        /// <remarks/>
        VENDOR_MODELS,
        
        /// <remarks/>
        PATIENT,
        
        /// <remarks/>
        USER_PROFILES,
        
        /// <remarks/>
        PORTALS,
        
        /// <remarks/>
        INSPECTIONS,
        
        /// <remarks/>
        SEGMENTS,
        
        /// <remarks/>
        MESSAGE_CENTER,
        
        /// <remarks/>
        SEGMENTFIELDS,
        
        /// <remarks/>
        STORED_PROCEDURE_SCHEDULE,
        
        /// <remarks/>
        SCHEDULES,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Priority))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public abstract partial class CodeBase : SDKBase
    {
        
        private int idField;
        
        private string codeField;
        
        private string descriptionField;
        
        private bool showField;
        
        private bool showInQueryField;
        
        private bool isNewHierarchicalCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool Show
        {
            get
            {
                return this.showField;
            }
            set
            {
                this.showField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public bool ShowInQuery
        {
            get
            {
                return this.showInQueryField;
            }
            set
            {
                this.showInQueryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public bool IsNewHierarchicalCode
        {
            get
            {
                return this.isNewHierarchicalCodeField;
            }
            set
            {
                this.isNewHierarchicalCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.frsoft.com/webservices")]
    public partial class Priority : CodeBase
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface PriorityWSSoapChannel : NbiPriorityWS.PriorityWSSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class PriorityWSSoapClient : System.ServiceModel.ClientBase<NbiPriorityWS.PriorityWSSoap>, NbiPriorityWS.PriorityWSSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public PriorityWSSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(PriorityWSSoapClient.GetBindingForEndpoint(endpointConfiguration), PriorityWSSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PriorityWSSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(PriorityWSSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PriorityWSSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(PriorityWSSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PriorityWSSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<NbiPriorityWS.Priority> LoadAsync(NbiPriorityWS.AuthenticationHeader header, string moduleCode, string code, int segmentID)
        {
            return base.Channel.LoadAsync(header, moduleCode, code, segmentID);
        }
        
        public System.Threading.Tasks.Task<NbiPriorityWS.Priority> SaveAsync(NbiPriorityWS.AuthenticationHeader header, NbiPriorityWS.Priority sdkCode)
        {
            return base.Channel.SaveAsync(header, sdkCode);
        }
        
        public System.Threading.Tasks.Task<NbiPriorityWS.Priority[]> GetCodesAsync(NbiPriorityWS.AuthenticationHeader header, string moduleCode, int segmentID, bool includeShared)
        {
            return base.Channel.GetCodesAsync(header, moduleCode, segmentID, includeShared);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.PriorityWSSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.PriorityWSSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.PriorityWSSoap))
            {
                return new System.ServiceModel.EndpointAddress("https://rwjbh.tmsonline.com/tmsconnect/Codes/PriorityWS.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.PriorityWSSoap12))
            {
                return new System.ServiceModel.EndpointAddress("https://rwjbh.tmsonline.com/tmsconnect/Codes/PriorityWS.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            PriorityWSSoap,
            
            PriorityWSSoap12,
        }
    }
}
