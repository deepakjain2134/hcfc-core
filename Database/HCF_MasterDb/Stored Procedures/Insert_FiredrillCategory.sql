Create Procedure [dbo].[Insert_FiredrillCategory]         
            @FiredrilCatId int ,   
			@CategoryName nvarchar(max) ,
			@Description nvarchar(max) = null,
			@IsActive bit ,
		    @CreatedBy int = null,
			@Applicable nvarchar(max),
			@newId int output
As
Begin
if(@FiredrilCatId >0)
begin
UPDATE [dbo].[FiredrillCategory]
   SET [IsActive] = @IsActive,
		[Description] = @Description,
		[CategoryName] = @CategoryName,
		[Applicable] = @Applicable
 WHERE FiredrillCatId=@FiredrilCatId
 select  @newId = @FiredrilCatId
			
end
else
BEGIN
INSERT INTO [dbo].[FiredrillCategory]
           ([CategoryName]
		   ,[Description]
           ,[IsActive]
           ,[CreatedBy]		   
           ,[CreatedDate]
		   ,IsCommonCat
		   ,Applicable
		   )
     VALUES
           (@CategoryName
		   ,@Description		 
           ,@IsActive
           ,@CreatedBy		 
           ,GETUTCDATE()
		   ,1
		   ,@Applicable)
		    select  @newId = @@IDENTITY
			return @newId
END
return @newId
End