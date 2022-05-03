CREATE Procedure [dbo].[Hcf_UserOrganization] --'adminhcf.com'
@userName nvarchar(20)
As
Begin
Select * from Organization Where Orgkey in (
Select distinct Orgkey from UserProfile Where UserName=@userName and IsActive=1
) and IsActive=1
END


