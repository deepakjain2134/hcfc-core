CREATE TABLE [dbo].[InboxEmails] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [EmailId]   NVARCHAR (50) NULL,
    [Password]  NVARCHAR (50) NULL,
    [PopServer] NVARCHAR (50) NULL,
    [IsActive]  BIT           NULL,
    [ClientNo]  INT           NULL,
    [Port]      INT           NULL,
    [UseSSL]    BIT           CONSTRAINT [DF_InboxEmails_UseSSL] DEFAULT ((1)) NULL,
    [AddedDate] DATETIME      CONSTRAINT [DF_InboxEmails_AddedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_InboxEmails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

