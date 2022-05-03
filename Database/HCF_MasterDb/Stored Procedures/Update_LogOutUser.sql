CREATE Procedure [dbo].[Update_LogOutUser]
@userId int =0,
@RefreshToken uniqueidentifier=null,
@IsAllSession bit = 0,
@LastLoginURL varchar(150) = ''
As
Begin
if(@IsAllSession =1)
BEGIN
   update [dbo].UserLogin set IsLogOn=0 ,LogOffDate =getutcdate() Where UserId = @userId and IsLogOn =1  
END
ELSe
BEGIN
update [dbo].UserLogin set IsLogOn=0 ,LogOffDate =getutcdate() Where RefreshToken=@RefreshToken
ENd
End
