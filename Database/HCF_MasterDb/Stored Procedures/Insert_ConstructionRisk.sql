CREATE PROCEDURE [dbo].[Insert_ConstructionRisk] --4
@ConstructionRiskId int = null,
@RiskName nvarchar(50),
@GroupName nvarchar(50),
@IsActive bit,
@CreatedBy int = null,
@newId int output
AS
BEGIN
SET NOCOUNT ON;   
   IF EXISTS (SELECT 1 FROM ConstructionRisk WHERE RiskName=@RiskName and IsActive =@IsActive)
	  BEGIN
	        select @newId=0;
            return @newId
      END
   ELSE if(@ConstructionRiskId > 0)
	 BEGIN
	   update ConstructionRisk set RiskName=@RiskName ,GroupName =@GroupName, IsActive = @IsActive where ConstructionRiskId = @ConstructionRiskId
	    select @newId = @ConstructionRiskId
        return @newID
	 END
   ELSE
	  BEGIN
	      INSERT INTO [dbo].[ConstructionRisk]([RiskName],[GroupName],[IsActive],CreatedBy)VALUES
          (@RiskName,@GroupName,@IsActive,@CreatedBy)
	      select @newId = Scope_Identity()
          return @newID
      END
END
















