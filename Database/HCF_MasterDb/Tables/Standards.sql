CREATE TABLE [dbo].[Standards] (
    [StDescID]          INT            IDENTITY (1, 1) NOT NULL,
    [CategoryId]        INT            NULL,
    [TJCStandard]       NVARCHAR (255) NULL,
    [TJCDescription]    NVARCHAR (MAX) NULL,
    [CmsStandard]       NVARCHAR (50)  NULL,
    [CmsStdDescription] NVARCHAR (MAX) NULL,
    [IsActive]          BIT            CONSTRAINT [DF_GoalMaster_IsActive] DEFAULT ((1)) NULL,
    [Version]           NVARCHAR (50)  NULL,
    [CreateDate]        DATETIME       CONSTRAINT [DF_TaskMaster_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]         INT            NULL,
    CONSTRAINT [PK_StandardDesc] PRIMARY KEY CLUSTERED ([StDescID] ASC),
    CONSTRAINT [FK_StandardDesc_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The Joint Commission Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Standards', @level2type = N'COLUMN', @level2name = N'TJCDescription';

