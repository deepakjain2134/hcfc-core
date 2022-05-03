CREATE PROCEDURE [dbo].[UpdateNewDevice]
@UserId int,
@token nvarchar(max)=null,
@newId int output

As
BEGIN
Declare @UserLoginID int;
Declare @UserLoginIDNew int;
if(@token is not null)
begin

select  @UserLoginID=UserLoginId  from UserLogin where UserId=@UserId AND IsLogOn=1  order by LogOnDate desc OFFSET 0 ROWS fetch next 1 ROWS ONLY

update   U set U.IsNewDevice=1,RefreshToken=@token from UserLogin  U where U.UserLoginId=@UserLoginID
select @newId=1

END
ELSE
BEGIN

select  @UserLoginIDNew=UserLoginId  from UserLogin where UserId=@UserId AND IsLogOn=1 And IsNewDevice=1  order by LogOnDate desc OFFSET 0 ROWS fetch next 1 ROWS ONLY
update   U set U.IsNewDevice=0 from UserLogin  U where U.UserLoginId=@UserLoginIDNew
select @newId=2 

END
END