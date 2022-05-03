Create Procedure [dbo].[Update_DocumentTypeMaster]
 @DocTypeId int,
 @Name nvarchar(250),
 @Path nvarchar(250),
 @IsActive bit,
 @CreatedBy int = NULL,
  @DocCategoryID int = null
As
Begin
update DocumentType set Name = @Name, Path =@Path, IsActive = @IsActive,DocCategoryID = @DocCategoryID
where DocTypeId = @DocTypeId
END
