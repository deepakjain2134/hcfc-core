CREATE PROCEDURE [dbo].[Rm_Update_ICRARiskArea]	
	
	@Name varchar(255),
	@ApprovalStatus int = null,
	@IsActive bit,	
	@CreatedBy int,
	@IsSendEmail bit = 0,
	@RiskAreaId int
	
AS
BEGIN 
UPDATE [dbo].[ICRARiskArea]
   SET [Name] = @Name
      ,ApprovalStatus = @ApprovalStatus
      ,[IsActive] = @IsActive    
	  ,[IsSendEmail] = @IsSendEmail
 WHERE  RiskAreaId=@RiskAreaId
	
END
