CREATE TABLE [dbo].[EPGroupsDetail] (
    [EPGroupDetailId] INT IDENTITY (1, 1) NOT NULL,
    [EPGroupId]       INT NULL,
    [EPDetailId]      INT NULL,
    [IsActive]        BIT CONSTRAINT [DF_EPGroupsDetail_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_EPGroupsDetail] PRIMARY KEY CLUSTERED ([EPGroupDetailId] ASC)
);

