CREATE TABLE [dbo].[FiredrillCategory] (
    [FiredrillCatId] INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName]   VARCHAR (100)  NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [IsActive]       BIT            CONSTRAINT [DF_FiredrillCategory_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [CreatedDate]    DATETIME       CONSTRAINT [DF_FiredrillCategory_CreatedDate] DEFAULT (getutcdate()) NOT NULL,
    [IsCommonCat] BIT NOT NULL DEFAULT 1, 
    [Applicable] NVARCHAR(MAX) NULL DEFAULT 1, 
    CONSTRAINT [PK_FiredrillCategory] PRIMARY KEY CLUSTERED ([FiredrillCatId] ASC)
);

