CREATE ProcEDURE [dbo].[Get_Refreshtoken] -- 0,'001781b2-6f4f-4e5e-b891-18d6a313b1af' --,'4gC5hnkcem1hsSAebZXxKKOacqmWJpJb2ZwbeS51azaQ9tgMvbSVBPWuUvQ3VJJWbuG+5GmTAPy2TGgOjf+Svw=='
@userId nvarchar(50) = null,
@refreshToken uniqueidentifier
AS
BEGIN   
declare @newId int;
declare @userProfileId uniqueidentifier ;
--declare @RefreshToken uniqueidentifier;
declare @LastLoginURL nvarchar(100);
declare @orgKey uniqueidentifier;
declare @IsNewDevice bit;
Select @orgKey=orgKey from Organization 
select  @newId=UserLoginId,@userId=UserId from Userlogin where RefreshToken = @refreshToken and IslogOn= 1 and IsCurrent = 1
 --Select * from Userlogin order by LogOndate desc
 Select @userProfileId=UserProfileId from UserProfile Where UserId =@userId 
	--Select @newId,@userId
	
select top 1 @RefreshToken=RefreshToken,@LastLoginURL=LastLoginURL,@IsNewDevice = IsNewDevice from Userlogin where IsLogOn =1  and UserId =@userId ANd UserLoginId=@newId 
Select 
UserProfile.UserId,
UserProfile.orgKey,
@refreshToken as 'RefreshToken' ,
@newId as 'UserLoginId', 
@RefreshToken as 'lastloginRefreshToken',
@LastLoginURL as 'LastLoginURL',
@IsNewDevice as 'IsNewDevice'
from UserProfile 
where UserProfile.UserId =@userId and IsActive=1 --ANd UserLoginId=@newId;		  
	
Select * from [Organization] Where IsActive=1  and ParentOrgKey is null 
and Orgkey in (Select UserOrgId from UserOrganization Where UserProfileId =@userProfileID)
END