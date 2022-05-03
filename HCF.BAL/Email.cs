//using HCF.BDO;
//using HCF.Utility;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web.Hosting;

//namespace HCF.BAL
//{
//    public static class Email
//    {
//        private static string MailFrom { get; } = AppSettings.SenderMailFrom;
//        private static string WebBaseUrl { get; } = AppSettings.WebUrlPath;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="to"></param>
//        /// <param name="subject"></param>
//        /// <param name="body"></param>
//        /// <param name="from"></param>
//        /// <param name="attachments"></param>
//        /// <param name="ccAddress"></param>
//        /// <returns></returns>
//        private static bool SendMail(string to, string subject, string body, string @from, List<string> attachments = null, string ccAddress = null)
//        {
//            var outgoingMailId = DAL.Email.SaveEmail(to, ccAddress, subject, body, from);
//            bool status = EmailProcessor.SendMail(to, subject, ccAddress, body, from, attachments);
//            if (outgoingMailId.HasValue && status)
//                DAL.Email.UpdateSentMail(outgoingMailId.Value);
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="toAddress"></param>
//        /// <param name="subject"></param>
//        /// <param name="ccAddress"></param>
//        /// <param name="body"></param>
//        /// <param name="from"></param>
//        /// <param name="fileBytes"></param>
//        /// <param name="filename"></param>
//        private static bool SendMail(string toAddress, string subject, string ccAddress, string body, string @from, byte[] fileBytes, string filename)
//        {
//            var outgoingMailId = DAL.Email.SaveEmail(toAddress, ccAddress, subject, body, from);
//            bool status = EmailProcessor.SendMail(toAddress, subject, ccAddress, body, from, fileBytes, filename);
//            if (outgoingMailId.HasValue && status)
//                DAL.Email.UpdateSentMail(outgoingMailId.Value);
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="notificationCategory"></param>
//        /// <param name="buildingId"></param>
//        /// <param name="floorId"></param>
//        /// <param name="siteId"></param>
//        /// <param name="EventId"></param>
//        /// <returns></returns>
//        private static NotificationEmails GetNotificationEmail(int notificationCategory, string buildingId, string floorId, string siteId, int? EventId = 1)
//        {
//            if (siteId == null)
//                throw new ArgumentNullException(nameof(siteId));

//            var objNotificationEmail = NotificationRepository.GetNotificationEmailByCatID(notificationCategory, buildingId, floorId, siteId, EventId);
//            return objNotificationEmail;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="to"></param>
//        /// <param name="subject"></param>
//        /// <param name="body"></param>
//        /// <param name="from"></param>
//        /// <param name="attachments"></param>
//        /// <param name="ccAddress"></param>
//        /// <returns></returns>
//        public static bool SendPasswordResetMail(string to, string subject, string body, string from, List<string> attachments = null, string ccAddress = null)
//        {
//            return SendMail(to, subject, body, MailFrom, attachments, ccAddress);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorWorkOrder"></param>
//        /// <param name="notificationCategory"></param>
//        public static bool SendRoundInsMail(WorkOrder floorWorkOrder, int notificationCategory)
//        {
//            string sender = MailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"];
//            string timeZone = OrganizationRepository.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(HcfSession.ClientNo))?.Timezone;

//            var userProfile = Users.GetUserProfile(floorWorkOrder.CreatedBy);
//            if (!string.IsNullOrEmpty(userProfile.Email))
//                sender = userProfile.Email;

//            string location = "";
//            if (floorWorkOrder.FloorId != null)
//            {
//                var floorInfo = DAL.FloorRepository.GetFloorDescription(floorWorkOrder.FloorId.Value);
//                if (floorInfo != null)
//                    location = floorInfo.FloorName + " , " + floorInfo.Building.BuildingName;
//                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                string ccAddress = obj.NotificationCCEmail;
//                foreach (var user in floorWorkOrder.Resources)
//                {
//                    if (!string.IsNullOrEmpty(user.Email))
//                    {
//                        try
//                        {
//                            FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/FloorInspection.html"));
//                            string emailBody = File.ReadAllText(fileInfo.FullName);
//                            emailBody = emailBody.Replace("{description}", floorWorkOrder.Description);
//                            emailBody = emailBody.Replace("{timetoresolve}", floorWorkOrder.TimetoResolve.ToString());
//                            emailBody = emailBody.Replace("{username}", userProfile.Name);
//                            emailBody = emailBody.Replace("{date}",
//                                $"{Conversion.GetUtcToLocal(floorWorkOrder.DateCreated.Value, timeZone):MMM dd, yyyy hh:mm tt}"); //   Utility.Conversion.GetUtcToLocal(floorWorkOrder.DateCreated.Value, timeZone).ToString("MM/dd/yyyy hh:mm tt")); //DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
//                            emailBody = emailBody.Replace("{location}", location);
//                            if (floorInfo != null)
//                                SendMail(user.Email, "CRx : " + location + " Adhoc Survey", emailBody, sender, null, ccAddress);
//                        }
//                        catch (Exception ex)
//                        {
//                            ErrorLog.LogError(ex);
//                        }
//                    }
//                }
//            }

//            return true;
//        }

//        public static bool VendorNotify(Guid? invitationId, Vendors vendor, Organization org)
//        {
//            if (invitationId.HasValue)
//            {
//                try
//                {
//                    var url = AppSettings.WebUrlPath + "create/user/" + invitationId;
//                    FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/VendorNotify.html"));
//                    var emailBody = File.ReadAllText(fileInfo.FullName);
//                    emailBody = emailBody.Replace("{orgName}", org.Name)
//                            .Replace("{url}", url);

//                    SendMail(vendor.Email, org.Name + " invited you to join the CRx", emailBody, null, null);
//                }
//                catch (Exception ex)
//                {
//                    ErrorLog.LogError(ex);
//                }

//            }

//            return true;

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="epDeatilId"></param>
//        /// <param name="sampleDocument"></param>
//        /// <returns></returns>
//        public static bool SampleDocument(int epDeatilId, Documents sampleDocument)
//        {
//            EPDetails epdetail = EpRepository.GetEpDescription(epDeatilId);
//            if (epdetail == null)
//                throw new ArgumentNullException(nameof(epDeatilId));

//            List<DocumentType> attch = DAL.DocumentTypeRepository.GetDocumentTypes(epDeatilId);
//            List<string> attachmentsPath = new List<string>();
//            bool status = true;
//            try
//            {
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SampleDocumentInfo.htm"));
//                var emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{emailbody}", sampleDocument.Details)
//                        .Replace("{TJCStandard}", epdetail.Standard.TJCStandard)
//                        .Replace("{EPDescription}", epdetail.Description)
//                        .Replace("{EPNumber}", epdetail.EPNumber);

//                if (attch.Count > 0)
//                    attachmentsPath = attch.Select(x => x.FileUrl).ToList();


//                SendMail(sampleDocument.To, sampleDocument.Subject, emailBody, sampleDocument.Sender, attachmentsPath);
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                status = false;
//            }
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newRejectDoc"></param>
//        /// <returns></returns>
//        public static bool ReplyDocument(Documents newRejectDoc)
//        {
//            bool status = true;
//            try
//            {
//                newRejectDoc.ClientNo = Convert.ToInt32(HcfSession.ClientNo);
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/RejectDocumentInfo.htm"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{emailbody}", newRejectDoc.Details);

//                var attachmentsPath = newRejectDoc.Attachments.Select(x => (x.FileUrl.ToLower())).ToList();
//                SendMail(newRejectDoc.Sender, newRejectDoc.Subject, emailBody, null, attachmentsPath);

//                newRejectDoc.Details = emailBody;
//                newRejectDoc.MessageId = Guid.NewGuid().ToString();
//                newRejectDoc.IsReplied = true;

//                newRejectDoc.ParentDocumentRepoId = newRejectDoc.DocumentRepoId;
//                int insertedid = DAL.DocumentsRepository.InsertDocument(newRejectDoc);
//                newRejectDoc.DocumentRepoId = insertedid;

//                //insert into AttachmentTable

//                DocumentsRepository.UpdateAttachment(newRejectDoc, 1);
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                status = false;
//            }
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userProfile"></param>
//        /// <returns></returns>
//        public static bool SendMailOnUserRegistration(UserProfile userProfile)
//        {
//            var date = DateTime.Now;
//            var currentUser = Users.GetUsers().Single(m => m.UserId == userProfile.CreatedBy);
//            string ccAddress = string.Empty;
//            if (userProfile.VendorId.HasValue)
//            {
//                SendVendorEmailOnUserRegistration(userProfile);
//                //ccAddress = HCF.BAL.VendorRepository.GetVendor(userProfile.VendorId.Value).Email;
//            }
//            var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/UserRegistration.htm"));
//            string emailBody = File.ReadAllText(fileInfo.FullName);
//            emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
//            emailBody = emailBody.Replace("{Password}", userProfile.Password);
//            emailBody = emailBody.Replace("{FullName}", userProfile.Name);
//            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
//            SendMail(userProfile.UserName, "New UserRegistration ", emailBody, currentUser.UserName, null, ccAddress);

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userProfile"></param>
//        /// <returns></returns>
//        public static bool SendMailOnUserChangePassword(UserProfile userProfile)
//        {
//            var date = DateTime.Now;
//            var currentUser = Users.GetUsers().Single(m => m.UserId == userProfile.CreatedBy);
//            string ccAddress = string.Empty;
//            var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/UserChangePassword.htm"));
//            string emailBody = File.ReadAllText(fileInfo.FullName);
//            emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
//            emailBody = emailBody.Replace("{siteUrl}", AppSettings.WebUrlPath);
//            emailBody = emailBody.Replace("{Password}", userProfile.Password);
//            emailBody = emailBody.Replace("{FullName}", userProfile.Name);
//            emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", date));
//            SendMail(userProfile.UserName, "Change Password ", emailBody, currentUser.UserName, null, ccAddress);

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userProfile"></param>
//        /// <returns></returns>
//        private static bool SendVendorEmailOnUserRegistration(UserProfile userProfile)
//        {
//            var date = DateTime.Now;
//            //string ccAddress = string.Empty;
//            //try
//            //{
//            //    if (userProfile.VendorId != null)
//            //    {
//            //        var objvendors = VendorRepository.GetVendor(userProfile.VendorId.Value);
//            //        var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/VendorEmailOnUserRegistration.htm"));
//            //        string emailBody = File.ReadAllText(fileInfo.FullName);
//            //        emailBody = emailBody.Replace("{Name}", objvendors.Name);
//            //        emailBody = emailBody.Replace("{UserName}", userProfile.UserName);
//            //        emailBody = emailBody.Replace("{FullName}", userProfile.Name);
//            //        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
//            //        SendMail(objvendors.Email, "New UserRegistration ", emailBody, userProfile.UserName, null, ccAddress);
//            //    }
//            //}
//            //catch (Exception ex)
//            //{
//            //    ErrorLog.LogError(ex);
//            //}
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newRoundInsp"></param>
//        /// <param name="notificationCategory"></param>
//        /// <returns></returns>
//        public static bool SendRoundFailMail(TRounds newRoundInsp, int notificationCategory)
//        {
//            //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//            //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
//            //string toAddress = ccAddress;
//            NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//            string toAddress = obj.NotificationEmail;
//            string ccAddress = obj.NotificationCCEmail;
//            string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"];
//            if (!string.IsNullOrEmpty(toAddress))
//            {
//                if (newRoundInsp.RoundCatId != null)
//                {
//                    if (newRoundInsp.FloorId != null)
//                    {
//                        TRounds rounds = Rounds.GetRoundsHistory(newRoundInsp.RoundCatId.Value, newRoundInsp.FloorId.Value).FirstOrDefault(x => x.TRoundId == newRoundInsp.TRoundId);
//                        // var date = DateTime.Now;
//                        //var currentUser = Users.GetUsers().Single(m => m.UserId == newRoundInsp.CreatedBy);
//                        var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/RoundStatusMail.html"));
//                        string emailBody = File.ReadAllText(fileInfo.FullName);
//                        if (rounds != null)
//                        {
//                            emailBody = emailBody.Replace("{roundcategories}", rounds.CategoryName);
//                            emailBody = emailBody.Replace("{user}", rounds.UserProfile.Name);
//                            emailBody = emailBody.Replace("{location}", $"{rounds.Floors.Building.BuildingName},{rounds.Floors.FloorName}");
//                            emailBody = emailBody.Replace("{date}", $"{rounds.CreatedDate:MMM dd, yyyy}");
//                            StringBuilder sb = new StringBuilder();
//                            sb.Append(
//                                "<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
//                            sb.Append("<tr>");
//                            sb.Append(
//                                "<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:15%'><strong>Status</strong></td>");
//                            sb.Append(
//                                "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:60%'><strong>Round Step</strong></td>");
//                            sb.Append(
//                                "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'><strong>Comment</strong></td>");
//                            sb.Append("</tr>");
//                            foreach (var questionnaires in rounds.TRoundsQuestionnaires.Where(x => x.Status == 0))
//                            {
//                                sb.Append("<tr>");
//                                sb.Append(
//                                    "<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:15%'><p style='border-radius: 8px;background: #b24649;padding: 5px 14px;display: inline-block;line-height: 12px;font-size: 11px;color: #fff;'>" +
//                                    "Non-Complaint" + "</p></td>");
//                                sb.Append(
//                                    "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:60%'>" +
//                                    questionnaires.RoundsQuestionnaires.RoundStep + "</td>");
//                                sb.Append(
//                                    "<td align='left' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" +
//                                    questionnaires.Comment + "</td>");
//                                sb.Append("</tr>");
//                            }
//                            sb.Append("</table><br />");
//                            emailBody = emailBody.Replace("{data}", sb.ToString());
//                            emailBody = emailBody.Replace("{Signedby}", "<img src=" + WebBaseUrl + newRoundInsp.DigitalSignature.FilePath.Replace("~/", "") + " alt='Signature'/>");
//                        }

