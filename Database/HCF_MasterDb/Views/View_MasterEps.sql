CREATE VIEW [dbo].[View_MasterEps]
	AS 
SELECT c.CategoryId,c.Name ,EP.StDescID,EP.EpdetailId,TJCStandard,EPNumber
FROM dbo.EPDetails EP 
inner join dbo.Standards S on S.stdescId=EP.stdescId
inner join dbo.Category c on C.CategoryId=S.CategoryId
