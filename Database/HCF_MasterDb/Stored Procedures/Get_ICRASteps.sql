CREATE proc [dbo].[Get_ICRASteps]
@icrastepId int =null
As
Begin
  select * from ICRASteps where ICRAStepId = isnull(@icrastepId,ICRAStepId)  
End






