-- =============================================
-- Author:		PK
-- Create date: 13-05-2019
-- Description:	Get Tips By Client Number
-- =============================================
CREATE PROCEDURE [dbo].[Get_Tips] -- 0,''
	-- Add the parameters for the stored procedure here
	@clientNo int=0,
	@routeUrl nvarchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Tips where ((ClientNo = @clientNo and RouteUrl=@routeUrl) or ClientNo is null) and IsApproved=1 and IsCurrent=1 and IsActive=1
	and TipType  in (1,2)
END

