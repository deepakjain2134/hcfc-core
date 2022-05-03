CREATE PROCEDURE [dbo].[Rm_Insert_ICRARiskArea]	
	
	@Name varchar(255),
	@ApprovalStatus int = null,
	@IsActive bit,	
	@CreatedBy int,
	@IsSendEmail bit = 0,
	@newId int output
AS
BEGIN 
IF EXISTS (SELECT 1 FROM ICRARiskArea WHERE Name =@Name)
		 BEGIN
	        select @newId=0;
            return @newId
         END
		 ELSE
	     BEGIN
			INSERT INTO [dbo].[ICRARiskArea]
           ([Name]
           ,[ApprovalStatus]
           ,[IsActive]
		   ,[IsSendEmail]
           ,[CreatedBy]
           ,[CreatedDate])
     VALUES
           (@Name
           ,@ApprovalStatus
           ,@IsActive
		   ,@IsSendEmail
           ,@CreatedBy
           ,getutcdate())
    select @newId = Scope_Identity()
	return @newID;
	END
	
END

