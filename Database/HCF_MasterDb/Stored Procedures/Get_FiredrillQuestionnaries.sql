CREATE Proc [dbo].[Get_FiredrillQuestionnaries]  
As
BEgin
select * from [dbo].[FiredrillCategory] where IsActive = 1
select * from [dbo].[FireDrillQuestionnaires]
End