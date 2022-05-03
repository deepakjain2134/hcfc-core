CREATE procedure [dbo].[Updade_EPsAssignList]
@EPGroupId int = null,
@EPDetailId int = null
as
begin 
delete from dbo.EPGroupsDetail where EPGroupId = @EPGroupId and EPDetailId = @EPDetailId
end
