CREATE proc [dbo].[Get_ConstructionType]
@constructiontypeid int =null
As
Begin
if(@constructiontypeid is not null )
  select * from ConstructionType where ConstructionTypeId = isnull(@constructiontypeid,ConstructionTypeId) 
  else
  select * from ConstructionType where ConstructionTypeId = isnull(@constructiontypeid,ConstructionTypeId) and IsActive=1

  select * from ConstructionActivity where ConstructionTypeId = isnull(@constructiontypeid,ConstructionTypeId)
End






