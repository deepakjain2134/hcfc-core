CREATE proc [dbo].[Get_BuildVersions]
As
BEgin
Select * from [dbo].BuildVersion order by ReleaseDate,Iscurrent Desc
END