//                        if (rounds != null)
//                        {
//                            string subject = $"CRx : Round Status - {$"{rounds.Floors.FloorName},{rounds.Floors.Building.BuildingName}"}";
//                            SendMail(ccAddress, subject, emailBody, sender, null, ccAddress);
//                        }
//                    }
//                }
//            }
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="insStepsId"></param>
//        /// <param name="notificationCategory"></param>
//        public static bool SendTimeToResolveToRAMail(int insStepsId, int notificationCategory)
//        {
//            try
//            {
//                StringBuilder stepsinfo = new StringBuilder();
//                StringBuilder deviceinfo = new StringBuilder();
//                TInspectionActivity activity = InspectionActivityRepository.GetInspectionActivitybyTinsStepsId(insStepsId);
//                foreach (var goal in activity.TInspectionDetail)
//                {
//                    var items = goal.InspectionSteps.Where(x => x.TInsStepsId == insStepsId).ToList();
//                    if (items.Count > 0)
//                    {
//                        stepsinfo.Append("<tr>");
//                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong>");
//                        stepsinfo.Append(goal.MainGoal.Goal);
//                        stepsinfo.Append("</strong></td></tr>");
//                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.TInsStepsId == insStepsId))
//                        {
//                            stepsinfo.Append("<tr>");
//                            stepsinfo.Append("<td width='29%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
//                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((Utility.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
//                            stepsinfo.Append("<td width='20%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
//                            stepsinfo.Append("<td width='10%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.DRTime + "</td>");
//                            stepsinfo.Append("</tr>");
//                        }
//                        stepsinfo.Append("<br />");
//                    }
//                }
//                if (activity.FloorAssetId.HasValue)
//                {
//                    deviceinfo.Append("<tr>");
//                    deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Device Name:</strong>&nbsp;" + activity.TFloorAssets.Name + "</td>");
//                    deviceinfo.Append("<td width='180' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo + "</td>");
//                    //deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Inspector Name:</strong>&nbsp;" + activity.UserProfile.Name + "</td>");
//                    deviceinfo.Append("</tr>");
//                }
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/TimeToResolveEmail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
//                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
//                emailBody = emailBody.Replace("{Errortext}", string.Format("Time To Resolve is closed for the following:"));
//                List<string> attachments = new List<string>();
//                string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
//                //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//                //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);             
//                //string toAddress = ccAddress;
//                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                string toAddress = obj.NotificationEmail;
//                string ccAddress = obj.NotificationCCEmail;
//                string subject = $"CRx : Time To Resolve # {activity.TFloorAssets.DeviceNo}";
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//            }
//            catch (Exception ex)
//            {
//                HCF.Utility.ErrorLog.LogError(ex);
//                //status = false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static bool SendDueDateMail()
//        {
//            try
//            {
//                int DueWithInDays = Convert.ToInt32(AppSettings.DueWithInDays);
//                var objEPDetails = EpRepository.GetDueWithInDaysEPs(DueWithInDays);
//                var epsinfo = new StringBuilder();
//                foreach (var item in objEPDetails)
//                {
//                    epsinfo.Append("<tr>");
//                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.StandardEp + "</td>");
//                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Description + "</td>");
//                    epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.DueWithInDays + " Days </td>");
//                    epsinfo.Append("</tr>");
//                    epsinfo.Append("<tr>");
//                    epsinfo.Append("<td colspan='3' style='padding:15px;'>");
//                    epsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
//                    foreach (Assets assets in item.Assets)
//                    {
//                        epsinfo.Append("<tr>");
//                        epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + assets.Name + "</td>");
//                        epsinfo.Append("<tr>");
//                        epsinfo.Append("<td colspan='1' style='padding:15px;'>");
//                        epsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
//                        foreach (TFloorAssets tfloorAssets in assets.TFloorAssets)
//                        {
//                            epsinfo.Append("<tr>");
//                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.Name + "</td>");
//                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.DeviceNo + "</td>");
//                            epsinfo.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + tfloorAssets.DueWithInDays + " Days </td>");
//                            epsinfo.Append("</tr>");
//                        }
//                        epsinfo.Append("</table>");
//                        epsinfo.Append("</td></tr></td></tr>");
//                    }
//                    epsinfo.Append("</table>");
//                    epsinfo.Append("</td></tr>");
//                }
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendDueDateEmail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", string.Format("Some inspection are due with in {0} days:", DueWithInDays));
//                emailBody = emailBody.Replace("{epsinfo}", epsinfo.ToString());
//                var attachments = new List<string>();
//                var sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
//                var emailType = (int)Enum.Parse(typeof(Enums.NotificationEmailType), Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//                var ccAddress = NotificationRepository.GetNotificationEmail(emailType);
//                // string ccAddress = OrganizationRepository.GetOrganization().Email;
//                string toAddress = ccAddress;
//                string subject = string.Format("CRx : Inspections Due WithIn {0} Days", DueWithInDays);
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, "");
//            }
//            catch (Exception ex)
//            {
//                Utility.ErrorLog.LogError(ex);
//                // status = false;
//                return false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static bool SendConfigurationMail(int notificationCategory)
//        {
//            try
//            {
//                var objAudit = CommonRepository.GetAuditConfiguration();
//                var sb = new StringBuilder();
//                foreach (var item in objAudit)
//                {
//                    sb.Append("<tr>");
//                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.TableName + "</td>");
//                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.FieldName + "</td>");
//                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Type + "</td>");
//                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.Description + "</td>");
//                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.OldValue + "</td>");
//                    //sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.NewValue + "</td>");
//                    sb.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + item.UserName + "</td>");
//                    sb.Append("</tr>");
//                }
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendConfigurationEmail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", string.Format("Some configuration changes as following:"));
//                emailBody = emailBody.Replace("{date}", string.Format("{0:MMM dd, yyyy}", DateTime.Now));
//                emailBody = emailBody.Replace("{info}", sb.ToString());
//                List<string> attachments = new List<string>();
//                string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
//                //int emailType = (int)Enum.Parse(typeof(HCF.Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//                //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);              
//                //string toAddress = ccAddress;
//                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                string toAddress = obj.NotificationEmail;
//                string ccAddress = obj.NotificationCCEmail;
//                string subject = string.Format("CRx : Configuration Changes");
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                // status = false;
//            }

//            return true;

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static bool SendWorkOrderCreatedMail(WorkOrder objWorkOrder, int notificationCategory)
//        {
//            try
//            {
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendWorkOrderCreatedMail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", $"WorkOrder # {objWorkOrder.WorkOrderNumber} created for the following:");
//                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                emailBody = emailBody.Replace("{WoNumber}", objWorkOrder.WorkOrderNumber);
//                emailBody = emailBody.Replace("{status}", objWorkOrder.StatusCode);
//                emailBody = emailBody.Replace("{requesttype}", objWorkOrder.TypeCode);
//                emailBody = emailBody.Replace("{WoDescription}", objWorkOrder.Description);
//                emailBody = emailBody.Replace("{WoUrl}", WebBaseUrl + "WorkOrder/UpdateWorkOrder?issueId=" + objWorkOrder.IssueId.ToString());

//                var attachments = new List<string>();
//                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                string toAddress = obj.NotificationEmail;
//                string subject = $"CRx : WorkOrder # {objWorkOrder.WorkOrderNumber}";
//                SendMail(toAddress, subject, emailBody, MailFrom, attachments, "");
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static bool SendWorkOrderClosedMail(WorkOrder objWorkOrder, int notificationCategory)
//        {
//            try
//            {
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendWorkOrderClosedMail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", string.Format("WorkOrder # {0} updated for the following:", objWorkOrder.WorkOrderNumber));
//                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                emailBody = emailBody.Replace("{WoNumber}", objWorkOrder.WorkOrderNumber);
//                emailBody = emailBody.Replace("{status}", objWorkOrder.StatusCode);
//                emailBody = emailBody.Replace("{requesttype}", objWorkOrder.TypeCode);
//                emailBody = emailBody.Replace("{WoDescription}", objWorkOrder.Description);
//                emailBody = emailBody.Replace("{WoUrl}", WebBaseUrl + "WorkOrder/UpdateWorkOrder?issueId=" + objWorkOrder.IssueId);
//                var attachments = new List<string>();
//                var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                string toAddress = obj.NotificationEmail;
//                string subject = $"CRx : WorkOrder# {objWorkOrder.WorkOrderNumber}";
//                SendMail(toAddress, subject, emailBody, MailFrom, attachments, "");
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                //   status = false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="permitNo"></param>
//        /// <param name="permitType"></param>
//        /// <param name="tpcraquestid"></param>
//        /// <param name="projectId"></param>
//        /// <returns></returns>
//        public static bool SendPCRANotifyEmail(int userId, string permitNo, string permitType, int? tpcraquestid, int? projectId)
//        {
//            var objQuestionTPCRA = new TPCRAQuestion();
//            objQuestionTPCRA = PCRARepository.GetQuestionTPCRA(projectId, tpcraquestid);
//            var ircaLog = DAL.ConstructionRepository.GetIcras(null).FirstOrDefault(x => x.PermitNo == permitNo);
//            bool status = false;
//            if (ircaLog != null && objQuestionTPCRA != null)
//            {
//                var date = DateTime.Now;
//                string ccAddress = string.Empty;
//                try
//                {
//                    var userProfile = Users.GetUserProfile(userId);
//                    string subject = "CRA Notification";
//                    if (!string.IsNullOrEmpty(userProfile.Email))
//                    {

