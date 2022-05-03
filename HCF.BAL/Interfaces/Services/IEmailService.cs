using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public interface IEmailService
    {
        bool ReplyDocument(Documents newRejectDoc);
        bool SampleDocument(int epDeatilId, Documents sampleDocument);
        bool SendAutoReportEmails(HttpResponseMessage objHttpResponseMessage, int notificationCategory);
        bool SendCommonEmail(NotificationEmails objnotificationEmails, object objects);
        bool SendConfigurationMail(int notificationCategory);
        bool SendConstruictionCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId);
        bool SendDueDateMail();
        bool SendEmail(string to, string subject, string body);
        bool SendEmailonRiskareaApproved(ICRARiskArea modelData);
        bool SendFireDrillCreatedMail(TExercises exercises, int notificationCategory);
        void SendFireDrillMailOnDone(TExercises exercises, int notificationCategory);
        void SendFireDrillOnDueDatePassed(TExercises exercises, int notificationCategory);
        void SendFireDrillReminderMail(TExercises exercises, int notificationCategory);
        bool SendICRANotifyEmail(int userId, string permitNo, string permitType,int version);
        bool SendIlsmCloseMail(int tilsmId, int notificationCategory);
        bool SendIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping);
        bool SendIlsmReOpenMail(int tilsmId, int notificationCategory);
        bool SendInspectionFailMail(Inspection newItem, int notificationCategory);
        bool SendMailOnUserChangePassword(UserProfile userProfile);
        bool SendMailOnUserRegistration(UserProfile userProfile , string token,string createPasswordUrl);
        bool SendManualIlsmMail(TIlsm tilsm, int notificationCategory, NotificationMapping notificationMapping);
        bool SendNotifyVolunteerMail(List<ActivityData> activity, string CurrentUserName);
        bool SendPasswordResetMail(string to, string subject, string body, string from, List<string> attachments = null, string ccAddress = null);
        bool SendPCRANotifyEmail(int userId, string permitNo, string permitType, int? tpcraquestid, int? projectId);
        bool SendPermitCreatedMail(PermitEmailModel objPermitEmailModel, int notificationCategory, byte[] fileBytes, string filename, string buildingId, string floorId, string SiteId);
        bool SendRoundFailMail(TRounds newRoundInsp, int notificationCategory);
        bool SendRoundInsMail(WorkOrder floorWorkOrder, int notificationCategory);
        bool SendTimeToResolveToRAMail(int insStepsId, int notificationCategory);
        bool SendTipsCreatedMail(Tips newItem, int notificationCategory);
        bool SendTipsFailMail(Tips newItem, int notificationCategory);
        bool SendTipsPassMail(Tips newItem, int notificationCategory);
        bool SendTipsUpdatedMail(Tips newItem, int notificationCategory);
        void ReportError(LogEntry logEntry);
        bool SendWorkOrderClosedMail(WorkOrder objWorkOrder, int notificationCategory);
        bool SendWorkOrderCreatedMail(WorkOrder objWorkOrder, int notificationCategory);
        bool VendorNotify(Guid? invitationId, Vendors vendor, Organization org);
        bool SendMailOnResetPasswordRequest(UserProfile userProfile, string token,string webUrl);
        bool SendVerificationCode(string userName, string newdevicecode, string fullName);
    }
}
