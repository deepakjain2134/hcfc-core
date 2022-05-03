CREATE Procedure [dbo].[Check_IsExistingUser] --'anoop@hcfcompliance.com'
@UserName nvarchar(150)
As
Begin
   
if exists( SELECT 1 FROM [dbo].UserProfile WHERE UserName=@UserName or Email=@UserName)
begin
   SELECT top 1 CAST(IsActive as varchar(10)) IsActive FROM [dbo].UserProfile WHERE UserName=@UserName or Email=@UserName 
END
else
Begin
   Select -1
end  
  
ENd