//                        var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendICRANotifyEmail.html"));
//                        string emailBody = File.ReadAllText(fileInfo.FullName);
//                        emailBody = emailBody.Replace("ICRA", "CRA");
//                        emailBody = emailBody.Replace("Permit", "CRA");
//                        emailBody = emailBody.Replace("{FullName}", userProfile.FirstName);
//                        emailBody = emailBody.Replace("{permitno}", objQuestionTPCRA.PCRANumber);
//                        emailBody = emailBody.Replace("{requestedby}", userProfile.Name);
//                        emailBody = emailBody.Replace("{projectname}", ircaLog.ProjectName);
//                        emailBody = emailBody.Replace("{url}", $"{WebBaseUrl}/pcra/addtpcra?projectId=" + objQuestionTPCRA.ProjectId + "&tpcraquestid=" + objQuestionTPCRA.TPCRAQuesId + "&mode=edit");
//                        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");

//                        SendMail(userProfile.Email, subject, emailBody, userProfile.UserName, null, ccAddress);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ErrorLog.LogError(ex);
//                    status = false;
//                }
//            }
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="permitNo"></param>
//        /// <param name="permitType"></param>
//        /// <returns></returns>
//        public static bool SendICRANotifyEmail(int userId, string permitNo, string permitType)
//        {

//            var ircaLog = DAL.ConstructionRepository.GetIcras(null).FirstOrDefault(x => x.PermitNo == permitNo);
//            bool status = true;
//            if (ircaLog != null)
//            {
//                var date = DateTime.Now;
//                var ccAddress = string.Empty;
//                try
//                {
//                    var userProfile = Users.GetUserProfile(userId);
//                    var subject = "";
//                    if (!string.IsNullOrEmpty(permitType))
//                        subject = "ICRA Notification";

//                    if (!string.IsNullOrEmpty(userProfile.Email))
//                    {
//                        var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendICRANotifyEmail.html"));
//                        string emailBody = File.ReadAllText(fileInfo.FullName);
//                        emailBody = emailBody.Replace("{FullName}", userProfile.FirstName);
//                        emailBody = emailBody.Replace("{permitno}", ircaLog.PermitNo);
//                        emailBody = emailBody.Replace("{requestedby}", userProfile.Name);
//                        emailBody = emailBody.Replace("{projectname}", ircaLog.ProjectName);
//                        emailBody = emailBody.Replace("{url}", $"{WebBaseUrl}/addinspectionicra?icraId={ircaLog.TicraId}&iseditable=True");
//                        emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
//                        SendMail(userProfile.Email, subject, emailBody, userProfile.UserName, null, ccAddress);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ErrorLog.LogError(ex);
//                    status = false;
//                }
//            }
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="modelData"></param>
//        /// <returns></returns>
//        public static bool SendEmailonRiskareaApproved(ICRARiskArea modelData)
//        {
//            bool status = true;
//            try
//            {
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendEmailonRiskareaApproved.html"));
//                var emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                emailBody = emailBody.Replace("{Errortext}", $"A Risk area ( {modelData.Name} ) is created and pending for your approval Please review :");
//                var attachments = new List<string>();
//                string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"];
//                int emailType = (int)Enum.Parse(typeof(Enums.NotificationEmailType), Enums.NotificationEmailType.CRxSystemUserEmail.ToString());
//                string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
//                string toAddress = ccAddress;
//                string subject = $"CRx : Need Action ( {modelData.Name} )";
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, "");
//            }
//            catch (Exception)
//            {
//                status = false;
//            }
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        /// <param name="notificationCategory"></param>
//        public static bool SendInspectionFailMail(Inspection newItem, int notificationCategory)
//        {
//            string timeZone = OrganizationRepository.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(HcfSession.ClientNo))?.Timezone;
//            try
//            {
//                StringBuilder stepsinfo = new StringBuilder();
//                StringBuilder deviceinfo = new StringBuilder();
//                var assetsUsers = new List<UserProfile>();
//                var activity = newItem.TInspectionActivity.FirstOrDefault();
//                var maingoal = MainGoalRepository.GetActivityMainGoals(activity.ActivityId); //need to improve methods
//                                                                                             // List<Steps> steps = StepsRepository.GetSteps();
//                newItem.EPDetails = DAL.EpRepository.GetEpShortDescription(newItem.EPDetailId);

//                activity.UserProfile = Users.GetUserProfile(activity.CreatedBy);
//                foreach (var goal in activity.TInspectionDetail)
//                {
//                    goal.MainGoal = maingoal.FirstOrDefault(x => x.MainGoalId == goal.MainGoalId);
//                    var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
//                    if (items.Count > 0)
//                    {
//                        stepsinfo.Append("<tr>");
//                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong><b>");
//                        stepsinfo.Append(goal.MainGoal.Goal);
//                        stepsinfo.Append("</b></strong></td></tr>");
//                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
//                        {
//                            inspectionStep.Steps = goal.MainGoal.Steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
//                            stepsinfo.Append("<tr>");
//                            stepsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
//                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((Utility.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
//                            stepsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
//                            stepsinfo.Append("</tr>");
//                        }
//                        stepsinfo.Append("<br />");
//                    }
//                }
//                if (activity.FloorAssetId.HasValue)
//                {
//                    string floorlocation = string.Empty;
//                    string stopName = string.Empty;
//                    activity.TFloorAssets = FloorAssetRepository.GetFloorAssetDescription(activity.FloorAssetId.Value);
//                    if (activity.TFloorAssets.Floor != null) { floorlocation = activity.TFloorAssets.Floor.FloorLocation; }
//                    if (activity.TFloorAssets.Stop != null) { stopName = $"{activity.TFloorAssets.Stop.StopName} ({activity.TFloorAssets.Stop.StopCode})"; }
//                    assetsUsers.ToList().AddRange(activity.TFloorAssets.Assets.Users);
//                    deviceinfo.Append("<tr>");
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Name:</strong>&nbsp;" + activity.TFloorAssets.Name.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + floorlocation.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>NearBy:</strong>&nbsp;" + activity.TFloorAssets.NearBy.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Stop Name:</strong>&nbsp;" + stopName.CastToNA() + "</td>");
//                    deviceinfo.Append("</tr>");
//                }
//                var objTInspectionFiles = new List<TInspectionFiles>();
//                objTInspectionFiles = DAL.Transaction.GetInspectionFiles(activity.ActivityId).ToList();
//                List<string> attachments = new List<string>();
//                foreach (var item in objTInspectionFiles)
//                {
//                    if (!string.IsNullOrEmpty(item.FilePath))
//                        attachments.Add(HostingEnvironment.MapPath(item.FilePath));
//                }
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/StepsFail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                if (newItem.EPDetails != null)
//                {
//                    emailBody = emailBody.Replace("{standardName}", newItem.EPDetails != null ? newItem.EPDetails.StandardEp : "Route Base Inspection (No EP link with this assets)");
//                    emailBody = emailBody.Replace("{EpNUmber}", newItem.EPDetails.EPNumber);
//                    emailBody = emailBody.Replace("{EpDescription}", newItem.EPDetails != null ? newItem.EPDetails.Description : string.Empty);
//                }
//                else { emailBody = emailBody.Replace("{standardName}", string.Empty); emailBody = emailBody.Replace("{EpDescription}", string.Empty); }
//                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                emailBody = emailBody.Replace("{inspector}", activity.UserProfile.Name);
//                emailBody = emailBody.Replace("{inspectorName}", activity.UserProfile.Name);
//                emailBody = emailBody.Replace("{inspectionDate}", Utility.Conversion.GetUtcToLocal(activity.ActivityInspectionDate.Value, timeZone).ToString("MMM dd, yyyy hh:mm tt "));  //activity.ActivityInspectionDate.Value.ToString("dd MMM yyyy hh:mm tt "));
//                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
//                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
//                string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"];
//                                                            //int emailType = (int)Enum.Parse(typeof(Utility.Enums.NotificationEmailType), HCF.Utility.Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//                                                            //string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
//                                                            //string toAddress = ccAddress;




//                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//                string toAddress = obj.NotificationEmail;

//                if (assetsUsers.Count > 0)
//                    toAddress = toAddress + "," + string.Join(",", assetsUsers.Select(i => i.Email));


//                string ccAddress = obj.NotificationCCEmail;
//                string subject = $"CRx: Inspection Failed Asset # {activity.TFloorAssets.DeviceNo} {(newItem.EPDetails != null ? ", " + newItem.EPDetails.StandardEp : string.Empty)}";
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                // status = false;
//            }

//            return true;

//        }


//        #region ILSM Emails 

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="currentUser"></param>
//        /// <param name="failEpDetail"></param>
//        /// <param name="activity"></param>
//        /// <returns></returns>
//        //public static bool SendIlsmFailMail(UserProfile currentUser, EPDetails failEpDetail, TInspectionActivity activity)
//        //{
//        //    TFloorAssets failAssets = null;
//        //    if (activity != null && (activity.ActivityType == 2))
//        //        failAssets = activity.TFloorAssets;

//        //    bool status = true;
//        //    string standardName = failEpDetail.Standard.TJCStandard;
//        //    string epNUmber = failEpDetail.EPNumber;
//        //    string epDescription = failEpDetail.Description;
//        //    if (activity != null)
//        //    {
//        //        string inspectorName = activity.UserProfile.Name;
//        //        DateTime date = DateTime.Now;
//        //        StringBuilder sb = new StringBuilder();
//        //        // Inspection list3 = failEPDetail.Inspection;//.FirstOrDefault();
//        //        //Guid activityId = activity.ActivityId;
//        //        string comment = activity.Comment;//string.Join(",", list3.TInspectionFiles.Where(x => x.Comment != null || x.Comment != string.Empty).Select(x => x.Comment.ToString()).ToArray());
//        //        var userName = activity.UserProfile.Name;
//        //        foreach (var goal in activity.TInspectionDetail)
//        //        {
//        //            date = DateTime.UtcNow;//goal.CreatedDate.Value;
//        //            var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
//        //            if (items.Count > 0)
//        //            {
//        //                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;>");
//        //                sb.Append("<tr>");
//        //                sb.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong>");
//        //                sb.Append(goal.MainGoal.Goal);
//        //                sb.Append("</strong></td></tr>");
//        //                foreach (InspectionSteps inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
//        //                {
//        //                    sb.Append("<tr>");
//        //                    sb.Append("<td style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
//        //                    sb.Append("<td align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + (Utility.Enums.Status)inspectionStep.Status + "</td>");
//        //                    sb.Append("<td align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
//        //                    sb.Append("</tr>");
//        //                }
//        //                sb.Append("</table><br />");
//        //            }
//        //        }

//        //        StringBuilder deviceinfo = new StringBuilder();
//        //        //deviceinfo.Append("<table>");

//        //        string bName = "";
//        //        string fName = "";
//        //        string aName = "";
//        //        string nearBy = "";
//        //        string locationName = "";
//        //        if (failAssets != null)
//        //        {

