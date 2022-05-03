CREATE Proc [dbo].[GetCommonFireDrillCategory] -- 14
As
Begin
Select * from dbo.FiredrillCategory order by CategoryName
END