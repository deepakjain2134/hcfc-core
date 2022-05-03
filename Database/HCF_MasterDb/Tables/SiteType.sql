CREATE TABLE [dbo].[SiteType] (
    [SiteTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [SiteTypeName] NVARCHAR (100) NULL,
    [IsActive]     BIT            NULL,
    [CreatedBy]    INT            NULL,
    [CreatedDate]  DATETIME       NULL,
    CONSTRAINT [PK_SiteType] PRIMARY KEY CLUSTERED ([SiteTypeId] ASC)
);

