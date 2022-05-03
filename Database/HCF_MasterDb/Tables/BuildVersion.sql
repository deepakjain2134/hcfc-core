CREATE TABLE [dbo].[BuildVersion] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Version]        NVARCHAR (20)  NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [isMajorRelease] BIT            CONSTRAINT [DF_BuildVersions_isMajor] DEFAULT ((1)) NULL,
    [FilePath]       NVARCHAR (250) NULL,
    [isCurrent]      BIT            CONSTRAINT [DF_BuildVersions_isCurrent] DEFAULT ((1)) NULL,
    [ReleaseDate]    DATETIME       CONSTRAINT [DF_BuildVersions_BuildDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_BuildVersions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

