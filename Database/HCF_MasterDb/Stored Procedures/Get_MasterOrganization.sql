Create procedure [dbo].[Get_MasterOrganization] --'HCF_Burke'
@dbname nvarchar(50) = null
As

Begin

Select * From Organization  Where dbname=ISNULL(@dbname,dbname) 

Select * from InboxEmails Where IsActive=1

End