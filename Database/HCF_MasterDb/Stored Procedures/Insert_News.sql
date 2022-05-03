CREATE Procedure [dbo].[Insert_News]
(
            @Title nvarchar(250)
           ,@StartDate datetime
           --,@Short nvarchar(150)
           ,@EndDate datetime
           ,@Description nvarchar(MAX)
		   ,@CreatedBy int
		   ,@Published bit 
		   ,@IsReleaseNotes bit
		   ,@newId int output
)
As
Begin
	   IF EXISTS (SELECT 1 FROM [News] WHERE Title =@Title And IsDeleted = 0 And Published = 1 )
		 BEGIN
	        select @newId=0;
            return @newId
         END
		 ELSE
	     BEGIN
		INSERT INTO [dbo].[News]
           ([Title]           
           ,[Description]
           ,[Published]
           ,[IsDeleted]
           ,[StartDate]
           ,[EndDate]
           ,[CreatedBy]
		   ,[IsReleaseNotes]
           ,[CreatedOn])
     VALUES
           (@Title          
           ,@Description
           ,@Published
           ,0
           ,@StartDate
           ,@EndDate
           ,@CreatedBy
		   ,@IsReleaseNotes
           ,GETDATE())
		    select @newId = Scope_Identity()
			 return @newID
         END
end