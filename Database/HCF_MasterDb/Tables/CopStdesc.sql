CREATE TABLE [dbo].[CopStdesc]
(
	[CopStdescId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CopName] NVARCHAR(150) NULL, 
    [CopTitle] NVARCHAR(150) NULL, 
    [TagCode] NVARCHAR(150) NULL, 
    [CreatedBy] INT NULL, 
    [CreatedDate] DATETIME NULL DEFAULT getutcdate()
)
