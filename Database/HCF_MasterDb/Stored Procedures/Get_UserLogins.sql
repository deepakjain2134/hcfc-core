CREATE Proc [dbo].[Get_UserLogins]

@userProfileId uniqueIdentifier = null

As

BEgin

declare @userId int;
Select @userId=UserId from UserProfile Where userProfileId=@userProfileId
Select * from UserLogin 
Where UserId=@userId  
and  DATEDIFF(day,logOnDate,GETDATE()) < 61
order by logOnDate desc


END
