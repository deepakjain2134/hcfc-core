CReate PROC [dbo].[ChangerecoverPassword]
(
@userName nvarchar(500),
@Password nvarchar(max),
@RecoveryCode nvarchar(100)= null,
@Salt uniqueidentifier
)
as
BEGIN
  update [dbo].UserProfile set Password = @Password , Salt = @Salt where UserName =@userName

  UPDATE [dbo].PasswordResetRequests SET RecoveredOn=GETUTCDATE() WHERE RecoveryMethod='E' AND 
  RecoveryToken=@RecoveryCode AND Status='Y';
END
