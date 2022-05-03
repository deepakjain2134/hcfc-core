CREATE TABLE [dbo].[UserProfileHistory](
	[UserId] [int] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[CellNo] [nvarchar](20) NULL,
	[Password] [nvarchar](450) NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserProfileHistory] ADD  CONSTRAINT [DF_Table_1_CreatedBy]  DEFAULT ((0)) FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[UserProfileHistory] ADD  CONSTRAINT [DF_Table_1_CreatedDate]  DEFAULT (getutcdate()) FOR [ModifiedDate]
GO