//        //            aName = failAssets.Name;
//        //            if (failAssets.Floor != null)
//        //            {
//        //                bName = failAssets.Floor.Building.BuildingName;
//        //                fName = failAssets.Floor.FloorName;
//        //                nearBy = failAssets.NearBy;
//        //                if (bName == null && fName == null && nearBy == null)
//        //                {
//        //                    locationName = "N/A";
//        //                }
//        //                else
//        //                {
//        //                    locationName = bName + " " + fName + " " + nearBy;
//        //                }
//        //            }
//        //            //byte[] imgBytes = Utility.Common.GenerateBarCode(failAssets.SerialNo);
//        //            //string imgString = Convert.ToBase64String(imgBytes);
//        //            //string htmlImage = String.Format("<img src=\"data:image/Bmp;base64,{0}\" style='height:90px; width:250px; margin: 0 0 -12px; 20px;'>", imgString);
//        //            deviceinfo.Append("<tr>");
//        //            deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Device Name:</strong>&nbsp;" + failAssets.Name + "</td>");
//        //            //deviceinfo.Append("<td width='189' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'><strong>Asset #:</strong>&nbsp;" + failAssets.DeviceNo + "</td>");
//        //            deviceinfo.Append("<td width='180' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;'><strong>Asset #:</strong></td>");
//        //            //deviceinfo.Append("<td width='269' style='font-family:arial; font-size:12px; color:#333; padding:5px;width:180px;'>" + htmlImage + "</span></td>");
//        //            deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + locationName + "</td>");
//        //            deviceinfo.Append("</tr>");
//        //        }
//        //        var objTInspectionFiles = DAL.Transaction.GetInspectionFiles(activity.ActivityId).ToList();
//        //        //StringBuilder Tfileinfo = new StringBuilder();
//        //        //Tfileinfo.Append("<tr>");
//        //        List<string> attachments = new List<string>();
//        //        foreach (var item in objTInspectionFiles)
//        //        {
//        //            if (!string.IsNullOrEmpty(item.FilePath))
//        //            {
//        //                attachments.Add(HostingEnvironment.MapPath(item.FilePath));
//        //            }
//        //            //Tfileinfo.Append("<td><img src=" + item.FilePath + " alt='file' style='width:100px;height:200px;'></td>");
//        //            //Tfileinfo.Append("<td><img src=" + WebConfigurationManager.AppSettings["BasePath"] + item.FilePath.Replace("~/", "") + " alt='file' style='width:100px;height:200px;'></td>");
//        //        }
//        //        //Tfileinfo.Append("</tr>");
//        //        //deviceinfo.Append("</table>");
//        //        try
//        //        {
//        //            var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/FailIlsmMail.htm"));
//        //            string emailBody = File.ReadAllText(fileInfo.FullName);
//        //            emailBody = emailBody.Replace("{standardName}", standardName);
//        //            emailBody = emailBody.Replace("{EpNUmber}", epNUmber);
//        //            emailBody = emailBody.Replace("{EpDescription}", epDescription);
//        //            emailBody = emailBody.Replace("{date}", $"{date:MMM dd, yyyy}");
//        //            emailBody = emailBody.Replace("{userName}", userName);
//        //            emailBody = emailBody.Replace("{failSteps}", sb.ToString());
//        //            emailBody = emailBody.Replace("{inspector}", inspectorName);
//        //            emailBody = emailBody.Replace("{Comment}", "<strong>Comment </strong>: " + comment);

//        //            if (failAssets != null && failAssets.FloorAssetsId > 0)
//        //                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
//        //            else
//        //                emailBody = emailBody.Replace("{deviceinfo}", "");
//        //            string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";
//        //            int emailType = (int)Enum.Parse(typeof(Enums.NotificationEmailType), Enums.NotificationEmailType.DeficiencyNotifications.ToString());
//        //            string ccAddress = NotificationRepository.GetNotificationEmail(emailType);
//        //            // string ccAddress = OrganizationRepository.GetOrganization().Email;
//        //            string toAddress = failEpDetail.AssigneeUser != null ? failEpDetail.AssigneeUser.Email : ccAddress;

//        //            string subject = $"CRx : Assets Fail {aName} {bName} {fName}";
//        //            string ilsmMailFrom = currentUser.Email ?? sender;
//        //            SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            ErrorLog.LogError(ex);
//        //            status = false;
//        //        }
//        //    }
//        //    return status;

//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="tilsm"></param>
//        /// <param name="notificationCategory"></param>
//        /// <param name="notificationMapping"></param>
//        public static bool SendIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping)
//        {
//            if (notificationMapping != null)
//            {
//                try
//                {
//                    StringBuilder assetsinfo = new StringBuilder();
//                    StringBuilder triggereps = new StringBuilder();
//                    foreach (var activity in tilsm.SourceInspection.TInspectionActivity)
//                    {
//                        assetsinfo.Append("<tr>");
//                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.TFloorAssets.Name + "</td>");
//                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.TFloorAssets.DeviceNo + "</td>");
//                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.UserProfile.Name + "</td>");
//                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.ActivityInspectionDate.Value.ToString("MMM dd, yyyy") + "</td>");
//                        assetsinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" + activity.Comment + "</td>");
//                        assetsinfo.Append("</tr>");
//                        assetsinfo.Append("<tr>");
//                        assetsinfo.Append("<td colspan='5' style='padding:15px;'>");
//                        assetsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse; font-family:arial; font-size:12px; color:#333; line-height:26px;'>");
//                        foreach (var goal in activity.TInspectionDetail)
//                        {
//                            var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
//                            if (items.Count > 0)
//                            {
//                                foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
//                                {
//                                    assetsinfo.Append("<tr>");
//                                    assetsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
//                                    assetsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
//                                    assetsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
//                                    assetsinfo.Append("</tr>");
//                                }
//                            }
//                        }
//                        assetsinfo.Append("</table>");
//                        assetsinfo.Append("</td></tr>");
//                    }
//                    if (tilsm.TIlsmEP != null)
//                    {
//                        foreach (var TriggerEp in tilsm.TIlsmEP)
//                        {
//                            triggereps.Append("<tr>");
//                            triggereps.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.StandardEP + "</td>");
//                            triggereps.Append("<td width='931' style='color: #333; font-family: arial; font-size: 12px; padding: 5px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.Description + "</td>");
//                            triggereps.Append("</tr>");
//                        }
//                    }
//                    var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/IlsmMail.htm"));
//                    string emailBody = File.ReadAllText(fileInfo.FullName);
//                    emailBody = emailBody.Replace("{standardName}", tilsm.SourceInspection.EPDetails.StandardEp);
//                    emailBody = emailBody.Replace("{EpNUmber}", tilsm.SourceInspection.EPDetails.EPNumber);
//                    emailBody = emailBody.Replace("{EpDescription}", tilsm.SourceInspection.EPDetails.Description);
//                    emailBody = emailBody.Replace("{TriggerEPs}", triggereps.ToString());
//                    emailBody = emailBody.Replace("{date}", $"{tilsm.llsmDate:MMM dd, yyyy}");
//                    emailBody = emailBody.Replace("{inspector}", tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().UserProfile.Name);
//                    emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
//                    emailBody = emailBody.Replace("{Errortext}", $"ILSM Incident #: {tilsm.IncidentId} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Assets.Name}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.FloorName}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.Building.BuildingName} created for the following:");
//                    List<string> attachments = new List<string>();
//                    //string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";

//                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//                    string toAddress = obj.NotificationEmail;
//                    string ccAddress = obj.NotificationCCEmail;
//                    string subject = $"CRx : ILSM Incident #: {tilsm.IncidentId} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault()?.TFloorAssets.Assets.Name} - {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.FloorName}, {tilsm.SourceInspection.TInspectionActivity.FirstOrDefault().TFloorAssets.Floor.Building.BuildingName}";
//                    // string ilsmMailFrom = sender;
//                    SendMail(toAddress, subject, emailBody, MailFrom, attachments, ccAddress);
//                }
//                catch (Exception ex)
//                {
//                    ErrorLog.LogError(ex);
//                    //status = false;
//                }
//            }
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="tilsm"></param>
//        /// <param name="notificationCategory"></param>
//        /// <param name="notificationMapping"></param>
//        public static bool SendManualIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping)
//        {
//            if (notificationMapping != null)
//            {
//                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
//                string subjectlocation = (tilsm.Buildings.Count > 0) ? string.Join(",", tilsm.Buildings.Select(x => x.BuildingName)) : "NA";
//                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
//                try
//                {
//                    StringBuilder assetsinfo = new StringBuilder();
//                    StringBuilder triggereps = new StringBuilder();
//                    if (tilsm.TIlsmEP != null)
//                    {
//                        foreach (var TriggerEp in tilsm.TIlsmEP)
//                        {
//                            triggereps.Append("<tr>");
//                            triggereps.Append("<td width='189' valign='top' style='color: #333; font-family: arial; font-size: 12px; padding:5px; width: 140px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.StandardEP + "</td>");
//                            triggereps.Append("<td width='931' style='color: #333; font-family: arial; font-size: 12px; padding: 5px;border:#ccc solid 1px;'>" + TriggerEp.StandardEp.Description + "</td>");
//                            triggereps.Append("</tr>");
//                        }
//                    }
//                    var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/ManualIlsmMail.htm"));
//                    string emailBody = File.ReadAllText(fileInfo.FullName);
//                    emailBody = emailBody.Replace("{name}", $"{tilsm.Name} (on {tilsm.llsmDate:MMM dd, yyyy})");
//                    emailBody = emailBody.Replace("{reason}", tilsm.Notes);
//                    emailBody = emailBody.Replace("{location}", location);
//                    emailBody = emailBody.Replace("{TriggerEPs}", triggereps.ToString());
//                    //emailBody = emailBody.Replace("{date}", $"{tilsm.llsmDate:MMM dd, yyyy}");
//                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
//                    //emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
//                    emailBody = emailBody.Replace("{ilsmtext}", $"ILSM Incident #: {tilsm.IncidentId} - {tilsm.Name} created for the following:");
//                    List<string> attachments = new List<string>();
//                    //string sender = AppSettings.SenderMailFrom; //System.Configuration.ConfigurationManager.AppSettings["SenderMailFrom"]; //"holyname@hcfcompliance.com";

//                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//                    string toAddress = obj.NotificationEmail;
//                    string ccAddress = obj.NotificationCCEmail;
//                    string subject = $"CRx : ILSM Incident #: {tilsm.IncidentId} - {tilsm.Name} - Location ({subjectlocation}) ";
//                    // string ilsmMailFrom = sender;
//                    SendMail(toAddress, subject, emailBody, MailFrom, attachments, ccAddress);
//                }
//                catch (Exception ex)
//                {
//                    ErrorLog.LogError(ex);
//                    //status = false;
//                }
//            }
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="tilsmId"></param>
//        /// <param name="notificationCategory"></param>
//        public static bool SendIlsmCloseMail(int tilsmId, int notificationCategory)
//        {
//            TIlsm tilsm = IlsmRepository.GetIlsmInspection(tilsmId);//IlsmRepository.GetIlsm().FirstOrDefault(x => x.TIlsmId == tilsmId);
//            try
//            {
//                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
//                string subjectlocation = (tilsm.Buildings.Count > 0) ? string.Join(",", tilsm.Buildings.Select(x => x.BuildingName)) : "NA";
//                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
//                StringBuilder assetsinfo = new StringBuilder();
//                if (tilsm != null)
//                {
//                    if (tilsm.TIlsmfloorAssets.Count > 0)
//                    {
//                        assetsinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Assets Details:</h4>");
//                        assetsinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                        assetsinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Device Name</strong></td>" +
//                            "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Asset #</strong></td>" +
//                            "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Inspector Name </strong></td>" +
//                            "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Inspection Date </strong></td>" +
//                            "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comment </strong></td>" +
//                            "</tr></thead>");
//                        assetsinfo.Append("<tbody>");
//                        foreach (TIlsmfloorAssets floorAssets in tilsm.TIlsmfloorAssets)
//                        {
//                            assetsinfo.Append("<tr>");
//                            assetsinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                floorAssets.TInspectionActivity.TFloorAssets.Name + "</td>");
//                            assetsinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                floorAssets.TInspectionActivity.TFloorAssets.DeviceNo + "</td>");
//                            assetsinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                floorAssets.TInspectionActivity.UserProfile.Name + "</td>");
//                            assetsinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                floorAssets.TInspectionActivity.ActivityInspectionDate.Value.ToString("MMM dd, yyyy") +
//                                "</td>");
//                            assetsinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                floorAssets.TInspectionActivity.Comment + "</td>");
//                            assetsinfo.Append("</tr>");
//                        }
//                        assetsinfo.Append("</tbody>");
//                        assetsinfo.Append("</table><br />");
//                    }

