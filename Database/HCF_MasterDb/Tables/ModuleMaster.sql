CREATE TABLE [dbo].[ModuleMaster]
(
	[ModuleId] INT NOT NULL PRIMARY KEY, 
    [ModuleName] NVARCHAR(250) NULL, 
    [MenuIds] NVARCHAR(MAX) NULL, 
    [IsActive] BIT NULL DEFAULT 1, 
    [CreatedBy] INT NULL, 
    [CreatedDate] DATETIME NULL DEFAULT getutcdate(), 
    [Sequence] INT NULL, 
    [ModuleCode] NVARCHAR(10) NULL
)

GO

CREATE TRIGGER Trigger_ModuleMaster
   ON  ModuleMaster
   AFTER UPDATE
AS 
BEGIN
   declare @moduleId int ;
   declare @oldMenuIds nvarchar(max);  
   declare @UserId int;
   SELECT @moduleId= INSERTED.ModuleId,@UserId= INSERTED.CreatedBy  FROM INSERTED
   IF UPDATE(MenuIds)
   BEGIn  
       declare @menuIds nvarchar(max);
       Select @menuIds=MenuIds from dbo.ModuleMaster Where ModuleId=@moduleId and MenuIds is not null
       delete from dbo.OrgServices Where ModuleId=@moduleId and MenuID is not null
       INSERT INTO dbo.OrgServices (MenuID,OrganizationKey,Status,Createdby,ModuleId)
       SELECT distinct Item,OrganizationKey,1,@UserId,@moduleId FROM dbo.SplitString(@menuIds, ','), dbo.OrgServices orgSer
        Where ModuleId=@moduleId and orgSer.Status=1  order by OrganizationKey,Item
 END
END