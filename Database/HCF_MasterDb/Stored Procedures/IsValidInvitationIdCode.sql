CREATE Proc [dbo].[IsValidInvitationIdCode]
   @RecoveryToken uniqueidentifier
As
BEgin
SELECT TOP 1 1 FROM HCF.dbo.VendorOrganizations 
WHERE 
InvitationId=@RecoveryToken 
--AND IsRequested=1
END