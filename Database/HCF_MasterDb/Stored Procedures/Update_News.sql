-- =============================================
-- Author:		Mukund
-- Create date: 03/04/2020
-- Description:	to update the news records
-- =============================================
CREATE Procedure [dbo].[Update_News]
(
		    @Id int
           ,@Title nvarchar(250)
		   ,@Short varchar(100) = null
		   ,@Description nvarchar(MAX)
		   ,@Published bit
           ,@StartDate datetime
		   ,@EndDate datetime
		   ,@IsReleaseNotes bit
		   ,@CreatedBy int
		   ,@newId int output
)
As
Begin
	   IF EXISTS (SELECT 1 FROM [dbo].[News] WHERE Id = @Id )
		 BEGIN
			IF EXISTS (SELECT 1 FROM [dbo].[News] WHERE Title = @Title AND Id != @Id And IsDeleted = 0)
			BEGIN
				SELECT @newId = 0
			END
			ELSE
			BEGIN
				Update [dbo].[News]
				set Title = @Title
				,Short = ISNULL(@Short,Short)
				,StartDate = @StartDate 
				,EndDate = @EndDate 
				,Description = ISNULL(@Description, Description)
				,CreatedBy = ISNULL(@CreatedBy, CreatedBy)
				,Published = ISNULL(@Published, Published)
				,IsReleaseNotes = ISNULL(@IsReleaseNotes, IsReleaseNotes)
				where Id = @Id
				Select @newId = @Id
		   END
         END
		 RETURN @newId
END