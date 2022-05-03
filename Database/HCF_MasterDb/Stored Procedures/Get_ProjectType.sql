CREATE Procedure [dbo].[Get_ProjectType]
As
Begin
Select * from ProjectType where IsActive=1
ENd
