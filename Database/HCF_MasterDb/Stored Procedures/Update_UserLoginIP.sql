CREATE proc [dbo].[Update_UserLoginIP]

@ip nvarchar(200) =null,

@city nvarchar(200) =null,

@country_name nvarchar(200) =null,

@organisation nvarchar(MAX) =null,
@OsName nvarchar(MAX) =null,
@UserLoginId int ,
@BrowserName nvarchar(MAX) =null

As

BEgin

UPDATE [dbo].[UserLogin]

   SET 

      [ip] = @ip
      ,[city] = @city
      ,[country_name] = @country_name
      ,[organisation] =@organisation
	  ,OsName=@OsName
	  ,BrowserName=@BrowserName

 WHERE UserLoginId=@UserLoginId



 END



