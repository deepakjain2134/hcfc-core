CREATE proc [dbo].[GetEmailAddressFromRecoveryCode]
@RecoveryToken  uniqueidentifier
As BEgin

SELECT EmailAddress FROM [dbo].PasswordResetRequests WHERE RecoveryMethod='E' AND RecoveryToken=@RecoveryToken AND Status='Y'
END


