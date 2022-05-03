CREATE TABLE [dbo].[RoundCategory](
	[RoundCatId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](100) NULL,
	[FrequencyIds] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,	
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL, 
    [Applicable] NVARCHAR(MAX) NULL, 
    [IsCommonCat] BIT NULL DEFAULT 1, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    CONSTRAINT [PK_RoundCategory] PRIMARY KEY ([RoundCatId])
)
