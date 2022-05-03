CREATE TABLE [dbo].[RolesInGroups] (
    [RoleId]  INT NULL,
    [GroupId] INT NULL,
    [Status]  BIT CONSTRAINT [DF_RolesInGroups_Status] DEFAULT ((0)) NULL,
    CONSTRAINT [FK_RolesInGroups_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId]),
    CONSTRAINT [FK_RolesInGroups_UserGroup] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[UserGroup] ([GroupId])
);

