CREATE TABLE [dbo].[EPDescriptions] (
    [EPDescriptionId] INT            IDENTITY (1, 1) NOT NULL,
    [EPDetailId]      INT            NULL,
    [HospitalTypeId]  INT            NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsCurrent]       BIT            CONSTRAINT [DF_EPDescriptions_IsCurrent] DEFAULT ((1)) NULL,
    [CreatedBy]       INT            NULL,
    [CreatedDate]     DATETIME       NULL,
    CONSTRAINT [PK_EPDescriptions] PRIMARY KEY CLUSTERED ([EPDescriptionId] ASC)
);

