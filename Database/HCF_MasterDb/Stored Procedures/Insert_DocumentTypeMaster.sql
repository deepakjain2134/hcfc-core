Create Procedure [dbo].[Insert_DocumentTypeMaster]
 @DocTypeId int = null
,@Name nvarchar(250) = null
,@Path nvarchar(250) = null
,@IsActive bit =null
,@CreatedBy int = NULL
,@DocCategoryID int = null
,@newId int output
As
Begin
IF EXISTS (SELECT 1 FROM DocumentType WHERE [DocTypeId] =@DocTypeId)
		BEGIN
		update DocumentType set Path = @Path where DocTypeId = @DocTypeId
	    select @newId=@DocTypeId;
        return @newId
        END
		ELSE
	    BEGIN			
        INSERT INTO DocumentType
        ([Name]
        ,[Path]
        ,[IsActive]
        ,[CreatedBy]
        ,[CreatedDate]
		,[DocCategoryID])
    VALUES
        (@Name
        ,@Path
        ,@IsActive
        ,@CreatedBy
        ,GETUTCDATE()
		,@DocCategoryID
)
		select @newId = Scope_Identity()
        return @newID
END
END
