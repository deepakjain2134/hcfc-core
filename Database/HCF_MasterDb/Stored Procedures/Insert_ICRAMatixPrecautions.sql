CREATE PROCEDURE [dbo].[Insert_ICRAMatixPrecautions] --4



@ConstructionRiskId int = null,



@ConstructionTypeId int = null,



@ClassIdss nvarchar(150) = null,

@Version int = 1,



@newId int output



AS



BEGIN



SET NOCOUNT ON; 



update ICRAMatixPrecautions set IsActive =0 Where ConstructionRiskId=@ConstructionRiskId  and ConstructionTypeId = @ConstructionTypeId and Version =@Version



 	  BEGIN



	      INSERT INTO [dbo].[ICRAMatixPrecautions]([Version],[ConstructionClassId],[ConstructionRiskId],[ConstructionTypeId],[IsActive])



		  SELECT @Version,Item,@ConstructionRiskId,@ConstructionTypeId,1 FROM dbo.SplitString(@ClassIdss, ',')



	      select @newId = Scope_Identity()



          return @newID



      END



END




