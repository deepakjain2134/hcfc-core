CREATE TABLE [dbo].[IlsmStepsMapping] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [StepsId]     INT      NULL,
    [ILsmStepsId] INT      NULL,
    [IsActive]    BIT      CONSTRAINT [DF_IlsmStepsMapping_IsActive] DEFAULT ((1)) NULL,
    [CreatedDate] DATETIME CONSTRAINT [DF_IlsmStepsMapping_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_IlsmStepsMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

