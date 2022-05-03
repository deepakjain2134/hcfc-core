CREATE TABLE [dbo].[NotificationMapping] (
    [NotificationMappingId] INT      IDENTITY (1, 1) NOT NULL,
    [NotificationCatId]     INT      NULL,
    [NotificationEventId]   INT      NULL,
    [Status]                INT      NULL,
    [CreatedBy]             INT      NULL,
    [CreatedDate]           DATETIME CONSTRAINT [DF_NotificationMapping_CreatedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_NotificationMapping] PRIMARY KEY CLUSTERED ([NotificationMappingId] ASC)
);

