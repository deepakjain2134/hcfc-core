CREATE Proc [dbo].[GetCommonRoundCategory] -- 14
As
Begin
Select * from dbo.RoundCategory Where IsDeleted=0 order by CategoryName 
select * from dbo.RoundsQuestionnaires
where RoundCatId in (select RoundCatId from  dbo.RoundCategory where IsActive = 1 and IsDeleted=0 )
END