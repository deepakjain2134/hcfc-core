CREATE PROCEDURE [dbo].[Insert_ConstructionClass] --4



@ConstructionClassId int = null,



@ClassName nvarchar(100),



@IsActive bit,



@CreatedBy int = null,

@Version int=0,



@newId int output



AS



BEGIN



SET NOCOUNT ON;   



   IF EXISTS (SELECT 1 FROM ConstructionClass WHERE ClassName=@ClassName and IsActive =@IsActive )



	  --BEGIN

	

	  --      select @newId=0;



   --         return @newId



   --   END

   BEGIN

   if(@ConstructionClassId > 0)



	 BEGIN



	   update ConstructionClass set ClassName=@ClassName , IsActive = @IsActive ,Version=@Version where ConstructionClassId = @ConstructionClassId 



	    select @newId = @ConstructionClassId



        return @newID



	 END

	 else

	 	  BEGIN

	

	        select @newId=0;



            return @newId



      END

	 END

   ELSE if(@ConstructionClassId > 0)



	 BEGIN



	   update ConstructionClass set ClassName=@ClassName , IsActive = @IsActive ,Version=@Version where ConstructionClassId = @ConstructionClassId 



	    select @newId = @ConstructionClassId



        return @newID



	 END



   ELSE



	  BEGIN



	      INSERT INTO [dbo].[ConstructionClass]([ClassName],[IsActive],CreatedBy,Version)VALUES



          (@ClassName,@IsActive,@CreatedBy,@Version)



	      select @newId = Scope_Identity()



          return @newID



      END



END
































































