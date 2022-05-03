Create PROCEDURE [dbo].[Get_TrainingSessionsDetails]
@ModuleSessionId int =null,
@ClientNo int =null 

AS
BEGIN	
   declare @OrgKey UNIQUEIDENTIFIER = (select orgkey from organization where clientNo=@clientNo)
   Select * from [dbo].TrainingSessionMaster Where ModuleSessionId = ISNULL(@ModuleSessionId,ModuleSessionId) 
   Select * from [dbo].OrgTrainingSessions Where  OrgKey=@OrgKey And IsCurrent=1

 ENd
