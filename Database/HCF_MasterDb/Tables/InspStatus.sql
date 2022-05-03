CREATE TABLE [dbo].[InspStatus] (
    [InspStatusId] INT           IDENTITY (1, 1) NOT NULL,
    [InspResultId] INT           NOT NULL,
    [StatusName]   NVARCHAR (50) NULL,
    [Code]         NVARCHAR (5)  NULL,
    [IsActive]     BIT           CONSTRAINT [DF_InspStatus_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]    INT           NULL,
    [CreatedDate]  DATETIME      CONSTRAINT [DF_InspStatus_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_InspStatus] PRIMARY KEY CLUSTERED ([InspStatusId] ASC),
    CONSTRAINT [FK_InspStatus_InspResult] FOREIGN KEY ([InspResultId]) REFERENCES [dbo].[InspResult] ([InspResultId])
);

