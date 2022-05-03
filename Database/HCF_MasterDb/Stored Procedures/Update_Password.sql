-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Modified By & date: Avinash Varsheny & 04-11-2016
-- Description:	to get the User Information bu UserId
-- =============================================
CREATE PROCEDURE [dbo].[Update_Password] ---1176,'5HaUgwKoguAxK4/uAYxYADqn+pLsRJwMqiyasU8VppKpI9tmBClSCO53LkGv4F/ONEWXDugyKB3I+glCJDUeaA=='
@UserId int,
@Password nvarchar(MAX)
AS
BEGIN
SET NOCOUNT ON;
   declare @dbName nvarchar(40);
   declare @OrgKey uniqueidentifier;
   select @OrgKey=Orgkey from  [UserProfile] WHERE UserId =@UserId
   Select @dbName=DbName from  Organization WHERE Orgkey=@OrgKey
   if(@OrgKey is not null)
     begin


	 declare @sqlQuery nvarchar(max);
	 SET @sqlQuery  = 'Update '+@dbName+'.[dbo].[UserProfile] Set [Password]='''+ @Password +''' Where UserId = '+CAST(@UserId as nvarchar(20))+ CHAR(13) +
	                 'INSERT INTO ' +@dbName+ '.[dbo].[UserProfileHistory](UserId,[Password],ModifiedBy)VALUES('+CAST(@UserId as nvarchar(20))+','''+@Password+''','+CAST(@UserId as nvarchar(20))+')'+ CHAR(13) +
	                 'Update '+@dbName+'.[dbo].[UserLogin] Set IsLogin = 0, LogoutDate = '''+ CAST(GETUTCDATE() as nvarchar(30)) +''' where UserId = '+CAST(@UserId as nvarchar(20))
	   --EXECUTE sp_executesql @sqlQuery
	     print @sqlQuery

	 END
    ELse
     BEGIN
       Update UserProfile Set [Password]=@Password,LastUpdatedPasswordDate=GETUTCDATE() Where UserId=@UserId
	   update UserLogin set IsLogOn = 0 , LogOffDate = GETUTCDATE() where UserId = @UserId
END
END
