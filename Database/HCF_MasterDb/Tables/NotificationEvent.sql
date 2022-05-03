CREATE TABLE [dbo].[NotificationEvent] (
    [NotificationEventId] INT            IDENTITY (1, 1) NOT NULL,
    [EventName]           NVARCHAR (150) NULL,
    [EventDescription]    NVARCHAR (250) NULL,
    [IsActive]            BIT            CONSTRAINT [DF_NotificationEvent_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_NotificationEvent] PRIMARY KEY CLUSTERED ([NotificationEventId] ASC)
);

