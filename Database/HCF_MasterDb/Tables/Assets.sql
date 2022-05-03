CREATE TABLE [dbo].[Assets] (
    [AssetId]         INT            IDENTITY (1, 1) NOT NULL,
    [AssetTypeId]     INT            NULL,
    [AssetCode]       NVARCHAR (250) NULL,
    [Name]            NVARCHAR (150) NULL,
    [IconPath]        NVARCHAR (250) NOT NULL,
    [IsActive]        BIT            CONSTRAINT [DF_Assets_IsActive] DEFAULT ((1)) NULL,
    [Version]         NVARCHAR (50)  NULL,
    [IsStepsOnReport] BIT            CONSTRAINT [DF_Assets_IsStepOnReport] DEFAULT ((0)) NULL,
    [CreatedBy]       INT            NULL,
    [CreateDate]      DATETIME       CONSTRAINT [DF_Assets_CreatedDate] DEFAULT (getdate()) NULL,
    [IsRouteInsp]     BIT            CONSTRAINT [DF_Assets_IsRouteInsp] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Assets] PRIMARY KEY CLUSTERED ([AssetId] ASC),
    CONSTRAINT [FK_Assets_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [dbo].[AssetType] ([TypeId]),
    CONSTRAINT [UQ__Assets__2DDE5240BB750A91] UNIQUE NONCLUSTERED ([AssetCode] ASC)
);

