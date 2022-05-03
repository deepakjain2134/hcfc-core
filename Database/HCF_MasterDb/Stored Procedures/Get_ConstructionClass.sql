CREATE proc [dbo].[Get_ConstructionClass]
@ConstructionClassId int =null
As
Begin
  select * from ConstructionClass where ConstructionClassId = isnull(@ConstructionClassId,ConstructionClassId)

  select * from ConstructionClassActivity where ConstClassId = isnull(@ConstructionClassId,ConstClassId)
End






