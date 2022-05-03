CREATE Procedure [dbo].[Update_Attachment]
(
@ids nvarchar(100),
@docId int,
@type int 
)
As
Begin
if(@type =1) -- Reject
Begin
update [dbo].Attachments Set IsRejected=1 Where Id in (SELECT Item FROM dbo.SplitString(@ids, ','))

Insert Into [dbo].Attachments (DocumentRepoId,FileName,FilePath,Extension,IsRejected,IsUsed,CreatedBy)
Select @docId,FileName,FilePath,Extension,IsRejected,0,CreatedBy From [dbo].Attachments Where Id in (SELECT Item FROM dbo.SplitString(@ids, ','))
END
Else -- Used Case
Begin
update [dbo].Attachments Set IsUsed=1 Where id in (SELECT Item FROM dbo.SplitString(@ids, ','))
END
END
