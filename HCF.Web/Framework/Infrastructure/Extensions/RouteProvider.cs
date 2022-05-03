using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.Framework.Infrastructure.Extensions
{
    public static class RouteCollectionsProvider
    {
        public static void EndpointRouteBuilder(IEndpointRouteBuilder endpoints)
        {
            #region

            endpoints.MapControllerRoute("epinspections", "ep/{epId}/inspections", new { controller = "Goal", action = "EpInspections" }, new { epId = @"\d+" });
            endpoints.MapControllerRoute("goals", "standards", new { controller = "Goal", action = "index" });
            endpoints.MapControllerRoute("recentActivity", "recentActivity", new { controller = "Goal", action = "RecentActivity" });
            endpoints.MapControllerRoute("addgoal", "goal/add", new { controller = "Goal", action = "creategoal" });
            endpoints.MapControllerRoute("editstandard", "standard/update/{gid}", new { controller = "Goal", action = "editstandard" }, new { wid = @"\d+" });
            endpoints.MapControllerRoute("epnumber", "epnumber/{id}", new { controller = "Goal", action = "epnumber" });
            endpoints.MapControllerRoute("addep", "ep/add", new { controller = "Goal", action = "addep" });
            endpoints.MapControllerRoute("epadd", "standard/{stDescId}/eps", new { controller = "Goal", action = "addepdetails" });
            endpoints.MapControllerRoute("userep", "user/ep", new { controller = "Goal", action = "EPAssignee" });
            endpoints.MapControllerRoute("EpFrequency", "ep/frequency", new { controller = "Goal", action = "EpFrequency" }); ;
            endpoints.MapControllerRoute("risk_management", "risk/management", new { controller = "Goal", action = "Risk_management" });
            endpoints.MapControllerRoute("tilsm", "ilsm/{tilsmId}/view", new { controller = "Goal", action = "GetIlsmInspection" }, new { tilsmId = @"\d+" });
            endpoints.MapControllerRoute("ilsmView", "ilsm/view", new { controller = "Goal", action = "IlsmView" });
            endpoints.MapControllerRoute("lsmAssets", "ilsm/assets/{epDetailId}/{inspectionId}", new { controller = "Goal", action = "IlsmAssets" }, new { epDetailId = @"\d+", inspectionId = @"\d+" });
            endpoints.MapControllerRoute("ilsm", "ilsm", new { controller = "Goal", action = "IlsmMatrix" });
            endpoints.MapControllerRoute("ilsmScore", "ilsm/score", new { controller = "Goal", action = "IlsmScore" });
            endpoints.MapControllerRoute("DeficientEPs", "eps/{score}/deficient", new { controller = "Goal", action = "EpDeficiency" });
            endpoints.MapControllerRoute("docdashboard", "dashboard/document", new { controller = "Goal", action = "DocumentDashBoard" });
            endpoints.MapControllerRoute("epCheckPoints", "eps/{epdetailid}/checkpoints", new { controller = "Goal", action = "addMaingoal" });
            endpoints.MapControllerRoute("assetCheckPoints", "assets/{assetId}/checkpoints", new { controller = "Goal", action = "addMaingoal" });
            endpoints.MapControllerRoute("NonTrackedEPs", "non-tracked-eps", new { controller = "Goal", action = "NonTrackedEPs" });
            endpoints.MapControllerRoute("CMSEpMapping", "cms-ep-mapping", new { controller = "Goal", action = "CMSEpMapping" });

            #endregion


            #region ILSM

            endpoints.MapControllerRoute("newilsm", "ilsm/new", new { controller = "ILSM", action = "AddILSM" });
            endpoints.MapControllerRoute("ilsmbinder", "ilsm/binder", new { controller = "ILSM", action = "IlsmBinder" });

            #endregion          

            #region Assets Controller

            endpoints.MapControllerRoute("assets", "assets", new { controller = "Assets", action = "assets" });
            endpoints.MapControllerRoute("assetstype", "assets/type", new { controller = "Assets", action = "assetstype" });
            endpoints.MapControllerRoute("floorAssets", "floorAssets", new { controller = "Assets", action = "floorassets" });
            endpoints.MapControllerRoute("configurefloorassets", "configurefloorassets", new { controller = "Assets", action = "ConfugureFloorAssetSteps" });
            endpoints.MapControllerRoute("setup", "setup", new { controller = "Assets", action = "Setup" });
            endpoints.MapControllerRoute("insassets", "inspection/assets", new { controller = "Assets", action = "InsAssets" });
            endpoints.MapControllerRoute("assetView", "inspection/category/assets", new { controller = "Assets", action = "AssetView" });
            endpoints.MapControllerRoute("inspection", "inspection/assets/floor", new { controller = "Assets", action = "Inspection" });
            endpoints.MapControllerRoute("asseteps", "asset/eps/{floorAssetId}/{epId}", new { controller = "Assets", action = "AssetEps" }, new { floorassetId = @"\d+", epId = @"\d+" });
            endpoints.MapControllerRoute("ephistory", "ep/history/{epId}", new { controller = "Assets", action = "EpHistory" }, new { epId = @"\d+" });
            endpoints.MapControllerRoute("epinshistory", "ep/inspection/history/{InspectionId}/{epId}", new { controller = "Assets", action = "EPActivityHistory" }, new { epId = @"\d+", InspectionId = @"\d+" });
            endpoints.MapControllerRoute("addnewassets", "asset/new", new { controller = "Assets", action = "CreateFloorassets" });
            endpoints.MapControllerRoute("inspectionbybarcode", "inspection/assets/barcode", new { controller = "Assets", action = "Inspectionbybarcode" });
            endpoints.MapControllerRoute("inspfloorAssets", "inspection/floor/assets", new { controller = "Assets", action = "GetTfloorAssets" });
            endpoints.MapControllerRoute("checkpointhistory", "insp/checkpoints/history/{activityId}", new { controller = "Assets", action = "ActivityHistory" });
            endpoints.MapControllerRoute("TrackingAssets", "TrackingAssets", new { controller = "Assets", action = "TrackingAssetCreation" });
            endpoints.MapControllerRoute("CreateTrackingAssetsReport", "CreateTrackingAssetsReport", new { controller = "pdf", action = "CreateTrackingAssetsReport" });

            #endregion

            #region Home Controller

            endpoints.MapControllerRoute("RiskAssessMent", "RiskAssessMent", new { controller = "Home", action = "RiskAssessMent" });
            endpoints.MapControllerRoute("activityDashboard", "dashboard/activity", new { controller = "Home", action = "ActivityDashboard" });
            endpoints.MapControllerRoute("dashboard", "dashboard", new { controller = "Home", action = "Index" });
            endpoints.MapControllerRoute("Deficiency", "deficiency", new { controller = "Home", action = "Deficiency" });
            //endpoints.MapControllerRoute("deficiencies", "deficiencies/{mode}/{type}/{taggedId}", new { controller = "Home", action = "Deficiencies", type = "0", mode = "all", taggedId = "0" });
            endpoints.MapControllerRoute("deficiencies", "deficiencies/{taggedId}/{orgId}", new { controller = "Home", action = "Deficiencies", taggedId = "0", orgId = "" });
            endpoints.MapControllerRoute("getPassword", "getPassword", new { controller = "Home", action = "GetPassword" });
            endpoints.MapControllerRoute("assetdashboard", "dashboard/assets", new { controller = "Assets", action = "AssetsDashBoard" });
            endpoints.MapControllerRoute("documentdashboard", "dashboard/documents", new { controller = "Goal", action = "DocumentDashBoard" });
            endpoints.MapControllerRoute("CMSDashboard", "dashboard/CMS", new { controller = "Home", action = "CMSDashboard" });
            #endregion

            #region Organization Controller

            endpoints.MapControllerRoute("site", "site", new { controller = "Organization", action = "Site" });
            endpoints.MapControllerRoute("buildings", "buildings", new { controller = "Organization", action = "Buildings" });
            endpoints.MapControllerRoute("addbuilding", "building/add", new { controller = "Organization", action = "addbuilding" });
            endpoints.MapControllerRoute("editBuilding", "editBuilding/{buildingId}", new { controller = "Organization", action = "editBuilding" }, new { buildingId = @"\d+" });
            endpoints.MapControllerRoute("floors", "floors", new { controller = "Organization", action = "Floor" });
            endpoints.MapControllerRoute("addfloor", "floor/add", new { controller = "Organization", action = "addfloor" });
            endpoints.MapControllerRoute("editfloor", "floor/update/{fid}", new { controller = "Organization", action = "editFloor" }, new { fid = @"\d+" });
            endpoints.MapControllerRoute("wings", "wings", new { controller = "Organization", action = "Wings" });
            endpoints.MapControllerRoute("addwings", "wings/add", new { controller = "Organization", action = "addwings" });
            endpoints.MapControllerRoute("editwings", "wings/edit/{wid}", new { controller = "Organization", action = "editWings" }, new { wid = @"\d+" });
            endpoints.MapControllerRoute("general", "general", new { controller = "Organization", action = "General" });
            endpoints.MapControllerRoute("epassignments", "ep/assignments/{type}", new { controller = "Organization", action = "EpAssignments", type = 0 }, new { type = @"\d+" });
            endpoints.MapControllerRoute("frequencies", "frequencies", new { controller = "Organization", action = "Frequencies" });
            endpoints.MapControllerRoute("mngfrequencies", "frequencies/update/{id}", new { controller = "Organization", action = "MngFrequencies" }, new { id = @"\d+" });
            endpoints.MapControllerRoute("inboxemails", "inbox/emails", new { controller = "Organization", action = "InboxSetup" });
            endpoints.MapControllerRoute("assign_ep_resposibility", "assign_ep_resposibility", new { controller = "Organization", action = "AssignEpResponsibility" });
            endpoints.MapControllerRoute("shift", "shift", new { controller = "Organization", action = "Shift" });
            endpoints.MapControllerRoute("addshift", "shift/add", new { controller = "Organization", action = "Addshift" });
            endpoints.MapControllerRoute("orgsettings", "orgsettings", new { controller = "Organization", action = "OrgSettings" });
            #endregion

            #region Account Controller

            endpoints.MapControllerRoute("forgotpassword", "forgot/password", new { controller = "Account", action = "ForgotPassword" });
            endpoints.MapControllerRoute("recover", "user/password/{type}/{token}", new { controller = "Account", action = "Recover" });
            //endpoints.MapControllerRoute("login", "login", new { controller = "Account", action = "Login" });
            //endpoints.MapControllerRoute("logout", "logout", new { controller = "Account", action = "LogOff" });
            endpoints.MapControllerRoute("generatepin", "generate-pin", new { controller = "Account", action = "ManagePin" });
            endpoints.MapControllerRoute("redirecttologout", "redirect-to-logout", new { controller = "Account", action = "RedirectToLogin" });
            // endpoints.MapControllerRoute("create", "create", new { controller = "Account", action = "create" });

            endpoints.MapControllerRoute("create", "create/user/{invitationId}", new { controller = "Auth", action = "create" });

            endpoints.MapControllerRoute("externallogin", "externallogin", new { controller = "Account", action = "ExternalLogin" });

            endpoints.MapControllerRoute("login", "login", new { controller = "Auth", action = "Login" });
            endpoints.MapControllerRoute("logout", "logout", new { controller = "Auth", action = "LogOff" });
            #endregion

            #region User Controller

            endpoints.MapControllerRoute("permissions", "permissions", new { controller = "User", action = "Permissions" });
            endpoints.MapControllerRoute("users", "users", new { controller = "User", action = "Users" });
            endpoints.MapControllerRoute("usergroups", "user/groups", new { controller = "User", action = "UserGroups" });
            endpoints.MapControllerRoute("changepassword", "change-password/{mode}", new { controller = "User", action = "Manage" });
            endpoints.MapControllerRoute("myprofile", "my/profile", new { controller = "User", action = "MyProfile" });
            endpoints.MapControllerRoute("manageprofile", "manage/profile", new { controller = "User", action = "EditProfile" });
            endpoints.MapControllerRoute("loginhistory", "login/history", new { controller = "User", action = "LoginHistory" });
            endpoints.MapControllerRoute("register", "user/update/{userId}/{vendorId}", new { controller = "User", action = "Register" }, new { userId = @"\d+" });
            endpoints.MapControllerRoute("adduser", "user/add", new { controller = "User", action = "Register" });
            endpoints.MapControllerRoute("userhospitals", "user/facilities/index", new { controller = "User", action = "UserHospitals" });

            endpoints.MapControllerRoute("switchuser", "switch-user", new { controller = "User", action = "SwitchUser" });
            endpoints.MapControllerRoute("usergroup", "user/group", new { controller = "User", action = "UserGroup" });
            endpoints.MapControllerRoute("usersitemapping", "users/campus", new { controller = "User", action = "UserSites" });
            endpoints.MapControllerRoute("userdrawings", "users/drawings", new { controller = "User", action = "UserDrawings" });

            endpoints.MapControllerRoute("usercertificate", "user/{userid}/certificates", new { controller = "User", action = "UserCertificates" });

            #endregion

            //#region News Controller

            //endpoints.MapControllerRoute("createnews", "create/news", new { controller = "News", action = "AddNews" });
            //endpoints.MapControllerRoute("news", "news", new { controller = "News", action = "News" });
            //endpoints.MapControllerRoute("latestreleases", "latest-releases/{type}/{newsId}", new { controller = "News", action = "NewsDescription", type = "0" });

            //#endregion

            #region DocumentType Controller

            endpoints.MapControllerRoute("createdocumenttype", "create/document/type", new { controller = "Documents", action = "AddDocumentTypeMaster" });
            endpoints.MapControllerRoute("documenttype", "document/type", new { controller = "Documents", action = "DocumentType" });
            #endregion          

            #region WorkOrder Controller

            endpoints.MapControllerRoute("workorder", "wo/workorder", new { controller = "WorkOrder", action = "Index" });
            endpoints.MapControllerRoute("wocreate", "wo/create", new { controller = "WorkOrder", action = "Create" });           
            endpoints.MapControllerRoute("woupdate", "wo/update/{issueId}", new { controller = "WorkOrder", action = "UpdateWorkOrder" }, new { issueId = @"\d+" });



            #endregion

            #region Import Controller

            endpoints.MapControllerRoute("downladSapmle", "DownladSapmle", new { controller = "Import", action = "DownladSapmle" });
            endpoints.MapControllerRoute("importExcel", "ImportExcel", new { controller = "Import", action = "ImportExcel" });
            endpoints.MapControllerRoute("importnonassetseps", "import/nonassetseps", new { controller = "Import", action = "ImportNonAssetsEPs" });
            endpoints.MapControllerRoute("importAsset", "ImportAsset", new { controller = "Import", action = "ImportAsset" });

            #endregion

            #region Repository Controller


            endpoints.MapControllerRoute("floorplans", "drawings", new { controller = "Drawing", action = "FloorPlans" });
            endpoints.MapControllerRoute("floorplanhistory", "drawings/{floorId}/history", new { controller = "Drawing", action = "FloorPlan" }, new { floorId = @"\d+" });
            endpoints.MapControllerRoute("floordrawing", "floors/drawing", new { controller = "Drawing", action = "Index" });

            endpoints.MapControllerRoute("docrepo", "docrepo", new { controller = "Repository", action = "DocRepoIndex" });
            endpoints.MapControllerRoute("binders", "binders", new { controller = "Repository", action = "Binders" });
            //endpoints.MapControllerRoute("getepdocbybinder", "get/ep/doc/by/binder/{id}", new { controller = "Repository", action = "GetEpDocByBinder" }, new { id = @"\d+" });
            endpoints.MapControllerRoute("getepdocbybinder", "get/ep/doc/by/binder", new { controller = "Repository", action = "GetEpDocByBinder" });
            endpoints.MapControllerRoute("addBinder", "addBinder", new { controller = "Repository", action = "AddBinders" });
            endpoints.MapControllerRoute("policies-list", "policies-list", new { controller = "Repository", action = "Policies" });
            endpoints.MapControllerRoute("getEpBinder", "getEpBinder/{id}", new { controller = "Repository", action = "GetEpBinder" }, new { id = @"\d+" });
            endpoints.MapControllerRoute("newbinders", "new/binders", new { controller = "Repository", action = "EPBinders" });
            endpoints.MapControllerRoute("docrecentfiles", "doc/recent/files", new { controller = "Repository", action = "DocRecentFiles" });
            endpoints.MapControllerRoute("deficienciesBinder", "binder/deficienciesBinder", new { controller = "Repository", action = "DeficienciesBinder" });
            endpoints.MapControllerRoute("roundreportBinder", "binder/roundreportBinder", new { controller = "Round", action = "RoundReportBinder" });

            #endregion

            #region Reports Controllers

            endpoints.MapControllerRoute("reports_Compliance", "reports/Compliance", new { controller = "Reports", action = "ComplianceIndex" });
            endpoints.MapControllerRoute("repors_assets", "reports/assets", new { controller = "Reports", action = "AssetsIndex" });
            endpoints.MapControllerRoute("reports_risk_management_reports", "reports/risk/management", new { controller = "Reports", action = "RiskManagementIndex" });
            endpoints.MapControllerRoute("reports_documents", "reports/documents", new { controller = "Reports", action = "DocumentsIndex" });
            endpoints.MapControllerRoute("reports_epassignments", "reports/ep/assignments", new { controller = "Reports", action = "EPAssignmentsIndex", type = 0 });
            endpoints.MapControllerRoute("compliancedashboard", "compliance/dashboard", new { controller = "Reports", action = "InspectionDetailReports" });
            endpoints.MapControllerRoute("reports", "reports", new { controller = "Reports", action = "Reports" });
            endpoints.MapControllerRoute("generate_report", "generate_report", new { controller = "Reports", action = "GenerateReports" });
            endpoints.MapControllerRoute("assets_generate_report", "reports/assets_generate", new { controller = "Reports", action = "AssetsInspectionReport" });
            endpoints.MapControllerRoute("ilsmreports", "reports/ilsm", new { controller = "Reports", action = "IlsmReports" });
            endpoints.MapControllerRoute("reviewtool", "review-tool", new { controller = "Reports", action = "EpsReviewTool" });

            endpoints.MapControllerRoute("inventoryAssetsReport", "report/assets/inventory", new { controller = "Reports", action = "InventoryAssetsReport" });
            endpoints.MapControllerRoute("inventoryAssetsInspectionReport", "report/assets/inventory/inspection", new { controller = "Reports", action = "AssetsInventoryInspectionReport" });
            endpoints.MapControllerRoute("roundreport", "reports/round", new { controller = "Reports", action = "RoundReports" });
            endpoints.MapControllerRoute("managementreport", "reports/management", new { controller = "Reports", action = "ManagementReport" });
            endpoints.MapControllerRoute("assetcompliancematrix", "reports/asset/compliance/matrix", new { controller = "Reports", action = "ComplianceAssetsTrackingReports" });
            endpoints.MapControllerRoute("archivedepsreport", "reports/ArchivedEPsReport", new { controller = "Reports", action = "ArchivedEPsReport" });
            #endregion

            #region Rounds Controllers

            endpoints.MapControllerRoute("rounds", "rounds", new { controller = "Rounds", action = "Rounds" });
            endpoints.MapControllerRoute("survey", "survey", new { controller = "Rounds", action = "Survey" });
            endpoints.MapControllerRoute("firedrill", "fire/drill", new { controller = "Rounds", action = "FireDrill" });
            endpoints.MapControllerRoute("addQuestionnaires", "addQuestionnaires", new { controller = "Rounds", action = "ManageQuestionnaires" });
            endpoints.MapControllerRoute("firedrillQues", "firedrillQues", new { controller = "Rounds", action = "FireDrillQuestionnaries" });
            endpoints.MapControllerRoute("firedrillCommonCateg", "firedrillCommonCateg", new { controller = "Rounds", action = "FiredrillCommonCategories" });
            endpoints.MapControllerRoute("FloorRounds", "FloorRounds/{floorId}/{rId}", new { controller = "Rounds", action = "FloorRounds" }, new { floorId = @"\d+", rId = @"\d+" });
            endpoints.MapControllerRoute("surveyhistory", "survey/history/{floorId}/{buildingId}", new { controller = "Rounds", action = "SurveyHistory" });
            endpoints.MapControllerRoute("roundQues", "roundQues", new { controller = "Rounds", action = "RoundQuestionnaries" });//remove view and action methods
            endpoints.MapControllerRoute("dailyLogs", "daily/logs", new { controller = "Rounds", action = "DailyLogs" });

            endpoints.MapControllerRoute("roundUserGroup", "roundUserGroup", new { Controller = "Rounds", action = "RoundUserGroup" });
            endpoints.MapControllerRoute("addRoundUserGroup", "addRoundUserGroup", new { Controller = "Rounds", action = "addRoundUserGroup" });
            endpoints.MapControllerRoute("editRoundUserGroup", "editRoundUserGroup", new { Controller = "Rounds", action = "editRoundUserGroup" });
            endpoints.MapControllerRoute("FireDrillSettings", "settings/fireDrill", new { controller = "Rounds", action = "FireDrillSettings" });

            endpoints.MapControllerRoute("TagSettings", "settings/tag", new { controller = "Organization", action = "TagSettings" });

            endpoints.MapControllerRoute("newrounds", "round/new", new { controller = "Round", action = "Index" });
            endpoints.MapControllerRoute("addround", "round/new/add", new { controller = "Round", action = "NewRoundInsp" });
            endpoints.MapControllerRoute("roundinsp", "round/{rid}/insp", new { controller = "Round", action = "RoundInspection" }, new { rid = @"\d+" });
            endpoints.MapControllerRoute("roundfloorinsp", "floor/round/{rId}/floor/{floorId}", new { controller = "Round", action = "FloorRoundInspction" }, new { rId = @"\d+" });
            endpoints.MapControllerRoute("roundIssue", "round/{rId}/issues", new { controller = "Round", action = "RoundIssues" }, new { rId = @"\d+" });
            endpoints.MapControllerRoute("roundcategories", "round/round/categories", new { controller = "Round", action = "RoundCategories" });
            endpoints.MapControllerRoute("roundQuest", "roundQuest", new { controller = "Round", action = "RoundQuestionnaire" });
            endpoints.MapControllerRoute("addquestocustomround", "add/questocustomround", new { controller = "Round", action = "AddQuesttoCustomRound" });
            endpoints.MapControllerRoute("roundschedular", "round/scheduler", new { controller = "Round", action = "RoundScheduler" });
            endpoints.MapControllerRoute("grouproundschedular", "round/groupscheduler", new { controller = "Round", action = "GroupRoundScheduler" });
            endpoints.MapControllerRoute("rounddeficienciesbinder", "binder/rounddeficienciesbinder", new { controller = "Round", action = "RoundDeficienciesBinder" });


            endpoints.MapControllerRoute("roundcommonques", "round/roundCommonQuestionnaire", new { controller = "Round", action = "RoundCommonQuestionnaire" });
            endpoints.MapControllerRoute("firedrillQuestionnaries", "firedrillQuestionnaries", new { controller = "Rounds", action = "FireDrillQues" });
            endpoints.MapControllerRoute("addFiredrillQuestionnaires", "addFiredrillQuestionnaires", new { controller = "Rounds", action = "AddFiredrillQuestionnaires" });
            endpoints.MapControllerRoute("roundcommoncategories", "round/common/categories", new { controller = "Round", action = "RoundCommonCategories" });
            endpoints.MapControllerRoute("firedrillCateg", "firedrillCateg", new { controller = "Rounds", action = "FiredrillCategories" });
            endpoints.MapControllerRoute("roundschedulerflow", "roundschedulerflow", new { controller = "Round", action = "SchedulerPageFlow" });
            endpoints.MapControllerRoute("firedrillreport", "firedrillreport", new { controller = "Rounds", action = "FireDrillReport" });
            endpoints.MapControllerRoute("newdailyLogs", "daily/logs/new/{pmlogId}", new { controller = "Rounds", action = "PMDailyLogs" });
            endpoints.MapControllerRoute("roundactivity", "roundactivity", new { controller = "Round", action = "RoundActivity" });
            #endregion

            #region Vendor Controller

            endpoints.MapControllerRoute("vendorConfigurations", "vendorConfigurations", new { controller = "Vendor", action = "VendorConfigurations" });
            endpoints.MapControllerRoute("vendors", "vendors", new { controller = "Vendor", action = "Vendors" });
            endpoints.MapControllerRoute("addVendor", "addVendor", new { controller = "Vendor", action = "Addvendors" });
            endpoints.MapControllerRoute("updateContactDetails", "updateContactDetails", new { controller = "Vendor", action = "UpdateContactDetails" });
            endpoints.MapControllerRoute("VendorResources", "vendor/resources", new { controller = "Vendor", action = "VendorResources" });
            endpoints.MapControllerRoute("AddVendorResource", "vendor/resources/add", new { controller = "Vendor", action = "AddVendorResources" });
            endpoints.MapControllerRoute("vendordashboard", "vendordashboard", new { controller = "Vendor", action = "Vendordashboard" });

            #endregion

            #region Usermenu Controller

            endpoints.MapControllerRoute("usermenuConfigurations", "usermenuConfigurations", new { controller = "Menu", action = "UsermenuConfigurations" });
            endpoints.MapControllerRoute("myplan", "myplan", new { controller = "Menu", action = "GetModules" });
            #endregion

            #region Main Controllers

            endpoints.MapControllerRoute("client", "client", new { controller = "Main", action = "Clients" });
            endpoints.MapControllerRoute("setClient", "setClient/{orgkey}", new { controller = "Main", action = "SetClient" });
            endpoints.MapControllerRoute("lists", "lists/{orgkey}", new { controller = "Main", action = "Hospitals" });

            #endregion region

            #region Common Controllers

            endpoints.MapControllerRoute("jsurls", "jsurls", new { controller = "Common", action = "CRxUrls" });
            endpoints.MapControllerRoute("TMSRefresh", "TMSRefresh", new { controller = "Common", action = "TMSRefresh" });

            #endregion

            #region Message Controllers

            endpoints.MapControllerRoute("unauthorized", "unauthorized", new { controller = "Message", action = "AccessDenied" });
            endpoints.MapControllerRoute("error404", "error404", new { controller = "Message", action = "Error404" });

            #endregion

            #region FireWatch Controllers

            endpoints.MapControllerRoute("firewatch", "firewatch", new { controller = "FireWatch", action = "FireWatch" });
            endpoints.MapControllerRoute("addfirewatch", "add/firewatch", new { controller = "FireWatch", action = "ManageFireWatchSchedules" });

            #endregion

            #region Notification Controllers

            endpoints.MapControllerRoute("notifications", "notifications", new { controller = "Notification", action = "NotificationEmails" });
            endpoints.MapControllerRoute("inspectiongroups", "inspection/groups", new { controller = "Inspection", action = "InspectionGroups" });
            endpoints.MapControllerRoute("notificationMatrix", "notificationMatrix", new { controller = "Notification", action = "NotificationSettings" });

            #endregion                                              

            #region Inspection Controller

            endpoints.MapControllerRoute("InspCalendar", "insp-calendar", new { controller = "Inspection", action = "InspCalendar" });
            endpoints.MapControllerRoute("addgroup", "add-insp-group", new { controller = "Inspection", action = "AddGroup" });
            endpoints.MapControllerRoute("inspquestion", "inspection/question", new { controller = "Inspection", action = "InspectionQuestion" });

            #endregion

            #region Manufacture Controller

            endpoints.MapControllerRoute("manufactures", "manufactures", new { controller = "Manufacture", action = "Manufactures" });
            endpoints.MapControllerRoute("addManufactures", "add/Manufactures", new { controller = "Manufacture", action = "mngManufacture" });

            #endregion

            #region ICRA Controller
            endpoints.MapControllerRoute("showhistory", "showhistory/{modulename}/{icraid}/{permittype}/{permitno}", new { controller = "ICRA", action = "IcraHistory" });
            endpoints.MapControllerRoute("showfiles", "showfiles/{modulename}/{icraid}/{permittype}", new { controller = "ICRA", action = "TicraFiles" });
            endpoints.MapControllerRoute("observationreport", "observationreport/{modulename}/{projectId}/{icraId}/{permitnumber}", new { controller = "ICRA", action = "TICRAProjectObservationReport" });
            endpoints.MapControllerRoute("viewobservationreport", "view/observationreport/{modulename}/{icraId}/{permittype}/{permitno}", new { controller = "ICRA", action = "ObservationReports" });
            endpoints.MapControllerRoute("constructiontype", "constructiontype", new { controller = "ICRA", action = "ConstructionType" });
            endpoints.MapControllerRoute("addConstructionType", "addConstructionType", new { controller = "ICRA", action = "AddConstructionType" });
            endpoints.MapControllerRoute("constructionrisk", "constructionrisk", new { controller = "ICRA", action = "ConstructionRisk" });
            endpoints.MapControllerRoute("addconstructionrisk", "addconstructionrisk", new { controller = "ICRA", action = "AddConstructionRisk" });
            endpoints.MapControllerRoute("constructionclass", "constructionclass", new { controller = "ICRA", action = "ConstructionClass" });
            endpoints.MapControllerRoute("addconstructionclass", "addconstructionclass", new { controller = "ICRA", action = "AddConstructionClass" });
            endpoints.MapControllerRoute("icrasteps", "icrasteps", new { controller = "ICRA", action = "ICRASteps" });
            endpoints.MapControllerRoute("addicrasteps", "addicrasteps", new { controller = "ICRA", action = "AddICRASteps" });
            endpoints.MapControllerRoute("inspectionicrav1", "inspection/icra/version/1", new { controller = "ICRA", action = "InspectionIcra", version = 1 });
            endpoints.MapControllerRoute("inspectionicrav2", "inspection/icra/version/2", new { controller = "ICRA", action = "InspectionIcra", version = 2 });
            endpoints.MapControllerRoute("addinspectionicra", "inspection/icra/add", new { controller = "ICRA", action = "AddInspectionIcra" });
            endpoints.MapControllerRoute("icrariskarea", "icra/riskarea", new { controller = "ICRA", action = "ICRARiskArea" });
            endpoints.MapControllerRoute("icramatrixv1", "icra/matrix/1", new { controller = "ICRA", action = "ICRAMatixPrecautions", version = 1 });
            endpoints.MapControllerRoute("icramatrixv2", "icra/matrix/2", new { controller = "ICRA", action = "ICRAMatixPrecautions", version = 2 });
            endpoints.MapControllerRoute("comingsoon", "comingsoon", new { controller = "Common", action = "ComingSoon" });
            endpoints.MapControllerRoute("MngReportCheckPoints", "MngReportCheckPoints", new { controller = "ICRA", action = "MngReportCheckPoints" });
            endpoints.MapControllerRoute("ReportCheckPoints", "ReportCheckPoints", new { controller = "ICRA", action = "ReportCheckPoints" });
            endpoints.MapControllerRoute("icraObservationReport", "icraObservationReport/{icraId}", new { controller = "ICRA", action = "TICRAObservationReport" }, new { tilsId = @"\d+" });
            //endpoints.MapControllerRoute("icrapermit", "icra/permits", new { controller = "ICRA", action = "IcraPermit" });
            //endpoints.MapControllerRoute("icrapermit", "icra/permits", new { controller = "ICRA", action = "IcraProjectPermit" });
            endpoints.MapControllerRoute("icrapermit", "permits/observation-reports", new { controller = "ICRA", action = "IcraProjectPermit" });
            endpoints.MapControllerRoute("observationreportbinder", "binder/observationreportbinder", new { controller = "ICRA", action = "ObservationReportBinder" });

            #endregion

            #region Search Controller

            endpoints.MapControllerRoute("search", "search", new { controller = "Search", action = "SearchView" });
            endpoints.MapControllerRoute("searchindex", "searchindex", new { controller = "Search", action = "CreateIndex" });
            endpoints.MapControllerRoute("OptimizeIndex", "OptimizeIndex", new { controller = "Search", action = "OptimizeIndex" });
            endpoints.MapControllerRoute("ClearIndex", "ClearIndex", new { controller = "Search", action = "ClearIndex" });

            #endregion  

            #region Fire Extinguisher Area Controller

            endpoints.MapControllerRoute("fireextinguisher", "rbi/dashboard", new { controller = "Building", action = "Index" });
            //endpoints.MapControllerRoute("fireextinguisherDash", "rbi/dashboard/routes", new { controller = "FireExtinguisher", action = "Index" }, new { floorId = @"\d+", inspType = @"\d+", assetId = @"\d+", routeId = @"\d+" });
            endpoints.MapControllerRoute("fereports", "reports/fire/extinguisher", new { controller = "FireExtinguisher", action = "FERouteReports" });
            endpoints.MapControllerRoute("fireExreports", "fireExreports", new { controller = "FireExtinguisher", action = "Reports" });
            endpoints.MapControllerRoute("locations", "locations", new { controller = "Location", action = "locations" });
            endpoints.MapControllerRoute("routes", "routes", new { controller = "Location", action = "routes" });
            endpoints.MapControllerRoute("routeInspection", "inspection/rbi", new { controller = "Building", action = "RouteInspection" });

            #endregion

            #region Tips Controller

            endpoints.MapControllerRoute("tips", "tips", new { controller = "Tips", action = "Tips" });
            endpoints.MapControllerRoute("howto", "howto", new { controller = "Tips", action = "HowTo" });

            #endregion

            #region Schedules

            endpoints.MapControllerRoute("schedules", "insp/schedules", new { controller = "Schedule", action = "Schedules" });
            endpoints.MapControllerRoute("assetschedules", "schedule/assets", new { controller = "Schedule", action = "AssetsSchedule" });
            endpoints.MapControllerRoute("epschedules", "schedule/eps", new { controller = "Schedule", action = "EPSchedule" });

            #endregion

            #region Questionnaire

            endpoints.MapControllerRoute("questionnaire", "questionnaire", new { controller = "Questionnaire", action = "Index" });
            endpoints.MapControllerRoute("addquestionnaire", "questionnaire/add", new { controller = "Questionnaire", action = "AddQuestionnaire" });
            endpoints.MapControllerRoute("updatequestionnaire", "questionnaire/update/{questionnaireId}", new { controller = "Questionnaire", action = "AddQuestionnaire" }, new { questionnaireId = @"\d+" });



            #endregion          

            #region Projects

            endpoints.MapControllerRoute("projects", "projects", new { controller = "IcraProject", action = "Index" });


            #endregion

            #region   Documents controller

            endpoints.MapControllerRoute("inbox", "inbox", new { controller = "Documents", action = "Inbox" });
            endpoints.MapControllerRoute("getmiscepdocbybinder", "misc/ep/doc/by/binder", new { controller = "Documents", action = "GetMiscEpDocByBinder" });
            endpoints.MapControllerRoute("getreqepdocwithoutbinder", "req/ep/doc/by/binder", new { controller = "Documents", action = "GetReqEpDocWithoutBinder" }); 
            //endpoints.MapControllerRoute("createdocumenttype", "createdocumenttype", new { controller = "Documents", action = "AddDocumentType" });
            #endregion

            #region EP Groups
            endpoints.MapControllerRoute("createepgroupsname", "createepgroupsname", new { controller = "EPGroups", action = "AddEPGroupsName" });
            endpoints.MapControllerRoute("getepgroupsnamelist", "getepgroupsnamelist", new { controller = "EPGroups", action = "GetEPGroupsNameList" });
            endpoints.MapControllerRoute("asigneps", "asigneps", new { controller = "EPGroups", action = "AsignEPs" });
            #endregion

            #region ICRA Project

            endpoints.MapControllerRoute("icraproject", "icra/project", new { controller = "IcraProject", action = "Index" });
            endpoints.MapControllerRoute("child_icra_project", "child_icra_project", new { controller = "IcraProject", action = "ChildIcraProject" });


            #endregion

            #region  PCRA
            endpoints.MapControllerRoute("addTPCRA", "pcra/addtpcra", new { controller = "PCRA", action = "AddPCRA" });
            endpoints.MapControllerRoute("getalltpcra", "permit/pcra", new { controller = "PCRA", action = "GetAllTPCRA" });
            endpoints.MapControllerRoute("questionPCRA", "questionPCRA", new { controller = "PCRA", action = "QuestionPCRA" });
            endpoints.MapControllerRoute("addquestionPCRA", "addquestionPCRA", new { controller = "PCRA", action = "AddQuestionPCRA" });
            endpoints.MapControllerRoute("addTCRA", "cra/addtcra", new { controller = "PCRA", action = "AddCRA" });
            //endpoints.MapControllerRoute("getalltpcra", "pcra/getalltpcra", new { controller = "PCRA", action = "GetAllTPCRA" });
            endpoints.MapControllerRoute("getalltcra", "permit/cra", new { controller = "PCRA", action = "GetAllTCRA" });



            #endregion

            #region TMS Maintenance Connection 

            endpoints.MapControllerRoute("mcassets", "tms/mc/assets", new { controller = "MaintenanceConnection", action = "GetTMSResults" });
            endpoints.MapControllerRoute("mcworkorders", "tms/mc/workorders", new { controller = "MaintenanceConnection", action = "GetTMSWorkOrder" });
            endpoints.MapControllerRoute("mcusers", "tms/mc/users", new { controller = "MaintenanceConnection", action = "GetTmsUsers" });
            endpoints.MapControllerRoute("mcwoassignments", "tms/mc/workorders/users", new { controller = "MaintenanceConnection", action = "GetTMSWorkOrderAssignment" });
            endpoints.MapControllerRoute("mcwotasks", "tms/mc/workorders/tasks", new { controller = "MaintenanceConnection", action = "GetWorkOrderTasks" });

            #endregion

            #region State and City

            endpoints.MapControllerRoute("State", "State", new { Controller = "Common", action = "GetStates" });
            endpoints.MapControllerRoute("City", "City", new { Controller = "Common", action = "GetCitiesForView" });

            #endregion

            #region  Permit
            endpoints.MapControllerRoute("settingpermit", "setting/permit", new { controller = "Permit", action = "PermitSettings" });
            endpoints.MapControllerRoute("ceilingpermit", "permit/ceiling/{taggedId}", new { controller = "Permit", action = "Index", taggedId = "0" });
            endpoints.MapControllerRoute("AddHotWorkPermit", "hot-work-permit/add", new { controller = "HotWorkPermit", action = "AddHotWorkPermit" });
            endpoints.MapControllerRoute("getallthotworkpermit", "permit/hot-work", new { controller = "HotWorkPermit", action = "GetAllHotWorkPermit" });

            endpoints.MapControllerRoute("lifesafetydevice", "life-safety-device", new { controller = "Permit", action = "LifeSafetyDevicesForms" });
            endpoints.MapControllerRoute("newLsdformadd", "life-safety-device/{formType}/form/{formId}", new { controller = "Permit", action = "AddLifeSafetyDevicesFormsPermit", formId = "-1", formType = "addition" }, dataTokens: new { Name = "life_safety_device" });

            endpoints.MapControllerRoute("method_of_procedure", "method-of-procedure", new { controller = "Permit", action = "MethodofProcedure" });
            endpoints.MapControllerRoute("addmethodofprocedure", "method_of_procedure/{id}", new { controller = "Permit", action = "AddMethodofProcedure" }, new { id = @"\d+" });


            endpoints.MapControllerRoute("permit", "permit/ceiling", new { controller = "Permit", action = "Index" });
            endpoints.MapControllerRoute("addceilingpermit", "ceiling/permit/add", new { controller = "Permit", action = "AddCeilingPermit" });
            endpoints.MapControllerRoute("updateceilingpermit", "ceiling/permit/update/{ceilingpermitId}", new { controller = "Permit", action = "AddCeilingPermit" }, new { ceilingpermitId = @"\d+" });


            endpoints.MapControllerRoute("fsbpermit", "permits/fsb", new { controller = "Permit", action = "FSBPermit" });
            endpoints.MapControllerRoute("addfsbpermit", "permits/fsbpermit/add", new { controller = "Permit", action = "AddFSBPermit" });

            endpoints.MapControllerRoute("occurrence-report", "occurrence-report", new { controller = "Permit", action = "FacilitiesMaintenanceOccurrence" });
            endpoints.MapControllerRoute("addfacilitiesmaintenanceoccurrence", "facilities_maintenance_occurrence/{id}", new { controller = "Permit", action = "AddFacilitiesMaintenanceOccurrence" });

            endpoints.MapControllerRoute("paperpermit", "permits/paper", new { controller = "Permit", action = "PaperPermit" });
            endpoints.MapControllerRoute("addpaperpermit", "permits/paper/{id}", new { controller = "Permit", action = "AddPaperPermit" }, new { id = @"\d+" });

            endpoints.MapControllerRoute("all_permits", "permits", new { controller = "Permit", action = "GetAllPermits" });
            endpoints.MapControllerRoute("permits_report", "permits/reports", new { controller = "Permit", action = "PermitReport" });
            //endpoints.MapControllerRoute("permitfloorplan", "permitfloorplan/{floorplanId}/{mode}/{permitId}", new { controller = "Permit", action = "AddFacilitiesMaintenanceOccurrence" }).DataTokens[dataTokenName] = "facilities_maintenance_occurrence";
            endpoints.MapControllerRoute("permit_workflow", "permit/workflow", new { controller = "Permit", action = "PermitFormSettings" });
            #endregion

            #region UserRole

            endpoints.MapControllerRoute("userrole", "user/role", new { controller = "Menu", action = "UserRole" });
            endpoints.MapControllerRoute("Roles", "Roles", new { controller = "Menu", action = "Roles" });
            endpoints.MapControllerRoute("AddorEditRoles", "AddorEditRoles", new { controller = "Menu", action = "AddorEditRoles" });
            #endregion

            #region menu epgroup

            endpoints.MapControllerRoute("EPGroups", "EPGroups", new { controller = "EPGroups", action = "EPGroups" });

            #endregion

            #region BBI

            endpoints.MapControllerRoute("BBI", "BBI", new { controller = "Site", action = "BBI" });

            #endregion         

            #region Fire System Type
            endpoints.MapControllerRoute("firesystemtype", "fire/system/type", new { controller = "Site", action = "firesystemtype" });
            #endregion


            #region Locations
            endpoints.MapControllerRoute("groupbuildings", "location/groupbuildings", new { controller = "Location", action = "LocationGroupBuildings" });
            #endregion
            #region Asset Device Change
            endpoints.MapControllerRoute("assetdevices", "assetdevices", new { controller = "Permit", action = "AssetDevicesList" });
            endpoints.MapControllerRoute("addAssetDevice", "addAssetDevice", new { controller = "Permit", action = "AddAssetDevices" });
            endpoints.MapControllerRoute("assetdevicechange", "asset-device-change", new { controller = "Permit", action = "AssetDeviceChangeForm" });
            endpoints.MapControllerRoute("newassetdeviceform", "asset-device-change/{formType}/form/{formId}", new { controller = "Permit", action = "AddAssetDeviceChangeFormsPermit", formId = "-1", formType = "addition" }, dataTokens: new { Name = "asset-device-change" });

            #endregion
            endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}