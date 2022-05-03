Create Proc [dbo].[Get_VendorbyOrgid] --'0D320358-9DC0-4302-AC04-7E39FB65B8BD'
@Orgkey varchar(50) =null
As
Begin
select * from dbo.Vendors VR
inner join dbo.Organization ORG on VR.ClientNo = ORG.ClientNo
where VR.RegistrationNo = @Orgkey
end