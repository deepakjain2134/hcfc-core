CREATE TABLE [dbo].[ICRARiskArea] (
    [RiskAreaId]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50) NULL,
    [ApprovalStatus] INT           CONSTRAINT [DF_ICRARiskArea_IsApporved] DEFAULT ((2)) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_ICRARiskArea_IsActive] DEFAULT ((0)) NULL,
    [IsSendEmail]    BIT           CONSTRAINT [DF_ICRARiskArea_IsSendEmail] DEFAULT ((0)) NULL,
    [CreatedBy]      INT           NULL,
    [CreatedDate]    DATETIME      CONSTRAINT [DF_ICRARiskArea_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ICRARiskArea] PRIMARY KEY CLUSTERED ([RiskAreaId] ASC)
);

