CREATE PROCEDURE [dbo].[Insert_Update_TrainingSessionMaster]
@ModuleSessionId int,
@Name nvarchar(100),
@ShortDescription nvarchar(100),
@Description nvarchar(max),
@CreatedBy int,
@IsActive bit =1,
@newId int output
AS
BEGIN 
   if Exists(select 1 from [dbo].TrainingSessionMaster where ModuleSessionId = @ModuleSessionId)
   BEGIN       
    update [dbo].TrainingSessionMaster 
	set 
	IsActive=@IsActive,
	ShortDescription=@ShortDescription,
	Description=@Description
	Where ModuleSessionId = @ModuleSessionId  
     Select @newId = @ModuleSessionId 
    return @newId 
   END
  else 
  begin
   INSERT INTO [dbo].[TrainingSessionMaster]
    (
      Name
     ,ShortDescription     
     ,Description     
     ,CreatedBy   
	 ,IsActive
    )
    values
    (
    @Name,
    @ShortDescription,    
    @Description,    
    @CreatedBy, 
	@IsActive
    )
    Select @newId = @@IDENTITY 
    return @newId 
  end   
   ENd