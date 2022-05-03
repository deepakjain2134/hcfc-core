CREATE TABLE [dbo].[UserProfile] (
    [UserId]         BIGINT              IDENTITY (39999, 1) NOT NULL,
    [Orgkey]         UNIQUEIDENTIFIER NULL,
    [UserProfileId]  UNIQUEIDENTIFIER CONSTRAINT [DF_UserProfile_UserProfileId] DEFAULT (newid()) NULL,
    [FirstName]      NVARCHAR (50)    NULL,
    [LastName]       NVARCHAR (50)    NULL,
    [Email]          NVARCHAR (150)   NULL,
    [UserName]       NVARCHAR (56)    NOT NULL,
    [PhoneNumber]    NVARCHAR (20)    NULL,
    [CellNo]         NVARCHAR (50)    NULL,
    [Password]       NVARCHAR (MAX)   NULL,
    [Salt]           UNIQUEIDENTIFIER CONSTRAINT [DF_UserProfile_Salt] DEFAULT (newid()) NULL,
    [IsActive]       BIT              CONSTRAINT [DF_UserProfile_IsActive] DEFAULT ((1)) NULL,
    [FileName]       NVARCHAR (150)   NULL,
    [FilePath]       NVARCHAR (150)   NULL,
    [IsSystemUser]   BIT              CONSTRAINT [DF_UserProfile_IsSystemUser] DEFAULT ((0)) NULL,
    [IsInternalUser] BIT              CONSTRAINT [DF_UserProfile_IsInternalUser] DEFAULT ((1)) NULL,
    [VendorId]       INT              NULL,
    [IsPwdChange]    BIT              CONSTRAINT [DF_UserProfile_isPwdChange] DEFAULT ((0)) NULL,
    [CreatedBy]      INT              CONSTRAINT [DF_UserProfile_CreatedBy] DEFAULT ((0)) NULL,
    [CreatedDate]    DATETIME         CONSTRAINT [DF_UserProfile_CreatedDate] DEFAULT (getdate()) NULL,
     [UserGroupId]           INT              NULL,
    [UserTypeId]     INT              NULL,
    [IsCRxUser]      BIT              CONSTRAINT [DF_UserProfile_IsCRxUser] DEFAULT ((0)) NOT NULL,
    [UserStatusCode] INT NULL DEFAULT 200, 
    [IPs] NVARCHAR(150) NULL, 
    [LastUpdatedPasswordDate] DATETIME NULL DEFAULT getutcdate(), 
    
    [LastUpdatedUser] DATETIME NULL DEFAULT getutcdate(), 
    [NormalizedUserName] NVARCHAR(256) NULL, 
    [NormalizedEmail] NVARCHAR(256) NULL, 
    [EmailConfirmed] BIT NULL DEFAULT 0, 
    [PasswordHash] NVARCHAR(MAX) NULL, 
    [SecurityStamp] NVARCHAR(MAX) NULL, 
    [ConcurrencyStamp] NVARCHAR(MAX) NULL, 
    [PhoneNumberConfirmed] BIT NULL DEFAULT 0, 
    [TwoFactorEnabled] BIT NULL DEFAULT 0, 
    [LockoutEnd] DATETIMEOFFSET NULL, 
    [LockoutEnabled] BIT NULL DEFAULT 0, 
    [AccessFailedCount] INT NULL DEFAULT 0, 
    [LatestUpdatedOn] DATETIMEOFFSET NULL, 
    [RefreshTokenHash] NVARCHAR(450) NULL, 
    [Culture] NVARCHAR(450) NULL, 
    [ExtensionData] NVARCHAR(MAX) NULL, 
    [IsDeleted] BIT NULL DEFAULT 0, 
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_UserProfile_Organization] FOREIGN KEY ([Orgkey]) REFERENCES [dbo].[Organization] ([Orgkey])
);
GO
CREATE TRIGGER [dbo].[UpdateUserProfileHistory]
ON [dbo].[UserProfile]
After  UPDATE
AS 
     
       DECLARE @UserId INT	;
	   DECLARE @Email nvarchar(50);
	   DECLARE @UserName nvarchar(50);
	   DECLARE @Password nvarchar(MAX)	;
	   DECLARE @modifiedBy INT;	
	   DECLARE @CellNumber nvarchar(50);

	
	 
	   SELECT @UserId = INSERTED.UserId,@modifiedBy=inserted.UserId  FROM INSERTED
	   SELECT @Email = INSERTED.Email  FROM INSERTED
	   SELECT @Password = INSERTED.[Password]  FROM INSERTED
	   select  @CellNumber=INSERTED.CellNo FROM INSERTED
	  
	  
if(@Password is not null )
begin
    INSERT INTO [dbo].[UserProfileHistory]
           ([UserId],[Password],[ModifiedDate],Email,CellNo,ModifiedBy)
		   VALUES(@UserId,@Password,getutcdate(),@Email,@CellNumber,@modifiedBy)
end