CREATE proc [dbo].[Insert_ClientUser]
@userId int,
@clientNo int,
@isActive bit,
@usergroupIds nvarchar(100)
As
BEGIn

declare @UserProfileId uniqueidentifier;
declare @UserOrgId uniqueidentifier;
declare @dbName nvarchar(100)
declare @sqlQuery nvarchar(max)


Select @UserProfileId=UserProfileId from UserProfile Where UserId=@userId
Select @UserOrgId=Orgkey,@dbName=DbName from Organization Where ClientNo=@clientNo
update UserOrganization set IsActive=0 Where  UserProfileId=@UserProfileId and UserOrgId=@UserOrgId 

if(@isActive=1)
begin
INSERT INTO [dbo].[UserOrganization]
           ([UserOrgId]
           ,[UserProfileId]
           ,[IsActive])
Select  @UserOrgId,@UserProfileId ,@isActive 

set @sqlQuery= @dbName+'.dbo.Insert_MainUser '+convert(nvarchar(40), @userId)+' ,'''+convert(nvarchar(40), @usergroupIds)+'''' ;
EXECUTE sp_executesql @sqlQuery 

END

END