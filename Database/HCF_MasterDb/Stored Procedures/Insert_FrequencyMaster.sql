-- =============================================
-- Author:		<Author: Pradeep Pal>
-- Create date: <Create: 30-04-2020>
-- Description:	<Description: creating a report>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_FrequencyMaster]
	@FrequencyId int
	,@GracePeriod int = null
	,@DisplayName nvarchar(250) = null
	,@Type nvarchar(5) = null
	,@Value int = null
	,@Days int = null
	,@Version nvarchar(50) = null
	,@IsActive bit =null
	,@CreatedBy int = null 
	,@CreateDate datetime = null
	,@newId int output
AS
BEGIN
	IF EXISTS (SELECT 1 FROM FrequencyMaster WHERE [FrequencyId] =@FrequencyId)
		BEGIN
		update FrequencyMaster set 
		GracePeriod = ISNULL(@GracePeriod,GracePeriod) ,DisplayName = ISNULL(@DisplayName,DisplayName) ,Type = ISNULL(@Type,Type) ,Value = ISNULL(@Value,Value) ,Days = ISNULL(@Days,Days) ,IsActive = ISNULL(@IsActive,IsActive)  where FrequencyId = @FrequencyId
	    select @newId=@FrequencyId;
        return @newId
        END
		ELSE
	    BEGIN			
        INSERT INTO FrequencyMaster
        (GracePeriod
        ,DisplayName
        ,Type
        ,Value
		,Days
		,Version
		,IsActive
		,CreatedBy
		,CreateDate)
    VALUES
        (@GracePeriod
		,@DisplayName
		,@Type
		,@Value
		,@Days
		,@Version
        ,@IsActive
        ,@CreatedBy
        ,GETUTCDATE()
)
		select @newId = Scope_Identity()
        return @newID

   
END
END

