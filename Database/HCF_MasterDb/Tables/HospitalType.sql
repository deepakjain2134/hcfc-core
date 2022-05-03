CREATE TABLE [dbo].[HospitalType] (
    [HospitalTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Type]           NVARCHAR (150) NULL,
    [IsActive]       BIT            NULL,
    [CreatedBy]      INT            NOT NULL,
    [CreateDate]     DATETIME       CONSTRAINT [DF_HospitalType_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_HospitalType] PRIMARY KEY CLUSTERED ([HospitalTypeId] ASC)
);

