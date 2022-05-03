CREATE TABLE [dbo].[EpAssets] (
    [MappingId]  INT      IDENTITY (1, 1) NOT NULL,
    [EPDetailId] INT      NOT NULL,
    [AssetId]    INT      NOT NULL,
    [IsActive]   BIT      CONSTRAINT [DF_EpAssets_isActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]  INT      NOT NULL,
    [CreateDate] DATETIME CONSTRAINT [DF_EpAssets_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EpAssets] PRIMARY KEY CLUSTERED ([MappingId] ASC),
    CONSTRAINT [FK_EpAssets_Assets] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Assets] ([AssetId]),
    CONSTRAINT [FK_EpAssets_EPDetails] FOREIGN KEY ([EPDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId])
);

