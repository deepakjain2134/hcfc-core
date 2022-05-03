create Procedure [dbo].[Delete_DocumentTypeMaster]
(
 @DocTypeId int
)
As
Begin
 Update DocumentType set IsActive=0 where DocTypeId= @DocTypeId 
end
