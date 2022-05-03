CREATE PROCEDURE [dbo].[Insert_ConstructionType] --4
@ConstructionTypeId int = null,
@TypeName nvarchar(100),
@Description nvarchar(max) = null,
@IsActive bit,
@CreatedBy int = null,
@newId int output
AS
BEGIN
SET NOCOUNT ON;   
   IF EXISTS (SELECT 1 FROM ConstructionType WHERE TypeName=@TypeName and IsActive =@IsActive)
	  BEGIN
	        select @newId=0;
            return @newId
      END
   ELSE if(@ConstructionTypeId > 0)
	 BEGIN
	   update ConstructionType set TypeName=@TypeName , IsActive = @IsActive ,[Description] =@Description where ConstructionTypeId = @ConstructionTypeId
	    select @newId = @ConstructionTypeId
        return @newID
	 END
   ELSE
	  BEGIN
	      INSERT INTO [dbo].[ConstructionType]([TypeName],[IsActive],CreatedBy,[Description])VALUES
          (@TypeName,@IsActive,@CreatedBy,@Description)
	      select @newId = Scope_Identity()
          return @newID
      END
END

