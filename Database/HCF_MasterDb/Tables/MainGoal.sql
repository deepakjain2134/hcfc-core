CREATE TABLE [dbo].[MainGoal] (
    [MainGoalId]   INT            IDENTITY (1, 1) NOT NULL,
    [EPDetailId]   INT            NULL,
    [AssetId]      INT            NULL,
    [FrequencyId]  INT            NULL,
    [DocTypeId]    INT            NULL,
    [GoalUId]      NVARCHAR (50)  NULL,
    [ActivityType] INT            NULL,
    [Goal]         NVARCHAR (MAX) NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_MainGoal_IsActive] DEFAULT ((1)) NULL,
    [IsCurrent]    BIT            CONSTRAINT [DF_MainGoal_IsCurrent] DEFAULT ((1)) NULL,
    [CreatedBy]    BigInt            NOT NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF_CheckLists_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ClientNo] INT NULL, 
    [FloorAssetId] INT NULL, 
    CONSTRAINT [PK_MainGoal] PRIMARY KEY CLUSTERED ([MainGoalId] ASC),
    CONSTRAINT [FK_MainGoal_Assets] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Assets] ([AssetId]),
    CONSTRAINT [FK_MainGoal_DocumentType] FOREIGN KEY ([DocTypeId]) REFERENCES [dbo].[DocumentType] ([DocTypeId]),
    CONSTRAINT [FK_MainGoal_EPDetails] FOREIGN KEY ([EPDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId]),
    CONSTRAINT [FK_MainGoal_FrequencyMaster] FOREIGN KEY ([FrequencyId]) REFERENCES [dbo].[FrequencyMaster] ([FrequencyId]),
    CONSTRAINT [FK_MainGoal_UserProfile] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[UserProfile] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [MainGoal_AssetId]
    ON [dbo].[MainGoal]([AssetId] ASC);


GO
CREATE NONCLUSTERED INDEX [MainGoal_EPDetailId]
    ON [dbo].[MainGoal]([EPDetailId] ASC);

