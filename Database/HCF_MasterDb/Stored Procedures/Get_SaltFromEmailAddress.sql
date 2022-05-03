CREATE Procedure [dbo].[Get_SaltFromEmailAddress] --'testuser@jkf.com'
@userName nvarchar(50)
As
Begin
SELECT Salt,UserId FROM UserProfile WHERE UserName=@userName
ENd


