CREATE PROCEDURE [dbo].[AssetType_CRUD]
(

-- update using dbPack
       @Name nvarchar(50) 
       ,@CreatedBy int
	   ,@TypeId int = null
	   ,@IsActive bit = null
	   ,@AssetTypeCode nvarchar(10)
	   ,@newId int output
)
As
BEGIN
   SET NOCOUNT ON; 
      BEGIN
	  IF(@TypeId > 0)
	   BEGIN
		   UPDATE [dbo].[AssetType]  SET [Name] = @Name, [IsActive] = @IsActive,AssetTypeCode=@AssetTypeCode
		   WHERE AssetTypeCode=@AssetTypeCode;
		   select @newId =(select TypeId from AssetType where AssetTypeCode = @AssetTypeCode)
	  END
	  ELSE
	  BEGIN
		 IF EXISTS (SELECT 1 FROM [dbo].[AssetType] AST WHERE AST.AssetTypeCode = @AssetTypeCode)
			 BEGIN
				Select @newId = -1
			 END
		 ELSE
			 BEGIN
			    INSERT INTO [dbo].[AssetType]([Name],[IsActive],[CreateDate],[CreatedBy],AssetTypeCode)
				VALUES (@Name,@IsActive,getdate(),@CreatedBy,@AssetTypeCode)
			    SELECT @newId = Scope_Identity()
				RETURN @newID
			END
	  END
	 
	  RETURN @newID
      END
  END
