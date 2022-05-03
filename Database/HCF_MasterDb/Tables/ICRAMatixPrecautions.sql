CREATE TABLE [dbo].[ICRAMatixPrecautions] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [ConstructionClassId] INT NULL,
    [ConstructionRiskId]  INT NULL,
    [ConstructionTypeId]  INT NULL,
    [IsActive]            BIT CONSTRAINT [DF_ICRAMatixPrecautions_IsActive] DEFAULT ((1)) NULL,
    [Version] INT NULL, 
    CONSTRAINT [PK_ICRAMatixPrecautions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

