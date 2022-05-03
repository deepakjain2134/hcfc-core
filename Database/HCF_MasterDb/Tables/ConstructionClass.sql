CREATE TABLE [dbo].[ConstructionClass] (
    [ConstructionClassId] INT           IDENTITY (1, 1) NOT NULL,
    [ClassName]           NVARCHAR (50) NULL,
    [IsActive]            BIT           CONSTRAINT [DF_ConstructionClass_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]           INT           NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF_ConstructionClass_CreatedDate] DEFAULT (getutcdate()) NULL,
    [Version] INT NULL, 
    CONSTRAINT [PK_ConstructionClass] PRIMARY KEY CLUSTERED ([ConstructionClassId] ASC)
);

