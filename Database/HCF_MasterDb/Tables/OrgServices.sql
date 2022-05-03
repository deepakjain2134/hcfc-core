CREATE TABLE [dbo].[OrgServices] (
    [ClientMenuMapID] INT      IDENTITY (1, 1) NOT NULL,
    [MenuID]          INT      NOT NULL,
    [OrganizationKey] INT      NOT NULL,
    [Status]          BIT      NOT NULL,
    [Createdby]       INT      NOT NULL,
    [CreatedDate]     DATETIME CONSTRAINT [DF_OrgServices_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [ModuleId] INT NULL, 
    [ServiceMode] INT NULL, 
    [TrialStartDate] DATETIME NULL, 
    [TrialEndDate] DATETIME NULL, 
    CONSTRAINT [PK__Usermenu__9675D77AE5B13470] PRIMARY KEY CLUSTERED ([ClientMenuMapID] ASC)
);

