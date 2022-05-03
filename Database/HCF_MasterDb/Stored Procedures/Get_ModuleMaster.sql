CREATE PROCEDURE [dbo].[Get_ModuleMaster] --111347
 -- Add the parameters for the stored procedure here  
 @Clientno int = null  
AS  
BEGIN     
	Select ModuleMaster.*, OrgServices.TrialStartDate,OrgServices.TrialEndDate,
	case when OrgServices.Status is null then Convert(bit,0) 
	when OrgServices.ServiceMode = 2 then Convert(bit,1) 
	when OrgServices.ServiceMode = 3 then Convert(bit,0) 
	when OrgServices.ServiceMode = 0 then Convert(bit,0)
	when OrgServices.ServiceMode = 5 then Convert(bit,1)
	else Convert(bit,OrgServices.Status) end as 'OrgModuleStatus',
	case when OrgServices.ServiceMode is null then -3 else OrgServices.ServiceMode end as 'OrgServiceMode'
	from ModuleMaster left join 
	OrgServices
	on ModuleMaster.ModuleId=OrgServices.ModuleId and OrganizationKey=@Clientno and MenuID is null
    order by Sequence
    select * from Menus
END 

 
