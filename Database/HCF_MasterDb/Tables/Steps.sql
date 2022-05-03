CREATE TABLE [dbo].[Steps] (
    [StepsId]     INT            IDENTITY (1, 1) NOT NULL,
    [MainGoalId]  INT            NULL,
    [GoalUId]     NVARCHAR (50)  NULL,
    [EPDetailId]  INT            NULL,
    [IsRA]        BIT            CONSTRAINT [DF_Steps_IsRA] DEFAULT ((0)) NULL,
    [RAScore]     INT            CONSTRAINT [DF_Steps_RAScore] DEFAULT ((0)) NULL,
    [Step]        NVARCHAR (MAX) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Steps_IsActive] DEFAULT ((1)) NOT NULL,
    [IsCurrent]   BIT            CONSTRAINT [DF_Steps_IsCurrent] DEFAULT ((1)) NOT NULL,
    [StepType]    INT            CONSTRAINT [DF_Steps_StepType] DEFAULT ((1)) NOT NULL,
    [IsIlsmLink]  BIT            CONSTRAINT [DF_Steps_IsIlsmLink] DEFAULT ((0)) NULL,
    [CreatedBy]   BigInt            NOT NULL,
    [CreatedDate] DATETIME       CONSTRAINT [DF_CheckPoints_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ParentStepId] INT NULL, 
    CONSTRAINT [PK_CheckPoints] PRIMARY KEY CLUSTERED ([StepsId] ASC),
    CONSTRAINT [FK_Steps_MainGoal] FOREIGN KEY ([MainGoalId]) REFERENCES [dbo].[MainGoal] ([MainGoalId]),
    CONSTRAINT [FK_Steps_UserProfile] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[UserProfile] ([UserId])
);

