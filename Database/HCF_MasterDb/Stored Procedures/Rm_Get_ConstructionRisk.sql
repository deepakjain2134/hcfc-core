﻿--CREATE proc [dbo].[Rm_Get_ConstructionRisk]  
--@ConstructionRiskId int =null  
--As  
--Begin  
--  select * from ConstructionRisk where ConstructionRiskId = isnull(@ConstructionRiskId,ConstructionRiskId) 
--  Select * from ConstRiskDeptMap  
--  inner join ICRARiskArea on ICRARiskArea.RiskAreaId=ConstRiskDeptMap.ConstructionRiskAreaId  and ICRARiskArea.IsActive=1
--  where ConstructionRiskId = isnull(@ConstructionRiskId,ConstructionRiskId) 
--  and ConstRiskDeptMap.IsActive=1
--End  
  
