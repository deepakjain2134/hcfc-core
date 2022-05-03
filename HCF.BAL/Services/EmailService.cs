using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using HCF.Utility;
using HCF.Utility.AppConfig;
using Microsoft.AspNetCore.Mvc;

namespace HCF.BAL
{
    public class EmailService : IEmailService
    {
        private readonly ILoggingService _logger;
        private readonly IEmailRepository _emailRepository;
        private readonly IConstructionRepository _constructionRepository;
        private readonly IPCRARepository _pCRARepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IIlsmRepository _ilsmRepository;
        private readonly IRounds _roundRepository;
        private readonly IBuildingsRepository _buildingsRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IEpRepository _epRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IInspectionActivityRepository _inspectionActivityRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IMainGoalRepository _mainGoalRepository;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly ITransaction _transactionRepository;
        private readonly IAppSetting _appSettings;
        private readonly IEmailProcessor _emailProcessor;
        private readonly ICommonProvider _commonProvider;
        public EmailService(
            IAppSetting appSettings,
            ITransaction transactionRepository,
            IFloorAssetRepository floorAssetRepository,
            IMainGoalRepository mainGoalRepository,
            ICommonRepository commonRepository,
            IInspectionActivityRepository inspectionActivityRepository,
            IFloorRepository floorRepository, IUsersRepository usersRepository,
            IEpRepository epRepository,
            IDocumentTypeRepository documentTypeRepository, IRounds roundRepository,
            ILoggingService logger, IEmailRepository emailRepository, IConstructionRepository constructionRepository,
            IPCRARepository pCRARepository, IVendorRepository vendorRepository,
            IOrganizationRepository organizationRepository,
            IIlsmRepository ilsmRepository, IBuildingsRepository buildingsRepository,
            INotificationRepository notificationRepository, IDocumentsRepository documentsRepository,
            IEmailProcessor emailProcessor, ICommonProvider commonProvider)
        {
            _emailProcessor = emailProcessor;
            _appSettings = appSettings;
            _transactionRepository = transactionRepository;
            _floorAssetRepository = floorAssetRepository;
            _mainGoalRepository = mainGoalRepository;
            _commonRepository = commonRepository;
            _inspectionActivityRepository = inspectionActivityRepository;
            _floorRepository = floorRepository;
            _usersRepository = usersRepository;
            _epRepository = epRepository;
            _documentTypeRepository = documentTypeRepository;
            _documentsRepository = documentsRepository;
            _notificationRepository = notificationRepository;
            _buildingsRepository = buildingsRepository;
            _roundRepository = roundRepository;
            _ilsmRepository = ilsmRepository;
            _constructionRepository = constructionRepository;
            _logger = logger;
            _emailRepository = emailRepository;
            _pCRARepository = pCRARepository;
            _vendorRepository = vendorRepository;
            _organizationRepository = organizationRepository;
            _commonProvider = commonProvider;


        }

