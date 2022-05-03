CREATE Procedure [dbo].[Get_UsersList] -- 4
@userId int = null
As
Begin 
  select IsActive,IsCRxUser,UserId,FirstName,LastName,Email,lower(UserName) as UserName,Password,Salt,PasswordHash from UserProfile 
  Where UserId=ISNULL(@userId,UserId)
  
  Select 1 as 'UserId',1 as 'EPsCount' --from EPAssignee 
END