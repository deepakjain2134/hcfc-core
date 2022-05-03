Create proc [dbo].[sp_UnlockUsers]  
@userids nvarchar(max)  
as begin  
update UserProfile 
set UserStatusCode=200,
LockoutEnd=null,
LockOutEnabled=0
where  userid in (SELECT Item FROM [dbo].[SplitString](@userids,',')) and UserId is not null  
end