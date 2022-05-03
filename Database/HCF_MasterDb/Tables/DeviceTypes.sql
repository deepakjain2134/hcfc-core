CREATE TABLE [dbo].[DeviceTypes] (
    [DeviceTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [DeviceType]   NVARCHAR (30) NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_DeviceTypes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DeviceTypes] PRIMARY KEY CLUSTERED ([DeviceTypeId] ASC)
);

