CREATE PROCEDURE [dbo].[Get_UserOrgsByUserId]
@UserId int
As
BEGIn

declare @userProfileID nvarchar(max);
select @userProfileID=userProfileID from userprofile where userid=@userid
Select * from [dbo].[Organization] Where IsActive=1  
        and Orgkey in (Select UserOrgId from [dbo].UserOrganization Where UserProfileId =@userProfileID and IsActive =1)
        ORDER BY CASE WHEN Name = 'Demo Hospital' THEN 1 ELSE 2 END, Name  
END