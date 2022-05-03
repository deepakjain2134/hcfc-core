CREATE TABLE [dbo].[AffectedEPs] (
    [Id]                 INT      IDENTITY (1, 1) NOT NULL,
    [StepsId]            INT      NULL,
    [AffectedEPDetailId] INT      NOT NULL,
    [IsActive]           BIT      CONSTRAINT [DF_AffectedEPs_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]          INT      NULL,
    [CreatedDate]        DATETIME CONSTRAINT [DF_AffectedEPs_CreatedDate] DEFAULT (getutcdate()) NULL,
    [EpDetailId] INT NULL, 
    CONSTRAINT [PK_AffectedEPs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffectedEPs_EPDetails] FOREIGN KEY ([AffectedEPDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId]),
    CONSTRAINT [FK_AffectedEPs_Steps] FOREIGN KEY ([StepsId]) REFERENCES [dbo].[Steps] ([StepsId])
);

