CREATE TABLE [dbo].[EPVersions] (
    [EPVersionId]     UNIQUEIDENTIFIER NOT NULL,
    [EPDetailId]      INT              NULL,
    [EPDescriptionId] NVARCHAR (50)    NOT NULL,
    [ScoreId]         INT              NULL,
    [FrequencyId]     INT              NULL,
    [AssetId]         INT              NULL,
    [DocTypeId]       INT              NULL,
    [BinderId]        INT              NULL,
    [IsCurrent]       BIT              CONSTRAINT [DF_EPVersions_IsCurrent] DEFAULT ((1)) NULL,
    [ModifiedType]    NVARCHAR (MAX)   NULL,
    [CreatedBy]       INT              NULL,
    [CreatedDate]     DATETIME         NULL,
    CONSTRAINT [PK_EPVersions] PRIMARY KEY CLUSTERED ([EPVersionId] ASC)
);

