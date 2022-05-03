CREATE TABLE [dbo].[UserGroup] (
    [GroupId]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [IsActive]    BIT            CONSTRAINT [DF_RoleGroup_IsActive] DEFAULT ((1)) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsReadOnly]  BIT            CONSTRAINT [DF_UserGroup_IsReadOnly] DEFAULT ((0)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_RoleGroup_CreatedDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_RoleGroup] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);


GO

CREATE TRIGGER InsertRoleInGroup
   ON  UserGroup
   AFTER INSERT
AS 
BEGIN

declare @usergroupId int;

Select @usergroupId=GroupId from inserted i
INSERT INTO [dbo].[RolesInGroups]
           ([RoleId]
           ,[GroupId]
           ,[Status])
    Select RoleId,@usergroupId,0 from Roles

END
