CREATE PROCEDURE [dbo].[Userlockout]
@userName nvarchar(50),
@Islock int 

AS
BEGIN   

--DECLARE @UserId int 

--select @UserId = UserId from UserProfile where UserName = @userName
if(@Islock=1)
BEGIn 
update UserProfile set UserStatusCode=204 where  UserName = @userName --and UserId=@UserId
END
ELSE
BEGIN
update UserProfile set UserStatusCode=200 where  UserName = @userName --and UserId=@UserId
END

select UserStatusCode as UserStatusCode from UserProfile where  UserName = @userName
END
