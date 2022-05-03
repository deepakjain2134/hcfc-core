CREATE TABLE [dbo].[RoundsQuestionnaires](
	[RouQuesId] [int] IDENTITY(1,1) NOT NULL,
	[RoundCatId] [int] NULL,
	[RoundStep] [nvarchar](max) NULL,
	[IsShared] [bit] NULL,
	[IsActive] [bit] NULL,
	[ParentRouQuesId] [int] NULL,
	[RiskType] [int] NULL,
	[RiskStepCode] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[Applicable] [nvarchar](max) NULL,
	[IsCommonRoundQues] [bit] NULL,
	[ShortDescription] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
	[AdditionalRoundType] [int] NULL,
 CONSTRAINT [PK_RoundItems] PRIMARY KEY CLUSTERED 
(
	[RouQuesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoundsQuestionnaires] ADD  CONSTRAINT [DF_RoundsQuestionnaires_IsShared]  DEFAULT ((0)) FOR [IsShared]
GO

ALTER TABLE [dbo].[RoundsQuestionnaires] ADD  CONSTRAINT [DF_RoundsQuestionnaires_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[RoundsQuestionnaires] ADD  CONSTRAINT [DF_RoundsQuestionnaires_RiskType]  DEFAULT ('') FOR [RiskType]
GO

ALTER TABLE [dbo].[RoundsQuestionnaires] ADD  CONSTRAINT [DF_RoundItems_CreatedBy]  DEFAULT ((4)) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[RoundsQuestionnaires] ADD  CONSTRAINT [DF_RoundItems_CreatedAt]  DEFAULT (getdate()) FOR [CreatedDate]
GO


