CREATE TABLE [dbo].[Attachments] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [DocumentRepoId] INT            NULL,
    [FileName]       NVARCHAR (MAX) NULL,
    [FilePath]       NVARCHAR (MAX) NULL,
    [Extension]      VARCHAR (10)   NULL,
    [IsRejected]     BIT            CONSTRAINT [DF_Attachments_IsRejected] DEFAULT ((0)) NOT NULL,
    [IsUsed]         BIT            CONSTRAINT [DF_Attachments_IsUsed] DEFAULT ((0)) NOT NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_Attachments_IsDeleted] DEFAULT ((0)) NULL,
    [CreatedBy]      INT            CONSTRAINT [DF_Attachments_CreatedBy] DEFAULT ((4)) NULL,
    [CreatedDate]    DATETIME       CONSTRAINT [DF_Attachments_CreatedDate] DEFAULT (getdate()) NULL,
    [FileSize] BIGINT NULL, 
    CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Attachments_IncomingMail] FOREIGN KEY ([DocumentRepoId]) REFERENCES [dbo].[IncomingMail] ([DocumentRepoId])
);

