CREATE Procedure [dbo].[Insert_FireDrillQuestionnaires]
            @FireDrillQuesId int
           ,@FireDrillCatId int        
           ,@Questionnaries nvarchar(max)     
		   ,@IsActive bit   
		   ,@CreatedBy int  
		   ,@Applicable nvarchar(max) = null   
		   ,@newId int output
As
Begin
INSERT INTO [dbo].[FireDrillQuestionnaires]
           (
		    [FireDrillCatId]
		   ,[Questionnaries]
           ,[IsActive]
           ,[CreatedBy]		  
           ,[CreatedDate]
		   ,[Applicable],[IsCommQues])
     VALUES
           (
		    @FireDrillCatId
           ,@Questionnaries
           ,@IsActive
           ,@CreatedBy
		   ,GETUTCDATE()
		   ,@Applicable,1)
		    select  @newId = @@IDENTITY
			return @newId
End
