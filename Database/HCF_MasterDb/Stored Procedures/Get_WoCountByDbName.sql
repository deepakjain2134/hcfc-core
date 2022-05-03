-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_WoCountByDbName]  --'HCF_Holy','2019-03-19'
	@DbName varchar(max),
	@date varchar(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @query varchar(max);
	declare @whereConditon varchar(max);

	set @whereConditon=' where DATEDIFF(DAY,CreatedDate,ISNULL( CONVERT(datetime,'''+@date+''') ,CreatedDate))=0 or DATEDIFF(DAY,UpdatedDate,ISNULL( CONVERT(datetime,'''+@date+''') ,UpdatedDate))=0';

	if @whereConditon is not null
	begin
	  set @query='select count(*) from '+@DbName+'.dbo.WorkOrder '+@whereConditon;
	end
	else
	begin
	 set @query='select count(*) from '+@DbName+'.dbo.WorkOrder';
	end
	

	-- select @query

	Exec (@query);


END

