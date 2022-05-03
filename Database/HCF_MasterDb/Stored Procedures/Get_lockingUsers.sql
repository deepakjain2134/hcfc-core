Create proc [dbo].[Get_lockingUsers]  
as begin  
select UserId,FirstName,LastName,Email,IsCRxUser,IsActive,UserName from UserProfile 
where UserStatusCode=204 and FirstName is not null
end  