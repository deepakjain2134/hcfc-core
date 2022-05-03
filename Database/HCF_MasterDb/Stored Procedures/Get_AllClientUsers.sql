CREATE proc Get_AllClientUsers
As
begin
Select *,1 as 'IsLoginEnable' from UserProfile Where IsActive=1
END
