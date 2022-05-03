CREATE PROCEDURE [dbo].[Insert_ConstructionActivity] --4
@ConstActivityId int = null,
@ConstructionTypeId int,
@Activity nvarchar(MAX),
@IsActive bit,
@CreatedBy int = null,
@newId int output
AS
BEGIN
SET NOCOUNT ON;   
  IF EXISTS (SELECT 1 FROM ConstructionActivity WHERE Activity=@Activity and IsActive = @IsActive)
     BEGIN
	        select @newId=0;
            return @newId
     END
  ELSE if(@ConstActivityId > 0)
	 BEGIN
	   update ConstructionActivity set Activity = @Activity , IsActive = @IsActive where ConstActivityId = @ConstActivityId
	     select @newId =@ConstActivityId
         return @newID
	 END
  ELSE
	 BEGIN
	   INSERT INTO [dbo].[ConstructionActivity]([ConstructionTypeId],[Activity],[IsActive],CreatedBy)VALUES
           (@ConstructionTypeId,@Activity,@IsActive,@CreatedBy)
	        select @newId = Scope_Identity()
            return @newID
	 END
END
















