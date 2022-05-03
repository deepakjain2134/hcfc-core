CREATE proc [dbo].[Get_ICRAMatixPrecautions]

As

Begin

  select ICM.ConstructionClassId,ICM.ConstructionTypeId,ICM.ConstructionRiskId,ICM.IsActive,TypeName,RiskName,ClassName ,ICM.Version

  from ICRAMatixPrecautions ICM inner join ConstructionClass CC on ICM.ConstructionClassId = CC.ConstructionClassId

  inner join ConstructionType CT on ICM.ConstructionTypeId = CT.ConstructionTypeId

  inner join ConstructionRisk CR on ICM.ConstructionRiskId = CR.ConstructionRiskId

  where ICM.IsActive =1 and CR.IsActive =1

End