//                    FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}",
//                        @"Templates/IlsmCloseMail.html"));
//                    string emailBody = File.ReadAllText(fileInfo.FullName);
//                    emailBody = emailBody.Replace("{name}", $"{tilsm.Name} (on {tilsm.CreatedDate:MMM dd, yyyy})");
//                    emailBody = emailBody.Replace("{reason}", (!string.IsNullOrEmpty(tilsm.Notes) ? tilsm.Notes : "NA"));
//                    emailBody = emailBody.Replace("{location}", location);
//                    //emailBody = emailBody.Replace("{date}", $"{tilsm.CreatedDate:MMM dd, yyyy}");
//                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
//                    emailBody = emailBody.Replace("{assetsinfo}", assetsinfo.ToString());
//                    emailBody = emailBody.Replace("{Errortext}",
//                        //string.Format("ILSM # {0} closed for the following:", tilsm.IncidentId)
//                        $"CRx Assisted ILSM {tilsm.IncidentId}-{tilsm.Name}-{subjectlocation} has been closed and filed in the ILSM Binder for reference. Attached to this e-mail is a finalized report"
//                        );
//                    List<string> attachments = new List<string>();
//                    if (!string.IsNullOrEmpty(tilsm.FilePath))
//                        attachments.Add(Common.FilePath(tilsm.FilePath));
//                    string sender = AppSettings.SenderMailFrom;
//                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnClosed));
//                    string toAddress = obj.NotificationEmail;
//                    string ccAddress = obj.NotificationCCEmail;
//                    string subject = $"CRx Assisted ILSM Closed:- {tilsm.IncidentId}-{tilsm.Name}-{location}";
//                    var ilsmMailFrom = sender;
//                    SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                //status = false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="tilsmId"></param>
//        /// <param name="notificationCategory"></param>
//        public static bool SendIlsmReOpenMail(int tilsmId, int notificationCategory)
//        {
//            TIlsm tilsm = IlsmRepository.GetIlsmInspection(tilsmId);//IlsmRepository.GetIlsm().FirstOrDefault(x => x.TIlsmId == tilsmId);
//            try
//            {
//                //string location = (tilsm.Floor != null) ? tilsm.Floor.FloorLocation : "NA";
//                string location = (tilsm.Buildings.Count > 0) ? getlocation(tilsm.Buildings) : "NA";
//                if (tilsm != null)
//                {
//                    FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}",
//                        @"Templates/IlsmReOpenMail.html"));
//                    string emailBody = File.ReadAllText(fileInfo.FullName);
//                    emailBody = emailBody.Replace("{name}", tilsm.Name);
//                    emailBody = emailBody.Replace("{reason}", (!string.IsNullOrEmpty(tilsm.Notes) ? tilsm.Notes : "NA"));
//                    emailBody = emailBody.Replace("{location}", location);
//                    emailBody = emailBody.Replace("{date}", $"{tilsm.CreatedDate:MMM dd, yyyy}");
//                    emailBody = emailBody.Replace("{inspector}", tilsm.Inspector.Name);
//                    emailBody = emailBody.Replace("{Errortext}",
//                        string.Format("ILSM # {0} Reopen for the following:", tilsm.IncidentId));
//                    List<string> attachments = new List<string>();
//                    string sender = AppSettings.SenderMailFrom;
//                    NotificationEmails obj = GetNotificationEmail(notificationCategory, tilsm.BuildingIds, tilsm.FloorIds, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//                    string toAddress = obj.NotificationEmail;
//                    string ccAddress = obj.NotificationCCEmail;
//                    string subject = $"CRx : ILSM Reopened # {tilsm.IncidentId}";
//                    var ilsmMailFrom = sender;
//                    SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                //status = false;
//            }

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="buildings"></param>
//        /// <returns></returns>
//        private static string getlocation(List<Buildings> buildings)
//        {
//            string location = string.Empty;
//            foreach (var build in buildings)
//            {
//                location = location + build.BuildingName;
//                if (build.Floor.Count > 0)
//                {
//                    location = location + $": ({string.Join(",", build.Floor.Select(x => x.FloorName))})";
//                }
//                location = location + (buildings.Count > 1 ? $"," : "");
//            }
//            return location;
//        }


//        #endregion

//        #region Tips Emails

//        private static bool SendTipsEmail(Tips newItem, string mode, int notificationCategory)
//        {
//            try
//            {
//                if (newItem.CreatedBy != null)
//                {
//                    var ownerUser = Users.GetUserProfile(newItem.CreatedBy.Value);
//                    var emailBody = File.ReadAllText(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/TipCreated.html"));
//                    emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
//                    emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                    emailBody = emailBody.Replace("{Errortext}", TipsText(newItem, mode));
//                    string sender = AppSettings.SenderMailFrom;

//                    var obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty);
//                    var toAddress = obj.NotificationEmail;

//                    if (ownerUser.UserId > 0)
//                        toAddress = toAddress + "," + string.Join(",", ownerUser.Email);
//                    var ccAddress = obj.NotificationCCEmail;
//                    var subject = TipsSubject(mode);
//                    SendMail(toAddress, subject, emailBody, sender, null, ccAddress);
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//            }

//            return true;
//        }

//        public static bool SendTipsFailMail(Tips newItem, int notificationCategory)
//        {
//            return SendTipsEmail(newItem, "SendTipsFailMail", notificationCategory);
//        }
//        public static bool SendTipsPassMail(Tips newItem, int notificationCategory)
//        {
//            return SendTipsEmail(newItem, "SendTipsPassMail", notificationCategory);
//        }
//        public static bool SendTipsCreatedMail(Tips newItem, int notificationCategory)
//        {
//            return SendTipsEmail(newItem, "SendTipsCreatedMail", notificationCategory);

//        }
//        public static bool SendTipsUpdatedMail(Tips newItem, int notificationCategory)
//        {
//            return SendTipsEmail(newItem, "SendTipsUpdatedMail", notificationCategory);
//        }

//        private static string TipsText(Tips newItem, string mode)
//        {
//            string text = string.Empty;
//            switch (mode)
//            {
//                case "SendTipsFailMail":
//                    text = string.Format("Tip {0} is rejected.", newItem.Title);
//                    break;
//                case "SendTipsPassMail":
//                    text = string.Format("Tip {0} is approved.", newItem.Title);
//                    break;
//                case "SendTipsCreatedMail":
//                    text = string.Format("A Tip having Title:{0} is created and pending for approval", newItem.Title);
//                    break;
//                case "SendTipsUpdatedMail":
//                    text = string.Format("A Tip have Title: {0} is updated and pending for approval", newItem.Title);
//                    break;
//            }
//            return text;
//        }

//        private static string TipsSubject(string mode)
//        {
//            string subject = string.Empty;
//            switch (mode)
//            {
//                case "SendTipsFailMail":
//                    subject = "CRx: Tip Rejected";
//                    break;
//                case "SendTipsPassMail":
//                    subject = "CRx: Tip Approved";
//                    break;
//                case "SendTipsCreatedMail":
//                    subject = "CRx: Tip Created";
//                    break;
//                case "SendTipsUpdatedMail":
//                    subject = "CRx: Tip Updated";
//                    break;
//            }
//            return subject;
//        }

//        #endregion

//        #region Permit Emails

//        public static bool SendPermitCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId)
//        {

//            bool status = false;
//            try
//            {
//                var toAddress = string.Empty;
//                var ccAddress = string.Empty;
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendPermitCreatedMail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{template}", GetPermitTemplate(objPermitEmailModel));

//                string emailvendorBody = File.ReadAllText(fileInfo.FullName);
//                emailvendorBody = emailvendorBody.Replace("{template}", GetVendorPermitTemplate(objPermitEmailModel));

//                var sender = AppSettings.SenderMailFrom;
//                var obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId, objPermitEmailModel.EventId);
//                if (obj != null)
//                {
//                    toAddress = !string.IsNullOrEmpty(obj.NotificationEmail) ? obj.NotificationEmail : string.Empty;
//                    ccAddress = !string.IsNullOrEmpty(obj.NotificationCCEmail) ? obj.NotificationCCEmail : string.Empty;
//                }
//                if (objPermitEmailModel.SendMailToApprover.HasValue && objPermitEmailModel.SendMailToApprover.Value)
//                {
//                    obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId, Convert.ToInt32(Enums.NotificationEvent.OnApproval));
//                    if (obj != null)
//                    {
//                        toAddress = toAddress + "," + obj.NotificationEmail;
//                        ccAddress = ccAddress + "," + obj.NotificationCCEmail;
//                    }
//                }
//                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status approve/reject/hold for same user in the notification matrix will not get the mail twice
//                {
//                }
//                else
//                {
//                    if (objPermitEmailModel.ApprovalStatus != 6)
//                    {
//                        if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
//                            toAddress = toAddress + "," + objPermitEmailModel.RequesterEmail;
//                    }
//                    if (objPermitEmailModel.EventId == 8)
//                    {
//                        toAddress = objPermitEmailModel.RequesterEmail;
//                        ccAddress = "";
//                    }
//                }
//                string subject = GetPermitSubject(objPermitEmailModel); //$"{objPermitEmailModel.PermitType} # {objPermitEmailModel.PermitNo} ";
//                string mailFrom = sender;

//                SendMail(toAddress, subject, ccAddress, emailBody, mailFrom, fileBytes, filename);
//                if (objPermitEmailModel.ApprovalStatus != 6)
//                {
//                    if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status request/create for vendor user 
//                    {
//                        //Send to Vendor 
//                        toAddress = "";
//                        if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
//                            toAddress = objPermitEmailModel.RequesterEmail;

//                        ccAddress = "";

//                        if (!string.IsNullOrEmpty(toAddress))
//                            SendMail(toAddress, subject, ccAddress, emailvendorBody, mailFrom, fileBytes, filename);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                // status = false;
//            }
//            return status;
//        }
//        public static bool SendConstruictionCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId)
//        {

//            bool status = false;
//            try
//            {
//                var toAddress = string.Empty;
//                var ccAddress = string.Empty;
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendPermitCreatedMail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{template}", GetPermitTemplate(objPermitEmailModel));

//                string emailvendorBody = File.ReadAllText(fileInfo.FullName);
//                emailvendorBody = emailvendorBody.Replace("{template}", GetVendorPermitTemplate(objPermitEmailModel));

//                var sender = AppSettings.SenderMailFrom;
//                var obj = GetNotificationEmail(notificationCategory, buildingId, floorId, SiteId);
//                if (obj != null)
//                {
//                    toAddress = obj.NotificationEmail;
//                    ccAddress = obj.NotificationCCEmail;
//                }

//                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status approve/reject/hold for same user in the notification matrix will not get the mail twice
//                {
//                }
//                else
//                {
//                    if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
//                        toAddress = toAddress + "," + objPermitEmailModel.RequesterEmail;
//                }
//                string subject = GetPermitSubject(objPermitEmailModel); //$"{objPermitEmailModel.PermitType} # {objPermitEmailModel.PermitNo} ";
//                string mailFrom = sender;
//                SendMail(toAddress, subject, ccAddress, emailBody, mailFrom, fileBytes, filename);

//                if (objPermitEmailModel.ApprovalStatus == 2 || objPermitEmailModel.ApprovalStatus == 4)// this condition for status request/create for vendor user 
//                {
//                    //Send to Vendor 
//                    toAddress = "";
//                    if (!string.IsNullOrEmpty(objPermitEmailModel.RequesterEmail))
//                        toAddress = objPermitEmailModel.RequesterEmail;

//                    ccAddress = "";

//                    if (!string.IsNullOrEmpty(toAddress))
//                        SendMail(toAddress, subject, ccAddress, emailvendorBody, mailFrom, fileBytes, filename);
//                }
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//                // status = false;
//            }
//            return status;
//        }
//        private static string GetPermitTemplate(PermitEmailModel objPermitEmailModel)
//        {
//            string templatetext;

//            string projectDetails = string.Empty;
//            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
//                projectDetails = $" for {objPermitEmailModel.ProjectName}";

