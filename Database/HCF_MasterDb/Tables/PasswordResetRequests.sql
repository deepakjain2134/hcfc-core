CREATE TABLE [dbo].[PasswordResetRequests] (
    [RequestId]       UNIQUEIDENTIFIER CONSTRAINT [PasswordResetRequests_PK_DF] DEFAULT (newsequentialid()) NOT NULL,
    [EmailAddress]    NVARCHAR (128)   NOT NULL,
    [RecoveryMethod]  CHAR (1)         NOT NULL,
    [RecoveryAddress] NVARCHAR (128)   NOT NULL,
    [RequestedOn]     DATETIME         CONSTRAINT [PasswordResetRequests_RequestedOn_DF] DEFAULT (getutcdate()) NOT NULL,
    [RecoveryToken]   VARCHAR (128)    NOT NULL,
    [RecoveredOn]     DATETIME         NULL,
    [Status]          AS               (case when ([RequestedOn]+(1))>=getutcdate() AND [RecoveredOn] IS NULL then 'Y' else 'N' end),
    [PasswordRequestType] VARCHAR(50) NULL DEFAULT 'recover', 
    CONSTRAINT [PasswordResetRequests_PK] PRIMARY KEY CLUSTERED ([RequestId] ASC)
);

