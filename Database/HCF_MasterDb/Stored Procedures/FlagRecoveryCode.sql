CREATE proc [dbo].[FlagRecoveryCode]
@RecoveryToken uniqueidentifier
As
BEgin
UPDATE [dbo].PasswordResetRequests SET RecoveredOn=GETUTCDATE() WHERE RecoveryMethod='E' AND RecoveryToken=@RecoveryToken AND Status='Y';
END


