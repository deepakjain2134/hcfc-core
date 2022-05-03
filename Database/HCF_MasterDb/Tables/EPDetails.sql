CREATE TABLE [dbo].[EPDetails] (
    [EPDetailId]        INT            IDENTITY (1, 1) NOT NULL,
    [StDescID]          INT            NOT NULL,
    [EPNumber]          NVARCHAR (50)  NOT NULL,
    [ScoreId]           INT            NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [CmsEPNumber]       NVARCHAR (50)  NULL,
    [CmsDescription]    NVARCHAR (MAX) NULL,
    [IsDocRequired]     BIT            CONSTRAINT [DF_EPDetails_IsDocRequired] DEFAULT ((0)) NULL,
    [Version]           NVARCHAR (150) NULL,
    [IsActive]          BIT            CONSTRAINT [DF_EPDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]         BIT            CONSTRAINT [DF_EPDetails_Isdeleted] DEFAULT ((0)) NOT NULL,
    [IsIlsmEP]          BIT            CONSTRAINT [DF_EPDetails_IsIlsmEP] DEFAULT ((0)) NOT NULL,
    [IsAssetsRequired]  BIT            CONSTRAINT [DF_EPDetails_IsAssetsRequired] DEFAULT ((0)) NOT NULL,
   
    [IsFrequencyChange] BIT            CONSTRAINT [DF_EPDetails_IsFrequencyChange] DEFAULT ((0)) NOT NULL,
    [Priority]          INT            CONSTRAINT [DF_EPDetails_Priority] DEFAULT ((0)) NOT NULL,
    [EffectiveDate]     DATE           NULL DEFAULT (getutcdate()),
    [ExpiryDate]        DATE           NULL,
	[CreatedBy]         INT            NOT NULL,
    [CreatedDate]       DATETIME       CONSTRAINT [DF_EPDetails_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdatedDate] DATETIME NULL, 
    [IsRelation] BIT NULL, 
    [ParentEPDetailId] INT NULL, 
    [IsCurrent] BIT NULL DEFAULT ((1)), 
    [EpdetailGuid] UNIQUEIDENTIFIER NULL DEFAULT (NewId()), 
    CONSTRAINT [PK_EPDetails] PRIMARY KEY CLUSTERED ([EPDetailId] ASC),
    CONSTRAINT [FK_EPDetails_StandardDesc] FOREIGN KEY ([StDescID]) REFERENCES [dbo].[Standards] ([StDescID])
);


GO
CREATE NONCLUSTERED INDEX [EPDetails_EPNumber]
    ON [dbo].[EPDetails]([EPNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [EPDetails_ScoreId]
    ON [dbo].[EPDetails]([ScoreId] ASC);


GO
CREATE NONCLUSTERED INDEX [EPDetails_IsDocRequired]
    ON [dbo].[EPDetails]([IsDocRequired] ASC);


GO
CREATE NONCLUSTERED INDEX [EPDetails_StDescID]
    ON [dbo].[EPDetails]([StDescID] ASC);


GO
CREATE NONCLUSTERED INDEX [EPDetails_Priority]
    ON [dbo].[EPDetails]([Priority] ASC);


	GO
-- =============================================
-- Author:		MOHIT YADAV
-- Create date: 10 DEC 2020
-- Description:	TRIGGER FOR UPDATION INTO EPDetails TABLE AFTER UPDATE OPERATION ON eps CREATED BEFORE 15 DAYS  IN TABLE
-- =============================================
CREATE TRIGGER [dbo].[Trg_Epdetail]
   ON  [dbo].[EPDetails]
   AFTER  Update
AS 
BEGIN
	
	SET NOCOUNT ON;
	Declare @EPDetailId int
	select @EPDetailId=i.EPDetailId from inserted i;
	if EXISTS (select * from Dbo.EPDetails eps where eps.EPDetailId= @EPDetailId and eps.CreatedDate  <= DATEADD(day,-15, GETUTCDATE()) )
        UPDATE  dbo.EPDetails SET  LastUpdatedDate=GETUTCDATE() WHERE EPDetailId=@EPDetailId
   	 
	End
