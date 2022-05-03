CREATE Proc [dbo].[Get_IsVendorExist] 
@Orgkey varchar(50) =null,
@Email varchar(50) =null
As
Begin
select DbName from dbo.Vendors VR
inner join dbo.Organization ORG on VR.ClientNo = ORG.ClientNo
where VR.RegistrationNo = @Orgkey
end