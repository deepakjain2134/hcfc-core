CREATE TABLE [dbo].[ConstructionType] (
    [ConstructionTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]           NVARCHAR (50)  NULL,
    [Description]        NVARCHAR (MAX) NULL,
    [IsActive]           BIT            CONSTRAINT [DF_ICRA_ConstructionType_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]          INT            NULL,
    [CreatedDate]        DATETIME       CONSTRAINT [DF_ICRA_ConstructionType_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ICRA_ConstructionType] PRIMARY KEY CLUSTERED ([ConstructionTypeId] ASC)
);

