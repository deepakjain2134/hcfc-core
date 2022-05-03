--[dbo].[AuthenticateUser] 'admin@hcf.com','dCRZekwToLDJP5npttwr+14sdvMh62ZypNQRJ/P7TlU=','335647076',3 ,'2.7.6.2'  
CREATE PROCEDURE [dbo].[AuthenticateUser] --'admin@hcf.com','dCRZekwToLDJP5npttwr+14sdvMh62ZypNQRJ/P7TlU=','335647076',3 ,'2.7.6.2'  
@userName nvarchar(50),    
@password nvarchar(200), 
@deviceToken nvarchar(200) = '0',  
@DeviceTypeId int = 3,
@version nvarchar(15) =null,
@ipAddress  nvarchar(25) =null,
@browserName nvarchar(100) =null,
@osName nvarchar(25) =null,
@deviceType nvarchar(25) =null

AS    
BEGIN   

declare @userId int;
declare @userProfileID uniqueidentifier;
declare @OrgKey uniqueidentifier; 
declare @RefreshToken uniqueidentifier;
declare @dbName nvarchar(40); 
declare @sqlQuery nvarchar(max);
set @RefreshToken=NEWID(); 
declare @IsNewDevice int=0;
 Select @UserId=UserId from [dbo].[UserProfile]  WHERE UserName=@userName


IF EXISTS (Select UserId from [UserProfile] WHERE UserName=@userName and Password=@password)  
   BEGIN 
    Select @userId=UserId,@userProfileId=UserProfileId,@OrgKey=Orgkey from [dbo].[UserProfile]  WHERE UserName=@userName and Password=@password   
    if(@OrgKey is not null)  
    BEGIN 
           Select @IsNewDevice= case when count(*)> 0 then 0 else 1 end  from UserLogin a  Where UserId=@userId and IsNewDevice=0 and (a.ip   in (@ipAddress) or a.DeviceId  in (@deviceToken))
           -- save user login details
           INSERT INTO dbo.UserLogin
           (BrowserName,OsName,[UserId] ,[DeviceId] ,[DeviceTypeId],[IsLogOn] ,[LogOnDate] ,BuildVersion,RefreshToken,[ip],orgKey,UserName,IsNewDevice,DeviceType) 
            VALUES  
           (@BrowserName,@OsName,@userId ,@deviceToken,@DeviceTypeId,1,getutcdate() ,@version,@RefreshToken,@ipAddress,@OrgKey,@userName,@IsNewDevice,@DeviceType) 

           -- get user Details based on client db
            Select @dbName=DbName from Organization WHERE Orgkey=@OrgKey   
            set @sqlQuery= @dbName+'.dbo.AuthenticateOrgUser '+convert(nvarchar(40), @userId)+' ,'''+convert(nvarchar(40), @RefreshToken)+''','''+convert(nvarchar(40), @version)+''','''+convert(nvarchar(40), @deviceToken)+''','''+convert(nvarchar(40), @ipAddress)+''','''+convert(nvarchar(40), @DeviceTypeId)+'''' ;
            EXECUTE sp_executesql @sqlQuery 
    END 
END
Else
begin

 Select UserId,1 as 'IsFailedLogin' from [dbo].[UserProfile]  WHERE UserName=@userName
  INSERT INTO [dbo].[UserLogin]
      (BrowserName,OsName,[UserId] ,[DeviceId] ,[DeviceTypeId],[IsLogOn] ,[LogOnDate] ,BuildVersion,RefreshToken,IsFaildLogin,UserName,ip,DeviceType) 
  VALUES  
       (@BrowserName,@OsName,@userId ,@deviceToken,@DeviceTypeId,1,getutcdate() ,@version,@RefreshToken,1,@userName,@ipAddress,@DeviceType)

end
END