CREATE TABLE [dbo].[EpBinder] (
    [EPBinderId]  INT      IDENTITY (1, 1) NOT NULL,
    [EPDetailId]  INT      NOT NULL,
    [BinderId]    INT      NOT NULL,
    [IsActive]    BIT      CONSTRAINT [DF_EpBinder_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]   INT      NOT NULL,
    [CreatedDate] DATETIME CONSTRAINT [DF_EpBinder_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EpBinder] PRIMARY KEY CLUSTERED ([EPBinderId] ASC),
    CONSTRAINT [FK_EpBinder_Binders] FOREIGN KEY ([BinderId]) REFERENCES [dbo].[Binders] ([BinderId]),
    CONSTRAINT [FK_EpBinder_EPDetails] FOREIGN KEY ([EPDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId])
);

