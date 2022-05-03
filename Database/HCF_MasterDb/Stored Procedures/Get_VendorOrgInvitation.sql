CREATE Proc [dbo].[Get_VendorOrgInvitation]
   @invitationId uniqueidentifier
As
BEgin
SELECT VO.*,o.Name as 'OrganizationName', o.DbName,o.Orgkey,v.Name as 'VendorsName',v.VendorId as 'VendorId',v.Email  FROM dbo.VendorOrganizations VO
inner join dbo.Organization o on VO.OrgKey=o.Orgkey
inner join Vendors v on v.VendorId=vo.VendorId
WHERE 
InvitationId=@invitationId
 and v.IsActive =1 and o.IsActive=1 

END