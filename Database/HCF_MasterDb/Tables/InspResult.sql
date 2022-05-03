CREATE TABLE [dbo].[InspResult] (
    [InspResultId] INT           IDENTITY (1, 1) NOT NULL,
    [ResultName]   NVARCHAR (50) NULL,
    [Code]         NVARCHAR (5)  NULL,
    [IsActive]     BIT           CONSTRAINT [DF_InspResult_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    INT           NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_InspResult_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_InspResult] PRIMARY KEY CLUSTERED ([InspResultId] ASC)
);

