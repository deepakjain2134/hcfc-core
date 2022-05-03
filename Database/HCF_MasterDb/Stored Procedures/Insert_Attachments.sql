CREATE Procedure [dbo].[Insert_Attachments]
(
 @MessageId nvarchar(250) =null
,@FileName nvarchar(MAx)
,@FilePath nvarchar(MAX)
,@DocumentRepoId int 
)
As
Begin
INSERT INTO [dbo].[Attachments]
           (DocumentRepoId
           ,[FileName]
           ,[FilePath]
           ,[CreatedBy]
           ,[CreatedDate]
		   ,Extension)
     VALUES
           (
		   @DocumentRepoId
           ,@FileName
           ,@FilePath
           ,4
           ,getdate()
		   ,reverse(left(reverse(@FileName),charindex('.',reverse(@FileName))-1)))
END

