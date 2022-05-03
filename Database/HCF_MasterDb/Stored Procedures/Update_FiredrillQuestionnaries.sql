CREATE Procedure [dbo].[Update_FiredrillQuestionnaries]
            @FireDrillQuesId int
           ,@FireDrillCatId int        
           ,@Questionnaries nvarchar(max)     
		   ,@IsActive bit   
		   ,@CreatedBy int   
		   ,@Applicable nvarchar(max) = null
		  
As
Begin
           update dbo.FireDrillQuestionnaires set FireDrillCatId = @FireDrillCatId, 
		   Questionnaries = @Questionnaries ,IsActive = @IsActive, CreatedBy = @CreatedBy,
		   Applicable = @Applicable
		    where FireDrillQuesId = @FireDrillQuesId
End