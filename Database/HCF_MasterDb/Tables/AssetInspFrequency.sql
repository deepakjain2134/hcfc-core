CREATE TABLE [dbo].[AssetInspFrequency] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [AssetId]     INT      NULL,
    [FrequencyId] INT      NULL,
    [IsActive]    BIT      CONSTRAINT [DF_AssetInspFrequency_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]   INT      NULL,
    [CreatedDate] DATETIME CONSTRAINT [DF_AssetInspFrequency_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AssetInspFrequency] PRIMARY KEY CLUSTERED ([Id] ASC)
);

