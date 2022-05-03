CREATE TABLE [dbo].[Manufactures] (
    [ManufactureId]   INT            IDENTITY (1, 1) NOT NULL,
    [ManufactureName] NVARCHAR (150) NULL,
    [IsActive]        BIT            CONSTRAINT [DF_Manufactures_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]       INT            NULL,
    [CreateDate]      DATETIME       CONSTRAINT [DF_Manufactures_CreateDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_Manufactures] PRIMARY KEY CLUSTERED ([ManufactureId] ASC)
);

