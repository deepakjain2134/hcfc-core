CREATE TABLE [dbo].[Status] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [StatusCode]  NVARCHAR (150) NULL,
    [StatusName]  NVARCHAR (50)  NULL,
    [Description] NVARCHAR (50)  NULL,
    [ColumnName]  NVARCHAR (50)  NULL,
    [TableName]   NVARCHAR (50)  NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Status_IsActive] DEFAULT ((0)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_Status_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([Id] ASC)
);

