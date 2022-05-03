CREATE procedure [dbo].[Get_UserOrgs] --'admin@hcf.com'
@userName nvarchar(200)
As
BEGIn
Select Org.* from [dbo].UserProfile 
inner join [dbo].Organization Org on ORg.Orgkey=UserProfile.Orgkey
Where UserName=@userName and UserProfile.IsActive=1
and ORg.IsActive=1
END


