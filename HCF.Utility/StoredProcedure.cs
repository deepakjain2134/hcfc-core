namespace HCF.Utility
{
    public static class StoredProcedures
    {
        public const string Get_lockingUsers = "Get_lockingUsers";

        public const string Get_ArchivedEPsInspection = "Get_ArchivedEPsInspection";
        public const string Insert_VendorInvitation = "Insert_VendorInvitation";
        public const string Get_VendorOrgInvitation = "Get_VendorOrgInvitation";
        public const string Get_EpInspection = "Get_EpInspection";
        public const string Get_VendorByPrefix = "Get_VendorByPrefix";
        public const string Check_UserVendorExist = "Check_UserVendorExist";
        public const string Get_IsVendorExist = "Get_IsVendorExist";
        public const string Get_VendorbyOrgid = "Get_VendorbyOrgid";
        public const string Get_Alltypes_Floorassets = "Get_Alltypes_Floorassets";
        public const string Get_Tracking_Asset_Creation = "Get_Tracking_Asset_Creation";
        public const string Get_TransactionalRecordsByActivityId = "Get_TransactionalRecordsByActivityId";
        public const string Get_FloorAssetByTmsAssetId = "Get_FloorAssetByTmsAssetId";
        public const string Get_RecentFiles = "Get_RecentFiles";
        public const string Get_FindUploadedFiles = "Get_FindUploadedFiles";
        public const string Get_UserRecordsCount = "Get_UserRecordsCount";
        public const string Get_AssetsDashBoard = "Get_AssetsDashBoard";
        public const string Get_RoundWODetails = "Get_RoundWODetails";
        public const string Save_RoundInspection = "Save_RoundInspection";
        public const string Get_RoundDetails = "Get_RoundDetails";
        public const string Get_RoundUserGroup = "Get_RoundUserGroup";
        public const string Get_RoundByDate = "Get_RoundByDate";
        public const string Get_RoundByDate_App = "Get_RoundByDate_App";
        public const string Get_RoundGroupList = "Get_RoundGroupList";

        public const string Get_RoundUserCatGroup = "Get_RoundUserCatGroup";
        public const string Insert_RoundUserGroup = "Insert_RoundUserGroup";
        public const string Update_RoundLocations = "Update_RoundLocations";
        public const string Update_RoundInspectors = "Update_RoundInspectors";
        public const string Get_UserOrganizations = "Get_UserOrganizations";
        public const string Insert_EpAsset = "Insert_EpAsset";
        public const string Get_FloorAssetsByAsset = "Get_FloorAssetsByAsset";
        public const string Get_AssetsBuilding = "Get_AssetsBuilding";
        public const string Get_InspectionFiles = "Get_InspectionFiles";
        public const string Get_LastInspections = "Get_LastInspections";
        public const string Get_Exercises = "Get_Exercises";
        public const string Get_Permissions = "Get_Permissions";
        public const string Get_UserGroupPermissions = "Get_UserGroupPermissions";
        public const string Get_QuarterMaster = "Get_QuarterMaster";
        public const string Get_QuarterMasterCount = "Get_QuarterMasterCount";
        public const string Get_GoalMaster = "Get_GoalMaster";
        public const string Get_AffectedEPs = "Get_AffectedEPs";
        public const string Get_EPAssets = "Get_EPAssets";
        public const string Get_EPAssetTypes = "Get_EPAssetTypes";
        public const string Get_EPAssignee = "Get_EPAssignee";
        public const string Get_Eps = "Get_Eps";
        public const string Get_AssetsEPs = "Get_AssetsEPs";
        public const string Update_EpAssets = "Update_EpAssets";
        public const string Update_EPScheduleDate = "Update_EPScheduleDate";
        public const string Get_Eps_for_Binders = "Get_Eps_for_Binders";
        public const string Get_NonAssetsEps = "Get_NonAssetsEps";
        public const string Get_MainGoal = "Get_MainGoal";
        public const string Get_GetMainGoalById = "Get_GetMainGoalById";
        public const string Get_Steps = "Get_Steps";
        public const string Get_FrequencyMaster = "Get_FrequencyMaster";
        public const string Get_EPTransInfo = "Get_EPTransInfo";
        public const string Get_EPFrequency = "Get_EPFrequency";
        public const string Get_DeficientEPs = "Get_DeficientEPs";
        public const string Get_UserDeficientEPs = "Get_UserDeficientEPs";
        public const string Get_RoundsCategories = "Get_RoundsCategories";
        public const string GetRoundCategory = "GetRoundCategory";
        public const string Get_UserFloorRoundCategory = "Get_UserFloorRoundCategory";
        // public const string GetRoundCategory_Data = "GetRoundCategory_Data";
        //   public const string Get_RoundCategorydropdown = "Get_RoundCategorydd";
        public const string AddQuestionToCustomRound = "AddQuestionToCustomRound";
        public const string Get_TRounds = "Get_TRounds";
        public const string Get_RoundsQuestionnaires = "Get_RoundsQuestionnaires";
        public const string Get_TRoundsQuestionnaires = "Get_TRoundsQuestionnaires";
        public const string Get_FloorRoundsQuestionnaires = "Get_FloorRoundsQuestionnaires";
        public const string Get_TRoundsQuestionnairesByFloorId = "Get_TRoundsQuestionnairesByFloorId";
        public const string Get_RoundMatrixData = "Get_RoundMatrixData";
        public const string Get_RoundFiles = "Get_RoundFiles";
        public const string Get_GoalTransactionalRecords = "Get_GoalTransactionalRecords";
        public const string Get_StepsTransactionalRecords = "Get_StepsTransactionalRecords";
        public const string Get_EPTranHistory = "Get_EPTranHistory";
        public const string Get_TfloorAssetsByEP = "Get_TfloorAssetsByEP";
        public const string Get_Inspection_ActivityId = "Get_Inspection_ActivityId";
        public const string Get_UserDashboardData = "Get_UserDashboardData";
        public const string Get_Inspections = "Get_Inspections";
        public const string Get_EPInspectionDoc = "Get_EPInspectionDoc";
        public const string Get_TranSteps = "Get_TranSteps";
        public const string Get_RoundVolunteerCatGroup = "Get_RoundVolunteerCatGroup";
        //public const string Get_InspectionCount = "Get_InspectionCount";
        //public const string Get_Inspection = "Get_Inspection";
        //public const string Get_AssetInspectionActivity = "Get_AssetInspectionActivity";
        //public const string Get_AssetByType = "Get_AssetsByType";
        //public const string Get_FloorAssetsByFloor = "Get_FloorAssetsByFloor";
        //public const string Get_UsersByEP = "Get_UsersByEP";
        //public const string Get_AssetsFloor = "Get_AssetsFloor";
        //public const string Update_FrequencyMaster = "Update_FrequencyMaster";
        //public const string Insert_Manufactures = "Insert_Manufactures";
        //public const string ValidateUserProfile = "ValidateUserProfile";
        //public const string Update_FloorAssetLocation = "Update_FloorAssetLocation";
        //public const string Insert_subCategory = "Insert_subCategory";
        //public const string Get_NewsOnMarquee = "Get_NewsOnMarquee";
        //public const string Get_AssetsWithoutFloor = "Get_AssetsWithoutFloor";
        //public const string Insert_RoundInspections = "Insert_RoundInspections";
        //public const string Get_Vendor = "Get_Vendor";


        public const string Get_TInspectionActivity = "Get_TInspectionActivity";
        public const string Get_TInspectionActivityByEPAssets = "Get_TInspectionActivityByEPAssets";

        public const string Get_Refreshtoken = "Get_Refreshtoken";
        public const string Get_Users = "Get_Users";
        public const string Get_UsersList = "Get_UsersList";
        public const string Get_UsersProfileDetails = "Get_UsersProfileDetails";
        public const string Get_AllClientUsers = "Get_AllClientUsers";
        //public const string Get_UserRole = "Get_UserRole";
        public const string Get_DashBoard = "Get_DashBoard";
        public const string Get_UserMenu = "Get_UserMenu";
        public const string Get_Vendors = "Get_Vendors";
        // public const string Get_VendorsBuildings = "Get_VendorsBuildings"; need to remove
        public const string Get_UserGroups = "Get_UserGroups";
        public const string Get_UserGroupeAccess = "Get_UserGroupeAccess";
        public const string Get_UserAdditionalRoles = "Get_UserAdditionalRoles";
        public const string Get_AssetsType = "Get_AssetsType";
        public const string Get_OfflineAssets = "Get_OfflineAssets";
        public const string Get_AssetsTypeMaster = "Get_AssetsTypeMaster";
        public const string Get_Assets = "Get_Assets";
        public const string Get_MasterAssets = "Get_MasterAssets";

        public const string Get_Category = "Get_Category";
        public const string Get_FloorAssetsForInspections = "Get_FloorAssetsForInspections";
        public const string Get_AssetsWorkOrderStatus = "Get_AssetsWorkOrderStatus";
        public const string Get_AssetsMapping = "Get_AssetsMapping";
        public const string Get_MainGoalByActivity = "Get_MainGoalByActivity";
        public const string Get_TFloorAssets = "Get_TFloorAssets";
        public const string GetT_FloorAssetByAssets = "GetT_FloorAssetByAssets";
        public const string Get_AssetsBarCode = "Get_AssetsBarCode";
        public const string Get_TFloorAssetsById = "Get_TFloorAssetsById";

        public const string Get_Documents = "Get_Documents";
        public const string Get_DocuemntSpaceStatus = "Get_AHJDocuemntSpaceStatus";
        public const string Get_DocumentTypes = "Get_DocumentTypes";
        public const string Get_EpDocumentTypes = "Get_EpDocumentTypes";
        public const string Get_DeficiencyDoc = "Get_DeficiencyDoc";
        public const string Get_News = "Get_News";
        public const string Get_Organization = "Get_Organization";
        public const string Get_Floors = "Get_Floors";
        public const string Get_UserSiteMappings = "Get_UserSiteMappings";
        public const string Get_Wing = "Get_Wing";
        public const string Get_MasterData = "Get_MasterData";
        public const string Get_Building = "Get_Building";
        public const string Get_AllBuilding = "Get_AllBuilding";
        public const string Get_BuildingDetails = "Get_BuildingDetails";
        public const string Get_FloorShapes = "Get_FloorShapes";
        public const string Get_DocumentReport = "Get_DocumentReport";
        public const string Get_RepositoryDashboard = "Get_RepositoryDashboard";
        public const string Get_TInspectionEPDocs = "Get_TInspectionEPDocs";
        public const string Get_LoginToken = "Get_LoginToken";
        public const string Get_Roles = "Get_Roles";
        public const string Get_FloorByAssets = "Get_FloorByAssets";

        /// public const string Get_InspectionStepsByWorkOrder = "Get_InspectionStepsByWorkOrder";
        // public const string Get_EpTransCount = "Get_EpTransCount";
        public const string Get_RoundInspectionWO = "Get_RoundInspectionWO";
        // public const string Get_EpResources = "Get_EpResources"; need to remove
        public const string Get_Department = "Get_Department";
        public const string Get_TMSStatus = "Get_TMSStatus";
        public const string Get_Sites = "Get_Sites";
        public const string Get_Priority = "Get_Priority";
        public const string Get_WoMasterData = "Get_WoMasterData";
        public const string Get_Workorders = "Get_Workorders";
        public const string Get_RoundWorkOrders = "Get_RoundWorkOrders";
        public const string Get_SubStatus = "Get_SubStatus";
        // public const string Get_TriggerEPs = "Get_TriggerEPs";
        public const string Get_InspctionCount = "Get_InspctionCount";
        public const string Get_CompleteTRounds = "Get_CompleteTRounds";
        //public const string Get_AssetsByFloor = "Get_AssetsByFloor";
        public const string Get_Locations = "Get_Locations";
        public const string Get_Binders = "Get_Binders";
        public const string Get_EpBinder = "Get_EpBinder";
        public const string Get_ConsolidatedRoundsReport = "Get_ConsolidatedRoundsReport";
        public const string Get_FloorPlans = "Get_FloorPlans";
        public const string Get_DrawingViewer = "Get_DrawingViewer";
        public const string Get_SaltFromEmailAddress = "Get_SaltFromEmailAddress";
        //public const string Get_AssetStatus = "Get_AssetStatus";

        public const string Get_GetStepWorkOrder = "Get_GetStepWorkOrder";
        public const string Get_TInspectionDetail = "Get_TInspectionDetail";
        public const string Get_EPsFrequency = "Get_EPsFrequency";
        //public const string Get_EpByFrequency = "Get_EpByFrequency";
        public const string Get_TinspectionStepsByActivityId = "Get_TinspectionStepsByActivityId";
        public const string Get_InspectionSteps = "Get_InspectionSteps";
        public const string Get_ChildOrganization = "Get_ChildOrganization";
        public const string Get_MasterOrganization = "Get_MasterOrganization";
        public const string Update_OrganizationSettings = "Update_OrganizationSettings";
        public const string GET_RiskCount = "GET_RiskCount";
        public const string Get_InspectionDetail = "Get_InspectionDetail";
        public const string Get_EPDetails_Paging_Proc = "Get_EPDetails_Paging_Proc";
        public const string Get_MasterEPsData = "Get_MasterEPsData";
        public const string Get_Workorders_Paging_Proc = "Get_Workorders_Paging_Proc";
        public const string Get_BinderDocs = "Get_BinderDocs";
        public const string Get_Skills = "Get_Skills";
        public const string Get_TinspectionSteps = "Get_TinspectionSteps";
        public const string Get_FloorTypes = "Get_FloorTypes";
        public const string Get_PopEmails = "Get_PopEmails";
        public const string Get_TInsReportDetails = "Get_TInsReportDetails";
        public const string Get_InspectionReports = "Get_InspectionReports";
        public const string Get_Digitalsignature = "Get_Digitalsignature";
        public const string Get_TInsStepsTask = "Get_TInsStepsTask";
        public const string Get_AssetsByFloorPlan = "Get_AssetsByFloorPlan";
        public const string Get_GetworkOrderSteps_ActivityId = "Get_GetworkOrderSteps_ActivityId";
        public const string GetBindersEPs = "GetBindersEPs";
        public const string Get_AssetsByType = "Get_AssetsByType";
        public const string Get_FloorAssetsByAssetTye = "Get_FloorAssetsByAssetTye";
        public const string Get_AssetsMainGoals = "Get_AssetsMainGoals";
        public const string Get_RequiredPermission = "Get_RequiredPermission";
        public const string Save_UserViewHistory = "Insert_UserViewHistory";
        public const string Get_IssuesFloorAssets = "Get_IssuesFloorAssets";
        public const string Get_AssetsSubCategory = "Get_AssetsSubCategory";
        public const string Get_EpDocs = "Get_EpDocs";
        public const string Get_InspectionDetailReports = "Get_InspectionDetailReports";

        public const string GetEmailAddressFromRecoveryCode = "GetEmailAddressFromRecoveryCode";
        public const string Get_Scheduler = "Get_Scheduler";
        public const string Get_FiredrillQuestionnaries = "Get_FiredrillQuestionnaries";
        public const string GetCommonFireDrillCategory = "GetCommonFireDrillCategory";
        public const string Get_TinspectionStepsByFrequency = "Get_TinspectionStepsByFrequency";
        //public const string Get_FloorAssetsByEP = "Get_FloorAssetsByEP";
        public const string Get_TIlsmDetails = "Get_TIlsmDetails";
        public const string Get_CurrentTInspectionActivity = "Get_CurrentTInspectionActivity";
        public const string Get_CompleteTInspectionActivity = "Get_CompleteTInspectionActivity";
        public const string Get_InspectionsforworkOrderbyActivityId = "Get_InspectionsforworkOrderbyActivityId";
        public const string Get_InspectionGroup = "Get_InspectionGroup";

        // public const string Get_NotificationTypes = "Get_NotificationTypes";
        public const string Get_DocumentTypesMaster = "Get_DocumentTypesMaster";
        public const string Get_FloorAssets = "Get_AllFloorAssets";
        public const string Get_AssetsByPrefix = "Get_AssetsByPrefix";
        public const string Get_NotificationEmails = "Get_NotificationEmails";
        public const string Get_FilterNotificationEmails = "Get_FilterNotificationEmails";
        //public const string Get_AssestsCSVMatch = "Get_AssestsCSVMatch";
        //public const string Get_EPsUserPagging = "Get_EPsUserPagging";
        public const string Get_FireSystemType = "Get_FireSystemType";
        //public const string Insert_NewAssestRecordFromCSV = "Insert_NewAssestRecordFromCSV";
        //public const string Update_ExistedAssestRecordFromCSV = "Update_ExistedAssestRecordFromCSV";

        public const string Insert_Exercises = "Insert_Exercises";
        public const string Insert_DocumentTypeMaster = "Insert_DocumentTypeMaster";
        public const string Update_DocumentTypeMaster = "Update_DocumentTypeMaster";
        public const string Delete_DocumentTypeMaster = "Delete_DocumentTypeMaster";
        public const string Insert_PasswordResetQueue = "Insert_PasswordResetQueue";
        public const string Insert_TExercise = "Insert_TExercise";
        public const string Insert_QuarterMaster = "Insert_QuarterMaster";
        public const string Insert_GoalMaster = "Insert_GoalMaster";
        public const string Insert_EPDetails = "Insert_EPDetails";
        public const string Insert_MainGoal = "Insert_MainGoal";
        public const string Insert_Step = "Insert_Step";
        public const string Insert_InspectionRound = "Insert_InspectionRound";
        public const string SaveRoundFloorInspection = "SaveRoundFloorInspection";

        public const string Insert_InspectionRoundItems = "Insert_InspectionRoundItems";
        public const string SetArchiveRound = "SetArchiveRound";
        public const string Insert_Inspection = "Insert_Inspection";
        public const string Insert_PrevInspection = "Insert_PrevInspection";
        public const string Insert_InspectionActivity = "Insert_InspectionActivity";
        public const string Insert_TComment = "Insert_TComment";
        public const string Insert_InspectionDetail = "Insert_InspectionDetail";
        public const string Insert_InspectionFiles = "Insert_InspectionFiles";
        public const string Insert_InspectionSteps = "Insert_InspectionSteps";
        public const string Insert_InspectionDocs = "Insert_InspectionDocs";
        public const string Insert_Certificates = "Insert_Certificates";

        public const string Insert_Vendors = "Insert_Vendors";
        //public const string Insert_Assets = "Insert_Assets";
        public const string Insert_FloorAssets = "Insert_FloorAssets";
        public const string Insert_AssetsMapping = "Insert_AssetsMapping";
        public const string Insert_Wing = "Insert_Wing";
        public const string Insert_DocumentType = "Insert_DocumentType";
        public const string Insert_FloorAssetsInspection = "Insert_FloorAssetsInspection";
        public const string Update_TDeviceTesting = "Update_TDeviceTesting";
        public const string Insert_Attachments = "InsertAttachments";
        public const string Insert_Mail = "Insert_Mail";
        public const string Insert_Building = "Insert_Building";
        public const string Insert_BBI = "Insert_BBI";
        public const string Delete_BBI = "Delete_BBI";
        public const string Insert_News = "Insert_News";
        public const string Insert_FloorShapes = "Insert_FloorShapes";
        public const string Insert_Floor = "Insert_Floor";
        public const string Insert_DrawingViewer = "Insert_DrawingViewer";
        public const string Update_PermitDrawingViewer = "Update_PermitDrawingViewer";
        public const string Insert_User = "Insert_User";
        public const string Insert_ErrorLog = "Insert_ErrorLog";
        public const string insert_userloginforguest = "insert_userloginforguest";
        // public const string Insert_Resource = "Insert_Resource";
        public const string Insert_Binders = "Insert_Binders";
        public const string Insert_WorkOrder = "Insert_WorkOrder";
        public const string Insert_RoundsQuestionnaires = "Insert_RoundsQuestionnaires";
        public const string Insert_Priority = "Insert_Priority";
        public const string Insert_Sites = "Insert_Sites";
        public const string Insert_Department = "Insert_Department";
        public const string Insert_TMSStatus = "Insert_TMSStatus";
        public const string Insert_subStatus = "Insert_subStatus";
        public const string Get_LinkWorkorder = "Get_LinkWorkorder";

        public const string Update_RoundInspections = "Update_RoundInspections";
        public const string Insert_WorkOrderFiles = "Insert_WorkOrderFiles";

        public const string Insert_Category = "Insert_Category";
        public const string Insert_Skills = "Insert_Skills";
        public const string Insert_Type = "Insert_Type";
        public const string Insert_WorkOrderFailSteps = "Insert_WorkOrderFailSteps";
        public const string Insert_UsermenuMapping = "Insert_UsermenuMapping";
        public const string Insert_FireDrillSetting = "Insert_FireDrillSetting";
        public const string Insert_PermitSettings = "Insert_PermitSettings";


        public const string Insert_UserRole = "Insert_UserRole";
        public const string Insert_RoundGroupUserDetails = "Insert_RoundGroupUserDetails";
        public const string Insert_TRoundGroupUser = "Insert_TRoundGroupUser";
        public const string Insert_GroupUserType = "Insert_GroupUserType";
        public const string Get_TRoundGroup = "Get_TRoundGroup";
        public const string Insert_Shift = "Insert_Shift";
        // public const string Get_RoundUserGroup = "Get_RoundUserGroup";


        public const string Insert_TFloorAssetLocation = "Insert_TFloorAssetLocation";
        public const string Insert_TInsReportDetails = "Insert_TInsReportDetails";
        public const string Insert_InspectionReport = "Insert_InspectionReport";
        public const string Insert_Digitalsignature = "Insert_Digitalsignature";
        public const string Update_Digitalsignature = "Update_Digitalsignature";
        public const string Insert_TInsStepsTask = "Insert_TInsStepsTask";
        public const string Save_WorkOrderResource = "Save_WorkOrderResource";
        public const string Save_WorkOrderAssets = "Save_WorkOrderAssets";
        public const string Insert_AssetInspectionDate = "Insert_AssetInspectionDate";
        public const string Insert_TInspectionDueDate = "Insert_TInspectionDueDate";
        public const string Insert_InspectionGroup = "Insert_InspectionGroup";
        public const string Insert_EPFrequency = "Insert_EPFrequency";
        public const string Insert_EPDocument = "Insert_EPDocument";
        public const string Insert_Scheduler = "Insert_Scheduler";
        public const string Insert_Tilsm = "Insert_Tilsm";
        public const string Insert_FireDrillQuestionnaires = "Insert_FireDrillQuestionnaires";
        public const string Insert_TExerciseQuestionnaires = "Insert_TExerciseQuestionnaires";
        public const string Insert_StepWithIlLsmStep = "Insert_StepWithIlLsmStep";
        public const string Insert_FireSystemType = "Insert_FireSystemType";
        public const string Update_News = "Update_News";
        //public const string Update_DocumentTypeMaster = "Update_DocumentTypeMaster";
        public const string Update_WorkOrder = "Update_WorkOrder";
        public const string Update_Attachment = "Update_Attachment";
        public const string Update_Attachment_IsUsed_Status = "Update_Attachment_IsUsed_Status";
        public const string Update_Organization = "Update_Organization";
        public const string Update_Building = "Update_Building";
        public const string Update_Exercises = "Update_Exercises";
        public const string Update_RoleInGroup = "Update_RoleInGroup";
        public const string Update_FloorAssets = "Update_FloorAssets";
        public const string Update_QuarterMaster = "Update_QuarterMaster";
        public const string Update_Standard = "Update_Standard";
        public const string Update_EPFrequency = "Update_EPFrequency";
        public const string Update_Vendor = "Update_Vendor";
        public const string Update_User = "Update_User";
        public const string Update_Password = "Update_Password";
        //public const string Update_VendorbuildingsId = "Update_VendorbuildingsId"; need to remove
        public const string UpdateTfloorassetstatus = "Update_Tfloorassetstatus";

        public const string Update_TFloorAssetLocation = "Update_TFloorAssetLocation";
        public const string Update_EPdocStatus = "Update_EPdocStatus";
        public const string Update_LogOutUser = "Update_LogOutUser";
        public const string Update_Assets = "Update_Assets";
        public const string Update_MainGoal = "Update_MainGoal";
        public const string Update_TFloorAssetsShape = "Update_TFloorAssetsShape";
        public const string Update_IlsmMatrix = "Update_IlsmMatrix";
        public const string Update_IlsmStep = "Update_IlsmStep";
        public const string Update_Binders = "Update_Binders";
        //public const string Update_FloorAssets_TMS = "Update_FloorAssets_TMS";
        public const string Update_UpdateFloor = "Update_UpdateFloor";
        public const string Update_InspectionSubstatus = "Update_InspectionSubstatus";
        public const string Update_InspectionSteps = "Update_InspectionSteps";
        public const string Update_FloorPlan = "Update_FloorPlan";
        public const string GetUploadedDrawings = "GetUploadedDrawings";

        public const string Update_Inspection = "Update_Inspection";
        public const string Update_FiredrillQuestionnaries = "Update_FiredrillQuestionnaries";
        public const string Update_InspectionGroup = "Update_InspectionGroup";
        public const string Get_ExerciseQuestionnaires = "Get_ExerciseQuestionnaires";
        public const string Delete_News = "Delete_News";
        //public const string Delete_DocumentTypeMaster = "Delete_DocumentTypeMaster";
        public const string Delete_FloorShapes = "Delete_FloorShapes";
        public const string Delete_Exercises = "Delete_Exercises";
        public const string Delete_Certificates = "Delete_Certificates";
        public const string Delete_Document = "Delete_Document";
        public const string AuthenticateUser = "AuthenticateUser";
        public const string AssetType_CRUD = "AssetType_CRUD";


        // public const string Check_IsExistingUser = "Check_IsExistingUser";
        //public const string Get_UserPermissions = "Get_UserPermissions";
        public const string IsValidRecoveryCode = "IsValidRecoveryCode";
        public const string IsValidInvitationIdCode = "IsValidInvitationIdCode";
        public const string FlagRecoveryCode = "FlagRecoveryCode";
        public const string Add_NotificationEmails = "Insert_NotificationEmails";
        public const string Save_PopEmails = "Save_PopEmails";
        public const string Hcf_UserOrganization = "Hcf_UserOrganization";
        public const string Hcf_GetOrganization = "Get_Organizations";
        // public const string Main_MasterOrganization = "Main_MasterOrganization"; remove the proc
        public const string Get_View_FloorAssets = "Get_View_FloorAssets";
        public const string Get_InspectionDocuments = "Get_InspectionDocuments";
        public const string Update_InspectionIlsmFailSteps = "Update_InspectionIlsmFailSteps";
        public const string Update_CompleteIlsmIncident = "Update_CompleteIlsmIncident";
        public const string Get_DeficientFloorAssets = "Get_DeficientFloorAssets";
        public const string Get_IlsmInspection = "Get_IlsmInspection";
        public const string Get_EpDocStatus = "Get_EPDocStatus";
        public const string Get_InspectionActivityDetails = "Get_InspectionActivityDetails";
        public const string Get_ViewEpDetails = "Get_ViewEpDetails";
        public const string Get_ILSMByAssetFLoor = "Get_ILSMByAssetFLoor";
        public const string Insert_OutgoingEmails = "Insert_OutgoingEmails";
        public const string Insert_InboxEmails = "Insert_InboxEmails";
        public const string Get_PolicyBinders = "Get_PolicyBinders";
        public const string Get_PolicyBinderHistory = "Get_PolicyBinderHistory";

        public const string Get_ComplianceRpeort = "Get_ComplianceRpeort";
        public const string Get_BuildVersions = "Get_BuildVersions";
        public const string Get_NotificationMatrix = "Get_NotificationMatrix";
        public const string Update_NotificationMatrix = "Update_NotificationMatrix";
        public const string Get_NotificationCategory = "Get_NotificationCategory";
        public const string Get_Notifications = "Get_Notifications";
        public const string Get_NotificationMail = "Get_NotificationMail";

        public const string Get_InspectionActivitybyTinsStepsId = "Get_InspectionActivitybyTinsStepsId";
        public const string Get_DueWithInDaysEPs = "Get_DueWithInDaysEPs";
        public const string Get_AuditConfiguration = "Get_AuditConfiguration";
        public const string Insert_RoundInspection = "Insert_RoundInspection";
        public const string Get_FilterBuildings = "Get_FilterBuildings";
        public const string Get_RecentActivity = "Get_RecentActivity";
        public const string Get_UserOrgs = "Get_UserOrgs";
        //   public const string Get_User = "Get_User";
        public const string Insert_ScheduledLogs = "Insert_ScheduledLogs";
        public const string Get_ScheduledLogs = "Get_ScheduledLogs";
        public const string Update_ScheduledLogs = "Update_ScheduledLogs";
        public const string Insert_FireWatchLog = "Insert_FireWatchLog";
        public const string Get_FireWatchLog = "Get_FireWatchLog";
        public const string Get_FloorAssetsForReports = "Get_FloorAssetsForReports";
        public const string Close_Schedulelog = "Close_Schedulelog";
        public const string Get_QueueWorkOrders = "Get_QueueWorkOrders";
        public const string Update_WorkOrderId = "Update_WorkOrderId";
        public const string Insert_Stops = "Insert_Stops";
        public const string Update_Stops = "Update_Stops";
        public const string Get_StopMaster = "Get_StopMaster";
        public const string Insert_Route = "Insert_Route";
        public const string Insert_StopRouteMapping = "Insert_StopRouteMapping ";
        public const string Get_RouteMaster = "Get_RouteMaster";
        public const string Get_StopRouteMapping = "Get_StopRouteMapping";
        public const string Get_ExtinguisherAssets = "Get_ExtinguisherAssets";
        public const string Get_ExtinguisherAsset = "Get_ExtinguisherAsset";
        // public const string Get_Makes = "Get_Makes";
        public const string Remove_FloorAssetsFromLocation = "Remove_FloorAssetsFromLocation";
        public const string Get_AssetInspFrequency = "Get_AssetInspFrequency";


        public const string Get_InspResult = "Get_InspResult";
        public const string Get_InspStatus = "Get_InspStatus";
        public const string Insert_FloorAssetsToLocation = "Insert_FloorAssetsToLocation";
        public const string Get_InternalAssetsByType = "Get_InternalAssetsByType";
        public const string Get_FloorByAssetsWithStatus = "Get_FloorByAssetsWithStatus";
        public const string Get_FEFloorAssetInspActivity = "Get_FEFloorAssetInspActivity";
        public const string Get_DocumentStandardReports = "Get_DocumentStandardReports";
        public const string Update_InspectionDueDatePassed = "Update_InspectionDueDatePassed";
        public const string Get_ExtinguisherAssetsWithOutFloor = "Get_ExtinguisherAssetsWithOutFloor";
        public const string Get_InspectionsForCalendar = "Get_InspectionsForCalendar";
        public const string Get_InspectionsForDashBoardCalendar = "Get_InspectionsForDashBoardCalendar";
        public const string Get_Menus = "Get_Menus";
        public const string Update_Menus = "Update_Menus";
        public const string Update_MenuOrders = "Update_MenuOrders";
        public const string Get_ExtinguisherAssetsReports = "Get_ExtinguisherAssetsReports";
        public const string Get_EPlists = "Get_EPlists";
        public const string Insert_EPUsers = "Insert_EPUsers";
        public const string Insert_EPAssignees = "Insert_EPAssignees";
        public const string Save_EPUsers = "Save_EPUsers";
        public const string Insert_UserEps = "Insert_UserEps";

        public const string Get_AllInspectionActivity = "Get_AllInspectionActivity";
        public const string Get_RouteByAsset = "Get_RouteByAsset";
        public const string Get_HCFMenus = "Get_HCFMenus";

        public const string Get_AllOrganizationDashInfo = "Get_AllOrganizationDashInfo";
        public const string Get_AssetChart = "Get_AssetChart";
        public const string Get_AllOrganizationChart = "Get_AllOrganizationChart";
        public const string Get_EpStatusChart = "Get_EpStatusChart";
        public const string Get_AssetInventoryReport = "Get_AssetInventoryReport";
        public const string Insert_Manufacture = "Insert_Manufacture";
        public const string Get_Manufacture = "Get_Manufacture";
        public const string Get_ExtinguisherRouteReports = "Get_ExtinguisherRouteReports";
        public const string Get_RouteByFloor = "Get_RouteByFloor";
        public const string Update_UserLoginIP = "Update_UserLoginIP";
        public const string Update_UserDrawing = "Update_UserDrawing";
        public const string Get_UserLogins = "Get_UserLogins";
        //public const string Get_UserGroup = "Get_UserGroup";
        public const string Update_UserGroup = "Update_UserGroup";
        // public const string Delete_UserGroup = "Delete_UserGroup"; need to remove
        public const string Update_First_InspectionDate = "Update_First_InspectionDate";
        public const string Get_ConstructionType = "Get_ConstructionType";

        public const string Insert_ClientUser = "Insert_ClientUser";
        public const string Insert_ConstructionType = "Insert_ConstructionType";
        public const string Insert_ConstructionActivity = "Insert_ConstructionActivity";
        public const string Get_ConstructionRisk = "Get_ConstructionRisk";
        public const string Insert_ConstructionRisk = "Insert_ConstructionRisk";
        public const string Get_ConstructionClass = "Get_ConstructionClass";
        public const string Insert_ConstructionClass = "Insert_ConstructionClass";
        public const string Insert_ConstructionClassActivity = "Insert_ConstructionClassActivity";
        public const string Get_ICRASteps = "Get_ICRASteps";
        public const string Insert_ICRASteps = "Insert_ICRASteps";
        public const string Insert_ICRARiskArea = "Insert_ICRARiskArea";
        public const string Update_ICRARiskArea = "Update_ICRARiskArea";
        public const string Get_ICRARiskArea = "Get_ICRARiskArea";
        public const string Insert_AddRiskAreaToArea = "Insert_AddRiskAreaToArea";
        public const string Get_ConstRiskDeptMap = "Get_ConstRiskDeptMap";

        public const string Delete_ConstRiskDeptMap = "Delete_ConstRiskDeptMap";
        public const string Insert_FrequencyMaster = "Insert_FrequencyMaster";
        public const string Get_InspectionIcraSteps = "Get_InspectionIcraSteps";
        public const string GetPermitsCount = "GetPermitsCount";

        public const string Get_Icras = "Get_Icras";
        public const string Insert_InspectionIcra = "Insert_InspectionIcra";
        public const string Insert_TIcraSteps = "Insert_TIcraSteps";
        public const string Insert_TIcraFiles = "Insert_TIcraFiles";
        public const string Insert_ICRAMatixPrecautions = "Insert_ICRAMatixPrecautions";
        public const string Get_ICRAMatixPrecautions = "Get_ICRAMatixPrecautions";
        public const string Insert_AreaSurrounding = "Insert_AreaSurrounding";
        public const string Get_IcraHistory = "Get_IcraHistory";
        public const string Insert_SearchFilter = "Insert_SearchFilter";
        public const string Get_SearchFilter = "Get_SearchFilter";
        public const string Get_SearchResult = "Get_SearchResult";
        public const string Insert_ICRAReportCheckPoint = "Insert_ICRAReportCheckPoint";
        public const string Get_ICRAObsReportCheckPoints = "Get_ICRAObsReportCheckPoints";
        public const string Insert_TICRAReportCheckPoint = "Insert_TICRAReportCheckPoint";
        public const string Save_ObservationReport = "Save_ObservationReport";
        public const string Get_TICRAReport = "Get_TICRAReport";
        public const string Get_TICRAReports = "Get_TICRAReports";
        public const string Delete_ICRAFiles = "Delete_ICRAFiles";
        public const string Insert_TICRAReports = "Insert_TICRAReports";
        public const string ChangerecoverPassword = "ChangerecoverPassword";
        public const string Get_StepsComment = "Get_StepsComment";
        public const string Insert_ConstructionTilsm = "Insert_ConstructionTilsm";
        public const string Get_AssetSubCategorySize = "Get_AssetSubCategorySize";
        public const string Save_TilsmReport = "Save_TilsmReport";
        public const string Get_ILMSWorkOrderDescriptions = "Get_ILMSWorkOrderDescriptions";
        public const string Get_ActivityMainGoals = "Get_ActivityMainGoals";
        public const string Get_UserEPs = "Get_UserEPs";
        public const string Get_EPsUser = "Get_EPsUser";
        public const string GetEpAssignees = "Get_EpAssignees";
        public const string Get_EpAssignees_Paging_Proc = "Get_EpAssignees_Paging_Proc";
        public const string IsResourceAllowed = "IsResourceAllowed";
        public const string Get_WoCountByDbName = "Get_WoCountByDbName";
        public const string Insert_DocumentMaster = "Insert_DocumentMaster";
        public const string Get_DocumentMaster = "Get_DocumentMaster";
        public const string Update_Attachmentdoc = "Update_Attachmentdoc";
        public const string Get_StopNotExistInFE = "Get_StopNotExistInFE";
        public const string Insert_MappingIcraILsm = "Insert_MappingIcraILsm";
        public const string Get_ICRALogWithILSM = "Get_ICRALogWithILSM";
        public const string Get_Notificationtype = "Get_Notificationtype";
        public const string Insert_FirewatchNotification = "Insert_FirewatchNotification";
        public const string HCFGet_Organization = "HCFGet_Organization";
        public const string HCF_GetGroupUsers = "HCF_GetGroupUsers";
        public const string HCF_GetRGroup = "HCF_GetRGroup";
        public const string Get_RoundReport = "Get_RoundReport";
        public const string Get_AssetDeficiencyData = "Get_AssetDeficiencyData";
        public const string Update_AssetType = "Update_AssetType";
        public const string GetCommonRoundCategory = "GetCommonRoundCategory";

        public const string Get_UserRoundDetails = "Get_UserRoundDetails";
        public const string Get_RoundVolunteer_UserDetails = "Get_RoundVolunteer_UserDetails";
        public const string GetNotifyDetails_Rounds = "GetNotifyDetails_Rounds";
        #region  Schedule

        public const string Insert_Schedule = "Insert_Schedule";
        public const string Insert_ScheduleFrequency = "Insert_ScheduleFrequency";
        public const string Get_Schedules = "Get_Schedules";
        public const string Insert_AssetsSchedule = "Insert_AssetsSchedule";
        public const string Update_Schedule = "Update_Schedule";

        #endregion


        //public const string Get_ScheduledLogsForFirewatch = "Get_ScheduledLogsForFirewatch";
        //public const string Get_IlsmEpAssets = "Get_IlsmEpAssets";
        //public const string Get_EpAssetsByTriggerEP = "Get_EpAssetsByTriggerEP";
        //public const string Get_IlsmInspections = "Get_IlsmInspections";
        //public const string Get_TriggerEPsByInsId = "Get_TriggerEPsByInsId";
        //public const string Get_TIlsm = "Get_TIlsm";
        //public const string Insert_IlsmInspections = "Insert_IlsmInspections";
        //public const string Update_InspectionActivity = "Update_InspectionActivity";
        //public const string Update_IlsmIncident = "Update_IlsmIncident";
        //public const string Get_InspectionEPDoc = "Get_InspectionEPDoc";
        //public const string Get_EPByActivtyId = "Get_EPByActivtyId";
        //public const string Get_IncomTInspectionActivity = "Get_IncomTInspectionActivity";
        //public const string Get_EpByDoc = "Get_EpByDoc";
        //public const string Get_ILSMMeasures = "Get_ILSMMeasures";
        //public const string Get_EpMainGoal = "Get_EpMainGoal";
        //public const string Delete_EPBinder = "Delete_EPBinder"; need to remove
        //public const string Get_EPDetailsByUser = "Get_EPDetailsByUser";
        //public const string Get_UsersLOginStatus = "Get_UsersLOginStatus";
        //public const string Get_ConstQuestionnaires = "Get_ConstQuestionnaires";
        //public const string Get_Constructions = "Get_Constructions";
        //public const string Get_DefeciencyCount = "Get_DefeciencyCount";
        //public const string Get_IlsmMatrix = "Get_IlsmMatrix";
        //public const string Get_SOC = "Get_SOC";
        //public const string Get_FloorAssetById = "Get_FloorAssetById";
        //public const string Get_IlsmQuestions = "Get_IlsmQuestions";
        //public const string Insert_AssetGoalMapping = "Insert_AssetGoalMapping";
        //public const string Insert_ConstInspection = "Insert_ConstInspection";
        //public const string Insert_ConstInspectionItems = "Insert_ConstInspectionItems";
        //public const string Update_SOCMatrix = "Update_SOCMatrix";
        //public const string Update_EPBinder = "Update_EPBinder"; // need to remove
        public const string Get_StandardEps = "Get_StandardEps";
        //public const string Get_AssetsEP = "Get_AssetsEP";
        // public const string Insert_IlsmInspection = "Insert_IlsmInspection";
        // public const string Get_AssetUsers = "Get_AssetsUsers";
        //public const string Get_WorkorderByActivityId = "Get_WorkorderByActivityId";

        public const string Get_EPDetailsCurrentStaus = "Get_EPDetailsCurrentStaus";

        // Tips

        public const string Get_Tips = "Get_Tips";
        // public const string Insert_Tip = "Insert_Tip";
        public const string Insert_Update_Tip = "Insert_Update_Tip";
        public const string Get_All_Tips = "Get_All_Tips";
        public const string Update_ApprovalOfTips = "Update_ApprovalOfTips";

        public const string Delete_DocumentMaster = "Delete_DocumentMaster";
        public const string Update_EPApplicablestatus = "Update_EPApplicablestatus";
        public const string Get_NonAssetEPsSchedule = "Get_NonAssetEPsSchedule";
        public const string Get_EPScedules = "Get_EPScedules";

        // EPCOnfig
        public const string EpConfig_CRUD = "EpConfig_CRUD";
        public const string Delete_EpDocs = "Delete_EpDocs";

        // TIcraProject
        public const string Crud_TIcraProject = "Crud_TIcraProject";
        public const string Delete_ProjectFiles = "Delete_ProjectFiles";

        public const string Assign_EP_Resposibility = "Assign_EP_Resposibility";

        public const string GET_AssignedEPCount = "GET_AssignedEPCount";

        public const string Get_TDeviceTesting = "Get_TDeviceTesting";
        public const string Insert_TDeviceTesting = "Insert_TDeviceTesting";
        public const string Update_UserRole = "Update_UserRole";
        public const string Delete_UserRole = "Delete_UserRole";
        public const string Insert_UserSiteMapping = "Insert_UserSiteMapping";

        public const string Get_UserRoles = "Get_UserRoles";
        public const string Get_ProjectType = "Get_ProjectType";

        public const string Get_RoundInspectionByDate = "Get_RoundInspectionByDate";
        public const string Get_Certificates = "Get_Certificates";

        //copdetails
        public const string Get_CopDetails = "Get_CopDetails";
        public const string Insert_CmsEpMapping = "Insert_CmsEpMapping";
        public const string Delete_CmsEpMapping = "Delete_CmsEpMapping";

        public const string Get_BinderSearch = "Get_BinderSearch";
        public const string GET_EpRelationStatus = "GET_EpRelationStatus";
        public const string Get_RelationalTInspectionEPDocs = "Get_RelationalTInspectionEPDocs";
        public const string GET_EpSearchbyEpNumber = "GET_EpSearchbyEpNumber";


        public const string User_ReplaceVolunteer = "User_ReplaceVolunteer";
        public const string Get_RoundGroupUsers = "Get_RoundGroupUsers";
        public const string Update_roundVolunteer = "Update_roundVolunteer";
        public const string Update_ActivityIsMailNotified = "Update_ActivityIsMailNotified";
        #region Questionnaires

        public const string Get_QuestionnairesTypes = "Get_QuestionnairesTypes";
        public const string Insert_Questionnaire = "Insert_Questionnaire";
        public const string Insert_QuestionnaireSteps = "Insert_QuestionnaireSteps";
        public const string Insert_QuestionnaireStepsDetail = "Insert_QuestionnaireStepsDetail";
        public const string Get_Questionnaires = "Get_Questionnaires";
        public const string Get_QuestionnaireHeaderTypes = "Get_QuestionnaireHeaderTypes";
        public const string Get_PMDailyLogs = "Get_PMDailyLogs";
        public const string Insert_PMDailyLog = "Insert_PMDailyLog";
        public const string Insert_PMLogSteps = "Insert_PMLogSteps";
        public const string Get_SchedulesbyEP = "Get_SchedulesbyEP";

        public const string Update_QuestionnaireSequence = "Update_QuestionnaireSequence";
        public const string Update_QuestionnaireStepSequence = "Update_QuestionnaireStepSequence";
        public const string Save_AdditionalTilsmEP = "Save_AdditionalTilsmEP";


        #endregion

        #region Ceilingpermit
        public const string Get_Ceilingpermit = "Get_Ceilingpermit";
        public const string Get_PermitMappingForm = "Get_PermitMappingForm";

        public const string Insert_Ceilingpermit = "Insert_Ceilingpermit";
        public const string Insert_CeilingPermitDetail = "Insert_CeilingPermitDetail";
        public const string Delete_CeilingPermitFiles = "Delete_CeilingPermitFiles";
        public const string Delete_CeilingDrawings = "Delete_CeilingDrawings";
        public const string Delete_FMCDrawings = "Delete_FMCDrawings";
        public const string Delete_FSBPDrawings = "Delete_FSBPDrawings";
        public const string Delete_FSBPFiles = "Delete_FSBPFiles";
        public const string Delete_LSDFiles = "Delete_LSDFiles";

        public const string Delete_ICRADrawings = "Delete_ICRADrawings";
        public const string Delete_LSDDrawings = "Delete_LSDDrawings";
        public const string Delete_PCRADrawings = "Delete_PCRADrawings";

        public const string Insert_CeilingPermitDetails = "Insert_CeilingPermitDetails";
        public const string Update_Tilsm = "Update_Tilsm";

        public const string Get_FloorAssetsbyBarcode = "Get_FloorAssetsbyBarcode";
        public const string Get_FloorAssetsbySearch = "Get_FloorAssetsbySearch";

        public const string Get_AssetFrequency = "Get_AssetFrequency";

        public const string Update_ILSMStatus = "Update_ILSMStatus";

        public const string Update_ILSMlinkToWO = "Update_ILSMlinkToWO";

        public const string Get_Assigned_Ep_Count = "Get_Assigned_Ep_Count";
        //public const string Get_Assigned_Total_Ep_Count = "Get_Assigned_Total_Ep_Count";


        public const string Get_RoundCommonQuestionaries = "Get_RoundCommonQuestionaries";

        #region EPGroups

        public const string Get_EPGroups = "Get_EPGroups";
        public const string Get_EPGroupsDetail = "Get_EPGroupsDetail";
        public const string Get_VendorEPGroups = "Get_VendorEPGroups";
        public const string Insert_VendorEPGroups = "Insert_VendorEPGroups";
        public const string Get_RouteInspectionActivity = "Get_RouteInspectionActivity";
        public const string Get_EPsAssignList = "Get_EPsAssignList";
        public const string Get_EPGroupsNameList = "Get_EPGroupNameList";
        public const string Insert_EPGroupsName = "Insert_EPGroupsName";
        public const string Updade_EPsAssignList = "Updade_EPsAssignList";
        public const string Insert_AssignEPs = "Insert_AssignEPs";
        public const string Get_StopAssets = "Get_StopAssets";

        public const string Update_QuestionnaireSteps = "Update_QuestionnaireSteps";
        public const string Save_RoundInspStatus = "Save_RoundInspStatus";
        public const string Save_RoundStatus = "Save_RoundStatus";
        public const string Save_RoundStatusForfail = "Save_RoundStatusForfail";
        public const string Insert_EPDescriptions = "Insert_EPDescriptions";
        public const string Get_HospitalType = "Get_HospitalType";
        public const string Save_TRounWorkOrder = "Save_TRounWorkOrder";

        public const string Check_ExistingEmail = "Check_ExistingEmail";
        public const string Delete_TInspectionFiles = "Delete_TInspectionFiles";

        public const string Insert_FloorPlan = "Insert_FloorPlan";

        public const string Get_Icras_Reports = "Get_Icras_Reports";

        public const string Insert_RoundCategory = "Insert_RoundCategory";

        public const string Insert_FiredrillCategory = "Insert_FiredrillCategory";

        public const string GetFireDrillCategory = "GetFireDrillCategory";

        public const string Update_RoundSchdulesDates = "UpdateRoundSchdulesDatesOnRoundCateID";

        public const string Get_TICRAProjectReport = "Get_TICRAProjectReport";
        public const string Insert_TICRAProjectReports = "Insert_TICRAProjectReports";

        public const string Get_PermitNumber = "Get_PermitNumber";

        public const string Update_FloorPlanThumbImage = "Update_FloorPlanThumbImage";


        public const string Check_ExistingAssetNo = "Check_ExistingAssetNo";

        public const string Insert_TaggedMaster = "Insert_TaggedMaster";
        public const string Remove_TagID = "Remove_TagID";
        public const string Update_TaggedIdInDeficiencies = "Update_TaggedIdInDeficiencies";

        public const string Check_ExistingTagId = "Check_ExistingTagId";
        #region TMS Related Proc 

        public const string TMS_GetAllTMSRecords = "TMS_GetAllTMSRecords";
        public const string TMSAkron_InsertResults = "TMSAkron_InsertResults";
        public const string TMSAkron_InsertAssets = "TMSAkron_InsertAssets";
        public const string TMSAkron_GetAllTMSAsset = "TMSAkron_GetAllTMSAsset";
        public const string TMSAkron_InsertWorkORders = "TMSAkron_InsertWorkORders";
        public const string TMS_MC_InsertUsers = "TMS_MaintenanceConnection_InsertUsers";
        public const string TMS_MC_WorkOrderAssignment = "TMS_MC_WorkOrderAssignment";
        public const string TMS_MC_GetUsers = "TMS_MC_GetUsers";
        public const string TMS_MC_InsertWorkOrderTask = "TMS_MC_InsertWorkOrderTask";
        public const string TMSAkron_InsertLookupTable = "TMSAkron_InsertLookupTable";
        public const string TMSAkron_InsertLookupTableValues = "TMSAkron_InsertLookupTableValues";

        #endregion

        #endregion

        #region PCRA

        public const string Get_QuestionPCRA = "Get_QuestionPCRA";
        public const string Insert_QuestionPCRA = "Insert_QuestionPCRA";
        public const string Update_QuestionOptionPCRA = "Update_QuestionOptionPCRA";
        public const string Get_QuestionOptionPCRA = "Get_QuestionOptionPCRA";
        public const string Delete_QuestionOptionPCRA = "Delete_QuestionOptionPCRA";


        #region TQuestionPCRA

        public const string Get_TPCRAQuestion = "Get_TPCRAQuestion";
        public const string Get_ProjectDetails = "Get_ProjectDetails";
        public const string Insert_TPCRAQuestion = "Insert_TPCRAQuestion";
        public const string Insert_TPCRAQuestionDetails = "Insert_TPCRAQuestionDetails";
        public const string Check_TPCRAdddorEdit = "Check_TPCRAdddorEdit";
        public const string Delete_TIcraFiles = "Delete_TIcraFiles";
        public const string Insert_TDepartmentNearConstruction = "Insert_TDepartmentNearConstruction";
        #endregion

        #endregion

        #endregion

        #region StatesAndCities
        public const string Get_States = "Get_States";
        public const string Get_Cities = "Get_Cities";
        public const string Insert_AddState = "Insert_AddState";
        public const string Insert_AddCity = "Insert_AddCity";
        #endregion

        #region SiteType
        public const string Get_SiteType = "Get_SiteType";
        #endregion

        #region Permits
        public const string Get_All_THotWorkPermit = "Get_All_THotWorkPermit";
        public const string Get_THotWorkPermits = "Get_THotWorkPermits";
        public const string Insert_THotWorkPermits = "Insert_THotWorkPermits";
        public const string Update_THotWorkPermits = "Update_THotWorkPermits";
        public const string Delete_HWPDrawings = "Delete_HWPDrawings";

        public const string Insert_LifeSafetyDevicesForms = "Insert_LifeSafetyDevicesForms";
        public const string Insert_LifeSafetyDeviceList = "Insert_LifeSafetyDeviceList";
        public const string Get_LifeSafetyDevicesForms = "Get_LifeSafetyDevicesForms";
        public const string Insert_THotWorkItems = "Insert_THotWorkItems";


        public const string Get_GetMethodofProcedure = "Get_GetMethodofProcedure";
        public const string Get_MopMaster = "Get_MopMaster";
        public const string Delete_TMOPFiles = "Delete_TMOPFiles";
        public const string Delete_TMOPDrawing = "Delete_TMOPDrawing";

        public const string Get_FSBPermit = "Get_FSBPermit";
        public const string Insert_FSBPermit = "Insert_FSBPermit";
        public const string Insert_FSBPermitDetails = "Insert_FSBPermitDetails";

        public const string Insert_FSBPBuildingDetails = "Insert_FSBPBuildingDetails";
        public const string Insert_MethodofProcedure = "Insert_MethodofProcedure";
        public const string Insert_TMopAdditionalForms = "Insert_TMopAdditionalForms";
        public const string Insert_TProjectContactList = "Insert_TProjectContactList";
        public const string Insert_TSystemImpactArea = "Insert_TSystemImpactArea";
        public const string Insert_TFiles = "Insert_TFiles";
        //public const string Get_File = "Get_File";
        public const string Get_Files = "Get_Files";
        public const string Insert_TmopAdditionalFormsRedirect = "Insert_TmopAdditionalFormsRedirect";

        public const string Get_All_FacilitiesMaintenanceOccurrence = "Get_All_FacilitiesMaintenanceOccurrence";
        public const string Get_FacilitiesMaintenanceOccurrence = "Get_FacilitiesMaintenanceOccurrence";
        public const string Insert_TFacilitiesMaintenanceOccurrence = "Insert_TFacilitiesMaintenanceOccurrence";
        public const string Get_Shift = "Get_Shift";
        public const string Delete_Permit = "Delete_Permit";

        public const string Get_PaperPermit = "Get_PaperPermit";
        public const string Get_PaperPermitById = "Get_PaperPermitById";
        public const string Delete_TPaperPermitFiles = "Delete_PaperPermitFiles";
        public const string Insert_PaperPermit = "Insert_PaperPermit";
        public const string GetAllPermits = "GetAllPermits";
        public const string Insert_TPermitMapping = "Insert_TPermitMapping";
        #endregion

        #region BuildingFloorId
        public const string Get_BuildingFloorId = "Get_BuildingFloorId";
        #endregion

        #region import asset
        public const string Insert_ImportedFloorAssets = "Insert_ImportedFloorAssets";

        #endregion

        #region Vendor

        public const string Update_ContactDetails = "Update_ContactDetails";

        public const string Get_VendorDashboardData = "Get_VendorDashboardData";

        public const string Delete_FloorPlan = "Delete_FloorPlan";

        public const string Get_AssetsDetailsbyTMSAssetId = "Get_AssetsDetailsbyTMSAssetId";
        public const string Insert_TExerciseActions = "Insert_TExerciseActions";
        public const string Delete_TExerciseActions = "Delete_TExerciseActions";

        public const string Insert_RoundGroupLocations = "Insert_RoundGroupLocations";

        public const string Get_RoundGroupLocations = "Get_RoundGroupLocations";

        //public const string Get_UserGroupRounds = "Get_UserGroupRounds";

        public const string Get_RoundsUserGroup = "Get_RoundsUserGroup";

        public const string Get_RoundCountonCalender = "Get_RoundCountonCalender";
        public const string Get_BinderStandardEps = "Get_BinderStandardEps";
        public const string Get_BinderFolders = "Get_BinderFolders";

        public const string Insert_BinderFolders = "Insert_BinderFolders";
        public const string Delete_BinderFolder = "Delete_BinderFolder";
        public const string Get_AllAssetsCurrentStatus = "Get_AllAssetsCurrentStatus";
        public const string Get_RoundRecentChangesByDate = "Get_RoundRecentChangesByDate";
        #endregion

        #region VendorResource
        public const string Insert_VendorResource = "Insert_VendorResource";
        public const string Get_VendorResources = "Get_VendorResources";
        public const string Get_VendorResources_Dashboard = "Get_VendorResources_Dashboard";
        public const string Get_VendorsResource = "Get_VendorsResource";

        #endregion

        #region Menu Check
        public const string CheckICRAPCRAMenuExist = "CheckICRAPCRAMenuExist";
        public const string IsPermitActiveByUserID = "IsPermitActiveByUserID";

        #endregion

        #region  CMS Dashboard
        public const string Get_CMSDashboard = "Get_CMSDashboard";
        #endregion

        #region CRx Auto Report
        public const string Get_AutoReportdata = "Get_AutoReportdata";
        #endregion

        public const string Get_AHJDocumentNotificationdata = "Get_AHJDocumentNotificationdata";

        #region Module Master

        public const string Get_ModuleMaster = "Get_ModuleMaster";

        public const string Get_AssetCurrentInspection = "Get_AssetCurrentInspection";

        public const string Get_MainGoalsbyFloorAssetId = "Get_MainGoalsbyFloorAssetId";

        #endregion

        public const string Get_FileDetailsById = "Get_FileDetailsById";
        public const string Update_Step = "Update_Step";

        public const string GET_FloorAssetsWorkOrderCount = "GET_FloorAssetsWorkOrderCount";

        public const string Get_InspectionActivityFromToDate = "Get_InspectionActivityFromToDate";
        public const string Get_TaggedList = "Get_TaggedList";

        public const string Get_ComplianceAssetsTrackingReports = "Get_ComplianceAssetsTrackingReports";

        #region Upgrade EP
        public const string Upgrade_EPDetails = "Upgrade_EPDetails";
        #endregion

        #region PermitWorkFlow
        public const string Get_PermitWorkflowSettings = "Get_PermitWorkflowSettings";
        public const string Insert_PermitSettingsWorkFlow = "Insert_PermitSettingsWorkFlow";
        public const string Insert_TPermitDetails = "Insert_TPermitDetails";
        public const string Delete_PermitWorkFlow = "Delete_PermitWorkFlow";

        #endregion
        #region Asset Device Change
        public const string Insert_AssetDeviceChangeForms = "Insert_AssetDeviceChangeForms";
        public const string Insert_AssetList = "Insert_AssetList";
        public const string Get_AssetChangeDeviceForms = "Get_AssetChangeDeviceForms";
        public const string Get_AssetChangeDeviceFormsById = "Get_AssetChangeDeviceFormsById";

        public const string Delete_ADCDrawings = "Delete_ADCDrawings";
        public const string Delete_ADCFiles = "Delete_ADCFiles";
        public const string GetAssetDevicesList = "GetAssetDevicesList";
        public const string Insert_AssetDevices = "Insert_AssetDevices";

        #endregion
        #region Campus
        public const string Get_Campus = "Get_Campus";
        public const string Insert_InspectionCampus = "Insert_InspectionCampus";
        public const string Get_EpsCampusInspections = "Get_EpsCampusInspections";
        #endregion

        public const string Get_TinspectionCycle = "Get_TinspectionCycle";
        public const string ResetUserPassword = "ResetUserPassword";
        public const string Userlockout = "Userlockout";
        public const string Get_Usersforlockout = "Get_Usersforlockout";
        public const string IsValidNewPassword = "IsValidNewPassword";

        //public const string Get_User_salt = "Get_User_salt";

        public const string Get_ExercisesFiles = "Get_ExercisesFiles";
        public const string IsNewDevice = "IsNewDevice";
        public const string UpdateNewDevice = "UpdateNewDevice";
        public const string Get_LastPasswords = "Get_LastPasswords";

        public const string Crud_EPAssignees = "Crud_EPAssignees";

        public const string sp_UnlockUsers = "sp_UnlockUsers";
        public const string Get_AssetsEPOnRoute = "Get_AssetsEPOnRoute";
        public const string Get_UserOrgsByUserId = "Get_UserOrgsByUserId";
        public const string Delete_Digitalsignature = "Delete_Digitalsignature";

        public const string Get_TrainingSessionsDetails = "Get_TrainingSessionsDetails";
        #region NBI Temp Table 

        public const string TMSNBI_InsertAssetsTempTable = "TMSNBI_InsertAssetsTempTable";
        public const string TMSNBI_InsertPriorityTempTable = "TMSNBI_InsertPriorityTempTable";
        public const string TMSNBI_InsertAccountTempTable = "TMSNBI_InsertAccountTempTable";
        public const string TMSNBI_InsertSiteTempTable = "TMSNBI_InsertSiteTempTable";
        public const string TMSNBI_InsertTypeTempTable = "TMSNBI_InsertTypeTempTable";
        public const string TMSNBI_InsertStatusTempTable = "TMSNBI_InsertStatusTempTable";
        public const string TMSNBI_InsertBuildingTempTable = "TMSNBI_InsertBuildingTempTable";
        public const string TMSNBI_InsertSubStatusTempTable = "TMSNBI_InsertSubStatusTempTable";
        public const string TMSNBI_InsertLocationTempTable = "TMSNBI_InsertLocationTempTable";
        public const string TMSNBI_InsertRequesterTempTable = "TMSNBI_InsertRequesterTempTable";
        public const string TMSNBI_InsertResourceTempTable = "TMSNBI_InsertResourceTempTable";
        public const string TMSNBI_InsertWorkOrderTempTable = "TMSNBI_InsertWorkOrderTempTable";

        #endregion

        #region  Location Groups
        public const string Insert_LocationGroup = "Insert_LocationGroup";
        public const string Get_LocationGroup = "Get_LocationGroup";
        public const string Insert_GroupLocationsDetails = "Insert_GroupLocationsDetails";
        public const string Get_GroupLocationsDetails = "Get_GroupLocationsDetails";
        public const string Get_GroupBuildings = "Get_GroupBuildings";
        public const string Update_ILSMlinkToObservationReport = "Update_ILSMlinkToObservationReport";

        public const string Get_MasterUsers = "Get_MasterUsers";



        #endregion
        public const string Insert_Update_TrainingSessionMaster = "Insert_Update_TrainingSessionMaster";
        public const string Insert_Update_OrgTrainingSessions = "Insert_Update_OrgTrainingSessions";
        public const string Get_InspectionDetailbyEPDetailId = "Get_InspectionDetailbyEPDetailId";
        public const string Get_EPDetailsByInspectionID = "Get_EPDetailsByInspectionID";
        public const string UndoInspection = "UndoInspection";

        public const string GetUserLoginCodes = "Get_UserLoginCodes";
        public const string SaveUserLoginCodes = "Insert_UserLoginCodes";

        public const string Get_UserOrgsByRefreshToken = "Get_UserOrgsByRefreshToken";



        public const string Delete_User = "Delete_User";

        public const string Update_TempWorkOrder = "Update_TempWorkOrder";

        public const string Get_RoundUserGroups = "Get_RoundUserGroups";
        public const string Insert_RoundActivity = "Insert_RoundActivity";
        public const string Get_RoundDetailsbyScheduleDateId = "Get_RoundDetailsbyScheduleDateId";
        public const string Insert_TRoundUserCategories = "Insert_TRoundUserCategories";
        public const string Insert_TRoundLocations = "Insert_TRoundLocations";
    }
}