//            if (objPermitEmailModel.ApprovalStatus == 1) // Approved 
//            {
//                templatetext =
//                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been approved by  " +
//                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the approved permit."; // {objPermitEmailModel.PermitType}.";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 0) // Rejected
//            {

//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} has been rejected by  " +
//                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
//                               $"You can update and resubmit the permit if required. <br /> Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx " +
//                $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";
//                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 3) // Pending/On hold
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} put on hold by " +
//                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.  <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
//                               $"Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
//                               $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";
//                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 4) // Created
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} created by " +
//                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent for furthur process <br /> " +
//                                 $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";
//                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 5) // Closed 
//            {
//                templatetext =
//                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been closed by  " +
//                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the closed permit."; // {objPermitEmailModel.PermitType}.";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 6) // Reviewed 
//            {
//                templatetext =
//                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been reviewed by  " +
//                        $"{objPermitEmailModel.Reviewer} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the reviewed permit."; // {objPermitEmailModel.PermitType}.";

//            }
//            else // Requested
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} requested by " +
//                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} is awaiting your approval <br />" +
//                               //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
//                               $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";

//            }
//            return templatetext;
//        }
//        private static string GetVendorPermitTemplate(PermitEmailModel objPermitEmailModel)
//        {
//            string templatetext;

//            string projectDetails = string.Empty;
//            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
//                projectDetails = $" for {objPermitEmailModel.ProjectName}";

//            if (objPermitEmailModel.ApprovalStatus == 1) // Approved 
//            {
//                templatetext =
//                        $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails}  has been approved by  " +
//                        $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.<br /> See attached for a PDF of the approved permit."; // {objPermitEmailModel.PermitType}.";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 0) // Rejected
//            {

//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} has been rejected by  " +
//                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
//                               $"You can update and resubmit the permit if required. <br /> Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
//                                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";
//                                $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 3) // Pending/On hold
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} put on hold by " +
//                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}.  <br /> The following reason was given: {objPermitEmailModel.Reason}. <br />" +
//                               $"Click here to review {objPermitEmailModel.PermitType.GetDescription()} in CRx" +
//                                //({WebBaseUrl + objPermitEmailModel.PermitUrl})";
//                                $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 4) // Created
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} created by " +
//                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent for furthur process <br /> " +
//                                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
//                                $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 5) // closed
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} closed by " +
//                               $"{objPermitEmailModel.Approver} of {objPermitEmailModel.Company}. <br /> ";

//            }
//            else if (objPermitEmailModel.ApprovalStatus == 6) // reviewed
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} reviewed by " +
//                               $"{objPermitEmailModel.Reviewer} of {objPermitEmailModel.Company}. <br /> ";

//            }
//            else // Requested
//            {
//                templatetext = $"The {objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {projectDetails} requested by " +
//                               $"{objPermitEmailModel.Requestor} of {objPermitEmailModel.Company} has been sent in for approval. <br />" +
//                                //$"({WebBaseUrl + objPermitEmailModel.PermitUrl})";
//                                $"(<a href='{WebBaseUrl + objPermitEmailModel.PermitUrl}'>{WebBaseUrl + objPermitEmailModel.PermitUrl}</a>)";

//            }
//            return templatetext;
//        }
//        private static string GetPermitSubject(PermitEmailModel objPermitEmailModel)
//        {
//            string projectDetails = string.Empty;
//            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
//                projectDetails = $" {objPermitEmailModel.ProjectName}";

//            string subject;
//            switch (objPermitEmailModel.ApprovalStatus)
//            {
//                // Approved 
//                case 1:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(APPROVED),"} {projectDetails} approved by {objPermitEmailModel.Approver}";
//                    break;
//                // Rejected 
//                case 0:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo}{"(REJECTED),"} {projectDetails} rejected by {objPermitEmailModel.Approver}";
//                    break;
//                // Pending/On hold
//                case 3:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(ON HOLD),"} {projectDetails} On hold by {objPermitEmailModel.Approver}";
//                    break;
//                //Created
//                case 4:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CREATED),"} {projectDetails} created by {objPermitEmailModel.Requestor}";
//                    break;
//                //closed
//                case 5:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CLOSED),"} {projectDetails} closed by {objPermitEmailModel.Approver}";
//                    break;
//                //Reviewed
//                case 6:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(REVIEWED),"} {projectDetails} reviewed by {objPermitEmailModel.Reviewer}";
//                    break;
//                // Requested
//                default:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {(string.IsNullOrEmpty(objPermitEmailModel.IsRequestEdited) ? "(REQUESTED)," : "(UPDATED),")} {projectDetails} requested by {objPermitEmailModel.Requestor}";
//                    break;
//            }
//            return subject;
//        }
//        private static string GetConstructionubject(PermitEmailModel objPermitEmailModel)
//        {
//            string projectDetails = string.Empty;
//            if (!string.IsNullOrEmpty(objPermitEmailModel.ProjectName))
//                projectDetails = $" {objPermitEmailModel.ProjectName}";

//            string subject;
//            switch (objPermitEmailModel.ApprovalStatus)
//            {
//                // Approved 
//                case 1:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(APPROVED),"} {projectDetails} approved by {objPermitEmailModel.Approver}";
//                    break;
//                // Rejected 
//                case 0:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo}{"(REJECTED),"} {projectDetails} rejected by {objPermitEmailModel.Approver}";
//                    break;
//                // Pending/On hold
//                case 3:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(ON HOLD),"} {projectDetails} On hold by {objPermitEmailModel.Approver}";
//                    break;
//                //Created
//                case 4:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(CREATED),"} {projectDetails} created by {objPermitEmailModel.Requestor}";
//                    break;
//                // Requested
//                default:
//                    subject = $"{objPermitEmailModel.PermitType.GetDescription()} #{objPermitEmailModel.PermitNo} {"(REQUESTED),"} {projectDetails} requested by {objPermitEmailModel.Requestor}";
//                    break;
//            }
//            return subject;
//        }
//        #endregion

//        #region Fire drill Mail
//        public static bool SendFireDrillCreatedMail(TExercises exercises, int notificationCategory)
//        {
//            return SendFireDrillEmail(exercises, "SendFiredrillCreatedMail", notificationCategory, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//        }

//        public static void SendFireDrillReminderMail(TExercises exercises, int notificationCategory)
//        {
//            SendFireDrillEmail(exercises, "SendFireDrillReminderMail", notificationCategory, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//        }

//        public static void SendFireDrillMailOnDone(TExercises exercises, int notificationCategory)
//        {
//            SendFireDrillEmail(exercises, "SendFireDrillMailOnDone", notificationCategory, Convert.ToInt32(Enums.NotificationEvent.OnClosed));
//        }

//        public static void SendFireDrillOnDueDatePassed(TExercises exercises, int notificationCategory)
//        {
//            SendFireDrillEmail(exercises, "SendFireDrillOnDueDatePassed", notificationCategory, Convert.ToInt32(Enums.NotificationEvent.OnDueDate));
//        }

//        private static bool SendFireDrillEmail(TExercises exercises, string mode, int notificationCategory, int notificationeventId)
//        {
//            if (exercises.CreatedBy != 0)
//            {
//                var ownerUser = Users.GetUserProfile(exercises.CreatedBy);
//                var buildingname = BuildingsRepository.GetBuildings().Where(x => x.BuildingId == exercises.BuildingId).Select(x => x.BuildingName).FirstOrDefault();
//                var emailBody = File.ReadAllText(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/FiredrillCreated.html"));
//                emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
//                emailBody = emailBody.Replace("{Errortext}", FiredrillText(exercises, mode, buildingname, ownerUser));
//                string sender = AppSettings.SenderMailFrom;

//                var obj = GetNotificationEmail(notificationCategory, exercises.BuildingId.ToString(), string.Empty, string.Empty, notificationeventId);
//                var toAddress = obj.NotificationEmail;

//                if (ownerUser.UserId > 0)
//                    toAddress = toAddress + "," + string.Join(",", ownerUser.Email);
//                var ccAddress = obj.NotificationCCEmail;
//                var subject = FiredrillSubject(mode);
//                SendMail(toAddress, subject, emailBody, sender, null, ccAddress);
//            }

//            return true;
//        }

//        private static string FiredrillText(TExercises newItem, string mode, string buildingname, UserProfile ownerUser)
//        {
//            string text = string.Empty;
//            switch (mode)
//            {
//                case "SendFiredrillCreatedMail":
//                    DateTime Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
//                    string date = $"{Firedrilldate:MMM dd, yyyy}";
//                    string Done = newItem.Status == 0 ? "Plan" : "Done";
//                    //var announced = newItem.Announced == false ? "No" : "Yes";
//                    text = string.Format("A FIRE DRILL SCHEDULE <br /> Building : {0} <br /> Created Date : {1} <br />  Schedule Date : {2} <br /> Time : {3} <br /> Planned By : {4} <br /> Status : {5}"
//                        , buildingname, $"{DateTime.Now:MMM dd, yyyy}", date, newItem.StartTime, ownerUser.FullName, Done);
//                    break;
//                case "SendFireDrillReminderMail":
//                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
//                    string duedate = $"{Firedrilldate:MMM dd, yyyy}";
//                    //var Dueannounced = newItem.Announced == false ? "No" : "Yes";
//                    text = string.Format("A FireDrill is Planned <br /> Building : {0} <br />  Schedule Date : {1} <br /> Time : {2} <br /> Planned By : {3}"
//                        , buildingname, duedate, newItem.StartTime, ownerUser.FullName);
//                    break;
//                case "SendFireDrillMailOnDone":
//                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
//                    date = $"{Firedrilldate:MMM dd, yyyy}";//date.Replace("12:00:00 AM", "");
//                    //announced = newItem.Announced == false ? "No" : "Yes";
//                    text = string.Format("A FIRE DRILL DONE <br /> Building : {0} <br />Completed Date : {1} <br />  Schedule Date : {2} <br /> Time : {3} <br /> Planned By : {4}"
//                        , buildingname, $"{DateTime.Now:MMM dd, yyyy}", date, newItem.StartTime, ownerUser.FullName);
//                    break;
//                case "SendFireDrillOnDueDatePassed":
//                    Firedrilldate = Utility.Conversion.ConvertToDateTime(newItem.DateTimeSpan);
//                    date = $"{Firedrilldate:MMM dd, yyyy}";//date.Replace("12:00:00 AM", "");
//                    //announced = newItem.Announced == false ? "No" : "Yes";
//                    text = string.Format("A FIRE DRILL DueDate Passed <br /> Building : {0} <br />Schedule Date : {1} <br /> Time : {2} <br /> Planned By : {3}"
//                        , buildingname, date, newItem.StartTime, ownerUser.FullName);
//                    break;

//            }
//            return text;
//        }

//        private static string FiredrillSubject(string mode)
//        {
//            string subject = string.Empty;
//            switch (mode)
//            {
//                case "SendFiredrillCreatedMail":
//                    subject = "CRx: FireDrill Scheduled";
//                    break;
//                case "SendFireDrillReminderMail":
//                    subject = "CRx: FireDrill Reminder";
//                    break;
//                case "SendFireDrillOnDueDatePassed":
//                    subject = "CRx: FireDrill Due Date Passed";
//                    break;
//                case "SendFireDrillMailOnDone":
//                    subject = "CRx: FireDrill Done";
//                    break;
//            }
//            return subject;
//        }

//        #endregion

