CREATE TABLE [dbo].[FrequencyMaster] (
    [FrequencyId] INT            IDENTITY (1, 1) NOT NULL,
    [GracePeriod] INT            NULL,
    [DisplayName] NVARCHAR (250) NULL,
    [Type]        NVARCHAR (5)   NULL,
    [Value]       INT            NULL,
    [Days]        INT            NULL,
    [Version]     NVARCHAR (50)  NULL,
    [IsActive]    BIT            CONSTRAINT [DF_FrequencyMaster_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]   INT            NOT NULL,
    [CreateDate]  DATETIME       CONSTRAINT [DF_FrequencyMaster_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_FrequencyMaster] PRIMARY KEY CLUSTERED ([FrequencyId] ASC)
);

