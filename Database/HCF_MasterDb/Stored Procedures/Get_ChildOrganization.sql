CREATE procedure [dbo].[Get_ChildOrganization] --'eb2c8600-cdfe-427f-86a5-73df567f432c'

@ParentOrgKey uniqueidentifier

As

Begin

Select * from [dbo].Organization Where ParentOrgKey=@ParentOrgKey and IsActive=1

end





