CREATE procedure [dbo].[Insert_EPGroupsName]
@EPGroupId int = null,
@EPGroupName nvarchar(max) = null,
@IsActive bit = null
--@newId int output
as 
begin
if(@EPGroupId > 0)
begin
update EPGroups set EPGroupName = @EPGroupName, IsActive = @IsActive where EPGroupId = @EPGroupId
end
else
begin
insert into EPGroups(EPGroupName,IsActive) values (@EPGroupName,@IsActive)
--select @newId = Scope_Identity()
--return @newID
end
end
