CREATE PROCEDURE [dbo].[IsNewDevice]
@userId int,
@deviceToken nvarchar(200) = '0',
@ipAddress  nvarchar(25) =null
As
BEGIN


if exists (select top 1 1 from UserLogin where UserId=@userId)
BEGIN
		select  COALESCE(max(0), 1) from (
		select  ip,DeviceId  from UserLogin where UserId=@userId   order by LogOnDate desc OFFSET 1 ROWS
		) as a
	where a.ip   in (@ipAddress) and a.DeviceId  in (@deviceToken)
END
ELSE
BEGIN
select 0
END
END

--we can easily  use last 3 lines committed 
--Select case when count(*)> 0 then 1 else 0 end as 'NewDevice' from UserLogin a  Where UserId=4 and (a.ip   in ('::1') OR a.DeviceId  in ('453263451'))