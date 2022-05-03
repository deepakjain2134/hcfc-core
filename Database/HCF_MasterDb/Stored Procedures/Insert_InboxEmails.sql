CREATE Procedure [dbo].[Insert_InboxEmails]
            @EmailId nvarchar(50)
           ,@Password nvarchar(50)
           ,@PopServer nvarchar(50)
           ,@IsActive bit
           ,@ClientNo nvarchar(MAX)
		   ,@Port int
		   ,@UseSSL bit
		   ,@newId int output
As

BEGIn

  IF EXISTS (SELECT 1 FROM InboxEmails WHERE EmailId=@EmailId and ClientNo=@ClientNo)
    BEGIN	   
	       UPDATE [dbo].[InboxEmails]	
		   SET
       [Password] = @Password
      ,[PopServer] = @PopServer
      ,[IsActive] = @IsActive      
      ,[Port] = @Port
      ,[UseSSL] = @UseSSL
     WHERE EmailId=@EmailId and ClientNo=@ClientNo

		 select @newId =Id FROM InboxEmails WHERE EmailId=@EmailId and ClientNo=@ClientNo
		return @newID

    END
	ELSE
	BEGIN

INSERT INTO [dbo].[InboxEmails]
           ([EmailId],[Password],[PopServer],[IsActive],[ClientNo],Port,UseSSL)
     VALUES
           (@EmailId,@Password,@PopServer,@IsActive,@ClientNo,@Port,@UseSSL)
		    select @newId = Scope_Identity()
			return @newID
END
END
