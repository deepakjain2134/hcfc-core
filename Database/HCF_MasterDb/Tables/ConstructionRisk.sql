CREATE TABLE [dbo].[ConstructionRisk] (
    [ConstructionRiskId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]          NVARCHAR (50) NULL,
    [RiskName]           NVARCHAR (50) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_ConstructionRisk_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]          INT           NULL,
    [CreatedDate]        DATETIME      CONSTRAINT [DF_ConstructionRisk_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ConstructionRisk] PRIMARY KEY CLUSTERED ([ConstructionRiskId] ASC)
);

