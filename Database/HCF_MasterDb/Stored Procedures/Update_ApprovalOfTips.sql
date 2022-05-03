-- =============================================
-- Author:		PK
-- Create date: 29 MAY 2019
-- Description:	Change Approval Status
-- =============================================
CREATE PROCEDURE [dbo].[Update_ApprovalOfTips]
	-- Add the parameters for the stored procedure here
	@ApproveStatus int,
	@tipId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @parentTipId int;
	select @parentTipId=ParentTipId from Tips where TipId=@tipId;

	IF(@parentTipId is not null and @parentTipId>0 and @ApproveStatus=1)
	BEGIN
	   Update Tips set IsCurrent=0 where TipId=@parentTipId;
	END

	Update Tips set IsApproved=@ApproveStatus,IsCurrent=1 where TipId=@tipId;
END

