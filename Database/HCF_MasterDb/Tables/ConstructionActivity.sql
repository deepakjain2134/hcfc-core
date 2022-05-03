CREATE TABLE [dbo].[ConstructionActivity] (
    [ConstActivityId]    INT            IDENTITY (1, 1) NOT NULL,
    [ConstructionTypeId] INT            NULL,
    [Activity]           NVARCHAR (MAX) NULL,
    [IsActive]           BIT            NULL,
    [CreatedBy]          INT            NULL,
    [CreatedDate]        DATETIME       CONSTRAINT [DF_ICRA_ConstructionActivity_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ICRA_ConstructionActivity] PRIMARY KEY CLUSTERED ([ConstActivityId] ASC)
);

