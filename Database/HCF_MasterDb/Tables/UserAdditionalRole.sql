CREATE TABLE [dbo].[UserAdditionalRole] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NCHAR (100)    NULL,
    [IsActive]    BIT            NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       NULL,
    CONSTRAINT [PK_UserAdditionalRole] PRIMARY KEY CLUSTERED ([Id] ASC)
);

