CREATE TABLE [dbo].[EPFrequency] (
    [EpFrequencyId]   INT      IDENTITY (1, 1) NOT NULL,
    [EpDetailId]      INT      NOT NULL,
    [TjcFrequencyId]  INT      CONSTRAINT [DF_EPFrequency_TjcFrequencyId] DEFAULT ((0)) NOT NULL,
    [RecFrequencyId]  INT      NULL,
    [IsActive]        BIT      CONSTRAINT [DF_EPFrequency_IsActive] DEFAULT ((1)) NOT NULL,
    [SecondFrequency] INT      NULL,
    [CreatedBy]       INT      NOT NULL,
    [CreatedDate]     DATETIME CONSTRAINT [DF_EPFrequency_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EPFrequency] PRIMARY KEY CLUSTERED ([EpFrequencyId] ASC),
    CONSTRAINT [FK_EPFrequency_EPDetails] FOREIGN KEY ([EpDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId])
);

