CREATE PROCEDURE [dbo].[IsValidNewPassword]
@password nvarchar(max),
@username nvarchar(50) 
As
BEGIN


if exists (select top 1 1 from userprofilehistory where Email=@username)
BEGIN
select  COALESCE(max(0), 1) from (
select  password from userprofilehistory where Email=@username  order by modifieddate desc OFFSET 0 ROWS
FETCH NEXT 3 ROWS ONLY) as a
where a.password  in (@password)
END
ELSE
BEGIN
select 1
END
END