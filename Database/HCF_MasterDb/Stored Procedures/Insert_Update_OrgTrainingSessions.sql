Create PROCEDURE [dbo].[Insert_Update_OrgTrainingSessions]
@Id INT,
@ModuleSessionId int,
@ClientNo int ,
@Date DateTime='',
@Status nvarchar(30),
@Participants nvarchar(900)='',
@Comments nvarchar(max)='',
@CreatedBy int,
@newId int output
AS
BEGIN	
  
    declare @OrgKey UNIQUEIDENTIFIER = (select orgkey from organization where clientNo=@ClientNo)
   if Exists(select 1 from [dbo].OrgTrainingSessions where Id=@Id AND ModuleSessionId = @ModuleSessionId AND OrgKey =@OrgKey AND iscurrent=1)
   BEGIN
				    
				update [dbo].OrgTrainingSessions set Iscurrent= 0 where Id=@Id AND ModuleSessionId = @ModuleSessionId AND OrgKey =@OrgKey 
				 
				 
INSERT INTO [dbo].[OrgTrainingSessions]
				(
					 ModuleSessionId
					,OrgKey
					,[Date]
					,Status
					,Participants
					,IsCurrent
					,Comments
					,CreatedBy
					,CreatedDate
					
				)
				values
				(
					@ModuleSessionId
					,@OrgKey
					,@Date
					,@Status
					,@Participants
					,1
					,@Comments
					,@CreatedBy
					,getutcdate()
					
				)


				
				Select @newId = @@IDENTITY 
				return @newId
			END
		else 
		begin
			INSERT INTO [dbo].[OrgTrainingSessions]
				(   
					 ModuleSessionId
					,OrgKey
					,[Date]
					,Status
					,Participants
					,IsCurrent
					,Comments
					,CreatedBy
					,CreatedDate
					
				)
				values
				(  
					 @ModuleSessionId
					,@OrgKey
					,@Date
					,@Status
					,@Participants
					,1
					,@Comments
					,@CreatedBy
					,getutcdate()
					
				)
				Select @newId = @@IDENTITY 
				return @newId 
				
		end   

   
   
   ENd
  