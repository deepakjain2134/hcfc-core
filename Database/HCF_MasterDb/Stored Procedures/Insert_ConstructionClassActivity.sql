CREATE PROCEDURE [dbo].[Insert_ConstructionClassActivity] --4
@ConstClassActivityId int = null,
@ConstClassCatId int = null,
@ConstClassId int,
@Activity nvarchar(MAX),
@IsActive bit,
@CreatedBy int = null,
@newId int output
AS
BEGIN
SET NOCOUNT ON;   
  IF EXISTS (SELECT 1 FROM ConstructionClassActivity WHERE Activity=@Activity and ConstClassCatId = @ConstClassCatId and IsActive = @IsActive and ConstClassId = @ConstClassId)
     BEGIN
	        select @newId=0;
            return @newId
     END
  ELSE if(@ConstClassActivityId > 0)
	 BEGIN
	   update ConstructionClassActivity set Activity = @Activity , IsActive = @IsActive where ConstClassActivityId = @ConstClassActivityId
	     select @newId =@ConstClassActivityId
         return @newID
	 END
  ELSE
	 BEGIN
	   INSERT INTO [dbo].[ConstructionClassActivity]([ConstClassCatId],[ConstClassId],[Activity],[IsActive],CreatedBy)VALUES
           (@ConstClassCatId,@ConstClassId,@Activity,@IsActive,@CreatedBy)
	        select @newId = Scope_Identity()
            return @newID
	 END
END
















