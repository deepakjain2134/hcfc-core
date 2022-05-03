CREATE TABLE [dbo].[DocumentType] (
    [DocTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (250) NULL,
    [Path]        NVARCHAR (250) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_DocumnetType_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_DocumnetType_CreatedDate] DEFAULT (getutcdate()) NULL,
    [DocCategoryID] INT NULL, 
    CONSTRAINT [PK_DocumnetType] PRIMARY KEY CLUSTERED ([DocTypeId] ASC)
);

