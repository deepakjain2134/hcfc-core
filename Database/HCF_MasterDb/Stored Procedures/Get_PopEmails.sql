CREATE Procedure [dbo].[Get_PopEmails]  
As  
Begin  
Select IE.EmailId,IE.Password,IE.PopServer,IE.IsActive,IE.ClientNo,ORG.DbName,IE.Port,IE.UseSSL from InboxEmails IE  
left join Organization ORG on IE.ClientNo = ORG.ClientNo  
where IE.IsActive =1  
End 