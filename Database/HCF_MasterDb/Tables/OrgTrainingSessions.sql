CREATE TABLE [dbo].[OrgTrainingSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleSessionId] [bigint] NOT NULL,
	[OrgKey] [uniqueidentifier] NOT NULL,
	[Date] [date] NULL,
	[Status] [nvarchar](30) NULL,
	[Participants] [nvarchar](450) NULL,
	[IsCurrent] [bit] NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrgTrainingSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrgTrainingSessions] ADD  CONSTRAINT [DF_OrgTrainingSessions_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[OrgTrainingSessions]  WITH CHECK ADD  CONSTRAINT [FK_OrgTrainingSessions_Organization] FOREIGN KEY([OrgKey])
REFERENCES [dbo].[Organization] ([Orgkey])
GO

ALTER TABLE [dbo].[OrgTrainingSessions] CHECK CONSTRAINT [FK_OrgTrainingSessions_Organization]
GO

ALTER TABLE [dbo].[OrgTrainingSessions]  WITH CHECK ADD  CONSTRAINT [FK_OrgTrainingSessions_TrainingSessionMaster] FOREIGN KEY([ModuleSessionId])
REFERENCES [dbo].[TrainingSessionMaster] ([ModuleSessionId])
GO

ALTER TABLE [dbo].[OrgTrainingSessions] CHECK CONSTRAINT [FK_OrgTrainingSessions_TrainingSessionMaster]
GO


