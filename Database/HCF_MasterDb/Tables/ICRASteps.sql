CREATE TABLE [dbo].[ICRASteps] (
    [ICRAStepId]  INT            IDENTITY (1, 1) NOT NULL,
    [Alias]       NVARCHAR (15)  NULL,
    [Steps]       NVARCHAR (MAX) NULL,
    [Sequence]    INT            NULL,
    [IsActive]    BIT            CONSTRAINT [DF_ICRASteps_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_ICRASteps_CreatedDate] DEFAULT (getutcdate()) NULL,
    [ParentICRAStepId] INT NULL, 
    CONSTRAINT [PK_ICRASteps] PRIMARY KEY CLUSTERED ([ICRAStepId] ASC)
);

