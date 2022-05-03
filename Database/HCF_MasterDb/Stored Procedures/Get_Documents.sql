CREATE Procedure [dbo].[Get_Documents] -- null,23132,null
@clientNo int=0,
@docRepoId int =0, 
@fileId int =0
As
Begin
if(@clientNo >0)
BEGIN
Select * from [dbo].IncomingMail IM
--inner join Attachments on Attachments.DocumentRepoId=IM.DocumentRepoId 
Where ParentDocumentRepoId=0 and IsDeleted=0 and IM.IsDeleted=0 and ClientNo=@clientNo 
and DocumentRepoId in (select DocumentRepoId from Attachments)
order by ReceivedDate desc
Select attch.* from [dbo].Attachments attch 
inner join [dbo].IncomingMail doc on doc.DocumentRepoId=attch.DocumentRepoId
Where doc.IsDeleted=0 and attch.IsDeleted =0
and ClientNo=@clientNo
----and (attch.Extension='pdf' or attch.Extension='doc' or attch.Extension='docx' or attch.Extension='xls') 
END
else if(@docRepoId >0)
BEGIN
Select * from [dbo].IncomingMail IM Where DocumentRepoId=@docRepoId
Select attch.* from [dbo].Attachments attch 
Where attch.DocumentRepoId=@docRepoId and attch.IsDeleted =0 
--and (attch.Extension='pdf' or attch.Extension='doc' or attch.Extension='docx' or attch.Extension='xls')
END
else if(@fileId >0)
BEGIN
Select attch.* from [dbo].Attachments attch
Where attch.Id=@fileId and attch.IsDeleted =0
--and (attch.Extension='pdf' or attch.Extension='doc' or attch.Extension='docx' or attch.Extension='xls')
END
End