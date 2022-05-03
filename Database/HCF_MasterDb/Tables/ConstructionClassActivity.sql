CREATE TABLE [dbo].[ConstructionClassActivity] (
    [ConstClassActivityId] INT            IDENTITY (1, 1) NOT NULL,
    [ConstClassId]         INT            NULL,
    [ConstClassCatId]      INT            NULL,
    [Activity]             NVARCHAR (MAX) NULL,
    [IsActive]             BIT            NULL,
    [CreatedBy]            INT            NULL,
    [CreatedDate]          DATETIME       CONSTRAINT [DF_ConstructionClassActivity_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ConstructionClassActivity] PRIMARY KEY CLUSTERED ([ConstClassActivityId] ASC)
);

