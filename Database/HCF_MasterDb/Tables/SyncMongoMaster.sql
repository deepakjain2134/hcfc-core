
CREATE TABLE [dbo].[SyncMongoMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](200) NULL,
	[Query] [nvarchar](max) NULL,
	[IsMasterTable] [bit] NULL,
	[IsApplicable] [bit] NULL,
	[IsStructureOnly] [bit] NULL,
	[IsMainDbTable] [bit] NULL,
	[LastSyncDateTime] [datetime] NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_SyncMongoMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SyncMongoMaster] ADD  CONSTRAINT [DF_SyncMongoMaster_IsMainTable]  DEFAULT ((0)) FOR [IsMasterTable]
GO

ALTER TABLE [dbo].[SyncMongoMaster] ADD  CONSTRAINT [DF_SyncMongoMaster_IsApplicable]  DEFAULT ((0)) FOR [IsApplicable]
GO

ALTER TABLE [dbo].[SyncMongoMaster] ADD  CONSTRAINT [DF_SyncMongoMaster_IsStructureOnly]  DEFAULT ((0)) FOR [IsStructureOnly]
GO

ALTER TABLE [dbo].[SyncMongoMaster] ADD  CONSTRAINT [DF_SyncMongoMaster_IsMainDbTable]  DEFAULT ((0)) FOR [IsMainDbTable]
GO

ALTER TABLE [dbo].[SyncMongoMaster] ADD  CONSTRAINT [DF_SyncMongoMaster_LastSyncDateTime]  DEFAULT (getdate()) FOR [LastSyncDateTime]
GO


