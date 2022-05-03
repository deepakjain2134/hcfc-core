CREATE proc [dbo].[Get_UserOrganizations] --19205
@userId int
As
Begin
declare @userprofileId uniqueidentifier;

Select @userprofileId =UserProfileId from UserProfile Where UserId=@userId

Select UserProfileId,UP.IsActive,UP.Orgkey,org.ClientNo,DbName from UserProfile UP
inner join Organization org on org.Orgkey=UP.Orgkey
Where UP.UserId=@userId and UP.Orgkey is not null and UP.IsActive=1 and org.Orgkey in (
select UserOrgId from UserOrganization where IsActive = 1 and UserProfileId = @userprofileId
)
union 
Select UserProfileId,UO.IsActive,UserOrgId,org.ClientNo,DbName from UserOrganization  UO
inner join Organization org on org.Orgkey=UO.UserOrgId
Where UserProfileId=@userprofileId and UO.IsActive=1

end