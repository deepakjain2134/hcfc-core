CREATE Proc [dbo].[Main_MasterOrganization]  --4,0
@userId int ,
@hostingEnvironment int = null
As
Begin
IF EXISTS (Select * from [dbo].UserProfile Where UserId=@userId and IsActive=1 )
BEGIN
	if exists(select 1 from [dbo].Organization where DbName ='HCF_TestHospital')
		BEGIN
		DECLARE @TestOrgKey uniqueidentifier;
        select @TestOrgKey = Orgkey from dbo.Organization where DbName = 'HCF_TestHospital' 
	      Select * from [dbo].Organization ORG Where IsActive=1
		  and Orgkey in (
			 Select UOT.UserOrgId from [dbo].UserOrganization UOT inner join [dbo].UserProfile UT
			 on UT.UserProfileId=UOT.UserProfileId    
			 Where ut.UserId=@userId and UOT.IsActive=1 
		  ) 		  
		  AND (@hostingEnvironment =1 ANd ORG.Orgkey not in (@TestOrgKey) OR 
		       @hostingEnvironment =2 and Orgkey in (@TestOrgKey))
		END
	ELSE
		BEGIN
		  Select * from [dbo].Organization Where IsActive=1
		  and Orgkey in (
			 Select UOT.UserOrgId from [dbo].UserOrganization UOT inner join [dbo].UserProfile UT
			 on UT.UserProfileId=UOT.UserProfileId    
			 Where ut.UserId=@userId and UOT.IsActive=1 
		  ) 
		  --union all 
		  --Select * from  [dbo].Organization Where Orgkey in (select Orgkey from UserProfile where UserId = @userId)
		END
	END
END
