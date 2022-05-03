CREATE Proc [dbo].[Get_RoundCommonQuestionaries] -- 14
As
Begin
Select * from RoundCategory order by CategoryName
Select * from RoundsQuestionnaires order by RoundStep 
END