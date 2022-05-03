CREATE PROCEDURE [dbo].[Insert_ICRASteps] --4
@ICRAStepId int = null,
@Alias nvarchar(50),
@Steps nvarchar(MAX),
@IsActive bit,
@CreatedBy int = null,
@newId int output
AS
BEGIN
SET NOCOUNT ON;   
   IF EXISTS (SELECT 1 FROM ICRASteps WHERE  Steps=@Steps and Alias = @Alias and IsActive =@IsActive)
	  BEGIN
	        select @newId=0;
            return @newId
      END
   ELSE if(@ICRAStepId > 0)
	 BEGIN
	   update ICRASteps set Steps=@Steps , Alias = @Alias, IsActive = @IsActive where ICRAStepId = @ICRAStepId
	    select @newId = @ICRAStepId
        return @newID
	 END
   ELSE
	  BEGIN
	      INSERT INTO [dbo].[ICRASteps]([Alias],[Steps],[IsActive],CreatedBy)VALUES
          (@Alias,@Steps,@IsActive,@CreatedBy)
	      select @newId = Scope_Identity()
          return @newID
      END
END
















