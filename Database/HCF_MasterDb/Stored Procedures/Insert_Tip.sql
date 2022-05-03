-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insert_Tip] 
	-- Add the parameters for the stored procedure here
	@tipId int=0,
	@title nvarchar(150),
	@description nvarchar(max),
	@source nvarchar(50),
	@routeUrl nvarchar(255)=null,
	@contributorName nvarchar(150)=null,
	@contributorOrg nvarchar(150)=null,
	@contributorPosition nvarchar(150)=null,
	@showContributorName bit,
	@showContributorOrg bit,
	@showContributorPosition bit,
	@isApproved bit,
	@type int,
	@createdBy int=4,
	@clientNo int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (select 1 from Tips where TipId = @tipId)
	BEGIN
	  Update Tips set
	  Title=@title,
	  Description=@description,
	  Source=@source,
	  RouteUrl=@routeUrl,
	  CreatedBy=@createdBy,
	  ClientNo=@clientNo,
	  ContributorName=@contributorName,
	  ContributorOrg=@contributorOrg,
	  ContributorPosition=@contributorPosition,
	  ShowContributorName=@showContributorName,
	  ShowContributorOrg=@showContributorOrg,
	  ShowContributorPosition=@showContributorPosition,
	  IsApproved=@isApproved,
	  TipType=@type
	  Where TipId=@tipId;
	END
	ELSE
	BEGIN
	  Insert Into Tips(Title,Description,Source,RouteUrl,CreatedBy,ClientNo,ContributorName,ContributorOrg,ContributorPosition,ShowContributorName,ShowContributorOrg,ShowContributorPosition,IsApproved,TipType) 
	  values(@title,@description,@source,@routeUrl,@createdBy,@clientNo,@contributorName,@contributorOrg,@contributorPosition,@showContributorName,@showContributorOrg,@showContributorPosition,@isApproved,@type);
	END

END

