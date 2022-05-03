CREATE TABLE [dbo].[VendorOrganizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InvitationId] [uniqueidentifier] NULL,
	[VendorId] [int] NULL,
	[OrgKey] [uniqueidentifier] NULL,
	[IsRequested] [bit] NULL DEFAULT 1,
	[RequestedDate] [datetime] NULL,
	[RequestedBy] [uniqueidentifier] NULL,
	[IsOrgVendor] [bit] NULL DEFAULT 1,
    CONSTRAINT [PK_VendorOrganizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[VendorOrganizations] ADD  CONSTRAINT [DF_VendorOrganizations_RequestId]  DEFAULT (newid()) FOR [InvitationId]
GO

ALTER TABLE [dbo].[VendorOrganizations] ADD  CONSTRAINT [DF_VendorOrganizations_RequestedDate]  DEFAULT (getutcdate()) FOR [RequestedDate]
GO

