CREATE TABLE [dbo].[IncomingMail] (
    [DocumentRepoId]       INT            IDENTITY (1, 1) NOT NULL,
    [ClientNo]             INT            NULL,
    [EmailId]              NVARCHAR (50)  NULL,
    [MessageId]            NVARCHAR (250) NULL,
    [Subject]              NVARCHAR (50)  NULL,
    [Sender]               NVARCHAR (50)  NULL,
    [To]                   NVARCHAR (MAX) NULL,
    [Cc]                   NVARCHAR (MAX) NULL,
    [Bcc]                  NVARCHAR (MAX) NULL,
    [Details]              NVARCHAR (MAX) NULL,
    [MailFilePath]         NVARCHAR (MAX) NULL,
    [IsReplied]            BIT            CONSTRAINT [DF_DocumentRepo_IsReplied] DEFAULT ((0)) NULL,
    [IsDeleted]            BIT            CONSTRAINT [DF_DocumentRepo_IsDeleted] DEFAULT ((0)) NULL,
    [ParentDocumentRepoId] INT            CONSTRAINT [DF_DocumentRepo_ParentDocumentRepoId] DEFAULT ((0)) NULL,
    [ReceivedDate]         DATETIME       NULL,
    [CreatedDate]          DATETIME       CONSTRAINT [DF_DocumentRepo_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_DocumentRepo] PRIMARY KEY CLUSTERED ([DocumentRepoId] ASC)
);

