CREATE TABLE [dbo].[AssetsSubCategory] (
    [AscId]       INT           IDENTITY (1, 1) NOT NULL,
    [AscName]     NVARCHAR (50) NULL,
    [AssetId]     INT           NULL,
    [IsActive]    BIT           CONSTRAINT [DF_AssetsSubCategory_IsActive] DEFAULT ((1)) NULL,
    [CreatedDate] DATETIME      CONSTRAINT [DF_AssetsSubCategory_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AssetsSubCategory] PRIMARY KEY CLUSTERED ([AscId] ASC)
);

