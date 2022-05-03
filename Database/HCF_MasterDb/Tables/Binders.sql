CREATE TABLE [dbo].[Binders] (
    [BinderId]       INT            IDENTITY (1, 1) NOT NULL,
    [BinderCode]     NVARCHAR (50)  NULL,
    [BinderName]     NVARCHAR (150) NULL,
    [DisplayName]    NVARCHAR (50)  NULL,
    [FileName]       NVARCHAR (150) NULL,
    [FilePath]       NVARCHAR (250) NULL,
    [IsActive]       BIT            CONSTRAINT [DF_Binders_IsActive] DEFAULT ((1)) NULL,
    [CreatedBy]      INT            NULL,
    [CreatedDate]    DATETIME       NULL,
    [ParentBinderId] INT            NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [ClientNo] INT NULL, 
    CONSTRAINT [PK_Binders] PRIMARY KEY CLUSTERED ([BinderId] ASC),
    UNIQUE NONCLUSTERED ([BinderCode] ASC),
    CONSTRAINT uq_binders_name_clientNo  UNIQUE NONCLUSTERED  ([BinderName] ,ClientNo)
);