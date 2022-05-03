CREATE TABLE [dbo].[StateMaster] (
    [StateId]     INT            IDENTITY (1, 1) NOT NULL,
    [StateName]   NVARCHAR (100) NOT NULL,
    [StateCode]   NVARCHAR (50)  NOT NULL,
    [IsActive]    BIT            NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDate] DATETIME       NULL
);

