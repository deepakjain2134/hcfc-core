CREATE TABLE [dbo].[News] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (255) NOT NULL,
    [Short]          NVARCHAR (150) NULL,
    [IsReleaseNotes] BIT            CONSTRAINT [DF_News_IsReleaseNotes] DEFAULT ((0)) NULL,
    [Description]    NVARCHAR (MAX) NOT NULL,
    [Published]      BIT            CONSTRAINT [DF_News_Published] DEFAULT ((1)) NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF_News_IsDeleted] DEFAULT ((0)) NULL,
    [StartDate]      DATE           NULL,
    [EndDate]        DATE           NULL,
    [CreatedBy]      BigInt            NOT NULL,
    [CreatedOn]      DATETIME       CONSTRAINT [DF_News_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_News_UserProfile] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[UserProfile] ([UserId])
);

