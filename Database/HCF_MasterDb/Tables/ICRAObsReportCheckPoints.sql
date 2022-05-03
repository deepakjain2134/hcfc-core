CREATE TABLE [dbo].[ICRAObsReportCheckPoints] (
    [ICRAReportPointId] INT            IDENTITY (1, 1) NOT NULL,
    [CheckPoints]       NVARCHAR (250) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [IsActive]          BIT            CONSTRAINT [DF_ICRAObsReportCheckPoints_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]         INT            NULL,
    [CreatedDate]       DATETIME       CONSTRAINT [DF_ICRAObsReportCheckPoints_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_ICRAObsReportCheckPoints] PRIMARY KEY CLUSTERED ([ICRAReportPointId] ASC)
);