//        #region Common Emails
//        public static bool SendCommonEmail(NotificationEmails objnotificationEmails, object objects)
//        {
//            List<string> toemails = new List<string>();
//            List<string> ccemails = new List<string>();
//            if (!string.IsNullOrEmpty(objnotificationEmails.NotificationEmail))
//                toemails = objnotificationEmails.NotificationEmail.Split(',').ToList();
//            if (!string.IsNullOrEmpty(objnotificationEmails.NotificationCCEmail))
//                ccemails = objnotificationEmails.NotificationCCEmail.Split(',').ToList();
//            var newNotificationEmail = NotificationRepository.GetNotificationEmailByCatID(objnotificationEmails.NotificationCatId, objnotificationEmails.BuildingId.ToString(), objnotificationEmails.FloorId.ToString(),
//                                      objnotificationEmails.SiteId.ToString(), objnotificationEmails.NotificationEventId);
//            if (newNotificationEmail != null)
//            {
//                toemails.AddRange(newNotificationEmail.NotificationEmail.Split(',').ToList());
//                ccemails.AddRange(newNotificationEmail.NotificationCCEmail.Split(',').ToList());
//            }
//            switch ((Enums.NotificationCategory)newNotificationEmail.NotificationCatId)
//            {
//                case Enums.NotificationCategory.Inspection:
//                    switch ((Enums.NotificationEvent)newNotificationEmail.NotificationEventId)
//                    {
//                        case Enums.NotificationEvent.OnDeficiencies: SendInspectionMailOnDeficiencies((HCF.BDO.Inspection)objects, toemails, ccemails); break;
//                    }
//                    break;
//                case Enums.NotificationCategory.ILSM: break;
//                case Enums.NotificationCategory.WorkOrder: break;
//                case Enums.NotificationCategory.RoundsInspection: break;
//                case Enums.NotificationCategory.TimeToResolveToRA: break;
//                case Enums.NotificationCategory.ConfigurationalItem: break;
//                case Enums.NotificationCategory.All: break;
//                case Enums.NotificationCategory.RouteBasedInspection: break;
//                case Enums.NotificationCategory.PCRA: break;
//                case Enums.NotificationCategory.FSBP: break;
//                case Enums.NotificationCategory.MOP: break;
//                case Enums.NotificationCategory.LSDAR: break;
//                case Enums.NotificationCategory.HWP: break;
//                case Enums.NotificationCategory.CP: break;
//                case Enums.NotificationCategory.FMC: break;
//                case Enums.NotificationCategory.Firedrill: break;
//                case Enums.NotificationCategory.CRA: break;
//                case Enums.NotificationCategory.ICRA: break;
//                case Enums.NotificationCategory.AutoReport: break;
//                case Enums.NotificationCategory.Binders:
//                    switch ((Enums.NotificationEvent)newNotificationEmail.NotificationEventId)
//                    {
//                        case Enums.NotificationEvent.OnExpiration: SendDocumentMailOnExpiration((HCF.BDO.HttpResponseMessage)objects, toemails, ccemails); break;
//                    }
//                    break;
//            }

//            return true;
//        }

//        #region Inspection  

//        private static bool SendInspectionMailOnDeficiencies(Inspection newItem, List<string> toemails, List<string> ccemails)
//        {
//            string timeZone = OrganizationRepository.GetOrganizations().FirstOrDefault(x => x.ClientNo == Convert.ToInt32(HcfSession.ClientNo))?.Timezone;
//            try
//            {
//                StringBuilder stepsinfo = new StringBuilder();
//                StringBuilder deviceinfo = new StringBuilder();
//                var assetsUsers = new List<UserProfile>();
//                var activity = newItem.TInspectionActivity.FirstOrDefault();
//                var maingoal = MainGoalRepository.GetActivityMainGoals(activity.ActivityId); //need to improve methods
//                                                                                             // List<Steps> steps = StepsRepository.GetSteps();
//                newItem.EPDetails = DAL.EpRepository.GetEpShortDescription(newItem.EPDetailId);

//                activity.UserProfile = Users.GetUserProfile(activity.CreatedBy);
//                foreach (var goal in activity.TInspectionDetail)
//                {
//                    goal.MainGoal = maingoal.FirstOrDefault(x => x.MainGoalId == goal.MainGoalId);
//                    var items = goal.InspectionSteps.Where(x => x.Status == 0).ToList();
//                    if (items.Count > 0)
//                    {
//                        stepsinfo.Append("<tr>");
//                        stepsinfo.Append("<td colspan='3' style='border:#ccc solid 1px; font-family:arial; font-size:14px; color:#333; padding:5px;>'<strong><b>");
//                        stepsinfo.Append(goal.MainGoal.Goal);
//                        stepsinfo.Append("</b></strong></td></tr>");
//                        foreach (var inspectionStep in goal.InspectionSteps.Where(x => x.Status == 0))
//                        {
//                            inspectionStep.Steps = goal.MainGoal.Steps.FirstOrDefault(x => x.StepsId == inspectionStep.StepsId);
//                            stepsinfo.Append("<tr>");
//                            stepsinfo.Append("<td width='34%' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:50%'>" + inspectionStep.Steps.Step + "</td>");
//                            stepsinfo.Append("<td width='41%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + System.Text.RegularExpressions.Regex.Replace(((Utility.Enums.MailStatus)inspectionStep.Status).ToString(), "(?<=[a-z])(?=[A-Z])", "-") + "</td>");
//                            stepsinfo.Append("<td width='25%' align='center' style='border:#ccc solid 1px; font-family:arial; font-size:12px; color:#333; padding:5px; width:25%'>" + inspectionStep.Comments + "</td>");
//                            stepsinfo.Append("</tr>");
//                        }
//                        stepsinfo.Append("<br />");
//                    }
//                }
//                if (activity.FloorAssetId.HasValue)
//                {
//                    string floorlocation = string.Empty;
//                    string stopName = string.Empty;
//                    activity.TFloorAssets = FloorAssetRepository.GetFloorAssetDescription(activity.FloorAssetId.Value);
//                    if (activity.TFloorAssets.Floor != null) { floorlocation = activity.TFloorAssets.Floor.FloorLocation; }
//                    if (activity.TFloorAssets.Stop != null) { stopName = $"{activity.TFloorAssets.Stop.StopName} ({activity.TFloorAssets.Stop.StopCode})"; }
//                    assetsUsers.ToList().AddRange(activity.TFloorAssets.Assets.Users);
//                    deviceinfo.Append("<tr>");
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Asset #:</strong>&nbsp;" + activity.TFloorAssets.DeviceNo.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Name:</strong>&nbsp;" + activity.TFloorAssets.Name.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Location:</strong>&nbsp;" + floorlocation.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Nearby:</strong>&nbsp;" + activity.TFloorAssets.NearBy.CastToNA());
//                    deviceinfo.Append("<td style='font-family:arial; font-size:12px; color:#333; padding:5px;'><strong>Stop Name:</strong>&nbsp;" + stopName.CastToNA() + "</td>");
//                    deviceinfo.Append("</tr>");
//                }
//                var objTInspectionFiles = new List<TInspectionFiles>();
//                objTInspectionFiles = DAL.Transaction.GetInspectionFiles(activity.ActivityId).ToList();
//                List<string> attachments = new List<string>();
//                foreach (var item in objTInspectionFiles)
//                {
//                    if (!string.IsNullOrEmpty(item.FilePath))
//                        attachments.Add(HostingEnvironment.MapPath(item.FilePath));
//                }
//                FileInfo fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/StepsFail.html"));
//                string emailBody = File.ReadAllText(fileInfo.FullName);
//                if (newItem.EPDetails != null)
//                {
//                    emailBody = emailBody.Replace("{standardName}", newItem.EPDetails != null ? newItem.EPDetails.StandardEp : "Route Base Inspection (No EP link with this assets)");
//                    emailBody = emailBody.Replace("{EpNUmber}", newItem.EPDetails.EPNumber);
//                    emailBody = emailBody.Replace("{EpDescription}", newItem.EPDetails != null ? newItem.EPDetails.Description : string.Empty);
//                }
//                else { emailBody = emailBody.Replace("{standardName}", string.Empty); emailBody = emailBody.Replace("{EpDescription}", string.Empty); }
//                emailBody = emailBody.Replace("{date}", $"{DateTime.Now:MMM dd, yyyy}");
//                emailBody = emailBody.Replace("{inspector}", activity.UserProfile.Name);
//                emailBody = emailBody.Replace("{inspectorName}", activity.UserProfile.Name);
//                emailBody = emailBody.Replace("{inspectionDate}", Conversion.GetUtcToLocal(activity.ActivityInspectionDate ?? DateTime.Now, timeZone).ToString("MMM dd, yyyy hh:mm tt "));  //activity.ActivityInspectionDate.Value.ToString("dd MMM yyyy hh:mm tt "));
//                emailBody = emailBody.Replace("{stepsinfo}", stepsinfo.ToString());
//                emailBody = emailBody.Replace("{deviceinfo}", deviceinfo.ToString());
//                string sender = AppSettings.SenderMailFrom;
//                string toAddress = string.Join(",", toemails);
//                if (assetsUsers.Count > 0)
//                    toAddress = toAddress + "," + string.Join(",", assetsUsers.Select(i => i.Email));
//                string ccAddress = string.Join(",", ccemails);
//                string subject = $"CRx: Inspection Failed Asset # {activity.TFloorAssets.DeviceNo} {(newItem.EPDetails != null ? ", " + newItem.EPDetails.StandardEp : string.Empty)}";
//                string ilsmMailFrom = sender;
//                SendMail(toAddress, subject, emailBody, ilsmMailFrom, attachments, ccAddress);
//            }
//            catch (Exception ex)
//            {

//            }

//            return true;
//        }

//        #endregion Inspection 

//        #region ILSM


//        #endregion

//        #region Binders (Send Document Notification Mail)
//        private static bool SendDocumentMailOnExpiration(HttpResponseMessage objHttpResponseMessage, List<string> toemails, List<string> ccemails)
//        {
//            try
//            {
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendDocumentNotificationMail.html"));
//                var emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", GenerateDocumentReports(objHttpResponseMessage));
//                var attachments = new List<string>();
//                string sender = AppSettings.SenderMailFrom;
//                string toAddress = string.Join(",", toemails);
//                string ccAddress = string.Join(",", ccemails);
//                string subject = $"CRx : AHJ Document(s) expiring soon ";
//                SendMail(toAddress, subject, emailBody, sender, attachments, ccAddress);
//            }
//            catch (Exception)
//            {

//            }

//            return true;
//        }

//        private static string GenerateDocumentReports(HttpResponseMessage objHttpResponseMessage)
//        {
//            StringBuilder reportinfo = new StringBuilder();
//            if (objHttpResponseMessage.Result.BinderFolders != null && objHttpResponseMessage.Result.BinderFolders.Count > 0)
//            {
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>File Name  </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Common Document Name </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Expiration Date  </strong></td>" +
//                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Uploaded By  </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.BinderFolders)
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.FolderName +
//                        "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                         item.CommonName + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.ValidUpTo.Value.ToShortDateString() + "</td>");
//                    reportinfo.Append(
//                       "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                       item.CreatedByUserProfile.Name + "</td>");
//                    reportinfo.Append("</tr>");
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }
//            return reportinfo.ToString();
//        }

//        #endregion Binders (Send Document Notification Mail)


//        #endregion

//        #region CRx Auto Report Emails

//        public static bool SendAutoReportEmails(HttpResponseMessage objHttpResponseMessage, int notificationCategory)
//        {
//            bool status = true;
//            try
//            {
//                var fileInfo = new FileInfo(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/SendAutoReportEmail.html"));
//                var emailBody = File.ReadAllText(fileInfo.FullName);
//                emailBody = emailBody.Replace("{Errortext}", GenerateReports(objHttpResponseMessage));
//                var attachments = new List<string>();
//                string sender = AppSettings.SenderMailFrom;
//                NotificationEmails obj = GetNotificationEmail(notificationCategory, string.Empty, string.Empty, string.Empty, Convert.ToInt32(Enums.NotificationEvent.OnCreated));
//                string toAddress = obj.NotificationEmail;
//                string ccAddress = obj.NotificationCCEmail;
//                string subject = $"CRx : Weekly Auto Report Emails";
//                SendMail(toAddress, subject, emailBody, sender, attachments, ccAddress);
//            }
//            catch (Exception ex)
//            {
//                status = false;
//            }
//            return status;
//        }

