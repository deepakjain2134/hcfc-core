CREATE TABLE [dbo].[FireExtinguisherTypes] (
    [FETypeId] INT           IDENTITY (1, 1) NOT NULL,
    [AscId]    INT           NOT NULL,
    [FeType]   NVARCHAR (50) NULL,
    [IsActive] BIT           CONSTRAINT [DF_FireExtinguisherTypes_IsActive] DEFAULT ((1)) NOT NULL,
    [Sequence] INT           CONSTRAINT [DF_FireExtinguisherTypes_Sequence] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FireExtinguisherTypes] PRIMARY KEY CLUSTERED ([FETypeId] ASC)
);

