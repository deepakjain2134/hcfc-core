CREATE TABLE [dbo].[Score] (
    [ScoreId]     INT NOT NULL,
    [Name]        VARCHAR (50)   NULL,
    [Description] NVARCHAR (MAX) NULL,
	[Value]     INT    CONSTRAINT [DF_Score_Value] DEFAULT(0) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Score_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_Score_CreatedDate] DEFAULT (getutcdate()) NULL,
    [Id] INT NOT NULL IDENTITY, 
    CONSTRAINT [PK_Score] PRIMARY KEY ([Id]) 
);