//        private static string GenerateReports(HttpResponseMessage objHttpResponseMessage)
//        {
//            StringBuilder reportinfo = new StringBuilder();
//            reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Assets Needing Inspection</h4>");

//            //Assets Needing Inspection
//            if (objHttpResponseMessage.Result.TFloorAssets != null && objHttpResponseMessage.Result.TFloorAssets.Count > 0)
//            {
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.TFloorAssets.GroupBy(x => x.SiteName).Select(grp => grp.First()).ToList())
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.SiteName + "</td>");
//                    reportinfo.Append("</tr>");
//                    foreach (var assets in objHttpResponseMessage.Result.TFloorAssets.Where(x => x.SiteName == item.SiteName).GroupBy(x => x.AssetId).Select(grp => grp.First()).ToList().ToList())
//                    {
//                        foreach (var floorAssets in objHttpResponseMessage.Result.TFloorAssets.Where(x => x.AssetId == assets.AssetId))
//                        {
//                            reportinfo.Append("<tr>");
//                            reportinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                assets.Assets.Name + "</td>");
//                            reportinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                 item.EPDetail.StandardEp + "</td>");
//                            reportinfo.Append(
//                                "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                                string.Join(",", objHttpResponseMessage.Result.TFloorAssets.Select(x => x.BuildingName)) + "</td>");
//                            reportinfo.Append(
//                               "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                               string.Empty + "</td>");
//                            reportinfo.Append("</tr>");
//                        }
//                    }
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }

//            //Policies Needing Review
//            if (objHttpResponseMessage.Result.PoliciesNeedingReview != null && objHttpResponseMessage.Result.PoliciesNeedingReview.Count > 0)
//            {
//                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Policies Needing Review</h4>");
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Policy Name </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>StandardEP </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Next Review Date </strong></td>" +
//                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Assigned User </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.PoliciesNeedingReview)
//                {
//                    if (item != null)
//                    {
//                        reportinfo.Append("<tr>");
//                        reportinfo.Append(
//                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                            "<a href = 'https://crxweb.com/newbinders' >" + item.StandardEp +
//                            "</a></td>");
//                        reportinfo.Append(
//                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                             item.StandardEp + "</td>");
//                        reportinfo.Append(
//                            "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                            string.Empty + "</td>");
//                        reportinfo.Append(
//                           "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                           string.Empty + "</td>");
//                        reportinfo.Append("</tr>");
//                    }
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }

//            //Other EPs Needing Review 
//            if (objHttpResponseMessage.Result.OtherEPsNeedingReview != null && objHttpResponseMessage.Result.OtherEPsNeedingReview.Count > 0)
//            {
//                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Other EPs Needing Review</h4>");
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>StandardEP </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Next Review Date </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Assigned User </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.OtherEPsNeedingReview)
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                       "<a href = 'https://crxweb.com/ep/'" + item.EPDetailId + "'/inspections' >" + item.StandardEp +
//                        "</a></td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                         string.Empty + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        string.Empty + "</td>");
//                    reportinfo.Append("</tr>");
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }

//            //Undocumented Fire Drills  
//            if (objHttpResponseMessage.Result.Exercises != null && objHttpResponseMessage.Result.Exercises.Count > 0)
//            {
//                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Undocumented Fire Drills</h4>");
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Building </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Quarter </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Shift </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.Exercises)
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.Building.BuildingName + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.QuarterMaster.QuarterName + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.Shift.Name + "</td>");
//                    reportinfo.Append("</tr>");
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }

//            //Open ILSMs (>90 Days)
//            if (objHttpResponseMessage.Result.TIlsms != null && objHttpResponseMessage.Result.TIlsms.Count > 0)
//            {
//                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Open ILSMs (>90 Days)</h4>");
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>ILSM Number </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>ILSM Name </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>User </strong></td>" +
//                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comments </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.TIlsms)
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        "<a href = 'https://crxweb.com/ilsm/view' >" + item.IncidentId +
//                        "</a></td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.Name + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.Name + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        string.Empty +
//                        "</td>");
//                    reportinfo.Append("</tr>");
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }

//            //Open Permits (>90 Days)
//            if (objHttpResponseMessage.Result.AllPermits != null && objHttpResponseMessage.Result.AllPermits.Count > 0)
//            {
//                reportinfo.Append("<h4 style='color: #333; font-family: arial; font-size: 14px; padding: 5px;'>Open Permits (>90 Days)</h4>");
//                reportinfo.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='border:#ccc solid 1px; border-collapse:collapse;'>");
//                reportinfo.Append("<thead><tr><td width='204' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Permit Type/Number </strong></td>" +
//                    "<td width = '297' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Project Name </strong></td>" +
//                    "<td width = '330' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Manager </strong></td>" +
//                    "<td width = '333' style = 'color: #333; font-family: arial; font-size: 14px; padding: 5px;border:#ccc solid 1px;'><strong>Comments </strong></td>" +
//                    "</tr></thead>");
//                reportinfo.Append("<tbody>");
//                foreach (var item in objHttpResponseMessage.Result.AllPermits)
//                {
//                    reportinfo.Append("<tr>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        $"{(Enums.PermitType)item.PermitType}/{item.PermitNo}" + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        item.ProjectName + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        string.Empty + "</td>");
//                    reportinfo.Append(
//                        "<td style='font-family:arial; font-size:12px; color:#333; padding:5px;width:70px;border:#ccc solid 1px;'>" +
//                        string.Empty +
//                        "</td>");
//                    reportinfo.Append("</tr>");
//                }
//                reportinfo.Append("</tbody>");
//                reportinfo.Append("</table><br />");
//            }
//            return reportinfo.ToString();
//        }

//        #endregion    

//        public static bool SendEmail(string to, string subject, string body)
//        {
//            return EmailProcessor.SendMail(to, subject, null, body, null, null);
//        }



//        #region Notify Volunteer
//        public static bool SendNotifyVolunteerMail(List<ActivityData> activity, string CurrentUserName)
//        {
//            var ownerUser = activity.FirstOrDefault().user;
//            var emailBody = File.ReadAllText(string.Format(HostingEnvironment.ApplicationPhysicalPath + "{0}", @"Templates/NotifyVolunteermail.html"));
//            StringBuilder midtext = new StringBuilder();
//            if (activity.Any(x => x.Type == Utility.Enum.UserActivityType.Added.ToString()))
//            {
//                var data = activity.Where(x => x.Type == Utility.Enum.UserActivityType.Added.ToString()).ToList();
//                if (data.Count > 0)
//                {
//                    midtext.Append("You have been added to the following round(s): <br />");
//                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
//                    midtext.Append("<tr>");
//                    midtext.Append("<td><b>" + "Round" + "</b></td>");
//                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
//                    midtext.Append("<td><b>" + "Next Date" + "</b></td>");
//                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
//                    midtext.Append("</tr>");
//                    midtext.Append("</thead><tbody>");

//                    foreach (var item in data)
//                    {
//                        midtext.Append("<tr>");
//                        midtext.Append("<td>" + item.roundcategories + "</td>");
//                        midtext.Append("<td>" + item.rGroupName + "</td>");
//                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
//                        midtext.Append("<td>" + item.frequency + "</td>");
//                        midtext.Append("</tr>");
//                    }
//                    midtext.Append("</tbody></table>");
//                }
//            }

//            if (activity.Any(x => x.Type == Utility.Enum.UserActivityType.TemporaryReplaced.ToString()))
//            {
//                var data = activity.Where(x => x.Type == Utility.Enum.UserActivityType.TemporaryReplaced.ToString()).ToList();
//                if (data.Count > 0)
//                {
//                    midtext.Append("You have been temporarily replaced to the following round(s):  <br />");
//                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
//                    midtext.Append("<tr>");
//                    midtext.Append("<td><b>" + "Round" + "</b></td>");
//                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
//                    midtext.Append("<td><b>" + "Effective Date" + "</b></td>");
//                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
//                    midtext.Append("</tr>");
//                    midtext.Append("</thead><tbody>");
//                    foreach (var item in data)
//                    {
//                        midtext.Append("<tr>");
//                        midtext.Append("<td>" + item.roundcategories + "</td>");
//                        midtext.Append("<td>" + item.rGroupName + "</td>");
//                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
//                        midtext.Append("<td>" + item.frequency + "</td>");
//                        midtext.Append("</tr>");
//                    }
//                    midtext.Append("</tbody></table>");
//                }
//            }

//            if (activity.Any(x => x.Type == Utility.Enum.UserActivityType.PermanentlyReplaced.ToString()))
//            {
//                var data = activity.Where(x => x.Type == Utility.Enum.UserActivityType.PermanentlyReplaced.ToString()).ToList();
//                if (data.Count > 0)
//                {
//                    midtext.Append("You have been permanently replaced to the following round(s):  <br />");                  
//                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
//                    midtext.Append("<tr>");
//                    midtext.Append("<td><b>" + "Round" + "</b></td>");
//                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
//                    midtext.Append("<td><b>" + "Effective Date" + "</b></td>");
//                    midtext.Append("<td><b>" + "Frequency" + "</b></td>");
//                    midtext.Append("</tr>");
//                    midtext.Append("</thead><tbody>");
//                    foreach (var item in data)
//                    {
//                        midtext.Append("<tr>");
//                        midtext.Append("<td>" + item.roundcategories + "</td>");
//                        midtext.Append("<td>" + item.rGroupName + "</td>");
//                        midtext.Append("<td>" + item.EffectiveDate.ToLongDateString() + "</td>");
//                        midtext.Append("<td>" + item.frequency + "</td>");
//                        midtext.Append("</tr>");
//                    }
//                    midtext.Append("</tbody></table>");
//                }
//            }

//            if (activity.Any(x => x.Type == Utility.Enum.UserActivityType.Removed.ToString()))
//            {
//                var data = activity.Where(x => x.Type == Utility.Enum.UserActivityType.Removed.ToString()).ToList();
//                if (data.Count > 0)
//                {
//                    midtext.Append("You have been removed from the follow round(s): <br />");

//                    midtext.Append("<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + " <thead>");
//                    midtext.Append("<tr>");
//                    midtext.Append("<td><b>" + "Round" + "</b></td>");
//                    midtext.Append("<td><b>" + "Location Group" + "</b></td>");
//                    midtext.Append("<td><b>" + "Removal Date" + "</b></td>");
//                    midtext.Append("</tr>");
//                    midtext.Append("</thead><tbody>");
//                    foreach (var item in data)
//                    {
//                        midtext.Append("<tr>");
//                        midtext.Append("<td>" + item.roundcategories + "</td>");
//                        midtext.Append("<td>" + item.rGroupName + "</td>");
//                        midtext.Append("<td>" + item.CreatedDate.ToLongDateString() + "</td>");
//                        midtext.Append("</tr>");
//                    }
//                    midtext.Append("</tbody></table>");
//                }
//            }

//            string text = "Hello " + ownerUser.FullName + "<br />" + "There has been an update in your Rounds Inspection. Please view your recent Round(s) Assignment update below." + "<br />"
//                          + midtext + "<br />" + "Your currently assigned rounds are viewable in ‘Round Inspection’ under the Round Module in CRx. Be sure to select your name in the dropdown box." + "<br />"
//                          + "Thank you, " + "<br />" + CurrentUserName + "<br />" + "via CRx System";

//            emailBody = emailBody.Replace("{UserName}", ownerUser.FullName);
//            emailBody = emailBody.Replace("{Errortext}", string.Format(text));
//            string sender = AppSettings.SenderMailFrom;
//            var toAddress = ownerUser.Email;           
//            var subject = "CRx: Rounds Assignment Update";
//            return SendMail(toAddress, subject, emailBody, sender, null, null);
//        }


//        #endregion
//    }
//}