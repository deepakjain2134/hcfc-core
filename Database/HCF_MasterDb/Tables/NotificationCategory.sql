CREATE TABLE [dbo].[NotificationCategory] (
    [NotificationCatId]   INT            IDENTITY (1, 1) NOT NULL,
    [EnumValue]           NVARCHAR (250) NULL,
    [CategoryName]        NVARCHAR (150) NULL,
    [CategoryDescription] NVARCHAR (250) NULL,
    [IsActive]            BIT            CONSTRAINT [DF_NotificationCategory_IsActive] DEFAULT ((1)) NULL,
    [MenuAlais]           NVARCHAR (50)  NULL,
    CONSTRAINT [PK_NotificationCategory] PRIMARY KEY CLUSTERED ([NotificationCatId] ASC)
);

