CREATE TABLE [dbo].[EPDocuments] (
    [EPDocumentId]         INT      IDENTITY (1, 1) NOT NULL,
    [EPDetailId]           INT      NOT NULL,
    [DocTypeId]            INT      NOT NULL,
    [IsActive]             BIT      CONSTRAINT [DF_EPDocuments_IsDeleted] DEFAULT ((1)) NOT NULL,
    [DocInspectionGroupId] INT      CONSTRAINT [DF_EPDocuments_DocInspectionGroupId] DEFAULT ((0)) NULL,
    [DateEffective]        DATETIME NULL,
    [CreatedBy]            INT      NOT NULL,
    [CreatedDate]          DATETIME CONSTRAINT [DF_EPDocuments_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EPDocuments] PRIMARY KEY CLUSTERED ([EPDocumentId] ASC),
    CONSTRAINT [FK_EPDocuments_EPDetails] FOREIGN KEY ([EPDetailId]) REFERENCES [dbo].[EPDetails] ([EPDetailId]),
    CONSTRAINT [FK_EPDocuments_DocumentType] FOREIGN KEY([DocTypeId]) REFERENCES [dbo].[DocumentType] ([DocTypeId])
);

