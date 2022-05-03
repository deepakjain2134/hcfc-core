CREATE TABLE [dbo].[EpRelation]
(
	[Id] [int] IDENTITY(1,1) ,
	[Epdetailid] [int] NULL,
	[RelatedTo] [nvarchar](max) NULL,
	[ClientNo] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
 CONSTRAINT [PK_EpRelation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TRIGGER [dbo].[Trg_EpRelation]
   ON  [dbo].[EpRelation]
   AFTER   Insert 
AS 
BEGIN
	
	SET NOCOUNT ON;
	Declare @EPDetailIds nvarchar(max)
	select @EPDetailIds=i.RelatedTo from inserted i;	
    UPDATE  dbo.EPDetails SET  IsRelation=1 WHERE EPDetailId in (Select distinct item from dbo.SplitString(@EPDetailIds,','))
   	 
	End