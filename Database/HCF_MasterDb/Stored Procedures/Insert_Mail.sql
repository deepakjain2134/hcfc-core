CREATE Procedure [dbo].[Insert_Mail] (
           @MessageId nvarchar(250)    
           ,@Subject nvarchar(50)
           ,@Sender nvarchar(50)
           ,@Cc nvarchar(max) =null
           ,@Bcc nvarchar(max) =null
           ,@Details nvarchar(150)
           ,@ReceivedDate datetime
		   ,@to nvarchar(450) =null
		   ,@ParentDocumentRepoId int=0
		   ,@IsReplied bit  =false
		   ,@EmailId nvarchar(50) =null
		   ,@MailFilePath nvarchar(max)=null
		   ,@ClientNo int
		   ,@newId int output         
)
As
Begin
 IF EXISTS (SELECT 1 FROM IncomingMail WHERE [MessageId] =@MessageId and ClientNo = @ClientNo )
 		 BEGIN
	      select @newId=0;
	    return @newId
         END
		 ELSE
	     BEGIN
           INSERT INTO [dbo].[IncomingMail]
           ([MessageId]         
           ,[Subject]
           ,[Sender]
           ,[Cc]
           ,[Bcc]
           ,[Details]
           ,[ReceivedDate]
           ,[CreatedDate]
		   ,[To]
		   ,ParentDocumentRepoId
		   ,IsReplied
		   ,EmailId
		   ,MailFilePath,ClientNo)
     VALUES
           (@MessageId      
           ,@Subject
           ,@Sender
           ,@Cc
           ,@Bcc
           ,@Details
           ,@ReceivedDate
           ,getdate(),@to,@ParentDocumentRepoId,@IsReplied,@EmailId,@MailFilePath,@ClientNo)
		   set @newId = Scope_Identity() 
	       return @newId
END
END
