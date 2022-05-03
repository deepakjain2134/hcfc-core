CREATE TABLE [dbo].[StandardManuals] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [StDescID]       INT NULL,
    [EpDetailId]     INT NULL,
    [HospitalTypeId] INT NULL,
    [IsApplicable]   BIT CONSTRAINT [DF_StandardManuals_IsApplicable] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_StandardManuals] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StandardManuals_EPDetails] FOREIGN KEY ([EpDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId]),
    CONSTRAINT [FK_StandardManuals_HospitalType] FOREIGN KEY ([HospitalTypeId]) REFERENCES [dbo].[HospitalType] ([HospitalTypeId]),
    CONSTRAINT [FK_StandardManuals_Standards] FOREIGN KEY ([StDescID]) REFERENCES [dbo].[Standards] ([StDescID])
);

