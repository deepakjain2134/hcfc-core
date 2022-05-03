CREATE procedure [dbo].[Get_LoginToken] --'D8C60C24-D2A0-4611-8C77-E2A9340E491A' 

@loginToken uniqueidentifier

As

Begin

      SET NOCOUNT ON;

      DECLARE @Exists INT

	  declare @userId int;

	  SELECT @userId=UserId FROM UserLogin WHERE RefreshToken=@loginToken
	  set @Exists=0;
	   IF EXISTS(SELECT UserId FROM UserProfile  WHERE UserId=@userId and IsActive=1)
	   Begin

			 IF EXISTS(SELECT UserLoginId FROM UserLogin  WHERE RefreshToken=@loginToken and IsLogOn=1 and IsCurrent=1)
				 BEGIN
						SET @Exists = 1
				 END
				 ELSE
				 BEGIN
                         SET @Exists = 0
                 END
	   end 

	   else

			begin
						SET @Exists = 0
			end
        select  @Exists;
	   end
