CREATE TABLE [dbo].[UserOrganization] (
    [UserOrgId]     UNIQUEIDENTIFIER NULL,
    [UserProfileId] UNIQUEIDENTIFIER NULL,
    [IsActive]      BIT              CONSTRAINT [DF_UserOrganization_IsActive] DEFAULT ((1)) NULL
);

