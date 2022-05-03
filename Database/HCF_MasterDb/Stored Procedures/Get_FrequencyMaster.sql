-- =============================================
-- Author:		<PRADEEP>
-- Create date: <Create Date,,>
-- Description:	<UPDATE FOR SHOWING ONLY CHECKED>
-- =============================================
CREATE PROCEDURE [dbo].[Get_FrequencyMaster]
AS
BEGIN
	Select * from [dbo].FrequencyMaster order by [Days] ,DisplayName desc 
END

