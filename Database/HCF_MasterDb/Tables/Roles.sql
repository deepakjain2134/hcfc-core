CREATE TABLE [dbo].[Roles] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (256) NOT NULL,
    [Sequence]    INT            NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Roles_IsActive] DEFAULT ((1)) NULL,
    [DisplayText] NVARCHAR (256) NULL,
    [ParentId]    INT            NULL,
    [IsChild]     BIT            CONSTRAINT [DF_Roles_IsChild] DEFAULT ((1)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_Roles_CreatedDate] DEFAULT (getutcdate()) NULL,
    [IsUserRole]  BIT            CONSTRAINT [DF__Roles__IsUserRol__0CE69C57] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__webpages__8AFACE1A9F73F73B] PRIMARY KEY CLUSTERED ([RoleId] ASC),
    CONSTRAINT [UQ__webpages__8A2B61604FDF090E] UNIQUE NONCLUSTERED ([RoleName] ASC)
);

GO

-- =============================================
-- Author:		MOHIT YADAV
-- Create date: 28 SEP 2020
-- Description:	TRIGGER FOR INSERTION INTO RolesinGroups TABLE AFTER INSER OPERATION ON Roles TABLE
-- =============================================
Create TRIGGER [dbo].[Trg_RolesinGroups]
   ON  [dbo].[Roles]
   AFTER  INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;
	
    INSERT INTO dbo.RolesinGroups(
		 groupId
		,RoleId
		,Status
    )
    SELECT
	   ug.GrouPId
       ,i.RoleId
	   ,case When ug.GroupId=1 then 1 else 0 end as 'Status'
    FROM
        inserted i ,dbo.UserGroup ug
		where  ug.IsActive=1
		 
	End