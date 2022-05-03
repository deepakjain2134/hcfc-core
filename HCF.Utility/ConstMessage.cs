namespace HCF.Utility
{
    public class APIUrls
    {

        public static readonly string Common_AddSubBinder = "repo/addBinders";
        public static readonly string Common_ReplyDocument = "repo/ReplyDocuments";
        public static readonly string Common_ForwardDocument = "repo/SampleDocuments";
        public static readonly string Documents_AddDocumentType = "repo/addDocumentType";
        public static readonly string Documents_SaveDocumentMasters = "repo/SaveDocumentMaster";
        public static readonly string Repository_GetDocumentNotificationdata = "repo/GetDocumentNotificationdata";
        public static readonly string Repository_AddBinders = "repo/addBinders";
        public static readonly string Repository_AddDocumentTypeMaster = "repo/addDocumentTypeMaster";
        public static readonly string Repository_EditDocumentTypeMaster = "repo/editDocumentTypeMaster";


        public static readonly string Reports_CreateReports = "reports/saveinsreport";
        public static readonly string Reports_GetAutoReportdata = "reports/GetAutoReportdata";

        public static readonly string Rounds_AddRoundsQuestonaries = "rounds/AddRoundsQuestionnaires";
        public static readonly string Rounds_Save = "rounds/AddRoundInspection";
        public static readonly string Rounds_AddRoundsCommQuestonaries = "rounds/AddRoundsCommQuestonaries";




        public static readonly string WorkOrder_SyncWorkOrders = "wo/syncworkorders";
        public static readonly string WorkOrder_WorkOrders = "wo/getIssuesPaging";

        public static readonly string WorkOrder_LoadWorkOrderData = "tms/GetTmsResource";
        public static readonly string WorkOrder_Create = "tms/WorkOrderSave";
        public static readonly string WorkOrderfiles_Save = "tms/WorkOrderfiles";
        public static readonly string WorkOrder_TMS_To_ToLocal = "tms/SaveTmsWorkOrder";
        public static readonly string WorkOrder_UpdateWorkOrder = "tms/WorkOrderSave";

        public static readonly string WorkOrder_Save_Wo_Inspection = "trans/SaveWoInspection";
        public static readonly string WorkOrder_SaveStatus = "trans/UpdateIssue";
        public static readonly string Rounds_FloorRoundInspection = "trans/saveFloorInspections";
        public static readonly string Goal_AddInspectionFiles = "trans/SaveInspectionFiles";
        public static readonly string Goal_GenerateIlsmReport = "trans/SaveTilsmReport";
        public static readonly string Goal_AddfixedInsDate = "trans/addFixedInspectionDate";
        public static readonly string Goal_EPInspectionDate = "trans/addTEPInspectionDate";
        public static readonly string Goal_UpdateInspection = "trans/UpdateInspection";
        public static readonly string Goal_SaveIlsm = "trans/AddIlsmInspection";
        public static readonly string Save_RouteInspection = "trans/SaveRouteInspection";
        public static readonly string Common_UpdateStepDeficeincyStatus = "trans/Updatetrsetp";
        public static readonly string Goal_InspectionReportUpdate = "trans/UploadFloorAssetsInsReport";
        public static readonly string Goal_Save = "trans/AddInspection";
        public static readonly string Assets_MarkPassAssets = "trans/MarkPassAssets";


        // public static readonly string Vendor_Addvendors_Add = "user/addvendor";
        // public static readonly string Vendor_Addvendors_Update = "user/updateVendor";
        // public static readonly string User_UserAdd_Add = "user/CreateUser";
        // public static readonly string User_UserAdd_Update = "user/UpdateUser";

        public static readonly string ICRA_AddConstructionType = "icra/addConstructionType";
        public static readonly string ICRA_AddConstuctionActivity = "icra/addConstuctionActivity";
        public static readonly string ICRA_AddConstructionRisk = "icra/addConstructionRisk";
        public static readonly string ICRA_AddConstructionClass = "icra/addConstructionClass";
        public static readonly string ICRA_AddConstClassActivity = "icra/addConstClassActivity";
        public static readonly string ICRA_AddICRASteps = "icra/addICRASteps";
        public static readonly string ICRA_MngICRARiskArea = "icra/addICRARiskArea";
        public static readonly string ICRA_AddInspectionIcra = "icra/addInspectionIcra";
        public static readonly string ICRA_SaveSingReport = "icra/SaveSignedReport"; // to save files of icra reports

        public static readonly string ICRA_GenerateObservationReport = "icra/SaveObservationReport";



        public static readonly string Assets_AddImportedFloorAssets = "assets/AddImportedFloorAssets";
        public static readonly string Assets_tfloorAssetEdit_Edit = "assets/editfloorassets";
        public static readonly string Assets_tfloorAssetEdit_Add = "assets/addfloorassets";
        public static readonly string Assets_EditAssets = "assets/EditAssets";
        public static readonly string Common_MovetfloorAsset = "assets/Movefloorassets";

        //public static readonly string Vendor_AddVendorResources_Add = "user/AddVendorResources";
        //public static readonly string Vendor_AddVendorResources_Update = "user/updateVendorResource";
        public static readonly string Certificate_Add = "user/addcertificates";
        public static readonly string Account_ForgotPassword = "user/ForgotPassword";
        public static readonly string User_Manage = "user/UpdatePassword";



        public static readonly string Organization_UpdateFloorPlanThumbImage = "org/UpdateFloorPlanThumbImage";
        public static readonly string Organization_General = "org/UpdateOrg";
        public static readonly string Organization_EditFloor = "org/AddFloor";
        public static readonly string Menu_ManageMenu = "org/UpdateMenu";



        public static readonly string Rounds_GenerateFireDrillDoc = "exec/GenerateFiredrillDoc";
        public static readonly string Exercises_GetFiredrillNotification = "exec/GetFiredrillNotification";
        public static readonly string Rounds_FireDrill = "exec/addNewExercises";
        public static readonly string Rounds_FireDrills = "exec/AddTExercises";
        public static readonly string Rounds_ManageQuestionnaires = "exec/addFiredrillQuestionnaries";


    }

    public class ConstMessage
    {
        public static readonly string Round_Not_exist = "Round Inspection # not found";
        public static readonly string Not_exist = "Permit # doesn't exist";
        public static readonly string NotAuthorized_Msg = "You are not authorized to view this page";
        public static readonly string PermissionDataTokenName = "_requiredPermission";
        public static readonly string Success_Authentication_Msg = "Success";
        public static readonly string Error_Authentication_Msg = "You are not registered with us. Please contact admin.";
        public static readonly string Invalid_UserName_and_Password = "Invalid user name or password";
        public static readonly string Invalid_Parameter = "Invalid Parameter";
        public static readonly string Password_Change_Success = "Password Changed successfully.";
        public static readonly string Insert_floor_Success = "Floor Inserted successfully.";
        public static readonly string Update_floor_Success = "Floor Updated successfully.";
        public static readonly string Update_floorPlan_Success = "FloorPlan Updated successfully.";
        public static readonly string Delete_floor_Success = "Floor Deleted successfully.";
        public static readonly string Insert_Profile_Success = "User Created Successfully.";
        public static readonly string Update_Profile_Success = "Profile Updated successfully.";
        public static readonly string Delete_Profile_Success = "Profile Deleted successfully.";
        public static readonly string Recovery_Success = "Recovery instruction was sent to ";
        public static readonly string Recovery_Error = "Recovery instruction was not sent to  ";
        public static readonly string Insert_Building_Success = "Building Inserted successfully.";
        public static readonly string Update_Building_Success = "Building Updated successfully.";
        public static readonly string Module_Training = " Updated successfully.";

        public static readonly string Delete_Building_Success = "Building Deleted successfully.";
        public static readonly string Current_Password_And_New_Password_Mathch = "The new password must be different from the current password.";
        public static readonly string Current_Password_NotMatch_With_Password = "Current password is incorrect";
        public static readonly string Floor_Assets_Add_AlreadyExists = "Asset # already registered.";
        public static readonly string Floor_Assets_Add_Success = "Asset added successfully.";
        public static readonly string AssetTypeCode_AlreadyExists = "Asset add failed. Asset type code already exists.";
        public static readonly string Insert_Manufacture_Success = "Manufacture saved successfully.";
        public static readonly string User_InActive = "InActive User.";
        public static readonly string Floor_Assets_Update_Sucessfully = "Asset updated successfully.";
        public static readonly string Floor_Assets_Remove_Sucessfully = "Asset removed successfully.";
        public static readonly string Floor_Assets_Move_Sucessfully = "Asset Moved Successfully.";
        public static readonly string Document_Reply_Mail_Sent_Successfully = "Reply message sent successfully";
        public static readonly string Floor_Assets_Is_In_Used = "Asset is already inspected.";
        public static readonly string Mail_Sent_Successfully = "Mail sent Successfully.";
        public static readonly string Mail_UnSuccessful = "Mail Send Failed";
        public static readonly string Document_Not_Found = "Document Not Found";
        public static readonly string Document_Deleted_Successfully = "Document Deleted Successfully.";
        public static readonly string No_Records_Found = "No Record Found";
        public static readonly string Contact_Admin = "Please Contact Admin.";
        public static readonly string Error = "Error Occurred";
        public static readonly string Attachment_Marked_IsUsed_Successfully = "Attachment marked IsUsed Successfully";
        public static readonly string Inspection_Added_Successfully = "Inspection Added Successfully";
        public static readonly string Score_Updated_Successfully = "Score Updated Successfully";
        public static readonly string News_Updated_Successfully = "News Updated Successfully";
        public static readonly string vendor_Add_Sucess = "Vendor added successfully";
        public static readonly string Update_vendor_Success = "Vendor has been updated successfully.";
        public static readonly string Success = "added successfully";
        public static readonly string Success_Updated = "Updated successfully";
        public static readonly string User_Already_Exist = "User With This Email Already Exists";
        public static readonly string EpGroup_Already_Exist = "EpGroup With This Name Already Exists";
        public static readonly string Vendor_Already_Exist = "Vendor Already Exist.";
        public static readonly string Document_Added_Successfully = "Document Uploaded Successfully.";
        public static readonly string Round_Inspection_Added_Successfully = "Round Inspection Added Successfully.";
        public static readonly string LoggedOutSucess = "You have successfully logged out.";
        public static readonly string Password_Does_Not_Exist = "Current password is wrong.";
        public static readonly string Mail_Sent_Successfully_Register_Mail = "Mail Sent Successfully On Your Register Email.";
        public static readonly string Certificate_Added_Successfully = "Certificate Added Successfully.";
        public static readonly string EP_Assign_Successfully = "EP Assign Successfully.";
        public static readonly string Construction_Inspection_Added_Successfully = "Construction Inspection Added Successfully.";
        public static readonly string Issue_Updated_Successfully = "Issue Updated Successfully.";
        public static readonly string Update_Organization_Success = "Organization Updated successfully.";
        public static readonly string Delete_Successfully = "Deleted Successfully.";
        public static readonly string Record_Already_Exists = "Record already exists.";
        public static readonly string Already_Exists = "{0} already exists.";
        public static readonly string Not_Found = "Not Found";
        public static readonly string Workorder_Open_Against_Steps = "WorkOrder is not completed against this steps first close the work order.";
        public static readonly string Frequency_Notupadted_Inprogress = "Frequency can not update due to inspection is In Progress";
        public static readonly string Forgot_Password_User_Not_Found = "Sorry, we don't recognize that email address.";
        public static readonly string InActive_vendor = "vendor is inactive.";
        public static readonly string InActive_vendor_user = "User vendor account has been de-activated.";
        public static readonly string Defeciencies_Message = "There are still pending deficiencies since last inspection.Please fix it before proceeding with new inspection";
        public static readonly string Insert_Schedule_Success = "Schedule created successfully.";
        public static readonly string Update_Schedule_Success = "Schedule updated successfully.";
        public static readonly string Schedule_Already_Exist = "Schedule already exist";
        public static readonly string TunnelDown = "We have assigned a temporary # {0}. However it is not in your Work Order system as yet because connection to work order system is currently unavailable.This number will be automatically replaced when Work Order system becomes available.";
        //"Access to work order system is not available right now. CRx has saved your information and work order will be created/update as soon as the access to work order system is available.";
        public static readonly string MandatoryFieldsError = "Required Mandatory Fields are missing in body";
        public static readonly string Internal_Server_Error = "Internal Server Error";
        public static readonly string DocumentTypeMaster_Error = "Document Type Name already exists.";
        public static readonly string FloorCode_Error = "Floor code already exists.";
        public static readonly string Tag_you_entered_does_not_exist = "Tag/BarCode you entered does not exists.";
        public static readonly string StopCode_you_entered_does_not_have_any_assets = "Stop Code you entered does not have any assets.";
        public static readonly string Inactive_User_login_Msg = "your account has been disabled please contact administrator.";

        public static readonly string Route_Name_Already_Exist = "Route name already exist";
        public static readonly string Route_Name_Blank = "Please enter route #";
        public static readonly string Saved_Successfully = "Saved successfully.";
        public static readonly string Generated_Successfully = "Generated successfully.";

        public static readonly string Vendor_Successfully = "Vendor saved successfully.";

        public static readonly string Tip_send_for_approval = "Your tip has been send for approval";
        public static readonly string Tip_Added = "Tip added successfully";
        public static readonly string Tip_Exists = "Tips Already Exists for selected Module!";
        public static readonly string User_InActive_Msg = "Your account has been disabled.";
        //  public static readonly string User_InActive_Msg = "This user is inactive. please contact administrator.";
        public static readonly string ImportCSV_ColumnsError = "([columns]) following column does not match from import file. Please download sample file and add the record then upload the data.";
        public static readonly string ImportCSV_fileNotMatch = "Please upload only [.CSV] file.";
        public static readonly string ImportCSV_fileRequired = "Please Upload Your file";
        public static readonly string ImportCSV_fileTypeRequired = "Please select file type";
        public static readonly string ImportCSV_NoRecord = "No data available in table";
        public static readonly string ImportCSV_UpdateSuccess = "Exists record has been update successfully.";
        public static readonly string ImportCSV_UpdateFail = "Update failed";
        public static readonly string ICRAToILSM_SaveMessage = "[IncidentId]  has been saved successfully.";
        public static readonly string LinkICRAToILSM_Success = "ICRA link with ILSM Successfully";
        public static readonly string LinkICRAToILSM_Fail = "ICRA not linked with ILSM";

        public static readonly string Project_Created_Successfully = "Project Created Successfully.";
        public static readonly string Project_Updated_Successfully = "Project Updated Successfully.";
        public static readonly string Project_Deleted_Successfully = "Project Deleted Successfully.";

        public static readonly string PermitSettingAlreadyExists = "Permit work flow settings already exists.";
        public static readonly string AscOrderSequence = "Sequence is not in ascending order";
        public static readonly string PermitSetting_Saved_Successfully = "Saved Successfully.";
        public static readonly string IcraProjectAlreadyExists = "Project already exists.";
        public static readonly string PermitNoExists = "already exists!";
        public static readonly string RoundExists = "Round name already exists!";
        public static readonly string SameOldNewPassword = "The new password must be different from the old password.";
        public static readonly string MisMatchCurrentPassword = "The password you have entered does not match your current one.";
        public static readonly string User_account_lockout = "Your account has been locked. Please contact your System Admin.";
        public static readonly string Observation_report_link_with_ILSM_Successfully = "Observation report link with ILSM Successfully.";
        public static readonly string Email_Not_Found = "email not found in our CRx system";
        public static readonly string User_Deleted_Successfully = "User Deleted Successfully";
        public static readonly string User_NotDeleted = "You cannot delete this user. Please contact CRx team";

        #region Pdf Related Message 

        public static readonly string PrintGeneratedByTitle = "Generated by:";
        public static readonly string PrintGeneratedFromText = "Generated from CRx";
        public static readonly string PrintGeneratedDateTitle = "";
        public static readonly string PrintGeneratedFromTitle = "";
        #endregion

    }

    public class APIStatus
    {
        public static readonly int Success = 200;
        public static readonly int Error = 201;
    }

    public class SessionKey
    {
        public static readonly string User = "User";
        public static readonly string UserOrganizations = "UserOrganizations";
        public static readonly string LogOnToken = "LogOnToken";
        public static readonly string OrgName = "orgName";
        public static readonly string Organization = "Organization";
        public static readonly string ClientNo = "clientno";
        public static readonly string UserOrgName = "UserOrgName";
        public static readonly string ErrorException = "ErrorException";
        public static readonly string TimeZoneOffSet = "timezoneoffset";
        public static readonly string ClientDBName = "dbName";
        public static readonly string IsSwitchUser = "IsSwitchUser";
        public static readonly string Exception = "exception";
        public static readonly string HelpTexts = "HelpTexts";
        public static readonly string ReturnUrl = "returnurl";
        public static readonly string OrganizationId = "OrganizationId";
        public static readonly string IsGuestUser = "IsGuestUser";
        public static readonly string TaggedId = "taggedid";
        public static readonly string ActIdFrmDshb = "actIdFrmDshb";
        public static readonly string IsExistTagDeficiency = "IsExistTagDeficiency";
        public static readonly string FilterBy = "FilterBy";
        public static readonly string combinevaluearr = "combinevaluearr";
        public static readonly string currentorg = "currentorg";
    }

    public class APIkeys
    {
        public static readonly string LogOnToken = "logintoken";
        public static readonly string DBName = "dbname";
        public static readonly string ClientNo = "clientno";
    }

    public class ServiceDeskConstantFields
    {
        public class HCF_Altantic
        {
            public static readonly string TemplateName = "Plant Operations";
            public static readonly string Mode = "CRx";
            public static readonly string Group = "Plant Operations";
            public static readonly string CategoryId = "3505700000050213"; // for plant ops
            public static readonly string CategoryName = "Plant Operations";
            public static readonly string AGHServiceDeskBaseURL = "https://support.atlanticgeneral.org/app/itdesk/api/v3";
            public static readonly string AGHServiceDeskAuthToken = "45d15b0ec90cb66222e735083ce6455d";
        }
    }

    public static class DymoPrinterConstants
    {
        public const string PRINTER_NAME = "DYMO LabelWriter 450";
        public const string FORMAT_PATH = "ReturnAddressLabel.label";
    }

    public static class Constants
    {
        public const string MessageViewBagName = "Message";
        public const string MessageViewBagError = "MessageViewBagError";
        public const string MessageViewBagSuccess = "MessageViewBagSuccess";
        public const string MessageViewBagWarning = "MessageViewBagWarning";

        public const string MessageError = "MessageError";
        public const string MessageSuccess = "MessageSuccess";
    }
}
