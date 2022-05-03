CREATE TABLE [dbo].[CmsEpMapping]
(
	[CMSEPMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EPDetailId] INT NULL, 
    [CopDetailsId] INT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedDate] DATETIME NULL DEFAULT getutcdate(), 
    [IsDeleted] BIT NULL DEFAULT 0 
)
