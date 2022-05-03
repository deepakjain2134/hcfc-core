CREATE TABLE [dbo].[Category] (
    [CategoryId]  INT            IDENTITY (1, 1) NOT NULL,
    [Alias]       CHAR (20)      NULL,
    [Code]        NVARCHAR (10)  NULL,
    [Name]        NVARCHAR (150) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_AssetsCategory_IsActive] DEFAULT ((1)) NULL,
    [Version]     NVARCHAR (50)  NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_AssetsCategory_CreatedDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AssetsCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Category__Name]
    ON [dbo].[Category]([Name] ASC);


GO
CREATE NONCLUSTERED INDEX [Category__Code]
    ON [dbo].[Category]([Code] ASC);

