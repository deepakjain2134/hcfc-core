CREATE procedure [dbo].[Insert_PasswordResetQueue] --'anp1987@gmail.com','E','anp1987@gmail.com','c4ad0de2-f4e4-454f-bc86-3cccc65356a6'
@EmailAddress nvarchar(50),
@RecoveryMethod nvarchar(5),
@RecoveryAddress  nvarchar(50),
@RecoveryToken nvarchar(50),
@passwordRequestType varchar(50)
As
Begin
IF NOT EXISTS(
	SELECT TOP 1 * from [dbo].PasswordResetRequests WHERE Status='Y' AND EmailAddress=@EmailAddress AND RecoveryMethod=@RecoveryMethod) 
BEGIN 
	INSERT INTO [dbo].PasswordResetRequests(RequestId,EmailAddress, RecoveryMethod, RecoveryAddress, RecoveryToken,PasswordRequestType) 
	SELECT newId(),@EmailAddress, @RecoveryMethod, @RecoveryAddress, @RecoveryToken,@passwordRequestType; 
	SELECT @RecoveryToken;
END 
ELSE 
BEGIN 
	SELECT RecoveryToken FROM [dbo].PasswordResetRequests
	WHERE Status='Y' AND EmailAddress=@EmailAddress AND RecoveryMethod=@RecoveryMethod; 
END
END


