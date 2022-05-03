CREATE PROCEDURE [dbo].[Get_Usersforlockout]
@userName nvarchar(50)
As
BEGIn
select * from UserProfile where userName=@userName
END