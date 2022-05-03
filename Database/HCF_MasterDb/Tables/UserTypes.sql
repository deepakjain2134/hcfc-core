CREATE TABLE [dbo].[UserTypes] (
    [UserTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NULL,
    [IsActive]   BIT           CONSTRAINT [DF_UserTypes_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED ([UserTypeId] ASC)
);

