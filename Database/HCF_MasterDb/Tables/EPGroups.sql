CREATE TABLE [dbo].[EPGroups] (
    [EPGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [EPGroupName] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_EPGroups_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_EPGroups] PRIMARY KEY CLUSTERED ([EPGroupId] ASC)
);

