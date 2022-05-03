CREATE Procedure [dbo].[Insert_Manufacture]
(
            @ManufactureName nvarchar(250)          
		   ,@CreatedBy int=4
		   ,@IsActive bit 
		   ,@ManufactureId int =0 
		   ,@newId int output

)

As

Begin

	   IF EXISTS (SELECT 1 FROM Manufactures WHERE ManufactureName =@ManufactureName and IsActive = @IsActive )
			 BEGIN
				select @newId=0;
				return @newId
			 END
		 ELSe if (@ManufactureId > 0)
			 BEGIN
			   update [Manufactures] set ManufactureName =@ManufactureName, IsActive = @IsActive 
			   where ManufactureId = @ManufactureId
			   select @newId = @ManufactureId
			   return @newId
			 END
		 ELSE
			 BEGIN
				INSERT INTO [dbo].[Manufactures]([ManufactureName],[CreatedBy],[IsActive])
				VALUES(@ManufactureName,@CreatedBy,@IsActive)
				select @newId = Scope_Identity()
				return @newID
			 END
end







