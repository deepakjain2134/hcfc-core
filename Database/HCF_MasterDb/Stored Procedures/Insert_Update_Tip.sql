-- =============================================
-- Author:		<PK>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_Update_Tip]
	-- Add the parameters for the stored procedure here
	@tipId int=0,
	@title nvarchar(150),
	@description nvarchar(max),
	@source nvarchar(50)=null,
	@routeUrl nvarchar(255)=null,
	@contributorName nvarchar(150)=null,
	@contributorOrg nvarchar(150)=null,
	@contributorPosition nvarchar(150)=null,
	@routename nvarchar(255) = null,
	@showContributorName bit,
	@showContributorOrg bit,
	@showContributorPosition bit,
	@isApproved int,
	@type int,
	@createdBy int=4,
	@clientNo int,
	@isActive bit,
	@updatedBy int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS (select 1 from Tips where TipId = @tipId)
	BEGIN
	   declare @createdDate datetime;
	   declare @isApprovedPrevious int;
	   declare @isSystemUser int;
	   select @createdDate=CreatedDate,@isApprovedPrevious=IsApproved from Tips where TipId = @tipId;
	   select @isSystemUser=count(*) from UserProfile where IsSystemUser=1 and UserProfile.UserId=@updatedBy;
		  --IF @isApprovedPrevious=1 and @isSystemUser=0
		  --BEGIN
				---- update Tips set IsCurrent=0 where TipId = @tipId;
			 -- Insert Into Tips(ParentTipId,Title,Description,Source,RouteUrl,CreatedBy,ContributorName,ContributorOrg,ContributorPosition,ShowContributorName,ShowContributorOrg,ShowContributorPosition,IsApproved,TipType,IsActive,UpdatedBy,UpdatedDate,CreatedDate,IsCurrent,RouteName) 
			 -- values(@tipId,@title,@description,@source,@routeUrl,@createdBy,@contributorName,@contributorOrg,@contributorPosition,@showContributorName,@showContributorOrg,@showContributorPosition,@isApproved,@type,@isActive,@updatedBy,GETUTCDATE(),@createdDate,0,@routename);
		  --END
		  --ELSE
		  BEGIN
			  Update Tips set
			  Title=@title,
			  Description=@description,
			  Source=@source,
			  RouteUrl=@routeUrl,
			  CreatedBy=@createdBy,
			  --ClientNo=NULL,
			  ContributorName=@contributorName,
			  ContributorOrg=@contributorOrg,
			  ContributorPosition=@contributorPosition,
			  ShowContributorName=@showContributorName,
			  ShowContributorOrg=@showContributorOrg,
			  ShowContributorPosition=@showContributorPosition,
			  IsApproved=@isApproved,
			  TipType=@type,
			  IsActive=@isActive,
			  UpdatedBy=@updatedBy,
			  RouteName=@routename,
			  UpdatedDate=GETUTCDATE()
			  Where TipId=@tipId;
		  END
	END
	ELSE
	BEGIN
	  Insert Into Tips(Title,Description,Source,RouteUrl,CreatedBy,ContributorName,ContributorOrg,ContributorPosition,ShowContributorName,ShowContributorOrg,ShowContributorPosition,IsApproved,TipType,IsActive,UpdatedBy,UpdatedDate,IsCurrent,RouteName) 
	  values(@title,@description,@source,@routeUrl,@createdBy,@contributorName,@contributorOrg,@contributorPosition,@showContributorName,@showContributorOrg,@showContributorPosition,@isApproved,@type,@isActive,@updatedBy,GETUTCDATE(),1,@routename);
	END
	
		--update Tips set IsApproved = 1 where TipType in (3,4) and [Description] like '%<%>%'
END