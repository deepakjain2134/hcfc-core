CREATE Procedure [dbo].[Get_News]
As
Begin
Select  News.[Id]
      ,News.[Title]
      ,News.[Short]
      ,News.[IsReleaseNotes]
      ,News.[Description]
      ,News.[Published]
      ,News.[IsDeleted]
      ,News.[StartDate]
      ,News.[EndDate]
      ,News.[CreatedBy]
      ,[CreatedOn],UP.UserName,UP.FirstName,UP.LastName,UP.UserId,UP.Email from News
      Inner join UserProfile  up on up.UserId=News.CreatedBy where News.IsDeleted=0 --and News.Published =1
ENd