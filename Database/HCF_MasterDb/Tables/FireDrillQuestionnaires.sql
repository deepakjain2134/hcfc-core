CREATE TABLE [dbo].[FireDrillQuestionnaires] (
    [FireDrillQuesId] INT            IDENTITY (1, 1) NOT NULL,
    [FireDrillCatId]  INT            CONSTRAINT [DF_Table_1_RoundId] DEFAULT ((0)) NOT NULL,
    [Questionnaries]  NVARCHAR (MAX) NOT NULL,
    [IsActive]        BIT            CONSTRAINT [DF_FireDrillQuestionnaires_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]       INT            CONSTRAINT [DF_FireDrillQuestionnaires_CreatedBy] DEFAULT ((4)) NOT NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_FireDrillQuestionnaires_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [Applicable] NVARCHAR(MAX) NULL, 
    [IsCommQues] BIT NULL, 
    CONSTRAINT [PK_FireDrillQuestionnaires] PRIMARY KEY CLUSTERED ([FireDrillQuesId] ASC)
);

