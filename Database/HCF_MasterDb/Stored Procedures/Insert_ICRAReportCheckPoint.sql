CREATE proc [dbo].[Insert_ICRAReportCheckPoint]
@ICRAReportPointId int = null,
@CheckPoints nvarchar(200),
@Description nvarchar(max) =null,
@IsActive bit,
@CreatedBy int,
@newId int output
As
Begin
SET NOCOUNT ON;   
    if(@ICRAReportPointId > 0)
	BEGIN
	  update ICRAObsReportCheckPoints set CheckPoints = @CheckPoints,Description =@Description,IsActive =@IsActive
	  where ICRAReportPointId = @ICRAReportPointId
	  select @newId = @ICRAReportPointId
      return @newID
	END
	ELSE 
      IF EXISTS (SELECT 1 FROM ICRAObsReportCheckPoints WHERE CheckPoints=@CheckPoints)
		 BEGIN
	        select @newId=0;
            return @newId
         END
	   ELSE 
	     BEGIN
			INSERT INTO [dbo].[ICRAObsReportCheckPoints]
				   ([CheckPoints]
				   ,[Description]
				   ,[IsActive]
				   ,[CreatedBy]
				   ,[CreatedDate])
			 VALUES
				   (@CheckPoints
				   ,@Description
				   ,@IsActive
				   ,@CreatedBy
				   ,GETUTCDATE())
			  select @newId = Scope_Identity()
			   return @newID
       END
END

