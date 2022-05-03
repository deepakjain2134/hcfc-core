CREATE Procedure [dbo].[Insert_RoundCategory]         
            @RoundCatId int ,   
			@CategoryName nvarchar(max) ,
		    @CreatedBy int = null,
			@IsActive bit ,
			@Applicable nvarchar(max) = null
		   ,@newId int output
As
Begin
if(@RoundCatId >0)
begin
UPDATE [dbo].[RoundCategory]
   SET [IsActive] = @IsActive,
   [CategoryName] = @CategoryName
      ,Applicable = @Applicable   
 WHERE RoundCatId=@RoundCatId
 select  @newId = @RoundCatId
			
end
else
BEGIN
INSERT INTO [dbo].[RoundCategory]
           ([CategoryName]
           ,[IsActive]
           ,[CreatedBy]		   
           ,[CreatedDate]
		   ,Applicable)
     VALUES
           (@CategoryName		 
           ,@IsActive
           ,@CreatedBy		 
           ,GETUTCDATE()
		   ,@Applicable)
		    select  @newId = @@IDENTITY
			return @newId
END
return @newId
End