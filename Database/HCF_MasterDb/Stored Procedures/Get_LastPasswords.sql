Create Proc Get_LastPasswords --4,'C01ttDFNm9zI01DEGf+w8HQkppGliuXVmsCovNdzROs='
@userid int,
@currentPassword nvarchar(450),
@status int output      
as
begin

DECLARE @UserPassword TABLE
(Password nvarchar(450),
 ModifiedDate datetime
)
INSERT INTO @UserPassword
SELECT distinct top 3  [Password] ,MAX(ModifiedDate) as 'ModifiedDate'  FROM UserProfileHistory Where UserId=@userid 
group by password
order by MAX(ModifiedDate) desc;
Select @status=count(*) from  @UserPassword Where Password=@currentPassword
return @status;
END