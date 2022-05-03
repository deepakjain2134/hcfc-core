--CREATE FUNCTION [dbo].[Rm_IsRiskAreaEmpty] (@RiskAreaId int)
--RETURNS bit
--AS BEGIN
--    DECLARE @isEmpty bit;
--	declare @count int;
--	set @isEmpty=0;

--    Select @count=COUNT(*) from ConstRiskDeptMap Where ConstructionRiskAreaId=@RiskAreaId and IsActive=1

--	if(@count=0)
--	begin
--	  set @isEmpty=1
--	end

--    RETURN @isEmpty
--END
