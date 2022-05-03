CREATE PROCEDURE [dbo].[CheckICRAPCRAMenuExist]
		@ClientId nvarchar(max),
		@MenuName nvarchar(max)
AS
BEGIN
	    select Count(*) as IsExist from  OrgServices as os inner join Menus as M on os.MenuID=M.Id
  where OrganizationKey=@ClientId and (M.Name=@MenuName ) and IsActive=1 and os.Status=1


END