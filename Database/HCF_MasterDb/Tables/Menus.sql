CREATE TABLE [dbo].[Menus] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Alias]       NVARCHAR(150)      NOT NULL,
    [Name]        NVARCHAR (150) NULL,
    [Seq]         INT            CONSTRAINT [DF_Menus_Seq] DEFAULT ((0)) NOT NULL,
    [Type]        INT            NULL,
    [ImagePath]   NVARCHAR (150) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Menus_IsActive] DEFAULT ((1)) NOT NULL,
    [ParentId]    INT            CONSTRAINT [DF_Menus_ParentId] DEFAULT ((0)) NOT NULL,
    [ChildCount]  INT            CONSTRAINT [DF_Menus_ChildCount] DEFAULT ((0)) NOT NULL,
    [PageUrl]     NVARCHAR (150) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CreatedBy]   INT            NOT NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_Menus_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [IsAllowed]   BIT            DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
-- =============================================
-- Author:		MOHIT YADAV
-- Create date: 24 SEP 2020
-- Description:	TRIGGER FOR INSERTION INTO ROLES TABLE AFTER INSER OPERATION ON MENU TABLE
-- =============================================
Create TRIGGER [dbo].[Trg_Roles]
   ON  dbo.Menus
   AFTER  INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;
	IF EXISTS (SELECT 1 FROM  inserted i WHERE i.ParentId not in(0) )
	Begin
	Declare @ParentID_Menu int , @ParentId_Roles int ;
	select @ParentID_Menu= i.ParentId from inserted i;
	select @ParentId_Roles = RoleId from dbo.Roles  where DisplayText = (Select name From dbo.Menus where Id =isnull(@ParentID_Menu,0))
	End
	if EXISTS (SELECT 1 FROM  inserted i WHERE i.Type not in(2))
	BEGIN
    INSERT INTO dbo.Roles(
		 RoleName
		,Sequence
		,IsActive
		,DisplayText
		,ParentId
		,IsChild
		,CreatedBy
		,CreatedDate
		,IsUserRole
    )
    SELECT
        i.Alias,
        i.Seq,
		i.IsActive,
		i.Name,
		isnull(@ParentId_Roles,0),
		case when @ParentID_Menu is null then 0 else 1 end,
		i.CreatedBy,
		i.CreatedDate,
		0
    FROM
        inserted i
	END
END