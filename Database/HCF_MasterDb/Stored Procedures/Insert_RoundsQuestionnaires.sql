CREATE Procedure [dbo].[Insert_RoundsQuestionnaires]   --- add round common ques.
           @RouQuesId int 
		   ,@RoundStep nvarchar(max)=null
		   ,@RiskStepCode nvarchar(5)=null
		   ,@RiskType int  =null    
           ,@CreatedBy int
		   ,@RoundCatId int    
		   ,@IsShared bit		   
		   ,@IsActive bit       
		   ,@Applicable nvarchar(max) = null
		   ,@shortdescription nvarchar(50) = null
		     ,@AdditionalRoundType int =null
		   ,@newId int output
As
Begin
if(@RouQuesId >0)
begin
UPDATE [HCF].[dbo].[RoundsQuestionnaires]
   SET [IsActive] = @IsActive  ,
   [RoundStep] = @RoundStep,
   [RiskStepCode] = @RiskStepCode,
   [RiskType] = @RiskType,
   [Applicable] = @Applicable,
   [ShortDescription] = @shortdescription,
   AdditionalRoundType=@AdditionalRoundType
 WHERE RouQuesId=@RouQuesId
 select  @newId = @RouQuesId
			
end
else
BEGIN
INSERT INTO [dbo].[RoundsQuestionnaires]
           (RoundCatId
		   ,[RoundStep]
		   ,[RiskStepCode]
		   ,[RiskType]
           ,[IsActive]
           ,[CreatedBy]
		   ,[IsShared]
           ,[CreatedDate]
		   ,[Applicable],[IsCommonRoundQues],[ShortDescription],AdditionalRoundType)
     VALUES
           (@RoundCatId
		   ,@RoundStep
		   ,@RiskStepCode
		   ,@RiskType
           ,1
           ,@CreatedBy
		   ,@IsShared
           ,GETUTCDATE()
		   ,@Applicable,1,@shortdescription,@AdditionalRoundType)
		    select  @newId = @@IDENTITY
			return @newId
END
return @newId
End