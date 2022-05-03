CREATE PROCEDURE [dbo].[Insert_UserLoginCodes]
@orgKey uniqueidentifier,
@noOfCodes int,
@createdBy int,
@codes nvarchar(450),
@newId int output
As
Begin
INSERT INTO [dbo].[UserLoginCodes]
           ([OrgKey]
           ,[Code]           
           ,[CreatedBy]
           )
Select @orgKey,item,@createdBy from dbo.splitString(@codes,',')

set @newId=1;
return @newId
END