CREATE procedure [dbo].[Get_DocumentTypesMaster]
As
Begin
Select * from [dbo].DocumentType  order by Name 
Select BinderId,BinderName from Binders Where IsActive=1
END
