CREATE proc [dbo].[IsValidRecoveryCode]
@RecoveryToken uniqueidentifier
As
BEgin


SELECT TOP 1 1 FROM [dbo].PasswordResetRequests 
WHERE RecoveryMethod='E' AND 
RecoveryToken=@RecoveryToken 
AND Status='Y' and RequestedOn < DATEADD(HOUR,24,RequestedOn);

--SELECT TOP 1 isnull(PasswordRequestType,'recover') as status FROM  [$(HCF_DB)].dbo.PasswordResetRequests 
--WHERE RecoveryMethod='E' AND 
--RecoveryToken=@RecoveryToken 
--AND Status='Y' and RequestedOn < DATEADD(HOUR,24,RequestedOn);
END