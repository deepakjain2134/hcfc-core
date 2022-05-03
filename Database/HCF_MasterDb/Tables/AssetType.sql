CREATE TABLE [dbo].[AssetType] (
    [TypeId]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50) NULL,
    [AssetTypeCode] NVARCHAR (10) NULL,
    [IsInternal]    BIT           CONSTRAINT [DF_AssetType_IsInternal] DEFAULT ((0)) NULL,
    [IsActive]      BIT           CONSTRAINT [DF_AssetType_IsActive] DEFAULT ((0)) NULL,
    [CreatedBy]     INT           NULL,
    [CreateDate]    DATETIME      CONSTRAINT [DF_AssestType_CreateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AssestType_1] PRIMARY KEY CLUSTERED ([TypeId] ASC),
    UNIQUE NONCLUSTERED ([AssetTypeCode] ASC)
);

