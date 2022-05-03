CREATE ProcEDURE [dbo].[Get_UserOrgsByRefreshToken] --''
@refreshToken uniqueidentifier
AS
BEGIN  
	select top 1 RefreshToken,Org.DbName,Org.ClientNo
	from Userlogin UL inner join HCF.dbo.Organization Org on UL.OrgKey = Org.Orgkey
	where RefreshToken = @refreshToken
END