        //private string MailFrom { get; } = _appSettings.SenderMailFrom;
        //private string WebBaseUrl { get; } = _appSettings.WebUrlPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="attachments"></param>
        /// <param name="ccAddress"></param>
        /// <returns></returns>
        private bool SendMail(string to, string subject, string body, string from, List<string> attachments = null, string ccAddress = null)
        {
            var outgoingMailId = _emailRepository.SaveEmail(to, ccAddress, subject, body, from);
            bool status = _emailProcessor.SendMail(to, subject, ccAddress, body, from, attachments);// true;// EmailProcessor.SendMail(to, subject, ccAddress, body, from, attachments);
            if (outgoingMailId.HasValue && status)
                _emailRepository.UpdateSentMail(outgoingMailId.Value);
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="ccAddress"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="fileBytes"></param>
        /// <param name="filename"></param>
        private bool SendMail(string toAddress, string subject, string ccAddress, string body, string @from, byte[] fileBytes, string filename)
        {
            var outgoingMailId = _emailRepository.SaveEmail(toAddress, ccAddress, subject, body, from);
            bool status = _emailProcessor.SendMail(toAddress, subject, ccAddress, body, from, fileBytes, filename);
            if (outgoingMailId.HasValue && status)
                _emailRepository.UpdateSentMail(outgoingMailId.Value);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationCategory"></param>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="siteId"></param>
        /// <param name="EventId"></param>
        /// <returns></returns>
        private NotificationEmails GetNotificationEmail(int notificationCategory, string buildingId, string floorId, string siteId, int? EventId = 1)
        {
            if (siteId == null)
                throw new ArgumentNullException(nameof(siteId));

            var objNotificationEmail = _notificationRepository.GetNotificationEmailsAddress(notificationCategory, buildingId, floorId, siteId, EventId);
            return objNotificationEmail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <param name="attachments"></param>
        /// <param name="ccAddress"></param>
        /// <returns></returns>
        public bool SendPasswordResetMail(string to, string subject, string body, string from, List<string> attachments = null, string ccAddress = null)
        {
            return SendMail(to, subject, body, _appSettings.SenderMailFrom, attachments, ccAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorWorkOrder"></param>
        /// <param name="notificationCategory"></param>
        public bool SendRoundInsMail(WorkOrder floorWorkOrder, int notificationCategory)
        {
            string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"];
            string timeZone = _organizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(123456))?.Timezone;

            var userProfile = _usersRepository.GetUsersList(floorWorkOrder.CreatedBy);
            if (!string.IsNullOrEmpty(userProfile.Email))
                sender = userProfile.Email;

            string location = "";
            if (floorWorkOrder.FloorId != null)
            {
                var floorInfo = _floorRepository.GetFloorDescription(floorWorkOrder.FloorId.Value);
                if (floorInfo != null)
                    location = floorInfo.FloorName + " , " + floorInfo.Building.BuildingName;
                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                string ccAddress = obj.NotificationCCEmail;
                foreach (var user in floorWorkOrder.Resources)
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        try
                        {
                            string path = _commonProvider.GetContentRootPath("wwwroot");
                            FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/FloorInspection.html"));
                            string emailBody = File.ReadAllText(fileInfo.FullName);
                            emailBody = emailBody.Replace("{description}", floorWorkOrder.Description);
                            emailBody = emailBody.Replace("{timetoresolve}", floorWorkOrder.TimetoResolve.ToString());
                            emailBody = emailBody.Replace("{username}", userProfile.Name);
                            emailBody = emailBody.Replace("{date}",
                                $"{Conversion.GetUtcToLocal(floorWorkOrder.DateCreated.Value, timeZone):MMM dd, yyyy hh:mm tt}"); //   Utility.Conversion.GetUtcToLocal(floorWorkOrder.DateCreated.Value, timeZone).ToString("MM/dd/yyyy hh:mm tt")); //DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                            emailBody = emailBody.Replace("{location}", location);
                            if (floorInfo != null)
                                SendMail(user.Email, "CRx : " + location + " Adhoc Survey", emailBody, sender, null, ccAddress);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }
                    }
                }
            }

            return true;
        }

        public bool VendorNotify(Guid? invitationId, Vendors vendor, Organization org)
        {
            if (invitationId.HasValue)
            {
                try
                {
                    var url = _appSettings.WebUrlPath + "create/user/" + invitationId;
                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/VendorNotify.html"));
                    var emailBody = File.ReadAllText(fileInfo.FullName);
                    emailBody = emailBody.Replace("{orgName}", org.Name)
                            .Replace("{url}", url);

                    SendMail(vendor.Email, org.Name + " invited you to join the CRx", emailBody, null, null);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

            }

            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epDeatilId"></param>
        /// <param name="sampleDocument"></param>
        /// <returns></returns>
        public bool SampleDocument(int epDeatilId, Documents sampleDocument)
        {
            EPDetails epdetail = _epRepository.GetEpDescription(epDeatilId);
            if (epdetail == null)
                throw new ArgumentNullException(nameof(epDeatilId));

            List<DocumentType> attch = _documentTypeRepository.GetDocumentTypes(epDeatilId);
            List<string> attachmentsPath = new List<string>();
            bool status = true;
            try
            {
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SampleDocumentInfo.htm"));
                var emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{emailbody}", sampleDocument.Details)
                        .Replace("{TJCStandard}", epdetail.Standard.TJCStandard)
                        .Replace("{EPDescription}", epdetail.Description)
                        .Replace("{EPNumber}", epdetail.EPNumber);

                if (attch.Count > 0)
                    attachmentsPath = attch.Select(x => x.FileUrl).ToList();


                SendMail(sampleDocument.To, sampleDocument.Subject, emailBody, sampleDocument.Sender, attachmentsPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                status = false;
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRejectDoc"></param>
        /// <returns></returns>
        public bool ReplyDocument(Documents newRejectDoc)
        {
            bool status = true;
            try
            {
                newRejectDoc.ClientNo = Convert.ToInt32(123456);
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/RejectDocumentInfo.htm"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{emailbody}", newRejectDoc.Details);
                foreach (var item in newRejectDoc.Attachments)
                {
                    item.FilePath = _commonProvider.FilePath(item.FilePath);
                }
                var attachmentsPath = newRejectDoc.Attachments.Select(x => x.FilePath.ToLower()).ToList();
                SendMail(newRejectDoc.Sender, newRejectDoc.Subject, emailBody, null, attachmentsPath);

                newRejectDoc.Details = emailBody;
                newRejectDoc.MessageId = Guid.NewGuid().ToString();
                newRejectDoc.IsReplied = true;

                newRejectDoc.ParentDocumentRepoId = newRejectDoc.DocumentRepoId;
                int insertedid = _documentsRepository.InsertDocument(newRejectDoc);
                newRejectDoc.DocumentRepoId = insertedid;

                //insert into AttachmentTable

                _documentsRepository.UpdateAttachment(newRejectDoc, 1);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                status = false;
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public bool SendMailOnUserRegistration(UserProfile userProfile, string token,string createPasswordUrl)
        {
            var date = DateTime.Now;
            string ccAddress = string.Empty;
            if (userProfile.VendorId.HasValue)
            {
                SendVendorEmailOnUserRegistration(userProfile);
                //ccAddress = HCF.BAL.VendorRepository.GetVendor(userProfile.VendorId.Value).Email;
            }
            var type = "create";
            var webUrl = _appSettings.WebUrlPath;
            // var url = $"{webUrl}recover/{token}";
            var url = $"{webUrl}user/password/{type}/{token}";
            url = string.Format("<a href='{0}' title='Reset Your Account on HCF'>here</a>", url);
            string path = _commonProvider.GetContentRootPath("wwwroot");
            FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/UserRegistration.htm"));
            string emailBody = File.ReadAllText(fileInfo.FullName);
            emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
            emailBody = emailBody.Replace("{url}", createPasswordUrl);
            emailBody = emailBody.Replace("{FullName}", userProfile.Name);
            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
            SendMail(userProfile.UserName, "New UserRegistration ", emailBody, "", null, ccAddress);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public bool SendMailOnUserChangePassword(UserProfile userProfile)
        {
            var date = DateTime.Now;
            // var currentUser = DAL.Users.GetUserProfile(userProfile.CreatedBy);  //_userService.GetUsers().Single(m => m.UserId == userProfile.CreatedBy);
            string ccAddress = string.Empty;
            string path = _commonProvider.GetContentRootPath("wwwroot");
            var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/UserChangePassword.htm"));
            string emailBody = File.ReadAllText(fileInfo.FullName);
            emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
            emailBody = emailBody.Replace("{siteUrl}", _appSettings.WebUrlPath);
            emailBody = emailBody.Replace("{Password}", userProfile.Password);
            emailBody = emailBody.Replace("{FullName}", userProfile.Name);
            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
            SendMail(userProfile.UserName, "Change Password ", emailBody, "", null, ccAddress);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        private bool SendVendorEmailOnUserRegistration(UserProfile userProfile)
        {
            var date = DateTime.Now;
            string ccAddress = string.Empty;
            try
            {
                if (userProfile.VendorId != null)
                {
                    var objVendor = _vendorRepository.GetVendor(userProfile.VendorId.Value).FirstOrDefault();
                    if (objVendor != null)
                    {
                        string path = _commonProvider.GetContentRootPath("wwwroot");
                        var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/VendorEmailOnUserRegistration.htm"));
                        string emailBody = File.ReadAllText(fileInfo.FullName);
                        emailBody = emailBody.Replace("{Name}", objVendor.Name);
                        emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
                        emailBody = emailBody.Replace("{FullName}", userProfile.Name);
                        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
                        SendMail(objVendor.Email, "New User Registration ", emailBody, userProfile.UserName, null, ccAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRoundInsp"></param>
        /// <param name="notificationCategory"></param>
        /// <returns></returns>
        public bool SendRoundFailMail(TRounds newRoundInsp, int notificationCategory)
        {
            //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());

            //string toAddress = ccAddress;
            NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
            string toAddress = obj.NotificationEmail;
            string ccAddress = obj.NotificationCCEmail;
            string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"];
            if (!string.IsNullOrEmpty(toAddress))
            {
                if (newRoundInsp.RoundCatId != null)
                {
                    if (newRoundInsp.FloorId != null)
                    {
                        TRounds rounds = _roundRepository.GetRoundsHistory(newRoundInsp.RoundCatId.Value, newRoundInsp.FloorId.Value).FirstOrDefault(x => x.TRoundId == newRoundInsp.TRoundId);
                        // var date = DateTime.Now;
                        //var currentUser = Users.GetUsers().Single(m => m.UserId == newRoundInsp.CreatedBy);
                        string path = _commonProvider.GetContentRootPath("wwwroot");
                        var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/RoundStatusMail.html"));
                        string emailBody = File.ReadAllText(fileInfo.FullName);
                        if (rounds != null)
                        {
                            emailBody = emailBody.Replace("{roundcategories}", rounds.CategoryName);
                            emailBody = emailBody.Replace("{user}", rounds.UserProfile.Name);
                            emailBody = emailBody.Replace("{location}", $"{rounds.Floors.Building.BuildingName},{rounds.Floors.FloorName}");
                            emailBody = emailBody.Replace("{date}", $"{rounds.CreatedDate:MMM dd, yyyy}");
                            StringBuilder sb = new StringBuilder();
                            sb.Append(
                                "<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
                            sb.Append("<tr>");
                            sb.Append(
                                "<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:15%'><strong>Status</strong></td>");
                            sb.Append(
                                "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:60%'><strong>Round Step</strong></td>");
                            sb.Append(
                                "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'><strong>Comment</strong></td>");
                            sb.Append("</tr>");
                            foreach (var questionnaires in rounds.TRoundsQuestionnaires.Where(x => x.Status == 0))
                            {
                                sb.Append("<tr>");
                                sb.Append(
                                    "<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:15%'><p style='border-radius: 8px;background: #b24649;padding: 5px 14px;display: inline-block;line-height: 12px;font-size: 11px;color: #fff;'>" +
                                    "Non-Complaint" + "</p></td>");
                                sb.Append(
                                    "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:60%'>" +
                                    questionnaires.RoundsQuestionnaires.RoundStep + "</td>");
                                sb.Append(
                                    "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" +
                                    questionnaires.Comment + "</td>");
                                sb.Append("</tr>");
                            }
                            sb.Append("</table><br />");
                            emailBody = emailBody.Replace("{data}", sb.ToString());
                            emailBody = emailBody.Replace("{Signedby}", "<img src=" + _appSettings.WebUrlPath + newRoundInsp.DigitalSignature.FilePath.Replace("~/", "") + " alt='Signature'/>");
                        }

                        if (rounds != null)
                        {
                            string subject = $"CRx : Round Status - {$"{rounds.Floors.FloorName},{rounds.Floors.Building.BuildingName}"}";
                            SendMail(ccAddress, subject, emailBody, sender, null, ccAddress);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insStepsId"></param>
        /// <param name="notificationCategory"></param>
        public bool SendTimeToResolveToRAMail(int insStepsId, int notificationCategory)
        {
            try
            {
                StringBuilder stepsinfo = new StringBuilder();
                StringBuilder deviceinfo = new StringBuilder();
                TInspectionActivity activity = _inspectionActivityRepository.GetInspectionActivitybyTinsStepsId(insStepsId);
                foreach (var goal in activity.TInspectionDetail)
                {
                    var items = goal.InspectionSteps.Where(x => x.TInsStepsId == insStepsId).ToList();
                    if (items.Count > 0)
                    {
                        stepsinfo.Append("<tr>");
                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong>");
                        stepsinfo.Append(goal.MainGoal.Goal);
                        stepsinfo.Append("</strong></td></tr>");
                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.TInsStepsId == insStepsId))
                        {
                            stepsinfo.Append("<tr>");
                            stepsinfo.Append("<td width='29%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((BDO.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
                            stepsinfo.Append("<td width='20%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
                            stepsinfo.Append("<td width='10%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.DRTime + "</td>");
                            stepsinfo.Append("</tr>");
                        }
                        stepsinfo.Append("<br />");
                    }
                }
                if (activity.FloorAssetId.HasValue)
                {
                    deviceinfo.Append("<tr>");
                    deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Device Name:</strong>&nbsp;" + activity.TFloorAssets.Name + "</td>");
                    deviceinfo.Append("<td width='180' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo + "</td>");
                    //deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Inspector Name:</strong>&nbsp;" + activity.UserProfile.Name + "</td>");
                    deviceinfo.Append("</tr>");
                }
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/TimeToResolveEmail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
                emailBody = emailBody.Replace("{Errortext}", string.Format("Time To Resolve is closed for the following:"));
                List<string> attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
                                                             //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());

                //string toAddress = ccAddress;
                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                string toAddress = obj.NotificationEmail;
                string ccAddress = obj.NotificationCCEmail;
                string subject = $"CRx : Time To Resolve # {activity.TFloorAssets.DeviceNo}";
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendDueDateMail()
        {
            try
            {
                int DueWithInDays = Convert.ToInt32(_appSettings.DueWithInDays);
                var objEPDetails = _epRepository.GetDueWithInDaysEPs(DueWithInDays);
                var epsinfo = new StringBuilder();
                foreach (var item in objEPDetails)
                {
                    epsinfo.Append("<tr>");
                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.StandardEp + "</td>");
                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Description + "</td>");
                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.DueWithInDays + " Days </td>");
                    epsinfo.Append("</tr>");
                    epsinfo.Append("<tr>");
                    epsinfo.Append("<td colspan='3' style='padding:15px;'>");
                    epsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
                    foreach (Assets assets in item.Assets)
                    {
                        epsinfo.Append("<tr>");
                        epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + assets.Name + "</td>");
                        epsinfo.Append("<tr>");
                        epsinfo.Append("<td colspan='1' style='padding:15px;'>");
                        epsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
                        foreach (TFloorAssets tfloorAssets in assets.TFloorAssets)
                        {
                            epsinfo.Append("<tr>");
                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.Name + "</td>");
                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.DeviceNo + "</td>");
                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.DueWithInDays + " Days </td>");
                            epsinfo.Append("</tr>");
                        }
                        epsinfo.Append("</table>");
                        epsinfo.Append("</td></tr></td></tr>");
                    }
                    epsinfo.Append("</table>");
                    epsinfo.Append("</td></tr>");
                }
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendDueDateEmail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", string.Format("Some inspection are due with in {0} days:", DueWithInDays));
                emailBody = emailBody.Replace("{epsinfo}", epsinfo.ToString());
                var attachments = new List<string>();
                var sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
                var notificationTypeId = (int)Enum.Parse(typeof(BDO.Enums.NotificationEmailType), BDO.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
                var ccAddress = _notificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId == notificationTypeId)?.NotificationEmail; //DAL.NotificationRepository.GetNotificationEmail(emailType);
                // string ccAddress = OrganizationRepository.GetOrganization().Email;
                string toAddress = ccAddress;
                string subject = string.Format("CRx : Inspections Due WithIn {0} Days", DueWithInDays);
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, "");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendConfigurationMail(int notificationCategory)
        {
            try
            {
                var objAudit = _commonRepository.GetAuditConfiguration();
                var sb = new StringBuilder();
                foreach (var item in objAudit)
                {
                    sb.Append("<tr>");
                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.TableName + "</td>");
                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.FieldName + "</td>");
                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Type + "</td>");
                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Description + "</td>");
                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.OldValue + "</td>");
                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.NewValue + "</td>");
                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.UserName + "</td>");
                    sb.Append("</tr>");
                }
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendConfigurationEmail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", string.Format("Some configuration changes as following:"));
                emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", DateTime.Now));
                emailBody = emailBody.Replace("{info}", sb.ToString());
                List<string> attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
                //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
                //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);              
                //string toAddress = ccAddress;
                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                string toAddress = obj.NotificationEmail;
                string ccAddress = obj.NotificationCCEmail;
                string subject = string.Format("CRx : Configuration Changes");
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendWorkOrderCreatedMail(WorkOrder objWorkOrder, int notificationCategory)
        {
            try
            {

                string path = _commonProvider.GetContentRootPath("wwwroot");
                var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendWorkOrderCreatedMail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", $"WorkOrder # {objWorkOrder.WorkOrderNumber} created for the following:");
                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                emailBody = emailBody.Replace("{WoNumber}", objWorkOrder.WorkOrderNumber);
                emailBody = emailBody.Replace("{status}", objWorkOrder.StatusCode);
                emailBody = emailBody.Replace("{requesttype}", objWorkOrder.TypeCode);
                emailBody = emailBody.Replace("{WoDescription}", objWorkOrder.Description);
                emailBody = emailBody.Replace("{WoUrl}", _appSettings.WebUrlPath + "WorkOrder/UpdateWorkOrder?issueId=" + objWorkOrder.IssueId.ToString());

                var attachments = new List<string>();
                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                string toAddress = obj.NotificationEmail;
                string subject = $"CRx : WorkOrder # {objWorkOrder.WorkOrderNumber}";
                SendMail(toAddress, subject, emailBody, _appSettings.SenderMailFrom, attachments, "");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SendWorkOrderClosedMail(WorkOrder objWorkOrder, int notificationCategory)
        {
            try
            {

                string path = _commonProvider.GetContentRootPath("wwwroot");
                var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendWorkOrderClosedMail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", string.Format("WorkOrder # {0} updated for the following:", objWorkOrder.WorkOrderNumber));
                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                emailBody = emailBody.Replace("{WoNumber}", objWorkOrder.WorkOrderNumber);
                emailBody = emailBody.Replace("{status}", objWorkOrder.StatusCode);
                emailBody = emailBody.Replace("{requesttype}", objWorkOrder.TypeCode);
                emailBody = emailBody.Replace("{WoDescription}", objWorkOrder.Description);
                emailBody = emailBody.Replace("{WoUrl}", _appSettings.WebUrlPath + "WorkOrder/UpdateWorkOrder?issueId=" + objWorkOrder.IssueId);
                var attachments = new List<string>();
                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                string toAddress = obj.NotificationEmail;
                string subject = $"CRx : WorkOrder# {objWorkOrder.WorkOrderNumber}";
                SendMail(toAddress, subject, emailBody, _appSettings.SenderMailFrom, attachments, "");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permitNo"></param>
        /// <param name="permitType"></param>
        /// <param name="tpcraquestid"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool SendPCRANotifyEmail(int userId, string permitNo, string permitType, int? tpcraquestid, int? projectId)
        {
            var objQuestionTPCRA = new TPCRAQuestion();
            objQuestionTPCRA = _pCRARepository.GetQuestionTPCRA(projectId, tpcraquestid, null, null);
            var ircaLog = _constructionRepository.GetIcras(null).FirstOrDefault(x => x.PermitNo == permitNo);
            bool status = false;
            if (ircaLog != null && objQuestionTPCRA != null)
            {
                var date = DateTime.Now;
                string ccAddress = string.Empty;
                try
                {
                    var userProfile = _usersRepository.GetUsersList(userId);  //_userService.GetUserProfile(userId);
                    string subject = "CRA Notification";
                    if (!string.IsNullOrEmpty(userProfile.Email))
                    {


                        string path = _commonProvider.GetContentRootPath("wwwroot");
                        var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendICRANotifyEmail.html"));
                        string emailBody = File.ReadAllText(fileInfo.FullName);
                        emailBody = emailBody.Replace("ICRA", "CRA");
                        emailBody = emailBody.Replace("Permit", "CRA");
                        emailBody = emailBody.Replace("{FullName}", userProfile.FirstName);
                        emailBody = emailBody.Replace("{permitno}", objQuestionTPCRA.PCRANumber);
                        emailBody = emailBody.Replace("{requestedby}", userProfile.Name);
                        emailBody = emailBody.Replace("{projectname}", ircaLog.ProjectName);
                        emailBody = emailBody.Replace("{url}", $"{_appSettings.WebUrlPath}/pcra/addtpcra?projectId=" + objQuestionTPCRA.ProjectId + "&tpcraquestid=" + objQuestionTPCRA.TPCRAQuesId + "&mode=edit");
                        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");

                        SendMail(userProfile.Email, subject, emailBody, userProfile.UserName, null, ccAddress);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    status = false;
                }
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permitNo"></param>
        /// <param name="permitType"></param>
        /// <returns></returns>
        public bool SendICRANotifyEmail(int userId, string permitNo, string permitType,int version)
        {

            var ircaLog = _constructionRepository.GetIcras(null).FirstOrDefault(x => x.PermitNo == permitNo);
            bool status = true;
            if (ircaLog != null)
            {
                var date = DateTime.Now;
                var ccAddress = string.Empty;
                try
                {
                    var userProfile = _usersRepository.GetUsersList(userId); //_userService.GetUserProfile(userId);
                    var subject = "";
                    if (!string.IsNullOrEmpty(permitType))
                        subject = "ICRA Notification";

                    if (!string.IsNullOrEmpty(userProfile.Email))
                    {
                        string path = _commonProvider.GetContentRootPath("wwwroot");
                        var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendICRANotifyEmail.html"));
                        string emailBody = File.ReadAllText(fileInfo.FullName);
                        emailBody = emailBody.Replace("{FullName}", userProfile.FirstName);
                        emailBody = emailBody.Replace("{permitno}", ircaLog.PermitNo);
                        emailBody = emailBody.Replace("{requestedby}", userProfile.Name);
                        emailBody = emailBody.Replace("{projectname}", ircaLog.ProjectName);
                        emailBody = emailBody.Replace("{url}", $"{_appSettings.WebUrlPath.TrimEnd('/')}/icra/addinspectionicra?icraId={ircaLog.TicraId}&iseditable=True&version="+version);
                        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
                        SendMail(userProfile.Email, subject, emailBody, userProfile.UserName, null, ccAddress);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    status = false;
                }
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelData"></param>
        /// <returns></returns>
        public bool SendEmailonRiskareaApproved(ICRARiskArea modelData)
        {
            bool status = true;
            try
            {

                string path = _commonProvider.GetContentRootPath("wwwroot");
                var fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendEmailonRiskareaApproved.html"));
                //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/SendEmailonRiskareaApproved.html"));
                var emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                emailBody = emailBody.Replace("{Errortext}", $"A Risk area ( {modelData.Name} ) is created and pending for your approval Please review :");
                var attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"];
                int emailType = (int)Enum.Parse(typeof(BDO.Enums.NotificationEmailType), BDO.Enums.NotificationEmailType.CRxSystemUserEmail.ToString());
                string ccAddress = _notificationRepository.GetNotificationEmails().FirstOrDefault(x => x.NotificationCatId == emailType)?.NotificationEmail; //NotificationRepository.GetNotificationEmail(emailType);
                string toAddress = ccAddress;
                string subject = $"CRx : Need Action ( {modelData.Name} )";
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, "");
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="notificationCategory"></param>
        public bool SendInspectionFailMail(Inspection newItem, int notificationCategory)
        {
            string timeZone = _organizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(123456))?.Timezone;
            try
            {
                StringBuilder stepsinfo = new StringBuilder();
                StringBuilder deviceinfo = new StringBuilder();
                var assetsUsers = new List<UserProfile>();
                var activity = newItem.TInspectionActivity.FirstOrDefault();
                var maingoal = _mainGoalRepository.GetActivityMainGoals(activity.ActivityId); //need to improve methods
                                                                                              // List<Steps> steps = StepsRepository.GetSteps();
                newItem.EPDetails = _epRepository.GetEpShortDescription(newItem.EPDetailId);

                activity.UserProfile = _usersRepository.GetUsersList(activity.CreatedBy); //_userService.GetUserProfile(activity.CreatedBy);
                foreach (var goal in activity.TInspectionDetail)
                {
                    goal.MainGoal = maingoal.FirstOrDefault(x => x.MainGoalId == goal.MainGoalId);
                    var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
                    if (items.Count > 0)
                    {
                        stepsinfo.Append("<tr>");
                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong><b>");
                        stepsinfo.Append(goal.MainGoal.Goal);
                        stepsinfo.Append("</b></strong></td></tr>");
                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
                        {
                            inspectionStep.Steps = goal.MainGoal.Steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                            stepsinfo.Append("<tr>");
                            stepsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((BDO.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
                            stepsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
                            stepsinfo.Append("</tr>");
                        }
                        stepsinfo.Append("<br />");
                    }
                }
                if (activity.FloorAssetId.HasValue)
                {
                    string floorlocation = string.Empty;
                    string stopName = string.Empty;
                    activity.TFloorAssets = _floorAssetRepository.GetFloorAssetDescription(activity.FloorAssetId.Value);
                    if (activity.TFloorAssets.Floor != null) { floorlocation = activity.TFloorAssets.Floor.FloorLocation; }
                    if (activity.TFloorAssets.Stop != null) { stopName = $"{activity.TFloorAssets.Stop.StopName} ({activity.TFloorAssets.Stop.StopCode})"; }
                    assetsUsers.ToList().AddRange(activity.TFloorAssets.Assets.Users);
                    deviceinfo.Append("<tr>");
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Name:</strong>&nbsp;" + activity.TFloorAssets.Name);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + floorlocation);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>NearBy:</strong>&nbsp;" + activity.TFloorAssets.NearBy);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Stop Name:</strong>&nbsp;" + stopName + "</td>");
                    deviceinfo.Append("</tr>");
                }
                var objTInspectionFiles = new List<TInspectionFiles>();
                objTInspectionFiles = _transactionRepository.GetInspectionFiles(activity.ActivityId).ToList();
                List<string> attachments = new List<string>();
                //foreach (var item in objTInspectionFiles)
                //{
                //    if (!string.IsNullOrEmpty(item.FilePath))
                //        attachments.Add(HostingEnvironment.MapPath(item.FilePath));
                //}
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/StepsFail.html"));

                //FileInfo fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/StepsFail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                if (newItem.EPDetails != null)
                {
                    emailBody = emailBody.Replace("{standardName}", newItem.EPDetails != null ? newItem.EPDetails.StandardEp : "Route Base Inspection (No EP link with this assets)");
                    emailBody = emailBody.Replace("{EpNUmber}", newItem.EPDetails.EPNumber);
                    emailBody = emailBody.Replace("{EpDescription}", newItem.EPDetails != null ? newItem.EPDetails.Description : string.Empty);
                }
                else { emailBody = emailBody.Replace("{standardName}", string.Empty); emailBody = emailBody.Replace("{EpDescription}", string.Empty); }
                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                emailBody = emailBody.Replace("{inspector}", activity.UserProfile.Name);
                emailBody = emailBody.Replace("{inspectorName}", activity.UserProfile.Name);
                emailBody = emailBody.Replace("{inspectionDate}", Utility.Conversion.GetUtcToLocal(activity.ActivityInspectionDate.Value, timeZone).ToString("MMM dd, yyyy hh:mm tt "));  //activity.ActivityInspectionDate.Value.ToString("dd MMM yyyy hh:mm tt "));
                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
                string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"];
                                                             //int emailType = (int)Enum.Parse(typeof(Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
                                                             //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
                                                             //string toAddress = ccAddress;




                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
                string toAddress = obj.NotificationEmail;

                if (assetsUsers.Count > 0)
                    toAddress = toAddress + "," + string.Join(",", assetsUsers.Select(i => i.Email));


                string ccAddress = obj.NotificationCCEmail;
                string subject = $"CRx: Inspection Failed Asset # {activity.TFloorAssets.DeviceNo} {(newItem.EPDetails != null ? ", " + newItem.EPDetails.StandardEp : string.Empty)}";
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;

        }


        #region ILSM Emails 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="failEpDetail"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        //public  bool SendIlsmFailMail(UserProfile currentUser, EPDetails failEpDetail, TInspectionActivity activity)
        //{
        //    TFloorAssets failAssets = null;
        //    if (activity != null && (activity.ActivityType == 2))
        //        failAssets = activity.TFloorAssets;

        //    bool status = true;
        //    string standardName = failEpDetail.Standard.TJCStandard;
        //    string epNUmber = failEpDetail.EPNumber;
        //    string epDescription = failEpDetail.Description;
        //    if (activity != null)
        //    {
        //        string inspectorName = activity.UserProfile.Name;
        //        DateTime date = DateTime.Now;
        //        StringBuilder sb = new StringBuilder();
        //        // Inspection list3 = failEPDetail.Inspection;//.FirstOrDefault();
        //        //Guid activityId = activity.ActivityId;
        //        string comment = activity.Comment;//string.Join(",", list3.TInspectionFiles.Where(x => x.Comment != null || x.Comment != string.Empty).Select(x => x.Comment.ToString()).ToArray());
        //        var userName = activity.UserProfile.Name;
        //        foreach (var goal in activity.TInspectionDetail)
        //        {
        //            date = DateTime.UtcNow;//goal.CreatedDate.Value;
        //            var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
        //            if (items.Count > 0)
        //            {
        //                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;>");
        //                sb.Append("<tr>");
        //                sb.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong>");
        //                sb.Append(goal.MainGoal.Goal);
        //                sb.Append("</strong></td></tr>");
        //                foreach (InspectionSteps inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
        //                {
        //                    sb.Append("<tr>");
        //                    sb.Append("<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
        //                    sb.Append("<td align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + (Utility.Enums.Status)inspectionStep.Status + "</td>");
        //                    sb.Append("<td align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
        //                    sb.Append("</tr>");
        //                }
        //                sb.Append("</table><br />");
        //            }
        //        }

        //        StringBuilder deviceinfo = new StringBuilder();
        //        //deviceinfo.Append("<table>");

        //        string bName = "";
        //        string fName = "";
        //        string aName = "";
        //        string nearBy = "";
        //        string locationName = "";
        //        if (failAssets != null)
        //        {

        //            aName = failAssets.Name;
        //            if (failAssets.Floor != null)
        //            {
        //                bName = failAssets.Floor.Building.BuildingName;
        //                fName = failAssets.Floor.FloorName;
        //                nearBy = failAssets.NearBy;
        //                if (bName == null && fName == null && nearBy == null)
        //                {
        //                    locationName = "N/A";
        //                }
        //                else
        //                {
        //                    locationName = bName + " " + fName + " " + nearBy;
        //                }
        //            }
        //            //byte[] imgBytes = Utility.Common.GenerateBarCode(failAssets.SerialNo);
        //            //string imgString = Convert.ToBase64String(imgBytes);
        //            //string htmlImage = String.Format("<img src=\"data:image/Bmp;base64,{0}\" style='height:90px; width:250px; margin: 0 0 -12px; 20px;'>", imgString);
        //            deviceinfo.Append("<tr>");
        //            deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Device Name:</strong>&nbsp;" + failAssets.Name + "</td>");
        //            //deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Asset #:</strong>&nbsp;" + failAssets.DeviceNo + "</td>");
        //            deviceinfo.Append("<td width='180' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;'><strong>Asset #:</strong></td>");
        //            //deviceinfo.Append("<td width='269' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'>" + htmlImage + "</span></td>");
        //            deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + locationName + "</td>");
        //            deviceinfo.Append("</tr>");
        //        }
        //        var objTInspectionFiles = DAL.Transaction.GetInspectionFiles(activity.ActivityId).ToList();
        //        //StringBuilder Tfileinfo = new StringBuilder();
        //        //Tfileinfo.Append("<tr>");
        //        List<string> attachments = new List<string>();
        //        foreach (var item in objTInspectionFiles)
        //        {
        //            if (!string.IsNullOrEmpty(item.FilePath))
        //            {
        //                attachments.Add(HostingEnvironment.MapPath(item.FilePath));
        //            }
        //            //Tfileinfo.Append("<td><img src=" + item.FilePath + " alt='file' style='width:100px;height:200px;'></td>");
        //            //Tfileinfo.Append("<td><img src=" + WebConfigurationManager._appSettings["BasePath"] + item.FilePath.Replace("~/", "") + " alt='file' style='width:100px;height:200px;'></td>");
        //        }
        //        //Tfileinfo.Append("</tr>");
        //        //deviceinfo.Append("</table>");
        //        try
        //        {
        //            var fileInfo = new FileInfo(string.Format("ApplicationPhysicalPath" + "{0}", @"Templates/FailIlsmMail.htm"));
        //            string emailBody = File.ReadAllText(fileInfo.FullName);
        //            emailBody = emailBody.Replace("{standardName}", standardName);
        //            emailBody = emailBody.Replace("{EpNUmber}", epNUmber);
        //            emailBody = emailBody.Replace("{EpDescription}", epDescription);
        //            emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
        //            emailBody = emailBody.Replace("{userName}", userName);
        //            emailBody = emailBody.Replace("{failSteps}", sb.ToString());
        //            emailBody = emailBody.Replace("{inspector}", inspectorName);
        //            emailBody = emailBody.Replace("{Comment}", "<strong>Comment </strong>: " + comment);

        //            if (failAssets != null && failAssets.FloorAssetsId > 0)
        //                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
        //            else
        //                emailBody = emailBody.Replace("{deviceinfo}", "");
        //            string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
        //            int emailType = (int)Enum.Parse(typeof(Enums.NotificationEmailType), Enums.NotificationEmailType.DeficiencyNotifications.ToString());
        //            string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
        //            // string ccAddress = OrganizationRepository.GetOrganization().Email;
        //            string toAddress = failEpDetail.AssigneeUser != null ? failEpDetail.AssigneeUser.Email : ccAddress;

        //            string subject = $"CRx : Assets Fail {aName} {bName} {fName}";
        //            string ilsmMailFrom = currentUser.Email ?? sender;
        //            SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.LogError(ex);
        //            status = false;
        //        }
        //    }
        //    return status;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsm"></param>
        /// <param name="notificationCategory"></param>
        /// <param name="notificationMapping"></param>
        public bool SendIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping)
        {
            if (notificationMapping != null)
            {
                try
                {
                    StringBuilder assetsinfo = new StringBuilder();
                    StringBuilder triggereps = new StringBuilder();
                    foreach (var activity in tilsm.SourceInspection.TInspectionActivity)
                    {
                        assetsinfo.Append("<tr>");
                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.TFloorAssets.Name + "</td>");
                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.TFloorAssets.DeviceNo + "</td>");
                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.UserProfile.Name + "</td>");
                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.ActivityInspectionDate.Value.ToString("MMM dd, yyyy") + "</td>");
                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.Comment + "</td>");
                        assetsinfo.Append("</tr>");
                        assetsinfo.Append("<tr>");
                        assetsinfo.Append("<td colspan='5' style='padding:15px;'>");
                        assetsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
                        foreach (var goal in activity.TInspectionDetail)
                        {
                            var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
                            if (items.Count > 0)
                            {
                                foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
                                {
                                    assetsinfo.Append("<tr>");
                                    assetsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
                                    assetsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((BDO.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
                                    assetsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
                                    assetsinfo.Append("</tr>");
                                }
                            }
                        }
                        assetsinfo.Append("</table>");
                        assetsinfo.Append("</td></tr>");
                    }
                    if (tilsm.TIlsmEP != null)
                    {
                        foreach (var TriggerEp in tilsm.TIlsmEP)
                        {
                            triggereps.Append("<tr>");
                            triggereps.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.StandardEP + "</td>");
                            triggereps.Append("<td width='931' style='color: #333; font-family: arial; font-size: 12px; padding: 5px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.Description + "</td>");
                            triggereps.Append("</tr>");
                        }
                    }

                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/IlsmMail.html"));

                    // var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/IlsmMail.htm"));
                    string emailBody = File.ReadAllText(fileInfo.FullName);
                    emailBody = emailBody.Replace("{standardName}", tilsm.SourceInspection.EPDetails.StandardEp);
                    emailBody = emailBody.Replace("{EpNUmber}", tilsm.SourceInspection.EPDetails.EPNumber);
                    emailBody = emailBody.Replace("{EpDescription}", tilsm.SourceInspection.EPDetails.Description);
                    emailBody = emailBody.Replace("{TriggerEPs}", triggereps.ToString());
                    emailBody = emailBody.Replace("{date}", $"{tilsm.llsmDate:MMM dd, yyyy}");
                    emailBody = emailBody.Replace("{inspector}", tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().UserProfile.Name);
                    emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
                    emailBody = emailBody.Replace("{Errortext}", $"ILSM Incident #: {tilsm.IncidentId} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Assets.Name}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.FloorName}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.Building.BuildingName} created for the following:");
                    List<string> attachments = new List<string>();
                    //string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";

                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
                    string toAddress = obj.NotificationEmail;
                    string ccAddress = obj.NotificationCCEmail;
                    string subject = $"CRx : ILSM Incident #: {tilsm.IncidentId} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault()?.TFloorAssets.Assets.Name} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.FloorName}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.Building.BuildingName}";
                    // string ilsmMailFrom = sender;
                    SendMail(toAddress, subject, emailBody, _appSettings.SenderMailFrom, attachments, ccAddress);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsm"></param>
        /// <param name="notificationCategory"></param>
        /// <param name="notificationMapping"></param>
        public bool SendManualIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping)
        {
            if (notificationMapping != null)
            {
                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
                string subjectlocation = (tilsm.Buildings.Count > 0) ? string.Join(",", tilsm.Buildings.Select(x => x.BuildingName)) : "NA";
                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
                try
                {
                    StringBuilder assetsinfo = new StringBuilder();
                    StringBuilder triggereps = new StringBuilder();
                    if (tilsm.TIlsmEP != null)
                    {
                        foreach (var TriggerEp in tilsm.TIlsmEP)
                        {
                            triggereps.Append("<tr>");
                            triggereps.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.StandardEP + "</td>");
                            triggereps.Append("<td width='931' style='color: #333; font-family: arial; font-size: 12px; padding: 5px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.Description + "</td>");
                            triggereps.Append("</tr>");
                        }
                    }
                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/ManualIlsmMail.htm"));

                    //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"/Templates/ManualIlsmMail.htm"));
                    string emailBody = File.ReadAllText(fileInfo.FullName);
                    emailBody = emailBody.Replace("{name}", $"{tilsm.Name} (on {tilsm.llsmDate:MMM dd, yyyy})");
                    emailBody = emailBody.Replace("{reason}", tilsm.Notes);
                    emailBody = emailBody.Replace("{location}", location);
                    emailBody = emailBody.Replace("{TriggerEPs}", triggereps.ToString());
                    //emailBody = emailBody.Replace("{date}", $"{tilsm.llsmDate:MMM dd, yyyy}");
                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
                    //emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
                    emailBody = emailBody.Replace("{ilsmtext}", $"ILSM Incident #: {tilsm.IncidentId} - {tilsm.Name} created for the following:");
                    List<string> attachments = new List<string>();
                    //string sender = _appSettings.SenderMailFrom; //System.Configuration.ConfigurationManager._appSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";

                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
                    string toAddress = obj.NotificationEmail;
                    string ccAddress = obj.NotificationCCEmail;
                    string subject = $"CRx : ILSM Incident #: {tilsm.IncidentId} - {tilsm.Name} - Location ({subjectlocation}) ";
                    // string ilsmMailFrom = sender;
                    SendMail(toAddress, subject, emailBody, _appSettings.SenderMailFrom, attachments, ccAddress);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsmId"></param>
        /// <param name="notificationCategory"></param>
        public bool SendIlsmCloseMail(int tilsmId, int notificationCategory)
        {
            TIlsm tilsm = _ilsmRepository.GetIlsmInspection(tilsmId);//IlsmRepository.GetIlsm().FirstOrDefault(x => x.TIlsmId == tilsmId);
            try
            {
                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
                string subjectlocation = (tilsm.Buildings.Count > 0) ? string.Join(",", tilsm.Buildings.Select(x => x.BuildingName)) : "NA";
                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
                StringBuilder assetsinfo = new StringBuilder();
                if (tilsm != null)
                {
                    if (tilsm.TIlsmfloorAssets.Count > 0)
                    {
                        assetsinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Assets Details:</h4>");
                        assetsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                        assetsinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Device Name</strong></td>" +
                            "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Asset #</strong></td>" +
                            "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Inspector Name </strong></td>" +
                            "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Inspection Date </strong></td>" +
                            "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comment </strong></td>" +
                            "</tr></thead>");
                        assetsinfo.Append("<tbody>");
                        foreach (TIlsmfloorAssets floorAssets in tilsm.TIlsmfloorAssets)
                        {
                            assetsinfo.Append("<tr>");
                            assetsinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                floorAssets.TInspectionActivity.TFloorAssets.Name + "</td>");
                            assetsinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                floorAssets.TInspectionActivity.TFloorAssets.DeviceNo + "</td>");
                            assetsinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                floorAssets.TInspectionActivity.UserProfile.Name + "</td>");
                            assetsinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                floorAssets.TInspectionActivity.ActivityInspectionDate.Value.ToString("MMM dd, yyyy") +
                                "</td>");
                            assetsinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                floorAssets.TInspectionActivity.Comment + "</td>");
                            assetsinfo.Append("</tr>");
                        }
                        assetsinfo.Append("</tbody>");
                        assetsinfo.Append("</table><br />");
                    }
                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/IlsmCloseMail.html"));

                    //FileInfo fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/IlsmCloseMail.html"));
                    string emailBody = File.ReadAllText(fileInfo.FullName);
                    emailBody = emailBody.Replace("{name}", $"{tilsm.Name} (on {tilsm.CreatedDate:MMM dd, yyyy})");
                    emailBody = emailBody.Replace("{reason}", (!string.IsNullOrEmpty(tilsm.Notes) ? tilsm.Notes : "NA"));
                    emailBody = emailBody.Replace("{location}", location);
                    //emailBody = emailBody.Replace("{date}", $"{tilsm.CreatedDate:MMM dd, yyyy}");
                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
                    emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
                    emailBody = emailBody.Replace("{Errortext}",
                        //string.Format("ILSM # {0} closed for the following:", tilsm.IncidentId)
                        $"CRx Assisted ILSM {tilsm.IncidentId}-{tilsm.Name}-{subjectlocation} has been closed and filed in the ILSM Binder for reference. Attached to this e-mail is a finalized report"
                        );
                    List<string> attachments = new List<string>();
                    //if (!string.IsNullOrEmpty(tilsm.FilePath))
                    //    attachments.Add(Common.FilePath(tilsm.FilePath));
                    string sender = _appSettings.SenderMailFrom;
                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnClosed));
                    string toAddress = obj.NotificationEmail;
                    string ccAddress = obj.NotificationCCEmail;
                    string subject = $"CRx Assisted ILSM Closed:- {tilsm.IncidentId}-{tilsm.Name}-{location}";
                    var ilsmMailFrom = sender;
                    SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilsmId"></param>
        /// <param name="notificationCategory"></param>
        public bool SendIlsmReOpenMail(int tilsmId, int notificationCategory)
        {
            TIlsm tilsm = _ilsmRepository.GetIlsmInspection(tilsmId);//IlsmRepository.GetIlsm().FirstOrDefault(x => x.TIlsmId == tilsmId);
            try
            {
                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
                if (tilsm != null)
                {

                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/IlsmReOpenMail.html"));
                    //FileInfo fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}",
                    //    @"Templates/IlsmReOpenMail.html"));
                    string emailBody = File.ReadAllText(fileInfo.FullName);
                    emailBody = emailBody.Replace("{name}", tilsm.Name);
                    emailBody = emailBody.Replace("{reason}", (!string.IsNullOrEmpty(tilsm.Notes) ? tilsm.Notes : "NA"));
                    emailBody = emailBody.Replace("{location}", location);
                    emailBody = emailBody.Replace("{date}", $"{tilsm.CreatedDate:MMM dd, yyyy}");
                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
                    emailBody = emailBody.Replace("{Errortext}",
                        string.Format("ILSM # {0} Reopen for the following:", tilsm.IncidentId));
                    List<string> attachments = new List<string>();
                    string sender = _appSettings.SenderMailFrom;
                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
                    string toAddress = obj.NotificationEmail;
                    string ccAddress = obj.NotificationCCEmail;
                    string subject = $"CRx : ILSM Reopened # {tilsm.IncidentId}";
                    var ilsmMailFrom = sender;
                    SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildings"></param>
        /// <returns></returns>
        private string getlocation(List<Buildings> buildings)
        {
            string location = string.Empty;
            foreach (var build in buildings)
            {
                location = location + build.BuildingName;
                if (build.Floor.Count > 0)
                {
                    location = location + $": ({string.Join(",", build.Floor.Select(x => x.FloorName))})";
                }
                location = location + (buildings.Count > 1 ? $"," : "");
            }
            return location;
        }


        #endregion

        #region Tips Emails

        private bool SendTipsEmail(Tips newItem, string mode, int notificationCategory)
        {
            try
            {
                if (newItem.CreatedBy != null)
                {
                    var ownerUser = _usersRepository.GetUsersList(newItem.CreatedBy.Value);// _userService.GetUserProfile(newItem.CreatedBy.Value);
                    string path = _commonProvider.GetContentRootPath("wwwroot");
                    var emailBody = File.ReadAllText(string.Format(path + "{0}", @"/Templates/TipCreated.html"));
                    //FileInfo emailBody = new FileInfo(string.Format(_commonProvider.GetContentRootPath("Templates/TipCreated.html")));
                    emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
                    emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                    emailBody = emailBody.Replace("{Errortext}", TipsText(newItem, mode));
                    string sender = _appSettings.SenderMailFrom;

                    var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
                    var toAddress = obj.NotificationEmail;

                    if (ownerUser.UserId > 0)
                        toAddress = toAddress + "," + string.Join(",", ownerUser.Email);
                    var ccAddress = obj.NotificationCCEmail;
                    var subject = TipsSubject(mode);
                    SendMail(toAddress, subject, emailBody, sender, null, ccAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        public bool SendTipsFailMail(Tips newItem, int notificationCategory)
        {
            return SendTipsEmail(newItem, "SendTipsFailMail", notificationCategory);
        }
        public bool SendTipsPassMail(Tips newItem, int notificationCategory)
        {
            return SendTipsEmail(newItem, "SendTipsPassMail", notificationCategory);
        }
        public bool SendTipsCreatedMail(Tips newItem, int notificationCategory)
        {
            return SendTipsEmail(newItem, "SendTipsCreatedMail", notificationCategory);

        }
        public bool SendTipsUpdatedMail(Tips newItem, int notificationCategory)
        {
            return SendTipsEmail(newItem, "SendTipsUpdatedMail", notificationCategory);
        }

        private string TipsText(Tips newItem, string mode)
        {
            string text = string.Empty;
            switch (mode)
            {
                case "SendTipsFailMail":
                    text = string.Format("Tip {0} is rejected.", newItem.Title);
                    break;
                case "SendTipsPassMail":
                    text = string.Format("Tip {0} is approved.", newItem.Title);
                    break;
                case "SendTipsCreatedMail":
                    text = string.Format("A Tip having Title:{0} is created and pending for approval", newItem.Title);
                    break;
                case "SendTipsUpdatedMail":
                    text = string.Format("A Tip have Title: {0} is updated and pending for approval", newItem.Title);
                    break;
            }
            return text;
        }

        private string TipsSubject(string mode)
        {
            string subject = string.Empty;
            switch (mode)
            {
                case "SendTipsFailMail":
                    subject = "CRx: Tip Rejected";
                    break;
                case "SendTipsPassMail":
                    subject = "CRx: Tip Approved";
                    break;
                case "SendTipsCreatedMail":
                    subject = "CRx: Tip Created";
                    break;
                case "SendTipsUpdatedMail":
                    subject = "CRx: Tip Updated";
                    break;
            }
            return subject;
        }

        #endregion

        #region Permit Emails

        public bool SendPermitCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId)
        {

            bool status = false;
            try
            {
                var toAddress = string.Empty;
                var ccAddress = string.Empty;
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendPermitCreatedMail.html"));

                //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/SendPermitCreatedMail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{template}", GetPermitTemplate(objPermitEmailModel));

                string emailvendorBody = File.ReadAllText(fileInfo.FullName);
                emailvendorBody = emailvendorBody.Replace("{template}", GetVendorPermitTemplate(objPermitEmailModel));

                var sender = _appSettings.SenderMailFrom;
                var obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId, objPermitEmailModel.EventId);
                if (obj != null)
                {
                    toAddress = !string.IsNullOrEmpty(obj.NotificationEmail) ? obj.NotificationEmail : string.Empty;
                    ccAddress = !string.IsNullOrEmpty(obj.NotificationCCEmail) ? obj.NotificationCCEmail : string.Empty;
                }
                if (objPermitEmailModel.SendMailToApprover.HasValue && objPermitEmailModel.SendMailToApprover.Value)
                {
                    obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId, Convert.ToInt32(BDO.Enums.NotificationEvent.OnApproval));
                    if (obj != null)
                    {
                        toAddress = toAddress + "," + obj.NotificationEmail;
                        ccAddress = ccAddress + "," + obj.NotificationCCEmail;
                    }
                }
                if (objPermitEmailModel.SendMailToNextPhase.HasValue && objPermitEmailModel.SendMailToNextPhase.Value)
                {
                    
                        if (!string.IsNullOrEmpty(objPermitEmailModel.AdditionalNextLevelToEmails))
                            toAddress = toAddress + "," + objPermitEmailModel.AdditionalNextLevelToEmails;

                        if (!string.IsNullOrEmpty(objPermitEmailModel.AdditionalNextLevelCCEmails))
                            ccAddress = ccAddress + "," + objPermitEmailModel.AdditionalNextLevelCCEmails;
                    
                }
                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status approve/reject/hold for same user in the notification matrix will not get the mail twice
                {
                }
                else
                {
                    if (objPermitEmailModel.ApprovalStatus != 6)
                    {
                        if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
                            toAddress = toAddress + "," + objPermitEmailModel.RequesterEmail;
                    }
                    if (objPermitEmailModel.EventId == 8)
                    {
                        if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
                            toAddress = toAddress + "," + objPermitEmailModel.RequesterEmail;

                        if (!string.IsNullOrEmpty(toAddress))
                            toAddress = string.Join(",", toAddress.Split(',').Select(o => o.Trim()).ToList().Distinct());
                    }
                }
                string subject = GetPermitSubject(objPermitEmailModel); //$"{objPermitEmailModel.PermitType} # {objPermitEmailModel.PermitNo} ";
                string mailFrom = sender;

                SendMail(toAddress, subject, ccAddress, emailBody, mailFrom, fileBytes, filename);
                if (objPermitEmailModel.ApprovalStatus != 6)
                {
                    if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status request/create for vendor user 
                    {
                        //Send to Vendor 
                        toAddress = "";
                        if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
                            toAddress = objPermitEmailModel.RequesterEmail;

                        ccAddress = "";

                        if (!string.IsNullOrEmpty(toAddress))
                            SendMail(toAddress, subject, ccAddress, emailvendorBody, mailFrom, fileBytes, filename);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return status;
        }
        public bool SendConstruictionCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId)
        {

            bool status = false;
            try
            {
                var toAddress = string.Empty;
                var ccAddress = string.Empty;
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendPermitCreatedMail.html"));

                //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/SendPermitCreatedMail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{template}", GetPermitTemplate(objPermitEmailModel));

                string emailvendorBody = File.ReadAllText(fileInfo.FullName);
                emailvendorBody = emailvendorBody.Replace("{template}", GetVendorPermitTemplate(objPermitEmailModel));

                var sender = _appSettings.SenderMailFrom;
                var obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId);
                if (obj != null)
                {
                    toAddress = obj.NotificationEmail;
                    ccAddress = obj.NotificationCCEmail;
                }

                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status approve/reject/hold for same user in the notification matrix will not get the mail twice
                {
                }
                else
                {
                    if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
                        toAddress = toAddress + "," + objPermitEmailModel.RequesterEmail;
                }
                string subject = GetPermitSubject(objPermitEmailModel); //$"{objPermitEmailModel.PermitType} # {objPermitEmailModel.PermitNo} ";
                string mailFrom = sender;
                SendMail(toAddress, subject, ccAddress, emailBody, mailFrom, fileBytes, filename);

                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status request/create for vendor user 
                {
                    //Send to Vendor 
                    toAddress = "";
                    if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
                        toAddress = objPermitEmailModel.RequesterEmail;

                    ccAddress = "";

                    if (!string.IsNullOrEmpty(toAddress))
                        SendMail(toAddress, subject, ccAddress, emailvendorBody, mailFrom, fileBytes, filename);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return status;
        }
        private string GetPermitTemplate(PermitEmailModel objPermitEmailModel)
        {
            string templatetext;

            string projectDetails = string.Empty;
            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
                projectDetails = $" for {objPermitEmailModel.ProjectName}";

            if (objPermitEmailModel.ApprovalStatus == 1) // Approved 
            {
                templatetext =
                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been approved by  " +
                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the approved permit."; // {objPermitEmailModel.PermitType}.";

            }
            else if (objPermitEmailModel.ApprovalStatus == 0) // Rejected
            {

                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} has been rejected by  " +
                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
                               $"You can update and resubmit the permit if required. <br /> Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx " +
                $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";
                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";

            }
            else if (objPermitEmailModel.ApprovalStatus == 3) // Pending/On hold
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} put on hold by " +
                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.  <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
                               $"Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
                               $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";
                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";

            }
            else if (objPermitEmailModel.ApprovalStatus == 4) // Created
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} created by " +
                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent for furthur process <br /> " +
                                 $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";
                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";

            }
            else if (objPermitEmailModel.ApprovalStatus == 5) // Closed 
            {
                templatetext =
                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been closed by  " +
                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the closed permit."; // {objPermitEmailModel.PermitType}.";

            }
            else if (objPermitEmailModel.ApprovalStatus == 6) // Reviewed 
            {
                templatetext =
                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been reviewed by  " +
                        $"{objPermitEmailModel.Reviewer} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the reviewed permit."; // {objPermitEmailModel.PermitType}.";

            }
            else // Requested
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} requested by " +
                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} is awaiting your approval <br />" +
                               //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
                               $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";

            }
            return templatetext;
        }
        private string GetVendorPermitTemplate(PermitEmailModel objPermitEmailModel)
        {
            string templatetext;

            string projectDetails = string.Empty;
            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
                projectDetails = $" for {objPermitEmailModel.ProjectName}";

            if (objPermitEmailModel.ApprovalStatus == 1) // Approved 
            {
                templatetext =
                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been approved by  " +
                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the approved permit."; // {objPermitEmailModel.PermitType}.";

            }
            else if (objPermitEmailModel.ApprovalStatus == 0) // Rejected
            {

                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} has been rejected by  " +
                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
                               $"You can update and resubmit the permit if required. <br /> Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
                                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";
                                $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";

            }
            else if (objPermitEmailModel.ApprovalStatus == 3) // Pending/On hold
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} put on hold by " +
                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.  <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
                               $"Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
                                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";
                                $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";

            }
            else if (objPermitEmailModel.ApprovalStatus == 4) // Created
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} created by " +
                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent for furthur process <br /> " +
                                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
                                $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";

            }
            else if (objPermitEmailModel.ApprovalStatus == 5) // closed
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} closed by " +
                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> ";

            }
            else if (objPermitEmailModel.ApprovalStatus == 6) // reviewed
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} reviewed by " +
                               $"{objPermitEmailModel.Reviewer} of {objPermitEmailModel.Company}. <br /> ";

            }
            else // Requested
            {
                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} requested by " +
                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent in for approval. <br />" +
                                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
                                $"(<a href='{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}'>{_appSettings.WebUrlPath + objPermitEmailModel.PermitUrl}</a>)";

            }
            return templatetext;
        }
        private string GetPermitSubject(PermitEmailModel objPermitEmailModel)
        {
            string projectDetails = string.Empty;
            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
                projectDetails = $" {objPermitEmailModel.ProjectName}";

            string subject;
            switch (objPermitEmailModel.ApprovalStatus)
            {
                // Approved 
                case 1:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(APPROVED),"} {projectDetails} approved by {objPermitEmailModel.Approver}";
                    break;
                // Rejected 
                case 0:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo}{"(REJECTED),"} {projectDetails} rejected by {objPermitEmailModel.Approver}";
                    break;
                // Pending/On hold
                case 3:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(ON HOLD),"} {projectDetails} On hold by {objPermitEmailModel.Approver}";
                    break;
                //Created
                case 4:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CREATED),"} {projectDetails} created by {objPermitEmailModel.Requestor}";
                    break;
                //closed
                case 5:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CLOSED),"} {projectDetails} closed by {objPermitEmailModel.Approver}";
                    break;
                //Reviewed
                case 6:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(REVIEWED),"} {projectDetails} reviewed by {objPermitEmailModel.Reviewer}";
                    break;
                // Requested
                default:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {(string.IsNullOrEmpty(objPermitEmailModel.IsRequestEdited) ? "(REQUESTED)," : "(UPDATED),")} {projectDetails} requested by {objPermitEmailModel.Requestor}";
                    break;
            }
            return subject;
        }
        private string GetConstructionubject(PermitEmailModel objPermitEmailModel)
        {
            string projectDetails = string.Empty;
            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
                projectDetails = $" {objPermitEmailModel.ProjectName}";

            string subject;
            switch (objPermitEmailModel.ApprovalStatus)
            {
                // Approved 
                case 1:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(APPROVED),"} {projectDetails} approved by {objPermitEmailModel.Approver}";
                    break;
                // Rejected 
                case 0:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo}{"(REJECTED),"} {projectDetails} rejected by {objPermitEmailModel.Approver}";
                    break;
                // Pending/On hold
                case 3:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(ON HOLD),"} {projectDetails} On hold by {objPermitEmailModel.Approver}";
                    break;
                //Created
                case 4:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CREATED),"} {projectDetails} created by {objPermitEmailModel.Requestor}";
                    break;
                // Requested
                default:
                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(REQUESTED),"} {projectDetails} requested by {objPermitEmailModel.Requestor}";
                    break;
            }
            return subject;
        }
        #endregion

        #region Fire drill Mail
        public bool SendFireDrillCreatedMail(TExercises exercises, int notificationCategory)
        {
            return SendFireDrillEmail(exercises, "SendFiredrillCreatedMail", notificationCategory, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
        }

        public void SendFireDrillReminderMail(TExercises exercises, int notificationCategory)
        {
            SendFireDrillEmail(exercises, "SendFireDrillReminderMail", notificationCategory, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
        }

        public void SendFireDrillMailOnDone(TExercises exercises, int notificationCategory)
        {
            SendFireDrillEmail(exercises, "SendFireDrillMailOnDone", notificationCategory, Convert.ToInt32(BDO.Enums.NotificationEvent.OnClosed));
        }

        public void SendFireDrillOnDueDatePassed(TExercises exercises, int notificationCategory)
        {
            SendFireDrillEmail(exercises, "SendFireDrillOnDueDatePassed", notificationCategory, Convert.ToInt32(BDO.Enums.NotificationEvent.OnDueDate));
        }

        private bool SendFireDrillEmail(TExercises exercises, string mode, int notificationCategory, int notificationeventId)
        {
            if (exercises.CreatedBy != 0)
            {
                var ownerUser = _usersRepository.GetUsersList(exercises.CreatedBy);// _userService.GetUserProfile(exercises.CreatedBy);
                var buildingname = _buildingsRepository.GetBuildings().Where(x => x.BuildingId == exercises.BuildingId).Select(x => x.BuildingName).FirstOrDefault();
                //FileInfo emailBody = new FileInfo(string.Format(_commonProvider.GetContentRootPath("Templates/FiredrillCreated.html")));
                string path = _commonProvider.GetContentRootPath("wwwroot"); ;
                var emailBody = File.ReadAllText(string.Format(path + "{0}", @"/Templates/FiredrillCreated.html"));
                emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
                emailBody = emailBody.Replace("{Errortext}", FiredrillText(exercises, mode, buildingname, ownerUser));
                string sender = _appSettings.SenderMailFrom;

                var obj = GetNotificationEmail(notificationCategory, exercises.BuildingId.ToString(), string.Empty, string.Empty, notificationeventId);
                var toAddress = obj.NotificationEmail;

                if (ownerUser.UserId > 0)
                    toAddress = toAddress + "," + string.Join(",", ownerUser.Email);
                var ccAddress = obj.NotificationCCEmail;
                var subject = FiredrillSubject(mode);
                SendMail(toAddress, subject, emailBody, sender, null, ccAddress);
            }

            return true;
        }

        private string FiredrillText(TExercises newItem, string mode, string buildingname, UserProfile ownerUser)
        {
            string text = string.Empty;
            switch (mode)
            {
                case "SendFiredrillCreatedMail":
                    DateTime Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
                    string date = $"{Firedrilldate:MMM dd, yyyy}";
                    string Done = newItem.Status == 0 ? "Plan" : "Done";
                    //var announced = newItem.Announced == false ? "No" : "Yes";
                    text = string.Format("A FIRE DRILL SCHEDULE <br /> Building : {0} <br /> Created Date : {1} <br />  Schedule Date : {2} <br /> Time : {3} <br /> Planned By : {4} <br /> Status : {5}"
                        , buildingname, $"{DateTime.Now:MMM dd, yyyy}", date, newItem.StartTime, ownerUser.FullName, Done);
                    break;
                case "SendFireDrillReminderMail":
                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
                    string duedate = $"{Firedrilldate:MMM dd, yyyy}";
                    //var Dueannounced = newItem.Announced == false ? "No" : "Yes";
                    text = string.Format("A FireDrill is Planned <br /> Building : {0} <br />  Schedule Date : {1} <br /> Time : {2} <br /> Planned By : {3}"
                        , buildingname, duedate, newItem.StartTime, ownerUser.FullName);
                    break;
                case "SendFireDrillMailOnDone":
                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
                    date = $"{Firedrilldate:MMM dd, yyyy}";//date.Replace("12:00:00 AM", "");
                    //announced = newItem.Announced == false ? "No" : "Yes";
                    text = string.Format("A FIRE DRILL DONE <br /> Building : {0} <br />Completed Date : {1} <br />  Schedule Date : {2} <br /> Time : {3} <br /> Planned By : {4}"
                        , buildingname, $"{DateTime.Now:MMM dd, yyyy}", date, newItem.StartTime, ownerUser.FullName);
                    break;
                case "SendFireDrillOnDueDatePassed":
                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
                    date = $"{Firedrilldate:MMM dd, yyyy}";//date.Replace("12:00:00 AM", "");
                    //announced = newItem.Announced == false ? "No" : "Yes";
                    text = string.Format("A FIRE DRILL DueDate Passed <br /> Building : {0} <br />Schedule Date : {1} <br /> Time : {2} <br /> Planned By : {3}"
                        , buildingname, date, newItem.StartTime, ownerUser.FullName);
                    break;

            }
            return text;
        }

        private string FiredrillSubject(string mode)
        {
            string subject = string.Empty;
            switch (mode)
            {
                case "SendFiredrillCreatedMail":
                    subject = "CRx: FireDrill Scheduled";
                    break;
                case "SendFireDrillReminderMail":
                    subject = "CRx: FireDrill Reminder";
                    break;
                case "SendFireDrillOnDueDatePassed":
                    subject = "CRx: FireDrill Due Date Passed";
                    break;
                case "SendFireDrillMailOnDone":
                    subject = "CRx: FireDrill Done";
                    break;
            }
            return subject;
        }

        #endregion

        #region Common Emails
        public bool SendCommonEmail(NotificationEmails objnotificationEmails, object objects)
        {
            List<string> toemails = new List<string>();
            List<string> ccemails = new List<string>();
            if (!string.IsNullOrEmpty(objnotificationEmails.NotificationEmail))
                toemails = objnotificationEmails.NotificationEmail.Split(',').ToList();
            if (!string.IsNullOrEmpty(objnotificationEmails.NotificationCCEmail))
                ccemails = objnotificationEmails.NotificationCCEmail.Split(',').ToList();
            var newNotificationEmail = _notificationRepository.GetNotificationEmailsAddress(objnotificationEmails.NotificationCatId, objnotificationEmails.BuildingId.ToString(), objnotificationEmails.FloorId.ToString(),
                                      objnotificationEmails.SiteId.ToString(), objnotificationEmails.NotificationEventId);
            if (newNotificationEmail != null && !string.IsNullOrEmpty(newNotificationEmail.NotificationEmail)  && !string.IsNullOrEmpty(newNotificationEmail.NotificationCCEmail))
            {
                toemails.AddRange(newNotificationEmail.NotificationEmail.Split(',').ToList());
                ccemails.AddRange(newNotificationEmail.NotificationCCEmail.Split(',').ToList());
            }
            else
            {
                return true;
            }
            switch ((BDO.Enums.NotificationCategory)newNotificationEmail.NotificationCatId)
            {
                case BDO.Enums.NotificationCategory.Inspection:
                    switch ((BDO.Enums.NotificationEvent)newNotificationEmail.NotificationEventId)
                    {
                        case BDO.Enums.NotificationEvent.OnDeficiencies: SendInspectionMailOnDeficiencies((HCF.BDO.Inspection)objects, toemails, ccemails); break;
                    }
                    break;
                case BDO.Enums.NotificationCategory.ILSM: break;
                case BDO.Enums.NotificationCategory.WorkOrder: break;
                case BDO.Enums.NotificationCategory.RoundsInspection: break;
                case BDO.Enums.NotificationCategory.TimeToResolveToRA: break;
                case BDO.Enums.NotificationCategory.ConfigurationalItem: break;
                case BDO.Enums.NotificationCategory.All: break;
                case BDO.Enums.NotificationCategory.RouteBasedInspection: break;
                case BDO.Enums.NotificationCategory.PCRA: break;
                case BDO.Enums.NotificationCategory.FSBP: break;
                case BDO.Enums.NotificationCategory.MOP: break;
                case BDO.Enums.NotificationCategory.LSDAR: break;
                case BDO.Enums.NotificationCategory.HWP: break;
                case BDO.Enums.NotificationCategory.CP: break;
                case BDO.Enums.NotificationCategory.FMC: break;
                case BDO.Enums.NotificationCategory.Firedrill: break;
                case BDO.Enums.NotificationCategory.CRA: break;
                case BDO.Enums.NotificationCategory.ICRA: break;
                case BDO.Enums.NotificationCategory.AutoReport: break;
                case BDO.Enums.NotificationCategory.Binders:
                    switch ((BDO.Enums.NotificationEvent)newNotificationEmail.NotificationEventId)
                    {
                        case BDO.Enums.NotificationEvent.OnExpiration: SendDocumentMailOnExpiration((HCF.BDO.HttpResponseMessage)objects, toemails, ccemails); break;
                    }
                    break;
            }

            return true;
        }

        #region Inspection  

        private bool SendInspectionMailOnDeficiencies(Inspection newItem, List<string> toemails, List<string> ccemails)
        {
            string timeZone = _organizationRepository.GetMasterOrganization().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(123456))?.Timezone;
            try
            {
                StringBuilder stepsinfo = new StringBuilder();
                StringBuilder deviceinfo = new StringBuilder();
                var assetsUsers = new List<UserProfile>();
                var activity = newItem.TInspectionActivity.FirstOrDefault();
                var maingoal = _mainGoalRepository.GetActivityMainGoals(activity.ActivityId); //need to improve methods
                                                                                              // List<Steps> steps = StepsRepository.GetSteps();
                newItem.EPDetails = _epRepository.GetEpShortDescription(newItem.EPDetailId);

                activity.UserProfile = _usersRepository.GetUsersList(activity.CreatedBy);// _userService.GetUserProfile(activity.CreatedBy);
                foreach (var goal in activity.TInspectionDetail)
                {
                    goal.MainGoal = maingoal.FirstOrDefault(x => x.MainGoalId == goal.MainGoalId);
                    var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
                    if (items.Count > 0)
                    {
                        stepsinfo.Append("<tr>");
                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong><b>");
                        stepsinfo.Append(goal.MainGoal.Goal);
                        stepsinfo.Append("</b></strong></td></tr>");
                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
                        {
                            inspectionStep.Steps = goal.MainGoal.Steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
                            stepsinfo.Append("<tr>");
                            stepsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((BDO.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
                            stepsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
                            stepsinfo.Append("</tr>");
                        }
                        stepsinfo.Append("<br />");
                    }
                }
                if (activity.FloorAssetId.HasValue)
                {
                    string floorlocation = string.Empty;
                    string stopName = string.Empty;
                    activity.TFloorAssets = _floorAssetRepository.GetFloorAssetDescription(activity.FloorAssetId.Value);
                    if (activity.TFloorAssets.Floor != null) { floorlocation = activity.TFloorAssets.Floor.FloorLocation; }
                    if (activity.TFloorAssets.Stop != null) { stopName = $"{activity.TFloorAssets.Stop.StopName} ({activity.TFloorAssets.Stop.StopCode})"; }
                    assetsUsers.ToList().AddRange(activity.TFloorAssets.Assets.Users);
                    deviceinfo.Append("<tr>");
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Name:</strong>&nbsp;" + activity.TFloorAssets.Name);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + floorlocation);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Nearby:</strong>&nbsp;" + activity.TFloorAssets.NearBy);
                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Stop Name:</strong>&nbsp;" + stopName + "</td>");
                    deviceinfo.Append("</tr>");
                }
                var objTInspectionFiles = new List<TInspectionFiles>();
                objTInspectionFiles = _transactionRepository.GetInspectionFiles(activity.ActivityId).ToList();
                List<string> attachments = new List<string>();
                foreach (var item in objTInspectionFiles)
                {
                    if (!string.IsNullOrEmpty(item.FilePath))
                        attachments.Add("");
                }
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/StepsFail.html"));

                //FileInfo fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/StepsFail.html"));
                string emailBody = File.ReadAllText(fileInfo.FullName);
                if (newItem.EPDetails != null)
                {
                    emailBody = emailBody.Replace("{standardName}", newItem.EPDetails != null ? newItem.EPDetails.StandardEp : "Route Base Inspection (No EP link with this assets)");
                    emailBody = emailBody.Replace("{EpNUmber}", newItem.EPDetails.EPNumber);
                    emailBody = emailBody.Replace("{EpDescription}", newItem.EPDetails != null ? newItem.EPDetails.Description : string.Empty);
                }
                else { emailBody = emailBody.Replace("{standardName}", string.Empty); emailBody = emailBody.Replace("{EpDescription}", string.Empty); }
                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
                emailBody = emailBody.Replace("{inspector}", activity.UserProfile.Name);
                emailBody = emailBody.Replace("{inspectorName}", activity.UserProfile.Name);
                emailBody = emailBody.Replace("{inspectionDate}", Conversion.GetUtcToLocal(activity.ActivityInspectionDate ?? DateTime.Now, timeZone).ToString("MMM dd, yyyy hh:mm tt "));  //activity.ActivityInspectionDate.Value.ToString("dd MMM yyyy hh:mm tt "));
                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
                string sender = _appSettings.SenderMailFrom;
                string toAddress = string.Join(",", toemails);
                if (assetsUsers.Count > 0)
                    toAddress = toAddress + "," + string.Join(",", assetsUsers.Select(i => i.Email));
                string ccAddress = string.Join(",", ccemails);
                string subject = $"CRx: Inspection Failed Asset # {activity.TFloorAssets.DeviceNo} {(newItem.EPDetails != null ? ", " + newItem.EPDetails.StandardEp : string.Empty)}";
                string ilsmMailFrom = sender;
                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return true;
        }

        #endregion Inspection 

        #region ILSM


        #endregion

        #region Binders (Send Document Notification Mail)
        private bool SendDocumentMailOnExpiration(HttpResponseMessage objHttpResponseMessage, List<string> toemails, List<string> ccemails)
        {
            try
            {
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendDocumentNotificationMail.html"));

                //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/SendDocumentNotificationMail.html"));
                var emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", GenerateDocumentReports(objHttpResponseMessage));
                var attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom;
                string toAddress = string.Join(",", toemails);
                string ccAddress = string.Join(",", ccemails);
                string subject = $"CRx : AHJ Document(s) expiring soon ";
                SendMail(toAddress, subject, emailBody, sender, attachments, ccAddress);
            }
            catch (Exception)
            {

            }

            return true;
        }

        private string GenerateDocumentReports(HttpResponseMessage objHttpResponseMessage)
        {
            StringBuilder reportinfo = new StringBuilder();
            if (objHttpResponseMessage.Result.BinderFolders != null && objHttpResponseMessage.Result.BinderFolders.Count > 0)
            {
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>File Name  </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Common Document Name </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Expiration Date  </strong></td>" +
                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Uploaded By  </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.BinderFolders)
                {
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.FolderName +
                        "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                         item.CommonName + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.ValidUpTo.Value.ToShortDateString() + "</td>");
                    reportinfo.Append(
                       "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                       item.CreatedByUserProfile.Name + "</td>");
                    reportinfo.Append("</tr>");
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }
            return reportinfo.ToString();
        }

        #endregion Binders (Send Document Notification Mail)


        #endregion

        #region CRx Auto Report Emails

        public bool SendAutoReportEmails(HttpResponseMessage objHttpResponseMessage, int notificationCategory)
        {
            bool status = true;
            try
            {
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendAutoReportEmail.html"));

                //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/SendAutoReportEmail.html"));
                var emailBody = File.ReadAllText(fileInfo.FullName);
                emailBody = emailBody.Replace("{Errortext}", GenerateReports(objHttpResponseMessage));
                var attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom;
                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.OnCreated));
                string toAddress = obj.NotificationEmail;
                string ccAddress = obj.NotificationCCEmail;
                string subject = $"CRx: Upcoming Compliance tasks"; //$"CRx : Weekly Auto Report Emails";
                SendMail(toAddress, subject, emailBody, sender, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                status = false;
            }
            return status;
        }

        private string GenerateReports(HttpResponseMessage objHttpResponseMessage)
        {
            StringBuilder reportinfo = new StringBuilder();
            reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Assets Needing Inspection</h4>");

            //Assets Needing Inspection
            if (objHttpResponseMessage.Result.TFloorAssets != null && objHttpResponseMessage.Result.TFloorAssets.Count > 0)
            {
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.TFloorAssets.GroupBy(x => x.SiteName).Select(grp => grp.First()).ToList())
                {
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        "<b>" + item.SiteName + "</b>" + "</td>");
                    reportinfo.Append("</tr>");
                    foreach (var assets in objHttpResponseMessage.Result.TFloorAssets.Where(x => x.SiteName == item.SiteName).GroupBy(x => x.AssetId).Select(grp => grp.First()).ToList().ToList())
                    {
                        foreach (var floorAssets in objHttpResponseMessage.Result.TFloorAssets.Where(x => x.AssetId == assets.AssetId))
                        {
                            var hyperlink = string.Format("https://crxweb.com/ep/{0}/inspections", item.EPDetail.EPDetailId);
                            reportinfo.Append("<tr>");
                            reportinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                assets.Assets.Name + "</td>");
                            reportinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                    "<a href = " + hyperlink + " >" + item.EPDetail.StandardEp +
                                     "</a></td>");
                            //reportinfo.Append(
                            //    "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                            //     item.EPDetail.StandardEp + "</td>");
                            reportinfo.Append(
                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                                string.Join(",", objHttpResponseMessage.Result.TFloorAssets.Select(x => x.BuildingName)) + "</td>");
                            reportinfo.Append(
                               "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                               string.Empty + "</td>");
                            reportinfo.Append("</tr>");
                        }
                    }
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }

            //Policies Needing Review
            if (objHttpResponseMessage.Result.PoliciesNeedingReview != null && objHttpResponseMessage.Result.PoliciesNeedingReview.Count > 0)
            {
                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Policies Needing Review</h4>");
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Policy Name </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>StandardEP </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Next Review Date </strong></td>" +
                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Assigned User </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.PoliciesNeedingReview)
                {
                    if (item != null)
                    {
                        var hyperlink = string.Format("https://crxweb.com/Repository/PolicyBinders?epId={0}", item.EPDetailId);
                        reportinfo.Append("<tr>");
                        reportinfo.Append(
                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                            "<a href = " + hyperlink + " >" + item.StandardEp +
                            "</a></td>");
                        reportinfo.Append(
                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                             item.StandardEp + "</td>");
                        reportinfo.Append(
                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                            item.Inspection.DueDate.ToFormatDate() + "</td>");
                        reportinfo.Append(
                           "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                           string.Join(",", item.EPUsers.Select(x => x.Name)) + "</td>");
                        reportinfo.Append("</tr>");
                    }
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }

            //Other EPs Needing Review 
            if (objHttpResponseMessage.Result.OtherEPsNeedingReview != null && objHttpResponseMessage.Result.OtherEPsNeedingReview.Count > 0)
            {
                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Other EPs Needing Review</h4>");
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>StandardEP </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Next Review Date </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Assigned User </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.OtherEPsNeedingReview)
                {
                    var hyperlink = string.Format("https://crxweb.com/ep/{0}/inspections", item.EPDetailId);
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                       "<a href = " + hyperlink + " >" + item.StandardEp +
                        "</a></td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                         item.Inspection.DueDate.ToFormatDate() + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        string.Join(",", item.EPUsers.Select(x => x.Name)) + "</td>");
                    reportinfo.Append("</tr>");
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }

            //Undocumented Fire Drills  
            if (objHttpResponseMessage.Result.Exercises != null && objHttpResponseMessage.Result.Exercises.Count > 0)
            {
                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Firedrills pending documents</h4>");
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Building </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Quarter </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Shift </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.Exercises)
                {
                    HCF.BDO.Enums.QuarterNo enums = (HCF.BDO.Enums.QuarterNo)item.QuarterNo;
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.Building.BuildingName + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        enums.GetDescription() + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.Shift.Name + "</td>");
                    reportinfo.Append("</tr>");
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }

            //Open ILSMs (>90 Days)
            if (objHttpResponseMessage.Result.TIlsms != null && objHttpResponseMessage.Result.TIlsms.Count > 0)
            {
                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>ILSMs open for > 90 days</h4>");
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>ILSM Number </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>ILSM Name </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>User </strong></td>" +
                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comments </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.TIlsms)
                {
                    var hyperlink = string.Format("https://crxweb.com/ilsm/{0}/view", item.TIlsmId);
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        "<a href = " + hyperlink + " >" + item.IncidentId +
                        "</a></td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.Name + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                       string.Join(",", item.Responsible.Select(x => x.Name)) + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        string.Empty +
                        "</td>");
                    reportinfo.Append("</tr>");
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }

            //Open Permits (>90 Days)
            if (objHttpResponseMessage.Result.AllPermits != null && objHttpResponseMessage.Result.AllPermits.Count > 0)
            {
                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Permits open for > 90 days</h4>");
                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Permit Type/Number </strong></td>" +
                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Project Name </strong></td>" +
                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Manager </strong></td>" +
                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comments </strong></td>" +
                    "</tr></thead>");
                reportinfo.Append("<tbody>");
                foreach (var item in objHttpResponseMessage.Result.AllPermits)
                {
                    reportinfo.Append("<tr>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        $"{(BDO.Enums.PermitType)item.PermitType}/{item.PermitNo}" + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        item.ProjectName + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        string.Empty + "</td>");
                    reportinfo.Append(
                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
                        string.Empty +
                        "</td>");
                    reportinfo.Append("</tr>");
                }
                reportinfo.Append("</tbody>");
                reportinfo.Append("</table><br />");
            }
            return reportinfo.ToString();
        }

        #endregion    

        public bool SendEmail(string to, string subject, string body)
        {
            //return true;
            return _emailProcessor.SendMail(to, subject, null, body, null, null);
        }

        #region Notify Volunteer
        public bool SendNotifyVolunteerMail(List<ActivityData> activity, string CurrentUserName)
        {
            var ownerUser = activity.FirstOrDefault().user;
            //FileInfo emailBody = new FileInfo(string.Format(_commonProvider.GetContentRootPath("Templates/NotifyVolunteermail.html")));
            string path = _commonProvider.GetContentRootPath("wwwroot"); ;
            var emailBody = File.ReadAllText(string.Format(path + "{0}", @"/Templates/NotifyVolunteermail.html"));
            StringBuilder midtext = new StringBuilder();
            if (activity.Any(x => x.Type == BDO.Enums.UserActivityType.Added.ToString()))
            {
                var data = activity.Where(x => x.Type == BDO.Enums.UserActivityType.Added.ToString()).ToList();
                if (data.Count > 0)
                {
                    midtext.Append("You have been added to the following round(s): <br />");
                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
                    midtext.Append("<tr>");
                    midtext.Append("<td><b>" + "Round" + "</b></td>");
                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
                    midtext.Append("<td><b>" + "Next Date" + "</b></td>");
                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
                    midtext.Append("</tr>");
                    midtext.Append("</thead><tbody>");

                    foreach (var item in data)
                    {
                        midtext.Append("<tr>");
                        midtext.Append("<td>" + item.roundcategories + "</td>");
                        midtext.Append("<td>" + item.rGroupName + "</td>");
                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
                        midtext.Append("<td>" + item.frequency + "</td>");
                        midtext.Append("</tr>");
                    }
                    midtext.Append("</tbody></table>");
                }
            }

            if (activity.Any(x => x.Type == BDO.Enums.UserActivityType.TemporaryReplaced.ToString()))
            {
                var data = activity.Where(x => x.Type == BDO.Enums.UserActivityType.TemporaryReplaced.ToString()).ToList();
                if (data.Count > 0)
                {
                    midtext.Append("You have been temporarily replaced to the following round(s):  <br />");
                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
                    midtext.Append("<tr>");
                    midtext.Append("<td><b>" + "Round" + "</b></td>");
                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
                    midtext.Append("<td><b>" + "Effective Date" + "</b></td>");
                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
                    midtext.Append("</tr>");
                    midtext.Append("</thead><tbody>");
                    foreach (var item in data)
                    {
                        midtext.Append("<tr>");
                        midtext.Append("<td>" + item.roundcategories + "</td>");
                        midtext.Append("<td>" + item.rGroupName + "</td>");
                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
                        midtext.Append("<td>" + item.frequency + "</td>");
                        midtext.Append("</tr>");
                    }
                    midtext.Append("</tbody></table>");
                }
            }

            if (activity.Any(x => x.Type == BDO.Enums.UserActivityType.PermanentlyReplaced.ToString()))
            {
                var data = activity.Where(x => x.Type == BDO.Enums.UserActivityType.PermanentlyReplaced.ToString()).ToList();
                if (data.Count > 0)
                {
                    midtext.Append("You have been permanently replaced to the following round(s):  <br />");
                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
                    midtext.Append("<tr>");
                    midtext.Append("<td><b>" + "Round" + "</b></td>");
                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
                    midtext.Append("<td><b>" + "Effective Date" + "</b></td>");
                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
                    midtext.Append("</tr>");
                    midtext.Append("</thead><tbody>");
                    foreach (var item in data)
                    {
                        midtext.Append("<tr>");
                        midtext.Append("<td>" + item.roundcategories + "</td>");
                        midtext.Append("<td>" + item.rGroupName + "</td>");
                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
                        midtext.Append("<td>" + item.frequency + "</td>");
                        midtext.Append("</tr>");
                    }
                    midtext.Append("</tbody></table>");
                }
            }

            if (activity.Any(x => x.Type == BDO.Enums.UserActivityType.Removed.ToString()))
            {
                var data = activity.Where(x => x.Type == BDO.Enums.UserActivityType.Removed.ToString()).ToList();
                if (data.Count > 0)
                {
                    midtext.Append("You have been removed from the follow round(s): <br />");

                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
                    midtext.Append("<tr>");
                    midtext.Append("<td><b>" + "Round" + "</b></td>");
                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
                    midtext.Append("<td><b>" + "Removal Date" + "</b></td>");
                    midtext.Append("</tr>");
                    midtext.Append("</thead><tbody>");
                    foreach (var item in data)
                    {
                        midtext.Append("<tr>");
                        midtext.Append("<td>" + item.roundcategories + "</td>");
                        midtext.Append("<td>" + item.rGroupName + "</td>");
                        midtext.Append("<td>" + item.CreatedDate.ToLongDateString() + "</td>");
                        midtext.Append("</tr>");
                    }
                    midtext.Append("</tbody></table>");
                }
            }

            string text = "Hello " + ownerUser.FullName + "<br />" + "There has been an update in your Rounds Inspection. Please view your recent Round(s) Assignment update below." + "<br />"
                          + midtext + "<br />" + "Your currently assigned rounds are viewable in ‘Round Inspection’ under the Round Module in CRx. Be sure to select your name in the dropdown box." + "<br />"
                          + "Thank you, " + "<br />" + CurrentUserName + "<br />" + "via CRx System";

            emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
            emailBody = emailBody.Replace("{Errortext}", string.Format(text));
            string sender = _appSettings.SenderMailFrom;
            var toAddress = ownerUser.Email;
            var subject = "CRx: Rounds Assignment Update";
            return SendMail(toAddress, subject, emailBody, sender, null, null);
        }


        #endregion

        #region Password Rest Email
        public bool SendMailOnResetPasswordRequest(UserProfile userProfile, string token,string webUrl)
        {
            var date = DateTime.Now;
            string ccAddress = string.Empty;
            var type = "reset";
            // var webUrl = _appSettings.WebUrlPath;
            // var url = $"{webUrl}user/password/{type}/{token}";
            var url = string.Format("<a href='{0}' title='Reset Your Account on HCF'>here</a>", webUrl);
            string path = _commonProvider.GetContentRootPath("wwwroot");
            FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/UserPasswordResetEmail.html"));

            //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/UserPasswordResetEmail.html"));
            string emailBody = File.ReadAllText(fileInfo.FullName);
            emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
            emailBody = emailBody.Replace("{url}", url);
            emailBody = emailBody.Replace("{FullName}", userProfile.Name);
            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
            SendMail(userProfile.UserName, "Password Reset for User", emailBody, "", null, ccAddress);

            return true;
        }
        #endregion
        public bool SendVerificationCode(string userName, string newdevicecode, string fullName)
        {

            var date = DateTime.Now;
            string ccAddress = string.Empty;
            string path = _commonProvider.GetContentRootPath("wwwroot");
            FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/NewDeviceVerification.html"));

            //var fileInfo = new FileInfo(string.Format(_commonProvider.GetContentRootPath() + "{0}", @"Templates/NewDeviceVerification.html"));
            string emailBody = File.ReadAllText(fileInfo.FullName);
            emailBody = emailBody.Replace("{FullName}", fullName);
            emailBody = emailBody.Replace("{UserName}", userName);
            emailBody = emailBody.Replace("{code}", newdevicecode);
            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
            SendMail(userName, "Verification For new Device Login", emailBody, "", null, ccAddress);

            return true;
        }

        public void ReportError(LogEntry logEntry)
        {
            string ccAddress = string.Empty;
            string path = _commonProvider.GetContentRootPath("wwwroot");
            FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/ReportError.html"));
            string emailBody = File.ReadAllText(fileInfo.FullName);
            emailBody = emailBody.Replace("{FullName}", logEntry.UserName);
            emailBody = emailBody.Replace("{OrgName}", logEntry.DbName);
            emailBody = emailBody.Replace("{ErrorCode}", logEntry.ErrorCode);
            emailBody = emailBody.Replace("{ErrorMessage}", logEntry.ErrorMessage);
            emailBody = emailBody.Replace("{StackTrace}", logEntry.StackTrace);
            emailBody = emailBody.Replace("{deviceInfo}", string.Format("{0} {1} {2}", logEntry.OsName, logEntry.DeviceType, logEntry.BrowserName));
            emailBody = emailBody.Replace("{date}", string.Format("{0}", DateTime.UtcNow.ToString("ddd, dd MMM yyy HH:MM:SS UTC")));
            emailBody = emailBody.Replace("{errorPath}", string.Format("{0}{1}", _appSettings.WebUrlPath, logEntry.Module));
            SendMail(_appSettings.ToErrorEmail, " Unexpected Error - " + logEntry.Module, emailBody, "", null, ccAddress);
        }


        #region Non-Tracked Annual Reports
        public bool SendNonTrackedEPReportEmails(HttpResponseMessage objHttpResponseMessage, int notificationCategory)
        {
            bool status = true;
            try
            {
                string path = _commonProvider.GetContentRootPath("wwwroot");
                FileInfo fileInfo = new FileInfo(string.Format(path + "{0}", @"/Templates/SendNonTrackedEPReportEmail.html"));
                var emailBody = File.ReadAllText(fileInfo.FullName);
                var attachments = new List<string>();
                string sender = _appSettings.SenderMailFrom;
                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty, Convert.ToInt32(BDO.Enums.NotificationEvent.AnnualReviewDate));
                string toAddress = obj.NotificationEmail;
                string ccAddress = obj.NotificationCCEmail;
                string subject = $"CRx: Non-Tracked EP Report";
                SendMail(toAddress, subject, emailBody, sender, attachments, ccAddress);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                status = false;
            }
            return status;
        }

        #endregion
    }
}
