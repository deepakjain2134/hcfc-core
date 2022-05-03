CREATE TABLE [dbo].[TrainingSessionMaster](
	[ModuleSessionId] [bigint] IDENTITY(9999,12) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ShortDescription] [nvarchar](50) NULL,
	[Sequence] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TrainingSessionMaster] PRIMARY KEY CLUSTERED 
(
	[ModuleSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TrainingSessionMaster] ADD  CONSTRAINT [DF_TrainingSessionMaster_Sequence]  DEFAULT ((0)) FOR [Sequence]
GO


GO

ALTER TABLE [dbo].[TrainingSessionMaster] ADD  CONSTRAINT [DF_TrainingSessionMaster_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[TrainingSessionMaster] ADD  CONSTRAINT [DF_TrainingSessionMaster_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO


