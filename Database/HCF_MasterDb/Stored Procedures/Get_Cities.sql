-- =============================================
-- Author:		Mukund Kumar
-- ALTER date: 02/03/2020
-- Description:	To get cities based on state name
-- Change : added condition to get all cities if stateid is 0
-- =============================================
CREATE ProcEDURE [dbo].[Get_Cities] 
	@stateId varchar(50) = null
AS
BEGIN
	
		SELECT * FROM [dbo].CityMaster WHERE StateId = isnull(@stateId,StateId) Order by CityName

END