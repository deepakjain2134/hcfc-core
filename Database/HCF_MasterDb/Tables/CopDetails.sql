CREATE TABLE [dbo].[CopDetails]
(
	[CopDetailsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CopStdescId] INT NULL, 
    [RequirementName] NVARCHAR(MAX) NULL, 
    [CopText] NVARCHAR(MAX) NULL, 
    [EPTextID] NVARCHAR(150) NULL, 
    [CreatedBy] INT NULL, 
    [CreatedDate] DATETIME NULL DEFAULT getutcdate()
)
